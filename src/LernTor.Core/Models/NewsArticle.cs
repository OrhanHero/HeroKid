namespace LernTor.Core.Models;

public enum NewsRegionFocus
{
    Deutschland,
    Berlin,
    Tuerkei,
    Istanbul,
    Samsun,
    Uenye,
    International
}

/// <summary>
/// Rubriken des News-Bereichs (siehe README, "News für Kinder"). Berlin ist die wichtigste
/// regionale Rubrik; KI/Spiele/Finanzen/Wetter sind Themen-Rubriken, die quer über alle Quellen
/// per Schlüsselwort-Klassifikation erkannt werden (siehe LernTor.News.NewsCategoryClassifier).
/// </summary>
public enum NewsCategory
{
    Berlin,
    Deutschland,
    Welt,
    Tuerkei,
    Ki,
    Spiele,
    Finanzen,
    Wetter
}

/// <summary>Geschätzter Schwierigkeitsgrad eines Artikels (Satz-/Wortlängen-Heuristik) -
/// hilft Kindern beim Einschätzen, ob ein Text leicht oder anstrengend zu lesen ist.</summary>
public enum NewsDifficulty
{
    Leicht,
    Mittel,
    Schwer
}

/// <summary>Ein im Artikeltext erkanntes schwieriges Wort samt kindgerechter Erklärung
/// (siehe LernTor.News.KidTermGlossary) - "ein schwieriges Wort = sofort erklären".</summary>
public sealed record ExplainedTerm(string Term, string Explanation);

public sealed record NewsArticle
{
    public required string Id { get; init; }
    public required string Title { get; init; }

    /// <summary>Kindgerecht vereinfachte Zusammenfassung (einfache Sprache, 10-15 Jahre).</summary>
    public required string SimplifiedSummary { get; init; }

    public string? ImageUrl { get; init; }
    public required string SourceName { get; init; }
    public required string SourceUrl { get; init; }
    public required DateTimeOffset PublishedAt { get; init; }
    public required NewsRegionFocus RegionFocus { get; init; }
    public required IReadOnlyList<QuizQuestion> ComprehensionQuestions { get; init; }

    // --- Kindgerechte Zusatz-Aufbereitung (Rubrik, Lesedauer, Erklärungen; siehe README) ---
    // Bewusst optional mit Standardwerten statt "required": bestehende Erzeuger/Tests bleiben
    // gültig, und die Anreicherung passiert zentral in RssNewsService.BuildArticle.

    public NewsCategory Category { get; init; } = NewsCategory.Deutschland;

    /// <summary>Emoji der Rubrik (z.B. 🐻 Berlin, 🤖 KI) - Kinder erkennen Rubriken an
    /// Symbolen schneller als an Text.</summary>
    public string CategoryEmoji { get; init; } = "📰";

    /// <summary>Geschätzte Lesedauer in Minuten (mindestens 1).</summary>
    public int ReadingMinutes { get; init; } = 1;

    public NewsDifficulty Difficulty { get; init; } = NewsDifficulty.Mittel;

    /// <summary>"Warum ist das wichtig?" - kindgerechte Einordnung je Rubrik. Bewusst ehrliche,
    /// rubrikbezogene Standardtexte statt erfundener artikel-spezifischer Behauptungen (eine
    /// regelbasierte Pipeline kann keine echte Einzelfall-Analyse leisten).</summary>
    public string WhyImportant { get; init; } = string.Empty;

    /// <summary>"Was bedeutet das für dich?" - Bezug zur Lebenswelt von Kindern/Jugendlichen.</summary>
    public string MeaningForKids { get; init; } = string.Empty;

    /// <summary>Im Titel/Text erkannte schwierige Wörter mit kindgerechter Erklärung.</summary>
    public IReadOnlyList<ExplainedTerm> ExplainedTerms { get; init; } = Array.Empty<ExplainedTerm>();
}
