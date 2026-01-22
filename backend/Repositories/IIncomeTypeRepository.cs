using MonthBalance.Models;

namespace MonthBalance.Repositories;

public interface IIncomeTypeRepository
{
    Task<List<IncomeType>> GetAllAsync();
    Task<IncomeType?> GetByIdAsync(int id);
    Task<IncomeType> CreateAsync(IncomeType incomeType);
    Task UpdateAsync(IncomeType incomeType);
    Task DeleteAsync(int id);
}
