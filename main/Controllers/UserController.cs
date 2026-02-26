using Microsoft.AspNetCore.Mvc;

namespace FitnesTracker;

[ApiController]
[Route("api/[controller]")]
[Produces("application/json")]
public class UserController : ControllerBase
{
    private IUserService _service;
    private IEmailService _emailService;

    public UserController(IUserService service, IEmailService emailService)
    {
        _service = service;
        _emailService = emailService;
    }

    /// <summary>
    /// Checking user authorization.
    /// </summary>
    /// <param name="token">User uniq token.</param>
    /// <returns>Info about user authorization.</returns>
    /// <response code="200">User authorize successfuly.</response>
    /// <response code="401">Email not confirmed.</response>
    [HttpGet("confirm-email")]
    public async Task<ActionResult> ConfirmEmail([FromQuery] string token)
    {
        await _emailService.ConfirmEmailAsync(token);
        return Ok("Email confirmed successfully");
    }

    /// <summary>
    /// Get all users from website.
    /// </summary>
    /// <param name="pageNumber">Number of page data.</param>
    /// <param name="pageSize">Number of records on page.</param>
    /// <returns>List of users.</returns>
    /// <response code="200">User found.</response>
    /// <response code="404">User not found.</response>
    [HttpGet("all")]
    public async Task<ActionResult<PagedResult<UserResponseDTO>>> GetAll([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10)
    {
        var result = await _service.GetAllAsync(pageNumber, pageSize);
        return Ok(result);
    }

    /// <summary>
    /// Get user by id async.
    /// </summary>
    /// <param name="id">Id user.</param>
    /// <returns>Specific user.</returns>
    /// <response code="200">User found.</response>
    /// <response code="404">User not found.</response>
    [HttpGet("{id:int}")]
    public async Task<ActionResult> GetByIdAsync(int id)
    {
        return Ok(await _service.GetByIdAsync(id));
    }

    /// <summary>
    /// Delete user by id.
    /// </summary>
    /// <param name="id">Id user.</param>
    /// <returns>Bool value in method and succsess status to user.</returns>
    /// <response code="200">User found.</response>
    /// <response code="404">User not found.</response>
    [HttpDelete("{id:int}")]
    public async Task<ActionResult> DeleteAsync(int id)
    {
        return Ok(await _service.DeleteAsync(id));
    }

    /// <summary>
    /// Update user by id async.
    /// </summary>
    /// <param name="id">Id user.</param>
    /// <param name="dto">User update dto with id, name and lastname feilds.</param>
    /// <returns>UserResponseDTO.</returns>
    /// <response code="200">User found.</response>
    /// <response code="404">User not found.</response>
    [HttpPut("{id:int}")]
    public async Task<ActionResult> UpdateAsync(int id, [FromBody] UserUpdateDTO dto)
    {
        return Ok(await _service.UpdateAsync(id, dto));
    }

    /// <summary>
    /// Create user by async.
    /// </summary>
    /// <param name="dto">User create DTO with feilds: name, lastname.</param>
    /// <returns>UserResponseDTO.</returns>
    /// <response code="200">User found.</response>
    /// <response code="404">User not found.</response>
    [HttpPost]
    public async Task<ActionResult> CreateAsync([FromBody] UserCreateDTO dto)
    {
        var res = await _service.CreateAsync(dto);

        return CreatedAtAction(nameof(GetByIdAsync), new { id = res.UserId }, res);
    }
}
