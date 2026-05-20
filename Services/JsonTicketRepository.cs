using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using AirportSystem.Avalonia.Models;
namespace AirportSystem.Avalonia.Services;
public class JsonTicketRepository : ITicketRepository
{
    private readonly string _filePath = "tickets.json";
    private List<Ticket> _tickets = new();
    public JsonTicketRepository() { LoadData(); }
    private void LoadData()
    {
        if (File.Exists(_filePath))
            _tickets = JsonSerializer.Deserialize<List<Ticket>>(File.ReadAllText(_filePath)) ?? new();
        else { _tickets = new List<Ticket>(); SaveData(); }
    }
    private void SaveData() => File.WriteAllText(_filePath, JsonSerializer.Serialize(_tickets, new JsonSerializerOptions { WriteIndented = true }));
    public Task<List<Ticket>> GetAllTickets() => Task.FromResult(_tickets);
    public Task<List<Ticket>> GetTicketsByPassenger(string passengerId) => Task.FromResult(_tickets.Where(t => t.PassengerId == passengerId).ToList());
    public Task<Ticket?> GetTicketByNumber(string ticketNumber) => Task.FromResult(_tickets.FirstOrDefault(t => t.TicketNumber == ticketNumber));
    public Task AddTicket(Ticket ticket) { _tickets.Add(ticket); SaveData(); return Task.CompletedTask; }
    public Task CancelTicket(string ticketNumber) { _tickets.RemoveAll(t => t.TicketNumber == ticketNumber); SaveData(); return Task.CompletedTask; }
}
