using Microsoft.EntityFrameworkCore;

namespace FitnesTracker;

public class CustomProgramRepository : Repository<CustomProgram>, ICustomProgramRepository
{
    public CustomProgramRepository(ApplicationDbContext context) : base(context)
    {
    }

    public async Task<User?> GetCreatorAsync(int custProgId)
    {
        var prog = await _context.CustomPrograms.Include(x => x.Creator).Where(x => x.CustProgId == custProgId).FirstOrDefaultAsync();

        return prog?.Creator;
    }

    public async Task<IEnumerable<Exercise>> GetExercisesAsync(int custProgId)
    {
        // We take all programs with linked Exercises, find required id and take first program
        var prog = await _context.CustomPrograms.Include(x => x.Exercises).Where(x => x.CustProgId == custProgId).FirstOrDefaultAsync();

        return prog?.Exercises ?? Enumerable.Empty<Exercise>();
    }

    public async Task<IEnumerable<CustomProgram>> GetByCreatorIdAsync(int creatorId)
    {
        return await _context.CustomPrograms.Where(x => x.CreatorId == creatorId).ToListAsync();
    }

    public async Task<IEnumerable<CustomProgram>> GetPublicProgramsAsync()
    {
        return await _context.CustomPrograms.Include(x => x.Exercises).Where(c => c.IsPublic).ToListAsync();
    }
}
