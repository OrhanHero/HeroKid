using LernTor.ContentGen.Generators;
using LernTor.Core.Enums;
using Xunit;

namespace LernTor.Tests;

/// <summary>
/// Der Erste-Hilfe-Bereich. Anders als bei den Schulfächern gilt hier: ein Fragenpool für alle
/// Klassenstufen - die fünf W-Fragen sind mit elf dieselben wie mit fünfzehn.
/// </summary>
public sealed class ErsteHilfeGeneratorTests
{
    private static readonly ErsteHilfeGenerator Generator = new();

    [Theory]
    [InlineData(GradeLevel.Klasse6)]
    [InlineData(GradeLevel.Klasse7)]
    [InlineData(GradeLevel.Klasse8)]
    [InlineData(GradeLevel.Klasse9)]
    [InlineData(GradeLevel.Klasse10)]
    public void Jede_Klassenstufe_bekommt_Fragen(GradeLevel grade)
    {
        // Klasse 8 und 10 haben keinen eigenen Eintrag und laufen über die Übergangsregel -
        // auch sie müssen etwas bekommen, sonst würde die Etappe kommentarlos übersprungen.
        var fragen = Generator.Generate(grade, 6, new Random(1));

        Assert.Equal(6, fragen.Count);
        Assert.All(fragen, frage => Assert.Equal(Subject.ErsteHilfe, frage.Subject));
    }

    [Fact]
    public void Alle_Klassenstufen_ziehen_aus_demselben_Pool()
    {
        // Der Kern der Entscheidung: kein Klassenunterschied. Bewusst mit VERSCHIEDENEN
        // Startwerten gezogen - mit demselben Startwert waeren beide Folgen ohnehin identisch,
        // und der Test wuerde nur sich selbst bestaetigen statt den gemeinsamen Pool.
        var sechs = Ziehen(GradeLevel.Klasse6, 3000, seed: 1);
        var neun = Ziehen(GradeLevel.Klasse9, 3000, seed: 2);

        Assert.Equal(80, sechs.Count);
        Assert.Equal(sechs, neun);
    }

    [Fact]
    public void Der_Pool_umfasst_alle_acht_Themen()
    {
        var themen = Ziehen(GradeLevel.Klasse7, 600, frage => frage.Topic);

        Assert.Equal(8, themen.Count);
    }

    [Fact]
    public void Der_Pool_ist_gross_genug_gegen_Poolermuedung()
    {
        // Bei sechs Aufgaben am Tag (StudentProfile.DefaultExercisesPerSubject) reicht ein
        // 40-Fragen-Pool keine zwei Wochen, bevor "richtig beantwortet kommt nicht wieder"
        // ihn leerräumt. Deshalb sind es 80.
        var prompts = Ziehen(GradeLevel.Klasse7, 3000);

        Assert.True(prompts.Count >= 80, $"Nur {prompts.Count} verschiedene Fragen im Pool.");
    }

    [Fact]
    public void Jede_Frage_hat_Antwortmoeglichkeiten_Erklaerung_und_Tipp()
    {
        var fragen = Generator.Generate(GradeLevel.Klasse7, 60, new Random(7));

        foreach (var frage in fragen)
        {
            Assert.True(frage.Options.Count >= 3, frage.Prompt);
            Assert.Single(frage.CorrectAnswers);
            Assert.Contains(frage.CorrectAnswers[0], frage.Options);
            Assert.False(string.IsNullOrWhiteSpace(frage.Explanation), frage.Prompt);
            Assert.False(string.IsNullOrWhiteSpace(frage.HelpHint), frage.Prompt);
            Assert.False(string.IsNullOrWhiteSpace(frage.Topic), frage.Prompt);
        }
    }

    [Fact]
    public void Die_lebenswichtigen_Zahlen_stehen_im_Pool()
    {
        // Diese vier sind der Kern des Bereichs. Wenn eine davon beim Umformulieren verloren
        // geht, merkt es sonst niemand - der Bereich sieht weiter vollständig aus.
        var text = string.Join(" ",
            Ziehen(GradeLevel.Klasse7, 3000)
                .Concat(Ziehen(GradeLevel.Klasse7, 3000, frage => frage.CorrectAnswers[0])));

        Assert.Contains("112", text);
        Assert.Contains("5 bis 6 Zentimeter", text);
        Assert.Contains("100 bis 120", text);
        Assert.Contains("30 Mal drücken", text);
    }

    [Fact]
    public void Die_Grenzen_der_Schocklage_kommen_vor()
    {
        // Halbgelerntes Erste-Hilfe-Wissen ist genau hier gefährlich: "Beine immer hoch" ist
        // bei Kopf-, Brust- und Atemproblemen falsch.
        var text = string.Join(" ", Ziehen(GradeLevel.Klasse7, 3000));

        Assert.Contains("Schocklage NICHT", text);
    }

    [Fact]
    public void Es_gibt_eine_Frage_zum_Sichnichttrauen()
    {
        // Für ein Kind die wichtigste Frage überhaupt - nichts zu tun ist der einzige echte Fehler.
        var text = string.Join(" ", Ziehen(GradeLevel.Klasse7, 3000));

        Assert.Contains("traust dich nicht", text);
    }

    private static HashSet<string> Ziehen(
        GradeLevel grade, int count,
        Func<LernTor.Core.Models.QuizQuestion, string>? auswahl = null, int seed = 42)
    {
        auswahl ??= frage => frage.Prompt;

        var zufall = new Random(seed);
        var ergebnis = new HashSet<string>(StringComparer.Ordinal);

        // In Bloecken ziehen: Generate() vermeidet Wiederholungen innerhalb eines Aufrufs, gibt
        // bei zu grossem count aber irgendwann doch Doppelte aus - viele kleine Ziehungen
        // decken den Pool zuverlaessiger ab.
        for (var i = 0; i < count / 10; i++)
        {
            foreach (var frage in Generator.Generate(grade, 10, zufall))
            {
                ergebnis.Add(auswahl(frage));
            }
        }

        return ergebnis;
    }
}
