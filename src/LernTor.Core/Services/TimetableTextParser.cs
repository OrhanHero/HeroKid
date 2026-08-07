using System.Globalization;
using System.Text.RegularExpressions;
using LernTor.Core.Models;

namespace LernTor.Core.Services;

/// <summary>Was beim Einlesen eines Stundenplan-Textes herauskam.</summary>
/// <param name="Lessons">Die erkannten Stunden.</param>
/// <param name="Warnings">Zeilen, die nicht gedeutet werden konnten - im Klartext, mit
/// Zeilennummer. Sie werden NICHT verschluckt: eine stillschweigend übersprungene Zeile wäre
/// eine Schulstunde, die im Plan des Kindes fehlt.</param>
public readonly record struct TimetableParseResult(
    IReadOnlyList<TimetableLesson> Lessons,
    IReadOnlyList<string> Warnings)
{
    public bool HasLessons => Lessons.Count > 0;

    public bool HasWarnings => Warnings.Count > 0;
}

/// <summary>
/// Liest einen von Hand getippten Stundenplan ein - eine Zeile je Wochentag:
///
/// <code>
/// Mo: 1 Deutsch, 2 Mathe, 3 Sport (TH1), 4 Sport, 6 Englisch, 7 Englisch
/// Di: 1 Musik, 2 Deutsch, 4 GeWi, 6 NaWi, 7 Englisch
/// </code>
///
/// <para><b>Bewusst dieses eine Format und kein Erraten fremder Tabellen.</b> Die Stundenpläne
/// der beiden Schulen sehen völlig verschieden aus - der eine ein eingescannter Untis-Ausdruck
/// mit OCR-Text voller Lesefehler, der andere eine Word-Tabelle mit umbrochenen Zellen. Ein
/// Importeur, der beides selbst zu deuten versucht, hätte bei jedem neuen Plan neu geraten, und
/// ein still falsch übernommener Stundenplan ist schlechter als gar keiner. Wer den Plan nicht
/// tippen will, trägt ihn im Raster des Eltern-Bereichs ein - das ist der eigentliche Weg,
/// dieser hier die Abkürzung für alle, die lieber schreiben.</para>
///
/// <para>Zugelassen sind Doppelpunkt oder nichts hinter dem Tag, Komma oder Semikolon zwischen
/// den Stunden, "1." oder "1" als Stundennummer und ein Raum in Klammern. Was nicht passt,
/// wird gemeldet, nicht übergangen.</para>
/// </summary>
public static class TimetableTextParser
{
    private static readonly IReadOnlyDictionary<string, DayOfWeek> Wochentage =
        new Dictionary<string, DayOfWeek>(StringComparer.OrdinalIgnoreCase)
        {
            ["mo"] = DayOfWeek.Monday,
            ["mon"] = DayOfWeek.Monday,
            ["montag"] = DayOfWeek.Monday,
            ["di"] = DayOfWeek.Tuesday,
            ["die"] = DayOfWeek.Tuesday,
            ["dienstag"] = DayOfWeek.Tuesday,
            ["mi"] = DayOfWeek.Wednesday,
            ["mit"] = DayOfWeek.Wednesday,
            ["mittwoch"] = DayOfWeek.Wednesday,
            ["do"] = DayOfWeek.Thursday,
            ["don"] = DayOfWeek.Thursday,
            ["donnerstag"] = DayOfWeek.Thursday,
            ["fr"] = DayOfWeek.Friday,
            ["fre"] = DayOfWeek.Friday,
            ["freitag"] = DayOfWeek.Friday
        };

    /// <summary>"1 Mathe", "3. Sport (TH1)", "2 Deutsch [A302]".</summary>
    private static readonly Regex StundenMuster = new(
        @"^(?<nr>\d{1,2})\s*[.)]?\s+(?<rest>.+)$",
        RegexOptions.Compiled);

    /// <summary>Raum in runden oder eckigen Klammern am Ende.</summary>
    private static readonly Regex RaumMuster = new(
        @"^(?<fach>.+?)\s*[\(\[](?<raum>[^\)\]]+)[\)\]]\s*$",
        RegexOptions.Compiled);

    public static TimetableParseResult Parse(string? text)
    {
        var stunden = new List<TimetableLesson>();
        var warnungen = new List<string>();

        if (string.IsNullOrWhiteSpace(text))
        {
            return new TimetableParseResult(stunden, warnungen);
        }

        var zeilen = text.Replace("\r\n", "\n").Replace('\r', '\n').Split('\n');
        // Ein Tag darf über mehrere Zeilen gehen: wer den Plan aus einem langen Tag umbricht,
        // soll nicht die Tagesangabe wiederholen muessen.
        DayOfWeek? aktuellerTag = null;

        for (var i = 0; i < zeilen.Length; i++)
        {
            var zeile = zeilen[i].Trim();
            if (zeile.Length == 0)
            {
                continue;
            }

            var nummer = i + 1;
            var (tag, rest) = TagAbtrennen(zeile);

            if (tag is not null)
            {
                aktuellerTag = tag;
                if (rest.Length == 0)
                {
                    continue;
                }
            }
            else
            {
                rest = zeile;
            }

            if (aktuellerTag is null)
            {
                warnungen.Add($"Zeile {nummer}: kein Wochentag erkannt – „{Kuerzen(zeile)}“");
                continue;
            }

            foreach (var eintrag in rest.Split(new[] { ',', ';' }, StringSplitOptions.RemoveEmptyEntries))
            {
                var roh = eintrag.Trim();
                if (roh.Length == 0)
                {
                    continue;
                }

                var stunde = StundeLesen(aktuellerTag.Value, roh);
                if (stunde is null)
                {
                    warnungen.Add($"Zeile {nummer}: „{Kuerzen(roh)}“ – erwartet wird „Stunde Fach“, z. B. „3 Mathe“.");
                    continue;
                }

                stunden.Add(stunde);
            }
        }

        // Doppelte Stunde/Tag-Kombinationen: die zuletzt genannte gewinnt, damit ein
        // nachgetragener Korrektur-Eintrag nicht wirkungslos bleibt.
        var bereinigt = stunden
            .GroupBy(stunde => (stunde.Day, stunde.Period))
            .Select(gruppe => gruppe.Last())
            .OrderBy(stunde => (int)stunde.Day)
            .ThenBy(stunde => stunde.Period)
            .ToList();

        return new TimetableParseResult(bereinigt, warnungen);
    }

    /// <remarks>Das zweite Feld heißt bewusst NICHT "Rest": <c>Rest</c> ist als Tupel-Feldname
    /// reserviert (<c>ValueTuple</c> nutzt ihn für Tupel ab acht Feldern) und ist an JEDER
    /// Position verboten - <c>CS8126</c>, ein Compilerfehler.</remarks>
    private static (DayOfWeek? Tag, string Restzeile) TagAbtrennen(string zeile)
    {
        var doppelpunkt = zeile.IndexOf(':');
        if (doppelpunkt > 0)
        {
            var kopf = zeile[..doppelpunkt].Trim();
            if (Wochentage.TryGetValue(kopf, out var mitDoppelpunkt))
            {
                return (mitDoppelpunkt, zeile[(doppelpunkt + 1)..].Trim());
            }
        }

        // Ohne Doppelpunkt: erstes Wort probieren ("Mo 1 Deutsch, 2 Mathe").
        var leerzeichen = zeile.IndexOf(' ');
        var erstesWort = leerzeichen < 0 ? zeile : zeile[..leerzeichen];
        if (Wochentage.TryGetValue(erstesWort.Trim(), out var ohneDoppelpunkt))
        {
            return (ohneDoppelpunkt, leerzeichen < 0 ? string.Empty : zeile[(leerzeichen + 1)..].Trim());
        }

        return (null, zeile);
    }

    /// <summary>
    /// Trennt einen angehängten Raum ab: aus "Mathe (A012a)" wird ("Mathe", "A012a").
    /// Ohne Klammern bleibt der Text unverändert und der Raum leer.
    ///
    /// <para>Öffentlich, weil das Eingaberaster im Eltern-Bereich dieselbe Schreibweise annimmt
    /// wie der getippte Plan - zwei Stellen mit derselben Regel wären zwei Stellen, an denen sie
    /// auseinanderlaufen kann.</para>
    /// </summary>
    public static (string Subject, string? Room) SplitRoom(string? cell)
    {
        var text = cell?.Trim() ?? string.Empty;
        if (text.Length == 0)
        {
            return (string.Empty, null);
        }

        var treffer = RaumMuster.Match(text);
        if (!treffer.Success)
        {
            return (text, null);
        }

        var fach = treffer.Groups["fach"].Value.Trim();
        var raum = treffer.Groups["raum"].Value.Trim();

        // "(Sport)" ohne Fach davor ist kein Raum, sondern das Fach selbst.
        return fach.Length == 0 ? (text, null) : (fach, raum.Length == 0 ? null : raum);
    }

    private static TimetableLesson? StundeLesen(DayOfWeek tag, string eintrag)
    {
        var treffer = StundenMuster.Match(eintrag);
        if (!treffer.Success)
        {
            return null;
        }

        if (!int.TryParse(treffer.Groups["nr"].Value, NumberStyles.None, CultureInfo.InvariantCulture, out var nr))
        {
            return null;
        }

        if (nr < 1 || nr > Timetable.MaxPeriod)
        {
            return null;
        }

        var rest = treffer.Groups["rest"].Value.Trim();
        string? raum = null;

        var raumTreffer = RaumMuster.Match(rest);
        if (raumTreffer.Success)
        {
            raum = raumTreffer.Groups["raum"].Value.Trim();
            rest = raumTreffer.Groups["fach"].Value.Trim();
        }

        return rest.Length == 0 ? null : new TimetableLesson(tag, nr, rest, null, raum);
    }

    private static string Kuerzen(string text) =>
        text.Length <= 40 ? text : text[..40] + "…";
}
