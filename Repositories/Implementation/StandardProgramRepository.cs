using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace FitnesTracker;

public class StandardProgramRepository : Repository<StandardProgram>, IStandardProgramRepository
{
    public StandardProgramRepository(ApplicationDbContext context) : base(context) 
    {
    }

    public async Task<PagedResult<StandardProgram>> GetPagedAsync(PaginationParams paginationParams)
    {
        var count = await _context.StandardPrograms.CountAsync();
        var standardProg = await _context.StandardPrograms
            .Skip((paginationParams.PageNumber - 1) * paginationParams.PageSize)
            .Take(paginationParams.PageSize)
            .ToListAsync();

        return new PagedResult<StandardProgram>(
            standardProg, count, paginationParams.PageNumber, paginationParams.PageSize);
    }

    public async Task<IEnumerable<Exercise>> GetExercisesAsync(int standardProgramId)
    {
        return await _context.StandardProgramExercises
                                .Where(x => x.ProgId == standardProgramId)
                                .Include(x => x.Exercise)
                                .Select(x => x.Exercise).ToListAsync();
    }

    public async Task<IEnumerable<StandardProgram>> GetByLevelAsync(Level level)
    {
        return await _context.StandardPrograms.Where(x => x.Level == level).ToListAsync();
    }

    public async Task<IEnumerable<StandardProgram>> GetByWorkoutTypeAsync(WorkoutType workoutType)
    {
        return await _context.StandardPrograms.Where(x => x.WorkoutType == workoutType).ToListAsync();
    }

    public async Task<IEnumerable<StandardProgram>> GetByCategoryAsync(Category category)
    {
        return await _context.StandardPrograms.Where(x => x.Category == category).ToListAsync();
    }
}
