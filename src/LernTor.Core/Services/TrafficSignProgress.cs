using LernTor.Core.Enums;
using LernTor.Core.Models;

namespace LernTor.Core.Services;

/// <summary>Lernstand einer Zeichengruppe: wie viele davon sitzen schon?</summary>
/// <param name="Category">Die Gruppe.</param>
/// <param name="Mastered">Zeichen, die zweimal in Folge richtig erkannt wurden.</param>
/// <param name="Total">Zeichen in der Gruppe insgesamt.</param>
public readonly record struct CategoryProgress(TrafficSignCategory Category, int Mastered, int Total)
{
    /// <summary>0..1 - für den Fortschrittsbalken.</summary>
    public double Fraction => Total == 0 ? 0 : (double)Mastered / Total;

    public int Percent => (int)Math.Round(Fraction * 100);

    public bool IsComplete => Total > 0 && Mastered == Total;
}

/// <summary>
/// Die Regeln, wann ein Verkehrszeichen als gekonnt gilt - bewusst in Core und ohne Datenbank,
/// damit sie prüfbar sind und nicht im Repository verstreut liegen.
/// </summary>
public static class TrafficSignProgress
{
    /// <summary>So oft muss ein Zeichen hintereinander richtig erkannt werden. Zwei statt eins,
    /// weil bei vier Antwortmöglichkeiten jeder vierte Rateversuch trifft - zweimal hintereinander
    /// zu raten gelingt nur in einem von sechzehn Fällen.</summary>
    public const int MasteredStreak = 2;

    public static bool IsMastered(int correctStreak) => correctStreak >= MasteredStreak;

    /// <summary>
    /// Neuer Serienstand nach einer Antwort. Richtig zählt hoch, falsch setzt auf null zurück -
    /// nicht auf "eins weniger": ein Zeichen, das gerade verwechselt wurde, sitzt nicht mehr fast.
    /// </summary>
    public static int NextStreak(int currentStreak, bool wasCorrect) => wasCorrect ? currentStreak + 1 : 0;

    /// <summary>Lernstand je Gruppe, in der Lernreihenfolge des Katalogs.</summary>
    public static IReadOnlyList<CategoryProgress> ByCategory(
        IReadOnlyList<TrafficSign> pool, IReadOnlySet<string> masteredNumbers) =>
        TrafficSignCatalog.LearningOrder
            .Select(category =>
            {
                var inCategory = pool.Where(sign => sign.Category == category).ToList();
                return new CategoryProgress(
                    category,
                    inCategory.Count(sign => masteredNumbers.Contains(sign.Number)),
                    inCategory.Count);
            })
            .Where(progress => progress.Total > 0)
            .ToList();

    /// <summary>
    /// Die als Nächstes empfohlene Gruppe: die erste in Lernreihenfolge, die noch nicht
    /// vollständig sitzt. Null, wenn alles gekonnt ist.
    /// </summary>
    public static TrafficSignCategory? NextRecommended(IReadOnlyList<CategoryProgress> progress)
    {
        foreach (var eintrag in progress)
        {
            if (!eintrag.IsComplete)
            {
                return eintrag.Category;
            }
        }

        return null;
    }
}
