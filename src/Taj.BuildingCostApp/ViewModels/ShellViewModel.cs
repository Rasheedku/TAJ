using CommunityToolkit.Mvvm.ComponentModel;

namespace Taj.BuildingCostApp.ViewModels;

public partial class ShellViewModel : ObservableObject
{
    public ShellViewModel()
    {
        var user = App.Services.CurrentUser;
        UserName = user?.Username ?? string.Empty;
        IsAdmin = user?.Role == Models.UserRole.Admin;
    }

    [ObservableProperty]
    private string _userName = string.Empty;

    [ObservableProperty]
    private bool _isAdmin;
}
