using System.Globalization;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace LernTor.App.Controls;

/// <summary>
/// Rechenhilfe bei offenen Mathematik-Aufgaben mit zwei Modi:
/// - Dezimal-Taschenrechner (Vier-Grundrechenarten plus Quadratwurzel/Quadrieren, z.B. für den Satz
///   des Pythagoras oder binomische Formeln) - klassisches Tastenverhalten wie ein physischer
///   Taschenrechner, kein Ausdrucks-Parser.
/// - Bruchrechner (a/b [Rechenart] c/d = Ergebnis, automatisch gekürzt) für die Bruchrechnung-Themen.
/// Das aktuelle Ergebnis kann per "↵"-Taste an der Cursor-Position ins Antwortfeld der Aufgabe
/// eingefügt werden.
/// </summary>
public partial class CalculatorControl : UserControl
{
    private double _storedValue;
    private char? _pendingOperator;
    private bool _startNewEntry = true;

    private char _fractionOperator = '+';
    private string _fractionResult = "0";

    /// <summary>Wird ausgelöst, wenn das Kind das aktuelle Ergebnis ins Antwortfeld übernehmen möchte.</summary>
    public event EventHandler<string>? InsertRequested;

    public CalculatorControl()
    {
        InitializeComponent();
        HighlightFractionOperator();
    }

    // ---- Modus-Umschaltung ----

    private void DecimalMode_Click(object sender, RoutedEventArgs e)
    {
        DecimalPanel.Visibility = Visibility.Visible;
        FractionPanel.Visibility = Visibility.Collapsed;
    }

    private void FractionMode_Click(object sender, RoutedEventArgs e)
    {
        DecimalPanel.Visibility = Visibility.Collapsed;
        FractionPanel.Visibility = Visibility.Visible;
    }

    // ---- Dezimal-Taschenrechner ----

    private double CurrentValue => double.Parse(DisplayText.Text.Replace(',', '.'), CultureInfo.InvariantCulture);

    private void SetDisplay(double value)
    {
        DisplayText.Text = value.ToString("0.##########", CultureInfo.InvariantCulture).Replace('.', ',');
    }

    private void Digit_Click(object sender, RoutedEventArgs e)
    {
        var digit = ((Button)sender).Content.ToString()!;

        if (_startNewEntry || DisplayText.Text == "0")
        {
            DisplayText.Text = digit;
            _startNewEntry = false;
        }
        else
        {
            DisplayText.Text += digit;
        }
    }

    private void Decimal_Click(object sender, RoutedEventArgs e)
    {
        if (_startNewEntry)
        {
            DisplayText.Text = "0,";
            _startNewEntry = false;
            return;
        }

        if (!DisplayText.Text.Contains(','))
        {
            DisplayText.Text += ",";
        }
    }

    private void ToggleSign_Click(object sender, RoutedEventArgs e)
    {
        SetDisplay(-CurrentValue);
    }

    private void Percent_Click(object sender, RoutedEventArgs e)
    {
        SetDisplay(CurrentValue / 100);
        _startNewEntry = true;
    }

    private void SquareRoot_Click(object sender, RoutedEventArgs e)
    {
        var value = CurrentValue;
        SetDisplay(value < 0 ? 0 : Math.Sqrt(value));
        _startNewEntry = true;
    }

    private void Square_Click(object sender, RoutedEventArgs e)
    {
        var value = CurrentValue;
        SetDisplay(value * value);
        _startNewEntry = true;
    }

    private void Clear_Click(object sender, RoutedEventArgs e)
    {
        DisplayText.Text = "0";
        _storedValue = 0;
        _pendingOperator = null;
        _startNewEntry = true;
    }

    private void Operator_Click(object sender, RoutedEventArgs e)
    {
        ApplyPendingOperation();
        _pendingOperator = (((Button)sender).Tag as string ?? ((Button)sender).Content.ToString())![0];
        _startNewEntry = true;
    }

    private void Equals_Click(object sender, RoutedEventArgs e)
    {
        ApplyPendingOperation();
        _pendingOperator = null;
        _startNewEntry = true;
    }

    private void ApplyPendingOperation()
    {
        var current = CurrentValue;

        if (_pendingOperator is null)
        {
            _storedValue = current;
            return;
        }

        _storedValue = _pendingOperator switch
        {
            '+' => _storedValue + current,
            '-' => _storedValue - current,
            '*' => _storedValue * current,
            '/' => current == 0 ? _storedValue : _storedValue / current,
            _ => current
        };

        SetDisplay(_storedValue);
    }

    private void Insert_Click(object sender, RoutedEventArgs e)
    {
        InsertRequested?.Invoke(this, DisplayText.Text);
    }

    // ---- Bruchrechner ----

    private static long Gcd(long a, long b)
    {
        a = Math.Abs(a);
        b = Math.Abs(b);
        while (b != 0)
        {
            (a, b) = (b, a % b);
        }

        return a == 0 ? 1 : a;
    }

    private static (long Numerator, long Denominator) Reduce(long numerator, long denominator)
    {
        if (denominator < 0)
        {
            numerator = -numerator;
            denominator = -denominator;
        }

        var g = Gcd(numerator, denominator);
        return (numerator / g, denominator / g);
    }

    private static bool TryReadFraction(TextBox numeratorBox, TextBox denominatorBox, out long numerator, out long denominator)
    {
        var okNumerator = long.TryParse(numeratorBox.Text, NumberStyles.Integer, CultureInfo.InvariantCulture, out numerator);
        var okDenominator = long.TryParse(denominatorBox.Text, NumberStyles.Integer, CultureInfo.InvariantCulture, out denominator);
        return okNumerator && okDenominator && denominator != 0;
    }

    private void FractionOperator_Click(object sender, RoutedEventArgs e)
    {
        _fractionOperator = (((Button)sender).Tag as string)![0];
        HighlightFractionOperator();
    }

    private void HighlightFractionOperator()
    {
        var selectedBrush = (Brush)FindResource("PrimaryBrush");
        var defaultBrush = (Brush)FindResource("CardBrush");

        FracPlusButton.Background = _fractionOperator == '+' ? selectedBrush : defaultBrush;
        FracMinusButton.Background = _fractionOperator == '-' ? selectedBrush : defaultBrush;
        FracTimesButton.Background = _fractionOperator == '*' ? selectedBrush : defaultBrush;
        FracDivideButton.Background = _fractionOperator == '/' ? selectedBrush : defaultBrush;
    }

    private void FractionEquals_Click(object sender, RoutedEventArgs e)
    {
        if (!TryReadFraction(Numerator1Box, Denominator1Box, out var n1, out var d1) ||
            !TryReadFraction(Numerator2Box, Denominator2Box, out var n2, out var d2))
        {
            FractionResultText.Text = "?";
            return;
        }

        long resultNumerator;
        long resultDenominator;

        switch (_fractionOperator)
        {
            case '+':
                resultNumerator = n1 * d2 + n2 * d1;
                resultDenominator = d1 * d2;
                break;
            case '-':
                resultNumerator = n1 * d2 - n2 * d1;
                resultDenominator = d1 * d2;
                break;
            case '*':
                resultNumerator = n1 * n2;
                resultDenominator = d1 * d2;
                break;
            case '/':
                if (n2 == 0)
                {
                    FractionResultText.Text = "?";
                    return;
                }

                resultNumerator = n1 * d2;
                resultDenominator = d1 * n2;
                break;
            default:
                return;
        }

        var (rn, rd) = Reduce(resultNumerator, resultDenominator);
        _fractionResult = rd == 1 ? $"{rn}" : $"{rn}/{rd}";
        FractionResultText.Text = _fractionResult;
    }

    private void FractionInsert_Click(object sender, RoutedEventArgs e)
    {
        InsertRequested?.Invoke(this, _fractionResult);
    }
}
