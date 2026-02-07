using Microsoft.EntityFrameworkCore;

namespace FitnesTracker;

public class WorkoutSessionRepository :  Repository<WorkoutSession>, IWorkoutSessionRepository
{
    public WorkoutSessionRepository(ApplicationDbContext context) : base(context)
    {
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
