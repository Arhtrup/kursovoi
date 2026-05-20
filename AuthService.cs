using System.Threading.Tasks;
using AirportSystem.Avalonia.Models;
namespace AirportSystem.Avalonia.Services;
public class AuthService : IAuthService
{
    private readonly IUserRepository _userRepository;
    private User? _currentUser;
    public AuthService(IUserRepository userRepository) => _userRepository = userRepository;
    public User? CurrentUser => _currentUser;
    public async Task LoginAsync(string username, string password)
    {
        var user = await _userRepository.GetUserByUsername(username);
        if (user != null && user.Password == password) _currentUser = user;
        else _currentUser = null;
    }
    public void Logout() => _currentUser = null;
    public async Task RegisterAsync(string username, string password, UserRole role = UserRole.Passenger)
    {
        if (await _userRepository.GetUserByUsername(username) != null) return;
        await _userRepository.AddUser(new User { Name = username, Password = password, Role = role });
    }
}