using System.Linq;
using LernTor.Core.Enums;
using LernTor.Core.Models;
using LernTor.Core.Services;
using LernTor.Data.Entities;
using LernTor.Data.Repositories;

namespace LernTor.App.Services;

/// <summary>
/// Service für die Tipp-Trainer-Logik: Genauigkeitsprüfung, Fortschritt, Freischaltung.
/// Liegt in App.Services, da es Data-Repositories nutzt.
/// </summary>
public sealed class TypingExerciseService
{
    private readonly TypingProgressRepository _progressRepo;

    public TypingExerciseService(TypingProgressRepository progressRepo)
    {
        _progressRepo = progressRepo;
    }

    /// <summary>
    /// Prüft die getippte Eingabe gegen den Zieltext und berechnet Genauigkeit, WPM, Fehler.
    /// Nutzt die lesson-spezifische MinimumAccuracy statt hartem 85%.
    /// </summary>
    public TypingResult CheckInput(string targetText, string userInput, TimeSpan elapsed, TypingLesson? lesson = null)
    {
        var result = new TypingResult
        {
            TargetText = targetText,
            UserInput = userInput,
            Elapsed = elapsed
        };

        int correctChars = 0;
        var errors = new List<TypingError>();

        int minLen = Math.Min(targetText.Length, userInput.Length);
        for (int i = 0; i < minLen; i++)
        {
            if (targetText[i] == userInput[i])
            {
                correctChars++;
            }
            else
            {
                errors.Add(new TypingError
                {
                    Position = i,
                    Expected = targetText[i],
                    Actual = userInput[i]
                });
            }
        }

        // Fehlende Zeichen am Ende zählen als Fehler
        if (userInput.Length < targetText.Length)
        {
            for (int i = userInput.Length; i < targetText.Length; i++)
            {
                errors.Add(new TypingError
                {
                    Position = i,
                    Expected = targetText[i],
                    Actual = '\0' // nicht getippt
                });
            }
        }

        // Extrazugetippte Zeichen am Ende zählen als Fehler
        if (userInput.Length > targetText.Length)
        {
            for (int i = targetText.Length; i < userInput.Length; i++)
            {
                errors.Add(new TypingError
                {
                    Position = i,
                    Expected = '\0',
                    Actual = userInput[i]
                });
            }
        }

        result.CorrectCharacters = correctChars;
        result.TotalCharacters = targetText.Length;
        result.Errors = errors;
        result.Accuracy = targetText.Length > 0 ? (double)correctChars / targetText.Length : 0.0;

        // WPM Berechnung: (korrekte Zeichen / 5) / Minuten
        double minutes = elapsed.TotalMinutes;
        if (minutes > 0)
        {
            result.Wpm = (correctChars / 5.0) / minutes;
        }

        // Nutze lesson-spezifische Mindestgenauigkeit (Default 85% falls keine Lektion übergeben)
        double minAccuracy = lesson?.MinimumAccuracy ?? 0.85;
        result.Passed = result.Accuracy >= minAccuracy && correctChars >= GetMinCharsForLesson(targetText);
        return result;
    }

    private int GetMinCharsForLesson(string targetText) => targetText.Length; // Vereinfacht: alle Zeichen

    /// <summary>
    /// Speichert den Versuch und prüft Freischaltung der nächsten Lektion.
    /// </summary>
    public async Task<LessonCompletionResult> RecordAttemptAsync(
        string profileId,
        TypingLesson lesson,
        TypingResult result,
        string? profileName = null)
    {
        var progress = await _progressRepo.GetOrCreateAsync(profileId, lesson.Id);

        progress.AttemptCount++;
        progress.TotalCharactersTyped += result.UserInput.Length;
        progress.LastAttemptAt = DateTimeOffset.Now;
        progress.LastAccuracy = result.Accuracy;
        progress.LastWpm = result.Wpm;

        bool newlyCompleted = false;
        if (!progress.IsCompleted && result.Passed)
        {
            progress.IsCompleted = true;
            progress.CompletedAt = DateTimeOffset.Now;
            progress.BestAccuracy = Math.Max(progress.BestAccuracy, result.Accuracy);
            progress.BestWpm = Math.Max(progress.BestWpm, result.Wpm);
            progress.CorrectCharacters = result.CorrectCharacters;
            progress.StarsEarned = CalculateStars(result.Accuracy, result.Wpm);
            newlyCompleted = true;
        }
        else if (result.Passed)
        {
            progress.BestAccuracy = Math.Max(progress.BestAccuracy, result.Accuracy);
            progress.BestWpm = Math.Max(progress.BestWpm, result.Wpm);
            if (result.CorrectCharacters > progress.CorrectCharacters)
            {
                progress.CorrectCharacters = result.CorrectCharacters;
            }
        }

        await _progressRepo.SaveAsync(progress);

        // Prüfe nächste Lektion (mit Profilnamen für persönliche Abschluss-Lektion)
        var allProgress = await _progressRepo.GetProgressAsync(profileId);
        var nextLesson = TypingContentProvider.GetNextLesson(progress.LessonId, profileName); // Profilname übergeben

        return new LessonCompletionResult
        {
            Progress = progress,
            NewlyCompleted = newlyCompleted,
            StarsEarned = progress.StarsEarned,
            NextLessonUnlocked = newlyCompleted, // Vereinfacht: nächste freigeschaltet bei Abschluss
            NextLessonId = nextLesson?.Id
        };
    }

    private int CalculateStars(double accuracy, double wpm)
    {
        int stars = 1;
        if (accuracy >= 0.90) stars++;
        if (accuracy >= 0.97) stars++;
        if (wpm >= 30) stars++;
        if (wpm >= 50) stars++;
        return Math.Min(stars, 5);
    }

    /// <summary>
    /// Holt Dashboard-Daten für ein Profil.
    /// </summary>
    public async Task<TypingDashboardData> GetDashboardDataAsync(string profileId, string? profileName = null)
    {
        var progressDict = await _progressRepo.GetProgressAsync(profileId);

        var lessonStates = new List<LessonState>();
        int completedCount = 0;
        double totalAccuracy = 0;
        int accuracyCount = 0;

        foreach (var lesson in TypingContentProvider.GetAllLessons())
        {
            progressDict.TryGetValue(lesson.Id, out var progress);
            bool completed = progress?.IsCompleted ?? false;
            if (completed) completedCount++;

            if (progress?.BestAccuracy > 0)
            {
                totalAccuracy += progress.BestAccuracy;
                accuracyCount++;
            }

            lessonStates.Add(new LessonState
            {
                Lesson = lesson,
                IsCompleted = completed,
                BestAccuracy = progress?.BestAccuracy ?? 0,
                BestWpm = progress?.BestWpm ?? 0,
                StarsEarned = progress?.StarsEarned ?? 0,
                AttemptCount = progress?.AttemptCount ?? 0,
                IsUnlocked = IsLessonUnlocked(lesson, progressDict)
            });
        }

        // Profil-spezifische Abschluss-Lektion zum Dashboard hinzufügen (als letzte Lektion)
        var finalLesson = TypingContentProvider.GetFinalLessonForProfile(profileName);
        progressDict.TryGetValue(finalLesson.Id, out var finalProgress);
        bool finalCompleted = finalProgress?.IsCompleted ?? false;
        if (finalCompleted) completedCount++;

        lessonStates.Add(new LessonState
        {
            Lesson = finalLesson,
            IsCompleted = finalCompleted,
            BestAccuracy = finalProgress?.BestAccuracy ?? 0,
            BestWpm = finalProgress?.BestWpm ?? 0,
            StarsEarned = finalProgress?.StarsEarned ?? 0,
            AttemptCount = finalProgress?.AttemptCount ?? 0,
            IsUnlocked = IsLessonUnlocked(finalLesson, progressDict)
        });

        double avgAccuracy = accuracyCount > 0 ? totalAccuracy / accuracyCount : 0;

        return new TypingDashboardData
        {
            Lessons = lessonStates,
            CompletedCount = completedCount,
            TotalLessons = TypingContentProvider.GetAllLessons().Count + 1, // +1 für Abschluss-Lektion
            AverageAccuracy = avgAccuracy,
            NextLesson = TypingContentProvider.GetNextUnlockedLesson(progressDict.Where(kvp => kvp.Value.IsCompleted).Select(kvp => kvp.Key).ToHashSet(), profileName)
        };
    }

    private bool IsLessonUnlocked(TypingLesson lesson, IReadOnlyDictionary<string, TypingLessonProgressEntity> progress)
    {
        if (lesson.LessonType == TypingLessonType.Grundreihe)
            return true; // Erste Lektion immer offen

        // Prüfe ob vorherige Lektion abgeschlossen (vereinfacht: irgendeine Lektion abgeschlossen)
        return progress.Values.Any(p => p.IsCompleted && p.LessonId != lesson.Id); // Vereinfacht
    }
}

/// <summary>Ergebnis eines Tipp-Versuchs.</summary>
public sealed class TypingResult
{
    public string TargetText { get; set; } = string.Empty;
    public string UserInput { get; set; } = string.Empty;
    public TimeSpan Elapsed { get; set; }
    public int CorrectCharacters { get; set; }
    public int TotalCharacters { get; set; }
    public double Accuracy { get; set; }
    public double Wpm { get; set; }
    public List<TypingError> Errors { get; set; } = new();
    public bool Passed { get; set; }
}

/// <summary>Einzelner Tippfehler.</summary>
public sealed class TypingError
{
    public int Position { get; set; }
    public char Expected { get; set; }
    public char Actual { get; set; } // '\0' = nicht getippt / Extrazugabe
}

/// <summary>Ergebnis einer Lektions-Absolvierung.</summary>
public sealed class LessonCompletionResult
{
    public TypingLessonProgressEntity Progress { get; set; } = null!;
    public bool NewlyCompleted { get; set; }
    public int StarsEarned { get; set; }
    public bool NextLessonUnlocked { get; set; }
    public string? NextLessonId { get; set; }
}

/// <summary>Dashboard-Daten für Typing-Dashboard.</summary>
public sealed class TypingDashboardData
{
    public List<LessonState> Lessons { get; set; } = new();
    public int CompletedCount { get; set; }
    public int TotalLessons { get; set; }
    public double AverageAccuracy { get; set; }
    public TypingLesson? NextLesson { get; set; }
}

/// <summary>Status einer einzelnen Lektion im Dashboard.</summary>
public sealed class LessonState
{
    public required TypingLesson Lesson { get; set; }
    public bool IsCompleted { get; set; }
    public double BestAccuracy { get; set; }
    public double BestWpm { get; set; }
    public int StarsEarned { get; set; }
    public int AttemptCount { get; set; }
    public bool IsUnlocked { get; set; }
}