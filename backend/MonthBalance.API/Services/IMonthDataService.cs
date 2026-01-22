using MonthBalance.API.DTOs;

namespace MonthBalance.API.Services;

public interface IMonthDataService
{
    Task<MonthDataDto?> GetByIdAsync(int id);
    Task<MonthDataDto?> GetByYearAndMonthAsync(int year, int month);
    Task<IEnumerable<MonthDataDto>> GetAllAsync();
    Task<MonthDataDto> CreateAsync(CreateMonthDataDto dto);
    Task DeleteAsync(int id);
    Task<MonthDataDto> DuplicateMonthAsync(int sourceYear, int sourceMonth, int targetYear, int targetMonth);
}
