using AutoMapper;

namespace FitnesTracker;

public interface IUserService
{
    Task<UserResponseDTO> CreateAsync(UserCreateDTO dto);
    Task<UserResponseDTO> UpdateAsync(int id, UserUpdateDTO dto);
    Task<bool> DeleteAsync(int id);
    Task<PagedResult<UserResponseDTO>> GetAllAsync(int pageNumber, int pageSize);
    Task<UserResponseDTO?> GetByIdAsync(int id);
}
