namespace FitnesTracker;

// What user should specify, when...
public class UserCreateDTO
{
    public string? Name { get; set; }
    public string? Lastname { get; set; }
}

public class UserResponseDTO
{
    public int IdUser { get; set; }
    public string? Name { get; set; }
    public string? Lastname { get; set; }
    public bool Author { get; set; }
}

public class UserUpdateDTO
{
    public int IdUser { get; set; }
    public string? Name { get; set; }
    public string? Lastname { get; set; }
}
