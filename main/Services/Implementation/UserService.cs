using AutoMapper;

namespace FitnesTracker;

public class UserService : IUserService
{
    private readonly IUserRepository _userRepository;
    private readonly IMapper _mapper;
    private readonly ILogger<ExerciseService> _logger;

    public UserService(IUserRepository userRepository, IMapper mapper, ILogger<ExerciseService> logger)
    {
        _userRepository = userRepository;
        _mapper = mapper;
        _logger = logger;
    }

    public async Task<PagedResult<UserResponseDTO>> GetAllAsync(int pageNumber, int pageSize)
    {
        var pagedUsers = await _userRepository.GetAllAsync(pageNumber, pageSize);
        var dtos = _mapper.Map<List<UserResponseDTO>>(pagedUsers.Items);
        return new PagedResult<UserResponseDTO>(dtos, pagedUsers.TotalCount, pageNumber, pageSize);
    }

    public async Task<UserResponseDTO> CreateAsync(UserCreateDTO dto)
    {
        var user = _mapper.Map<User>(dto);
        await _userRepository.AddAsync(user);
        await _userRepository.SaveChangesAsync();
        _logger.LogInformation("User already added");
        return _mapper.Map<UserResponseDTO>(user);
    }

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

        await _userRepository.SaveChangesAsync();
        return _mapper.Map<UserResponseDTO>(updated);
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var entity = await _userRepository.GetByIDAsync(id);

        if(entity == null)
        {
            _logger.LogInformation($"User with id: {id} not found");

            throw new KeyNotFoundException("Not found");
        }

        await _userRepository.DeleteByIDAsync(id);
        await _userRepository.SaveChangesAsync();
        return true;
    }
}