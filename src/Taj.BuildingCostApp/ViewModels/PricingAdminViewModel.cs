using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Taj.BuildingCostApp.Models;
using Taj.BuildingCostApp.Services;

namespace Taj.BuildingCostApp.ViewModels;

public partial class PricingAdminViewModel : ObservableObject
{
    private readonly PricingService _pricingService;

    public PricingAdminViewModel()
    {
        _pricingService = App.Services.Pricing;
        Prices = _pricingService.Prices;
        DoorTypes = new ObservableCollection<DoorTypePrice>(Prices.Doors.DoorTypes);
        DeliveryRates = new ObservableCollection<DeliveryRate>(Prices.Delivery.Rates);
    }

    public PriceConfig Prices { get; }
    public ObservableCollection<DoorTypePrice> DoorTypes { get; }
    public ObservableCollection<DeliveryRate> DeliveryRates { get; }

    [RelayCommand]
    private void AddDoorType()
    {
        var newDoor = new DoorTypePrice { Name = "New", Price = 0 };
        DoorTypes.Add(newDoor);
        Prices.Doors.DoorTypes = DoorTypes.ToList();
    }

    [RelayCommand]
    private void RemoveDoorType(DoorTypePrice door)
    {
        DoorTypes.Remove(door);
        Prices.Doors.DoorTypes = DoorTypes.ToList();
    }

    [RelayCommand]
    private void AddDeliveryRate()
    {
        var rate = new DeliveryRate { Emirate = "New Emirate", PerSkidCost = 0 };
        DeliveryRates.Add(rate);
        Prices.Delivery.Rates = DeliveryRates.ToList();
    }

    [RelayCommand]
    private void RemoveDeliveryRate(DeliveryRate rate)
    {
        DeliveryRates.Remove(rate);
        Prices.Delivery.Rates = DeliveryRates.ToList();
    }

    [RelayCommand]
    private async Task SaveAsync()
    {
        Prices.Doors.DoorTypes = DoorTypes.ToList();
        Prices.Delivery.Rates = DeliveryRates.ToList();
        await _pricingService.SaveAsync();
    }
}
