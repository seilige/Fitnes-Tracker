using Microsoft.AspNetCore.Mvc;
using FluentValidation;
using Microsoft.EntityFrameworkCore;

namespace FitnesTracker;

[ApiController]
[Route("api/[controller]")]
public class ErorController : ControllerBase
{
    private ApplicationDbContext _context;

    public ErorController(ApplicationDbContext context)
    {
        _context = context;
    }

    [HttpGet("debug/check-db")]
    public async Task<ActionResult> CheckDb()
    {
        var usersCount = await _context.Users.CountAsync();
        var sessionsCount = await _context.WorkoutSessions.CountAsync();
        var setsCount = await _context.WorkoutExerciseSets.CountAsync();
        
        return Ok(new { usersCount, sessionsCount, setsCount });
    }

    [HttpGet("test-notfound")]
    public IActionResult TestNotFound()
    {
        throw new KeyNotFoundException("User not found");
    }

    [HttpGet("test-validation")]
    public IActionResult TestValidation()
    {
        throw new ValidationException("Invalid input data");
    }

    [HttpGet("test-server-error")]
    public IActionResult TestServerError()
    {
        throw new Exception("Something went wrong");
    }
}