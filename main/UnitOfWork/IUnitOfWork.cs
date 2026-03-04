namespace FitnesTracker;

public interface IUnitOfWork
{
    Task SaveChangesAsync();
}
