using System.Security.Cryptography;
using Microsoft.EntityFrameworkCore;
using MonthBalance.API.Data;
using MonthBalance.API.Models;
using MonthBalance.API.Repositories;

namespace MonthBalance.API.Services;

public class PasswordResetService : IPasswordResetService
{
    private readonly IUserRepository _userRepository;
    private readonly IEmailService _emailService;
    private readonly IAnalyticsService _analyticsService;
    private readonly ApplicationDbContext _context;
    private readonly ILogger<PasswordResetService> _logger;
    
    public PasswordResetService(
        IUserRepository userRepository,
        IEmailService emailService,
        IAnalyticsService analyticsService,
        ApplicationDbContext context,
        ILogger<PasswordResetService> logger)
    {
        _userRepository = userRepository;
        _emailService = emailService;
        _analyticsService = analyticsService;
        _context = context;
        _logger = logger;
    }
    
    public async Task RequestPasswordResetAsync(string email)
    {
        var user = await _userRepository.GetByEmailAsync(email);
        
        if (user == null)
        {
            _logger.LogWarning("Password reset requested for non-existent email: {Email}", email);
            return;
        }
        
        var token = GenerateSecureToken();
        
        var resetToken = new PasswordResetToken
        {
            UserId = user.Id,
            Token = token,
            ExpiresAt = DateTime.UtcNow.AddHours(1),
            IsUsed = false,
            CreatedAt = DateTime.UtcNow
        };
        
        _context.PasswordResetTokens.Add(resetToken);
        await _context.SaveChangesAsync();
        
        await _emailService.SendPasswordResetEmailAsync(user.Email, user.Name, token);
        
        await _analyticsService.TrackActivityAsync(user.Id, ActivityType.PasswordResetRequested);
        
        _logger.LogInformation("Password reset token created for user {UserId}", user.Id);
    }
    
    public async Task ResetPasswordAsync(string token, string newPassword)
    {
        var resetToken = await _context.PasswordResetTokens
            .Include(t => t.User)
            .FirstOrDefaultAsync(t => t.Token == token);
        
        if (resetToken == null)
            throw new InvalidOperationException("Token inválido");
        
        if (resetToken.IsUsed)
            throw new InvalidOperationException("Token já foi utilizado");
        
        if (resetToken.ExpiresAt < DateTime.UtcNow)
            throw new InvalidOperationException("Token expirado");
        
        resetToken.User.PasswordHash = BCrypt.Net.BCrypt.HashPassword(newPassword);
        resetToken.IsUsed = true;
        
        await _context.SaveChangesAsync();
        
        await _analyticsService.TrackActivityAsync(resetToken.UserId, ActivityType.PasswordResetCompleted);
        
        _logger.LogInformation("Password reset completed for user {UserId}", resetToken.UserId);
    }
    
    private static string GenerateSecureToken()
    {
        var randomBytes = new byte[32];
        using var rng = RandomNumberGenerator.Create();
        rng.GetBytes(randomBytes);
        return Convert.ToBase64String(randomBytes).Replace("+", "-").Replace("/", "_").Replace("=", "");
    }
}
