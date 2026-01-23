using backend.Models;

namespace backend.Repositories;

public interface IMonthIncomeRepository
{
    Task<List<MonthIncome>> GetByMonthDataIdAsync(int monthDataId);
    Task<MonthIncome?> GetByIdAsync(int id);
    Task<bool> ExistsAsync(int monthDataId, int incomeId);
    Task<MonthIncome> CreateAsync(MonthIncome monthIncome);
    Task<MonthIncome> UpdateAsync(MonthIncome monthIncome);
    Task DeleteAsync(int id);
}
