using backend.Models;

namespace backend.Repositories;

public interface IIncomeRepository
{
    Task<List<Income>> GetByUserIdAsync(int userId);
    Task<Income?> GetByIdAsync(int id);
    Task<Income> CreateAsync(Income income);
    Task<Income> UpdateAsync(Income income);
    Task DeleteAsync(int id);
    Task<bool> HasMonthIncomesAsync(int incomeId);
}
