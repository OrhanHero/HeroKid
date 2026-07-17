using System.Windows.Input;
using System.Windows.Threading;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using LernTor.App.Localization;
using LernTor.App.Services;
using LernTor.Core.Enums;
using LernTor.Core.Models;
using LernTor.Core.Services;
using LernTor.Data.Repositories;

namespace LernTor.App.ViewModels;

/// <summary>
/// ViewModel für eine einzelne Tipp-Übung (virtuelle Tastatur, Echtzeit-Feedback, Genauigkeit).
/// </summary>
public sealed partial class TypingExerciseViewModel : ObservableObject
{
    private readonly TypingExerciseService _service;
    private readonly TypingProgressRepository _progressRepo;
    private readonly string _profileId;
    private readonly string _profileName;
    private readonly Action<string?> _onLessonCompleted;
    private readonly DispatcherTimer _timer;

    public TypingExerciseViewModel(
        TypingLesson lesson,
        TypingExerciseService service,
        TypingProgressRepository progressRepo,
        string profileId,
        string profileName,
        Action<string?> onLessonCompleted)
    {
        Lesson = lesson;
        _service = service;
        _progressRepo = progressRepo;
        _profileId = profileId;
        _profileName = profileName;
        _onLessonCompleted = onLessonCompleted;

        _timer = new DispatcherTimer { Interval = TimeSpan.FromMilliseconds(100) };
        _timer.Tick += (_, _) => Tick();
        _timer.Start();

        // Initialisiere Tasten
        Keys = BuildKeyboardLayout();
    }

    public TypingLesson Lesson { get; }

    public string LessonId => Lesson.Id;
    public string Title => Lesson.Title;
    public string Instruction => LocalizationService.Instance.CurrentLanguage == AppLanguage.Tuerkisch
        ? Lesson.InstructionTr
        : Lesson.InstructionDe;

    [ObservableProperty]
    private string currentInput = string.Empty;

    [ObservableProperty]
    private int currentPosition = 0;

    [ObservableProperty]
    private double accuracy = 0.0;

    [ObservableProperty]
    private double wpm = 0.0;

    [ObservableProperty]
    private int correctChars = 0;

    [ObservableProperty]
    private int totalChars = 0;

    [ObservableProperty]
    private TimeSpan elapsed = TimeSpan.Zero;

    [ObservableProperty]
    private bool isCompleted = false;

    [ObservableProperty]
    private bool isPassed = false;

    [ObservableProperty]
    private bool showResult = false;

    [ObservableProperty]
    private int starsEarned = 0;

    [ObservableProperty]
    private int currentHighlightIndex = -1;

    public string ProgressText => $"{CurrentPosition}/{TotalChars}";

    public double ProgressPercent => TotalChars > 0 ? (double)CurrentPosition / TotalChars : 0.0;

    [RelayCommand]
    private void Continue()
    {
        // Navigation wird über _onLessonCompleted-Callback in SaveAndContinueAsync behandelt.
        // Dieser Command dient nur dazu, den "Weiter"-Button im XAML zu binden.
    }

    partial void OnCurrentInputChanged(string value)
    {
        if (IsCompleted) return;

        CurrentPosition = Math.Min(value.Length, Lesson.TargetText.Length);
        CurrentHighlightIndex = Math.Min(value.Length, Lesson.TargetText.Length - 1);

        // Echtzeit-Validierung
        var result = _service.CheckInput(Lesson.TargetText, value, Elapsed);
        Accuracy = result.Accuracy;
        Wpm = result.Wpm;
        CorrectChars = result.CorrectCharacters;
        TotalChars = result.TotalCharacters;

        // Prüfen ob abgeschlossen
        if (value.Length >= Lesson.TargetText.Length)
        {
            CompleteExercise(value);
        }
    }

    private void CompleteExercise(string finalInput)
    {
        if (IsCompleted) return;

        IsCompleted = true;
        _timer.Stop();
        ShowResult = true;

        var result = _service.CheckInput(Lesson.TargetText, finalInput, Elapsed);
        Accuracy = result.Accuracy;
        Wpm = result.Wpm;
        CorrectChars = result.CorrectCharacters;
        TotalChars = result.TotalCharacters;
        IsPassed = result.Passed;

        // Speichern und nächste Lektion freischalten
        _ = SaveAndContinueAsync();
    }

    private async Task SaveAndContinueAsync()
    {
        // Fortschritt speichern über den Service (der auch die nächste Lektion mit Profilnamen ermittelt)
        var result = new TypingResult
        {
            Accuracy = Accuracy,
            Wpm = Wpm,
            CorrectCharacters = CorrectChars,
            TotalCharacters = TotalChars,
            Passed = IsPassed,
            Elapsed = Elapsed
        };
        await _service.RecordAttemptAsync(_profileId, Lesson, result, _profileName);

        // Sterne berechnen
        int stars = CalculateStars(Accuracy, Wpm);
        StarsEarned = stars;

        // Callback für nächste Lektion
        _onLessonCompleted?.Invoke(null); // null = bleib im Dashboard, nächste wird dort geladen
    }

    private int CalculateStars(double accuracy, double wpm)
    {
        int stars = 1;
        if (accuracy >= 0.90) stars++;
        if (accuracy >= 0.97) stars++;
        if (Wpm >= 30) stars++;
        if (Wpm >= 50) stars++;
        return Math.Min(stars, 5);
    }

    private void Tick()
    {
        if (!IsCompleted)
        {
            Elapsed = Elapsed.Add(TimeSpan.FromSeconds(0.1));
        }
    }

    // Tastatur-Layout für virtuelle Tastatur
    public IReadOnlyList<KeyboardRowViewModel> Keys { get; }

    private List<KeyboardRowViewModel> BuildKeyboardLayout()
    {
        var rows = new List<KeyboardRowViewModel>();

        // Reihe 1: Zahlen
        rows.Add(new KeyboardRowViewModel
        {
            Keys = new[]
            {
                CreateKey("1", "1"), CreateKey("2", "2"), CreateKey("3", "3"), CreateKey("4", "4"), CreateKey("5", "5"),
                CreateKey("6", "6"), CreateKey("7", "7"), CreateKey("8", "8"), CreateKey("9", "9"), CreateKey("0", "0"),
                CreateKey("ß", "ß"), CreateKey("´", "´")
            }
        });

        // Reihe 2: QWERTZUIOPÜ+
        rows.Add(new KeyboardRowViewModel
        {
            Keys = new[]
            {
                CreateKey("q", "q"), CreateKey("w", "w"), CreateKey("e", "e"), CreateKey("r", "r"),
                CreateKey("t", "t"), CreateKey("z", "z"), CreateKey("u", "u"), CreateKey("i", "i"),
                CreateKey("o", "o"), CreateKey("p", "p"), CreateKey("ü", "ü"), CreateKey("+", "+")
            }
        });

        // Reihe 3: ASDFGHJKLÖÄ#
        rows.Add(new KeyboardRowViewModel
        {
            Keys = new[]
            {
                CreateKey("a", "a"), CreateKey("s", "s"), CreateKey("d", "d"), CreateKey("f", "f"),
                CreateKey("g", "g"), CreateKey("h", "h"), CreateKey("j", "j"), CreateKey("k", "k"),
                CreateKey("l", "l"), CreateKey("ö", "ö"), CreateKey("ä", "ä"), CreateKey("#", "#")
            }
        });

        // Reihe 4: YXCVBNM,.-
        rows.Add(new KeyboardRowViewModel
        {
            Keys = new[]
            {
                CreateKey("<", "<"), CreateKey("y", "y"), CreateKey("x", "x"), CreateKey("c", "c"),
                CreateKey("v", "v"), CreateKey("b", "b"), CreateKey("n", "n"), CreateKey("m", "m"),
                CreateKey(",", ","), CreateKey(".", "."), CreateKey("-", "-")
            }
        });

        // Leertaste
        rows.Add(new KeyboardRowViewModel
        {
            Keys = new[] { CreateKey(" ", "␣", isSpace: true) }
        });

        return rows;
    }

    private KeyboardKeyViewModel CreateKey(string value, string display, bool isSpace = false)
    {
        var finger = GetFingerForKey(value);
        return new KeyboardKeyViewModel
        {
            Value = value,
            Display = display,
            FingerColor = GetFingerColor(value),
            FingerName = GetFingerName(value),
            IsSpace = isSpace,
            Width = isSpace ? 600 : 45
        };
    }

    private static TypingFinger GetFingerForKey(string key)
    {
        var lower = key.ToLowerInvariant();
        return lower switch
        {
            "q" => TypingFinger.LPinky, "a" => TypingFinger.LPinky, "y" => TypingFinger.RIndex, "1" => TypingFinger.LPinky,
            "w" => TypingFinger.LRing, "s" => TypingFinger.LRing, "x" => TypingFinger.LRing, "2" => TypingFinger.LRing,
            "e" => TypingFinger.LMiddle, "d" => TypingFinger.LMiddle, "c" => TypingFinger.LMiddle, "3" => TypingFinger.LMiddle,
            "r" => TypingFinger.LIndex, "f" => TypingFinger.LIndex, "v" => TypingFinger.LIndex, "t" => TypingFinger.LIndex, "g" => TypingFinger.LIndex, "b" => TypingFinger.LIndex, "4" => TypingFinger.LIndex, "5" => TypingFinger.LIndex,
            "z" => TypingFinger.LIndex, "h" => TypingFinger.RIndex, "n" => TypingFinger.RIndex, "6" => TypingFinger.RIndex, "7" => TypingFinger.RIndex,
            "u" => TypingFinger.RIndex, "j" => TypingFinger.RIndex, "m" => TypingFinger.RMiddle, "8" => TypingFinger.RMiddle,
            "i" => TypingFinger.RMiddle, "k" => TypingFinger.RMiddle, "," => TypingFinger.RPinky, "9" => TypingFinger.RRing,
            "o" => TypingFinger.RRing, "l" => TypingFinger.RRing, "." => TypingFinger.RPinky, "0" => TypingFinger.RPinky,
            "p" => TypingFinger.RPinky, "ö" => TypingFinger.RPinky, "ä" => TypingFinger.RPinky, "-" => TypingFinger.RPinky, "ß" => TypingFinger.RPinky,
            _ => TypingFinger.Thumb
        };
    }

    private string GetFingerColor(string key)
    {
        var finger = GetFingerForKey(key);
        return finger switch
        {
            TypingFinger.LPinky => "#FF6B6B",    // Rot
            TypingFinger.LRing => "#FFB347",     // Orange
            TypingFinger.LMiddle => "#7DD3FC",   // Hellblau
            TypingFinger.LIndex => "#4ADE80",    // Grün
            TypingFinger.RIndex => "#4ADE80",    // Grün
            TypingFinger.RMiddle => "#7DD3FC",   // Hellblau
            TypingFinger.RRing => "#FFB347",     // Orange
            TypingFinger.RPinky => "#FF6B6B",    // Rot
            _ => "#A5B4FC"                       // Lila (Daumen)
        };
    }

    private string GetFingerName(string key)
    {
        var finger = GetFingerForKey(key);
        return finger switch
        {
            TypingFinger.LPinky => "👈", TypingFinger.LRing => "👈",
            TypingFinger.LMiddle => "👈", TypingFinger.LIndex => "☝️",
            TypingFinger.RIndex => "☝️", TypingFinger.RMiddle => "👈",
            TypingFinger.RRing => "👈", TypingFinger.RPinky => "👈",
            _ => "👍"
        };
    }
}

/// <summary>Eine Tastenreihe der virtuellen Tastatur.</summary>
public sealed class KeyboardRowViewModel
{
    public IReadOnlyList<KeyboardKeyViewModel> Keys { get; set; } = Array.Empty<KeyboardKeyViewModel>();
}

/// <summary>Eine einzelne Taste der virtuellen Tastatur.</summary>
public sealed partial class KeyboardKeyViewModel : ObservableObject
{
    public string Value { get; set; } = string.Empty;
    public string Display { get; set; } = string.Empty;
    public string FingerColor { get; set; } = string.Empty;
    public string FingerName { get; set; } = string.Empty;
    public bool IsSpace { get; set; }
    public double Width { get; set; } = 45;

    [ObservableProperty]
    private bool isTarget = false;

    [ObservableProperty]
    private bool isError = false;

    [ObservableProperty]
    private bool isCorrect = false;
}