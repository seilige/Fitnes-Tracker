using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace FitnesTracker;

public class WorkoutSessionRepository :  Repository<WorkoutSession>, IWorkoutSessionRepository
{
    public WorkoutSessionRepository(ApplicationDbContext context) : base(context)
    {
    }

    public async Task<PagedResult<WorkoutSession>> GetAllAsync(int pageNum, int pageSize)
    {
        var workoutSessions = _context.WorkoutSessions.Include(x => x.WorkoutExerciseSets);

        var count = await workoutSessions.CountAsync();
        var sessions = await workoutSessions.Skip((pageNum - 1) * pageSize).Take(pageSize).ToListAsync();

        return new PagedResult<WorkoutSession>(
            items: sessions,
            totalCount: count,
            pageNumber: pageNum,
            pageSize: pageSize
        );
    }

    public async Task<PagedResult<WorkoutSession>> GetUserSessionsAsync(int userId, int pageNumber, int pageSize)
    {
        var query = _context.WorkoutSessions
            .Include(x => x.WorkoutExerciseSets)
            .ThenInclude(s => s.Exercise)
            .Where(x => x.UserId == userId);
        
        var totalCount = await query.CountAsync();
        
        var sessions = await query
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();
        
        return new PagedResult<WorkoutSession>(
            items: sessions,
            totalCount: totalCount,
            pageNumber: pageNumber,
            pageSize: pageSize
        );
    }

    public async Task<IEnumerable<WorkoutSession>> GetByUserIdAsync(int userId)
    {
        return await _context.WorkoutSessions
            .Where(x => x.UserId == userId)
            .ToListAsync();
    }

    public async Task<WorkoutSession?> GetByIdWithDetailsAsync(int id)
    {
        return await _context.WorkoutSessions
            .Include(x => x.WorkoutExerciseSets)
                .ThenInclude(set => set.Exercise)
            .FirstOrDefaultAsync(x => x.SessionId == id);
    }
}
