using MonthBalance.API.DTOs;
using MonthBalance.API.Models;
using MonthBalance.API.Repositories;

namespace MonthBalance.API.Services;

public class MonthDataService : IMonthDataService
{
    private readonly IMonthDataRepository _monthDataRepository;
    
    public MonthDataService(IMonthDataRepository monthDataRepository)
    {
        _monthDataRepository = monthDataRepository;
    }
    
    public async Task<List<MonthDataDto>> GetAllByUserAsync(int userId)
    {
        var monthDataList = await _monthDataRepository.GetByUserIdAsync(userId);
        
        return monthDataList.Select(MapToDto).ToList();
    }
    
    public async Task<MonthDataDto> GetByYearAndMonthAsync(int userId, int year, int month)
    {
        if (year < 2000 || year > 2100)
            throw new ArgumentException("Year must be between 2000 and 2100");
        
        if (month < 1 || month > 12)
            throw new ArgumentException("Month must be between 1 and 12");
        
        var monthData = await _monthDataRepository.GetByYearAndMonthAsync(userId, year, month);
        
        if (monthData == null)
            throw new InvalidOperationException("Month data not found");
        
        return MapToDto(monthData);
    }
    
    public async Task<MonthDataDto> CreateAsync(int userId, CreateMonthDataRequest request)
    {
        if (request.Year < 2000 || request.Year > 2100)
            throw new ArgumentException("Year must be between 2000 and 2100");
        
        if (request.Month < 1 || request.Month > 12)
            throw new ArgumentException("Month must be between 1 and 12");
        
        if (await _monthDataRepository.ExistsAsync(userId, request.Year, request.Month))
            throw new InvalidOperationException("Month data already exists for this period");
        
        var monthData = new MonthData
        {
            UserId = userId,
            Year = request.Year,
            Month = request.Month
        };
        
        var created = await _monthDataRepository.CreateAsync(monthData);
        
        return MapToDto(created);
    }
    
    public async Task UpdateLastAccessedAsync(int userId, int id)
    {
        var monthData = await _monthDataRepository.GetByIdAsync(id);
        
        if (monthData == null)
            throw new InvalidOperationException("Month data not found");
        
        if (monthData.UserId != userId)
            throw new UnauthorizedAccessException("Access denied");
        
        monthData.LastAccessed = DateTime.UtcNow;
        
        await _monthDataRepository.UpdateAsync(monthData);
    }
    
    public async Task DeleteAsync(int userId, int id)
    {
        var monthData = await _monthDataRepository.GetByIdAsync(id);
        
        if (monthData == null)
            throw new InvalidOperationException("Month data not found");
        
        if (monthData.UserId != userId)
            throw new UnauthorizedAccessException("Access denied");
        
        await _monthDataRepository.DeleteAsync(id);
    }
    
    private static MonthDataDto MapToDto(MonthData monthData)
    {
        return new MonthDataDto(
            monthData.Id,
            monthData.Year,
            monthData.Month,
            monthData.LastAccessed
        );
    }
}
