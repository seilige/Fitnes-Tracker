using System.Runtime.CompilerServices;
using Microsoft.AspNetCore.Mvc;

namespace FitnesTracker;

[ApiController]
[Route("api/[controller]")]
public class WorkoutSessionController : ControllerBase
{
    private IWorkoutSessionService _service;

    public WorkoutSessionController(IWorkoutSessionService service)
    {
        _service = service;
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
        return CreatedAtAction(nameof(GetSessionId), new { id = result.UserId }, result);
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
    public async Task<ActionResult> UpdateSessionStatus(int id, [FromBody] WorkoutStatus status)
    {
        return Ok(await _service.UpdateSessionStatusAsync(id, status));
    }
}
