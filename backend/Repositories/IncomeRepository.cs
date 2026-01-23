using Microsoft.EntityFrameworkCore;
using backend.Data;
using backend.Models;

namespace backend.Repositories;

public class IncomeRepository : IIncomeRepository
{
    private readonly ApplicationDbContext _context;

    public IncomeRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<List<Income>> GetByUserIdAsync(int userId)
    {
        return await _context.Incomes
            .Where(i => i.UserId == userId)
            .OrderBy(i => i.Description)
            .ToListAsync();
    }

    public async Task<Income?> GetByIdAsync(int id)
    {
        return await _context.Incomes.FindAsync(id);
    }

    public async Task<Income> CreateAsync(Income income)
    {
        _context.Incomes.Add(income);
        await _context.SaveChangesAsync();
        return income;
    }

    public async Task<Income> UpdateAsync(Income income)
    {
        income.UpdatedAt = DateTime.UtcNow;
        _context.Incomes.Update(income);
        await _context.SaveChangesAsync();
        return income;
    }

    public async Task DeleteAsync(int id)
    {
        var income = await GetByIdAsync(id);
        
        if (income == null) return;
        
        _context.Incomes.Remove(income);
        await _context.SaveChangesAsync();
    }

    public async Task<bool> HasMonthIncomesAsync(int incomeId)
    {
        return await _context.MonthIncomes
            .AnyAsync(mi => mi.IncomeId == incomeId);
    }
}
