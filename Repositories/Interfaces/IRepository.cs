
namespace FitnesTracker;

public interface IRepository<T> where T : class
{
    Task<IEnumerable<T>> GetAllAsync();
    Task<T> GetByIDAsync(int id);
    Task<T> UpdateAsync(T entity);
    Task DeleteByIDAsync(int id);
    Task AddAsync(T entity);
}
