using MonthBalance.API.DTOs;

namespace MonthBalance.API.Services;

public interface IIncomeTypeService
{
    Task<List<IncomeTypeDto>> GetAllByUserAsync(int userId);
    Task<IncomeTypeDto> GetByIdAsync(int userId, int id);
    Task<IncomeTypeDto> CreateAsync(int userId, CreateIncomeTypeRequest request);
    Task<IncomeTypeDto> UpdateAsync(int userId, int id, UpdateIncomeTypeRequest request);
    Task DeleteAsync(int userId, int id);
}
