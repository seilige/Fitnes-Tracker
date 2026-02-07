namespace FitnesTracker;

public class WorkoutSessionCreateDTO
{
    public int UserId { get; set; }
    public DateTime Date { get; set; }
    public int? StandardProgramId { get; set; }
    public int? CustomProgramId { get; set; }  
}

public class WorkoutSessionResponseDTO
{
    public int UserId { get; set; }
    public WorkoutStatus Status { get; set; }
    public int SessionId { get; set; }
    public DateTime Date { get; set; }
    public ICollection<WorkoutExerciseSet> WorkoutExerciseSets { get; set; }
}

// What user must spicify, when he would to update workout session:
public class WorkoutSessionUpdateDTO
{
    public WorkoutStatus Status { get; set; }
    public int SessionId { get; set; }
}
