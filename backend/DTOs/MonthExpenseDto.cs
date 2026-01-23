namespace backend.DTOs;

public record MonthExpenseDto(
    int Id,
    int ExpenseId,
    string ExpenseDescription,
    decimal Value
);

public record CreateMonthExpenseDto(
    int ExpenseId,
    decimal Value
);

public record UpdateMonthExpenseDto(
    decimal Value
);
