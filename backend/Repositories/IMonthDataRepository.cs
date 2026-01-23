using backend.Models;

namespace backend.Repositories;

public interface IMonthDataRepository
{
    Task<MonthData?> GetByYearAndMonthAsync(int userId, int year, int month);
    Task<MonthData?> GetByIdAsync(int id);
    Task<List<MonthData>> GetByUserIdAsync(int userId);
    Task<MonthData> CreateAsync(MonthData monthData);
    Task DeleteAsync(int id);
}
