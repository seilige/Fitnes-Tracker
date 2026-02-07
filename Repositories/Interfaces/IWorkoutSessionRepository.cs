namespace FitnesTracker;

public interface IWorkoutSessionRepository : IRepository<WorkoutSession>
{
    Task<IEnumerable<WorkoutSession>> GetByUserIdAsync(int userId);
    Task<WorkoutSession?> GetByIdWithDetailsAsync(int id);
}
