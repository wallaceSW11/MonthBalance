using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace backend.Models;

public class MonthExpense
{
    public int Id { get; set; }
    
    public int MonthDataId { get; set; }
    public MonthData MonthData { get; set; } = null!;
    
    public int ExpenseId { get; set; }
    public Expense Expense { get; set; } = null!;
    
    [Required]
    [Column(TypeName = "decimal(18,2)")]
    public decimal Value { get; set; }
    
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
}
