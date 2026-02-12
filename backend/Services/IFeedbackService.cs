using MonthBalance.API.DTOs;

namespace MonthBalance.API.Services;

public interface IFeedbackService
{
    Task<FeedbackDto> CreateAsync(int? userId, string email, CreateFeedbackRequest request);
    Task<List<FeedbackDto>> GetAllAsync(bool? isRead = null, int page = 1, int pageSize = 20);
    Task<FeedbackDto?> GetByIdAsync(int id);
    Task MarkAsReadAsync(int id);
    Task<int> GetUnreadCountAsync();
}
