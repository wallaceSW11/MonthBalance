using Microsoft.EntityFrameworkCore;
using MonthBalance.API.Data;
using MonthBalance.API.Models;

namespace MonthBalance.API.Repositories;

public class SessionRepository : ISessionRepository
{
    private readonly ApplicationDbContext _context;
    
    public SessionRepository(ApplicationDbContext context)
    {
        _context = context;
    }
    
    public async Task<UserSession> CreateAsync(UserSession session)
    {
        _context.UserSessions.Add(session);
        await _context.SaveChangesAsync();
        return session;
    }
    
    public async Task<UserSession?> GetByIdAsync(int id)
    {
        return await _context.UserSessions
            .Include(s => s.User)
            .FirstOrDefaultAsync(s => s.Id == id);
    }
    
    public async Task<UserSession?> GetActiveSessionByUserIdAsync(int userId)
    {
        return await _context.UserSessions
            .Where(s => s.UserId == userId && s.IsActive)
            .OrderByDescending(s => s.LoginAt)
            .FirstOrDefaultAsync();
    }
    
    public async Task<List<UserSession>> GetByUserIdAsync(int userId, int limit = 50)
    {
        return await _context.UserSessions
            .Where(s => s.UserId == userId)
            .OrderByDescending(s => s.LoginAt)
            .Take(limit)
            .ToListAsync();
    }
    
    public async Task UpdateAsync(UserSession session)
    {
        _context.UserSessions.Update(session);
        await _context.SaveChangesAsync();
    }
    
    public async Task DeactivateAllUserSessionsAsync(int userId)
    {
        var sessions = await _context.UserSessions
            .Where(s => s.UserId == userId && s.IsActive)
            .ToListAsync();
        
        foreach (var session in sessions)
        {
            session.IsActive = false;
            session.LogoutAt = DateTime.UtcNow;
        }
        
        await _context.SaveChangesAsync();
    }
}
