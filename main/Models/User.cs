namespace FitnesTracker;

public class User
{
    public int UserId { get; set; }
    public string? Name { get; set; }
    public string? Lastname { get; set; }
    public string? Email { get; set; }
    public bool Author { get; set; } = false; // Can user create an offical traning program
    public ICollection<WorkoutSession> WorkoutSessions { get; set; }
    public string? PasswordHash { get; set; } // password never stored in the public view
}
