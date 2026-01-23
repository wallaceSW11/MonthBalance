using backend.DTOs;
using backend.Models;
using backend.Repositories;

namespace backend.Services;

public class ExpenseService : IExpenseService
{
    private readonly IExpenseRepository _repository;

    public ExpenseService(IExpenseRepository repository)
    {
        _repository = repository;
    }

    public async Task<List<ExpenseDto>> GetByUserIdAsync(int userId)
    {
        var expenses = await _repository.GetByUserIdAsync(userId);
        return expenses.Select(MapToDto).ToList();
    }

    public async Task<ExpenseDto?> GetByIdAsync(int id)
    {
        var expense = await _repository.GetByIdAsync(id);
        return expense != null ? MapToDto(expense) : null;
    }

    public async Task<ExpenseDto> CreateAsync(int userId, CreateExpenseDto dto)
    {
        var expense = new Expense
        {
            Description = dto.Description,
            UserId = userId
        };

        var created = await _repository.CreateAsync(expense);
        return MapToDto(created);
    }

    public async Task<ExpenseDto> UpdateAsync(int id, UpdateExpenseDto dto)
    {
        var expense = await _repository.GetByIdAsync(id);
        
        if (expense == null)
            throw new InvalidOperationException("Despesa não encontrada");

        expense.Description = dto.Description;

        var updated = await _repository.UpdateAsync(expense);
        return MapToDto(updated);
    }

    public async Task DeleteAsync(int id)
    {
        var hasMonthExpenses = await _repository.HasMonthExpensesAsync(id);
        
        if (hasMonthExpenses)
            throw new InvalidOperationException("Não é possível deletar despesa vinculada a meses");

        await _repository.DeleteAsync(id);
    }

    private static ExpenseDto MapToDto(Expense expense)
    {
        return new ExpenseDto(
            expense.Id,
            expense.Description
        );
    }
}
