using Taj.BuildingCostApp.Models;

namespace Taj.BuildingCostApp.Services;

public class SettingsService
{
    private readonly JsonStorageService _storage;
    public AppSettings Settings { get; private set; } = new();

    public SettingsService(JsonStorageService storage)
    {
        _storage = storage;
    }

    public async Task InitializeAsync()
    {
        Settings = await _storage.LoadAsync("settings.json", () => new AppSettings());
    }

    public Task SaveAsync() => _storage.SaveAsync("settings.json", Settings);
}
