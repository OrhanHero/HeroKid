using System.Globalization;
using System.Windows;
using System.Windows.Data;
using System.Windows.Media;
using LernTor.Core.Enums;

namespace LernTor.App.Converters;

public sealed class SubjectToBrushConverter : IValueConverter
{
    public object Convert(object? value, Type targetType, object parameter, CultureInfo culture)
    {
        var resourceKey = value switch
        {
            Subject.Mathematik => "MathBrush",
            Subject.Deutsch => "GermanBrush",
            Subject.Tuerkisch => "TurkishBrush",
            Subject.Naturwissenschaften => "ScienceBrush",
            Subject.News => "NewsBrush",
            _ => "PrimaryBrush"
        };

        return Application.Current.TryFindResource(resourceKey) as Brush ?? Brushes.Black;
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotSupportedException();
    }
}
