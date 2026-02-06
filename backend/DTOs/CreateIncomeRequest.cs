namespace MonthBalance.API.DTOs;

public sealed record CreateIncomeRequest(
    int MonthDataId,
    int IncomeTypeId,
    decimal? GrossValue,
    decimal? NetValue,
    decimal? HourlyRate,
    int? Hours,
    int? Minutes
);
