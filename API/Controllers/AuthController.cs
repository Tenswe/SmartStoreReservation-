using Microsoft.AspNetCore.Mvc;
using SmartStoreReservation.Core.DTOs;
using SmartStoreReservation.Services;

namespace SmartStoreReservation.API.Controllers;

[ApiController]
[Route("api/v1/[controller]")]
public class AuthController : ControllerBase
{
    private readonly IAuthService _authService;
    private readonly ILogger<AuthController> _logger;

    public AuthController(IAuthService authService, ILogger<AuthController> logger)
    {
        _authService = authService;
        _logger = logger;
    }

    /// <summary>
    /// Înregistrare utilizator nou
    /// </summary>
    [HttpPost("register")]
    public async Task<ActionResult> Register([FromBody] RegisterDto dto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        try
        {
            var user = await _authService.RegisterAsync(dto);
            return Ok(new { 
                Message = "Înregistrare reușită!", 
                UserId = user.Id,
                Name = user.Name,
                Email = user.Email
            });
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new { Message = ex.Message });
        }
    }

    /// <summary>
    /// Autentificare utilizator
    /// </summary>
    [HttpPost("login")]
    public async Task<ActionResult> Login([FromBody] LoginDto dto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        try
        {
            var user = await _authService.LoginAsync(dto);
            return Ok(new { 
                Message = "Autentificare reușită!", 
                UserId = user.Id,
                Name = user.Name,
                Email = user.Email
            });
        }
        catch (UnauthorizedAccessException ex)
        {
            return Unauthorized(new { Message = ex.Message });
        }
    }
}
