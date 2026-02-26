using Microsoft.AspNetCore.Mvc;

namespace FitnesTracker;

[ApiController]
[Produces("application/json")]
[Route("api/[controller]")]
public class StandardProgramController : ControllerBase
{
    private IStandardProgramService _service;

    public StandardProgramController(IStandardProgramService service)
    {
        _service = service;
    }

    
    [HttpGet("all")]
    public async Task<ActionResult<PagedResult<UserResponseDTO>>> GetAll([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10)
    {
        var result = await _service.GetAllAsync(pageNumber, pageSize);
        return Ok(result);
    }

    [HttpGet("paged")]
    public async Task<ActionResult<PagedResult<StandardProgramResponseDTO>>> GetPaged([FromQuery] PaginationParams paginationParams)
    {
        var result = await _service.GetPagedAsync(paginationParams);
        return Ok(result);
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult> GetByIdAsync(int id)
    {
        return Ok(await _service.GetByIdAsync(id));
    }
}
