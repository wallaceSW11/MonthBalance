using Microsoft.AspNetCore.Mvc;
using MonthBalance.DTOs;
using MonthBalance.Services;

namespace MonthBalance.Controllers;

[ApiController]
[Route("api/months/{year}/{month}/incomes")]
public class IncomesController : ControllerBase
{
    private readonly IIncomeService _incomeService;

    public IncomesController(IIncomeService incomeService)
    {
        _incomeService = incomeService;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<IncomeDto>>> GetByMonth(int year, int month)
    {
        var incomes = await _incomeService.GetByMonthAsync(year, month);

        return Ok(incomes);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<IncomeDto>> GetById(int id)
    {
        var income = await _incomeService.GetByIdAsync(id);

        if (income == null)
        {
            return NotFound();
        }

        return Ok(income);
    }

    [HttpPost]
    public async Task<ActionResult<IncomeDto>> Create(int year, int month, [FromBody] CreateIncomeDto dto)
    {
        var created = await _incomeService.CreateAsync(year, month, dto);

        return CreatedAtAction(nameof(GetById), new { year, month, id = created.Id }, created);
    }

    [HttpPut("{id}")]
    public async Task<ActionResult<IncomeDto>> Update(int id, [FromBody] UpdateIncomeDto dto)
    {
        try
        {
            var updated = await _incomeService.UpdateAsync(id, dto);

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
        await _incomeService.DeleteAsync(id);

        return NoContent();
    }
}
