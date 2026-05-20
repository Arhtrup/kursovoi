using System;
namespace AirportSystem.Avalonia.Models;
public class Ticket
{
    public string TicketNumber { get; set; } = string.Empty;
    public string FlightNumber { get; set; } = string.Empty;
    public string PassengerId { get; set; } = string.Empty;
    public double Price { get; set; }
    public DateTime BookingDate { get; set; }
    public bool IsPaid { get; set; }
}
