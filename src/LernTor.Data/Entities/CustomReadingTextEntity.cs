namespace LernTor.Data.Entities;

/// <summary>
/// Ein von den Eltern selbst hinterlegter Lesetext für den täglichen Vorlese-Abschnitt
/// (z.B. ein Gedicht, das gerade in der Schule dran ist, oder ein türkischer Text aus der Familie).
/// Ergänzt den festen Pool aus <c>ReadingContentProvider</c>, ersetzt ihn nicht.
///
/// <para>Pro Profil, weil ein Zehnjähriger und ein Fünfzehnjähriger unterschiedliche Texte
/// brauchen - genau wie bei den eigenen Tipp-Texten.</para>
///
/// <para>Die drei Sprachfelder sind einzeln optional: Eltern haben ein Gedicht meist nur in einer
/// Sprache vorliegen. Leere Felder zeigt die Leseansicht mit einem kurzen Hinweis an, statt eine
/// leere Spalte zu lassen.</para>
/// </summary>
public sealed class CustomReadingTextEntity
{
    public string Id { get; set; } = string.Empty;
    public string ProfileId { get; set; } = string.Empty;
    public string Title { get; set; } = string.Empty;
    public string Author { get; set; } = string.Empty;
    public string TextDe { get; set; } = string.Empty;
    public string TextTr { get; set; } = string.Empty;
    public string TextEn { get; set; } = string.Empty;
    public DateTimeOffset CreatedAt { get; set; } = DateTimeOffset.Now;
}
