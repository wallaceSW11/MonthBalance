using MonthBalance.API.Models;

namespace MonthBalance.API.Repositories;

public interface IIncomeTypeRepository
{
    Task<List<IncomeTypeModel>> GetByUserIdAsync(int userId);
    Task<IncomeTypeModel?> GetByIdAsync(int id);
    Task<IncomeTypeModel> CreateAsync(IncomeTypeModel incomeType);
    Task UpdateAsync(IncomeTypeModel incomeType);
    Task DeleteAsync(int id);
    Task<bool> HasIncomesAsync(int incomeTypeId);
}
