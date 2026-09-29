using LernTor.Core.Enums;

namespace LernTor.Core.Services;

/// <summary>Die vier Meisterschaftsstufen eines Themas, von „gerade angefangen“ bis „gemeistert“.</summary>
public enum MasteryLevel
{
    /// <summary>Weniger als <see cref="TopicMasteryCalculator.MinAnswers"/> Antworten - noch keine Aussage möglich.</summary>
    Angefangen = 0,

    /// <summary>Genug Antworten, aber zuletzt unter <see cref="TopicMasteryCalculator.SicherRate"/> richtig.</summary>
    Vertraut = 1,

    /// <summary>Zuletzt mindestens <see cref="TopicMasteryCalculator.SicherRate"/> richtig.</summary>
    Sicher = 2,

    /// <summary>Zuletzt mindestens <see cref="TopicMasteryCalculator.GemeistertRate"/> richtig UND Fragen des
    /// Themas haben eine Wiederholung nach Abstand bestanden.</summary>
    Gemeistert = 3
}

/// <summary>Eine beantwortete Aufgabe, so wie das Aktivitätsprotokoll sie kennt.</summary>
public readonly record struct MasteryAnswer(
    Subject Subject, string Topic, string Prompt, bool WasCorrect, DateTimeOffset Timestamp);

/// <summary>Stand eines Themas.</summary>
/// <param name="Answered">Alle Antworten zu diesem Thema.</param>
/// <param name="RecentAnswered">Davon die letzten, höchstens <see cref="TopicMasteryCalculator.RecentWindow"/>.</param>
/// <param name="RecentCorrect">Davon richtig.</param>
/// <param name="ReviewedPrompts">Verschiedene Fragen des Themas, die eine Wiederholung nach Abstand
/// (7/30/90 Tage) bestanden haben.</param>
public sealed record TopicMasteryStatus(
    Subject Subject,
    string Topic,
    int Answered,
    int RecentAnswered,
    int RecentCorrect,
    int ReviewedPrompts,
    MasteryLevel Level)
{
    public double RecentRate => RecentAnswered == 0 ? 0 : (double)RecentCorrect / RecentAnswered;
}

/// <summary>
/// Leitet für jedes geübte Thema eine Meisterschaftsstufe ab - nach dem Vorbild von Khan Academy
/// („versucht → vertraut → sicher → gemeistert“).
///
/// <para><b>Warum:</b> Das Kind sah bisher Sterne und eine Trefferquote je Fach. Ob es ein Thema
/// <i>beherrscht</i>, sah niemand - auch die Eltern nicht. Eine Quote von 80 % in Mathematik kann
/// heißen, dass Bruchrechnen sitzt und Prozentrechnung gar nicht.</para>
///
/// <para><b>Gezählt werden die letzten Antworten, nicht alle.</b> Wer im September Bruchrechnen
/// nicht konnte und es im Oktober kann, soll „sicher“ sehen - nicht einen Durchschnitt, der den
/// September für immer mitschleppt. Deshalb nur die letzten <see cref="RecentWindow"/>.</para>
///
/// <para><b>„Gemeistert“ verlangt Behalten, nicht nur Können.</b> Eine hohe Quote an einem
/// einzigen Tag beweist, dass etwas gerade frisch ist. Gemeistert ist ein Thema erst, wenn
/// mindestens <see cref="MinReviewedPromptsForMastery"/> seiner Fragen nach Tagen oder Wochen
/// wiedergekommen und wieder richtig beantwortet worden sind (Wiederholung nach Abstand, siehe
/// <see cref="SpacedRepetitionSchedule"/>).</para>
///
/// <para><b>Stufen gehen auch zurück.</b> Wer ein Thema wieder verlernt, sieht das. Anders als
/// Abzeichen ist die Meisterschaft eine Beschreibung des jetzigen Stands, keine Belohnung.</para>
/// </summary>
public static class TopicMasteryCalculator
{
    /// <summary>Darunter keine Aussage: drei Antworten sind Zufall, kein Stand.</summary>
    public const int MinAnswers = 5;

    /// <summary>So viele der jüngsten Antworten zählen für die Quote.</summary>
    public const int RecentWindow = 10;

    public const double SicherRate = 0.7;

    public const double GemeistertRate = 0.9;

    public const int MinReviewedPromptsForMastery = 2;

    /// <summary>Präfix, mit dem die Fehler-Kartei ihre Themen markiert
    /// (<c>ReviewQuestionRepository</c>). „🔁 Bruchrechnen“ ist dasselbe Thema wie „Bruchrechnen“.</summary>
    public const string ReviewTopicPrefix = "🔁 ";

    /// <summary>
    /// Bereiche mit Themen im Sinne der Aufgabengeneratoren. Die News haben wechselnde Artikel statt
    /// Themen, der Tipptrainer misst Anschläge statt Antworten, und der Führerschein hat seine
    /// eigene Themenauswertung (<see cref="TheoryExamComposer.BuildMastery"/>).
    /// </summary>
    public static bool CountsForMastery(Subject subject) =>
        subject is not (Subject.News or Subject.Tippen or Subject.Fuehrerschein);

    public static string NormalizeTopic(string? topic)
    {
        var text = (topic ?? string.Empty).Trim();
        return text.StartsWith(ReviewTopicPrefix, StringComparison.Ordinal)
            ? text[ReviewTopicPrefix.Length..].Trim()
            : text;
    }

    /// <param name="answers">Alle protokollierten Antworten des Kindes (Reihenfolge egal).</param>
    /// <param name="reviewPassedPrompts">Fragetexte, die mindestens eine Wiederholung nach Abstand
    /// bestanden haben (<c>MasteredPromptRepository.GetReviewPassedPromptsAsync</c>).</param>
    /// <returns>Ein Eintrag je Fach und Thema, sortiert nach Fach (Reihenfolge des Enums) und
    /// Themenname.</returns>
    public static IReadOnlyList<TopicMasteryStatus> Calculate(
        IEnumerable<MasteryAnswer> answers,
        IReadOnlySet<string> reviewPassedPrompts)
    {
        ArgumentNullException.ThrowIfNull(answers);
        ArgumentNullException.ThrowIfNull(reviewPassedPrompts);

        return answers
            .Where(antwort => CountsForMastery(antwort.Subject))
            .Select(antwort => antwort with { Topic = NormalizeTopic(antwort.Topic) })
            .Where(antwort => antwort.Topic.Length > 0)
            .GroupBy(antwort => (antwort.Subject, antwort.Topic))
            .Select(gruppe => Evaluate(gruppe.Key.Subject, gruppe.Key.Topic, gruppe.ToList(), reviewPassedPrompts))
            .OrderBy(stand => (int)stand.Subject)
            .ThenBy(stand => stand.Topic, StringComparer.CurrentCulture)
            .ToList();
    }

    private static TopicMasteryStatus Evaluate(
        Subject subject, string topic, IReadOnlyList<MasteryAnswer> antworten, IReadOnlySet<string> wiederholtBestanden)
    {
        var juengste = antworten
            .OrderByDescending(antwort => antwort.Timestamp)
            .Take(RecentWindow)
            .ToList();
        var richtig = juengste.Count(antwort => antwort.WasCorrect);
        var wiederholt = antworten
            .Select(antwort => antwort.Prompt)
            .Distinct(StringComparer.Ordinal)
            .Count(wiederholtBestanden.Contains);

        var quote = juengste.Count == 0 ? 0 : (double)richtig / juengste.Count;
        var stufe = antworten.Count < MinAnswers ? MasteryLevel.Angefangen
            : quote >= GemeistertRate && wiederholt >= MinReviewedPromptsForMastery ? MasteryLevel.Gemeistert
            : quote >= SicherRate ? MasteryLevel.Sicher
            : MasteryLevel.Vertraut;

        return new TopicMasteryStatus(subject, topic, antworten.Count, juengste.Count, richtig, wiederholt, stufe);
    }

    /// <summary>Wie viele Themen auf welcher Stufe stehen - für die Kopfzeile und den Elternbericht.</summary>
    public static IReadOnlyDictionary<MasteryLevel, int> CountByLevel(IEnumerable<TopicMasteryStatus> topics) =>
        Enum.GetValues<MasteryLevel>().ToDictionary(
            stufe => stufe,
            stufe => topics.Count(thema => thema.Level == stufe));
}
