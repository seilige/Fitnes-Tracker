namespace FitnesTracker;

public interface IWorkoutExerciseRepository
{
    Task<WorkoutExerciseSet> AddAsync(WorkoutExerciseSet set);
    Task<WorkoutExerciseSet?> GetByIdAsync(int id);
    Task<IEnumerable<WorkoutExerciseSet>> GetBySessionIdAsync(int sessionId);
    Task UpdateAsync(WorkoutExerciseSet set);
    Task DeleteAsync(int id);
}
