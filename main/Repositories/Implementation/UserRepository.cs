using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Migrations.Operations;

namespace FitnesTracker;

public class UserRepository : Repository<User>, IUserRepository
{
    public UserRepository(ApplicationDbContext context) : base(context) 
    { // User gets context and provide to base class
    }

    public async Task AddUser(User user)
    {
        await _context.Users.AddAsync(user);
    }

    public async Task<ICollection<User>> GetAllAsync()
    {
        return await _context.Users.Include(x => x.WorkoutSessions).ToListAsync();
    }

    public async Task<IEnumerable<User>> GetAllAuthorsAsync()
    {
        return await _context.Users.Where(x => x.Author).ToListAsync(); // protected _context from base class 
    }

    public async Task<IEnumerable<User>> GetByFullNameAsync(string name, string lastname)
    {
        return await _context.Users.Where(x => x.Name == name && x.Lastname == lastname).ToListAsync();
    }

    public async Task<User> GetUserByEmail(string email)
    {
        return await _context.Users.FirstOrDefaultAsync(x => x.Email == email);
    }
}
