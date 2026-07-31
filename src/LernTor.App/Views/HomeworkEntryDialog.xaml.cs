using System.Linq;
using System.Windows;
using LernTor.Core.Enums;

namespace LernTor.App.Views;

/// <summary>
/// Eingabefenster für eine Hausaufgabe. Wird vom Kind (Willkommensbildschirm) und vom
/// Eltern-Bereich benutzt; wer der Urheber ist, entscheidet der Aufrufer, nicht dieses Fenster.
/// </summary>
public partial class HomeworkEntryDialog : Window
{
    public HomeworkEntryDialog()
    {
        InitializeComponent();

        // News ist kein Schulfach, in dem es Hausaufgaben gibt.
        SubjectBox.ItemsSource = System.Enum.GetValues<Subject>()
            .Where(s => s != Subject.News)
            .ToList();
        SubjectBox.SelectedItem = Subject.Mathematik;

        // Vorgabe morgen: der mit Abstand haeufigste Fall bei Hausaufgaben.
        DateBox.SelectedDate = System.DateTime.Today.AddDays(1);
    }

    public Subject SelectedSubject { get; private set; } = Subject.Mathematik;

    public System.DateOnly SelectedDate { get; private set; }

    public string EnteredDescription { get; private set; } = string.Empty;

    private void Save_Click(object sender, RoutedEventArgs e)
    {
        if (DescriptionBox.Text.Trim().Length == 0)
        {
            ShowError("Bitte eintragen, was zu tun ist.");
            return;
        }

        if (DateBox.SelectedDate is not { } date)
        {
            ShowError("Bitte ein Datum auswählen.");
            return;
        }

        // Anders als bei Klausuren ist ein Datum in der Vergangenheit hier ZULAESSIG: eine
        // vergessene Hausaufgabe von gestern nachzutragen ist ein voellig normaler Fall.
        SelectedSubject = SubjectBox.SelectedItem is Subject subject ? subject : Subject.Mathematik;
        SelectedDate = System.DateOnly.FromDateTime(date);
        EnteredDescription = DescriptionBox.Text.Trim();

        DialogResult = true;
    }

    private void ShowError(string message)
    {
        ErrorText.Text = message;
        ErrorText.Visibility = Visibility.Visible;
    }

    private void Cancel_Click(object sender, RoutedEventArgs e) => DialogResult = false;
}
