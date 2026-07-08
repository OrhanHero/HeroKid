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
    News,
    Mathematik,
    Deutsch,
    Tuerkisch,
    Englisch,
    Biologie,
    Chemie,
    Physik,
    Gewi,
    Politik,
    Geo,
    Ethik,
    Itg,
    Abschlussquiz,
    Freigeschaltet
}
