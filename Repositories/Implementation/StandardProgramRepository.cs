using Microsoft.EntityFrameworkCore;

namespace FitnesTracker;

public class StandardProgramRepository : Repository<StandardProgram>, IStandardProgramRepository
{
    public StandardProgramRepository(ApplicationDbContext context) : base(context) 
    {
    }

    public async Task<IEnumerable<StandardProgram>> GetByLevelAsync(Level level)
    {
        return await _context.StandardPrograms.Where(x => x.Level == level).ToListAsync();
    }

    public async Task<IEnumerable<StandardProgram>> GetByWorkoutTypeAsync(WorkoutType workoutType)
    {
        return await _context.StandardPrograms.Where(x => x.WorkoutType == workoutType).ToListAsync();
    }

    public async Task<IEnumerable<StandardProgram>> GetByCategoryAsync(Category category)
    {
        return await _context.StandardPrograms.Where(x => x.Category == category).ToListAsync();
    }
}
