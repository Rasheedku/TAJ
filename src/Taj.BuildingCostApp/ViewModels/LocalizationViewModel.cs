using CommunityToolkit.Mvvm.ComponentModel;
using Taj.BuildingCostApp.Services;

namespace Taj.BuildingCostApp.ViewModels;

public partial class LocalizationViewModel : ObservableObject
{
    private readonly LocalizationService _localization;

    public LocalizationViewModel()
    {
        _localization = App.Services.Localization;
        _useArabic = _localization.CurrentLanguage.StartsWith("ar", StringComparison.OrdinalIgnoreCase);
    }

    [ObservableProperty]
    private bool _useArabic;

    public Microsoft.UI.Xaml.FlowDirection FlowDirection => _localization.CurrentFlowDirection;

    partial void OnUseArabicChanged(bool value)
    {
        _ = _localization.ToggleLanguageAsync(value);
        OnPropertyChanged(nameof(FlowDirection));
    }
}
