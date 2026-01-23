using backend.DTOs;
using backend.Models;
using backend.Repositories;

namespace backend.Services;

public class IncomeService : IIncomeService
{
    private readonly IIncomeRepository _repository;

    public IncomeService(IIncomeRepository repository)
    {
        _repository = repository;
    }

    public async Task<List<IncomeDto>> GetByUserIdAsync(int userId)
    {
        var incomes = await _repository.GetByUserIdAsync(userId);
        return incomes.Select(MapToDto).ToList();
    }

    public async Task<IncomeDto?> GetByIdAsync(int id)
    {
        var income = await _repository.GetByIdAsync(id);
        return income != null ? MapToDto(income) : null;
    }

    public async Task<IncomeDto> CreateAsync(int userId, CreateIncomeDto dto)
    {
        var income = new Income
        {
            Description = dto.Description,
            Type = dto.Type,
            UserId = userId
        };

        var created = await _repository.CreateAsync(income);
        return MapToDto(created);
    }

    public async Task<IncomeDto> UpdateAsync(int id, UpdateIncomeDto dto)
    {
        var income = await _repository.GetByIdAsync(id);
        
        if (income == null)
            throw new InvalidOperationException("Receita não encontrada");

        income.Description = dto.Description;
        income.Type = dto.Type;

        var updated = await _repository.UpdateAsync(income);
        return MapToDto(updated);
    }

    public async Task DeleteAsync(int id)
    {
        var hasMonthIncomes = await _repository.HasMonthIncomesAsync(id);
        
        if (hasMonthIncomes)
            throw new InvalidOperationException("Não é possível deletar receita vinculada a meses");

        await _repository.DeleteAsync(id);
    }

    private static IncomeDto MapToDto(Income income)
    {
        return new IncomeDto(
            income.Id,
            income.Description,
            income.Type
        );
    }
}
