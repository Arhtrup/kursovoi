using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows.Input;
using AirportSystem.Avalonia.Models;
using AirportSystem.Avalonia.Services;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace AirportSystem.Avalonia.ViewModels;
public partial class AdminViewModel : ViewModelBase
{
    private readonly IUserRepository _userRepository;
    private readonly MainWindowViewModel _mainViewModel;
    [ObservableProperty] private ObservableCollection<User> _users = new();
    [ObservableProperty] private User? _selectedUser;
    [ObservableProperty] private string _newUsername = string.Empty;
    [ObservableProperty] private string _newPassword = string.Empty;
    [ObservableProperty] private UserRole _newRole = UserRole.Passenger;
    [ObservableProperty] private string _statusMessage = string.Empty;
    public ICommand AddUserCommand { get; }
    public ICommand DeleteUserCommand { get; }
    public ICommand BackCommand { get; }
    public System.Array Roles => System.Enum.GetValues(typeof(UserRole));
    public AdminViewModel(IUserRepository userRepository, MainWindowViewModel mainViewModel)
    {
        _userRepository = userRepository;
        _mainViewModel = mainViewModel;
        AddUserCommand = new AsyncRelayCommand(AddUserAsync);
        DeleteUserCommand = new AsyncRelayCommand(DeleteUserAsync, () => SelectedUser != null);
        BackCommand = new RelayCommand(() => _mainViewModel.CurrentView = null);
        _ = LoadUsers();
    }
    private async Task LoadUsers() { var list = await _userRepository.GetAllUsers(); Users.Clear(); foreach (var u in list) Users.Add(u); }
    private async Task AddUserAsync()
    {
        if (string.IsNullOrWhiteSpace(NewUsername) || string.IsNullOrWhiteSpace(NewPassword)) { StatusMessage = "Заполните поля"; return; }
        if (await _userRepository.GetUserByUsername(NewUsername) != null) { StatusMessage = "Пользователь уже существует"; return; }
        await _userRepository.AddUser(new User { Name = NewUsername, Password = NewPassword, Role = NewRole });
        StatusMessage = $"Пользователь {NewUsername} добавлен"; NewUsername = NewPassword = ""; await LoadUsers();
    }
    private async Task DeleteUserAsync() { if (SelectedUser != null) { await _userRepository.DeleteUser(SelectedUser.Id); StatusMessage = $"Удалён {SelectedUser.Name}"; await LoadUsers(); } }
}
