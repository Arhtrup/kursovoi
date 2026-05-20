using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AirportSystem.Avalonia.Models;
namespace AirportSystem.Avalonia.Services;
public interface IFlightRepository
{
    Task<List<Flight>> GetAllFlights();
    Task<Flight?> GetFlightByNumber(string flightNumber);
    Task AddFlight(Flight flight);
    Task UpdateFlight(Flight flight);
    Task DeleteFlight(string flightNumber);
    Task<List<Flight>> SearchFlights(string? from, string? to, DateTime? date);
}
