namespace FitnesTracker;

public class StandardProgramResponseDTO
{
    public int StandardProgramId { get; set; }
    public string Description { get; set; }
    public string Title { get; set; }
    public Category Category { get; set; }
    public Level Level { get; set; }
    public WorkoutType WorkoutType { get; set; }    
}
