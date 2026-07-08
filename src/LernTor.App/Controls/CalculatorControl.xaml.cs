using System.Globalization;
using System.Windows;
using System.Windows.Controls;

namespace LernTor.App.Controls;

/// <summary>
/// Einfacher Vier-Grundrechenarten-Taschenrechner als Rechenhilfe bei offenen Mathematik-Aufgaben.
/// Rein zum Rechnen gedacht (klassisches Tastenverhalten wie ein physischer Taschenrechner, kein
/// Ausdrucks-Parser) - das aktuelle Ergebnis kann per "↵"-Taste an der Cursor-Position ins
/// Antwortfeld der Aufgabe eingefügt werden.
/// </summary>
public partial class CalculatorControl : UserControl
{
    private double _storedValue;
    private char? _pendingOperator;
    private bool _startNewEntry = true;

    /// <summary>Wird ausgelöst, wenn das Kind das aktuelle Ergebnis ins Antwortfeld übernehmen möchte.</summary>
    public event EventHandler<string>? InsertRequested;

    public CalculatorControl()
    {
        InitializeComponent();
    }

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
}
