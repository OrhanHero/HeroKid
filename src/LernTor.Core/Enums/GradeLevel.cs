namespace LernTor.Core.Enums;

public enum GradeLevel
{
    Klasse6 = 6,

    /// <summary>Klasse 7 (Sek I, Doppeljahrgang 7/8 im Berliner RLP). Fächer ohne eigenen
    /// Klasse-7-Themenpool fallen übergangsweise auf die nächstniedrigere Stufe zurück
    /// (siehe ExerciseGeneratorBase.Generate) - Content wird Fach für Fach ergänzt.</summary>
    Klasse7 = 7,

    Klasse9 = 9
}
