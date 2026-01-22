using Microsoft.AspNetCore.Mvc;
using MonthBalance.DTOs;
using MonthBalance.Services;

namespace MonthBalance.Controllers;

[ApiController]
[Route("api/months/{year}/{month}/expenses")]
public class ExpensesController : ControllerBase
{
    private readonly IExpenseService _expenseService;

    public ExpensesController(IExpenseService expenseService)
    {
        _expenseService = expenseService;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<ExpenseDto>>> GetByMonth(int year, int month)
    {
        var expenses = await _expenseService.GetByMonthAsync(year, month);

        return Ok(expenses);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<ExpenseDto>> GetById(int id)
    {
        var expense = await _expenseService.GetByIdAsync(id);

        if (expense == null)
        {
            return NotFound();
        }

        return Ok(expense);
    }

    [HttpPost]
    public async Task<ActionResult<ExpenseDto>> Create(int year, int month, [FromBody] CreateExpenseDto dto)
    {
        var created = await _expenseService.CreateAsync(year, month, dto);

        return CreatedAtAction(nameof(GetById), new { year, month, id = created.Id }, created);
    }

    [HttpPut("{id}")]
    public async Task<ActionResult<ExpenseDto>> Update(int id, [FromBody] UpdateExpenseDto dto)
    {
        try
        {
            var updated = await _expenseService.UpdateAsync(id, dto);

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
        await _expenseService.DeleteAsync(id);

        return NoContent();
    }
}
