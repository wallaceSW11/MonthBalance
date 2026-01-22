using MonthBalance.Models;

namespace MonthBalance.DTOs;

public record IncomeTypeDto(
    int Id,
    string Name,
    IncomeTypeEnum Type
);

public record CreateIncomeTypeDto(
    string Name,
    IncomeTypeEnum Type
);

public record UpdateIncomeTypeDto(
    string Name,
    IncomeTypeEnum Type
);
