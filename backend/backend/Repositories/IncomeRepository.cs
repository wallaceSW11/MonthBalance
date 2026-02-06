using Microsoft.EntityFrameworkCore;
using MonthBalance.API.Data;
using MonthBalance.API.Models;

namespace MonthBalance.API.Repositories;

public class IncomeRepository : IIncomeRepository
{
    private readonly ApplicationDbContext _context;
    
    public IncomeRepository(ApplicationDbContext context)
    {
        _context = context;
    }
    
    public async Task<List<Income>> GetByMonthDataIdAsync(int monthDataId)
    {
        return await _context.Incomes
            .Include(i => i.IncomeType)
            .Where(i => i.MonthDataId == monthDataId)
            .OrderBy(i => i.CreatedAt)
            .ToListAsync();
    }
    
    public async Task<Income?> GetByIdAsync(int id)
    {
        return await _context.Incomes
            .Include(i => i.IncomeType)
            .Include(i => i.MonthData)
            .FirstOrDefaultAsync(i => i.Id == id);
    }
    
    public async Task<Income> CreateAsync(Income income)
    {
        income.CreatedAt = DateTime.UtcNow;
        income.UpdatedAt = DateTime.UtcNow;
        
        _context.Incomes.Add(income);
        await _context.SaveChangesAsync();
        
        await _context.Entry(income).Reference(i => i.IncomeType).LoadAsync();
        
        return income;
    }
    
    public async Task UpdateAsync(Income income)
    {
        income.UpdatedAt = DateTime.UtcNow;
        
        _context.Incomes.Update(income);
        await _context.SaveChangesAsync();
    }
    
    public async Task DeleteAsync(int id)
    {
        var income = await _context.Incomes.FindAsync(id);
        
        if (income == null) return;
        
        _context.Incomes.Remove(income);
        await _context.SaveChangesAsync();
    }
}
