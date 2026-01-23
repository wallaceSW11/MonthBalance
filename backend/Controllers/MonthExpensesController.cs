using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using backend.DTOs;
using backend.Services;

namespace backend.Controllers;

[ApiController]
[Route("api/months/{year}/{month}/expenses")]
[Authorize]
public class MonthExpensesController : ControllerBase
{
    private readonly IMonthExpenseService _monthExpenseService;

    public MonthExpensesController(IMonthExpenseService monthExpenseService)
    {
        _monthExpenseService = monthExpenseService;
    }

    private int GetCurrentUserId()
    {
        var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        return int.Parse(userIdClaim!);
    }

    [HttpGet]
    public async Task<ActionResult<List<MonthExpenseDto>>> GetByMonth(int year, int month)
    {
        var userId = GetCurrentUserId();
        var expenses = await _monthExpenseService.GetByMonthAsync(userId, year, month);
        return Ok(expenses);
    }

    [HttpPost]
    public async Task<ActionResult<MonthExpenseDto>> AddToMonth(
        int year, 
        int month, 
        [FromBody] CreateMonthExpenseDto dto)
    {
        try
        {
            var userId = GetCurrentUserId();
            var created = await _monthExpenseService.AddToMonthAsync(userId, year, month, dto);
            return Ok(created);
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    [HttpPut("{id}")]
    public async Task<ActionResult<MonthExpenseDto>> Update(int id, [FromBody] UpdateMonthExpenseDto dto)
    {
        try
        {
            var updated = await _monthExpenseService.UpdateAsync(id, dto);
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
        await _monthExpenseService.DeleteAsync(id);
        return NoContent();
    }
}
