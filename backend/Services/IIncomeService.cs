using backend.DTOs;

namespace backend.Services;

public interface IIncomeService
{
    Task<List<IncomeDto>> GetByUserIdAsync(int userId);
    Task<IncomeDto?> GetByIdAsync(int id);
    Task<IncomeDto> CreateAsync(int userId, CreateIncomeDto dto);
    Task<IncomeDto> UpdateAsync(int id, UpdateIncomeDto dto);
    Task DeleteAsync(int id);
}
