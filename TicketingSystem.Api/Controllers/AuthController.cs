using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using TicketingSystem.Application.Interfaces;

namespace TicketingSystem.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly IAuthService _authService;

    public AuthController(IAuthService authService)
    {
        _authService = authService;
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] AuthRequest request)
    {
        var result = await _authService.RegisterAsync(request.Email, request.Password);
        if (result)
        {
            return Ok(new { message = "Usuario registrado exitosamente." });
        }
        return BadRequest(new { error = "No se pudo registrar el usuario. Comprueba que la contraseña cumpla los requisitos." });
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] AuthRequest request)
    {
        try
        {
            var token = await _authService.LoginAsync(request.Email, request.Password);
            return Ok(new { token });
        }
        catch (UnauthorizedAccessException ex)
        {
            return Unauthorized(new { error = ex.Message });
        }
    }
}

public class AuthRequest
{
    public string Email { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
}