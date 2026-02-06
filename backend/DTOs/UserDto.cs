namespace MonthBalance.API.DTOs;

public sealed record UserDto(
    int Id,
    string Name,
    string Email,
    string? Avatar,
    bool NotificationsEnabled
);
