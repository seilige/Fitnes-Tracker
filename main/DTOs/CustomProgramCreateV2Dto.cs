namespace FitnesTracker;

public class CustomProgramCreateV2DTO
{
    public string? Title { get; set; }
    public string? Description { get; set; }
    public bool IsPublic { get; set; }
    public ICollection<int> ExerciseIDs { get; set; }
    public bool IsEditable { get; set; }
}
