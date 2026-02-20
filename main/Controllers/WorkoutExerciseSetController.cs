using Microsoft.AspNetCore.Mvc;

namespace FitnesTracker;

[ApiController]
[Route("api/[controller]")]
public class WorkoutExerciseSetController : ControllerBase
{
    private IWorkoutExerciseService _service;

    public WorkoutExerciseSetController(IWorkoutExerciseService service)
    {
        _service = service;
    }

    [HttpGet("all")]
    public async Task<ActionResult<PagedResult<UserResponseDTO>>> GetAll([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10)
    {
        var result = await _service.GetAllAsync(pageNumber, pageSize);
        return Ok(result);
    }

    [HttpPost]
    public async Task<ActionResult<WorkoutExerciseSetResponseDTO>> AddSetAsync([FromBody] WorkoutExerciseSetCreateDTO dto)
    {
        var res = await _service.AddSetAsync(dto);

        return CreatedAtAction(nameof(GetById), new { id = res.WorkoutExerciseSetId }, res);
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<WorkoutExerciseSetResponseDTO>> GetById(int id)
    {
        return Ok(await _service.GetSetByIdAsync(id));
    }

    [HttpGet("session/{sessionId:int}")]
    public async Task<ActionResult<IEnumerable<WorkoutExerciseSetResponseDTO>>> GetSeesionById(int sessionId)
    {
        return Ok(await _service.GetSessionSetsAsync(sessionId));
    }

    [HttpPut("{id:int}")]
    public async Task<ActionResult<WorkoutExerciseSetResponseDTO>> UpdateSet(int id, [FromBody] WorkoutExerciseSetUpdateDTO dto)
    {
        return Ok(await _service.UpdateSetAsync(id, dto));
    }

    [HttpDelete("{id:int}")]
    public async Task<ActionResult> DeleteSet(int id)
    {
        bool res = await _service.DeleteSetAsync(id);

        if(res == true) return Ok();

        return NoContent();
    }
}
