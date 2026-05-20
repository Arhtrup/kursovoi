using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Windows.Input;

namespace AirportSystem.Avalonia.ViewModels;
public partial class TechnicianViewModel : ViewModelBase
{
    [ObservableProperty] private string _message = "Обслуживание, инструменты, ремонт.";
    public ICommand TestCommand { get; }
    public TechnicianViewModel() => TestCommand = new RelayCommand(() => Message = "");
}
