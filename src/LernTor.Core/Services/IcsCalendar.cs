using System.Globalization;
using System.Text;
using LernTor.Core.Enums;
using LernTor.Core.Models;

namespace LernTor.Core.Services;

/// <summary>Ein aus einer .ics-Datei gelesener Termin, bevor die Eltern ihn übernehmen.</summary>
/// <param name="Title">SUMMARY des Termins.</param>
/// <param name="Description">DESCRIPTION, sofern vorhanden.</param>
/// <param name="Date">Der Tag des Termins.</param>
/// <param name="GuessedSubject">Aus dem Titel erratenes Fach, <c>null</c> wenn nichts passt.</param>
public sealed record ImportedExam(string Title, string Description, DateOnly Date, Subject? GuessedSubject);

/// <summary>
/// Import und Export von Klausurterminen als iCalendar-Datei (.ics, RFC 5545).
///
/// <para>Das ist die bewusste Alternative zu einer Google-Calendar-Anbindung: eine .ics-Datei ist
/// reiner Text, ein offener Standard, den Schulportale, Outlook, Apple und Google alle exportieren
/// und lesen können - ohne Konto, ohne Netzwerk, ohne OAuth und damit ohne Browserfenster, das in
/// einer Kiosk-App genau das Loch wäre, das die Kiosk-Sperre verhindern soll.</para>
///
/// <para>Bewusst nur Termine als GANZE TAGE (<c>VALUE=DATE</c>): für "wann ist die Mathearbeit"
/// ist die Uhrzeit irrelevant, und ganztägige Einträge ersparen jede Zeitzonenrechnung beim
/// Export. Beim Import wird eine vorhandene Uhrzeit dagegen ausgewertet, weil fremde Kalender sie
/// nun einmal mitliefern.</para>
/// </summary>
public static class IcsCalendar
{
    private const string LineBreak = "\r\n";

    /// <summary>
    /// Erzeugt eine .ics-Datei mit einem ganztägigen Termin je Klausur - importierbar in jeden
    /// gängigen Kalender, damit die Klausur auch im Familienkalender auftaucht.
    /// </summary>
    public static string Export(IEnumerable<ExamEntry> exams, string calendarName = "LernTor Klausuren")
    {
        var builder = new StringBuilder();
        builder.Append("BEGIN:VCALENDAR").Append(LineBreak);
        builder.Append("VERSION:2.0").Append(LineBreak);
        builder.Append("PRODID:-//LernTor//Klausurkalender//DE").Append(LineBreak);
        builder.Append("CALSCALE:GREGORIAN").Append(LineBreak);
        builder.Append(Fold($"X-WR-CALNAME:{EscapeText(calendarName)}")).Append(LineBreak);

        foreach (var exam in exams)
        {
            var summary = string.IsNullOrWhiteSpace(exam.Title)
                ? $"{exam.Subject}-Klausur"
                : $"{exam.Subject}: {exam.Title}";

            builder.Append("BEGIN:VEVENT").Append(LineBreak);
            builder.Append($"UID:{exam.Id}@lerntor").Append(LineBreak);
            builder.Append($"DTSTAMP:{exam.CreatedAt.UtcDateTime:yyyyMMdd'T'HHmmss}Z").Append(LineBreak);
            builder.Append($"DTSTART;VALUE=DATE:{exam.ExamDate:yyyyMMdd}").Append(LineBreak);

            // Bei ganztaegigen Terminen ist DTEND laut RFC 5545 EXKLUSIV - also der Folgetag.
            // Mit demselben Tag zeigen manche Kalender den Termin gar nicht an.
            builder.Append($"DTEND;VALUE=DATE:{exam.ExamDate.AddDays(1):yyyyMMdd}").Append(LineBreak);

            builder.Append(Fold($"SUMMARY:{EscapeText(summary)}")).Append(LineBreak);

            if (!string.IsNullOrWhiteSpace(exam.Topics))
            {
                builder.Append(Fold($"DESCRIPTION:{EscapeText(exam.Topics)}")).Append(LineBreak);
            }

            builder.Append("END:VEVENT").Append(LineBreak);
        }

        builder.Append("END:VCALENDAR").Append(LineBreak);
        return builder.ToString();
    }

    /// <summary>
    /// Liest Termine aus einer .ics-Datei. Unbekannte Eigenschaften werden ignoriert - fremde
    /// Kalender liefern eine Menge mit, was hier niemanden interessiert.
    /// </summary>
    public static IReadOnlyList<ImportedExam> Import(string icsContent)
    {
        var results = new List<ImportedExam>();
        if (string.IsNullOrWhiteSpace(icsContent))
        {
            return results;
        }

        string? summary = null, description = null, start = null;
        var insideEvent = false;

        foreach (var line in Unfold(icsContent))
        {
            if (line.StartsWith("BEGIN:VEVENT", StringComparison.OrdinalIgnoreCase))
            {
                insideEvent = true;
                summary = description = start = null;
                continue;
            }

            if (line.StartsWith("END:VEVENT", StringComparison.OrdinalIgnoreCase))
            {
                if (insideEvent && TryParseDate(start, out var date))
                {
                    var title = UnescapeText(summary ?? string.Empty).Trim();
                    results.Add(new ImportedExam(
                        title,
                        UnescapeText(description ?? string.Empty).Trim(),
                        date,
                        GuessSubject(title)));
                }

                insideEvent = false;
                continue;
            }

            if (!insideEvent)
            {
                continue;
            }

            var (name, value) = SplitProperty(line);
            switch (name)
            {
                case "SUMMARY":
                    summary = value;
                    break;
                case "DESCRIPTION":
                    description = value;
                    break;
                case "DTSTART":
                    start = value;
                    break;
            }
        }

        return results;
    }

    /// <summary>
    /// Errät das Fach aus dem Titel ("Mathearbeit", "Klassenarbeit Deutsch"). Trifft nichts zu,
    /// bleibt es <c>null</c> - dann wählen die Eltern es beim Übernehmen selbst, statt dass ein
    /// falsch geratenes Fach still die Übungsgewichtung verstellt.
    /// </summary>
    public static Subject? GuessSubject(string title)
    {
        if (string.IsNullOrWhiteSpace(title))
        {
            return null;
        }

        foreach (var (needle, subject) in SubjectHints)
        {
            if (title.Contains(needle, StringComparison.OrdinalIgnoreCase))
            {
                return subject;
            }
        }

        return null;
    }

    // Reihenfolge zaehlt: "Mathe" muss vor "Ethik" geprueft werden koennen, ohne dass ein
    // kuerzeres Stichwort ein laengeres verdeckt. Deshalb eine Liste statt eines Dictionary.
    private static readonly (string Needle, Subject Subject)[] SubjectHints =
    {
        ("mathe", Subject.Mathematik),
        ("deutsch", Subject.Deutsch),
        ("englisch", Subject.Englisch),
        ("english", Subject.Englisch),
        ("türkisch", Subject.Tuerkisch),
        ("tuerkisch", Subject.Tuerkisch),
        ("türkçe", Subject.Tuerkisch),
        ("biologie", Subject.Biologie),
        ("bio", Subject.Biologie),
        ("chemie", Subject.Chemie),
        ("physik", Subject.Physik),
        ("geschichte", Subject.Geschichte),
        ("geografie", Subject.Geo),
        ("geographie", Subject.Geo),
        ("erdkunde", Subject.Geo),
        ("gesellschaftswissenschaft", Subject.Gewi),
        ("gewi", Subject.Gewi),
        ("politik", Subject.Politik),
        ("ethik", Subject.Ethik),
        ("kunst", Subject.Kunst),
        ("musik", Subject.Musik),
        ("informatik", Subject.Itg),
        ("itg", Subject.Itg)
    };

    /// <summary>
    /// Zerlegt eine Eigenschaftszeile in Name und Wert. Parameter hinter dem Namen
    /// (<c>DTSTART;VALUE=DATE:…</c>) werden abgeschnitten - ausgewertet wird nur der Wert.
    /// </summary>
    private static (string Name, string Value) SplitProperty(string line)
    {
        var colon = line.IndexOf(':');
        if (colon < 0)
        {
            return (line.ToUpperInvariant(), string.Empty);
        }

        var rawName = line[..colon];
        var semicolon = rawName.IndexOf(';');
        var name = (semicolon >= 0 ? rawName[..semicolon] : rawName).Trim().ToUpperInvariant();

        return (name, line[(colon + 1)..]);
    }

    /// <summary>
    /// Liest den Tag aus einem DTSTART-Wert. Alle Formen enden auf demselben Kalendertag:
    /// <c>20260914</c>, <c>20260914T080000</c> und <c>20260914T060000Z</c>. Bei UTC-Angaben wird
    /// in die lokale Zeit umgerechnet, sonst läge ein Termin am frühen Morgen einen Tag daneben.
    /// </summary>
    private static bool TryParseDate(string? value, out DateOnly date)
    {
        date = default;
        var text = value?.Trim();
        if (string.IsNullOrEmpty(text) || text.Length < 8)
        {
            return false;
        }

        if (text.EndsWith('Z') &&
            DateTime.TryParseExact(text, "yyyyMMdd'T'HHmmss'Z'", CultureInfo.InvariantCulture,
                DateTimeStyles.AdjustToUniversal | DateTimeStyles.AssumeUniversal, out var utc))
        {
            date = DateOnly.FromDateTime(utc.ToLocalTime());
            return true;
        }

        return DateOnly.TryParseExact(text[..8], "yyyyMMdd", CultureInfo.InvariantCulture,
            DateTimeStyles.None, out date);
    }

    /// <summary>
    /// Hebt die Zeilenfaltung nach RFC 5545 auf: Zeilen über 75 Zeichen werden mit Zeilenumbruch
    /// plus führendem Leerzeichen oder Tabulator fortgesetzt. Ohne das Zusammenfügen zerfiele
    /// jeder längere Titel in Bruchstücke.
    /// </summary>
    private static IEnumerable<string> Unfold(string content)
    {
        var current = new StringBuilder();

        foreach (var rawLine in content.Replace("\r\n", "\n").Replace('\r', '\n').Split('\n'))
        {
            if (rawLine.Length > 0 && (rawLine[0] == ' ' || rawLine[0] == '\t'))
            {
                current.Append(rawLine[1..]);
                continue;
            }

            if (current.Length > 0)
            {
                yield return current.ToString();
                current.Clear();
            }

            current.Append(rawLine);
        }

        if (current.Length > 0)
        {
            yield return current.ToString();
        }
    }

    /// <summary>Faltet lange Zeilen auf 75 Zeichen - manche Kalender lehnen längere ab.</summary>
    internal static string Fold(string line)
    {
        const int maxLength = 75;
        if (line.Length <= maxLength)
        {
            return line;
        }

        var builder = new StringBuilder(line[..maxLength]);
        var position = maxLength;

        while (position < line.Length)
        {
            var take = Math.Min(maxLength - 1, line.Length - position);
            builder.Append(LineBreak).Append(' ').Append(line, position, take);
            position += take;
        }

        return builder.ToString();
    }

    internal static string EscapeText(string text) => text
        .Replace("\\", "\\\\")
        .Replace(";", "\\;")
        .Replace(",", "\\,")
        .Replace("\r\n", "\\n")
        .Replace("\n", "\\n");

    internal static string UnescapeText(string text)
    {
        var builder = new StringBuilder(text.Length);

        for (var i = 0; i < text.Length; i++)
        {
            if (text[i] != '\\' || i + 1 >= text.Length)
            {
                builder.Append(text[i]);
                continue;
            }

            i++;
            builder.Append(text[i] switch
            {
                'n' or 'N' => '\n',
                _ => text[i]
            });
        }

        return builder.ToString();
    }
}
