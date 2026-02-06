namespace MonthBalance.API.DTOs;

public sealed record RegisterRequest(
    string Name,
    string Email,
    string Password
);
