using backend.DTOs;

namespace backend.Services;

public interface IExpenseService
{
    Task<List<ExpenseDto>> GetByUserIdAsync(int userId);
    Task<ExpenseDto?> GetByIdAsync(int id);
    Task<ExpenseDto> CreateAsync(int userId, CreateExpenseDto dto);
    Task<ExpenseDto> UpdateAsync(int id, UpdateExpenseDto dto);
    Task DeleteAsync(int id);
}
