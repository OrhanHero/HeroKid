namespace LernTor.Core.Models;

/// <summary>
/// Ein Gedicht/wichtiges Werk für den täglichen Pflicht-Leseabschnitt (siehe ReadingContentProvider).
/// Wird immer parallel in drei Sprachen angezeigt (nebeneinander), damit die Kinder denselben Text
/// einmal auf Deutsch, einmal auf Türkisch und einmal auf Englisch lesen können.
/// </summary>
public sealed class ReadingPiece
{
    public required string Title { get; init; }
    public required string Author { get; init; }

    public required string TextDe { get; init; }
    public required string TextTr { get; init; }
    public required string TextEn { get; init; }

    /// <summary>
    /// true = Pop-Kultur-Stück (Minecraft/Anime/etc.), false (Default) = literarisches Werk/
    /// Allgemeinwissen. Genutzt von <see cref="ReadingContentProvider.GetSecondForDate"/>, um
    /// unabhängig von der Gesamtgröße des Pools garantiert ein Stück aus jeder Kategorie zu wählen.
    /// </summary>
    public bool IsPopKultur { get; init; }
}
