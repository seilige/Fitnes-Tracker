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
        var users = await _context.Users.CountAsync();
        var sessions = await _context.WorkoutSessions.CountAsync();
        var sets = await _context.WorkoutExerciseSets.CountAsync();
        var stProgs = await _context.StandardPrograms.CountAsync();
        var customProgs = await _context.CustomPrograms.CountAsync();

        return Ok(new { users, sessions, sets, stProgs, customProgs });
    }

    [HttpGet("test-validation")]
    public IActionResult TestValidation()
    {
        throw new ValidationException("Invalid input data");
    }
}