using LernTor.ContentGen.Generators;
using LernTor.Core.Enums;
using Xunit;

namespace LernTor.Tests;

/// <summary>
/// Hält die Mindestgröße der Pools fest, die bei der Übung nach Stundenplan zuerst leer liefen
/// (gerechnet mit scripts/pool-reichweite.py). Englisch Klasse 6 reichte knapp acht Wochen,
/// Musik Klasse 6 gut acht, Englisch Klasse 9 genau zehn - danach kamen nur noch Wiederholungen.
/// Die Erweiterung vom 29.09.2026 darf nicht still wieder schrumpfen, etwa durch doppelte
/// Fragetexte, die Generate() als "schon gesehen" überspringt.
/// </summary>
public class PoolReichweiteTests
{
    public static TheoryData<string, GradeLevel, int> Pools => new()
    {
        { "Englisch", GradeLevel.Klasse6, 195 },
        { "Englisch", GradeLevel.Klasse9, 215 },
        { "Musik", GradeLevel.Klasse6, 135 },
        // Klasse-7-Ausbau vom 30.09.2026 (Emirhan wechselt im Sommer 2027 in Klasse 7).
        { "Englisch", GradeLevel.Klasse7, 195 },
        { "Musik", GradeLevel.Klasse7, 135 },
        { "Deutsch", GradeLevel.Klasse7, 175 },
        { "Gewi", GradeLevel.Klasse7, 175 },
        { "KiWissen", GradeLevel.Klasse7, 78 },
        // Lehrplan-Lücken, Version 3.1 (docs/NAECHSTES-LEVEL-3-1.md, Schritt 4).
        { "Biologie", GradeLevel.Klasse6, 155 },
        { "Kunst", GradeLevel.Klasse7, 115 },
        { "Politik", GradeLevel.Klasse7, 115 },
    };

    [Theory]
    [MemberData(nameof(Pools))]
    public void Pool_hat_genug_verschiedene_Fragen(string fach, GradeLevel stufe, int mindestens)
    {
        ExerciseGeneratorBase generator = fach switch
        {
            "Englisch" => new EnglischGenerator(),
            "Musik" => new MusikGenerator(),
            "Deutsch" => new GermanGenerator(),
            "Gewi" => new GewiGenerator(),
            "KiWissen" => new KiWissenGenerator(),
            "Biologie" => new BiologieGenerator(),
            "Kunst" => new KunstGenerator(),
            "Politik" => new PolitikGenerator(),
            _ => throw new ArgumentOutOfRangeException(nameof(fach))
        };

        var gesehen = new HashSet<string>();
        var zufall = new Random(29);

        for (var runde = 0; runde < 60; runde++)
        {
            foreach (var frage in generator.Generate(stufe, 30, zufall, gesehen))
            {
                Assert.Equal(stufe, frage.GradeLevel);
                gesehen.Add(frage.Prompt);
            }
        }

        Assert.True(gesehen.Count >= mindestens,
            $"{fach} {stufe}: nur {gesehen.Count} verschiedene Fragen, erwartet mindestens {mindestens}.");
    }
}
