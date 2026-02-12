using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MonthBalance.API.DTOs;
using MonthBalance.API.Services;

namespace MonthBalance.API.Controllers;

[ApiController]
[Route("api/admin")]
[Authorize(Roles = "Admin")]
public class AdminController : ControllerBase
{
    private readonly IAdminService _adminService;
    
    public AdminController(IAdminService adminService)
    {
        _adminService = adminService;
    }
    
    [HttpGet("dashboard")]
    public async Task<ActionResult<AdminDashboardDto>> GetDashboard()
    {
        var dashboard = await _adminService.GetDashboardAsync();
        return Ok(dashboard);
    }
    
    [HttpGet("users")]
    public async Task<ActionResult<UserListResponseDto>> GetUsers(
        [FromQuery] string? search,
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 20)
    {
        var users = await _adminService.GetUsersAsync(search, page, pageSize);
        return Ok(users);
    }
    
    [HttpGet("users/{id}")]
    public async Task<ActionResult<UserSummaryDto>> GetUser(int id)
    {
        var user = await _adminService.GetUserSummaryAsync(id);
        
        if (user == null)
            return NotFound(new { message = "Usuário não encontrado" });
        
        return Ok(user);
    }
}
