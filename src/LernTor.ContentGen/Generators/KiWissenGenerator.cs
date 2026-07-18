using LernTor.Core.Enums;
using LernTor.Core.Models;

namespace LernTor.ContentGen.Generators;

/// <summary>
/// KI-Bereich-Generator: KI verstehen und sicher nutzen (kein Rahmenlehrplan-Fach, sondern
/// Medien-/KI-Kompetenz). Klasse 6 = Grundlagen (Werkzeug-Verständnis, Alltag, Grundregeln),
/// Klasse 9 = vertieft (Halluzinationen/Fakten-Check, Bias, Deepfakes/Datenschutz).
/// Die Lerntexte dazu liefert KiContentService (Core); die Fragen hier sind die "KI-Checkliste".
/// Distraktoren sind bewusst ähnlich lang wie die richtige Antwort formuliert
/// (siehe scripts/check-answer-length-bias.py - "nimm die längste" darf hier nicht funktionieren).
/// </summary>
public sealed class KiWissenGenerator : ExerciseGeneratorBase
{
    public override Subject Subject => Subject.KiWissen;

    protected override IReadOnlyDictionary<GradeLevel, IReadOnlyList<TopicFactory>> TopicsByGrade { get; } =
        new Dictionary<GradeLevel, IReadOnlyList<TopicFactory>>
        {
            [GradeLevel.Klasse6] = new List<TopicFactory>
            {
                WieKiFunktioniert,
                KiImAlltag,
                SichereNutzung
            },
            [GradeLevel.Klasse9] = new List<TopicFactory>
            {
                HalluzinationenUndFaktencheck,
                BiasUndVerantwortung,
                DeepfakesUndDatenschutz
            }
        };

    private static readonly (string Frage, string[] Optionen, string Antwort, string Erklaerung)[] WieKiFunktioniertListe =
    {
        ("Was ist eine KI am ehesten?", new[] { "Ein Computerprogramm, das Muster aus Daten gelernt hat", "Ein denkendes Wesen mit eigenen Gefühlen und Wünschen", "Ein Roboter, der alles auf der Welt sicher weiß" }, "Ein Computerprogramm, das Muster aus Daten gelernt hat",
            "KI ist Software - wie ein Taschenrechner für Sprache oder Bilder. Sie denkt und fühlt nicht."),
        ("Wie kommt eine Sprach-KI zu ihren Antworten?", new[] { "Sie setzt Wort für Wort die wahrscheinlichste Fortsetzung zusammen", "Sie schlägt jede Antwort live in einem geheimen Riesenlexikon nach", "Sie fragt im Hintergrund heimlich echte Menschen um deren Rat" }, "Sie setzt Wort für Wort die wahrscheinlichste Fortsetzung zusammen",
            "Eine Sprach-KI hat aus riesigen Textmengen gelernt, welche Wörter oft aufeinander folgen - daraus baut sie ihre Antwort."),
        ("Warum klingt eine KI oft so überzeugend?", new[] { "Weil flüssige Sprache ihr Spezialgebiet ist - auch bei Fehlern", "Weil sie grundsätzlich nur geprüfte, wahre Sätze ausgeben kann", "Weil ein Mensch jede Antwort vor dem Absenden noch kontrolliert" }, "Weil flüssige Sprache ihr Spezialgebiet ist - auch bei Fehlern",
            "Die KI ist auf gut klingende Sprache trainiert. Überzeugender Klang sagt nichts über die Richtigkeit aus."),
        ("Womit lässt sich eine Sprach-KI am besten vergleichen?", new[] { "Mit einem Bibliothekar, der aus dem Gedächtnis antwortet", "Mit einem Richter, der immer das letzte Wort behalten muss", "Mit einem Fernseher, der feste Sendungen der Reihe nach zeigt" }, "Mit einem Bibliothekar, der aus dem Gedächtnis antwortet",
            "Wie ein belesener Bibliothekar ohne Bücher: meistens richtig, manchmal verwechselt - nachprüfen bleibt dein Job."),
        ("Was bedeutet 'Training' bei einer KI?", new[] { "Das Lernen von Mustern aus sehr vielen Beispieldaten", "Ein tägliches Sportprogramm für die Computer-Hardware", "Das Auswendiglernen aller Fragen, die je gestellt wurden" }, "Das Lernen von Mustern aus sehr vielen Beispieldaten",
            "Beim Training sieht die KI Millionen Beispiele und lernt daraus Muster - kein Auswendiglernen einzelner Antworten."),
        ("Was passiert, wenn du einer KI zweimal dieselbe Frage stellst?", new[] { "Die Antwort kann jedes Mal etwas anders ausfallen", "Sie gibt immer exakt denselben Wortlaut zurück", "Sie verweigert die zweite Antwort als Wiederholung" }, "Die Antwort kann jedes Mal etwas anders ausfallen",
            "Die KI würfelt bei der Wortwahl leicht mit - deshalb variieren die Antworten. Auch das zeigt: Sie schlägt nichts nach."),
        ("Hat eine KI eigene Gefühle?", new[] { "Nein - sie kann Gefühle nur in Worten nachahmen", "Ja - sie freut sich wirklich über nette Fragen", "Ja - aber nur, wenn sie lange genug trainiert wurde" }, "Nein - sie kann Gefühle nur in Worten nachahmen",
            "Wenn eine KI 'Das freut mich!' schreibt, ist das gelerntes Sprachmuster - dahinter fühlt niemand etwas."),
        ("Woher hat eine Sprach-KI ihr 'Wissen'?", new[] { "Aus Texten, die Menschen irgendwann geschrieben haben", "Aus eigenen Experimenten, die sie nachts durchführt", "Aus einer amtlich geprüften Datenbank aller Wahrheiten" }, "Aus Texten, die Menschen irgendwann geschrieben haben",
            "Alles stammt aus menschengemachten Texten - mit deren Fehlern, Lücken und Meinungen."),
        ("Warum kennt eine KI aktuelle Ereignisse oft nicht?", new[] { "Ihr Training endete vor einem bestimmten Stichtag", "Neuigkeiten sind ihr laut Gesetz streng verboten", "Sie interessiert sich nur für sehr alte Geschichte" }, "Ihr Training endete vor einem bestimmten Stichtag",
            "Eine KI kennt nur, was bis zu ihrem Trainingsende in den Daten stand - Neueres kann sie höchstens raten."),
        ("Was kann ein Taschenrechner besser als eine Sprach-KI?", new[] { "Er rechnet garantiert fehlerfrei nach festen Regeln", "Er erklärt jede Aufgabe mit anschaulichen Beispielen", "Er versteht auch unklar formulierte Textaufgaben" }, "Er rechnet garantiert fehlerfrei nach festen Regeln",
            "Der Taschenrechner folgt exakten Rechenregeln. Eine Sprach-KI 'schätzt' auch beim Rechnen - und verrechnet sich dabei."),
        ("Eine KI schreibt: 'Ich bin mir zu 100% sicher.' Was heißt das?", new[] { "Wenig - solche Sätze sind nur gelernte Formulierungen", "Die Antwort wurde dreifach gegen Fachbücher geprüft", "Bei dieser Formulierung ist ein Irrtum ausgeschlossen" }, "Wenig - solche Sätze sind nur gelernte Formulierungen",
            "Selbstsichere Sätze sind Sprachmuster, keine Garantie. Auch eine '100% sichere' KI-Antwort kann falsch sein."),
        ("Was ist der Unterschied zwischen einer KI und einer Suchmaschine?", new[] { "Die Suchmaschine zeigt Quellen, die KI formuliert selbst", "Die KI ist immer aktueller als jede Suchmaschine", "Es gibt keinen - beide arbeiten vollkommen gleich" }, "Die Suchmaschine zeigt Quellen, die KI formuliert selbst",
            "Die Suchmaschine verlinkt echte Seiten zum Selberlesen; die KI baut eine eigene Antwort - deren Quelle du nicht siehst."),
        ("Kann eine KI etwas 'wollen', z.B. dich ärgern?", new[] { "Nein - sie verfolgt keine eigenen Absichten", "Ja - manche KIs entwickeln heimlich eigene Pläne", "Ja - aber nur, wenn man unhöflich zu ihr war" }, "Nein - sie verfolgt keine eigenen Absichten",
            "Eine KI hat keine Ziele. Was wie Absicht wirkt, ist das Ergebnis von Mustern und Zufall in der Wortwahl."),
        ("Warum macht dieselbe KI mal gute, mal schlechte Antworten?", new[] { "Die Qualität hängt stark von Frage und Thema ab", "Sie hat wie Menschen gute und schlechte Tage", "Nachts antwortet sie besser, weil weniger los ist" }, "Die Qualität hängt stark von Frage und Thema ab",
            "Zu häufigen Themen hat die KI viele Muster gelernt, zu seltenen wenige - und eine klare Frage hilft ihr zusätzlich."),
        ("Was ist ein 'Prompt'?", new[] { "Deine Eingabe bzw. Aufgabe an die KI", "Der eingebaute Hauptprozessor der KI", "Ein Gütesiegel für geprüfte KI-Antworten" }, "Deine Eingabe bzw. Aufgabe an die KI",
            "Prompt = das, was du der KI schreibst. Je klarer der Prompt, desto brauchbarer meist die Antwort."),
        ("Die LernTor-KI läuft komplett auf diesem PC. Was folgt daraus?", new[] { "Deine Fragen verlassen den Computer nicht", "Sie ist dadurch automatisch immer fehlerfrei", "Sie kann dafür nur Ja/Nein-Fragen beantworten" }, "Deine Fragen verlassen den Computer nicht",
            "Lokale KI heißt: keine Daten gehen ins Internet. Fehler machen kann sie trotzdem - lokal sagt nichts über Qualität."),
        ("Versteht eine KI den Sinn ihrer eigenen Antworten?", new[] { "Nein - sie verarbeitet Muster ohne echtes Verstehen", "Ja - sonst könnte sie gar nicht so gut formulieren", "Ja - aber nur in ihrer Hauptsprache Englisch" }, "Nein - sie verarbeitet Muster ohne echtes Verstehen",
            "Gutes Formulieren braucht kein Verstehen - die KI berechnet passende Wortfolgen, mehr steckt nicht dahinter."),
        ("Warum kann eine KI auch beim Rechnen danebenliegen?", new[] { "Sie behandelt Zahlen wie Wörter statt wie Mathematik", "Ihre Batterie wird bei langen Aufgaben zu schwach", "Rechnen wurde ihr aus Sicherheitsgründen gesperrt" }, "Sie behandelt Zahlen wie Wörter statt wie Mathematik",
            "Für eine Sprach-KI ist '7 mal 8' eine Wortfolge, kein Rechenbefehl - deshalb Mathe lieber selbst oder mit Taschenrechner prüfen."),
        ("Woran erkennst du eine gute Frage an eine KI?", new[] { "Sie ist konkret und nennt, was genau du brauchst", "Sie ist möglichst kurz, am besten nur ein Wort", "Sie beginnt immer mit einer höflichen Anrede" }, "Sie ist konkret und nennt, was genau du brauchst",
            "'Erkläre Fotosynthese für Klasse 6 in 3 Sätzen' bringt mehr als nur 'Fotosynthese?' - Kontext hilft der KI."),
        ("Was ist die wichtigste Grundregel im Umgang mit KI-Antworten?", new[] { "Als Startpunkt nutzen und Wichtiges selbst prüfen", "Alles glauben, denn Computer machen keine Fehler", "Nichts glauben und KI grundsätzlich nie benutzen" }, "Als Startpunkt nutzen und Wichtiges selbst prüfen",
            "KI ist ein starkes Werkzeug für Ideen und Erklärungen - aber bei Fakten gilt: prüfen, bevor du dich darauf verlässt.")
    };

    private static QuizQuestion WieKiFunktioniert(Random r)
    {
        var f = WieKiFunktioniertListe[r.Next(WieKiFunktioniertListe.Length)];
        return new QuizQuestion
        {
            Id = NewId(), Subject = Subject.KiWissen, GradeLevel = GradeLevel.Klasse6,
            Topic = "Wie KI funktioniert", Type = QuestionType.MultipleChoice,
            Prompt = f.Frage, Options = f.Optionen, CorrectAnswers = new[] { f.Antwort }, Explanation = f.Erklaerung,
            HelpHint = "KI = Computerprogramm, das Muster aus Daten gelernt hat. Es denkt/fühlt nicht und schlägt nichts nach - wie ein Bibliothekar, der nur aus dem Gedächtnis antwortet."
        };
    }

    private static readonly (string Frage, string[] Optionen, string Antwort, string Erklaerung)[] KiImAlltagListe =
    {
        ("Wo steckt im Handy fast immer KI drin?", new[] { "In Kamera, Autokorrektur und Sprachassistent", "Nur in teuren Spiele-Apps ab 18 Jahren", "Im Akku und im Ladekabel des Geräts" }, "In Kamera, Autokorrektur und Sprachassistent",
            "Gesichter scharfstellen, Tippfehler korrigieren, Sprache verstehen - alles KI, meist unsichtbar eingebaut."),
        ("Warum schlägt dir eine Video-App genau diese Videos vor?", new[] { "Eine KI hat aus deinem Verhalten gelernt, was dich hält", "Die Videos werden für alle Nutzer in gleicher Reihenfolge gezeigt", "Ein Redaktionsteam wählt sie jeden Morgen von Hand aus" }, "Eine KI hat aus deinem Verhalten gelernt, was dich hält",
            "Empfehlungs-KIs analysieren, was du schaust und likst - ihr Ziel ist, dass du möglichst lange bleibst."),
        ("Was ist das Hauptziel von Empfehlungs-Algorithmen?", new[] { "Deine Zeit in der App so lang wie möglich zu machen", "Dir jeden Tag etwas völlig Neues beizubringen", "Dich so schnell wie möglich offline zu bringen" }, "Deine Zeit in der App so lang wie möglich zu machen",
            "Mehr Zeit in der App = mehr Werbung. Wer das weiß, kann bewusster entscheiden, wann Schluss ist."),
        ("Die Autokorrektur ändert 'Berln' zu 'Berlin'. Was passiert da?", new[] { "Eine KI vergleicht dein Wort mit gelernten Mustern", "Das Handy fragt kurz bei einem Wörterbuch-Amt an", "Ein Mitarbeiter liest mit und bessert live aus" }, "Eine KI vergleicht dein Wort mit gelernten Mustern",
            "Die Korrektur-KI hat Millionen Texte gesehen und erkennt: 'Berln' ist fast sicher 'Berlin'."),
        ("Ein Übersetzer überträgt Deutsch in Türkisch. Wie macht er das?", new[] { "Eine KI hat aus Millionen Satzpaaren übersetzen gelernt", "Er sucht jeden Satz einzeln in einem Riesen-Wörterbuch", "Muttersprachler tippen die Übersetzung blitzschnell ein" }, "Eine KI hat aus Millionen Satzpaaren übersetzen gelernt",
            "Übersetzungs-KIs lernen aus riesigen Mengen zweisprachiger Texte - Wort-für-Wort-Nachschlagen wäre viel zu ungenau."),
        ("Woran merkst du, dass eine Spiel-Figur KI-gesteuert ist?", new[] { "Sie reagiert auf deine Züge, folgt aber Programmregeln", "Sie gewinnt ausnahmslos jede einzelne Runde", "Sie bewegt sich nur, wenn du das Spiel neu startest" }, "Sie reagiert auf deine Züge, folgt aber Programmregeln",
            "Spiel-KI wirkt lebendig, folgt aber Regeln der Entwickler - oft absichtlich so, dass du gewinnen kannst."),
        ("Der Sprachassistent versteht 'Wecker auf 7 Uhr'. Welche KI arbeitet da?", new[] { "Spracherkennung, die Schall in Text und Befehle umwandelt", "Eine Kamera, die deine Lippenbewegungen genau abliest", "Ein Anrufdienst, der die Frage an Menschen weiterleitet" }, "Spracherkennung, die Schall in Text und Befehle umwandelt",
            "Spracherkennungs-KI wandelt Schallwellen in Text um und erkennt darin den Befehl - alles in Sekundenbruchteilen."),
        ("Warum zeigt dir dein Feed andere Beiträge als deinen Freunden?", new[] { "Die KI sortiert für jede Person nach deren Verhalten", "Beiträge werden streng nach Uhrzeit für alle gleich sortiert", "Deine Freunde haben schlicht zu wenige Konten abonniert" }, "Die KI sortiert für jede Person nach deren Verhalten",
            "Jeder Feed ist eine persönliche KI-Auswahl. Deshalb sieht jeder eine andere 'Wirklichkeit' - gut zu wissen!"),
        ("Was ist ein 'Filterblasen'-Effekt?", new[] { "Man sieht fast nur noch Inhalte, die zur eigenen Meinung passen", "Ein Wasserschaden, der Handy-Lautsprecher leiser macht", "Ein Bildfilter, der Fotos automatisch aufhübscht" }, "Man sieht fast nur noch Inhalte, die zur eigenen Meinung passen",
            "Empfehlungs-KIs zeigen dir mehr von dem, was dir gefällt - andere Sichtweisen tauchen immer seltener auf."),
        ("Eine Musik-App erstellt dir eine 'Für dich'-Playlist. Woher kennt sie deinen Geschmack?", new[] { "Aus deinem bisherigen Hörverhalten und ähnlichen Nutzern", "Sie hat dein Zimmer per Mikrofon rund um die Uhr belauscht", "Die Lieblingslieder wurden bei deiner Schule erfragt" }, "Aus deinem bisherigen Hörverhalten und ähnlichen Nutzern",
            "Die KI vergleicht dein Hörverhalten mit Millionen anderer Profile: 'Wem X gefällt, dem gefällt oft auch Y.'"),
        ("Was können Navigations-Apps dank KI besonders gut?", new[] { "Staus vorhersagen und schnellere Routen vorschlagen", "Rote Ampeln für dich auf Grün umschalten lassen", "Das Auto bei Regen automatisch selbst lenken" }, "Staus vorhersagen und schnellere Routen vorschlagen",
            "Aus den Bewegungsdaten vieler Handys berechnet die KI, wo es sich staut - und leitet dich vorbei."),
        ("Ein Online-Shop zeigt: 'Kunden kauften auch...'. Was steckt dahinter?", new[] { "Eine KI, die Kaufmuster vieler Kunden vergleicht", "Der Zufall - die Produkte wechseln stündlich wild durch", "Eine Liste, die der Chef persönlich zusammenstellt" }, "Eine KI, die Kaufmuster vieler Kunden vergleicht",
            "Empfehlungs-KI im Shop: Wer A kaufte, kaufte oft auch B. Ziel ist natürlich, dass du mehr kaufst."),
        ("Wie hilft KI bei der E-Mail?", new[] { "Sie sortiert Spam und Werbung automatisch aus", "Sie beantwortet alle Mails ungefragt selbst", "Sie druckt wichtige Mails zu Hause direkt aus" }, "Sie sortiert Spam und Werbung automatisch aus",
            "Spam-Filter sind trainierte KIs: Aus Millionen Beispielen haben sie gelernt, wie Betrugs- und Werbemails aussehen."),
        ("Die Handykamera macht nachts erstaunlich helle Fotos. Wieso?", new[] { "KI rechnet mehrere Aufnahmen zu einem Bild zusammen", "Der Blitz brennt einfach zehnmal länger als früher", "Die Linse sammelt Mondlicht in einem Spezialspeicher" }, "KI rechnet mehrere Aufnahmen zu einem Bild zusammen",
            "Nachtmodus = KI-Bildverarbeitung: viele kurze Aufnahmen werden verrechnet und aufgehellt - das Foto ist teils 'errechnet'."),
        ("Was bedeutet es, wenn eine App 'personalisiert' ist?", new[] { "Sie passt Inhalte an dein bisheriges Verhalten an", "Sie funktioniert nur mit Vor- und Nachnamen", "Sie wurde extra für genau ein Handymodell gebaut" }, "Sie passt Inhalte an dein bisheriges Verhalten an",
            "Personalisierung heißt: Die KI baut dir deine eigene Version der App - nach dem, was du bisher getan hast."),
        ("Warum fühlen sich manche Apps so schwer wegklickbar an?", new[] { "Sie sind bewusst so gestaltet, dass Aufhören schwerfällt", "Der Bildschirm wird bei diesen Apps technisch gesperrt", "Das ist Einbildung - alle Apps wirken völlig gleich" }, "Sie sind bewusst so gestaltet, dass Aufhören schwerfällt",
            "Endloses Scrollen, Autoplay, Belohnungen - vieles ist absichtlich so designt. Das zu wissen macht das Aufhören leichter."),
        ("Ein Fitness-Armband erkennt, ob du läufst oder schläfst. Wie?", new[] { "KI deutet die Muster deiner Bewegungssensoren", "Es fragt dich nachts leise über den Lautsprecher", "Ein Arzt wertet die Daten jeden Morgen aus" }, "KI deutet die Muster deiner Bewegungssensoren",
            "Die Sensor-Daten (Beschleunigung, Puls) ergeben Muster - eine trainierte KI ordnet sie Aktivitäten zu."),
        ("Was haben fast alle Alltags-KIs gemeinsam?", new[] { "Sie lernen aus Daten und bleiben trotzdem Programme", "Sie werden von kleinen Robotern im Gerät bedient", "Sie funktionieren nur mit ständiger Internetverbindung" }, "Sie lernen aus Daten und bleiben trotzdem Programme",
            "Ob Kamera, Feed oder Übersetzer: Es sind Programme mit gelernten Mustern - keine Wesen, und manche laufen sogar offline."),
        ("Welche Frage lohnt sich bei jeder Empfehlung einer App?", new[] { "Wem nützt es, dass ich genau das jetzt sehe?", "Wie viele Megabyte hat dieses Video wohl?", "Welche Uhrzeit ist gerade in Australien?" }, "Wem nützt es, dass ich genau das jetzt sehe?",
            "Empfehlungen sind nie neutral - meist verdient jemand daran, dass du bleibst. Die Frage nach dem 'Wem nützt es?' schützt dich."),
        ("KI plant in Krankenhäusern OP-Termine und erkennt Röntgen-Auffälligkeiten. Was zeigt das?", new[] { "KI kann als Werkzeug echte Profis unterstützen", "KI hat Ärztinnen und Ärzte längst komplett ersetzt", "KI darf in Deutschland gar nicht eingesetzt werden" }, "KI kann als Werkzeug echte Profis unterstützen",
            "In guten Händen ist KI ein Hilfsmittel: Sie unterstützt Fachleute, entscheidet aber nicht allein - der Mensch bleibt verantwortlich.")
    };

    private static QuizQuestion KiImAlltag(Random r)
    {
        var f = KiImAlltagListe[r.Next(KiImAlltagListe.Length)];
        return new QuizQuestion
        {
            Id = NewId(), Subject = Subject.KiWissen, GradeLevel = GradeLevel.Klasse6,
            Topic = "KI im Alltag", Type = QuestionType.MultipleChoice,
            Prompt = f.Frage, Options = f.Optionen, CorrectAnswers = new[] { f.Antwort }, Explanation = f.Erklaerung,
            HelpHint = "KI steckt in Kamera, Autokorrektur, Übersetzer, Feeds und Spielen. Empfehlungen wollen dich halten - frag dich: Wem nützt es, dass ich das sehe?"
        };
    }

    private static readonly (string Frage, string[] Optionen, string Antwort, string Erklaerung)[] SichereNutzungListe =
    {
        ("Was solltest du einer Online-KI niemals verraten?", new[] { "Vollen Namen, Adresse, Schule oder eigene Fotos", "Deine Meinung zu einem Buch oder einem Film", "Fragen zu Hausaufgaben oder Schulthemen" }, "Vollen Namen, Adresse, Schule oder eigene Fotos",
            "Alles, was du online eintippst, kann gespeichert werden. Persönliche Daten gehören nicht in fremde KI-Chats."),
        ("Die KI nennt dir eine Jahreszahl für deinen Vortrag. Was tust du?", new[] { "In einer zweiten Quelle nachprüfen, bevor du sie nutzt", "Direkt übernehmen - Zahlen kann eine KI nicht verwechseln", "Zur Sicherheit einfach eine ganz andere Zahl nehmen" }, "In einer zweiten Quelle nachprüfen, bevor du sie nutzt",
            "Gerade Zahlen, Namen und Daten verwechselt KI gern. Kurz gegenprüfen kostet eine Minute und rettet deinen Vortrag."),
        ("Ein Bild im Netz sieht unglaublich aus. Was prüfst du zuerst?", new[] { "Wer es gepostet hat und ob seriöse Seiten es auch zeigen", "Wie viele Likes und Herzen es schon gesammelt hat", "Ob die Farben darauf schön kräftig leuchten" }, "Wer es gepostet hat und ob seriöse Seiten es auch zeigen",
            "Quelle vor Likes: Wenn nur ein unbekanntes Konto das Bild zeigt und keine seriöse Seite berichtet, ist Skepsis angesagt."),
        ("Woran erkennst du KI-generierte Bilder oft?", new[] { "Seltsame Hände, unlesbare Schrift, zu glatte Haut", "Am kleinen roten Rahmen um jedes solche Bild", "KI-Bilder sind grundsätzlich schwarz-weiß gehalten" }, "Seltsame Hände, unlesbare Schrift, zu glatte Haut",
            "Typische KI-Fehler: sechs Finger, Buchstabensalat im Hintergrund, wachsartige Haut, unmögliches Licht. Genau hinsehen hilft."),
        ("Darfst du einen KI-Text als deine eigene Hausaufgabe abgeben?", new[] { "Nein - das ist Täuschung, auch wenn es niemand merkt", "Ja - dafür wurden solche Programme schließlich gebaut", "Ja - solange du drei Wörter darin veränderst" }, "Nein - das ist Täuschung, auch wenn es niemand merkt",
            "Fremde Texte als eigene ausgeben ist Täuschung - egal ob von KI oder Mitschüler. KI darf dir beim Verstehen helfen, nicht beim Schummeln."),
        ("Wofür ist KI bei Hausaufgaben eine gute Hilfe?", new[] { "Erklären lassen, was du nicht verstanden hast", "Alle Aufgaben komplett für dich lösen lassen", "Ausreden erfinden, warum die Mappe fehlt" }, "Erklären lassen, was du nicht verstanden hast",
            "KI als Erklär-Helfer nutzen ('Erkläre mir Brüche einfacher') ist schlau - abschreiben lernt für dich nicht."),
        ("Ein KI-Chat schreibt etwas, das dich verletzt oder erschreckt. Was tust du?", new[] { "Den Chat beenden und einem Erwachsenen davon erzählen", "Höflich bleiben, damit die KI nicht beleidigt ist", "So lange weiterschreiben, bis sie sich entschuldigt" }, "Den Chat beenden und einem Erwachsenen davon erzählen",
            "Du schuldest einer KI nichts - sie fühlt nichts. Bei komischen oder verletzenden Antworten: raus und Bescheid sagen."),
        ("Warum solltest du KI-Antworten zu Gesundheitsfragen besonders misstrauen?", new[] { "Falsche Tipps können hier direkt schaden - das ist Arztsache", "Gesundheitsthemen sind für KI-Programme zu langweilig", "Solche Antworten sind immer kostenpflichtig" }, "Falsche Tipps können hier direkt schaden - das ist Arztsache",
            "Bei Gesundheit, Medikamenten oder Verletzungen gilt: echte Fachleute fragen. Eine halluzinierte Dosierung kann gefährlich sein."),
        ("Was bedeutet es, dass die LernTor-KI 'lokal' läuft?", new[] { "Sie arbeitet nur auf diesem PC, ohne Internet-Versand", "Sie funktioniert nur im Umkreis deiner Stadt", "Sie wurde von einer Firma aus der Nachbarschaft gebaut" }, "Sie arbeitet nur auf diesem PC, ohne Internet-Versand",
            "Lokal = das Modell liegt auf dem PC, deine Fragen verlassen ihn nicht. Bei Online-KIs weißt du nie genau, was gespeichert wird."),
        ("Jemand schickt dir ein 'geheimes Promi-Video'. Es wirkt echt. Was nun?", new[] { "Misstrauisch bleiben und es nicht weiterverbreiten", "Sofort allen Freunden schicken, bevor es gelöscht wird", "Es glauben, denn Videos kann man nicht fälschen" }, "Misstrauisch bleiben und es nicht weiterverbreiten",
            "Videos sind heute fälschbar. Wer Unklares weiterleitet, verbreitet vielleicht eine Lüge - im Zweifel: stoppen statt teilen."),
        ("Welches Passwort-Verhalten ist bei KI-Diensten richtig?", new[] { "Passwörter gehören niemals in einen Chat - auch nicht 'zum Testen'", "Das Passwort nur dann nennen, wenn die KI höflich und mit gutem Grund danach fragt", "Ein altes, nicht mehr genutztes Passwort ist okay" }, "Passwörter gehören niemals in einen Chat - auch nicht 'zum Testen'",
            "Kein seriöser Dienst braucht dein Passwort im Chat. Einmal getippt, hast du keine Kontrolle mehr darüber."),
        ("Die KI hat dir bei einem Text geholfen. Was ist fair?", new[] { "Ehrlich sagen, dass du KI-Hilfe benutzt hast", "Die Hilfe verschweigen - das prüft ja niemand", "Behaupten, ein Profi habe den Text geschrieben" }, "Ehrlich sagen, dass du KI-Hilfe benutzt hast",
            "Transparenz ist fair: Wer offen sagt, wo KI geholfen hat, muss nichts verstecken - so bleibt deine Leistung ehrlich."),
        ("Warum solltest du nicht stundenlang mit einer KI 'befreundet' chatten?", new[] { "Die KI simuliert Nähe nur - echte Freunde sind unersetzbar", "KIs werden nach einer Stunde automatisch unhöflich", "Lange Chats machen den Computer dauerhaft langsamer" }, "Die KI simuliert Nähe nur - echte Freunde sind unersetzbar",
            "Eine KI spiegelt Freundlichkeit, aber niemand ist wirklich da. Für Sorgen und Freundschaft sind echte Menschen wichtig."),
        ("Was ist eine gute Regel für KI-Bilder in deinen Referaten?", new[] { "Kennzeichnen, wenn ein Bild KI-generiert ist", "KI-Bilder heimlich als eigene Fotos ausgeben", "Nur Bilder ohne Menschen darauf verwenden" }, "Kennzeichnen, wenn ein Bild KI-generiert ist",
            "Ehrlichkeit gilt auch für Bilder: 'Bild: KI-generiert' als Hinweis ist fair und völlig in Ordnung."),
        ("Eine App verlangt Kamera- UND Standortzugriff für einen KI-Filter. Was denkst du?", new[] { "Stutzig werden - braucht ein Filter wirklich meinen Standort?", "Zustimmen - je mehr Zugriffe, desto besser die Fotos", "Egal - Zugriffsrechte haben keine echte Bedeutung" }, "Stutzig werden - braucht ein Filter wirklich meinen Standort?",
            "Frag bei jeder Berechtigung: Braucht die App das für ihre Aufgabe? Ein Fotofilter braucht deinen Standort nicht."),
        ("Was passiert mit Fotos, die du in fremde KI-Apps hochlädst?", new[] { "Das ist oft unklar - sie könnten gespeichert und weiterverwendet werden", "Sie werden nach genau 24 Stunden von allen Servern weltweit automatisch gelöscht", "Sie bleiben gesetzlich geschützt für immer nur bei dir" }, "Das ist oft unklar - sie könnten gespeichert und weiterverwendet werden",
            "Viele Dienste nutzen Hochgeladenes weiter, z.B. fürs Training. Lade nur hoch, was fremd gesehen werden dürfte - eigene Fotos besser nicht."),
        ("Deine KI-Antwort widerspricht deinem Schulbuch. Was gilt?", new[] { "Erst mal dem geprüften Schulbuch - und nachfragen", "Immer der KI, denn sie ist moderner als Bücher", "Keinem von beiden - das Thema einfach weglassen" }, "Erst mal dem geprüften Schulbuch - und nachfragen",
            "Schulbücher sind von Fachleuten geprüft, KI-Antworten nicht. Bei Widerspruch: Buch schlägt Bot - und die Lehrkraft fragen."),
        ("Was ist 'Quellenkritik' in einem Satz?", new[] { "Prüfen, wer etwas sagt und wie vertrauenswürdig das ist", "Möglichst viele Quellen wortwörtlich abschreiben", "Nur Quellen nutzen, die deine Meinung bestätigen" }, "Prüfen, wer etwas sagt und wie vertrauenswürdig das ist",
            "Quellenkritik fragt: Wer sagt das? Woher weiß er es? Wem nützt es? Das funktioniert bei Webseiten wie bei KI-Antworten."),
        ("Ein Freund glaubt alles, was 'seine' KI sagt. Was rätst du ihm?", new[] { "KI ist ein Helfer mit Fehlern - wichtige Sachen prüfen", "Recht hat er - Computer irren sich grundsätzlich nie", "Er soll lieber einer anderen, klügeren KI glauben" }, "KI ist ein Helfer mit Fehlern - wichtige Sachen prüfen",
            "Blindes Vertrauen ist der häufigste KI-Fehler von uns Menschen. Werkzeug ja, Wahrheitsmaschine nein."),
        ("Welcher Satz fasst sicheren KI-Umgang am besten zusammen?", new[] { "Neugierig nutzen, kritisch prüfen, Privates schützen", "Alles vermeiden, was mit Computern zu tun hat", "Der KI vertrauen, damit sie dir auch vertraut" }, "Neugierig nutzen, kritisch prüfen, Privates schützen",
            "Genau das ist die Checkliste: KI ruhig ausprobieren und nutzen - aber Fakten prüfen und persönliche Daten für dich behalten.")
    };

    private static QuizQuestion SichereNutzung(Random r)
    {
        var f = SichereNutzungListe[r.Next(SichereNutzungListe.Length)];
        return new QuizQuestion
        {
            Id = NewId(), Subject = Subject.KiWissen, GradeLevel = GradeLevel.Klasse6,
            Topic = "KI-Checkliste: Sicher nutzen", Type = QuestionType.MultipleChoice,
            Prompt = f.Frage, Options = f.Optionen, CorrectAnswers = new[] { f.Antwort }, Explanation = f.Erklaerung,
            HelpHint = "Die KI-Checkliste: Privates nie eintippen, Fakten in zweiter Quelle prüfen, KI-Hilfe ehrlich angeben, Unklares nicht weiterleiten."
        };
    }

    // ----- Klasse 9 -----

    private static readonly (string Frage, string[] Optionen, string Antwort, string Erklaerung)[] HalluzinationListe =
    {
        ("Was ist eine 'Halluzination' bei einer KI?", new[] { "Erfundene Inhalte, die völlig überzeugend klingen", "Ein Bildschirmflackern bei zu langer Nutzung", "Eine Antwort in einer unerwarteten Fremdsprache" }, "Erfundene Inhalte, die völlig überzeugend klingen",
            "Die KI 'weiß' nicht, was wahr ist - sie erzeugt wahrscheinliche Sätze. Manchmal sind die eben erfunden, aber perfekt formuliert."),
        ("Warum halluzinieren Sprach-KIs überhaupt?", new[] { "Sie vervollständigen Muster statt Fakten nachzuschlagen", "Ihre Server stehen in zu warmen Rechenzentren", "Sie wollen in Tests interessanter und klüger wirken, als sie wirklich sind" }, "Sie vervollständigen Muster statt Fakten nachzuschlagen",
            "Das Modell berechnet plausible Fortsetzungen. Fehlt Wissen, füllt es die Lücke mit Wahrscheinlichem - ohne es zu merken."),
        ("Die KI zitiert ein Buch mit Autor und Seitenzahl. Das Buch existiert nicht. Wie heißt das?", new[] { "Halluzination - erfundene Quelle in seriösem Gewand", "Plagiat - die KI hat das Buch heimlich kopiert", "Zensur - das Buch wurde nachträglich gelöscht" }, "Halluzination - erfundene Quelle in seriösem Gewand",
            "Erfundene Quellenangaben sind eine klassische Halluzination - gerade weil sie so seriös aussehen, unbedingt prüfen."),
        ("Welche Angaben halluziniert KI besonders häufig?", new[] { "Zahlen, Namen, Zitate und Quellenangaben", "Ausschließlich Sportergebnisse vom Wochenende", "Nur Texte, die länger als tausend Wörter sind" }, "Zahlen, Namen, Zitate und Quellenangaben",
            "Präzise Fakten (wer, wann, wie viel, wo steht das) sind die typischen Schwachstellen - genau die solltest du prüfen."),
        ("Was ist der beste Fakten-Check für eine wichtige KI-Aussage?", new[] { "Zwei unabhängige, seriöse Quellen suchen", "Dieselbe KI fragen, ob sie sich sicher ist", "Die Aussage besonders langsam nochmal lesen" }, "Zwei unabhängige, seriöse Quellen suchen",
            "Die KI zu fragen, ob sie sicher ist, bringt nichts - sie bestätigt auch Falsches souverän. Unabhängige Quellen sind der Maßstab."),
        ("Warum hilft 'Frag die KI einfach nochmal' NICHT als Beweis?", new[] { "Sie kann denselben Fehler erneut selbstsicher wiederholen", "Doppelte Fragen sind bei KI-Diensten nicht erlaubt", "Die zweite Antwort ist automatisch immer die falsche" }, "Sie kann denselben Fehler erneut selbstsicher wiederholen",
            "Wiederholung ist keine Bestätigung: Das Modell kann konsequent dieselbe falsche Angabe reproduzieren."),
        ("Woran erkennst du eine vertrauenswürdige Quelle im Netz?", new[] { "Impressum, erkennbarer Autor und nachprüfbare Belege", "Viele Ausrufezeichen und GROSSGESCHRIEBENE Wörter", "Ein modernes Design mit vielen bunten Animationen" }, "Impressum, erkennbarer Autor und nachprüfbare Belege",
            "Seriosität erkennt man an Verantwortlichen und Belegen - nicht an Design oder Lautstärke der Sprache."),
        ("Die KI beantwortet deine Frage zur gestrigen Wahl. Was ist zu bedenken?", new[] { "Ihr Wissen endet am Trainings-Stichtag - Aktuelles kann erfunden sein", "Wahlergebnisse ändern sich sowieso noch wochenlang und sind deshalb nie zitierfähig", "Politik ist für KI-Systeme gesetzlich gesperrt" }, "Ihr Wissen endet am Trainings-Stichtag - Aktuelles kann erfunden sein",
            "Nach dem Stichtag ist alles Vermutung. Für Aktuelles: Nachrichtenquellen statt Sprachmodell."),
        ("Was bedeutet 'plausibel klingt nicht gleich wahr'?", new[] { "Gut Formuliertes kann trotzdem komplett falsch sein", "Nur komplizierte Sätze enthalten echte Wahrheit", "Wahre Sätze erkennt man an ihrer Länge" }, "Gut Formuliertes kann trotzdem komplett falsch sein",
            "KI ist Weltmeister im Plausibel-Klingen. Deine Prüfung muss beim Inhalt ansetzen, nicht beim Stil."),
        ("Wann ist eine KI-Antwort besonders fehleranfällig?", new[] { "Bei sehr speziellen Nischenthemen mit wenig Trainingsmaterial", "Bei einfachen Alltagsfragen wie Kochrezepten oder bekannten Sprichwörtern", "Direkt nach Mitternacht wegen der Serverwartung" }, "Bei sehr speziellen Nischenthemen mit wenig Trainingsmaterial",
            "Wenig Trainingsdaten = wenig gelernte Muster = mehr Lücken, die das Modell mit Erfundenem füllt."),
        ("Wie formulierst du Prompts, die Halluzinationen unwahrscheinlicher machen?", new[] { "Präzise fragen und um Quellen oder Unsicherheits-Hinweise bitten", "Möglichst vage bleiben, damit die KI freier antworten kann", "Die Frage komplett in Großbuchstaben eintippen" }, "Präzise fragen und um Quellen oder Unsicherheits-Hinweise bitten",
            "Klare, eingegrenzte Fragen ('Nenne nur belegte...', 'Sag, wenn du unsicher bist') reduzieren Erfundenes - verhindern es aber nicht ganz."),
        ("Die KI gesteht: 'Da bin ich unsicher.' Was ist davon zu halten?", new[] { "Ein nützlicher Hinweis - aber auch sichere Antworten können falsch sein", "Solche Sätze bedeuten im Umkehrschluss, dass alle übrigen Angaben garantiert stimmen", "Ein Programmfehler, den man dem Hersteller melden sollte" }, "Ein nützlicher Hinweis - aber auch sichere Antworten können falsch sein",
            "Unsicherheits-Hinweise sind hilfreich, doch das Fehlen davon ist keine Garantie - die Selbstsicherheit ist Teil des Sprachmusters."),
        ("Was ist der Unterschied zwischen einer Lüge und einer Halluzination?", new[] { "Lügen brauchen Absicht - die KI hat keine", "Halluzinationen sind schlimmer, weil sie absichtlich passieren", "Es gibt keinen - beides ist bewusste Täuschung" }, "Lügen brauchen Absicht - die KI hat keine",
            "Die KI täuscht nicht absichtlich, sie produziert fehlerhafte Muster. Für dich bleibt die Folge gleich: prüfen."),
        ("Für ein Referat übernimmst du drei KI-'Fakten' ungeprüft. Was riskierst du?", new[] { "Vor der Klasse mit erfundenen Angaben dazustehen", "Nichts - im Unterricht prüft solche Details niemand", "Eine Urheberrechtsklage des KI-Herstellers" }, "Vor der Klasse mit erfundenen Angaben dazustehen",
            "Halluzinierte Fakten fliegen oft auf - spätestens, wenn jemand nachfragt. Der Fakten-Check vorher ist dein Schutz."),
        ("Welcher Arbeitsablauf mit KI ist am sinnvollsten?", new[] { "KI für Entwurf und Ideen, dann selbst prüfen und überarbeiten", "KI-Ergebnis direkt abgeben, Zeit ist schließlich Geld", "Erst alles selbst schreiben, dann von der KI loben lassen" }, "KI für Entwurf und Ideen, dann selbst prüfen und überarbeiten",
            "KI liefert Rohmaterial. Prüfen, korrigieren und verantworten musst du - so entsteht ehrliche, gute Arbeit."),
        ("Warum erfinden KIs manchmal Details zu realen Personen?", new[] { "Sie mischen gelernte Muster verschiedener Personen zusammen", "Prominente bezahlen für geschönte KI-Beschreibungen", "Personendaten sind im Training grundsätzlich verboten" }, "Sie mischen gelernte Muster verschiedener Personen zusammen",
            "Ähnliche Namen, ähnliche Berufe - die Muster verschwimmen. Deshalb bei Personenangaben besonders vorsichtig sein."),
        ("Was ist ein 'Fakten-Dreieck' für KI-Aussagen?", new[] { "KI-Aussage, unabhängige Quelle und gesunder Menschenverstand abgleichen", "Drei verschiedene KIs dieselbe Frage stellen und die häufigste Antwort als bewiesen ansehen", "Die Antwort dreimal hintereinander durchlesen" }, "KI-Aussage, unabhängige Quelle und gesunder Menschenverstand abgleichen",
            "Drei Prüfsteine: Was sagt die KI? Was sagen seriöse Quellen? Klingt es überhaupt logisch? Erst wenn alles zusammenpasst, übernehmen."),
        ("Die KI rechnet dir eine Physik-Aufgabe vor. Das Ergebnis wirkt seltsam. Was tun?", new[] { "Selbst nachrechnen - Sprach-KIs verrechnen sich öfter", "Übernehmen - Computer rechnen prinzipiell fehlerfrei", "Die Aufgabe weglassen und eine leichtere wählen" }, "Selbst nachrechnen - Sprach-KIs verrechnen sich öfter",
            "Sprachmodelle behandeln Rechnungen als Text und machen dabei echte Rechenfehler - dein Taschenrechner ist hier die bessere Instanz."),
        ("Wobei ist eine Sprach-KI trotz Halluzinationsgefahr richtig stark?", new[] { "Erklären, zusammenfassen und Texte verbessern", "Amtliche Dokumente rechtsgültig unterschreiben", "Zukünftige Lottozahlen zuverlässig vorhersagen" }, "Erklären, zusammenfassen und Texte verbessern",
            "Bei Sprache glänzt KI: verständlich erklären, kürzen, umformulieren. Kritisch wird es bei präzisen Fakten - dort prüfen."),
        ("Welche Haltung gegenüber KI-Antworten ist die klügste?", new[] { "Wie bei einem klugen Bekannten: zuhören, aber nachprüfen", "Wie bei einem Gesetzestext: jedes Wort ist bindend", "Wie bei einem Märchen: nichts davon stimmt jemals" }, "Wie bei einem klugen Bekannten: zuhören, aber nachprüfen",
            "Weder blind glauben noch pauschal ablehnen: KI-Antworten sind wie Tipps eines belesenen Bekannten - oft nützlich, nie automatisch wahr.")
    };

    private static QuizQuestion HalluzinationenUndFaktencheck(Random r)
    {
        var f = HalluzinationListe[r.Next(HalluzinationListe.Length)];
        return new QuizQuestion
        {
            Id = NewId(), Subject = Subject.KiWissen, GradeLevel = GradeLevel.Klasse9,
            Topic = "Halluzinationen und Fakten-Check", Type = QuestionType.MultipleChoice,
            Prompt = f.Frage, Options = f.Optionen, CorrectAnswers = new[] { f.Antwort }, Explanation = f.Erklaerung,
            HelpHint = "Halluzination = überzeugend klingender erfundener Inhalt (oft Zahlen, Namen, Quellen). Gegenmittel: zwei unabhängige seriöse Quellen - nicht die KI selbst fragen."
        };
    }

    private static readonly (string Frage, string[] Optionen, string Antwort, string Erklaerung)[] BiasListe =
    {
        ("Was bedeutet 'Bias' bei KI-Systemen?", new[] { "Schieflagen aus den Trainingsdaten prägen die Antworten", "Ein Hardware-Defekt in der Grafikkarte des Servers", "Die absichtliche Bosheit mancher Programme" }, "Schieflagen aus den Trainingsdaten prägen die Antworten",
            "KI lernt aus menschlichen Texten - samt deren Vorurteilen. Diese Schieflagen reproduziert sie dann, ohne es zu 'wollen'."),
        ("Die Bild-KI zeigt auf 'ein Arzt' fast nur Männer. Warum?", new[] { "In den Trainingsbildern waren Ärzte überwiegend männlich", "Das Programm hält Frauen für schlechtere Ärztinnen", "Männerbilder sind für Computer leichter zu zeichnen" }, "In den Trainingsbildern waren Ärzte überwiegend männlich",
            "Die KI spiegelt die Verteilung ihrer Daten - nicht die Wirklichkeit und schon gar nicht, was richtig wäre."),
        ("Warum ist Bias mehr als ein Schönheitsfehler?", new[] { "KI-Entscheidungen können Menschen real benachteiligen", "Er verbraucht unnötig viel teuren Serverstrom", "Er macht die Antworten unangenehm lang" }, "KI-Entscheidungen können Menschen real benachteiligen",
            "Wenn KI bei Bewerbungen, Krediten oder Polizei-Software mitentscheidet, werden Daten-Schieflagen zu echter Ungerechtigkeit."),
        ("Eine KI wurde vor allem mit englischen Texten trainiert. Welche Folge ist typisch?", new[] { "Sie ist bei deutschen oder türkischen Themen deutlich schwächer", "Sie weigert sich, andere Sprachen überhaupt anzuzeigen", "Sie übersetzt alles zuerst laut vor sich hin" }, "Sie ist bei deutschen oder türkischen Themen deutlich schwächer",
            "Weniger Trainingsdaten zu einer Sprache oder Kultur = dünnere Muster = mehr Fehler und Klischees genau dort."),
        ("Wer trägt die Verantwortung, wenn eine KI diskriminierende Ergebnisse liefert?", new[] { "Die Menschen und Firmen, die sie bauen und einsetzen", "Niemand - Software kann man nicht verantwortlich machen", "Die Nutzer, weil sie die falschen Fragen stellen" }, "Die Menschen und Firmen, die sie bauen und einsetzen",
            "KI ist ein Produkt. Für Auswahl der Daten, Tests und Einsatz haften Menschen - 'das war der Algorithmus' zählt nicht."),
        ("Was ist eine Filterblase im Zusammenhang mit Bias?", new[] { "Die KI zeigt dir bevorzugt, was deine Sicht bestätigt", "Ein Schutzprogramm gegen Viren in sozialen Medien", "Eine Funktion, die alle Meinungen gleich oft zeigt" }, "Die KI zeigt dir bevorzugt, was deine Sicht bestätigt",
            "Empfehlungs-KIs verstärken, was dir gefällt. So entsteht der Eindruck, 'alle' dächten wie du - ein Bias deines Feeds."),
        ("Wie kannst du Bias in KI-Antworten selbst entlarven?", new[] { "Gegenfragen stellen und bewusst andere Perspektiven suchen", "Die Antwort mehrfach kopieren und die Kopien Wort für Wort miteinander vergleichen", "Nur noch Ja/Nein-Fragen an die KI stellen" }, "Gegenfragen stellen und bewusst andere Perspektiven suchen",
            "Frag aktiv nach anderen Sichtweisen ('Wie sehen das andere Gruppen?') und prüfe, wessen Perspektive in der Antwort fehlt."),
        ("Eine Bewerbungs-KI sortiert Lebensläufe mit ausländisch klingenden Namen schlechter ein. Woher kommt das meist?", new[] { "Aus alten Einstellungs-Daten, die genau diese Benachteiligung enthielten", "Aus einem Zufallsgenerator, der bei jedem Durchlauf andere Bewerbungen bevorzugt", "Aus zu wenig Arbeitsspeicher beim Sortieren" }, "Aus alten Einstellungs-Daten, die genau diese Benachteiligung enthielten",
            "Trainiert man KI mit diskriminierenden Alt-Entscheidungen, lernt sie die Diskriminierung als 'Muster' - und wiederholt sie."),
        ("Warum betrifft Bias auch Berliner Jugendliche mit türkischen Wurzeln direkt?", new[] { "Ihre Lebenswelt ist in Trainingsdaten oft unterrepräsentiert oder klischeehaft", "Bias tritt nur bei älteren Menschen auf, weil Jugendliche mit Technik viel besser umgehen können", "Berlin ist aus allen Trainingsdaten ausgeschlossen" }, "Ihre Lebenswelt ist in Trainingsdaten oft unterrepräsentiert oder klischeehaft",
            "Wessen Alltag in den Daten selten oder verzerrt vorkommt, über den 'weiß' die KI wenig Echtes - Klischees füllen die Lücke."),
        ("Was hilft Firmen, Bias in ihren KI-Systemen zu verringern?", new[] { "Vielfältige Daten, gemischte Teams und regelmäßige Tests", "Schnellere Prozessoren, mehr Speicher und deutlich größere Bildschirme", "Geheimhaltung, damit niemand Fehler entdeckt" }, "Vielfältige Daten, gemischte Teams und regelmäßige Tests",
            "Gegen Schieflagen helfen breite Datenauswahl, Tests auf Benachteiligung und Teams, denen die Probleme überhaupt auffallen."),
        ("Welche Aussage über 'neutrale KI' stimmt?", new[] { "Völlig neutrale KI gibt es nicht - Daten haben immer eine Herkunft", "KIs sind von Natur aus neutraler als jeder Mensch, weil ihnen Gefühle fehlen", "Neutralität lässt sich per Software-Update garantieren" }, "Völlig neutrale KI gibt es nicht - Daten haben immer eine Herkunft",
            "Jede KI trägt die Spuren ihrer Daten und ihrer Entwickler. Deshalb kritisch bleiben - auch bei 'objektiv' wirkenden Antworten."),
        ("Die KI beschreibt ein Land nur mit Urlaubs-Klischees. Was ist passiert?", new[] { "Reisewerbung dominierte vermutlich die Trainingstexte zu diesem Land", "Das Land hat der KI per Gesetz verboten, seine echten Daten zu verwenden", "Die KI war selbst schon einmal dort im Urlaub" }, "Reisewerbung dominierte vermutlich die Trainingstexte zu diesem Land",
            "Wenn über ein Thema vor allem Werbung oder Klischees geschrieben wurden, lernt die KI genau dieses verzerrte Bild."),
        ("Warum sollten wichtige Entscheidungen nie allein einer KI überlassen werden?", new[] { "Sie kann Schieflagen enthalten und trägt keine Verantwortung", "Menschen entscheiden grundsätzlich immer fehlerfrei", "KIs sind zu langsam für wichtige Entscheidungen" }, "Sie kann Schieflagen enthalten und trägt keine Verantwortung",
            "KI kann unterstützen - aber prüfen, abwägen und verantworten muss ein Mensch. Genau das fordern auch neue KI-Gesetze."),
        ("Was bedeutet 'Repräsentation' in Trainingsdaten?", new[] { "Ob alle Gruppen angemessen und realistisch vorkommen", "Wie schön die Daten grafisch dargestellt werden", "Die Anzahl der Server, auf denen Daten liegen" }, "Ob alle Gruppen angemessen und realistisch vorkommen",
            "Fehlt eine Gruppe in den Daten oder kommt nur im Klischee vor, behandelt die KI sie später genau so - lückenhaft oder verzerrt."),
        ("Ein Übersetzer macht aus dem neutralen türkischen 'o' automatisch 'er' beim Arzt und 'sie' bei der Pflegekraft. Was zeigt das?", new[] { "Rollen-Klischees aus den Trainingsdaten wirken in der Übersetzung fort", "Die türkische Sprache lässt sich von Computern grundsätzlich nicht korrekt übersetzen", "Das Programm kennt den Buchstaben O nicht richtig" }, "Rollen-Klischees aus den Trainingsdaten wirken in der Übersetzung fort",
            "Türkisch kennt kein er/sie - die KI ergänzt es nach gelernten Klischees. Ein Bias-Beispiel direkt aus deiner Sprachwelt."),
        ("Wie gehst du mit einer KI-Antwort zu einem Streitthema am besten um?", new[] { "Als eine Stimme von vielen behandeln und weitere Positionen einholen", "Als endgültiges, neutrales Urteil ansehen, das jede weitere Diskussion beendet", "Das Streitthema künftig ganz vermeiden" }, "Als eine Stimme von vielen behandeln und weitere Positionen einholen",
            "Zu Streitfragen liefert die KI einen Daten-Durchschnitt mit Schieflagen - echte Meinungsbildung braucht mehrere Quellen."),
        ("Warum prüfen Forscher KI-Systeme mit 'Audits'?", new[] { "Um versteckte Benachteiligungen systematisch aufzudecken", "Um die Rechtschreibung der Antworten zu benoten", "Um die Stromkosten der Server zu berechnen" }, "Um versteckte Benachteiligungen systematisch aufzudecken",
            "Bias-Audits testen gezielt: Behandelt das System verschiedene Gruppen unterschiedlich? Solche Prüfungen fordern auch Gesetze."),
        ("Was kannst DU gegen Bias tun, wenn du KI nutzt?", new[] { "Bewusst nachfragen, prüfen und Schieflagen benennen", "Nichts - gegen Technik ist man machtlos", "Nur noch KIs aus dem eigenen Land verwenden" }, "Bewusst nachfragen, prüfen und Schieflagen benennen",
            "Kritische Nutzer sind das beste Gegenmittel: Wer Schieflagen erkennt und anspricht, fällt nicht auf sie herein."),
        ("Eine KI bewertet Aufsätze. Welcher Bias wäre denkbar?", new[] { "Sie bevorzugt den Stil, der in ihren Trainingsdaten dominierte", "Sie mag grundsätzlich keine Texte, die zuerst handgeschrieben und dann abgetippt wurden", "Sie benotet nach dem Alphabet der Nachnamen" }, "Sie bevorzugt den Stil, der in ihren Trainingsdaten dominierte",
            "Wer anders schreibt als die Trainingstexte - etwa mehrsprachig geprägt - könnte unfair bewertet werden. Deshalb: Mensch prüft mit."),
        ("Welcher Satz beschreibt das Verhältnis von KI und Gerechtigkeit richtig?", new[] { "Gerechtigkeit muss von Menschen bewusst hineingearbeitet werden", "KI ist automatisch gerechter, weil sie keine Gefühle hat", "Gerechtigkeit spielt bei Technik keine Rolle" }, "Gerechtigkeit muss von Menschen bewusst hineingearbeitet werden",
            "Ohne bewusste Gegenmaßnahmen übernimmt KI die Ungerechtigkeiten ihrer Daten. Fairness ist Arbeit - keine Automatik.")
    };

    private static QuizQuestion BiasUndVerantwortung(Random r)
    {
        var f = BiasListe[r.Next(BiasListe.Length)];
        return new QuizQuestion
        {
            Id = NewId(), Subject = Subject.KiWissen, GradeLevel = GradeLevel.Klasse9,
            Topic = "Bias und Verantwortung", Type = QuestionType.MultipleChoice,
            Prompt = f.Frage, Options = f.Optionen, CorrectAnswers = new[] { f.Antwort }, Explanation = f.Erklaerung,
            HelpHint = "Bias = Schieflagen aus den Trainingsdaten (z.B. Rollen-Klischees). Verantwortung tragen die Menschen, die KI bauen und einsetzen - nie 'der Algorithmus'."
        };
    }

    private static readonly (string Frage, string[] Optionen, string Antwort, string Erklaerung)[] DeepfakeListe =
    {
        ("Was ist ein Deepfake?", new[] { "Ein KI-gefälschtes Bild, Video oder eine gefälschte Stimme", "Ein besonders tiefer Bass in elektronischer Musik", "Ein Tippfehler, der sich durch einen Text zieht" }, "Ein KI-gefälschtes Bild, Video oder eine gefälschte Stimme",
            "Deepfakes sind KI-generierte Medien, die echte Personen täuschend echt Dinge sagen oder tun lassen, die nie passiert sind."),
        ("Woran können KI-Videos derzeit noch scheitern?", new[] { "Unnatürliches Blinzeln, seltsame Übergänge, asynchroner Ton", "Sie sind grundsätzlich maximal zehn Sekunden lang und immer ohne jeden Ton", "Der Himmel ist in ihnen immer grün gefärbt" }, "Unnatürliches Blinzeln, seltsame Übergänge, asynchroner Ton",
            "Typische Schwächen: Augen/Blinzeln, Haare, Zähne, Ton-Lippen-Versatz. Aber Achtung - die Technik wird schnell besser."),
        ("Am Telefon klingt 'Oma' seltsam und will dringend Geld. Was tun?", new[] { "Auflegen und sie unter ihrer bekannten Nummer zurückrufen", "Schnell zahlen - bei Familie zählt jede Sekunde", "Nach dem Wetter von gestern fragen, um die Stimme gründlich zu testen" }, "Auflegen und sie unter ihrer bekannten Nummer zurückrufen",
            "Stimmen lassen sich mit KI klonen. Der sichere Weg: selbst zurückrufen - unter der Nummer, die du kennst. Nie unter Druck zahlen."),
        ("Warum sind Deepfakes für die Demokratie gefährlich?", new[] { "Gefälschte Politiker-Aussagen können Wähler gezielt täuschen", "Sie verbrauchen den knappen Speicherplatz der Computer in den Wahllokalen", "Politiker haben dadurch weniger Zeit für Reden" }, "Gefälschte Politiker-Aussagen können Wähler gezielt täuschen",
            "Ein gefälschtes 'Skandal-Video' kurz vor einer Wahl kann Meinungen kippen, bevor die Fälschung auffliegt."),
        ("Was ist der beste erste Schritt bei einem verdächtigen 'Beweis-Video'?", new[] { "Prüfen, ob seriöse Medien ebenfalls darüber berichten", "Es sofort teilen und die Follower entscheiden lassen", "Die Bildhelligkeit am Handy höher stellen" }, "Prüfen, ob seriöse Medien ebenfalls darüber berichten",
            "Echte Skandale schaffen es in seriöse Nachrichten. Berichtet niemand Seriöses, ist Skepsis das Gebot der Stunde."),
        ("Jemand erstellt ein Deepfake einer Mitschülerin. Was ist das?", new[] { "Eine Persönlichkeitsverletzung mit möglichen rechtlichen Folgen", "Ein harmloser Streich, solange es in der Klasse bleibt und niemand es Erwachsenen zeigt", "Erlaubte Kunst, solange es lustig gemeint ist" }, "Eine Persönlichkeitsverletzung mit möglichen rechtlichen Folgen",
            "Gefälschte Bilder realer Personen verletzen deren Rechte massiv - das kann Schule, Eltern und sogar Polizei beschäftigen. Sofort melden."),
        ("Was ist eine 'Rückwärts-Bildersuche'?", new[] { "Ein Werkzeug, das zeigt, wo ein Bild sonst noch auftaucht", "Eine Funktion, die Fotos spiegelverkehrt anzeigt", "Ein Filter, der Bilder rückwärts abspielen kann" }, "Ein Werkzeug, das zeigt, wo ein Bild sonst noch auftaucht",
            "Damit findest du Ursprung und Alter eines Bildes - oft entlarvt das alte Fotos, die als 'aktuell' verkauft werden."),
        ("Welche Datenspur hinterlässt du bei Online-KI-Chats mindestens?", new[] { "Deine Eingaben können beim Anbieter gespeichert werden", "Gar keine - Chats verschwinden beim Schließen restlos", "Nur die Uhrzeit, sonst wird nichts erfasst" }, "Deine Eingaben können beim Anbieter gespeichert werden",
            "Eingaben landen auf fremden Servern und können gespeichert, ausgewertet oder fürs Training genutzt werden."),
        ("Warum solltest du keine Fotos von Freunden in KI-Apps hochladen?", new[] { "Du verfügst über fremde Gesichter, ohne die Erlaubnis dazu zu haben", "Fremde Gesichter bringen die Gesichtserkennung der Apps technisch durcheinander", "Solche Fotos werden automatisch unscharf gerechnet" }, "Du verfügst über fremde Gesichter, ohne die Erlaubnis dazu zu haben",
            "Das Gesicht gehört der Person. Hochladen ohne Einverständnis verletzt ihre Privatsphäre - egal wie praktisch der Filter ist."),
        ("Was bedeutet 'Einwilligung' beim Thema Daten?", new[] { "Die informierte, freiwillige Zustimmung der betroffenen Person", "Das automatische Häkchen, das viele Apps bei der Installation für dich setzen", "Eine Unterschrift, die nur Erwachsene geben können" }, "Die informierte, freiwillige Zustimmung der betroffenen Person",
            "Ohne echte, informierte Zustimmung dürfen persönliche Daten - auch Gesichter und Stimmen - nicht verwendet werden."),
        ("Ein 'kostenloser' KI-Dienst verlangt Zugriff auf alle deine Fotos. Womit bezahlst du wirklich?", new[] { "Mit deinen Daten, die der Anbieter verwerten kann", "Mit nichts - kostenlos bedeutet wirklich geschenkt", "Mit dem Strom, den dein Handy verbraucht" }, "Mit deinen Daten, die der Anbieter verwerten kann",
            "Faustregel: Ist das Produkt gratis, bist du oft das Produkt - bezahlt wird mit Daten. Zugriffe deshalb sparsam vergeben."),
        ("Wie reagierst du am besten auf ein peinliches Deepfake, das über WhatsApp kreist?", new[] { "Nicht weiterleiten, Betroffene informieren, bei Erwachsenen melden", "Mitlachen und es in weitere Gruppen stellen, solange man selbst nicht darauf zu sehen ist", "Einen eigenen, noch besseren Fake erstellen" }, "Nicht weiterleiten, Betroffene informieren, bei Erwachsenen melden",
            "Jede Weiterleitung vergrößert den Schaden. Stoppen, den Betroffenen Bescheid geben, Erwachsene einschalten - so handelst du richtig."),
        ("Was ist ein starkes Indiz für ein echtes (nicht KI-generiertes) Nachrichtenfoto?", new[] { "Eine nachvollziehbare Quelle mit Fotograf, Ort und Datum", "Besonders dramatische Farben und ausdrucksstarke Gesichter im Vordergrund", "Eine sehr hohe Anzahl an Kommentaren darunter" }, "Eine nachvollziehbare Quelle mit Fotograf, Ort und Datum",
            "Seriöse Medien nennen Herkunft und Kontext ihrer Bilder. Fehlt jede Quellenangabe, ist Vorsicht angebracht."),
        ("Warum klonen Betrüger gerade die Stimmen von Familienmitgliedern?", new[] { "Vertraute Stimmen schalten unser Misstrauen aus", "Familienstimmen sind technisch leichter zu klonen", "Fremde Stimmen sind gesetzlich besser geschützt" }, "Vertraute Stimmen schalten unser Misstrauen aus",
            "Der 'Enkeltrick 2.0' nutzt Vertrauen als Waffe. Gegenmittel: Rückruf über bekannte Nummern und ein Familien-Codewort."),
        ("Was ist ein sinnvolles 'Familien-Codewort'?", new[] { "Ein geheimes Wort, das echte Anrufe von Fakes unterscheidet", "Das WLAN-Passwort, das man Besuchern der Familie im Notfall nennen kann", "Der Spitzname des jüngsten Familienmitglieds" }, "Ein geheimes Wort, das echte Anrufe von Fakes unterscheidet",
            "Ein vereinbartes Codewort, das nur die Familie kennt, entlarvt geklonte Stimmen sofort - simpel und wirksam."),
        ("Welche Regel gilt für Sprachnachrichten mit sensiblen Infos?", new[] { "Auch Stimmen können missbraucht werden - sensibel bleibt offline", "Sprachnachrichten sind grundsätzlich sicherer als jede getippte Textnachricht", "Nach dem Abhören löschen sie sich von selbst überall" }, "Auch Stimmen können missbraucht werden - sensibel bleibt offline",
            "Stimmaufnahmen lassen sich klonen und weiterleiten. Wirklich Privates besprichst du besser persönlich."),
        ("Was regeln neue KI-Gesetze wie der EU AI Act unter anderem?", new[] { "Kennzeichnungspflichten für KI-Inhalte und Regeln für riskante Systeme", "Ein weltweites Komplettverbot künstlicher Intelligenz ab dem kommenden Jahr", "Die Mindestanzahl von Emojis in KI-Antworten" }, "Kennzeichnungspflichten für KI-Inhalte und Regeln für riskante Systeme",
            "Die EU verlangt u.a., dass KI-generierte Inhalte erkennbar sind und riskante Systeme strenger geprüft werden - Verantwortung per Gesetz."),
        ("Warum ist 'Das Video war doch so echt!' keine Entschuldigung fürs Weiterleiten?", new[] { "Gerade weil Fakes echt wirken, ist Prüfen vor dem Teilen Pflicht", "Videos gelten rechtlich nicht als Information und dürfen darum frei geteilt werden", "Weiterleiten ist technisch gar nicht rückverfolgbar" }, "Gerade weil Fakes echt wirken, ist Prüfen vor dem Teilen Pflicht",
            "'Wirkt echt' ist bei Deepfakes der Normalfall. Verantwortung heißt: erst prüfen, dann teilen - oder eben nicht teilen."),
        ("Welche Einstellung schützt deine Daten bei neuen Apps am meisten?", new[] { "Nur die nötigsten Berechtigungen erteilen und Rest ablehnen", "Alle Berechtigungen erlauben, um Fehler zu vermeiden", "Die App bewerten, bevor man sie ausprobiert" }, "Nur die nötigsten Berechtigungen erteilen und Rest ablehnen",
            "Datensparsamkeit: Jede App bekommt nur, was sie für ihre Aufgabe wirklich braucht. Alles Weitere kannst du später immer noch erlauben."),
        ("Was ist die wichtigste Erkenntnis aus dem Thema Deepfakes?", new[] { "Sehen ist kein Beweis mehr - Quellen und Kontext zählen", "Alle Videos im Internet sind ausnahmslos gefälscht", "Nur Profis können überhaupt noch getäuscht werden" }, "Sehen ist kein Beweis mehr - Quellen und Kontext zählen",
            "'Ich hab's doch gesehen' reicht nicht mehr. Wer nach Quelle, Kontext und Motiv fragt, lässt sich nicht so leicht täuschen.")
    };

    private static QuizQuestion DeepfakesUndDatenschutz(Random r)
    {
        var f = DeepfakeListe[r.Next(DeepfakeListe.Length)];
        return new QuizQuestion
        {
            Id = NewId(), Subject = Subject.KiWissen, GradeLevel = GradeLevel.Klasse9,
            Topic = "Deepfakes und Datenschutz", Type = QuestionType.MultipleChoice,
            Prompt = f.Frage, Options = f.Optionen, CorrectAnswers = new[] { f.Antwort }, Explanation = f.Erklaerung,
            HelpHint = "Deepfake = KI-gefälschtes Bild/Video/Stimme. Schutz: Quelle prüfen, Rückruf über bekannte Nummern, Familien-Codewort, nichts Ungeprüftes weiterleiten, Datensparsamkeit."
        };
    }
}
