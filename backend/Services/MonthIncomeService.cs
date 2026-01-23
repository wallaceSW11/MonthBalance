using backend.DTOs;
using backend.Models;
using backend.Repositories;

namespace backend.Services;

public class MonthIncomeService : IMonthIncomeService
{
    private readonly IMonthDataRepository _monthDataRepository;
    private readonly IMonthIncomeRepository _monthIncomeRepository;
    private readonly IIncomeRepository _incomeRepository;

    public MonthIncomeService(
        IMonthDataRepository monthDataRepository,
        IMonthIncomeRepository monthIncomeRepository,
        IIncomeRepository incomeRepository)
    {
        _monthDataRepository = monthDataRepository;
        _monthIncomeRepository = monthIncomeRepository;
        _incomeRepository = incomeRepository;
    }

    public async Task<List<MonthIncomeDto>> GetByMonthAsync(int userId, int year, int month)
    {
        var monthData = await _monthDataRepository.GetByYearAndMonthAsync(userId, year, month);

        if (monthData == null) return new List<MonthIncomeDto>();

        var monthIncomes = await _monthIncomeRepository.GetByMonthDataIdAsync(monthData.Id);
        return monthIncomes.Select(MapToDto).ToList();
    }

    public async Task<MonthIncomeDto> AddToMonthAsync(int userId, int year, int month, CreateMonthIncomeDto dto)
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

        var income = await _incomeRepository.GetByIdAsync(dto.IncomeId);

        if (income == null)
            throw new InvalidOperationException("Receita não encontrada");

        var exists = await _monthIncomeRepository.ExistsAsync(monthData.Id, dto.IncomeId);

        if (exists)
            throw new InvalidOperationException("Receita já adicionada a este mês");

        var monthIncome = new MonthIncome
        {
            MonthDataId = monthData.Id,
            IncomeId = dto.IncomeId,
            GrossValue = dto.GrossValue,
            NetValue = dto.NetValue,
            HourlyRate = dto.HourlyRate,
            Hours = dto.Hours,
            Minutes = dto.Minutes
        };

        var created = await _monthIncomeRepository.CreateAsync(monthIncome);
        var result = await _monthIncomeRepository.GetByIdAsync(created.Id);
        return MapToDto(result!);
    }

    public async Task<MonthIncomeDto> UpdateAsync(int id, UpdateMonthIncomeDto dto)
    {
        var monthIncome = await _monthIncomeRepository.GetByIdAsync(id);

        if (monthIncome == null)
            throw new InvalidOperationException("Vínculo de receita não encontrado");

        monthIncome.GrossValue = dto.GrossValue;
        monthIncome.NetValue = dto.NetValue;
        monthIncome.HourlyRate = dto.HourlyRate;
        monthIncome.Hours = dto.Hours;
        monthIncome.Minutes = dto.Minutes;

        var updated = await _monthIncomeRepository.UpdateAsync(monthIncome);
        var result = await _monthIncomeRepository.GetByIdAsync(updated.Id);
        return MapToDto(result!);
    }

    public async Task DeleteAsync(int id)
    {
        await _monthIncomeRepository.DeleteAsync(id);
    }

    private static MonthIncomeDto MapToDto(MonthIncome monthIncome)
    {
        return new MonthIncomeDto(
            monthIncome.Id,
            monthIncome.IncomeId,
            monthIncome.Income.Description,
            monthIncome.Income.Type,
            monthIncome.GrossValue,
            monthIncome.NetValue,
            monthIncome.HourlyRate,
            monthIncome.Hours,
            monthIncome.Minutes
        );
    }
}
