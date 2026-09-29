using LernTor.Core.Enums;

namespace LernTor.Core.Models;

/// <summary>
/// Alle Einstellungen eines Profils, die der Eltern-Bereich auf einmal speichert
/// (<c>StudentProfileRepository.UpdateSettingsAsync</c>).
///
/// <para><b>Warum ein Objekt mit lauter <c>required</c>-Eigenschaften:</b> der Vorgänger war eine
/// Methode mit zwanzig Positionsparametern, zehn davon optional. Ein Aufruf mit vierzehn Argumenten
/// hat am 07.08.2026 still sechs Einstellungen auf ihre Vorgabewerte zurückgesetzt - einen auf
/// „Streng“ gestellten Jugendschutzfilter gelockert, abgeschaltete Bereiche wieder eingeschaltet
/// (siehe CLAUDE.md, „A repository method that overwrites ALL fields“). Bewacht wurde das danach
/// von einer Vorab-Prüfung. Jetzt verhindert es der Compiler: wer ein
/// <see cref="ProfileSettings"/> baut und ein Feld vergisst, bekommt CS9035.</para>
///
/// <para>Wer nur etwas ändern will, beginnt mit <see cref="From"/> und ändert per <c>with</c> -
/// dann kann nichts versehentlich zurückgesetzt werden:
/// <c>ProfileSettings.From(profil) with { WeeklyGoalDays = 4 }</c>.</para>
///
/// <para><b>Neue Profil-Einstellung?</b> Hier als <c>required</c> ergänzen, in <see cref="From"/>
/// übernehmen und im Repository schreiben. Der Test <c>ProfileSettingsTests</c> prüft, dass
/// <see cref="From"/> jede Eigenschaft kopiert; <c>scripts/preflight.py</c> prüft, dass keine
/// Eigenschaft ohne <c>required</c> dazukommt.</para>
/// </summary>
public sealed record ProfileSettings
{
    public required double TypingMinAccuracy { get; init; }

    public required double QuizFirstAttemptThreshold { get; init; }

    public required double QuizRetryThreshold { get; init; }

    public required int ReadingMinutes { get; init; }

    public required int NewsSecondsPerArticle { get; init; }

    public required int NewsArticleCount { get; init; }

    public required NewsFilterStrictness NewsFilterStrictness { get; init; }

    public required int ExerciseSecondsPerQuestion { get; init; }

    public required int ExercisesPerSubject { get; init; }

    public required int QuizQuestionCount { get; init; }

    public required int QuizRetryQuestionCount { get; init; }

    public required string? CustomTypingSentenceText { get; init; }

    public required string? CustomTypingFinalText { get; init; }

    public required int WeeklyGoalDays { get; init; }

    public required string? PinnedReadingTextKey { get; init; }

    public required bool DrivingAreaEnabled { get; init; }

    public required bool ErsteHilfeEnabled { get; init; }

    public required int DrivingChallengeSignCount { get; init; }

    public required IReadOnlySet<TrafficSignCategory> DisabledSignCategories { get; init; }

    /// <summary>Der gespeicherte Stand eines Profils - der Ausgangspunkt für jede Änderung.</summary>
    public static ProfileSettings From(StudentProfile profile)
    {
        ArgumentNullException.ThrowIfNull(profile);

        return new ProfileSettings
        {
            TypingMinAccuracy = profile.TypingMinAccuracy,
            QuizFirstAttemptThreshold = profile.QuizFirstAttemptThreshold,
            QuizRetryThreshold = profile.QuizRetryThreshold,
            ReadingMinutes = profile.ReadingMinutes,
            NewsSecondsPerArticle = profile.NewsSecondsPerArticle,
            NewsArticleCount = profile.NewsArticleCount,
            NewsFilterStrictness = profile.NewsFilterStrictness,
            ExerciseSecondsPerQuestion = profile.ExerciseSecondsPerQuestion,
            ExercisesPerSubject = profile.ExercisesPerSubject,
            QuizQuestionCount = profile.QuizQuestionCount,
            QuizRetryQuestionCount = profile.QuizRetryQuestionCount,
            CustomTypingSentenceText = profile.CustomTypingSentenceText,
            CustomTypingFinalText = profile.CustomTypingFinalText,
            WeeklyGoalDays = profile.WeeklyGoalDays,
            PinnedReadingTextKey = profile.PinnedReadingTextKey,
            DrivingAreaEnabled = profile.DrivingAreaEnabled,
            ErsteHilfeEnabled = profile.ErsteHilfeEnabled,
            DrivingChallengeSignCount = profile.DrivingChallengeSignCount,
            DisabledSignCategories = new HashSet<TrafficSignCategory>(profile.DisabledSignCategories),
        };
    }
}
