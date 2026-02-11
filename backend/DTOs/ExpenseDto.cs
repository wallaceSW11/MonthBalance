namespace MonthBalance.API.DTOs;

public sealed record ExpenseDto(
    int Id,
    int MonthDataId,
    int ExpenseTypeId,
    decimal Value
);
