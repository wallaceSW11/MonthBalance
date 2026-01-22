using Microsoft.AspNetCore.Mvc;
using MonthBalance.DTOs;
using MonthBalance.Services;

namespace MonthBalance.Controllers;

[ApiController]
[Route("api/incometypes")]
public class IncomeTypesController : ControllerBase
{
    private readonly IIncomeTypeService _incomeTypeService;

    public IncomeTypesController(IIncomeTypeService incomeTypeService)
    {
        _incomeTypeService = incomeTypeService;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<IncomeTypeDto>>> GetAll()
    {
        var incomeTypes = await _incomeTypeService.GetAllAsync();
        return Ok(incomeTypes);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<IncomeTypeDto>> GetById(int id)
    {
        var incomeType = await _incomeTypeService.GetByIdAsync(id);
        
        if (incomeType == null)
        {
            return NotFound();
        }

        return Ok(incomeType);
    }

    [HttpPost]
    public async Task<ActionResult<IncomeTypeDto>> Create([FromBody] CreateIncomeTypeDto dto)
    {
        var created = await _incomeTypeService.CreateAsync(dto);
        return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
    }

    [HttpPut("{id}")]
    public async Task<ActionResult<IncomeTypeDto>> Update(int id, [FromBody] UpdateIncomeTypeDto dto)
    {
        try
        {
            var updated = await _incomeTypeService.UpdateAsync(id, dto);
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
        await _incomeTypeService.DeleteAsync(id);
        return NoContent();
    }
}
