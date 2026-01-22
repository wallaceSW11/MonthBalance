using MonthBalance.DTOs;

namespace MonthBalance.Services;

public interface IIncomeTypeService
{
    Task<List<IncomeTypeDto>> GetAllAsync();
    Task<IncomeTypeDto?> GetByIdAsync(int id);
    Task<IncomeTypeDto> CreateAsync(CreateIncomeTypeDto dto);
    Task<IncomeTypeDto> UpdateAsync(int id, UpdateIncomeTypeDto dto);
    Task DeleteAsync(int id);
}
