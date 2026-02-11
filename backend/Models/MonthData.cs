using System.ComponentModel.DataAnnotations;

namespace MonthBalance.API.Models;

public class MonthData
{
    public int Id { get; set; }
    
    [Required]
    public int UserId { get; set; }
    
    [Required]
    public int Year { get; set; }
    
    [Required]
    public int Month { get; set; }
    
    public DateTime LastAccessed { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    
    public User User { get; set; } = null!;
    public ICollection<Income> Incomes { get; set; } = new List<Income>();
    public ICollection<Expense> Expenses { get; set; } = new List<Expense>();
}
