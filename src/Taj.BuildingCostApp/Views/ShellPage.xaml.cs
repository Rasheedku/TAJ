using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Taj.BuildingCostApp.ViewModels;

namespace Taj.BuildingCostApp.Views;

public sealed partial class ShellPage : Page
{
    private readonly LocalizationViewModel _localizationViewModel = new();

    public ShellPage()
    {
        InitializeComponent();
        LanguageToggle.IsOn = _localizationViewModel.UseArabic;
        FlowDirection = _localizationViewModel.FlowDirection;
        ContentFrame.Navigate(typeof(HomePage));
    }

    private void OnNavSelectionChanged(NavigationView sender, NavigationViewSelectionChangedEventArgs args)
    {
        if (args.SelectedItem is NavigationViewItem item)
        {
            switch (item.Tag?.ToString())
            {
                case "home":
                    ContentFrame.Navigate(typeof(HomePage));
                    break;
                case "calculator":
                    ContentFrame.Navigate(typeof(CalculatorPage));
                    break;
                case "quotation":
                    ContentFrame.Navigate(typeof(PlaceholderPage), "Quotation Generator");
                    break;
                case "invoice":
                    ContentFrame.Navigate(typeof(PlaceholderPage), "Invoice Generator");
                    break;
                case "pricing":
                    ContentFrame.Navigate(typeof(PricingAdminPage));
                    break;
                case "users":
                    ContentFrame.Navigate(typeof(UsersAdminPage));
                    break;
            }
        }
    }

    private void OnLanguageToggle(object sender, RoutedEventArgs e)
    {
        _localizationViewModel.UseArabic = LanguageToggle.IsOn;
        FlowDirection = _localizationViewModel.FlowDirection;
    }
}
