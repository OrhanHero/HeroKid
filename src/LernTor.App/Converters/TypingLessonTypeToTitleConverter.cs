using System.Globalization;
using System.Windows.Data;
using LernTor.App.Localization;
using LernTor.Core.Enums;

namespace LernTor.App.Converters;

public sealed class TypingLessonTypeToTitleConverter : IValueConverter
{
    public object Convert(object? value, Type targetType, object parameter, CultureInfo culture)
    {
        var key = value switch
        {
            TypingLessonType.Grundreihe => "TypingLesson_Grundreihe",
            TypingLessonType.Oberreihe => "TypingLesson_Oberreihe",
            TypingLessonType.Unterreihe => "TypingLesson_Unterreihe",
            TypingLessonType.Zahlenreihe => "TypingLesson_Zahlenreihe",
            TypingLessonType.WoerterSilben => "TypingLesson_WoerterSilben",
            TypingLessonType.Saetze => "TypingLesson_Saetze",
            TypingLessonType.Abschluss => "TypingLesson_Abschluss",
            _ => string.Empty
        };

        return LocalizationService.Instance[key];
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotSupportedException();
    }
}