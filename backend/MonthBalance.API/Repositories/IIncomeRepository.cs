using MonthBalance.API.Models;

namespace MonthBalance.API.Repositories;

public interface IIncomeRepository
{
    Task<Income?> GetByIdAsync(int id);
    Task<IEnumerable<Income>> GetByMonthDataIdAsync(int monthDataId);
    Task<Income> CreateAsync(Income income);
    Task<Income> UpdateAsync(Income income);
    Task DeleteAsync(int id);
}
