using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using AutoMapper;

namespace FitnesTracker;

public class Authentication : IAuthentication
{
    private readonly IUserRepository _repository;
    private readonly IConfiguration _configuration;
    private readonly ILogger<ExerciseService> _logger;
    private readonly IMapper _mapper;

    public Authentication(IUserRepository repository, IConfiguration configuration, ILogger<ExerciseService> logger, IMapper mapper)
    {
        _repository = repository;
        _configuration = configuration;
        _logger = logger;
        _mapper = mapper;
    }

    public async Task<PagedResult<UserResponseDTO>> GetAllAsync(int pageNumber, int pageSize)
    {
        var pagedUsers = await _repository.GetAllAsync(pageNumber, pageSize);
        var dtos = _mapper.Map<List<UserResponseDTO>>(pagedUsers.Items);
        return new PagedResult<UserResponseDTO>(dtos, pagedUsers.TotalCount, pageNumber, pageSize);
    }

    public async Task<string> Register(string email, string password, string name, string lastname)
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
            Lastname = lastname
        };

        await _repository.AddUser(newUser);
        await _repository.SaveChangesAsync();

        return GenerateToken(newUser);
    }

    public async Task<string> Login(string email, string password)
    {
        var user = await _repository.GetUserByEmail(email);

        if(user == null) throw new KeyNotFoundException("User not found");

        if(!BCrypt.Net.BCrypt.Verify(password, user.PasswordHash))
            throw new UnauthorizedAccessException("Invalid password");


        return GenerateToken(user);
    }

    private string GenerateToken(User user)
    {
        var claims = new[]
        {
            new Claim(ClaimTypes.NameIdentifier, user.UserId.ToString()),
            new Claim(ClaimTypes.Email, user.Email)
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
