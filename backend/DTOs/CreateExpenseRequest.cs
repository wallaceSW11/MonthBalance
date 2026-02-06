namespace MonthBalance.API.DTOs;

public sealed record CreateExpenseRequest(
    int MonthDataId,
    int ExpenseTypeId,
    decimal Value
);
