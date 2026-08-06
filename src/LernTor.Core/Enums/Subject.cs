namespace LernTor.Core.Enums;

public enum Subject
{
    News,
    Mathematik,
    Deutsch,
    Tuerkisch,
    Englisch,
    Biologie,
    Chemie,
    Physik,
    Geschichte,
    Gewi,
    Politik,
    Geo,
    Ethik,
    Kunst,
    Musik,
    Itg,
    Tippen,
    /// <summary>KI-Bereich: KI verstehen und sicher nutzen (kein Rahmenlehrplan-Fach, bewusst
    /// als eigener Modulbereich - am Ende angefügt, Persistenz erfolgt ohnehin als String).</summary>
    KiWissen,
    /// <summary>Führerschein Klasse B: Verkehrszeichen, Theoriefragen und Theorie-Kurs. Wie
    /// Tippen ein eigener Bereich ohne Aufgaben-Generator - der Inhalt kommt aus
    /// TrafficSignCatalog, nicht aus ExerciseGeneratorBase. Am Ende angefügt.</summary>
    Fuehrerschein,
    /// <summary>Erste Hilfe und Notfallwissen. Kein Rahmenlehrplan-Fach, sondern ein eigener
    /// Bereich - anders als die Schulfächer mit EINEM Fragenpool für alle Klassenstufen, weil
    /// die fünf W-Fragen mit elf dieselben sind wie mit fünfzehn (siehe ErsteHilfeGenerator).
    /// Am Ende angefügt.</summary>
    ErsteHilfe
}
