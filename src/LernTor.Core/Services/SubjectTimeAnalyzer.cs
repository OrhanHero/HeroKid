using LernTor.Core.Enums;

namespace LernTor.Core.Services;

/// <summary>Wie sich Zeitaufwand und Trefferquote eines Fachs zueinander verhalten.</summary>
public enum SubjectEffort
{
    /// <summary>Zu wenige gemessene Antworten für eine Aussage.</summary>
    KeineDaten,

    /// <summary>Schnell und überwiegend richtig - sitzt.</summary>
    SitztSicher,

    /// <summary>Schnell und überwiegend falsch - das deutlichste Rate-Signal.</summary>
    VermutlichGeraten,

    /// <summary>Braucht lange, liegt aber richtig - gründlich, kein Problem.</summary>
    GruendlichAberLangsam,

    /// <summary>Braucht lange UND liegt oft falsch - hier hakt es wirklich.</summary>
    HierHaktEs,

    /// <summary>Unauffällig im Vergleich zu den anderen Fächern.</summary>
    Unauffaellig
}

/// <summary>Zeit- und Trefferbilanz eines Fachs im ausgewerteten Zeitraum.</summary>
/// <param name="Subject">Das Fach.</param>
/// <param name="Answered">Bearbeitete Aufgaben insgesamt.</param>
/// <param name="MeasuredCount">Davon mit gemessener Dauer (Alt-Zeilen haben keine).</param>
/// <param name="TotalMs">Summe der gemessenen Dauern.</param>
/// <param name="MedianMs">Median der gemessenen Dauern - robuster als der Mittelwert, den eine
/// einzelne lange Pause (Kind geht zwischendurch weg) sonst verzerrt.</param>
/// <param name="CorrectCount">Richtig beantwortet.</param>
/// <param name="Effort">Einordnung im Vergleich zu den übrigen Fächern des Kindes.</param>
public readonly record struct SubjectTimeStat(
    Subject Subject,
    int Answered,
    int MeasuredCount,
    long TotalMs,
    int MedianMs,
    int CorrectCount,
    SubjectEffort Effort)
{
    public double Accuracy => Answered > 0 ? (double)CorrectCount / Answered : 0;

    public TimeSpan TotalTime => TimeSpan.FromMilliseconds(TotalMs);

    public TimeSpan MedianTime => TimeSpan.FromMilliseconds(MedianMs);
}

/// <summary>
/// Wertet aus, wie viel Zeit ein Kind je Fach verbringt - und setzt sie ins Verhältnis zur
/// Trefferquote.
///
/// <para>Die Antwortdauern wurden bisher nur für die "zu schnell geklickt"-Warnung ausgewertet
/// (siehe <see cref="AnswerPaceAnalyzer"/>). Dabei steckt darin die Antwort auf eine Frage, die
/// eine Trefferquote allein nicht beantwortet: <b>warum</b> ein Fach schlecht läuft. Wer schnell
/// antwortet und falsch liegt, rät. Wer lange braucht und falsch liegt, hat das Thema nicht
/// verstanden. Das sind zwei völlig verschiedene Probleme mit zwei verschiedenen Antworten -
/// und für die Eltern der Unterschied zwischen "setz dich hin und lies die Frage" und "das
/// müssen wir gemeinsam nochmal durchgehen".</para>
///
/// <para>"Schnell" und "langsam" sind bewusst RELATIV zum Kind selbst definiert, nicht absolut:
/// ein gründliches Kind braucht überall länger, ein flottes überall kürzer. Verglichen wird der
/// Median eines Fachs mit dem Median über alle Fächer desselben Kindes.</para>
/// </summary>
public static class SubjectTimeAnalyzer
{
    /// <summary>Ab so vielen gemessenen Antworten je Fach wird eingeordnet - darunter ist jede
    /// Aussage Zufall.</summary>
    public const int MinMeasuredForVerdict = 5;

    /// <summary>Ab diesem Vielfachen des Gesamt-Medians gilt ein Fach als langsam.</summary>
    public const double SlowFactor = 1.3;

    /// <summary>Bis zu diesem Anteil des Gesamt-Medians gilt ein Fach als schnell.</summary>
    public const double FastFactor = 0.7;

    /// <summary>Ab dieser Trefferquote gilt ein Fach als sicher.</summary>
    public const double SolidAccuracy = 0.7;

    /// <summary>Eine ausgewertete Antwort.</summary>
    /// <param name="Subject">Fach als String (so steht es im Aktivitätsprotokoll).</param>
    /// <param name="WasCorrect">Richtig beantwortet.</param>
    /// <param name="DurationMs">Dauer in Millisekunden; 0 oder negativ = nicht gemessen.</param>
    public readonly record struct Entry(string Subject, bool WasCorrect, int DurationMs);

    /// <summary>
    /// Liefert je Fach eine Zeit-/Trefferbilanz, zeitaufwendigstes Fach zuerst.
    /// </summary>
    public static IReadOnlyList<SubjectTimeStat> Analyze(IEnumerable<Entry> entries)
    {
        var bySubject = new Dictionary<Subject, List<Entry>>();

        foreach (var entry in entries)
        {
            if (!Enum.TryParse<Subject>(entry.Subject, out var subject))
            {
                continue;
            }

            if (!bySubject.TryGetValue(subject, out var list))
            {
                list = new List<Entry>();
                bySubject[subject] = list;
            }

            list.Add(entry);
        }

        if (bySubject.Count == 0)
        {
            return Array.Empty<SubjectTimeStat>();
        }

        // Vergleichsmassstab: der Median ueber ALLE gemessenen Antworten des Kindes.
        var allDurations = bySubject.Values
            .SelectMany(list => list)
            .Where(entry => entry.DurationMs > 0)
            .Select(entry => entry.DurationMs)
            .ToList();

        var overallMedian = Median(allDurations);

        var stats = new List<SubjectTimeStat>();

        foreach (var (subject, list) in bySubject)
        {
            var measured = list.Where(entry => entry.DurationMs > 0).Select(entry => entry.DurationMs).ToList();
            var median = Median(measured);
            var correct = list.Count(entry => entry.WasCorrect);

            stats.Add(new SubjectTimeStat(
                subject,
                Answered: list.Count,
                MeasuredCount: measured.Count,
                TotalMs: measured.Sum(duration => (long)duration),
                MedianMs: median,
                CorrectCount: correct,
                Effort: Classify(measured.Count, median, overallMedian, (double)correct / list.Count)));
        }

        return stats
            .OrderByDescending(stat => stat.TotalMs)
            .ThenBy(stat => stat.Subject.ToString(), StringComparer.Ordinal)
            .ToList();
    }

    internal static SubjectEffort Classify(int measuredCount, int median, int overallMedian, double accuracy)
    {
        if (measuredCount < MinMeasuredForVerdict || overallMedian <= 0)
        {
            return SubjectEffort.KeineDaten;
        }

        var slow = median >= overallMedian * SlowFactor;
        var fast = median <= overallMedian * FastFactor;
        var solid = accuracy >= SolidAccuracy;

        return (slow, fast, solid) switch
        {
            (true, _, false) => SubjectEffort.HierHaktEs,
            (true, _, true) => SubjectEffort.GruendlichAberLangsam,
            (_, true, false) => SubjectEffort.VermutlichGeraten,
            (_, true, true) => SubjectEffort.SitztSicher,
            _ => SubjectEffort.Unauffaellig
        };
    }

    private static int Median(IReadOnlyList<int> values)
    {
        if (values.Count == 0)
        {
            return 0;
        }

        var sorted = values.OrderBy(value => value).ToList();
        var middle = sorted.Count / 2;

        return sorted.Count % 2 == 1
            ? sorted[middle]
            : (sorted[middle - 1] + sorted[middle]) / 2;
    }
}
