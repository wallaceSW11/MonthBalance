using backend.DTOs;

namespace backend.Services;

public interface IMonthIncomeService
{
    Task<List<MonthIncomeDto>> GetByMonthAsync(int userId, int year, int month);
    Task<MonthIncomeDto> AddToMonthAsync(int userId, int year, int month, CreateMonthIncomeDto dto);
    Task<MonthIncomeDto> UpdateAsync(int id, UpdateMonthIncomeDto dto);
    Task DeleteAsync(int id);
}
