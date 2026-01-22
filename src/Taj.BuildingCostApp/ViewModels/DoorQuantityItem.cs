using CommunityToolkit.Mvvm.ComponentModel;
using Taj.BuildingCostApp.Models;

namespace Taj.BuildingCostApp.ViewModels;

public partial class DoorQuantityItem : ObservableObject
{
    public DoorTypePrice DoorType { get; }

    public DoorQuantityItem(DoorTypePrice doorType)
    {
        DoorType = doorType;
    }

    [ObservableProperty]
    private int _quantity;

    public double Total => Quantity * DoorType.Price;

    partial void OnQuantityChanged(int value)
    {
        OnPropertyChanged(nameof(Total));
    }
}
