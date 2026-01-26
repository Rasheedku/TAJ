using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Taj.BuildingCostApp.Views;

namespace Taj.BuildingCostApp;

public sealed partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();
        App.Services.Navigation.Initialize(RootFrame);
        RootFrame.Navigate(typeof(LoginPage));
    }
}
