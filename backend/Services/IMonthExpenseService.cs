using backend.DTOs;

namespace backend.Services;

public interface IMonthExpenseService
{
    Task<List<MonthExpenseDto>> GetByMonthAsync(int userId, int year, int month);
    Task<MonthExpenseDto> AddToMonthAsync(int userId, int year, int month, CreateMonthExpenseDto dto);
    Task<MonthExpenseDto> UpdateAsync(int id, UpdateMonthExpenseDto dto);
    Task DeleteAsync(int id);
}
