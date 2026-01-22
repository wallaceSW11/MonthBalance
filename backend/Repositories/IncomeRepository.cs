using Microsoft.EntityFrameworkCore;
using MonthBalance.Data;
using MonthBalance.Models;

namespace MonthBalance.Repositories;

public class IncomeRepository : IIncomeRepository
{
    private readonly ApplicationDbContext _context;

    public IncomeRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Income?> GetByIdAsync(int id)
    {
        return await _context.Incomes
            .Include(i => i.IncomeType)
            .FirstOrDefaultAsync(i => i.Id == id);
    }

    public async Task<IEnumerable<Income>> GetByMonthDataIdAsync(int monthDataId)
    {
        return await _context.Incomes
            .Include(i => i.IncomeType)
            .Where(i => i.MonthDataId == monthDataId)
            .ToListAsync();
    }

    public async Task<Income> CreateAsync(Income income)
    {
        _context.Incomes.Add(income);
        await _context.SaveChangesAsync();

        return await GetByIdAsync(income.Id) ?? income;
    }

    public async Task<Income> UpdateAsync(Income income)
    {
        income.UpdatedAt = DateTime.UtcNow;
        _context.Incomes.Update(income);
        await _context.SaveChangesAsync();

        return await GetByIdAsync(income.Id) ?? income;
    }

    public async Task DeleteAsync(int id)
    {
        var income = await _context.Incomes.FindAsync(id);

        if (income != null)
        {
            _context.Incomes.Remove(income);
            await _context.SaveChangesAsync();
        }
    }
}
