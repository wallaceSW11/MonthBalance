using MonthBalance.API.Models;

namespace MonthBalance.API.Repositories;

public interface IExpenseRepository
{
    Task<List<Expense>> GetByMonthDataIdAsync(int monthDataId);
    Task<Expense?> GetByIdAsync(int id);
    Task<Expense> CreateAsync(Expense expense);
    Task UpdateAsync(Expense expense);
    Task DeleteAsync(int id);
}
