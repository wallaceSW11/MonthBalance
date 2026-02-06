using MonthBalance.API.DTOs;

namespace MonthBalance.API.Services;

public interface IExpenseTypeService
{
    Task<List<ExpenseTypeDto>> GetAllByUserAsync(int userId);
    Task<ExpenseTypeDto> GetByIdAsync(int userId, int id);
    Task<ExpenseTypeDto> CreateAsync(int userId, CreateExpenseTypeRequest request);
    Task<ExpenseTypeDto> UpdateAsync(int userId, int id, UpdateExpenseTypeRequest request);
    Task DeleteAsync(int userId, int id);
}
