using MonthBalance.API.DTOs;
using MonthBalance.API.Models;
using MonthBalance.API.Repositories;

namespace MonthBalance.API.Services;

public class ExpenseTypeService : IExpenseTypeService
{
    private readonly IExpenseTypeRepository _expenseTypeRepository;
    
    public ExpenseTypeService(IExpenseTypeRepository expenseTypeRepository)
    {
        _expenseTypeRepository = expenseTypeRepository;
    }
    
    public async Task<List<ExpenseTypeDto>> GetAllByUserAsync(int userId)
    {
        var expenseTypes = await _expenseTypeRepository.GetByUserIdAsync(userId);
        
        return expenseTypes.Select(MapToDto).ToList();
    }
    
    public async Task<ExpenseTypeDto> GetByIdAsync(int userId, int id)
    {
        var expenseType = await _expenseTypeRepository.GetByIdAsync(id);
        
        if (expenseType == null)
            throw new InvalidOperationException("Expense type not found");
        
        if (expenseType.UserId != userId)
            throw new UnauthorizedAccessException("Access denied");
        
        return MapToDto(expenseType);
    }
    
    public async Task<ExpenseTypeDto> CreateAsync(int userId, CreateExpenseTypeRequest request)
    {
        var expenseType = new ExpenseTypeModel
        {
            UserId = userId,
            Name = request.Name
        };
        
        var created = await _expenseTypeRepository.CreateAsync(expenseType);
        
        return MapToDto(created);
    }
    
    public async Task<ExpenseTypeDto> UpdateAsync(int userId, int id, UpdateExpenseTypeRequest request)
    {
        var expenseType = await _expenseTypeRepository.GetByIdAsync(id);
        
        if (expenseType == null)
            throw new InvalidOperationException("Expense type not found");
        
        if (expenseType.UserId != userId)
            throw new UnauthorizedAccessException("Access denied");
        
        expenseType.Name = request.Name;
        
        await _expenseTypeRepository.UpdateAsync(expenseType);
        
        return MapToDto(expenseType);
    }
    
    public async Task DeleteAsync(int userId, int id)
    {
        var expenseType = await _expenseTypeRepository.GetByIdAsync(id);
        
        if (expenseType == null)
            throw new InvalidOperationException("Expense type not found");
        
        if (expenseType.UserId != userId)
            throw new UnauthorizedAccessException("Access denied");
        
        if (await _expenseTypeRepository.HasExpensesAsync(id))
            throw new InvalidOperationException("Cannot delete expense type with associated expenses");
        
        await _expenseTypeRepository.DeleteAsync(id);
    }
    
    private static ExpenseTypeDto MapToDto(ExpenseTypeModel expenseType)
    {
        return new ExpenseTypeDto(
            expenseType.Id,
            expenseType.Name
        );
    }
}
