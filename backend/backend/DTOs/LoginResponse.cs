namespace MonthBalance.API.DTOs;

public sealed record LoginResponse(
    string Token,
    UserDto User
);
