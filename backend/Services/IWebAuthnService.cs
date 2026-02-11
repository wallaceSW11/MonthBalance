using MonthBalance.API.DTOs;

namespace MonthBalance.API.Services;

public interface IWebAuthnService
{
    Task<WebAuthnRegisterChallengeResponse> GenerateRegisterChallengeAsync(int userId);
    Task<WebAuthnRegisterResponse> RegisterCredentialAsync(int userId, WebAuthnRegisterRequest request);
    Task<WebAuthnAuthenticateChallengeResponse> GenerateAuthenticateChallengeAsync(string email);
    Task<LoginResponse> AuthenticateAsync(WebAuthnAuthenticateRequest request);
}
