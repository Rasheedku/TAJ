namespace Taj.BuildingCostApp.ViewModels;

public record SummaryLine(string Label, double Value);

public class CalculatorSummary
{
    public List<SummaryLine> Lines { get; } = new();
    public double GrandTotal { get; set; }
}
