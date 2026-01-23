using System.ComponentModel.DataAnnotations.Schema;

namespace backend.Models;

public class MonthIncome
{
    public int Id { get; set; }
    
    public int MonthDataId { get; set; }
    public MonthData MonthData { get; set; } = null!;
    
    public int IncomeId { get; set; }
    public Income Income { get; set; } = null!;
    
    [Column(TypeName = "decimal(18,2)")]
    public decimal? GrossValue { get; set; }
    
    [Column(TypeName = "decimal(18,2)")]
    public decimal? NetValue { get; set; }
    
    [Column(TypeName = "decimal(18,2)")]
    public decimal? HourlyRate { get; set; }
    
    public int? Hours { get; set; }
    public int? Minutes { get; set; }
    
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
}
