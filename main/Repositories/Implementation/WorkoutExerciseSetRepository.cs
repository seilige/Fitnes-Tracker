using Microsoft.EntityFrameworkCore;

namespace FitnesTracker;

public class WorkoutExerciseSetRepository : Repository<WorkoutExerciseSet>, IWorkoutExerciseSetRepository
{
    public WorkoutExerciseSetRepository(ApplicationDbContext context) : base(context)
    {
    }

    public async Task<PagedResult<WorkoutExerciseSet>> GetAllAsync(int pageNum, int pageSize)
    {
        var workoutExercises = _context.WorkoutExerciseSets
            .AsNoTracking()
            .Include(x => x.Exercise)
            .Include(x => x.WorkoutSession);

        var count = await workoutExercises.CountAsync();
        var exercises = await workoutExercises.OrderBy(x => x.ExerciseId).Skip((pageNum - 1) * pageSize).Take(pageSize).ToListAsync();

        return new PagedResult<WorkoutExerciseSet>(
            items: exercises,
            totalCount: count,
            pageNumber: pageNum,
            pageSize: pageSize
        );
    }
    public async Task<WorkoutExerciseSet?> GetSetByIdWithSessionAsync(int id)
    {
        return await _context.WorkoutExerciseSets
        .AsNoTracking()
                    .Include(x => x.WorkoutSession)
                    .FirstAsync(x => x.WorkoutExerciseSetId == id);
    }
    public new async Task<WorkoutExerciseSet> AddAsync(WorkoutExerciseSet set)
    {
        await _context.WorkoutExerciseSets.AddAsync(set);
        return set;
    }

    public async Task<WorkoutExerciseSet?> GetByIdAsync(int id)
    {
        return await _context.WorkoutExerciseSets.AsNoTracking().FirstOrDefaultAsync(x => x.WorkoutExerciseSetId == id);
    }

    public async Task<IEnumerable<WorkoutExerciseSet>> GetByWorkoutSessionIdAsync(int sessionId)
    {
        return await _context.WorkoutExerciseSets.AsNoTracking().Where(x => x.WorkoutSessionId == sessionId).ToListAsync(); // cuz method returns IEnumerable
    }

    public async Task UpdateAsync(WorkoutExerciseSet set)
    {
        _context.WorkoutExerciseSets.Update(set);
    }

    public async Task DeleteAsync(int id)
    {
        var item = await _context.WorkoutExerciseSets.FirstOrDefaultAsync(x => x.WorkoutExerciseSetId == id);
        _context.WorkoutExerciseSets.Remove(item);
    }
}

