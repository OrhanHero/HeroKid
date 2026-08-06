using LernTor.Core.Models;

namespace LernTor.Core.Services;

/// <summary>
/// Die tägliche Verkehrszeichen-Challenge: fünf Zeichen, die das Kind erkennen soll.
///
/// <para><b>Warum fest je Tag und Kind:</b> die Auswahl wird aus Profil-Kennung und Datum
/// abgeleitet, nicht aus dem Zufallsgenerator. Damit sind es nach einem App-Neustart dieselben
/// fünf Zeichen - sonst könnte ein Kind so lange neu starten, bis leichte Zeichen kommen. Und
/// beide Kinder bekommen am selben Tag unterschiedliche, sodass Abschreiben nichts bringt.</para>
///
/// <para><b>Noch nicht Gekonntes zuerst.</b> Zeichen, die schon sitzen, kommen erst dran, wenn
/// nicht genug ungelernte übrig sind - sonst würde die Challenge mit wachsendem Können immer
/// leichter statt gezielter.</para>
/// </summary>
public static class DailySignChallenge
{
    /// <summary>Fünf Zeichen - kurz genug, dass es auch an einem vollen Schultag drin ist.</summary>
    public const int SignsPerDay = 5;

    /// <summary>
    /// Die Zeichen des Tages für ein Profil.
    /// </summary>
    /// <param name="pool">Zur Verfügung stehende Zeichen (z.B. nur die von den Eltern
    /// freigegebenen Kategorien). Ist er kleiner als <see cref="SignsPerDay"/>, kommt eben
    /// weniger zurück - lieber drei echte Zeichen als zwei Wiederholungen zum Auffüllen.</param>
    /// <param name="profileId">Damit Geschwister nicht dieselben fünf bekommen.</param>
    /// <param name="day">Der Tag, für den ausgewählt wird.</param>
    /// <param name="masteredNumbers">Nummern der Zeichen, die das Kind schon sicher kann.</param>
    public static IReadOnlyList<TrafficSign> ForDay(
        IReadOnlyList<TrafficSign> pool,
        string profileId,
        DateOnly day,
        IReadOnlySet<string> masteredNumbers)
    {
        if (pool.Count == 0)
        {
            return Array.Empty<TrafficSign>();
        }

        var random = new Random(SeedFor(profileId, day));

        var offen = Shuffle(pool.Where(sign => !masteredNumbers.Contains(sign.Number)).ToList(), random);
        var gekonnt = Shuffle(pool.Where(sign => masteredNumbers.Contains(sign.Number)).ToList(), random);

        return offen.Concat(gekonnt).Take(SignsPerDay).ToList();
    }

    /// <summary>
    /// Ableitung des Startwerts aus Profil und Tag. Bewusst nicht <c>string.GetHashCode()</c>:
    /// dessen Ergebnis ist in .NET pro Prozessstart zufällig verwürfelt, die Auswahl wäre nach
    /// jedem Neustart eine andere - genau das, was hier nicht passieren darf.
    /// </summary>
    private static int SeedFor(string profileId, DateOnly day)
    {
        var hash = 17;
        foreach (var c in profileId)
        {
            hash = unchecked(hash * 31 + c);
        }

        return unchecked(hash * 31 + day.DayNumber);
    }

    private static List<T> Shuffle<T>(List<T> items, Random random)
    {
        for (var i = items.Count - 1; i > 0; i--)
        {
            var j = random.Next(i + 1);
            (items[i], items[j]) = (items[j], items[i]);
        }

        return items;
    }
}
