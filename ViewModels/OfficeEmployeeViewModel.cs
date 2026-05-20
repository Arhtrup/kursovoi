using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Windows.Input;

namespace AirportSystem.Avalonia.ViewModels;
public partial class OfficeEmployeeViewModel : ViewModelBase
{
    [ObservableProperty] private string _message = "Управление рейсами, расписанием, самолётами, сотрудниками, инфраструктурой.";
    public ICommand TestCommand { get; }
    public OfficeEmployeeViewModel() => TestCommand = new RelayCommand(() => Message = "Тест выполнен");
}
