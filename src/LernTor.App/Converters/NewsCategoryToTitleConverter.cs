using System.Globalization;
using System.Windows.Data;
using LernTor.App.Localization;
using LernTor.Core.Models;

namespace LernTor.App.Converters;

/// <summary>Rubrik-Enum → lokalisierter Rubrikname (DE/TR), analog zu SubjectToTitleConverter.</summary>
public sealed class NewsCategoryToTitleConverter : IValueConverter
{
    public object Convert(object? value, Type targetType, object parameter, CultureInfo culture)
    {
        var key = value switch
        {
            NewsCategory.Berlin => "News_Cat_Berlin",
            NewsCategory.Deutschland => "News_Cat_Deutschland",
            NewsCategory.Welt => "News_Cat_Welt",
            NewsCategory.Tuerkei => "News_Cat_Tuerkei",
            NewsCategory.Ki => "News_Cat_Ki",
            NewsCategory.Spiele => "News_Cat_Spiele",
            NewsCategory.Finanzen => "News_Cat_Finanzen",
            NewsCategory.Wetter => "News_Cat_Wetter",
            _ => string.Empty
        };

        return LocalizationService.Instance[key];
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotSupportedException();
    }
}

/// <summary>Schwierigkeits-Enum → lokalisierter Text mit Ampel-Symbol (🟢/🟡/🔴).</summary>
public sealed class NewsDifficultyToTitleConverter : IValueConverter
{
    public object Convert(object? value, Type targetType, object parameter, CultureInfo culture)
    {
        var key = value switch
        {
            NewsDifficulty.Leicht => "News_Diff_Leicht",
            NewsDifficulty.Mittel => "News_Diff_Mittel",
            NewsDifficulty.Schwer => "News_Diff_Schwer",
            _ => string.Empty
        };

        return LocalizationService.Instance[key];
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotSupportedException();
    }
}
