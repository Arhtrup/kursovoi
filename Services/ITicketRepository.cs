using System.Collections.Generic;
using System.Threading.Tasks;
using AirportSystem.Avalonia.Models;
namespace AirportSystem.Avalonia.Services;
public interface ITicketRepository
{
    Task<List<Ticket>> GetAllTickets();
    Task<List<Ticket>> GetTicketsByPassenger(string passengerId);
    Task<Ticket?> GetTicketByNumber(string ticketNumber);
    Task AddTicket(Ticket ticket);
    Task CancelTicket(string ticketNumber);
}
