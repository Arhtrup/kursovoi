using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows.Input;
using AirportSystem.Avalonia.Models;
using AirportSystem.Avalonia.Services;
using AirportSystem.Avalonia.Infrastructure.Payment;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace AirportSystem.Avalonia.ViewModels;

public partial class PassengerViewModel : ViewModelBase
{
    private readonly IFlightRepository _flightRepo;
    private readonly ITicketRepository _ticketRepo;
    private readonly IPaymentGateway _paymentGateway;
    private readonly IAuthService _authService;

    [ObservableProperty] private ObservableCollection<Flight> _availableFlights = new();
    [ObservableProperty] private ObservableCollection<Ticket> _myTickets = new();
    [ObservableProperty] private string _searchFrom = string.Empty;
    [ObservableProperty] private string _searchTo = string.Empty;
    [ObservableProperty] private DateTime? _searchDate;
    [ObservableProperty] private Flight? _selectedFlight;
    [ObservableProperty] private string _statusMessage = string.Empty;

    public ICommand SearchFlightsCommand { get; }
    public ICommand BuyTicketCommand { get; }

    public PassengerViewModel(IFlightRepository flightRepo, ITicketRepository ticketRepo,
                              IPaymentGateway paymentGateway, IAuthService authService)
    {
        _flightRepo = flightRepo;
        _ticketRepo = ticketRepo;
        _paymentGateway = paymentGateway;
        _authService = authService;
        SearchFlightsCommand = new AsyncRelayCommand(SearchFlightsAsync);
        BuyTicketCommand = new AsyncRelayCommand(BuyTicketAsync, () => SelectedFlight != null);
        _ = Task.Run(LoadMyTickets);
    }

    private async Task SearchFlightsAsync()
    {
        var flights = await _flightRepo.SearchFlights(SearchFrom, SearchTo, SearchDate);
        AvailableFlights.Clear();
        foreach (var f in flights) AvailableFlights.Add(f);
    }

    private async Task BuyTicketAsync()
    {
        if (SelectedFlight == null) return;
        var currentUser = _authService.CurrentUser;
        if (currentUser == null) { StatusMessage = "Не авторизован"; return; }
        bool paid = await _paymentGateway.ProcessPaymentAsync(SelectedFlight.Price);
        if (!paid) { StatusMessage = "Оплата не прошла"; return; }
        var ticket = new Ticket
        {
            TicketNumber = Guid.NewGuid().ToString(),
            FlightNumber = SelectedFlight.FlightNumber,
            PassengerId = currentUser.Id,
            Price = SelectedFlight.Price,
            BookingDate = DateTime.Now,
            IsPaid = true
        };
        await _ticketRepo.AddTicket(ticket);
        SelectedFlight.AvailableSeats--;
        await _flightRepo.UpdateFlight(SelectedFlight);
        StatusMessage = $"Билет на рейс {SelectedFlight.FlightNumber} куплен!";
        await LoadMyTickets();
        await SearchFlightsAsync();
    }

    private async Task LoadMyTickets()
    {
        var user = _authService.CurrentUser;
        if (user != null)
        {
            var tickets = await _ticketRepo.GetTicketsByPassenger(user.Id);
            MyTickets.Clear();
            foreach (var t in tickets) MyTickets.Add(t);
        }
    }
}
