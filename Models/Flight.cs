using System;
namespace AirportSystem.Avalonia.Models;
public enum FlightStatus { Scheduled, Boarding, Departed, Cancelled, Completed }
public class Flight
{
    public string FlightNumber { get; set; } = string.Empty;
    public string AircraftId { get; set; } = string.Empty;
    public string PilotId { get; set; } = string.Empty;
    public DateTime DepartureTime { get; set; }
    public DateTime ArrivalTime { get; set; }
    public string DepartureCity { get; set; } = string.Empty;
    public string ArrivalCity { get; set; } = string.Empty;
    public int AvailableSeats { get; set; } = 30;
    public double Price { get; set; } = 100.0;
    public FlightStatus Status { get; set; } = FlightStatus.Scheduled;
    public Flight() { }
    public Flight(string number, DateTime depTime) { FlightNumber = number; DepartureTime = depTime; }
}
