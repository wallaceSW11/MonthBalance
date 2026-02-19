using Microsoft.EntityFrameworkCore;
using MonthBalance.API.Data;
using MonthBalance.API.Models;

namespace MonthBalance.API.Repositories;

public class ActivityRepository : IActivityRepository
{
    private readonly ApplicationDbContext _context;
    
    public ActivityRepository(ApplicationDbContext context)
    {
        _context = context;
    }
    
    public async Task<UserActivity> CreateAsync(UserActivity activity)
    {
        _context.UserActivities.Add(activity);
        await _context.SaveChangesAsync();
        return activity;
    }
    
    public async Task<List<UserActivity>> GetByUserIdAsync(int userId, int limit = 100)
    {
        return await _context.UserActivities
            .Where(a => a.UserId == userId)
            .OrderByDescending(a => a.Timestamp)
            .Take(limit)
            .Include(a => a.User)
            .ToListAsync();
    }
    
    public async Task<List<UserActivity>> GetByActivityTypeAsync(ActivityType activityType, int limit = 100)
    {
        return await _context.UserActivities
            .Where(a => a.ActivityType == activityType)
            .OrderByDescending(a => a.Timestamp)
            .Take(limit)
            .Include(a => a.User)
            .ToListAsync();
    }
    
    public async Task<List<UserActivity>> GetByDateRangeAsync(DateTime startDate, DateTime endDate, int? userId = null)
    {
        var query = _context.UserActivities
            .Where(a => a.Timestamp >= startDate && a.Timestamp <= endDate);
        
        if (userId.HasValue)
        {
            query = query.Where(a => a.UserId == userId.Value);
        }
        
        return await query
            .OrderByDescending(a => a.Timestamp)
            .Include(a => a.User)
            .ToListAsync();
    }
    
    public async Task<Dictionary<ActivityType, int>> GetActivityCountsAsync(DateTime? startDate = null, DateTime? endDate = null)
    {
        var query = _context.UserActivities.AsQueryable();
        
        if (startDate.HasValue)
        {
            query = query.Where(a => a.Timestamp >= startDate.Value);
        }
        
        if (endDate.HasValue)
        {
            query = query.Where(a => a.Timestamp <= endDate.Value);
        }
        
        return await query
            .GroupBy(a => a.ActivityType)
            .Select(g => new { ActivityType = g.Key, Count = g.Count() })
            .ToDictionaryAsync(x => x.ActivityType, x => x.Count);
    }
}
