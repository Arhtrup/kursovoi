using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Windows.Input;

namespace AirportSystem.Avalonia.ViewModels;
public partial class PilotViewModel : ViewModelBase
{
    [ObservableProperty] private string _message = "План полётов, управление полётами.";
    public ICommand TestCommand { get; }
    public PilotViewModel() => TestCommand = new RelayCommand(() => Message = "");
}
