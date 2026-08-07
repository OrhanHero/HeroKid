using System.Linq;
using System.Windows;
using LernTor.Core.Enums;

namespace LernTor.App.Views;

/// <summary>
/// Eingabefenster für einen Klausurtermin. Wird sowohl vom Kind (Willkommensbildschirm) als auch
/// vom Eltern-Bereich benutzt; wer der Urheber ist, entscheidet der Aufrufer, nicht dieses Fenster.
/// </summary>
public partial class ExamEntryDialog : Window
{
    public ExamEntryDialog()
    {
        InitializeComponent();

        // Nur echte Schulfaecher (siehe SchoolSubjects), und mit ihrem ANGEZEIGTEN Namen.
        // Vorher standen Tipptrainer, KI-Bereich, Fuehrerschein und Erste Hilfe mit in der
        // Liste - "Klausur in Fuehrerschein" ist kein Termin, den es gibt. Und die Namen kamen
        // roh aus der Aufzaehlung: dort stand der Enum-Name "Tuerkisch" statt "Türkisch" und
        // "Itg" statt "Medienbildung (ITG)".
        var faecher = SubjectChoice.SchoolSubjectList();
        SubjectBox.ItemsSource = faecher;
        SubjectBox.DisplayMemberPath = nameof(SubjectChoice.Title);
        SubjectBox.SelectedItem = faecher.FirstOrDefault(eintrag => eintrag.Subject == Subject.Mathematik);

        // Vorgabe eine Woche voraus: genau der Punkt, ab dem die Übungen anziehen.
        DateBox.SelectedDate = System.DateTime.Today.AddDays(7);
    }

    public Subject SelectedSubject { get; private set; } = Subject.Mathematik;

    public System.DateOnly SelectedDate { get; private set; }

    public string EnteredTitle { get; private set; } = string.Empty;

    public string EnteredTopics { get; private set; } = string.Empty;

    private void Save_Click(object sender, RoutedEventArgs e)
    {
        if (DateBox.SelectedDate is not { } date)
        {
            ShowError("Bitte ein Datum auswählen.");
            return;
        }

        var chosen = System.DateOnly.FromDateTime(date);
        if (chosen < System.DateOnly.FromDateTime(System.DateTime.Today))
        {
            // Ein Termin in der Vergangenheit ist immer ein Vertipper - und er wuerde die
            // Lern-Gewichtung nie ausloesen, also faende ihn niemand wieder.
            ShowError("Das Datum liegt in der Vergangenheit.");
            return;
        }

        SelectedSubject = SubjectBox.SelectedItem is SubjectChoice choice ? choice.Subject : Subject.Mathematik;
        SelectedDate = chosen;
        EnteredTitle = TitleBox.Text.Trim();
        EnteredTopics = TopicsBox.Text.Trim();

        DialogResult = true;
    }

    private void ShowError(string message)
    {
        ErrorText.Text = message;
        ErrorText.Visibility = Visibility.Visible;
    }

    private void Cancel_Click(object sender, RoutedEventArgs e) => DialogResult = false;
}
