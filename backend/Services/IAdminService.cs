using MonthBalance.API.DTOs;

namespace MonthBalance.API.Services;

public interface IAdminService
{
    Task<AdminDashboardDto> GetDashboardAsync();
    Task<UserListResponseDto> GetUsersAsync(string? search = null, int page = 1, int pageSize = 20);
    Task<UserSummaryDto?> GetUserSummaryAsync(int userId);
}
