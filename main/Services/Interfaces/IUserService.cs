using AutoMapper;

namespace FitnesTracker;

public interface IUserService
{
    Task<IEnumerable<UserResponseDTO>> GetAllAsync();
    Task<UserResponseDTO> CreateAsync(UserCreateDTO dto);
    Task<UserResponseDTO> UpdateAsync(int id, UserUpdateDTO dto);
    Task<bool> DeleteAsync(int id);
    Task<UserResponseDTO?> GetByIdAsync(int id);
}
