using LernTor.ContentGen.Generators;
using LernTor.Core.Enums;
using LernTor.Core.Services;

namespace LernTor.ContentGen.Curriculum;

/// <summary>Rahmenlehrplan-Stand eines Fachs für die Klassenstufe eines Kindes.</summary>
/// <param name="Themenfelder">Themenfelder, die sich als Quiz üben lassen.</param>
/// <param name="Abgedeckt">Davon übt LernTor (ganz oder teilweise).</param>
/// <param name="Geuebt">Davon hat das Kind mindestens eine Frage beantwortet.</param>
public sealed record RlpFachStand(string Fach, RlpStufe Stufe, int Themenfelder, int Abgedeckt, int Geuebt)
{
    /// <summary>„Biologie 5/6“ - ohne die Stufe zu verdoppeln, wo der Fachname sie schon trägt
    /// („Naturwissenschaften 5/6“).</summary>
    public string Bezeichnung => Fach.Contains(RahmenlehrplanStand.StufenName(Stufe), StringComparison.Ordinal)
        ? Fach
        : $"{Fach} {RahmenlehrplanStand.StufenName(Stufe)}";
}

/// <summary>
/// Der Rahmenlehrplan-Stand im Eltern-Bericht (docs/NAECHSTES-LEVEL-3-2.md, Schritt 5): je Fach,
/// wie viele Themenfelder der Doppeljahrgangsstufe des Kindes LernTor abdeckt und wie viele davon
/// das Kind schon geübt hat - „Biologie 5/6: 6 von 6 Themenfeldern, davon 4 geübt“.
/// </summary>
public static class RahmenlehrplanStand
{
    /// <summary>Klasse 8 und 10 gehören zum Doppeljahrgang ihrer Vorgängerklasse.</summary>
    public static RlpStufe StufeFuer(GradeLevel klasse) => klasse switch
    {
        GradeLevel.Klasse6 => RlpStufe.Stufe5_6,
        GradeLevel.Klasse7 or GradeLevel.Klasse8 => RlpStufe.Stufe7_8,
        _ => RlpStufe.Stufe9_10
    };

    /// <summary>Die Klassenstufe, deren Themenpool eine Doppeljahrgangsstufe übt.</summary>
    public static GradeLevel PoolKlasse(RlpStufe stufe) => stufe switch
    {
        RlpStufe.Stufe5_6 => GradeLevel.Klasse6,
        RlpStufe.Stufe7_8 => GradeLevel.Klasse7,
        _ => GradeLevel.Klasse9
    };

    public static string StufenName(RlpStufe stufe) => stufe switch
    {
        RlpStufe.Stufe5_6 => "5/6",
        RlpStufe.Stufe7_8 => "7/8",
        _ => "9/10"
    };

    /// <param name="klasse">Klassenstufe des Kindes.</param>
    /// <param name="generatoren">Die Generatoren der App - sie übersetzen die Themen-Methoden des
    /// Katalogs in die Themennamen, die im Aktivitätsprotokoll stehen.</param>
    /// <param name="beantwortet">Fach und Thema jeder protokollierten Antwort (Reihenfolge egal,
    /// Fehler-Kartei-Präfix erlaubt).</param>
    /// <returns>Ein Eintrag je Fach mit mindestens einem übbaren Themenfeld, in Katalog-Reihenfolge.
    /// Leer, wenn der Katalog die Stufe noch nicht kennt.</returns>
    public static IReadOnlyList<RlpFachStand> Berechne(
        GradeLevel klasse,
        IEnumerable<IExerciseGenerator> generatoren,
        IEnumerable<(Subject Fach, string Thema)> beantwortet)
    {
        var stufe = StufeFuer(klasse);
        var poolKlasse = PoolKlasse(stufe);

        var namen = new Dictionary<(Subject, string), IReadOnlySet<string>>();
        foreach (var generator in generatoren.OfType<ExerciseGeneratorBase>())
        {
            foreach (var (methode, themen) in generator.TopicNamesByMethod(poolKlasse))
            {
                namen[(generator.Subject, methode)] = themen;
            }
        }

        var geuebteThemen = beantwortet
            .Select(a => (a.Fach, TopicMasteryCalculator.NormalizeTopic(a.Thema)))
            .ToHashSet();

        bool IstGeuebt(RlpThemenfeld feld) => feld.Themen.Any(z =>
            namen.TryGetValue((z.Fach, z.Thema), out var themen)
            && themen.Any(t => geuebteThemen.Contains((z.Fach, t))));

        var ergebnis = new List<RlpFachStand>();
        foreach (var fach in RahmenlehrplanKatalog.Alle.Where(f => f.Stufe == stufe).GroupBy(f => f.Fach))
        {
            var uebbar = fach.Where(f => f.Status is not (RlpStatus.NichtAlsQuiz or RlpStatus.NichtInLernTor)).ToList();
            if (uebbar.Count == 0)
            {
                continue;
            }

            var abgedeckt = uebbar.Where(f => f.Status is RlpStatus.Abgedeckt or RlpStatus.Teilweise).ToList();
            ergebnis.Add(new RlpFachStand(fach.Key, stufe, uebbar.Count, abgedeckt.Count, abgedeckt.Count(IstGeuebt)));
        }

        return ergebnis;
    }
}
