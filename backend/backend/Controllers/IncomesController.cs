using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MonthBalance.API.DTOs;
using MonthBalance.API.Services;

namespace MonthBalance.API.Controllers;

[Authorize]
[ApiController]
[Route("api/incomes")]
public class IncomesController : ControllerBase
{
    private readonly IIncomeService _incomeService;
    
    public IncomesController(IIncomeService incomeService)
    {
        _incomeService = incomeService;
    }
    
    [HttpGet("month/{monthDataId}")]
    public async Task<ActionResult<List<IncomeDto>>> GetByMonthDataId(int monthDataId)
    {
        var userId = GetUserIdFromToken();
        
        try
        {
            var incomes = await _incomeService.GetByMonthDataIdAsync(userId, monthDataId);
            return Ok(incomes);
        }
        catch (InvalidOperationException ex)
        {
            return NotFound(new { message = ex.Message });
        }
        catch (UnauthorizedAccessException)
        {
            return NotFound(new { message = "Month data not found" });
        }
    }
    
    [HttpGet("{id}")]
    public async Task<ActionResult<IncomeDto>> GetById(int id)
    {
        var userId = GetUserIdFromToken();
        
        try
        {
            var income = await _incomeService.GetByIdAsync(userId, id);
            return Ok(income);
        }
        catch (InvalidOperationException ex)
        {
            return NotFound(new { message = ex.Message });
        }
        catch (UnauthorizedAccessException)
        {
            return NotFound(new { message = "Income not found" });
        }
    }
    
    [HttpPost]
    public async Task<ActionResult<IncomeDto>> Create([FromBody] CreateIncomeRequest request)
    {
        var userId = GetUserIdFromToken();
        
        try
        {
            var income = await _incomeService.CreateAsync(userId, request);
            return CreatedAtAction(nameof(GetById), new { id = income.Id }, income);
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
        catch (UnauthorizedAccessException)
        {
            return BadRequest(new { message = "Invalid month data or income type" });
        }
    }
    
    [HttpPut("{id}")]
    public async Task<ActionResult<IncomeDto>> Update(int id, [FromBody] UpdateIncomeRequest request)
    {
        var userId = GetUserIdFromToken();
        
        try
        {
            var income = await _incomeService.UpdateAsync(userId, id, request);
            return Ok(income);
        }
        catch (InvalidOperationException ex)
        {
            return NotFound(new { message = ex.Message });
        }
        catch (UnauthorizedAccessException)
        {
            return NotFound(new { message = "Income not found" });
        }
    }
    
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var userId = GetUserIdFromToken();
        
        try
        {
            await _incomeService.DeleteAsync(userId, id);
            return NoContent();
        }
        catch (InvalidOperationException ex)
        {
            return NotFound(new { message = ex.Message });
        }
        catch (UnauthorizedAccessException)
        {
            return NotFound(new { message = "Income not found" });
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
