using LernTor.Core.Enums;

namespace LernTor.ContentGen.TeacherImport;

/// <summary>
/// Welcher LLM-Anbieter aktuell für <see cref="CompositeTeacherQuestionSuggester"/> aktiv ist. Eigene,
/// mutable Options-Klasse aus demselben Grund wie <see cref="NotebookLmOptions"/>/<see cref="LocalLlmOptions"/>:
/// wird vom Eltern-Bereich (ParentSettingsViewModel) aus <c>AppSettings.TeacherImportProvider</c> befüllt.
/// </summary>
public sealed class TeacherImportProviderOptions
{
    public TeacherImportProvider Provider { get; set; } = TeacherImportProvider.NotebookLm;
}
