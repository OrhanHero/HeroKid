using System.Globalization;
using System.Windows.Data;
using LernTor.App.Localization;
using LernTor.Core.Enums;
using LernTor.Core.Models;

namespace LernTor.App.Converters;

/// <summary>
/// Konvertiert eine TypingLesson in die lokalisierte Anleitung (DE/TR basierend auf CurrentLanguage).
/// </summary>
public sealed class LocalizedInstructionConverter : IValueConverter
{
    public object Convert(object? value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value is TypingLesson lesson)
        {
            return LocalizationService.Instance.CurrentLanguage switch
            {
                AppLanguage.Tuerkisch => lesson.InstructionTr,
                _ => lesson.InstructionDe
            };
        }
        return string.Empty;
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        => throw new NotSupportedException();
}

/// <summary>
/// Konvertiert bool (IsCompleted) in SuccessBrush (grün) oder ErrorBrush (rot).
/// </summary>
public sealed class BoolToSuccessBrushConverter : IValueConverter
{
    public object Convert(object? value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value is bool isCompleted)
        {
            var resourceKey = isCompleted ? "SuccessBrush" : "ErrorBrush";
            return System.Windows.Application.Current.TryFindResource(resourceKey)
                ?? (isCompleted ? System.Windows.Media.Brushes.Green : System.Windows.Media.Brushes.Red);
        }
        return System.Windows.Application.Current.TryFindResource("TextSecondaryBrush")
            ?? System.Windows.Media.Brushes.Gray;
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        => throw new NotSupportedException();
}