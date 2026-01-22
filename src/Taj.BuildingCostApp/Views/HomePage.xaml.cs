using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;

namespace Taj.BuildingCostApp.Views;

public sealed partial class HomePage : Page
{
    public HomePage()
    {
        InitializeComponent();
    }

    private void OnCalculatorClick(object sender, RoutedEventArgs e)
    {
        Frame.Navigate(typeof(CalculatorPage));
    }

    private void OnQuotationClick(object sender, RoutedEventArgs e)
    {
        Frame.Navigate(typeof(PlaceholderPage), "Quotation Generator");
    }

    private void OnInvoiceClick(object sender, RoutedEventArgs e)
    {
        Frame.Navigate(typeof(PlaceholderPage), "Invoice Generator");
    }
}
