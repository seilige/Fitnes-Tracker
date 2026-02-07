using Microsoft.EntityFrameworkCore;

namespace FitnesTracker;

public class ExerciseRepository : Repository<Exercise>, IExerciseRepository
{
    // context contains all DbSets
    public ExerciseRepository(ApplicationDbContext context) : base(context)
    {
    }

    public async Task<PagedResult<Exercise>> GetFilteredExercisesAsync(ExerciseQueryParameters parameters)
    {
        var query = _context.Exercises.AsQueryable();

        // catch title if string != null so if string == null we may fillter exercise via muscle group 
        if (!string.IsNullOrEmpty(parameters.Title))
            query = query.Where(e => e.Title.Contains(parameters.Title));

        if (parameters.MuscleGroup != null)
            query = query.Where(e => e.MuscleGroup == parameters.MuscleGroup);

        var totalCount = await query.CountAsync();

        var items = await query
            .Skip((parameters.PageNumber - 1) * parameters.PageSize)
            .Take(parameters.PageSize)
            .ToListAsync();

        return new PagedResult<Exercise>(items, totalCount, parameters.PageNumber, parameters.PageSize);
    }

    // base logic of paginaiton - skip pages
    public async Task<PagedResult<Exercise>> GetPagedAsync(PaginationParams paginationParams)
    {
        var count = await _context.Exercises.CountAsync();
        var exercise = await _context.Exercises
            .Skip((paginationParams.PageNumber - 1) * paginationParams.PageSize)
            .Take(paginationParams.PageSize)
            .ToListAsync();

        return new PagedResult<Exercise>(
            exercise, count, paginationParams.PageNumber, paginationParams.PageSize);
    }

    public async Task<Exercise?> GetByTitleAsync(string title)
    {
        return await _context.Exercises.Where(x => x.Title == title).FirstOrDefaultAsync();
    }

    public async Task<IEnumerable<Exercise>> GetByMuscleGroupAsync(MuscleGroup group)
    {
        return await _context.Exercises.Where(x => x.MuscleGroup == group).ToListAsync();
    }
}
