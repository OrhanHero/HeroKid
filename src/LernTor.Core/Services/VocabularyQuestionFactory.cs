using LernTor.Core.Enums;
using LernTor.Core.Models;

namespace LernTor.Core.Services;

/// <summary>
/// Macht aus den hinterlegten Vokabelpaaren abfragbare Aufgaben. Die Fragen entstehen bei jedem
/// Aufruf neu aus der Datenbank - anders als bei den Fach-Generatoren gibt es hier keinen
/// kuratierten Pool, sondern die Liste, die die Eltern gerade gepflegt haben.
/// </summary>
public static class VocabularyQuestionFactory
{
    /// <summary>Themenname der Vokabelaufgaben - taucht so in der Themen-Heatmap des Elternberichts auf.</summary>
    public const string Topic = "Vokabeln";

    /// <summary>
    /// Baut eine Aufgabe aus einem Wortpaar. Die Abfragerichtung wechselt <em>je Vokabel und Tag</em>
    /// deterministisch: dieselbe Vokabel wird am selben Tag immer gleich herum gefragt (sonst
    /// würde ein Neustart der App die Aufgabe verändern), an einem anderen Tag aber andersherum.
    /// Beide Richtungen sind nötig - wer nur Fremdsprache→Deutsch übt, kann das Wort erkennen,
    /// aber nicht selbst benutzen.
    /// </summary>
    public static QuizQuestion Create(VocabularyEntry entry, GradeLevel gradeLevel, DateOnly date)
    {
        var foreignToGerman = ((uint)(StableHash(entry.Id) + date.DayOfYear)) % 2 == 0;

        var (prompt, answer, hint) = foreignToGerman
            ? ($"Was heißt \"{entry.Foreign}\" auf Deutsch?", entry.German,
                $"Übersetze ins Deutsche. Die Vokabel steht in deiner Liste für {SubjectLabel(entry.Subject)}.")
            : ($"Wie heißt \"{entry.German}\" auf {SubjectLabel(entry.Subject)}?", entry.Foreign,
                $"Übersetze ins {SubjectLabel(entry.Subject)}e. Achte auf die richtige Schreibweise.");

        return new QuizQuestion
        {
            Id = $"vokabel-{entry.Id}",
            Subject = entry.Subject,
            GradeLevel = gradeLevel,
            Topic = Topic,
            Type = QuestionType.OpenText,
            Prompt = prompt,
            CorrectAnswers = new[] { answer },
            Explanation = $"\"{entry.German}\" heißt auf {SubjectLabel(entry.Subject)} \"{entry.Foreign}\".",
            HelpHint = hint,
            // Türkische Vokabeln brauchen die Sonderzeichen-Hilfe (ç ğ ı ş) auf deutscher Tastatur.
            RequiresTurkishCharacters = entry.Subject == Subject.Tuerkisch && !foreignToGerman
        };
    }

    private static string SubjectLabel(Subject subject) => subject == Subject.Tuerkisch ? "Türkisch" : "Englisch";

    /// <summary>
    /// Stabiler Hash über die Id. <see cref="string.GetHashCode()"/> ist in .NET pro Prozess
    /// zufällig gesalzen - damit würde dieselbe Vokabel nach jedem App-Start die Richtung wechseln.
    /// </summary>
    private static int StableHash(string value)
    {
        unchecked
        {
            int hash = 17;
            foreach (var c in value)
            {
                hash = hash * 31 + c;
            }

            return hash & 0x7FFFFFFF;
        }
    }
}
