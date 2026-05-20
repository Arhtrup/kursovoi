using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using AirportSystem.Avalonia.Models;
namespace AirportSystem.Avalonia.Services;
public class JsonFlightRepository : IFlightRepository
{
    private readonly string _filePath = "flights.json";
    private List<Flight> _flights = new();
    public JsonFlightRepository() { LoadData(); }
    private void LoadData()
    {
        if (File.Exists(_filePath))
            _flights = JsonSerializer.Deserialize<List<Flight>>(File.ReadAllText(_filePath)) ?? new();
        else { _flights = new List<Flight>(); SaveData(); }
    }
    private void SaveData() => File.WriteAllText(_filePath, JsonSerializer.Serialize(_flights, new JsonSerializerOptions { WriteIndented = true }));
    public Task<List<Flight>> GetAllFlights() => Task.FromResult(_flights);
    public Task<Flight?> GetFlightByNumber(string flightNumber) => Task.FromResult(_flights.FirstOrDefault(f => f.FlightNumber == flightNumber));
    public Task AddFlight(Flight flight) { _flights.Add(flight); SaveData(); return Task.CompletedTask; }
    public Task UpdateFlight(Flight flight) { var idx = _flights.FindIndex(f => f.FlightNumber == flight.FlightNumber); if (idx >= 0) _flights[idx] = flight; SaveData(); return Task.CompletedTask; }
    public Task DeleteFlight(string flightNumber) { _flights.RemoveAll(f => f.FlightNumber == flightNumber); SaveData(); return Task.CompletedTask; }
    public Task<List<Flight>> SearchFlights(string? from, string? to, DateTime? date)
    {
        var q = _flights.AsQueryable();
        if (!string.IsNullOrEmpty(from)) q = q.Where(f => f.DepartureCity.Contains(from, StringComparison.OrdinalIgnoreCase));
        if (!string.IsNullOrEmpty(to)) q = q.Where(f => f.ArrivalCity.Contains(to, StringComparison.OrdinalIgnoreCase));
        if (date.HasValue) q = q.Where(f => f.DepartureTime.Date == date.Value.Date);
        return Task.FromResult(q.ToList());
    }
}
