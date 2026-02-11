namespace MonthBalance.API.DTOs;

public sealed record CreateIncomeTypeRequest(
    string Name,
    string Type
);
