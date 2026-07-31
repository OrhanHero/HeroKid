namespace LernTor.Core.Enums;

/// <summary>
/// Wie streng der Jugendschutz-Filter im News-Bereich arbeitet - pro Kind einstellbar
/// (siehe <c>StudentProfile.NewsFilterStrictness</c>, ausgewertet in <c>NewsSuitability</c>).
///
/// <para>Vorher hing die Strenge allein am hinterlegten Alter: unter zehn Jahren wurde alles
/// Heikle gesperrt, darüber gar nichts. Das ist eine Entscheidung, die Eltern treffen sollten
/// und nicht ein Geburtsdatum - zwei gleichaltrige Kinder vertragen sehr Unterschiedliches, und
/// der ältere Bruder braucht nicht automatisch die lockere Stufe.</para>
///
/// <para>Die harte Sperre (sexualisierte Gewalt, Suizid, Folter, Missbrauch, Massaker) gilt in
/// JEDER Stufe. Einstellbar ist nur der Umgang mit heiklen, aber lehrplanrelevanten Themen wie
/// Krieg, Kriminalität und Unfällen.</para>
/// </summary>
public enum NewsFilterStrictness
{
    /// <summary>Heikle Themen sind ganz gesperrt - es kommen nur unbedenkliche Nachrichten.
    /// An manchen Tagen bedeutet das spürbar weniger Artikel.</summary>
    Streng,

    /// <summary>Heikle Themen sind erlaubt, werden aber nachrangig behandelt: aus einer Quelle
    /// gewinnt der unbedenklichste Artikel. Die Voreinstellung.</summary>
    Normal,

    /// <summary>Nur die harte Sperre greift; ansonsten zählt allein die Aktualität. Für ältere
    /// Jugendliche, die Nachrichten bewusst mitverfolgen sollen.</summary>
    Locker
}
