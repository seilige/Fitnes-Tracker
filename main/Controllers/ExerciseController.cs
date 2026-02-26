using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

namespace FitnesTracker;

[ApiController]
[Route("api/[controller]")]
[Produces("application/json")]
[Authorize]
public class ExerciseController : ControllerBase
{
    private IExerciseService _service;
    public ExerciseController(IExerciseService service)
    {
        _service = service;
    }

    [AllowAnonymous]
    [HttpGet("all")]
    public async Task<ActionResult<PagedResult<UserResponseDTO>>> GetAll([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10)
    {
        var result = await _service.GetAllAsync(pageNumber, pageSize);
        return Ok(result);
    }

    [AllowAnonymous]
    [HttpGet("filter")]
    public async Task<ActionResult<PagedResult<ExerciseResponseDTO>>> GetFilteredExercises([FromQuery] ExerciseQueryParameters parameters)
    {
        var result = await _service.GetPagedAsync(parameters);
        return Ok(result);
    }

    [AllowAnonymous]
    [HttpGet("muscle_group/{group}")]
    public async Task<ActionResult> GetByMuscleGroupAsync(MuscleGroup group)
    {
        return Ok(await _service.GetByMuscleGroupAsync(group));
    }

    [AllowAnonymous]
    [HttpGet("{title:alpha}")]
    public async Task<ActionResult> GetExerciseAsync(string title)
    {
        return Ok(await _service.GetByTitleAsync(title));
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult> GetByIdAsync(int id)
    {
        return Ok(await _service.GetByIdAsync(id));
    }

    [HttpPut("{id:int}")]
    public async Task<ActionResult> UpdateAsync(int id, [FromBody] ExerciseRequestDTO dto)
    {
        return Ok(await _service.UpdateAsync(id, dto));
    }

    [HttpDelete("{id:int}")]
    public async Task<ActionResult> DeleteAsync(int id)
    {
        return Ok(await _service.DeleteAsync(id));
    }

    [HttpPost]
    [Authorize(Roles = "Author")]
    public async Task<ActionResult> CreateAsync([FromBody] ExerciseRequestDTO dto)
    {
        var result = await _service.CreateAsync(dto);
        return CreatedAtAction(nameof(GetByIdAsync), new { id = result.ExerciseId }, result);
    }
}
