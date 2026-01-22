using Microsoft.AspNetCore.Mvc;
using MonthBalance.API.DTOs;
using MonthBalance.API.Services;

namespace MonthBalance.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class MonthDataController : ControllerBase
{
    private readonly IMonthDataService _monthDataService;

    public MonthDataController(IMonthDataService monthDataService)
    {
        _monthDataService = monthDataService;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<MonthDataDto>>> GetAll()
    {
        var monthDataList = await _monthDataService.GetAllAsync();

        return Ok(monthDataList);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<MonthDataDto>> GetById(int id)
    {
        var monthData = await _monthDataService.GetByIdAsync(id);

        if (monthData == null)
        {
            return NotFound();
        }

        return Ok(monthData);
    }

    [HttpGet("{year}/{month}")]
    public async Task<ActionResult<MonthDataDto>> GetByYearAndMonth(int year, int month)
    {
        var monthData = await _monthDataService.GetByYearAndMonthAsync(year, month);

        if (monthData == null)
        {
            return NotFound();
        }

        return Ok(monthData);
    }

    [HttpPost]
    public async Task<ActionResult<MonthDataDto>> Create([FromBody] CreateMonthDataDto dto)
    {
        try
        {
            var created = await _monthDataService.CreateAsync(dto);

            return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        await _monthDataService.DeleteAsync(id);

        return NoContent();
    }

    [HttpPost("duplicate")]
    public async Task<ActionResult<MonthDataDto>> Duplicate([FromBody] DuplicateMonthDto dto)
    {
        try
        {
            var duplicated = await _monthDataService.DuplicateMonthAsync(
                dto.SourceYear,
                dto.SourceMonth,
                dto.TargetYear,
                dto.TargetMonth
            );

            return CreatedAtAction(nameof(GetById), new { id = duplicated.Id }, duplicated);
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }
}

public class DuplicateMonthDto
{
    public int SourceYear { get; set; }
    public int SourceMonth { get; set; }
    public int TargetYear { get; set; }
    public int TargetMonth { get; set; }
}
