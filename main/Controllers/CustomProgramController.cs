using Microsoft.AspNetCore.Mvc;

namespace FitnesTracker;

[ApiController]
[Route("api/[controller]")]
public class CustomProgramController : ControllerBase
{
    private ICustomProgramService _service;

    public CustomProgramController(ICustomProgramService service)
    {
        _service = service;
    }

    [HttpGet]
    public async Task<ActionResult> GetAllAsync()
    {
        return Ok(await _service.GetAllAsync());
    }

    [HttpPost("{creatorId}")]
    public async Task<ActionResult> CreateAsync([FromBody] CustomProgramCreateDTO dto, [FromRoute] int creatorId)
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
