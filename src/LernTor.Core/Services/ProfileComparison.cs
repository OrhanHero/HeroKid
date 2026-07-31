namespace LernTor.Core.Services;

/// <summary>Eine Zeile des Geschwister-Vergleichs: die Bilanz eines Kindes im Berichtszeitraum.</summary>
/// <param name="ProfileId">Profil-Id (für die Zuordnung, nicht zur Anzeige).</param>
/// <param name="Name">Anzeigename des Kindes.</param>
/// <param name="LearnedDays">Tage mit mindestens einer beantworteten Aufgabe.</param>
/// <param name="Answered">Beantwortete Aufgaben insgesamt.</param>
/// <param name="Correct">Davon richtig.</param>
/// <param name="LearningTimeMs">Summe der gemessenen Antwortdauern (Alt-Zeilen ohne Messung zählen nicht mit).</param>
/// <param name="TotalStars">Gesammelte Sterne insgesamt - bewusst der Gesamtstand, nicht der des
/// Zeitraums: das Sterne-Konto ist ein laufendes Guthaben (siehe <c>StudentProfile.TotalStars</c>).</param>
public readonly record struct ProfileComparisonRow(
    string ProfileId,
    string Name,
    int LearnedDays,
    int Answered,
    int Correct,
    long LearningTimeMs,
    int TotalStars)
{
    public double Accuracy => Answered > 0 ? (double)Correct / Answered : 0;

    public TimeSpan LearningTime => TimeSpan.FromMilliseconds(LearningTimeMs);

    /// <summary>Falsch, wenn das Kind im Zeitraum gar nichts bearbeitet hat - die Zeile bleibt
    /// trotzdem stehen, sonst verschwände genau das Kind, über das man reden müsste.</summary>
    public bool HasData => Answered > 0;
}

/// <summary>
/// Stellt die Lernbilanz mehrerer Kinder nebeneinander - optional und standardmäßig ABGESCHALTET
/// (<c>AppSettings.ProfileComparisonEnabled</c>).
///
/// <para>Das Abschalten ist keine Bequemlichkeit, sondern der Kern der Sache: Geschwister
/// gegeneinander zu stellen kann motivieren, aber genauso gut das Kind beschädigen, das immer
/// hinten liegt. Ob das in einer Familie hilft oder schadet, können nur die Eltern beurteilen -
/// also ist es eine bewusste Entscheidung und keine Voreinstellung.</para>
///
/// <para>Aus demselben Grund sortiert <see cref="Build"/> <b>nach Namen</b> und nicht nach
/// Leistung: eine nach Trefferquote sortierte Liste ist ein Siegertreppchen, und ein Kind steht
/// dort dann dauerhaft unten. Die Zahlen stehen nebeneinander, die Deutung bleibt bei den Eltern.</para>
/// </summary>
public static class ProfileComparison
{
    /// <summary>Unter so vielen Profilen gibt es nichts zu vergleichen.</summary>
    public const int MinProfiles = 2;

    /// <summary>Eine beantwortete Aufgabe aus dem Aktivitätsprotokoll.</summary>
    /// <param name="Day">Lerntag (lokales Datum).</param>
    /// <param name="WasCorrect">Richtig beantwortet.</param>
    /// <param name="DurationMs">Dauer in Millisekunden; 0 oder negativ = nicht gemessen.</param>
    public readonly record struct Answer(DateOnly Day, bool WasCorrect, int DurationMs);

    /// <summary>Ein Kind mit seinen Antworten im Zeitraum.</summary>
    public readonly record struct Input(string ProfileId, string Name, int TotalStars, IReadOnlyList<Answer> Answers);

    /// <summary>
    /// Liefert je Kind eine Zeile, alphabetisch nach Name. Bei weniger als
    /// <see cref="MinProfiles"/> Kindern leer - ein "Vergleich" mit sich selbst wäre nur eine
    /// zweite, schlechtere Darstellung des ohnehin vorhandenen Berichts.
    /// </summary>
    public static IReadOnlyList<ProfileComparisonRow> Build(IEnumerable<Input> profiles)
    {
        var inputs = profiles.ToList();
        if (inputs.Count < MinProfiles)
        {
            return Array.Empty<ProfileComparisonRow>();
        }

        return inputs
            .Select(input =>
            {
                var answers = input.Answers ?? Array.Empty<Answer>();

                return new ProfileComparisonRow(
                    input.ProfileId,
                    input.Name,
                    LearnedDays: answers.Select(answer => answer.Day).Distinct().Count(),
                    Answered: answers.Count,
                    Correct: answers.Count(answer => answer.WasCorrect),
                    LearningTimeMs: answers.Where(answer => answer.DurationMs > 0).Sum(answer => (long)answer.DurationMs),
                    input.TotalStars);
            })
            .OrderBy(row => row.Name, StringComparer.CurrentCultureIgnoreCase)
            .ThenBy(row => row.ProfileId, StringComparer.Ordinal)
            .ToList();
    }
}
