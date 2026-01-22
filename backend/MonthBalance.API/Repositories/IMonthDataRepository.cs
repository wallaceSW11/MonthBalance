using MonthBalance.API.Models;

namespace MonthBalance.API.Repositories;

public interface IMonthDataRepository
{
    Task<MonthData?> GetByIdAsync(int id);
    Task<MonthData?> GetByYearAndMonthAsync(int year, int month);
    Task<IEnumerable<MonthData>> GetAllAsync();
    Task<MonthData> CreateAsync(MonthData monthData);
    Task<MonthData> UpdateAsync(MonthData monthData);
    Task DeleteAsync(int id);
    Task<bool> ExistsAsync(int year, int month);
}
