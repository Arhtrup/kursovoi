using System.Threading.Tasks;
using AirportSystem.Avalonia.Models;
namespace AirportSystem.Avalonia.Services;
public interface IAuthService
{
    Task<User?> LoginAsync(string username, string password);
    void Logout();
    User? CurrentUser { get; }
    Task<bool> RegisterAsync(string username, string password, UserRole role = UserRole.Passenger);
}
