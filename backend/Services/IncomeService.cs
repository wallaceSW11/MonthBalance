using MonthBalance.DTOs;
using MonthBalance.Models;
using MonthBalance.Repositories;

namespace MonthBalance.Services;

public class IncomeService : IIncomeService
{
    private readonly IIncomeRepository _incomeRepository;
    private readonly IMonthDataRepository _monthDataRepository;

    public IncomeService(IIncomeRepository incomeRepository, IMonthDataRepository monthDataRepository)
    {
        _incomeRepository = incomeRepository;
        _monthDataRepository = monthDataRepository;
    }

    public async Task<IncomeDto?> GetByIdAsync(int id)
    {
        var income = await _incomeRepository.GetByIdAsync(id);
        return income == null ? null : MapToDto(income);
    }

    public async Task<IEnumerable<IncomeDto>> GetByMonthAsync(int year, int month)
    {
        var monthData = await _monthDataRepository.GetByYearAndMonthAsync(year, month);
        
        if (monthData == null)
        {
            return Enumerable.Empty<IncomeDto>();
        }

        var incomes = await _incomeRepository.GetByMonthDataIdAsync(monthData.Id);
        return incomes.Select(MapToDto);
    }

    public async Task<IncomeDto> CreateAsync(int year, int month, CreateIncomeDto dto)
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

        var income = new Income
        {
            IncomeTypeId = dto.IncomeTypeId,
            GrossValue = dto.GrossValue,
            NetValue = dto.NetValue,
            HourlyRate = dto.HourlyRate,
            Hours = dto.Hours,
            Minutes = dto.Minutes,
            MonthDataId = monthData.Id,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };

        var created = await _incomeRepository.CreateAsync(income);
        return MapToDto(created);
    }

    public async Task<IncomeDto> UpdateAsync(int id, UpdateIncomeDto dto)
    {
        var income = await _incomeRepository.GetByIdAsync(id);
        
        if (income == null)
        {
            throw new InvalidOperationException($"Income with id {id} not found");
        }

        income.IncomeTypeId = dto.IncomeTypeId;
        income.GrossValue = dto.GrossValue;
        income.NetValue = dto.NetValue;
        income.HourlyRate = dto.HourlyRate;
        income.Hours = dto.Hours;
        income.Minutes = dto.Minutes;

        var updated = await _incomeRepository.UpdateAsync(income);
        return MapToDto(updated);
    }

    public async Task DeleteAsync(int id)
    {
        await _incomeRepository.DeleteAsync(id);
    }

    private static IncomeDto MapToDto(Income income)
    {
        return new IncomeDto(
            income.Id,
            income.IncomeTypeId,
            income.IncomeType.Name,
            income.IncomeType.Type,
            income.GrossValue,
            income.NetValue,
            income.HourlyRate,
            income.Hours,
            income.Minutes,
            income.MonthDataId
        );
    }
}

