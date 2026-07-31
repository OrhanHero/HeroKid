using System.Text.RegularExpressions;
using LernTor.Core.Enums;

namespace LernTor.News;

/// <summary>
/// Jugendschutz-Prüfung für Nachrichtenartikel: entscheidet, ob ein Artikel für die Zielgruppe
/// (10-15 Jahre) überhaupt in Frage kommt und wie heikel er ist.
///
/// <para>Zwei Stufen mit Absicht. <b>Hart gesperrt</b> ist, was in einer Lern-App für Kinder
/// nichts zu suchen hat, egal wie aktuell es ist: sexualisierte Gewalt, Suizid, Folter,
/// Missbrauch, Massaker. Solche Artikel werden nie ausgewählt. <b>Heikel</b> sind Themen wie
/// Krieg, Kriminalität oder Unfälle - die gehören zum Rahmenlehrplan und dürfen vorkommen, aber
/// sie werden nur genommen, wenn dieselbe Quelle an dem Tag nichts Harmloseres hergibt.</para>
///
/// <para>Wichtig ist die <b>Sprachtrennung</b>: die Stichwörter gelten nur für Quellen in der
/// jeweiligen Sprache. Sonst hätte das englische "war" jeden deutschen Artikel getroffen, in dem
/// "war" als ganz normale Vergangenheitsform steht ("Das Wetter war gut") - und das türkische
/// "kaza" steckt in nichts Deutschem, aber "var" wäre ähnlich gefährlich gewesen.</para>
///
/// <para>Ebenso wichtig: es wird auf <b>ganze Wörter</b> geprüft, nicht auf Teilzeichenketten.
/// Ohne Wortgrenzen hätte "Mord" auch in "Mordent" oder "Nordmord..." gegriffen, "tot" in
/// "total", und englisch "war" in "warm", "Warschau" und "Antwort".</para>
/// </summary>
public static class NewsSuitability
{
    /// <summary>Ergebnis der Prüfung eines Artikels.</summary>
    /// <param name="IsBlocked">Artikel darf nicht ausgewählt werden.</param>
    /// <param name="SensitiveHits">Wie viele heikle Stichwörter vorkommen (0 = unbedenklich).</param>
    public sealed record Verdict(bool IsBlocked, int SensitiveHits);

    /// <summary>
    /// Unter diesem Alter gilt immer <see cref="NewsFilterStrictness.Streng"/>, egal was im
    /// Eltern-Bereich eingestellt ist. Eine Einstellung soll den Schutz für ein Grundschulkind
    /// nicht versehentlich aushebeln - lockerer als "streng" muss man dort nicht können.
    /// </summary>
    public const int AlwaysStrictBelowAge = 10;

    /// <summary>
    /// Welche Strenge tatsächlich gilt: die eingestellte, aber nie lockerer als das Alter erlaubt.
    /// (<see cref="NewsFilterStrictness.Streng"/> ist der kleinste Enum-Wert, deshalb Math.Min.)
    /// </summary>
    public static NewsFilterStrictness EffectiveStrictness(NewsFilterStrictness configured, int? childAge) =>
        childAge is { } age && age < AlwaysStrictBelowAge
            ? NewsFilterStrictness.Streng
            : configured;

    // Hart gesperrt - unabhängig vom Alter des Kindes.
    private static readonly IReadOnlyDictionary<NewsFeedLanguage, string[]> HardBlockedWords =
        new Dictionary<NewsFeedLanguage, string[]>
        {
            [NewsFeedLanguage.Deutsch] = new[]
            {
                "Vergewaltigung", "Vergewaltiger", "vergewaltigt", "Sexualstraftat", "Missbrauchsopfer",
                "Kindesmissbrauch", "Kinderpornografie", "Kinderpornographie", "pädophil", "Pädophiler",
                "Selbstmord", "Suizid", "suizidal", "Selbsttötung",
                "Folter", "gefoltert", "Enthauptung", "enthauptet", "verstümmelt", "Verstümmelung",
                "Massaker", "Amoklauf", "Amokläufer", "Kinderleiche", "Missbrauchsfall"
            },
            [NewsFeedLanguage.Tuerkisch] = new[]
            {
                "tecavüz", "tecavüzcü", "istismar", "cinsel saldırı", "pedofili",
                "intihar", "işkence", "katliam", "başı kesilen", "uzuvları kesilen"
            },
            [NewsFeedLanguage.Englisch] = new[]
            {
                "rape", "raped", "rapist", "sexual assault", "sexual abuse", "child abuse",
                "child sexual", "paedophile", "pedophile",
                "suicide", "torture", "tortured", "beheading", "beheaded", "mutilated", "mutilation",
                "massacre"
            }
        };

    // Heikel - erlaubt, aber nachrangig (und für unter Zehnjährige gesperrt).
    private static readonly IReadOnlyDictionary<NewsFeedLanguage, string[]> SensitiveWords =
        new Dictionary<NewsFeedLanguage, string[]>
        {
            [NewsFeedLanguage.Deutsch] = new[]
            {
                "Krieg", "Kriegs", "Mord", "Mörder", "ermordet", "Totschlag",
                "Terror", "Terroranschlag", "Anschlag", "Attentat", "Attentäter",
                "Leiche", "getötet", "erschossen", "erstochen", "Gewaltverbrechen", "Gewalttat",
                "Bombe", "Bombenanschlag", "Schüsse", "Schießerei", "Geisel",
                "Todesopfer", "gestorben", "ums Leben", "Drogen", "Rauschgift"
            },
            [NewsFeedLanguage.Tuerkisch] = new[]
            {
                "savaş", "cinayet", "katil", "terör", "terörist", "saldırı", "suikast",
                "öldürüldü", "ceset", "bomba", "silahlı", "rehine", "uyuşturucu", "hayatını kaybetti"
            },
            [NewsFeedLanguage.Englisch] = new[]
            {
                "war", "warfare", "murder", "murdered", "killer", "terror", "terrorist", "terrorism",
                "attack", "killed", "shooting", "shot dead", "stabbed", "bomb", "bombing",
                "hostage", "corpse", "died", "death toll", "drugs"
            }
        };

    private static readonly IReadOnlyDictionary<NewsFeedLanguage, Regex> HardBlockedPattern =
        BuildPatterns(HardBlockedWords);

    private static readonly IReadOnlyDictionary<NewsFeedLanguage, Regex> SensitivePattern =
        BuildPatterns(SensitiveWords);

    private static Dictionary<NewsFeedLanguage, Regex> BuildPatterns(
        IReadOnlyDictionary<NewsFeedLanguage, string[]> words) =>
        words.ToDictionary(
            entry => entry.Key,
            entry => new Regex(
                @"\b(" + string.Join("|", entry.Value.Select(Regex.Escape)) + @")\b",
                RegexOptions.IgnoreCase | RegexOptions.CultureInvariant | RegexOptions.Compiled,
                TimeSpan.FromSeconds(1)));

    /// <summary>
    /// Prüft Titel und Anrisstext eines Artikels.
    /// </summary>
    /// <param name="strictness">
    /// Von den Eltern pro Kind eingestellt. Die harte Sperre gilt in jeder Stufe; einstellbar ist
    /// nur der Umgang mit heiklen, aber lehrplanrelevanten Themen.
    /// </param>
    /// <param name="childAge">
    /// Alter des Kindes; unter <see cref="AlwaysStrictBelowAge"/> gilt immer "streng", auch wenn
    /// etwas anderes eingestellt ist. <c>null</c> (kein Alter hinterlegt) ändert nichts an der
    /// Einstellung - sonst bliebe der News-Teil für Profile ohne Altersangabe grundlos leer.
    /// </param>
    public static Verdict Evaluate(
        string? title,
        string? summary,
        NewsFeedLanguage language,
        NewsFilterStrictness strictness = NewsFilterStrictness.Normal,
        int? childAge = null)
    {
        var text = $"{title} {summary}";
        if (string.IsNullOrWhiteSpace(text))
        {
            return new Verdict(IsBlocked: false, SensitiveHits: 0);
        }

        if (Matches(HardBlockedPattern, language, text) > 0)
        {
            return new Verdict(IsBlocked: true, SensitiveHits: int.MaxValue);
        }

        var effective = EffectiveStrictness(strictness, childAge);
        var sensitiveHits = Matches(SensitivePattern, language, text);

        return effective switch
        {
            // Streng: heikle Themen kommen gar nicht vor.
            NewsFilterStrictness.Streng => new Verdict(sensitiveHits > 0, sensitiveHits),

            // Locker: nur die harte Sperre zaehlt, die Reihenfolge entscheidet allein die
            // Aktualitaet - deshalb ohne Treffer zurueckmelden.
            NewsFilterStrictness.Locker => new Verdict(IsBlocked: false, SensitiveHits: 0),

            // Normal: erlaubt, aber nachrangig.
            _ => new Verdict(IsBlocked: false, sensitiveHits)
        };
    }

    private static int Matches(
        IReadOnlyDictionary<NewsFeedLanguage, Regex> patterns, NewsFeedLanguage language, string text)
    {
        if (!patterns.TryGetValue(language, out var pattern))
        {
            return 0;
        }

        try
        {
            return pattern.Matches(text).Count;
        }
        catch (RegexMatchTimeoutException)
        {
            // Bei einem sehr langen Text lieber als "heikel" behandeln als ungeprueft durchlassen.
            return 1;
        }
    }
}
