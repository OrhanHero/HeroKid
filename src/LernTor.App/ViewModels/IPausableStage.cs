namespace LernTor.App.ViewModels;

/// <summary>
/// Eine Lernetappe, deren Mindestzeit-Uhr angehalten werden kann.
///
/// <para><b>Warum es das braucht:</b> mehrere Etappen haben eine Mindestverweildauer (Lesezeit,
/// Sekunden je Aufgabe, Sekunden je Artikel), die von einem <c>DispatcherTimer</c> heruntergezählt
/// wird. Der Planer-Knopf lässt das Kind mitten aus einer Etappe heraus zum Hausaufgaben- und
/// Klausurplan wechseln. Liefe die Uhr dabei weiter, wäre der Planer der bequemste Weg, die
/// Mindestzeit abzusitzen: aufmachen, warten, zurück, weiter. Genau das soll er nicht sein.</para>
///
/// <para>Etappen ohne Uhr müssen das hier nicht implementieren - <c>MainViewModel</c> prüft
/// vor dem Aufruf auf den Typ.</para>
/// </summary>
public interface IPausableStage
{
    /// <summary>Hält die Mindestzeit-Uhr an. Mehrfach aufrufbar.</summary>
    void PauseStage();

    /// <summary>Lässt sie weiterlaufen - nur, wenn sie vorher auch lief.</summary>
    void ResumeStage();
}
