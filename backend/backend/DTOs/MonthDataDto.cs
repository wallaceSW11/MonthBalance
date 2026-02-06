namespace MonthBalance.API.DTOs;

public sealed record MonthDataDto(
    int Id,
    int Year,
    int Month,
    DateTime LastAccessed
);
