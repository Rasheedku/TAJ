using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;

namespace Taj.BuildingCostApp.Views;

public sealed partial class PricingAdminPage : Page
{
    public PricingAdminPage()
    {
        InitializeComponent();
        Loaded += OnLoaded;
    }

    private void OnLoaded(object sender, RoutedEventArgs e)
    {
        FlowDirection = App.Services.Localization.CurrentFlowDirection;
    }
}
