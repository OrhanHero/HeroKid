namespace LernTor.Core.Services;

/// <summary>Wie eine Kurslektion steht.</summary>
public enum LessonStatus
{
    /// <summary>Noch nicht geöffnet.</summary>
    Offen,

    /// <summary>Gelesen, aber die Kontrolle noch nicht bestanden.</summary>
    Gelesen,

    /// <summary>Kontrolle bestanden.</summary>
    Geschafft
}

/// <summary>
/// Die Regeln des Theorie-Kurses.
///
/// <para><b>Gelesen und geschafft sind zwei verschiedene Dinge</b>, und beides wird getrennt
/// festgehalten. Nur "gelesen" zu zählen hieße, dass Durchscrollen als Lernen gilt; nur
/// "geschafft" zu zählen hieße, dass eine gelesene Lektion nach einer verpatzten Kontrolle
/// wieder aussieht wie nie geöffnet. Beides wäre gelogen.</para>
/// </summary>
public static class DrivingCourseRules
{
    /// <summary>Ab dieser Trefferquote gilt die Lernstandskontrolle als bestanden.</summary>
    public const int PassPercent = 70;

    /// <summary>
    /// Höchstzahl der Fragen in einer Lernstandskontrolle. Sie soll den Stoff der Lektion prüfen,
    /// nicht eine zweite Prüfung sein - wer nach dem Lesen zwölf Fragen vorgesetzt bekommt,
    /// liest beim nächsten Mal nicht mehr.
    /// </summary>
    public const int MaxCheckQuestions = 5;

    public static int Percent(int correct, int total) =>
        total == 0 ? 0 : (int)Math.Round(correct * 100.0 / total);

    /// <summary>Bestanden - über <see cref="Percent"/> gerechnet, damit die angezeigte Zahl und
    /// das Urteil nie auseinanderfallen können.</summary>
    public static bool HasPassed(int correct, int total) =>
        total > 0 && Percent(correct, total) >= PassPercent;
}
