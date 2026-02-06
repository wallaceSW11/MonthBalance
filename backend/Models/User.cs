using System.ComponentModel.DataAnnotations;

namespace MonthBalance.API.Models;

public class User
{
    public int Id { get; set; }
    
    [Required]
    [MaxLength(200)]
    public string Name { get; set; } = string.Empty;
    
    [Required]
    [MaxLength(200)]
    public string Email { get; set; } = string.Empty;
    
    [Required]
    public string PasswordHash { get; set; } = string.Empty;
    
    [MaxLength(500)]
    public string? Avatar { get; set; }
    
    public bool NotificationsEnabled { get; set; } = true;
    
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    
    public ICollection<MonthData> MonthData { get; set; } = new List<MonthData>();
    public ICollection<IncomeTypeModel> IncomeTypes { get; set; } = new List<IncomeTypeModel>();
    public ICollection<ExpenseTypeModel> ExpenseTypes { get; set; } = new List<ExpenseTypeModel>();
}
