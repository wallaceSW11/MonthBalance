using System.ComponentModel.DataAnnotations;

namespace MonthBalance.API.Models;

public class ExpenseTypeModel
{
    public int Id { get; set; }
    
    [Required]
    public int UserId { get; set; }
    
    [Required]
    [MaxLength(200)]
    public string Name { get; set; } = string.Empty;
    
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    
    public User User { get; set; } = null!;
    public ICollection<Expense> Expenses { get; set; } = new List<Expense>();
}
