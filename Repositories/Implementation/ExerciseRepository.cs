using Microsoft.EntityFrameworkCore;

namespace FitnesTracker;

public class ExerciseRepository : Repository<Exercise>, IExerciseRepository
{
    public ExerciseRepository(ApplicationDbContext context) : base(context)
    {
    }

    public async Task<Exercise?> GetByTitleAsync(string title)
    {
        return await _context.Exercises.Where(x => x.Title == title).FirstOrDefaultAsync();
    }

    public async Task<IEnumerable<Exercise>> GetByMuscleGroupAsync(MuscleGroup group)
    {
        return await _context.Exercises.Where(x => x.MuscleGroup == group).ToListAsync();
    }
}
