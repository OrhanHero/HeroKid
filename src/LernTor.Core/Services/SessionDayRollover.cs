namespace LernTor.Core.Services;

/// <summary>
/// Wann eine laufende Lernsitzung zu einem neuen Tag gehört.
///
/// <para><b>Der Fall:</b> Der Tagesfortschritt hängt am Datum, an dem die Sitzung begonnen hat
/// (<c>StudentProgress.SessionDate</c>). Ein Kind, das um 23:58 anfängt, bleibt damit bis zum
/// Schluss in "seinem" Tag - gewollt: eine Sitzung, die um Mitternacht still von vorn anfinge,
/// wäre ungerecht. Bleibt der PC aber über Nacht gesperrt stehen, beendete das Kind morgens den
/// Rest von GESTERN, die App entsperrte und beendete sich - und der heutige Lerntag fiel aus.</para>
///
/// <para><b>Die Regel:</b> Über Mitternacht hinaus gilt die Sitzung noch bis
/// <see cref="GraceUntil"/> (4 Uhr morgens) als laufend. Danach, oder wenn mehr als ein Tag
/// dazwischenliegt, beginnt ein neuer Tag. Rein rechnend, damit prüfbar.</para>
/// </summary>
public static class SessionDayRollover
{
    /// <summary>Bis zu dieser Uhrzeit gehört eine Sitzung vom Vortag noch zum Vortag.</summary>
    public static TimeOnly GraceUntil { get; } = new(4, 0);

    /// <summary>Soll die Sitzung, die an <paramref name="sessionDate"/> begann, jetzt als neuer
    /// Tag von vorn beginnen?</summary>
    public static bool ShouldStartNewDay(DateOnly sessionDate, DateTime now)
    {
        var heute = DateOnly.FromDateTime(now);
        var tageDazwischen = heute.DayNumber - sessionDate.DayNumber;

        return tageDazwischen switch
        {
            <= 0 => false,
            1 => TimeOnly.FromDateTime(now) >= GraceUntil,
            _ => true
        };
    }
}
