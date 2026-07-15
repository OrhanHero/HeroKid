using UglyToad.PdfPig;

namespace LernTor.ContentGen.TeacherImport;

/// <summary>Extrahiert reinen Text aus PDF-Dateien über die (reine .NET-, Windows-unabhängige) PdfPig-Bibliothek.</summary>
public sealed class PdfPigTextExtractor : ITeacherDocumentTextExtractor
{
    public bool CanHandle(string fileName) =>
        fileName.EndsWith(".pdf", StringComparison.OrdinalIgnoreCase);

    public Task<string> ExtractTextAsync(Stream fileStream, string fileName, CancellationToken cancellationToken = default)
    {
        // PdfPig braucht wahlfreien Lesezugriff (Seek) - ein hochgeladenes Dokument kommt i.d.R. aus
        // einem FileStream, notfalls hier in den Speicher kopieren.
        var seekableStream = fileStream.CanSeek ? fileStream : CopyToMemory(fileStream);

        using var document = PdfDocument.Open(seekableStream);
        var text = string.Join(
            "\n\n",
            document.GetPages().Select(page => page.Text));

        return Task.FromResult(text);
    }

    private static MemoryStream CopyToMemory(Stream source)
    {
        var memory = new MemoryStream();
        source.CopyTo(memory);
        memory.Position = 0;
        return memory;
    }
}
