using System.Security.Claims;
using MonthBalance.API.Models;
using MonthBalance.API.Services;

namespace MonthBalance.API.Middleware;

public class ActivityTrackingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<ActivityTrackingMiddleware> _logger;
    
    public ActivityTrackingMiddleware(RequestDelegate next, ILogger<ActivityTrackingMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }
    
    public async Task InvokeAsync(HttpContext context, IAnalyticsService analyticsService)
    {
        await _next(context);
        
        if (!context.User.Identity?.IsAuthenticated ?? true)
        {
            return;
        }
        
        var userIdClaim = context.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        
        if (string.IsNullOrEmpty(userIdClaim) || !int.TryParse(userIdClaim, out var userId))
        {
            return;
        }
        
        var path = context.Request.Path.Value?.ToLower() ?? string.Empty;
        var method = context.Request.Method;
        
        var activityType = DetermineActivityType(path, method);
        
        if (!activityType.HasValue)
        {
            return;
        }
        
        var ipAddress = context.Connection.RemoteIpAddress?.ToString();
        var userAgent = context.Request.Headers.UserAgent.ToString();
        
        _ = Task.Run(async () =>
        {
            try
            {
                await analyticsService.TrackActivityAsync(userId, activityType.Value, ipAddress, userAgent);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error tracking activity in background");
            }
        });
    }
    
    private static ActivityType? DetermineActivityType(string path, string method)
    {
        if (path.Contains("/api/monthdata") && method == "POST")
            return ActivityType.MonthDataCreated;
        
        if (path.Contains("/api/monthdata") && method == "GET")
            return ActivityType.MonthDataViewed;
        
        if (path.Contains("/api/monthdata") && method == "PUT")
            return ActivityType.MonthDataUpdated;
        
        if (path.Contains("/api/monthdata") && method == "DELETE")
            return ActivityType.MonthDataDeleted;
        
        if (path.Contains("/incomes") && method == "POST")
            return ActivityType.IncomeCreated;
        
        if (path.Contains("/incomes") && method == "PUT")
            return ActivityType.IncomeUpdated;
        
        if (path.Contains("/incomes") && method == "DELETE")
            return ActivityType.IncomeDeleted;
        
        if (path.Contains("/incometypes") && method == "POST")
            return ActivityType.IncomeTypeCreated;
        
        if (path.Contains("/incometypes") && method == "PUT")
            return ActivityType.IncomeTypeUpdated;
        
        if (path.Contains("/incometypes") && method == "DELETE")
            return ActivityType.IncomeTypeDeleted;
        
        if (path.Contains("/expenses") && method == "POST")
            return ActivityType.ExpenseCreated;
        
        if (path.Contains("/expenses") && method == "PUT")
            return ActivityType.ExpenseUpdated;
        
        if (path.Contains("/expenses") && method == "DELETE")
            return ActivityType.ExpenseDeleted;
        
        if (path.Contains("/expensetypes") && method == "POST")
            return ActivityType.ExpenseTypeCreated;
        
        if (path.Contains("/expensetypes") && method == "PUT")
            return ActivityType.ExpenseTypeUpdated;
        
        if (path.Contains("/expensetypes") && method == "DELETE")
            return ActivityType.ExpenseTypeDeleted;
        
        if (path.Contains("/admin"))
            return ActivityType.AdminPanelAccessed;
        
        return null;
    }
}
