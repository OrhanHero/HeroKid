namespace LernTor.ContentGen.TeacherImport;

/// <summary>
/// Extrahiert reinen Text aus einer von den Eltern hochgeladenen Lehrer-Unterlage (PDF oder Word),
/// als erster Schritt vor der eigentlichen Fragen-Vorschlag-Generierung durch
/// <see cref="ITeacherQuestionSuggester"/>. Getrennt von der Vorschlags-Erzeugung, damit die
/// Textextraktion (reine Bibliotheksarbeit, kein LLM nötig) unabhängig von der LLM-Anbindung
/// getestet werden kann.
///
/// Mögliche Implementierungen: PdfPig für PDF, DocumentFormat.OpenXml für .docx - beides reine
/// .NET-Bibliotheken ohne Windows-Abhängigkeit. Noch nicht eingebunden (siehe README).
/// </summary>
public interface ITeacherDocumentTextExtractor
{
    /// <summary>Prüft anhand des Dateinamens/der Endung, ob dieser Extractor zuständig ist.</summary>
    bool CanHandle(string fileName);

    /// <summary>Liest den Dateiinhalt und gibt den enthaltenen Fließtext zurück.</summary>
    Task<string> ExtractTextAsync(Stream fileStream, string fileName, CancellationToken cancellationToken = default);
}
