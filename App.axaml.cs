using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;
using AirportSystem.Avalonia.Services;
using AirportSystem.Avalonia.ViewModels;
using AirportSystem.Avalonia.Views;
using AirportSystem.Avalonia.Infrastructure.Payment;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace AirportSystem.Avalonia;

public partial class App : Application
{
    public override void Initialize() => AvaloniaXamlLoader.Load(this);

    public override void OnFrameworkInitializationCompleted()
    {
        var services = new ServiceCollection();

        services.AddSingleton<IUserRepository, JsonUserRepository>();
        services.AddSingleton<IFlightRepository, JsonFlightRepository>();
        services.AddSingleton<ITicketRepository, JsonTicketRepository>();
        services.AddSingleton<IPaymentGateway, MockPaymentGateway>();
        services.AddSingleton<IAuthService, AuthService>();

        services.AddTransient<MainWindowViewModel>();
        services.AddTransient<LoginViewModel>();
        services.AddTransient<RegistrationViewModel>();
        services.AddTransient<PassengerViewModel>();
        services.AddTransient<OfficeEmployeeViewModel>();
        services.AddTransient<PilotViewModel>();
        services.AddTransient<TechnicianViewModel>();
        services.AddTransient<AdminViewModel>();

        services.AddTransient<MainWindow>();
        services.AddTransient<LoginView>();
        services.AddTransient<RegistrationView>();
        services.AddTransient<PassengerView>();
        services.AddTransient<OfficeEmployeeView>();
        services.AddTransient<PilotView>();
        services.AddTransient<TechnicianView>();
        services.AddTransient<AdminView>();

        var sp = services.BuildServiceProvider();

        if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
        {
            var mainWindow = sp.GetRequiredService<MainWindow>();
            mainWindow.DataContext = sp.GetRequiredService<MainWindowViewModel>();
            desktop.MainWindow = mainWindow;
            mainWindow.Show();
        }

        base.OnFrameworkInitializationCompleted();
    }
}
