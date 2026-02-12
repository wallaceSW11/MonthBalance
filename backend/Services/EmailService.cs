using MailKit.Net.Smtp;
using MailKit.Security;
using MimeKit;

namespace MonthBalance.API.Services;

public class EmailService : IEmailService
{
    private readonly IConfiguration _configuration;
    private readonly ILogger<EmailService> _logger;
    
    public EmailService(IConfiguration configuration, ILogger<EmailService> logger)
    {
        _configuration = configuration;
        _logger = logger;
    }
    
    public async Task SendPasswordResetEmailAsync(string toEmail, string toName, string resetToken)
    {
        var frontendUrl = _configuration["Frontend:Url"] ?? "http://localhost:5173";
        var resetLink = $"{frontendUrl}/reset-password?token={resetToken}";
        
        var subject = "MonthBalance - Recuperação de Senha";
        var body = $@"
<!DOCTYPE html>
<html>
<head>
    <style>
        body {{ font-family: Arial, sans-serif; line-height: 1.6; color: #333; }}
        .container {{ max-width: 600px; margin: 0 auto; padding: 20px; }}
        .header {{ background-color: #4F46E5; color: white; padding: 20px; text-align: center; border-radius: 8px 8px 0 0; }}
        .content {{ background-color: #f9fafb; padding: 30px; border-radius: 0 0 8px 8px; }}
        .button {{ display: inline-block; background-color: #4F46E5; color: white; padding: 12px 30px; text-decoration: none; border-radius: 6px; margin: 20px 0; }}
        .footer {{ text-align: center; margin-top: 20px; font-size: 12px; color: #666; }}
    </style>
</head>
<body>
    <div class='container'>
        <div class='header'>
            <h1>MonthBalance</h1>
        </div>
        <div class='content'>
            <h2>Olá, {toName}!</h2>
            <p>Recebemos uma solicitação para redefinir sua senha.</p>
            <p>Clique no botão abaixo para criar uma nova senha:</p>
            <a href='{resetLink}' class='button'>Redefinir Senha</a>
            <p>Ou copie e cole este link no seu navegador:</p>
            <p style='word-break: break-all; color: #4F46E5;'>{resetLink}</p>
            <p><strong>Este link expira em 1 hora.</strong></p>
            <p>Se você não solicitou a redefinição de senha, ignore este email.</p>
        </div>
        <div class='footer'>
            <p>MonthBalance - Controle Financeiro Pessoal</p>
        </div>
    </div>
</body>
</html>";
        
        await SendEmailAsync(toEmail, toName, subject, body);
    }
    
    public async Task SendFeedbackToAdminAsync(string userName, string userEmail, string subject, string message, int? rating)
    {
        var adminEmail = _configuration["Email:AdminEmail"] ?? throw new InvalidOperationException("Admin email not configured");
        var adminName = "Admin";
        
        var ratingText = rating.HasValue ? $"⭐ {rating.Value}/5" : "Sem avaliação";
        
        var emailSubject = $"Novo Feedback - MonthBalance: {subject}";
        var body = $@"
<!DOCTYPE html>
<html>
<head>
    <style>
        body {{ font-family: Arial, sans-serif; line-height: 1.6; color: #333; }}
        .container {{ max-width: 600px; margin: 0 auto; padding: 20px; }}
        .header {{ background-color: #059669; color: white; padding: 20px; text-align: center; border-radius: 8px 8px 0 0; }}
        .content {{ background-color: #f9fafb; padding: 30px; border-radius: 0 0 8px 8px; }}
        .info {{ background-color: white; padding: 15px; border-left: 4px solid #059669; margin: 15px 0; }}
        .message {{ background-color: white; padding: 20px; border-radius: 6px; margin: 15px 0; }}
    </style>
</head>
<body>
    <div class='container'>
        <div class='header'>
            <h1>📬 Novo Feedback Recebido</h1>
        </div>
        <div class='content'>
            <div class='info'>
                <p><strong>Usuário:</strong> {userName}</p>
                <p><strong>Email:</strong> {userEmail}</p>
                <p><strong>Avaliação:</strong> {ratingText}</p>
                <p><strong>Data:</strong> {DateTime.Now:dd/MM/yyyy HH:mm}</p>
            </div>
            <h3>Assunto:</h3>
            <p>{subject}</p>
            <h3>Mensagem:</h3>
            <div class='message'>
                <p>{message.Replace("\n", "<br>")}</p>
            </div>
        </div>
    </div>
</body>
</html>";
        
        await SendEmailAsync(adminEmail, adminName, emailSubject, body);
    }
    
    public async Task SendFeedbackConfirmationAsync(string toEmail, string toName)
    {
        var subject = "MonthBalance - Feedback Recebido";
        var body = $@"
<!DOCTYPE html>
<html>
<head>
    <style>
        body {{ font-family: Arial, sans-serif; line-height: 1.6; color: #333; }}
        .container {{ max-width: 600px; margin: 0 auto; padding: 20px; }}
        .header {{ background-color: #059669; color: white; padding: 20px; text-align: center; border-radius: 8px 8px 0 0; }}
        .content {{ background-color: #f9fafb; padding: 30px; border-radius: 0 0 8px 8px; }}
        .footer {{ text-align: center; margin-top: 20px; font-size: 12px; color: #666; }}
    </style>
</head>
<body>
    <div class='container'>
        <div class='header'>
            <h1>✅ Feedback Recebido</h1>
        </div>
        <div class='content'>
            <h2>Olá, {toName}!</h2>
            <p>Obrigado por compartilhar seu feedback conosco!</p>
            <p>Sua opinião é muito importante para melhorarmos o MonthBalance.</p>
            <p>Vamos analisar sua mensagem e, se necessário, entraremos em contato.</p>
            <p>Continue aproveitando o MonthBalance! 🚀</p>
        </div>
        <div class='footer'>
            <p>MonthBalance - Controle Financeiro Pessoal</p>
        </div>
    </div>
</body>
</html>";
        
        await SendEmailAsync(toEmail, toName, subject, body);
    }
    
    private async Task SendEmailAsync(string toEmail, string toName, string subject, string htmlBody)
    {
        try
        {
            var smtpHost = _configuration["Email:SmtpHost"] ?? throw new InvalidOperationException("SMTP host not configured");
            var smtpPort = int.Parse(_configuration["Email:SmtpPort"] ?? "587");
            var useSsl = bool.Parse(_configuration["Email:UseSsl"] ?? "true");
            var fromEmail = _configuration["Email:FromEmail"] ?? throw new InvalidOperationException("From email not configured");
            var fromName = _configuration["Email:FromName"] ?? "MonthBalance";
            var username = Environment.GetEnvironmentVariable("EMAIL_USERNAME") ?? throw new InvalidOperationException("Email username not configured");
            var password = Environment.GetEnvironmentVariable("EMAIL_PASSWORD") ?? throw new InvalidOperationException("Email password not configured");
            
            var message = new MimeMessage();
            message.From.Add(new MailboxAddress(fromName, fromEmail));
            message.To.Add(new MailboxAddress(toName, toEmail));
            message.Subject = subject;
            
            var bodyBuilder = new BodyBuilder
            {
                HtmlBody = htmlBody
            };
            message.Body = bodyBuilder.ToMessageBody();
            
            using var client = new SmtpClient();
            await client.ConnectAsync(smtpHost, smtpPort, useSsl ? SecureSocketOptions.StartTls : SecureSocketOptions.None);
            await client.AuthenticateAsync(username, password);
            await client.SendAsync(message);
            await client.DisconnectAsync(true);
            
            _logger.LogInformation("Email sent successfully to {Email}", toEmail);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error sending email to {Email}", toEmail);
            throw;
        }
    }
}
