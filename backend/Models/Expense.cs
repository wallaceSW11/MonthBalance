using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MonthBalance.API.Models;

public class Expense
{
    public int Id { get; set; }
    
    [Required]
    public int MonthDataId { get; set; }
    
    [Required]
    public int ExpenseTypeId { get; set; }
    
    [Required]
    [Column(TypeName = "decimal(18,2)")]
    public decimal Value { get; set; }
    
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    
    public MonthData MonthData { get; set; } = null!;
    public ExpenseTypeModel ExpenseType { get; set; } = null!;
}
