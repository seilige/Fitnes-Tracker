namespace FitnesTracker;

public class CorsOptions
{
    public string PolicyName { get; set; } = string.Empty;
    public string[] AllowedOrigins { get; set; } = [];
    public string[] AllowedMethods { get; set; } = [];
    public string[] AllowedHeaders { get; set; } = [];
}
