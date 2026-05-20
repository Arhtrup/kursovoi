using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using AirportSystem.Avalonia.Models;
namespace AirportSystem.Avalonia.Services;
public class JsonUserRepository : IUserRepository
{
    private readonly string _filePath = "users.json";
    private List<User> _users = new();
    public JsonUserRepository() { LoadData(); }
    private void LoadData()
    {
        if (File.Exists(_filePath))
            _users = JsonSerializer.Deserialize<List<User>>(File.ReadAllText(_filePath)) ?? new();
        else
        {
            _users = new List<User>
            {
                new() { Id = Guid.NewGuid().ToString(), Name = "admin", Password = "admin", Role = UserRole.Admin },
                new() { Id = Guid.NewGuid().ToString(), Name = "passenger", Password = "pass", Role = UserRole.Passenger },
                new() { Id = Guid.NewGuid().ToString(), Name = "office", Password = "office", Role = UserRole.OfficeEmployee },
                new() { Id = Guid.NewGuid().ToString(), Name = "pilot", Password = "pilot", Role = UserRole.Pilot },
                new() { Id = Guid.NewGuid().ToString(), Name = "tech", Password = "tech", Role = UserRole.Technician }
            };
            SaveData();
        }
    }
    private void SaveData() => File.WriteAllText(_filePath, JsonSerializer.Serialize(_users, new JsonSerializerOptions { WriteIndented = true }));
    public Task<List<User>> GetAllUsers() => Task.FromResult(_users);
    public Task<User?> GetUserByUsername(string username) => Task.FromResult(_users.FirstOrDefault(u => u.Name == username));
    public Task AddUser(User user) { user.Id = Guid.NewGuid().ToString(); _users.Add(user); SaveData(); return Task.CompletedTask; }
    public Task DeleteUser(string id) { _users.RemoveAll(u => u.Id == id); SaveData(); return Task.CompletedTask; }
}
