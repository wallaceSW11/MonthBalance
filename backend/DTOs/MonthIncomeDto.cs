using backend.Models;

namespace backend.DTOs;

public record MonthIncomeDto(
    int Id,
    int IncomeId,
    string IncomeDescription,
    IncomeTypeEnum IncomeType,
    decimal? GrossValue,
    decimal? NetValue,
    decimal? HourlyRate,
    int? Hours,
    int? Minutes
);

public record CreateMonthIncomeDto(
    int IncomeId,
    decimal? GrossValue,
    decimal? NetValue,
    decimal? HourlyRate,
    int? Hours,
    int? Minutes
);

public record UpdateMonthIncomeDto(
    decimal? GrossValue,
    decimal? NetValue,
    decimal? HourlyRate,
    int? Hours,
    int? Minutes
);
