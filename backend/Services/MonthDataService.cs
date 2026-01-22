using MonthBalance.DTOs;
using MonthBalance.Models;
using MonthBalance.Repositories;

namespace MonthBalance.Services;

public class MonthDataService : IMonthDataService
{
    private readonly IMonthDataRepository _monthDataRepository;

    public MonthDataService(IMonthDataRepository monthDataRepository)
    {
        _monthDataRepository = monthDataRepository;
    }

    public async Task<MonthDataDto?> GetByIdAsync(int id)
    {
        var monthData = await _monthDataRepository.GetByIdAsync(id);

        return monthData == null ? null : MapToDto(monthData);
    }

    public async Task<MonthDataDto?> GetByYearAndMonthAsync(int year, int month)
    {
        var monthData = await _monthDataRepository.GetByYearAndMonthAsync(year, month);

        return monthData == null ? null : MapToDto(monthData);
    }

    public async Task<IEnumerable<MonthDataDto>> GetAllAsync()
    {
        var monthDataList = await _monthDataRepository.GetAllAsync();

        return monthDataList.Select(MapToDto);
    }

    public async Task<MonthDataDto> CreateAsync(CreateMonthDataDto dto)
    {
        if (await _monthDataRepository.ExistsAsync(dto.Year, dto.Month))
        {
            throw new InvalidOperationException($"Month data for {dto.Year}/{dto.Month} already exists");
        }

        var monthData = new MonthData
        {
            Year = dto.Year,
            Month = dto.Month,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };

        var created = await _monthDataRepository.CreateAsync(monthData);

        return MapToDto(created);
    }

    public async Task DeleteAsync(int id)
    {
        await _monthDataRepository.DeleteAsync(id);
    }

    public async Task<MonthDataDto> DuplicateMonthAsync(int sourceYear, int sourceMonth, int targetYear, int targetMonth)
    {
        var sourceMonthData = await _monthDataRepository.GetByYearAndMonthAsync(sourceYear, sourceMonth);

        if (sourceMonthData == null)
        {
            throw new InvalidOperationException($"Source month {sourceYear}/{sourceMonth} not found");
        }

        if (await _monthDataRepository.ExistsAsync(targetYear, targetMonth))
        {
            throw new InvalidOperationException($"Target month {targetYear}/{targetMonth} already exists");
        }

        var newMonthData = new MonthData
        {
            Year = targetYear,
            Month = targetMonth,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };

        foreach (var income in sourceMonthData.Incomes)
        {
            newMonthData.Incomes.Add(new Income
            {
                Name = income.Name,
                Type = income.Type,
                GrossValue = income.GrossValue,
                NetValue = income.NetValue,
                HourlyRate = income.HourlyRate,
                Hours = income.Hours,
                Minutes = income.Minutes,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            });
        }

        foreach (var expense in sourceMonthData.Expenses)
        {
            newMonthData.Expenses.Add(new Expense
            {
                Name = expense.Name,
                Value = expense.Value,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            });
        }

        var created = await _monthDataRepository.CreateAsync(newMonthData);

        return MapToDto(created);
    }

    private static MonthDataDto MapToDto(MonthData monthData)
    {
        var incomes = monthData.Incomes.Select(i => new IncomeDto
        {
            Id = i.Id,
            Name = i.Name,
            Type = i.Type,
            GrossValue = i.GrossValue,
            NetValue = i.NetValue,
            HourlyRate = i.HourlyRate,
            Hours = i.Hours,
            Minutes = i.Minutes
        }).ToList();

        var expenses = monthData.Expenses.Select(e => new ExpenseDto
        {
            Id = e.Id,
            Name = e.Name,
            Value = e.Value
        }).ToList();

        var totalIncome = incomes.Sum(i => i.NetValue ?? 0);
        var totalExpense = expenses.Sum(e => e.Value);

        return new MonthDataDto
        {
            Id = monthData.Id,
            Year = monthData.Year,
            Month = monthData.Month,
            Incomes = incomes,
            Expenses = expenses,
            TotalIncome = totalIncome,
            TotalExpense = totalExpense,
            Balance = totalIncome - totalExpense
        };
    }
}
