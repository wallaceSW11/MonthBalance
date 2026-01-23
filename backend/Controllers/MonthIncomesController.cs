using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using backend.DTOs;
using backend.Services;

namespace backend.Controllers;

[ApiController]
[Route("api/months/{year}/{month}/incomes")]
[Authorize]
public class MonthIncomesController : ControllerBase
{
    private readonly IMonthIncomeService _monthIncomeService;

    public MonthIncomesController(IMonthIncomeService monthIncomeService)
    {
        _monthIncomeService = monthIncomeService;
    }

    private int GetCurrentUserId()
    {
        var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        return int.Parse(userIdClaim!);
    }

    [HttpGet]
    public async Task<ActionResult<List<MonthIncomeDto>>> GetByMonth(int year, int month)
    {
        var userId = GetCurrentUserId();
        var incomes = await _monthIncomeService.GetByMonthAsync(userId, year, month);
        return Ok(incomes);
    }

    [HttpPost]
    public async Task<ActionResult<MonthIncomeDto>> AddToMonth(
        int year, 
        int month, 
        [FromBody] CreateMonthIncomeDto dto)
    {
        try
        {
            var userId = GetCurrentUserId();
            var created = await _monthIncomeService.AddToMonthAsync(userId, year, month, dto);
            return Ok(created);
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    [HttpPut("{id}")]
    public async Task<ActionResult<MonthIncomeDto>> Update(int id, [FromBody] UpdateMonthIncomeDto dto)
    {
        try
        {
            var updated = await _monthIncomeService.UpdateAsync(id, dto);
            return Ok(updated);
        }
        catch (InvalidOperationException ex)
        {
            return NotFound(new { message = ex.Message });
        }
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        await _monthIncomeService.DeleteAsync(id);
        return NoContent();
    }
}
