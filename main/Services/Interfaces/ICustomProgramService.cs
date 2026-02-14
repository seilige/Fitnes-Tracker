using AutoMapper;

namespace FitnesTracker;

public interface ICustomProgramService
{
    Task<PagedResult<CustomProgramResponseDTO>> GetAllAsync(int pageNumber, int pageSize);
    Task<CustomProgramResponseDTO> CreateAsync(CustomProgramCreateDTO dto, int creatorId);
    Task<CustomProgramResponseDTO?> GetByIdAsync(int id);
    Task<CustomProgramResponseDTO> UpdateAsync(int id, CustomProgramUpdateDTO dto);
    Task<bool> DeleteAsync(int id);
}
