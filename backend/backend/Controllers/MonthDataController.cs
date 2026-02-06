using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MonthBalance.API.DTOs;
using MonthBalance.API.Services;

namespace MonthBalance.API.Controllers;

[Authorize]
[ApiController]
[Route("api/month-data")]
public class MonthDataController : ControllerBase
{
    private readonly IMonthDataService _monthDataService;
    
    public MonthDataController(IMonthDataService monthDataService)
    {
        _monthDataService = monthDataService;
    }
    
    [HttpGet]
    public async Task<ActionResult<List<MonthDataDto>>> GetAll()
    {
        var userId = GetUserIdFromToken();
        var monthDataList = await _monthDataService.GetAllByUserAsync(userId);
        
        return Ok(monthDataList);
    }
    
    [HttpGet("{year}/{month}")]
    public async Task<ActionResult<MonthDataDto>> GetByYearAndMonth(int year, int month)
    {
        var userId = GetUserIdFromToken();
        
        try
        {
            var monthData = await _monthDataService.GetByYearAndMonthAsync(userId, year, month);
            return Ok(monthData);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
        catch (InvalidOperationException ex)
        {
            return NotFound(new { message = ex.Message });
        }
    }
    
    [HttpPost]
    public async Task<ActionResult<MonthDataDto>> Create([FromBody] CreateMonthDataRequest request)
    {
        var userId = GetUserIdFromToken();
        
        try
        {
            var monthData = await _monthDataService.CreateAsync(userId, request);
            return CreatedAtAction(nameof(GetByYearAndMonth), 
                new { year = monthData.Year, month = monthData.Month }, 
                monthData);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }
    
    [HttpPut("{id}/last-accessed")]
    public async Task<IActionResult> UpdateLastAccessed(int id)
    {
        var userId = GetUserIdFromToken();
        
        try
        {
            await _monthDataService.UpdateLastAccessedAsync(userId, id);
            return NoContent();
        }
        catch (InvalidOperationException)
        {
            return NotFound(new { message = "Month data not found" });
        }
        catch (UnauthorizedAccessException)
        {
            return Forbid();
        }
    }
    
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var userId = GetUserIdFromToken();
        
        try
        {
            await _monthDataService.DeleteAsync(userId, id);
            return NoContent();
        }
        catch (InvalidOperationException)
        {
            return NotFound(new { message = "Month data not found" });
        }
        catch (UnauthorizedAccessException)
        {
            return Forbid();
        }
    }
    
    private int GetUserIdFromToken()
    {
        var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value 
            ?? User.FindFirst(JwtRegisteredClaimNames.Sub)?.Value;
        
        if (userIdClaim == null)
            throw new UnauthorizedAccessException("Invalid token");
        
        return int.Parse(userIdClaim);
    }
}
