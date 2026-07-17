namespace LernTor.Core.Services;

/// <summary>
/// Adaptive Übungsauswahl: berechnet aus der bisherigen Trefferquote je Thema (Aktivitätsprotokoll)
/// Gewichte für die Aufgabenauswahl - schwache Themen bekommen bis zu <see cref="MaxWeight"/>-fache
/// Ziehwahrscheinlichkeit, sichere Themen bleiben bei 1,0. So üben Kinder automatisch dort mehr,
/// wo es hakt, ohne dass ein Thema je ganz verschwindet (kein Gewicht unter 1,0 - auch starke
/// Themen kommen weiter vor, nur seltener relativ zu schwachen). Themen mit weniger als
/// <see cref="MinAttempts"/> Antworten werden nicht bewertet (zu wenig Datenbasis) und bleiben
/// neutral. Verbraucher: ExerciseGeneratorBase.Generate über MainViewModel.
/// </summary>
public static class AdaptiveTopicWeighting
{
    /// <summary>Mindestzahl beantworteter Aufgaben, bevor ein Thema als "schwach/stark" bewertet wird.</summary>
    public const int MinAttempts = 3;

    /// <summary>Maximales Gewicht (bei 0% Trefferquote) relativ zum Neutralgewicht 1,0.</summary>
    public const double MaxWeight = 3.0;

    /// <summary>
    /// Gewicht je Thema: 1,0 (alles richtig) bis <see cref="MaxWeight"/> (alles falsch), linear
    /// nach Fehlerquote. Themen unterhalb von <see cref="MinAttempts"/> fehlen im Ergebnis
    /// (Aufrufer behandeln fehlende Themen als neutral 1,0).
    /// </summary>
    public static IReadOnlyDictionary<string, double> ComputeWeights(
        IEnumerable<(string Topic, int Attempts, int Correct)> topicStats)
    {
        var weights = new Dictionary<string, double>();

        foreach (var (topic, attempts, correct) in topicStats)
        {
            if (attempts < MinAttempts)
            {
                continue;
            }

            var accuracy = Math.Clamp((double)correct / attempts, 0.0, 1.0);
            weights[topic] = 1.0 + (MaxWeight - 1.0) * (1.0 - accuracy);
        }

        return weights;
    }
}
