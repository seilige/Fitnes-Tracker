namespace FitnesTracker;

public class CustomProgramCreateDTO
{
    public string? Title { get; set; }
    public string? Description { get; set; }
    public bool IsPublic { get; set; }
    public ICollection<int> ExerciseIDs { get; set; }
}

public class CustomProgramResponseDTO
{
    public int CustProgId { get; set; }
    public string? Title { get; set; }
    public string? Description { get; set; }
    public bool IsPublic { get; set; }
    public ICollection<int> ExerciseIDs { get; set; }
    public int? CreatorId { get; set; }
}

public class CustomProgramUpdateDTO
{
    public string? Title { get; set; }
    public string? Description { get; set; }
    public bool IsPublic { get; set; }
    public ICollection<int> ExerciseIDs { get; set; }
}

