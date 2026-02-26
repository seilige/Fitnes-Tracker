using Microsoft.AspNetCore.Mvc;

namespace FitnesTracker;

[ApiController]
[Route("api/[controller]")]
[Produces("application/json")]
public class CustomProgramController : ControllerBase
{
    private ICustomProgramService _service;

    public CustomProgramController(ICustomProgramService service)
    {
        _service = service;
    }
    
    /// <summary>
    /// Get paginated list of all custom programs.
    /// </summary>
    /// <param name="pageNumber">Page number (default: 1).</param>
    /// <param name="pageSize">Number of items per page (default: 10).</param>
    /// <response code="200">Returns paginated list of custom programs.</response>
    [ProducesResponseType(typeof(PagedResult<UserResponseDTO>), 200)]
    [HttpGet("all")]
    public async Task<ActionResult<PagedResult<UserResponseDTO>>> GetAll([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10)
    {
        var result = await _service.GetAllAsync(pageNumber, pageSize);
        return Ok(result);
    }

    /// <summary>
    /// Create a new custom program.
    /// </summary>
    /// <param name="dto">Custom program creation data.</param>
    /// <param name="creatorId">ID of the user creating the program.</param>
    /// <response code="201">Custom program created successfully.</response>
    /// <response code="400">Invalid creation data.</response>
    /// <response code="404">Creator not found.</response>
    [ProducesResponseType(typeof(CustomProgramResponseDTO), 201)]
    [ProducesResponseType(400)]
    [ProducesResponseType(404)]
    [HttpPost("{creatorId}")]
    public async Task<ActionResult> CreateAsync([FromBody] CustomProgramCreateDTO dto, [FromRoute] int creatorId)
    {
        var result = await _service.CreateAsync(dto, creatorId);
        return CreatedAtAction(nameof(GetByIdAsync), new { id = result.CustProgId }, result);
    }

    /// <summary>
    /// Get custom program by ID.
    /// </summary>
    /// <param name="id">Custom program ID.</param>
    /// <response code="200">Returns custom program.</response>
    /// <response code="404">Custom program not found.</response>
    [ProducesResponseType(typeof(CustomProgramResponseDTO), 200)]
    [ProducesResponseType(404)]
    [HttpGet("{id:int}")]
    public async Task<ActionResult> GetByIdAsync(int id)
    {
        return Ok(await _service.GetByIdAsync(id));
    }

    /// <summary>
    /// Update custom program by ID.
    /// </summary>
    /// <param name="id">Custom program ID.</param>
    /// <param name="dto">Custom program update data.</param>
    /// <response code="200">Custom program updated successfully.</response>
    /// <response code="400">Invalid update data.</response>
    /// <response code="404">Custom program not found.</response>
    [ProducesResponseType(typeof(CustomProgramResponseDTO), 200)]
    [ProducesResponseType(400)]
    [ProducesResponseType(404)]
    [HttpPut("{id:int}")]
    public async Task<ActionResult> UpdateAsync(int id, [FromBody] CustomProgramUpdateDTO dto)
    {
        return Ok(await _service.UpdateAsync(id, dto));
    }

    /// <summary>
    /// Delete custom program by ID.
    /// </summary>
    /// <param name="id">Custom program ID.</param>
    /// <response code="200">Custom program deleted successfully.</response>
    /// <response code="404">Custom program not found.</response>
    [ProducesResponseType(200)]
    [ProducesResponseType(404)]
    [HttpDelete("{id:int}")]
    public async Task<ActionResult> DeleteAsync(int id)
    {
        return Ok(await _service.DeleteAsync(id));
    }
}
