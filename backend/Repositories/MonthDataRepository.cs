using Microsoft.EntityFrameworkCore;
using MonthBalance.Data;
using MonthBalance.Models;

namespace MonthBalance.Repositories;

public class MonthDataRepository : IMonthDataRepository
{
    private readonly ApplicationDbContext _context;

    public MonthDataRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<MonthData?> GetByIdAsync(int id)
    {
        return await _context.MonthData
            .Include(m => m.Incomes)
            .Include(m => m.Expenses)
            .FirstOrDefaultAsync(m => m.Id == id);
    }

    public async Task<MonthData?> GetByYearAndMonthAsync(int year, int month)
    {
        return await _context.MonthData
            .Include(m => m.Incomes)
            .Include(m => m.Expenses)
            .FirstOrDefaultAsync(m => m.Year == year && m.Month == month);
    }

    public async Task<IEnumerable<MonthData>> GetAllAsync()
    {
        return await _context.MonthData
            .Include(m => m.Incomes)
            .Include(m => m.Expenses)
            .OrderByDescending(m => m.Year)
            .ThenByDescending(m => m.Month)
            .ToListAsync();
    }

    public async Task<MonthData> CreateAsync(MonthData monthData)
    {
        _context.MonthData.Add(monthData);
        await _context.SaveChangesAsync();

        return monthData;
    }

    public async Task<MonthData> UpdateAsync(MonthData monthData)
    {
        monthData.UpdatedAt = DateTime.UtcNow;
        _context.MonthData.Update(monthData);
        await _context.SaveChangesAsync();

        return monthData;
    }

    public async Task DeleteAsync(int id)
    {
        var monthData = await _context.MonthData.FindAsync(id);

        if (monthData != null)
        {
            _context.MonthData.Remove(monthData);
            await _context.SaveChangesAsync();
        }
    }

    public async Task<bool> ExistsAsync(int year, int month)
    {
        return await _context.MonthData
            .AnyAsync(m => m.Year == year && m.Month == month);
    }
}
