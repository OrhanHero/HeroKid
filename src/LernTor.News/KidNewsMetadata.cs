using LernTor.Core.Models;

namespace LernTor.News;

/// <summary>
/// Kindgerechte Zusatz-Metadaten je Artikel: Lesedauer, Schwierigkeitsgrad und die Einordnungen
/// "Warum ist das wichtig?" / "Was bedeutet das für dich?".
///
/// <para><b>Ehrlichkeits-Prinzip:</b> Die Einordnungen sind rubrikbezogene, redaktionell
/// formulierte Standardtexte (neutral, ohne Angstmache, ohne politische Wertung - Stil logo!/
/// Checker-Sendungen). Eine regelbasierte Pipeline KANN nicht seriös artikel-spezifische
/// Analysen erfinden; lieber eine ehrliche allgemeine Einordnung als eine falsche konkrete.</para>
/// </summary>
public static class KidNewsMetadata
{
    /// <summary>Langsames Kinder-Lesetempo (Wörter pro Minute) als Basis der Lesedauer-Schätzung.</summary>
    private const int WordsPerMinute = 140;

    public static int ComputeReadingMinutes(params string?[] textParts)
    {
        var wordCount = textParts
            .Where(t => !string.IsNullOrWhiteSpace(t))
            .Sum(t => t!.Split(' ', StringSplitOptions.RemoveEmptyEntries).Length);

        return Math.Max(1, (int)Math.Ceiling(wordCount / (double)WordsPerMinute));
    }

    /// <summary>
    /// Heuristik über durchschnittliche Satzlänge und Anteil langer Wörter (≥ 12 Zeichen) -
    /// dieselben Größen, auf denen gängige Lesbarkeitsformeln (z.B. LIX) beruhen.
    /// </summary>
    public static NewsDifficulty ComputeDifficulty(string? text)
    {
        if (string.IsNullOrWhiteSpace(text))
        {
            return NewsDifficulty.Leicht;
        }

        var words = text.Split(' ', StringSplitOptions.RemoveEmptyEntries);
        if (words.Length == 0)
        {
            return NewsDifficulty.Leicht;
        }

        var sentenceCount = Math.Max(1, text.Count(c => c is '.' or '!' or '?'));
        var wordsPerSentence = words.Length / (double)sentenceCount;
        var longWordShare = words.Count(w => w.Trim('.', ',', '!', '?', ':', ';', '"').Length >= 12)
                            / (double)words.Length;

        var score = wordsPerSentence + longWordShare * 100;

        return score switch
        {
            < 18 => NewsDifficulty.Leicht,
            < 28 => NewsDifficulty.Mittel,
            _ => NewsDifficulty.Schwer
        };
    }

    /// <summary>"Warum ist das wichtig?" je Rubrik.</summary>
    public static string WhyImportantFor(NewsCategory category) => category switch
    {
        NewsCategory.Berlin =>
            "Das passiert direkt in Berlin - also dort, wo du wohnst. Nachrichten aus der eigenen " +
            "Stadt betreffen dich oft am meisten: deine Schule, deinen Kiez, deine Wege.",
        NewsCategory.Deutschland =>
            "Was in Deutschland entschieden wird, gilt für alle Menschen im Land - auch für dich " +
            "und deine Familie. Wer Bescheid weiß, kann mitreden.",
        NewsCategory.Welt =>
            "Die Welt hängt zusammen: Was in anderen Ländern passiert, wirkt sich oft auch auf " +
            "uns aus - beim Handel, beim Klima oder bei Erfindungen.",
        NewsCategory.Tuerkei =>
            "Viele Familien in Berlin haben Wurzeln in der Türkei. Zu wissen, was dort passiert, " +
            "hilft, beide Länder zu verstehen und mit Verwandten mitzureden.",
        NewsCategory.Ki =>
            "Künstliche Intelligenz verändert gerade, wie wir lernen, arbeiten und spielen. Wer " +
            "früh versteht, wie KI funktioniert, kann sie klug nutzen - wie einen Taschenrechner.",
        NewsCategory.Spiele =>
            "Millionen Kinder und Jugendliche spielen jeden Tag. Zu wissen, was in der " +
            "Spielewelt passiert, gehört heute zur Alltagskultur - wie früher Fernsehen und Musik.",
        NewsCategory.Finanzen =>
            "Geld spielt im Leben aller Menschen eine Rolle - beim Taschengeld genauso wie bei " +
            "großen Firmen. Wer Geld versteht, trifft später bessere Entscheidungen.",
        NewsCategory.Wetter =>
            "Wetter betrifft jeden Tag alle Menschen: Schulweg, Sport, Ausflüge. Und am Wetter " +
            "kann man beobachten, wie sich unser Klima langsam verändert.",
        _ => string.Empty
    };

    /// <summary>"Was bedeutet das für dich?" je Rubrik.</summary>
    public static string MeaningForKidsFor(NewsCategory category) => category switch
    {
        NewsCategory.Berlin =>
            "Vielleicht merkst du davon etwas auf deinem Schulweg, in deinem Bezirk oder bei " +
            "Angeboten für Kinder und Jugendliche in deiner Nähe.",
        NewsCategory.Deutschland =>
            "Solche Entscheidungen können deine Schule, deine Freizeit oder das Geld deiner " +
            "Familie betreffen - manchmal sofort, manchmal erst in ein paar Jahren.",
        NewsCategory.Welt =>
            "Auch wenn es weit weg klingt: Viele Dinge aus aller Welt landen bei uns - als " +
            "Produkte, Ideen oder neue Mitschülerinnen und Mitschüler mit ihren Geschichten.",
        NewsCategory.Tuerkei =>
            "Wenn deine Familie Verbindungen in die Türkei hat, kannst du beim nächsten " +
            "Telefonat oder Besuch mitreden - und allen anderen hilft es, Freunde besser zu verstehen.",
        NewsCategory.Ki =>
            "KI steckt schon jetzt in Handys, Spielen und Hausaufgaben-Helfern. Probiere Neues " +
            "neugierig aus - und bleib kritisch: Nicht alles, was eine KI sagt, stimmt.",
        NewsCategory.Spiele =>
            "Neue Spiele, Updates oder Regeln können genau die Spiele betreffen, die du und " +
            "deine Freunde spielt. Denk dran: Pausen machen und auf Altersfreigaben achten.",
        NewsCategory.Finanzen =>
            "Auch dein Taschengeld ist Wirtschaft im Kleinen: Wenn Preise steigen, bekommst du " +
            "fürs gleiche Geld weniger. Sparen und Vergleichen lohnt sich fast immer.",
        NewsCategory.Wetter =>
            "Schau vor dem Rausgehen auf das Wetter: richtige Kleidung, Sonnenschutz bei Hitze, " +
            "und bei Unwetterwarnungen lieber drinnen bleiben.",
        _ => string.Empty
    };
}
