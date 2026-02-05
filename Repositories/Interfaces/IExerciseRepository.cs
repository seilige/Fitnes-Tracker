namespace FitnesTracker;

public interface IExerciseRepository : IRepository<Exercise>
{
    Task<Exercise?> GetByTitleAsync(string title);
    Task<IEnumerable<Exercise>> GetByMuscleGroupAsync(MuscleGroup group);
}
