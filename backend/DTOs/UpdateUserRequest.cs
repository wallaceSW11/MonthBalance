namespace MonthBalance.API.DTOs;

public sealed record UpdateUserRequest(
    string Name,
    string? Avatar,
    bool NotificationsEnabled
);
