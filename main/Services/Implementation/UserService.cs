using AutoMapper;

namespace FitnesTracker;

public class UserService : IUserService
{
    private readonly IUserRepository _userRepository;
    private readonly IMapper _mapper;
    private readonly ILogger<ExerciseService> _logger;
    private readonly IEmailService _email;
    private readonly IUnitOfWork _unitOfWork;

    public UserService(IUserRepository userRepository, IMapper mapper, ILogger<ExerciseService> logger, IEmailService email, IUnitOfWork unitOfWork)
    {
        _userRepository = userRepository;
        _mapper = mapper;
        _logger = logger;
        _unitOfWork = unitOfWork;
        // _email = email;
    }

    /// <summary>
    /// Returns a paged list of users by page number and page size.
    /// </summary>
    public async Task<PagedResult<UserResponseDTO>> GetAllAsync(int pageNumber, int pageSize)
    {
        var pagedUsers = await _userRepository.GetAllAsync(pageNumber, pageSize);
        var dtos = _mapper.Map<List<UserResponseDTO>>(pagedUsers.Items);
        return new PagedResult<UserResponseDTO>(dtos, pagedUsers.TotalCount, pageNumber, pageSize);
    }

    /// <summary>
    /// Creates a new user and saves it to the database. Returns the created user.
    /// </summary>
    public async Task<UserResponseDTO> CreateAsync(UserCreateDTO dto)
    {
        var user = _mapper.Map<User>(dto);

        // user.EmailConfirmationToken = _email.GenerateTokenEmail();
        // user.EmailTokenExpiry = DateTime.UtcNow.AddHours(24);
        // _email.SendConfirmationEmail(user.Email, user.EmailConfirmationToken);

        await _userRepository.AddAsync(user);
        await _unitOfWork.SaveChangesAsync();
        _logger.LogInformation("User already added");
        return _mapper.Map<UserResponseDTO>(user);
    }

    /// <summary>
    /// Returns a user by ID. Throws KeyNotFoundException if the user is not found.
    /// </summary>
    public async Task<UserResponseDTO?> GetByIdAsync(int id)
    {
        var user = await _userRepository.GetByIDAsync(id);

        if(user == null)
        {
            _logger.LogInformation($"User with id: {id} not found");
            throw new KeyNotFoundException($"User with id: {id} not found");
        }

        return _mapper.Map<UserResponseDTO?>(user);
    }

    /// <summary>
    /// Updates an existing user by ID. Throws KeyNotFoundException if the user is not found.
    /// </summary>
    public async Task<UserResponseDTO> UpdateAsync(int id, UserUpdateDTO dto)
    {
        var user = _mapper.Map<User>(dto);
        user.UserId = id;
        var updated = await _userRepository.UpdateAsync(user);

        if(updated == null)
        {
            _logger.LogInformation($"User with id: {id} not found");
            throw new KeyNotFoundException($"User with id: {id} not found");
        }

        await _unitOfWork.SaveChangesAsync();
        return _mapper.Map<UserResponseDTO>(updated);
    }

    /// <summary>
    /// Deletes a user by ID. Throws KeyNotFoundException if the user is not found. Returns true on success.
    /// </summary>
    public async Task<bool> DeleteAsync(int id)
    {
        var entity = await _userRepository.GetByIDAsync(id);

        if(entity == null)
        {
            _logger.LogInformation($"User with id: {id} not found");
            throw new KeyNotFoundException("Not found");
        }

        await _userRepository.DeleteByIDAsync(id);
        await _unitOfWork.SaveChangesAsync();
        return true;
    }
}