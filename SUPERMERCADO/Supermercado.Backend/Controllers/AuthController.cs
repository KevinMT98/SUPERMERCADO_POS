using Microsoft.AspNetCore.Mvc;
using Supermercado.Backend.UnitsOfWork.Interfaces;
using Supermercado.Shared.DTOs;

namespace Supermercado.Backend.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly IAuthUnitOfWork _authUnitOfWork;

    public AuthController(IAuthUnitOfWork authUnitOfWork)
    {
        _authUnitOfWork = authUnitOfWork;
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginDTO dto)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }
        var response = await _authUnitOfWork.LoginAsync(dto);
        if (!response.WasSuccess)
        {
            return BadRequest(new { message = response.Message });
        }
        return Ok(response.Result);
    }

    [HttpPost("Logout")]

    public IActionResult Logout()
    {
        return NoContent();
    }
}
