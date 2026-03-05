using Microsoft.IdentityModel.Tokens;
using Moq;
using Xunit;

namespace FitnesTracker;

public class EmailServiceTests
{
    private readonly Mock<IUserRepository> _repoMock;
    private readonly Mock<IConfiguration> _configMock;
    private readonly EmailService _service;
    private readonly Mock<IUnitOfWork> _UoW;

    public EmailServiceTests()
    {
        _repoMock = new Mock<IUserRepository>();
        _configMock = new Mock<IConfiguration>();
        _UoW = new Mock<IUnitOfWork>();

        _configMock.Setup(c => c["EmailSettings:SkipEmailSending"]).Returns("true");

        _service = new EmailService(_repoMock.Object, _configMock.Object, _UoW.Object);
    }

    [Fact]
    public void GenerateTokenEmail_ReturnsNonEmptyString()
    {
        var token = _service.GenerateTokenEmail();
        Assert.False(string.IsNullOrWhiteSpace(token));
    }

    [Fact]
    public void GenerateTokenEmail_ReturnsDifferentTokensEachCall()
    {
        var token1 = _service.GenerateTokenEmail();
        var token2 = _service.GenerateTokenEmail();
        Assert.NotEqual(token1, token2);
    }

    [Fact]
    public void GenerateTokenEmail_IsValidBase64()
    {
        var token = _service.GenerateTokenEmail();
        var ex = Record.Exception(() => Convert.FromBase64String(token));
        Assert.Null(ex);
    }

    [Fact]
    public async Task SendEmailAsync_WhenSkipIsTrue_DoesNotThrow()
    {
        _configMock.Setup(c => c["EmailSettings:SkipEmailSending"]).Returns("true");

        var ex = await Record.ExceptionAsync(() =>
            _service.SendEmailAsync("test@example.com", "Subject", "<p>Body</p>"));

        Assert.Null(ex);
    }

    [Fact]
    public async Task SendEmailAsync_WhenSkipIsFalse_AttemptsSmtpConnection()
    {
        _configMock.Setup(c => c["EmailSettings:SkipEmailSending"]).Returns("false");
        _configMock.Setup(c => c["EmailSettings:SmtpServer"]).Returns("invalid.smtp.host");
        _configMock.Setup(c => c["EmailSettings:Port"]).Returns("587");
        _configMock.Setup(c => c["EmailSettings:SenderEmail"]).Returns("sender@example.com");
        _configMock.Setup(c => c["EmailSettings:SenderPassword"]).Returns("password");

        await Assert.ThrowsAnyAsync<Exception>(() =>
            _service.SendEmailAsync("to@example.com", "Subject", "<p>Body</p>"));
    }

    [Fact]
    public async Task ConfirmEmailAsync_WhenUserNotFound_ThrowsKeyNotFoundException()
    {
        _repoMock
            .Setup(r => r.GetByEmailConfirmationTokenAsync(It.IsAny<string>()))
            .ReturnsAsync((User?)null);

        await Assert.ThrowsAsync<KeyNotFoundException>(() =>
            _service.ConfirmEmailAsync("invalid-token"));
    }

    [Fact]
    public async Task ConfirmEmailAsync_WhenTokenExpired_ThrowsSecurityTokenExpiredException()
    {
        var user = new User
        {
            EmailConfirmationToken = "token",
            EmailTokenExpiry = DateTime.UtcNow.AddMinutes(-10),
            IsEmailConfirmed = false
        };

        _repoMock
            .Setup(r => r.GetByEmailConfirmationTokenAsync("token"))
            .ReturnsAsync(user);

        await Assert.ThrowsAsync<SecurityTokenExpiredException>(() =>
            _service.ConfirmEmailAsync("token"));
    }

    [Fact]
    public async Task ConfirmEmailAsync_WhenTokenValid_SetsEmailConfirmedAndClearsToken()
    {
        var user = new User
        {
            EmailConfirmationToken = "valid-token",
            EmailTokenExpiry = DateTime.UtcNow.AddHours(1),
            IsEmailConfirmed = false
        };

        _repoMock.Setup(r => r.GetByEmailConfirmationTokenAsync("valid-token"))
            .ReturnsAsync(user);

        _UoW.Setup(r => r.SaveChangesAsync())
            .Returns(Task.CompletedTask);

        await _service.ConfirmEmailAsync("valid-token");

        Assert.True(user.IsEmailConfirmed);
        Assert.Null(user.EmailConfirmationToken);
        Assert.Null(user.EmailTokenExpiry);
    }

    [Fact]
    public async Task ConfirmEmailAsync_WhenTokenValid_CallsSaveChanges()
    {
        var user = new User
        {
            EmailConfirmationToken = "valid-token",
            EmailTokenExpiry = DateTime.UtcNow.AddHours(1),
            IsEmailConfirmed = false
        };

        _repoMock.Setup(r => r.GetByEmailConfirmationTokenAsync("valid-token"))
            .ReturnsAsync(user);

        _UoW.Setup(r => r.SaveChangesAsync())
            .Returns(Task.CompletedTask);

        await _service.ConfirmEmailAsync("valid-token");

        _UoW.Verify(r => r.SaveChangesAsync(), Times.Once);
    }
}
