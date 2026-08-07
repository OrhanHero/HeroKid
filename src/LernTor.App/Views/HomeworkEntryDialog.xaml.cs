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

        // Nur echte Schulfaecher (siehe SchoolSubjects) und mit ihrem ANGEZEIGTEN Namen:
        // fuer den Tipptrainer, den KI-Bereich, den Fuehrerschein und Erste Hilfe gibt die
        // Schule nichts auf, und die Namen kamen roh aus der Aufzaehlung.
        var faecher = SubjectChoice.SchoolSubjectList();
        SubjectBox.ItemsSource = faecher;
        SubjectBox.DisplayMemberPath = nameof(SubjectChoice.Title);
        SubjectBox.SelectedItem = faecher.FirstOrDefault(eintrag => eintrag.Subject == Subject.Mathematik);

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
        SelectedSubject = SubjectBox.SelectedItem is SubjectChoice choice ? choice.Subject : Subject.Mathematik;
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
