namespace FitnesTracker;

public interface IWorkoutExerciseSetRepository : IRepository<WorkoutExerciseSet>
{
    Task<WorkoutExerciseSet> AddAsync(WorkoutExerciseSet set);
    Task<WorkoutExerciseSet?> GetByIdAsync(int id);
    Task<IEnumerable<WorkoutExerciseSet>> GetByWorkoutSessionIdAsync(int sessionId); // изменил название
    Task<WorkoutExerciseSet?> GetSetByIdWithSessionAsync(int id); // добавил этот
    Task UpdateAsync(WorkoutExerciseSet set);
    Task DeleteAsync(int id);
}