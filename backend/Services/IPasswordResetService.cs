namespace MonthBalance.API.Services;

public interface IPasswordResetService
{
    Task RequestPasswordResetAsync(string email);
    Task<string> ResetPasswordAsync(string token, string newPassword);
}
