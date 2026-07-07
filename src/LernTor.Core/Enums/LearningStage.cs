namespace LernTor.Core.Enums;

/// <summary>
/// Strikt sequenzieller Ablauf: jede Stufe schaltet erst nach Abschluss der vorherigen frei.
/// </summary>
public enum LearningStage
{
    Willkommen = 0,
    News = 1,
    Mathematik = 2,
    Deutsch = 3,
    Tuerkisch = 4,
    Naturwissenschaften = 5,
    Abschlussquiz = 6,
    Freigeschaltet = 7
}
