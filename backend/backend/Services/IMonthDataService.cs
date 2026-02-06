using MonthBalance.API.DTOs;

namespace MonthBalance.API.Services;

public interface IMonthDataService
{
    Task<List<MonthDataDto>> GetAllByUserAsync(int userId);
    Task<MonthDataDto> GetByYearAndMonthAsync(int userId, int year, int month);
    Task<MonthDataDto> CreateAsync(int userId, CreateMonthDataRequest request);
    Task UpdateLastAccessedAsync(int userId, int id);
    Task DeleteAsync(int userId, int id);
}
