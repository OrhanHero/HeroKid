using LernTor.ContentGen.Generators;
using LernTor.Core.Enums;
using LernTor.Core.Services;
using Xunit;

namespace LernTor.Tests;

public sealed class AdaptiveTopicWeightingTests
{
    [Fact]
    public void Schwaches_Thema_bekommt_Maximalgewicht()
    {
        var weights = AdaptiveTopicWeighting.ComputeWeights(new[] { ("Brüche", 10, 0) });

        Assert.Equal(AdaptiveTopicWeighting.MaxWeight, weights["Brüche"]);
    }

    [Fact]
    public void Perfektes_Thema_bleibt_beim_Neutralgewicht()
    {
        var weights = AdaptiveTopicWeighting.ComputeWeights(new[] { ("Brüche", 10, 10) });

        Assert.Equal(1.0, weights["Brüche"]);
    }

    [Fact]
    public void Halbe_Trefferquote_liegt_genau_in_der_Mitte()
    {
        var weights = AdaptiveTopicWeighting.ComputeWeights(new[] { ("Brüche", 10, 5) });

        Assert.Equal((1.0 + AdaptiveTopicWeighting.MaxWeight) / 2.0, weights["Brüche"], precision: 5);
    }

    [Fact]
    public void Themen_mit_zu_wenig_Antworten_werden_nicht_bewertet()
    {
        var weights = AdaptiveTopicWeighting.ComputeWeights(new[]
        {
            ("Brüche", AdaptiveTopicWeighting.MinAttempts - 1, 0),
            ("Prozente", AdaptiveTopicWeighting.MinAttempts, 0)
        });

        Assert.False(weights.ContainsKey("Brüche"));
        Assert.True(weights.ContainsKey("Prozente"));
    }

    [Fact]
    public void Generator_zieht_hochgewichtetes_Thema_deutlich_haeufiger()
    {
        var generator = new MathGenerator();
        var random = new Random(42);
        const string weightedTopic = "Kongruenzabbildungen";
        var weights = new Dictionary<string, double> { [weightedTopic] = AdaptiveTopicWeighting.MaxWeight };

        var weightedCount = 0;
        var draws = 300;
        for (var i = 0; i < draws; i++)
        {
            var question = generator.Generate(GradeLevel.Klasse6, 1, random, topicWeights: weights).Single();
            if (question.Topic == weightedTopic)
            {
                weightedCount++;
            }
        }

        // Ohne Gewichtung läge der Anteil bei 1/Themenzahl; mit 3x-Gewicht muss das Thema klar
        // überrepräsentiert sein. Fester Seed -> deterministisch, kein flakiger Statistiktest.
        var topicCount = 0;
        var seenTopics = new HashSet<string>();
        for (var i = 0; i < 200; i++)
        {
            seenTopics.Add(generator.Generate(GradeLevel.Klasse6, 1, new Random(i), topicWeights: null).Single().Topic);
        }
        topicCount = seenTopics.Count;

        var unweightedExpectation = (double)draws / topicCount;
        Assert.True(weightedCount > 1.5 * unweightedExpectation,
            $"Gewichtetes Thema kam nur {weightedCount}x in {draws} Ziehungen ({topicCount} Themen, neutral wären ~{unweightedExpectation:0}).");
    }
}
