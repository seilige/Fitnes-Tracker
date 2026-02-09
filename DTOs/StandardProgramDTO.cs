namespace FitnesTracker;

public class StandardProgramResponseDTO
{
    public int ProgId { get; set; }
    public string Description { get; set; }
    public string Title { get; set; }
    public Category Category { get; set; }
    public Level Level { get; set; }
    public WorkoutType WorkoutType { get; set; }
    // N + 1 guaranteed + the entire object graph is loaded
    // public ICollection<Exercise> Exercises { get; set; }
    
}
