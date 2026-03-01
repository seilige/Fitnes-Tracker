using Microsoft.EntityFrameworkCore;
using Org.BouncyCastle.Bcpg;

namespace FitnesTracker;

public class UserRepository : Repository<User>, IUserRepository
{
    public UserRepository(ApplicationDbContext context) : base(context) 
    {
    }

    public async Task AddUser(User user)
    {
        await _context.Users.AddAsync(user);
    }

    public async Task<User?> GetByEmailConfirmationTokenAsync(string token)
    {
        return await _context.Users.AsNoTracking().FirstOrDefaultAsync(x => x.EmailConfirmationToken == token);
    }

    public async Task<PagedResult<User>> GetAllAsync(int pageNum, int pageSize)
    {
        var users = _context.Users.AsNoTracking().Include(x => x.WorkoutSessions);

        var count = await users.CountAsync();
        var items = await users.OrderBy(x => x.UserId).Skip((pageNum - 1) * pageSize).Take(pageSize).ToListAsync();

        return new PagedResult<User>(
            items: items,
            totalCount: count,
            pageNumber: pageNum,
            pageSize: pageSize
        );
    }

    public async Task<IEnumerable<User>> GetAllAuthorsAsync()
    {
        return await _context.Users.AsNoTracking().Where(x => x.Author).ToListAsync(); // protected _context from base class 
    }

    public async Task<IEnumerable<User>> GetByFullNameAsync(string name, string lastname)
    {
        return await _context.Users.AsNoTracking().Where(x => x.Name == name && x.Lastname == lastname).ToListAsync();
    }

    public async Task<User?> GetUserByEmail(string email)
    {
        return await _context.Users.AsNoTracking().FirstOrDefaultAsync(x => x.Email == email);
    }

    public async Task SaveRefreshToken(RefreshToken token)
    {
        await _context.RefreshTokens.AddAsync(token);
    }

    public async Task<RefreshToken?> GetUsersActiveToken(User user)
    {
        return await _context.RefreshTokens.Where(x => x.UserId == user.UserId && x.IsRevoked == false && x.ExpiresAt > DateTime.UtcNow).FirstOrDefaultAsync();
    }

    public async Task<User?> GetUserByToken(string token)
    {
        var refreshToken = await _context.RefreshTokens
                                            .Include(rt => rt.User)
                                            .FirstOrDefaultAsync(rt => rt.Token == token && !rt.IsRevoked && rt.ExpiresAt > DateTime.UtcNow);

        return refreshToken?.User;
    }

    public async Task<RefreshToken?> GetActiveToken(string token)
    {
        return await _context.RefreshTokens
            .Include(rt => rt.User)
            .Where(x => x.Token == token)
            .FirstOrDefaultAsync();
    }
}
