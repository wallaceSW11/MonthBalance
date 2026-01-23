using backend.Models;

namespace backend.Repositories;

public interface IExpenseRepository
{
    Task<List<Expense>> GetByUserIdAsync(int userId);
    Task<Expense?> GetByIdAsync(int id);
    Task<Expense> CreateAsync(Expense expense);
    Task<Expense> UpdateAsync(Expense expense);
    Task DeleteAsync(int id);
    Task<bool> HasMonthExpensesAsync(int expenseId);
}
