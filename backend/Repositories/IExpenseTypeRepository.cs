using MonthBalance.API.Models;

namespace MonthBalance.API.Repositories;

public interface IExpenseTypeRepository
{
    Task<List<ExpenseTypeModel>> GetByUserIdAsync(int userId);
    Task<ExpenseTypeModel?> GetByIdAsync(int id);
    Task<ExpenseTypeModel> CreateAsync(ExpenseTypeModel expenseType);
    Task UpdateAsync(ExpenseTypeModel expenseType);
    Task DeleteAsync(int id);
    Task<bool> HasExpensesAsync(int expenseTypeId);
}
