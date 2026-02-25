namespace FitnesTracker;

public interface IUserRepository : IRepository<User>
{
    Task<IEnumerable<User>> GetAllAuthorsAsync();
    Task<IEnumerable<User>> GetByFullNameAsync(string name, string lastname);
    Task<User> GetUserByEmail(string email);
    Task AddUser(User user);
    Task<PagedResult<User>> GetAllAsync(int pageNum, int pageSize);
    Task<User?> GetByEmailConfirmationTokenAsync(string token);
    Task SaveRefreshToken(RefreshToken token);
    Task<RefreshToken?> GetUsersActiveToken(User user);
    Task<RefreshToken?> GetActiveToken(string token);
    Task<User?> GetUserByToken(string token);
}
