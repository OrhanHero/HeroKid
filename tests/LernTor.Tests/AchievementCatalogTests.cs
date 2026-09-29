using LernTor.Core.Enums;
using LernTor.Core.Services;
using Xunit;

namespace LernTor.Tests;

public class AchievementCatalogTests
{
    [Fact]
    public void Kennungen_sind_eindeutig_und_Texte_vollstaendig()
    {
        // Die Kennungen werden gespeichert - doppelte oder leere wuerden Abzeichen vermischen.
        var ids = AchievementCatalog.All.Select(a => a.Id).ToList();
        Assert.Equal(ids.Count, ids.Distinct(StringComparer.Ordinal).Count());
        Assert.InRange(ids.Count, 20, 40);

        Assert.All(AchievementCatalog.All, a =>
        {
            Assert.False(string.IsNullOrWhiteSpace(a.Id));
            Assert.False(string.IsNullOrWhiteSpace(a.Emoji));
            Assert.False(string.IsNullOrWhiteSpace(a.TitleDe));
            Assert.False(string.IsNullOrWhiteSpace(a.TitleTr));
            Assert.False(string.IsNullOrWhiteSpace(a.HowToDe));
            Assert.False(string.IsNullOrWhiteSpace(a.HowToTr));
        });
    }

    [Fact]
    public void Keine_Flaggen_Emoji()
    {
        // WPF setzt Regional-Indicator-Paare nicht zur Flagge zusammen (siehe CLAUDE.md).
        Assert.All(AchievementCatalog.All, a =>
            Assert.DoesNotContain(a.Emoji.EnumerateRunes(), r => r.Value is >= 0x1F1E6 and <= 0x1F1FF));
    }

    [Fact]
    public void Ohne_Leistung_gibt_es_kein_Abzeichen()
    {
        Assert.Empty(AchievementCatalog.Earned(new AchievementFacts()));
    }

    [Fact]
    public void Kein_Abzeichen_fuer_Tage_am_Stueck()
    {
        // Grundsatz der App: Leistung statt Anwesenheit, nichts, das an einem verpassten Tag
        // zerbricht. Lerntage zaehlen nur gesamt.
        Assert.DoesNotContain(AchievementCatalog.All, a =>
            a.TitleDe.Contains("Folge", StringComparison.OrdinalIgnoreCase)
            || a.HowToDe.Contains("am Stück", StringComparison.OrdinalIgnoreCase));
    }

    [Theory]
    [InlineData(9, false)]
    [InlineData(10, true)]
    public void Erste_Schritte_ab_zehn_richtigen(int richtig, bool erwartet)
    {
        var erreicht = AchievementCatalog.Earned(new AchievementFacts { CorrectAnswers = richtig });

        Assert.Equal(erwartet, erreicht.Any(a => a.Id == "richtig-10"));
    }

    [Fact]
    public void Dreisprachig_braucht_alle_drei_Sprachen()
    {
        var zweiSprachen = new AchievementFacts
        {
            CorrectBySubject = new Dictionary<Subject, int> { [Subject.Deutsch] = 40, [Subject.Englisch] = 40 }
        };
        var dreiSprachen = zweiSprachen with
        {
            CorrectBySubject = new Dictionary<Subject, int>
            {
                [Subject.Deutsch] = 25, [Subject.Englisch] = 25, [Subject.Tuerkisch] = 25
            }
        };

        Assert.DoesNotContain(AchievementCatalog.Earned(zweiSprachen), a => a.Id == "dreisprachig");
        Assert.Contains(AchievementCatalog.Earned(dreiSprachen), a => a.Id == "dreisprachig");
    }

    [Fact]
    public void Allrounder_zaehlt_News_und_Tippen_nicht_mit()
    {
        var faecher = new Dictionary<Subject, int>
        {
            [Subject.Mathematik] = 20, [Subject.Deutsch] = 20, [Subject.Englisch] = 20,
            [Subject.Tuerkisch] = 20, [Subject.Biologie] = 20, [Subject.Geo] = 20,
            [Subject.Musik] = 20,
            [Subject.News] = 500, [Subject.Tippen] = 500,
        };
        var sieben = new AchievementFacts { CorrectBySubject = faecher };
        var acht = new AchievementFacts
        {
            CorrectBySubject = new Dictionary<Subject, int>(faecher) { [Subject.Kunst] = 20 }
        };

        Assert.DoesNotContain(AchievementCatalog.Earned(sieben), a => a.Id == "allrounder");
        Assert.Contains(AchievementCatalog.Earned(acht), a => a.Id == "allrounder");
    }

    [Fact]
    public void Schilder_Kenner_nur_mit_verfuegbaren_Zeichen()
    {
        // 0 verfuegbar (Bereich aus) darf nicht als "alle 0 von 0 gekonnt" durchgehen.
        Assert.DoesNotContain(AchievementCatalog.Earned(new AchievementFacts()), a => a.Id == "schilder-alle");
        Assert.DoesNotContain(
            AchievementCatalog.Earned(new AchievementFacts { TrafficSignsAvailable = 77, TrafficSignsMastered = 76 }),
            a => a.Id == "schilder-alle");
        Assert.Contains(
            AchievementCatalog.Earned(new AchievementFacts { TrafficSignsAvailable = 77, TrafficSignsMastered = 77 }),
            a => a.Id == "schilder-alle");
    }

    [Fact]
    public void Neu_sind_nur_die_noch_nicht_gespeicherten()
    {
        var fakten = new AchievementFacts { CorrectAnswers = 150 };

        var neu = AchievementCatalog.NewlyEarned(fakten, new HashSet<string> { "richtig-10" });

        Assert.Equal(new[] { "richtig-100" }, neu.Select(a => a.Id).ToArray());
    }

    [Fact]
    public void Fakten_aus_Antworten_zaehlen_Tage_Faecher_und_korrigierte_Fehler()
    {
        var tag1 = new DateTimeOffset(2026, 9, 28, 16, 0, 0, TimeSpan.FromHours(2));
        var tag2 = tag1.AddDays(1);
        var antworten = new[]
        {
            new MasteryAnswer(Subject.Mathematik, "Bruchrechnen", "a", true, tag1),
            new MasteryAnswer(Subject.Mathematik, "Bruchrechnen", "b", false, tag1.AddMinutes(5)),
            new MasteryAnswer(Subject.Mathematik, "🔁 Bruchrechnen", "b", true, tag2),
            new MasteryAnswer(Subject.Englisch, "🔁 Question Words", "c", false, tag2),
        };
        var themen = new[]
        {
            new TopicMasteryStatus(Subject.Mathematik, "Bruchrechnen", 12, 10, 10, 2, MasteryLevel.Gemeistert),
            new TopicMasteryStatus(Subject.Englisch, "Question Words", 6, 6, 5, 0, MasteryLevel.Sicher),
            new TopicMasteryStatus(Subject.Musik, "Stimme", 3, 3, 1, 0, MasteryLevel.Angefangen),
        };

        var fakten = AchievementCatalog.FromAnswers(antworten, themen);

        Assert.Equal(2, fakten.CorrectAnswers);
        Assert.Equal(2, fakten.CorrectIn(Subject.Mathematik));
        Assert.Equal(0, fakten.CorrectIn(Subject.Englisch));
        Assert.Equal(2, fakten.LearningDays);
        Assert.Equal(1, fakten.CorrectedMistakes);
        Assert.Equal(2, fakten.SecureTopics);
        Assert.Equal(1, fakten.MasteredTopics);
    }
}
