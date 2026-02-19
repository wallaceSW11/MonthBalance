using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MonthBalance.API.DTOs;
using MonthBalance.API.Services;

namespace MonthBalance.API.Controllers;

[ApiController]
[Route("api/feedback")]
public class FeedbackController : ControllerBase
{
    private readonly IFeedbackService _feedbackService;
    
    public FeedbackController(IFeedbackService feedbackService)
    {
        _feedbackService = feedbackService;
    }
    
    [HttpPost]
    public async Task<ActionResult<FeedbackDto>> Create([FromBody] CreateFeedbackRequest request)
    {
        int? userId = null;
        string email = "anonimo@monthbalance.com";
        
        if (User.Identity?.IsAuthenticated ?? false)
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value
                ?? User.FindFirst(JwtRegisteredClaimNames.Sub)?.Value;
            
            if (userIdClaim != null && int.TryParse(userIdClaim, out var parsedUserId))
            {
                userId = parsedUserId;
            }
            
            var emailClaim = User.FindFirst(ClaimTypes.Email)?.Value
                ?? User.FindFirst(JwtRegisteredClaimNames.Email)?.Value;
            
            if (emailClaim != null)
            {
                email = emailClaim;
            }
        }
        
        var feedback = await _feedbackService.CreateAsync(userId, email, request);
        
        return CreatedAtAction(nameof(GetById), new { id = feedback.Id }, feedback);
    }
    
    [Authorize(Roles = "Admin")]
    [HttpGet]
    public async Task<ActionResult<List<FeedbackDto>>> GetAll([FromQuery] bool? isRead, [FromQuery] int page = 1, [FromQuery] int pageSize = 20)
    {
        var feedbacks = await _feedbackService.GetAllAsync(isRead, page, pageSize);
        return Ok(feedbacks);
    }
    
    [Authorize(Roles = "Admin")]
    [HttpGet("{id}")]
    public async Task<ActionResult<FeedbackDto>> GetById(int id)
    {
        var feedback = await _feedbackService.GetByIdAsync(id);
        
        if (feedback == null)
            return NotFound(new { message = "Feedback não encontrado" });
        
        return Ok(feedback);
    }
    
    [Authorize(Roles = "Admin")]
    [HttpPut("{id}/mark-read")]
    public async Task<IActionResult> MarkAsRead(int id)
    {
        try
        {
            await _feedbackService.MarkAsReadAsync(id);
            return NoContent();
        }
        catch (InvalidOperationException ex)
        {
            return NotFound(new { message = ex.Message });
        }
    }
    
    [Authorize(Roles = "Admin")]
    [HttpGet("unread-count")]
    public async Task<ActionResult<int>> GetUnreadCount()
    {
        var count = await _feedbackService.GetUnreadCountAsync();
        return Ok(new { count });
    }
}
