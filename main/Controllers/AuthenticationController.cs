using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;

namespace FitnesTracker;

[ApiController]
[Route("api/[controller]")]
[EnableRateLimiting("Fixed")]
public class AuthenticationController : ControllerBase
{
    private readonly IAuthentication _auth;

    public AuthenticationController(IAuthentication auth)
    {
        _auth = auth;
    }

    [HttpPost("login")]
    public async Task<ActionResult> Login([FromBody] UserLoginDTO dto)
    {
        var token = await _auth.Login(dto.Email, dto.Password);
        return Ok(token);
    }

    [HttpPost("register")]
    public async Task<ActionResult> Register([FromBody] UserRegisterDTO dto)
    {
        var token = await _auth.Register(dto.Email, dto.Password, dto.Name, dto.Lastname);
        return Ok(token);
    }

}
