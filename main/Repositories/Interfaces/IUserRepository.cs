namespace FitnesTracker;

public interface IUserRepository : IRepository<User>
{
    Task<IEnumerable<User>> GetAllAuthorsAsync();
    Task<IEnumerable<User>> GetByFullNameAsync(string name, string lastname);
    Task<User> GetUserByEmail(string email);
    Task AddUser(User user);
    Task<PagedResult<User>> GetAllAsync(int pageNum, int pageSize);
}
