namespace MonthBalance.DTOs;

public record ExpenseDto(
    int Id,
    string Name,
    decimal Value,
    int MonthDataId
);

public record CreateExpenseDto(
    string Name,
    decimal Value
);

public record UpdateExpenseDto(
    string Name,
    decimal Value
);

