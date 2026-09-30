using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using LernTor.App.Controls;
using LernTor.App.Services;
using LernTor.App.ViewModels;
using LernTor.ContentGen.HomeworkChat;
using LernTor.Core.Enums;
using LernTor.Core.Models;
using LernTor.Core.Speech;
using Xunit;

namespace LernTor.UiTests;

/// <summary>
/// „🔊 Vorlesen“ an jeder Frage (docs/NAECHSTES-LEVEL-3-1.md, Schritt 1). Welche Stimmen auf
/// dem CI-Rechner installiert sind, weiß der Test nicht - er prüft deshalb die Regel für sich
/// und dass der Knopf GENAU dann sichtbar ist, wenn sie für alle Abschnitte einer Frage gilt.
/// </summary>
public sealed class QuestionReadAloudTests
{
    private static void EnsureAppResourcesLoaded()
    {
        if (Application.Current is null)
        {
            var app = new LernTor.App.App();
            app.InitializeComponent();
        }
    }

    private static readonly QuizQuestion EnglischeFrage = new()
    {
        Id = "vorlesen-en", Subject = Subject.Englisch, GradeLevel = GradeLevel.Klasse6,
        Topic = "Simple Present", Type = QuestionType.MultipleChoice,
        Prompt = "Setze die richtige Form ein: \"She ___ to school every day.\"",
        Options = new[] { "goes", "go", "going" }, CorrectAnswers = new[] { "goes" },
        Explanation = "he/she/it: -s."
    };

    [Theory]
    [InlineData(SpeechLanguage.Englisch, true, new string[0], true)]           // Piper hat alle drei
    [InlineData(SpeechLanguage.Englisch, false, new[] { "en" }, true)]
    [InlineData(SpeechLanguage.Deutsch, false, new[] { "en" }, false)]          // nur eine englische Windows-Stimme
    [InlineData(SpeechLanguage.Tuerkisch, false, new[] { "de", "en" }, false)] // häufig: keine türkische
    [InlineData(SpeechLanguage.Tuerkisch, false, new[] { "TR" }, true)]
    public void Knopf_nur_mit_passender_Stimme(SpeechLanguage sprache, bool piper, string[] windowsStimmen, bool erwartet) =>
        Assert.Equal(erwartet, TextToSpeechService.CanSpeak(sprache, piper, windowsStimmen));

    [WpfFact]
    public void Knopf_ist_sichtbar_genau_wenn_jede_Sprache_eine_Stimme_hat()
    {
        EnsureAppResourcesLoaded();
        using var sprachausgabe = new TextToSpeechService(new PiperTtsEngine());

        var frage = new QuestionAnswerViewModel(EnglischeFrage, new StummerChat(), speech: sprachausgabe);
        var erwartet = SpeechSegmenter.ForQuestion(EnglischeFrage, frage.DisplayOptions)
            .All(abschnitt => sprachausgabe.CanSpeak(abschnitt.Language));
        Assert.Equal(erwartet, frage.CanReadAloud);

        var karte = new QuestionCard { DataContext = frage };
        karte.Measure(new Size(900, 900));
        karte.Arrange(new Rect(0, 0, 900, 900));
        karte.UpdateLayout();

        var knopf = Knoepfe(karte).Single(b => ReferenceEquals(b.Command, frage.ReadAloudCommand));
        Assert.Equal(erwartet ? Visibility.Visible : Visibility.Collapsed, knopf.Visibility);
    }

    // WpfFact: die Sprachausgabe enthält einen MediaPlayer, und der will einen STA-Thread.
    [WpfFact]
    public void Ohne_Sprachausgabe_und_beim_Diktat_nie()
    {
        Assert.False(new QuestionAnswerViewModel(EnglischeFrage, new StummerChat()).CanReadAloud);

        var diktat = new QuizQuestion
        {
            Id = "vorlesen-diktat", Subject = Subject.Deutsch, GradeLevel = GradeLevel.Klasse6,
            Topic = "Diktat", Type = QuestionType.Diktat, Prompt = "Der Hund bellt laut.",
            CorrectAnswers = new[] { "Der Hund bellt laut." }, Explanation = "-"
        };
        using var sprachausgabe = new TextToSpeechService(new PiperTtsEngine());
        Assert.False(new QuestionAnswerViewModel(diktat, new StummerChat(), speech: sprachausgabe).CanReadAloud);
    }

    private static IEnumerable<Button> Knoepfe(DependencyObject knoten)
    {
        if (knoten is Button knopf)
        {
            yield return knopf;
        }

        for (var i = 0; i < VisualTreeHelper.GetChildrenCount(knoten); i++)
        {
            foreach (var kind in Knoepfe(VisualTreeHelper.GetChild(knoten, i)))
            {
                yield return kind;
            }
        }
    }

    private sealed class StummerChat : IHomeworkHelpChatService
    {
        public Task<string> AskAsync(QuizQuestion question, IReadOnlyList<ChatMessage> conversation, CancellationToken cancellationToken = default) =>
            Task.FromResult(string.Empty);
    }
}
