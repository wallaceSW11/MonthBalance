using Microsoft.EntityFrameworkCore;
using backend.Data;
using backend.Models;

namespace backend.Repositories;

public class MonthExpenseRepository : IMonthExpenseRepository
{
    private readonly ApplicationDbContext _context;

    public MonthExpenseRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<List<MonthExpense>> GetByMonthDataIdAsync(int monthDataId)
    {
        return await _context.MonthExpenses
            .Include(me => me.Expense)
            .Where(me => me.MonthDataId == monthDataId)
            .ToListAsync();
    }

    public async Task<MonthExpense?> GetByIdAsync(int id)
    {
        return await _context.MonthExpenses
            .Include(me => me.Expense)
            .FirstOrDefaultAsync(me => me.Id == id);
    }

    public async Task<bool> ExistsAsync(int monthDataId, int expenseId)
    {
        return await _context.MonthExpenses
            .AnyAsync(me => me.MonthDataId == monthDataId && me.ExpenseId == expenseId);
    }

    public async Task<MonthExpense> CreateAsync(MonthExpense monthExpense)
    {
        _context.MonthExpenses.Add(monthExpense);
        await _context.SaveChangesAsync();
        return monthExpense;
    }

    public async Task<MonthExpense> UpdateAsync(MonthExpense monthExpense)
    {
        monthExpense.UpdatedAt = DateTime.UtcNow;
        _context.MonthExpenses.Update(monthExpense);
        await _context.SaveChangesAsync();
        return monthExpense;
    }

    public async Task DeleteAsync(int id)
    {
        var monthExpense = await _context.MonthExpenses.FindAsync(id);
        
        if (monthExpense == null) return;
        
        _context.MonthExpenses.Remove(monthExpense);
        await _context.SaveChangesAsync();
    }
}
