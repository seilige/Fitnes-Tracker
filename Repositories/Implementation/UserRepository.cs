using Microsoft.EntityFrameworkCore;

namespace FitnesTracker;

public class UserRepository : Repository<User>, IUserRepository
{
    public UserRepository(ApplicationDbContext context) : base(context) 
    { // User gets context and provide to base class
    }

    public async Task<IEnumerable<User>> GetAllAuthorsAsync()
    {
        return await _context.Users.Where(x => x.Author).ToListAsync(); // protected _context from base class 
    }

    public async Task<IEnumerable<User>> GetByFullNameAsync(string name, string lastname)
    {
        return await _context.Users.Where(x => x.Name == name && x.Lastname == lastname).ToListAsync();
    }
}
