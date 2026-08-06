namespace LernTor.Core.Enums;

/// <summary>
/// Die Sachgebiete der Theorieprüfung Klasse B, in der Gliederung der amtlichen Fahrschüler-
/// Ausbildungsordnung. Sie strukturieren den Lernstoff und den Schwachstellen-Trainer: wer in
/// "Vorfahrt" schwächelt, soll Vorfahrt-Fragen bekommen und nicht mehr Umweltfragen.
///
/// <para>Neue Werte gehören ans ENDE - die Auswahl wird als String persistiert, aber die
/// Reihenfolge ist zugleich die Kursreihenfolge und soll stabil bleiben.</para>
/// </summary>
public enum DrivingTheoryTopic
{
    /// <summary>Wer darf überhaupt fahren: Alter, Fahrerlaubnis, Alkohol, Müdigkeit, Medikamente.</summary>
    PersoenlicheVoraussetzungen,

    /// <summary>Führerschein, Zulassung, Versicherung, Halterpflichten, Bußgeld und Punkte.</summary>
    RechtlicheRahmenbedingungen,

    /// <summary>Wie das Straßenverkehrssystem aufgebaut ist und wer es wie benutzen darf.</summary>
    Strassenverkehrssystem,

    /// <summary>Verkehrszeichen und Verkehrseinrichtungen - der Bereich, der auch eigenständig geübt wird.</summary>
    VerkehrszeichenUndEinrichtungen,

    /// <summary>Vorfahrt, Vorrang, Ampeln, Polizeizeichen - die häufigste Fehlerquelle der Prüfung.</summary>
    VorfahrtUndRegelung,

    /// <summary>Geschwindigkeit, Abstand, Bremsweg - was Physik mit Fahren zu tun hat.</summary>
    GeschwindigkeitUndAbstand,

    /// <summary>Überholen, Vorbeifahren, Abbiegen, Wenden, Rückwärtsfahren.</summary>
    FahrmanoeverUndUeberholen,

    /// <summary>Halten, Parken, Haltverbote, Parkscheibe.</summary>
    RuhenderVerkehr,

    /// <summary>Fußgänger, Radfahrende, Kinder, ältere Menschen, LKW - und wie man sie einschätzt.</summary>
    AndereVerkehrsteilnehmer,

    /// <summary>Dunkelheit, Nebel, Regen, Schnee, Tunnel, Bahnübergang, Autobahn.</summary>
    BesondereSituationen,

    /// <summary>Unfall, Absichern, Erste Hilfe, Panne, Abschleppen.</summary>
    UnfallUndPanne,

    /// <summary>Umweltbewusst und sparsam fahren - fester Bestandteil der Prüfung.</summary>
    UmweltUndSparsamkeit,

    /// <summary>Reifen, Bremsen, Licht, Ladung, Sicherheitsgurt, technische Mängel.</summary>
    FahrzeugtechnikUndSicherheit,

    /// <summary>Personen mitnehmen, Ladung sichern, Anhänger ziehen.</summary>
    BefoerderungUndAnhaenger
}
