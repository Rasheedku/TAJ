using Taj.BuildingCostApp.Models;

namespace Taj.BuildingCostApp.Services;

public class PricingService
{
    private readonly JsonStorageService _storage;
    public PriceConfig Prices { get; private set; } = new();

    public PricingService(JsonStorageService storage)
    {
        _storage = storage;
    }

    public async Task InitializeAsync()
    {
        Prices = await _storage.LoadAsync("prices.json", () => new PriceConfig());
    }

    public Task SaveAsync() => _storage.SaveAsync("prices.json", Prices);
}
