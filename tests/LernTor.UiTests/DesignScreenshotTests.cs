using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using LernTor.App.ViewModels;
using LernTor.App.Views;
using LernTor.ContentGen.HomeworkChat;
using LernTor.Core.Enums;
using LernTor.Core.Models;
using LernTor.Core.Services;
using Xunit;

namespace LernTor.UiTests;

/// <summary>
/// Bildschirmfotos der wichtigsten Ansichten mit Beispieldaten, in 1366 × 768 (der kleinste
/// Bildschirm, für den LernTor gebaut ist).
///
/// <para><b>Warum:</b> das Aussehen lässt sich in der Entwicklungsumgebung nicht am Bildschirm
/// prüfen (Linux, kein WPF). Die CI legt die Bilder als Artefakt „Design-Vorschau“ ab - so kann
/// jede Änderung am Aussehen angesehen werden, bevor sie beim Kind landet
/// (docs/NAECHSTES-LEVEL-3.md, Schritt 1). Der Test selbst prüft nur, dass jede Ansicht ohne
/// Fehler zeichnet und nicht leer ist; das Urteil über das Aussehen trifft ein Mensch.</para>
///
/// <para>Zielordner: Umgebungsvariable <c>LERNTOR_SCREENSHOT_DIR</c>, sonst ein Temp-Ordner.</para>
/// </summary>
public sealed class DesignScreenshotTests
{
    private const int Breite = 1366;
    private const int Hoehe = 768;

    private static void EnsureAppResourcesLoaded()
    {
        if (Application.Current is null)
        {
            var app = new LernTor.App.App();
            app.InitializeComponent();
        }
    }

    private static string Zielordner()
    {
        var ordner = Environment.GetEnvironmentVariable("LERNTOR_SCREENSHOT_DIR");
        if (string.IsNullOrWhiteSpace(ordner))
        {
            ordner = Path.Combine(Path.GetTempPath(), "lerntor-design-vorschau");
        }

        Directory.CreateDirectory(ordner);
        return ordner;
    }

    /// <summary>Die Ansichten, die ein Kind jeden Tag sieht - mit Daten, damit die
    /// Item-Templates wirklich geladen werden.</summary>
    internal static IReadOnlyList<(string Name, Func<FrameworkElement> Ansicht)> Ansichten() => new List<(string, Func<FrameworkElement>)>
    {
        ("01-profilwahl", () =>
        {
            var vm = new ProfileSelectionViewModel(null!, null!, _ => { });
            vm.ProfileTiles.Add(new ProfileTileViewModel(Profil("Emirhan", "🦊", 42), Fortschritt(LearningStage.Englisch)));
            vm.ProfileTiles.Add(new ProfileTileViewModel(Profil("Batuhan", "🐺", 118), Fortschritt(LearningStage.Freigeschaltet, unlocked: true)));
            return new ProfileSelectionView { DataContext = vm };
        }),
        ("02-startseite", () => new WelcomeView
        {
            DataContext = new WelcomeViewModel(
                "Emirhan", currentStreak: 4, onContinue: () => { }, onSwitchLanguage: _ => { },
                dueReviewCount: 3,
                weeklyGoal: new WeeklyGoalCalculator.WeeklyGoalStatus(4, 2, 3),
                practiceSubjects: new HashSet<Subject> { Subject.Tuerkisch, Subject.Englisch, Subject.Mathematik },
                onOpenProgress: () => { })
        }),
        ("03-uebung-auswahl", () => new ExerciseView
        {
            DataContext = new ExerciseViewModel(Subject.Englisch, new[]
            {
                new QuizQuestion
                {
                    Id = "shot-mc", Subject = Subject.Englisch, GradeLevel = GradeLevel.Klasse6,
                    Topic = "Präpositionen in, on, at", Type = QuestionType.MultipleChoice,
                    Prompt = "Welches Wort passt? \"We have football practice ___ Monday.\"",
                    Options = new[] { "in", "on", "at" }, CorrectAnswers = new[] { "on" },
                    Explanation = "Vor Wochentagen steht \"on\".", HelpHint = "in: Monate, on: Tage, at: Uhrzeiten."
                }
            }, (_, _, _) => { }, () => { }, new StummerChat())
        }),
        ("04-uebung-offen", () => new ExerciseView
        {
            DataContext = new ExerciseViewModel(Subject.Tuerkisch, new[]
            {
                new QuizQuestion
                {
                    Id = "shot-open", Subject = Subject.Tuerkisch, GradeLevel = GradeLevel.Klasse6,
                    Topic = "İyelik Ekleri", Type = QuestionType.OpenText,
                    Prompt = "\"benim kitap___\" -> Wie heißt \"mein Buch\"?",
                    CorrectAnswers = new[] { "kitabım" },
                    Explanation = "Nach einem Konsonanten -ım; p wird zu b.",
                    RequiresTurkishCharacters = true
                }
            }, (_, _, _) => { }, () => { }, new StummerChat())
        }),
        ("05-geschafft", () => new ResultView
        {
            DataContext = new ResultViewModel(
                passed: true, result: null, earnedStarsToday: 12, totalStars: 118,
                todayAnsweredCount: 36, todayCorrectPercent: 83, currentStreak: 4,
                onRetryRequested: () => { }, onUnlockConfirmed: () => { },
                newAchievements: AchievementCatalog.All.Take(1).ToList())
        }),
        ("06-fortschritt", () => new ProgressOverviewView
        {
            DataContext = new ProgressOverviewViewModel("Emirhan", new[]
            {
                new TopicMasteryStatus(Subject.Mathematik, "Bruchrechnen", 12, 10, 10, 2, MasteryLevel.Gemeistert),
                new TopicMasteryStatus(Subject.Englisch, "Question Words", 8, 8, 6, 0, MasteryLevel.Sicher),
                new TopicMasteryStatus(Subject.Tuerkisch, "Hâl Ekleri", 6, 6, 3, 0, MasteryLevel.Vertraut),
                new TopicMasteryStatus(Subject.Musik, "Stimme, Gesang und Chor", 2, 2, 1, 0, MasteryLevel.Angefangen),
            }, () => { }, AchievementRowViewModel.BuildList(
                new Dictionary<string, DateTimeOffset> { ["richtig-10"] = DateTimeOffset.Now },
                _ => true, DateOnly.FromDateTime(DateTime.Today)))
        }),
    };

    [WpfFact]
    public void Wichtige_Ansichten_als_Bildschirmfotos()
    {
        EnsureAppResourcesLoaded();
        var ordner = Zielordner();

        foreach (var (name, ansicht) in Ansichten())
        {
            var datei = Path.Combine(ordner, $"{name}.png");
            var bild = Fotografiere(ansicht(), 1.0);
            Speichere(bild, datei);

            Assert.True(new FileInfo(datei).Length > 5_000, $"{name}: das Bild ist verdächtig klein - leer gezeichnet?");
            Assert.True(HatInhalt(bild), $"{name}: das Bild ist einfarbig - nichts gezeichnet?");
        }
    }

    /// <summary>
    /// Zeichnet eine Ansicht so, wie MainWindow sie zeigt: auf dem Hintergrund-Pinsel, in
    /// 1366 × 768, optional vergrößert (Textgröße).
    /// </summary>
    internal static RenderTargetBitmap Fotografiere(FrameworkElement ansicht, double skalierung)
    {
        var rahmen = new Border
        {
            Width = Breite,
            Height = Hoehe,
            Child = new Border
            {
                LayoutTransform = new ScaleTransform(skalierung, skalierung),
                Width = Breite / skalierung,
                Height = Hoehe / skalierung,
                Child = ansicht
            }
        };
        rahmen.SetResourceReference(Border.BackgroundProperty, "BackgroundBrush");
        TextOptions.SetTextFormattingMode(rahmen, TextFormattingMode.Display);

        rahmen.Measure(new Size(Breite, Hoehe));
        rahmen.Arrange(new Rect(0, 0, Breite, Hoehe));
        rahmen.UpdateLayout();

        var bild = new RenderTargetBitmap(Breite, Hoehe, 96, 96, PixelFormats.Pbgra32);
        bild.Render(rahmen);
        return bild;
    }

    internal static void Speichere(BitmapSource bild, string datei)
    {
        var encoder = new PngBitmapEncoder();
        encoder.Frames.Add(BitmapFrame.Create(bild));
        using var stream = File.Create(datei);
        encoder.Save(stream);
    }

    /// <summary>Mehr als eine Farbe im Bild - eine leer gezeichnete Ansicht wäre einfarbig.</summary>
    private static bool HatInhalt(BitmapSource bild)
    {
        var pixel = new int[bild.PixelWidth * bild.PixelHeight];
        bild.CopyPixels(pixel, bild.PixelWidth * 4, 0);
        return pixel.Distinct().Take(20).Count() >= 20;
    }

    private static StudentProfile Profil(string name, string avatar, int sterne) => new()
    {
        Id = name.ToLowerInvariant(),
        Name = name,
        GradeLevel = GradeLevel.Klasse6,
        AvatarEmoji = avatar,
        TotalStars = sterne
    };

    private static StudentProgress Fortschritt(LearningStage stufe, bool unlocked = false)
    {
        var progress = new StudentProgress { ProfileId = "x", CurrentStage = stufe };
        if (unlocked)
        {
            progress.IsUnlocked = true;
        }

        return progress;
    }

    private sealed class StummerChat : IHomeworkHelpChatService
    {
        public Task<string> AskAsync(QuizQuestion question, IReadOnlyList<ChatMessage> conversation, CancellationToken cancellationToken = default) =>
            Task.FromResult(string.Empty);
    }
}
