namespace FitnesTracker;

public class WorkoutSessionCreateDTO
{
    public int UserId { get; set; }
    public DateTime Date { get; set; }
    public int? StandardProgramId { get; set; }
    public int? CustomProgramId { get; set; }  
    public ICollection<WorkoutExerciseSet> WorkoutExerciseSets { get; set; }
}

public class WorkoutSessionResponseDTO
{
    public int UserId { get; set; }
    public WorkoutStatus Status { get; set; }
    public int SessionId { get; set; }
    public DateTime Date { get; set; } // when was session carried out
    // public ICollection<WorkoutExerciseSet> WorkoutExerciseSets { get; set; }
    public ICollection<ExerciseSetDTO> ExerciseSets { get; set; }
}

// What user must spicify, when he would to update workout session:
public class WorkoutSessionUpdateDTO // update all session
{
    public WorkoutStatus Status { get; set; }
    public int SessionId { get; set; }
}

public class SetUpdateDTO // update data in session
{
    public int Id { get; set; }
    public double Weight { get; set; }
    public int Reps { get; set; }
}
