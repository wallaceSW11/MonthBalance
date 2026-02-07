using Microsoft.EntityFrameworkCore;
using MonthBalance.API.Data;
using MonthBalance.API.Models;

namespace MonthBalance.API.Repositories;

public class WebAuthnRepository : IWebAuthnRepository
{
    private readonly ApplicationDbContext _context;
    
    public WebAuthnRepository(ApplicationDbContext context)
    {
        _context = context;
    }
    
    public async Task<WebAuthnCredential?> GetByCredentialIdAsync(string credentialId)
    {
        return await _context.WebAuthnCredentials
            .Include(c => c.User)
            .FirstOrDefaultAsync(c => c.CredentialId == credentialId);
    }
    
    public async Task<List<WebAuthnCredential>> GetByUserIdAsync(int userId)
    {
        return await _context.WebAuthnCredentials
            .Where(c => c.UserId == userId)
            .ToListAsync();
    }
    
    public async Task<WebAuthnCredential> CreateAsync(WebAuthnCredential credential)
    {
        credential.CreatedAt = DateTime.UtcNow;
        
        await _context.WebAuthnCredentials.AddAsync(credential);
        await _context.SaveChangesAsync();
        
        return credential;
    }
    
    public async Task UpdateAsync(WebAuthnCredential credential)
    {
        _context.WebAuthnCredentials.Update(credential);
        await _context.SaveChangesAsync();
    }
    
    public async Task<bool> CredentialExistsAsync(string credentialId)
    {
        return await _context.WebAuthnCredentials
            .AnyAsync(c => c.CredentialId == credentialId);
    }
}
