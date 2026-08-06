namespace LernTor.Core.Services;

/// <summary>
/// Wann eine Theoriefrage als gekonnt gilt - dieselbe Regel wie bei den Verkehrszeichen
/// (<see cref="TrafficSignProgress"/>) und in der Fehler-Kartei, damit die Kinder nicht drei
/// verschiedene Vorstellungen von "sitzt" lernen.
///
/// <para><b>Warum das für den Schwachstellen-Trainer wichtig ist:</b> die Trefferquote eines
/// Sachgebiets wird nicht aus allen jemals gegebenen Antworten gemittelt, sondern daraus, wie
/// viele Fragen des Gebiets <i>gerade jetzt</i> sitzen. Ein Mittelwert über die ganze Historie
/// würde ein Gebiet auch dann noch als Schwachstelle führen, wenn das Kind es längst kann - und
/// der Trainer würde weiter Fragen daraus schicken, statt zum nächsten Problem zu gehen.</para>
/// </summary>
public static class TheoryProgress
{
    /// <summary>So oft muss eine Frage hintereinander richtig beantwortet werden, damit sie
    /// als gekonnt gilt. Zwei statt eins, weil bei vier Antworten jeder vierte Rateversuch
    /// trifft - zweimal hintereinander zu raten gelingt nur in einem von sechzehn Fällen.</summary>
    public const int MasteredStreak = 2;

    public static bool IsMastered(int correctStreak) => correctStreak >= MasteredStreak;

    /// <summary>
    /// Neuer Serienstand nach einer Antwort. Richtig zählt hoch, falsch setzt auf null zurück -
    /// nicht auf "eins weniger": eine Frage, die gerade verhauen wurde, sitzt nicht mehr fast.
    /// </summary>
    public static int NextStreak(int currentStreak, bool wasCorrect) => wasCorrect ? currentStreak + 1 : 0;
}
