namespace LernTor.Core.Models;

/// <summary>Ein Gedicht/wichtiges Werk für den täglichen Pflicht-Leseabschnitt (siehe ReadingContentProvider).</summary>
public sealed class ReadingPiece
{
    public required string Title { get; init; }
    public required string Author { get; init; }

    /// <summary>Anzeigesprache des Texts: "Deutsch", "Türkçe", "English" oder "Allgemeinwissen".</summary>
    public required string Language { get; init; }

    public required string Text { get; init; }
}
