namespace LernTor.Core.Models;

/// <summary>
/// Von den Eltern im Eltern-Bereich hinterlegte eigene Zieltexte für die beiden letzten
/// Tipp-Lektionen. Beide Felder sind optional - ist eines leer, bleibt der eingebaute Text stehen.
///
/// <para>Bewusst nur die letzten beiden Lektionen: die Aufbaulektionen (Grundreihe, Oberreihe,
/// Unterreihe, Zahlen) trainieren gezielt einzelne Tastenbereiche, ein freier Text würde diesen
/// Zweck zerstören. Lektion 6 (Sätze) und die Abschluss-Lektion sind dagegen ohnehin freie
/// Fließtexte - dort ist ein eigener Text sinnvoll (Steckbrief, Lieblingsbuch, Vokabelsatz).</para>
///
/// <para>Die Längenbegrenzung ist kein technisches Limit, sondern pädagogisch: ein Kind, das
/// 600 Zeichen abtippen muss, gibt entnervt auf, statt sauber zu tippen.</para>
/// </summary>
public sealed record TypingTextOverrides
{
    /// <summary>Kürzester zulässiger eigener Text - darunter lohnt eine Übung nicht.</summary>
    public const int MinLength = 20;

    /// <summary>Längster zulässiger eigener Text. Die Eltern-Oberfläche zeigt diesen Wert an.</summary>
    public const int MaxLength = 200;

    /// <summary>Ersetzt den Zieltext von Lektion 6 (Einfache Sätze).</summary>
    public string? SentenceText { get; init; }

    /// <summary>Ersetzt den Zieltext der profil-spezifischen Abschluss-Lektion.</summary>
    public string? FinalText { get; init; }

    /// <summary>Keine Überschreibung - die eingebauten Texte bleiben stehen.</summary>
    public static readonly TypingTextOverrides None = new();

    public bool IsEmpty => string.IsNullOrWhiteSpace(SentenceText) && string.IsNullOrWhiteSpace(FinalText);

    /// <summary>
    /// Bereinigt einen von Eltern eingegebenen Text: Zeilenumbrüche und Mehrfach-Leerzeichen werden
    /// zu einem einfachen Leerzeichen, danach wird auf <see cref="MaxLength"/> gekürzt. Texte unter
    /// <see cref="MinLength"/> Zeichen ergeben <c>null</c> (= eingebauter Text bleibt aktiv), damit
    /// ein versehentlich halb geleertes Feld nicht in einer 3-Zeichen-Übung endet.
    /// </summary>
    public static string? Sanitize(string? raw)
    {
        if (string.IsNullOrWhiteSpace(raw)) return null;

        var collapsed = string.Join(' ', raw.Split((char[]?)null, StringSplitOptions.RemoveEmptyEntries));
        if (collapsed.Length > MaxLength)
        {
            collapsed = collapsed[..MaxLength].TrimEnd();
        }

        return collapsed.Length >= MinLength ? collapsed : null;
    }

    /// <summary>Baut die Überschreibungen aus den Rohwerten eines Profils.</summary>
    public static TypingTextOverrides From(string? sentenceText, string? finalText) => new()
    {
        SentenceText = Sanitize(sentenceText),
        FinalText = Sanitize(finalText)
    };
}
