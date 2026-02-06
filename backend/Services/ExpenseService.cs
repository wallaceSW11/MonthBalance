using MonthBalance.API.DTOs;
using MonthBalance.API.Models;
using MonthBalance.API.Repositories;

namespace MonthBalance.API.Services;

public class ExpenseService : IExpenseService
{
    private readonly IExpenseRepository _expenseRepository;
    private readonly IMonthDataRepository _monthDataRepository;
    private readonly IExpenseTypeRepository _expenseTypeRepository;
    
    public ExpenseService(
        IExpenseRepository expenseRepository,
        IMonthDataRepository monthDataRepository,
        IExpenseTypeRepository expenseTypeRepository)
    {
        _expenseRepository = expenseRepository;
        _monthDataRepository = monthDataRepository;
        _expenseTypeRepository = expenseTypeRepository;
    }
    
    public async Task<List<ExpenseDto>> GetByMonthDataIdAsync(int userId, int monthDataId)
    {
        var monthData = await _monthDataRepository.GetByIdAsync(monthDataId);
        
        if (monthData == null)
            throw new InvalidOperationException("Month data not found");
        
        if (monthData.UserId != userId)
            throw new UnauthorizedAccessException("Access denied");
        
        var expenses = await _expenseRepository.GetByMonthDataIdAsync(monthDataId);
        
        return expenses.Select(MapToDto).ToList();
    }
    
    public async Task<ExpenseDto> GetByIdAsync(int userId, int id)
    {
        var expense = await _expenseRepository.GetByIdAsync(id);
        
        if (expense == null)
            throw new InvalidOperationException("Expense not found");
        
        if (expense.MonthData.UserId != userId)
            throw new UnauthorizedAccessException("Access denied");
        
        return MapToDto(expense);
    }
    
    public async Task<ExpenseDto> CreateAsync(int userId, CreateExpenseRequest request)
    {
        var monthData = await _monthDataRepository.GetByIdAsync(request.MonthDataId);
        
        if (monthData == null)
            throw new InvalidOperationException("Month data not found");
        
        if (monthData.UserId != userId)
            throw new UnauthorizedAccessException("Access denied");
        
        var expenseType = await _expenseTypeRepository.GetByIdAsync(request.ExpenseTypeId);
        
        if (expenseType == null)
            throw new InvalidOperationException("Expense type not found");
        
        if (expenseType.UserId != userId)
            throw new UnauthorizedAccessException("Access denied");
        
        var expense = new Expense
        {
            MonthDataId = request.MonthDataId,
            ExpenseTypeId = request.ExpenseTypeId,
            Value = request.Value
        };
        
        var created = await _expenseRepository.CreateAsync(expense);
        
        return MapToDto(created);
    }
    
    public async Task<ExpenseDto> UpdateAsync(int userId, int id, UpdateExpenseRequest request)
    {
        var expense = await _expenseRepository.GetByIdAsync(id);
        
        if (expense == null)
            throw new InvalidOperationException("Expense not found");
        
        if (expense.MonthData.UserId != userId)
            throw new UnauthorizedAccessException("Access denied");
        
        expense.Value = request.Value;
        
        await _expenseRepository.UpdateAsync(expense);
        
        return MapToDto(expense);
    }
    
    public async Task DeleteAsync(int userId, int id)
    {
        var expense = await _expenseRepository.GetByIdAsync(id);
        
        if (expense == null)
            throw new InvalidOperationException("Expense not found");
        
        if (expense.MonthData.UserId != userId)
            throw new UnauthorizedAccessException("Access denied");
        
        await _expenseRepository.DeleteAsync(id);
    }
    
    private static ExpenseDto MapToDto(Expense expense)
    {
        return new ExpenseDto(
            expense.Id,
            expense.MonthDataId,
            expense.ExpenseTypeId,
            expense.Value
        );
    }
}
