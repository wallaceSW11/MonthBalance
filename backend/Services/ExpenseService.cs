using MonthBalance.DTOs;
using MonthBalance.Models;
using MonthBalance.Repositories;

namespace MonthBalance.Services;

public class ExpenseService : IExpenseService
{
    private readonly IExpenseRepository _expenseRepository;
    private readonly IMonthDataRepository _monthDataRepository;

    public ExpenseService(IExpenseRepository expenseRepository, IMonthDataRepository monthDataRepository)
    {
        _expenseRepository = expenseRepository;
        _monthDataRepository = monthDataRepository;
    }

    public async Task<ExpenseDto?> GetByIdAsync(int id)
    {
        var expense = await _expenseRepository.GetByIdAsync(id);

        return expense == null ? null : MapToDto(expense);
    }

    public async Task<IEnumerable<ExpenseDto>> GetByMonthAsync(int year, int month)
    {
        var monthData = await _monthDataRepository.GetByYearAndMonthAsync(year, month);

        if (monthData == null)
        {
            return Enumerable.Empty<ExpenseDto>();
        }

        var expenses = await _expenseRepository.GetByMonthDataIdAsync(monthData.Id);

        return expenses.Select(MapToDto);
    }

    public async Task<ExpenseDto> CreateAsync(int year, int month, CreateExpenseDto dto)
    {
        var monthData = await _monthDataRepository.GetByYearAndMonthAsync(year, month);

        if (monthData == null)
        {
            monthData = await _monthDataRepository.CreateAsync(new MonthData
            {
                Year = year,
                Month = month,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            });
        }

        var expense = new Expense
        {
            Name = dto.Name,
            Value = dto.Value,
            MonthDataId = monthData.Id,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };

        var created = await _expenseRepository.CreateAsync(expense);

        return MapToDto(created);
    }

    public async Task<ExpenseDto> UpdateAsync(int id, UpdateExpenseDto dto)
    {
        var expense = await _expenseRepository.GetByIdAsync(id);

        if (expense == null)
        {
            throw new InvalidOperationException($"Expense with id {id} not found");
        }

        expense.Name = dto.Name;
        expense.Value = dto.Value;

        var updated = await _expenseRepository.UpdateAsync(expense);

        return MapToDto(updated);
    }

    public async Task DeleteAsync(int id)
    {
        await _expenseRepository.DeleteAsync(id);
    }

    private static ExpenseDto MapToDto(Expense expense)
    {
        return new ExpenseDto
        {
            Id = expense.Id,
            Name = expense.Name,
            Value = expense.Value
        };
    }
}
