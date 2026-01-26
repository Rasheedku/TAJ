using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Taj.BuildingCostApp.ViewModels;

namespace Taj.BuildingCostApp.Views;

public sealed partial class LoginPage : Page
{
    public LoginPage()
    {
        InitializeComponent();
        Loaded += OnLoaded;
    }

    private void OnLoaded(object sender, RoutedEventArgs e)
    {
        FlowDirection = App.Services.Localization.CurrentFlowDirection;
    }

    private void OnLoginClick(object sender, RoutedEventArgs e)
    {
        if (DataContext is LoginViewModel vm)
        {
            vm.LoginCommand.Execute(PasswordBox.Password);
        }
    }
}
