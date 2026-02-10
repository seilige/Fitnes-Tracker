namespace FitnesTracker;

public class ExerciseQueryParameters : PaginationParams
{
    public MuscleGroup? MuscleGroup { get; set; }
    public string? Title { get; set; }
}
