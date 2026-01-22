using MonthBalance.Models;

namespace MonthBalance.Repositories;

public interface IExpenseRepository
{
    Task<Expense?> GetByIdAsync(int id);
    Task<IEnumerable<Expense>> GetByMonthDataIdAsync(int monthDataId);
    Task<Expense> CreateAsync(Expense expense);
    Task<Expense> UpdateAsync(Expense expense);
    Task DeleteAsync(int id);
}
