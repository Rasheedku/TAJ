using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Taj.BuildingCostApp.Models;
using Taj.BuildingCostApp.Services;
using Taj.BuildingCostApp.Views;

namespace Taj.BuildingCostApp.ViewModels;

public partial class LoginViewModel : ObservableObject
{
    private readonly UserService _users;
    private readonly NavigationService _navigation;

    public LoginViewModel()
    {
        _users = App.Services.Users;
        _navigation = App.Services.Navigation;
    }

    [ObservableProperty]
    private string _username = string.Empty;

    [ObservableProperty]
    private string _errorMessage = string.Empty;

    [RelayCommand]
    private void Login(string password)
    {
        ErrorMessage = string.Empty;
        if (string.IsNullOrWhiteSpace(Username) || string.IsNullOrWhiteSpace(password))
        {
            ErrorMessage = "Please enter username and password.";
            return;
        }

        var user = _users.Authenticate(Username.Trim(), password);
        if (user == null)
        {
            ErrorMessage = "Invalid credentials.";
            return;
        }

        App.Services.CurrentUser = user;
        _navigation.Navigate<ShellPage>();
    }
}
