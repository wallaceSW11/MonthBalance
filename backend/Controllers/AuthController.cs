using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MonthBalance.API.DTOs;
using MonthBalance.API.Services;

namespace MonthBalance.API.Controllers;

[ApiController]
[Route("api/auth")]
public class AuthController : ControllerBase
{
    private readonly IAuthService _authService;
    private readonly IWebAuthnService _webAuthnService;

    public AuthController(IAuthService authService, IWebAuthnService webAuthnService)
    {
        _authService = authService;
        _webAuthnService = webAuthnService;
    }

    [HttpPost("register")]
    public async Task<ActionResult<LoginResponse>> Register([FromBody] RegisterRequest request)
    {
        try
        {
            var response = await _authService.RegisterAsync(request);
            return Ok(response);
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    [HttpPost("login")]
    public async Task<ActionResult<LoginResponse>> Login([FromBody] LoginRequest request)
    {
        try
        {
            var response = await _authService.LoginAsync(request);
            return Ok(response);
        }
        catch (UnauthorizedAccessException ex)
        {
            return Unauthorized(new { message = ex.Message });
        }
    }

    [Authorize]
    [HttpGet("me")]
    public async Task<ActionResult<UserDto>> GetCurrentUser()
    {
        var userId = GetUserIdFromToken();

        try
        {
            var user = await _authService.GetCurrentUserAsync(userId);
            return Ok(user);
        }
        catch (InvalidOperationException ex)
        {
            return NotFound(new { message = ex.Message });
        }
    }

    [Authorize]
    [HttpPut("me")]
    public async Task<ActionResult<UserDto>> UpdateUser([FromBody] UpdateUserRequest request)
    {
        var userId = GetUserIdFromToken();

        try
        {
            var user = await _authService.UpdateUserAsync(userId, request);
            return Ok(user);
        }
        catch (InvalidOperationException ex)
        {
            return NotFound(new { message = ex.Message });
        }
    }

    [Authorize]
    [HttpPost("change-password")]
    public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordRequest request)
    {
        var userId = GetUserIdFromToken();

        try
        {
            await _authService.ChangePasswordAsync(userId, request);
            return NoContent();
        }
        catch (InvalidOperationException ex)
        {
            return NotFound(new { message = ex.Message });
        }
        catch (UnauthorizedAccessException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }


    [Authorize]
    [HttpDelete("me")]
    public async Task<IActionResult> DeleteAccount()
    {
        var userId = GetUserIdFromToken();

        try
        {
            await _authService.DeleteAccountAsync(userId);
            return NoContent();
        }
        catch (InvalidOperationException ex)
        {
            return NotFound(new { message = ex.Message });
        }
    }

    [Authorize]
    [HttpPost("webauthn/register/challenge")]
    public async Task<ActionResult<WebAuthnRegisterChallengeResponse>> GenerateRegisterChallenge()
    {
        var userId = GetUserIdFromToken();

        try
        {
            var response = await _webAuthnService.GenerateRegisterChallengeAsync(userId);
            return Ok(response);
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    [Authorize]
    [HttpPost("webauthn/register")]
    public async Task<ActionResult<WebAuthnRegisterResponse>> RegisterCredential([FromBody] WebAuthnRegisterRequest request)
    {
        var userId = GetUserIdFromToken();

        try
        {
            var response = await _webAuthnService.RegisterCredentialAsync(userId, request);
            return CreatedAtAction(nameof(RegisterCredential), response);
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    [HttpPost("webauthn/authenticate/challenge")]
    public async Task<ActionResult<WebAuthnAuthenticateChallengeResponse>> GenerateAuthenticateChallenge([FromBody] WebAuthnAuthenticateChallengeRequest request)
    {
        try
        {
            var response = await _webAuthnService.GenerateAuthenticateChallengeAsync(request.Email);
            return Ok(response);
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    [HttpPost("webauthn/authenticate")]
    public async Task<ActionResult<LoginResponse>> AuthenticateWithWebAuthn([FromBody] WebAuthnAuthenticateRequest request)
    {
        try
        {
            var response = await _webAuthnService.AuthenticateAsync(request);
            return Ok(response);
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
        catch (UnauthorizedAccessException ex)
        {
            return Unauthorized(new { message = ex.Message });
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

