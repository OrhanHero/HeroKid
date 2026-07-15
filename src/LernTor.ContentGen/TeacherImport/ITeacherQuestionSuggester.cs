using LernTor.Core.Enums;

namespace LernTor.ContentGen.TeacherImport;

/// <summary>
/// Wandelt den Fließtext einer Lehrer-Unterlage in <see cref="ExtractedQuestionDraft"/>-Vorschläge um
/// (z.B. per LLM). Ergebnisse sind immer Vorschläge, keine fertigen Fragen - der Aufrufer (Eltern-
/// Bereich) muss sie review(en)/bearbeiten/bestätigen, bevor sie über
/// CustomQuestionRepository.AddAsync (LernTor.Data) dauerhaft gespeichert werden. Das ist bewusst
/// keine Automatik ohne menschliche Kontrolle: eine falsch erkannte "richtige Antwort" wäre schlimmer
/// als gar keine automatisch erzeugte Frage.
/// </summary>
public interface ITeacherQuestionSuggester
{
    Task<IReadOnlyList<ExtractedQuestionDraft>> SuggestQuestionsAsync(
        string documentText,
        Subject subject,
        GradeLevel gradeLevel,
        CancellationToken cancellationToken = default);
}
