using Microsoft.AspNetCore.Mvc;

namespace FitnesTracker;

[ApiController]
[Route("api/[controller]")]
public class UserController : ControllerBase
{
    private IUserService _service;

    public UserController(IUserService service)
    {
        _service = service;
    }

    [HttpGet]
    public async Task<ActionResult> GetAllAsync()
    {
        return Ok(await _service.GetAllAsync());
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult> GetByIdAsync(int id)
    {
        return Ok(await _service.GetByIdAsync(id));
    }

    [HttpDelete("{id:int}")]
    public async Task<ActionResult> DeleteAsync(int id)
    {
        return Ok(await _service.DeleteAsync(id));
    }

    [HttpPut("{id:int}")]
    public async Task<ActionResult> UpdateAsync(int id, [FromBody] UserUpdateDTO dto)
    {
        return Ok(await _service.UpdateAsync(id, dto));
    }

    [HttpPost]
    public async Task<ActionResult> CreateAsync([FromBody] UserCreateDTO dto)
    {
        var res = await _service.CreateAsync(dto);

        return CreatedAtAction(nameof(GetByIdAsync), new { id = res.IdUser }, res);
    }
}
