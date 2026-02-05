namespace FitnesTracker;

public class User
{
    public int IdUser { get; set; }
    public string? Name { get; set; }
    public string? Lastname { get; set; }
    public bool Author = false; // Can user create an offical traning program
}
