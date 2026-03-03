namespace FitnesTracker;

public class RateLimiterOptions
{
    public string PolicyName { get; set; } = string.Empty;
    public int WindowMinutes { get; set; }
    public int PermitLimit { get; set; }
    public int QueueLimit { get; set; }
}
