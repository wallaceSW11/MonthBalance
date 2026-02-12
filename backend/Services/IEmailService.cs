namespace MonthBalance.API.Services;

public interface IEmailService
{
    Task SendPasswordResetEmailAsync(string toEmail, string toName, string resetToken);
    Task SendFeedbackToAdminAsync(string userName, string userEmail, string subject, string message, int? rating);
    Task SendFeedbackConfirmationAsync(string toEmail, string toName);
}
