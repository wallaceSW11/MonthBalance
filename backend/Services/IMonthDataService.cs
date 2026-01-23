using backend.DTOs;

namespace backend.Services;

public interface IMonthDataService
{
    Task<MonthDataDto> GetOrCreateAsync(int userId, int year, int month);
    Task<List<MonthDataDto>> GetByUserIdAsync(int userId);
    Task<MonthDataDto> DuplicateAsync(int userId, DuplicateMonthDto dto);
    Task DeleteAsync(int userId, int year, int month);
}
