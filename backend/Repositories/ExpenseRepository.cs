using Microsoft.EntityFrameworkCore;
using MonthBalance.API.Data;
using MonthBalance.API.Models;

namespace MonthBalance.API.Repositories;

public class ExpenseRepository : IExpenseRepository
{
    private readonly ApplicationDbContext _context;
    
    public ExpenseRepository(ApplicationDbContext context)
    {
        _context = context;
    }
    
    public async Task<List<Expense>> GetByMonthDataIdAsync(int monthDataId)
    {
        return await _context.Expenses
            .Include(e => e.ExpenseType)
            .Where(e => e.MonthDataId == monthDataId)
            .OrderBy(e => e.CreatedAt)
            .ToListAsync();
    }
    
    public async Task<Expense?> GetByIdAsync(int id)
    {
        return await _context.Expenses
            .Include(e => e.ExpenseType)
            .Include(e => e.MonthData)
            .FirstOrDefaultAsync(e => e.Id == id);
    }
    
    public async Task<Expense> CreateAsync(Expense expense)
    {
        expense.CreatedAt = DateTime.UtcNow;
        expense.UpdatedAt = DateTime.UtcNow;
        
        _context.Expenses.Add(expense);
        await _context.SaveChangesAsync();
        
        await _context.Entry(expense).Reference(e => e.ExpenseType).LoadAsync();
        
        return expense;
    }
    
    public async Task UpdateAsync(Expense expense)
    {
        expense.UpdatedAt = DateTime.UtcNow;
        
        _context.Expenses.Update(expense);
        await _context.SaveChangesAsync();
    }
    
    public async Task DeleteAsync(int id)
    {
        var expense = await _context.Expenses.FindAsync(id);
        
        if (expense == null) return;
        
        _context.Expenses.Remove(expense);
        await _context.SaveChangesAsync();
    }
}
