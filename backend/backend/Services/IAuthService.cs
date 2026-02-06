using MonthBalance.API.DTOs;

namespace MonthBalance.API.Services;

public interface IAuthService
{
    Task<LoginResponse> RegisterAsync(RegisterRequest request);
    Task<LoginResponse> LoginAsync(LoginRequest request);
    Task<UserDto> GetCurrentUserAsync(int userId);
    Task<UserDto> UpdateUserAsync(int userId, UpdateUserRequest request);
    Task ChangePasswordAsync(int userId, ChangePasswordRequest request);
}
