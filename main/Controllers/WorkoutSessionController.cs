using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

namespace FitnesTracker;

[ApiController]
[Route("api/[controller]")]
[Produces("application/json")]
public class WorkoutSessionController : ControllerBase
{
    private IWorkoutSessionService _service;

    public WorkoutSessionController(IWorkoutSessionService service)
    {
        _service = service;
    }

    [Authorize]
    [HttpGet("all")]
    public async Task<ActionResult<PagedResult<UserResponseDTO>>> GetAll([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10)
    {
        var result = await _service.GetAllAsync(pageNumber, pageSize);
        return Ok(result);
    }

    [HttpPatch("session/{sessionId}/complete")]
    public async Task<ActionResult<WorkoutSessionResponseDTO>> CompleteSession(int sessionId)
    {
        var result = await _service.CompleteSessionAsync(sessionId);

        if(result == null) return NoContent();

        return Ok(result);
    }

    [HttpPatch("set/update")]
    public async Task<ActionResult<SetUpdateDTO>> UpdateSet(SetUpdateDTO dto)
    {
        var result = await _service.UpdateSetAsync(dto);

        if(result == null) return NoContent();

        return Ok(result);
    }

    [HttpGet("history/{userId}")]
    public async Task<ActionResult<PagedResult<WorkoutSessionResponseDTO>>> GetUserHistory(int userId, [FromQuery] PaginationParams paginationParams)
    {
        var history = await _service.GetUserHistoryAsync(userId, paginationParams.PageNumber, paginationParams.PageSize);
        return Ok(history);
    }

    [HttpPost("session_create")]
    public async Task<ActionResult> SessionCreate(WorkoutSessionCreateDTO dto)
    {
        var result = await _service.CreateSessionAsync(dto);
        return CreatedAtAction(nameof(GetSessionId), new { id = result.SessionId }, result);
    }

    [HttpGet("session/{id}")]
    public async Task<ActionResult> GetSessionId(int id)
    {
        return Ok(await _service.GetSessionByIdAsync(id));
    }

    [HttpGet("user/{userId}/sessions")]
    public async Task<ActionResult> GetUserSessions(int userId)
    {
        return Ok(await _service.GetUserSessionsAsync(userId));
    }

    [HttpPatch("session/{id}/status")]
    public async Task<ActionResult> UpdateSessionStatus(int id, [FromBody] UpdateStatusDTO status)
    {
        return Ok(await _service.UpdateSessionStatusAsync(id, status));
    }
}
