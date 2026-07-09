using LernTor.Core.Models;

namespace LernTor.ContentGen.HomeworkChat;

/// <summary>
/// KI-Lernchat: Kinder können zu jeder Aufgabe (News, Übung, Abschlussquiz - überall, wo
/// <see cref="LernTor.App.Controls.QuestionCard"/> verwendet wird) Fragen stellen, wie sie einen
/// Taschenrechner oder ein Nachschlagewerk nutzen würden. Bewusst kein "gib mir die Lösung"-Automat:
/// siehe <see cref="HomeworkChatPromptBuilder"/> für die Leitplanken, die verhindern sollen, dass die
/// KI die fertige Antwort einfach verrät.
/// </summary>
public interface IHomeworkHelpChatService
{
    /// <summary>
    /// <paramref name="conversation"/> enthält den bisherigen Chatverlauf inklusive der neuesten
    /// Kind-Nachricht als letztem Eintrag. Gibt nur die neue Antwort des Assistenten zurück - der
    /// Aufrufer hängt sie selbst an den sichtbaren Verlauf an.
    /// </summary>
    Task<string> AskAsync(
        QuizQuestion question,
        IReadOnlyList<ChatMessage> conversation,
        CancellationToken cancellationToken = default);
}
