using MonthBalance.API.DTOs;

namespace MonthBalance.API.Services;

public interface IExpenseService
{
    Task<List<ExpenseDto>> GetByMonthDataIdAsync(int userId, int monthDataId);
    Task<ExpenseDto> GetByIdAsync(int userId, int id);
    Task<ExpenseDto> CreateAsync(int userId, CreateExpenseRequest request);
    Task<ExpenseDto> UpdateAsync(int userId, int id, UpdateExpenseRequest request);
    Task DeleteAsync(int userId, int id);
}
