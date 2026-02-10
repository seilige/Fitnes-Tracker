using AutoMapper;

namespace FitnesTracker;

public class UserService : IUserService
{
    private readonly IUserRepository _userRepository;
    private readonly IMapper _mapper;

    public UserService(IUserRepository userRepository, IMapper mapper)
    {
        _userRepository = userRepository;
        _mapper = mapper;
    }

    public async Task<IEnumerable<UserResponseDTO>> GetAllAsync()
    {
        var users = await _userRepository.GetAllAsync();
        return _mapper.Map<IEnumerable<UserResponseDTO>>(users);
    }

    public async Task<UserResponseDTO> CreateAsync(UserCreateDTO dto)
    {
        var user = _mapper.Map<User>(dto);
        await _userRepository.AddAsync(user);
        await _userRepository.SaveChangesAsync();
        return _mapper.Map<UserResponseDTO>(user);
    }

    public async Task<UserResponseDTO?> GetByIdAsync(int id)
    {
        var user = await _userRepository.GetByIDAsync(id);
        return _mapper.Map<UserResponseDTO?>(user);
    }

    public async Task<UserResponseDTO> UpdateAsync(int id, UserUpdateDTO dto)
    {
        var user = _mapper.Map<User>(dto);
        user.IdUser = id;
        var updated = await _userRepository.UpdateAsync(user);
        await _userRepository.SaveChangesAsync();
        return _mapper.Map<UserResponseDTO>(updated);
    }

    public async Task<bool> DeleteAsync(int id)
    {
        await _userRepository.DeleteByIDAsync(id);
        await _userRepository.SaveChangesAsync();
        return true;
    }
}