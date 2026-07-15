using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Wordprocessing;

namespace LernTor.ContentGen.TeacherImport;

/// <summary>Extrahiert reinen Text aus Word-(.docx)-Dateien über die (reine .NET-, Windows-unabhängige)
/// DocumentFormat.OpenXml-Bibliothek. Nur .docx (Open-XML-Format), kein altes binäres .doc.</summary>
public sealed class OpenXmlWordTextExtractor : ITeacherDocumentTextExtractor
{
    public bool CanHandle(string fileName) =>
        fileName.EndsWith(".docx", StringComparison.OrdinalIgnoreCase);

    public Task<string> ExtractTextAsync(Stream fileStream, string fileName, CancellationToken cancellationToken = default)
    {
        using var document = WordprocessingDocument.Open(fileStream, isEditable: false);
        var body = document.MainDocumentPart?.Document?.Body;

        if (body is null)
        {
            return Task.FromResult(string.Empty);
        }

        // Absatzweise statt body.InnerText, damit Absätze durch Zeilenumbrüche getrennt bleiben
        // (InnerText würde den gesamten Fließtext ohne jede Trennung aneinanderreihen).
        var paragraphs = body.Descendants<Paragraph>().Select(p => p.InnerText);
        var text = string.Join("\n", paragraphs);

        return Task.FromResult(text);
    }
}
