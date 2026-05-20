using System.Threading.Tasks;
using System.Windows.Input;
using AirportSystem.Avalonia.Services;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace AirportSystem.Avalonia.ViewModels;
public partial class RegistrationViewModel : ViewModelBase
{
    private readonly IAuthService _authService;
    private readonly MainWindowViewModel _mainViewModel;
    [ObservableProperty] private string _username = string.Empty;
    [ObservableProperty] private string _password = string.Empty;
    [ObservableProperty] private string _confirmPassword = string.Empty;
    [ObservableProperty] private string _errorMessage = string.Empty;
    public ICommand RegisterCommand { get; }
    public ICommand CancelCommand { get; }
    public RegistrationViewModel(IAuthService authService, MainWindowViewModel mainViewModel)
    {
        _authService = authService;
        _mainViewModel = mainViewModel;
        RegisterCommand = new AsyncRelayCommand(RegisterAsync);
        CancelCommand = new RelayCommand(() => _mainViewModel.CurrentView = null);
    }
    private async Task RegisterAsync()
    {
        if (string.IsNullOrWhiteSpace(Username) || string.IsNullOrWhiteSpace(Password)) { ErrorMessage = "Заполните все поля"; return; }
        if (Password != ConfirmPassword) { ErrorMessage = "Пароли не совпадают"; return; }
        var success = await _authService.RegisterAsync(Username, Password);
        if (success) _mainViewModel.UpdateWelcomeMessage();
        else ErrorMessage = "Пользователь уже существует";
    }
}
