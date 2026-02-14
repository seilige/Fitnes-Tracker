namespace FitnesTracker;

public interface IAuthentication
{
    Task<string> Login(string email, string password);
    Task<string> Register(string email, string password, string name, string lastname);
    Task<PagedResult<UserResponseDTO>> GetAllAsync(int pageNumber, int pageSize);
}
