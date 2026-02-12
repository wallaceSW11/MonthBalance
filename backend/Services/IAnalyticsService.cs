using MonthBalance.API.Models;

namespace MonthBalance.API.Services;

public interface IAnalyticsService
{
    Task TrackActivityAsync(int userId, ActivityType activityType, string? ipAddress = null, string? userAgent = null, string? metadata = null);
    Task<int> GetTotalUsersAsync();
    Task<int> GetActiveUsersAsync(DateTime startDate);
    Task<int> GetNewUsersAsync(DateTime startDate);
    Task<Dictionary<string, int>> GetRetentionRatesAsync();
    Task<Dictionary<ActivityType, int>> GetTopActivitiesAsync(DateTime? startDate = null, DateTime? endDate = null);
    Task<List<UserActivity>> GetRecentActivitiesAsync(int limit = 10);
}
