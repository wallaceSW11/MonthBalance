using MonthBalance.API.DTOs;
using MonthBalance.API.Models;
using MonthBalance.API.Repositories;

namespace MonthBalance.API.Services;

public class IncomeService : IIncomeService
{
    private readonly IIncomeRepository _incomeRepository;
    private readonly IMonthDataRepository _monthDataRepository;
    private readonly IIncomeTypeRepository _incomeTypeRepository;
    
    public IncomeService(
        IIncomeRepository incomeRepository,
        IMonthDataRepository monthDataRepository,
        IIncomeTypeRepository incomeTypeRepository)
    {
        _incomeRepository = incomeRepository;
        _monthDataRepository = monthDataRepository;
        _incomeTypeRepository = incomeTypeRepository;
    }
    
    public async Task<List<IncomeDto>> GetByMonthDataIdAsync(int userId, int monthDataId)
    {
        var monthData = await _monthDataRepository.GetByIdAsync(monthDataId);
        
        if (monthData == null)
            throw new InvalidOperationException("Month data not found");
        
        if (monthData.UserId != userId)
            throw new UnauthorizedAccessException("Access denied");
        
        var incomes = await _incomeRepository.GetByMonthDataIdAsync(monthDataId);
        
        return incomes.Select(MapToDto).ToList();
    }
    
    public async Task<IncomeDto> GetByIdAsync(int userId, int id)
    {
        var income = await _incomeRepository.GetByIdAsync(id);
        
        if (income == null)
            throw new InvalidOperationException("Income not found");
        
        if (income.MonthData.UserId != userId)
            throw new UnauthorizedAccessException("Access denied");
        
        return MapToDto(income);
    }
    
    public async Task<IncomeDto> CreateAsync(int userId, CreateIncomeRequest request)
    {
        var monthData = await _monthDataRepository.GetByIdAsync(request.MonthDataId);
        
        if (monthData == null)
            throw new InvalidOperationException("Month data not found");
        
        if (monthData.UserId != userId)
            throw new UnauthorizedAccessException("Access denied");
        
        var incomeType = await _incomeTypeRepository.GetByIdAsync(request.IncomeTypeId);
        
        if (incomeType == null)
            throw new InvalidOperationException("Income type not found");
        
        if (incomeType.UserId != userId)
            throw new UnauthorizedAccessException("Access denied");
        
        var income = new Income
        {
            MonthDataId = request.MonthDataId,
            IncomeTypeId = request.IncomeTypeId,
            GrossValue = request.GrossValue,
            NetValue = request.NetValue,
            HourlyRate = request.HourlyRate,
            Hours = request.Hours,
            Minutes = request.Minutes,
            CalculatedValue = CalculateValue(incomeType.Type, request)
        };
        
        var created = await _incomeRepository.CreateAsync(income);
        
        return MapToDto(created);
    }
    
    public async Task<IncomeDto> UpdateAsync(int userId, int id, UpdateIncomeRequest request)
    {
        var income = await _incomeRepository.GetByIdAsync(id);
        
        if (income == null)
            throw new InvalidOperationException("Income not found");
        
        if (income.MonthData.UserId != userId)
            throw new UnauthorizedAccessException("Access denied");
        
        income.GrossValue = request.GrossValue;
        income.NetValue = request.NetValue;
        income.HourlyRate = request.HourlyRate;
        income.Hours = request.Hours;
        income.Minutes = request.Minutes;
        income.CalculatedValue = CalculateValue(income.IncomeType.Type, request);
        
        await _incomeRepository.UpdateAsync(income);
        
        return MapToDto(income);
    }
    
    public async Task DeleteAsync(int userId, int id)
    {
        var income = await _incomeRepository.GetByIdAsync(id);
        
        if (income == null)
            throw new InvalidOperationException("Income not found");
        
        if (income.MonthData.UserId != userId)
            throw new UnauthorizedAccessException("Access denied");
        
        await _incomeRepository.DeleteAsync(id);
    }
    
    private static decimal CalculateValue(IncomeType type, CreateIncomeRequest request)
    {
        return type switch
        {
            IncomeType.Paycheck => request.NetValue ?? request.GrossValue ?? 0,
            IncomeType.Hourly => CalculateHourlyValue(request.HourlyRate, request.Hours, request.Minutes),
            IncomeType.Extra => request.NetValue ?? request.GrossValue ?? 0,
            _ => 0
        };
    }
    
    private static decimal CalculateValue(IncomeType type, UpdateIncomeRequest request)
    {
        return type switch
        {
            IncomeType.Paycheck => request.NetValue ?? request.GrossValue ?? 0,
            IncomeType.Hourly => CalculateHourlyValue(request.HourlyRate, request.Hours, request.Minutes),
            IncomeType.Extra => request.NetValue ?? request.GrossValue ?? 0,
            _ => 0
        };
    }
    
    private static decimal CalculateHourlyValue(decimal? hourlyRate, int? hours, int? minutes)
    {
        if (hourlyRate == null || hours == null) return 0;
        
        var totalHours = hours.Value + (minutes ?? 0) / 60m;
        return hourlyRate.Value * totalHours;
    }
    
    private static IncomeDto MapToDto(Income income)
    {
        return new IncomeDto(
            income.Id,
            income.MonthDataId,
            income.IncomeTypeId,
            income.GrossValue,
            income.NetValue,
            income.HourlyRate,
            income.Hours,
            income.Minutes,
            income.CalculatedValue
        );
    }
}
