using LernTor.Core.Enums;

namespace LernTor.ContentGen.TeacherImport;

/// <summary>
/// Orchestriert Textextraktion + Fragen-Vorschlag für eine hochgeladene Lehrer-Unterlage. Reine
/// Verdrahtung ohne eigene Fachlogik, damit Extraktion und LLM-Anbindung unabhängig ausgetauscht/
/// getestet werden können (siehe <see cref="ITeacherDocumentTextExtractor"/>,
/// <see cref="ITeacherQuestionSuggester"/>).
/// </summary>
public sealed class TeacherDocumentImportService
{
    private readonly IReadOnlyList<ITeacherDocumentTextExtractor> _extractors;
    private readonly ITeacherQuestionSuggester _suggester;

    public TeacherDocumentImportService(
        IEnumerable<ITeacherDocumentTextExtractor> extractors,
        ITeacherQuestionSuggester suggester)
    {
        _extractors = extractors.ToList();
        _suggester = suggester;
    }

    public async Task<IReadOnlyList<ExtractedQuestionDraft>> ImportAsync(
        Stream fileStream,
        string fileName,
        Subject subject,
        GradeLevel gradeLevel,
        CancellationToken cancellationToken = default)
    {
        var text = await ExtractTextAsync(fileStream, fileName, cancellationToken);
        return await _suggester.SuggestQuestionsAsync(text, subject, gradeLevel, cancellationToken);
    }

    /// <summary>
    /// Nur die Textextraktion, ohne LLM. Der Lesetext-Import braucht genau diesen Zwischenschritt:
    /// aus einem Dokument einen Lesetext zu machen ist Aufbereitung (siehe
    /// <see cref="ReadingTextImport"/>), kein Fragen-Erfinden - ein mehrere Gigabyte großes Modell
    /// dafür zu laden wäre die falsche Größenordnung.
    /// </summary>
    public async Task<string> ExtractTextAsync(
        Stream fileStream,
        string fileName,
        CancellationToken cancellationToken = default)
    {
        var extractor = _extractors.FirstOrDefault(e => e.CanHandle(fileName))
            ?? throw new NotSupportedException(
                $"Kein Textextraktor für '{fileName}' registriert. Unterstützte Formate hängen davon " +
                "ab, welche ITeacherDocumentTextExtractor-Implementierungen registriert wurden.");

        return await extractor.ExtractTextAsync(fileStream, fileName, cancellationToken);
    }
}
