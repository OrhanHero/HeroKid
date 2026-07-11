using LernTor.Core.Enums;
using LernTor.Core.Models;

namespace LernTor.News;

/// <summary>
/// Kuratierte "Finanzwissen"-Artikel: kindgerechte Dauerbrenner-Erklärstücke (Was ist Geld?
/// Warum steigen Preise? Taschengeld, Sparen, Börse, Berufswelt, ...) im Stil von
/// logo!/Checker-Sendungen - kurze Sätze, alles erklärt, keine Wertung. Zu Finanzthemen gibt es
/// selten kindtaugliche Tagesmeldungen in den RSS-Feeds; damit die Finanzen-Rubrik trotzdem
/// täglich lebt, hängt <see cref="RssNewsService"/> jeden Tag EIN rotierendes Stück aus diesem
/// Pool an die Tagesartikel an. Verständnisfragen sind handgeschrieben (die Heuristik-Fragen
/// aus RSS-Texten wären hier schwächer als selbst formulierte).
/// </summary>
public static class FinanceKnowledgeArticles
{
    /// <summary>Ein Stück pro Tag, rotierend über den Pool (deterministisch je Datum).</summary>
    public static NewsArticle GetForDate(DateOnly date)
    {
        var piece = Pool[date.DayOfYear % Pool.Count];

        // Zusatz-Metadaten zentral berechnen (gleiche Pipeline wie echte RSS-Artikel).
        return piece with
        {
            PublishedAt = date.ToDateTime(TimeOnly.MinValue),
            ReadingMinutes = KidNewsMetadata.ComputeReadingMinutes(piece.Title, piece.SimplifiedSummary),
            Difficulty = KidNewsMetadata.ComputeDifficulty(piece.SimplifiedSummary),
            WhyImportant = KidNewsMetadata.WhyImportantFor(NewsCategory.Finanzen),
            MeaningForKids = KidNewsMetadata.MeaningForKidsFor(NewsCategory.Finanzen),
            ExplainedTerms = KidTermGlossary.FindTerms($"{piece.Title} {piece.SimplifiedSummary}")
        };
    }

    private static NewsArticle Explainer(string idSuffix, string title, string text) => new()
    {
        Id = $"lerntor-finanzwissen-{idSuffix}",
        Title = title,
        SimplifiedSummary = text,
        SourceName = "LernTor Finanzwissen",
        SourceUrl = "https://github.com/OrhanHero/HeroKid",
        PublishedAt = DateTimeOffset.Now,
        RegionFocus = NewsRegionFocus.Deutschland,
        Category = NewsCategory.Finanzen,
        CategoryEmoji = "💰",
        ComprehensionQuestions = Array.Empty<QuizQuestion>()
    };

    private static QuizQuestion Choice(string id, string prompt, string[] options, string correct, string explanation) => new()
    {
        Id = $"lerntor-finanzwissen-frage-{id}",
        Subject = Subject.News,
        GradeLevel = GradeLevel.Klasse6,
        Topic = "Finanzwissen",
        Prompt = prompt,
        Type = QuestionType.MultipleChoice,
        Options = options,
        CorrectAnswers = new[] { correct },
        Explanation = explanation
    };

    /// <summary>Der komplette Erklärstück-Pool - public, damit die Tests jedes Stück auf
    /// Vollständigkeit und beantwortbare Fragen prüfen können (Tests sind eine eigene Assembly).</summary>
    public static readonly IReadOnlyList<NewsArticle> Pool = new[]
    {
        Explainer("geld", "Was ist Geld eigentlich?",
            "Geld ist ein Tauschmittel. Früher haben Menschen Waren direkt getauscht: Getreide gegen " +
            "Werkzeug. Das war umständlich. Deshalb haben sich alle auf Geld geeinigt - erst Münzen, " +
            "dann Scheine, heute oft nur Zahlen auf einem Konto. Geld funktioniert nur, weil alle " +
            "daran glauben, dass man dafür etwas bekommt. Der Schein selbst ist nur Papier - " +
            "wertvoll macht ihn das Vertrauen aller.")
        with { ComprehensionQuestions = new[]
        {
            Choice("geld", "Warum funktioniert Geld als Tauschmittel?",
                new[] { "Weil alle darauf vertrauen, dass man dafür etwas bekommt", "Weil Geldscheine aus wertvollem Papier bestehen", "Weil Münzen aus purem Gold sind" },
                "Weil alle darauf vertrauen, dass man dafür etwas bekommt",
                "Der Schein selbst ist fast wertlos - das gemeinsame Vertrauen macht Geld wertvoll.")
        }},

        Explainer("inflation", "Warum steigen Preise? Inflation einfach erklärt",
            "Inflation bedeutet: Dinge werden nach und nach teurer. Ein Eis, das letztes Jahr 1,50 " +
            "Euro gekostet hat, kostet vielleicht jetzt 1,70 Euro. Für das gleiche Geld bekommt man " +
            "also weniger. Eine kleine Inflation ist normal. Wird sie zu groß, achten die " +
            "Zentralbanken darauf, sie wieder zu bremsen - zum Beispiel über die Zinsen. Für dich " +
            "heißt das: Preise vergleichen lohnt sich!")
        with { ComprehensionQuestions = new[]
        {
            Choice("inflation", "Was bedeutet Inflation?",
                new[] { "Dinge werden nach und nach teurer", "Alle Preise sinken", "Geld wird abgeschafft" },
                "Dinge werden nach und nach teurer",
                "Bei Inflation steigen die Preise - für das gleiche Geld bekommt man weniger.")
        }},

        Explainer("sparen", "Sparen: Wie aus wenig Geld mehr wird",
            "Sparen heißt: Geld nicht sofort ausgeben, sondern für später zurücklegen. Das hilft bei " +
            "größeren Wünschen - ein neues Fahrrad kauft man selten vom Taschengeld einer Woche. " +
            "Ein Trick: Teile dein Geld in drei Teile - einen zum Ausgeben, einen zum Sparen, einen " +
            "für spontane Ideen. Auf einem Sparkonto gibt es manchmal Zinsen dazu: Die Bank zahlt " +
            "dich dafür, dass dein Geld dort liegt.")
        with { ComprehensionQuestions = new[]
        {
            Choice("sparen", "Was sind Zinsen auf dem Sparkonto?",
                new[] { "Geld, das die Bank dir dazugibt", "Eine Gebühr fürs Geldabheben", "Eine Steuer für Kinder" },
                "Geld, das die Bank dir dazugibt",
                "Zinsen sind eine kleine Belohnung der Bank dafür, dass dein Geld dort liegt.")
        }},

        Explainer("boerse", "Was ist die Börse?",
            "Die Börse ist ein Marktplatz - aber statt Obst und Gemüse werden dort Anteile von " +
            "Firmen gehandelt. Diese Anteile heißen Aktien. Wer eine Aktie kauft, dem gehört ein " +
            "winziges Stück der Firma. Läuft die Firma gut, wird die Aktie mehr wert. Läuft sie " +
            "schlecht, verliert die Aktie an Wert. Deshalb gilt: An der Börse kann man gewinnen, " +
            "aber auch verlieren - sie ist kein Sparschwein.")
        with { ComprehensionQuestions = new[]
        {
            Choice("boerse", "Was kauft man mit einer Aktie?",
                new[] { "Einen kleinen Anteil an einer Firma", "Einen Gutschein fürs Einkaufen", "Ein Sparkonto mit festen Zinsen" },
                "Einen kleinen Anteil an einer Firma",
                "Eine Aktie ist ein Mini-Anteil an einer Firma - ihr Wert steigt und fällt mit der Firma.")
        }},

        Explainer("taschengeld", "Taschengeld: Dein Trainingsgeld",
            "Taschengeld ist wie ein Trainingsplatz für den Umgang mit Geld. Du entscheidest selbst: " +
            "sofort ausgeben oder sparen? Wer sein Taschengeld einteilt, lernt etwas, das auch " +
            "Erwachsenen hilft: den Überblick behalten. Ein guter Start: Schreib eine Woche lang auf, " +
            "wofür du Geld ausgibst. Viele wundern sich, wohin die kleinen Beträge verschwinden - " +
            "genau das nennt man ein Haushaltsbuch.")
        with { ComprehensionQuestions = new[]
        {
            Choice("taschengeld", "Wozu hilft ein Haushaltsbuch?",
                new[] { "Den Überblick zu behalten, wohin das Geld geht", "Mehr Taschengeld zu bekommen", "Schulden zu machen" },
                "Den Überblick zu behalten, wohin das Geld geht",
                "Wer aufschreibt, wofür er Geld ausgibt, erkennt schnell, wo es versickert.")
        }},

        Explainer("berufe", "Berufswelt: Wie verdient man Geld?",
            "Die meisten Erwachsenen verdienen Geld mit einem Beruf: Sie arbeiten und bekommen dafür " +
            "einen Lohn oder ein Gehalt. Wie viel, hängt von Ausbildung, Beruf und Verhandlung ab. " +
            "In Deutschland gibt es einen Mindestlohn - weniger pro Stunde darf niemandem gezahlt " +
            "werden. Für dich lohnt sich Neugier: Je mehr Berufe du kennst, desto leichter findest " +
            "du später einen, der zu dir passt - vom Handwerk über Medizin bis zur Spieleentwicklung.")
        with { ComprehensionQuestions = new[]
        {
            Choice("berufe", "Was ist der Mindestlohn?",
                new[] { "Der niedrigste erlaubte Stundenlohn", "Das Gehalt von Chefinnen und Chefs", "Ein Bonus zu Weihnachten" },
                "Der niedrigste erlaubte Stundenlohn",
                "Der Mindestlohn legt fest, wie viel pro Stunde mindestens gezahlt werden muss.")
        }},
    };
}
