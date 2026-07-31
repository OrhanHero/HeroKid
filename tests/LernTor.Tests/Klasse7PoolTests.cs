using LernTor.ContentGen.Generators;
using LernTor.Core.Enums;
using Xunit;

namespace LernTor.Tests;

/// <summary>
/// Klasse-7-Pools: jedes Fach soll eigene Themen haben statt auf Klasse 6 zurückzufallen, und
/// die Pools sollen dick genug sein, dass sie sich nicht sofort erschöpfen.
/// </summary>
public sealed class Klasse7PoolTests
{
    /// <summary>Alle Fächer mit eigenem Klasse-7-Pool. KiWissen fehlt bewusst - der KI-Bereich
    /// hat nur Klasse 6 und 9 und fällt für Klasse 7 auf Klasse 6 zurück (siehe KiWissenTests).</summary>
    public static IEnumerable<object[]> GeneratorenMitKlasse7 => new List<object[]>
    {
        new object[] { new MathGenerator() },
        new object[] { new GermanGenerator() },
        new object[] { new TurkishGenerator() },
        new object[] { new EnglischGenerator() },
        new object[] { new BiologieGenerator() },
        new object[] { new ChemieGenerator() },
        new object[] { new PhysikGenerator() },
        new object[] { new GeschichteGenerator() },
        new object[] { new GewiGenerator() },
        new object[] { new PolitikGenerator() },
        new object[] { new GeoGenerator() },
        new object[] { new EthikGenerator() },
        new object[] { new KunstGenerator() },
        new object[] { new MusikGenerator() },
        new object[] { new ItgGenerator() }
    };

    [Theory]
    [MemberData(nameof(GeneratorenMitKlasse7))]
    public void Jedes_Fach_hat_eigene_Klasse7_Aufgaben(IExerciseGenerator generator)
    {
        // Faellt ein Fach auf Klasse 6 zurueck, traegt die Aufgabe auch GradeLevel.Klasse6 -
        // genau daran laesst sich der Rueckfall erkennen.
        var fragen = generator.Generate(GradeLevel.Klasse7, 12, new Random(7));

        Assert.NotEmpty(fragen);
        Assert.All(fragen, f => Assert.Equal(GradeLevel.Klasse7, f.GradeLevel));
    }

    [Theory]
    [MemberData(nameof(GeneratorenMitKlasse7))]
    public void Der_Klasse7_Pool_ist_dick_genug(IExerciseGenerator generator)
    {
        // Korrekt beantwortete Aufgaben pausieren 7/30/90 Tage (Spaced Repetition). Ein duenner
        // Pool waere danach leer - deshalb der Zielumfang aus dem fach-pool-Skill.
        var prompts = generator.Generate(GradeLevel.Klasse7, 40, new Random(11))
            .Select(f => f.Prompt)
            .Distinct()
            .Count();

        Assert.True(prompts >= 30, $"{generator.Subject} liefert nur {prompts} verschiedene Klasse-7-Aufgaben.");
    }

    [Theory]
    [MemberData(nameof(GeneratorenMitKlasse7))]
    public void Klasse7_Aufgaben_sind_vollstaendig_und_loesbar(IExerciseGenerator generator)
    {
        var fragen = generator.Generate(GradeLevel.Klasse7, 30, new Random(3));

        Assert.All(fragen, f =>
        {
            Assert.False(string.IsNullOrWhiteSpace(f.Prompt));
            Assert.NotEmpty(f.CorrectAnswers);
            Assert.False(string.IsNullOrWhiteSpace(f.Explanation));
            Assert.True(f.CheckAnswer(f.CorrectAnswers[0]));
        });
    }

    [Fact]
    public void Ethik_und_Geo_decken_die_neuen_Klasse7_Themen_ab()
    {
        // Beide Faecher hatten mit drei Themen die duennsten Klasse-7-Pools ueberhaupt.
        var ethik = new EthikGenerator().Generate(GradeLevel.Klasse7, 200, new Random(5))
            .Select(f => f.Topic).Distinct().ToList();
        var geo = new GeoGenerator().Generate(GradeLevel.Klasse7, 200, new Random(5))
            .Select(f => f.Topic).Distinct().ToList();

        Assert.Contains("Medien, Wahrheit und Verantwortung", ethik);
        Assert.Contains("Tier- und Umweltethik", ethik);
        Assert.Contains("Konflikt, Gewalt und Zivilcourage", ethik);

        Assert.Contains("Europa: Räume, Grenzen und Vielfalt", geo);
        Assert.Contains("Landwirtschaft und Ernährung", geo);
        Assert.Contains("Naturgefahren und Naturrisiken", geo);
    }

    [Theory]
    [InlineData(GradeLevel.Klasse8, GradeLevel.Klasse7)]
    public void Klasse8_nutzt_den_Klasse7_Pool(GradeLevel angefragt, GradeLevel erwartet)
    {
        // Doppeljahrgangs-Regel: Klasse 8 hat keinen eigenen Pool und uebt den Stoff der 7.
        var fragen = new GeoGenerator().Generate(angefragt, 10, new Random(9));

        Assert.All(fragen, f => Assert.Equal(erwartet, f.GradeLevel));
    }
}
