using Microsoft.EntityFrameworkCore;
using backend.Data;
using backend.Models;

namespace backend.Repositories;

public class MonthDataRepository : IMonthDataRepository
{
    private readonly ApplicationDbContext _context;

    public MonthDataRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<MonthData?> GetByYearAndMonthAsync(int userId, int year, int month)
    {
        return await _context.MonthData
            .Include(md => md.MonthIncomes)
                .ThenInclude(mi => mi.Income)
            .Include(md => md.MonthExpenses)
                .ThenInclude(me => me.Expense)
            .FirstOrDefaultAsync(md => md.UserId == userId && md.Year == year && md.Month == month);
    }

    public async Task<MonthData?> GetByIdAsync(int id)
    {
        return await _context.MonthData
            .Include(md => md.MonthIncomes)
                .ThenInclude(mi => mi.Income)
            .Include(md => md.MonthExpenses)
                .ThenInclude(me => me.Expense)
            .FirstOrDefaultAsync(md => md.Id == id);
    }

    public async Task<List<MonthData>> GetByUserIdAsync(int userId)
    {
        return await _context.MonthData
            .Where(md => md.UserId == userId)
            .OrderByDescending(md => md.Year)
            .ThenByDescending(md => md.Month)
            .ToListAsync();
    }

    public async Task<MonthData> CreateAsync(MonthData monthData)
    {
        _context.MonthData.Add(monthData);
        await _context.SaveChangesAsync();
        return monthData;
    }

    public async Task DeleteAsync(int id)
    {
        var monthData = await _context.MonthData.FindAsync(id);
        
        if (monthData == null) return;
        
        _context.MonthData.Remove(monthData);
        await _context.SaveChangesAsync();
    }
}
