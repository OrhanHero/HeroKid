using System.Text;
using LernTor.Core.Models;

namespace LernTor.ContentGen.HomeworkChat;

/// <summary>
/// Prompt-Aufbau für <see cref="IHomeworkHelpChatService"/>. Das Modell bekommt die richtige(n)
/// Antwort(en) und die Erklärung der Aufgabe bewusst MIT in den Kontext: ohne sie zu kennen, kann die
/// KI kein echtes Lern-Werkzeug sein, sondern würde nur ins Blaue raten - das Kind soll sich mit der
/// Aufgabe auseinandersetzen und Verständnis aufbauen, nicht die KI raten sehen. Die Leitplanke, die
/// fertige Antwort nicht sofort preiszugeben, ist deshalb eine reine Prompt-Anweisung (siehe
/// <see cref="Guardrails"/>), kein struktureller Schutz.
/// </summary>
public static class HomeworkChatPromptBuilder
{
    private const string Guardrails =
        "Du bist ein freundlicher Lern-Assistent für ein Kind (Klasse 6 oder 9, Berlin). Das Kind arbeitet " +
        "gerade an der oben genannten Aufgabe und stellt dir dazu Fragen. Du kennst die richtige Antwort und " +
        "die Erklärung (oben angegeben) - nutze dieses Wissen, um wirklich zu helfen, aber halte dich an " +
        "diese Regeln:\n" +
        "1. Verrate die fertige Antwort nicht schon in deiner ERSTEN Nachricht - hilf zuerst durch " +
        "Rückfragen und kleine Denkanstöße, damit das Kind selbst darauf kommt, wie eine Nachhilfelehrkraft.\n" +
        "2. Wenn das Kind es mehrfach probiert hat oder ausdrücklich nach der Lösung fragt, erkläre sie " +
        "verständlich - besser eine erklärte Antwort als ein frustriertes Kind.\n" +
        "3. Antworte kurz (2-5 Sätze), einfach und altersgerecht auf Deutsch.\n" +
        "4. Bleib beim Thema der Aufgabe, auch wenn das Kind vom Thema abweicht.";

    public static string BuildPrompt(QuizQuestion question, IReadOnlyList<ChatMessage> conversation)
    {
        var sb = new StringBuilder();
        sb.AppendLine($"Fach: {question.Subject}, Klassenstufe: {question.GradeLevel}");
        sb.AppendLine($"Aufgabenstellung: {question.Prompt}");
        if (question.Options.Count > 0)
        {
            sb.AppendLine("Antwortmöglichkeiten: " + string.Join(", ", question.Options));
        }

        sb.AppendLine("Richtige Antwort: " + string.Join(" / ", question.CorrectAnswers));
        sb.AppendLine("Erklärung: " + question.Explanation);
        sb.AppendLine();
        sb.AppendLine(Guardrails);
        sb.AppendLine();
        sb.AppendLine("Bisheriger Chatverlauf:");
        foreach (var message in conversation)
        {
            var speaker = message.Role == ChatRole.Kind ? "Kind" : "Assistent";
            sb.AppendLine($"{speaker}: {message.Text}");
        }

        sb.Append("Assistent:");
        return sb.ToString();
    }
}
