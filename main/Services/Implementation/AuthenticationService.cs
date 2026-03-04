using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using System.Net;
using AutoMapper;

namespace FitnesTracker;

public class Authentication : IAuthentication
{
    private readonly IUserRepository _repository;
    private readonly IConfiguration _configuration;
    private readonly ILogger<ExerciseService> _logger;
    private readonly IMapper _mapper;
    private readonly IEmailService _email;

    public Authentication(IUserRepository repository, IConfiguration configuration, ILogger<ExerciseService> logger, IMapper mapper, IEmailService email)
    {
        _repository = repository;
        _configuration = configuration;
        _logger = logger;
        _mapper = mapper;
        _email = email; 
    }

    /// <summary>
    /// Returns a paged list of all users.
    /// </summary>
    public async Task<PagedResult<UserResponseDTO>> GetAllAsync(int pageNumber, int pageSize)
    {
        var pagedUsers = await _repository.GetAllAsync(pageNumber, pageSize);
        var dtos = _mapper.Map<List<UserResponseDTO>>(pagedUsers.Items);
        return new PagedResult<UserResponseDTO>(dtos, pagedUsers.TotalCount, pageNumber, pageSize);
    }

    /// <summary>
    /// Validates the refresh token, revokes it, and issues a new access and refresh token pair.
    /// Throws UnauthorizedAccessException if the token is invalid or expired.
    /// </summary>
    public async Task<AuthResponseDTO> GetTokenAsync(RefreshRequestDTO dto)
    {
        var token = await _repository.GetActiveToken(dto.Token);

        if (token == null || token.ExpiresAt <= DateTime.UtcNow || token.IsRevoked)
            throw new UnauthorizedAccessException("Invalid refresh token");

        token.IsRevoked = true;
        await _repository.SaveChangesAsync();

        var user = token.User;

        if (user == null)
            throw new KeyNotFoundException("User not found");

        RefreshToken rToken = new RefreshToken
        {
            Token = Convert.ToBase64String(Guid.NewGuid().ToByteArray()),
            ExpiresAt = DateTime.UtcNow.AddDays(7),
            IsRevoked = false,
            UserId = user.UserId
        };

        await _repository.SaveRefreshToken(rToken);

        return new AuthResponseDTO
        {
            AccessToken = GenerateToken(user),
            RefreshToken = rToken.Token,
            ExpiresAt = rToken.ExpiresAt
        };
    }

    /// <summary>
    /// Registers a new user, sends a confirmation email, and returns an access and refresh token pair.
    /// Throws KeyNotFoundException if the email is already taken.
    /// </summary>
    public async Task<AuthResponseDTO> Register(string email, string password, string name, string lastname)
    {
        var existUser = await _repository.GetUserByEmail(email);

        if(existUser != null)
        {
            _logger.LogInformation("User already exists");
            throw new KeyNotFoundException("User already exists");
        }

        User newUser = new User
        {
            Email = email,
            PasswordHash = BCrypt.Net.BCrypt.HashPassword(password),
            Name = name,
            Lastname = lastname,
            EmailConfirmationToken = _email.GenerateTokenEmail(),
            EmailTokenExpiry = DateTime.UtcNow.AddHours(24),
            IsEmailConfirmed = false
        };

        var confirmLink = $"http://localhost:5000/api/auth/confirm-email?token={WebUtility.UrlEncode(newUser.EmailConfirmationToken)}";
        await _email.SendEmailAsync(
            newUser.Email,
            "Confirm your email",
            $"<a href='{confirmLink}'>Click here to confirm</a>"
        );

        await _repository.AddUser(newUser);
        await _repository.SaveChangesAsync();

        RefreshToken rToken = new RefreshToken
        {
            Token = Convert.ToBase64String(Guid.NewGuid().ToByteArray()),
            ExpiresAt = DateTime.UtcNow.AddDays(7),
            IsRevoked = false,
            UserId = newUser.UserId
        };

        await _repository.SaveRefreshToken(rToken);
        await _repository.SaveChangesAsync();

        return new AuthResponseDTO
        {
            AccessToken = GenerateToken(newUser),
            RefreshToken = rToken.Token,
            ExpiresAt = rToken.ExpiresAt
        };
    }

    /// <summary>
    /// Logs in a user by email and password, revokes any existing refresh token, and returns a new token pair.
    /// Throws KeyNotFoundException if user not found, UnauthorizedAccessException if password is wrong.
    /// </summary>
    public async Task<AuthResponseDTO> Login(string email, string password)
    {
        var user = await _repository.GetUserByEmail(email);

        if(user == null) throw new KeyNotFoundException("User not found");

        if(!BCrypt.Net.BCrypt.Verify(password, user.PasswordHash))
            throw new UnauthorizedAccessException("Invalid password");

        if (!user.IsEmailConfirmed)
            throw new UnauthorizedAccessException("Email not confirmed");

        var token = await _repository.GetUsersActiveToken(user);

        if(token != null)
        {
            token.IsRevoked = true;
        }

        RefreshToken rToken = new RefreshToken
        {
            Token = Convert.ToBase64String(Guid.NewGuid().ToByteArray()),
            ExpiresAt = DateTime.UtcNow.AddDays(7),
            IsRevoked = false,
            UserId = user.UserId
        };

        await _repository.SaveRefreshToken(rToken);
        await _repository.SaveChangesAsync();

        return new AuthResponseDTO
        {
            AccessToken = GenerateToken(user),
            RefreshToken = rToken.Token,
            ExpiresAt = rToken.ExpiresAt
        };
    }

    /// <summary>
    /// Revokes the given refresh token to log the user out.
    /// Throws KeyNotFoundException if the token is invalid or already revoked.
    /// </summary>
    public async Task Logout(string token)
    {
        var tokenExist = await _repository.GetActiveToken(token);

        if(tokenExist == null || tokenExist.ExpiresAt <= DateTime.UtcNow || tokenExist.IsRevoked)
            throw new KeyNotFoundException("Token not found");

        tokenExist.IsRevoked = true;

        await _repository.SaveChangesAsync();
    }

    /// <summary>
    /// Generates a signed JWT access token for the given user. Expires in 60 minutes.
    /// </summary>
    private string GenerateToken(User user)
    {
        var claims = new[]
        {
            new Claim(ClaimTypes.NameIdentifier, user.UserId.ToString()),
            new Claim(ClaimTypes.Email, user.Email),
            new Claim(ClaimTypes.Name, user.Name),
            new Claim(ClaimTypes.Role, user.Author ? "Author" : "User")
        };

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]!));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            issuer: _configuration["Jwt:Issuer"],
            audience: _configuration["Jwt:Audience"],
            claims: claims,
            expires: DateTime.UtcNow.AddMinutes(60),
            signingCredentials: creds
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}
