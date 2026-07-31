using System.Globalization;
using System.Text;
using LernTor.Core.Enums;
using LernTor.Core.Models;

namespace LernTor.Core.Services;

/// <summary>
/// Erzeugt aus Aufgaben ein ausdruckbares Übungsblatt - mit Lösungsblatt auf einer eigenen Seite.
///
/// <para>Warum überhaupt Papier in einer Bildschirm-App: Klausurvorbereitung findet am
/// Küchentisch statt, oft ohne PC, oft mit einem Elternteil daneben. Und wer eine Rechnung
/// aufschreibt, statt eine von drei Optionen anzuklicken, rechnet sie wirklich - das ist ein
/// anderer Lernvorgang als der am Bildschirm, kein schlechterer Ersatz dafür.</para>
///
/// <para><b>Die Lösungen stehen auf einem eigenen Blatt</b>, hinter einem harten Seitenumbruch.
/// Das ist die wichtigste Einzelheit an dieser Datei: stünde die Antwort neben der Aufgabe, wäre
/// das Blatt wertlos, weil kein Kind wegsieht - und niemand sollte das von ihm verlangen.</para>
///
/// <para>Ausgabeformat ist wie beim Elternbericht HTML statt PDF: eine einzelne Datei, die jeder
/// Rechner ohne Zusatzsoftware öffnet und drucken kann (siehe <see cref="ReportExport"/>).</para>
/// </summary>
public static class WorksheetExport
{
    /// <summary>Buchstaben für die Antwortoptionen auf dem Blatt.</summary>
    private const string OptionLetters = "ABCDEFGH";

    /// <summary>
    /// Baut das Blatt. <paramref name="questions"/> kommt bereits fertig gemischt vom Aufrufer -
    /// diese Klasse wählt nichts aus, sie setzt nur.
    /// </summary>
    /// <param name="questions">Die Aufgaben in Druckreihenfolge.</param>
    /// <param name="subjectLabel">Fach im Klartext (Übersetzung liegt in der App).</param>
    /// <param name="grade">Klassenstufe für die Kopfzeile.</param>
    /// <param name="createdOn">Datum für die Kopfzeile.</param>
    /// <param name="random">Für das Mischen der Antwortoptionen; null = feste Reihenfolge (Tests).</param>
    public static string ToHtml(
        IReadOnlyList<QuizQuestion> questions,
        string subjectLabel,
        GradeLevel grade,
        DateOnly createdOn,
        Random? random = null)
    {
        // Die Optionen MUESSEN gemischt werden: die Generatoren legen die richtige Antwort meist
        // als erstes Element an, ungemischt waere auf dem ganzen Blatt fast immer "A" richtig -
        // und das durchschaut ein Kind sofort. Der Bildschirm mischt aus demselben Grund
        // (siehe QuestionAnswerViewModel.DisplayOptions). Einmal gemischt und dann fuer Aufgabe
        // UND Loesung verwendet, sonst zeigte das Loesungsblatt einen anderen Buchstaben.
        var shuffler = random ?? new Random();

        var printable = questions
            .Where(q => q.Type != QuestionType.Diktat)
            .Select(q => (Question: q, Options: (IReadOnlyList<string>)(q.Options.Count == 0
                ? q.Options
                : q.Options.OrderBy(_ => shuffler.Next()).ToList())))
            .ToList();

        var dictations = questions.Where(q => q.Type == QuestionType.Diktat).ToList();

        var html = new StringBuilder();

        html.Append("<!DOCTYPE html>\n<html lang=\"de\">\n<head>\n<meta charset=\"utf-8\" />\n");
        html.Append("<title>Übungsblatt ").Append(ReportExport.Escape(subjectLabel)).Append("</title>\n");
        html.Append("<style>\n").Append(Css).Append("</style>\n</head>\n<body>\n");

        // --- Aufgabenblatt ---
        html.Append("<h1>Übungsblatt ").Append(ReportExport.Escape(subjectLabel)).Append("</h1>\n");
        html.Append("<p class=\"meta\">Klasse ").Append(GradeNumber(grade))
            .Append(" · ").Append(createdOn.ToString("dd.MM.yyyy", CultureInfo.GetCultureInfo("de-DE")))
            .Append("</p>\n");
        html.Append("<p class=\"namefield\">Name: ______________________________</p>\n");

        for (var i = 0; i < printable.Count; i++)
        {
            AppendTask(html, printable[i].Question, printable[i].Options, i + 1);
        }

        if (printable.Count == 0)
        {
            html.Append("<p class=\"empty\">Für dieses Fach und diese Klassenstufe gibt es keine druckbaren Aufgaben.</p>\n");
        }

        // --- Lösungsblatt, hinter hartem Seitenumbruch ---
        html.Append("<div class=\"pagebreak\"></div>\n");
        html.Append("<h1>Lösungen</h1>\n");
        html.Append("<p class=\"meta\">").Append(ReportExport.Escape(subjectLabel))
            .Append(" · Klasse ").Append(GradeNumber(grade))
            .Append(" · ").Append(createdOn.ToString("dd.MM.yyyy", CultureInfo.GetCultureInfo("de-DE")))
            .Append("</p>\n");

        for (var i = 0; i < printable.Count; i++)
        {
            AppendSolution(html, printable[i].Question, printable[i].Options, i + 1);
        }

        // Diktate lassen sich nicht als Aufgabe drucken - der Satz DARF ja nicht dastehen. Auf dem
        // Lösungsblatt sind sie dagegen genau richtig: von dort liest ein Elternteil sie vor.
        if (dictations.Count > 0)
        {
            html.Append("<h2>Zum Vorlesen (Diktat)</h2>\n");
            html.Append("<p class=\"note\">Diese Sätze langsam vorlesen und aufschreiben lassen. " +
                        "Auf Satzzeichen kommt es nicht an, auf die Schreibweise der Wörter schon.</p>\n");

            foreach (var dictation in dictations)
            {
                html.Append("<div class=\"solution\"><div class=\"answer\">")
                    .Append(ReportExport.Escape(dictation.Prompt))
                    .Append("</div><div class=\"explain\">")
                    .Append(ReportExport.Escape(dictation.Explanation))
                    .Append("</div></div>\n");
            }
        }

        html.Append("<p class=\"footer\">Erstellt mit LernTor.</p>\n");
        html.Append("</body>\n</html>\n");

        return html.ToString();
    }

    private static void AppendTask(
        StringBuilder html, QuizQuestion question, IReadOnlyList<string> options, int number)
    {
        html.Append("<div class=\"task\">\n  <div class=\"prompt\"><span class=\"num\">")
            .Append(number).Append(".</span> ")
            .Append(ReportExport.Escape(question.Prompt))
            .Append("</div>\n");

        if (options.Count > 0)
        {
            html.Append("  <ol class=\"options\">\n");
            foreach (var option in options)
            {
                html.Append("    <li>").Append(ReportExport.Escape(option)).Append("</li>\n");
            }

            html.Append("  </ol>\n");
        }
        else
        {
            // Zwei Schreiblinien für offene Aufgaben - bei einer Rechnung braucht es Platz für
            // den Rechenweg, nicht nur für das Ergebnis.
            html.Append("  <div class=\"writeline\"></div>\n  <div class=\"writeline\"></div>\n");
        }

        html.Append("</div>\n");
    }

    private static void AppendSolution(
        StringBuilder html, QuizQuestion question, IReadOnlyList<string> options, int number)
    {
        var answer = question.CorrectAnswers.Count > 0
            ? string.Join(" / ", question.CorrectAnswers)
            : "-";

        // Bei Multiple Choice zusätzlich der Buchstabe - "B) 42" ist beim Vergleichen schneller
        // als das bloße Wiederfinden des Antworttextes in der Liste.
        var letter = string.Empty;
        if (options.Count > 0)
        {
            var index = options
                .Select((option, i) => (option, i))
                .Where(pair => question.CorrectAnswers.Contains(pair.option))
                .Select(pair => pair.i)
                .FirstOrDefault(-1);

            if (index >= 0 && index < OptionLetters.Length)
            {
                letter = $"{OptionLetters[index]}) ";
            }
        }

        html.Append("<div class=\"solution\">\n  <div class=\"answer\"><span class=\"num\">")
            .Append(number).Append(".</span> ")
            .Append(ReportExport.Escape(letter))
            .Append(ReportExport.Escape(answer))
            .Append("</div>\n");

        if (!string.IsNullOrWhiteSpace(question.Explanation))
        {
            html.Append("  <div class=\"explain\">").Append(ReportExport.Escape(question.Explanation)).Append("</div>\n");
        }

        html.Append("</div>\n");
    }

    internal static string GradeNumber(GradeLevel grade) => grade switch
    {
        GradeLevel.Klasse6 => "6",
        GradeLevel.Klasse7 => "7",
        GradeLevel.Klasse9 => "9",
        _ => grade.ToString().Replace("Klasse", string.Empty)
    };

    /// <summary>Dateiname-Vorschlag, z.B. <c>Uebungsblatt-Mathematik-2026-08-05.html</c>.</summary>
    public static string SuggestFileName(string? subjectLabel, DateOnly today)
    {
        var cleaned = new StringBuilder();
        foreach (var c in (subjectLabel ?? string.Empty).Trim())
        {
            cleaned.Append(char.IsLetterOrDigit(c) || c is '-' or '_' ? c : '-');
        }

        var name = cleaned.ToString().Trim('-');
        while (name.Contains("--", StringComparison.Ordinal))
        {
            name = name.Replace("--", "-", StringComparison.Ordinal);
        }

        var namePart = string.IsNullOrEmpty(name) ? string.Empty : $"-{name}";

        return $"Uebungsblatt{namePart}-{today:yyyy-MM-dd}.html";
    }

    private const string Css = """
        body { font-family: Segoe UI, Arial, sans-serif; margin: 28px auto; max-width: 740px;
               color: #14181f; line-height: 1.5; }
        h1 { font-size: 22px; margin: 0 0 4px; }
        h2 { font-size: 17px; margin: 24px 0 6px; }
        .meta { color: #5a6270; margin: 0 0 4px; font-size: 14px; }
        .namefield { margin: 0 0 22px; font-size: 15px; }
        .task { margin: 0 0 20px; break-inside: avoid; }
        .prompt { font-size: 15px; }
        .num { font-weight: 700; margin-right: 6px; }
        .options { margin: 6px 0 0 26px; padding: 0; }
        .options li { margin: 3px 0; font-size: 15px; }
        .writeline { border-bottom: 1px solid #9aa1ad; height: 24px; margin: 10px 0 0 26px; }
        .solution { margin: 0 0 12px; break-inside: avoid; }
        .answer { font-size: 15px; font-weight: 600; }
        .explain { font-size: 13px; color: #4a515e; margin: 2px 0 0 26px; }
        .note { font-size: 13px; color: #4a515e; }
        .empty { font-size: 15px; color: #5a6270; }
        .footer { margin-top: 26px; font-size: 12px; color: #79808d; }
        /* Der harte Umbruch ist der Kern des Blattes: Loesungen gehoeren auf eine eigene Seite. */
        .pagebreak { break-before: page; page-break-before: always; height: 0; }
        @media print { body { margin: 0; max-width: none; } }
        """;
}
