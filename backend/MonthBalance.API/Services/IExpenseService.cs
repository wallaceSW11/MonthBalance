using MonthBalance.API.DTOs;

namespace MonthBalance.API.Services;

public interface IExpenseService
{
    Task<ExpenseDto?> GetByIdAsync(int id);
    Task<IEnumerable<ExpenseDto>> GetByMonthAsync(int year, int month);
    Task<ExpenseDto> CreateAsync(int year, int month, CreateExpenseDto dto);
    Task<ExpenseDto> UpdateAsync(int id, UpdateExpenseDto dto);
    Task DeleteAsync(int id);
}
