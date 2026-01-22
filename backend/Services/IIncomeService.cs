using MonthBalance.DTOs;

namespace MonthBalance.Services;

public interface IIncomeService
{
    Task<IncomeDto?> GetByIdAsync(int id);
    Task<IEnumerable<IncomeDto>> GetByMonthAsync(int year, int month);
    Task<IncomeDto> CreateAsync(int year, int month, CreateIncomeDto dto);
    Task<IncomeDto> UpdateAsync(int id, UpdateIncomeDto dto);
    Task DeleteAsync(int id);
}
