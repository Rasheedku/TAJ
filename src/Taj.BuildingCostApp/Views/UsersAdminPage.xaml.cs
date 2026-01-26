using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Taj.BuildingCostApp.ViewModels;

namespace Taj.BuildingCostApp.Views;

public sealed partial class UsersAdminPage : Page
{
    public UsersAdminPage()
    {
        InitializeComponent();
        Loaded += OnLoaded;
    }

    private void OnLoaded(object sender, RoutedEventArgs e)
    {
        FlowDirection = App.Services.Localization.CurrentFlowDirection;
    }

    private void OnPasswordChanged(object sender, RoutedEventArgs e)
    {
        if (DataContext is UsersAdminViewModel vm && sender is PasswordBox box)
        {
            vm.NewPassword = box.Password;
        }
    }
}
