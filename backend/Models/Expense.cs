using System.ComponentModel.DataAnnotations;

namespace backend.Models;

public class Expense
{
    public int Id { get; set; }
    
    [Required]
    [MaxLength(200)]
    public string Description { get; set; } = string.Empty;
    
    public int UserId { get; set; }
    public User User { get; set; } = null!;
    
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
    
    public ICollection<MonthExpense> MonthExpenses { get; set; } = new List<MonthExpense>();
}
