namespace backend.DTOs;

public record MonthDataDto(
    int Id,
    int Year,
    int Month,
    List<MonthIncomeDto> Incomes,
    List<MonthExpenseDto> Expenses,
    decimal TotalIncome,
    decimal TotalExpense,
    decimal Balance
);

public record DuplicateMonthDto(
    int SourceYear,
    int SourceMonth,
    int TargetYear,
    int TargetMonth
);
