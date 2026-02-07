using System.ComponentModel.DataAnnotations;

namespace MonthBalance.API.Models;

public class WebAuthnCredential
{
    public int Id { get; set; }
    
    [Required]
    public int UserId { get; set; }
    
    [Required]
    [MaxLength(500)]
    public string CredentialId { get; set; } = string.Empty;
    
    [Required]
    public string PublicKey { get; set; } = string.Empty;
    
    [Required]
    public long Counter { get; set; } = 0;
    
    [MaxLength(200)]
    public string? Transports { get; set; }
    
    public DateTime CreatedAt { get; set; }
    public DateTime? LastUsedAt { get; set; }
    
    public User User { get; set; } = null!;
}
