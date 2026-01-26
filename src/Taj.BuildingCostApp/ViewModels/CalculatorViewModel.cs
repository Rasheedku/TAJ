using System.Collections.ObjectModel;
using System.IO;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Taj.BuildingCostApp.Models;
using Taj.BuildingCostApp.Services;

namespace Taj.BuildingCostApp.ViewModels;

public partial class CalculatorViewModel : ObservableObject
{
    private readonly PriceConfig _prices;

    public CalculatorViewModel()
    {
        _prices = App.Services.Pricing.Prices;
        DoorItems = new ObservableCollection<DoorQuantityItem>(
            _prices.Doors.DoorTypes.Select(type => new DoorQuantityItem(type)));
        foreach (var item in DoorItems)
        {
            item.PropertyChanged += (_, __) => Recalculate();
        }
        EmirateOptions = new ObservableCollection<string>(_prices.Delivery.Rates.Select(r => r.Emirate));
        SelectedEmirate = EmirateOptions.FirstOrDefault() ?? string.Empty;
        Recalculate();
    }

    public ObservableCollection<DoorQuantityItem> DoorItems { get; }
    public ObservableCollection<string> EmirateOptions { get; }
    public IReadOnlyList<BuildingShape> BuildingShapeOptions { get; } = Enum.GetValues<BuildingShape>();
    public IReadOnlyList<FoundationType> FoundationOptions { get; } = Enum.GetValues<FoundationType>();
    public IReadOnlyList<GlassType> GlassOptions { get; } = Enum.GetValues<GlassType>();
    public IReadOnlyList<SkidFinish> SkidOptions { get; } = Enum.GetValues<SkidFinish>();
    public IReadOnlyList<ExteriorFinish> ExteriorFinishOptions { get; } = Enum.GetValues<ExteriorFinish>();
    public IReadOnlyList<ExternalFloorFinish> ExternalFloorOptions { get; } = Enum.GetValues<ExternalFloorFinish>();
    public IReadOnlyList<PantryFinish> PantryFinishOptions { get; } = Enum.GetValues<PantryFinish>();

    [ObservableProperty]
    private BuildingShape _selectedShape = BuildingShape.Rectangle;

    [ObservableProperty]
    private double _length = 10;

    [ObservableProperty]
    private double _width = 8;

    [ObservableProperty]
    private double _overallLength = 12;

    [ObservableProperty]
    private double _overallWidth = 10;

    [ObservableProperty]
    private double _cutoutLength = 3;

    [ObservableProperty]
    private double _cutoutWidth = 3;

    [ObservableProperty]
    private double _courtyardWidth = 4;

    [ObservableProperty]
    private double _courtyardDepth = 4;

    [ObservableProperty]
    private double _internalHeight = 3;

    [ObservableProperty]
    private double _externalHeight = 3.5;

    [ObservableProperty]
    private FoundationType _foundationType = FoundationType.PackedBlock;

    [ObservableProperty]
    private GlassType _glassType = GlassType.Single;

    [ObservableProperty]
    private double _glassLinearMeters = 12;

    [ObservableProperty]
    private double _glassHeight = 2.4;

    [ObservableProperty]
    private SkidFinish _skidFinish = SkidFinish.Ceramic;

    [ObservableProperty]
    private int _numberOfSkids = 1;

    [ObservableProperty]
    private double _totalPartitionsLength = 10;

    [ObservableProperty]
    private double _wetAreaWallsLength = 6;

    [ObservableProperty]
    private double _exteriorWallsLength = 20;

    [ObservableProperty]
    private ExteriorFinish _exteriorFinish = ExteriorFinish.Paint;

    [ObservableProperty]
    private bool _hasParapet;

    [ObservableProperty]
    private double _parapetAreaM2 = 5;

    [ObservableProperty]
    private double _ceiling6060AreaM2 = 20;

    [ObservableProperty]
    private double _gypsumWithLightingAreaM2 = 10;

    [ObservableProperty]
    private bool _enableExternalFlooring;

    [ObservableProperty]
    private double _externalFloorAreaM2 = 12;

    [ObservableProperty]
    private ExternalFloorFinish _externalFloorFinish = ExternalFloorFinish.Ceramic;

    [ObservableProperty]
    private bool _hasBathrooms;

    [ObservableProperty]
    private double _bathroomTotalCost = 0;

    [ObservableProperty]
    private bool _hasPantry;

    [ObservableProperty]
    private double _cabinetLengthLm = 3;

    [ObservableProperty]
    private PantryFinish _pantryFinish = PantryFinish.Aluminum;

    [ObservableProperty]
    private string _selectedEmirate = string.Empty;

    [ObservableProperty]
    private double _craneOffloadingCost = 0;

    [ObservableProperty]
    private double _extraCosts = 0;

    [ObservableProperty]
    private double _grandTotal;

    [ObservableProperty]
    private string _lastExportPath = string.Empty;

    public double BuildingArea => Math.Max(0, SelectedShape switch
    {
        BuildingShape.Rectangle => Length * Width,
        BuildingShape.LShape => OverallLength * OverallWidth - CutoutLength * CutoutWidth,
        BuildingShape.UShape => OverallLength * OverallWidth - CourtyardWidth * CourtyardDepth,
        _ => 0
    });

    public bool IsRectangle => SelectedShape == BuildingShape.Rectangle;
    public bool IsLShape => SelectedShape == BuildingShape.LShape;
    public bool IsUShape => SelectedShape == BuildingShape.UShape;

    public double GlassArea => GlassLinearMeters * GlassHeight;
    public double ExteriorWallArea => ExteriorWallsLength * ExternalHeight;
    public double WetWallArea => WetAreaWallsLength * InternalHeight;
    public double PartitionWallArea => TotalPartitionsLength * InternalHeight;

    public double FoundationCost => BuildingArea * FoundationRate;
    public double GlassCost => GlassArea * GlassRate;
    public double SkidCost => BuildingArea * SkidRate;
    public double ExteriorWallCost => ExteriorWallArea * ExteriorWallRate;
    public double WetWallCost => WetWallArea * _prices.Walls.WetAreaPerM2;
    public double PartitionWallCost => PartitionWallArea * _prices.Walls.PartitionPerM2;
    public double RoofCost => BuildingArea * _prices.Roof.RoofPerM2;
    public double ParapetCost => HasParapet ? ParapetAreaM2 * _prices.Roof.ParapetPerM2 : 0;
    public double Ceiling6060Cost => Ceiling6060AreaM2 * _prices.Ceilings.Ceiling6060PerM2;
    public double GypsumCost => GypsumWithLightingAreaM2 * _prices.Ceilings.GypsumWithLightingPerM2;
    public double ExternalFloorCost => EnableExternalFlooring ? ExternalFloorAreaM2 * ExternalFloorRate : 0;
    public double ElectricalCost => BuildingArea * _prices.Electrical.ElectricalPerM2;
    public double BathroomCost => HasBathrooms ? BathroomTotalCost : 0;
    public double DoorCost => DoorItems.Sum(item => item.Total);
    public double PantryCost => HasPantry ? CabinetLengthLm * PantryRate : 0;
    public double DeliveryCost => NumberOfSkids * DeliveryRate;

    private double FoundationRate => FoundationType switch
    {
        FoundationType.PackedBlock => _prices.Foundations.PackedBlockPerM2,
        FoundationType.Precast => _prices.Foundations.PrecastPerM2,
        FoundationType.ConcreteSlab => _prices.Foundations.ConcreteSlabPerM2,
        _ => 0
    };

    private double GlassRate => GlassType switch
    {
        GlassType.Single => _prices.Glass.SinglePerM2,
        GlassType.Double => _prices.Glass.DoublePerM2,
        GlassType.Curved => _prices.Glass.CurvedPerM2,
        _ => 0
    };

    private double SkidRate => SkidFinish switch
    {
        SkidFinish.Ceramic => _prices.Skids.CeramicPerM2,
        SkidFinish.Parquet => _prices.Skids.ParquetPerM2,
        SkidFinish.Pvc => _prices.Skids.PvcPerM2,
        _ => 0
    };

    private double ExteriorWallRate => ExteriorFinish switch
    {
        ExteriorFinish.Paint => _prices.Walls.ExteriorPaintPerM2,
        ExteriorFinish.AluminumCladding => _prices.Walls.ExteriorAluminumPerM2,
        _ => 0
    };

    private double ExternalFloorRate => ExternalFloorFinish switch
    {
        ExternalFloorFinish.Ceramic => _prices.ExternalFlooring.CeramicPerM2,
        ExternalFloorFinish.Wpc => _prices.ExternalFlooring.WpcPerM2,
        _ => 0
    };

    private double PantryRate => PantryFinish switch
    {
        PantryFinish.Aluminum => _prices.Pantry.AluminumPerLm,
        PantryFinish.Wood => _prices.Pantry.WoodPerLm,
        _ => 0
    };

    private double DeliveryRate => _prices.Delivery.Rates.FirstOrDefault(r => r.Emirate == SelectedEmirate)?.PerSkidCost ?? 0;

    [RelayCommand]
    private async Task ExportPdfAsync()
    {
        var summary = BuildSummary();
        var path = await App.Services.Export.ExportPdfAsync(summary);
        LastExportPath = path ?? string.Empty;
    }

    [RelayCommand]
    private async Task ExportPngAsync(object? element)
    {
        if (element is not Microsoft.UI.Xaml.UIElement uiElement)
        {
            return;
        }

        var path = await App.Services.Export.ExportPngAsync(uiElement);
        LastExportPath = path ?? string.Empty;
    }

    [RelayCommand]
    private async Task ShareWhatsAppAsync()
    {
        var uri = new Uri("https://web.whatsapp.com/");
        await Windows.System.Launcher.LaunchUriAsync(uri);

        if (!string.IsNullOrWhiteSpace(LastExportPath))
        {
            var dataPackage = new Windows.ApplicationModel.DataTransfer.DataPackage();
            dataPackage.SetText(LastExportPath);
            Windows.ApplicationModel.DataTransfer.Clipboard.SetContent(dataPackage);

            var folderPath = Path.GetDirectoryName(LastExportPath);
            if (!string.IsNullOrWhiteSpace(folderPath))
            {
                var folder = await Windows.Storage.StorageFolder.GetFolderFromPathAsync(folderPath);
                await Windows.System.Launcher.LaunchFolderAsync(folder);
            }
        }
    }

    public CalculatorSummary BuildSummary()
    {
        var summary = new CalculatorSummary();
        summary.Lines.Add(new SummaryLine("Foundations", FoundationCost));
        summary.Lines.Add(new SummaryLine("Glass", GlassCost));
        summary.Lines.Add(new SummaryLine("Skids", SkidCost));
        summary.Lines.Add(new SummaryLine("Walls", ExteriorWallCost + WetWallCost + PartitionWallCost));
        summary.Lines.Add(new SummaryLine("Roof", RoofCost + ParapetCost));
        summary.Lines.Add(new SummaryLine("False Ceiling", Ceiling6060Cost + GypsumCost));
        summary.Lines.Add(new SummaryLine("External Flooring", ExternalFloorCost));
        summary.Lines.Add(new SummaryLine("Electrical", ElectricalCost));
        summary.Lines.Add(new SummaryLine("Plumbing", BathroomCost));
        summary.Lines.Add(new SummaryLine("Internal Doors", DoorCost));
        summary.Lines.Add(new SummaryLine("Pantry", PantryCost));
        summary.Lines.Add(new SummaryLine("Delivery", DeliveryCost + CraneOffloadingCost));
        summary.Lines.Add(new SummaryLine("Extra Costs", ExtraCosts));
        summary.GrandTotal = GrandTotal;
        return summary;
    }

    public void Recalculate()
    {
        OnPropertyChanged(nameof(BuildingArea));
        OnPropertyChanged(nameof(GlassArea));
        OnPropertyChanged(nameof(ExteriorWallArea));
        OnPropertyChanged(nameof(WetWallArea));
        OnPropertyChanged(nameof(PartitionWallArea));

        OnPropertyChanged(nameof(FoundationCost));
        OnPropertyChanged(nameof(GlassCost));
        OnPropertyChanged(nameof(SkidCost));
        OnPropertyChanged(nameof(ExteriorWallCost));
        OnPropertyChanged(nameof(WetWallCost));
        OnPropertyChanged(nameof(PartitionWallCost));
        OnPropertyChanged(nameof(RoofCost));
        OnPropertyChanged(nameof(ParapetCost));
        OnPropertyChanged(nameof(Ceiling6060Cost));
        OnPropertyChanged(nameof(GypsumCost));
        OnPropertyChanged(nameof(ExternalFloorCost));
        OnPropertyChanged(nameof(ElectricalCost));
        OnPropertyChanged(nameof(BathroomCost));
        OnPropertyChanged(nameof(DoorCost));
        OnPropertyChanged(nameof(PantryCost));
        OnPropertyChanged(nameof(DeliveryCost));

        GrandTotal = FoundationCost + GlassCost + SkidCost + ExteriorWallCost + WetWallCost +
                     PartitionWallCost + RoofCost + ParapetCost + Ceiling6060Cost + GypsumCost +
                     ExternalFloorCost + ElectricalCost + BathroomCost + DoorCost + PantryCost +
                     DeliveryCost + CraneOffloadingCost + ExtraCosts;
    }

    partial void OnSelectedShapeChanged(BuildingShape value)
    {
        OnPropertyChanged(nameof(IsRectangle));
        OnPropertyChanged(nameof(IsLShape));
        OnPropertyChanged(nameof(IsUShape));
        Recalculate();
    }
    partial void OnLengthChanged(double value) => Recalculate();
    partial void OnWidthChanged(double value) => Recalculate();
    partial void OnOverallLengthChanged(double value) => Recalculate();
    partial void OnOverallWidthChanged(double value) => Recalculate();
    partial void OnCutoutLengthChanged(double value) => Recalculate();
    partial void OnCutoutWidthChanged(double value) => Recalculate();
    partial void OnCourtyardWidthChanged(double value) => Recalculate();
    partial void OnCourtyardDepthChanged(double value) => Recalculate();
    partial void OnInternalHeightChanged(double value) => Recalculate();
    partial void OnExternalHeightChanged(double value) => Recalculate();
    partial void OnFoundationTypeChanged(FoundationType value) => Recalculate();
    partial void OnGlassTypeChanged(GlassType value) => Recalculate();
    partial void OnGlassLinearMetersChanged(double value) => Recalculate();
    partial void OnGlassHeightChanged(double value) => Recalculate();
    partial void OnSkidFinishChanged(SkidFinish value) => Recalculate();
    partial void OnNumberOfSkidsChanged(int value) => Recalculate();
    partial void OnTotalPartitionsLengthChanged(double value) => Recalculate();
    partial void OnWetAreaWallsLengthChanged(double value) => Recalculate();
    partial void OnExteriorWallsLengthChanged(double value) => Recalculate();
    partial void OnExteriorFinishChanged(ExteriorFinish value) => Recalculate();
    partial void OnHasParapetChanged(bool value) => Recalculate();
    partial void OnParapetAreaM2Changed(double value) => Recalculate();
    partial void OnCeiling6060AreaM2Changed(double value) => Recalculate();
    partial void OnGypsumWithLightingAreaM2Changed(double value) => Recalculate();
    partial void OnEnableExternalFlooringChanged(bool value) => Recalculate();
    partial void OnExternalFloorAreaM2Changed(double value) => Recalculate();
    partial void OnExternalFloorFinishChanged(ExternalFloorFinish value) => Recalculate();
    partial void OnHasBathroomsChanged(bool value) => Recalculate();
    partial void OnBathroomTotalCostChanged(double value) => Recalculate();
    partial void OnHasPantryChanged(bool value) => Recalculate();
    partial void OnCabinetLengthLmChanged(double value) => Recalculate();
    partial void OnPantryFinishChanged(PantryFinish value) => Recalculate();
    partial void OnSelectedEmirateChanged(string value) => Recalculate();
    partial void OnCraneOffloadingCostChanged(double value) => Recalculate();
    partial void OnExtraCostsChanged(double value) => Recalculate();
}
