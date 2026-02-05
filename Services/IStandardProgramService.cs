namespace FitnesTracker;

public interface IStandardProgramService
{
    Task<IEnumerable<StandardProgramResponseDTO>> GetAllAsync();
    Task<StandardProgramResponseDTO?> GetByIdAsync(int id);
}
