using LernTor.Core.Enums;
using LernTor.Core.Models;

namespace LernTor.Core.Services;

public static partial class DrivingCourseCatalog
{
    /// <summary>Die ersten sieben Lektionen: wer fahren darf, was gilt, und wie man liest,
    /// was am Straßenrand steht.</summary>
    private static readonly DrivingCourseLesson[] Grundlagen =
    {
        new()
        {
            Id = "kurs-voraussetzungen",
            Topic = DrivingTheoryTopic.PersoenlicheVoraussetzungen,
            Title = "Wer darf überhaupt fahren",
            Intro = "Bevor es um Zeichen und Regeln geht: Am Steuer entscheidet zuerst dein eigener "
                  + "Zustand. Die meisten schweren Unfälle junger Fahrer gehen nicht auf Unwissen zurück, "
                  + "sondern auf Alkohol, Müdigkeit oder Ablenkung.",
            Sections = new CourseSection[]
            {
                new("Alkohol: für dich gilt null",
                    "Allgemein liegt die Grenze bei 0,5 Promille. Für Fahranfänger in der Probezeit und "
                  + "für alle unter 21 gilt aber 0,0 Promille - ohne Ausnahme. Schon ab 0,3 Promille kann "
                  + "dir die Fahrerlaubnis entzogen werden, wenn du auffällig fährst oder einen Unfall "
                  + "baust. Alkohol macht dabei nicht nur langsamer: er macht vor allem mutiger, und das "
                  + "ist der gefährlichere Teil."),
                new("Müdigkeit wirkt wie Alkohol",
                    "Nach 17 Stunden ohne Schlaf entspricht deine Reaktion etwa 0,5 Promille. Der "
                  + "Sekundenschlaf kommt nicht angekündigt - wer sich zwingt wachzubleiben, hat den "
                  + "Punkt meist schon überschritten. Hilft: anhalten, 15 bis 20 Minuten schlafen. "
                  + "Nicht hilft: Fenster auf, laute Musik, Kaffee allein."),
                new("Medikamente und Drogen",
                    "Viele ganz normale Medikamente machen fahruntüchtig - Hustensaft, Allergietabletten, "
                  + "Schmerzmittel. Im Beipackzettel steht es. Bei Drogen gibt es keine Grenze wie beim "
                  + "Alkohol: nachgewiesen ist nachgewiesen, und Cannabis ist im Blut noch Tage später "
                  + "nachweisbar, auch wenn die Wirkung längst weg ist."),
                new("Ablenkung: das Handy",
                    "Das Handy in der Hand ist verboten, auch beim Stehen an der roten Ampel, solange der "
                  + "Motor läuft. Bei Tempo 50 legst du in zwei Sekunden Blick aufs Display fast 28 Meter "
                  + "blind zurück. Das ist die halbe Länge eines Fußballfeld-Strafraums - mit geschlossenen "
                  + "Augen.")
            },
            KeyPoints = new[]
            {
                "In der Probezeit und unter 21: 0,0 Promille.",
                "Müdigkeit ist kein Willensproblem - anhalten und 15 Minuten schlafen ist die einzige Lösung.",
                "Handy in der Hand ist auch an der roten Ampel verboten."
            }
        },
        new()
        {
            Id = "kurs-recht",
            Topic = DrivingTheoryTopic.RechtlicheRahmenbedingungen,
            Title = "Führerschein, Papiere, Punkte",
            Intro = "Der Papierkram klingt langweilig, entscheidet aber, ob eine Kontrolle glimpflich "
                  + "ausgeht - und ob die Versicherung nach einem Unfall zahlt.",
            Sections = new CourseSection[]
            {
                new("Die Probezeit dauert zwei Jahre",
                    "Wer sie mit einem schweren Verstoß (A-Verstoß) belastet, bekommt vier Jahre Probezeit "
                  + "und muss zu einem Aufbauseminar. Zwei leichte Verstöße (B-Verstöße) zählen zusammen "
                  + "wie ein schwerer. Beim begleiteten Fahren ab 17 beginnt die Probezeit schon mit dem "
                  + "17. Geburtstag, nicht erst mit 18."),
                new("Was immer dabei sein muss",
                    "Führerschein und Zulassungsbescheinigung Teil I gehören ins Auto. Dazu die "
                  + "Sicherheitsausrüstung: Warndreieck, Verbandkasten und Warnweste. Die Warnweste muss "
                  + "griffbereit sein, also nicht im Kofferraum - du sollst sie anziehen, bevor du "
                  + "aussteigst."),
                new("Punkte in Flensburg",
                    "Verstöße werden im Fahreignungsregister mit einem bis drei Punkten eingetragen. Bei "
                  + "acht Punkten ist die Fahrerlaubnis weg. Vorher gibt es Ermahnung und Verwarnung - das "
                  + "System warnt also, bevor es zuschlägt."),
                new("Halter und Fahrer",
                    "Der Halter ist, wer das Fahrzeug bezahlt und darüber bestimmt - nicht unbedingt der "
                  + "Eigentümer. Er muss dafür sorgen, dass das Auto verkehrssicher ist, versichert bleibt "
                  + "und nur von Leuten gefahren wird, die eine Fahrerlaubnis haben. Wer jemandem ohne "
                  + "Führerschein sein Auto leiht, macht sich selbst strafbar.")
            },
            KeyPoints = new[]
            {
                "Probezeit: zwei Jahre, bei einem schweren Verstoß vier plus Aufbauseminar.",
                "Acht Punkte in Flensburg bedeuten den Entzug der Fahrerlaubnis.",
                "Die Warnweste gehört in den Innenraum, nicht in den Kofferraum."
            }
        },
        new()
        {
            Id = "kurs-strassen",
            Topic = DrivingTheoryTopic.Strassenverkehrssystem,
            Title = "Welche Straße wie funktioniert",
            Intro = "Wo du bist, entscheidet, was gilt. Ortstafel, Autobahnschild und der blaue "
                  + "Spielstraßen-Anfang ändern jeweils Tempo, Vorfahrt und Verhalten auf einen Schlag.",
            Sections = new CourseSection[]
            {
                new("Die Ortstafel ist ein Tempolimit",
                    "Das gelbe Ortsschild sagt nicht nur, wo du bist: Ab hier gilt 50 km/h, auch wenn kein "
                  + "Tempolimit-Schild dasteht. Die Rückseite hebt es wieder auf - dahinter gelten 100 km/h "
                  + "für Pkw, sofern nichts anderes angeordnet ist.",
                    "310"),
                new("Autobahn und Kraftfahrstraße",
                    "Beide sind nur für Fahrzeuge, die bauartbedingt schneller als 60 km/h fahren können. "
                  + "Auf der Autobahn gilt die Richtgeschwindigkeit von 130 km/h - kein Limit, aber wer "
                  + "schneller fährt und in einen Unfall gerät, haftet oft mit. Wenden, rückwärtsfahren und "
                  + "halten sind dort verboten, auch auf dem Seitenstreifen.",
                    "330.1"),
                new("Verkehrsberuhigter Bereich",
                    "Im Volksmund Spielstraße. Hier gilt Schrittgeschwindigkeit, Fußgänger dürfen die "
                  + "ganze Breite der Straße benutzen und Kinder dort spielen. Geparkt wird nur auf "
                  + "gekennzeichneten Flächen. Beim Ausfahren auf eine normale Straße musst du dich "
                  + "einordnen wie beim Verlassen eines Grundstücks - alle anderen haben Vorrang.",
                    "325.1"),
                new("Tempo-30-Zone",
                    "In einer Zone gilt das Tempo für alle Straßen darin, bis das Ende-Zeichen kommt - du "
                  + "musst also nicht an jeder Ecke nach einem neuen Schild suchen. Innerhalb einer Zone "
                  + "gilt außerdem meist rechts vor links, weil Vorfahrtstraßen dort selten sind.",
                    "274.1")
            },
            KeyPoints = new[]
            {
                "Ortstafel = 50 km/h, Rückseite = 100 km/h für Pkw.",
                "Im verkehrsberuhigten Bereich: Schrittgeschwindigkeit, Kinder dürfen spielen.",
                "Ein Zonen-Zeichen gilt für alle Straßen der Zone, bis das Ende-Zeichen kommt."
            }
        },
        new()
        {
            Id = "kurs-zeichen",
            Topic = DrivingTheoryTopic.VerkehrszeichenUndEinrichtungen,
            Title = "Zeichen lesen statt auswendig lernen",
            Intro = "Es gibt hunderte Verkehrszeichen, aber nur eine Handvoll Bauregeln. Wer die kennt, "
                  + "versteht auch ein Zeichen, das er noch nie gesehen hat.",
            Sections = new CourseSection[]
            {
                new("Dreieck mit rotem Rand warnt",
                    "Gefahrzeichen ordnen nichts an - sie sagen nur: hier kommt etwas. Was zu tun ist, "
                  + "musst du selbst ableiten, nämlich der Lage angemessen langsamer und aufmerksamer "
                  + "fahren. Genau daran scheitern viele in der Prüfung: sie suchen nach einer Zahl, die "
                  + "es nicht gibt.",
                    "101"),
                new("Roter Kreis verbietet, blauer Kreis gebietet",
                    "Ein Fahrrad im roten Kreis ist ein Radfahrverbot, dasselbe Fahrrad im blauen Kreis "
                  + "ein Radweg, den du benutzen musst. Diese eine Regel erklärt die Hälfte aller runden "
                  + "Zeichen.",
                    "237"),
                new("Zusatzzeichen ändern alles darüber",
                    "Das kleine weiße Schild unter dem Hauptzeichen schränkt es ein oder erweitert es - "
                  + "auf bestimmte Fahrzeuge, Zeiten, Richtungen oder Wetterlagen. \"Anlieger frei\" unter "
                  + "einem Durchfahrtverbot heißt: wer dort wohnt, arbeitet oder jemanden besucht, darf "
                  + "fahren. Nur abkürzen zählt nicht.",
                    "1020-30"),
                new("Verkehrseinrichtungen sind keine Zeichen",
                    "Baken, Leitkegel und Absperrschranken ordnen nichts an, sie zeigen dir den Weg und "
                  + "sichern Gefahrstellen ab. Sie dürfen nicht überfahren oder beiseitegeräumt werden, "
                  + "auch wenn dahinter frei aussieht.",
                    "610")
            },
            KeyPoints = new[]
            {
                "Dreieck warnt, roter Kreis verbietet, blauer Kreis gebietet.",
                "Gefahrzeichen schreiben kein Tempo vor - du musst selbst angemessen langsamer werden.",
                "Ein Zusatzzeichen gilt immer für das Zeichen direkt darüber."
            }
        },
        new()
        {
            Id = "kurs-vorfahrt",
            Topic = DrivingTheoryTopic.VorfahrtUndRegelung,
            Title = "Vorfahrt: die Rangfolge entscheidet",
            Intro = "Vorfahrt ist die häufigste Fehlerquelle in der Prüfung - und im echten Verkehr die "
                  + "häufigste Ursache für Kreuzungsunfälle. Der Schlüssel ist nicht Auswendiglernen, "
                  + "sondern eine feste Reihenfolge, in der du nachsiehst.",
            Sections = new CourseSection[]
            {
                new("Die Rangfolge, immer in dieser Reihenfolge",
                    "Erstens: Weist ein Polizeibeamter ein? Dann gilt nur das. Zweitens: Gibt es eine "
                  + "Ampel? Sie schlägt jedes Schild. Drittens: Stehen Verkehrszeichen da? Sie schlagen "
                  + "die allgemeinen Regeln. Und erst wenn nichts davon zutrifft, gilt rechts vor links. "
                  + "Wer diese vier Stufen der Reihe nach abarbeitet, kommt an jeder Kreuzung zum "
                  + "richtigen Ergebnis."),
                new("Vorfahrt gewähren heißt nicht immer anhalten",
                    "Beim abgestumpften Dreieck musst du anhalten, wenn jemand kommt - sonst darfst du "
                  + "vorsichtig weiterrollen. Das Stoppschild dagegen verlangt immer Anhalten, auch auf "
                  + "einer völlig leeren Kreuzung um drei Uhr nachts. Diese beiden zu verwechseln, ist "
                  + "ein Klassiker.",
                    "206"),
                new("Vorfahrtstraße gilt weiter, Vorfahrt nur einmal",
                    "Die gelbe Raute gilt fortlaufend an jeder Kreuzung, bis das Ende-Zeichen kommt. Das "
                  + "Dreieck mit dem Kreuz darin gilt dagegen nur für die eine Kreuzung, vor der es steht. "
                  + "Zwei Zeichen, die viele verwechseln - und die genau darauf angelegt sind.",
                    "306"),
                new("Kreisverkehr und Grünpfeil",
                    "Im Kreisverkehr hat, wer schon drin ist. Geblinkt wird nur beim Ausfahren, nie beim "
                  + "Einfahren. Der grüne Pfeil neben der roten Ampel erlaubt Rechtsabbiegen - aber erst "
                  + "nach vollständigem Anhalten, wie beim Stoppschild.",
                    "215")
            },
            KeyPoints = new[]
            {
                "Reihenfolge: Polizei vor Ampel vor Zeichen vor rechts vor links.",
                "Nur das Stoppschild verlangt Anhalten auch bei freier Kreuzung.",
                "Im Kreisverkehr blinkst du nur beim Ausfahren."
            }
        },
        new()
        {
            Id = "kurs-tempo",
            Topic = DrivingTheoryTopic.GeschwindigkeitUndAbstand,
            Title = "Tempo und Abstand sind Physik",
            Intro = "Hier hilft kein Gefühl, hier rechnet man. Zwei einfache Formeln erklären, warum "
                  + "doppelt so schnell nicht doppelt so langer Bremsweg heißt, sondern viermal so langer.",
            Sections = new CourseSection[]
            {
                new("Anhalteweg = Reaktionsweg + Bremsweg",
                    "Der Reaktionsweg ist Tempo geteilt durch 10, mal 3. Der Bremsweg ist Tempo geteilt "
                  + "durch 10, das Ganze mal sich selbst. Bei 100 km/h: 30 Meter Reaktion plus 100 Meter "
                  + "Bremsen, macht 130 Meter. Bei 50 km/h sind es 15 plus 25, also 40 Meter. Halbes "
                  + "Tempo, aber nicht halber Weg - ein Drittel."),
                new("Bei Gefahr halbiert sich der Bremsweg",
                    "Wer voll in die Eisen geht, statt sanft zu bremsen, kommt mit dem halben Bremsweg "
                  + "aus. Aus 100 Metern werden 50. Deshalb ist Zögern im Notfall teurer als jede "
                  + "Fehleinschätzung - und deshalb hat modernes ABS keinen Nachteil mehr: du darfst "
                  + "und sollst voll draufsteigen."),
                new("Abstand: halber Tacho",
                    "Außerorts hältst du in Metern mindestens den halben Tachostand Abstand - bei 100 "
                  + "km/h also 50 Meter. Praktischer ist die Zwei-Sekunden-Regel: Wenn das Auto vor dir "
                  + "an einem Pfosten vorbei ist, zählst du zwei Sekunden. Bist du früher da, ist der "
                  + "Abstand zu klein."),
                new("Angemessen ist oft langsamer als erlaubt",
                    "Ein Tempolimit ist eine Obergrenze, keine Empfehlung. Bei Nebel, Nässe, Dunkelheit "
                  + "oder Schnee gilt: Du darfst nur so schnell fahren, dass du innerhalb der übersehbaren "
                  + "Strecke anhalten kannst. Bei Sichtweite unter 50 Metern sind höchstens 50 km/h "
                  + "erlaubt - überall, auch auf der Autobahn.",
                    "274-50")
            },
            KeyPoints = new[]
            {
                "Bremsweg = (Tempo ÷ 10)², bei Gefahrbremsung die Hälfte davon.",
                "Abstand außerorts: halber Tacho in Metern, oder zwei Sekunden.",
                "Unter 50 Metern Sichtweite: höchstens 50 km/h, auch auf der Autobahn."
            }
        },
        new()
        {
            Id = "kurs-manoever",
            Topic = DrivingTheoryTopic.FahrmanoeverUndUeberholen,
            Title = "Überholen, abbiegen, wenden",
            Intro = "Fast alles, was schiefgeht, geht beim Wechsel der Spur oder der Richtung schief. "
                  + "Dafür gibt es eine feste Abfolge, die immer gleich ist.",
            Sections = new CourseSection[]
            {
                new("Spiegel, blinken, Schulterblick",
                    "In dieser Reihenfolge, jedes Mal. Der Schulterblick kommt zuletzt und ist "
                  + "unverzichtbar: Der tote Winkel ist genau der Bereich, den kein Spiegel zeigt und in "
                  + "dem ein Radfahrer verschwindet. Blinken ist eine Ankündigung, kein Anspruch - es "
                  + "gibt dir kein Recht, einfach rüberzuziehen."),
                new("Überholt wird links, und nur wenn es passt",
                    "Du darfst nur überholen, wenn du die Strecke übersiehst, deutlich schneller bist und "
                  + "niemanden behinderst. Beim Überholen von Radfahrenden gilt innerorts mindestens 1,5 "
                  + "Meter Abstand, außerorts 2 Meter - notfalls musst du warten. Rechts überholen ist "
                  + "grundsätzlich verboten; auf der Autobahn bei Stau darfst du rechts nur mit mäßig "
                  + "höherem Tempo vorbeifahren.",
                    "276"),
                new("Abbiegen: einordnen, langsam, doppelt schauen",
                    "Rechtsabbiegen heißt: rechts einordnen, und vor dem Abbiegen noch einmal über die "
                  + "Schulter nach Radfahrenden sehen, die geradeaus wollen. Linksabbiegen heißt: den "
                  + "Gegenverkehr durchlassen und dabei auch an Fußgänger denken, die in die Straße "
                  + "einbiegen, in die du willst - die haben Vorrang."),
                new("Wenden und rückwärts",
                    "Beides ist da verboten, wo es andere gefährdet: auf Autobahnen und "
                  + "Kraftfahrstraßen immer, sonst überall dort, wo das Wendeverbot steht oder die "
                  + "Übersicht fehlt. Rückwärts darfst du nur so weit, wie du selbst sehen kannst - "
                  + "notfalls lässt du dich einweisen.",
                    "272")
            },
            KeyPoints = new[]
            {
                "Immer Spiegel, blinken, Schulterblick - in dieser Reihenfolge.",
                "Überholabstand zu Radfahrenden: 1,5 m innerorts, 2 m außerorts.",
                "Blinken kündigt an, es berechtigt zu nichts."
            }
        }
    };
}
