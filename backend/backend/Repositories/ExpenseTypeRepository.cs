using Microsoft.EntityFrameworkCore;
using MonthBalance.API.Data;
using MonthBalance.API.Models;

namespace MonthBalance.API.Repositories;

public class ExpenseTypeRepository : IExpenseTypeRepository
{
    private readonly ApplicationDbContext _context;
    
    public ExpenseTypeRepository(ApplicationDbContext context)
    {
        _context = context;
    }
    
    public async Task<List<ExpenseTypeModel>> GetByUserIdAsync(int userId)
    {
        return await _context.ExpenseTypes
            .Where(t => t.UserId == userId)
            .OrderBy(t => t.Name)
            .ToListAsync();
    }
    
    public async Task<ExpenseTypeModel?> GetByIdAsync(int id)
    {
        return await _context.ExpenseTypes.FindAsync(id);
    }
    
    public async Task<ExpenseTypeModel> CreateAsync(ExpenseTypeModel expenseType)
    {
        expenseType.CreatedAt = DateTime.UtcNow;
        expenseType.UpdatedAt = DateTime.UtcNow;
        
        _context.ExpenseTypes.Add(expenseType);
        await _context.SaveChangesAsync();
        
        return expenseType;
    }
    
    public async Task UpdateAsync(ExpenseTypeModel expenseType)
    {
        expenseType.UpdatedAt = DateTime.UtcNow;
        
        _context.ExpenseTypes.Update(expenseType);
        await _context.SaveChangesAsync();
    }
    
    public async Task DeleteAsync(int id)
    {
        var expenseType = await _context.ExpenseTypes.FindAsync(id);
        
        if (expenseType == null) return;
        
        _context.ExpenseTypes.Remove(expenseType);
        await _context.SaveChangesAsync();
    }
    
    public async Task<bool> HasExpensesAsync(int expenseTypeId)
    {
        return await _context.Expenses.AnyAsync(e => e.ExpenseTypeId == expenseTypeId);
    }
}
