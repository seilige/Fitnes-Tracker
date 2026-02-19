namespace FitnesTracker;

public interface IEmailService
{
    string GenerateTokenEmail();
    Task SendEmailAsync(string to, string subject, string body);
    Task ConfirmEmailAsync(string token);
}
