using MonthBalance.Models;

namespace MonthBalance.Data;

public static class DbInitializer
{
    public static void Initialize(ApplicationDbContext context)
    {
        context.Database.EnsureCreated();

        if (context.MonthData.Any())
        {
            return;
        }

        var currentDate = DateTime.Now;
        var monthData = new MonthData
        {
            Year = currentDate.Year,
            Month = currentDate.Month,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };

        context.MonthData.Add(monthData);
        context.SaveChanges();

        var sampleIncome = new Income
        {
            Name = "Sample Salary",
            Type = "manual",
            GrossValue = 5000.00m,
            NetValue = 4000.00m,
            MonthDataId = monthData.Id,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };

        var sampleExpense = new Expense
        {
            Name = "Sample Rent",
            Value = 1500.00m,
            MonthDataId = monthData.Id,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };

        context.Incomes.Add(sampleIncome);
        context.Expenses.Add(sampleExpense);
        context.SaveChanges();
    }
}
