namespace MonthBalance.API.DTOs;

public sealed record IncomeDto(
    int Id,
    int MonthDataId,
    int IncomeTypeId,
    decimal? GrossValue,
    decimal? NetValue,
    decimal? HourlyRate,
    int? Hours,
    int? Minutes,
    decimal CalculatedValue
);
