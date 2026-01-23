using Microsoft.EntityFrameworkCore;
using backend.Data;
using backend.Models;

namespace backend.Repositories;

public class MonthIncomeRepository : IMonthIncomeRepository
{
    private readonly ApplicationDbContext _context;

    public MonthIncomeRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<List<MonthIncome>> GetByMonthDataIdAsync(int monthDataId)
    {
        return await _context.MonthIncomes
            .Include(mi => mi.Income)
            .Where(mi => mi.MonthDataId == monthDataId)
            .ToListAsync();
    }

    public async Task<MonthIncome?> GetByIdAsync(int id)
    {
        return await _context.MonthIncomes
            .Include(mi => mi.Income)
            .FirstOrDefaultAsync(mi => mi.Id == id);
    }

    public async Task<bool> ExistsAsync(int monthDataId, int incomeId)
    {
        return await _context.MonthIncomes
            .AnyAsync(mi => mi.MonthDataId == monthDataId && mi.IncomeId == incomeId);
    }

    public async Task<MonthIncome> CreateAsync(MonthIncome monthIncome)
    {
        _context.MonthIncomes.Add(monthIncome);
        await _context.SaveChangesAsync();
        return monthIncome;
    }

    public async Task<MonthIncome> UpdateAsync(MonthIncome monthIncome)
    {
        monthIncome.UpdatedAt = DateTime.UtcNow;
        _context.MonthIncomes.Update(monthIncome);
        await _context.SaveChangesAsync();
        return monthIncome;
    }

    public async Task DeleteAsync(int id)
    {
        var monthIncome = await _context.MonthIncomes.FindAsync(id);
        
        if (monthIncome == null) return;
        
        _context.MonthIncomes.Remove(monthIncome);
        await _context.SaveChangesAsync();
    }
}
