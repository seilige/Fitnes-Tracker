namespace FitnesTracker;

public class WorkoutExerciseSetCreateDTO
{
    public int WorkoutExerciseSetId { get; set; }
    public int Reps { get; set; }
    public double Weight { get; set; }
    public int WorkoutSessionId { get; set; }
}

// What user gets
public class WorkoutExerciseSetResponseDTO
{
    public int WorkoutExerciseSetId { get; set; }
    public Exercise Exercise { get; set; }
    public int Sets { get; set; }
    public int Reps { get; set; }
    public double Weight { get; set; }
}

public class WorkoutExerciseSetUpdateDTO
{
    public int WorkoutExerciseSetId { get; set; }
    public int Reps { get; set; }
    public double Weight { get; set; }
}

public class ExerciseSetDTO
{
    public int ExerciseSetId { get; set; }
    public string ExerciseName { get; set; }
    public int Reps { get; set; }
    public decimal Weight { get; set; }
}
