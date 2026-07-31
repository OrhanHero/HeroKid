using LernTor.Core.Enums;

namespace LernTor.Core.Services;

/// <summary>Ein Diktatsatz samt der Regel, um die es darin geht.</summary>
/// <param name="Sentence">Der zu diktierende Satz (so, wie er richtig geschrieben aussieht).</param>
/// <param name="Rule">Die Rechtschreibregel - wird nach der Auswertung als Erklärung gezeigt.</param>
/// <param name="Hint">Kurzer Hinweis, den das Kind sich VOR dem Schreiben holen darf.</param>
public readonly record struct DictationSentence(string Sentence, string Rule, string Hint);

/// <summary>
/// Die Sätze für das Diktat.
///
/// <para>Jeder Satz ist auf <b>eine</b> Rechtschreibfalle gebaut - das/dass, ss/ß, Dehnungs-h,
/// Groß- und Kleinschreibung nach Artikel, Doppelkonsonanten. Beliebige Sätze zu diktieren wäre
/// Tipptraining, kein Rechtschreibtraining; der Tipptrainer existiert dafür schon separat.</para>
///
/// <para>Die Sätze bleiben kurz (6-12 Wörter). Ein langer Text wäre für ein Diktat am Bildschirm
/// zermürbend: das Kind kann nicht zwischendurch nachschauen, und ein einziger Fehler in Wort 40
/// entwertet die ganze Arbeit.</para>
/// </summary>
public static class DictationContentProvider
{
    /// <summary>Sätze für die jüngere Stufe: Grundregeln, kurze Sätze, Alltagswortschatz.</summary>
    private static readonly DictationSentence[] Klasse6 =
    {
        new("Der Hund bellt laut im Garten.",
            "Nomen (Hund, Garten) schreibt man groß - erkennbar am Artikel davor: der Hund, im Garten.",
            "Wörter mit der/die/das oder im/am davor sind Nomen und werden großgeschrieben."),
        new("Wir wissen, dass der Zug heute später kommt.",
            "\"dass\" mit Doppel-s leitet einen Nebensatz ein. Man kann es nicht durch dieses/jenes/welches ersetzen.",
            "Probier \"dieses\" statt des Wortes: passt es nicht, schreibt man dass."),
        new("Das Buch, das auf dem Tisch liegt, gehört mir.",
            "Hier ist \"das\" ein Artikel bzw. Relativpronomen - man kann dieses/jenes/welches einsetzen, deshalb nur ein s.",
            "Passt \"welches\" an die Stelle, schreibt man das mit einem s."),
        new("Im Sommer essen wir gern ein großes Eis.",
            "Nach langem Vokal steht ß (großes), nach kurzem ss (essen).",
            "Sprich das Wort langsam: langer Vokal davor → ß, kurzer → ss."),
        new("Die Straße war nass vom Regen.",
            "Straße mit ß (langes a), nass mit ss (kurzes a).",
            "Langer Selbstlaut vor dem Zischlaut → ß, kurzer → ss."),
        new("Mein Bruder fährt jeden Tag mit dem Fahrrad zur Schule.",
            "Das Dehnungs-h in fährt/Fahrrad hört man nicht - es zeigt an, dass der Vokal lang gesprochen wird.",
            "Bei lang gesprochenem Vokal steht oft ein stummes h dahinter."),
        new("Der Lehrer erklärt die Aufgabe noch einmal.",
            "Lehrer und erklärt haben beide ein Dehnungs-h nach dem langen e bzw. ä.",
            "Höre auf den langen Vokal - dort steckt häufig ein h."),
        new("Am Wochenende spielen wir zusammen Fußball.",
            "Wochenende und Fußball sind zusammengesetzte Nomen und werden großgeschrieben; Fußball mit ß.",
            "Zusammengesetzte Nomen bleiben ein Wort und werden großgeschrieben."),
        new("Sie hat ihre Hausaufgaben schon gemacht.",
            "\"ihre\" wird kleingeschrieben - es ist ein Begleiter, kein Nomen. Hausaufgaben dagegen groß.",
            "Nur Nomen groß; Begleiter wie mein, dein, ihre bleiben klein."),
        new("Der kleine Junge lief schnell über die Wiese.",
            "kleine und schnell sind Adjektive und bleiben klein, Junge und Wiese sind Nomen.",
            "Wie-Wörter (Adjektive) bleiben klein, auch wenn sie vor einem Nomen stehen."),
        new("Wir treffen uns morgen früh am Bahnhof.",
            "morgen (Zeitangabe) klein, Bahnhof als Nomen groß; früh mit Dehnungs-h.",
            "Zeitangaben wie morgen und heute bleiben klein."),
        new("Das Wetter ist heute besser als gestern.",
            "besser mit Doppel-s nach kurzem e; als vergleicht und hat nur ein s.",
            "Kurzer Vokal davor bedeutet meist doppelter Konsonant."),
        new("Der Vater kocht Suppe für die ganze Familie.",
            "Vater, Suppe und Familie sind Nomen; Suppe mit Doppel-p nach kurzem u.",
            "Nach kurzem Vokal verdoppelt sich der folgende Konsonant."),
        new("Meine Schwester liest jeden Abend ein spannendes Buch.",
            "liest mit ie (langes i), spannendes mit Doppel-n nach kurzem a.",
            "Langes i schreibt man im Deutschen meist ie."),
        new("Die Kinder freuen sich auf die Ferien.",
            "Kinder und Ferien groß, freuen klein; eu und ie richtig unterscheiden.",
            "Achte auf die Zwielaute eu und ei sowie auf ie."),
        new("Wir haben viel Spaß beim Schwimmen gehabt.",
            "Spaß mit ß, Schwimmen als Nomen (nach \"beim\") groß mit Doppel-m.",
            "Nach beim, zum, im wird ein Verb zum Nomen und großgeschrieben."),
        new("Der Ball rollte langsam den Hügel hinunter.",
            "Ball und Hügel groß, langsam als Adjektiv klein; Ball mit Doppel-l.",
            "Kurzer Vokal am Wortende → doppelter Mitlaut."),
        new("Ich möchte wissen, wann der Film beginnt.",
            "wissen mit Doppel-s (kurzes i), beginnt mit Doppel-n.",
            "Zwei kurze Vokale, zwei doppelte Mitlaute - sprich langsam mit."),
        new("Unsere Katze schläft gern auf dem warmen Sofa.",
            "Katze und Sofa groß, schläft mit ä (von Schlaf abgeleitet).",
            "Bei ä hilft das verwandte Wort: Schlaf → schläft."),
        new("Die Blätter fallen im Herbst von den Bäumen.",
            "Blätter von Blatt, Bäumen von Baum - deshalb ä und äu, nicht e und eu.",
            "Suche das Grundwort: steht dort a oder au, schreibt man ä oder äu.")
    };

    /// <summary>Sätze für die ältere Stufe: längere Satzbauten, Fremdwörter, Getrennt-/Zusammenschreibung.</summary>
    private static readonly DictationSentence[] Klasse9 =
    {
        new("Obwohl es stark regnete, gingen wir spazieren.",
            "Nach dem eingeschobenen Nebensatz steht ein Komma; spazieren gehen wird getrennt geschrieben.",
            "Nebensätze mit obwohl, weil, dass werden mit Komma abgetrennt."),
        new("Der Wissenschaftler erklärte das Experiment sehr ausführlich.",
            "Wissenschaftler mit Doppel-s (kurzes i), ausführlich mit Dehnungs-h.",
            "Achte bei Fachwörtern besonders auf Doppelkonsonanten."),
        new("Sie hat sich entschieden, das Praktikum in Berlin zu machen.",
            "entschieden mit ie, Praktikum als Fremdwort mit k; Infinitivgruppe mit Komma.",
            "Fremdwörter aus dem Lateinischen behalten oft ihr k statt ck."),
        new("Die Demonstration verlief trotz aller Befürchtungen friedlich.",
            "Demonstration und Befürchtungen sind Nomen; friedlich klein mit ie.",
            "Nomen erkennt man am Artikel oder daran, dass man einen einsetzen könnte."),
        new("Wir müssen dringend darüber sprechen, was als Nächstes passiert.",
            "müssen mit Doppel-s, Nächstes hier großgeschrieben (substantiviertes Adjektiv).",
            "Nach als, etwas, nichts wird ein Adjektiv oft zum Nomen und groß geschrieben."),
        new("Der Vertrag wurde gestern von beiden Seiten unterschrieben.",
            "Vertrag und Seiten groß, unterschrieben mit ie; Passivform ohne Trennung.",
            "Vorsilben wie unter- bleiben am Verb, es wird nicht getrennt."),
        new("Ihre Argumente waren überzeugend und gut belegt.",
            "Argumente groß, überzeugend und belegt klein; überzeugend mit eu.",
            "Partizipien wie überzeugend bleiben klein, wenn sie wie Adjektive stehen."),
        new("Das Gerät funktioniert nur, wenn man die Anleitung genau befolgt.",
            "Gerät und Anleitung groß, funktioniert mit k; Nebensatz mit Komma.",
            "Wenn-Sätze werden immer mit Komma abgetrennt."),
        new("Er behauptete, dass er nichts davon gewusst habe.",
            "dass mit Doppel-s als Konjunktion, gewusst mit Doppel-s nach kurzem u.",
            "Nach behaupten, sagen, wissen folgt fast immer ein dass-Satz."),
        new("Die Bevölkerung wuchs in den letzten Jahrzehnten erheblich.",
            "Bevölkerung und Jahrzehnten groß, wuchs mit ch, erheblich mit Dehnungs-h.",
            "Zusammengesetzte Zeitangaben wie Jahrzehnt sind Nomen."),
        new("Trotz des schlechten Wetters fand das Fest im Freien statt.",
            "im Freien großgeschrieben (substantiviert), stattfinden hier getrennt: fand statt.",
            "Trennbare Verben stehen im Hauptsatz auseinander: fand ... statt."),
        new("Der Verdächtige konnte seine Unschuld schließlich beweisen.",
            "Verdächtige und Unschuld groß, schließlich mit ß, beweisen mit ei.",
            "Substantivierte Adjektive (der Verdächtige) werden großgeschrieben."),
        new("Wir haben uns lange darüber unterhalten, wie es weitergehen soll.",
            "unterhalten und weitergehen zusammen, dazwischen Komma vor dem Nebensatz.",
            "weitergehen ist ein zusammengesetztes Verb und bleibt ein Wort."),
        new("Die Maßnahmen zeigten erst nach mehreren Wochen Wirkung.",
            "Maßnahmen mit ß, Wochen und Wirkung groß; mehreren klein.",
            "Nach langem a folgt ß, nicht ss."),
        new("Er interessiert sich besonders für Geschichte und Politik.",
            "interessiert mit Doppel-s in der Mitte, Geschichte und Politik groß.",
            "Fremdwörter auf -ieren behalten das Doppel-s aus dem Stamm."),
        new("Das Ergebnis der Untersuchung war eindeutig und überraschend.",
            "Ergebnis mit einem s am Ende, Untersuchung groß, überraschend klein.",
            "Auf -nis endende Nomen haben nur ein s im Singular."),
        new("Sie hat versprochen, uns rechtzeitig Bescheid zu geben.",
            "Bescheid geben: Bescheid ist ein Nomen und wird großgeschrieben.",
            "In festen Wendungen bleibt das Nomen ein Nomen: Bescheid geben."),
        new("Der Zusammenhang zwischen beiden Ereignissen ist offensichtlich.",
            "Zusammenhang und Ereignissen groß, Ereignissen mit Doppel-s.",
            "Nach kurzem i steht das doppelte s im Plural."),
        new("Wir sollten uns darauf einigen, wer die Verantwortung übernimmt.",
            "einigen klein, Verantwortung groß, übernimmt mit Doppel-m.",
            "Verantwortung ist ein Nomen auf -ung und immer groß."),
        new("Trotz mehrmaligen Nachfragens erhielt er keine Antwort.",
            "Nachfragens hier großgeschrieben (substantivierter Infinitiv im Genitiv), erhielt mit ie.",
            "Ein Verb mit Artikel oder Beugung davor wird zum Nomen.")
    };

    /// <summary>Die Sätze der Stufe. Klasse 7/8 nutzen den Klasse-6-Pool, Klasse 10 den der 9 -
    /// dieselbe Doppeljahrgangs-Regel wie bei den Fachgeneratoren.</summary>
    public static IReadOnlyList<DictationSentence> ForGrade(GradeLevel grade) =>
        grade >= GradeLevel.Klasse9 ? Klasse9 : Klasse6;
}
