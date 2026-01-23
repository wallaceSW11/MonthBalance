using backend.DTOs;
using backend.Models;
using backend.Repositories;

namespace backend.Services;

public class MonthExpenseService : IMonthExpenseService
{
    private readonly IMonthDataRepository _monthDataRepository;
    private readonly IMonthExpenseRepository _monthExpenseRepository;
    private readonly IExpenseRepository _expenseRepository;

    public MonthExpenseService(
        IMonthDataRepository monthDataRepository,
        IMonthExpenseRepository monthExpenseRepository,
        IExpenseRepository expenseRepository)
    {
        _monthDataRepository = monthDataRepository;
        _monthExpenseRepository = monthExpenseRepository;
        _expenseRepository = expenseRepository;
    }

    public async Task<List<MonthExpenseDto>> GetByMonthAsync(int userId, int year, int month)
    {
        var monthData = await _monthDataRepository.GetByYearAndMonthAsync(userId, year, month);

        if (monthData == null) return new List<MonthExpenseDto>();

        var monthExpenses = await _monthExpenseRepository.GetByMonthDataIdAsync(monthData.Id);
        return monthExpenses.Select(MapToDto).ToList();
    }

    public async Task<MonthExpenseDto> AddToMonthAsync(int userId, int year, int month, CreateMonthExpenseDto dto)
    {
        var monthData = await _monthDataRepository.GetByYearAndMonthAsync(userId, year, month);

        if (monthData == null)
        {
            monthData = await _monthDataRepository.CreateAsync(new MonthData
            {
                Year = year,
                Month = month,
                UserId = userId
            });
        }

        var expense = await _expenseRepository.GetByIdAsync(dto.ExpenseId);

        if (expense == null)
            throw new InvalidOperationException("Despesa não encontrada");

        var exists = await _monthExpenseRepository.ExistsAsync(monthData.Id, dto.ExpenseId);

        if (exists)
            throw new InvalidOperationException("Despesa já adicionada a este mês");

        var monthExpense = new MonthExpense
        {
            MonthDataId = monthData.Id,
            ExpenseId = dto.ExpenseId,
            Value = dto.Value
        };

        var created = await _monthExpenseRepository.CreateAsync(monthExpense);
        var result = await _monthExpenseRepository.GetByIdAsync(created.Id);
        return MapToDto(result!);
    }

    public async Task<MonthExpenseDto> UpdateAsync(int id, UpdateMonthExpenseDto dto)
    {
        var monthExpense = await _monthExpenseRepository.GetByIdAsync(id);

        if (monthExpense == null)
            throw new InvalidOperationException("Vínculo de despesa não encontrado");

        monthExpense.Value = dto.Value;

        var updated = await _monthExpenseRepository.UpdateAsync(monthExpense);
        var result = await _monthExpenseRepository.GetByIdAsync(updated.Id);
        return MapToDto(result!);
    }

    public async Task DeleteAsync(int id)
    {
        await _monthExpenseRepository.DeleteAsync(id);
    }

    private static MonthExpenseDto MapToDto(MonthExpense monthExpense)
    {
        return new MonthExpenseDto(
            monthExpense.Id,
            monthExpense.ExpenseId,
            monthExpense.Expense.Description,
            monthExpense.Value
        );
    }
}
