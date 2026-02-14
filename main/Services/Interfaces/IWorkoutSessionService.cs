namespace FitnesTracker;

public interface IWorkoutSessionService
{
    Task<WorkoutSessionResponseDTO> CreateSessionAsync(WorkoutSessionCreateDTO dto);
    Task<WorkoutSessionResponseDTO> GetSessionByIdAsync(int id);
    Task<IEnumerable<WorkoutSessionResponseDTO>> GetUserSessionsAsync(int userId);
    Task<WorkoutSessionResponseDTO> UpdateSessionStatusAsync(int id, WorkoutStatus status);
    Task<PagedResult<WorkoutSessionResponseDTO>> GetUserHistoryAsync(int userId, int pageNumber, int pageSize);
    Task<WorkoutSessionResponseDTO> CompleteSessionAsync(int sessionId);
    Task<SetUpdateDTO> UpdateSetAsync(SetUpdateDTO dto);
    Task<PagedResult<WorkoutSessionResponseDTO>> GetAllAsync(int pageNumber, int pageSize);
}
