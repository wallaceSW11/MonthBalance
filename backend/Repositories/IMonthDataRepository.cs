using MonthBalance.API.Models;

namespace MonthBalance.API.Repositories;

public interface IMonthDataRepository
{
    Task<List<MonthData>> GetByUserIdAsync(int userId);
    Task<MonthData?> GetByIdAsync(int id);
    Task<MonthData?> GetByYearAndMonthAsync(int userId, int year, int month);
    Task<MonthData> CreateAsync(MonthData monthData);
    Task UpdateAsync(MonthData monthData);
    Task DeleteAsync(int id);
    Task<bool> ExistsAsync(int userId, int year, int month);
}
