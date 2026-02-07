namespace FitnesTracker;

public class WorkoutExerciseSetCreateDTO
{
    public int WorkoutSessionId { get; set; }
    public int Sets { get; set; }
    public int Reps { get; set; }
    public int ExerciseId { get; set; }
    public double Weight { get; set; }
}

// What user gets
public class WorkoutExerciseSetResponseDTO
{
    public int Id { get; set; }
    public Exercise Exercise { get; set; }
    public int Sets { get; set; }
    public int Reps { get; set; }
    public double Weight { get; set; }
}

public class WorkoutExerciseSetUpdateDTO
{
    public int Id { get; set; }
    public int Sets { get; set; }
    public int Reps { get; set; }
    public double Weight { get; set; }
}
