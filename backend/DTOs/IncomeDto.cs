using backend.Models;

namespace backend.DTOs;

public record IncomeDto(
    int Id,
    string Description,
    IncomeTypeEnum Type
);

public record CreateIncomeDto(
    string Description,
    IncomeTypeEnum Type
);

public record UpdateIncomeDto(
    string Description,
    IncomeTypeEnum Type
);
