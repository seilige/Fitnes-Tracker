using Microsoft.EntityFrameworkCore;

namespace FitnesTracker;

public class CustomProgramRepository : Repository<CustomProgram>, ICustomProgramRepository
{
    public CustomProgramRepository(ApplicationDbContext context) : base(context)
    {
    }

    public async Task<PagedResult<User>> GetAllAsync(int pageNum, int pageSize)
    {
        var users = _context.Users.AsNoTracking().Include(x => x.WorkoutSessions);

        var count = await users.CountAsync();
        var items = await users.Skip((pageNum - 1) * pageSize).Take(pageSize).ToListAsync();

        return new PagedResult<User>(
            items: items,
            totalCount: count,
            pageNumber: pageNum,
            pageSize: pageSize
        );
    }

    public async Task<User?> GetCreatorAsync(int custProgId)
    {
        var prog = await _context.CustomPrograms.AsNoTracking().Include(x => x.Creator).Where(x => x.CustomProgramId == custProgId).FirstOrDefaultAsync();

        return prog?.Creator;
    }

    public async Task<IEnumerable<Exercise>> GetExercisesAsync(int custProgId)
    {
        // We take all programs with linked Exercises, find required id and take first program
        var prog = await _context.CustomPrograms.AsNoTracking().Include(x => x.Exercises).Where(x => x.CustomProgramId == custProgId).FirstOrDefaultAsync();

        return prog?.Exercises ?? Enumerable.Empty<Exercise>();
    }

    public async Task<IEnumerable<CustomProgram>> GetByCreatorIdAsync(int creatorId)
    {
        return await _context.CustomPrograms.AsNoTracking().Where(x => x.CreatorId == creatorId).ToListAsync();
    }

    public async Task<IEnumerable<CustomProgram>> GetPublicProgramsAsync()
    {
        // all cust prog with exercises and programs must be public
        return await _context.CustomPrograms.AsNoTracking().Include(x => x.Exercises).Where(c => c.IsPublic).ToListAsync();
    }
}
