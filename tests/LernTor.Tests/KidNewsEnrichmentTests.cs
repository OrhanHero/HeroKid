using LernTor.Core.Enums;
using LernTor.Core.Models;
using LernTor.News;
using Xunit;

namespace LernTor.Tests;

public class NewsCategoryClassifierTests
{
    [Theory]
    [InlineData("Neues Minecraft-Update erschienen", "Viele Spieler freuen sich.", NewsCategory.Deutschland, NewsCategory.Spiele)]
    [InlineData("Nintendo kündigt Konsole an", "", NewsCategory.Welt, NewsCategory.Spiele)]
    [InlineData("ChatGPT bekommt neue Funktionen", "", NewsCategory.Deutschland, NewsCategory.Ki)]
    [InlineData("Inflation sinkt leicht", "Die Preise steigen langsamer.", NewsCategory.Deutschland, NewsCategory.Finanzen)]
    [InlineData("Unwetter über Brandenburg", "Der Wetterdienst warnt.", NewsCategory.Berlin, NewsCategory.Wetter)]
    public void Classify_erkennt_Themenrubriken(string title, string summary, NewsCategory sourceDefault, NewsCategory expected)
    {
        Assert.Equal(expected, NewsCategoryClassifier.Classify(title, summary, sourceDefault));
    }

    [Fact]
    public void Classify_faellt_auf_Quellen_Rubrik_zurueck()
    {
        var category = NewsCategoryClassifier.Classify(
            "Neue Fahrradwege eröffnet", "Der Bezirk freut sich.", NewsCategory.Berlin);

        Assert.Equal(NewsCategory.Berlin, category);
    }

    [Fact]
    public void Spiele_hat_Vorrang_vor_KI()
    {
        // "KI-Gegner in Super Mario" ist eine Spiele-Meldung, keine KI-Meldung.
        var category = NewsCategoryClassifier.Classify(
            "Super Mario bekommt KI-Gegner", "", NewsCategory.Deutschland);

        Assert.Equal(NewsCategory.Spiele, category);
    }

    [Fact]
    public void Jede_Rubrik_hat_ein_Emoji()
    {
        foreach (var category in Enum.GetValues<NewsCategory>())
        {
            Assert.False(string.IsNullOrWhiteSpace(NewsCategoryClassifier.EmojiFor(category)));
        }
    }
}

public class KidTermGlossaryTests
{
    [Fact]
    public void FindTerms_erkennt_Begriffe_in_Reihenfolge_des_Auftretens()
    {
        var terms = KidTermGlossary.FindTerms(
            "Wegen der Inflation diskutiert die Koalition über den Haushaltsplan.");

        Assert.Equal(new[] { "Inflation", "Koalition", "Haushaltsplan" }, terms.Select(t => t.Term));
        Assert.All(terms, t => Assert.False(string.IsNullOrWhiteSpace(t.Explanation)));
    }

    [Fact]
    public void FindTerms_matcht_zusammengesetzte_Woerter_am_Wortanfang()
    {
        var terms = KidTermGlossary.FindTerms("Die Inflationsrate ist gesunken.");

        Assert.Contains(terms, t => t.Term == "Inflation");
    }

    [Fact]
    public void FindTerms_matcht_nicht_mitten_im_Wort()
    {
        // "International" enthält "nation", darf aber keinen Treffer für irgendeinen Begriff
        // liefern, der nur als Teilwort vorkommt.
        var terms = KidTermGlossary.FindTerms("Ein internationales Turnier beginnt.");

        Assert.Empty(terms);
    }

    [Fact]
    public void FindTerms_liefert_hoechstens_vier_Begriffe()
    {
        var terms = KidTermGlossary.FindTerms(
            "Koalition, Opposition, Bundestag, Inflation, Börse und Zinsen in einem Satz.");

        Assert.Equal(4, terms.Count);
    }

    [Fact]
    public void FindTerms_mit_leerem_Text_liefert_leer()
    {
        Assert.Empty(KidTermGlossary.FindTerms(null));
        Assert.Empty(KidTermGlossary.FindTerms("   "));
    }
}

public class HeuristicComprehensionQuestionGeneratorTests
{
    private static NewsArticle Article(string summary) => new()
    {
        Id = "a1",
        Title = "Neue Schwimmhalle in Spandau eröffnet",
        SimplifiedSummary = summary,
        SourceName = "rbb24 Berlin",
        SourceUrl = "https://example.org",
        PublishedAt = DateTimeOffset.Now,
        RegionFocus = NewsRegionFocus.Berlin,
        Category = NewsCategory.Berlin,
        CategoryEmoji = "🐻",
        ComprehensionQuestions = Array.Empty<QuizQuestion>()
    };

    [Fact]
    public void Keine_Ueberschrift_Wort_Frage_und_keine_Rubrik_Frage_mehr()
    {
        var questions = new HeuristicComprehensionQuestionGenerator().GenerateQuestions(
            Article("Die neue Schwimmhalle wurde nach zwei Jahren Bauzeit feierlich eröffnet."));

        Assert.DoesNotContain(questions, q => q.Prompt.Contains("wichtiges Wort aus der Überschrift"));
        Assert.DoesNotContain(questions, q => q.Prompt.Contains("Themenbereich"));
        Assert.All(questions, q => Assert.EndsWith("-lueckentext", q.Id));
    }

    [Fact]
    public void Genau_eine_Lueckentext_Frage_pro_Artikel()
    {
        var questions = new HeuristicComprehensionQuestionGenerator().GenerateQuestions(
            Article("Die neue Schwimmhalle wurde nach zwei Jahren Bauzeit feierlich eröffnet. " +
                    "Hunderte Familien kamen zur Eröffnung und probierten die Rutschen aus."));

        var cloze = Assert.Single(questions);
        Assert.EndsWith("-lueckentext", cloze.Id);
        Assert.Contains("_____", cloze.Prompt);
        Assert.Contains(cloze.CorrectAnswers[0], cloze.Options);
        Assert.True(cloze.Options.Count >= 3);
        Assert.True(cloze.CheckAnswer(cloze.CorrectAnswers[0]));
        // Das gesuchte Wort darf im Lücken-Satz des Prompts nicht mehr sichtbar sein.
        Assert.DoesNotContain(cloze.CorrectAnswers[0], cloze.Prompt);
    }

    [Fact]
    public void Zu_kurze_Zusammenfassung_liefert_keine_Frage()
    {
        var questions = new HeuristicComprehensionQuestionGenerator().GenerateQuestions(Article("Kurz."));

        Assert.Empty(questions);
    }
}

public class KidNewsMetadataTests
{
    [Fact]
    public void ReadingMinutes_ist_mindestens_eins()
    {
        Assert.Equal(1, KidNewsMetadata.ComputeReadingMinutes("Kurzer Titel"));
        Assert.Equal(1, KidNewsMetadata.ComputeReadingMinutes(null, ""));
    }

    [Fact]
    public void ReadingMinutes_waechst_mit_Textlaenge()
    {
        var longText = string.Join(" ", Enumerable.Repeat("Wort", 300));
        Assert.True(KidNewsMetadata.ComputeReadingMinutes(longText) >= 2);
    }

    [Fact]
    public void Difficulty_kurze_Saetze_sind_leicht()
    {
        var difficulty = KidNewsMetadata.ComputeDifficulty(
            "Das ist ein Satz. Er ist kurz. Alle Wörter sind klein.");

        Assert.Equal(NewsDifficulty.Leicht, difficulty);
    }

    [Fact]
    public void Difficulty_lange_Schachtelsaetze_sind_schwerer()
    {
        var difficulty = KidNewsMetadata.ComputeDifficulty(
            "Die Bundesnetzagentur veröffentlichte gemeinsam mit dem Wirtschaftsministerium eine " +
            "umfangreiche Verwaltungsvorschrift zur Beschleunigung energiewirtschaftlicher " +
            "Genehmigungsverfahren einschließlich sämtlicher nachgelagerter Planfeststellungsverfahren " +
            "und Umweltverträglichkeitsprüfungen ohne dabei einen einzigen Punkt zu setzen");

        Assert.NotEqual(NewsDifficulty.Leicht, difficulty);
    }

    [Fact]
    public void Jede_Rubrik_hat_Einordnungstexte()
    {
        foreach (var category in Enum.GetValues<NewsCategory>())
        {
            Assert.False(string.IsNullOrWhiteSpace(KidNewsMetadata.WhyImportantFor(category, GradeLevel.Klasse6)));
            Assert.False(string.IsNullOrWhiteSpace(KidNewsMetadata.MeaningForKidsFor(category, GradeLevel.Klasse6)));
        }
    }
}
