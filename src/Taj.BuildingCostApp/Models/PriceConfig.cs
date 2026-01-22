namespace Taj.BuildingCostApp.Models;

public class PriceConfig
{
    public int Version { get; set; } = 1;
    public FoundationPrices Foundations { get; set; } = new();
    public GlassPrices Glass { get; set; } = new();
    public SkidPrices Skids { get; set; } = new();
    public WallPrices Walls { get; set; } = new();
    public RoofPrices Roof { get; set; } = new();
    public CeilingPrices Ceilings { get; set; } = new();
    public ExternalFlooringPrices ExternalFlooring { get; set; } = new();
    public ElectricalPrices Electrical { get; set; } = new();
    public PantryPrices Pantry { get; set; } = new();
    public DoorPricing Doors { get; set; } = new();
    public DeliveryPricing Delivery { get; set; } = new();
}

public class FoundationPrices
{
    public double PackedBlockPerM2 { get; set; } = 120;
    public double PrecastPerM2 { get; set; } = 150;
    public double ConcreteSlabPerM2 { get; set; } = 200;
}

public class GlassPrices
{
    public double SinglePerM2 { get; set; } = 300;
    public double DoublePerM2 { get; set; } = 450;
    public double CurvedPerM2 { get; set; } = 650;
}

public class SkidPrices
{
    public double CeramicPerM2 { get; set; } = 70;
    public double ParquetPerM2 { get; set; } = 110;
    public double PvcPerM2 { get; set; } = 55;
}

public class WallPrices
{
    public double ExteriorPaintPerM2 { get; set; } = 40;
    public double ExteriorAluminumPerM2 { get; set; } = 140;
    public double WetAreaPerM2 { get; set; } = 120;
    public double PartitionPerM2 { get; set; } = 80;
}

public class RoofPrices
{
    public double RoofPerM2 { get; set; } = 95;
    public double ParapetPerM2 { get; set; } = 130;
}

public class CeilingPrices
{
    public double Ceiling6060PerM2 { get; set; } = 35;
    public double GypsumWithLightingPerM2 { get; set; } = 120;
}

public class ExternalFlooringPrices
{
    public double CeramicPerM2 { get; set; } = 65;
    public double WpcPerM2 { get; set; } = 110;
}

public class ElectricalPrices
{
    public double ElectricalPerM2 { get; set; } = 90;
}

public class PantryPrices
{
    public double AluminumPerLm { get; set; } = 420;
    public double WoodPerLm { get; set; } = 520;
}

public class DoorTypePrice
{
    public string Name { get; set; } = string.Empty;
    public double Price { get; set; }
}

public class DoorPricing
{
    public List<DoorTypePrice> DoorTypes { get; set; } = new()
    {
        new DoorTypePrice { Name = "90x210", Price = 650 },
        new DoorTypePrice { Name = "100x210", Price = 720 }
    };
}

public class DeliveryRate
{
    public string Emirate { get; set; } = string.Empty;
    public double PerSkidCost { get; set; }
}

public class DeliveryPricing
{
    public List<DeliveryRate> Rates { get; set; } = new()
    {
        new DeliveryRate { Emirate = "Abu Dhabi", PerSkidCost = 180 },
        new DeliveryRate { Emirate = "Dubai", PerSkidCost = 220 },
        new DeliveryRate { Emirate = "Sharjah", PerSkidCost = 200 },
        new DeliveryRate { Emirate = "Ajman", PerSkidCost = 190 },
        new DeliveryRate { Emirate = "Umm Al Quwain", PerSkidCost = 210 },
        new DeliveryRate { Emirate = "Ras Al Khaimah", PerSkidCost = 230 },
        new DeliveryRate { Emirate = "Fujairah", PerSkidCost = 240 },
        new DeliveryRate { Emirate = "Al Ain", PerSkidCost = 200 }
    };
}
