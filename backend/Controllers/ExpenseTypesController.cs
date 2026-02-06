using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MonthBalance.API.DTOs;
using MonthBalance.API.Services;

namespace MonthBalance.API.Controllers;

[Authorize]
[ApiController]
[Route("api/expense-types")]
public class ExpenseTypesController : ControllerBase
{
    private readonly IExpenseTypeService _expenseTypeService;
    
    public ExpenseTypesController(IExpenseTypeService expenseTypeService)
    {
        _expenseTypeService = expenseTypeService;
    }
    
    [HttpGet]
    public async Task<ActionResult<List<ExpenseTypeDto>>> GetAll()
    {
        var userId = GetUserIdFromToken();
        var expenseTypes = await _expenseTypeService.GetAllByUserAsync(userId);
        
        return Ok(expenseTypes);
    }
    
    [HttpGet("{id}")]
    public async Task<ActionResult<ExpenseTypeDto>> GetById(int id)
    {
        var userId = GetUserIdFromToken();
        
        try
        {
            var expenseType = await _expenseTypeService.GetByIdAsync(userId, id);
            return Ok(expenseType);
        }
        catch (InvalidOperationException ex)
        {
            return NotFound(new { message = ex.Message });
        }
        catch (UnauthorizedAccessException)
        {
            return NotFound(new { message = "Expense type not found" });
        }
    }
    
    [HttpPost]
    public async Task<ActionResult<ExpenseTypeDto>> Create([FromBody] CreateExpenseTypeRequest request)
    {
        var userId = GetUserIdFromToken();
        var expenseType = await _expenseTypeService.CreateAsync(userId, request);
        
        return CreatedAtAction(nameof(GetById), new { id = expenseType.Id }, expenseType);
    }
    
    [HttpPut("{id}")]
    public async Task<ActionResult<ExpenseTypeDto>> Update(int id, [FromBody] UpdateExpenseTypeRequest request)
    {
        var userId = GetUserIdFromToken();
        
        try
        {
            var expenseType = await _expenseTypeService.UpdateAsync(userId, id, request);
            return Ok(expenseType);
        }
        catch (InvalidOperationException ex)
        {
            return NotFound(new { message = ex.Message });
        }
        catch (UnauthorizedAccessException)
        {
            return NotFound(new { message = "Expense type not found" });
        }
    }
    
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var userId = GetUserIdFromToken();
        
        try
        {
            await _expenseTypeService.DeleteAsync(userId, id);
            return NoContent();
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
        catch (UnauthorizedAccessException)
        {
            return NotFound(new { message = "Expense type not found" });
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
