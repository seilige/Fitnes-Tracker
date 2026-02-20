namespace FitnesTracker;

public class ExerciseResponseDTO
{
    public int ExerciseId { get; set; }
    public string? Title { get; set; }
    public int? Sets { get; set; }
    public int? Reps { get; set; }
    public MuscleGroup MuscleGroup { get; set; }
}

public class ExerciseRequestDTO
{
    public string? Title { get; set; }
    public int? Sets { get; set; }
    public int? Reps { get; set; }
    public MuscleGroup MuscleGroup { get; set; }
}
