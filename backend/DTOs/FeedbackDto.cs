namespace MonthBalance.API.DTOs;

public record FeedbackDto(
    int Id,
    int? UserId,
    string UserName,
    string Email,
    string Subject,
    string Message,
    int? Rating,
    DateTime CreatedAt,
    bool IsRead,
    string? AdminNotes
);
