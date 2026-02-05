namespace FitnesTracker;

public class StandardProgram
{
    public int ProgId { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    public Category Category { get; set; }
    public Level Level { get; set; }
    public WorkoutType WorkoutType { get; set; }
    public ICollection<Exercise> Exercises { get; set; }
}

public class CustomProgram
{
    public int CustProgId { get; set; }
    public int? CreatorId { get; set; } // Link to user
    public bool IsPublic { get; set; }
    public string? Title { get; set; }
    public string? Description { get; set; }
    public ICollection<Exercise> Exercises { get; set; }
    public User? Creator { get; set; }
}
