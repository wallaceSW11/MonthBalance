using MonthBalance.Models;

namespace MonthBalance.DTOs;

public record IncomeDto(
    int Id,
    int IncomeTypeId,
    string IncomeTypeName,
    IncomeTypeEnum IncomeTypeType,
    decimal? GrossValue,
    decimal? NetValue,
    decimal? HourlyRate,
    int? Hours,
    int? Minutes,
    int MonthDataId
);

public record CreateIncomeDto(
    int IncomeTypeId,
    decimal? GrossValue,
    decimal? NetValue,
    decimal? HourlyRate,
    int? Hours,
    int? Minutes
);

public record UpdateIncomeDto(
    int IncomeTypeId,
    decimal? GrossValue,
    decimal? NetValue,
    decimal? HourlyRate,
    int? Hours,
    int? Minutes
);

