using ByWay.Domain.Dtos.AuthDtos;
using ByWay.Domain.Interfaces.Service;
using Microsoft.AspNetCore.Mvc;

namespace ByWay.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class AuthController : ControllerBase
{
  private readonly IAuthService _authService;

  public AuthController(IAuthService authService)
  {
    _authService = authService;
  }

  [HttpPost("register")]
  [ProducesResponseType(StatusCodes.Status200OK)]
  [ProducesResponseType(StatusCodes.Status400BadRequest)]
  [ProducesResponseType(StatusCodes.Status401Unauthorized)]
  public async Task<IActionResult> Register([FromBody] RegisterDto model)
  {
    if (!ModelState.IsValid)
    {
      return BadRequest(ModelState);
    }

    var result = await _authService.RegisterAsync(model);
    if (!result.IsAuthenticated)
    {
      return BadRequest(result.Message);
    }

    return Ok(result);
  }

  [HttpPost("login")]
  [ProducesResponseType(StatusCodes.Status200OK)]
  [ProducesResponseType(StatusCodes.Status400BadRequest)]
  [ProducesResponseType(StatusCodes.Status401Unauthorized)]
  public async Task<IActionResult> Login([FromBody] LoginDto model)
  {
    if (!ModelState.IsValid)
    {
      return BadRequest(ModelState);
    }

    var result = await _authService.LoginAsync(model);
    if (!result.IsAuthenticated)
    {
      return BadRequest(result.Message);
    }

    return Ok(result);
  }
}
