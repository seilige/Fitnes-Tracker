namespace FitnesTracker;

public interface IAuthentication
{
    Task<string> Login(string email, string password);
    Task<string> Register(string email, string password, string name, string lastname);
}
