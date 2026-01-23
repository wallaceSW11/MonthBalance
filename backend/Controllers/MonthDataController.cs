using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using backend.DTOs;
using backend.Services;

namespace backend.Controllers;

[ApiController]
[Route("api/months")]
[Authorize]
public class MonthDataController : ControllerBase
{
    private readonly IMonthDataService _monthDataService;

    public MonthDataController(IMonthDataService monthDataService)
    {
        _monthDataService = monthDataService;
    }

    private int GetCurrentUserId()
    {
        var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        return int.Parse(userIdClaim!);
    }

    [HttpGet("{year}/{month}")]
    public async Task<ActionResult<MonthDataDto>> GetOrCreate(int year, int month)
    {
        var userId = GetCurrentUserId();
        var monthData = await _monthDataService.GetOrCreateAsync(userId, year, month);
        return Ok(monthData);
    }

    [HttpGet]
    public async Task<ActionResult<List<MonthDataDto>>> GetAll()
    {
        var userId = GetCurrentUserId();
        var monthDataList = await _monthDataService.GetByUserIdAsync(userId);
        return Ok(monthDataList);
    }

    [HttpPost("duplicate")]
    public async Task<ActionResult<MonthDataDto>> Duplicate([FromBody] DuplicateMonthDto dto)
    {
        try
        {
            var userId = GetCurrentUserId();
            var duplicated = await _monthDataService.DuplicateAsync(userId, dto);
            return Ok(duplicated);
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    [HttpDelete("{year}/{month}")]
    public async Task<IActionResult> Delete(int year, int month)
    {
        var userId = GetCurrentUserId();
        await _monthDataService.DeleteAsync(userId, year, month);
        return NoContent();
    }
}
