using System.Windows.Threading;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using LernTor.App.Services;
using LernTor.Core.Models;

namespace LernTor.App.ViewModels;

/// <summary>
/// Pflicht-Leseabschnitt: zeigt täglich ein Gedicht/wichtiges Werk (Mix DE/TR/EN), das laut
/// vorgelesen werden soll. Es gibt bewusst KEINE Überspringen-Funktion - stattdessen läuft ein
/// 5-Minuten-Timer herunter, erst danach wird "Weiter" nutzbar. Eine Prüfung, ob tatsächlich laut
/// vorgelesen wurde, ist technisch nicht möglich und wird hier auch nicht versucht.
/// Neben der Drei-Spalten-Ansicht ("Alle") gibt es Sprach-Tabs, die eine einzelne Sprache größer
/// und mit mehr Zeilenabstand zeigen, plus eine Vorlesen-Funktion (Windows-TTS, offline).
/// </summary>
public sealed partial class ReadingViewModel : ObservableObject
{
    private static readonly TimeSpan MinimumDuration = TimeSpan.FromMinutes(5);

    /// <summary>Tab-Kennungen - als Konstanten statt Enum, da sie direkt aus XAML als
    /// CommandParameter-Strings kommen und der EqualsToSelectedTag-Converter Strings vergleicht.</summary>
    public const string TabAll = "All";
    public const string TabDe = "De";
    public const string TabTr = "Tr";
    public const string TabEn = "En";

    private readonly Action _onCompleted;
    private readonly TextToSpeechService _tts;
    private readonly DispatcherTimer _timer;

    public ReadingPiece Piece { get; }

    /// <summary>Zweiter Lesetext des Tages (literarisch + Pop-Kultur kombiniert, siehe
    /// ReadingContentProvider.GetSecondForDate) - ein Text allein war für 5 Minuten zu wenig Stoff.</summary>
    public ReadingPiece SecondPiece { get; }

    [ObservableProperty]
    private TimeSpan remainingTime = MinimumDuration;

    [ObservableProperty]
    [NotifyCanExecuteChangedFor(nameof(ContinueCommand))]
    private bool canContinue;

    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(IsAllTab))]
    [NotifyPropertyChangedFor(nameof(IsSingleTab))]
    [NotifyPropertyChangedFor(nameof(SingleTabText))]
    [NotifyPropertyChangedFor(nameof(SecondSingleTabText))]
    private string selectedTab = TabAll;

    [ObservableProperty]
    private bool isSpeaking;

    public bool IsAllTab => SelectedTab == TabAll;
    public bool IsSingleTab => !IsAllTab;

    /// <summary>Text 1 in der aktuell gewählten Einzelsprache (leer im "Alle"-Modus).</summary>
    public string SingleTabText => TextForTab(Piece);

    /// <summary>Text 2 in der aktuell gewählten Einzelsprache (leer im "Alle"-Modus).</summary>
    public string SecondSingleTabText => TextForTab(SecondPiece);

    private string TextForTab(ReadingPiece piece) => SelectedTab switch
    {
        TabDe => piece.TextDe,
        TabTr => piece.TextTr,
        TabEn => piece.TextEn,
        _ => string.Empty
    };

    public string RemainingTimeDisplay => $"{(int)RemainingTime.TotalMinutes}:{RemainingTime.Seconds:00}";

    public ReadingViewModel(ReadingPiece piece, ReadingPiece secondPiece, Action onCompleted, TextToSpeechService tts)
    {
        Piece = piece;
        SecondPiece = secondPiece;
        _onCompleted = onCompleted;
        _tts = tts;
        _tts.SpeakingChanged += OnSpeakingChanged;

        _timer = new DispatcherTimer(DispatcherPriority.Background) { Interval = TimeSpan.FromSeconds(1) };
        _timer.Tick += (_, _) => Tick();
        _timer.Start();
    }

    private void OnSpeakingChanged(bool speaking) => IsSpeaking = speaking;

    partial void OnRemainingTimeChanged(TimeSpan value) => OnPropertyChanged(nameof(RemainingTimeDisplay));

    private void Tick()
    {
        var next = RemainingTime - TimeSpan.FromSeconds(1);
        RemainingTime = next <= TimeSpan.Zero ? TimeSpan.Zero : next;

        if (RemainingTime <= TimeSpan.Zero)
        {
            _timer.Stop();
            CanContinue = true;
        }
    }

    [RelayCommand]
    private void SelectTab(string tab)
    {
        // Beim Sprachwechsel eine laufende Vorlesung stoppen - sonst liest die alte Sprache weiter,
        // während schon der neue Text angezeigt wird.
        _tts.Stop();
        SelectedTab = tab;
    }

    /// <summary>Liest den Text der gewählten Sprache vor (im "Alle"-Modus: Deutsch); zweiter Klick stoppt.</summary>
    [RelayCommand]
    private void ToggleReadAloud()
    {
        if (IsSpeaking)
        {
            _tts.Stop();
            return;
        }

        var (text, secondText, culture) = SelectedTab switch
        {
            TabTr => (Piece.TextTr, SecondPiece.TextTr, "tr-TR"),
            TabEn => (Piece.TextEn, SecondPiece.TextEn, "en-US"),
            _ => (Piece.TextDe, SecondPiece.TextDe, "de-DE")
        };

        _tts.Speak($"{Piece.Title}. {text} … {SecondPiece.Title}. {secondText}", culture);
    }

    [RelayCommand(CanExecute = nameof(CanContinue))]
    private void Continue()
    {
        _timer.Stop();
        // Handler abhängen: der TTS-Dienst ist ein App-Singleton, dieses ViewModel aber pro Lerntag
        // kurzlebig - ohne Abmelden würde jede Session einen weiteren toten Handler ansammeln.
        _tts.Stop();
        _tts.SpeakingChanged -= OnSpeakingChanged;
        _onCompleted();
    }
}
