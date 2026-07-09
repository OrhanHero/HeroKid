using System.Globalization;
using System.Windows.Data;
using LernTor.Core.Enums;

namespace LernTor.App.Converters;

/// <summary>
/// Für die Provider-Auswahl (RadioButtons) im Eltern-Bereich - genutzt sowohl für den Lehrer-Import-
/// als auch den KI-Lernchat-Anbieter: vergleicht den gebundenen <see cref="LlmProvider"/>-Wert mit dem
/// als ConverterParameter übergebenen Enum-Namen (z.B. "NotebookLm"/"LocalLlm") und liefert/parst ihn
/// als Bool für RadioButton.IsChecked.
/// </summary>
public sealed class LlmProviderToBooleanConverter : IValueConverter
{
    public object Convert(object? value, Type targetType, object? parameter, CultureInfo culture) =>
        value is LlmProvider provider && parameter is string name && provider.ToString() == name;

    public object? ConvertBack(object value, Type targetType, object? parameter, CultureInfo culture) =>
        value is true && parameter is string name && Enum.TryParse<LlmProvider>(name, out var provider)
            ? provider
            : Binding.DoNothing;
}
