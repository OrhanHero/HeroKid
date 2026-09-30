using System.Globalization;
using System.Windows.Data;
using LernTor.App.Localization;
using LernTor.Core.Enums;

namespace LernTor.App.Converters;

/// <summary>Klassenstufe als lesbarer Text („Klasse 10“ statt „Klasse10“) für Auswahllisten im
/// Eltern-Bereich - vorher zeigten sie den Namen aus dem Code (Testlauf vom 30.09.2026).</summary>
public sealed class GradeLevelToTitleConverter : IValueConverter
{
    public object Convert(object? value, Type targetType, object parameter, CultureInfo culture) =>
        value is GradeLevel stufe
            ? string.Format(LocalizationService.Instance["Grade_Title"], (int)stufe)
            : string.Empty;

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) =>
        throw new NotSupportedException();
}

/// <summary>Fragetyp als lesbarer Text („Offene Frage“ statt „OpenText“).</summary>
public sealed class QuestionTypeToTitleConverter : IValueConverter
{
    public object Convert(object? value, Type targetType, object parameter, CultureInfo culture)
    {
        var key = value switch
        {
            QuestionType.MultipleChoice => "QuestionType_MultipleChoice",
            QuestionType.TrueFalse => "QuestionType_TrueFalse",
            QuestionType.OpenText => "QuestionType_OpenText",
            QuestionType.Diktat => "QuestionType_Diktat",
            _ => null
        };

        return key is null ? value?.ToString() ?? string.Empty : LocalizationService.Instance[key];
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) =>
        throw new NotSupportedException();
}
