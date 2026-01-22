using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Shapes;
using Taj.BuildingCostApp.Models;
using Taj.BuildingCostApp.ViewModels;

namespace Taj.BuildingCostApp.Views;

public sealed partial class CalculatorPage : Page
{
    public CalculatorPage()
    {
        InitializeComponent();
        Loaded += OnLoaded;
    }

    private void OnLoaded(object sender, RoutedEventArgs e)
    {
        FlowDirection = App.Services.Localization.CurrentFlowDirection;
        if (DataContext is CalculatorViewModel vm)
        {
            vm.PropertyChanged += (_, __) => RenderPlan(vm);
            PlanCanvas.SizeChanged += (_, __) => RenderPlan(vm);
            RenderPlan(vm);
        }
    }

    private void RenderPlan(CalculatorViewModel vm)
    {
        PlanCanvas.Children.Clear();
        var width = Math.Max(1, PlanCanvas.ActualWidth == 0 ? PlanCanvas.Width : PlanCanvas.ActualWidth);
        var height = Math.Max(1, PlanCanvas.ActualHeight == 0 ? PlanCanvas.Height : PlanCanvas.ActualHeight);
        var padding = 20;
        var availableWidth = width - padding * 2;
        var availableHeight = height - padding * 2;

        double baseWidth = vm.SelectedShape switch
        {
            BuildingShape.Rectangle => vm.Width,
            _ => vm.OverallWidth
        };
        double baseLength = vm.SelectedShape switch
        {
            BuildingShape.Rectangle => vm.Length,
            _ => vm.OverallLength
        };

        var scale = Math.Min(availableWidth / Math.Max(1, baseLength), availableHeight / Math.Max(1, baseWidth));
        var originX = padding;
        var originY = padding;

        if (vm.SelectedShape == BuildingShape.Rectangle)
        {
            DrawRect(originX, originY, vm.Length * scale, vm.Width * scale, Microsoft.UI.Colors.SteelBlue);
            DrawDimensionLine(originX, originY + vm.Width * scale + 6, originX + vm.Length * scale, originY + vm.Width * scale + 6, $"{vm.Length:N1} m");
            DrawDimensionLine(originX - 6, originY, originX - 6, originY + vm.Width * scale, $"{vm.Width:N1} m", vertical: true);
        }
        else if (vm.SelectedShape == BuildingShape.LShape)
        {
            DrawRect(originX, originY, vm.OverallLength * scale, vm.OverallWidth * scale, Microsoft.UI.Colors.SteelBlue);
            DrawRect(originX + (vm.OverallLength - vm.CutoutLength) * scale, originY, vm.CutoutLength * scale, vm.CutoutWidth * scale, Microsoft.UI.Colors.LightGray);
            DrawDimensionLine(originX, originY + vm.OverallWidth * scale + 6, originX + vm.OverallLength * scale, originY + vm.OverallWidth * scale + 6, $"{vm.OverallLength:N1} m");
            DrawDimensionLine(originX - 6, originY, originX - 6, originY + vm.OverallWidth * scale, $"{vm.OverallWidth:N1} m", vertical: true);
        }
        else if (vm.SelectedShape == BuildingShape.UShape)
        {
            DrawRect(originX, originY, vm.OverallLength * scale, vm.OverallWidth * scale, Microsoft.UI.Colors.SteelBlue);
            DrawRect(originX + (vm.OverallLength - vm.CourtyardWidth) / 2 * scale, originY, vm.CourtyardWidth * scale, vm.CourtyardDepth * scale, Microsoft.UI.Colors.LightGray);
            DrawDimensionLine(originX, originY + vm.OverallWidth * scale + 6, originX + vm.OverallLength * scale, originY + vm.OverallWidth * scale + 6, $"{vm.OverallLength:N1} m");
            DrawDimensionLine(originX - 6, originY, originX - 6, originY + vm.OverallWidth * scale, $"{vm.OverallWidth:N1} m", vertical: true);
        }

        var label = new TextBlock
        {
            Text = $"Area: {vm.BuildingArea:N2} m²",
            Foreground = new SolidColorBrush(Colors.Black)
        };
        Canvas.SetLeft(label, padding);
        Canvas.SetTop(label, height - padding - 20);
        PlanCanvas.Children.Add(label);
    }

    private void DrawRect(double x, double y, double width, double height, Microsoft.UI.Color color)
    {
        var rect = new Rectangle
        {
            Width = Math.Max(1, width),
            Height = Math.Max(1, height),
            Fill = new SolidColorBrush(color),
            Stroke = new SolidColorBrush(Microsoft.UI.Colors.White),
            StrokeThickness = 2
        };
        Canvas.SetLeft(rect, x);
        Canvas.SetTop(rect, y);
        PlanCanvas.Children.Add(rect);
    }

    private void DrawDimensionLine(double x1, double y1, double x2, double y2, string text, bool vertical = false)
    {
        var line = new Line
        {
            X1 = x1,
            Y1 = y1,
            X2 = x2,
            Y2 = y2,
            Stroke = new SolidColorBrush(Microsoft.UI.Colors.DimGray),
            StrokeThickness = 1
        };
        PlanCanvas.Children.Add(line);

        var label = new TextBlock
        {
            Text = text,
            Foreground = new SolidColorBrush(Microsoft.UI.Colors.DimGray),
            FontSize = 12
        };

        if (vertical)
        {
            Canvas.SetLeft(label, x1 - 18);
            Canvas.SetTop(label, (y1 + y2) / 2 - 8);
        }
        else
        {
            Canvas.SetLeft(label, (x1 + x2) / 2 - 20);
            Canvas.SetTop(label, y1 + 2);
        }

        PlanCanvas.Children.Add(label);
    }
}
