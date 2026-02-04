namespace FitnesTracker;

public interface IStandardProgramRepository : IRepository<StandardProgram>
{
    Task<IEnumerable<StandardProgram>> GetByLevelAsync(Level level);
    Task<IEnumerable<StandardProgram>> GetByWorkoutTypeAsync(WorkoutType workoutType);
    Task<IEnumerable<StandardProgram>> GetByCategoryAsync(Category category);
}
