namespace FitnesTracker;

public interface IExerciseService
{
    Task<ExerciseResponseDTO?> GetByTitleAsync(string title);
    Task<IEnumerable<ExerciseResponseDTO>> GetByMuscleGroupAsync(MuscleGroup group);
    Task<PagedResult<ExerciseResponseDTO>> GetAllAsync(int pageNumber, int pageSize);
    Task<ExerciseResponseDTO> CreateAsync(ExerciseRequestDTO dto);
    Task<ExerciseResponseDTO> UpdateAsync(int id, ExerciseRequestDTO dto);
    Task<bool> DeleteAsync(int id);
    Task<ExerciseResponseDTO?> GetByIdAsync(int id);
    Task<PagedResult<ExerciseResponseDTO>> GetPagedAsync(PaginationParams paginationParams);
}
