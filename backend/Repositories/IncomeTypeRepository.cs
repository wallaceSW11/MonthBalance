using Microsoft.EntityFrameworkCore;
using MonthBalance.Data;
using MonthBalance.Models;

namespace MonthBalance.Repositories;

public class IncomeTypeRepository : IIncomeTypeRepository
{
    private readonly ApplicationDbContext _context;

    public IncomeTypeRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<List<IncomeType>> GetAllAsync()
    {
        return await _context.IncomeTypes
            .OrderBy(it => it.Name)
            .ToListAsync();
    }

    public async Task<IncomeType?> GetByIdAsync(int id)
    {
        return await _context.IncomeTypes.FindAsync(id);
    }

    public async Task<IncomeType> CreateAsync(IncomeType incomeType)
    {
        _context.IncomeTypes.Add(incomeType);
        await _context.SaveChangesAsync();
        return incomeType;
    }

    public async Task UpdateAsync(IncomeType incomeType)
    {
        incomeType.UpdatedAt = DateTime.UtcNow;
        _context.IncomeTypes.Update(incomeType);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(int id)
    {
        var incomeType = await GetByIdAsync(id);
        
        if (incomeType == null) return;
        
        _context.IncomeTypes.Remove(incomeType);
        await _context.SaveChangesAsync();
    }
}
