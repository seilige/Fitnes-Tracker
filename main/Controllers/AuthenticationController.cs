using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using Microsoft.AspNetCore.Authorization;

namespace FitnesTracker;

[ApiController]
[Route("api/auth")]
[EnableRateLimiting("Fixed")]
[Produces("application/json")]
[Authorize]
public class AuthenticationController : ControllerBase
{
    private readonly IAuthentication _auth;

    public AuthenticationController(IAuthentication auth)
    {
        _auth = auth;
    }

    /// <summary>
    /// Refresh token.
    /// </summary>
    /// <param name="dto">Refresh request dto.</param>
    /// <response code="200">Token refreshs successfuly.</response>
    /// <response code="401">Invalid token.</response>
    /// <response code="404">User not found.</response>
    [ProducesResponseType(typeof(AuthResponseDTO), 200)]
    [ProducesResponseType(401)]
    [ProducesResponseType(404)]
    [HttpPost("refresh")]
    public async Task<ActionResult> RefreshToken([FromBody] RefreshRequestDTO dto)
    {
        var result = await _auth.GetTokenAsync(dto);
        return Ok(result);
    }

    /// <summary>
    /// Logout user and invalidate refresh token.
    /// </summary>
    /// <param name="dto">Refresh request dto containing the token to invalidate.</param>
    /// <response code="200">Successfully logged out.</response>
    /// <response code="401">Token is missing or invalid.</response>
    [ProducesResponseType(200)]
    [ProducesResponseType(401)]
    [AllowAnonymous] // не проверяет access токен.
    [HttpPost("logout")]
    public async Task<ActionResult> Logout([FromBody] RefreshRequestDTO dto)
    {
        var token = dto.Token;
    
        if (string.IsNullOrEmpty(token))
            return Unauthorized("Token is required");

        await _auth.Logout(token);

        return Ok("Successfully logged out");
    }

    /// <summary>
    /// Get paginated list of all users.
    /// </summary>
    /// <param name="pageNumber">Page number (default: 1).</param>
    /// <param name="pageSize">Number of items per page (default: 10).</param>
    /// <response code="200">Returns paginated list of users.</response>
    [HttpGet("all")]
    public async Task<ActionResult<PagedResult<UserResponseDTO>>> GetAll([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10)
    {
        var result = await _auth.GetAllAsync(pageNumber, pageSize);
        return Ok(result);
    }

    /// <summary>
    /// Authenticate user and return access token.
    /// </summary>
    /// <param name="dto">User login credentials.</param>
    /// <response code="200">Returns auth token.</response>
    /// <response code="401">Invalid email or password.</response>
    /// <response code="404">User not found.</response>
    [AllowAnonymous]
    [HttpPost("login")]
    public async Task<ActionResult> Login([FromBody] UserLoginDTO dto)
    {
        var token = await _auth.Login(dto.Email, dto.Password);
        return Ok(token);
    }

    /// <summary>
    /// Register a new user and return access token.
    /// </summary>
    /// <param name="dto">User registration data.</param>
    /// <response code="200">User registered successfully, returns auth token.</response>
    /// <response code="400">Invalid registration data or user already exists.</response>
    [AllowAnonymous]
    [HttpPost("register")]
    public async Task<ActionResult> Register([FromBody] UserRegisterDTO dto)
    {
        var token = await _auth.Register(dto.Email, dto.Password, dto.Name, dto.Lastname);
        return Ok(token);
    }
}
