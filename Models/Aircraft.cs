using System;
namespace AirportSystem.Avalonia.Models;
public class Aircraft
{
    public string Id { get; set; } = Guid.NewGuid().ToString();
    public string Model { get; set; } = string.Empty;
    public string RegistrationNumber { get; set; } = string.Empty;
    public int Capacity { get; set; }
    public bool IsUnderMaintenance { get; set; }
}
