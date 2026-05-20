using System;
using System.Windows.Input;
using Avalonia.Controls;
using AirportSystem.Avalonia.Models;
using AirportSystem.Avalonia.Services;
using AirportSystem.Avalonia.Views;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.ComponentModel;
using Microsoft.Extensions.DependencyInjection;

namespace AirportSystem.Avalonia.ViewModels;

public partial class MainWindowViewModel : ViewModelBase
{
    private readonly IAuthService _authService;
    private readonly IServiceProvider _services;
    private bool _isUpdating;

    [ObservableProperty] private object? _currentView;
    [ObservableProperty] private User? _currentUser;
    [ObservableProperty] private string _welcomeMessage = string.Empty;

    public ICommand ShowLoginCommand { get; }
    public ICommand ShowRegistrationCommand { get; }
    public ICommand LogoutCommand { get; }

    public MainWindowViewModel(IAuthService authService, IServiceProvider services)
    {
        _authService = authService;
        _services = services;
        ShowLoginCommand = new RelayCommand(() => SetView<LoginView, LoginViewModel>());
        ShowRegistrationCommand = new RelayCommand(() => SetView<RegistrationView, RegistrationViewModel>());
        LogoutCommand = new RelayCommand(Logout);
        UpdateWelcomeMessage();
    }

    private void SetView<TView, TViewModel>() where TView : Control where TViewModel : class
    {
        var view = _services.GetRequiredService<TView>();
        var vm = _services.GetRequiredService<TViewModel>();
        view.DataContext = vm;
        CurrentView = view;
    }

    public void UpdateWelcomeMessage()
    {
        if (_isUpdating) return;
        _isUpdating = true;
        try
        {
            CurrentUser = _authService.CurrentUser;
            if (CurrentUser != null)
            {
                WelcomeMessage = $"Добро пожаловать, {CurrentUser.Name} ({CurrentUser.Role})";
                switch (CurrentUser.Role)
                {
                    case UserRole.Passenger: SetView<PassengerView, PassengerViewModel>(); break;
                    case UserRole.OfficeEmployee: SetView<OfficeEmployeeView, OfficeEmployeeViewModel>(); break;
                    case UserRole.Pilot: SetView<PilotView, PilotViewModel>(); break;
                    case UserRole.Technician: SetView<TechnicianView, TechnicianViewModel>(); break;
                    case UserRole.Admin: SetView<AdminView, AdminViewModel>(); break;
                    default: CurrentView = null; break;
                }
            }
            else
            {
                WelcomeMessage = "Вы не авторизованы";
                SetView<LoginView, LoginViewModel>();
            }
        }
        finally { _isUpdating = false; }
    }

    private void Logout() { _authService.Logout(); UpdateWelcomeMessage(); }
}