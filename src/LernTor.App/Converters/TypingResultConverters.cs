using System;
using System.Globalization;
using System.Windows.Data;
using LernTor.App.Localization;

namespace LernTor.App.Converters;

/// <summary>Konvertiert true/false zu Emoji für Tipp-Ergebnis.</summary>
public sealed class BoolToResultEmojiConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value is bool passed)
        {
            return passed ? "🎉" : "😅";
        }
        return "😐";
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        => throw new NotSupportedException();
}

/// <summary>Konvert true/false zu Ergebnis-Titel (Geschafft/Nicht ganz).</summary>
public sealed class BoolToResultTitleConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value is bool passed)
        {
            var loc = LocalizationService.Instance;
            return passed ? loc["Typing_ResultPassed"] : loc["Typing_ResultFailed"];
        }
        return string.Empty;
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        => throw new NotSupportedException();
}