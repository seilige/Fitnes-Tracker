using System.Runtime.CompilerServices;
using Microsoft.AspNetCore.Mvc;

namespace FitnesTracker;

[ApiController]
[Route("api/[controller]")]
public class ExerciseController : ControllerBase
{
    private IExerciseService _service;
    public ExerciseController(IExerciseService service)
    {
        _service = service;
    }

    [HttpGet("filter")]
    public async Task<ActionResult<PagedResult<ExerciseResponseDTO>>> GetFilteredExercises([FromQuery] ExerciseQueryParameters parameters)
    {
        var result = await _service.GetPagedAsync(parameters);
        return Ok(result);
    }

    [HttpGet("muscle_group/{group}")]
    public async Task<ActionResult> GetByMuscleGroupAsync(MuscleGroup group)
    {
        return Ok(await _service.GetByMuscleGroupAsync(group));
    }

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

    [HttpGet]
    public async Task<ActionResult> GetAllAsync()
    {
        return Ok(await _service.GetAllAsync());
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
    public async Task<ActionResult> CreateAsync([FromBody] ExerciseRequestDTO dto)
    {
        return Ok(await _service.CreateAsync(dto));
    }
}
