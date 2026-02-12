using Microsoft.EntityFrameworkCore;
using MonthBalance.API.Data;
using MonthBalance.API.DTOs;

namespace MonthBalance.API.Services;

public class AdminService : IAdminService
{
    private readonly ApplicationDbContext _context;
    private readonly IFeedbackService _feedbackService;
    
    public AdminService(ApplicationDbContext context, IFeedbackService feedbackService)
    {
        _context = context;
        _feedbackService = feedbackService;
    }
    
    public async Task<AdminDashboardDto> GetDashboardAsync()
    {
        var now = DateTime.UtcNow;
        var today = now.Date;
        var weekAgo = now.AddDays(-7);
        var monthAgo = now.AddDays(-30);
        
        var totalUsers = await _context.Users.CountAsync();
        
        var newUsersToday = await _context.Users
            .Where(u => u.CreatedAt >= today)
            .CountAsync();
        
        var newUsersThisWeek = await _context.Users
            .Where(u => u.CreatedAt >= weekAgo)
            .CountAsync();
        
        var newUsersThisMonth = await _context.Users
            .Where(u => u.CreatedAt >= monthAgo)
            .CountAsync();
        
        var activeUsersToday = await _context.UserSessions
            .Where(s => s.LoginAt >= today)
            .Select(s => s.UserId)
            .Distinct()
            .CountAsync();
        
        var activeUsersThisWeek = await _context.UserSessions
            .Where(s => s.LoginAt >= weekAgo)
            .Select(s => s.UserId)
            .Distinct()
            .CountAsync();
        
        var activeUsersThisMonth = await _context.UserSessions
            .Where(s => s.LoginAt >= monthAgo)
            .Select(s => s.UserId)
            .Distinct()
            .CountAsync();
        
        var unreadFeedbacks = await _feedbackService.GetUnreadCountAsync();
        
        var recentUsers = await GetRecentUsersAsync(5);
        
        return new AdminDashboardDto(
            totalUsers,
            newUsersToday,
            newUsersThisWeek,
            newUsersThisMonth,
            activeUsersToday,
            activeUsersThisWeek,
            activeUsersThisMonth,
            unreadFeedbacks,
            recentUsers
        );
    }
    
    public async Task<UserListResponseDto> GetUsersAsync(string? search = null, int page = 1, int pageSize = 20)
    {
        var query = _context.Users.AsQueryable();
        
        if (!string.IsNullOrWhiteSpace(search))
        {
            var searchLower = search.ToLower();
            query = query.Where(u => 
                u.Name.ToLower().Contains(searchLower) || 
                u.Email.ToLower().Contains(searchLower)
            );
        }
        
        var totalCount = await query.CountAsync();
        
        var users = await query
            .OrderByDescending(u => u.CreatedAt)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .Select(u => new
            {
                User = u,
                LastLogin = _context.UserSessions
                    .Where(s => s.UserId == u.Id)
                    .OrderByDescending(s => s.LoginAt)
                    .Select(s => s.LoginAt)
                    .FirstOrDefault(),
                TotalLogins = _context.UserSessions
                    .Where(s => s.UserId == u.Id)
                    .Count()
            })
            .ToListAsync();
        
        var userSummaries = users.Select(u => new UserSummaryDto(
            u.User.Id,
            u.User.Name,
            u.User.Email,
            u.User.CreatedAt,
            u.LastLogin == default ? null : u.LastLogin,
            u.TotalLogins,
            u.LastLogin >= DateTime.UtcNow.AddDays(-7)
        )).ToList();
        
        return new UserListResponseDto(userSummaries, totalCount, page, pageSize);
    }
    
    public async Task<UserSummaryDto?> GetUserSummaryAsync(int userId)
    {
        var user = await _context.Users.FindAsync(userId);
        
        if (user == null) return null;
        
        var lastLogin = await _context.UserSessions
            .Where(s => s.UserId == userId)
            .OrderByDescending(s => s.LoginAt)
            .Select(s => s.LoginAt)
            .FirstOrDefaultAsync();
        
        var totalLogins = await _context.UserSessions
            .Where(s => s.UserId == userId)
            .CountAsync();
        
        var isActive = lastLogin >= DateTime.UtcNow.AddDays(-7);
        
        return new UserSummaryDto(
            user.Id,
            user.Name,
            user.Email,
            user.CreatedAt,
            lastLogin == default ? null : lastLogin,
            totalLogins,
            isActive
        );
    }
    
    private async Task<List<UserSummaryDto>> GetRecentUsersAsync(int limit)
    {
        var users = await _context.Users
            .OrderByDescending(u => u.CreatedAt)
            .Take(limit)
            .Select(u => new
            {
                User = u,
                LastLogin = _context.UserSessions
                    .Where(s => s.UserId == u.Id)
                    .OrderByDescending(s => s.LoginAt)
                    .Select(s => s.LoginAt)
                    .FirstOrDefault(),
                TotalLogins = _context.UserSessions
                    .Where(s => s.UserId == u.Id)
                    .Count()
            })
            .ToListAsync();
        
        return users.Select(u => new UserSummaryDto(
            u.User.Id,
            u.User.Name,
            u.User.Email,
            u.User.CreatedAt,
            u.LastLogin == default ? null : u.LastLogin,
            u.TotalLogins,
            u.LastLogin >= DateTime.UtcNow.AddDays(-7)
        )).ToList();
    }
}
