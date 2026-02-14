using Microsoft.EntityFrameworkCore;

namespace FitnesTracker;

public class Repository<T> : IRepository<T> where T : class
{
    protected readonly ApplicationDbContext _context;

    public Repository(ApplicationDbContext context)
    {
        _context = context;
    }

    // SaveChanges must be present in every transaction for atomicity
    public async Task SaveChangesAsync()
    {
        await _context.SaveChangesAsync();
    }

    public async Task<IEnumerable<T>> GetAllAsync()
    {
        return await _context.Set<T>().ToListAsync();
    }

    public async Task<PagedResult<T>> GetAllAsync(int pageNum, int pageSize)
    {
        var query = _context.Set<T>().AsNoTracking();
        
        var totalCount = await query.CountAsync();
        
        var items = await query
            .Skip((pageNum - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();
        
        return new PagedResult<T>(items, totalCount, pageNum, pageSize);
    }

    public async Task<T> GetByIDAsync(int id)
    {
        return await _context.Set<T>().FindAsync(id);
    }

    public async Task<T> UpdateAsync(T entity)
    {
        if(entity == null) throw new KeyNotFoundException("Entity not found");

        _context.Set<T>().Update(entity);
        return entity;
    }

    public async Task DeleteByIDAsync(int id)
    {
        var entity = await _context.Set<T>().FindAsync(id);
        _context.Set<T>().Remove(entity);
    }

    public async Task AddAsync(T entity)
    {
        _context.Set<T>().Add(entity);
    }
}
