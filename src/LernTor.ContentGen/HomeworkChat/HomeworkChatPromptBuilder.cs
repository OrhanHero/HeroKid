using System.Text;
using LernTor.Core.Models;

namespace LernTor.ContentGen.HomeworkChat;

/// <summary>
/// Gemeinsamer Prompt-Aufbau für beide <see cref="IHomeworkHelpChatService"/>-Implementierungen.
/// Bewusst wird der Aufgabenkontext, den das Modell bekommt, NIE um <c>Question.CorrectAnswers</c>
/// oder <c>Question.Explanation</c> ergänzt: kleine/quantisierte lokale Modelle folgen komplexen
/// Anweisungen ("verrate die Lösung nicht") nicht zuverlässig genug, um sich allein auf die
/// Prompt-Regeln zu verlassen. Wenn das Modell die richtige Antwort strukturell gar nicht kennt, kann
/// es sie auch nicht versehentlich verraten - das ist robuster als reine Anweisungsbefolgung.
/// </summary>
public static class HomeworkChatPromptBuilder
{
    private const string Guardrails =
        "Du bist ein freundlicher Lern-Assistent für ein Kind (Klasse 6 oder 9, Berlin). Das Kind arbeitet " +
        "gerade an der oben genannten Aufgabe und stellt dir dazu Fragen. Regeln, die du IMMER befolgst:\n" +
        "1. Verrate NIEMALS direkt die fertige/richtige Antwort oder das Lösungswort.\n" +
        "2. Hilf stattdessen durch Rückfragen, kleine Denkanstöße und einzelne Zwischenschritte, damit das " +
        "Kind selbst darauf kommt - wie eine Nachhilfelehrkraft, nicht wie ein Lösungsblatt.\n" +
        "3. Antworte kurz (2-4 Sätze), einfach und altersgerecht auf Deutsch.\n" +
        "4. Bleib beim Thema der Aufgabe, auch wenn das Kind vom Thema abweicht.";

    /// <summary>Aufgabenkontext ohne Lösung - bei NotebookLM die hochgeladene "Quelle", beim lokalen
    /// Modell der vorangestellte Teil des Gesamt-Prompts.</summary>
    public static string BuildQuestionContext(QuizQuestion question)
    {
        var sb = new StringBuilder();
        sb.AppendLine($"Fach: {question.Subject}, Klassenstufe: {question.GradeLevel}");
        sb.AppendLine($"Aufgabenstellung: {question.Prompt}");
        if (question.Options.Count > 0)
        {
            sb.AppendLine("Antwortmöglichkeiten: " + string.Join(", ", question.Options));
        }

        return sb.ToString();
    }

    /// <summary>Leitplanken + bisheriger Chatverlauf, endet offen mit "Assistent:" zur Fortsetzung.</summary>
    public static string BuildConversationPrompt(IReadOnlyList<ChatMessage> conversation)
    {
        var sb = new StringBuilder();
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

    /// <summary>Für das lokale LLM: ein einziger Prompt-String ohne den NotebookLM-typischen Split
    /// zwischen hochgeladener "Quelle" und Frage - Kontext und Leitplanken direkt vorangestellt.</summary>
    public static string BuildLocalPrompt(QuizQuestion question, IReadOnlyList<ChatMessage> conversation) =>
        BuildQuestionContext(question) + "\n" + BuildConversationPrompt(conversation);
}
