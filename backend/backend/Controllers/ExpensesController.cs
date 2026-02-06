using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MonthBalance.API.DTOs;
using MonthBalance.API.Services;

namespace MonthBalance.API.Controllers;

[Authorize]
[ApiController]
[Route("api/expenses")]
public class ExpensesController : ControllerBase
{
    private readonly IExpenseService _expenseService;
    
    public ExpensesController(IExpenseService expenseService)
    {
        _expenseService = expenseService;
    }
    
    [HttpGet("month/{monthDataId}")]
    public async Task<ActionResult<List<ExpenseDto>>> GetByMonthDataId(int monthDataId)
    {
        var userId = GetUserIdFromToken();
        
        try
        {
            var expenses = await _expenseService.GetByMonthDataIdAsync(userId, monthDataId);
            return Ok(expenses);
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
    public async Task<ActionResult<ExpenseDto>> GetById(int id)
    {
        var userId = GetUserIdFromToken();
        
        try
        {
            var expense = await _expenseService.GetByIdAsync(userId, id);
            return Ok(expense);
        }
        catch (InvalidOperationException ex)
        {
            return NotFound(new { message = ex.Message });
        }
        catch (UnauthorizedAccessException)
        {
            return NotFound(new { message = "Expense not found" });
        }
    }
    
    [HttpPost]
    public async Task<ActionResult<ExpenseDto>> Create([FromBody] CreateExpenseRequest request)
    {
        var userId = GetUserIdFromToken();
        
        try
        {
            var expense = await _expenseService.CreateAsync(userId, request);
            return CreatedAtAction(nameof(GetById), new { id = expense.Id }, expense);
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
        catch (UnauthorizedAccessException)
        {
            return BadRequest(new { message = "Invalid month data or expense type" });
        }
    }
    
    [HttpPut("{id}")]
    public async Task<ActionResult<ExpenseDto>> Update(int id, [FromBody] UpdateExpenseRequest request)
    {
        var userId = GetUserIdFromToken();
        
        try
        {
            var expense = await _expenseService.UpdateAsync(userId, id, request);
            return Ok(expense);
        }
        catch (InvalidOperationException ex)
        {
            return NotFound(new { message = ex.Message });
        }
        catch (UnauthorizedAccessException)
        {
            return NotFound(new { message = "Expense not found" });
        }
    }
    
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var userId = GetUserIdFromToken();
        
        try
        {
            await _expenseService.DeleteAsync(userId, id);
            return NoContent();
        }
        catch (InvalidOperationException ex)
        {
            return NotFound(new { message = ex.Message });
        }
        catch (UnauthorizedAccessException)
        {
            return NotFound(new { message = "Expense not found" });
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
