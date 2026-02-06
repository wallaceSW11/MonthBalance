using MonthBalance.API.DTOs;
using MonthBalance.API.Models;
using MonthBalance.API.Repositories;

namespace MonthBalance.API.Services;

public class IncomeTypeService : IIncomeTypeService
{
    private readonly IIncomeTypeRepository _incomeTypeRepository;
    
    public IncomeTypeService(IIncomeTypeRepository incomeTypeRepository)
    {
        _incomeTypeRepository = incomeTypeRepository;
    }
    
    public async Task<List<IncomeTypeDto>> GetAllByUserAsync(int userId)
    {
        var incomeTypes = await _incomeTypeRepository.GetByUserIdAsync(userId);
        
        return incomeTypes.Select(MapToDto).ToList();
    }
    
    public async Task<IncomeTypeDto> GetByIdAsync(int userId, int id)
    {
        var incomeType = await _incomeTypeRepository.GetByIdAsync(id);
        
        if (incomeType == null)
            throw new InvalidOperationException("Income type not found");
        
        if (incomeType.UserId != userId)
            throw new UnauthorizedAccessException("Access denied");
        
        return MapToDto(incomeType);
    }
    
    public async Task<IncomeTypeDto> CreateAsync(int userId, CreateIncomeTypeRequest request)
    {
        if (!IsValidIncomeType(request.Type))
            throw new ArgumentException("Invalid income type. Must be 'paycheck', 'hourly', or 'extra'");
        
        var incomeType = new IncomeTypeModel
        {
            UserId = userId,
            Name = request.Name,
            Type = ParseIncomeType(request.Type)
        };
        
        var created = await _incomeTypeRepository.CreateAsync(incomeType);
        
        return MapToDto(created);
    }
    
    public async Task<IncomeTypeDto> UpdateAsync(int userId, int id, UpdateIncomeTypeRequest request)
    {
        var incomeType = await _incomeTypeRepository.GetByIdAsync(id);
        
        if (incomeType == null)
            throw new InvalidOperationException("Income type not found");
        
        if (incomeType.UserId != userId)
            throw new UnauthorizedAccessException("Access denied");
        
        incomeType.Name = request.Name;
        
        await _incomeTypeRepository.UpdateAsync(incomeType);
        
        return MapToDto(incomeType);
    }
    
    public async Task DeleteAsync(int userId, int id)
    {
        var incomeType = await _incomeTypeRepository.GetByIdAsync(id);
        
        if (incomeType == null)
            throw new InvalidOperationException("Income type not found");
        
        if (incomeType.UserId != userId)
            throw new UnauthorizedAccessException("Access denied");
        
        if (await _incomeTypeRepository.HasIncomesAsync(id))
            throw new InvalidOperationException("Cannot delete income type with associated incomes");
        
        await _incomeTypeRepository.DeleteAsync(id);
    }
    
    private static bool IsValidIncomeType(string type)
    {
        var validTypes = new[] { "paycheck", "hourly", "extra" };
        return validTypes.Contains(type.ToLower());
    }
    
    private static IncomeType ParseIncomeType(string type)
    {
        return type.ToLower() switch
        {
            "paycheck" => IncomeType.Paycheck,
            "hourly" => IncomeType.Hourly,
            "extra" => IncomeType.Extra,
            _ => throw new ArgumentException("Invalid income type")
        };
    }
    
    private static IncomeTypeDto MapToDto(IncomeTypeModel incomeType)
    {
        return new IncomeTypeDto(
            incomeType.Id,
            incomeType.Name,
            incomeType.Type.ToString().ToLower()
        );
    }
}
