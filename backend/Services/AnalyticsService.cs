using Microsoft.EntityFrameworkCore;
using MonthBalance.API.Data;
using MonthBalance.API.Models;
using MonthBalance.API.Repositories;

namespace MonthBalance.API.Services;

public class AnalyticsService : IAnalyticsService
{
    private readonly IActivityRepository _activityRepository;
    private readonly ApplicationDbContext _context;
    private readonly ILogger<AnalyticsService> _logger;
    private readonly IConfiguration _configuration;
    
    public AnalyticsService(
        IActivityRepository activityRepository,
        ApplicationDbContext context,
        ILogger<AnalyticsService> logger,
        IConfiguration configuration)
    {
        _activityRepository = activityRepository;
        _context = context;
        _logger = logger;
        _configuration = configuration;
    }
    
    public async Task TrackActivityAsync(int userId, ActivityType activityType, string? ipAddress = null, string? userAgent = null, string? metadata = null)
    {
        // Verifica se o tracking detalhado está habilitado
        var enableDetailedTracking = _configuration.GetValue<bool>("Analytics:EnableDetailedTracking", false);
        
        // Sempre loga eventos críticos, independente da configuração
        var criticalEvents = new[]
        {
            ActivityType.UserRegistered,
            ActivityType.UserLogin,
            ActivityType.FeedbackSent
        };
        
        if (!enableDetailedTracking && !criticalEvents.Contains(activityType))
        {
            return;
        }
        
        try
        {
            var activity = new UserActivity
            {
                UserId = userId,
                ActivityType = activityType,
                Timestamp = DateTime.UtcNow,
                IpAddress = ipAddress,
                UserAgent = userAgent,
                Metadata = metadata
            };
            
            await _activityRepository.CreateAsync(activity);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error tracking activity {ActivityType} for user {UserId}", activityType, userId);
        }
    }
    
    public async Task<int> GetTotalUsersAsync()
    {
        return await _context.Users.CountAsync();
    }
    
    public async Task<int> GetActiveUsersAsync(DateTime startDate)
    {
        return await _context.UserActivities
            .Where(a => a.Timestamp >= startDate)
            .Select(a => a.UserId)
            .Distinct()
            .CountAsync();
    }
    
    public async Task<int> GetNewUsersAsync(DateTime startDate)
    {
        return await _context.Users
            .Where(u => u.CreatedAt >= startDate)
            .CountAsync();
    }
    
    public async Task<Dictionary<string, int>> GetRetentionRatesAsync()
    {
        var now = DateTime.UtcNow;
        var day1 = now.AddDays(-1);
        var day7 = now.AddDays(-7);
        var day30 = now.AddDays(-30);
        
        var usersDay1 = await _context.Users.Where(u => u.CreatedAt <= day1).CountAsync();
        var activeDay1 = await GetActiveUsersAsync(day1);
        
        var usersDay7 = await _context.Users.Where(u => u.CreatedAt <= day7).CountAsync();
        var activeDay7 = await GetActiveUsersAsync(day7);
        
        var usersDay30 = await _context.Users.Where(u => u.CreatedAt <= day30).CountAsync();
        var activeDay30 = await GetActiveUsersAsync(day30);
        
        return new Dictionary<string, int>
        {
            { "day1Total", usersDay1 },
            { "day1Active", activeDay1 },
            { "day7Total", usersDay7 },
            { "day7Active", activeDay7 },
            { "day30Total", usersDay30 },
            { "day30Active", activeDay30 }
        };
    }
    
    public async Task<Dictionary<ActivityType, int>> GetTopActivitiesAsync(DateTime? startDate = null, DateTime? endDate = null)
    {
        return await _activityRepository.GetActivityCountsAsync(startDate, endDate);
    }
    
    public async Task<List<UserActivity>> GetRecentActivitiesAsync(int limit = 10)
    {
        return await _context.UserActivities
            .OrderByDescending(a => a.Timestamp)
            .Take(limit)
            .Include(a => a.User)
            .ToListAsync();
    }
}
