using System.Collections.Generic;
using System.Threading.Tasks;
using AirportSystem.Avalonia.Models;
namespace AirportSystem.Avalonia.Services;
public interface IUserRepository
{
    Task<List<User>> GetAllUsers();
    Task<User?> GetUserByUsername(string username);
    Task AddUser(User user);
    Task DeleteUser(string id);
}
