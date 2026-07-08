using LernTor.Core.Enums;

namespace LernTor.ContentGen.TeacherImport;

/// <summary>
/// Orchestriert Textextraktion + Fragen-Vorschlag für eine hochgeladene Lehrer-Unterlage. Reine
/// Verdrahtung ohne eigene Fachlogik, damit Extraktion und LLM-Anbindung unabhängig ausgetauscht/
/// getestet werden können (siehe <see cref="ITeacherDocumentTextExtractor"/>,
/// <see cref="ITeacherQuestionSuggester"/>).
///
/// Noch nicht mit einer echten Implementierung von <see cref="ITeacherQuestionSuggester"/> verdrahtet
/// und noch nicht an eine Eltern-Bereich-UI angebunden - siehe README, Abschnitt "Automatisches
/// Einlesen von Lehrer-Unterlagen", für die noch offenen Entscheidungen (LLM-Anbieter, Datenschutz).
/// </summary>
public sealed class TeacherDocumentImportService
{
    private readonly IReadOnlyList<ITeacherDocumentTextExtractor> _extractors;
    private readonly ITeacherQuestionSuggester _suggester;

    public TeacherDocumentImportService(
        IReadOnlyList<ITeacherDocumentTextExtractor> extractors,
        ITeacherQuestionSuggester suggester)
    {
        _extractors = extractors;
        _suggester = suggester;
    }

    public async Task<IReadOnlyList<ExtractedQuestionDraft>> ImportAsync(
        Stream fileStream,
        string fileName,
        Subject subject,
        GradeLevel gradeLevel,
        CancellationToken cancellationToken = default)
    {
        var extractor = _extractors.FirstOrDefault(e => e.CanHandle(fileName))
            ?? throw new NotSupportedException(
                $"Kein Textextraktor für '{fileName}' registriert. Unterstützte Formate hängen davon " +
                "ab, welche ITeacherDocumentTextExtractor-Implementierungen registriert wurden.");

        var text = await extractor.ExtractTextAsync(fileStream, fileName, cancellationToken);
        return await _suggester.SuggestQuestionsAsync(text, subject, gradeLevel, cancellationToken);
    }
}

/// <summary>
/// Platzhalter-Implementierung von <see cref="ITeacherQuestionSuggester"/>, solange keine echte
/// LLM-Anbindung konfiguriert ist. Wirft absichtlich statt still leere Listen zurückzugeben, damit ein
/// versehentlich verdrahteter Aufruf sofort auffällt statt Eltern eine leere Vorschlagsliste ohne
/// Erklärung zu zeigen.
/// </summary>
public sealed class NotConfiguredTeacherQuestionSuggester : ITeacherQuestionSuggester
{
    public Task<IReadOnlyList<ExtractedQuestionDraft>> SuggestQuestionsAsync(
        string documentText,
        Subject subject,
        GradeLevel gradeLevel,
        CancellationToken cancellationToken = default)
    {
        throw new NotSupportedException(
            "Automatisches Einlesen von Lehrer-Unterlagen ist vorbereitet, aber noch nicht mit einem " +
            "LLM-Anbieter verdrahtet (siehe README, Abschnitt 'Automatisches Einlesen von " +
            "Lehrer-Unterlagen'). Der manuelle Eigene-Aufgaben-Editor im Eltern-Bereich deckt den " +
            "Kernbedarf in der Zwischenzeit ab.");
    }
}
