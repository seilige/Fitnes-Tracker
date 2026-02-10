namespace FitnesTracker;

// Large number of this class = mtm between user and custProg
public class UserStandardProgram
{
    public User User { get; set; }
    public StandardProgram StandardProgram { get; set; }
    public int IdUser { get; set; }
    public int ProgId { get; set; }
}

public class CustomProgramUser
{
    public User User { get; set; }
    public CustomProgram CustomProgram { get; set; }
    public int IdUser { get; set; }
    public int CustProgId { get; set; }
}

public class StandardProgramExercise
{
    public StandardProgram StandardProgram { get; set; }
    public Exercise Exercise { get; set; }
    public int ProgId { get; set; }
    public int ExId { get; set; }
}

public class CustomProgramExercise
{
    public CustomProgram CustomProgram { get; set; }
    public Exercise Exercise { get; set; }
    public int CustProgId { get; set; }
    public int ExId { get; set; }
}
