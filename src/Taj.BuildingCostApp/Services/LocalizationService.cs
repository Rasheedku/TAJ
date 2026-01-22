using Microsoft.UI.Xaml;
using Taj.BuildingCostApp.Models;

namespace Taj.BuildingCostApp.Services;

public class LocalizationService
{
    private readonly SettingsService _settings;

    public LocalizationService(SettingsService settings)
    {
        _settings = settings;
    }

    public string CurrentLanguage => _settings.Settings.Language;
    public FlowDirection CurrentFlowDirection => CurrentLanguage.StartsWith("ar", StringComparison.OrdinalIgnoreCase)
        ? FlowDirection.RightToLeft
        : FlowDirection.LeftToRight;

    public Task InitializeAsync() => ApplyLanguageAsync(_settings.Settings.Language);

    public async Task ToggleLanguageAsync(bool useArabic)
    {
        var language = useArabic ? "ar-SA" : "en-US";
        await ApplyLanguageAsync(language);
    }

    public async Task ApplyLanguageAsync(string language)
    {
        _settings.Settings.Language = language;
        Windows.Globalization.ApplicationLanguages.PrimaryLanguageOverride = language;
        await _settings.SaveAsync();
    }
}
