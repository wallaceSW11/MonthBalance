namespace MonthBalance.API.DTOs;

public class ExpenseDto
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public decimal Value { get; set; }
}

public class CreateExpenseDto
{
    public string Name { get; set; } = string.Empty;
    public decimal Value { get; set; }
}

public class UpdateExpenseDto
{
    public string Name { get; set; } = string.Empty;
    public decimal Value { get; set; }
}
