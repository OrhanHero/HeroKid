using System.Windows;
using Xunit;

namespace LernTor.UiTests;

/// <summary>
/// Instanziiert jede parameterlose View/jedes Control einmal MIT geladenen App-Ressourcen
/// (Colors.xaml/Styles.xaml/Converter) und erzwingt einen Layout-Durchlauf. Fängt damit die
/// XamlParseException-Bug-Klasse aus den "Hard-won gotchas" (StaticResource-Auflösung über
/// Geschwister-Dictionaries, kaputte Styles/Template-Referenzen), die sauber kompiliert und
/// erst beim Laden zur Laufzeit crasht - unsichtbar für alle Unit-Tests in LernTor.Tests.
/// </summary>
public sealed class XamlLoadTests
{
    private static bool _appResourcesLoaded;

    /// <summary>
    /// Lädt die App.xaml-Ressourcen genau einmal in Application.Current. WPF erlaubt nur eine
    /// Application pro Prozess - deshalb ein Guard statt Fixture (alle Tests dieser Klasse
    /// laufen ohnehin sequenziell auf dem WPF-STA-Thread von Xunit.StaFact).
    /// </summary>
    private static void EnsureAppResourcesLoaded()
    {
        if (_appResourcesLoaded)
        {
            return;
        }

        if (Application.Current is null)
        {
            // Voll qualifiziert: der unqualifizierte Name "App" kollidiert mit dem Namespace
            // LernTor.App (CS0118).
            var app = new LernTor.App.App();
            app.InitializeComponent();
        }

        _appResourcesLoaded = true;
    }

    public static TheoryData<Type> AllParameterlessViews => new()
    {
        typeof(LernTor.App.Views.WelcomeView),
        typeof(LernTor.App.Views.ProgressOverviewView),
        typeof(LernTor.App.Views.ProfileSelectionView),
        typeof(LernTor.App.Views.PauseModeView),
        typeof(LernTor.App.Views.ReadingView),
        typeof(LernTor.App.Views.WritingView),
        typeof(LernTor.App.Views.NewsView),
        typeof(LernTor.App.Views.ExerciseView),
        typeof(LernTor.App.Views.KiBereichView),
        typeof(LernTor.App.Views.FinalQuizView),
        typeof(LernTor.App.Views.ResultView),
        typeof(LernTor.App.Views.TypingDashboardView),
        typeof(LernTor.App.Views.TypingExerciseView),
        typeof(LernTor.App.Views.TypingLessonCompleteView),
        typeof(LernTor.App.Views.DrivingDashboardView),
        typeof(LernTor.App.Views.SignQuizView),
        typeof(LernTor.App.Views.SignFlashcardView),
        typeof(LernTor.App.Views.TheoryHubView),
        typeof(LernTor.App.Views.TheoryQuestionView),
        typeof(LernTor.App.Views.TheoryResultView),
        typeof(LernTor.App.Views.CourseOverviewView),
        typeof(LernTor.App.Views.CourseLessonView),
        typeof(LernTor.App.Controls.QuestionCard),
        typeof(LernTor.App.Controls.CalculatorControl),
    };

    [WpfTheory]
    [MemberData(nameof(AllParameterlessViews))]
    public void View_laedt_ohne_XamlParseException_und_uebersteht_einen_Layout_Durchlauf(Type viewType)
    {
        EnsureAppResourcesLoaded();

        // Wirft XamlParseException, wenn Ressourcen/Styles/Bindings der View kaputt sind.
        var element = (FrameworkElement)Activator.CreateInstance(viewType)!;

        // Layout erzwingen, damit Styles/Templates tatsächlich angewendet werden - erst dabei
        // fliegen einige der Laufzeitfehler (z.B. Template-Referenzen auf fehlende Ressourcen).
        element.Measure(new Size(1920, 1080));
        element.Arrange(new Rect(0, 0, 1920, 1080));

        Assert.True(element.IsMeasureValid);
    }

    /// <summary>
    /// "Mein Fortschritt" MIT Daten: der parameterlose Ladetest oben instanziiert die
    /// Item-Templates nie (ohne DataContext gibt es keine Einträge) - die Stufen-Etiketten mit
    /// ihren DataTriggern auf ein Enum würden dort also gar nicht geladen. Hier wird für jede
    /// Stufe ein Thema gerendert.
    /// </summary>
    [WpfFact]
    public void Fortschritt_rendert_alle_Stufen_mit_Daten()
    {
        EnsureAppResourcesLoaded();

        var themen = new[]
        {
            new LernTor.Core.Services.TopicMasteryStatus(LernTor.Core.Enums.Subject.Mathematik, "Bruchrechnen", 12, 10, 10, 2, LernTor.Core.Services.MasteryLevel.Gemeistert),
            new LernTor.Core.Services.TopicMasteryStatus(LernTor.Core.Enums.Subject.Mathematik, "Prozentrechnung", 8, 8, 6, 0, LernTor.Core.Services.MasteryLevel.Sicher),
            new LernTor.Core.Services.TopicMasteryStatus(LernTor.Core.Enums.Subject.Englisch, "Question Words", 6, 6, 3, 0, LernTor.Core.Services.MasteryLevel.Vertraut),
            new LernTor.Core.Services.TopicMasteryStatus(LernTor.Core.Enums.Subject.Musik, "Stimme, Gesang und Chor", 2, 2, 1, 0, LernTor.Core.Services.MasteryLevel.Angefangen),
        };
        var view = new LernTor.App.Views.ProgressOverviewView
        {
            DataContext = new LernTor.App.ViewModels.ProgressOverviewViewModel("Test", themen, () => { })
        };

        view.Measure(new Size(1920, 1080));
        view.Arrange(new Rect(0, 0, 1920, 1080));
        view.UpdateLayout();

        var texte = new List<string>();
        SammleTexte(view, texte);
        Assert.Contains("Bruchrechnen", texte);
        Assert.Contains("Stimme, Gesang und Chor", texte);
    }

    private static void SammleTexte(DependencyObject knoten, List<string> texte)
    {
        if (knoten is System.Windows.Controls.TextBlock block)
        {
            texte.Add(block.Text);
        }

        for (var i = 0; i < System.Windows.Media.VisualTreeHelper.GetChildrenCount(knoten); i++)
        {
            SammleTexte(System.Windows.Media.VisualTreeHelper.GetChild(knoten, i), texte);
        }
    }

    // MainWindow und ParentSettingsWindow brauchen ViewModels aus dem DI-Container und sind hier
    // bewusst ausgenommen - ihr Startpfad wird vom Prozess-Smoke-Test (StartupSmokeTests)
    // abgedeckt, der die echte App inklusive DI/DB hochfahren lässt.
}
