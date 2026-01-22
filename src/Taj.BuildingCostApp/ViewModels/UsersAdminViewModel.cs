using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Taj.BuildingCostApp.Models;
using Taj.BuildingCostApp.Services;

namespace Taj.BuildingCostApp.ViewModels;

public partial class UsersAdminViewModel : ObservableObject
{
    private readonly UserService _userService;

    public UsersAdminViewModel()
    {
        _userService = App.Services.Users;
        Users = new ObservableCollection<UserRecord>(_userService.UsersData.Users);
    }

    public ObservableCollection<UserRecord> Users { get; }
    public IReadOnlyList<UserRole> Roles { get; } = Enum.GetValues<UserRole>();

    [ObservableProperty]
    private string _newUsername = string.Empty;

    [ObservableProperty]
    private string _newPassword = string.Empty;

    [ObservableProperty]
    private UserRole _newRole = UserRole.User;

    [ObservableProperty]
    private string _statusMessage = string.Empty;

    [RelayCommand]
    private async Task CreateUserAsync()
    {
        StatusMessage = string.Empty;
        if (string.IsNullOrWhiteSpace(NewUsername) || string.IsNullOrWhiteSpace(NewPassword))
        {
            StatusMessage = "Username and password are required.";
            return;
        }

        if (Users.Any(u => u.Username.Equals(NewUsername, StringComparison.OrdinalIgnoreCase)))
        {
            StatusMessage = "Username already exists.";
            return;
        }

        var user = _userService.CreateUser(NewUsername.Trim(), NewPassword, NewRole);
        Users.Add(user);
        await _userService.SaveAsync();
        NewUsername = string.Empty;
        NewPassword = string.Empty;
        NewRole = UserRole.User;
        StatusMessage = "User created.";
    }

    [RelayCommand]
    private async Task ToggleActiveAsync(UserRecord user)
    {
        _userService.ToggleActive(user);
        await _userService.SaveAsync();
        StatusMessage = "User status updated.";
    }

    [RelayCommand]
    private async Task ChangePasswordAsync(UserRecord user)
    {
        if (string.IsNullOrWhiteSpace(NewPassword))
        {
            StatusMessage = "Enter a password to change.";
            return;
        }

        _userService.UpdatePassword(user, NewPassword);
        await _userService.SaveAsync();
        NewPassword = string.Empty;
        StatusMessage = "Password updated.";
    }
}
