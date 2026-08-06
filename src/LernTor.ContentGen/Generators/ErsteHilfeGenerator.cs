using LernTor.Core.Enums;
using LernTor.Core.Models;

namespace LernTor.ContentGen.Generators;

/// <summary>
/// Erste Hilfe und Notfallwissen.
///
/// <para><b>Ein einziger Fragenpool für alle Klassenstufen</b> - anders als bei den
/// Schulfächern. Erste-Hilfe-Wissen hat keine Klasse-6-Fassung: die fünf W-Fragen sind mit elf
/// dieselben wie mit fünfzehn, und die Drucktiefe bei der Herzdruckmassage ändert sich nicht mit
/// dem Alter dessen, der sie anwendet. "Prüfen - Rufen - Drücken" wird an deutschen Schulen ab
/// Klasse 7 unterrichtet, mit denselben Zahlen wie bei Erwachsenen. Deshalb zeigt
/// <see cref="TopicsByGrade"/> für jede Stufe auf dieselbe Liste.</para>
///
/// <para><b>Trotzdem acht getrennte Themen statt eines Blocks:</b> die adaptive
/// Übungsauswahl (<c>AdaptiveTopicWeighting</c>) und die Themen-Heatmap im Eltern-Bericht
/// arbeiten je Topic. Ein einziger großer Topf würde beiden die Auflösung nehmen.</para>
///
/// <para><b>Zur Genauigkeit:</b> die Zahlen folgen den in Deutschland gelehrten Erste-Hilfe-
/// Leitlinien (Notruf 112, Herzdruckmassage 5-6 cm tief, 100-120 pro Minute, 30:2). Wo eine
/// Maßnahme Grenzen hat - Schocklage NICHT bei Kopf-, Brust- oder Atemproblemen -, steht das
/// ausdrücklich in einer eigenen Frage: halbgelerntes Erste-Hilfe-Wissen ist genau dort
/// gefährlich, wo es sich vollständig anfühlt.</para>
/// </summary>
public sealed class ErsteHilfeGenerator : ExerciseGeneratorBase
{
    public override Subject Subject => Subject.ErsteHilfe;

    private static readonly IReadOnlyList<TopicFactory> AlleThemen = new List<TopicFactory>
    {
        Notruf, Bewusstsein, Seitenlage, Wunden, Verbrennungen, Reanimation, Aed, Notfaelle
    };

    // Für jede Stufe dieselbe Liste - siehe Klassenkommentar.
    protected override IReadOnlyDictionary<GradeLevel, IReadOnlyList<TopicFactory>> TopicsByGrade { get; } =
        new Dictionary<GradeLevel, IReadOnlyList<TopicFactory>>
        {
            [GradeLevel.Klasse6] = AlleThemen,
            [GradeLevel.Klasse7] = AlleThemen,
            [GradeLevel.Klasse9] = AlleThemen
        };

    private static QuizQuestion Bauen(
        (string Frage, string[] Optionen, string Antwort, string Erklaerung)[] liste,
        Random r, string thema, string tipp)
    {
        var f = liste[r.Next(liste.Length)];
        return new QuizQuestion
        {
            Id = NewId(), Subject = Subject.ErsteHilfe, GradeLevel = GradeLevel.Klasse7,
            Topic = thema, Type = QuestionType.MultipleChoice,
            Prompt = f.Frage, Options = f.Optionen, CorrectAnswers = new[] { f.Antwort },
            Explanation = f.Erklaerung, HelpHint = tipp
        };
    }

    // ===================== 1. Notruf 112 und Eigenschutz =====================

    private static readonly (string Frage, string[] Optionen, string Antwort, string Erklaerung)[] NotrufListe =
    {
        ("Welche Nummer wählst du bei einem medizinischen Notfall?",
            new[] { "112", "110, denn das ist die allgemeine Notrufnummer für sämtliche Arten von Notfällen", "116117" },
            "112",
            "Die 112 ist in ganz Europa der Notruf für Rettungsdienst und Feuerwehr - kostenlos, ohne Vorwahl und von jedem Handy aus."),
        ("Was ist die WICHTIGSTE der fünf W-Fragen beim Notruf?",
            new[] { "Wo ist der Notfallort", "Wie heißt die verletzte Person mit vollem Namen und Geburtsdatum", "Wer ruft gerade an" },
            "Wo ist der Notfallort",
            "Ohne den Ort kann kein Rettungswagen losfahren. Deshalb steht das Wo an erster Stelle - alles andere kann die Leitstelle nachfragen."),
        ("Wann darfst du das Gespräch mit der Leitstelle beenden?",
            new[] { "Erst wenn die Leitstelle es sagt", "Sobald du den Ort durchgegeben hast und nichts weiter erklären möchtest", "Nach genau einer Minute" },
            "Erst wenn die Leitstelle es sagt",
            "Das fünfte W heißt Warten auf Rückfragen. Die Leitstelle stellt die Fragen und leitet dich sogar an - auflegen darf immer sie."),
        ("Was tust du ZUERST, bevor du jemandem hilfst?",
            new[] { "Auf die eigene Sicherheit achten", "Sofort zur verletzten Person laufen, weil jede einzelne Sekunde zählt", "Ein Foto für den Rettungsdienst machen" },
            "Auf die eigene Sicherheit achten",
            "Eigenschutz geht immer vor. Wer selbst verunglückt, ist kein Helfer mehr, sondern ein zweiter Patient - und bindet Rettungskräfte."),
        ("Du bist erst 12 und traust dich nicht zu helfen. Was ist richtig?",
            new[] { "Den Notruf wählen ist schon Erste Hilfe", "Lieber gar nichts tun, weil du zu jung bist und Fehler machen könntest", "Warten, bis ein Erwachsener zufällig vorbeikommt" },
            "Den Notruf wählen ist schon Erste Hilfe",
            "Nichts zu tun ist der einzige echte Fehler. Ein Notruf allein rettet Leben - dafür braucht es kein Alter und keine Ausbildung."),
        ("Kann man bei Erster Hilfe etwas falsch machen und dafür bestraft werden?",
            new[] { "Nein - bestraft wird nur, wer gar nicht hilft", "Ja, jeder Fehler bei der Hilfe wird als Körperverletzung geahndet", "Nur wenn man älter als 18 ist" },
            "Nein - bestraft wird nur, wer gar nicht hilft",
            "Unterlassene Hilfeleistung ist strafbar, ein gut gemeinter Fehler beim Helfen nicht. Diese Angst hält viele ab - zu Unrecht."),
        ("Was gehört zum Absichern einer Unfallstelle an der Straße?",
            new[] { "Warnblinker, Warnweste anziehen, Warndreieck aufstellen", "Nur die Handy-Taschenlampe einschalten und damit winken", "Die Verletzten sofort zur Seite tragen" },
            "Warnblinker, Warnweste anziehen, Warndreieck aufstellen",
            "Erst absichern, dann melden, dann helfen. Die Warnweste ziehst du noch im Auto an, bevor du aussteigst."),
        ("Wie weit vor der Unfallstelle stellst du auf einer Landstraße das Warndreieck auf?",
            new[] { "Etwa 100 Meter", "Etwa 10 Meter, damit man es von der Unfallstelle aus noch gut sehen kann", "Direkt neben das Fahrzeug" },
            "Etwa 100 Meter",
            "Innerorts rund 50 Meter, auf der Landstraße 100, auf der Autobahn 200 - je schneller der Verkehr, desto früher die Warnung."),
        ("Was bedeutet \"Rettungskette\"?",
            new[] { "Alle Schritte vom Absichern bis zum Krankenhaus greifen ineinander", "Eine Kette, mit der Verletzte aus einem Fahrzeug gezogen werden", "Die Reihenfolge, in der Rettungswagen losfahren" },
            "Alle Schritte vom Absichern bis zum Krankenhaus greifen ineinander",
            "Jedes Glied zählt: Absichern, Notruf, Erste Hilfe, Rettungsdienst, Klinik. Das schwächste Glied bestimmt, wie gut es ausgeht - und das erste bist du."),
        ("Ein Notruf vom Handy ohne Guthaben oder ohne Netz deines Anbieters - geht das?",
            new[] { "Ja, die 112 funktioniert über jedes verfügbare Netz", "Nein, ohne Guthaben ist gar kein Anruf möglich", "Nur wenn eine SIM-Karte eingelegt ist" },
            "Ja, die 112 funktioniert über jedes verfügbare Netz",
            "Der Notruf hat Vorrang und nutzt jedes erreichbare Mobilfunknetz. Guthaben braucht es dafür keins.")
    };

    private static QuizQuestion Notruf(Random r) => Bauen(NotrufListe, r,
        "Notruf 112 und Eigenschutz",
        "Merke die Reihenfolge: erst absichern, dann 112 rufen, dann helfen. Und das Wo ist die wichtigste Angabe.");

    // ===================== 2. Bewusstsein und Atmung prüfen =====================

    private static readonly (string Frage, string[] Optionen, string Antwort, string Erklaerung)[] BewusstseinListe =
    {
        ("Wie prüfst du, ob jemand bei Bewusstsein ist?",
            new[] { "Laut ansprechen und an den Schultern rütteln", "Nur von weitem zusehen, ob sich die Person von selbst wieder bewegt", "Kaltes Wasser ins Gesicht schütten" },
            "Laut ansprechen und an den Schultern rütteln",
            "Ansprechen und anfassen: \"Hallo, hören Sie mich?\" und dabei an beiden Schultern rütteln. Keine Reaktion heißt bewusstlos."),
        ("Warum musst du bei einem Bewusstlosen den Kopf überstrecken?",
            new[] { "Weil sonst die Zunge den Rachen verschließt", "Damit die Person besser sehen kann, wer ihr gerade hilft", "Damit der Nacken nicht steif wird" },
            "Weil sonst die Zunge den Rachen verschließt",
            "Bei Bewusstlosigkeit erschlafft die Zungenmuskulatur und blockiert die Atemwege. Kopf nackenwärts neigen, Kinn anheben - das schafft freie Bahn."),
        ("Wie lange prüfst du die Atmung?",
            new[] { "Etwa 10 Sekunden", "Mindestens zwei volle Minuten, um ganz sicher zu sein", "Nur einen kurzen Blick, das reicht aus" },
            "Etwa 10 Sekunden",
            "Zehn Sekunden: sehen, ob sich der Brustkorb hebt, hören und an der Wange fühlen. Länger zu warten kostet nur Zeit."),
        ("Was heißt \"Sehen, Hören, Fühlen\"?",
            new[] { "Brustkorb ansehen, Atemgeräusche hören, Luft an der Wange fühlen", "Den Puls sehen, den Herzschlag hören und die Temperatur fühlen", "Die Augen, die Ohren und die Haut untersuchen" },
            "Brustkorb ansehen, Atemgeräusche hören, Luft an der Wange fühlen",
            "Du beugst dich mit dem Ohr über Mund und Nase und schaust dabei zum Brustkorb - so prüfst du alle drei Dinge gleichzeitig."),
        ("Was ist Schnappatmung?",
            new[] { "Ein Zeichen für Herzstillstand - keine normale Atmung", "Eine besonders tiefe und gesunde Form der Atmung im Schlaf", "Ein Schluckauf, der beim Erschrecken auftritt" },
            "Ein Zeichen für Herzstillstand - keine normale Atmung",
            "Vereinzelte, schnappende Atemzüge sehen aus wie Atmung, sind aber keine. Wer das für Atmung hält, unterlässt die lebensrettende Herzdruckmassage."),
        ("Die Person ist bewusstlos und atmet normal. Was tust du?",
            new[] { "Stabile Seitenlage und 112 rufen", "Sofort mit der Herzdruckmassage beginnen, weil sie nicht antwortet", "Sie auf den Rücken legen und zudecken" },
            "Stabile Seitenlage und 112 rufen",
            "Bewusstlos plus normale Atmung ist genau der Fall für die stabile Seitenlage. Herzdruckmassage wäre hier falsch."),
        ("Die Person ist bewusstlos und atmet NICHT normal. Was tust du?",
            new[] { "Sofort Herzdruckmassage und 112 rufen lassen", "Erst die stabile Seitenlage herstellen und danach beobachten", "Wasser zu trinken geben" },
            "Sofort Herzdruckmassage und 112 rufen lassen",
            "Keine normale Atmung heißt Herz-Kreislauf-Stillstand. Jede Sekunde ohne Herzdruckmassage senkt die Überlebenschance."),
        ("Solltest du bei einem Bewusstlosen den Puls suchen?",
            new[] { "Nein, als Laie nicht - das kostet nur Zeit", "Ja, unbedingt am Handgelenk, bevor du irgendetwas anderes tust", "Ja, aber nur am Fuß" },
            "Nein, als Laie nicht - das kostet nur Zeit",
            "Puls tasten ist selbst für Geübte unzuverlässig. Für Laien zählt nur: reagiert die Person, und atmet sie normal?"),
        ("Was tust du zuerst, wenn du eine reglose Person findest und allein bist?",
            new[] { "Ansprechen, Atmung prüfen, dann 112 rufen", "Zuerst die Angehörigen suchen und benachrichtigen", "Die Person erst einmal in ein Auto setzen" },
            "Ansprechen, Atmung prüfen, dann 112 rufen",
            "Prüfen - Rufen - Drücken ist die Reihenfolge, die in der Schule gelehrt wird. Erst wissen, was los ist, dann melden."),
        ("Jemand ist bewusstlos, du bist nicht allein. Was ist am besten?",
            new[] { "Eine bestimmte Person direkt ansprechen und sie den Notruf wählen lassen", "In die Runde rufen, ob nicht irgendjemand mal die 112 anrufen könnte", "Warten, bis jemand von selbst hilft" },
            "Eine bestimmte Person direkt ansprechen und sie den Notruf wählen lassen",
            "\"Sie in der roten Jacke: rufen Sie die 112!\" In eine Gruppe hineingerufen fühlt sich niemand zuständig - direkt angesprochen schon.")
    };

    private static QuizQuestion Bewusstsein(Random r) => Bauen(BewusstseinListe, r,
        "Bewusstsein und Atmung prüfen",
        "Zwei Fragen entscheiden alles: Reagiert die Person? Atmet sie normal? Schnappatmung zählt NICHT als Atmung.");

    // ===================== 3. Stabile Seitenlage =====================

    private static readonly (string Frage, string[] Optionen, string Antwort, string Erklaerung)[] SeitenlageListe =
    {
        ("Wann bringst du jemanden in die stabile Seitenlage?",
            new[] { "Bewusstlos, aber mit normaler Atmung", "Immer, sobald jemand über starke Schmerzen klagt und sich hinlegen will", "Wenn jemand nicht mehr atmet" },
            "Bewusstlos, aber mit normaler Atmung",
            "Beide Bedingungen müssen zutreffen. Wer wach ist, braucht sie nicht; wer nicht atmet, braucht Herzdruckmassage."),
        ("Warum ist die stabile Seitenlage überhaupt nötig?",
            new[] { "Damit Erbrochenes und Blut aus dem Mund abfließen können", "Damit die Person bequemer liegt und schneller wieder aufwacht", "Damit man den Rücken untersuchen kann" },
            "Damit Erbrochenes und Blut aus dem Mund abfließen können",
            "Ein Bewusstloser kann nicht schlucken und nicht husten. Auf dem Rücken erstickt er an dem, was im Rachen steht."),
        ("Wo muss der Mund in der stabilen Seitenlage sein?",
            new[] { "Am tiefsten Punkt des Kopfes", "Möglichst weit oben, damit die Person besser Luft bekommt", "Auf gleicher Höhe wie die Schulter" },
            "Am tiefsten Punkt des Kopfes",
            "Nur wenn der Mund der tiefste Punkt ist, läuft Flüssigkeit von allein heraus. Deshalb wird der Kopf leicht überstreckt und der Mund geöffnet."),
        ("Was machst du mit dem Kopf, nachdem die Person auf der Seite liegt?",
            new[] { "Überstrecken und den Mund öffnen", "Ein festes Kissen darunterschieben, damit er ruhig liegt", "Mit einer Jacke zudecken" },
            "Überstrecken und den Mund öffnen",
            "Der überstreckte Kopf hält die Atemwege frei, der geöffnete Mund lässt abfließen. Ein Kissen würde beides zunichtemachen."),
        ("Wozu dient das angewinkelte obere Bein in der Seitenlage?",
            new[] { "Es verhindert, dass die Person auf den Bauch rollt", "Es verbessert die Durchblutung im Bein", "Es hält die Person warm" },
            "Es verhindert, dass die Person auf den Bauch rollt",
            "Das im rechten Winkel aufgestellte Knie ist die Stütze, die die Lage \"stabil\" macht - daher der Name."),
        ("Was tust du, nachdem die Person stabil auf der Seite liegt?",
            new[] { "Zudecken, die Atmung weiter beobachten und beim Betroffenen bleiben", "Weggehen, weil die Lage nun sicher ist", "Sie regelmäßig wieder auf den Rücken drehen" },
            "Zudecken, die Atmung weiter beobachten und beim Betroffenen bleiben",
            "Die Atmung kann jederzeit aussetzen. Bleibt sie aus, drehst du zurück und beginnst mit der Herzdruckmassage."),
        ("Die Person in der Seitenlage hört auf zu atmen. Was tust du?",
            new[] { "Auf den Rücken drehen und Herzdruckmassage beginnen", "In der Seitenlage lassen und auf den Rettungsdienst warten", "Den Kopf noch weiter überstrecken und abwarten" },
            "Auf den Rücken drehen und Herzdruckmassage beginnen",
            "Ohne Atmung wird aus der Seitenlage sofort ein Wiederbelebungsfall - drehen und drücken."),
        ("Darfst du eine bewusstlose Person mit Verdacht auf Wirbelsäulenverletzung in die Seitenlage bringen?",
            new[] { "Ja - freie Atemwege sind wichtiger als die Sorge um die Wirbelsäule", "Nein, dann darf man sie unter keinen Umständen bewegen", "Nur mit ärztlicher Erlaubnis am Telefon" },
            "Ja - freie Atemwege sind wichtiger als die Sorge um die Wirbelsäule",
            "Ersticken tötet in Minuten. Deshalb geht die Atmung vor, auch wenn du dabei möglichst behutsam drehst."),
        ("Auf welche Seite drehst du die Person?",
            new[] { "Die Seite ist grundsätzlich frei wählbar", "Immer nur nach links, weil dort das Herz liegt", "Immer nur nach rechts" },
            "Die Seite ist grundsätzlich frei wählbar",
            "Entscheidend ist die stabile Lage mit freien Atemwegen, nicht die Seite. Bei Verletzungen nimmt man die unverletzte Seite nach unten."),
        ("Wie oft prüfst du bei einem Bewusstlosen in Seitenlage die Atmung nach?",
            new[] { "Ständig, bis der Rettungsdienst da ist", "Ein einziges Mal reicht völlig aus", "Alle zwanzig Minuten" },
            "Ständig, bis der Rettungsdienst da ist",
            "Der Zustand kann sich jederzeit ändern. Beim Betroffenen zu bleiben und zu beobachten gehört zur Ersten Hilfe dazu.")
    };

    private static QuizQuestion Seitenlage(Random r) => Bauen(SeitenlageListe, r,
        "Stabile Seitenlage",
        "Zwei Bedingungen: bewusstlos UND normale Atmung. Der Mund muss der tiefste Punkt sein.");

    // ===================== 4. Wunden und starke Blutungen =====================

    private static readonly (string Frage, string[] Optionen, string Antwort, string Erklaerung)[] WundenListe =
    {
        ("Was tust du mit einem Fremdkörper, der in einer Wunde steckt?",
            new[] { "Stecken lassen und die Wunde ringsum abpolstern", "Vorsichtig herausziehen und die Wunde danach reinigen", "So lange hin und her bewegen, bis er sich löst" },
            "Stecken lassen und die Wunde ringsum abpolstern",
            "Der Fremdkörper verschließt die Wunde oft selbst. Herausziehen löst schwere Blutungen aus - das macht erst die Klinik."),
        ("Womit deckst du eine Wunde ab?",
            new[] { "Mit einer keimfreien Wundauflage", "Mit einem sauberen Taschentuch aus der Hosentasche", "Mit der bloßen Hand, bis der Rettungsdienst kommt" },
            "Mit einer keimfreien Wundauflage",
            "Keimfrei heißt steril verpackt aus dem Verbandkasten. Alles andere bringt Keime in die Wunde."),
        ("Wie behandelst du eine stark blutende Wunde am Arm?",
            new[] { "Wundauflage drauf, Druckverband anlegen, Arm hochhalten", "Die Wunde erst gründlich mit Seife auswaschen", "Ein Pflaster aufkleben und abwarten" },
            "Wundauflage drauf, Druckverband anlegen, Arm hochhalten",
            "Druck stoppt die Blutung, Hochhalten senkt den Blutdruck in der Wunde. Auswaschen kostet nur Zeit und Blut."),
        ("Woraus besteht ein Druckverband?",
            new[] { "Wundauflage, Druckkörper darüber, fest umwickelt", "Nur einer sehr fest gewickelten Binde ohne weitere Auflage", "Einem Pflaster und einer Kühlkompresse" },
            "Wundauflage, Druckkörper darüber, fest umwickelt",
            "Der Druckkörper - etwa eine ungeöffnete Verbandpackung - presst auf die Wunde. Ohne ihn ist es nur ein normaler Verband."),
        ("Der Druckverband blutet durch. Was tust du?",
            new[] { "Einen zweiten Druckverband darüber anlegen", "Den ersten abnehmen und einen neuen darunter anlegen", "Den Verband lockern, damit die Haut Luft bekommt" },
            "Einen zweiten Druckverband darüber anlegen",
            "Den ersten abzunehmen reißt das begonnene Gerinnsel wieder auf. Es wird immer darübergelegt, nie ausgetauscht."),
        ("Warum sollst du bei fremdem Blut Einmalhandschuhe tragen?",
            new[] { "Zum Schutz vor Krankheitserregern im Blut", "Damit die Wunde nicht so stark schmerzt", "Weil das gesetzlich für jeden Ersthelfer vorgeschrieben ist" },
            "Zum Schutz vor Krankheitserregern im Blut",
            "Eigenschutz gilt auch hier. Die Handschuhe liegen in jedem Verbandkasten ganz oben."),
        ("Was tust du bei starkem Nasenbluten?",
            new[] { "Kopf nach vorn beugen und die Nasenflügel zusammendrücken", "Den Kopf weit in den Nacken legen, damit es aufhört zu tropfen", "Ein Taschentuch tief in die Nase schieben" },
            "Kopf nach vorn beugen und die Nasenflügel zusammendrücken",
            "Kopf in den Nacken lässt Blut in den Rachen und in den Magen laufen - Übelkeit und Erbrechen sind die Folge."),
        ("Was ist bei einer abgetrennten Fingerkuppe zu tun?",
            new[] { "Das Teil sauber und trocken mitnehmen, nicht direkt auf Eis legen", "Es sofort in Wasser einlegen, damit es nicht austrocknet", "Es wegwerfen, es lässt sich ohnehin nicht mehr verwenden" },
            "Das Teil sauber und trocken mitnehmen, nicht direkt auf Eis legen",
            "Amputate werden trocken und gekühlt transportiert - direkter Eiskontakt zerstört das Gewebe endgültig."),
        ("Wie versorgst du eine kleine Schürfwunde am Knie?",
            new[] { "Nicht auspusten, keimfrei abdecken, in Ruhe lassen", "Kräftig auspusten und mit Spucke säubern", "Mit einer Bürste gründlich ausschrubben" },
            "Nicht auspusten, keimfrei abdecken, in Ruhe lassen",
            "Im Atem und im Speichel stecken Keime. Kleine Wunden brauchen nur eine saubere Abdeckung."),
        ("Warum soll ein Betroffener mit starker Blutung sich hinlegen?",
            new[] { "Weil er jederzeit ohnmächtig werden kann", "Damit die Blutung schneller von selbst aufhört", "Weil Liegen die Schmerzen sofort beseitigt" },
            "Weil er jederzeit ohnmächtig werden kann",
            "Blutverlust führt zum Kreislaufschock. Wer schon liegt, kann nicht stürzen und sich zusätzlich verletzen.")
    };

    private static QuizQuestion Wunden(Random r) => Bauen(WundenListe, r,
        "Wunden und starke Blutungen",
        "Fremdkörper bleiben stecken. Druckverbände werden übereinandergelegt, nie ausgetauscht.");

    // ===================== 5. Verbrennungen und Verbrühungen =====================

    private static readonly (string Frage, string[] Optionen, string Antwort, string Erklaerung)[] VerbrennungListe =
    {
        ("Womit kühlst du eine kleine Verbrennung an der Hand?",
            new[] { "Mit handwarmem Leitungswasser", "Mit Eiswasser, denn je kälter desto besser wirkt die Kühlung", "Mit Eiswürfeln direkt auf der Haut" },
            "Mit handwarmem Leitungswasser",
            "Handwarm reicht und ist sicher. Eis schädigt die ohnehin verletzte Haut zusätzlich und kann die Wunde vergrößern."),
        ("Wie lange kühlst du eine kleine Verbrennung etwa?",
            new[] { "So lange, wie es guttut - meist 10 bis 15 Minuten", "Mindestens eine volle Stunde ohne Unterbrechung", "Nur wenige Sekunden" },
            "So lange, wie es guttut - meist 10 bis 15 Minuten",
            "Gekühlt wird gegen den Schmerz. Sobald es unangenehm wird oder der Betroffene friert, wird aufgehört."),
        ("Warum kühlt man großflächige Verbrennungen NICHT?",
            new[] { "Weil der Körper dabei gefährlich auskühlt", "Weil die Haut dann nicht mehr richtig nachwachsen kann", "Weil Wasser die Brandwunde entzündet" },
            "Weil der Körper dabei gefährlich auskühlt",
            "Bei großen Flächen ist die Unterkühlung die größere Gefahr als der Schmerz. Dort wird zugedeckt und der Notruf gewählt."),
        ("Was gehört NICHT auf eine Brandwunde?",
            new[] { "Mehl, Öl, Butter oder Zahnpasta", "Eine keimfreie, nicht klebende Wundauflage", "Locker aufgelegte, saubere Tücher" },
            "Mehl, Öl, Butter oder Zahnpasta",
            "Hausmittel schließen die Hitze ein und müssen in der Klinik schmerzhaft entfernt werden. Sie helfen nie."),
        ("Was tust du mit Brandblasen?",
            new[] { "Nicht öffnen", "Vorsichtig mit einer sauberen Nadel aufstechen und ausdrücken", "Fest zusammendrücken, damit sie schneller verschwinden" },
            "Nicht öffnen",
            "Die Blasenhaut ist der beste Schutz gegen Keime. Ist sie ab, wird aus der Verbrennung leicht eine Infektion."),
        ("Kleidung klebt in der Brandwunde fest. Was tust du?",
            new[] { "Sie kleben lassen und nur ringsum entfernen", "Sie kräftig abziehen, damit die Wunde freiliegt", "Sie mit Wasser einweichen und dann abziehen" },
            "Sie kleben lassen und nur ringsum entfernen",
            "Festgeklebte Kleidung abzuziehen reißt die Haut mit ab. Nur was locker sitzt, wird entfernt."),
        ("Wie hilfst du jemandem, dessen Kleidung brennt?",
            new[] { "Mit Wasser löschen oder in eine Decke wickeln und am Boden wälzen", "Ihn schnell laufen lassen, damit der Fahrtwind die Flammen ausbläst", "Ihn kräftig anpusten" },
            "Mit Wasser löschen oder in eine Decke wickeln und am Boden wälzen",
            "Laufen facht die Flammen erst richtig an. Ersticken der Flammen oder Wasser sind die einzigen richtigen Wege."),
        ("Was tust du bei einem Sonnenbrand?",
            new[] { "Aus der Sonne gehen, kühlen und viel trinken", "Weiter in der Sonne bleiben, damit die Haut sich daran gewöhnt", "Die Haut mit einer Bürste abrubbeln" },
            "Aus der Sonne gehen, kühlen und viel trinken",
            "Ein Sonnenbrand ist eine echte Verbrennung. Mit Blasen oder Fieber gehört er in ärztliche Behandlung."),
        ("Was ist eine Verbrühung?",
            new[] { "Eine Verletzung durch heiße Flüssigkeit oder Dampf", "Eine Verletzung durch eine offene Flamme", "Eine Verletzung durch Chemikalien" },
            "Eine Verletzung durch heiße Flüssigkeit oder Dampf",
            "Heißer Tee oder Wasserdampf verbrühen - versorgt wird genauso wie eine Verbrennung."),
        ("Wann muss eine Verbrennung immer ärztlich behandelt werden?",
            new[] { "Bei Kindern, im Gesicht, an Händen oder bei großer Fläche", "Nur wenn sie mehrere Tage lang wehtut", "Nur wenn sie sichtbar blutet" },
            "Bei Kindern, im Gesicht, an Händen oder bei großer Fläche",
            "An Gesicht und Händen drohen bleibende Schäden, bei Kindern zählt schon eine kleinere Fläche als gefährlich.")
    };

    private static QuizQuestion Verbrennungen(Random r) => Bauen(VerbrennungListe, r,
        "Verbrennungen und Verbrühungen",
        "Handwarmes Wasser, nur kleine Flächen, nie Eis und nie Hausmittel. Blasen bleiben zu.");

    // ===================== 6. Reanimation =====================

    private static readonly (string Frage, string[] Optionen, string Antwort, string Erklaerung)[] ReanimationListe =
    {
        ("Wo drückst du bei der Herzdruckmassage?",
            new[] { "In der Mitte des Brustkorbs, auf dem Brustbein", "Auf der linken Seite des Brustkorbs, direkt über dem Herzen", "Auf dem Bauch, kurz unterhalb der Rippen" },
            "In der Mitte des Brustkorbs, auf dem Brustbein",
            "Die Mitte des Brustkorbs ist der richtige Punkt - nicht links. Von dort aus wird das Herz zwischen Brustbein und Wirbelsäule gepresst."),
        ("Wie tief drückst du bei einem Erwachsenen?",
            new[] { "5 bis 6 Zentimeter", "Nur etwa 1 Zentimeter, um keine Rippen zu brechen", "So tief wie irgend möglich" },
            "5 bis 6 Zentimeter",
            "Weniger als 5 cm pumpt kein Blut. Gebrochene Rippen sind dabei möglich und kein Grund aufzuhören."),
        ("Wie schnell drückst du?",
            new[] { "100 bis 120 Mal pro Minute", "Etwa 40 Mal pro Minute in aller Ruhe", "So schnell wie du überhaupt kannst" },
            "100 bis 120 Mal pro Minute",
            "Das ist der Takt von \"Stayin' Alive\" - langsamer pumpt zu wenig, schneller füllt sich das Herz nicht mehr."),
        ("Was ist der 30:2-Rhythmus?",
            new[] { "30 Mal drücken, dann 2 Mal beatmen", "30 Sekunden drücken, dann 2 Minuten Pause", "2 Mal drücken, dann 30 Sekunden warten" },
            "30 Mal drücken, dann 2 Mal beatmen",
            "So wechseln sich Druck und Beatmung ab. Wer sich die Beatmung nicht zutraut, drückt einfach ohne Pause weiter."),
        ("Du traust dich nicht, Mund zu Mund zu beatmen. Was ist richtig?",
            new[] { "Ohne Beatmung durchgehend drücken", "Dann lieber gar nichts tun, weil es sonst nicht wirkt", "Auf den Rettungsdienst warten und nichts machen" },
            "Ohne Beatmung durchgehend drücken",
            "Reine Herzdruckmassage ist deutlich besser als nichts und wird Laien ausdrücklich empfohlen. Im Blut ist zu Beginn noch Sauerstoff."),
        ("Wann hörst du mit der Herzdruckmassage auf?",
            new[] { "Wenn der Rettungsdienst übernimmt oder die Person normal atmet", "Nach fünf Minuten, weil dann keine Chance mehr besteht", "Sobald du müde wirst" },
            "Wenn der Rettungsdienst übernimmt oder die Person normal atmet",
            "Bis dahin wird durchgedrückt. Sind mehrere Helfer da, wechselt ihr euch etwa alle zwei Minuten ab."),
        ("Warum solltet ihr euch bei der Herzdruckmassage abwechseln?",
            new[] { "Weil die Druckqualität mit der Erschöpfung stark nachlässt", "Weil das so vorgeschrieben ist und kontrolliert wird", "Damit jeder einmal geübt hat" },
            "Weil die Druckqualität mit der Erschöpfung stark nachlässt",
            "Schon nach zwei Minuten drückt man messbar flacher, ohne es zu merken. Der Wechsel soll nur wenige Sekunden dauern."),
        ("Wie hältst du die Arme beim Drücken?",
            new[] { "Gestreckt, senkrecht über dem Brustkorb", "Angewinkelt, um besser dosieren zu können", "Seitlich versetzt neben dem Körper" },
            "Gestreckt, senkrecht über dem Brustkorb",
            "Mit gestreckten Armen arbeitet das Körpergewicht mit, nicht die Armkraft - nur so hältst du es lange genug durch."),
        ("Was bedeutet \"Prüfen - Rufen - Drücken\"?",
            new[] { "Bewusstsein und Atmung prüfen, 112 rufen, Herzdruckmassage beginnen", "Den Puls prüfen, um Hilfe rufen und die Person schütteln", "Die Wunde prüfen, den Arzt rufen und den Verband andrücken" },
            "Bewusstsein und Atmung prüfen, 112 rufen, Herzdruckmassage beginnen",
            "Diese drei Wörter sind die ganze Wiederbelebung für Laien - so wird sie an deutschen Schulen unterrichtet."),
        ("Darf ein Jugendlicher eine Herzdruckmassage durchführen?",
            new[] { "Ja - entscheidend ist, dass überhaupt gedrückt wird", "Nein, das dürfen nur ausgebildete Sanitäter", "Nur ab 18 Jahren" },
            "Ja - entscheidend ist, dass überhaupt gedrückt wird",
            "Ohne Herzdruckmassage sinkt die Überlebenschance mit jeder Minute um etwa zehn Prozent. Wer drückt, kann nichts schlimmer machen.")
    };

    private static QuizQuestion Reanimation(Random r) => Bauen(ReanimationListe, r,
        "Reanimation und Herzdruckmassage",
        "Mitte des Brustkorbs, 5-6 cm tief, 100-120 pro Minute - der Takt von \"Stayin' Alive\".");

    // ===================== 7. AED =====================

    private static readonly (string Frage, string[] Optionen, string Antwort, string Erklaerung)[] AedListe =
    {
        ("Was ist ein AED?",
            new[] { "Ein Gerät, das dem Helfer per Sprachansage durch die Wiederbelebung hilft", "Ein Messgerät, das nur den Puls anzeigt", "Ein Beatmungsgerät für den Rettungswagen" },
            "Ein Gerät, das dem Helfer per Sprachansage durch die Wiederbelebung hilft",
            "AED heißt automatisierter externer Defibrillator. Er erklärt jeden Schritt laut - man muss nichts auswendig können."),
        ("Muss man für die Bedienung eines AED ausgebildet sein?",
            new[] { "Nein - er ist ausdrücklich für Laien gebaut", "Ja, ohne Erste-Hilfe-Schein darf man ihn nicht benutzen", "Nur Ärzte dürfen ihn einsetzen" },
            "Nein - er ist ausdrücklich für Laien gebaut",
            "Deshalb hängen AEDs öffentlich in Bahnhöfen, Schulen und Rathäusern. Anmachen und den Anweisungen folgen genügt."),
        ("Wohin kommen die beiden Elektroden?",
            new[] { "Rechts oben unter dem Schlüsselbein, links unten seitlich am Brustkorb", "Beide nebeneinander auf die linke Brustseite", "Auf Bauch und Rücken" },
            "Rechts oben unter dem Schlüsselbein, links unten seitlich am Brustkorb",
            "So verläuft der Stromstoß quer durch das Herz. Auf den Klebepads ist die richtige Lage aufgedruckt."),
        ("Was tust du, während der AED den Herzrhythmus prüft?",
            new[] { "Niemand darf die Person berühren", "Weiter drücken, ohne die Hände wegzunehmen", "Die Person festhalten, damit sie sich nicht bewegt" },
            "Niemand darf die Person berühren",
            "Berührung verfälscht die Messung. Der AED sagt selbst an, wann losgelassen und wann weitergedrückt wird."),
        ("Was tust du, wenn der AED zum Schock auffordert?",
            new[] { "Laut warnen, alle zurücktreten lassen, dann auslösen", "Sofort drücken, ohne auf die anderen zu achten", "Die Elektroden vorher wieder abnehmen" },
            "Laut warnen, alle zurücktreten lassen, dann auslösen",
            "\"Alle weg vom Patienten!\" - der Stromstoß trifft sonst auch den Helfer. Erst schauen, dann auslösen."),
        ("Was machst du direkt nach dem Schock?",
            new[] { "Sofort mit der Herzdruckmassage weitermachen", "Zwei Minuten abwarten, ob sich etwas tut", "Die Elektroden abziehen und den AED ausschalten" },
            "Sofort mit der Herzdruckmassage weitermachen",
            "Der Schock allein bringt das Herz selten in Gang. Der AED sagt es auch an: sofort weiterdrücken."),
        ("Der AED sagt \"kein Schock empfohlen\". Was heißt das?",
            new[] { "Weiter Herzdruckmassage - der Rhythmus ist nicht schockbar", "Die Person ist gesund und du kannst aufhören", "Das Gerät ist kaputt" },
            "Weiter Herzdruckmassage - der Rhythmus ist nicht schockbar",
            "Nicht jeder Herzstillstand lässt sich durch einen Schock beenden. Gedrückt wird trotzdem weiter."),
        ("Die Brust der Person ist nass. Was tust du vor dem Aufkleben?",
            new[] { "Die Brust abtrocknen", "Die Elektroden einfach auf die nasse Haut kleben", "Erst warten, bis sie von allein trocknet" },
            "Die Brust abtrocknen",
            "Auf nasser Haut haften die Pads nicht und der Strom verteilt sich über die Oberfläche, statt durchs Herz zu fließen."),
        ("Wo findest du im Alltag einen AED?",
            new[] { "An öffentlichen Orten wie Bahnhöfen, Schulen und Rathäusern", "Nur in Krankenhäusern", "Nur in Rettungswagen" },
            "An öffentlichen Orten wie Bahnhöfen, Schulen und Rathäusern",
            "Ein grünes Schild mit Herz und Blitz zeigt sie an. Es lohnt sich zu wissen, wo der nächste hängt."),
        ("Darf ein AED bei einem Kind eingesetzt werden?",
            new[] { "Ja, viele Geräte haben dafür einen eigenen Modus oder Kinderelektroden", "Nein, bei Kindern ist er grundsätzlich verboten", "Nur wenn ein Arzt anwesend ist" },
            "Ja, viele Geräte haben dafür einen eigenen Modus oder Kinderelektroden",
            "Gibt es keine Kinderelektroden, werden die normalen genommen - ein Schock ist besser als keiner.")
    };

    private static QuizQuestion Aed(Random r) => Bauen(AedListe, r,
        "AED - der Defibrillator für alle",
        "Einschalten und zuhören: das Gerät sagt jeden Schritt an. Beim Schock alle wegtreten lassen.");

    // ===================== 8. Verschlucken, Vergiftung, Schock =====================

    private static readonly (string Frage, string[] Optionen, string Antwort, string Erklaerung)[] NotfaelleListe =
    {
        ("Jemand verschluckt sich und kann noch husten. Was tust du?",
            new[] { "Zum Weiterhusten auffordern", "Sofort kräftig auf den Rücken schlagen", "Ihm Wasser zu trinken geben" },
            "Zum Weiterhusten auffordern",
            "Husten ist der wirksamste Mechanismus überhaupt. Eingreifen darfst du erst, wenn er nicht mehr hustet oder keine Luft mehr bekommt."),
        ("Jemand bekommt keine Luft mehr. Was tust du zuerst?",
            new[] { "Fünf feste Schläge zwischen die Schulterblätter", "Sofort das Heimlich-Manöver anwenden", "Ihn hinlegen und die Beine hochlagern" },
            "Fünf feste Schläge zwischen die Schulterblätter",
            "Erst die Rückenschläge bei vorgebeugtem Oberkörper. Erst wenn sie nicht helfen, folgt der Oberbauchgriff."),
        ("Was ist das Heimlich-Manöver?",
            new[] { "Ruckartiger Druck in den Oberbauch von hinten", "Ein fester Schlag auf den Brustkorb von vorn", "Kräftiges Drücken auf den Hals" },
            "Ruckartiger Druck in den Oberbauch von hinten",
            "Der Druck presst Restluft aus der Lunge und schleudert den Fremdkörper heraus. Danach muss immer ein Arzt nachsehen."),
        ("Welche Nummer wählst du bei einer Vergiftung?",
            new[] { "Den Giftnotruf oder die 112", "Die 110", "Die Nummer der Apotheke" },
            "Den Giftnotruf oder die 112",
            "Der Giftnotruf sagt genau, was zu tun ist. Wichtig: die Packung des Mittels bereithalten."),
        ("Was tust du NICHT bei einer Vergiftung?",
            new[] { "Erbrechen auslösen", "Die Packung des Mittels aufbewahren", "Den Giftnotruf anrufen" },
            "Erbrechen auslösen",
            "Ätzende Stoffe verletzen beim Erbrechen die Speiseröhre ein zweites Mal. Erbrechen wird nur auf ausdrückliche Anweisung ausgelöst."),
        ("Was tust du bei einer Verätzung der Haut mit einem Reinigungsmittel?",
            new[] { "Sofort lange mit viel Wasser spülen", "Die Stelle mit einem trockenen Tuch abreiben", "Eine Creme darauf auftragen" },
            "Sofort lange mit viel Wasser spülen",
            "Verdünnen ist alles. Bei den Augen wird vom inneren Augenwinkel nach außen gespült, damit nichts ins andere Auge läuft."),
        ("Woran erkennst du einen Kreislaufschock?",
            new[] { "Blasse, kalte Haut, Frieren, schneller Puls, Unruhe", "Rotes Gesicht, langsamer Puls und große Müdigkeit", "Nur an starken Schmerzen" },
            "Blasse, kalte Haut, Frieren, schneller Puls, Unruhe",
            "Der Körper zieht das Blut in die Mitte zurück, um Herz und Gehirn zu versorgen - deshalb wird die Haut blass und kalt."),
        ("Wie hilfst du bei einem Kreislaufschock?",
            new[] { "Hinlegen, Beine hoch, zudecken, beruhigen", "Aufstehen und umhergehen lassen", "Etwas Süßes zu essen geben" },
            "Hinlegen, Beine hoch, zudecken, beruhigen",
            "Die Schocklage bringt Blut aus den Beinen zurück in den Rumpf. Wärme und Zuspruch gehören dazu."),
        ("Wann darfst du die Schocklage NICHT anwenden?",
            new[] { "Bei Kopf- oder Brustverletzungen und bei Atemnot", "Wenn der Betroffene friert", "Wenn der Betroffene sehr blass ist" },
            "Bei Kopf- oder Brustverletzungen und bei Atemnot",
            "Hochgelagerte Beine erhöhen den Druck in Kopf und Brustkorb. Bei Atemnot und Herzproblemen wird stattdessen der Oberkörper hoch gelagert."),
        ("Was tust du bei einem Sonnenstich?",
            new[] { "In den Schatten bringen, Kopf kühlen, Oberkörper erhöht lagern", "In die Sonne zurückbringen, damit der Körper sich anpasst", "Beine hochlagern und flach hinlegen" },
            "In den Schatten bringen, Kopf kühlen, Oberkörper erhöht lagern",
            "Beim Sonnenstich ist der Kopf überhitzt - erhöht gelagert und gekühlt. Erbrechen oder Bewusstseinstrübung gehören in ärztliche Hand.")
    };

    private static QuizQuestion Notfaelle(Random r) => Bauen(NotfaelleListe, r,
        "Verschlucken, Vergiftung und Schock",
        "Husten lassen, solange es geht. Bei Vergiftung nie Erbrechen auslösen. Schocklage nicht bei Kopf-, Brust- oder Atemproblemen.");
}
