using System.ComponentModel.DataAnnotations;

namespace MonthBalance.API.Models;

public class UserSession
{
    public int Id { get; set; }
    
    public int UserId { get; set; }
    
    public DateTime LoginAt { get; set; }
    
    public DateTime? LogoutAt { get; set; }
    
    [MaxLength(50)]
    public string? IpAddress { get; set; }
    
    [MaxLength(500)]
    public string? UserAgent { get; set; }
    
    public bool IsActive { get; set; }
    
    public User User { get; set; } = null!;
}
