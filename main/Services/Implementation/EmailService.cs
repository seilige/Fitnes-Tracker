using System.Runtime.CompilerServices;
using Microsoft.IdentityModel.Tokens;
using MailKit.Net.Smtp;
using MailKit.Security;
using MimeKit;

namespace FitnesTracker;

public class EmailService : IEmailService
{
    private readonly IUserRepository _repo;
    private readonly IConfiguration _config;

    public EmailService(IUserRepository repo, IConfiguration config)
    {
        _repo = repo;
        _config = config;
    }

    public string GenerateTokenEmail()
    {
        return Convert.ToBase64String(Guid.NewGuid().ToByteArray());
    }

    public async Task SendEmailAsync(string to, string subject, string body)
    {
        var smtp = _config["EmailSettings:SmtpServer"];
        var port = int.Parse(_config["EmailSettings:Port"]);
        var sender = _config["EmailSettings:SenderEmail"];
        var password = _config["EmailSettings:SenderPassword"];

        var message = new MimeMessage();

            message.From.Add(new MailboxAddress("Fitness Tracker", sender));
            message.To.Add(new MailboxAddress("", to));
            message.Subject = subject;
            message.Body = new TextPart("html") { Text = body };

            using var client = new SmtpClient();
            await client.ConnectAsync(smtp, port, SecureSocketOptions.StartTls);
            await client.AuthenticateAsync(sender, password);
            await client.SendAsync(message);
            await client.DisconnectAsync(true);
    }

    public async Task ConfirmEmailAsync(string token)
    {
        var user = await _repo.GetByEmailConfirmationTokenAsync(token);

        if(user == null) throw new KeyNotFoundException("User not found");

        if (user.EmailTokenExpiry < DateTime.UtcNow)
        {
            throw new SecurityTokenExpiredException("Token expired");
        }

        user.IsEmailConfirmed = true;
        user.EmailConfirmationToken = null;
        user.EmailTokenExpiry = null;
        await _repo.SaveChangesAsync();
    }
}
