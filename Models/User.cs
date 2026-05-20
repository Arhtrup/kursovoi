namespace AirportSystem.Avalonia.Models;
public enum UserRole { Passenger, OfficeEmployee, Pilot, Technician, Admin }
public class User
{
    public string Id { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
    public UserRole Role { get; set; }
}
