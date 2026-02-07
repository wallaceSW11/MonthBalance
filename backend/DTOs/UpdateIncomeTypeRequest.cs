namespace MonthBalance.API.DTOs;

public sealed record UpdateIncomeTypeRequest(
    string Name,
    string Type
);
