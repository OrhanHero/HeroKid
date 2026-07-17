using LernTor.Core.Enums;
using LernTor.Core.Models;

namespace LernTor.News;

/// <summary>
/// Kindgerechte Zusatz-Metadaten je Artikel: Lesedauer, Schwierigkeitsgrad und die Einordnungen
/// "Warum ist das wichtig?" / "Was bedeutet das für dich?".
///
/// <para><b>Ehrlichkeits-Prinzip:</b> Die Einordnungen sind rubrikbezogene, redaktionell
/// formulierte Standardtexte (neutral, ohne Angstmache, ohne politische Wertung - Stil logo!/
/// Checker-Sendungen). Eine regelbasierte Pipeline KANN nicht seriös artikel-spezifische
/// Analysen erfinden; lieber eine ehrliche allgemeine Einordnung als eine falsche konkrete.</para>
/// <para><b>Altersanpassung:</b> Zwei Stufen - Klasse 6 (einfach, direkt, wir-betrifft-mich) vs.
/// Klasse 9 (anspruchsvoller, abstrakter, gesellschaftlicher Kontext).</para>
/// </summary>
public static class KidNewsMetadata
{
    /// <summary>Langsames Kinder-Lesetempo (Wörter pro Minute) als Basis der Lesedauer-Schätzung.</summary>
    private const int WordsPerMinute = 140;

    public static int ComputeReadingMinutes(params string?[] textParts)
    {
        var wordCount = textParts
            .Where(t => !string.IsNullOrWhiteSpace(t))
            .Sum(t => t!.Split(' ', StringSplitOptions.RemoveEmptyEntries).Length);

        return Math.Max(1, (int)Math.Ceiling(wordCount / (double)WordsPerMinute));
    }

    /// <summary>
    /// Heuristik über durchschnittliche Satzlänge und Anteil langer Wörter (≥ 12 Zeichen) -
    /// dieselben Größen, auf denen gängige Lesbarkeitsformeln (z.B. LIX) beruhen.
    /// </summary>
    public static NewsDifficulty ComputeDifficulty(string? text)
    {
        if (string.IsNullOrWhiteSpace(text))
        {
            return NewsDifficulty.Leicht;
        }

        var words = text.Split(' ', StringSplitOptions.RemoveEmptyEntries);
        if (words.Length == 0)
        {
            return NewsDifficulty.Leicht;
        }

        var sentenceCount = Math.Max(1, text.Count(c => c is '.' or '!' or '?'));
        var wordsPerSentence = words.Length / (double)sentenceCount;
        var longWordShare = words.Count(w => w.Trim('.', ',', '!', '?', ':', ';', '"').Length >= 12)
                            / (double)words.Length;

        var score = wordsPerSentence + longWordShare * 100;

        return score switch
        {
            < 18 => NewsDifficulty.Leicht,
            < 28 => NewsDifficulty.Mittel,
            _ => NewsDifficulty.Schwer
        };
    }

    /// <summary>"Warum ist das wichtig?" je Rubrik und Klassenstufe.</summary>
    public static string WhyImportantFor(NewsCategory category, GradeLevel gradeLevel) => (category, gradeLevel) switch
    {
        // Klasse 6: direkt, wir-betrifft-mich, einfach
        (NewsCategory.Berlin, GradeLevel.Klasse6) =>
            "Das passiert direkt in Berlin – also dort, wo du wohnst. Nachrichten aus deiner " +
            "eigenen Stadt betreffen dich oft am meisten: deine Schule, dein Kiez, deine Wege.",

        (NewsCategory.Deutschland, GradeLevel.Klasse6) =>
            "Was in Deutschland entschieden wird, gilt für alle Menschen im Land – auch für dich " +
            "und deine Familie. Wer Bescheid weiß, kann mitreden.",

        (NewsCategory.Welt, GradeLevel.Klasse6) =>
            "Die Welt hängt zusammen: Was in anderen Ländern passiert, wirkt sich oft auch auf " +
            "uns aus – beim Handel, beim Klima oder bei Erfindungen.",

        (NewsCategory.Tuerkei, GradeLevel.Klasse6) =>
            "Viele Familien in Berlin haben Wurzeln in der Türkei. Zu wissen, was dort passiert, " +
            "hilft, beide Länder zu verstehen und mit Verwandten mitzureden.",

        (NewsCategory.Ki, GradeLevel.Klasse6) =>
            "Künstliche Intelligenz verändert gerade, wie wir lernen, arbeiten und spielen. Wer " +
            "früh versteht, wie KI funktioniert, kann sie klug nutzen – wie einen Taschenrechner.",

        (NewsCategory.Spiele, GradeLevel.Klasse6) =>
            "Millionen Kinder und Jugendliche spielen jeden Tag. Zu wissen, was in der " +
            "Spielewelt passiert, gehört heute zur Alltagskultur – wie früher Fernsehen und Musik.",

        (NewsCategory.Finanzen, GradeLevel.Klasse6) =>
            "Geld spielt im Leben aller Menschen eine Rolle – beim Taschengeld genauso wie bei " +
            "großen Firmen. Wer Geld versteht, trifft später bessere Entscheidungen.",

        (NewsCategory.Wetter, GradeLevel.Klasse6) =>
            "Wetter betrifft jeden Tag alle Menschen: Schulweg, Sport, Ausflüge. Und am Wetter " +
            "kann man beobachten, wie sich unser Klima langsam verändert.",

        // Klasse 9: anspruchsvoller, gesellschaftlicher Kontext, abstrakter
        (NewsCategory.Berlin, GradeLevel.Klasse9) =>
            "Berliner Landesentscheidungen prägen deinen Alltag direkt – von Schulbau über " +
            "ÖPNV bis hin zu Jugendangeboten. Wer die lokale Politik versteht, kann sie mitgestalten.",

        (NewsCategory.Deutschland, GradeLevel.Klasse9) =>
            "Bundespolitik setzt den Rahmen für Bildung, Sozialsysteme, Klimaschutz und Wirtschaft. " +
            "Entscheidungen in Berlin wirken sich auf deine Ausbildung, dein späteres Einkommen und " +
            "die gesellschaftliche Stimmung aus – informiert zu sein stärkt deine Handlungsfähigkeit.",

        (NewsCategory.Welt, GradeLevel.Klasse9) =>
            "Globalisierung verknüpft Lieferketten, Finanzmärkte, Migrationsströme und Klimapolitik. " +
            "Ereignisse in anderen Weltregionen beeinflussen Preise, Arbeitsmärkte und politische " +
            "Debatten in Deutschland – systemisches Denken ist gefragt.",

        (NewsCategory.Tuerkei, GradeLevel.Klasse9) =>
            "Die Türkei ist Deutschlands wichtigster Partner bei Migration, Handel und NATO. " +
            "Politische Entwicklungen dort wirken sich direkt auf bilaterale Beziehungen, " +
            "Visafragen und die Situation der großen türkischstämmigen Community in Berlin aus.",

        (NewsCategory.Ki, GradeLevel.Klasse9) =>
            "KI transformiert Bildung, Arbeitsmarkt, Medien und Demokratie (Deepfakes, Desinformation). " +
            "Grundverständnis von LLMs, Bias und Alignment ist künftige Kernkompetenz – analog zu " +
            "Medienkompetenz heute. Kritischer, reflektierter Umgang entscheidet über Chancen.",

        (NewsCategory.Spiele, GradeLevel.Klasse9) =>
            "Gaming ist Milliardenindustrie, Kulturtreiber und sozialer Raum. Monetarisierung " +
            "(Lootboxen, Battle Passes), Datenschutz, eSports-Professionalisierung und " +
            "Altersfreigaben (USK/PEGI) sind regulatorische Themen, die dich als Nutzer:in betreffen.",

        (NewsCategory.Finanzen, GradeLevel.Klasse9) =>
            "Finanzbildung (Zinseszinseffekt, Inflation, ETFs, Krypto-Risiken, Altersvorsorge) " +
            "ist essenzielle Lebenskompetenz. Frühes Verständnis ökonomischer Mechanismen verhindert " +
            "Überschuldung und ermöglicht Vermögensaufbau – Startkapital: Taschengeld.",

        (NewsCategory.Wetter, GradeLevel.Klasse9) =>
            "Wetterextreme (Hitzewellen, Starkregen, Dürren) nehmen durch Klimawandel zu. " +
            "Meteorologische Modelle, Warnsysteme (Cell Broadcast, NINA) und Anpassungsstrategien " +
            "(Schwammstadt, Hitzeschutzpläne) sind Teil moderner Daseinsvorsorge.",

        _ => string.Empty
    };

    /// <summary>"Was bedeutet das für dich?" je Rubrik und Klassenstufe.</summary>
    public static string MeaningForKidsFor(NewsCategory category, GradeLevel gradeLevel) => (category, gradeLevel) switch
    {
        // Klasse 6: konkret, alltagsnah, "du"
        (NewsCategory.Berlin, GradeLevel.Klasse6) =>
            "Vielleicht merkst du davon etwas auf deinem Schulweg, in deinem Bezirk oder bei " +
            "Angeboten für Kinder und Jugendliche in deiner Nähe.",

        (NewsCategory.Deutschland, GradeLevel.Klasse6) =>
            "Solche Entscheidungen können deine Schule, deine Freizeit oder das Geld deiner " +
            "Familie betreffen – manchmal sofort, manchmal erst in ein paar Jahren.",

        (NewsCategory.Welt, GradeLevel.Klasse6) =>
            "Auch wenn es weit weg klingt: Viele Dinge aus aller Welt landen bei uns – als " +
            "Produkte, Ideen oder neue Mitschülerinnen und Mitschüler mit ihren Geschichten.",

        (NewsCategory.Tuerkei, GradeLevel.Klasse6) =>
            "Wenn deine Familie Verbindungen in die Türkei hat, kannst du beim nächsten " +
            "Telefonat oder Besuch mitreden – und allen anderen hilft es, Freunde besser zu verstehen.",

        (NewsCategory.Ki, GradeLevel.Klasse6) =>
            "KI steckt schon jetzt in Handys, Spielen und Hausaufgaben-Helfern. Probiere Neues " +
            "neugierig aus – und bleib kritisch: Nicht alles, was eine KI sagt, stimmt.",

        (NewsCategory.Spiele, GradeLevel.Klasse6) =>
            "Neue Spiele, Updates oder Regeln können genau die Spiele betreffen, die du und " +
            "deine Freunde spielt. Denk dran: Pausen machen und auf Altersfreigaben achten.",

        (NewsCategory.Finanzen, GradeLevel.Klasse6) =>
            "Auch dein Taschengeld ist Wirtschaft im Kleinen: Wenn Preise steigen, bekommst du " +
            "fürs gleiche Geld weniger. Sparen und Vergleichen lohnt sich fast immer.",

        (NewsCategory.Wetter, GradeLevel.Klasse6) =>
            "Schau vor dem Rausgehen auf das Wetter: richtige Kleidung, Sonnenschutz bei Hitze, " +
            "und bei Unwetterwarnungen lieber drinnen bleiben.",

        // Klasse 9: abstrakter, handlungsorientiert, Kompetenz-Fokus
        (NewsCategory.Berlin, GradeLevel.Klasse9) =>
            "Prüfe, ob Bezirksverordnetenversammlungen, Jugendbeteiligungsformate oder " +
            "Schülerhaushalte Themen aufgreifen, die dich betreffen. Lokale Partizipation " +
            "funktioniert nur, wenn junge Menschen sie nutzen.",

        (NewsCategory.Deutschland, GradeLevel.Klasse9) =>
            "Wahlprogramme vergleichen, Petitionen unterstützen, Jugendverbände/Parteijugenden " +
            "beitreten – demokratische Teilhabe beginnt vor der Wahl. Entscheidungen zu " +
            "BAföG-Reform, Mietrecht, Klimagesetzgebung betreffen deine Generation überproportional.",

        (NewsCategory.Welt, GradeLevel.Klasse9) =>
            "Internationale Abkommen (Paris, UN-Kinderrechte, WTO) prägen nationale Gesetze. " +
            "Entwicklungszusammenarbeit, Menschenrechte, Lieferkettengesetz – dein Konsumverhalten " +
            "und politisches Engagement haben globale Rückkopplungseffekte.",

        (NewsCategory.Tuerkei, GradeLevel.Klasse9) =>
            "Beobachte Menschenrechtslage, Pressefreiheit, Wirtschaftskurs (Zinsen, Inflation) und " +
            "EU-Beitrittsperspektive. Für Berlin relevante Themen: Familiennachzug, " +
            "Anerkennung türkischer Berufsabschlüsse, Doppelstaatsbürgerschaftsreform.",

        (NewsCategory.Ki, GradeLevel.Klasse9) =>
            "Teste KI-Tools (Copilot, Perplexity, lokale LLMs) bewusst für Schule/Projekte. " +
            "Lerne Prompting, evaluiere Halluzinationen, nutze Open-Source-Alternativen. " +
            "Verstehe: KI = Werkzeug, kein Orakel. Urheberrecht, Datenschutz, Bias bleiben deine Verantwortung.",

        (NewsCategory.Spiele, GradeLevel.Klasse9) =>
            "Analysiere Geschäftsmodelle (F2P, GaaS, Mikrotransaktionen), prüfe Datenschutz " +
            "(DSGVO, Telemetrie), nutze Jugendschutz-Tools (Bildschirmzeit, Käufe sperren). " +
            "eSports als Karrierepfad: Training, Sponsoring, Streaming – aber realistisch bleiben.",

        (NewsCategory.Finanzen, GradeLevel.Klasse9) =>
            "Eröffne Jugendkonto/Tagesgeld, verstehe Zinseszins, Inflation, ETF-Kostenquote (TER). " +
            "Simuliere Altersvorsorge (Riester/Rürup/ETF-Sparplan) – Zeit ist dein größter Hebel. " +
            "Vorsicht bei Krypto, CFDs, 'schnell reich'-Versprechen: hohes Verlustrisiko.",

        (NewsCategory.Wetter, GradeLevel.Klasse9) =>
            "Nutze Warn-Apps (NINA, DWD WarnWetter), verstehe Warnstufen (gelb/orange/rot). " +
            "Hitzeschutz: Trinken, Schatten, Mittagshitze meiden. Starkregen: Keller sichern, " +
            "Rückstausicherung prüfen. Klimafolgenanpassung ist kommunale Pflichtaufgabe – " +
            "fordere sie bei deiner Bezirksverordnetenversammlung ein.",

        _ => string.Empty
    };
}
