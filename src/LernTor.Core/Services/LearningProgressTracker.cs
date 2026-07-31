using LernTor.Core.Enums;

namespace LernTor.Core.Services;

/// <summary>Wie sich ein Fach entwickelt hat.</summary>
public enum ProgressTrend
{
    /// <summary>Deutlich besser als im Zeitraum davor.</summary>
    Verbessert,

    /// <summary>Im Wesentlichen gleich geblieben.</summary>
    Gleich,

    /// <summary>Etwas schwächer als im Zeitraum davor.</summary>
    Schwaecher
}

/// <summary>Der Vergleich eines Fachs zwischen zwei Zeiträumen.</summary>
/// <param name="Subject">Das Fach.</param>
/// <param name="EarlierPercent">Trefferquote im früheren Zeitraum, in Prozent.</param>
/// <param name="RecentPercent">Trefferquote im jüngeren Zeitraum, in Prozent.</param>
/// <param name="EarlierAnswered">Aufgaben im früheren Zeitraum.</param>
/// <param name="RecentAnswered">Aufgaben im jüngeren Zeitraum.</param>
public readonly record struct SubjectProgress(
    Subject Subject,
    int EarlierPercent,
    int RecentPercent,
    int EarlierAnswered,
    int RecentAnswered)
{
    /// <summary>Veränderung in Prozentpunkten; positiv = besser geworden.</summary>
    public int Change => RecentPercent - EarlierPercent;

    public ProgressTrend Trend => Change >= LearningProgressTracker.MeaningfulChangePercent
        ? ProgressTrend.Verbessert
        : Change <= -LearningProgressTracker.MeaningfulChangePercent
            ? ProgressTrend.Schwaecher
            : ProgressTrend.Gleich;
}

/// <summary>
/// Zeigt den Kindern ihre eigene Entwicklung - "vor vier Wochen 40 %, jetzt 70 %".
///
/// <para>Die Kinder tragen die ganze Arbeit, sehen davon aber als Einziges eine Sternezahl.
/// Sterne sind eine Belohnungswährung; sie sagen nichts darüber, ob man besser geworden ist,
/// und sie hängen daran, dass Eltern etwas dafür eintragen. Zu sehen, dass man in Mathe von
/// 40 % auf 70 % gekommen ist, ist die Motivation, die niemandem gehört außer dem Kind selbst.</para>
///
/// <para><b>Kein Vergleich mit dem Geschwisterkind</b> - der ist eine bewusste Eltern-Entscheidung
/// und steht abschaltbar im Eltern-Bericht (siehe <see cref="ProfileComparison"/>). Hier geht es
/// ausschließlich um das Kind und sein früheres Ich.</para>
///
/// <para><b>Rückschritte werden gezeigt, aber nicht angeprangert.</b> Sie zu verschweigen wäre
/// unehrlich, sie rot zu markieren wäre eine Strafe für etwas, das jedem passiert. Die Zahlen
/// stehen da, sortiert mit dem Erfreulichsten zuerst - gewertet wird nicht.</para>
/// </summary>
public static class LearningProgressTracker
{
    /// <summary>Länge je Vergleichszeitraum in Tagen.</summary>
    public const int WindowDays = 14;

    /// <summary>So viele Aufgaben braucht ein Fach in <b>beiden</b> Zeiträumen, damit verglichen
    /// wird - eine Aussage aus drei Aufgaben ist Zufall, kein Fortschritt.</summary>
    public const int MinAnswersPerWindow = 5;

    /// <summary>Ab so vielen Prozentpunkten gilt eine Veränderung als echt. Darunter ist es
    /// Rauschen, und "du bist um 2 Punkte besser geworden" wäre eine leere Aussage.</summary>
    public const int MeaningfulChangePercent = 5;

    /// <summary>Eine beantwortete Aufgabe aus dem Aktivitätsprotokoll.</summary>
    public readonly record struct Answer(Subject Subject, DateOnly Day, bool WasCorrect);

    /// <summary>
    /// Vergleicht die letzten <see cref="WindowDays"/> Tage mit den <see cref="WindowDays"/> Tagen
    /// davor. Fächer ohne genug Daten in beiden Zeiträumen fallen weg - lieber nichts zeigen als
    /// eine Zahl, die nichts bedeutet. Sortiert: die größte Verbesserung zuerst.
    /// </summary>
    public static IReadOnlyList<SubjectProgress> Compare(IEnumerable<Answer> answers, DateOnly today)
    {
        // Der jüngere Zeitraum schließt heute ein, der frühere endet unmittelbar davor - so
        // entstehen zwei gleich lange, lückenlose und überschneidungsfreie Fenster.
        var recentFrom = today.AddDays(-(WindowDays - 1));
        var earlierFrom = recentFrom.AddDays(-WindowDays);

        var result = new List<SubjectProgress>();

        foreach (var group in answers.GroupBy(answer => answer.Subject))
        {
            var recent = group.Where(a => a.Day >= recentFrom && a.Day <= today).ToList();
            var earlier = group.Where(a => a.Day >= earlierFrom && a.Day < recentFrom).ToList();

            if (recent.Count < MinAnswersPerWindow || earlier.Count < MinAnswersPerWindow)
            {
                continue;
            }

            result.Add(new SubjectProgress(
                group.Key,
                Percent(earlier),
                Percent(recent),
                earlier.Count,
                recent.Count));
        }

        return result
            .OrderByDescending(progress => progress.Change)
            .ThenBy(progress => progress.Subject.ToString(), StringComparer.Ordinal)
            .ToList();
    }

    private static int Percent(IReadOnlyCollection<Answer> answers) =>
        answers.Count == 0 ? 0 : (int)Math.Round(answers.Count(a => a.WasCorrect) * 100.0 / answers.Count);
}
