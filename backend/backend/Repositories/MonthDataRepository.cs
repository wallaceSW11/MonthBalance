using Microsoft.EntityFrameworkCore;
using MonthBalance.API.Data;
using MonthBalance.API.Models;

namespace MonthBalance.API.Repositories;

public class MonthDataRepository : IMonthDataRepository
{
    private readonly ApplicationDbContext _context;
    
    public MonthDataRepository(ApplicationDbContext context)
    {
        _context = context;
    }
    
    public async Task<List<MonthData>> GetByUserIdAsync(int userId)
    {
        return await _context.MonthData
            .Where(m => m.UserId == userId)
            .OrderByDescending(m => m.Year)
            .ThenByDescending(m => m.Month)
            .ToListAsync();
    }
    
    public async Task<MonthData?> GetByIdAsync(int id)
    {
        return await _context.MonthData.FindAsync(id);
    }
    
    public async Task<MonthData?> GetByYearAndMonthAsync(int userId, int year, int month)
    {
        return await _context.MonthData
            .FirstOrDefaultAsync(m => m.UserId == userId && m.Year == year && m.Month == month);
    }
    
    public async Task<MonthData> CreateAsync(MonthData monthData)
    {
        monthData.CreatedAt = DateTime.UtcNow;
        monthData.UpdatedAt = DateTime.UtcNow;
        monthData.LastAccessed = DateTime.UtcNow;
        
        _context.MonthData.Add(monthData);
        await _context.SaveChangesAsync();
        
        return monthData;
    }
    
    public async Task UpdateAsync(MonthData monthData)
    {
        monthData.UpdatedAt = DateTime.UtcNow;
        
        _context.MonthData.Update(monthData);
        await _context.SaveChangesAsync();
    }
    
    public async Task DeleteAsync(int id)
    {
        var monthData = await _context.MonthData.FindAsync(id);
        
        if (monthData == null) return;
        
        _context.MonthData.Remove(monthData);
        await _context.SaveChangesAsync();
    }
    
    public async Task<bool> ExistsAsync(int userId, int year, int month)
    {
        return await _context.MonthData
            .AnyAsync(m => m.UserId == userId && m.Year == year && m.Month == month);
    }
}
