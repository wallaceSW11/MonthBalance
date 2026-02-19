using System.ComponentModel.DataAnnotations;

namespace MonthBalance.API.Models;

public class UserActivity
{
    public int Id { get; set; }
    
    public int UserId { get; set; }
    
    public ActivityType ActivityType { get; set; }
    
    public DateTime Timestamp { get; set; }
    
    [MaxLength(50)]
    public string? IpAddress { get; set; }
    
    [MaxLength(500)]
    public string? UserAgent { get; set; }
    
    public string? Metadata { get; set; }
    
    public User User { get; set; } = null!;
}
