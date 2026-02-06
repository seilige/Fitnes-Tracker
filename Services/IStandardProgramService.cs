namespace FitnesTracker;

public interface IStandardProgramService
{
    Task<IEnumerable<StandardProgramResponseDTO>> GetAllAsync();
    Task<StandardProgramResponseDTO?> GetByIdAsync(int id);
    Task<PagedResult<StandardProgramResponseDTO>> GetPagedAsync(PaginationParams paginationParams);
}
