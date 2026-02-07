using Microsoft.EntityFrameworkCore;

namespace FitnesTracker;

public interface IWorkoutExerciseSetRepository : IRepository<WorkoutExerciseSet>
{
    Task<IEnumerable<WorkoutExerciseSet>> GetByWorkoutSessionIdAsync(int sessionId);
    Task<WorkoutExerciseSet> AddAsync(WorkoutExerciseSet set); // overrided method, because in Repository it returnd void(Task)
}

