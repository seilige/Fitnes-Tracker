namespace FitnesTracker;

public enum Category { Bodybuilding, WeightLoss, Health }
public enum Level { Beginner, Intermediate, Advanced }
public enum Goal { Mass, Cut, Strength }
public enum WorkoutType { Circuit, Strength, Cardio }
public enum Split { FullBody, UpperLower, PPL, BroSplit }
public enum MuscleGroup { Chest, Back, Legs }

public class User
{
    public int IdUser { get; set; }
    public string? Name { get; set; }
    public string? Lastname { get; set; }
    public bool Author = false; // Can user create an offical traning program
}

public class StandardProgram
{
    public int ProgId { get; set; }
    public string? Title { get; set; }
    public string? Descripltion { get; set; }
    public Category Category { get; set; }
    public Level Level { get; set; }
    public WorkoutType WorkoutType { get; set; }
}

public class CustomProgram
{
    public int CustProgId { get; set; }
    public int? CreatorId { get; set; } // Link to user
    public bool IsPublic { get; set; }
    public string? Descripltion { get; set; }
    public ICollection<Exercise> Exercises { get; set; }
}

public class Exercise
{
    public int ExId { get; set; }
    public string? Title { get; set; }
    public int? Sets { get; set; }
    public int? Reps { get; set; }
    public MuscleGroup MuscleGroup { get; set; }
}

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
