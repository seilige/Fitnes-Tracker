namespace FitnesTracker;

public interface IAuthentication
{
    Task<AuthResponseDTO> Login(string email, string password);
    Task<AuthResponseDTO> Register(string email, string password, string name, string lastname);
    Task<PagedResult<UserResponseDTO>> GetAllAsync(int pageNumber, int pageSize);
    Task<AuthResponseDTO> GetTokenAaync(RefreshRequestDTO dto);
    Task Logout(string token);
}
