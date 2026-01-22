using MonthBalance.DTOs;
using MonthBalance.Models;
using MonthBalance.Repositories;

namespace MonthBalance.Services;

public class IncomeTypeService : IIncomeTypeService
{
    private readonly IIncomeTypeRepository _repository;

    public IncomeTypeService(IIncomeTypeRepository repository)
    {
        _repository = repository;
    }

    public async Task<List<IncomeTypeDto>> GetAllAsync()
    {
        var incomeTypes = await _repository.GetAllAsync();
        return incomeTypes.Select(MapToDto).ToList();
    }

    public async Task<IncomeTypeDto?> GetByIdAsync(int id)
    {
        var incomeType = await _repository.GetByIdAsync(id);
        return incomeType != null ? MapToDto(incomeType) : null;
    }

    public async Task<IncomeTypeDto> CreateAsync(CreateIncomeTypeDto dto)
    {
        var incomeType = new IncomeType
        {
            Name = dto.Name,
            Type = dto.Type
        };

        var created = await _repository.CreateAsync(incomeType);
        return MapToDto(created);
    }

    public async Task<IncomeTypeDto> UpdateAsync(int id, UpdateIncomeTypeDto dto)
    {
        var incomeType = await _repository.GetByIdAsync(id);
        
        if (incomeType == null)
        {
            throw new InvalidOperationException($"IncomeType with id {id} not found");
        }

        incomeType.Name = dto.Name;
        incomeType.Type = dto.Type;

        await _repository.UpdateAsync(incomeType);
        return MapToDto(incomeType);
    }

    public async Task DeleteAsync(int id)
    {
        await _repository.DeleteAsync(id);
    }

    private static IncomeTypeDto MapToDto(IncomeType incomeType)
    {
        return new IncomeTypeDto(
            incomeType.Id,
            incomeType.Name,
            incomeType.Type
        );
    }
}
