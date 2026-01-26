using Microsoft.UI.Xaml;
using Taj.BuildingCostApp.Services;

namespace Taj.BuildingCostApp;

public partial class App : Application
{
    public static AppServices Services { get; } = new();
    public static Window? MainWindowInstance { get; private set; }

    public App()
    {
        InitializeComponent();
    }

    protected override async void OnLaunched(LaunchActivatedEventArgs args)
    {
        await Services.InitializeAsync();

        MainWindowInstance = new MainWindow();
        MainWindowInstance.Activate();
    }
}
