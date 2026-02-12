using Microsoft.EntityFrameworkCore;
using MonthBalance.API.Data;
using MonthBalance.API.DTOs;
using MonthBalance.API.Models;

namespace MonthBalance.API.Services;

public class FeedbackService : IFeedbackService
{
    private readonly ApplicationDbContext _context;
    private readonly IEmailService _emailService;
    private readonly IAnalyticsService _analyticsService;
    private readonly ILogger<FeedbackService> _logger;
    
    public FeedbackService(
        ApplicationDbContext context,
        IEmailService emailService,
        IAnalyticsService analyticsService,
        ILogger<FeedbackService> logger)
    {
        _context = context;
        _emailService = emailService;
        _analyticsService = analyticsService;
        _logger = logger;
    }
    
    public async Task<FeedbackDto> CreateAsync(int? userId, string email, CreateFeedbackRequest request)
    {
        string userName = "Anônimo";
        
        if (userId.HasValue)
        {
            var user = await _context.Users.FindAsync(userId.Value);
            if (user != null)
            {
                userName = user.Name;
                email = user.Email;
            }
        }
        
        var feedback = new UserFeedback
        {
            UserId = userId,
            Email = email,
            Subject = request.Subject,
            Message = request.Message,
            Rating = request.Rating,
            CreatedAt = DateTime.UtcNow,
            IsRead = false
        };
        
        _context.UserFeedbacks.Add(feedback);
        await _context.SaveChangesAsync();
        
        try
        {
            await _emailService.SendFeedbackToAdminAsync(userName, email, request.Subject, request.Message, request.Rating);
            await _emailService.SendFeedbackConfirmationAsync(email, userName);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error sending feedback emails");
        }
        
        if (userId.HasValue)
        {
            await _analyticsService.TrackActivityAsync(userId.Value, ActivityType.FeedbackSent);
        }
        
        return MapToDto(feedback, userName);
    }
    
    public async Task<List<FeedbackDto>> GetAllAsync(bool? isRead = null, int page = 1, int pageSize = 20)
    {
        var query = _context.UserFeedbacks
            .Include(f => f.User)
            .AsQueryable();
        
        if (isRead.HasValue)
        {
            query = query.Where(f => f.IsRead == isRead.Value);
        }
        
        var feedbacks = await query
            .OrderByDescending(f => f.CreatedAt)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();
        
        return feedbacks.Select(f => MapToDto(f, f.User?.Name ?? "Anônimo")).ToList();
    }
    
    public async Task<FeedbackDto?> GetByIdAsync(int id)
    {
        var feedback = await _context.UserFeedbacks
            .Include(f => f.User)
            .FirstOrDefaultAsync(f => f.Id == id);
        
        if (feedback == null) return null;
        
        return MapToDto(feedback, feedback.User?.Name ?? "Anônimo");
    }
    
    public async Task MarkAsReadAsync(int id)
    {
        var feedback = await _context.UserFeedbacks.FindAsync(id);
        
        if (feedback == null)
            throw new InvalidOperationException("Feedback não encontrado");
        
        feedback.IsRead = true;
        await _context.SaveChangesAsync();
    }
    
    public async Task<int> GetUnreadCountAsync()
    {
        return await _context.UserFeedbacks.CountAsync(f => !f.IsRead);
    }
    
    private static FeedbackDto MapToDto(UserFeedback feedback, string userName)
    {
        return new FeedbackDto(
            feedback.Id,
            feedback.UserId,
            userName,
            feedback.Email,
            feedback.Subject,
            feedback.Message,
            feedback.Rating,
            feedback.CreatedAt,
            feedback.IsRead,
            feedback.AdminNotes
        );
    }
}
