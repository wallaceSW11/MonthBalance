using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MonthBalance.Models;

public class Income
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }

    [Required]
    [MaxLength(200)]
    public string Name { get; set; } = string.Empty;

    [Required]
    [MaxLength(20)]
    public string Type { get; set; } = "manual";

    [Column(TypeName = "decimal(18,2)")]
    public decimal? GrossValue { get; set; }

    [Column(TypeName = "decimal(18,2)")]
    public decimal? NetValue { get; set; }

    [Column(TypeName = "decimal(18,2)")]
    public decimal? HourlyRate { get; set; }

    public int? Hours { get; set; }

    public int? Minutes { get; set; }

    public int MonthDataId { get; set; }

    [ForeignKey(nameof(MonthDataId))]
    public MonthData MonthData { get; set; } = null!;

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
}
