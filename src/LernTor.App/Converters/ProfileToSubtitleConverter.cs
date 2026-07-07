using System.Globalization;
using System.Windows.Data;
using LernTor.App.Localization;
using LernTor.Core.Models;

namespace LernTor.App.Converters;

/// <summary>Formt "15 Jahre, Klasse 9a" bzw. reduzierte Varianten, falls Alter/Klasse fehlen.</summary>
public sealed class ProfileToSubtitleConverter : IValueConverter
{
    public object Convert(object? value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value is not StudentProfile profile)
        {
            return string.Empty;
        }

        var gradeText = ((int)profile.GradeLevel).ToString();
        var classText = string.IsNullOrWhiteSpace(profile.ClassLabel) ? gradeText : profile.ClassLabel;

        if (profile.Age is int age)
        {
            var format = LocalizationService.Instance["Profile_AgeAndClass"];
            return string.Format(format, age, classText);
        }

        return $"Klasse {classText}";
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotSupportedException();
    }
}
