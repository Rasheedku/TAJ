using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Data;

namespace Taj.BuildingCostApp.Converters;

public class InverseBooleanToVisibilityConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, string language)
    {
        return value is bool boolValue && !boolValue ? Visibility.Visible : Visibility.Collapsed;
    }

    public object ConvertBack(object value, Type targetType, object parameter, string language)
    {
        return value is Visibility visibility && visibility != Visibility.Visible;
    }
}
