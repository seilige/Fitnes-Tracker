namespace FitnesTracker;

public interface IWorkoutExerciseSetRepository : IRepository<WorkoutExerciseSet>
{
    Task<WorkoutExerciseSet> AddAsync(WorkoutExerciseSet set);
    Task<WorkoutExerciseSet?> GetByIdAsync(int id);
    Task<IEnumerable<WorkoutExerciseSet>> GetByWorkoutSessionIdAsync(int sessionId);
    Task<PagedResult<WorkoutExerciseSet>> GetAllAsync(int pageNum, int pageSize);
    Task<WorkoutExerciseSet?> GetSetByIdWithSessionAsync(int id);
    Task UpdateAsync(WorkoutExerciseSet set);
    Task DeleteAsync(int id);
}