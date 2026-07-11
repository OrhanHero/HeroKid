using System.Text;
using LernTor.Core.Models;

namespace LernTor.ContentGen.HomeworkChat;

/// <summary>
/// Prompt-Aufbau für <see cref="IHomeworkHelpChatService"/>. Das Modell bekommt die richtige(n)
/// Antwort(en) und die Erklärung der Aufgabe bewusst MIT in den Kontext: ohne sie zu kennen, kann die
/// KI kein echtes Lern-Werkzeug sein, sondern würde nur ins Blaue raten - das Kind soll sich mit der
/// Aufgabe auseinandersetzen und Verständnis aufbauen, nicht die KI raten sehen. Die Leitplanke, die
/// fertige Antwort nicht sofort preiszugeben, ist deshalb eine reine Prompt-Anweisung, kein
/// struktureller Schutz.
///
/// <para>Struktur auf Qwen2.5-Instruct zugeschnitten (klare Abschnitts-Überschriften, nummerierte
/// Regeln, ein Beispiel für den gewünschten Ton): das 7B-Modell folgt solchen gegliederten
/// Anweisungen deutlich zuverlässiger als einem Fließtext-Prompt. Das Gesprächsformat
/// "Kind:"/"Assistent:" ist fest mit den AntiPrompts-Stoppwörtern in
/// <see cref="LocalLlmHomeworkHelpChatService"/> verzahnt - bei Änderungen beide Stellen anpassen.</para>
/// </summary>
public static class HomeworkChatPromptBuilder
{
    public static string BuildPrompt(QuizQuestion question, IReadOnlyList<ChatMessage> conversation)
    {
        var sb = new StringBuilder();

        sb.AppendLine("### AUFGABE, AN DER DAS KIND GERADE ARBEITET");
        sb.AppendLine($"Fach: {question.Subject} | Klassenstufe: {question.GradeLevel} (Berliner Rahmenlehrplan)");
        sb.AppendLine($"Aufgabenstellung: {question.Prompt}");
        if (question.Options.Count > 0)
        {
            sb.AppendLine("Antwortmöglichkeiten: " + string.Join(" | ", question.Options));
        }

        sb.AppendLine("Richtige Antwort (nur für dich, nicht sofort verraten): " + string.Join(" / ", question.CorrectAnswers));
        sb.AppendLine("Erklärung (nur für dich): " + question.Explanation);
        sb.AppendLine();
        sb.AppendLine("### DEINE ROLLE");
        sb.AppendLine(
            "Du bist ein geduldiger, freundlicher Nachhilfelehrer für ein Kind aus Berlin (deutsch-türkische " +
            "Familie). Du kennst die richtige Antwort - dein Ziel ist aber, dass das Kind sie SELBST findet " +
            "und die Idee dahinter versteht, wie bei einem Taschenrechner: ein Werkzeug, kein Lösungsblatt.");
        sb.AppendLine();
        sb.AppendLine("### REGELN");
        sb.AppendLine("1. Stelle pro Antwort genau EINE Rückfrage oder gib EINEN kleinen Denkanstoß - nie mehrere Schritte auf einmal.");
        sb.AppendLine("2. Verrate die Lösung nicht in deiner ersten Antwort. Erst wenn das Kind zweimal falsch lag oder ausdrücklich um die Lösung bittet, erkläre sie Schritt für Schritt.");
        sb.AppendLine("3. Antworte kurz (2-4 Sätze), in einfacher Sprache passend zur Klassenstufe. Lobe konkrete Denkschritte des Kindes, nicht pauschal.");
        sb.AppendLine("4. Schreibt das Kind auf Türkisch, antworte auf Türkisch; sonst auf Deutsch. Fachbegriffe darfst du in beiden Sprachen nennen.");
        sb.AppendLine("5. Bleib beim Thema der Aufgabe, auch wenn das Kind abschweift - führe es freundlich zurück.");
        sb.AppendLine("6. Erfinde keine weiteren Gesprächsrunden: gib genau EINE Assistent-Antwort und höre dann auf.");
        sb.AppendLine();
        sb.AppendLine("### BEISPIEL FÜR DEN GEWÜNSCHTEN TON");
        sb.AppendLine("Kind: Ich weiß nicht, wie ich anfangen soll.");
        sb.AppendLine("Assistent: Kein Problem, das kriegen wir hin! Lies die Aufgabe noch einmal langsam: Welche Zahl oder welches Wort ist darin wohl am wichtigsten?");
        sb.AppendLine();
        sb.AppendLine("### AKTUELLES GESPRÄCH");
        foreach (var message in conversation)
        {
            var speaker = message.Role == ChatRole.Kind ? "Kind" : "Assistent";
            sb.AppendLine($"{speaker}: {message.Text}");
        }

        sb.Append("Assistent:");
        return sb.ToString();
    }
}
