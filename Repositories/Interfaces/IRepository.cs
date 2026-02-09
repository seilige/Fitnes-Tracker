
namespace FitnesTracker;

public interface IRepository<T> where T : class
{
    Task<IEnumerable<T>> GetAllAsync();
    Task SaveChangesAsync();
    Task<T> GetByIDAsync(int id);
    Task<T> UpdateAsync(T entity); // Object "entity" updated before call method
    Task DeleteByIDAsync(int id);
    Task AddAsync(T entity);
}
