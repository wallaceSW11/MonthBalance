namespace MonthBalance.API.DTOs;

public record CreateFeedbackRequest(
    string Subject,
    string Message,
    int? Rating
);
