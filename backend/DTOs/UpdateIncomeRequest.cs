namespace MonthBalance.API.DTOs;

public sealed record UpdateIncomeRequest(
    decimal? GrossValue,
    decimal? NetValue,
    decimal? HourlyRate,
    int? Hours,
    int? Minutes
);
