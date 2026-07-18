namespace LernTor.Core.Enums;

/// <summary>
/// Strikt sequenzieller Ablauf: jede Stufe schaltet erst nach Abschluss der vorherigen frei.
/// Fachbereiche, die Eltern über "Bereiche deaktivieren" ausschalten, werden automatisch
/// übersprungen (siehe ProgressGateService) - so lässt sich die tägliche Liste pro Kind auf eine
/// sinnvolle Auswahl reduzieren, statt täglich alle Fächer durchzugehen.
/// </summary>
public enum LearningStage
{
    Willkommen,
    Vorlesen,
    Tippen,
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
    KiWissen,
    Abschlussquiz,
    Freigeschaltet
}
