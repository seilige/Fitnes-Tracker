using Microsoft.AspNetCore.Mvc;

namespace FitnesTracker;

[ApiController]
[Route("api/v2/customprograms")] // if its be api/v2/[controller] user recives - api/v2/CustomProgramV2
public class CustomProgramV2Controller : ControllerBase
{
    private ICustomProgramService _service;

    public CustomProgramV2Controller(ICustomProgramService service)
    {
        _service = service;
    }
    
    [HttpGet("all")]
    public async Task<ActionResult<PagedResult<UserResponseDTO>>> GetAll([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10)
    {
        var result = await _service.GetAllAsync(pageNumber, pageSize);
        return Ok(result);
    }

    [HttpPost("{creatorId}")]
    public async Task<ActionResult> CreateAsync([FromBody] CustomProgramCreateV2DTO dto, [FromRoute] int creatorId)
    {
        var result = await _service.CreateAsync(dto, creatorId);
        return CreatedAtAction(nameof(GetByIdAsync), new { id = result.CustProgId }, result);
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult> GetByIdAsync(int id)
    {
        return Ok(await _service.GetByIdAsync(id));
    }

    [HttpPut("{id:int}")]
    public async Task<ActionResult> UpdateAsync(int id, [FromBody] CustomProgramUpdateDTO dto)
    {
        return Ok(await _service.UpdateAsync(id, dto));
    }

    [HttpDelete("{id:int}")]
    public async Task<ActionResult> DeleteAsync(int id)
    {
        return Ok(await _service.DeleteAsync(id));
    }
}
