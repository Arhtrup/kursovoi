#!/bin/bash
# Создание проекта AirportSystem с нуля

set -e

PROJECT_NAME="AirportSystem"
mkdir -p $PROJECT_NAME
cd $PROJECT_NAME

# 1. Создание файла проекта
cat > $PROJECT_NAME.csproj << 'EOF'
<Project Sdk="Microsoft.NET.Sdk">
  <PropertyGroup>
    <OutputType>WinExe</OutputType>
    <TargetFramework>net8.0</TargetFramework>
    <Nullable>enable</Nullable>
    <AvaloniaUseCompiledBindingsByDefault>false</AvaloniaUseCompiledBindingsByDefault>
  </PropertyGroup>
  <ItemGroup>
    <PackageReference Include="Avalonia" Version="11.0.2" />
    <PackageReference Include="Avalonia.Desktop" Version="11.0.2" />
    <PackageReference Include="Avalonia.Themes.Fluent" Version="11.0.2" />
    <PackageReference Include="CommunityToolkit.Mvvm" Version="8.2.2" />
    <PackageReference Include="Microsoft.Extensions.DependencyInjection" Version="8.0.0" />
  </ItemGroup>
</Project>
EOF

# 2. Program.cs
cat > Program.cs << 'EOF'
using System;
using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;

namespace AirportSystem;

class Program
{
    [STAThread]
    public static void Main(string[] args) => BuildAvaloniaApp().StartWithClassicDesktopLifetime(args);
    public static AppBuilder BuildAvaloniaApp() => AppBuilder.Configure<App>().UsePlatformDetect().LogToTrace();
}
EOF

# 3. App.axaml
cat > App.axaml << 'EOF'
<Application xmlns="https://github.com/avaloniaui"
             xmlns:x="http://schemas.microsoft.com/winfx/2006/xaml"
             x:Class="AirportSystem.App">
    <Application.Resources>
        <ResourceDictionary>
            <!-- Здесь ничего не нужно -->
        </ResourceDictionary>
    </Application.Resources>
</Application>
EOF

# 4. App.axaml.cs
cat > App.axaml.cs << 'EOF'
using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;
using Microsoft.Extensions.DependencyInjection;
using AirportSystem.ViewModels;
using AirportSystem.Views;

namespace AirportSystem;

public partial class App : Application
{
    public override void Initialize() => AvaloniaXamlLoader.Load(this);

    public override void OnFrameworkInitializationCompleted()
    {
        var services = new ServiceCollection();
        services.AddSingleton<MainWindowViewModel>();
        services.AddSingleton<MainWindow>();

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
EOF

# 5. ViewModels/ViewModelBase.cs
mkdir -p ViewModels
cat > ViewModels/ViewModelBase.cs << 'EOF'
using CommunityToolkit.Mvvm.ComponentModel;

namespace AirportSystem.ViewModels;
public class ViewModelBase : ObservableObject { }
EOF

# 6. ViewModels/MainWindowViewModel.cs
cat > ViewModels/MainWindowViewModel.cs << 'EOF'
using CommunityToolkit.Mvvm.Input;
using System.Windows.Input;

namespace AirportSystem.ViewModels;
public partial class MainWindowViewModel : ViewModelBase
{
    [ObservableProperty] private string _title = "Airport System";
    [ObservableProperty] private string _message = "Добро пожаловать!";

    public ICommand TestCommand { get; }

    public MainWindowViewModel()
    {
        TestCommand = new RelayCommand(() => Message = "Кнопка сработала!");
    }
}
EOF

# 7. Views/MainWindow.axaml
mkdir -p Views
cat > Views/MainWindow.axaml << 'EOF'
<Window xmlns="https://github.com/avaloniaui"
        xmlns:x="http://schemas.microsoft.com/winfx/2006/xaml"
        x:Class="AirportSystem.Views.MainWindow"
        Title="{Binding Title}" Width="400" Height="300">
    <StackPanel Margin="20">
        <TextBlock Text="{Binding Message}" FontSize="20"/>
        <Button Content="Тест" Command="{Binding TestCommand}" Margin="0,20"/>
    </StackPanel>
</Window>
EOF

# 8. Views/MainWindow.axaml.cs
cat > Views/MainWindow.axaml.cs << 'EOF'
using Avalonia.Controls;

namespace AirportSystem.Views;
public partial class MainWindow : Window
{
    public MainWindow() => InitializeComponent();
}
EOF

# 9. Восстановление и запуск
dotnet restore
dotnet build
dotnet run
