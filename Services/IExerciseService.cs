namespace FitnesTracker;

public interface IExerciseService
{
    Task<ExerciseResponseDTO?> GetByTitleAsync(string title);
    Task<IEnumerable<ExerciseResponseDTO>> GetByMuscleGroupAsync(MuscleGroup group);

    Task<IEnumerable<ExerciseResponseDTO>> GetAllAsync();
    Task<ExerciseResponseDTO> CreateAsync(ExerciseRequestDTO dto);
    Task<ExerciseResponseDTO> UpdateAsync(int id, ExerciseRequestDTO dto);
    Task<bool> DeleteAsync(int id);
    Task<ExerciseResponseDTO?> GetByIdAsync(int id);
}
