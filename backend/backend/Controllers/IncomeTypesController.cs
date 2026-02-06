using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MonthBalance.API.DTOs;
using MonthBalance.API.Services;

namespace MonthBalance.API.Controllers;

[Authorize]
[ApiController]
[Route("api/income-types")]
public class IncomeTypesController : ControllerBase
{
    private readonly IIncomeTypeService _incomeTypeService;
    
    public IncomeTypesController(IIncomeTypeService incomeTypeService)
    {
        _incomeTypeService = incomeTypeService;
    }
    
    [HttpGet]
    public async Task<ActionResult<List<IncomeTypeDto>>> GetAll()
    {
        var userId = GetUserIdFromToken();
        var incomeTypes = await _incomeTypeService.GetAllByUserAsync(userId);
        
        return Ok(incomeTypes);
    }
    
    [HttpGet("{id}")]
    public async Task<ActionResult<IncomeTypeDto>> GetById(int id)
    {
        var userId = GetUserIdFromToken();
        
        try
        {
            var incomeType = await _incomeTypeService.GetByIdAsync(userId, id);
            return Ok(incomeType);
        }
        catch (InvalidOperationException ex)
        {
            return NotFound(new { message = ex.Message });
        }
        catch (UnauthorizedAccessException)
        {
            return NotFound(new { message = "Income type not found" });
        }
    }
    
    [HttpPost]
    public async Task<ActionResult<IncomeTypeDto>> Create([FromBody] CreateIncomeTypeRequest request)
    {
        var userId = GetUserIdFromToken();
        
        try
        {
            var incomeType = await _incomeTypeService.CreateAsync(userId, request);
            return CreatedAtAction(nameof(GetById), new { id = incomeType.Id }, incomeType);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }
    
    [HttpPut("{id}")]
    public async Task<ActionResult<IncomeTypeDto>> Update(int id, [FromBody] UpdateIncomeTypeRequest request)
    {
        var userId = GetUserIdFromToken();
        
        try
        {
            var incomeType = await _incomeTypeService.UpdateAsync(userId, id, request);
            return Ok(incomeType);
        }
        catch (InvalidOperationException ex)
        {
            return NotFound(new { message = ex.Message });
        }
        catch (UnauthorizedAccessException)
        {
            return NotFound(new { message = "Income type not found" });
        }
    }
    
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var userId = GetUserIdFromToken();
        
        try
        {
            await _incomeTypeService.DeleteAsync(userId, id);
            return NoContent();
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
        catch (UnauthorizedAccessException)
        {
            return NotFound(new { message = "Income type not found" });
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
