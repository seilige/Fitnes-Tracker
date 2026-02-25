namespace FitnesTracker;

public class UserRegisterDTO
{
    public string Email { get; set; }
    public string Password { get; set; }
    public string Name { get; set; }
    public string Lastname { get; set; }
}

public class UserLoginDTO
{
    public string Email { get; set; }
    public string Password { get; set; }
}

public class AuthResponseDTO
{
    public string AccessToken { get; set; }
    public string RefreshToken { get; set; }
    public DateTime ExpiresAt { get; set; } 
}

public class RefreshRequestDTO
{
    public string Token { get; set; }
}
