namespace FitnesTracker;

// EF can't work directly with interfaces, so we will make another model
public class WorkoutSession
{
    public int SessionId { get; set; }
    public DateTime Date { get; set; }
    public int UserId { get; set; }
    public User User { get; set; }
    public int? StandardProgramId { get; set; }
    public int? CustomProgramId { get; set; }
    public StandardProgram? StandardProgram { get; set; }
    public CustomProgram? CustomProgram { get; set; }
    public WorkoutStatus Status { get; set; }
    public ICollection<WorkoutExerciseSet> WorkoutExerciseSets { get; set; }
}
