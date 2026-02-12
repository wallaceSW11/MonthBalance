using MonthBalance.API.Models;

namespace MonthBalance.API.Repositories;

public interface ISessionRepository
{
    Task<UserSession> CreateAsync(UserSession session);
    Task<UserSession?> GetByIdAsync(int id);
    Task<UserSession?> GetActiveSessionByUserIdAsync(int userId);
    Task<List<UserSession>> GetByUserIdAsync(int userId, int limit = 50);
    Task UpdateAsync(UserSession session);
    Task DeactivateAllUserSessionsAsync(int userId);
}
