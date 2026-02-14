namespace FitnesTracker;

public interface IWorkoutExerciseService
{
    Task<WorkoutExerciseSetResponseDTO> AddSetAsync(WorkoutExerciseSetCreateDTO dto);
    Task<WorkoutExerciseSetResponseDTO> GetSetByIdAsync(int id);
    Task<IEnumerable<WorkoutExerciseSetResponseDTO>> GetSessionSetsAsync(int sessionId);
    Task<WorkoutExerciseSetResponseDTO> UpdateSetAsync(int id, WorkoutExerciseSetUpdateDTO dto);
    Task<bool> DeleteSetAsync(int id);
    Task<PagedResult<WorkoutExerciseSetResponseDTO>> GetAllAsync(int pageNumber, int pageSize);
}
