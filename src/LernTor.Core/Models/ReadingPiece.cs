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

    /// <summary>
    /// true = von den Eltern im Eltern-Bereich hinterlegter Text (siehe
    /// <c>CustomReadingTextRepository</c>). Solche Texte bekommen bewusst einen der beiden
    /// Tagesplätze, statt sich unter 63 eingebaute Stücke zu mischen - ein Gedicht, das nächste
    /// Woche in der Schule dran ist, nützt nichts, wenn es erst in zwei Monaten drankommt.
    /// </summary>
    public bool IsCustom { get; init; }

    /// <summary>
    /// Ob für diese Sprache überhaupt ein Text vorliegt. Eltern haben ein Gedicht meist nur in
    /// einer Sprache - die Leseansicht zeigt für leere Sprachen einen Hinweis statt einer
    /// leeren Spalte.
    /// </summary>
    public bool HasText(string language) => language switch
    {
        "De" => !string.IsNullOrWhiteSpace(TextDe),
        "Tr" => !string.IsNullOrWhiteSpace(TextTr),
        "En" => !string.IsNullOrWhiteSpace(TextEn),
        _ => false
    };
}
