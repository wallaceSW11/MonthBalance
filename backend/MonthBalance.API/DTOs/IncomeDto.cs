namespace MonthBalance.API.DTOs;

public class IncomeDto
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Type { get; set; } = "manual";
    public decimal? GrossValue { get; set; }
    public decimal? NetValue { get; set; }
    public decimal? HourlyRate { get; set; }
    public int? Hours { get; set; }
    public int? Minutes { get; set; }
}

public class CreateIncomeDto
{
    public string Name { get; set; } = string.Empty;
    public string Type { get; set; } = "manual";
    public decimal? GrossValue { get; set; }
    public decimal? NetValue { get; set; }
    public decimal? HourlyRate { get; set; }
    public int? Hours { get; set; }
    public int? Minutes { get; set; }
}

public class UpdateIncomeDto
{
    public string Name { get; set; } = string.Empty;
    public string Type { get; set; } = "manual";
    public decimal? GrossValue { get; set; }
    public decimal? NetValue { get; set; }
    public decimal? HourlyRate { get; set; }
    public int? Hours { get; set; }
    public int? Minutes { get; set; }
}
