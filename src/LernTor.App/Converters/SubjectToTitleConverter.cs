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
            Subject.Naturwissenschaften => "Stage_Naturwissenschaften",
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
