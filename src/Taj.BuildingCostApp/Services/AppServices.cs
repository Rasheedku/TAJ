using Taj.BuildingCostApp.Models;
using Taj.BuildingCostApp.Utilities;

namespace Taj.BuildingCostApp.Services;

public class AppServices
{
    public JsonStorageService Storage { get; } = new();
    public UserService Users { get; private set; } = null!;
    public PricingService Pricing { get; private set; } = null!;
    public SettingsService Settings { get; private set; } = null!;
    public LocalizationService Localization { get; private set; } = null!;
    public NavigationService Navigation { get; } = new();
    public ExportService Export { get; } = new();
    public UserRecord? CurrentUser { get; set; }

    public async Task InitializeAsync()
    {
        Settings = new SettingsService(Storage);
        Pricing = new PricingService(Storage);
        Users = new UserService(Storage);

        await Settings.InitializeAsync();
        await Pricing.InitializeAsync();
        await Users.InitializeAsync();

        Localization = new LocalizationService(Settings);
        await Localization.InitializeAsync();
    }
}
