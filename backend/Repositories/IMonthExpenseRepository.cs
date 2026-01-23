using backend.Models;

namespace backend.Repositories;

public interface IMonthExpenseRepository
{
    Task<List<MonthExpense>> GetByMonthDataIdAsync(int monthDataId);
    Task<MonthExpense?> GetByIdAsync(int id);
    Task<bool> ExistsAsync(int monthDataId, int expenseId);
    Task<MonthExpense> CreateAsync(MonthExpense monthExpense);
    Task<MonthExpense> UpdateAsync(MonthExpense monthExpense);
    Task DeleteAsync(int id);
}
