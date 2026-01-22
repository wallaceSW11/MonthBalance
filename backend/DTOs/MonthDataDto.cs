namespace MonthBalance.DTOs;

public class MonthDataDto
{
    public int Id { get; set; }
    public int Year { get; set; }
    public int Month { get; set; }
    public List<IncomeDto> Incomes { get; set; } = new();
    public List<ExpenseDto> Expenses { get; set; } = new();
    public decimal TotalIncome { get; set; }
    public decimal TotalExpense { get; set; }
    public decimal Balance { get; set; }
}

public class CreateMonthDataDto
{
    public int Year { get; set; }
    public int Month { get; set; }
}

public class MonthKeyDto
{
    public int Year { get; set; }
    public int Month { get; set; }
}
