using System.Diagnostics.Contracts;
using System.Runtime.CompilerServices;
using Microsoft.AspNetCore.Mvc;

namespace FitnesTracker;

[ApiController]
[Route("api/[controller]")]
public class StandardProgramController : ControllerBase
{
    private IStandardProgramService _service;

    public StandardProgramController(IStandardProgramService service)
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
}
