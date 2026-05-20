using System.Threading.Tasks;
using System.Windows.Input;
using AirportSystem.Avalonia.Services;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace AirportSystem.Avalonia.ViewModels;

public partial class LoginViewModel : ViewModelBase
{
    private readonly IAuthService _authService;
    private readonly MainWindowViewModel _mainViewModel;

    [ObservableProperty] private string _username = string.Empty;
    [ObservableProperty] private string _password = string.Empty;
    [ObservableProperty] private string _errorMessage = string.Empty;

    public ICommand LoginCommand { get; }
    public ICommand CancelCommand { get; }

    public LoginViewModel(IAuthService authService, MainWindowViewModel mainViewModel)
    {
        _authService = authService;
        _mainViewModel = mainViewModel;
        LoginCommand = new AsyncRelayCommand(LoginAsync);
        CancelCommand = new RelayCommand(() => _mainViewModel.CurrentView = null);
    }

    private async Task LoginAsync()
    {
        ErrorMessage = string.Empty;
        var user = await _authService.LoginAsync(Username, Password);
        if (user != null) _mainViewModel.UpdateWelcomeMessage();
        else ErrorMessage = "Неверное имя пользователя или пароль";
    }
}
