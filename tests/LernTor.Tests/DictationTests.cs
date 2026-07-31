using LernTor.ContentGen.Generators;
using LernTor.Core.Enums;
using LernTor.Core.Models;
using LernTor.Core.Services;
using Xunit;

namespace LernTor.Tests;

/// <summary>Diktat: was als richtig gilt und was das Kind als Rückmeldung sieht.</summary>
public sealed class DictationTests
{
    private const string Satz = "Der Hund bellt laut im Garten.";

    [Fact]
    public void Der_richtige_Satz_ist_richtig()
    {
        Assert.True(DictationEvaluator.Evaluate(Satz, Satz).IsPerfect);
    }

    [Theory]
    [InlineData("Der Hund bellt laut im Garten")]      // Punkt fehlt
    [InlineData("Der Hund bellt laut im Garten!")]     // anderes Satzzeichen
    [InlineData("Der Hund, bellt laut im Garten.")]    // Komma zu viel
    [InlineData("  Der Hund   bellt laut im Garten. ")] // Leerraum
    public void Satzzeichen_und_Leerraum_entscheiden_nicht(string getippt)
    {
        // Die Sprachausgabe spricht kein Komma. Einem Kind vorzuwerfen, es habe etwas nicht
        // geschrieben, das es gar nicht hoeren konnte, waere unfair.
        Assert.True(DictationEvaluator.Evaluate(Satz, getippt).IsPerfect);
    }

    [Fact]
    public void Gross_und_Kleinschreibung_entscheidet_sehr_wohl()
    {
        // Sie ist im Deutschen regelbasiert und damit hoerbar ableitbar - genau darum geht es
        // bei einem Diktat. Ohne diese Pruefung bliebe von der Rechtschreibung wenig uebrig.
        var ergebnis = DictationEvaluator.Evaluate(Satz, "Der hund bellt laut im Garten.");

        Assert.False(ergebnis.IsPerfect);
        Assert.Contains(ergebnis.Words, w => w.Expected == "Hund" && w.Given == "hund" && !w.IsCorrect);
    }

    [Fact]
    public void Ein_Rechtschreibfehler_macht_das_Diktat_falsch()
    {
        var ergebnis = DictationEvaluator.Evaluate(Satz, "Der Hund belt laut im Garten.");

        Assert.False(ergebnis.IsPerfect);
        Assert.Equal(5, ergebnis.CorrectCount);
        Assert.Equal(6, ergebnis.WordCount);
    }

    [Fact]
    public void Fehlende_und_ueberzaehlige_Woerter_werden_benannt()
    {
        var fehlt = DictationEvaluator.Evaluate(Satz, "Der Hund bellt im Garten.");
        Assert.False(fehlt.IsPerfect);

        var zuViel = DictationEvaluator.Evaluate(Satz, "Der Hund bellt laut im Garten heute.");
        Assert.False(zuViel.IsPerfect);
        Assert.Contains(zuViel.Words, w => w.Expected.Length == 0 && w.Given == "heute");
    }

    [Fact]
    public void Leere_Eingabe_ist_nie_richtig()
    {
        Assert.False(DictationEvaluator.Evaluate(Satz, string.Empty).IsPerfect);
        Assert.False(DictationEvaluator.Evaluate(Satz, "   ").IsPerfect);
        Assert.False(DictationEvaluator.Evaluate(string.Empty, string.Empty).IsPerfect);
    }

    [Fact]
    public void Die_Rueckmeldung_nennt_das_falsche_Wort_und_das_richtige()
    {
        // "Leider falsch" hilft nach einem Zehn-Wort-Satz niemandem weiter.
        var text = DictationEvaluator.Feedback(
            DictationEvaluator.Evaluate(Satz, "Der Hund belt laut im Garten."));

        Assert.Contains("belt", text);
        Assert.Contains("bellt", text);
    }

    [Fact]
    public void Fehlerfrei_wird_auch_so_gemeldet()
    {
        Assert.Contains("Fehlerfrei", DictationEvaluator.Feedback(DictationEvaluator.Evaluate(Satz, Satz)));
    }

    [Fact]
    public void Sehr_viele_Fehler_werden_gekuerzt_gemeldet()
    {
        // Eine Rueckmeldung mit zwanzig Eintraegen liest niemand.
        var text = DictationEvaluator.Feedback(
            DictationEvaluator.Evaluate(
                "eins zwei drei vier fuenf sechs sieben acht neun zehn",
                "a b c d e f g h i j"));

        Assert.Contains("weitere", text);
    }

    [Fact]
    public void QuizQuestion_wertet_ein_Diktat_ueber_den_ganzen_Satz_aus()
    {
        // Bei OpenText genuegt "enthaelt" - fuer ein Diktat waere das falsch: dann kaeme jeder
        // Text durch, in dem der Satz irgendwo vorkommt.
        var frage = new QuizQuestion
        {
            Id = "d1",
            Subject = Subject.Deutsch,
            GradeLevel = GradeLevel.Klasse6,
            Topic = "Diktat",
            Type = QuestionType.Diktat,
            Prompt = Satz,
            CorrectAnswers = new[] { Satz },
            Explanation = "Regel"
        };

        Assert.True(frage.CheckAnswer(Satz));
        Assert.False(frage.CheckAnswer("Der hund bellt laut im Garten."));
        Assert.False(frage.CheckAnswer($"Ich schreibe: {Satz} So war es."));
    }

    [Theory]
    [InlineData(GradeLevel.Klasse6)]
    [InlineData(GradeLevel.Klasse7)]
    [InlineData(GradeLevel.Klasse9)]
    public void Jede_Stufe_hat_genug_Diktatsaetze(GradeLevel stufe)
    {
        var saetze = DictationContentProvider.ForGrade(stufe);

        // Wie bei den uebrigen Pools: rund 20, damit ein Profil einen Satz nicht sofort
        // ausgeschoepft hat, sobald richtig beantwortete Aufgaben pausieren.
        Assert.True(saetze.Count >= 20, $"{stufe} hat nur {saetze.Count} Saetze");
        Assert.All(saetze, satz =>
        {
            Assert.False(string.IsNullOrWhiteSpace(satz.Sentence));
            Assert.False(string.IsNullOrWhiteSpace(satz.Rule));
            Assert.False(string.IsNullOrWhiteSpace(satz.Hint));
        });
    }

    [Fact]
    public void Die_Saetze_bleiben_kurz_genug_zum_Merken()
    {
        // Ein langer Text waere am Bildschirm zermuerbend - und ein Fehler in Wort 40 entwertete
        // die ganze Arbeit.
        foreach (var stufe in new[] { GradeLevel.Klasse6, GradeLevel.Klasse9 })
        {
            Assert.All(DictationContentProvider.ForGrade(stufe), satz =>
            {
                var woerter = DictationEvaluator.SplitWords(satz.Sentence).Count;
                Assert.InRange(woerter, 5, 13);
            });
        }
    }

    [Fact]
    public void Die_Saetze_sind_untereinander_verschieden()
    {
        foreach (var stufe in new[] { GradeLevel.Klasse6, GradeLevel.Klasse9 })
        {
            var saetze = DictationContentProvider.ForGrade(stufe).Select(s => s.Sentence).ToList();
            Assert.Equal(saetze.Count, saetze.Distinct().Count());
        }
    }

    [Fact]
    public void Deutsch_liefert_Diktate_als_eigenen_Aufgabentyp()
    {
        var generator = new GermanGenerator();

        foreach (var stufe in new[] { GradeLevel.Klasse6, GradeLevel.Klasse9 })
        {
            // Viele Aufgaben ziehen, damit das Diktat-Topic mit hoher Sicherheit vorkommt.
            var fragen = generator.Generate(stufe, 120, new Random(7));
            var diktate = fragen.Where(f => f.Type == QuestionType.Diktat).ToList();

            Assert.NotEmpty(diktate);
            Assert.All(diktate, d =>
            {
                Assert.Equal("Diktat", d.Topic);
                Assert.Empty(d.Options);
                // Der Satz steht im Prompt (fuer Vorlesen, Doppelten-Erkennung und Fehler-Kartei)
                // UND in den richtigen Antworten - beides muss dasselbe sein.
                Assert.Equal(d.Prompt, d.CorrectAnswers.Single());
            });
        }
    }
}
