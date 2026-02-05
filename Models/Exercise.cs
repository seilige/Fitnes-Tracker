namespace FitnesTracker;

public class Exercise
{
    public int ExId { get; set; }
    public string? Title { get; set; }
    public int? Sets { get; set; }
    public int? Reps { get; set; }
    public MuscleGroup MuscleGroup { get; set; }
}
