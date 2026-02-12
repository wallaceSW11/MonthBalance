using System.ComponentModel.DataAnnotations;

namespace MonthBalance.API.Models;

public class UserFeedback
{
    public int Id { get; set; }
    
    public int? UserId { get; set; }
    
    [Required]
    [MaxLength(200)]
    public string Email { get; set; } = string.Empty;
    
    [Required]
    [MaxLength(200)]
    public string Subject { get; set; } = string.Empty;
    
    [Required]
    [MaxLength(5000)]
    public string Message { get; set; } = string.Empty;
    
    public int? Rating { get; set; }
    
    public DateTime CreatedAt { get; set; }
    
    public bool IsRead { get; set; }
    
    [MaxLength(2000)]
    public string? AdminNotes { get; set; }
    
    public User? User { get; set; }
}
