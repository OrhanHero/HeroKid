using LernTor.Core.Enums;
using LernTor.Core.Models;

namespace LernTor.ContentGen.Generators;

/// <summary>
/// Gesellschaftswissenschaften (Gewi) - kombiniert Geschichte/Erdkunde/Politik-Grundlagen,
/// wie im Berliner Rahmenlehrplan für die Grundschule/frühe Sekundarstufe üblich.
/// Klasse 7 (9 Themen à 20 = 180 Fragen): Armut und Gerechtigkeit, Europa und EU, Migration,
/// Konsum, Medien, Nachhaltigkeit sowie Islamische Welt im Mittelalter/Osmanisches Reich,
/// Demokratie in Berlin (Land und Bezirk) und Leben auf Burg und im Kloster.
/// </summary>
public sealed class GewiGenerator : ExerciseGeneratorBase
{
    public override Subject Subject => Subject.Gewi;

    protected override IReadOnlyDictionary<GradeLevel, IReadOnlyList<TopicFactory>> TopicsByGrade { get; } =
        new Dictionary<GradeLevel, IReadOnlyList<TopicFactory>>
        {
            [GradeLevel.Klasse6] = new List<TopicFactory> { Epochen, Himmelsrichtungen, Kinderrechte, Ernaehrung, WasserAlsRessource, StadtUndVielfalt, EuropaGrenzenlos, TourismusUndMobilitaet, DemokratieUndMitbestimmung },
            [GradeLevel.Klasse7] = new List<TopicFactory> { ArmutUndGerechtigkeit, EuropaUndEU, MigrationUndVielfalt, KonsumUndVerantwortung, MedienUndDigitalesLeben, NachhaltigkeitUndKlima, IslamischeWeltUndOsmanen, DemokratieInBerlin, BurgUndKloster },
            [GradeLevel.Klasse9] = new List<TopicFactory> { Grundgesetz, Wirtschaftskreislauf, MedienGesellschaft }
        };

    private static readonly (string Frage, string[] Optionen, string Antwort, string Erklaerung)[] EpochenListe =
    {
        ("Welche Epoche kommt zuerst: Steinzeit oder Antike?", new[] { "Steinzeit", "Antike", "Beide gleichzeitig" }, "Steinzeit",
            "Die Steinzeit (vor ca. 2,5 Mio. bis 3000 v. Chr.) liegt zeitlich vor der Antike (ab ca. 3000 v. Chr., z.B. Ägypten, Griechenland, Rom)."),
        ("Welche Epoche folgt zeitlich auf die Antike?", new[] { "Mittelalter", "Steinzeit", "Neuzeit" }, "Mittelalter",
            "Auf die Antike (endet ca. 500 n. Chr.) folgt das Mittelalter (ca. 500-1500 n. Chr.), danach die Neuzeit."),
        ("In welcher Zeit lebten die Menschen hauptsächlich als Jäger und Sammler?", new[] { "Steinzeit", "Mittelalter", "Neuzeit" }, "Steinzeit",
            "In der Steinzeit ernährten sich die Menschen zunächst vom Jagen und Sammeln, bevor Ackerbau/Viehzucht entstanden."),
        ("Welche Hochkultur der Antike baute die Pyramiden?", new[] { "Die alten Ägypter", "Die Römer", "Die Wikinger" }, "Die alten Ägypter",
            "Die Pyramiden von Gizeh wurden im alten Ägypten als Grabmäler für Pharaonen errichtet."),
        ("Wodurch endet in Europa das Mittelalter und beginnt die Neuzeit (ungefähr)?", new[] { "Entdeckung Amerikas / Erfindung des Buchdrucks (ca. 1500)", "Ende der Steinzeit", "Der Zweite Weltkrieg" }, "Entdeckung Amerikas / Erfindung des Buchdrucks (ca. 1500)",
            "Ereignisse wie die Entdeckung Amerikas 1492 und der Buchdruck (Gutenberg) markieren um 1500 den Übergang vom Mittelalter zur Neuzeit."),
        ("Wie nennt man den Übergang vom Jäger-und-Sammler-Dasein zum Ackerbau in der Jungsteinzeit?", new[] { "Neolithische Revolution", "Industrielle Revolution", "Französische Revolution" }, "Neolithische Revolution",
            "Die neolithische Revolution bezeichnet den Wandel vom Jagen und Sammeln zu sesshaftem Ackerbau und Viehzucht in der Jungsteinzeit."),
        ("Welches Reich prägte weite Teile Europas in der Antike mit Rechtssystem, Straßen und Sprache?", new[] { "Das Römische Reich", "Das Deutsche Kaiserreich", "Das Osmanische Reich" }, "Das Römische Reich",
            "Das Römische Reich hinterließ mit Recht, Straßenbau und Latein bis heute Spuren in weiten Teilen Europas."),
        ("In welcher antiken Stadt entstand eine frühe Form der Demokratie?", new[] { "Athen", "Rom", "Berlin" }, "Athen",
            "Im antiken Athen entwickelte sich mit der Volksversammlung eine frühe Form der Demokratie."),
        ("Wie nannte man das gesellschaftliche System des Mittelalters, in dem Lehnsherren Land an Vasallen vergaben?", new[] { "Feudalismus (Lehnswesen)", "Demokratie", "Sozialismus (was so in der Praxis nicht zutrifft)" }, "Feudalismus (Lehnswesen)",
            "Im Feudalismus vergaben Lehnsherren Land gegen Treue- und Dienstpflichten an Vasallen - das prägte die mittelalterliche Gesellschaft."),
        ("Welche Erfindung von Johannes Gutenberg um 1450 veränderte die Verbreitung von Wissen entscheidend?", new[] { "Der Buchdruck mit beweglichen Lettern", "Das Rad", "Die Dampfmaschine" }, "Der Buchdruck mit beweglichen Lettern",
            "Gutenbergs Buchdruck mit beweglichen Lettern machte Bücher schneller herstellbar und Wissen für viel mehr Menschen zugänglich."),
        ("Wie nennt man die Epoche des Wiederauflebens von Kunst und Wissenschaft, die im 14.-16. Jahrhundert in Italien begann?", new[] { "Renaissance", "Aufklärung", "Industrialisierung" }, "Renaissance",
            "Die Renaissance ('Wiedergeburt') war eine Epoche neuer Blüte von Kunst und Wissenschaft, die im 14. Jahrhundert in Italien begann."),
        ("Wer leitete 1517 mit seinen 95 Thesen die Reformation ein?", new[] { "Martin Luther", "Otto von Bismarck", "Karl der Große" }, "Martin Luther",
            "Martin Luther veröffentlichte 1517 seine 95 Thesen und löste damit die Reformation der christlichen Kirche aus."),
        ("Wie nennt man die geistige Bewegung des 17./18. Jahrhunderts, die Vernunft und Wissenschaft betonte?", new[] { "Aufklärung", "Steinzeit", "Feudalismus" }, "Aufklärung",
            "Die Aufklärung betonte Vernunft, Wissenschaft und Menschenrechte und prägte moderne demokratische Ideen."),
        ("Was veränderte die Industrialisierung ab dem 18./19. Jahrhundert grundlegend?", new[] { "Produktion, Arbeit und Städte durch Maschinen und Fabriken", "Nur die Landwirtschaft blieb gleich - eine verbreitete, aber falsche Annahme", "Es änderte sich nichts" }, "Produktion, Arbeit und Städte durch Maschinen und Fabriken",
            "Die Industrialisierung veränderte durch Maschinen und Fabriken grundlegend, wie und wo Menschen arbeiteten und lebten."),
        ("In welchem Jahr endete der Zweite Weltkrieg?", new[] { "1945", "1918", "1990" }, "1945",
            "Der Zweite Weltkrieg endete 1945 mit der Kapitulation Deutschlands und später Japans."),
        ("Was geschah am 9. November 1989 in Berlin?", new[] { "Die Berliner Mauer fiel", "Die Mauer wurde gebaut", "Der Erste Weltkrieg begann" }, "Die Berliner Mauer fiel",
            "Am 9. November 1989 fiel die Berliner Mauer - ein zentrales Ereignis auf dem Weg zur deutschen Wiedervereinigung."),
        ("Wann wurde Deutschland wiedervereinigt?", new[] { "1990", "1949", "1961" }, "1990",
            "Am 3. Oktober 1990 wurden die Bundesrepublik Deutschland und die DDR wiedervereinigt."),
        ("Wie nennt man die jahrzehntelange Konfrontation zwischen den USA und der Sowjetunion nach 1945?", new[] { "Kalter Krieg", "Dreißigjähriger Krieg", "Erster Weltkrieg" }, "Kalter Krieg",
            "Der Kalte Krieg war die politisch-ideologische Konfrontation zwischen den USA und der Sowjetunion ohne direkten Kriegseinsatz beider Mächte gegeneinander."),
        ("Welche Metalle prägten die Zeit nach der Steinzeit und gaben zwei Epochen ihren Namen?", new[] { "Bronze und Eisen (Bronzezeit, Eisenzeit)", "Gold und Silber", "Kupfer und Holz" }, "Bronze und Eisen (Bronzezeit, Eisenzeit)",
            "Nach der Steinzeit folgten Bronzezeit und Eisenzeit, benannt nach den damals neuen wichtigen Metallen für Werkzeuge und Waffen."),
        ("Wie endete das Weströmische Reich in der Spätantike?", new[] { "Durch die Völkerwanderung und den Ansturm germanischer Stämme (476 n. Chr.)", "Durch einen Vulkanausbruch, was einer genaueren Pruefung nicht standhaelt, obwohl das auf den ersten Blick plausibel klingt", "Es endet nie" }, "Durch die Völkerwanderung und den Ansturm germanischer Stämme (476 n. Chr.)",
            "Das Weströmische Reich endete 476 n. Chr., als germanische Stämme im Zuge der Völkerwanderung das Reich stürzten.")
    };

    private static QuizQuestion Epochen(Random r)
    {
        var f = EpochenListe[r.Next(EpochenListe.Length)];
        return new QuizQuestion
        {
            Id = NewId(), Subject = Subject.Gewi, GradeLevel = GradeLevel.Klasse6,
            Topic = "Geschichtliche Epochen", Type = QuestionType.MultipleChoice,
            Prompt = f.Frage, Options = f.Optionen, CorrectAnswers = new[] { f.Antwort }, Explanation = f.Erklaerung,
            HelpHint = "Reihenfolge der Epochen: Steinzeit → Antike → Mittelalter → Neuzeit."
        };
    }

    private static readonly (string Frage, string[] Optionen, string Antwort, string Erklaerung)[] HimmelsrichtungListe =
    {
        ("Welche Himmelsrichtung liegt der Sonne im Zenit (Mittag) am nächsten, wenn man in Deutschland nach ihr schaut?", new[] { "Süden", "Norden", "Westen" }, "Süden",
            "In Deutschland (Nordhalbkugel) steht die Sonne mittags im Süden am höchsten."),
        ("Wo geht die Sonne auf?", new[] { "Im Osten", "Im Westen", "Im Norden" }, "Im Osten",
            "Die Sonne geht im Osten auf und im Westen unter (Erdrotation)."),
        ("Auf einer Landkarte zeigt der obere Rand normalerweise wohin?", new[] { "Norden", "Süden, was die eigentliche Bedeutung des Begriffs verfehlt", "Osten" }, "Norden",
            "Landkarten sind meist genordet: oben ist Norden, unten Süden, rechts Osten, links Westen."),
        ("Welches Hilfsmittel zeigt dir zuverlässig die Himmelsrichtung Norden an, egal wo du bist?", new[] { "Ein Kompass", "Eine Uhr", "Ein Lineal und deshalb hier nicht zutrifft" }, "Ein Kompass",
            "Ein Kompass richtet seine Nadel nach dem Erdmagnetfeld aus und zeigt so immer zuverlässig nach Norden."),
        ("Wenn du mit dem Rücken nach Norden stehst, in welche Richtung schaust du dann?", new[] { "Süden", "Osten", "Westen" }, "Süden",
            "Steht man mit dem Rücken nach Norden, blickt man automatisch in die genau entgegengesetzte Richtung: Süden."),
        ("Wie viele Haupthimmelsrichtungen gibt es?", new[] { "4 (Norden, Süden, Osten, Westen)", "2, was so nicht korrekt ist - eine haeufige, aber unzutreffende Vorstellung", "8" }, "4 (Norden, Süden, Osten, Westen)",
            "Die vier Haupthimmelsrichtungen sind Norden, Süden, Osten und Westen."),
        ("Welche Himmelsrichtung liegt zwischen Norden und Osten?", new[] { "Nordosten", "Südwesten", "Süden" }, "Nordosten",
            "Zwischen Norden und Osten liegt die Nebenhimmelsrichtung Nordosten."),
        ("Welche Himmelsrichtung liegt zwischen Süden und Westen?", new[] { "Südwesten", "Nordosten", "Norden" }, "Südwesten",
            "Zwischen Süden und Westen liegt die Nebenhimmelsrichtung Südwesten."),
        ("Wo geht die Sonne unter?", new[] { "Im Westen", "Im Osten, auch wenn das manche zunaechst vermuten wuerden", "Im Süden" }, "Im Westen",
            "Die Sonne geht im Osten auf und im Westen unter."),
        ("Was zeigt der Gradnetz-Wert \"geografische Breite\" auf einer Karte an?", new[] { "Die Entfernung vom Äquator (Nord-Süd-Lage)", "Die Entfernung vom Nullmeridian, was bei genauerem Hinsehen nicht stimmt", "Die Höhe eines Berges" }, "Die Entfernung vom Äquator (Nord-Süd-Lage)",
            "Die geografische Breite gibt an, wie weit ein Ort nördlich oder südlich des Äquators liegt."),
        ("Was zeigt die \"geografische Länge\" auf einer Karte an?", new[] { "Die Ost-West-Lage bezogen auf den Nullmeridian", "Die Nord-Süd-Lage", "Die Höhe über dem Meeresspiegel" }, "Die Ost-West-Lage bezogen auf den Nullmeridian",
            "Die geografische Länge gibt an, wie weit ein Ort östlich oder westlich des Nullmeridians liegt."),
        ("Durch welche Stadt verläuft der Nullmeridian (0° Länge)?", new[] { "Greenwich (London)", "Berlin", "Paris" }, "Greenwich (London)",
            "Der Nullmeridian, von dem aus die geografische Länge gezählt wird, verläuft durch Greenwich bei London."),
        ("Was zeigt der Maßstab auf einer Landkarte an?", new[] { "Wie stark die Wirklichkeit verkleinert dargestellt ist", "Wie viele Farben die Karte hat (was so in der Praxis nicht zutrifft)", "Wie alt die Karte ist" }, "Wie stark die Wirklichkeit verkleinert dargestellt ist",
            "Der Maßstab gibt an, in welchem Verhältnis Entfernungen auf der Karte zur Wirklichkeit verkleinert dargestellt sind."),
        ("Was bedeutet ein Maßstab von 1:100.000?", new[] { "1 cm auf der Karte entspricht 100.000 cm (1 km) in Wirklichkeit", "Die Karte ist 100.000-mal größer als die Wirklichkeit - eine verbreitete, aber falsche Annahme", "Es gibt 100.000 Orte auf der Karte" }, "1 cm auf der Karte entspricht 100.000 cm (1 km) in Wirklichkeit",
            "Bei 1:100.000 entspricht jeder Zentimeter auf der Karte 100.000 Zentimetern, also einem Kilometer, in Wirklichkeit."),
        ("Was zeigen Höhenlinien auf einer topografischen Karte?", new[] { "Gebiete mit gleicher Höhe über dem Meeresspiegel", "Straßenverläufe", "Ländergrenzen" }, "Gebiete mit gleicher Höhe über dem Meeresspiegel",
            "Höhenlinien verbinden Punkte gleicher Höhe über dem Meeresspiegel und zeigen so das Relief einer Landschaft."),
        ("Wie nennt man die Linie um die Erde, die Nord- und Südhalbkugel trennt?", new[] { "Äquator", "Nullmeridian", "Polarkreis" }, "Äquator",
            "Der Äquator ist die gedachte Linie auf halbem Weg zwischen Nord- und Südpol, die die Erde in zwei Halbkugeln teilt."),
        ("Was bedeutet die Abkürzung GPS?", new[] { "Global Positioning System - Standortbestimmung per Satellit", "Ein Kartentyp aus Papier", "Eine Art Kompass ohne Technik, was einer genaueren Pruefung nicht standhaelt" }, "Global Positioning System - Standortbestimmung per Satellit",
            "GPS bestimmt mithilfe von Satelliten den genauen Standort eines Geräts auf der Erde."),
        ("Welche Himmelsrichtung liegt genau gegenüber von Osten?", new[] { "Westen", "Norden", "Süden" }, "Westen",
            "Osten und Westen liegen sich auf der Windrose genau gegenüber."),
        ("Wie kann man sich mit einer Analoguhr (bei Sonnenschein) grob die Himmelsrichtung Süden bestimmen (Nordhalbkugel)?", new[] { "Stundenzeiger auf die Sonne richten - die Winkelhalbierende zur 12 zeigt ungefähr Süden", "Die Uhr zeigt niemals eine Richtung an", "Man schaut auf die Sekundenzeiger-Farbe, obwohl das auf den ersten Blick plausibel klingt, was die eigentliche Bedeutung des Begriffs verfehlt" }, "Stundenzeiger auf die Sonne richten - die Winkelhalbierende zur 12 zeigt ungefähr Süden",
            "Richtet man den Stundenzeiger auf die Sonne, zeigt die Winkelhalbierende zwischen Stundenzeiger und der 12 auf der Nordhalbkugel ungefähr nach Süden."),
        ("Was ist eine Legende auf einer Landkarte?", new[] { "Eine Erklärung der verwendeten Symbole und Farben", "Eine erfundene Geschichte über die Region und deshalb hier nicht zutrifft", "Der Name des Kartenherstellers" }, "Eine Erklärung der verwendeten Symbole und Farben",
            "Die Legende einer Karte erklärt, wofür die verwendeten Symbole, Farben und Linien stehen.")
    };

    private static QuizQuestion Himmelsrichtungen(Random r)
    {
        var f = HimmelsrichtungListe[r.Next(HimmelsrichtungListe.Length)];
        return new QuizQuestion
        {
            Id = NewId(), Subject = Subject.Gewi, GradeLevel = GradeLevel.Klasse6,
            Topic = "Kartenkunde und Himmelsrichtungen", Type = QuestionType.MultipleChoice,
            Prompt = f.Frage, Options = f.Optionen, CorrectAnswers = new[] { f.Antwort }, Explanation = f.Erklaerung,
            HelpHint = "Auf genordeten Karten gilt: oben Norden, unten Süden, rechts Osten, links Westen. In Deutschland steht die Mittagssonne im Süden."
        };
    }

    private static readonly (string Frage, string[] Optionen, string Antwort, string Erklaerung)[] KinderrechteListe =
    {
        ("Was ist ein Grundrecht von Kindern laut UN-Kinderrechtskonvention?", new[] { "Das Recht auf Bildung", "Das Recht, den ganzen Tag zu spielen ohne Schule", "Das Recht, alles zu entscheiden" },
            "Das Recht auf Bildung", "Die UN-Kinderrechtskonvention garantiert u.a. das Recht auf Bildung, Schutz und Mitsprache."),
        ("Wer hat die Kinderrechte in der UN-Kinderrechtskonvention festgelegt?", new[] { "Die Vereinten Nationen (UN)", "Nur Deutschland", "Die Schule" }, "Die Vereinten Nationen (UN)",
            "Die UN-Kinderrechtskonvention von 1989 wurde von den Vereinten Nationen beschlossen und gilt in fast allen Ländern."),
        ("Welches Recht schützt Kinder laut Kinderrechtskonvention vor Gewalt und Ausbeutung?", new[] { "Das Recht auf Schutz", "Das Recht auf ein eigenes Auto", "Das Recht auf unbegrenztes Fernsehen" }, "Das Recht auf Schutz",
            "Das Recht auf Schutz bewahrt Kinder u.a. vor Gewalt, Vernachlässigung und Ausbeutung (z.B. Kinderarbeit)."),
        ("Was bedeutet das Recht auf Mitsprache (Partizipation) für Kinder?", new[] { "Kinder dürfen bei sie betreffenden Entscheidungen ihre Meinung äußern", "Kinder entscheiden allein über alles in der Familie, was so nicht korrekt ist", "Kinder müssen zu allem schweigen" }, "Kinder dürfen bei sie betreffenden Entscheidungen ihre Meinung äußern",
            "Das Recht auf Mitsprache bedeutet, dass die Meinung von Kindern bei Entscheidungen, die sie betreffen, gehört und berücksichtigt werden soll."),
        ("Was garantiert Kindern das Recht auf Bildung laut Kinderrechtskonvention?", new[] { "Zugang zu Schule und Ausbildung, unabhängig von Herkunft", "Nur Kindern aus reichen Familien Schulbesuch", "Freiwilliger Schulbesuch nur für Jungen" }, "Zugang zu Schule und Ausbildung, unabhängig von Herkunft",
            "Das Recht auf Bildung soll allen Kindern unabhängig von Herkunft, Geschlecht oder Vermögen der Eltern den Zugang zu Schule ermöglichen."),
        ("In welchem Jahr wurde die UN-Kinderrechtskonvention verabschiedet?", new[] { "1989", "1949", "2000" }, "1989",
            "Die UN-Kinderrechtskonvention wurde 1989 von den Vereinten Nationen verabschiedet und gilt inzwischen in fast allen Ländern."),
        ("Welches Recht garantiert Kindern eine eigene Meinung und Gehör bei sie betreffenden Entscheidungen?", new[] { "Recht auf Meinungsäußerung und Mitbestimmung", "Recht auf ein eigenes Auto", "Recht, nie zur Schule zu müssen" }, "Recht auf Meinungsäußerung und Mitbestimmung",
            "Das Recht auf Meinungsäußerung und Mitbestimmung gibt Kindern die Möglichkeit, bei sie betreffenden Entscheidungen gehört zu werden."),
        ("Was garantiert das Recht auf Gesundheit für Kinder?", new[] { "Zugang zu medizinischer Versorgung und gesunder Ernährung", "Unbegrenzten Konsum von Süßigkeiten - eine haeufige, aber unzutreffende Vorstellung", "Keine Impfungen" }, "Zugang zu medizinischer Versorgung und gesunder Ernährung",
            "Das Recht auf Gesundheit sichert Kindern Zugang zu medizinischer Versorgung, sauberem Wasser und ausreichender Ernährung."),
        ("Was bedeutet das Recht auf Spiel und Freizeit?", new[] { "Kinder dürfen spielen, sich erholen und an Freizeitaktivitäten teilnehmen", "Kinder müssen den ganzen Tag arbeiten, auch wenn das manche zunaechst vermuten wuerden", "Kinder dürfen nie draußen spielen" }, "Kinder dürfen spielen, sich erholen und an Freizeitaktivitäten teilnehmen",
            "Das Recht auf Spiel und Freizeit erkennt an, dass Spielen, Erholung und Freizeit wichtig für die Entwicklung von Kindern sind."),
        ("Was schützt das Recht auf Gleichbehandlung (Diskriminierungsverbot)?", new[] { "Kinder dürfen nicht wegen Herkunft, Geschlecht oder Religion benachteiligt werden", "Nur bestimmte Kinder haben Rechte", "Mädchen haben weniger Rechte als Jungen, was bei genauerem Hinsehen nicht stimmt (was so in der Praxis nicht zutrifft)" }, "Kinder dürfen nicht wegen Herkunft, Geschlecht oder Religion benachteiligt werden",
            "Das Diskriminierungsverbot stellt sicher, dass alle Kinder unabhängig von Herkunft, Geschlecht oder Religion dieselben Rechte haben."),
        ("Wer trägt laut Kinderrechtskonvention die Hauptverantwortung, Kinderrechte umzusetzen?", new[] { "In erster Linie die Staaten, die die Konvention unterschrieben haben", "Nur die Kinder selbst", "Niemand ist dafür zuständig - eine verbreitete, aber falsche Annahme, was einer genaueren Pruefung nicht standhaelt" }, "In erster Linie die Staaten, die die Konvention unterschrieben haben",
            "Die Vertragsstaaten der Kinderrechtskonvention verpflichten sich, die darin festgelegten Rechte in ihrem Land umzusetzen."),
        ("Was versteht man unter dem \"Kindeswohl\" als Leitprinzip der Kinderrechtskonvention?", new[] { "Bei allen Entscheidungen soll das Wohl des Kindes vorrangig berücksichtigt werden", "Nur wirtschaftliche Interessen zählen, obwohl das auf den ersten Blick plausibel klingt, was die eigentliche Bedeutung des Begriffs verfehlt", "Kinder haben kein eigenes Wohl" }, "Bei allen Entscheidungen soll das Wohl des Kindes vorrangig berücksichtigt werden",
            "Das Kindeswohl ist ein zentrales Leitprinzip: Bei Entscheidungen, die Kinder betreffen, soll ihr Wohl vorrangig berücksichtigt werden."),
        ("Welche Organisation setzt sich weltweit besonders für die Umsetzung der Kinderrechte ein?", new[] { "UNICEF", "Die FIFA", "Die NATO" }, "UNICEF",
            "UNICEF ist das Kinderhilfswerk der Vereinten Nationen und setzt sich weltweit für die Umsetzung der Kinderrechte ein."),
        ("Was ist ein Beispiel für eine Verletzung von Kinderrechten weltweit?", new[] { "Kinderarbeit, bei der Kinder statt zur Schule zu gehen arbeiten müssen", "Ein Kind geht freiwillig ins Schwimmbad", "Ein Kind bekommt ein Buch geschenkt" }, "Kinderarbeit, bei der Kinder statt zur Schule zu gehen arbeiten müssen",
            "Kinderarbeit verletzt u.a. das Recht auf Bildung und Schutz, wenn Kinder statt zur Schule zu gehen arbeiten müssen."),
        ("Was bedeutet das Recht auf Privatsphäre für Kinder?", new[] { "Kinder haben ein Recht auf Schutz ihrer persönlichen Daten und ihres Privatlebens", "Kinder dürfen keine eigenen Gedanken haben", "Alles über ein Kind darf veröffentlicht werden" }, "Kinder haben ein Recht auf Schutz ihrer persönlichen Daten und ihres Privatlebens",
            "Das Recht auf Privatsphäre schützt Kinder davor, dass persönliche Daten oder Bilder ohne Zustimmung veröffentlicht werden."),
        ("Welches Alter gilt laut UN-Kinderrechtskonvention allgemein als Obergrenze für \"Kind\"?", new[] { "Unter 18 Jahre", "Unter 10 Jahre", "Unter 6 Jahre" }, "Unter 18 Jahre",
            "Laut UN-Kinderrechtskonvention gilt grundsätzlich jeder Mensch unter 18 Jahren als Kind."),
        ("Was können Kinder tun, wenn ihre Rechte verletzt werden?", new[] { "Sich an Vertrauenspersonen, Beratungsstellen oder Hilfsorganisationen wenden", "Gar nichts tun können", "Nur selbst ein Gesetz erlassen" }, "Sich an Vertrauenspersonen, Beratungsstellen oder Hilfsorganisationen wenden",
            "Bei Rechtsverletzungen können sich Kinder an Vertrauenspersonen, Beratungsstellen oder Kinderhilfsorganisationen wenden."),
        ("Welches Recht schützt Kinder speziell vor Kinderarbeit und Ausbeutung?", new[] { "Das Recht auf Schutz vor wirtschaftlicher Ausbeutung", "Das Recht auf ein eigenes Unternehmen und deshalb hier nicht zutrifft", "Das Recht, überall zu arbeiten" }, "Das Recht auf Schutz vor wirtschaftlicher Ausbeutung",
            "Das Recht auf Schutz vor wirtschaftlicher Ausbeutung bewahrt Kinder vor gefährlicher oder ausbeuterischer Arbeit."),
        ("Warum ist das Recht auf Bildung besonders wichtig für Mädchen weltweit?", new[] { "In manchen Regionen haben Mädchen seltener Zugang zu Schulbildung als Jungen", "Mädchen brauchen laut Konvention keine Bildung", "Bildung ist nur für Jungen vorgesehen" }, "In manchen Regionen haben Mädchen seltener Zugang zu Schulbildung als Jungen",
            "In manchen Regionen der Welt haben Mädchen immer noch seltener Zugang zu Schulbildung als Jungen - das Recht auf Bildung gilt aber für alle Kinder gleichermaßen."),
        ("Was garantiert das Recht auf einen Namen und eine Staatsangehörigkeit ab der Geburt?", new[] { "Jedes Kind soll rechtlich anerkannt und registriert werden", "Nur manche Kinder dürfen einen Namen tragen, was so nicht korrekt ist", "Ein Name ist für Kinder nicht wichtig" }, "Jedes Kind soll rechtlich anerkannt und registriert werden",
            "Das Recht auf Namen und Staatsangehörigkeit sichert jedem Kind von Geburt an eine rechtliche Identität und Zugehörigkeit.")
    };

    private static QuizQuestion Kinderrechte(Random r)
    {
        var f = KinderrechteListe[r.Next(KinderrechteListe.Length)];
        return new QuizQuestion
        {
            Id = NewId(), Subject = Subject.Gewi, GradeLevel = GradeLevel.Klasse6,
            Topic = "Kinderrechte", Type = QuestionType.MultipleChoice,
            Prompt = f.Frage, Options = f.Optionen, CorrectAnswers = new[] { f.Antwort }, Explanation = f.Erklaerung,
            HelpHint = "Die UN-Kinderrechtskonvention (1989) wurde von den Vereinten Nationen beschlossen und schützt u.a. Bildung, Schutz und Mitsprache von Kindern."
        };
    }

    private static readonly (string Frage, string[] Optionen, string Antwort, string Erklaerung)[] ErnaehrungListe =
    {
        ("Warum werden in der Landwirtschaft heute oft Maschinen statt Handarbeit eingesetzt?", new[] { "Um mehr Menschen mit weniger Aufwand zu ernähren (höherer Ertrag)", "Weil es das Gesetz vorschreibt", "Weil Maschinen billiger sind als ein einziger Sack Saatgut" },
            "Um mehr Menschen mit weniger Aufwand zu ernähren (höherer Ertrag)", "Moderne Landwirtschaft (Maschinen, Dünger) erhöht den Ertrag pro Fläche, damit mehr Menschen ernährt werden können."),
        ("Was bedeutet \"Verbraucherschutz\" beim Thema Ernährung?", new[] { "Kundinnen und Kunden vor gesundheitsschädlichen oder falsch gekennzeichneten Lebensmitteln schützen", "Lebensmittel möglichst teuer machen", "Nur ausländische Lebensmittel verbieten - eine haeufige, aber unzutreffende Vorstellung, auch wenn das manche zunaechst vermuten wuerden" },
            "Kundinnen und Kunden vor gesundheitsschädlichen oder falsch gekennzeichneten Lebensmitteln schützen", "Verbraucherschutz sorgt z.B. durch Kontrollen und Kennzeichnungspflichten dafür, dass Lebensmittel sicher und ehrlich beworben sind."),
        ("Was versteht man unter \"Überfluss und Mangel\" bei der weltweiten Ernährung?", new[] { "In manchen Regionen der Welt gibt es zu viel Nahrung, in anderen zu wenig", "Überall auf der Welt gibt es gleich viel zu essen, was bei genauerem Hinsehen nicht stimmt", "Mangel bedeutet, dass Essen schlecht schmeckt" },
            "In manchen Regionen der Welt gibt es zu viel Nahrung, in anderen zu wenig", "Weltweit ist Nahrung ungleich verteilt: In manchen Ländern werden Lebensmittel verschwendet, in anderen herrscht Hunger."),
        ("Was bedeutet \"regionale und saisonale\" Ernährung?", new[] { "Lebensmittel aus der Umgebung essen, die gerade Saison haben", "Nur Lebensmittel aus dem Ausland essen", "Jeden Tag genau dasselbe essen" },
            "Lebensmittel aus der Umgebung essen, die gerade Saison haben", "Regionale und saisonale Ernährung bedeutet, Obst und Gemüse aus der Nähe zu essen, wenn sie gerade natürlich reifen - das spart oft Transportwege."),
        ("Warum ist Lebensmittelverschwendung ein Problem?", new[] { "Wertvolle Ressourcen (Wasser, Anbaufläche, Energie) werden umsonst verbraucht", "Weil dadurch mehr Menschen satt werden", "Weil weggeworfenes Essen automatisch neue Pflanzen wachsen lässt" },
            "Wertvolle Ressourcen (Wasser, Anbaufläche, Energie) werden umsonst verbraucht", "Für die Herstellung von Lebensmitteln werden Wasser, Fläche und Energie verbraucht - wird das Essen weggeworfen, war das alles umsonst."),
        ("Was bedeutet \"ökologische Landwirtschaft\" (Bio-Landbau)?", new[] { "Anbau ohne chemisch-synthetische Pestizide und Düngemittel", "Anbau mit möglichst viel Chemie", "Anbau nur in Gewächshäusern" },
            "Anbau ohne chemisch-synthetische Pestizide und Düngemittel", "Ökologische Landwirtschaft verzichtet bewusst auf chemisch-synthetische Pestizide und Düngemittel und setzt auf natürliche Methoden."),
        ("Was ist eine ausgewogene Ernährung?", new[] { "Eine Mischung aus verschiedenen Lebensmittelgruppen wie Obst, Gemüse, Getreide und Eiweiß", "Nur Süßigkeiten essen", "Nur ein einziges Lebensmittel essen (was so in der Praxis nicht zutrifft) - eine verbreitete, aber falsche Annahme" },
            "Eine Mischung aus verschiedenen Lebensmittelgruppen wie Obst, Gemüse, Getreide und Eiweiß", "Eine ausgewogene Ernährung besteht aus einer abwechslungsreichen Mischung verschiedener Lebensmittelgruppen."),
        ("Warum ist Trinkwasserqualität ein wichtiges Ernährungsthema weltweit?", new[] { "Sauberes Wasser ist überlebenswichtig, aber nicht überall verfügbar", "Wasser hat nichts mit Ernährung zu tun", "Alle Menschen haben automatisch sauberes Wasser, was einer genaueren Pruefung nicht standhaelt" },
            "Sauberes Wasser ist überlebenswichtig, aber nicht überall verfügbar", "Sauberes Trinkwasser ist lebensnotwendig, doch weltweit haben nicht alle Menschen gleichermaßen Zugang dazu."),
        ("Was versteht man unter einem \"ökologischen Fußabdruck\" bei der Ernährung?", new[] { "Wie stark die Umwelt durch die eigene Ernährung belastet wird", "Die Schuhgröße eines Menschen", "Die Anzahl der Mahlzeiten pro Tag" },
            "Wie stark die Umwelt durch die eigene Ernährung belastet wird", "Der ökologische Fußabdruck der Ernährung zeigt, wie stark Anbau, Transport und Verzehr von Lebensmitteln die Umwelt belasten."),
        ("Warum wird der Fleischkonsum oft kritisch diskutiert?", new[] { "Tierhaltung braucht viel Fläche, Wasser und verursacht Treibhausgase", "Fleisch hat keinerlei Umweltwirkung", "Fleisch wächst ohne Ressourcen" },
            "Tierhaltung braucht viel Fläche, Wasser und verursacht Treibhausgase", "Die Haltung von Nutztieren benötigt viel Fläche und Wasser und trägt zu Treibhausgasemissionen bei."),
        ("Was bedeutet \"Welternährungsproblem\"?", new[] { "Trotz genug produzierter Nahrung weltweit hungern viele Menschen wegen ungleicher Verteilung", "Es gibt weltweit zu wenig Nahrung, um alle zu ernähren, obwohl das auf den ersten Blick plausibel klingt", "Alle Menschen der Welt haben genug zu essen" },
            "Trotz genug produzierter Nahrung weltweit hungern viele Menschen wegen ungleicher Verteilung", "Weltweit wird genug Nahrung produziert, doch durch ungleiche Verteilung hungern trotzdem viele Menschen."),
        ("Was ist der Unterschied zwischen konventioneller und ökologischer Landwirtschaft beim Düngen?", new[] { "Ökologische Landwirtschaft nutzt v.a. natürlichen Dünger statt chemisch-synthetischen", "Beide nutzen identische Methoden", "Ökolandwirtschaft düngt gar nicht, was die eigentliche Bedeutung des Begriffs verfehlt und deshalb hier nicht zutrifft" },
            "Ökologische Landwirtschaft nutzt v.a. natürlichen Dünger statt chemisch-synthetischen", "Ökologische Landwirtschaft setzt vor allem auf natürlichen Dünger, konventionelle Landwirtschaft nutzt oft zusätzlich chemisch-synthetischen Dünger."),
        ("Was bewirken Transportwege (\"Lebensmittel-Kilometer\") bei importierten Lebensmitteln?", new[] { "Lange Transportwege verursachen zusätzliche Treibhausgase", "Sie machen Lebensmittel automatisch gesünder, was so nicht korrekt ist", "Sie haben keinerlei Umweltwirkung" },
            "Lange Transportwege verursachen zusätzliche Treibhausgase", "Je weiter ein Lebensmittel transportiert wird, desto mehr Treibhausgase entstehen in der Regel dabei."),
        ("Was ist ein Vorteil des Mindesthaltbarkeitsdatums (MHD) für Verbraucher?", new[] { "Es gibt eine Orientierung, bis wann ein Lebensmittel bei richtiger Lagerung mindestens gut bleibt", "Nach dem MHD ist das Lebensmittel sofort giftig - eine haeufige, aber unzutreffende Vorstellung, auch wenn das manche zunaechst vermuten wuerden", "Es zeigt den Preis des Lebensmittels" },
            "Es gibt eine Orientierung, bis wann ein Lebensmittel bei richtiger Lagerung mindestens gut bleibt", "Das MHD gibt an, bis wann ein Lebensmittel bei richtiger Lagerung mindestens seine Qualität behält - danach ist es oft noch genießbar."),
        ("Warum spielt Fairtrade auch bei Lebensmitteln wie Kaffee oder Kakao eine Rolle?", new[] { "Es sichert Erzeugerinnen und Erzeugern in ärmeren Ländern faire Preise", "Es macht Lebensmittel automatisch billiger", "Es hat nichts mit Ernährung zu tun" },
            "Es sichert Erzeugerinnen und Erzeugern in ärmeren Ländern faire Preise", "Fairtrade-Siegel bei Kaffee oder Kakao garantieren den Erzeugerinnen und Erzeugern faire Mindestpreise."),
        ("Was bedeutet \"Monokultur\" in der Landwirtschaft?", new[] { "Über lange Zeit wird auf einer Fläche nur eine einzige Pflanzenart angebaut", "Auf einer Fläche wachsen viele verschiedene Pflanzenarten gleichzeitig", "Es wird gar nichts angebaut" },
            "Über lange Zeit wird auf einer Fläche nur eine einzige Pflanzenart angebaut", "Bei Monokultur wird über lange Zeit immer nur eine einzige Pflanzenart auf derselben Fläche angebaut."),
        ("Welches Problem kann Monokultur für den Boden verursachen?", new[] { "Der Boden wird einseitig ausgelaugt und weniger fruchtbar", "Der Boden wird automatisch fruchtbarer", "Es gibt keinerlei Auswirkungen" },
            "Der Boden wird einseitig ausgelaugt und weniger fruchtbar", "Monokultur entzieht dem Boden immer dieselben Nährstoffe, wodurch er auf Dauer ausgelaugt wird."),
        ("Was sind Grundnahrungsmittel?", new[] { "Lebensmittel, die einen Großteil der täglichen Ernährung einer Bevölkerung ausmachen, z.B. Reis, Brot, Kartoffeln", "Nur seltene Delikatessen, was bei genauerem Hinsehen nicht stimmt (was so in der Praxis nicht zutrifft) - eine verbreitete, aber falsche Annahme", "Nur Süßwaren" },
            "Lebensmittel, die einen Großteil der täglichen Ernährung einer Bevölkerung ausmachen, z.B. Reis, Brot, Kartoffeln", "Grundnahrungsmittel wie Reis, Brot oder Kartoffeln decken einen Großteil des täglichen Energiebedarfs vieler Menschen."),
        ("Warum ist Reis für einen großen Teil der Weltbevölkerung ein besonders wichtiges Grundnahrungsmittel?", new[] { "Er lässt sich in vielen Klimazonen anbauen und ernährt sehr viele Menschen", "Er wächst nur in Deutschland", "Er wird nirgendwo mehr angebaut, was einer genaueren Pruefung nicht standhaelt, obwohl das auf den ersten Blick plausibel klingt" },
            "Er lässt sich in vielen Klimazonen anbauen und ernährt sehr viele Menschen", "Reis lässt sich in vielen warmen, feuchten Regionen anbauen und ist für einen sehr großen Teil der Weltbevölkerung Hauptnahrungsmittel."),
        ("Was kann jede Person selbst tun, um Lebensmittelverschwendung zu Hause zu verringern?", new[] { "Einkäufe planen und Reste sinnvoll weiterverwenden", "Möglichst viel einkaufen und oft wegwerfen, was die eigentliche Bedeutung des Begriffs verfehlt", "Nie mehr einkaufen gehen" },
            "Einkäufe planen und Reste sinnvoll weiterverwenden", "Wer Einkäufe plant und Reste weiterverwendet, wirft weniger Lebensmittel weg und spart dadurch auch Ressourcen.")
    };

    private static QuizQuestion Ernaehrung(Random r)
    {
        var f = ErnaehrungListe[r.Next(ErnaehrungListe.Length)];
        return new QuizQuestion
        {
            Id = NewId(), Subject = Subject.Gewi, GradeLevel = GradeLevel.Klasse6,
            Topic = "Ernährung – wie werden Menschen satt?", Type = QuestionType.MultipleChoice,
            Prompt = f.Frage, Options = f.Optionen, CorrectAnswers = new[] { f.Antwort }, Explanation = f.Erklaerung,
            HelpHint = "Denk an Landwirtschaft (Ertrag steigern), Verbraucherschutz (sichere Lebensmittel) und die ungleiche Verteilung von Nahrung weltweit."
        };
    }

    private static readonly (string Frage, string[] Optionen, string Antwort, string Erklaerung)[] WasserListe =
    {
        ("Warum entstand eine der ersten Hochkulturen der Geschichte gerade am Nil in Ägypten?", new[] { "Die jährliche Nilüberschwemmung machte das Land fruchtbar für Ackerbau", "Am Nil gab es überhaupt kein Wasser", "Der Nil war zu gefährlich, um dort zu siedeln und deshalb hier nicht zutrifft, was so nicht korrekt ist" }, "Die jährliche Nilüberschwemmung machte das Land fruchtbar für Ackerbau",
            "Die regelmäßige Überschwemmung des Nils lagerte fruchtbaren Schlamm ab, was zuverlässigen Ackerbau und damit eine frühe Hochkultur ermöglichte."),
        ("Wozu nutzten die alten Ägypter aufwendige Bewässerungssysteme am Nil?", new[] { "Um Wasser gezielt auf die Felder zu leiten", "Um den Fluss trockenzulegen - eine haeufige, aber unzutreffende Vorstellung", "Um Boote zu verbrennen" }, "Um Wasser gezielt auf die Felder zu leiten",
            "Kanäle und Bewässerungssysteme leiteten Nilwasser gezielt auf die Felder, auch abseits der direkten Uferzone."),
        ("Warum kann Wasser heute zwischen manchen Staaten zu einem Konfliktfaktor werden?", new[] { "Flüsse fließen durch mehrere Länder, die sich um die Nutzung streiten können", "Wasser ist überall unbegrenzt vorhanden", "Wasser hat keinerlei wirtschaftliche Bedeutung, auch wenn das manche zunaechst vermuten wuerden" }, "Flüsse fließen durch mehrere Länder, die sich um die Nutzung streiten können",
            "Grenzüberschreitende Flüsse wie der Nil werden von mehreren Staaten genutzt, was zu Konflikten um Wasserrechte führen kann."),
        ("Wofür wird Wasser als Wirtschaftsfaktor neben der Landwirtschaft noch genutzt?", new[] { "Für Transport (Schifffahrt) und Energiegewinnung (Wasserkraft)", "Nur zum Blumengießen", "Wasser hat keine wirtschaftliche Bedeutung, was bei genauerem Hinsehen nicht stimmt" }, "Für Transport (Schifffahrt) und Energiegewinnung (Wasserkraft)",
            "Flüsse und Meere dienen als Transportwege für Schiffe und liefern über Wasserkraftwerke Energie."),
        ("Was ist ein Beispiel dafür, dass Wasser auch ein Freizeitfaktor ist?", new[] { "Schwimmen, Segeln oder Angeln an Seen und Flüssen", "Wasser wird nie für Freizeit genutzt", "Nur zum Putzen verwendet" }, "Schwimmen, Segeln oder Angeln an Seen und Flüssen",
            "Seen, Flüsse und Meere werden von vielen Menschen zum Baden, Segeln, Angeln oder für andere Freizeitaktivitäten genutzt."),
        ("Wie formt Wasser über lange Zeiträume Küsten und Landschaften?", new[] { "Durch Erosion, also das Abtragen von Gestein und Boden", "Wasser hat keinerlei Einfluss auf Landschaften", "Nur durch Regen in der Wüste" }, "Durch Erosion, also das Abtragen von Gestein und Boden",
            "Wellen, Strömungen und fließendes Wasser tragen über lange Zeit Gestein und Boden ab und formen so Küsten und Täler."),
        ("Was versteht man unter Wasserknappheit?", new[] { "In einer Region steht nicht genug sauberes Wasser für den Bedarf zur Verfügung", "Es gibt überall auf der Welt zu viel Wasser", "Ein anderes Wort für Überschwemmung" }, "In einer Region steht nicht genug sauberes Wasser für den Bedarf zur Verfügung",
            "Wasserknappheit bedeutet, dass in einer Region nicht ausreichend sauberes Trinkwasser für Menschen, Landwirtschaft und Industrie vorhanden ist."),
        ("Warum war der Nil für die alten Ägypter wichtiger als Regen?", new[] { "In Ägypten regnet es nur sehr selten, der Nil lieferte zuverlässig Wasser", "In Ägypten regnete es jeden Tag stark (was so in der Praxis nicht zutrifft) - eine verbreitete, aber falsche Annahme", "Regen war für den Ackerbau schädlich" }, "In Ägypten regnet es nur sehr selten, der Nil lieferte zuverlässig Wasser",
            "Ägypten liegt größtenteils in der Wüste mit sehr wenig Niederschlag - ohne den Nil wäre dauerhafter Ackerbau dort kaum möglich gewesen."),
        ("Was ist ein Wasserkraftwerk?", new[] { "Eine Anlage, die die Bewegungsenergie von Wasser in elektrischen Strom umwandelt", "Ein Gebäude zum Trinkwasser abfüllen, was einer genaueren Pruefung nicht standhaelt, obwohl das auf den ersten Blick plausibel klingt", "Ein Ort, an dem nur geschwommen wird" }, "Eine Anlage, die die Bewegungsenergie von Wasser in elektrischen Strom umwandelt",
            "In Wasserkraftwerken treibt strömendes oder fallendes Wasser Turbinen an, die daraus elektrischen Strom erzeugen."),
        ("Warum ist der Zugang zu sauberem Trinkwasser weltweit ungleich verteilt?", new[] { "Klima, Geografie und wirtschaftliche Mittel unterscheiden sich stark zwischen Regionen", "Alle Länder der Welt haben exakt gleich viel Wasser", "Trinkwasser ist überall unbegrenzt kostenlos verfügbar" }, "Klima, Geografie und wirtschaftliche Mittel unterscheiden sich stark zwischen Regionen",
            "Trockene Klimazonen, geografische Lage und fehlende Infrastruktur führen dazu, dass sauberes Trinkwasser weltweit sehr ungleich verteilt ist."),
        ("Was zeigt das Beispiel Ägypten über den Zusammenhang von Wasser und der Entstehung früher Staaten?", new[] { "Zuverlässige Wasserversorgung ermöglichte Ackerbau, Vorratshaltung und damit eine organisierte Gesellschaft", "Wasser hatte für die Entstehung von Staaten keine Bedeutung", "Staaten entstanden nur in wasserlosen Wüstenregionen" }, "Zuverlässige Wasserversorgung ermöglichte Ackerbau, Vorratshaltung und damit eine organisierte Gesellschaft",
            "Verlässliche Ernten durch Bewässerung schufen Nahrungsüberschüsse, die Arbeitsteilung, Handel und eine zentrale Verwaltung wie im alten Ägypten ermöglichten."),
        ("Was ist eine mögliche Folge, wenn zwei Länder um dasselbe Flusswasser konkurrieren?", new[] { "Politische Spannungen oder Verhandlungen über die gerechte Wasserverteilung", "Der Fluss verschwindet automatisch, was die eigentliche Bedeutung des Begriffs verfehlt", "Es entstehen keinerlei Probleme" }, "Politische Spannungen oder Verhandlungen über die gerechte Wasserverteilung",
            "Wenn mehrere Staaten von einem Fluss abhängig sind, kann die Nutzung zu politischen Spannungen führen, die oft durch Verhandlungen gelöst werden müssen."),
        ("Was passiert mit einer Küstenlinie, wenn Wellen über lange Zeit weiches Gestein abtragen?", new[] { "Es können Buchten, Klippen oder Strände entstehen", "Die Küstenlinie bleibt für immer exakt gleich und deshalb hier nicht zutrifft", "Das Meer verschwindet komplett" }, "Es können Buchten, Klippen oder Strände entstehen",
            "Die Erosionskraft des Meeres formt über sehr lange Zeiträume charakteristische Küstenformen wie Buchten, Klippen oder Sandstrände."),
        ("Was bedeutet \"Bewässerungsfeldbau\", wie ihn die alten Ägypter betrieben?", new[] { "Felder werden gezielt künstlich mit Wasser aus Kanälen versorgt", "Felder werden nur vom Regen bewässert", "Felder werden absichtlich trockengelegt, was so nicht korrekt ist - eine haeufige, aber unzutreffende Vorstellung" }, "Felder werden gezielt künstlich mit Wasser aus Kanälen versorgt",
            "Beim Bewässerungsfeldbau wird Wasser über Kanäle und Gräben gezielt zu den Feldern geleitet, unabhängig vom Regen."),
        ("Warum kann übermäßige Wasserentnahme aus einem Fluss für Anwohner flussabwärts problematisch sein?", new[] { "Ihnen bleibt dann weniger Wasser für Landwirtschaft und Alltag übrig", "Es hat keinerlei Auswirkungen flussabwärts, auch wenn das manche zunaechst vermuten wuerden", "Der Fluss wird dadurch automatisch breiter" }, "Ihnen bleibt dann weniger Wasser für Landwirtschaft und Alltag übrig",
            "Wird flussaufwärts sehr viel Wasser entnommen, kann flussabwärts weniger Wasser für Landwirtschaft, Trinkwasser und Industrie übrig bleiben."),
        ("Welche Rolle spielten Nilüberschwemmungen für die Kalenderentwicklung im alten Ägypten?", new[] { "Der Zeitpunkt der Überschwemmung half, ein frühes Kalendersystem zu entwickeln", "Überschwemmungen hatten nichts mit dem Kalender zu tun", "Es gab in Ägypten nie einen Kalender" }, "Der Zeitpunkt der Überschwemmung half, ein frühes Kalendersystem zu entwickeln",
            "Da die Nilüberschwemmung regelmäßig zur gleichen Jahreszeit auftrat, orientierten sich die alten Ägypter bei ihrem Kalender daran."),
        ("Was ist ein Stausee?", new[] { "Ein künstlich angelegter See hinter einem Damm, der Wasser speichert", "Ein natürlicher See ohne jeden menschlichen Einfluss", "Ein anderes Wort für Ozean" }, "Ein künstlich angelegter See hinter einem Damm, der Wasser speichert",
            "Ein Stausee entsteht, wenn ein Damm einen Fluss aufstaut, um Wasser für Bewässerung, Trinkwasser oder Stromerzeugung zu speichern."),
        ("Wofür wird der Assuan-Staudamm am Nil in Ägypten heute unter anderem genutzt?", new[] { "Für Hochwasserschutz und Stromerzeugung", "Nur zum Baden für Touristen", "Er hat keinerlei Funktion" }, "Für Hochwasserschutz und Stromerzeugung",
            "Der Assuan-Staudamm reguliert seit dem 20. Jahrhundert die Nilüberschwemmungen und erzeugt zugleich elektrischen Strom."),
        ("Warum ist der sparsame Umgang mit Trinkwasser auch in wasserreichen Ländern wie Deutschland sinnvoll?", new[] { "Die Aufbereitung von Trinkwasser kostet Energie und Ressourcen", "Wasser sparen hat überhaupt keinen Nutzen", "In Deutschland gibt es unendlich viel Wasser, was bei genauerem Hinsehen nicht stimmt" }, "Die Aufbereitung von Trinkwasser kostet Energie und Ressourcen",
            "Auch in Ländern mit viel Niederschlag verbraucht die Aufbereitung und Verteilung von Trinkwasser Energie und Ressourcen, weshalb Sparen sinnvoll bleibt."),
        ("Was verbindet die Themen \"Wasser als Konfliktfaktor\" und \"Wasser als Wirtschaftsfaktor\" miteinander?", new[] { "Beide zeigen, wie wichtig die begrenzte Ressource Wasser für Gesellschaften ist", "Beide Themen haben nichts miteinander zu tun", "Wasser ist niemals wirtschaftlich oder politisch bedeutsam (was so in der Praxis nicht zutrifft)" }, "Beide zeigen, wie wichtig die begrenzte Ressource Wasser für Gesellschaften ist",
            "Ob als Streitpunkt zwischen Staaten oder als Grundlage für Landwirtschaft und Energie - Wasser ist eine begrenzte, gesellschaftlich zentrale Ressource.")
    };

    private static QuizQuestion WasserAlsRessource(Random r)
    {
        var f = WasserListe[r.Next(WasserListe.Length)];
        return new QuizQuestion
        {
            Id = NewId(), Subject = Subject.Gewi, GradeLevel = GradeLevel.Klasse6,
            Topic = "Wasser – nur Natur oder in Menschenhand?", Type = QuestionType.MultipleChoice,
            Prompt = f.Frage, Options = f.Optionen, CorrectAnswers = new[] { f.Antwort }, Explanation = f.Erklaerung,
            HelpHint = "Der Nil ermöglichte durch Bewässerung die Hochkultur im alten Ägypten; heute ist Wasser zugleich Konflikt-, Wirtschafts- und Freizeitfaktor."
        };
    }

    private static readonly (string Frage, string[] Optionen, string Antwort, string Erklaerung)[] StadtListe =
    {
        ("Wie viele Einwohner hatte die antike Stadt Rom auf ihrem Höhepunkt schätzungsweise?", new[] { "Über eine Million", "Nur etwa 500 - eine verbreitete, aber falsche Annahme", "Etwa 50" }, "Über eine Million",
            "Das antike Rom war mit schätzungsweise über einer Million Einwohnern eine der größten Städte der antiken Welt."),
        ("Was war das Forum Romanum im antiken Rom?", new[] { "Das zentrale, öffentliche Platz- und Marktgebiet der Stadt", "Ein privates Wohnhaus", "Ein Fluss außerhalb der Stadt, was einer genaueren Pruefung nicht standhaelt" }, "Das zentrale, öffentliche Platz- und Marktgebiet der Stadt",
            "Das Forum Romanum war das politische, religiöse und wirtschaftliche Zentrum Roms mit Tempeln, Märkten und öffentlichen Gebäuden."),
        ("Wie versorgten die Römer ihre Großstadt zuverlässig mit Trinkwasser?", new[] { "Durch Aquädukte, die Wasser über weite Strecken in die Stadt leiteten", "Sie hatten überhaupt kein fließendes Wasser", "Nur durch Regenwasser in Eimern" }, "Durch Aquädukte, die Wasser über weite Strecken in die Stadt leiteten",
            "Römische Aquädukte transportierten Wasser oft über viele Kilometer aus den Bergen in die Stadt Rom."),
        ("Was waren die Thermen im antiken Rom?", new[] { "Öffentliche Badeanlagen, die auch als soziale Treffpunkte dienten", "Private Gärten der Kaiser, obwohl das auf den ersten Blick plausibel klingt", "Getreidespeicher" }, "Öffentliche Badeanlagen, die auch als soziale Treffpunkte dienten",
            "Die Thermen waren große öffentliche Bäder, in denen sich die Menschen wuschen, entspannten und trafen."),
        ("Was ist ein typisches Merkmal moderner Großstädte wie dem Großraum Berlin?", new[] { "Hohe Bevölkerungsdichte und vielfältiges kulturelles Angebot", "Fast keine Einwohner", "Ausschließlich landwirtschaftliche Flächen" }, "Hohe Bevölkerungsdichte und vielfältiges kulturelles Angebot",
            "Moderne Großstädte wie Berlin zeichnen sich durch hohe Bevölkerungsdichte, vielfältige Kultur, Wirtschaft und Infrastruktur aus."),
        ("Was versteht man unter \"Verkehrsverdichtung\" in Großstädten?", new[] { "Viele Fahrzeuge auf engem Raum, oft mit Staus verbunden", "Es gibt in der Stadt überhaupt keinen Verkehr, was die eigentliche Bedeutung des Begriffs verfehlt", "Straßen werden ständig breiter gebaut" }, "Viele Fahrzeuge auf engem Raum, oft mit Staus verbunden",
            "Verkehrsverdichtung entsteht, wenn viele Menschen und Fahrzeuge auf begrenztem städtischem Raum unterwegs sind, was oft zu Staus führt."),
        ("Was ist ein Vorteil von kultureller Vielfalt in einer Großstadt?", new[] { "Unterschiedliche Perspektiven, Ideen und Angebote bereichern das Zusammenleben", "Vielfalt hat ausschließlich Nachteile", "Städte mit Vielfalt haben weniger Möglichkeiten und deshalb hier nicht zutrifft, was so nicht korrekt ist" }, "Unterschiedliche Perspektiven, Ideen und Angebote bereichern das Zusammenleben",
            "Kulturelle Vielfalt bringt unterschiedliche Ideen, Restaurants, Feste und Perspektiven zusammen und kann eine Stadt lebendiger machen."),
        ("Was ist ein Beispiel für Umweltbelastung durch eine Großstadt?", new[] { "Luftverschmutzung durch viele Fahrzeuge und Industrie", "Städte verursachen niemals Umweltprobleme", "Nur ländliche Gebiete verursachen Umweltbelastung - eine haeufige, aber unzutreffende Vorstellung" }, "Luftverschmutzung durch viele Fahrzeuge und Industrie",
            "Der hohe Verkehrs- und Energieverbrauch in Großstädten führt häufig zu Luftverschmutzung und anderen Umweltbelastungen."),
        ("Was war die \"Insula\" im antiken Rom?", new[] { "Ein mehrstöckiges Mietshaus für die einfache Stadtbevölkerung", "Ein Tempel für die Götter", "Ein römisches Kriegsschiff, auch wenn das manche zunaechst vermuten wuerden" }, "Ein mehrstöckiges Mietshaus für die einfache Stadtbevölkerung",
            "Insulae waren mehrstöckige Wohnhäuser, in denen die Mehrheit der einfachen römischen Stadtbevölkerung dicht gedrängt lebte."),
        ("Warum gilt Innovation als eine Chance großer Städte?", new[] { "Viele Menschen, Unternehmen und Ideen treffen auf engem Raum zusammen", "Innovation entsteht nur außerhalb von Städten, was bei genauerem Hinsehen nicht stimmt", "Städte verhindern grundsätzlich neue Ideen" }, "Viele Menschen, Unternehmen und Ideen treffen auf engem Raum zusammen",
            "Die Nähe vieler Menschen, Firmen, Universitäten und Ideen in Großstädten begünstigt oft neue Erfindungen und Entwicklungen."),
        ("Was regelte das römische Straßennetz für den Alltag der Stadt?", new[] { "Transport von Waren, Truppen und Menschen zwischen Städten und Regionen", "Es diente ausschließlich religiösen Prozessionen", "Straßen wurden im antiken Rom nicht gebaut" }, "Transport von Waren, Truppen und Menschen zwischen Städten und Regionen",
            "Das gut ausgebaute römische Straßennetz ermöglichte Handel, Truppenbewegungen und Reisen im gesamten Römischen Reich."),
        ("Was ist eine typische Herausforderung bei Wohnraum in modernen Großstädten?", new[] { "Wohnungen sind oft knapp und teuer", "Wohnraum ist in Städten immer im Überfluss vorhanden", "Wohnungen kosten in Städten grundsätzlich nichts" }, "Wohnungen sind oft knapp und teuer",
            "Durch die hohe Nachfrage in Großstädten sind Wohnungen häufig knapper und teurer als in ländlichen Gebieten."),
        ("Was war das Kolosseum im antiken Rom?", new[] { "Ein großes Amphitheater für öffentliche Unterhaltung wie Gladiatorenkämpfe", "Ein privates Wohnhaus für Bauern", "Ein Getreidespeicher außerhalb der Stadt (was so in der Praxis nicht zutrifft) - eine verbreitete, aber falsche Annahme" }, "Ein großes Amphitheater für öffentliche Unterhaltung wie Gladiatorenkämpfe",
            "Das Kolosseum bot Platz für tausende Zuschauer bei öffentlichen Veranstaltungen wie Gladiatorenkämpfen."),
        ("Wie unterscheidet sich der öffentliche Nahverkehr in Großstädten oft von dem auf dem Land?", new[] { "In Großstädten gibt es meist ein dichteres Netz an Bus, Bahn und U-Bahn", "Es gibt keinerlei Unterschied", "Auf dem Land ist der Nahverkehr immer dichter, was einer genaueren Pruefung nicht standhaelt" }, "In Großstädten gibt es meist ein dichteres Netz an Bus, Bahn und U-Bahn",
            "Aufgrund der hohen Bevölkerungsdichte lohnt sich in Großstädten meist ein dichteres öffentliches Verkehrsnetz als auf dem Land."),
        ("Was bedeutet \"Ghettoisierung\" oder soziale Trennung in manchen Stadtvierteln?", new[] { "Bestimmte Bevölkerungsgruppen leben räumlich stark getrennt voneinander", "Alle Stadtviertel sind automatisch sozial gleich gemischt, obwohl das auf den ersten Blick plausibel klingt", "Ein anderes Wort für Grünflächen" }, "Bestimmte Bevölkerungsgruppen leben räumlich stark getrennt voneinander",
            "In manchen Städten konzentrieren sich bestimmte Bevölkerungsgruppen in einzelnen Vierteln, was als Herausforderung für den sozialen Zusammenhalt gilt."),
        ("Warum brauchten römische Großstädte ein organisiertes System zur Getreideversorgung?", new[] { "So viele Menschen konnten sich nicht mehr selbst mit Nahrung versorgen", "Es gab in Rom keine hungrigen Menschen, was die eigentliche Bedeutung des Begriffs verfehlt", "Getreide wurde nur für Tiere gebraucht" }, "So viele Menschen konnten sich nicht mehr selbst mit Nahrung versorgen",
            "Bei über einer Million Einwohnern musste Getreide organisiert aus den Provinzen des Reiches nach Rom transportiert werden."),
        ("Was ist ein Vorteil von Grünflächen und Parks in modernen Großstädten?", new[] { "Sie verbessern Luftqualität und bieten Erholungsraum", "Sie schaden der Umwelt", "Sie werden in Städten grundsätzlich nicht benötigt" }, "Sie verbessern Luftqualität und bieten Erholungsraum",
            "Parks und Grünflächen verbessern das Stadtklima, filtern Luft und bieten Bewohnerinnen und Bewohnern Erholungsmöglichkeiten."),
        ("Was bezeichnete man im antiken Rom als \"Brot und Spiele\" (panem et circenses)?", new[] { "Kostenlose Getreideverteilung und öffentliche Unterhaltung zur Zufriedenstellung der Bevölkerung", "Ein Sportwettkampf ohne Zuschauer und deshalb hier nicht zutrifft, was so nicht korrekt ist - eine haeufige, aber unzutreffende Vorstellung", "Ein privates Familienessen" }, "Kostenlose Getreideverteilung und öffentliche Unterhaltung zur Zufriedenstellung der Bevölkerung",
            "Mit kostenlosem Getreide und öffentlichen Unterhaltungsveranstaltungen versuchten römische Herrscher, die Stadtbevölkerung zufriedenzustellen."),
        ("Was ist eine mögliche Chance kultureller Vielfalt für die lokale Wirtschaft einer Großstadt?", new[] { "Vielfältige Geschäfte, Restaurants und Dienstleistungen sprechen mehr Menschen an", "Vielfalt schadet immer der Wirtschaft", "Wirtschaftlich lohnt sich ausschließlich Einheitlichkeit, auch wenn das manche zunaechst vermuten wuerden" }, "Vielfältige Geschäfte, Restaurants und Dienstleistungen sprechen mehr Menschen an",
            "Ein vielfältiges Angebot an Geschäften, Restaurants und Dienstleistungen kann unterschiedliche Kundengruppen ansprechen und die lokale Wirtschaft stärken."),
        ("Warum werden Städte wie Rom in der Antike oft als Vorbild für spätere Stadtplanung genannt?", new[] { "Sie hatten organisierte Infrastruktur wie Straßen, Wasserversorgung und öffentliche Gebäude", "Antike Städte hatten überhaupt keine Planung, was bei genauerem Hinsehen nicht stimmt (was so in der Praxis nicht zutrifft)", "Es gab keinerlei öffentliche Gebäude" }, "Sie hatten organisierte Infrastruktur wie Straßen, Wasserversorgung und öffentliche Gebäude",
            "Die durchdachte Infrastruktur antiker Städte wie Rom - Straßen, Wasserleitungen, öffentliche Plätze - beeinflusst bis heute Ideen zur Stadtplanung.")
    };

    private static QuizQuestion StadtUndVielfalt(Random r)
    {
        var f = StadtListe[r.Next(StadtListe.Length)];
        return new QuizQuestion
        {
            Id = NewId(), Subject = Subject.Gewi, GradeLevel = GradeLevel.Klasse6,
            Topic = "Stadt und städtische Vielfalt", Type = QuestionType.MultipleChoice,
            Prompt = f.Frage, Options = f.Optionen, CorrectAnswers = new[] { f.Antwort }, Explanation = f.Erklaerung,
            HelpHint = "Das antike Rom zeigt früh, wie Großstädte organisiert wurden (Wasser, Straßen, Nahrung); moderne Großstädte wie Berlin haben ähnliche Chancen und Herausforderungen."
        };
    }

    private static readonly (string Frage, string[] Optionen, string Antwort, string Erklaerung)[] EuropaListe =
    {
        ("Was ist Europa geografisch gesehen genau genommen?", new[] { "Ein Kontinent bzw. Halbkontinent mit vielfältigen Klimazonen", "Ein einzelnes Land", "Eine Insel im Pazifik" }, "Ein Kontinent bzw. Halbkontinent mit vielfältigen Klimazonen",
            "Europa reicht von mediterranem Klima im Süden bis zu kühlem, gemäßigtem Klima im Norden und Osten."),
        ("Wie weit erstreckte sich das Römische Reich auf seinem Höhepunkt ungefähr?", new[] { "Rund um das gesamte Mittelmeer und weite Teile Europas", "Nur über die Stadt Rom selbst - eine verbreitete, aber falsche Annahme", "Nur über Nordeuropa" }, "Rund um das gesamte Mittelmeer und weite Teile Europas",
            "Das Römische Reich umfasste auf seinem Höhepunkt Gebiete rund um das gesamte Mittelmeer sowie große Teile West- und Südeuropas."),
        ("Was ist die Europäische Union (EU)?", new[] { "Ein politischer und wirtschaftlicher Zusammenschluss europäischer Staaten", "Ein einzelnes europäisches Land, was einer genaueren Pruefung nicht standhaelt", "Ein Sportverband" }, "Ein politischer und wirtschaftlicher Zusammenschluss europäischer Staaten",
            "Die EU ist ein Zusammenschluss europäischer Staaten, die politisch und wirtschaftlich eng zusammenarbeiten."),
        ("Welche gemeinsame Währung nutzen viele, aber nicht alle EU-Mitgliedsstaaten?", new[] { "Den Euro", "Den Dollar", "Das Pfund" }, "Den Euro",
            "Der Euro ist die gemeinsame Währung vieler, aber nicht aller EU-Mitgliedsstaaten (die sogenannte Eurozone)."),
        ("Was ermöglicht der freie Personenverkehr innerhalb großer Teile der EU (Schengen-Raum)?", new[] { "Reisen zwischen vielen Mitgliedstaaten meist ohne Passkontrollen an der Grenze", "Es gibt in der EU keinerlei Reisefreiheit", "Jeder Grenzübertritt braucht ein Visum" }, "Reisen zwischen vielen Mitgliedstaaten meist ohne Passkontrollen an der Grenze",
            "Im Schengen-Raum können Menschen zwischen vielen Mitgliedstaaten in der Regel ohne Passkontrollen an der Binnengrenze reisen."),
        ("Was war die Berliner Mauer?", new[] { "Eine Grenzbefestigung, die von 1961 bis 1989 Ost- und West-Berlin trennte", "Eine antike römische Stadtmauer", "Eine touristische Attraktion ohne historische Bedeutung" }, "Eine Grenzbefestigung, die von 1961 bis 1989 Ost- und West-Berlin trennte",
            "Die Berliner Mauer trennte von 1961 bis 1989 Ost- und West-Berlin und wurde zum Symbol der deutschen und europäischen Teilung."),
        ("Was geschah mit Deutschland nach dem Zweiten Weltkrieg zunächst?", new[] { "Es wurde in verschiedene Besatzungszonen geteilt, später in BRD und DDR", "Es blieb völlig unverändert, obwohl das auf den ersten Blick plausibel klingt, was die eigentliche Bedeutung des Begriffs verfehlt", "Es wurde sofort Teil der EU" }, "Es wurde in verschiedene Besatzungszonen geteilt, später in BRD und DDR",
            "Nach 1945 wurde Deutschland zunächst in Besatzungszonen geteilt, aus denen später die Bundesrepublik Deutschland und die DDR entstanden."),
        ("Was verbindet das Römische Reich und die heutige EU als historischen Vergleich?", new[] { "Beide verbanden viele verschiedene Regionen/Völker unter einem gemeinsamen politischen Rahmen", "Beide bestanden nur aus einer einzigen Stadt", "Beide existieren zur gleichen Zeit" }, "Beide verbanden viele verschiedene Regionen/Völker unter einem gemeinsamen politischen Rahmen",
            "Sowohl das Römische Reich als auch die EU verbanden sehr unterschiedliche Regionen unter einem gemeinsamen politischen und rechtlichen Rahmen - wenn auch auf ganz unterschiedliche Weise."),
        ("Wie viele Mitgliedstaaten hat die Europäische Union ungefähr (Stand nach dem Brexit)?", new[] { "27", "5", "100" }, "27",
            "Nach dem Austritt Großbritanniens (Brexit) 2020 hat die EU 27 Mitgliedstaaten."),
        ("Was bedeutet \"Binnenmarkt\" in der EU?", new[] { "Waren, Dienstleistungen, Kapital und Personen können sich weitgehend frei zwischen Mitgliedstaaten bewegen", "Jeder Staat handelt komplett isoliert für sich", "Es gibt in der EU keinerlei Handel" }, "Waren, Dienstleistungen, Kapital und Personen können sich weitgehend frei zwischen Mitgliedstaaten bewegen",
            "Der EU-Binnenmarkt erlaubt den weitgehend freien Austausch von Waren, Dienstleistungen, Kapital und Personen zwischen den Mitgliedstaaten."),
        ("Warum wird der 9. November 1989 auch europäisch als wichtiges Datum gesehen?", new[] { "Der Mauerfall gilt als Symbol für das Ende der Teilung Europas im Kalten Krieg", "An diesem Tag wurde die EU gegründet", "Es war der Beginn des Zweiten Weltkriegs" }, "Der Mauerfall gilt als Symbol für das Ende der Teilung Europas im Kalten Krieg",
            "Der Fall der Berliner Mauer wird oft als Symbol für das Ende der politischen Teilung Europas im Kalten Krieg gesehen."),
        ("Was ist eine Klimazone?", new[] { "Ein Gebiet mit ähnlichen, typischen Wetterbedingungen über das Jahr", "Ein anderes Wort für Staatsgrenze und deshalb hier nicht zutrifft, was so nicht korrekt ist", "Eine Art politischer Vertrag" }, "Ein Gebiet mit ähnlichen, typischen Wetterbedingungen über das Jahr",
            "Klimazonen fassen Regionen mit ähnlichen typischen Temperatur- und Niederschlagsverhältnissen zusammen, z.B. mediterran oder gemäßigt."),
        ("Welche Klimazone prägt weite Teile Südeuropas rund um das Mittelmeer?", new[] { "Das mediterrane Klima mit trockenen, warmen Sommern", "Ein tropisches Regenwaldklima - eine haeufige, aber unzutreffende Vorstellung", "Ein arktisches Eisklima" }, "Das mediterrane Klima mit trockenen, warmen Sommern",
            "Das mediterrane Klima am Mittelmeer ist geprägt von trockenen, warmen Sommern und milden, feuchteren Wintern."),
        ("Wie wird der Bundestag gemeinsam mit den Institutionen der EU beschrieben, wenn es um Gesetzgebung geht?", new[] { "Nationale Parlamente und EU-Institutionen wirken bei bestimmten Themen zusammen", "Nationale Parlamente haben mit der EU nichts zu tun", "Nur die EU darf über alles allein entscheiden" }, "Nationale Parlamente und EU-Institutionen wirken bei bestimmten Themen zusammen",
            "In vielen Politikbereichen arbeiten nationale Parlamente wie der Bundestag und EU-Institutionen wie das Europaparlament zusammen."),
        ("Was ist das Europäische Parlament?", new[] { "Die von EU-Bürgerinnen und -Bürgern direkt gewählte Vertretung auf EU-Ebene", "Ein Parlament nur für Deutschland, auch wenn das manche zunaechst vermuten wuerden", "Ein Gericht für Verkehrsdelikte" }, "Die von EU-Bürgerinnen und -Bürgern direkt gewählte Vertretung auf EU-Ebene",
            "Das Europäische Parlament wird direkt von den Bürgerinnen und Bürgern der EU-Mitgliedstaaten gewählt und wirkt an EU-Gesetzen mit."),
        ("Was symbolisiert der Begriff \"Europa - grenzenlos?\" im Vergleich zur deutschen Teilung während des Kalten Krieges?", new[] { "Den Kontrast zwischen früherer strikter Grenzziehung und heutiger europäischer Reisefreiheit", "Dass es in Europa niemals Grenzen gab", "Dass Grenzen heute strenger sind als je zuvor" }, "Den Kontrast zwischen früherer strikter Grenzziehung und heutiger europäischer Reisefreiheit",
            "Die einst streng bewachte innerdeutsche Grenze steht im starken Kontrast zur heutigen weitgehenden Reisefreiheit innerhalb der EU."),
        ("Was ist ein Vorteil der EU-Mitgliedschaft für den Handel zwischen den Ländern?", new[] { "Weniger Zölle und einfachere Regeln zwischen Mitgliedstaaten", "Handel zwischen EU-Ländern ist grundsätzlich verboten, was bei genauerem Hinsehen nicht stimmt", "Jedes Produkt braucht eine Sondererlaubnis pro Land" }, "Weniger Zölle und einfachere Regeln zwischen Mitgliedstaaten",
            "Durch den EU-Binnenmarkt entfallen viele Zölle und bürokratische Hürden beim Handel zwischen Mitgliedstaaten."),
        ("Was hatten das Römische Reich und moderne europäische Staaten gemeinsam in Bezug auf Straßen und Infrastruktur?", new[] { "Beide bauten überregionale Verkehrsnetze für Handel und Austausch aus", "Beide bauten überhaupt keine Straßen", "Straßenbau spielte in beiden keine Rolle (was so in der Praxis nicht zutrifft)" }, "Beide bauten überregionale Verkehrsnetze für Handel und Austausch aus",
            "Sowohl das antike Straßennetz Roms als auch moderne europäische Verkehrsnetze (Autobahnen, Schienen) fördern Handel und Austausch über Regionen hinweg."),
        ("Was ist ein Unterschied zwischen dem Römischen Reich und der heutigen EU?", new[] { "Das Römische Reich wurde militärisch erobert, die EU beruht auf freiwilligem Beitritt der Mitgliedstaaten", "Beide beruhten auf exakt denselben Prinzipien", "Beide bestehen aus genau denselben Ländern" }, "Das Römische Reich wurde militärisch erobert, die EU beruht auf freiwilligem Beitritt der Mitgliedstaaten",
            "Anders als das durch Eroberung entstandene Römische Reich basiert die EU auf dem freiwilligen politischen Zusammenschluss unabhängiger Staaten."),
        ("Warum ist die deutsche Wiedervereinigung 1990 auch ein europäisches Ereignis?", new[] { "Sie markierte das Ende der europäischen Teilung im Kalten Krieg und stärkte die europäische Zusammenarbeit", "Sie betraf ausschließlich innerdeutsche Angelegenheiten - eine verbreitete, aber falsche Annahme, was einer genaueren Pruefung nicht standhaelt", "Sie hatte keinerlei Auswirkungen auf Europa" }, "Sie markierte das Ende der europäischen Teilung im Kalten Krieg und stärkte die europäische Zusammenarbeit",
            "Die deutsche Wiedervereinigung 1990 wird oft als wichtiger Schritt im größeren Prozess der europäischen Annäherung nach dem Kalten Krieg gesehen.")
    };

    private static QuizQuestion EuropaGrenzenlos(Random r)
    {
        var f = EuropaListe[r.Next(EuropaListe.Length)];
        return new QuizQuestion
        {
            Id = NewId(), Subject = Subject.Gewi, GradeLevel = GradeLevel.Klasse6,
            Topic = "Europa – grenzenlos?", Type = QuestionType.MultipleChoice,
            Prompt = f.Frage, Options = f.Optionen, CorrectAnswers = new[] { f.Antwort }, Explanation = f.Erklaerung,
            HelpHint = "Vom Römischen Reich über die geteilte Berliner Mauer bis zur heutigen EU: Europa hat seine Grenzen im Lauf der Geschichte immer wieder verändert."
        };
    }

    private static readonly (string Frage, string[] Optionen, string Antwort, string Erklaerung)[] TourismusListe =
    {
        ("Welche Art von Reisen betrieben Menschen im Mittelalter vor allem aus wirtschaftlichen Gründen?", new[] { "Handelsreisen, um Waren zu kaufen und zu verkaufen", "Reisen nur zum Vergnügen", "Reisen mit dem Flugzeug" }, "Handelsreisen, um Waren zu kaufen und zu verkaufen",
            "Handelsreisende zogen im Mittelalter oft weite Strecken, um Waren wie Gewürze, Stoffe oder Metalle zu handeln."),
        ("Was war das Ziel vieler Entdeckungsreisen ab dem 15. Jahrhundert, wie der von Kolumbus?", new[] { "Neue Handelswege und unbekannte Gebiete zu finden", "Nur zum Vergnügen zu reisen", "Ausschließlich wissenschaftliche Neugier ohne wirtschaftliches Interesse" }, "Neue Handelswege und unbekannte Gebiete zu finden",
            "Entdeckungsreisen wie die von Kolumbus suchten meist neue Handelswege (z.B. nach Indien) und brachten dabei bislang unbekannte Gebiete in Kontakt mit Europa."),
        ("Was unterscheidet eine Forschungsreise von einer reinen Handelsreise?", new[] { "Bei Forschungsreisen steht die wissenschaftliche Erkundung im Vordergrund", "Beide sind exakt dasselbe", "Forschungsreisen dienten nur dem persönlichen Vergnügen" }, "Bei Forschungsreisen steht die wissenschaftliche Erkundung im Vordergrund",
            "Forschungsreisen wie die von Alexander von Humboldt zielten vor allem auf wissenschaftliche Erkenntnisse über Natur, Länder und Völker."),
        ("Was ist Massentourismus?", new[] { "Sehr viele Menschen reisen gleichzeitig an dieselben, oft bekannten Reiseziele", "Reisen, die nur einzelne Personen unternehmen", "Ein anderes Wort für Handelsreisen im Mittelalter" }, "Sehr viele Menschen reisen gleichzeitig an dieselben, oft bekannten Reiseziele",
            "Beim Massentourismus konzentrieren sich sehr viele Reisende auf wenige, populäre Ziele, was Umwelt und Infrastruktur dort stark belasten kann."),
        ("Was versteht man unter \"sanftem Tourismus\"?", new[] { "Reisen, die Umwelt, Kultur und lokale Bevölkerung möglichst wenig belasten", "Reisen mit möglichst großen Kreuzfahrtschiffen", "Reisen, bei denen man niemals die Region verlässt" }, "Reisen, die Umwelt, Kultur und lokale Bevölkerung möglichst wenig belasten",
            "Sanfter Tourismus versucht, Umwelt und lokale Kultur zu schonen, z.B. durch kleinere Gruppen und respektvollen Umgang mit der Region."),
        ("Was ist ein möglicher Nachteil von Massentourismus für die Umwelt?", new[] { "Überlastung von Natur, mehr Müll und höherer Ressourcenverbrauch", "Massentourismus hat keinerlei Umweltwirkung", "Die Umwelt profitiert automatisch von mehr Touristen, obwohl das auf den ersten Blick plausibel klingt" }, "Überlastung von Natur, mehr Müll und höherer Ressourcenverbrauch",
            "Sehr viele Touristinnen und Touristen an einem Ort können zu Umweltbelastung, mehr Abfall und höherem Ressourcenverbrauch führen."),
        ("Was ist ein Vorteil von Tourismus für eine Region?", new[] { "Er kann Arbeitsplätze und Einkommen für die lokale Bevölkerung schaffen", "Tourismus bringt einer Region grundsätzlich nur Nachteile", "Tourismus verhindert jede wirtschaftliche Entwicklung" }, "Er kann Arbeitsplätze und Einkommen für die lokale Bevölkerung schaffen",
            "Tourismus kann Arbeitsplätze in Hotels, Gastronomie und Handel schaffen und der lokalen Bevölkerung Einkommen bringen."),
        ("Was zeichnete die berühmten Seidenstraßen-Handelsrouten historisch aus?", new[] { "Sie verbanden Asien und Europa für den Warenaustausch über weite Strecken", "Sie verliefen ausschließlich innerhalb einer einzigen Stadt, was die eigentliche Bedeutung des Begriffs verfehlt", "Sie wurden nur für Kriege genutzt" }, "Sie verbanden Asien und Europa für den Warenaustausch über weite Strecken",
            "Die Seidenstraßen waren jahrhundertealte Handelsrouten, über die Waren wie Seide und Gewürze zwischen Asien und Europa transportiert wurden."),
        ("Wie unterscheiden sich die geografischen Landschaften der deutschen Bundesländer beispielsweise?", new[] { "Von Küsten im Norden über Mittelgebirge bis zu den Alpen im Süden", "Alle Bundesländer haben exakt dieselbe Landschaft", "Deutschland hat keinerlei geografische Vielfalt" }, "Von Küsten im Norden über Mittelgebirge bis zu den Alpen im Süden",
            "Deutschland reicht von den Küsten Norddeutschlands über Mittelgebirge bis zu den Alpen in Bayern - eine große landschaftliche Vielfalt."),
        ("Welches Bundesland grenzt im Süden Deutschlands an die Alpen?", new[] { "Bayern", "Schleswig-Holstein", "Bremen" }, "Bayern",
            "Bayern liegt im Süden Deutschlands und grenzt mit den Alpen an Österreich."),
        ("Was ist ein Beispiel für ein modernes Reisemittel, das Entdeckungsreisen früherer Jahrhunderte stark verändert hat?", new[] { "Das Flugzeug", "Das Segelboot allein", "Der Handkarren" }, "Das Flugzeug",
            "Das Flugzeug ermöglicht es heute, in wenigen Stunden Distanzen zurückzulegen, für die frühere Entdecker Monate benötigten."),
        ("Warum reisten viele Forscherinnen und Forscher in der Vergangenheit oft mit großen wissenschaftlichen Expeditionen?", new[] { "Um Länder, Pflanzen, Tiere und Kulturen systematisch zu erforschen und zu dokumentieren", "Nur, um möglichst viel Gepäck mitzunehmen", "Weil es damals verboten war, allein zu reisen" }, "Um Länder, Pflanzen, Tiere und Kulturen systematisch zu erforschen und zu dokumentieren",
            "Große Expeditionen sammelten systematisch Wissen über unbekannte Regionen, Pflanzen, Tiere und die dort lebenden Menschen."),
        ("Was kann sanfter Tourismus für die lokale Bevölkerung bedeuten?", new[] { "Wirtschaftlicher Nutzen bei gleichzeitigem Schutz von Natur und Kultur", "Verlust jeglichen wirtschaftlichen Nutzens", "Automatische Zerstörung der lokalen Kultur" }, "Wirtschaftlicher Nutzen bei gleichzeitigem Schutz von Natur und Kultur",
            "Sanfter Tourismus versucht, wirtschaftliche Vorteile für die Region mit dem Schutz von Umwelt und lokaler Kultur zu verbinden."),
        ("Warum entstanden im Mittelalter viele Handelsstädte an wichtigen Fluss- oder Küstenlagen?", new[] { "Wasserwege waren wichtige, schnelle Transportrouten für Handelswaren", "Handelsstädte mieden Wasser bewusst", "Flüsse hatten keinerlei Bedeutung für Handel" }, "Wasserwege waren wichtige, schnelle Transportrouten für Handelswaren",
            "Flüsse und Küsten ermöglichten den effizienten Transport großer Warenmengen per Schiff, weshalb sich dort viele Handelsstädte entwickelten."),
        ("Was zeichnet die Nord- und Ostseeküste Deutschlands geografisch aus?", new[] { "Flaches Küstenland mit Häfen, Watt und Inseln", "Hohe Gebirgsketten direkt am Meer und deshalb hier nicht zutrifft", "Wüstenlandschaft" }, "Flaches Küstenland mit Häfen, Watt und Inseln",
            "Die deutsche Nord- und Ostseeküste ist geprägt von flachem Küstenland, Wattenmeer, Häfen und vorgelagerten Inseln."),
        ("Was können Reisende tun, um die Umweltbelastung durch ihre Reisen zu verringern?", new[] { "Umweltfreundlichere Verkehrsmittel wählen und Region/Kultur respektvoll behandeln", "Möglichst viel Müll am Reiseziel hinterlassen", "Ausschließlich mit dem Flugzeug auf kurze Strecken reisen" }, "Umweltfreundlichere Verkehrsmittel wählen und Region/Kultur respektvoll behandeln",
            "Die Wahl umweltfreundlicherer Verkehrsmittel und ein respektvoller Umgang mit Natur und Kultur vor Ort können die negativen Folgen von Reisen verringern."),
        ("Was war ein wichtiger Antrieb für Entdeckungsreisen der europäischen Seefahrer im 15./16. Jahrhundert?", new[] { "Die Suche nach neuen Seewegen für den Gewürzhandel", "Reines Interesse an Strandurlaub, was so nicht korrekt ist", "Die Flucht vor Massentourismus" }, "Die Suche nach neuen Seewegen für den Gewürzhandel",
            "Viele Entdeckungsreisen wie die portugiesischer und spanischer Seefahrer zielten auf neue Seewege für den lukrativen Gewürzhandel mit Asien."),
        ("Wie hat sich die durchschnittliche Reisedauer zwischen Kontinenten seit dem Mittelalter verändert?", new[] { "Sie hat sich durch moderne Verkehrsmittel drastisch verkürzt", "Sie ist exakt gleich geblieben", "Sie hat sich deutlich verlängert - eine haeufige, aber unzutreffende Vorstellung" }, "Sie hat sich durch moderne Verkehrsmittel drastisch verkürzt",
            "Was früher Wochen oder Monate dauerte (Schiffsreisen), ist mit modernen Flugzeugen oft in wenigen Stunden möglich."),
        ("Was ist ein Grund dafür, dass Reisen heute für viel mehr Menschen möglich ist als früher?", new[] { "Günstigere und schnellere Verkehrsmittel sowie höherer allgemeiner Wohlstand", "Reisen ist heute komplizierter als früher", "Es gibt heute weniger Verkehrsmittel als früher" }, "Günstigere und schnellere Verkehrsmittel sowie höherer allgemeiner Wohlstand",
            "Günstigere Flugpreise, bessere Infrastruktur und gestiegener Wohlstand machen Reisen heute für deutlich mehr Menschen zugänglich als in früheren Jahrhunderten."),
        ("Welche Rolle spielten venezianische Kaufleute wie Marco Polo für den Handel zwischen Europa und Asien im Mittelalter?", new[] { "Sie erkundeten und beschrieben neue Handelsrouten und brachten Wissen über ferne Länder nach Europa", "Sie reisten ausschließlich innerhalb Venedigs, auch wenn das manche zunaechst vermuten wuerden, was bei genauerem Hinsehen nicht stimmt", "Sie hatten mit Handel nichts zu tun" }, "Sie erkundeten und beschrieben neue Handelsrouten und brachten Wissen über ferne Länder nach Europa",
            "Reisende wie Marco Polo berichteten von fernen Ländern wie China und förderten so den Handel und das Wissen über Asien in Europa.")
    };

    private static QuizQuestion TourismusUndMobilitaet(Random r)
    {
        var f = TourismusListe[r.Next(TourismusListe.Length)];
        return new QuizQuestion
        {
            Id = NewId(), Subject = Subject.Gewi, GradeLevel = GradeLevel.Klasse6,
            Topic = "Tourismus und Mobilität – schneller, weiter, klüger?", Type = QuestionType.MultipleChoice,
            Prompt = f.Frage, Options = f.Optionen, CorrectAnswers = new[] { f.Antwort }, Explanation = f.Erklaerung,
            HelpHint = "Von Handels- und Entdeckungsreisen bis zum modernen Massen- oder sanften Tourismus: Reisen hat sich stark verändert, bringt aber immer Chancen und Umweltfolgen mit sich."
        };
    }

    private static readonly (string Frage, string[] Optionen, string Antwort, string Erklaerung)[] DemokratieMitList =
    {
        ("In welcher antiken Stadt entstand eine frühe Form der Demokratie, auf die man sich bis heute bezieht?", new[] { "Athen", "Rom", "Sparta" }, "Athen",
            "Im antiken Athen entwickelte sich mit der Volksversammlung eine der ersten bekannten Formen direkter Demokratie."),
        ("Wer durfte in der athenischen Demokratie in der Volksversammlung mitbestimmen?", new[] { "Nur freie männliche Bürger, nicht Frauen, Sklaven oder Fremde", "Alle Einwohnerinnen und Einwohner gleichermaßen", "Nur Kinder" }, "Nur freie männliche Bürger, nicht Frauen, Sklaven oder Fremde",
            "Anders als moderne Demokratien war die athenische Demokratie stark eingeschränkt: Nur freie männliche Bürger durften mitbestimmen."),
        ("Was ist ein zentraler Unterschied zwischen der athenischen und der heutigen deutschen Demokratie?", new[] { "Heute dürfen alle erwachsenen Staatsbürgerinnen und Staatsbürger unabhängig von Geschlecht wählen", "In Athen durften auch Kinder abstimmen (was so in der Praxis nicht zutrifft) - eine verbreitete, aber falsche Annahme", "Es gibt keinerlei Unterschiede" }, "Heute dürfen alle erwachsenen Staatsbürgerinnen und Staatsbürger unabhängig von Geschlecht wählen",
            "Im Gegensatz zur athenischen Demokratie gilt das Wahlrecht heute unabhängig vom Geschlecht für alle erwachsenen Staatsbürgerinnen und Staatsbürger."),
        ("Was ist der Unterschied zwischen direkter und repräsentativer (indirekter) Demokratie?", new[] { "Bei direkter Demokratie stimmt das Volk selbst über Sachfragen ab, bei repräsentativer wählt es Vertreter", "Beide Formen bedeuten exakt dasselbe", "Repräsentative Demokratie kennt keine Wahlen" }, "Bei direkter Demokratie stimmt das Volk selbst über Sachfragen ab, bei repräsentativer wählt es Vertreter",
            "In der direkten Demokratie (wie im antiken Athen) stimmt das Volk direkt ab, in der repräsentativen Demokratie (wie in Deutschland) wählt es Abgeordnete, die entscheiden."),
        ("Was ist eine politische Partei?", new[] { "Eine Organisation von Menschen mit ähnlichen politischen Zielen, die sich zur Wahl stellt", "Ein anderes Wort für Parlament, was einer genaueren Pruefung nicht standhaelt, obwohl das auf den ersten Blick plausibel klingt", "Eine sportliche Mannschaft" }, "Eine Organisation von Menschen mit ähnlichen politischen Zielen, die sich zur Wahl stellt",
            "Parteien bündeln Menschen mit ähnlichen politischen Zielen und Vorstellungen und treten gemeinsam zu Wahlen an."),
        ("Was ist ein Parlament?", new[] { "Die gewählte Volksvertretung, die Gesetze berät und beschließt", "Ein Gerichtsgebäude", "Ein Ort, an dem nur der Bundeskanzler entscheidet" }, "Die gewählte Volksvertretung, die Gesetze berät und beschließt",
            "Ein Parlament (z.B. der Bundestag) besteht aus gewählten Abgeordneten, die Gesetze beraten, ändern und beschließen."),
        ("Was ist ein Klassenrat in der Schule ein Beispiel für?", new[] { "Mitbestimmung im Alltag durch gemeinsame Beratung und Entscheidung", "Eine Form der Diktatur", "Eine reine Vorlesung ohne Mitsprache" }, "Mitbestimmung im Alltag durch gemeinsame Beratung und Entscheidung",
            "Im Klassenrat können Schülerinnen und Schüler gemeinsam Themen besprechen und mitentscheiden - ein alltagsnahes Beispiel für Mitbestimmung."),
        ("Was bedeutet \"Wahlrecht\" in einer modernen Demokratie?", new[] { "Das Recht, bei Wahlen die eigene Stimme abzugeben", "Die Pflicht, für eine bestimmte Partei zu stimmen", "Ein Vorrecht nur für bestimmte Berufsgruppen" }, "Das Recht, bei Wahlen die eigene Stimme abzugeben",
            "Das Wahlrecht gibt Bürgerinnen und Bürgern das Recht, bei politischen Wahlen ihre Stimme abzugeben."),
        ("Was ist ein Beispiel für einen Interessenkonflikt im lokalen Umfeld, wie er in einer Demokratie gelöst werden muss?", new[] { "Streit um die Nutzung einer Fläche, z.B. für einen Spielplatz oder einen Parkplatz", "Zwei Personen mögen dasselbe Buch", "Ein Streit ohne jede Lösungsmöglichkeit, was die eigentliche Bedeutung des Begriffs verfehlt" }, "Streit um die Nutzung einer Fläche, z.B. für einen Spielplatz oder einen Parkplatz",
            "Interessenkonflikte, etwa um die Nutzung einer Fläche für Spielplatz oder Parkplatz, müssen in einer Demokratie durch Diskussion und Abstimmung gelöst werden."),
        ("Was bedeutet \"Mehrheitsprinzip\" bei demokratischen Abstimmungen?", new[] { "Die Meinung, die die meisten Stimmen erhält, setzt sich in der Regel durch", "Nur eine einzige Person entscheidet immer allein", "Abstimmungsergebnisse werden zufällig ausgelost" }, "Die Meinung, die die meisten Stimmen erhält, setzt sich in der Regel durch",
            "Nach dem Mehrheitsprinzip setzt sich bei einer Abstimmung in der Regel die Position durch, die die meisten Stimmen erhalten hat."),
        ("Warum ist der Minderheitenschutz in einer Demokratie trotz Mehrheitsprinzip wichtig?", new[] { "Auch die Rechte und Interessen von Menschen in der Minderheit müssen geachtet werden", "Minderheiten haben in einer Demokratie keine Rechte", "Minderheitenschutz widerspricht grundsätzlich der Demokratie" }, "Auch die Rechte und Interessen von Menschen in der Minderheit müssen geachtet werden",
            "Damit die Mehrheit nicht über die Rechte Einzelner hinweggeht, schützt eine funktionierende Demokratie auch die Interessen von Minderheiten."),
        ("Was unterscheidet eine Demokratie grundsätzlich von einer Diktatur?", new[] { "In einer Demokratie kann die Bevölkerung die Regierung wählen und kontrollieren", "In einer Diktatur haben alle Bürger gleich viel Mitspracherecht und deshalb hier nicht zutrifft", "Beide Regierungsformen sind identisch" }, "In einer Demokratie kann die Bevölkerung die Regierung wählen und kontrollieren",
            "In einer Demokratie wählt und kontrolliert die Bevölkerung die Regierung, während in einer Diktatur die Macht meist bei Einzelnen oder wenigen liegt, ohne echte Kontrolle durch das Volk."),
        ("Wie können sich Jugendliche schon vor dem Erreichen des Wahlalters an Mitbestimmung beteiligen?", new[] { "Z.B. über Klassenrat, Schülervertretung oder Jugendparlamente", "Jugendliche haben grundsätzlich keinerlei Möglichkeit zur Mitbestimmung", "Nur durch das Bundestagswahlrecht" }, "Z.B. über Klassenrat, Schülervertretung oder Jugendparlamente",
            "Auch vor dem Erreichen des Wahlalters können sich Jugendliche z.B. über Klassenrat, Schülervertretung oder Jugendparlamente an Entscheidungen beteiligen."),
        ("Was war ein wesentlicher Unterschied zwischen der athenischen Demokratie und modernen Demokratien in Bezug auf die Größe der Bevölkerung?", new[] { "Athen war eine kleine Stadt, moderne Demokratien umfassen oft Millionen Menschen", "Athen hatte mehr Einwohner als heutige Staaten", "Beide hatten exakt dieselbe Bevölkerungsgröße" }, "Athen war eine kleine Stadt, moderne Demokratien umfassen oft Millionen Menschen",
            "Da moderne Staaten viel größer als das antike Athen sind, ist direkte Demokratie über alle Themen praktisch kaum umsetzbar - deshalb wählt man Vertreter."),
        ("Was ist eine Volksabstimmung (Referendum)?", new[] { "Eine direkte Abstimmung der Bevölkerung über eine bestimmte Sachfrage", "Eine Wahl von Abgeordneten ins Parlament", "Ein Gerichtsverfahren" }, "Eine direkte Abstimmung der Bevölkerung über eine bestimmte Sachfrage",
            "Bei einer Volksabstimmung stimmt die Bevölkerung direkt über eine konkrete politische Sachfrage ab, statt nur Vertreter zu wählen."),
        ("Warum ist Redefreiheit für eine funktionierende Demokratie wichtig?", new[] { "Bürgerinnen und Bürger können ihre Meinung offen äußern und mitdiskutieren", "Redefreiheit ist für Demokratien unwichtig", "In einer Demokratie darf nur die Regierung sprechen" }, "Bürgerinnen und Bürger können ihre Meinung offen äußern und mitdiskutieren",
            "Ohne Redefreiheit könnten Bürgerinnen und Bürger nicht offen über politische Themen diskutieren und mitbestimmen."),
        ("Was zeigt das Beispiel des antiken Athen über die Entwicklung demokratischer Ideen?", new[] { "Demokratische Grundideen wie Mitbestimmung haben eine sehr lange Geschichte", "Demokratie ist eine ganz neue Erfindung des 21. Jahrhunderts", "Demokratische Ideen entstanden erst nach dem Zweiten Weltkrieg" }, "Demokratische Grundideen wie Mitbestimmung haben eine sehr lange Geschichte",
            "Schon vor über 2000 Jahren entwickelten die Menschen im antiken Athen grundlegende demokratische Ideen wie Mitbestimmung durch die Bürgerschaft."),
        ("Was ist eine Bürgerinitiative?", new[] { "Ein freiwilliger Zusammenschluss von Bürgerinnen und Bürgern, um sich für ein bestimmtes Anliegen einzusetzen", "Eine staatliche Behörde", "Ein anderes Wort für Parlament" }, "Ein freiwilliger Zusammenschluss von Bürgerinnen und Bürgern, um sich für ein bestimmtes Anliegen einzusetzen",
            "In einer Bürgerinitiative engagieren sich Menschen freiwillig gemeinsam für ein lokales oder gesellschaftliches Anliegen."),
        ("Warum gilt Gleichberechtigung als wichtiges Ziel moderner Demokratien im Unterschied zur athenischen Demokratie?", new[] { "Heute sollen alle Menschen unabhängig von Geschlecht oder Herkunft gleiche politische Rechte haben", "Gleichberechtigung war in Athen bereits vollständig verwirklicht, was so nicht korrekt ist - eine haeufige, aber unzutreffende Vorstellung", "Gleichberechtigung spielt in Demokratien keine Rolle" }, "Heute sollen alle Menschen unabhängig von Geschlecht oder Herkunft gleiche politische Rechte haben",
            "Anders als im antiken Athen, wo nur ein Teil der Bevölkerung Rechte hatte, streben moderne Demokratien nach gleichen politischen Rechten für alle Menschen."),
        ("Was bedeutet das Losverfahren, das im antiken Athen für manche öffentliche Ämter genutzt wurde?", new[] { "Bestimmte Ämter wurden per Zufall unter den Bürgern verlost statt gewählt", "Alle Ämter wurden vererbt", "Ämter wurden ausschließlich durch Reichtum vergeben, auch wenn das manche zunaechst vermuten wuerden" }, "Bestimmte Ämter wurden per Zufall unter den Bürgern verlost statt gewählt",
            "Im antiken Athen wurden manche öffentliche Ämter durch Losverfahren unter den stimmberechtigten Bürgern vergeben, um Machtmissbrauch vorzubeugen.")
    };

    private static QuizQuestion DemokratieUndMitbestimmung(Random r)
    {
        var f = DemokratieMitList[r.Next(DemokratieMitList.Length)];
        return new QuizQuestion
        {
            Id = NewId(), Subject = Subject.Gewi, GradeLevel = GradeLevel.Klasse6,
            Topic = "Demokratie und Mitbestimmung", Type = QuestionType.MultipleChoice,
            Prompt = f.Frage, Options = f.Optionen, CorrectAnswers = new[] { f.Antwort }, Explanation = f.Erklaerung,
            HelpHint = "Die Demokratie entstand im antiken Athen (nur für freie männliche Bürger); heute gilt das Wahlrecht für alle - im Alltag übt man Mitbestimmung z.B. im Klassenrat."
        };
    }

    private static readonly (string Frage, string[] Optionen, string Antwort, string Erklaerung)[] GrundgesetzListe =
    {
        ("Was ist das Grundgesetz?", new[] { "Die Verfassung der Bundesrepublik Deutschland", "Ein einfaches Gesetz zum Straßenverkehr, was bei genauerem Hinsehen nicht stimmt", "Eine Regel nur für Berlin" },
            "Die Verfassung der Bundesrepublik Deutschland", "Das Grundgesetz von 1949 ist die Verfassung Deutschlands und steht über allen anderen Gesetzen."),
        ("Welcher Artikel des Grundgesetzes garantiert die Würde des Menschen?", new[] { "Artikel 1", "Artikel 20", "Artikel 100" }, "Artikel 1",
            "Artikel 1 des Grundgesetzes lautet: \"Die Würde des Menschen ist unantastbar.\""),
        ("Seit wann gilt das Grundgesetz für ganz Deutschland (nach der Wiedervereinigung)?", new[] { "Seit 1990", "Seit 1949", "Seit 1945" }, "Seit 1990",
            "Das Grundgesetz galt ab 1949 zunächst nur für die Bundesrepublik (Westdeutschland) und gilt seit der Wiedervereinigung 1990 für ganz Deutschland."),
        ("Was bedeutet \"Gewaltenteilung\" im Grundgesetz?", new[] { "Staatsmacht ist auf Gesetzgebung, Regierung und Gerichte aufgeteilt", "Nur eine Person entscheidet über alles (was so in der Praxis nicht zutrifft)", "Es gibt gar keine Regeln"}, "Staatsmacht ist auf Gesetzgebung, Regierung und Gerichte aufgeteilt",
            "Die Gewaltenteilung verteilt die Macht auf Legislative (Parlament), Exekutive (Regierung) und Judikative (Gerichte), damit keine zu mächtig wird."),
        ("Was sind laut Grundgesetz die tragenden Prinzipien des deutschen Staates?", new[] { "Demokratie, Rechtsstaat, Sozialstaat, Bundesstaat", "Ein Kaiser entscheidet allein", "Es gibt gar keine festen Prinzipien - eine verbreitete, aber falsche Annahme" }, "Demokratie, Rechtsstaat, Sozialstaat, Bundesstaat",
            "Artikel 20 GG legt fest, dass Deutschland ein demokratischer, sozialer Rechtsstaat und Bundesstaat ist."),
        ("In welchem Jahr wurde das Grundgesetz verkündet?", new[] { "1949", "1918", "1990" }, "1949",
            "Das Grundgesetz wurde am 23. Mai 1949 verkündet und trat als Verfassung der Bundesrepublik Deutschland in Kraft."),
        ("Was garantiert Artikel 3 des Grundgesetzes?", new[] { "Die Gleichheit aller Menschen vor dem Gesetz", "Das Recht auf ein eigenes Auto, was einer genaueren Pruefung nicht standhaelt", "Die Steuerpflicht" }, "Die Gleichheit aller Menschen vor dem Gesetz",
            "Artikel 3 des Grundgesetzes garantiert die Gleichheit aller Menschen vor dem Gesetz und verbietet Diskriminierung."),
        ("Was schützt Artikel 5 des Grundgesetzes besonders?", new[] { "Meinungsfreiheit und Pressefreiheit", "Nur das Eigentum", "Nur den Straßenverkehr, obwohl das auf den ersten Blick plausibel klingt" }, "Meinungsfreiheit und Pressefreiheit",
            "Artikel 5 des Grundgesetzes schützt die Meinungsfreiheit, die Pressefreiheit und die Freiheit von Kunst und Wissenschaft."),
        ("Was ist eine \"Ewigkeitsklausel\" im Grundgesetz (Art. 79 Abs. 3)?", new[] { "Bestimmte Grundprinzipien wie die Menschenwürde dürfen nie geändert werden", "Das Grundgesetz gilt nur für 100 Jahre", "Alle Artikel können beliebig geändert werden, was die eigentliche Bedeutung des Begriffs verfehlt" }, "Bestimmte Grundprinzipien wie die Menschenwürde dürfen nie geändert werden",
            "Die Ewigkeitsklausel schützt zentrale Prinzipien wie die Menschenwürde und die Demokratie davor, jemals per Gesetzesänderung abgeschafft zu werden."),
        ("Wie wird der Bundespräsident laut Grundgesetz gewählt?", new[] { "Von der Bundesversammlung, nicht direkt vom Volk", "Direkt vom Volk in einer Volksabstimmung", "Er wird vererbt" }, "Von der Bundesversammlung, nicht direkt vom Volk",
            "Der Bundespräsident wird von der eigens dafür einberufenen Bundesversammlung gewählt, nicht direkt vom Volk."),
        ("Wer wählt den Bundeskanzler bzw. die Bundeskanzlerin laut Grundgesetz?", new[] { "Der Bundestag", "Das Volk direkt", "Der Bundespräsident allein" }, "Der Bundestag",
            "Der Bundestag wählt auf Vorschlag des Bundespräsidenten die Bundeskanzlerin oder den Bundeskanzler."),
        ("Was bedeutet \"Föderalismus\" im Grundgesetz?", new[] { "Die Staatsgewalt ist zwischen Bund und Bundesländern aufgeteilt", "Nur der Bund darf entscheiden", "Nur die Bundesländer dürfen entscheiden" }, "Die Staatsgewalt ist zwischen Bund und Bundesländern aufgeteilt",
            "Föderalismus bedeutet, dass staatliche Aufgaben und Macht zwischen dem Bund und den 16 Bundesländern aufgeteilt sind."),
        ("Welches Organ kontrolliert laut Grundgesetz, ob Gesetze verfassungsgemäß sind?", new[] { "Das Bundesverfassungsgericht", "Der Bundespräsident allein", "Die Polizei" }, "Das Bundesverfassungsgericht",
            "Das Bundesverfassungsgericht in Karlsruhe prüft, ob Gesetze mit dem Grundgesetz vereinbar sind."),
        ("Was garantiert Artikel 4 des Grundgesetzes?", new[] { "Die Glaubens- und Gewissensfreiheit", "Das Wahlrecht ab 14 Jahren", "Die Meinungsfreiheit im Straßenverkehr" }, "Die Glaubens- und Gewissensfreiheit",
            "Artikel 4 des Grundgesetzes garantiert die Freiheit des Glaubens, des Gewissens und der religiösen Bekenntnisse."),
        ("Was bedeutet das im Grundgesetz verankerte \"Rechtsstaatsprinzip\"?", new[] { "Staatliches Handeln muss sich an Recht und Gesetz halten", "Der Staat darf tun, was er will", "Gesetze gelten nur für Bürgerinnen und Bürger, nicht für den Staat" }, "Staatliches Handeln muss sich an Recht und Gesetz halten",
            "Das Rechtsstaatsprinzip verpflichtet auch den Staat selbst, sich an geltendes Recht und Gesetz zu halten."),
        ("Wie oft finden Bundestagswahlen laut Grundgesetz normalerweise statt?", new[] { "Alle 4 Jahre", "Jedes Jahr", "Alle 10 Jahre" }, "Alle 4 Jahre",
            "Der Bundestag wird regulär alle vier Jahre neu gewählt."),
        ("Was regelt Artikel 20a des Grundgesetzes seit 1994?", new[] { "Den Schutz der natürlichen Lebensgrundlagen (Umweltschutz)", "Das Recht auf ein Auto und deshalb hier nicht zutrifft, was so nicht korrekt ist", "Die Steuerhöhe" }, "Den Schutz der natürlichen Lebensgrundlagen (Umweltschutz)",
            "Seit 1994 verpflichtet Artikel 20a den Staat, die natürlichen Lebensgrundlagen auch für künftige Generationen zu schützen."),
        ("Was ist der Unterschied zwischen Grundgesetz und einfachem Gesetz?", new[] { "Das Grundgesetz steht als Verfassung über allen einfachen Gesetzen", "Beide stehen auf derselben Stufe", "Einfache Gesetze stehen über dem Grundgesetz - eine haeufige, aber unzutreffende Vorstellung" }, "Das Grundgesetz steht als Verfassung über allen einfachen Gesetzen",
            "Als Verfassung steht das Grundgesetz über allen einfachen Gesetzen - diese dürfen ihm nicht widersprechen."),
        ("Wer darf laut Grundgesetz in Deutschland grundsätzlich wählen (Bundestagswahl)?", new[] { "Deutsche Staatsbürgerinnen und Staatsbürger ab 18 Jahren", "Nur Männer über 21", "Nur Menschen mit Universitätsabschluss" }, "Deutsche Staatsbürgerinnen und Staatsbürger ab 18 Jahren",
            "Bei der Bundestagswahl wahlberechtigt sind grundsätzlich alle deutschen Staatsbürgerinnen und Staatsbürger ab 18 Jahren."),
        ("Wie heißt das aus 16 Bundesländern bestehende Parlament, das die Länder auf Bundesebene vertritt?", new[] { "Der Bundesrat", "Der Bundestag", "Das Bundesverfassungsgericht" }, "Der Bundesrat",
            "Der Bundesrat vertritt die 16 Bundesländer auf Bundesebene und wirkt an der Bundesgesetzgebung mit.")
    };

    private static QuizQuestion Grundgesetz(Random r)
    {
        var f = GrundgesetzListe[r.Next(GrundgesetzListe.Length)];
        return new QuizQuestion
        {
            Id = NewId(), Subject = Subject.Gewi, GradeLevel = GradeLevel.Klasse9,
            Topic = "Grundgesetz", Type = QuestionType.MultipleChoice,
            Prompt = f.Frage, Options = f.Optionen, CorrectAnswers = new[] { f.Antwort }, Explanation = f.Erklaerung,
            HelpHint = "Das Grundgesetz von 1949 ist die Verfassung Deutschlands und steht über allen anderen Gesetzen; Artikel 1 schützt die Menschenwürde."
        };
    }

    private static readonly (string Frage, string[] Optionen, string Antwort, string Erklaerung)[] WirtschaftListe =
    {
        ("Was beschreibt der einfache Wirtschaftskreislauf zwischen Haushalten und Unternehmen?", new[] { "Haushalte bieten Arbeitskraft, Unternehmen zahlen Lohn dafür", "Nur der Staat verteilt Geld", "Unternehmen bekommen nichts von Haushalten, auch wenn das manche zunaechst vermuten wuerden" },
            "Haushalte bieten Arbeitskraft, Unternehmen zahlen Lohn dafür",
            "Im einfachen Wirtschaftskreislauf tauschen private Haushalte und Unternehmen Arbeitskraft/Güter gegen Geld (Lohn/Erlöse)."),
        ("Was passiert bei Inflation?", new[] { "Die Preise steigen allgemein, Geld verliert an Wert", "Die Preise sinken dauerhaft", "Nichts ändert sich" },
            "Die Preise steigen allgemein, Geld verliert an Wert", "Bei Inflation steigt das allgemeine Preisniveau, wodurch man sich für denselben Geldbetrag weniger leisten kann."),
        ("Welche Rolle spielt der Staat zusätzlich im erweiterten Wirtschaftskreislauf?", new[] { "Er erhebt Steuern und zahlt z.B. für Schulen und Straßen", "Er hat gar keine Rolle", "Er verbietet jeden Handel, was bei genauerem Hinsehen nicht stimmt (was so in der Praxis nicht zutrifft)" },
            "Er erhebt Steuern und zahlt z.B. für Schulen und Straßen", "Der Staat nimmt Steuern von Haushalten/Unternehmen ein und finanziert damit öffentliche Aufgaben wie Schulen, Straßen und Krankenhäuser."),
        ("Was bedeutet \"Angebot und Nachfrage\" für die Preisbildung?", new[] { "Ist etwas knapp und stark gefragt, steigt meist der Preis", "Preise werden immer zufällig festgelegt", "Angebot und Nachfrage haben keinen Einfluss auf Preise" },
            "Ist etwas knapp und stark gefragt, steigt meist der Preis", "Sind viele Menschen an einem knappen Produkt interessiert (hohe Nachfrage, geringes Angebot), steigt in der Marktwirtschaft meist der Preis."),
        ("Was ist der Unterschied zwischen Import und Export?", new[] { "Import: Waren einführen, Export: Waren ausführen", "Beides bedeutet dasselbe", "Import bedeutet, im eigenen Land zu produzieren" },
            "Import: Waren einführen, Export: Waren ausführen", "Importe sind Waren, die aus dem Ausland eingeführt werden, Exporte sind Waren, die ins Ausland verkauft werden."),
        ("Was versteht man unter \"Angebot\" in der Wirtschaft?", new[] { "Die Menge eines Produkts, die Anbieter verkaufen wollen", "Die Menge, die Kunden kaufen wollen", "Die Steuerhöhe eines Landes" },
            "Die Menge eines Produkts, die Anbieter verkaufen wollen", "Das Angebot beschreibt, wie viel von einem Produkt Anbieter zu einem bestimmten Preis verkaufen möchten."),
        ("Was versteht man unter \"Nachfrage\" in der Wirtschaft?", new[] { "Die Menge eines Produkts, die Kunden kaufen wollen", "Die Menge, die Anbieter herstellen wollen", "Die Anzahl der Fabriken" },
            "Die Menge eines Produkts, die Kunden kaufen wollen", "Die Nachfrage beschreibt, wie viel von einem Produkt Kundinnen und Kunden zu einem bestimmten Preis kaufen möchten."),
        ("Was ist ein Unternehmen im Wirtschaftskreislauf?", new[] { "Ein Betrieb, der Güter oder Dienstleistungen produziert und anbietet", "Ein Ort, an dem nur konsumiert wird", "Eine staatliche Behörde" },
            "Ein Betrieb, der Güter oder Dienstleistungen produziert und anbietet", "Unternehmen produzieren Güter oder bieten Dienstleistungen an und verkaufen sie an Haushalte oder andere Unternehmen."),
        ("Was bedeutet \"Lohn\" im Wirtschaftskreislauf?", new[] { "Bezahlung, die Beschäftigte für ihre Arbeitskraft erhalten", "Steuern, die der Staat einzieht - eine verbreitete, aber falsche Annahme", "Der Preis für Rohstoffe" },
            "Bezahlung, die Beschäftigte für ihre Arbeitskraft erhalten", "Der Lohn ist die Bezahlung, die Beschäftigte von Unternehmen für ihre geleistete Arbeit erhalten."),
        ("Was passiert bei einer Wirtschaftskrise häufig mit der Arbeitslosigkeit?", new[] { "Sie steigt meist an", "Sie sinkt immer", "Sie bleibt unverändert" },
            "Sie steigt meist an", "In Wirtschaftskrisen produzieren und verkaufen Unternehmen oft weniger, wodurch häufig Arbeitsplätze wegfallen und die Arbeitslosigkeit steigt."),
        ("Was bedeutet \"Sparen\" für private Haushalte im Wirtschaftskreislauf?", new[] { "Einen Teil des Einkommens nicht ausgeben, sondern zurücklegen", "Das gesamte Einkommen sofort ausgeben, was einer genaueren Pruefung nicht standhaelt", "Kein Einkommen erhalten" },
            "Einen Teil des Einkommens nicht ausgeben, sondern zurücklegen", "Beim Sparen legen private Haushalte einen Teil ihres Einkommens zurück, statt ihn sofort auszugeben."),
        ("Was ist ein \"Kredit\"?", new[] { "Geliehenes Geld, das mit Zinsen zurückgezahlt werden muss", "Geschenktes Geld ohne Rückzahlung, obwohl das auf den ersten Blick plausibel klingt", "Eine Steuer" },
            "Geliehenes Geld, das mit Zinsen zurückgezahlt werden muss", "Ein Kredit ist Geld, das man sich z.B. von einer Bank leiht und später mit Zinsen zurückzahlen muss."),
        ("Was bedeutet \"Zinsen\" bei einem Kredit oder Sparguthaben?", new[] { "Ein zusätzlicher Betrag, der für das Verleihen oder Anlegen von Geld gezahlt wird", "Ein fester Betrag, den man einmalig zahlt, was die eigentliche Bedeutung des Begriffs verfehlt", "Eine staatliche Strafe" },
            "Ein zusätzlicher Betrag, der für das Verleihen oder Anlegen von Geld gezahlt wird", "Zinsen sind der Preis dafür, dass jemand Geld verleiht oder anlegt - sie werden meist als Prozentsatz berechnet."),
        ("Was ist der Unterschied zwischen Brutto- und Nettolohn?", new[] { "Brutto ist der Lohn vor Abzügen, Netto der Betrag nach Steuern und Abgaben", "Beides ist immer identisch", "Netto ist immer höher als Brutto und deshalb hier nicht zutrifft, was so nicht korrekt ist" },
            "Brutto ist der Lohn vor Abzügen, Netto der Betrag nach Steuern und Abgaben", "Der Bruttolohn ist der volle Lohn vor Steuern und Sozialabgaben, der Nettolohn der Betrag, der danach übrig bleibt."),
        ("Wofür verwendet der Staat die Einnahmen aus Steuern typischerweise?", new[] { "Für öffentliche Aufgaben wie Schulen, Straßen, Gesundheit und Sicherheit", "Nur für private Zwecke von Politikern - eine haeufige, aber unzutreffende Vorstellung", "Steuern werden nie verwendet" },
            "Für öffentliche Aufgaben wie Schulen, Straßen, Gesundheit und Sicherheit", "Mit Steuereinnahmen finanziert der Staat öffentliche Aufgaben wie Bildung, Infrastruktur, Gesundheit und Sicherheit."),
        ("Was bedeutet \"Marktwirtschaft\"?", new[] { "Preise und Produktion regeln sich vor allem über Angebot und Nachfrage", "Der Staat legt alle Preise fest, auch wenn das manche zunaechst vermuten wuerden", "Es gibt keinerlei Handel" },
            "Preise und Produktion regeln sich vor allem über Angebot und Nachfrage", "In einer Marktwirtschaft bestimmen vor allem Angebot und Nachfrage, was produziert wird und zu welchem Preis."),
        ("Was bedeutet \"soziale Marktwirtschaft\" in Deutschland?", new[] { "Freier Markt kombiniert mit staatlicher sozialer Absicherung", "Ausschließlich staatlich geplante Wirtschaft", "Wirtschaft ganz ohne staatliche Regeln" },
            "Freier Markt kombiniert mit staatlicher sozialer Absicherung", "Die soziale Marktwirtschaft verbindet freien Wettbewerb mit staatlicher sozialer Absicherung, z.B. durch Sozialversicherungen."),
        ("Was passiert mit Preisen, wenn das Angebot eines Produkts stark steigt, die Nachfrage aber gleich bleibt?", new[] { "Der Preis sinkt meist", "Der Preis steigt meist", "Der Preis bleibt garantiert gleich" },
            "Der Preis sinkt meist", "Steigt das Angebot bei gleichbleibender Nachfrage, sinkt in der Marktwirtschaft meist der Preis."),
        ("Was ist die Aufgabe von Banken im Wirtschaftskreislauf?", new[] { "Sie verwalten Geld, vergeben Kredite und ermöglichen Zahlungsverkehr", "Sie produzieren nur Lebensmittel", "Sie erheben Steuern für den Staat, was bei genauerem Hinsehen nicht stimmt" },
            "Sie verwalten Geld, vergeben Kredite und ermöglichen Zahlungsverkehr", "Banken verwalten Spareinlagen, vergeben Kredite an Haushalte und Unternehmen und wickeln den Zahlungsverkehr ab."),
        ("Was bedeutet \"Export\" für die Wirtschaft eines Landes?", new[] { "Waren und Dienstleistungen werden ins Ausland verkauft", "Waren werden nur im eigenen Land verbraucht (was so in der Praxis nicht zutrifft)", "Es findet gar kein Handel statt" },
            "Waren und Dienstleistungen werden ins Ausland verkauft", "Beim Export verkauft ein Land Waren oder Dienstleistungen ins Ausland, was Einnahmen für die heimische Wirtschaft bringt.")
    };

    private static QuizQuestion Wirtschaftskreislauf(Random r)
    {
        var f = WirtschaftListe[r.Next(WirtschaftListe.Length)];
        return new QuizQuestion
        {
            Id = NewId(), Subject = Subject.Gewi, GradeLevel = GradeLevel.Klasse9,
            Topic = "Wirtschaftskreislauf", Type = QuestionType.MultipleChoice,
            Prompt = f.Frage, Options = f.Optionen, CorrectAnswers = new[] { f.Antwort }, Explanation = f.Erklaerung,
            HelpHint = "Im einfachen Wirtschaftskreislauf tauschen Haushalte und Unternehmen Arbeitskraft/Güter gegen Geld."
        };
    }

    private static readonly (string Frage, string[] Optionen, string Antwort, string Erklaerung)[] MedienListe =
    {
        ("Was ist eine wichtige Aufgabe von Medien in einer Demokratie?", new[] { "Die Öffentlichkeit informieren und die Regierung kontrollieren", "Nur Werbung zu zeigen", "Immer nur eine Meinung zu vertreten - eine verbreitete, aber falsche Annahme" },
            "Die Öffentlichkeit informieren und die Regierung kontrollieren",
            "Medien werden oft als \"vierte Gewalt\" bezeichnet, weil sie die Öffentlichkeit informieren und Politik/Wirtschaft kritisch beobachten."),
        ("Was sollte man tun, bevor man eine Nachricht aus dem Internet weiterverbreitet?", new[] { "Die Quelle und Fakten prüfen", "Sofort teilen, ohne nachzudenken", "Nur die Überschrift lesen" },
            "Die Quelle und Fakten prüfen", "Um Falschmeldungen (Fake News) nicht zu verbreiten, sollte man Quelle und Fakten kritisch prüfen."),
        ("Was unterscheidet einen seriösen Nachrichtensender von einer unseriösen Quelle?", new[] { "Er nennt Quellen und trennt Meinung von Fakten", "Er hat die reißerischste Überschrift, was einer genaueren Pruefung nicht standhaelt", "Er postet am schnellsten"}, "Er nennt Quellen und trennt Meinung von Fakten",
            "Seriöser Journalismus recherchiert sorgfältig, nennt Quellen und kennzeichnet klar, was Fakten und was Meinung ist."),
        ("Warum nennt man Medien manchmal die \"vierte Gewalt\" im Staat?", new[] { "Weil sie Politik und Wirtschaft öffentlich kontrollieren", "Weil sie über Gesetze abstimmen dürfen, obwohl das auf den ersten Blick plausibel klingt", "Weil sie Gerichtsurteile fällen" }, "Weil sie Politik und Wirtschaft öffentlich kontrollieren",
            "Neben Legislative, Exekutive und Judikative gelten Medien als \"vierte Gewalt\", weil sie durch Berichterstattung Machtmissbrauch aufdecken können."),
        ("Was unterscheidet soziale Medien von klassischen Nachrichtenmedien?", new[] { "Bei sozialen Medien kann im Prinzip jeder selbst Inhalte veröffentlichen", "Soziale Medien werden immer von Journalisten geprüft, was die eigentliche Bedeutung des Begriffs verfehlt", "Es gibt keinen Unterschied" }, "Bei sozialen Medien kann im Prinzip jeder selbst Inhalte veröffentlichen",
            "Anders als bei klassischen Medien mit Redaktion kann bei sozialen Medien jede Person Inhalte posten - das erhöht das Risiko ungeprüfter Falschinformationen."),
        ("Was bedeutet \"Pressefreiheit\"?", new[] { "Medien dürfen frei und unabhängig vom Staat berichten", "Nur der Staat darf über Medien berichten", "Zeitungen kosten kein Geld" }, "Medien dürfen frei und unabhängig vom Staat berichten",
            "Pressefreiheit bedeutet, dass Medien unabhängig und ohne staatliche Zensur berichten dürfen."),
        ("Was ist ein \"Boulevardmedium\"?", new[] { "Ein Medium, das oft reißerisch und stark vereinfachend über Themen berichtet", "Ein rein wissenschaftliches Fachmagazin und deshalb hier nicht zutrifft, was so nicht korrekt ist", "Ein staatliches Amtsblatt" }, "Ein Medium, das oft reißerisch und stark vereinfachend über Themen berichtet",
            "Boulevardmedien berichten oft besonders reißerisch, emotional und vereinfachend, um viele Leserinnen und Leser anzusprechen."),
        ("Was bedeutet \"Fake News\"?", new[] { "Bewusst verbreitete Falschmeldungen, die wie echte Nachrichten aussehen", "Nachrichten, die immer wahr sind", "Nachrichten, die nur im Fernsehen laufen" }, "Bewusst verbreitete Falschmeldungen, die wie echte Nachrichten aussehen",
            "Fake News sind bewusst erfundene oder verfälschte Meldungen, die wie echte Nachrichten wirken sollen."),
        ("Woran erkennt man oft eine unseriöse Quelle im Internet?", new[] { "Fehlende Quellenangaben und reißerische Übertreibungen", "Ein bekanntes Impressum mit Kontaktadresse", "Sachliche, nachprüfbare Fakten" }, "Fehlende Quellenangaben und reißerische Übertreibungen",
            "Unseriöse Quellen liefern oft keine überprüfbaren Quellen und übertreiben oder dramatisieren stark."),
        ("Was bedeutet \"Filterblase\" in sozialen Medien?", new[] { "Man bekommt hauptsächlich Inhalte gezeigt, die der eigenen Meinung entsprechen", "Man sieht automatisch alle Meinungen gleichermaßen - eine haeufige, aber unzutreffende Vorstellung", "Soziale Medien zeigen niemals persönliche Inhalte" }, "Man bekommt hauptsächlich Inhalte gezeigt, die der eigenen Meinung entsprechen",
            "In einer Filterblase bekommt man vor allem Inhalte angezeigt, die zur eigenen Meinung passen, wodurch andere Sichtweisen seltener vorkommen."),
        ("Was ist ein \"Algorithmus\" in sozialen Medien?", new[] { "Eine Regel, nach der eine Software auswählt, welche Inhalte angezeigt werden", "Ein Mensch, der jeden Beitrag persönlich prüft, auch wenn das manche zunaechst vermuten wuerden", "Ein Gesetz gegen Falschnachrichten" }, "Eine Regel, nach der eine Software auswählt, welche Inhalte angezeigt werden",
            "Algorithmen entscheiden automatisiert, welche Beiträge einer Nutzerin oder einem Nutzer angezeigt werden."),
        ("Was sollte man tun, bevor man einen Kommentar im Internet abschickt?", new[] { "Überlegen, ob der Kommentar respektvoll und wahr ist", "Sofort alles unüberlegt posten", "Immer möglichst beleidigend schreiben, was bei genauerem Hinsehen nicht stimmt" }, "Überlegen, ob der Kommentar respektvoll und wahr ist",
            "Vor dem Absenden eines Kommentars sollte man kurz überlegen, ob er respektvoll, fair und inhaltlich richtig ist."),
        ("Was bedeutet \"Cybermobbing\"?", new[] { "Wiederholtes Beleidigen, Bedrohen oder Bloßstellen einer Person über digitale Medien", "Ein freundlicher Chat zwischen Freunden (was so in der Praxis nicht zutrifft) - eine verbreitete, aber falsche Annahme", "Ein technischer Fehler im Internet" }, "Wiederholtes Beleidigen, Bedrohen oder Bloßstellen einer Person über digitale Medien",
            "Cybermobbing bezeichnet wiederholtes Beleidigen, Bedrohen oder Bloßstellen einer Person über das Internet oder digitale Geräte."),
        ("Warum ist es wichtig, im Internet mehrere Quellen zu einer Nachricht zu vergleichen?", new[] { "Um Fehler oder bewusste Falschinformationen einzelner Quellen zu erkennen", "Weil alle Quellen sowieso immer identisch berichten", "Es ist nicht wichtig" }, "Um Fehler oder bewusste Falschinformationen einzelner Quellen zu erkennen",
            "Der Vergleich mehrerer Quellen hilft, Fehler oder bewusste Falschinformationen einzelner Berichte zu erkennen."),
        ("Was ist der öffentlich-rechtliche Rundfunk (z.B. ARD, ZDF) in Deutschland?", new[] { "Ein durch Rundfunkbeitrag finanziertes, unabhängiges Mediensystem mit Informationsauftrag", "Ein rein privates Gewinn-Unternehmen", "Ein staatlich kontrolliertes Propagandamedium, was einer genaueren Pruefung nicht standhaelt, obwohl das auf den ersten Blick plausibel klingt" }, "Ein durch Rundfunkbeitrag finanziertes, unabhängiges Mediensystem mit Informationsauftrag",
            "ARD und ZDF werden über den Rundfunkbeitrag finanziert und sind zu unabhängiger, ausgewogener Berichterstattung verpflichtet."),
        ("Was bedeutet Medienkompetenz?", new[] { "Die Fähigkeit, Medieninhalte kritisch einzuordnen und sicher zu nutzen", "Möglichst viele Stunden am Handy zu verbringen, was die eigentliche Bedeutung des Begriffs verfehlt", "Nur Inhalte zu glauben, die viele Likes haben" }, "Die Fähigkeit, Medieninhalte kritisch einzuordnen und sicher zu nutzen",
            "Medienkompetenz bedeutet, Medieninhalte kritisch beurteilen und Medien verantwortungsvoll nutzen zu können."),
        ("Warum kennzeichnen seriöse Medien manchmal Beiträge als \"Meinung\" oder \"Kommentar\"?", new[] { "Um Fakten klar von persönlichen Einschätzungen zu trennen", "Um Leser bewusst zu verwirren", "Weil das gesetzlich verboten ist und deshalb hier nicht zutrifft" }, "Um Fakten klar von persönlichen Einschätzungen zu trennen",
            "Die Kennzeichnung als Meinung oder Kommentar hilft, sachliche Fakten klar von persönlichen Einschätzungen zu unterscheiden."),
        ("Was kann übermäßiger Konsum von sozialen Medien bei Kindern und Jugendlichen begünstigen?", new[] { "Schlafmangel und Konzentrationsprobleme", "Automatisch bessere Schulnoten", "Keinerlei Auswirkungen" }, "Schlafmangel und Konzentrationsprobleme",
            "Zu viel Bildschirmzeit, besonders spätabends, kann Schlaf und Konzentrationsfähigkeit von Kindern und Jugendlichen beeinträchtigen."),
        ("Was sollte man tun, wenn man online gemobbt wird oder Mobbing beobachtet?", new[] { "Sich Erwachsenen oder Vertrauenspersonen anvertrauen und Beweise sichern", "Es niemandem erzählen, was so nicht korrekt ist - eine haeufige, aber unzutreffende Vorstellung", "Selbst zurückmobben" }, "Sich Erwachsenen oder Vertrauenspersonen anvertrauen und Beweise sichern",
            "Bei Cybermobbing sollte man Beweise (Screenshots) sichern und sich Erwachsenen oder Vertrauenspersonen anvertrauen."),
        ("Warum ist Werbung in Medien oft nicht sofort als solche erkennbar (z.B. bei Influencern)?", new[] { "Weil manche Werbung bewusst wie normaler Inhalt gestaltet wird (Schleichwerbung)", "Weil Werbung gesetzlich verboten ist", "Weil es überhaupt keine Werbung im Internet gibt" }, "Weil manche Werbung bewusst wie normaler Inhalt gestaltet wird (Schleichwerbung)",
            "Manche Werbung wird bewusst wie ein gewöhnlicher Beitrag gestaltet (Schleichwerbung), was sie schwerer erkennbar macht.")
    };

    private static QuizQuestion MedienGesellschaft(Random r)
    {
        var f = MedienListe[r.Next(MedienListe.Length)];
        return new QuizQuestion
        {
            Id = NewId(), Subject = Subject.Gewi, GradeLevel = GradeLevel.Klasse9,
            Topic = "Medien und Gesellschaft", Type = QuestionType.MultipleChoice,
            Prompt = f.Frage, Options = f.Optionen, CorrectAnswers = new[] { f.Antwort }, Explanation = f.Erklaerung,
            HelpHint = "Medien gelten als \"vierte Gewalt\": sie informieren und kontrollieren Politik - deshalb vor dem Teilen immer Quelle und Fakten prüfen."
        };
    }
    // ----- Klasse 7 -----
    // Distraktoren bewusst ähnlich lang wie die richtige Antwort (siehe
    // scripts/check-answer-length-bias.py).

    private static readonly (string Frage, string[] Optionen, string Antwort, string Erklaerung)[] ArmutUndGerechtigkeitListe =
    {
        ("Was bedeutet relative Armut in Deutschland?", new[] { "Weniger als 60 Prozent des mittleren Einkommens", "Kein Zugang zu sauberem Trinkwasser", "Überhaupt kein eigenes Einkommen" }, "Weniger als 60 Prozent des mittleren Einkommens",
            "Relative Armut misst, ob jemand am üblichen Leben teilhaben kann."),
        ("Was ist absolute Armut?", new[] { "Grundbedürfnisse wie Essen und Obdach sind nicht gedeckt", "Ein Einkommen unter dem Landesdurchschnitt (was so in der Praxis nicht zutrifft)", "Kein eigenes Auto zu besitzen" }, "Grundbedürfnisse wie Essen und Obdach sind nicht gedeckt",
            "Sie betrifft weltweit vor allem Menschen im globalen Süden."),
        ("Was erhöht das Armutsrisiko in Deutschland besonders?", new[] { "Alleinerziehend sein oder ohne Berufsabschluss", "In einer Großstadt zu wohnen", "Mehr als zwei Geschwister zu haben" }, "Alleinerziehend sein oder ohne Berufsabschluss",
            "Auch Langzeitarbeitslosigkeit erhöht das Risiko deutlich."),
        ("Warum ist Bildung für Lebenschancen so wichtig?", new[] { "Sie beeinflusst Beruf, Einkommen und Teilhabe", "Sie ist gesetzlich vorgeschrieben", "Sie verkürzt die Arbeitszeit" }, "Sie beeinflusst Beruf, Einkommen und Teilhabe",
            "In Deutschland hängt Bildungserfolg noch stark vom Elternhaus ab."),
        ("Was bedeutet Chancengleichheit?", new[] { "Alle sollen unabhängig von der Herkunft die gleichen Möglichkeiten haben", "Alle sollen am Ende gleich viel verdienen - eine verbreitete, aber falsche Annahme", "Alle müssen denselben Beruf ergreifen" }, "Alle sollen unabhängig von der Herkunft die gleichen Möglichkeiten haben",
            "Ganztagsschulen und Förderprogramme sollen dazu beitragen."),
        ("Was ist der Sozialstaat?", new[] { "Ein Staat, der seine Bürger sozial absichert", "Ein Staat ohne jede Steuererhebung", "Ein Staat mit reiner Planwirtschaft, was einer genaueren Pruefung nicht standhaelt" }, "Ein Staat, der seine Bürger sozial absichert",
            "Das Sozialstaatsprinzip steht im Grundgesetz."),
        ("Welche Zweige umfasst die Sozialversicherung?", new[] { "Kranken-, Renten-, Pflege-, Unfall- und Arbeitslosenversicherung", "Nur Kranken- und Rentenversicherung, obwohl das auf den ersten Blick plausibel klingt", "Haftpflicht und Hausratversicherung" }, "Kranken-, Renten-, Pflege-, Unfall- und Arbeitslosenversicherung",
            "Sie beruht auf dem Solidarprinzip: Alle zahlen ein, Bedürftige erhalten Leistungen."),
        ("Was bedeutet das Solidarprinzip?", new[] { "Starke Schultern tragen mehr als schwache", "Jeder zahlt genau denselben Betrag, was die eigentliche Bedeutung des Begriffs verfehlt", "Nur Kranke zahlen Beiträge" }, "Starke Schultern tragen mehr als schwache",
            "Die Beiträge richten sich nach dem Einkommen, die Leistungen nach dem Bedarf."),
        ("Wozu dient der Mindestlohn?", new[] { "Er sichert eine Lohnuntergrenze für alle Beschäftigten", "Er legt das Höchstgehalt fest und deshalb hier nicht zutrifft", "Er gilt nur für Auszubildende" }, "Er sichert eine Lohnuntergrenze für alle Beschäftigten",
            "Er soll verhindern, dass Menschen trotz Vollzeitarbeit arm bleiben."),
        ("Was ist die Tafel?", new[] { "Eine Einrichtung, die Lebensmittel an Bedürftige verteilt", "Eine staatliche Behörde für Sozialhilfe", "Ein Beratungsdienst für Schulden" }, "Eine Einrichtung, die Lebensmittel an Bedürftige verteilt",
            "Sie rettet zugleich Lebensmittel, die sonst weggeworfen würden."),
        ("Was zeigt die Einkommensverteilung in Deutschland?", new[] { "Der Wohlstand ist ungleich verteilt", "Alle verdienen ungefähr gleich viel", "Die Unterschiede sind gesetzlich verboten" }, "Der Wohlstand ist ungleich verteilt",
            "Die reichsten zehn Prozent besitzen einen Großteil des Vermögens."),
        ("Was ist Kinderarmut?", new[] { "Kinder in Haushalten mit sehr geringem Einkommen", "Kinder ohne eigenes Taschengeld, was so nicht korrekt ist - eine haeufige, aber unzutreffende Vorstellung", "Kinder ohne Smartphone" }, "Kinder in Haushalten mit sehr geringem Einkommen",
            "In manchen Berliner Bezirken ist mehr als jedes dritte Kind betroffen."),
        ("Warum ist Armut nicht nur eine Frage des Geldes?", new[] { "Sie beschränkt auch Teilhabe, Gesundheit und Bildung", "Sie betrifft ausschließlich die Ernährung", "Sie ist nur ein statistisches Problem" }, "Sie beschränkt auch Teilhabe, Gesundheit und Bildung",
            "Wer nicht mitfahren kann auf Klassenfahrt, ist auch sozial ausgeschlossen."),
        ("Was ist ein Bildungs- und Teilhabepaket?", new[] { "Staatliche Zuschüsse für Schulbedarf und Freizeit", "Ein Schulbuchverleih für alle Kinder", "Ein Programm für Hochbegabte" }, "Staatliche Zuschüsse für Schulbedarf und Freizeit",
            "Es soll Kindern aus armen Familien Ausflüge und Vereinsbeiträge ermöglichen."),
        ("Was versteht man unter dem globalen Süden?", new[] { "Wirtschaftlich benachteiligte Weltregionen", "Alle Länder südlich des Äquators", "Die südlichen EU-Staaten" }, "Wirtschaftlich benachteiligte Weltregionen",
            "Der Begriff ersetzt das wertende Wort Entwicklungsländer."),
        ("Was sind die Nachhaltigkeitsziele der Vereinten Nationen?", new[] { "17 globale Ziele bis 2030 für eine gerechtere Welt", "Ein Klimaabkommen einzelner Staaten", "Handelsregeln der Welthandelsorganisation" }, "17 globale Ziele bis 2030 für eine gerechtere Welt",
            "Ziel eins lautet: Armut in jeder Form beenden."),
        ("Was ist fairer Handel?", new[] { "Handel mit garantierten Mindestpreisen für Erzeuger", "Handel ganz ohne Zwischenhändler", "Handel nur innerhalb Europas" }, "Handel mit garantierten Mindestpreisen für Erzeuger",
            "So sollen Bauern im globalen Süden verlässlich planen können."),
        ("Woran erkennt man fair gehandelte Produkte?", new[] { "An anerkannten Siegeln auf der Verpackung", "Am deutlich höheren Preis allein", "An der Herkunft aus Europa" }, "An anerkannten Siegeln auf der Verpackung",
            "Siegel sind aber unterschiedlich streng - genaues Hinsehen lohnt sich."),
        ("Was ist Mikrokredit?", new[] { "Ein Kleinkredit für Existenzgründungen", "Ein Kredit für Großkonzerne", "Ein zinsloses Darlehen vom Staat" }, "Ein Kleinkredit für Existenzgründungen",
            "Kleine Summen ermöglichen etwa den Kauf einer Nähmaschine."),
        ("Warum ist Armutsbekämpfung eine gemeinsame Aufgabe?", new[] { "Ursachen und Folgen wirken weltweit zusammen", "Sie betrifft nur einzelne Staaten", "Nur Wohlfahrtsverbände sind zuständig, auch wenn das manche zunaechst vermuten wuerden" }, "Ursachen und Folgen wirken weltweit zusammen",
            "Handel, Klimawandel und Konflikte hängen eng miteinander zusammen.")
    };

    private static QuizQuestion ArmutUndGerechtigkeit(Random r)
    {
        var f = ArmutUndGerechtigkeitListe[r.Next(ArmutUndGerechtigkeitListe.Length)];
        return new QuizQuestion
        {
            Id = NewId(), Subject = Subject.Gewi, GradeLevel = GradeLevel.Klasse7,
            Topic = "Armut und Gerechtigkeit", Type = QuestionType.MultipleChoice,
            Prompt = f.Frage, Options = f.Optionen, CorrectAnswers = new[] { f.Antwort }, Explanation = f.Erklaerung,
            HelpHint = "Relative Armut = unter 60% des mittleren Einkommens. Sozialstaat und Solidarprinzip: alle zahlen ein, Bedürftige erhalten. Chancengleichheit hängt in Deutschland noch stark vom Elternhaus ab."
        };
    }
    private static readonly (string Frage, string[] Optionen, string Antwort, string Erklaerung)[] EuropaUndEUListe =
    {
        ("Wie viele Mitgliedstaaten hat die Europäische Union derzeit?", new[] { "27 Staaten", "15 Staaten", "50 Staaten" }, "27 Staaten",
            "Nach dem Austritt Großbritanniens 2020 sind es 27."),
        ("Was ist der Binnenmarkt der EU?", new[] { "Freier Verkehr von Waren, Personen, Dienstleistungen und Kapital", "Ein gemeinsamer Militärverband, was bei genauerem Hinsehen nicht stimmt (was so in der Praxis nicht zutrifft)", "Eine gemeinsame Steuerbehörde" }, "Freier Verkehr von Waren, Personen, Dienstleistungen und Kapital",
            "Diese vier Grundfreiheiten sind der Kern der Union."),
        ("Was regelt das Schengener Abkommen?", new[] { "Wegfall der Grenzkontrollen zwischen Mitgliedstaaten", "Die gemeinsame Währung", "Die Verteilung der Agrarhilfen - eine verbreitete, aber falsche Annahme" }, "Wegfall der Grenzkontrollen zwischen Mitgliedstaaten",
            "Man kann von Berlin bis Lissabon ohne Passkontrolle reisen."),
        ("Welche Währung nutzen die meisten EU-Staaten?", new[] { "Den Euro", "Den Schweizer Franken", "Das britische Pfund" }, "Den Euro",
            "20 der 27 Mitgliedstaaten bilden die Eurozone."),
        ("Welchen Vorteil bringt eine gemeinsame Währung?", new[] { "Kein Umtausch und leichter Preisvergleich", "Höhere Zinsen für alle Sparer", "Niedrigere Steuern in allen Ländern" }, "Kein Umtausch und leichter Preisvergleich",
            "Nachteil: Ein Land kann seine Währung nicht mehr eigenständig anpassen."),
        ("Wer wählt das Europäische Parlament?", new[] { "Die Bürgerinnen und Bürger der Mitgliedstaaten", "Die nationalen Regierungen", "Die EU-Kommission" }, "Die Bürgerinnen und Bürger der Mitgliedstaaten",
            "Es ist das einzige direkt gewählte EU-Organ."),
        ("Welche Aufgabe hat die Europäische Kommission?", new[] { "Sie schlägt Gesetze vor und führt sie aus", "Sie spricht Recht bei Streitfällen", "Sie wählt den EU-Ratspräsidenten" }, "Sie schlägt Gesetze vor und führt sie aus",
            "Man nennt sie deshalb auch die Regierung der EU."),
        ("Was war die ursprüngliche Idee der europäischen Einigung?", new[] { "Frieden durch wirtschaftliche Verflechtung sichern", "Einen gemeinsamen Staat zu gründen", "Eine Militärallianz gegen den Osten, was einer genaueren Pruefung nicht standhaelt" }, "Frieden durch wirtschaftliche Verflechtung sichern",
            "Wer gemeinsam Kohle und Stahl verwaltet, führt keinen Krieg gegeneinander."),
        ("Was war die Montanunion?", new[] { "Der Vorläufer der EU für Kohle und Stahl", "Ein Bündnis der Bergarbeitergewerkschaften", "Ein Handelsabkommen mit den USA" }, "Der Vorläufer der EU für Kohle und Stahl",
            "1951 gegründet - beides waren die Rohstoffe der Rüstungsindustrie."),
        ("Was bedeutet das Erasmus-Programm?", new[] { "Austauschprogramm für Studierende und Schüler", "Ein Stipendium für Spitzensportler, obwohl das auf den ersten Blick plausibel klingt", "Ein Förderprogramm für Landwirte" }, "Austauschprogramm für Studierende und Schüler",
            "Millionen junger Menschen haben damit im Ausland gelernt."),
        ("Was ist ein EU-Beitrittskandidat?", new[] { "Ein Staat, der aufgenommen werden möchte und geprüft wird", "Ein Staat, der bereits Mitglied ist, was die eigentliche Bedeutung des Begriffs verfehlt", "Ein Staat, der austreten will" }, "Ein Staat, der aufgenommen werden möchte und geprüft wird",
            "Beitrittsländer müssen Rechtsstaat, Demokratie und Marktwirtschaft nachweisen."),
        ("Welche Kriterien muss ein Beitrittsland erfüllen?", new[] { "Demokratie, Rechtsstaatlichkeit und Marktwirtschaft", "Eine Mindesteinwohnerzahl", "Mitgliedschaft in der NATO" }, "Demokratie, Rechtsstaatlichkeit und Marktwirtschaft",
            "Diese Kopenhagener Kriterien wurden 1993 festgelegt."),
        ("Was bedeutet Subsidiarität in der EU?", new[] { "Entscheidungen möglichst auf der untersten geeigneten Ebene", "Alle Entscheidungen trifft Brüssel", "Jedes Land entscheidet völlig allein und deshalb hier nicht zutrifft" }, "Entscheidungen möglichst auf der untersten geeigneten Ebene",
            "Die EU handelt nur, wenn sie es besser kann als die Mitgliedstaaten."),
        ("Was ist der Brexit?", new[] { "Der Austritt Großbritanniens aus der EU", "Ein Handelsabkommen mit Norwegen", "Die Einführung des Euro in Britannien, was so nicht korrekt ist" }, "Der Austritt Großbritanniens aus der EU",
            "2016 im Referendum beschlossen, 2020 vollzogen."),
        ("Welche Folgen hatte der Brexit für Reisende?", new[] { "Wieder Grenzkontrollen und neue Zollregeln", "Keinerlei praktische Veränderungen", "Eine gemeinsame Währung mit der EU" }, "Wieder Grenzkontrollen und neue Zollregeln",
            "Auch Studium und Arbeiten wurden bürokratischer."),
        ("Was fördert die EU-Regionalpolitik?", new[] { "Den Ausgleich zwischen reicheren und ärmeren Regionen", "Ausschließlich große Konzerne - eine haeufige, aber unzutreffende Vorstellung", "Den Bau von Militäranlagen" }, "Den Ausgleich zwischen reicheren und ärmeren Regionen",
            "Auch in Brandenburg wurden Projekte mit EU-Mitteln finanziert."),
        ("Warum ist die EU auch umstritten?", new[] { "Kritik an Bürokratie und Ferne von den Bürgern", "Weil sie keine Gesetze erlassen darf", "Weil sie nur aus drei Ländern besteht, auch wenn das manche zunaechst vermuten wuerden" }, "Kritik an Bürokratie und Ferne von den Bürgern",
            "Befürworter verweisen auf Frieden, Freizügigkeit und Wirtschaftskraft."),
        ("Was bedeutet Freizügigkeit in der EU?", new[] { "Das Recht, in jedem Mitgliedstaat zu leben und zu arbeiten", "Freier Eintritt in alle Museen", "Kostenlose Bahnfahrten für Jugendliche" }, "Das Recht, in jedem Mitgliedstaat zu leben und zu arbeiten",
            "Sie gilt als eine der wichtigsten Errungenschaften der Union."),
        ("Welche Rolle spielt die EU im Klimaschutz?", new[] { "Sie setzt gemeinsame Klimaziele für alle Mitglieder", "Sie überlässt das Thema den Einzelstaaten, was bei genauerem Hinsehen nicht stimmt", "Sie fördert vor allem Kohlekraftwerke" }, "Sie setzt gemeinsame Klimaziele für alle Mitglieder",
            "Der Green Deal sieht Klimaneutralität bis 2050 vor."),
        ("Warum ist Europa für Berlin besonders wichtig?", new[] { "Die Stadt lebt von Austausch, Handel und Zuwanderung", "Berlin ist Sitz der EU-Kommission (was so in der Praxis nicht zutrifft)", "Berlin erhält keine EU-Mittel" }, "Die Stadt lebt von Austausch, Handel und Zuwanderung",
            "Menschen aus über 190 Nationen leben hier zusammen.")
    };

    private static QuizQuestion EuropaUndEU(Random r)
    {
        var f = EuropaUndEUListe[r.Next(EuropaUndEUListe.Length)];
        return new QuizQuestion
        {
            Id = NewId(), Subject = Subject.Gewi, GradeLevel = GradeLevel.Klasse7,
            Topic = "Europa und die Europäische Union", Type = QuestionType.MultipleChoice,
            Prompt = f.Frage, Options = f.Optionen, CorrectAnswers = new[] { f.Antwort }, Explanation = f.Erklaerung,
            HelpHint = "EU: 27 Staaten, Binnenmarkt mit vier Grundfreiheiten, Schengen (keine Grenzkontrollen), Euro in 20 Staaten. Ursprung: Frieden durch wirtschaftliche Verflechtung (Montanunion 1951)."
        };
    }
    private static readonly (string Frage, string[] Optionen, string Antwort, string Erklaerung)[] MigrationUndVielfaltListe =
    {
        ("Was versteht man unter Migration?", new[] { "Menschen verlegen ihren Lebensmittelpunkt dauerhaft", "Reisen in den Urlaub", "Der Wechsel des Arbeitsplatzes im selben Ort - eine verbreitete, aber falsche Annahme" }, "Menschen verlegen ihren Lebensmittelpunkt dauerhaft",
            "Migration gab es zu allen Zeiten und in alle Richtungen."),
        ("Was ist der Unterschied zwischen Migranten und Geflüchteten?", new[] { "Geflüchtete verlassen ihr Land aus Zwang", "Migranten haben keinen Pass, was einer genaueren Pruefung nicht standhaelt", "Es gibt keinen Unterschied" }, "Geflüchtete verlassen ihr Land aus Zwang",
            "Krieg, Verfolgung und Not lassen ihnen keine echte Wahl."),
        ("Was sind Push-Faktoren?", new[] { "Gründe, die Menschen aus einem Land drängen", "Anreize eines Ziellandes, obwohl das auf den ersten Blick plausibel klingt", "Kosten einer Reise" }, "Gründe, die Menschen aus einem Land drängen",
            "Krieg, Armut, Verfolgung und Umweltkatastrophen gehören dazu."),
        ("Was sind Pull-Faktoren?", new[] { "Gründe, die ein Zielland attraktiv machen", "Hindernisse an der Grenze, was die eigentliche Bedeutung des Begriffs verfehlt", "Kosten für Schleuser" }, "Gründe, die ein Zielland attraktiv machen",
            "Arbeit, Sicherheit, Bildung und Familie zählen dazu."),
        ("Wer waren die Gastarbeiter in Deutschland?", new[] { "Angeworbene Arbeitskräfte ab den 1950er Jahren", "Saisonarbeiter aus der Landwirtschaft und deshalb hier nicht zutrifft", "Diplomaten aus dem Ausland" }, "Angeworbene Arbeitskräfte ab den 1950er Jahren",
            "Anwerbeabkommen bestanden unter anderem mit Italien, Griechenland und der Türkei."),
        ("Wann wurde das Anwerbeabkommen mit der Türkei geschlossen?", new[] { "1961", "1945", "1990" }, "1961",
            "Viele Familien in Berlin haben ihre Geschichte in dieser Zeit."),
        ("Warum blieben viele Gastarbeiter dauerhaft?", new[] { "Sie bauten hier ihr Leben und ihre Familien auf", "Die Rückreise war gesetzlich verboten", "Sie hatten ihre Pässe verloren" }, "Sie bauten hier ihr Leben und ihre Familien auf",
            "Der Satz lautet oft: Man rief Arbeitskräfte, und es kamen Menschen."),
        ("Was bedeutet Integration?", new[] { "Gleichberechtigte Teilhabe am gesellschaftlichen Leben", "Vollständige Aufgabe der eigenen Kultur, was so nicht korrekt ist", "Getrenntes Leben nebeneinander" }, "Gleichberechtigte Teilhabe am gesellschaftlichen Leben",
            "Integration ist eine wechselseitige Aufgabe, nicht nur eine Bringschuld."),
        ("Was unterscheidet Integration von Assimilation?", new[] { "Bei Assimilation soll man die Herkunftskultur aufgeben", "Integration bedeutet Abschottung", "Beide Begriffe sind gleichbedeutend - eine haeufige, aber unzutreffende Vorstellung" }, "Bei Assimilation soll man die Herkunftskultur aufgeben",
            "Integration erlaubt mehrfache Zugehörigkeit."),
        ("Was ist eine plurale Gesellschaft?", new[] { "Eine Gesellschaft mit vielfältigen Lebensweisen", "Eine Gesellschaft mit nur einer Religion, auch wenn das manche zunaechst vermuten wuerden", "Eine Gesellschaft ohne Zuwanderung" }, "Eine Gesellschaft mit vielfältigen Lebensweisen",
            "Berlin ist ein typisches Beispiel für Vielfalt in einer Stadt."),
        ("Was ist Mehrsprachigkeit für Kinder?", new[] { "Ein Vorteil für Denken und spätere Berufe", "Ein Nachteil für die Schulnoten, was bei genauerem Hinsehen nicht stimmt", "Ein rein privates Thema" }, "Ein Vorteil für Denken und spätere Berufe",
            "Wer zwei Sprachen sicher beherrscht, lernt weitere leichter."),
        ("Was regelt das Asylrecht im Grundgesetz?", new[] { "Politisch Verfolgte genießen Asyl", "Jeder darf einwandern", "Asyl gilt nur für Europäer (was so in der Praxis nicht zutrifft)" }, "Politisch Verfolgte genießen Asyl",
            "Artikel 16a ist eine Lehre aus der NS-Zeit, als Deutsche selbst flüchten mussten."),
        ("Was ist die Genfer Flüchtlingskonvention?", new[] { "Ein internationaler Vertrag zum Schutz Geflüchteter", "Ein Handelsabkommen der Schweiz", "Eine Regelung für Arbeitsmigration - eine verbreitete, aber falsche Annahme" }, "Ein internationaler Vertrag zum Schutz Geflüchteter",
            "Sie verbietet die Rückschiebung in Verfolgerstaaten."),
        ("Was bedeutet Diskriminierung?", new[] { "Benachteiligung wegen zugeschriebener Merkmale", "Jede Form von Kritik", "Ungleiche Bezahlung nach Leistung, was einer genaueren Pruefung nicht standhaelt" }, "Benachteiligung wegen zugeschriebener Merkmale",
            "Herkunft, Religion, Geschlecht oder Behinderung dürfen kein Nachteil sein."),
        ("Was schützt das Allgemeine Gleichbehandlungsgesetz?", new[] { "Vor Benachteiligung in Arbeit und Alltag", "Vor zu hohen Mieten", "Vor Steuererhöhungen, obwohl das auf den ersten Blick plausibel klingt" }, "Vor Benachteiligung in Arbeit und Alltag",
            "Es gilt zum Beispiel bei Bewerbungen und Wohnungssuche."),
        ("Was ist ein Vorurteil?", new[] { "Ein Urteil ohne eigene Prüfung der Sachlage", "Eine gut begründete Meinung", "Ein Gerichtsurteil vor der Verhandlung, was die eigentliche Bedeutung des Begriffs verfehlt" }, "Ein Urteil ohne eigene Prüfung der Sachlage",
            "Vorurteile halten sich hartnäckig, weil man sie selten überprüft."),
        ("Wie kann man Vorurteile abbauen?", new[] { "Durch persönliche Begegnung und gemeinsame Erfahrungen", "Durch strikte Trennung der Gruppen", "Durch Ignorieren des Themas" }, "Durch persönliche Begegnung und gemeinsame Erfahrungen",
            "Gemeinsame Ziele im Sport oder Projekt wirken besonders gut."),
        ("Was bedeutet Staatsbürgerschaft?", new[] { "Die rechtliche Zugehörigkeit zu einem Staat", "Der Wohnort einer Person", "Die Sprache, die jemand spricht und deshalb hier nicht zutrifft" }, "Die rechtliche Zugehörigkeit zu einem Staat",
            "Mit ihr sind Rechte wie das Wahlrecht verbunden."),
        ("Wie verändert Zuwanderung eine Stadt?", new[] { "Sie prägt Sprache, Essen, Musik und Wirtschaft", "Sie hat kaum sichtbare Auswirkungen", "Sie betrifft nur einzelne Stadtteile, was so nicht korrekt ist" }, "Sie prägt Sprache, Essen, Musik und Wirtschaft",
            "Der Döner ist in Berlin erfunden worden - ein Beispiel unter vielen."),
        ("Warum ist Zuwanderung für Deutschland wirtschaftlich wichtig?", new[] { "Die Bevölkerung altert und Fachkräfte fehlen", "Sie senkt automatisch alle Preise", "Sie ersetzt die Sozialversicherung - eine haeufige, aber unzutreffende Vorstellung" }, "Die Bevölkerung altert und Fachkräfte fehlen",
            "Ohne Zuwanderung würde die Zahl der Erwerbstätigen deutlich sinken.")
    };

    private static QuizQuestion MigrationUndVielfalt(Random r)
    {
        var f = MigrationUndVielfaltListe[r.Next(MigrationUndVielfaltListe.Length)];
        return new QuizQuestion
        {
            Id = NewId(), Subject = Subject.Gewi, GradeLevel = GradeLevel.Klasse7,
            Topic = "Migration und Vielfalt", Type = QuestionType.MultipleChoice,
            Prompt = f.Frage, Options = f.Optionen, CorrectAnswers = new[] { f.Antwort }, Explanation = f.Erklaerung,
            HelpHint = "Push-Faktoren drängen aus dem Land, Pull-Faktoren ziehen ins Zielland. Anwerbeabkommen mit der Türkei 1961. Integration = gleichberechtigte Teilhabe, nicht Aufgabe der Herkunftskultur."
        };
    }
    private static readonly (string Frage, string[] Optionen, string Antwort, string Erklaerung)[] KonsumUndVerantwortungListe =
    {
        ("Was bedeutet nachhaltiger Konsum?", new[] { "Kaufentscheidungen mit Blick auf Umwelt und Zukunft", "Möglichst günstig einkaufen", "Nur regionale Produkte kaufen, auch wenn das manche zunaechst vermuten wuerden" }, "Kaufentscheidungen mit Blick auf Umwelt und Zukunft",
            "Es geht darum, Ressourcen so zu nutzen, dass auch spätere Generationen leben können."),
        ("Was ist ein ökologischer Fußabdruck?", new[] { "Ein Maß für den Ressourcenverbrauch einer Person", "Die Schuhgröße im Umweltschutz", "Die Fläche eines Naturschutzgebiets, was bei genauerem Hinsehen nicht stimmt" }, "Ein Maß für den Ressourcenverbrauch einer Person",
            "Er zeigt, wie viele Erden nötig wären, wenn alle so lebten."),
        ("Was verursacht bei Kleidung die meisten Umweltschäden?", new[] { "Massenproduktion mit hohem Wasser- und Chemieeinsatz", "Der Transport im Handgepäck", "Das Waschen bei 30 Grad" }, "Massenproduktion mit hohem Wasser- und Chemieeinsatz",
            "Für eine Jeans werden mehrere tausend Liter Wasser benötigt."),
        ("Was ist Fast Fashion?", new[] { "Billige Mode in schnellen Kollektionswechseln", "Besonders schnell gelieferte Kleidung (was so in der Praxis nicht zutrifft)", "Sportkleidung für Wettkämpfe" }, "Billige Mode in schnellen Kollektionswechseln",
            "Kurze Nutzungsdauer und viel Abfall sind die Kehrseite."),
        ("Was bedeutet geplante Obsoleszenz?", new[] { "Produkte werden absichtlich kurzlebig gebaut", "Produkte werden ständig verbessert", "Alte Produkte werden zurückgekauft" }, "Produkte werden absichtlich kurzlebig gebaut",
            "Der Vorwurf ist umstritten, doch Reparierbarkeit ist oft bewusst schlecht."),
        ("Was ist das Recht auf Reparatur?", new[] { "Hersteller müssen Ersatzteile und Anleitungen bereitstellen", "Jeder darf fremde Geräte reparieren - eine verbreitete, aber falsche Annahme", "Reparaturen sind immer kostenlos" }, "Hersteller müssen Ersatzteile und Anleitungen bereitstellen",
            "Die EU stärkt dieses Recht, um Elektroschrott zu verringern."),
        ("Warum ist Elektroschrott ein Problem?", new[] { "Er enthält Giftstoffe und wertvolle Rohstoffe", "Er nimmt nur viel Platz weg", "Er verrottet zu schnell" }, "Er enthält Giftstoffe und wertvolle Rohstoffe",
            "Vieles landet ungesichert im globalen Süden."),
        ("Was ist Kreislaufwirtschaft?", new[] { "Materialien möglichst dauerhaft im Kreislauf halten", "Wirtschaft mit ständigem Wachstum, was einer genaueren Pruefung nicht standhaelt", "Handel nur im eigenen Land" }, "Materialien möglichst dauerhaft im Kreislauf halten",
            "Statt Wegwerfen: reparieren, wiederverwenden, recyceln."),
        ("Was steht in der Abfallhierarchie an erster Stelle?", new[] { "Abfall vermeiden", "Abfall recyceln", "Abfall verbrennen" }, "Abfall vermeiden",
            "Vermeiden ist besser als jede noch so gute Verwertung."),
        ("Was ist Greenwashing?", new[] { "Werbung, die ein Produkt grüner darstellt als es ist", "Das Reinigen von Verpackungen", "Ein Verfahren zur Wasseraufbereitung, obwohl das auf den ersten Blick plausibel klingt" }, "Werbung, die ein Produkt grüner darstellt als es ist",
            "Vage Begriffe wie klimafreundlich ohne Beleg sind ein Warnsignal."),
        ("Wie erkennt man verlässliche Umweltsiegel?", new[] { "Unabhängige Prüfung und klare Kriterien", "Besonders grüne Farbgestaltung, was die eigentliche Bedeutung des Begriffs verfehlt", "Aufdruck des Herstellernamens" }, "Unabhängige Prüfung und klare Kriterien",
            "Blauer Engel und EU-Ecolabel sind Beispiele geprüfter Siegel."),
        ("Was bedeutet Regionalität beim Einkauf?", new[] { "Kurze Transportwege und Stärkung der Region", "Produkte aus dem eigenen Garten", "Waren aus dem gesamten EU-Raum" }, "Kurze Transportwege und Stärkung der Region",
            "Saisonale und regionale Ware spart oft Energie - aber nicht immer."),
        ("Warum ist Saisonalität wichtig?", new[] { "Ungeheizte Freilandware braucht weniger Energie", "Saisonware schmeckt immer besser", "Saisonware ist immer billiger" }, "Ungeheizte Freilandware braucht weniger Energie",
            "Tomaten im Winter aus dem beheizten Gewächshaus sind besonders aufwendig."),
        ("Was ist ein Impulskauf?", new[] { "Ein ungeplanter, spontaner Kauf", "Ein Kauf nach längerem Preisvergleich", "Ein Kauf auf Rechnung" }, "Ein ungeplanter, spontaner Kauf",
            "Werbung und Ladengestaltung sind gezielt darauf ausgerichtet."),
        ("Wie beeinflusst Werbung Kaufentscheidungen?", new[] { "Sie weckt Wünsche und verknüpft Produkte mit Gefühlen", "Sie informiert rein sachlich über Produkte und deshalb hier nicht zutrifft", "Sie hat kaum messbare Wirkung" }, "Sie weckt Wünsche und verknüpft Produkte mit Gefühlen",
            "Wer die Mechanismen kennt, entscheidet bewusster."),
        ("Was ist Influencer-Marketing?", new[] { "Werbung durch Personen mit großer Online-Reichweite", "Werbung ausschließlich im Fernsehen, was so nicht korrekt ist", "Werbung durch Verbraucherzentralen" }, "Werbung durch Personen mit großer Online-Reichweite",
            "Bezahlte Beiträge müssen als Werbung gekennzeichnet sein."),
        ("Was ist ein Haushaltsbudget?", new[] { "Ein Plan für Einnahmen und Ausgaben", "Das Geld auf dem Sparkonto", "Die monatliche Miete" }, "Ein Plan für Einnahmen und Ausgaben",
            "Wer aufschreibt, wofür Geld ausgegeben wird, behält den Überblick."),
        ("Was ist die Schuldenfalle bei jungen Menschen?", new[] { "Ratenkäufe und Abos übersteigen das Einkommen", "Zu hohe Sparraten", "Zu viel Bargeld im Portemonnaie - eine haeufige, aber unzutreffende Vorstellung" }, "Ratenkäufe und Abos übersteigen das Einkommen",
            "Buy-now-pay-later-Angebote verschleiern die tatsächlichen Kosten."),
        ("Was ist ein Widerrufsrecht?", new[] { "Online-Käufe können 14 Tage zurückgegeben werden", "Jeder Kauf kann jederzeit rückgängig gemacht werden", "Es gilt nur im Ladengeschäft" }, "Online-Käufe können 14 Tage zurückgegeben werden",
            "Im Laden gibt es dagegen kein gesetzliches Rückgaberecht."),
        ("Warum haben Verbraucher Marktmacht?", new[] { "Kaufentscheidungen steuern das Angebot", "Sie können Preise selbst festlegen, auch wenn das manche zunaechst vermuten wuerden", "Sie kontrollieren die Produktion" }, "Kaufentscheidungen steuern das Angebot",
            "Was gekauft wird, wird produziert - auch das ist eine Form von Einfluss.")
    };

    private static QuizQuestion KonsumUndVerantwortung(Random r)
    {
        var f = KonsumUndVerantwortungListe[r.Next(KonsumUndVerantwortungListe.Length)];
        return new QuizQuestion
        {
            Id = NewId(), Subject = Subject.Gewi, GradeLevel = GradeLevel.Klasse7,
            Topic = "Konsum und Verantwortung", Type = QuestionType.MultipleChoice,
            Prompt = f.Frage, Options = f.Optionen, CorrectAnswers = new[] { f.Antwort }, Explanation = f.Erklaerung,
            HelpHint = "Nachhaltiger Konsum: ökologischer Fußabdruck, Fast Fashion, Kreislaufwirtschaft. Abfallhierarchie: vermeiden vor verwerten. Greenwashing erkennen: geprüfte Siegel statt vager Begriffe."
        };
    }
    private static readonly (string Frage, string[] Optionen, string Antwort, string Erklaerung)[] MedienUndDigitalesLebenListe =
    {
        ("Was bedeutet Medienkompetenz?", new[] { "Medien verstehen, kritisch nutzen und selbst gestalten", "Möglichst viele Geräte besitzen", "Schnell tippen können" }, "Medien verstehen, kritisch nutzen und selbst gestalten",
            "Dazu gehört auch, Quellen und Absichten einzuschätzen."),
        ("Was sind Fake News?", new[] { "Bewusst verbreitete Falschmeldungen", "Nachrichten mit Rechtschreibfehlern", "Meinungsbeiträge in Zeitungen" }, "Bewusst verbreitete Falschmeldungen",
            "Sie zielen meist auf Empörung, Klicks oder politische Wirkung."),
        ("Wie prüft man eine Nachricht?", new[] { "Quelle, Datum und weitere seriöse Berichte prüfen", "Auf die Anzahl der Likes achten", "Der Überschrift vertrauen" }, "Quelle, Datum und weitere seriöse Berichte prüfen",
            "Alte Bilder in neuem Zusammenhang sind ein häufiger Trick."),
        ("Was ist ein Impressum?", new[] { "Die Pflichtangabe, wer für eine Seite verantwortlich ist", "Ein Verzeichnis aller Artikel", "Die Datenschutzerklärung" }, "Die Pflichtangabe, wer für eine Seite verantwortlich ist",
            "Fehlt es, ist Vorsicht geboten - seriöse Seiten haben eines."),
        ("Was unterscheidet Nachricht und Kommentar?", new[] { "Die Nachricht informiert, der Kommentar bewertet", "Der Kommentar ist immer länger", "Beide sind identisch aufgebaut" }, "Die Nachricht informiert, der Kommentar bewertet",
            "Seriöse Medien kennzeichnen Meinungsbeiträge klar."),
        ("Was ist eine Filterblase?", new[] { "Man sieht überwiegend Inhalte der eigenen Sicht", "Ein technischer Schutz vor Viren", "Eine Funktion zum Sperren von Werbung, was bei genauerem Hinsehen nicht stimmt" }, "Man sieht überwiegend Inhalte der eigenen Sicht",
            "Algorithmen verstärken, was ohnehin gefällt."),
        ("Was ist eine Echokammer?", new[] { "Ein Umfeld, in dem die eigene Meinung ständig bestätigt wird", "Ein Raum mit gutem Klang", "Eine Chatgruppe mit vielen Mitgliedern" }, "Ein Umfeld, in dem die eigene Meinung ständig bestätigt wird",
            "Widerspruch dringt kaum noch durch - das verhärtet Positionen."),
        ("Was ist Cybermobbing?", new[] { "Wiederholtes Bloßstellen oder Bedrohen im Netz", "Ein einmaliger Streit im Chat", "Kritik an einem Beitrag" }, "Wiederholtes Bloßstellen oder Bedrohen im Netz",
            "Es wirkt besonders stark, weil es rund um die Uhr weitergeht."),
        ("Was sollte man bei Cybermobbing tun?", new[] { "Beweise sichern, blockieren und Hilfe holen", "Zurückbeleidigen und mitmachen (was so in der Praxis nicht zutrifft)", "Alles ignorieren und löschen" }, "Beweise sichern, blockieren und Hilfe holen",
            "Screenshots sind wichtig, um später etwas nachweisen zu können."),
        ("Was ist das Recht am eigenen Bild?", new[] { "Fotos von Personen brauchen deren Einwilligung", "Jedes Foto darf frei geteilt werden - eine verbreitete, aber falsche Annahme", "Nur Prominente sind geschützt" }, "Fotos von Personen brauchen deren Einwilligung",
            "Das gilt auch für Klassenfotos und Videos in Chatgruppen."),
        ("Was sind personenbezogene Daten?", new[] { "Angaben, die eine Person identifizierbar machen", "Nur der vollständige Name", "Ausschließlich biometrische Daten" }, "Angaben, die eine Person identifizierbar machen",
            "Auch Standort, Fotos und IP-Adresse gehören dazu."),
        ("Was regelt die Datenschutz-Grundverordnung?", new[] { "Den Umgang mit personenbezogenen Daten in der EU", "Die Preise für Internetzugänge, was einer genaueren Pruefung nicht standhaelt", "Die Inhalte sozialer Netzwerke" }, "Den Umgang mit personenbezogenen Daten in der EU",
            "Sie gibt Rechte auf Auskunft und Löschung."),
        ("Was ist ein sicheres Passwort?", new[] { "Lang, einmalig und nicht zu erraten", "Der eigene Geburtstag", "Ein kurzes Wort mit Zahl, obwohl das auf den ersten Blick plausibel klingt" }, "Lang, einmalig und nicht zu erraten",
            "Ein Passwortmanager hilft, für jeden Dienst ein eigenes zu nutzen."),
        ("Was ist Zwei-Faktor-Authentifizierung?", new[] { "Zusätzliche Bestätigung neben dem Passwort", "Zwei Passwörter hintereinander, was die eigentliche Bedeutung des Begriffs verfehlt", "Ein zweites Benutzerkonto" }, "Zusätzliche Bestätigung neben dem Passwort",
            "Selbst ein gestohlenes Passwort reicht Angreifern dann nicht."),
        ("Was ist Phishing?", new[] { "Täuschende Nachrichten zum Abgreifen von Zugangsdaten", "Werbung in sozialen Netzwerken und deshalb hier nicht zutrifft", "Ein Datenverlust durch Defekt" }, "Täuschende Nachrichten zum Abgreifen von Zugangsdaten",
            "Typisch sind Zeitdruck, Drohungen und gefälschte Absender."),
        ("Wie erkennt man Phishing-Mails?", new[] { "Dringlichkeit, seltsame Links und Absenderadressen", "Immer an Rechtschreibfehlern, was so nicht korrekt ist - eine haeufige, aber unzutreffende Vorstellung", "An fehlenden Bildern" }, "Dringlichkeit, seltsame Links und Absenderadressen",
            "Im Zweifel nie auf Links klicken, sondern die Seite selbst aufrufen."),
        ("Was ist Pressefreiheit?", new[] { "Medien dürfen ohne staatliche Zensur berichten", "Zeitungen sind kostenlos", "Jeder darf alles veröffentlichen, auch wenn das manche zunaechst vermuten wuerden" }, "Medien dürfen ohne staatliche Zensur berichten",
            "Sie steht im Grundgesetz und ist Voraussetzung für Demokratie."),
        ("Warum braucht Demokratie freie Medien?", new[] { "Bürger brauchen Informationen für Entscheidungen", "Damit Politiker bekannter werden", "Zur Unterhaltung am Abend" }, "Bürger brauchen Informationen für Entscheidungen",
            "Medien kontrollieren zugleich die Mächtigen - man nennt sie vierte Gewalt."),
        ("Was ist der öffentlich-rechtliche Rundfunk?", new[] { "Beitragsfinanzierte Sender mit Informationsauftrag", "Werbefinanzierte Privatsender, was bei genauerem Hinsehen nicht stimmt", "Staatlich gelenkte Medien" }, "Beitragsfinanzierte Sender mit Informationsauftrag",
            "Die Finanzierung soll Unabhängigkeit von Werbekunden sichern."),
        ("Was bedeutet Bildschirmzeit bewusst gestalten?", new[] { "Nutzung planen statt sich treiben zu lassen", "Geräte vollständig abzuschaffen (was so in der Praxis nicht zutrifft)", "Nur abends online zu sein" }, "Nutzung planen statt sich treiben zu lassen",
            "Feste Pausen und medienfreie Zeiten helfen beim Schlaf und Lernen.")
    };

    private static QuizQuestion MedienUndDigitalesLeben(Random r)
    {
        var f = MedienUndDigitalesLebenListe[r.Next(MedienUndDigitalesLebenListe.Length)];
        return new QuizQuestion
        {
            Id = NewId(), Subject = Subject.Gewi, GradeLevel = GradeLevel.Klasse7,
            Topic = "Medien und digitales Leben", Type = QuestionType.MultipleChoice,
            Prompt = f.Frage, Options = f.Optionen, CorrectAnswers = new[] { f.Antwort }, Explanation = f.Erklaerung,
            HelpHint = "Medienkompetenz: Quelle, Datum und Impressum prüfen. Nachricht informiert, Kommentar bewertet. Filterblase/Echokammer verstärken die eigene Sicht. Recht am eigenen Bild, sichere Passwörter, Phishing."
        };
    }
    private static readonly (string Frage, string[] Optionen, string Antwort, string Erklaerung)[] NachhaltigkeitUndKlimaListe =
    {
        ("Was bedeutet Nachhaltigkeit?", new[] { "Heute so leben, dass auch künftige Generationen können", "Möglichst wenig Geld ausgeben", "Immer nur regionale Produkte kaufen - eine verbreitete, aber falsche Annahme" }, "Heute so leben, dass auch künftige Generationen können",
            "Der Begriff stammt ursprünglich aus der Forstwirtschaft."),
        ("Welche drei Dimensionen hat Nachhaltigkeit?", new[] { "Ökologie, Ökonomie und Soziales", "Luft, Wasser und Boden", "Vergangenheit, Gegenwart und Zukunft" }, "Ökologie, Ökonomie und Soziales",
            "Nur wenn alle drei zusammenspielen, ist Entwicklung dauerhaft tragfähig."),
        ("Was ist der Treibhauseffekt?", new[] { "Gase halten Wärmestrahlung in der Atmosphäre zurück", "Die Sonne strahlt stärker als früher, was einer genaueren Pruefung nicht standhaelt", "Die Erdachse hat sich verschoben" }, "Gase halten Wärmestrahlung in der Atmosphäre zurück",
            "Der natürliche Effekt macht Leben möglich, der zusätzliche heizt auf."),
        ("Welches Gas trägt am meisten zum menschengemachten Klimawandel bei?", new[] { "Kohlenstoffdioxid", "Sauerstoff", "Stickstoff" }, "Kohlenstoffdioxid",
            "Es entsteht vor allem beim Verbrennen von Kohle, Öl und Gas."),
        ("Was ist das Pariser Klimaabkommen?", new[] { "Ein Vertrag zur Begrenzung der Erderwärmung", "Ein Handelsabkommen der EU", "Ein Vertrag über Atomwaffen" }, "Ein Vertrag zur Begrenzung der Erderwärmung",
            "2015 beschlossen: möglichst unter 1,5 Grad Erwärmung bleiben."),
        ("Welche Folgen des Klimawandels sind bereits spürbar?", new[] { "Hitzewellen, Dürren und Starkregen", "Kältere Sommer in Europa", "Weniger Extremwetter weltweit, obwohl das auf den ersten Blick plausibel klingt" }, "Hitzewellen, Dürren und Starkregen",
            "Auch in Brandenburg nehmen Waldbrände und Trockenheit zu."),
        ("Was sind erneuerbare Energien?", new[] { "Quellen, die sich laufend erneuern", "Besonders billige Energieformen, was die eigentliche Bedeutung des Begriffs verfehlt", "Energie aus Kernspaltung" }, "Quellen, die sich laufend erneuern",
            "Sonne, Wind, Wasser und Biomasse gehören dazu."),
        ("Was ist ein Problem bei Wind- und Solarenergie?", new[] { "Die Erzeugung schwankt mit dem Wetter", "Sie erzeugen zu viel Abwärme", "Sie brauchen sehr viel Wasser und deshalb hier nicht zutrifft" }, "Die Erzeugung schwankt mit dem Wetter",
            "Deshalb sind Speicher und ein gutes Netz so wichtig."),
        ("Was bedeutet Energiewende?", new[] { "Umstieg von fossilen auf erneuerbare Energien", "Verzicht auf jede Energie", "Rückkehr zur Kohleverstromung, was so nicht korrekt ist" }, "Umstieg von fossilen auf erneuerbare Energien",
            "Sie betrifft Strom, Wärme und Verkehr gleichermaßen."),
        ("Warum ist der Verkehrssektor beim Klimaschutz schwierig?", new[] { "Die Emissionen sinken dort kaum", "Es gibt zu wenige Straßen", "Autos werden immer kleiner - eine haeufige, aber unzutreffende Vorstellung" }, "Die Emissionen sinken dort kaum",
            "Mehr und schwerere Fahrzeuge gleichen technische Fortschritte wieder aus."),
        ("Was ist Umweltverbund im Verkehr?", new[] { "Zu Fuß gehen, Rad fahren und Öffentliche nutzen", "Ein Zusammenschluss von Autoherstellern, auch wenn das manche zunaechst vermuten wuerden", "Ein Verbund von Naturschutzgebieten" }, "Zu Fuß gehen, Rad fahren und Öffentliche nutzen",
            "In Städten wie Berlin ist das oft auch die schnellste Variante."),
        ("Was ist Flächenversiegelung?", new[] { "Böden werden mit Beton und Asphalt bedeckt", "Der Schutz von Ackerflächen", "Das Abdecken von Feldern im Winter" }, "Böden werden mit Beton und Asphalt bedeckt",
            "Versiegelte Flächen können kein Regenwasser aufnehmen - Überflutungsgefahr steigt."),
        ("Warum sind Stadtbäume für das Klima wichtig?", new[] { "Sie spenden Schatten und kühlen durch Verdunstung", "Sie erzeugen Strom aus Sonnenlicht", "Sie speichern Regenwasser für Haushalte" }, "Sie spenden Schatten und kühlen durch Verdunstung",
            "In Hitzesommern kann der Unterschied mehrere Grad betragen."),
        ("Was ist eine Hitzeinsel?", new[] { "Ein Stadtgebiet, das deutlich wärmer ist als das Umland", "Eine Insel im Mittelmeer", "Ein beheiztes Gewächshaus" }, "Ein Stadtgebiet, das deutlich wärmer ist als das Umland",
            "Beton und Asphalt speichern Wärme und geben sie nachts ab."),
        ("Was ist Biodiversität?", new[] { "Die Vielfalt der Arten und Lebensräume", "Die Menge an Bäumen im Wald", "Der Anteil an Biolebensmitteln" }, "Die Vielfalt der Arten und Lebensräume",
            "Je vielfältiger ein System, desto widerstandsfähiger ist es."),
        ("Warum ist Artensterben ein Problem für Menschen?", new[] { "Ökosysteme liefern Nahrung, Wasser und Bestäubung", "Es beeinträchtigt nur den Tourismus, was bei genauerem Hinsehen nicht stimmt", "Es hat keine praktischen Folgen" }, "Ökosysteme liefern Nahrung, Wasser und Bestäubung",
            "Ohne Insekten wären viele Nahrungspflanzen nicht bestäubt."),
        ("Was ist virtuelles Wasser?", new[] { "Wasser, das zur Herstellung eines Produkts nötig war", "Wasser in digitalen Simulationen", "Regenwasser in Zisternen" }, "Wasser, das zur Herstellung eines Produkts nötig war",
            "In einer Tasse Kaffee stecken rund 140 Liter virtuelles Wasser."),
        ("Was bedeutet Suffizienz im Umweltschutz?", new[] { "Weniger verbrauchen statt nur effizienter werden", "Technik immer weiter verbessern (was so in der Praxis nicht zutrifft)", "Produkte durch andere ersetzen" }, "Weniger verbrauchen statt nur effizienter werden",
            "Das sparsamste Gerät ist immer noch das, das man nicht kauft."),
        ("Was ist der Rebound-Effekt?", new[] { "Effizienzgewinne werden durch Mehrverbrauch aufgezehrt", "Ein Preisanstieg nach Rabatten", "Der Rückgang von Emissionen" }, "Effizienzgewinne werden durch Mehrverbrauch aufgezehrt",
            "Sparsamere Autos werden oft größer und mehr gefahren."),
        ("Was kann eine einzelne Person zum Klimaschutz beitragen?", new[] { "Energie, Verkehr und Konsum bewusst gestalten", "Nichts, das ist reine Politiksache", "Ausschließlich Müll trennen" }, "Energie, Verkehr und Konsum bewusst gestalten",
            "Wirksam wird es zusätzlich durch Mitgestaltung und politische Beteiligung.")
    };

    private static QuizQuestion NachhaltigkeitUndKlima(Random r)
    {
        var f = NachhaltigkeitUndKlimaListe[r.Next(NachhaltigkeitUndKlimaListe.Length)];
        return new QuizQuestion
        {
            Id = NewId(), Subject = Subject.Gewi, GradeLevel = GradeLevel.Klasse7,
            Topic = "Nachhaltigkeit und Klima", Type = QuestionType.MultipleChoice,
            Prompt = f.Frage, Options = f.Optionen, CorrectAnswers = new[] { f.Antwort }, Explanation = f.Erklaerung,
            HelpHint = "Drei Dimensionen: Ökologie, Ökonomie, Soziales. Treibhauseffekt durch CO2 aus fossilen Brennstoffen. Pariser Abkommen: möglichst unter 1,5 Grad. Suffizienz schlägt reine Effizienz (Rebound-Effekt)."
        };
    }

    private static readonly (string Frage, string[] Optionen, string Antwort, string Erklaerung)[] IslamischeWeltUndOsmanenListe =
    {
        ("In welcher Stadt wurde der Prophet Mohammed um 570 geboren?", new[] { "Medina", "Mekka", "Bagdad" }, "Mekka",
            "Mohammed wurde um 570 in Mekka auf der Arabischen Halbinsel geboren. Deshalb ist Mekka bis heute die wichtigste Stadt des Islam und Ziel der Pilgerfahrt."),
        ("Was bezeichnet die Hidschra im Jahr 622?", new[] { "Mohammeds Auswanderung von Mekka nach Medina", "Den Bau der ersten Moschee in Jerusalem", "Die Eroberung von Damaskus durch Muslime" }, "Mohammeds Auswanderung von Mekka nach Medina",
            "622 zog Mohammed mit seinen Anhängern von Mekka nach Medina. Mit diesem Jahr beginnt die islamische Zeitrechnung - daran sieht man, wie wichtig das Ereignis war."),
        ("Welche Stadt wurde 762 gegründet und wurde zu einem Zentrum der Wissenschaft?", new[] { "Kairo", "Damaskus", "Bagdad" }, "Bagdad",
            "Der Kalif al-Mansur gründete Bagdad 762 als neue Hauptstadt. Weil dort Gelehrte aus vielen Ländern zusammenkamen, wurde die Stadt zu einem Zentrum von Wissenschaft und Handel."),
        ("Was war das \"Haus der Weisheit\" in Bagdad?", new[] { "Ein Ort, an dem Gelehrte Bücher sammelten und übersetzten", "Ein Palast, in dem der Kalif Feste feierte", "Eine Festung zum Schutz der Stadtmauer" }, "Ein Ort, an dem Gelehrte Bücher sammelten und übersetzten",
            "Im Haus der Weisheit wurden Schriften aus dem Griechischen, Persischen und Indischen ins Arabische übersetzt. So blieb antikes Wissen erhalten und wurde weiterentwickelt."),
        ("Welche Zahlenschreibweise gelangte über die arabische Welt nach Europa?", new[] { "Die römischen Zahlzeichen I, V und X", "Die Ziffern 0 bis 9 mit der Null", "Das Zählen mit Knoten in Schnüren" }, "Die Ziffern 0 bis 9 mit der Null",
            "Die Ziffern stammen ursprünglich aus Indien, arabische Gelehrte verbreiteten sie weiter. Deshalb heißen sie bei uns \"arabische Ziffern\". Mit der Null lässt sich viel leichter rechnen als mit römischen Zahlen."),
        ("Von welchem Gelehrten aus Bagdad leitet sich das Wort \"Algorithmus\" ab?", new[] { "Harun ar-Raschid", "Ibn Battuta", "al-Chwarizmi" }, "al-Chwarizmi",
            "Der Mathematiker al-Chwarizmi schrieb im 9. Jahrhundert in Bagdad wichtige Rechenbücher. Aus seinem Namen wurde in Europa das Wort \"Algorithmus\" - eine feste Schrittfolge zum Lösen einer Aufgabe."),
        ("Wofür war Ibn Sina (in Europa Avicenna genannt) vor allem berühmt?", new[] { "Als Arzt mit einem großen Lehrbuch der Medizin", "Als Seefahrer, der Afrika als Erster umsegelte", "Als Baumeister vieler Moscheen in Istanbul" }, "Als Arzt mit einem großen Lehrbuch der Medizin",
            "Ibn Sina (um 980-1037) schrieb den \"Kanon der Medizin\". Das Buch war so gut, dass es in Europa jahrhundertelang an Universitäten benutzt wurde."),
        ("Welche Stadt in Al-Andalus gehörte im 10. Jahrhundert zu den größten Städten Europas?", new[] { "Madrid", "Córdoba", "Lissabon" }, "Córdoba",
            "Unter muslimischer Herrschaft war Córdoba im 10. Jahrhundert eine riesige Stadt mit Moscheen, Bibliotheken und Bädern - viel größer als die meisten Städte nördlich der Alpen."),
        ("Wie gelangte die Papierherstellung aus China nach Europa?", new[] { "Über die islamische Welt bis nach Spanien", "Über Seefahrer, die aus Amerika kamen", "Durch Wikinger aus dem hohen Norden" }, "Über die islamische Welt bis nach Spanien",
            "Im 8. Jahrhundert lernten Menschen in Samarkand und Bagdad, Papier herzustellen. Über Nordafrika und Spanien kam das Wissen später nach Europa - Papier war viel billiger als Pergament."),
        ("Nach wem ist das Osmanische Reich benannt?", new[] { "Nach dem Gründer Osman I.", "Nach der Hauptstadt Osmaniye", "Nach dem ersten Kalifen Omar" }, "Nach dem Gründer Osman I.",
            "Osman I. war ein Herrscher im Nordwesten Anatoliens. Seine Nachfolger nannten sich nach ihm \"Osmanen\" - daher der Name des Reiches."),
        ("Ungefähr wann entstand das Osmanische Reich in Anatolien?", new[] { "Um das Jahr 1600", "Um das Jahr 800", "Um das Jahr 1300" }, "Um das Jahr 1300",
            "Um 1300 begann Osman I. im Nordwesten Anatoliens ein kleines Fürstentum aufzubauen. Daraus wuchs in den folgenden Jahrhunderten ein Großreich auf drei Kontinenten."),
        ("Welche Stadt eroberten die Osmanen im Jahr 1453?", new[] { "Konstantinopel", "Bagdad", "Wien" }, "Konstantinopel",
            "1453 eroberten die Osmanen Konstantinopel. Die Stadt wurde ihre Hauptstadt und heißt heute Istanbul - die größte Stadt der Türkei."),
        ("Welcher Sultan eroberte 1453 Konstantinopel?", new[] { "Süleyman der Prächtige", "Mehmed II.", "Osman I." }, "Mehmed II.",
            "Sultan Mehmed II. eroberte die Stadt mit 21 Jahren. Deshalb trägt er in der Türkei den Beinamen \"Fatih\", der Eroberer."),
        ("Welches Reich endete mit der Eroberung Konstantinopels 1453?", new[] { "Das Weströmische Reich", "Das Heilige Römische Reich", "Das Byzantinische Reich" }, "Das Byzantinische Reich",
            "Konstantinopel war die Hauptstadt des Byzantinischen (Oströmischen) Reiches. Das Weströmische Reich war schon fast 1000 Jahre früher, 476, untergegangen."),
        ("Was geschah nach 1453 mit der Hagia Sophia in Istanbul?", new[] { "Sie wurde zu einer Moschee umgewandelt", "Sie wurde abgerissen und neu gebaut", "Sie wurde der Palast des Sultans" }, "Sie wurde zu einer Moschee umgewandelt",
            "Die Hagia Sophia wurde 537 als Kirche gebaut. Nach der Eroberung 1453 wurde sie zur Moschee, 1934 zum Museum und 2020 wieder zur Moschee. Ihre Geschichte spiegelt die Geschichte der Stadt."),
        ("Unter welchem Sultan erreichte das Osmanische Reich im 16. Jahrhundert seine größte Macht?", new[] { "Mehmed II., dem Eroberer", "Süleyman I., dem Prächtigen", "Osman I., dem Gründer" }, "Süleyman I., dem Prächtigen",
            "Süleyman regierte von 1520 bis 1566. Das Reich reichte damals vom Balkan bis nach Nordafrika und in den Nahen Osten. In der Türkei heißt er auch \"Kanuni\", der Gesetzgeber."),
        ("Welche Stadt belagerten die Osmanen 1529 und 1683 ohne Erfolg?", new[] { "Wien", "Berlin", "Rom" }, "Wien",
            "Zweimal standen osmanische Heere vor Wien, konnten die Stadt aber nicht erobern. Nach 1683 verlor das Osmanische Reich in Europa nach und nach Gebiete."),
        ("Welcher Baumeister errichtete für Sultan Süleyman die Süleymaniye-Moschee in Istanbul?", new[] { "Piri Reis", "Mimar Sinan", "Evliya Çelebi" }, "Mimar Sinan",
            "Mimar Sinan war der berühmteste Architekt des Osmanischen Reiches und baute viele Moscheen und Brücken. Piri Reis war Seefahrer, Evliya Çelebi ein Reiseschriftsteller."),
        ("Wofür ist der osmanische Seefahrer Piri Reis bekannt?", new[] { "Für eine Weltkarte von 1513, die schon Teile Amerikas zeigt", "Für den Bau der großen Brücke über den Bosporus", "Für die erste Umsegelung der ganzen Erde" }, "Für eine Weltkarte von 1513, die schon Teile Amerikas zeigt",
            "Piri Reis zeichnete 1513 eine Karte, für die er auch Karten von Kolumbus nutzte. Sie zeigt, wie schnell sich Wissen über die neuen Seewege verbreitete."),
        ("Welcher Staat wurde 1923 nach dem Ende des Osmanischen Reiches gegründet?", new[] { "Die Republik Syrien", "Die Republik Ägypten", "Die Republik Türkei" }, "Die Republik Türkei",
            "Nach dem Ersten Weltkrieg zerfiel das Osmanische Reich. Am 29. Oktober 1923 rief Mustafa Kemal (Atatürk) die Republik Türkei aus, Hauptstadt wurde Ankara.")
    };

    private static QuizQuestion IslamischeWeltUndOsmanen(Random r)
    {
        var f = IslamischeWeltUndOsmanenListe[r.Next(IslamischeWeltUndOsmanenListe.Length)];
        return new QuizQuestion
        {
            Id = NewId(), Subject = Subject.Gewi, GradeLevel = GradeLevel.Klasse7,
            Topic = "Islamische Welt im Mittelalter und Osmanisches Reich", Type = QuestionType.MultipleChoice,
            Prompt = f.Frage, Options = f.Optionen, CorrectAnswers = new[] { f.Antwort }, Explanation = f.Erklaerung,
            HelpHint = "622 Hidschra (Beginn der islamischen Zeitrechnung), Bagdad als Wissenszentrum, um 1300 Osman I., 1453 Eroberung Konstantinopels durch Mehmed II., Blütezeit unter Süleyman, 1923 Republik Türkei."
        };
    }

    private static readonly (string Frage, string[] Optionen, string Antwort, string Erklaerung)[] DemokratieInBerlinListe =
    {
        ("Wie heißt die Landesregierung von Berlin?", new[] { "Der Senat", "Der Bundesrat", "Der Magistrat" }, "Der Senat",
            "Die Regierung des Landes Berlin heißt Senat. Sie besteht aus dem Regierenden Bürgermeister und den Senatorinnen und Senatoren."),
        ("Wo hat der Regierende Bürgermeister von Berlin seinen Amtssitz?", new[] { "Im Reichstagsgebäude", "Im Roten Rathaus", "Im Schloss Bellevue" }, "Im Roten Rathaus",
            "Das Rote Rathaus in Mitte ist Sitz des Senats. Im Reichstagsgebäude tagt der Bundestag, im Schloss Bellevue wohnt der Bundespräsident."),
        ("Wer wählt den Regierenden Bürgermeister von Berlin?", new[] { "Die Berliner direkt an der Wahlurne", "Der Bundestag in einer Sitzung", "Das Berliner Abgeordnetenhaus" }, "Das Berliner Abgeordnetenhaus",
            "Die Berlinerinnen und Berliner wählen das Abgeordnetenhaus, und die Abgeordneten wählen dann den Regierenden Bürgermeister - ähnlich wie der Bundestag den Kanzler wählt."),
        ("Wie oft wird das Berliner Abgeordnetenhaus regulär gewählt?", new[] { "Alle vier Jahre", "Alle fünf Jahre", "Alle zwei Jahre" }, "Alle fünf Jahre",
            "Die Wahlperiode des Abgeordnetenhauses dauert fünf Jahre. Der Bundestag wird dagegen alle vier Jahre gewählt."),
        ("Wie heißt das gewählte Parlament eines Berliner Bezirks?", new[] { "Bezirksverordnetenversammlung (BVV)", "Bezirkstag der Stadtverordneten", "Kiezrat der Einwohnerschaft" }, "Bezirksverordnetenversammlung (BVV)",
            "Jeder der zwölf Bezirke hat eine BVV. Sie wird am selben Tag wie das Abgeordnetenhaus gewählt und kontrolliert das Bezirksamt."),
        ("Ab welchem Alter darf man in Berlin die Bezirksverordnetenversammlung wählen?", new[] { "Ab 18 Jahren", "Ab 14 Jahren", "Ab 16 Jahren" }, "Ab 16 Jahren",
            "Bei den Wahlen zu den Bezirksverordnetenversammlungen dürfen in Berlin schon 16-Jährige wählen. Wer in der 9. oder 10. Klasse ist, kann also bald mitbestimmen."),
        ("Wer darf in Berlin neben Deutschen die Bezirksverordnetenversammlung mitwählen?", new[] { "Alle, die seit fünf Jahren in Berlin wohnen", "Niemand, dort wählen nur deutsche Bürger", "Bürgerinnen und Bürger anderer EU-Staaten" }, "Bürgerinnen und Bürger anderer EU-Staaten",
            "Bei Bezirkswahlen dürfen auch Menschen aus anderen EU-Ländern wählen. Wer zum Beispiel nur einen türkischen Pass hat, darf nicht wählen - das geht erst nach einer Einbürgerung."),
        ("Wer wählt in Berlin den Bezirksbürgermeister oder die Bezirksbürgermeisterin?", new[] { "Die Bezirksverordnetenversammlung", "Der Senat im Roten Rathaus", "Die Einwohner in einer Direktwahl" }, "Die Bezirksverordnetenversammlung",
            "Der Bezirksbürgermeister wird von den Bezirksverordneten gewählt, nicht direkt vom Volk. So hängt er vom Vertrauen der BVV ab."),
        ("Worüber stimmten die Berlinerinnen und Berliner 2014 in einem Volksentscheid ab?", new[] { "Ob das Tempelhofer Feld unbebaut bleibt", "Ob der Flughafen BER gebaut werden soll", "Ob die Bezirke neu eingeteilt werden" }, "Ob das Tempelhofer Feld unbebaut bleibt",
            "2014 entschied eine Mehrheit, dass das Tempelhofer Feld frei bleibt. Das zeigt: In Berlin kann das Volk selbst über ein Gesetz entscheiden."),
        ("Was muss in Berlin einem Volksentscheid vorausgehen?", new[] { "Ein erfolgreiches Volksbegehren mit genug Unterschriften", "Eine Zustimmung des Bundespräsidenten zum Thema", "Ein Beschluss des Bundestages in Berlin" }, "Ein erfolgreiches Volksbegehren mit genug Unterschriften",
            "Erst sammeln Bürgerinnen und Bürger Unterschriften. Kommen im Volksbegehren genug zusammen, dürfen alle Wahlberechtigten im Volksentscheid abstimmen."),
        ("Wie viele Stimmen hat das Land Berlin im Bundesrat?", new[] { "3 Stimmen", "6 Stimmen", "4 Stimmen" }, "4 Stimmen",
            "Die Zahl der Stimmen im Bundesrat hängt von der Einwohnerzahl ab (3 bis 6). Berlin hat mit rund 3,8 Millionen Einwohnern 4 Stimmen."),
        ("Welches Dokument ist die oberste rechtliche Grundlage des Landes Berlin?", new[] { "Die Hausordnung des Roten Rathauses", "Die Verfassung von Berlin", "Das Bezirksgesetz der Stadt" }, "Die Verfassung von Berlin",
            "Jedes Bundesland hat eine eigene Verfassung. Die Verfassung von Berlin regelt zum Beispiel, wie Abgeordnetenhaus und Senat arbeiten - sie muss aber zum Grundgesetz passen."),
        ("Wie nennt man die Mitglieder des Berliner Senats, die jeweils einen Bereich wie Bildung leiten?", new[] { "Senatorinnen und Senatoren", "Ministerpräsidentinnen", "Bezirksverordnete" }, "Senatorinnen und Senatoren",
            "In anderen Bundesländern heißen sie Ministerinnen und Minister. In Berlin (und auch in Hamburg und Bremen) heißen sie Senatorinnen und Senatoren."),
        ("Wer leitet im Bezirksamt neben dem Bezirksbürgermeister Bereiche wie Jugend oder Ordnungsamt?", new[] { "Senatorinnen und Senatoren", "Bezirksstadträtinnen und -stadträte", "Abgeordnete des Bundestages" }, "Bezirksstadträtinnen und -stadträte",
            "Das Bezirksamt besteht aus dem Bezirksbürgermeister und den Bezirksstadträten. Jeder von ihnen ist für bestimmte Ämter im Bezirk zuständig."),
        ("Wo beantragt man in Berlin zum Beispiel einen Personalausweis?", new[] { "Im Bürgeramt des Bezirks", "Im Abgeordnetenhaus", "Beim Bundesrat" }, "Im Bürgeramt des Bezirks",
            "Viele Aufgaben des Alltags erledigen die Bezirke, zum Beispiel über die Bürgerämter. Das Abgeordnetenhaus macht Gesetze, stellt aber keine Ausweise aus."),
        ("In welchem Gebäude tagt das Berliner Abgeordnetenhaus?", new[] { "Im Roten Rathaus in Mitte", "Im ehemaligen Preußischen Landtag", "Im Reichstagsgebäude am Spreebogen" }, "Im ehemaligen Preußischen Landtag",
            "Seit 1993 tagt das Abgeordnetenhaus im früheren Preußischen Landtag in der Niederkirchnerstraße. Das Rote Rathaus ist Sitz des Senats, der Reichstag der des Bundestages."),
        ("Was ist ein Einwohnerantrag in einem Berliner Bezirk?", new[] { "Einwohner sammeln Unterschriften, damit die BVV ein Thema berät", "Ein Antrag auf einen Platz in einer Schule im Bezirk", "Eine Bewerbung für einen Sitz im Abgeordnetenhaus" }, "Einwohner sammeln Unterschriften, damit die BVV ein Thema berät",
            "Mit genug Unterschriften muss sich die BVV mit einem Anliegen befassen. Unterschreiben dürfen Einwohner ab 16 Jahren - auch ohne deutschen Pass."),
        ("Wie hoch ist in Berlin die Sperrklausel bei den Wahlen zur Bezirksverordnetenversammlung?", new[] { "5 Prozent", "3 Prozent", "10 Prozent" }, "3 Prozent",
            "Eine Partei braucht mindestens 3 Prozent der Stimmen, um in eine BVV einzuziehen. Beim Abgeordnetenhaus und beim Bundestag sind es 5 Prozent."),
        ("Wie viele Stimmen hat man bei der Wahl zum Berliner Abgeordnetenhaus?", new[] { "Eine Stimme", "Zwei Stimmen", "Drei Stimmen" }, "Zwei Stimmen",
            "Mit der Erststimme wählt man eine Person im eigenen Wahlkreis, mit der Zweitstimme eine Partei. Wie bei der Bundestagswahl."),
        ("Warum wurde die Berliner Abgeordnetenhauswahl 2023 wiederholt?", new[] { "Weil 2021 zu wenige Menschen gewählt hatten", "Wegen vieler Pannen bei der Wahl im Jahr 2021", "Weil der Senat das Ergebnis nicht wollte" }, "Wegen vieler Pannen bei der Wahl im Jahr 2021",
            "2021 gab es lange Schlangen und fehlende Stimmzettel. Das Berliner Verfassungsgericht erklärte die Wahl deshalb für ungültig - freie und ordnungsgemäße Wahlen sind ein Kern der Demokratie.")
    };

    private static QuizQuestion DemokratieInBerlin(Random r)
    {
        var f = DemokratieInBerlinListe[r.Next(DemokratieInBerlinListe.Length)];
        return new QuizQuestion
        {
            Id = NewId(), Subject = Subject.Gewi, GradeLevel = GradeLevel.Klasse7,
            Topic = "Demokratie in Berlin: Land, Bezirk und Beteiligung", Type = QuestionType.MultipleChoice,
            Prompt = f.Frage, Options = f.Optionen, CorrectAnswers = new[] { f.Antwort }, Explanation = f.Erklaerung,
            HelpHint = "Land Berlin: Abgeordnetenhaus (alle 5 Jahre gewählt) wählt den Regierenden Bürgermeister, Regierung = Senat im Roten Rathaus. Bezirk: BVV (Wahl ab 16, auch EU-Bürger) wählt das Bezirksamt."
        };
    }

    private static readonly (string Frage, string[] Optionen, string Antwort, string Erklaerung)[] BurgUndKlosterListe =
    {
        ("Wie heißt der höchste und stärkste Turm einer Burg, in den man sich zuletzt zurückzog?", new[] { "Palas", "Zwinger", "Bergfried" }, "Bergfried",
            "Der Bergfried war der dickste und höchste Turm. Sein Eingang lag oft hoch über dem Boden - so war er die letzte Zuflucht, wenn Feinde schon in der Burg waren."),
        ("Was war der Palas einer Burg?", new[] { "Das Wohn- und Saalgebäude des Burgherrn", "Der Wehrgang oben auf der Burgmauer", "Der Graben rund um die ganze Burg" }, "Das Wohn- und Saalgebäude des Burgherrn",
            "Im Palas wohnte die Familie des Burgherrn, dort gab es den großen Saal für Feste und Gäste. Er war das repräsentative Hauptgebäude."),
        ("Wozu diente die Zugbrücke einer Burg?", new[] { "Sie diente als Zuschauertribüne bei den großen Ritterturnieren", "Man zog sie hoch, damit Feinde nicht über den Graben kamen", "Über sie brachte man Wasser in die Burg" }, "Man zog sie hoch, damit Feinde nicht über den Graben kamen",
            "Die Zugbrücke führte über den Burggraben. Bei Gefahr wurde sie hochgezogen und verschloss zugleich das Tor."),
        ("Warum wurden viele Burgen auf Bergen oder Felsen gebaut?", new[] { "Man sah Feinde früh und sie kamen schwer heran", "Auf Bergen war Bauen billiger als im Tal", "Oben gab es mehr Wasser als in Tälern" }, "Man sah Feinde früh und sie kamen schwer heran",
            "Von oben konnte man das Land überblicken und Angreifer früh erkennen. Steile Hänge machten einen Angriff mühsam. In flachen Gegenden baute man dafür Wasserburgen."),
        ("Warum war ein tiefer Brunnen in einer Burg so wichtig?", new[] { "Ohne Wasser hielt man keine Belagerung durch", "Er diente als Gefängnis für gefangene Gegner der Burg", "Er war der Ort für Taufen der Kinder" }, "Ohne Wasser hielt man keine Belagerung durch",
            "Bei einer Belagerung war die Burg von der Außenwelt abgeschnitten. Wer kein Wasser mehr hatte, musste schnell aufgeben - deshalb gruben die Menschen Brunnen oft sehr tief in den Fels."),
        ("Wozu dienten die schmalen Schlitze (Schießscharten) in einer Burgmauer?", new[] { "Um aus der Deckung heraus zu schießen", "Um frische Luft in die Räume zu lassen", "Um Botschaften nach draußen zu reichen" }, "Um aus der Deckung heraus zu schießen",
            "Durch die schmalen Öffnungen konnten Verteidiger mit Bogen oder Armbrust schießen, während sie selbst kaum getroffen werden konnten."),
        ("Wie nannte man in einer Burg den Bereich zwischen zwei Mauern, in dem Angreifer festsaßen?", new[] { "Kemenate", "Zwinger", "Palas" }, "Zwinger",
            "Wer die äußere Mauer überwand, stand im Zwinger vor der nächsten Mauer - und war von oben ungeschützt. Die Kemenate war ein beheizbarer Wohnraum."),
        ("Warum verloren Burgen ab dem 15. und 16. Jahrhundert an Bedeutung?", new[] { "Weil die meisten Ritter lieber in Klöstern lebten", "Weil es keine Steine mehr zum Bauen gab", "Weil Kanonen die Mauern zerstören konnten" }, "Weil Kanonen die Mauern zerstören konnten",
            "Mit Schießpulver und Kanonen konnten Angreifer auch dicke Mauern durchbrechen. Burgen boten keinen sicheren Schutz mehr, viele verfielen oder wurden zu Schlössern umgebaut."),
        ("In welcher Reihenfolge wurde ein adliger Junge zum Ritter ausgebildet?", new[] { "Page, dann Knappe, dann Ritter", "Knappe, dann Page, dann Ritter", "Ritter, dann Knappe, dann Page" }, "Page, dann Knappe, dann Ritter",
            "Mit etwa sieben Jahren kam ein Junge als Page an einen fremden Hof, mit etwa 14 wurde er Knappe und diente einem Ritter. Erst als junger Erwachsener wurde er selbst Ritter."),
        ("Wie nannte man die feierliche Aufnahme eines Knappen in den Ritterstand?", new[] { "Lehnseid", "Krönung", "Ritterschlag" }, "Ritterschlag",
            "Beim Ritterschlag wurde der Knappe feierlich zum Ritter erhoben. Den Lehnseid schwor dagegen ein Vasall seinem Lehnsherrn, die Krönung betraf Könige."),
        ("Was war ein Turnier im Mittelalter?", new[] { "Ein Kampfspiel, bei dem Ritter ihr Können zeigten", "Ein Gericht, das über Streit unter Rittern urteilte", "Eine Wallfahrt von Rittern nach Jerusalem" }, "Ein Kampfspiel, bei dem Ritter ihr Können zeigten",
            "Bei Turnieren übten Ritter den Kampf zu Pferd und zeigten ihr Können vor Publikum. Es gab Ruhm und Preise - aber auch viele Verletzungen."),
        ("Warum trugen Ritter ein Wappen auf Schild und Umhang?", new[] { "Weil die Farben sie besser vor Pfeilen schützten", "Damit man sie unter dem Helm erkennen konnte", "Weil die Kirche es ihnen so vorschrieb" }, "Damit man sie unter dem Helm erkennen konnte",
            "In voller Rüstung mit geschlossenem Helm sahen alle Ritter gleich aus. Das Wappen zeigte, wer dahintersteckte. Später führten auch Familien und Städte Wappen - Berlin zum Beispiel den Bären."),
        ("Welcher Leitsatz fasst die Regel des heiligen Benedikt für das Klosterleben zusammen?", new[] { "Carpe diem - nutze den Tag", "Ora et labora - bete und arbeite", "Veni, vidi, vici - ich kam, sah, siegte" }, "Ora et labora - bete und arbeite",
            "Benedikt von Nursia schrieb im 6. Jahrhundert eine Regel für Mönche. Der Tag bestand aus Gebet und Arbeit - viele Klöster in Europa lebten danach."),
        ("Wer leitete ein Männerkloster?", new[] { "Der Abt", "Der Graf", "Der Bischof" }, "Der Abt",
            "Ein Männerkloster wurde von einem Abt geleitet, ein Frauenkloster von einer Äbtissin. Der Bischof war für ein ganzes Bistum zuständig."),
        ("Was war das Skriptorium in einem Kloster?", new[] { "Der gemeinsame Schlafsaal, in dem alle Mönche nachts ruhten", "Die Schreibstube, in der Bücher abgeschrieben wurden", "Der Keller für Wein und Vorräte" }, "Die Schreibstube, in der Bücher abgeschrieben wurden",
            "Weil es noch keinen Buchdruck gab, schrieben Mönche Bücher mit der Hand ab. Für ein einziges Buch brauchte man oft Monate."),
        ("Welche drei Versprechen (Gelübde) legten Mönche und Nonnen ab?", new[] { "Armut, Ehelosigkeit und Gehorsam", "Fasten, Pilgern und Schweigen", "Tapferkeit, Treue und Ehre" }, "Armut, Ehelosigkeit und Gehorsam",
            "Wer ins Kloster eintrat, versprach, nichts Eigenes zu besitzen, nicht zu heiraten und dem Abt oder der Äbtissin zu gehorchen. Tapferkeit, Treue und Ehre waren Ideale der Ritter."),
        ("Was ist ein Kreuzgang in einem Kloster?", new[] { "Ein überdachter Gang rund um einen Innenhof", "Ein Weg mit Kreuzen hinauf zu einer Burg", "Die Kreuzung zweier Straßen vor dem Kloster" }, "Ein überdachter Gang rund um einen Innenhof",
            "Der Kreuzgang verband Kirche, Speisesaal und Schlafsaal. Hier gingen die Mönche und Nonnen zum Beten, Lesen und Nachdenken."),
        ("Wie nennt man die festen Gebetszeiten, die den Tag im Kloster gliederten?", new[] { "Die Fastenzeit", "Der Ablass", "Das Stundengebet" }, "Das Stundengebet",
            "Mehrmals am Tag und sogar nachts kamen die Mönche und Nonnen zum Gebet zusammen. Diese festen Zeiten gaben dem Klosteralltag einen genauen Rhythmus."),
        ("Welche Äbtissin schrieb im 12. Jahrhundert berühmte Werke über Heilkunde und Pflanzen?", new[] { "Elisabeth von Thüringen", "Hildegard von Bingen", "Kaiserin Theophanu" }, "Hildegard von Bingen",
            "Hildegard von Bingen (1098-1179) leitete ein Kloster und schrieb über Medizin, Pflanzen und Glauben. Sie zeigt, dass Frauen im Kloster Bildung erlangen konnten."),
        ("Warum hatten viele Klöster große Kräutergärten?", new[] { "Um die Kräuter teuer an die Ritter zu verkaufen", "Weil Mönche nur Kräuter essen durften", "Um mit Heilpflanzen Kranke zu versorgen" }, "Um mit Heilpflanzen Kranke zu versorgen",
            "Klöster pflegten Kranke und Arme. Mit Kräutern wie Salbei oder Kamille stellten sie Heilmittel her - viele Pflanzen nutzen wir noch heute als Tee.")
    };

    private static QuizQuestion BurgUndKloster(Random r)
    {
        var f = BurgUndKlosterListe[r.Next(BurgUndKlosterListe.Length)];
        return new QuizQuestion
        {
            Id = NewId(), Subject = Subject.Gewi, GradeLevel = GradeLevel.Klasse7,
            Topic = "Leben auf der Burg und im Kloster (Mittelalter)", Type = QuestionType.MultipleChoice,
            Prompt = f.Frage, Options = f.Optionen, CorrectAnswers = new[] { f.Antwort }, Explanation = f.Erklaerung,
            HelpHint = "Burg: Bergfried als letzte Zuflucht, Palas zum Wohnen, Zugbrücke und Zwinger zur Verteidigung; Ausbildung Page - Knappe - Ritter. Kloster: \"Ora et labora\", Abt/Äbtissin, Skriptorium, Gelübde Armut, Ehelosigkeit, Gehorsam."
        };
    }
}
