using LernTor.Core.Services;
using Xunit;

namespace LernTor.Tests;

/// <summary>Geschwister-Vergleich: was nebeneinander steht - und in welcher Reihenfolge.</summary>
public sealed class ProfileComparisonTests
{
    private static readonly DateOnly Tag1 = new(2026, 8, 3);

    private static ProfileComparison.Input Kind(
        string name, int stars, params ProfileComparison.Answer[] answers) =>
        new(name.ToLowerInvariant(), name, stars, answers);

    private static ProfileComparison.Answer Antwort(int tagOffset, bool richtig, int dauerMs = 0) =>
        new(Tag1.AddDays(tagOffset), richtig, dauerMs);

    [Fact]
    public void Ein_einzelnes_Kind_ergibt_keinen_Vergleich()
    {
        // Ein "Vergleich" mit sich selbst waere nur eine zweite, schlechtere Darstellung des
        // ohnehin vorhandenen Berichts.
        Assert.Empty(ProfileComparison.Build(new[] { Kind("Emirhan", 5, Antwort(0, true)) }));
        Assert.Empty(ProfileComparison.Build(Array.Empty<ProfileComparison.Input>()));
    }

    [Fact]
    public void Sortiert_wird_nach_Namen_und_nicht_nach_Leistung()
    {
        // Das ist der Kern der Entscheidung: eine nach Trefferquote sortierte Liste ist ein
        // Siegertreppchen, und ein Kind stuende dort dauerhaft unten.
        var schwaecher = Kind("Batuhan", 0, Antwort(0, false), Antwort(0, false));
        var staerker = Kind("Emirhan", 0, Antwort(0, true), Antwort(0, true));

        var reihenfolge = ProfileComparison.Build(new[] { staerker, schwaecher })
            .Select(row => row.Name)
            .ToList();

        Assert.Equal(new[] { "Batuhan", "Emirhan" }, reihenfolge);
    }

    [Fact]
    public void Lerntage_zaehlen_verschiedene_Tage_nicht_Aufgaben()
    {
        var rows = ProfileComparison.Build(new[]
        {
            Kind("Ali", 0, Antwort(0, true), Antwort(0, true), Antwort(0, false), Antwort(2, true)),
            Kind("Zeynep", 0, Antwort(0, true))
        });

        Assert.Equal(2, rows.Single(r => r.Name == "Ali").LearnedDays);
        Assert.Equal(4, rows.Single(r => r.Name == "Ali").Answered);
    }

    [Fact]
    public void Trefferquote_und_Sterne_stehen_nebeneinander()
    {
        var rows = ProfileComparison.Build(new[]
        {
            Kind("Ali", 42, Antwort(0, true), Antwort(0, true), Antwort(0, false), Antwort(0, false)),
            Kind("Zeynep", 7, Antwort(0, true))
        });

        var ali = rows.Single(r => r.Name == "Ali");
        Assert.Equal(0.5, ali.Accuracy, 3);
        Assert.Equal(42, ali.TotalStars);
        Assert.Equal(1.0, rows.Single(r => r.Name == "Zeynep").Accuracy, 3);
    }

    [Fact]
    public void Lernzeit_summiert_nur_gemessene_Antworten()
    {
        // Alt-Zeilen ohne Zeitmessung duerfen die Summe nicht als "0 Sekunden" verwaessern.
        var rows = ProfileComparison.Build(new[]
        {
            Kind("Ali", 0, Antwort(0, true, 30_000), Antwort(0, true, 0), Antwort(0, false, 15_000)),
            Kind("Zeynep", 0, Antwort(0, true, 5_000))
        });

        Assert.Equal(TimeSpan.FromSeconds(45), rows.Single(r => r.Name == "Ali").LearningTime);
    }

    [Fact]
    public void Kind_ohne_Aufgaben_bleibt_in_der_Liste_stehen()
    {
        // Wer im Zeitraum nichts gemacht hat, ist genau das Kind, ueber das man reden muss -
        // die Zeile darf nicht verschwinden.
        var rows = ProfileComparison.Build(new[]
        {
            Kind("Ali", 0, Antwort(0, true)),
            Kind("Zeynep", 3)
        });

        var zeynep = rows.Single(r => r.Name == "Zeynep");
        Assert.Equal(2, rows.Count);
        Assert.False(zeynep.HasData);
        Assert.Equal(0, zeynep.LearnedDays);
        Assert.Equal(0, zeynep.Accuracy);
        Assert.Equal(3, zeynep.TotalStars);
    }

    [Fact]
    public void Gleiche_Namen_bleiben_stabil_sortiert()
    {
        // Zwei Kinder mit demselben Rufnamen sind selten, aber eine wechselnde Reihenfolge bei
        // jedem Oeffnen des Berichts waere unbrauchbar.
        var erst = new ProfileComparison.Input("a-id", "Ali", 0, Array.Empty<ProfileComparison.Answer>());
        var zweit = new ProfileComparison.Input("b-id", "Ali", 0, Array.Empty<ProfileComparison.Answer>());

        Assert.Equal(
            new[] { "a-id", "b-id" },
            ProfileComparison.Build(new[] { zweit, erst }).Select(row => row.ProfileId).ToArray());
    }
}
