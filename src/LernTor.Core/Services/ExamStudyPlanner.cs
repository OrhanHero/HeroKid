using LernTor.Core.Models;

namespace LernTor.Core.Services;

/// <summary>Ein Tag im Lernplan vor einer Klausur.</summary>
/// <param name="Date">Der Tag.</param>
/// <param name="DaysUntilExam">Tage bis zur Klausur (0 = Klausurtag).</param>
/// <param name="Topics">Was an diesem Tag drankommt (leer, wenn keine Themen eingetragen sind).</param>
/// <param name="IsReviewDay">Wiederholungstag - am Tag vor der Klausur wird alles noch einmal angesehen.</param>
public readonly record struct ExamStudyDay(
    DateOnly Date,
    int DaysUntilExam,
    IReadOnlyList<string> Topics,
    bool IsReviewDay);

/// <summary>
/// Verteilt die zu einer Klausur eingetragenen Themen auf die verbleibenden Tage.
///
/// <para>Die Lern-Gewichtung (<see cref="ExamEntry.LearningWeight"/>) erhöht vor einer Klausur
/// schon heute still die Aufgabenzahl im betroffenen Fach - <b>sichtbar ist davon nichts</b>.
/// Ein Kind merkt nur, dass es plötzlich mehr Mathe gibt, ohne den Zusammenhang zu sehen; und
/// "in 5 Tagen ist Mathe-Klausur" ist eine Information, aber noch kein Plan. Was fehlt, ist der
/// Satz, den ein Erwachsener beim Lernen selbst formuliert: <i>heute die Brüche</i>.</para>
///
/// <para><b>Ohne eingetragene Themen wird nichts erfunden.</b> Dann bleibt es bei "mehr Aufgaben
/// in Mathe" - ein ausgedachter Plan wäre schlimmer als keiner, weil das Kind ihm glauben würde.</para>
///
/// <para>Die Themen werden <b>wiederholt durchlaufen</b>, statt jedes nur einmal zu vergeben: bei
/// zwei Themen und sieben Tagen kommt jedes mehrfach dran, und genau das ist der Sinn des
/// Vorlaufs. Der letzte Tag vor der Klausur ist bewusst ein Wiederholungstag über alles - am
/// Vorabend ein neues Thema anzufangen hilft niemandem.</para>
/// </summary>
public static class ExamStudyPlanner
{
    /// <summary>Trennzeichen, mit denen Eltern und Kinder Themen aufzählen.</summary>
    private static readonly char[] TopicSeparators = { ',', ';', '\n', '\r', '/', '|' };

    /// <summary>Wie viele Themen höchstens an einem Tag stehen - eine lange Liste ist kein Plan.</summary>
    public const int MaxTopicsPerDay = 2;

    /// <summary>
    /// Zerlegt das Freitextfeld in einzelne Themen. Leere Stücke und Dubletten fallen weg,
    /// die Reihenfolge der Eingabe bleibt erhalten - sie ist oft die Reihenfolge im Unterricht.
    /// </summary>
    public static IReadOnlyList<string> ParseTopics(string? topics)
    {
        if (string.IsNullOrWhiteSpace(topics))
        {
            return Array.Empty<string>();
        }

        var seen = new HashSet<string>(StringComparer.CurrentCultureIgnoreCase);
        var result = new List<string>();

        foreach (var part in topics.Split(TopicSeparators, StringSplitOptions.RemoveEmptyEntries))
        {
            var topic = part.Trim(' ', '\t', '-', '.', '·');
            if (topic.Length > 0 && seen.Add(topic))
            {
                result.Add(topic);
            }
        }

        return result;
    }

    /// <summary>
    /// Der Plan für die Tage von heute bis zur Klausur (einschließlich). Leer, wenn die Klausur
    /// vorbei ist oder noch weiter weg als der Vorlauf (<see cref="ExamEntry.LearningBoostLeadDays"/>) -
    /// so lange gibt es auch keinen Grund, etwas anzuzeigen.
    /// </summary>
    public static IReadOnlyList<ExamStudyDay> BuildPlan(ExamEntry exam, DateOnly today)
    {
        var daysUntil = exam.DaysUntil(today);
        if (daysUntil < 0 || daysUntil > ExamEntry.LearningBoostLeadDays)
        {
            return Array.Empty<ExamStudyDay>();
        }

        var topics = ParseTopics(exam.Topics);
        var days = new List<ExamStudyDay>();

        // Rückwärts denken: der Tag vor der Klausur ist Wiederholung, davor laufen die Themen der
        // Reihe nach - so beginnt der Plan immer beim gleichen Thema, egal wann man draufschaut.
        for (var remaining = daysUntil; remaining >= 0; remaining--)
        {
            var date = today.AddDays(daysUntil - remaining);
            var isReviewDay = remaining <= 1 && topics.Count > 0;

            days.Add(new ExamStudyDay(
                date,
                remaining,
                isReviewDay ? topics : TopicsForDay(topics, remaining),
                isReviewDay));
        }

        return days;
    }

    /// <summary>
    /// Welche Themen an einem Tag mit <paramref name="daysUntilExam"/> Resttagen drankommen.
    /// Der Index zählt von der Klausur weg, damit derselbe Tag immer dasselbe Thema bekommt.
    /// </summary>
    private static IReadOnlyList<string> TopicsForDay(IReadOnlyList<string> topics, int daysUntilExam)
    {
        if (topics.Count == 0)
        {
            return Array.Empty<string>();
        }

        // Tag 7 vor der Klausur ist Position 0, Tag 6 Position 1 usw. (Tag 1 und 0 sind
        // Wiederholung und kommen hier nicht an).
        var position = ExamEntry.LearningBoostLeadDays - daysUntilExam;

        // Bei mehr Themen als Vorlauftagen kommen zwei an einem Tag dran, sonst faende die
        // Haelfte gar nicht statt. Mehr als zwei bewusst nie: eine lange Liste ist kein Plan,
        // sondern nur die Themenliste noch einmal. Was dabei vor dem Wiederholungstag nicht
        // drankam, steht dort ohnehin vollstaendig.
        var planDays = ExamEntry.LearningBoostLeadDays - 1;
        var perDay = topics.Count > planDays ? MaxTopicsPerDay : 1;

        return Enumerable.Range(0, perDay)
            .Select(offset => topics[(position * perDay + offset) % topics.Count])
            .Distinct(StringComparer.CurrentCultureIgnoreCase)
            .ToList();
    }

    /// <summary>Der Tag von heute aus dem Plan; null, wenn kein Plan läuft.</summary>
    public static ExamStudyDay? Today(ExamEntry exam, DateOnly today)
    {
        var plan = BuildPlan(exam, today);
        return plan.Count == 0 ? null : plan[0];
    }

    /// <summary>
    /// Der heutige Lernhinweis in einem Satz - genau das, was auf dem Startbildschirm an der
    /// Klausur steht. Leer, wenn kein Plan läuft.
    /// </summary>
    /// <param name="exam">Die Klausur.</param>
    /// <param name="today">Heutiges Datum.</param>
    /// <param name="subjectLabel">Fach im Klartext (Übersetzung liegt in der App).</param>
    public static string TodayHint(ExamEntry exam, DateOnly today, string subjectLabel)
    {
        if (Today(exam, today) is not { } day)
        {
            return string.Empty;
        }

        // Ohne eingetragene Themen bleibt es bei der ehrlichen Aussage: es gibt mehr Aufgaben.
        if (day.Topics.Count == 0)
        {
            return day.DaysUntilExam == 0
                ? $"Heute ist die {subjectLabel}-Klausur. Viel Erfolg!"
                : $"Deshalb gibt es heute mehr {subjectLabel}-Aufgaben.";
        }

        var topicList = string.Join(", ", day.Topics);

        return day.DaysUntilExam switch
        {
            0 => $"Heute ist die {subjectLabel}-Klausur. Viel Erfolg!",
            1 => $"Morgen ist es soweit - heute noch einmal alles ansehen: {topicList}.",
            _ => $"Heute dran: {topicList}."
        };
    }
}
