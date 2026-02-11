using MonthBalance.API.Models;

namespace MonthBalance.API.Repositories;

public interface IWebAuthnRepository
{
    Task<WebAuthnCredential?> GetByCredentialIdAsync(string credentialId);
    Task<List<WebAuthnCredential>> GetByUserIdAsync(int userId);
    Task<WebAuthnCredential> CreateAsync(WebAuthnCredential credential);
    Task UpdateAsync(WebAuthnCredential credential);
    Task<bool> CredentialExistsAsync(string credentialId);
}
