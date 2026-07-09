using LernTor.Core.Enums;

namespace LernTor.ContentGen.HomeworkChat;

/// <summary>
/// Welcher LLM-Anbieter aktuell für <see cref="CompositeHomeworkHelpChatService"/> aktiv ist. Eigene,
/// mutable Options-Klasse aus demselben Grund wie <see cref="TeacherImport.TeacherImportProviderOptions"/>:
/// wird vom Eltern-Bereich (ParentSettingsViewModel) aus <c>AppSettings.HomeworkChatProvider</c>
/// befüllt. Standard bewusst lokal, damit Kinder-Fragen ohne explizite Eltern-Entscheidung nie in
/// die Cloud gehen.
/// </summary>
public sealed class HomeworkChatProviderOptions
{
    public LlmProvider Provider { get; set; } = LlmProvider.LocalLlm;
}
