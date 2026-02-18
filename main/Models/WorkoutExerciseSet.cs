namespace FitnesTracker;

public class WorkoutExerciseSet
{
    public int WorkoutExerciseSetId { get; set; }
    public int ExerciseId { get; set; }
    public int Sets { get; set; }
    public int Reps { get; set; }
    public double Weight { get; set; }
    public int WorkoutSessionId { get; set; } // Link to workout session
    public Exercise Exercise { get; set; }
    public WorkoutSession WorkoutSession { get; set; }
}
