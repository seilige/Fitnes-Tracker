namespace FitnesTracker;

public interface IStandardProgramService
{
    Task<PagedResult<StandardProgramResponseDTO>> GetAllAsync(int pageNumber, int pageSize);    
    Task<StandardProgramResponseDTO?> GetByIdAsync(int id);
    Task<PagedResult<StandardProgramResponseDTO>> GetPagedAsync(PaginationParams paginationParams);
    
}
