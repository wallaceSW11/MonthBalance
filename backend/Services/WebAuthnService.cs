using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using Microsoft.Extensions.Caching.Memory;
using MonthBalance.API.DTOs;
using MonthBalance.API.Models;
using MonthBalance.API.Repositories;

namespace MonthBalance.API.Services;

public class WebAuthnService : IWebAuthnService
{
    private readonly IWebAuthnRepository _webAuthnRepository;
    private readonly IUserRepository _userRepository;
    private readonly IMemoryCache _cache;
    private readonly IConfiguration _configuration;
    
    private const int ChallengeExpirationMinutes = 5;
    private const int ChallengeSize = 32;
    
    public WebAuthnService(
        IWebAuthnRepository webAuthnRepository,
        IUserRepository userRepository,
        IMemoryCache cache,
        IConfiguration configuration)
    {
        _webAuthnRepository = webAuthnRepository;
        _userRepository = userRepository;
        _cache = cache;
        _configuration = configuration;
    }
    
    public async Task<WebAuthnRegisterChallengeResponse> GenerateRegisterChallengeAsync(int userId)
    {
        var user = await _userRepository.GetByIdAsync(userId);
        
        if (user == null)
            throw new InvalidOperationException("User not found");
        
        var challenge = GenerateChallenge();
        var cacheKey = $"webauthn_register_{userId}";
        
        _cache.Set(cacheKey, challenge, TimeSpan.FromMinutes(ChallengeExpirationMinutes));
        
        var rpId = _configuration["WebAuthn:RpId"] ?? "localhost";
        var rpName = _configuration["WebAuthn:RpName"] ?? "Month Balance";
        
        return new WebAuthnRegisterChallengeResponse(
            challenge,
            Convert.ToBase64String(Encoding.UTF8.GetBytes(userId.ToString())),
            rpId,
            rpName,
            60000
        );
    }
    
    public async Task<WebAuthnRegisterResponse> RegisterCredentialAsync(int userId, WebAuthnRegisterRequest request)
    {
        var cacheKey = $"webauthn_register_{userId}";
        
        if (!_cache.TryGetValue(cacheKey, out string? challenge) || challenge == null)
            throw new InvalidOperationException("Invalid or expired challenge");
        
        if (await _webAuthnRepository.CredentialExistsAsync(request.CredentialId))
            throw new InvalidOperationException("Credential already registered");
        
        var credential = new WebAuthnCredential
        {
            UserId = userId,
            CredentialId = request.CredentialId,
            PublicKey = request.PublicKey,
            Counter = request.Counter,
            Transports = string.Join(",", request.Transports),
            CreatedAt = DateTime.UtcNow
        };
        
        await _webAuthnRepository.CreateAsync(credential);
        
        _cache.Remove(cacheKey);
        
        return new WebAuthnRegisterResponse(true, "Biometric authentication enabled");
    }
    
    public async Task<WebAuthnAuthenticateChallengeResponse> GenerateAuthenticateChallengeAsync(string email)
    {
        var user = await _userRepository.GetByEmailAsync(email);
        
        if (user == null)
            throw new InvalidOperationException("User not found");
        
        var credentials = await _webAuthnRepository.GetByUserIdAsync(user.Id);
        
        if (credentials.Count == 0)
            throw new InvalidOperationException("No biometric credentials registered");
        
        var challenge = GenerateChallenge();
        var cacheKey = $"webauthn_auth_{user.Id}";
        
        _cache.Set(cacheKey, challenge, TimeSpan.FromMinutes(ChallengeExpirationMinutes));
        
        var allowCredentials = credentials.Select(c => new AllowedCredential(
            c.CredentialId,
            "public-key",
            c.Transports?.Split(',') ?? Array.Empty<string>()
        )).ToList();
        
        var rpId = _configuration["WebAuthn:RpId"] ?? "localhost";
        
        return new WebAuthnAuthenticateChallengeResponse(
            challenge,
            allowCredentials,
            60000,
            rpId
        );
    }
    
    public async Task<LoginResponse> AuthenticateAsync(WebAuthnAuthenticateRequest request)
    {
        var credential = await _webAuthnRepository.GetByCredentialIdAsync(request.CredentialId);
        
        if (credential == null)
            throw new InvalidOperationException("Credential not found");
        
        var cacheKey = $"webauthn_auth_{credential.UserId}";
        
        if (!_cache.TryGetValue(cacheKey, out string? challenge) || challenge == null)
            throw new InvalidOperationException("Invalid or expired challenge");
        
        var clientData = ParseClientDataJSON(request.ClientDataJSON);
        
        if (clientData.Challenge != challenge)
            throw new UnauthorizedAccessException("Challenge mismatch");
        
        var isValid = VerifySignature(
            request.AuthenticatorData,
            request.ClientDataJSON,
            request.Signature,
            credential.PublicKey
        );
        
        if (!isValid)
            throw new UnauthorizedAccessException("Invalid signature");
        
        var counter = ExtractCounter(request.AuthenticatorData);
        
        if (counter <= credential.Counter)
            throw new UnauthorizedAccessException("Invalid counter - possible replay attack");
        
        credential.Counter = counter;
        credential.LastUsedAt = DateTime.UtcNow;
        
        await _webAuthnRepository.UpdateAsync(credential);
        
        _cache.Remove(cacheKey);
        
        var token = GenerateJwtToken(credential.User);
        var userDto = MapToDto(credential.User);
        
        return new LoginResponse(token, userDto);
    }
    
    private static string GenerateChallenge()
    {
        var bytes = new byte[ChallengeSize];
        using var rng = RandomNumberGenerator.Create();
        rng.GetBytes(bytes);
        return Convert.ToBase64String(bytes);
    }
    
    private static ClientData ParseClientDataJSON(string clientDataJSON)
    {
        var json = Encoding.UTF8.GetString(Convert.FromBase64String(clientDataJSON));
        var data = JsonSerializer.Deserialize<ClientData>(json);
        
        if (data == null)
            throw new InvalidOperationException("Invalid client data");
        
        return data;
    }
    
    private static bool VerifySignature(string authenticatorData, string clientDataJSON, string signature, string publicKey)
    {
        try
        {
            var authData = Convert.FromBase64String(authenticatorData);
            var clientData = Convert.FromBase64String(clientDataJSON);
            var sig = Convert.FromBase64String(signature);
            var pubKey = Convert.FromBase64String(publicKey);
            
            var clientDataHash = SHA256.HashData(clientData);
            
            var signedData = new byte[authData.Length + clientDataHash.Length];
            Buffer.BlockCopy(authData, 0, signedData, 0, authData.Length);
            Buffer.BlockCopy(clientDataHash, 0, signedData, authData.Length, clientDataHash.Length);
            
            using var ecdsa = ECDsa.Create();
            ecdsa.ImportSubjectPublicKeyInfo(pubKey, out _);
            
            return ecdsa.VerifyData(signedData, sig, HashAlgorithmName.SHA256);
        }
        catch
        {
            return false;
        }
    }
    
    private static long ExtractCounter(string authenticatorData)
    {
        var data = Convert.FromBase64String(authenticatorData);
        
        if (data.Length < 37)
            return 0;
        
        var counterBytes = new byte[4];
        Buffer.BlockCopy(data, 33, counterBytes, 0, 4);
        
        if (BitConverter.IsLittleEndian)
            Array.Reverse(counterBytes);
        
        return BitConverter.ToUInt32(counterBytes, 0);
    }
    
    private string GenerateJwtToken(User user)
    {
        var jwtSecret = _configuration["Jwt:Secret"] 
            ?? throw new InvalidOperationException("JWT Secret not configured");
        var jwtIssuer = _configuration["Jwt:Issuer"] 
            ?? throw new InvalidOperationException("JWT Issuer not configured");
        var jwtAudience = _configuration["Jwt:Audience"] 
            ?? throw new InvalidOperationException("JWT Audience not configured");
        var jwtExpirationHours = int.Parse(_configuration["Jwt:ExpirationHours"] ?? "24");
        
        var claims = new[]
        {
            new System.Security.Claims.Claim(System.IdentityModel.Tokens.Jwt.JwtRegisteredClaimNames.Sub, user.Id.ToString()),
            new System.Security.Claims.Claim(System.IdentityModel.Tokens.Jwt.JwtRegisteredClaimNames.Email, user.Email),
            new System.Security.Claims.Claim(System.IdentityModel.Tokens.Jwt.JwtRegisteredClaimNames.Name, user.Name),
            new System.Security.Claims.Claim(System.IdentityModel.Tokens.Jwt.JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
        };
        
        var key = new Microsoft.IdentityModel.Tokens.SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSecret));
        var credentials = new Microsoft.IdentityModel.Tokens.SigningCredentials(key, Microsoft.IdentityModel.Tokens.SecurityAlgorithms.HmacSha256);
        
        var token = new System.IdentityModel.Tokens.Jwt.JwtSecurityToken(
            issuer: jwtIssuer,
            audience: jwtAudience,
            claims: claims,
            expires: DateTime.UtcNow.AddHours(jwtExpirationHours),
            signingCredentials: credentials
        );
        
        return new System.IdentityModel.Tokens.Jwt.JwtSecurityTokenHandler().WriteToken(token);
    }
    
    private static UserDto MapToDto(User user)
    {
        return new UserDto(
            user.Id,
            user.Name,
            user.Email,
            user.Avatar,
            user.NotificationsEnabled
        );
    }
    
    private class ClientData
    {
        public string Type { get; set; } = string.Empty;
        public string Challenge { get; set; } = string.Empty;
        public string Origin { get; set; } = string.Empty;
    }
}
