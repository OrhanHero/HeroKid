using System.Globalization;
using System.Text;

namespace LernTor.Core.Services;

/// <summary>Eine Zeile im ausgegebenen Bericht.</summary>
/// <param name="Label">Beschriftung (Fach, Thema, Kindername …).</param>
/// <param name="Value">Fertig formatierter Wert rechts.</param>
/// <param name="Rate">0..1 für einen Ampel-Balken; null = keine Quote, kein Balken.</param>
/// <param name="Note">Optionale zweite Zeile in klein (z.B. die Lernzeit-Einordnung).</param>
public sealed record ReportExportRow(string Label, string Value, double? Rate = null, string? Note = null);

/// <summary>Ein Abschnitt mit Überschrift; leere Abschnitte fallen beim Ausgeben weg.</summary>
public sealed record ReportExportSection(string Title, IReadOnlyList<ReportExportRow> Rows);

/// <summary>Alles, was in der Berichtsdatei landet.</summary>
/// <param name="ChildName">Name des Kindes (Überschrift).</param>
/// <param name="PeriodLabel">Zeitraum im Klartext, z.B. "Letzte 7 Tage".</param>
/// <param name="CreatedAt">Erstellungszeitpunkt (steht in der Fußzeile).</param>
/// <param name="SummaryLines">Die Zeilen über den Tabellen (Lerntage, Quiz-Verlauf, Tempo …).</param>
/// <param name="Sections">Tabellen-Abschnitte in Ausgabereihenfolge.</param>
public sealed record ReportExportDocument(
    string ChildName,
    string PeriodLabel,
    DateTimeOffset CreatedAt,
    IReadOnlyList<string> SummaryLines,
    IReadOnlyList<ReportExportSection> Sections);

/// <summary>
/// Schreibt den Eltern-Bericht als eine Datei zum Aufheben, Ausdrucken oder Mitnehmen zum
/// Elternabend.
///
/// <para>Ausgabeformat ist <b>HTML</b>, bewusst ohne PDF-Bibliothek: eine einzelne HTML-Datei
/// öffnet jeder Rechner ohne Zusatzsoftware, und über "Drucken → Als PDF speichern" entsteht das
/// PDF genau dann, wenn es gebraucht wird. Eine PDF-Bibliothek wäre eine weitere Abhängigkeit im
/// self-contained Build für ein Ergebnis, das der Browser ohnehin liefert.</para>
///
/// <para>Die Datei ist vollständig eigenständig: Stil inline, keine Bilder, keine Netzwerk-Aufrufe.
/// Ein Bericht, der auf einem Rechner ohne Internet nur halb aussieht, wäre wertlos - und ein
/// Nachladen von außen hätte in einer Familien-App ohnehin nichts zu suchen.</para>
/// </summary>
public static class ReportExport
{
    /// <summary>Ab dieser Quote gilt ein Fach als sicher (grün) - wie die Ampel in der App.</summary>
    public const double GoodRate = 0.75;

    /// <summary>Unter dieser Quote wird rot markiert.</summary>
    public const double WeakRate = 0.5;

    public static string ToHtml(ReportExportDocument document)
    {
        var html = new StringBuilder();

        html.Append("<!DOCTYPE html>\n<html lang=\"de\">\n<head>\n<meta charset=\"utf-8\" />\n");
        html.Append("<title>LernTor-Bericht – ").Append(Escape(document.ChildName)).Append("</title>\n");
        html.Append("<style>\n").Append(Css).Append("</style>\n</head>\n<body>\n");

        html.Append("<h1>LernTor-Bericht</h1>\n");
        html.Append("<p class=\"meta\"><strong>")
            .Append(Escape(document.ChildName))
            .Append("</strong> · ")
            .Append(Escape(document.PeriodLabel))
            .Append("</p>\n");

        foreach (var line in document.SummaryLines.Where(line => !string.IsNullOrWhiteSpace(line)))
        {
            html.Append("<p class=\"summary\">").Append(Escape(line)).Append("</p>\n");
        }

        foreach (var section in document.Sections.Where(section => section.Rows.Count > 0))
        {
            html.Append("<h2>").Append(Escape(section.Title)).Append("</h2>\n");

            foreach (var row in section.Rows)
            {
                html.Append("<div class=\"row\">\n  <div class=\"line\"><span class=\"label\">")
                    .Append(Escape(row.Label))
                    .Append("</span><span class=\"value\">")
                    .Append(Escape(row.Value))
                    .Append("</span></div>\n");

                if (row.Rate is { } rate)
                {
                    var percent = Math.Clamp(rate, 0, 1) * 100;
                    html.Append("  <div class=\"bar\"><span class=\"fill ")
                        .Append(RateClass(rate))
                        .Append("\" style=\"width:")
                        .Append(percent.ToString("0.#", CultureInfo.InvariantCulture))
                        .Append("%\"></span></div>\n");
                }

                if (!string.IsNullOrWhiteSpace(row.Note))
                {
                    html.Append("  <div class=\"note\">").Append(Escape(row.Note)).Append("</div>\n");
                }

                html.Append("</div>\n");
            }
        }

        html.Append("<p class=\"footer\">Erstellt am ")
            .Append(Escape(document.CreatedAt.ToString("dd.MM.yyyy HH:mm", CultureInfo.GetCultureInfo("de-DE"))))
            .Append(" mit LernTor. Alle Daten stammen aus dem lokalen Aktivitätsprotokoll dieses Rechners.</p>\n");

        html.Append("</body>\n</html>\n");

        return html.ToString();
    }

    internal static string RateClass(double rate) =>
        rate >= GoodRate ? "good" : rate >= WeakRate ? "ok" : "weak";

    /// <summary>
    /// HTML-Maskierung. Zwingend, nicht kosmetisch: Kindernamen, eigene Aufgaben und
    /// Themenbezeichnungen sind frei eingegebener Text - ein "&amp;" oder eine spitze Klammer
    /// darin würde die Datei sonst zerlegen.
    /// </summary>
    public static string Escape(string? text)
    {
        if (string.IsNullOrEmpty(text))
        {
            return string.Empty;
        }

        return text
            .Replace("&", "&amp;")
            .Replace("<", "&lt;")
            .Replace(">", "&gt;")
            .Replace("\"", "&quot;")
            .Replace("'", "&#39;");
    }

    /// <summary>
    /// Dateiname-Vorschlag, z.B. <c>LernTor-Bericht-Emirhan-2026-08-05.html</c>.
    /// Zeichen, die Windows in Dateinamen verbietet, werden ersetzt - "Ali/Ayşe" als Profilname
    /// hätte den Speichern-Dialog sonst mit einem Pfadfehler abgewiesen.
    /// </summary>
    public static string SuggestFileName(string? childName, DateOnly today)
    {
        var cleaned = new StringBuilder();
        foreach (var c in (childName ?? string.Empty).Trim())
        {
            cleaned.Append(char.IsLetterOrDigit(c) || c is '-' or '_' ? c : '-');
        }

        var name = cleaned.ToString().Trim('-');
        while (name.Contains("--", StringComparison.Ordinal))
        {
            name = name.Replace("--", "-", StringComparison.Ordinal);
        }

        var namePart = string.IsNullOrEmpty(name) ? string.Empty : $"-{name}";

        return $"LernTor-Bericht{namePart}-{today:yyyy-MM-dd}.html";
    }

    private const string Css = """
        body { font-family: Segoe UI, Arial, sans-serif; margin: 32px auto; max-width: 780px;
               color: #1f2430; line-height: 1.45; }
        h1 { font-size: 24px; margin: 0 0 4px; }
        h2 { font-size: 17px; margin: 26px 0 8px; border-bottom: 1px solid #dfe3ea; padding-bottom: 4px; }
        .meta { color: #5a6270; margin: 0 0 18px; }
        .summary { margin: 4px 0; }
        .row { margin: 0 0 9px; }
        .line { display: flex; justify-content: space-between; gap: 16px; }
        .label { font-weight: 600; }
        .value { white-space: nowrap; color: #3c4453; }
        .note { font-size: 12px; color: #5a6270; margin-top: 2px; }
        .bar { background: #e8ebf0; border-radius: 5px; height: 9px; margin-top: 4px; overflow: hidden; }
        .fill { display: block; height: 100%; border-radius: 5px; }
        .fill.good { background: #2e9e5b; }
        .fill.ok   { background: #d7a017; }
        .fill.weak { background: #cc3b3b; }
        .footer { margin-top: 30px; font-size: 12px; color: #79808d; }
        @media print { body { margin: 0; max-width: none; } h2 { break-after: avoid; } .row { break-inside: avoid; } }
        """;
}
