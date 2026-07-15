using System.Globalization;
using System.Windows.Data;
using LernTor.App.Localization;
using LernTor.Core.Enums;

namespace LernTor.App.Converters;

public sealed class SubjectToTitleConverter : IValueConverter
{
    public object Convert(object? value, Type targetType, object parameter, CultureInfo culture)
    {
        var key = value switch
        {
            Subject.Mathematik => "Stage_Mathematik",
            Subject.Deutsch => "Stage_Deutsch",
            Subject.Tuerkisch => "Stage_Tuerkisch",
            Subject.Englisch => "Stage_Englisch",
            Subject.Biologie => "Stage_Biologie",
            Subject.Chemie => "Stage_Chemie",
            Subject.Physik => "Stage_Physik",
            Subject.Geschichte => "Stage_Geschichte",
            Subject.Gewi => "Stage_Gewi",
            Subject.Politik => "Stage_Politik",
            Subject.Geo => "Stage_Geo",
            Subject.Ethik => "Stage_Ethik",
            Subject.Kunst => "Stage_Kunst",
            Subject.Musik => "Stage_Musik",
            Subject.Itg => "Stage_Itg",
            Subject.Tippen => "Stage_Tippen",
            Subject.News => "Stage_News",
            _ => string.Empty
        };

        return LocalizationService.Instance[key];
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotSupportedException();
    }
}
