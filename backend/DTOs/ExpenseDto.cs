namespace backend.DTOs;

public record ExpenseDto(
    int Id,
    string Description
);

public record CreateExpenseDto(
    string Description
);

public record UpdateExpenseDto(
    string Description
);
