using System.ComponentModel.DataAnnotations;

namespace backend.Models;

public class MonthData
{
    public int Id { get; set; }
    
    [Required]
    public int Year { get; set; }
    
    [Required]
    public int Month { get; set; }
    
    public int UserId { get; set; }
    public User User { get; set; } = null!;
    
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
    
    public ICollection<MonthIncome> MonthIncomes { get; set; } = new List<MonthIncome>();
    public ICollection<MonthExpense> MonthExpenses { get; set; } = new List<MonthExpense>();
}
