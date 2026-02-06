using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MonthBalance.API.Models;

public class Income
{
    public int Id { get; set; }
    
    [Required]
    public int MonthDataId { get; set; }
    
    [Required]
    public int IncomeTypeId { get; set; }
    
    [Column(TypeName = "decimal(18,2)")]
    public decimal? GrossValue { get; set; }
    
    [Column(TypeName = "decimal(18,2)")]
    public decimal? NetValue { get; set; }
    
    [Column(TypeName = "decimal(18,2)")]
    public decimal? HourlyRate { get; set; }
    
    public int? Hours { get; set; }
    public int? Minutes { get; set; }
    
    [Column(TypeName = "decimal(18,2)")]
    public decimal CalculatedValue { get; set; }
    
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    
    public MonthData MonthData { get; set; } = null!;
    public IncomeTypeModel IncomeType { get; set; } = null!;
}
