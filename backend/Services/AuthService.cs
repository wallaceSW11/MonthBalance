using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using MonthBalance.API.DTOs;
using MonthBalance.API.Models;
using MonthBalance.API.Repositories;
using BCrypt.Net;

namespace MonthBalance.API.Services;

public class AuthService : IAuthService
{
    private readonly IUserRepository _userRepository;
    private readonly ISessionRepository _sessionRepository;
    private readonly IAnalyticsService _analyticsService;
    private readonly IConfiguration _configuration;
    private readonly IHttpContextAccessor _httpContextAccessor;
    
    public AuthService(
        IUserRepository userRepository,
        ISessionRepository sessionRepository,
        IAnalyticsService analyticsService,
        IConfiguration configuration,
        IHttpContextAccessor httpContextAccessor)
    {
        _userRepository = userRepository;
        _sessionRepository = sessionRepository;
        _analyticsService = analyticsService;
        _configuration = configuration;
        _httpContextAccessor = httpContextAccessor;
    }
    
    public async Task<LoginResponse> RegisterAsync(RegisterRequest request)
    {
        if (await _userRepository.EmailExistsAsync(request.Email))
            throw new InvalidOperationException("Email already registered");
        
        var user = new User
        {
            Name = request.Name,
            Email = request.Email,
            PasswordHash = BCrypt.Net.BCrypt.HashPassword(request.Password),
            NotificationsEnabled = true,
            Role = UserRole.User
        };
        
        var createdUser = await _userRepository.CreateAsync(user);
        
        await _analyticsService.TrackActivityAsync(
            createdUser.Id,
            ActivityType.UserRegistered,
            GetIpAddress(),
            GetUserAgent()
        );
        
        var token = GenerateJwtToken(createdUser);
        
        return new LoginResponse(token, MapToDto(createdUser));
    }
    
    public async Task<LoginResponse> LoginAsync(LoginRequest request)
    {
        var user = await _userRepository.GetByEmailAsync(request.Email);
        
        if (user == null)
            throw new UnauthorizedAccessException("Invalid email or password");
        
        if (!BCrypt.Net.BCrypt.Verify(request.Password, user.PasswordHash))
            throw new UnauthorizedAccessException("Invalid email or password");
        
        var session = new UserSession
        {
            UserId = user.Id,
            LoginAt = DateTime.UtcNow,
            IpAddress = GetIpAddress(),
            UserAgent = GetUserAgent(),
            IsActive = true
        };
        
        await _sessionRepository.CreateAsync(session);
        
        await _analyticsService.TrackActivityAsync(
            user.Id,
            ActivityType.UserLogin,
            GetIpAddress(),
            GetUserAgent()
        );
        
        var token = GenerateJwtToken(user);
        
        return new LoginResponse(token, MapToDto(user));
    }
    
    public async Task<UserDto> GetCurrentUserAsync(int userId)
    {
        var user = await _userRepository.GetByIdAsync(userId);
        
        if (user == null)
            throw new InvalidOperationException("User not found");
        
        return MapToDto(user);
    }
    
    public async Task<UserDto> UpdateUserAsync(int userId, UpdateUserRequest request)
    {
        var user = await _userRepository.GetByIdAsync(userId);
        
        if (user == null)
            throw new InvalidOperationException("User not found");
        
        user.Name = request.Name;
        user.Avatar = request.Avatar;
        user.NotificationsEnabled = request.NotificationsEnabled;
        
        await _userRepository.UpdateAsync(user);
        
        return MapToDto(user);
    }
    
    public async Task ChangePasswordAsync(int userId, ChangePasswordRequest request)
    {
        var user = await _userRepository.GetByIdAsync(userId);
        
        if (user == null)
            throw new InvalidOperationException("User not found");
        
        if (!BCrypt.Net.BCrypt.Verify(request.CurrentPassword, user.PasswordHash))
            throw new UnauthorizedAccessException("Current password is incorrect");
        
        user.PasswordHash = BCrypt.Net.BCrypt.HashPassword(request.NewPassword);
        
        await _userRepository.UpdateAsync(user);
        
        await _analyticsService.TrackActivityAsync(
            userId,
            ActivityType.PasswordChanged,
            GetIpAddress(),
            GetUserAgent()
        );
    }

    public async Task DeleteAccountAsync(int userId)
    {
        var user = await _userRepository.GetByIdAsync(userId);

        if (user == null)
            throw new InvalidOperationException("User not found");

        await _userRepository.DeleteAsync(userId);
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
            new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
            new Claim(JwtRegisteredClaimNames.Email, user.Email),
            new Claim(JwtRegisteredClaimNames.Name, user.Name),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            new Claim("role", user.Role.ToString())
        };
        
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSecret));
        var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
        
        var token = new JwtSecurityToken(
            issuer: jwtIssuer,
            audience: jwtAudience,
            claims: claims,
            expires: DateTime.UtcNow.AddHours(jwtExpirationHours),
            signingCredentials: credentials
        );
        
        return new JwtSecurityTokenHandler().WriteToken(token);
    }
    
    private string? GetIpAddress()
    {
        return _httpContextAccessor.HttpContext?.Connection.RemoteIpAddress?.ToString();
    }
    
    private string? GetUserAgent()
    {
        return _httpContextAccessor.HttpContext?.Request.Headers.UserAgent.ToString();
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
}
