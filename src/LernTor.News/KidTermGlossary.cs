using System.Text.RegularExpressions;
using LernTor.Core.Models;

namespace LernTor.News;

/// <summary>
/// Kindgerechtes Begriffs-Glossar für den News-Bereich: erkennt schwierige Wörter in
/// Titel+Zusammenfassung eines Artikels und liefert sofort eine einfache Erklärung dazu
/// ("ein schwieriges Wort = sofort erklären", Stil orientiert an logo!/Checker-Sendungen).
/// Anders als die Ersetzungen im <see cref="RuleBasedTextSimplifier"/> wird der Originaltext
/// dabei NICHT verändert - das Kind sieht das echte Wort UND lernt, was es bedeutet.
/// Kuratierte Liste statt automatischer Erkennung: nur so sind die Erklärungen garantiert
/// richtig, neutral und ohne Angstmache formuliert.
/// </summary>
public static class KidTermGlossary
{
    private const int MaxTermsPerArticle = 4;

    /// <summary>Begriff → einfache Erklärung. Erklärungen in kurzen Sätzen, ohne Wertung.</summary>
    internal static readonly IReadOnlyDictionary<string, string> Terms =
        new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
    {
        // Politik & Gesellschaft
        ["Koalition"] = "Ein Bündnis: Mehrere Parteien regieren zusammen, weil keine allein genug Stimmen hat.",
        ["Opposition"] = "Die Parteien im Parlament, die gerade nicht regieren. Sie kontrollieren die Regierung.",
        ["Bundestag"] = "Das Parlament von Deutschland. Hier beschließen gewählte Abgeordnete die Gesetze.",
        ["Bundesrat"] = "Die Vertretung der 16 Bundesländer. Sie redet bei vielen Gesetzen mit.",
        ["Bundeskanzler"] = "Der Chef oder die Chefin der deutschen Regierung.",
        ["Ministerium"] = "Eine große Behörde der Regierung, die sich um ein Thema kümmert - zum Beispiel Schule oder Gesundheit.",
        ["Senat"] = "So heißt die Regierung von Berlin. Berlin ist Stadt und Bundesland zugleich.",
        ["Demokratie"] = "Eine Staatsform, in der die Menschen ihre Regierung selbst wählen und frei ihre Meinung sagen dürfen.",
        ["Wahlkampf"] = "Die Zeit vor einer Wahl, in der Parteien um Stimmen werben.",
        ["Gesetzentwurf"] = "Der Vorschlag für ein neues Gesetz - er muss erst noch beschlossen werden.",
        ["Referendum"] = "Eine Abstimmung, bei der die Bürger direkt über eine Frage entscheiden.",
        ["Verfassung"] = "Das wichtigste Regelwerk eines Landes. In Deutschland heißt es Grundgesetz.",
        ["Grundgesetz"] = "Die Verfassung von Deutschland - das wichtigste Regelwerk mit den Grundrechten aller Menschen.",
        ["Bürokratie"] = "Viele Regeln, Formulare und Ämter. Manchmal dauert dadurch alles länger.",
        ["Demonstration"] = "Viele Menschen gehen gemeinsam auf die Straße, um friedlich ihre Meinung zu zeigen.",
        ["Streik"] = "Beschäftigte legen die Arbeit nieder, um bessere Bezahlung oder Arbeitsbedingungen zu erreichen.",
        ["Tarifvertrag"] = "Ein Vertrag zwischen Arbeitgebern und Gewerkschaften über Löhne und Arbeitszeiten.",
        ["Gewerkschaft"] = "Ein Zusammenschluss von Beschäftigten, der sich für ihre Rechte einsetzt.",
        ["Asyl"] = "Schutz für Menschen, die in ihrem Heimatland verfolgt werden.",
        ["Integration"] = "Wenn Menschen, die neu in ein Land kommen, Teil der Gemeinschaft werden - z.B. durch Sprache und Schule.",
        ["Kommune"] = "Eine Stadt oder Gemeinde mit eigener Verwaltung.",
        ["Sanktionen"] = "Strafmaßnahmen von Ländern gegen ein anderes Land, z.B. Handelsverbote.",
        ["Vereinte Nationen"] = "Ein Zusammenschluss von fast allen Ländern der Welt, der sich für Frieden einsetzt (kurz: UNO).",
        ["NATO"] = "Ein Bündnis vieler Länder, die sich gegenseitig militärisch schützen.",
        ["Europäische Union"] = "Ein Zusammenschluss von 27 europäischen Ländern, die eng zusammenarbeiten (kurz: EU).",

        // Wirtschaft & Finanzen
        ["Inflation"] = "Wenn Dinge im Laden immer teurer werden. Für das gleiche Geld bekommt man dann weniger.",
        ["Börse"] = "Ein Marktplatz, auf dem Anteile von Firmen (Aktien) gekauft und verkauft werden.",
        ["Aktie"] = "Ein kleiner Anteil an einer Firma. Wem Aktien gehören, dem gehört ein Stückchen der Firma.",
        ["Zinsen"] = "Der Preis für geliehenes Geld. Wer spart, bekommt Zinsen dazu; wer sich Geld leiht, zahlt Zinsen drauf.",
        ["Leitzins"] = "Der wichtigste Zins, den die Zentralbank festlegt. Er beeinflusst, wie teuer Kredite für alle sind.",
        ["Rezession"] = "Wenn die Wirtschaft eines Landes eine Zeit lang schrumpft statt wächst.",
        ["Subventionen"] = "Geld vom Staat, um etwas gezielt zu unterstützen - z.B. günstigere Bus-Tickets.",
        ["Mindestlohn"] = "Der niedrigste Stundenlohn, der in Deutschland erlaubt ist.",
        ["Investition"] = "Geld ausgeben, damit später etwas Besseres entsteht - z.B. eine neue Schule bauen.",
        ["Haushaltsplan"] = "Der Plan einer Regierung, wofür sie ihr Geld im Jahr ausgibt.",
        ["Etat"] = "Ein anderes Wort für das Geld, das für einen Zweck eingeplant ist.",
        ["Kryptowährung"] = "Digitales Geld, das nur im Internet existiert - zum Beispiel Bitcoin.",

        // Technik & KI
        ["Künstliche Intelligenz"] = "Computerprogramme, die Dinge können, für die Menschen ihr Gehirn brauchen - z.B. Texte schreiben oder Bilder erkennen.",
        ["Algorithmus"] = "Eine Rechenvorschrift: Schritt-für-Schritt-Anweisungen, nach denen ein Computer arbeitet.",
        ["Chatbot"] = "Ein Programm, mit dem man sich schreibend unterhalten kann - wie der KI-Helfer in dieser App.",
        ["Sprachmodell"] = "Eine KI, die mit sehr vielen Texten trainiert wurde und deshalb selbst Texte schreiben kann.",
        ["Deepfake"] = "Ein mit KI gefälschtes Bild oder Video, das täuschend echt aussieht. Nicht alles im Internet ist echt!",
        ["Datenschutz"] = "Regeln, die deine persönlichen Daten (Name, Fotos, Adresse) vor Missbrauch schützen.",
        ["Software-Update"] = "Eine neue, verbesserte Version eines Programms.",

        // Wissenschaft, Umwelt & Gesundheit
        ["Klimawandel"] = "Die Erde wird durch Abgase langsam wärmer. Deshalb sparen viele Länder jetzt Energie und bauen Windräder und Solaranlagen.",
        ["CO2"] = "Ein unsichtbares Gas, das beim Verbrennen von Kohle, Öl und Benzin entsteht und die Erde aufheizt.",
        ["Emissionen"] = "Abgase und Schadstoffe, die in die Luft gelangen.",
        ["erneuerbare Energien"] = "Strom aus Sonne, Wind und Wasser - er verbraucht keine Rohstoffe und macht kaum Abgase.",
        ["Photovoltaik"] = "Technik, die aus Sonnenlicht Strom macht - die dunklen Platten auf vielen Dächern.",
        ["Biodiversität"] = "Die Vielfalt von Tieren und Pflanzen. Je mehr Arten, desto gesünder ist die Natur.",
        ["Pandemie"] = "Wenn sich eine Krankheit über viele Länder der Welt ausbreitet.",
        ["Impfung"] = "Ein Pikser, der den Körper trainiert, eine Krankheit abzuwehren, bevor man sie bekommt.",
        ["Studie"] = "Eine wissenschaftliche Untersuchung, bei der Forscher etwas genau prüfen.",

        // Wetter
        ["Unwetterwarnung"] = "Der Wetterdienst sagt: Es kommt gefährliches Wetter. Dann besser drinnen bleiben.",
        ["Orkan"] = "Ein extrem starker Sturm mit mehr als 117 km/h Windgeschwindigkeit.",
        ["Dürre"] = "Eine lange Zeit ohne Regen, in der Felder und Pflanzen vertrocknen.",
        ["Starkregen"] = "Sehr viel Regen in kurzer Zeit - Straßen und Keller können dann volllaufen.",
    };

    /// <summary>
    /// Findet die ersten (maximal <see cref="MaxTermsPerArticle"/>) Glossar-Begriffe im Text,
    /// in der Reihenfolge ihres Auftretens. Gematcht wird am Wortanfang, aber bewusst nicht bis
    /// zum Wortende: Deutsch bildet zusammengesetzte Wörter ("Inflationsrate", "Etatentwurf"),
    /// die dieselbe Erklärung verdienen - mitten im Wort ("Nation" in "International") greift
    /// der Treffer dagegen nicht.
    /// </summary>
    public static IReadOnlyList<ExplainedTerm> FindTerms(string? text)
    {
        if (string.IsNullOrWhiteSpace(text))
        {
            return Array.Empty<ExplainedTerm>();
        }

        var found = new List<(int Position, ExplainedTerm Term)>();

        foreach (var (term, explanation) in Terms)
        {
            var match = Regex.Match(text, $@"\b{Regex.Escape(term)}", RegexOptions.IgnoreCase);
            if (match.Success)
            {
                found.Add((match.Index, new ExplainedTerm(term, explanation)));
            }
        }

        return found
            .OrderBy(f => f.Position)
            .Take(MaxTermsPerArticle)
            .Select(f => f.Term)
            .ToList();
    }
}
