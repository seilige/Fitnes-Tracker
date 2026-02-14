namespace FitnesTracker;

public interface ICustomProgramRepository : IRepository<CustomProgram>
{
    Task<User?> GetCreatorAsync(int custProgId);
    Task<IEnumerable<Exercise>> GetExercisesAsync(int custProgId);
    Task<IEnumerable<CustomProgram>> GetByCreatorIdAsync(int creatorId); // All users programs
    Task<PagedResult<User>> GetAllAsync(int pageNum, int pageSize);
    Task<IEnumerable<CustomProgram>> GetPublicProgramsAsync();
}
