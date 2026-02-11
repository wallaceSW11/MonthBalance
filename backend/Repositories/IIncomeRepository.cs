using MonthBalance.API.Models;

namespace MonthBalance.API.Repositories;

public interface IIncomeRepository
{
    Task<List<Income>> GetByMonthDataIdAsync(int monthDataId);
    Task<Income?> GetByIdAsync(int id);
    Task<Income> CreateAsync(Income income);
    Task UpdateAsync(Income income);
    Task DeleteAsync(int id);
}
