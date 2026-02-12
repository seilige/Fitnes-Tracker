using Microsoft.EntityFrameworkCore;

namespace FitnesTracker;

public class WorkoutExerciseSetRepository : Repository<WorkoutExerciseSet>, IWorkoutExerciseSetRepository
{
    public WorkoutExerciseSetRepository(ApplicationDbContext context) : base(context)
    {
    }

    public async Task<ICollection<WorkoutExerciseSet>> GetAllAsync()
    {
        return await _context.WorkoutExerciseSets
            .Include(x => x.Exercise)
            .Include(x => x.WorkoutSession)
            .ToListAsync();
    }
    public async Task<WorkoutExerciseSet?> GetSetByIdWithSessionAsync(int id)
    {
        return await _context.WorkoutExerciseSets
                    .Include(x => x.WorkoutSession)
                    .FirstAsync(x => x.Id == id);
    }
    public new async Task<WorkoutExerciseSet> AddAsync(WorkoutExerciseSet set)
    {
        await _context.WorkoutExerciseSets.AddAsync(set);
        // await _context.SaveChangesAsync();
        return set;
    }

    public async Task<WorkoutExerciseSet?> GetByIdAsync(int id)
    {
        return await _context.WorkoutExerciseSets.FirstOrDefaultAsync(x => x.Id == id);
    }

    public async Task<IEnumerable<WorkoutExerciseSet>> GetByWorkoutSessionIdAsync(int sessionId)
    {
        return await _context.WorkoutExerciseSets.Where(x => x.WorkoutSessionId == sessionId).ToListAsync(); // cuz method returns IEnumerable
    }

    public async Task UpdateAsync(WorkoutExerciseSet set)
    {
        _context.WorkoutExerciseSets.Update(set);
    }

    public async Task DeleteAsync(int id)
    {
        var item = await _context.WorkoutExerciseSets.FirstOrDefaultAsync(x => x.Id == id);
        _context.WorkoutExerciseSets.Remove(item);
    }
}

