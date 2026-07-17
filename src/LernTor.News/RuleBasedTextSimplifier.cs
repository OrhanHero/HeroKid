using System.Text.RegularExpressions;
using LernTor.Core.Enums;

namespace LernTor.News;

/// <summary>
/// Einfache regelbasierte Vereinfachung: HTML entfernen, auf kurze Sätze kürzen,
/// komplexe Wörter durch kindgerechtere Begriffe ersetzen.
/// Zwei Stufen: Klasse 6 (stark vereinfacht) vs. Klasse 9 (mild vereinfacht).
/// Kein Ersatz für echtes NLP, aber lauffähig ohne externe Abhängigkeiten.
/// </summary>
public sealed partial class RuleBasedTextSimplifier : ITextSimplifier
{
    // Klasse 6: sehr kurz, sehr einfach
    private const int MaxSentencesK6 = 2;
    private const int MaxTotalLengthK6 = 300;

    // Klasse 9: etwas länger, normale Sätze ok
    private const int MaxSentencesK9 = 4;
    private const int MaxTotalLengthK9 = 700;

    // Vokabular-Ersetzungen: Klasse 6 = alle, Klasse 9 = nur die schwierigsten
    private static readonly Dictionary<string, string> VereinfachungenK6 = new(StringComparer.OrdinalIgnoreCase)
    {
        ["Koalition"] = "Bündnis mehrerer Parteien",
        ["Verhandlungen"] = "Gespräche",
        ["Diplomaten"] = "Vertreter der Regierung",
        ["Infrastruktur"] = "Straßen, Brücken und Gebäude",
        ["Bevölkerung"] = "Menschen, die dort leben",
        ["signifikant"] = "deutlich",
        ["implementieren"] = "einführen",
        ["Ministerium"] = "Behörde der Regierung",
        ["Investitionen"] = "Geld, das ausgegeben wird",
        ["Subventionen"] = "Zuschüsse vom Staat",
        ["Inflation"] = "Preise steigen",
        ["Rezession"] = "Wirtschaft schrumpft",
        ["Bundesregierung"] = "Regierung von Deutschland",
        ["Landtag"] = "Parlament im Bundesland",
        ["Bundestag"] = "Parlament in Berlin",
        ["Abgeordnete"] = "Politiker im Parlament",
        ["Fraktion"] = "Gruppe von Abgeordneten",
        ["Klimaschutz"] = "Schutz für das Klima",
        ["Erneuerbare Energien"] = "Wind- und Solarstrom",
        ["Kohleausstieg"] = "Ende der Kohleverstromung",
        ["Wasserstoff"] = "sauberer Energieträger",
        ["Digitalisierung"] = "Alles wird digital",
        ["Algorithmus"] = "Rechenvorschrift",
        ["Künstliche Intelligenz"] = "Computer, die lernen",
        ["Datenschutz"] = "Schutz deiner Daten",
        ["Grundgesetz"] = "Wichtigste Gesetze in Deutschland",
        ["Verfassungsschutz"] = "Behörde für Demokratieschutz",
        ["Bundespolizei"] = "Polizei des Bundes",
        ["Europäische Union"] = "EU (Staatenbund in Europa)",
        ["NATO"] = "Verteidigungsbündnis",
        ["UNO"] = "Vereinte Nationen",
        ["Klimawandel"] = "Erde wird wärmer",
        ["Treibhausgase"] = "Gase, die die Erde aufheizen",
        ["CO2"] = "Kohlenstoffdioxid",
        ["Emissionshandel"] = "Handel mit Verschmutzungsrechten",
        ["Lieferketten"] = "Wege, wie Produkte zu uns kommen",
        ["Globalisierung"] = "Welt wächst zusammen",
        ["Migration"] = "Menschen ziehen um",
        ["Integration"] = "Einleben in neues Land",
        ["Asyl"] = "Schutz vor Verfolgung",
        ["Flüchtling"] = "Mensch auf der Flucht",
        ["Deportation"] = "Abschiebung",
        ["Abschiebung"] = "Zurückschicken in Heimatland",
        ["Wahlkampf"] = "Zeit vor der Wahl",
        ["Koalitionsvertrag"] = "Vertrag zwischen Regierungsparteien",
        ["Haushalt"] = "Plan für Einnahmen und Ausgaben",
        ["Schuldenbremse"] = "Regel gegen zu viele Schulden",
        ["Steuererhöhung"] = "Mehr Steuern zahlen",
        ["Steuersenkung"] = "Weniger Steuern zahlen",
        ["Mindestlohn"] = "Geringster erlaubter Lohn",
        ["Rente"] = "Geld im Alter",
        ["Krankenkasse"] = "Versicherung für Gesundheit",
        ["Pflegeversicherung"] = "Versicherung für Pflege",
        ["Arbeitslosengeld"] = "Geld bei Arbeitslosigkeit",
        ["Bürgergeld"] = "Grundsicherung für Erwachsene",
        ["Kindergeld"] = "Geld für Kinder",
        ["Elterngeld"] = "Geld für Eltern nach Geburt",
        ["Kita"] = "Kindertagesstätte",
        ["Ganztagsschule"] = "Schule bis nachmittags",
        ["Inklusion"] = "Alle lernen zusammen",
        ["Förderschule"] = "Schule mit extra Hilfe",
        ["Abitur"] = "Höchster Schulabschluss",
        ["Realschulabschluss"] = "Mittlerer Schulabschluss",
        ["Hauptschulabschluss"] = "Einfacher Schulabschluss",
        ["Berufsausbildung"] = "Lernen im Betrieb",
        ["Studium"] = "Lernen an Uni/Hochschule",
        ["BAföG"] = "Geld fürs Studium",
        ["Wohngeld"] = "Zuschuss zur Miete",
        ["Mietpreisbremse"] = "Deckel für Mieten",
        ["Energiewende"] = "Umstieg auf saubere Energie",
        ["Atomausstieg"] = "Ende der Atomkraft",
        ["Endlager"] = "Lager für Atommüll",
        ["Windrad"] = "Strom aus Wind",
        ["Solarkraftwerk"] = "Strom aus Sonne",
        ["Wasserstoff"] = "Energieträger der Zukunft",
        ["E-Mobilität"] = "Autos mit Strom",
        ["Ladesäule"] = "Tankstelle für E-Autos",
        ["Autonomes Fahren"] = "Auto fährt selbst",
        ["Digitalisierung"] = "Alles wird digital",
        ["5G"] = "Schnelles Handynetz",
        ["Glasfaser"] = "Schnelles Internetkabel",
        ["Cyberangriff"] = "Angriff auf Computer",
        ["Ransomware"] = "Erpressungssoftware",
        ["Datenschutz"] = "Schutz deiner Daten",
        ["Urheberrecht"] = "Recht am eigenen Werk",
        ["Plattformökonomie"] = "Geschäft mit Apps",
        ["Gig-Economy"] = "Jobs per App",
        ["Homeoffice"] = "Arbeit von zu Hause",
        ["Vier-Tage-Woche"] = "Nur 4 Tage arbeiten",
        ["New Work"] = "Neue Arbeitswelten",
        ["Fachkräftemangel"] = "Zu wenige Experten",
        ["Weiterbildung"] = "Neues lernen im Job",
        ["Lebenslanges Lernen"] = "Immer weiter lernen",
    };

    private static readonly Dictionary<string, string> VereinfachungenK9 = new(StringComparer.OrdinalIgnoreCase)
    {
        ["Koalition"] = "Bündnis mehrerer Parteien",
        ["Verhandlungen"] = "Gespräche",
        ["Diplomaten"] = "Vertreter der Regierung",
        ["Infrastruktur"] = "Straßen, Brücken, Netzwerke",
        ["signifikant"] = "deutlich",
        ["implementieren"] = "einführen",
        ["Investitionen"] = "Geldausgaben für Zukunft",
        ["Subventionen"] = "Staatliche Zuschüsse",
        ["Rezession"] = "Wirtschaftliche Schrumpfung",
        ["Bundesregierung"] = "Regierung Deutschlands",
        ["Klimaschutz"] = "Schutz des Klimas",
        ["Kohleausstieg"] = "Ende der Kohleverstromung",
        ["Wasserstoff"] = "Sauberer Energieträger",
        ["Algorithmus"] = "Rechenvorschrift",
        ["Künstliche Intelligenz"] = "Lernende Computer",
        ["Grundgesetz"] = "Verfassung Deutschlands",
        ["Verfassungsschutz"] = "Behörde für Demokratieschutz",
        ["Europäische Union"] = "EU (Europäischer Staatenbund)",
        ["Treibhausgase"] = "Klimaerwärmende Gase",
        ["Emissionshandel"] = "CO2-Zertifikatehandel",
        ["Lieferketten"] = "Produktionswege",
        ["Globalisierung"] = "Weltweite Vernetzung",
        ["Integration"] = "Einleben in Gesellschaft",
        ["Deportation"] = "Zwangsausweisung",
        ["Koalitionsvertrag"] = "Regierungsvereinbarung",
        ["Schuldenbremse"] = "Schuldenregel",
        ["Bürgergeld"] = "Grundsicherung",
        ["Energiewende"] = "Umbau der Energieversorgung",
        ["Atomausstieg"] = "Ende der Kernkraft",
        ["Endlager"] = "Endlager für Atommüll",
        ["E-Mobilität"] = "Elektrofahrzeuge",
        ["Autonomes Fahren"] = "Selbstfahrende Autos",
        ["Cyberangriff"] = "Hackerangriff",
        ["Ransomware"] = "Erpressungs-Trojaner",
        ["Plattformökonomie"] = "App-basierte Geschäftsmodelle",
        ["Gig-Economy"] = "Plattform-Jobs",
        ["Fachkräftemangel"] = "Mangel an Experten",
    };

    [GeneratedRegex("<[^>]+>")]
    private static partial Regex HtmlTagRegex();

    [GeneratedRegex(@"(?<=[.!?])\s+")]
    private static partial Regex SentenceSplitRegex();

    public string Simplify(string rawText, GradeLevel gradeLevel)
    {
        if (string.IsNullOrWhiteSpace(rawText))
        {
            return "Zu diesem Artikel gibt es noch keine Zusammenfassung.";
        }

        var isK6 = gradeLevel == GradeLevel.Klasse6;
        var maxSentences = isK6 ? MaxSentencesK6 : MaxSentencesK9;
        var maxTotalLength = isK6 ? MaxTotalLengthK6 : MaxTotalLengthK9;
        var replacements = isK6 ? VereinfachungenK6 : VereinfachungenK9;

        var withoutHtml = HtmlTagRegex().Replace(rawText, " ").Trim();
        var normalizedWhitespace = Regex.Replace(withoutHtml, @"\s+", " ");

        var sentences = SentenceSplitRegex()
            .Split(normalizedWhitespace)
            .Where(s => !string.IsNullOrWhiteSpace(s))
            .Take(maxSentences)
            .ToList();

        var simplified = string.Join(" ", sentences);

        // Vokabular-Ersetzungen
        foreach (var (complex, simple) in replacements)
        {
            simplified = Regex.Replace(simplified, Regex.Escape(complex), simple, RegexOptions.IgnoreCase);
        }

        // Klasse 6: Zusätzliche Vereinfachung der Satzstruktur
        if (isK6)
        {
            simplified = SimplifySentenceStructure(simplified);
        }

        if (simplified.Length > maxTotalLength)
        {
            simplified = simplified[..maxTotalLength].TrimEnd() + "…";
        }

        return simplified;
    }

    /// <summary>
    /// Klasse 6: Lange Sätze aufteilen, Passiv→Aktiv, Hauptsätze bevorzugen.
    /// Einfache Heuristik: an Kommas in langen Sätzen splitten, Passiv-Konstruktionen auflösen.
    /// </summary>
    private static string SimplifySentenceStructure(string text)
    {
        // Passiv-Konstruktionen auflösen (sehr vereinfacht)
        var passivePatterns = new[]
        {
            (@"wird\s+(\w+)\s+gemacht", "macht man $1"),
            (@"wurde\s+(\w+)\s+gemacht", "machte man $1"),
            (@"ist\s+(\w+)\s+worden", "ist $1 geworden"),
            (@"werden\s+(\w+)\s+gemacht", "macht man $1"),
            (@"kann\s+(\w+)\s+werden", "kann man $1"),
            (@"muss\s+(\w+)\s+werden", "muss man $1"),
            (@"soll\s+(\w+)\s+werden", "soll man $1"),
        };

        foreach (var (pattern, replacement) in passivePatterns)
        {
            text = Regex.Replace(text, pattern, replacement, RegexOptions.IgnoreCase);
        }

        // Sehr lange Sätze (> 25 Wörter) an Kommas aufteilen
        var sentences = SentenceSplitRegex().Split(text);
        var result = new List<string>();

        foreach (var sentence in sentences)
        {
            var words = sentence.Split(' ', StringSplitOptions.RemoveEmptyEntries);
            if (words.Length > 25 && sentence.Contains(','))
            {
                // Am ersten Komma nach der Mitte splitten
                var half = sentence.Length / 2;
                var commaIdx = sentence.IndexOf(',', half);
                if (commaIdx > 0)
                {
                    var part1 = sentence[..commaIdx].Trim() + ".";
                    var part2 = char.ToUpper(sentence[commaIdx + 1]) + sentence[(commaIdx + 2)..].Trim();
                    if (!part2.EndsWith('.')) part2 += ".";
                    result.Add(part1);
                    result.Add(part2);
                    continue;
                }
            }
            result.Add(sentence.Trim());
        }

        return string.Join(" ", result);
    }
}
