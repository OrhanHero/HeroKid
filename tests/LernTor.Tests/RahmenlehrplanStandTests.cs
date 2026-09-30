using LernTor.ContentGen;
using LernTor.ContentGen.Curriculum;
using LernTor.Core.Enums;
using Xunit;

namespace LernTor.Tests;

/// <summary>
/// Rahmenlehrplan-Stand im Eltern-Bericht (docs/NAECHSTES-LEVEL-3-2.md, Schritt 5): der Katalog
/// kennt Themen-Methoden, das Aktivitätsprotokoll nur Themennamen - beides muss zusammenfinden.
/// </summary>
public sealed class RahmenlehrplanStandTests
{
    private static readonly QuizComposer Composer = new();

    [Fact]
    public void Ohne_Antworten_ist_nichts_geuebt_und_die_Zahlen_kommen_aus_dem_Katalog()
    {
        var stand = RahmenlehrplanStand.Berechne(GradeLevel.Klasse6, Composer.Generators, []);

        Assert.NotEmpty(stand);
        Assert.All(stand, s => Assert.Equal(0, s.Geuebt));
        Assert.All(stand, s => Assert.InRange(s.Abgedeckt, 1, s.Themenfelder));
        var biologie = Assert.Single(stand, s => s.Fach == "Biologie");
        Assert.Equal(biologie.Themenfelder, biologie.Abgedeckt);
        Assert.Equal("Biologie 5/6", biologie.Bezeichnung);
        Assert.DoesNotContain(stand, s => s.Fach == "Sport");
    }

    [Fact]
    public void Eine_Antwort_zaehlt_ihr_Themenfeld_als_geuebt_auch_aus_der_Fehler_Kartei()
    {
        var thema = Composer.GenerateExercises(Subject.Physik, GradeLevel.Klasse6, 60, new Random(3))
            .First(q => q.Topic.StartsWith("Die Sonne als Energiequelle", StringComparison.Ordinal)).Topic;

        var stand = RahmenlehrplanStand.Berechne(GradeLevel.Klasse6, Composer.Generators,
            [(Subject.Physik, "🔁 " + thema), (Subject.Biologie, thema)]);

        var nawi = Assert.Single(stand, s => s.Fach == "Naturwissenschaften 5/6");
        Assert.Equal(1, nawi.Geuebt);
        Assert.Equal("Naturwissenschaften 5/6", nawi.Bezeichnung);
        Assert.Equal(0, Assert.Single(stand, s => s.Fach == "Biologie").Geuebt);
    }

    [Fact]
    public void Jedes_zugeordnete_Thema_hat_einen_Namen_im_Protokoll()
    {
        // Wer alle Themen jeder Stufe beantwortet hat, hat jedes abgedeckte Themenfeld geübt.
        foreach (var klasse in new[] { GradeLevel.Klasse6, GradeLevel.Klasse9 })
        {
            var alle = Composer.Generators
                .OfType<LernTor.ContentGen.Generators.ExerciseGeneratorBase>()
                .SelectMany(g => g.TopicNamesByMethod(klasse).Values.SelectMany(n => n).Select(n => (g.Subject, n)))
                .ToList();

            Assert.All(RahmenlehrplanStand.Berechne(klasse, Composer.Generators, alle),
                s => Assert.True(s.Geuebt == s.Abgedeckt, $"{s.Fach} {klasse}: {s.Geuebt} von {s.Abgedeckt}"));
        }
    }

    [Theory]
    [InlineData(GradeLevel.Klasse6, RlpStufe.Stufe5_6)]
    [InlineData(GradeLevel.Klasse7, RlpStufe.Stufe7_8)]
    [InlineData(GradeLevel.Klasse8, RlpStufe.Stufe7_8)]
    [InlineData(GradeLevel.Klasse9, RlpStufe.Stufe9_10)]
    [InlineData(GradeLevel.Klasse10, RlpStufe.Stufe9_10)]
    public void Klasse_8_und_10_gehoeren_zu_ihrem_Doppeljahrgang(GradeLevel klasse, RlpStufe stufe) =>
        Assert.Equal(stufe, RahmenlehrplanStand.StufeFuer(klasse));
}
