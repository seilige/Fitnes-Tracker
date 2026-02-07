using Microsoft.EntityFrameworkCore;

namespace FitnesTracker;

public class WorkoutExerciseRepository : Repository<WorkoutSession>, IWorkoutExerciseRepository
{
    public WorkoutExerciseRepository(ApplicationDbContext context) : base(context)
    {
    }

    public new async Task<WorkoutExerciseSet> AddAsync(WorkoutExerciseSet set)
    {
        await _context.WorkoutExerciseSets.AddAsync(set);
        await _context.SaveChangesAsync();
        return set;
    }

    public async Task<WorkoutExerciseSet?> GetByIdAsync(int id)
    {
        return await _context.WorkoutExerciseSets.FirstOrDefaultAsync(x => x.Id == id);
    }

    public async Task<IEnumerable<WorkoutExerciseSet>> GetBySessionIdAsync(int sessionId)
    {
        return await _context.WorkoutExerciseSets.Where(x => x.WorkoutSessionId == sessionId).ToListAsync(); // cuz method returns IEnumerable
    }

    public async Task UpdateAsync(WorkoutExerciseSet set)
    {
        _context.WorkoutExerciseSets.Update(set);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(int id)
    {
        var item = await _context.WorkoutExerciseSets.FirstOrDefaultAsync(x => x.Id == id);
        _context.WorkoutExerciseSets.Remove(item);
        await _context.SaveChangesAsync();
    }
}

