namespace MonthBalance.API.DTOs;

public record UserSummaryDto(
    int Id,
    string Name,
    string Email,
    DateTime CreatedAt,
    DateTime? LastLoginAt,
    int TotalLogins,
    bool IsActive
);
