namespace MonthBalance.API.DTOs;

public sealed record CreateMonthDataRequest(
    int Year,
    int Month
);
