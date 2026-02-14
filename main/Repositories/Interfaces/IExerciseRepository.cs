namespace FitnesTracker;

public interface IExerciseRepository : IRepository<Exercise>
{
    Task<Exercise?> GetByTitleAsync(string title);
    Task<IEnumerable<Exercise>> GetByMuscleGroupAsync(MuscleGroup group);
    Task<PagedResult<Exercise>> GetPagedAsync(PaginationParams paginationParams);
    Task<PagedResult<Exercise>> GetFilteredExercisesAsync(ExerciseQueryParameters parameters);
    Task<PagedResult<Exercise>> GetAllAsync(int pageNum, int pageSize);
}
