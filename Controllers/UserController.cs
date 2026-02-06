using System.ComponentModel.Design.Serialization;
using System.Data.Common;
using System.Runtime.Versioning;
using Microsoft.AspNetCore.Mvc;

namespace FitnesTracker;

[ApiController]
[Route("api/[controller]")]
public class UserController : ControllerBase
{
    private IUserService _service;

    public UserController(UserService service)
    {
        _service = service;
    }

    [HttpGet]
    public async Task<ActionResult> GetAllAsync()
    {
        return Ok(await _service.GetAllAsync());
    }

    // обработка ошибок будет добавлена позже
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
        return Ok(await _service.CreateAsync(dto));
    }
}
