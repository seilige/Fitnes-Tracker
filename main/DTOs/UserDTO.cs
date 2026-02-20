namespace FitnesTracker;

public class UserCreateDTO
{
    public string? Name { get; set; }
    public string? Lastname { get; set; }
}

public class UserResponseDTO
{
    public int UserId { get; set; }
    public string? Name { get; set; }
    public string? Lastname { get; set; }
    public bool Author { get; set; }
}

public class UserUpdateDTO
{
    public int UserId { get; set; }
    public string? Name { get; set; }
    public string? Lastname { get; set; }
}
