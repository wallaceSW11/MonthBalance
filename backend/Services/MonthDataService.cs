using backend.DTOs;
using backend.Models;
using backend.Repositories;

namespace backend.Services;

public class MonthDataService : IMonthDataService
{
    private readonly IMonthDataRepository _monthDataRepository;
    private readonly IMonthIncomeRepository _monthIncomeRepository;
    private readonly IMonthExpenseRepository _monthExpenseRepository;

    public MonthDataService(
        IMonthDataRepository monthDataRepository,
        IMonthIncomeRepository monthIncomeRepository,
        IMonthExpenseRepository monthExpenseRepository)
    {
        _monthDataRepository = monthDataRepository;
        _monthIncomeRepository = monthIncomeRepository;
        _monthExpenseRepository = monthExpenseRepository;
    }

    public async Task<MonthDataDto> GetOrCreateAsync(int userId, int year, int month)
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

        return MapToDto(monthData);
    }

    public async Task<List<MonthDataDto>> GetByUserIdAsync(int userId)
    {
        var monthDataList = await _monthDataRepository.GetByUserIdAsync(userId);
        return monthDataList.Select(MapToDto).ToList();
    }

    public async Task<MonthDataDto> DuplicateAsync(int userId, DuplicateMonthDto dto)
    {
        var source = await _monthDataRepository.GetByYearAndMonthAsync(
            userId, dto.SourceYear, dto.SourceMonth);

        if (source == null)
            throw new InvalidOperationException("Mês de origem não encontrado");

        var existing = await _monthDataRepository.GetByYearAndMonthAsync(
            userId, dto.TargetYear, dto.TargetMonth);

        if (existing != null)
        {
            await _monthDataRepository.DeleteAsync(existing.Id);
        }

        var target = await _monthDataRepository.CreateAsync(new MonthData
        {
            Year = dto.TargetYear,
            Month = dto.TargetMonth,
            UserId = userId
        });

        foreach (var monthIncome in source.MonthIncomes)
        {
            await _monthIncomeRepository.CreateAsync(new MonthIncome
            {
                MonthDataId = target.Id,
                IncomeId = monthIncome.IncomeId,
                GrossValue = monthIncome.GrossValue,
                NetValue = monthIncome.NetValue,
                HourlyRate = monthIncome.HourlyRate,
                Hours = monthIncome.Hours,
                Minutes = monthIncome.Minutes
            });
        }

        foreach (var monthExpense in source.MonthExpenses)
        {
            await _monthExpenseRepository.CreateAsync(new MonthExpense
            {
                MonthDataId = target.Id,
                ExpenseId = monthExpense.ExpenseId,
                Value = monthExpense.Value
            });
        }

        var result = await _monthDataRepository.GetByIdAsync(target.Id);
        return MapToDto(result!);
    }

    public async Task DeleteAsync(int userId, int year, int month)
    {
        var monthData = await _monthDataRepository.GetByYearAndMonthAsync(userId, year, month);

        if (monthData == null) return;

        await _monthDataRepository.DeleteAsync(monthData.Id);
    }

    private static MonthDataDto MapToDto(MonthData monthData)
    {
        var incomes = monthData.MonthIncomes.Select(mi => new MonthIncomeDto(
            mi.Id,
            mi.IncomeId,
            mi.Income.Description,
            mi.Income.Type,
            mi.GrossValue,
            mi.NetValue,
            mi.HourlyRate,
            mi.Hours,
            mi.Minutes
        )).ToList();

        var expenses = monthData.MonthExpenses.Select(me => new MonthExpenseDto(
            me.Id,
            me.ExpenseId,
            me.Expense.Description,
            me.Value
        )).ToList();

        var totalIncome = incomes.Sum(i => i.NetValue ?? 0);
        var totalExpense = expenses.Sum(e => e.Value);
        var balance = totalIncome - totalExpense;

        return new MonthDataDto(
            monthData.Id,
            monthData.Year,
            monthData.Month,
            incomes,
            expenses,
            totalIncome,
            totalExpense,
            balance
        );
    }
}
