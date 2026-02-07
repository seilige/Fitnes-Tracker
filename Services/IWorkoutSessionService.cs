namespace FitnesTracker;

public interface IWorkoutSessionService
{
    Task<WorkoutSessionResponseDTO> CreateSessionAsync(WorkoutSessionCreateDTO dto);
    Task<WorkoutSessionResponseDTO> GetSessionByIdAsync(int id);
    Task<IEnumerable<WorkoutSessionResponseDTO>> GetUserSessionsAsync(int userId);
    Task<WorkoutSessionResponseDTO> UpdateSessionStatusAsync(int id, WorkoutStatus status);
}
