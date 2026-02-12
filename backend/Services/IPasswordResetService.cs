namespace MonthBalance.API.Services;

public interface IPasswordResetService
{
    Task RequestPasswordResetAsync(string email);
    Task ResetPasswordAsync(string token, string newPassword);
}
