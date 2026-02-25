namespace FitnesTracker;

public class RefreshToken
{
    public string Token { get; set; }
    public DateTime ExpiresAt { get; set; }
    public bool IsRevoked { get; set; }
    public int RefreshTokenId { get; set; }
    public int UserId { get; set; }
    public User User { get; set; }
}
