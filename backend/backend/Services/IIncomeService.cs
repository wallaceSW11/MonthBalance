using MonthBalance.API.DTOs;

namespace MonthBalance.API.Services;

public interface IIncomeService
{
    Task<List<IncomeDto>> GetByMonthDataIdAsync(int userId, int monthDataId);
    Task<IncomeDto> GetByIdAsync(int userId, int id);
    Task<IncomeDto> CreateAsync(int userId, CreateIncomeRequest request);
    Task<IncomeDto> UpdateAsync(int userId, int id, UpdateIncomeRequest request);
    Task DeleteAsync(int userId, int id);
}
