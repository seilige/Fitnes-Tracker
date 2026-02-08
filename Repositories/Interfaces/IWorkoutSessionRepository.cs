namespace FitnesTracker;

public interface IWorkoutSessionRepository : IRepository<WorkoutSession>
{
    Task<IEnumerable<WorkoutSession>> GetByUserIdAsync(int userId);
    Task<WorkoutSession?> GetByIdWithDetailsAsync(int id);
    Task<PagedResult<WorkoutSession>> GetUserSessionsAsync(int userId, int pageNumber, int pageSize);
}
