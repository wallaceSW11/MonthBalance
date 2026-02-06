using Microsoft.EntityFrameworkCore;
using MonthBalance.API.Data;
using MonthBalance.API.Models;

namespace MonthBalance.API.Repositories;

public class IncomeTypeRepository : IIncomeTypeRepository
{
    private readonly ApplicationDbContext _context;
    
    public IncomeTypeRepository(ApplicationDbContext context)
    {
        _context = context;
    }
    
    public async Task<List<IncomeTypeModel>> GetByUserIdAsync(int userId)
    {
        return await _context.IncomeTypes
            .Where(t => t.UserId == userId)
            .OrderBy(t => t.Name)
            .ToListAsync();
    }
    
    public async Task<IncomeTypeModel?> GetByIdAsync(int id)
    {
        return await _context.IncomeTypes.FindAsync(id);
    }
    
    public async Task<IncomeTypeModel> CreateAsync(IncomeTypeModel incomeType)
    {
        incomeType.CreatedAt = DateTime.UtcNow;
        incomeType.UpdatedAt = DateTime.UtcNow;
        
        _context.IncomeTypes.Add(incomeType);
        await _context.SaveChangesAsync();
        
        return incomeType;
    }
    
    public async Task UpdateAsync(IncomeTypeModel incomeType)
    {
        incomeType.UpdatedAt = DateTime.UtcNow;
        
        _context.IncomeTypes.Update(incomeType);
        await _context.SaveChangesAsync();
    }
    
    public async Task DeleteAsync(int id)
    {
        var incomeType = await _context.IncomeTypes.FindAsync(id);
        
        if (incomeType == null) return;
        
        _context.IncomeTypes.Remove(incomeType);
        await _context.SaveChangesAsync();
    }
    
    public async Task<bool> HasIncomesAsync(int incomeTypeId)
    {
        return await _context.Incomes.AnyAsync(i => i.IncomeTypeId == incomeTypeId);
    }
}
