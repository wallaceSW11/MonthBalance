using MonthBalance.API.Models;

namespace MonthBalance.API.Repositories;

public interface IActivityRepository
{
    Task<UserActivity> CreateAsync(UserActivity activity);
    Task<List<UserActivity>> GetByUserIdAsync(int userId, int limit = 100);
    Task<List<UserActivity>> GetByActivityTypeAsync(ActivityType activityType, int limit = 100);
    Task<List<UserActivity>> GetByDateRangeAsync(DateTime startDate, DateTime endDate, int? userId = null);
    Task<Dictionary<ActivityType, int>> GetActivityCountsAsync(DateTime? startDate = null, DateTime? endDate = null);
}
