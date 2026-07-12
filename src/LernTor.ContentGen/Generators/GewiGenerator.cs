using LernTor.Core.Enums;
using LernTor.Core.Models;

namespace LernTor.ContentGen.Generators;

/// <summary>
/// Gesellschaftswissenschaften (Gewi) - kombiniert Geschichte/Erdkunde/Politik-Grundlagen,
/// wie im Berliner Rahmenlehrplan für die Grundschule/frühe Sekundarstufe üblich.
/// </summary>
public sealed class GewiGenerator : ExerciseGeneratorBase
{
    public override Subject Subject => Subject.Gewi;

    protected override IReadOnlyDictionary<GradeLevel, IReadOnlyList<TopicFactory>> TopicsByGrade { get; } =
        new Dictionary<GradeLevel, IReadOnlyList<TopicFactory>>
        {
            [GradeLevel.Klasse6] = new List<TopicFactory> { Epochen, Himmelsrichtungen, Kinderrechte, Ernaehrung, WasserAlsRessource, StadtUndVielfalt, EuropaGrenzenlos, TourismusUndMobilitaet, DemokratieUndMitbestimmung },
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
        ("Wie nannte man das gesellschaftliche System des Mittelalters, in dem Lehnsherren Land an Vasallen vergaben?", new[] { "Feudalismus (Lehnswesen)", "Demokratie", "Sozialismus" }, "Feudalismus (Lehnswesen)",
            "Im Feudalismus vergaben Lehnsherren Land gegen Treue- und Dienstpflichten an Vasallen - das prägte die mittelalterliche Gesellschaft."),
        ("Welche Erfindung von Johannes Gutenberg um 1450 veränderte die Verbreitung von Wissen entscheidend?", new[] { "Der Buchdruck mit beweglichen Lettern", "Das Rad", "Die Dampfmaschine" }, "Der Buchdruck mit beweglichen Lettern",
            "Gutenbergs Buchdruck mit beweglichen Lettern machte Bücher schneller herstellbar und Wissen für viel mehr Menschen zugänglich."),
        ("Wie nennt man die Epoche des Wiederauflebens von Kunst und Wissenschaft, die im 14.-16. Jahrhundert in Italien begann?", new[] { "Renaissance", "Aufklärung", "Industrialisierung" }, "Renaissance",
            "Die Renaissance ('Wiedergeburt') war eine Epoche neuer Blüte von Kunst und Wissenschaft, die im 14. Jahrhundert in Italien begann."),
        ("Wer leitete 1517 mit seinen 95 Thesen die Reformation ein?", new[] { "Martin Luther", "Otto von Bismarck", "Karl der Große" }, "Martin Luther",
            "Martin Luther veröffentlichte 1517 seine 95 Thesen und löste damit die Reformation der christlichen Kirche aus."),
        ("Wie nennt man die geistige Bewegung des 17./18. Jahrhunderts, die Vernunft und Wissenschaft betonte?", new[] { "Aufklärung", "Steinzeit", "Feudalismus" }, "Aufklärung",
            "Die Aufklärung betonte Vernunft, Wissenschaft und Menschenrechte und prägte moderne demokratische Ideen."),
        ("Was veränderte die Industrialisierung ab dem 18./19. Jahrhundert grundlegend?", new[] { "Produktion, Arbeit und Städte durch Maschinen und Fabriken", "Nur die Landwirtschaft blieb gleich", "Es änderte sich nichts" }, "Produktion, Arbeit und Städte durch Maschinen und Fabriken",
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
        ("Wie endete das Weströmische Reich in der Spätantike?", new[] { "Durch die Völkerwanderung und den Ansturm germanischer Stämme (476 n. Chr.)", "Durch einen Vulkanausbruch", "Es endet nie" }, "Durch die Völkerwanderung und den Ansturm germanischer Stämme (476 n. Chr.)",
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
        ("Auf einer Landkarte zeigt der obere Rand normalerweise wohin?", new[] { "Norden", "Süden", "Osten" }, "Norden",
            "Landkarten sind meist genordet: oben ist Norden, unten Süden, rechts Osten, links Westen."),
        ("Welches Hilfsmittel zeigt dir zuverlässig die Himmelsrichtung Norden an, egal wo du bist?", new[] { "Ein Kompass", "Eine Uhr", "Ein Lineal" }, "Ein Kompass",
            "Ein Kompass richtet seine Nadel nach dem Erdmagnetfeld aus und zeigt so immer zuverlässig nach Norden."),
        ("Wenn du mit dem Rücken nach Norden stehst, in welche Richtung schaust du dann?", new[] { "Süden", "Osten", "Westen" }, "Süden",
            "Steht man mit dem Rücken nach Norden, blickt man automatisch in die genau entgegengesetzte Richtung: Süden."),
        ("Wie viele Haupthimmelsrichtungen gibt es?", new[] { "4 (Norden, Süden, Osten, Westen)", "2", "8" }, "4 (Norden, Süden, Osten, Westen)",
            "Die vier Haupthimmelsrichtungen sind Norden, Süden, Osten und Westen."),
        ("Welche Himmelsrichtung liegt zwischen Norden und Osten?", new[] { "Nordosten", "Südwesten", "Süden" }, "Nordosten",
            "Zwischen Norden und Osten liegt die Nebenhimmelsrichtung Nordosten."),
        ("Welche Himmelsrichtung liegt zwischen Süden und Westen?", new[] { "Südwesten", "Nordosten", "Norden" }, "Südwesten",
            "Zwischen Süden und Westen liegt die Nebenhimmelsrichtung Südwesten."),
        ("Wo geht die Sonne unter?", new[] { "Im Westen", "Im Osten", "Im Süden" }, "Im Westen",
            "Die Sonne geht im Osten auf und im Westen unter."),
        ("Was zeigt der Gradnetz-Wert \"geografische Breite\" auf einer Karte an?", new[] { "Die Entfernung vom Äquator (Nord-Süd-Lage)", "Die Entfernung vom Nullmeridian", "Die Höhe eines Berges" }, "Die Entfernung vom Äquator (Nord-Süd-Lage)",
            "Die geografische Breite gibt an, wie weit ein Ort nördlich oder südlich des Äquators liegt."),
        ("Was zeigt die \"geografische Länge\" auf einer Karte an?", new[] { "Die Ost-West-Lage bezogen auf den Nullmeridian", "Die Nord-Süd-Lage", "Die Höhe über dem Meeresspiegel" }, "Die Ost-West-Lage bezogen auf den Nullmeridian",
            "Die geografische Länge gibt an, wie weit ein Ort östlich oder westlich des Nullmeridians liegt."),
        ("Durch welche Stadt verläuft der Nullmeridian (0° Länge)?", new[] { "Greenwich (London)", "Berlin", "Paris" }, "Greenwich (London)",
            "Der Nullmeridian, von dem aus die geografische Länge gezählt wird, verläuft durch Greenwich bei London."),
        ("Was zeigt der Maßstab auf einer Landkarte an?", new[] { "Wie stark die Wirklichkeit verkleinert dargestellt ist", "Wie viele Farben die Karte hat", "Wie alt die Karte ist" }, "Wie stark die Wirklichkeit verkleinert dargestellt ist",
            "Der Maßstab gibt an, in welchem Verhältnis Entfernungen auf der Karte zur Wirklichkeit verkleinert dargestellt sind."),
        ("Was bedeutet ein Maßstab von 1:100.000?", new[] { "1 cm auf der Karte entspricht 100.000 cm (1 km) in Wirklichkeit", "Die Karte ist 100.000-mal größer als die Wirklichkeit", "Es gibt 100.000 Orte auf der Karte" }, "1 cm auf der Karte entspricht 100.000 cm (1 km) in Wirklichkeit",
            "Bei 1:100.000 entspricht jeder Zentimeter auf der Karte 100.000 Zentimetern, also einem Kilometer, in Wirklichkeit."),
        ("Was zeigen Höhenlinien auf einer topografischen Karte?", new[] { "Gebiete mit gleicher Höhe über dem Meeresspiegel", "Straßenverläufe", "Ländergrenzen" }, "Gebiete mit gleicher Höhe über dem Meeresspiegel",
            "Höhenlinien verbinden Punkte gleicher Höhe über dem Meeresspiegel und zeigen so das Relief einer Landschaft."),
        ("Wie nennt man die Linie um die Erde, die Nord- und Südhalbkugel trennt?", new[] { "Äquator", "Nullmeridian", "Polarkreis" }, "Äquator",
            "Der Äquator ist die gedachte Linie auf halbem Weg zwischen Nord- und Südpol, die die Erde in zwei Halbkugeln teilt."),
        ("Was bedeutet die Abkürzung GPS?", new[] { "Global Positioning System - Standortbestimmung per Satellit", "Ein Kartentyp aus Papier", "Eine Art Kompass ohne Technik" }, "Global Positioning System - Standortbestimmung per Satellit",
            "GPS bestimmt mithilfe von Satelliten den genauen Standort eines Geräts auf der Erde."),
        ("Welche Himmelsrichtung liegt genau gegenüber von Osten?", new[] { "Westen", "Norden", "Süden" }, "Westen",
            "Osten und Westen liegen sich auf der Windrose genau gegenüber."),
        ("Wie kann man sich mit einer Analoguhr (bei Sonnenschein) grob die Himmelsrichtung Süden bestimmen (Nordhalbkugel)?", new[] { "Stundenzeiger auf die Sonne richten - die Winkelhalbierende zur 12 zeigt ungefähr Süden", "Die Uhr zeigt niemals eine Richtung an", "Man schaut auf die Sekundenzeiger-Farbe" }, "Stundenzeiger auf die Sonne richten - die Winkelhalbierende zur 12 zeigt ungefähr Süden",
            "Richtet man den Stundenzeiger auf die Sonne, zeigt die Winkelhalbierende zwischen Stundenzeiger und der 12 auf der Nordhalbkugel ungefähr nach Süden."),
        ("Was ist eine Legende auf einer Landkarte?", new[] { "Eine Erklärung der verwendeten Symbole und Farben", "Eine erfundene Geschichte über die Region", "Der Name des Kartenherstellers" }, "Eine Erklärung der verwendeten Symbole und Farben",
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
        ("Was bedeutet das Recht auf Mitsprache (Partizipation) für Kinder?", new[] { "Kinder dürfen bei sie betreffenden Entscheidungen ihre Meinung äußern", "Kinder entscheiden allein über alles in der Familie", "Kinder müssen zu allem schweigen" }, "Kinder dürfen bei sie betreffenden Entscheidungen ihre Meinung äußern",
            "Das Recht auf Mitsprache bedeutet, dass die Meinung von Kindern bei Entscheidungen, die sie betreffen, gehört und berücksichtigt werden soll."),
        ("Was garantiert Kindern das Recht auf Bildung laut Kinderrechtskonvention?", new[] { "Zugang zu Schule und Ausbildung, unabhängig von Herkunft", "Nur Kindern aus reichen Familien Schulbesuch", "Freiwilliger Schulbesuch nur für Jungen" }, "Zugang zu Schule und Ausbildung, unabhängig von Herkunft",
            "Das Recht auf Bildung soll allen Kindern unabhängig von Herkunft, Geschlecht oder Vermögen der Eltern den Zugang zu Schule ermöglichen."),
        ("In welchem Jahr wurde die UN-Kinderrechtskonvention verabschiedet?", new[] { "1989", "1949", "2000" }, "1989",
            "Die UN-Kinderrechtskonvention wurde 1989 von den Vereinten Nationen verabschiedet und gilt inzwischen in fast allen Ländern."),
        ("Welches Recht garantiert Kindern eine eigene Meinung und Gehör bei sie betreffenden Entscheidungen?", new[] { "Recht auf Meinungsäußerung und Mitbestimmung", "Recht auf ein eigenes Auto", "Recht, nie zur Schule zu müssen" }, "Recht auf Meinungsäußerung und Mitbestimmung",
            "Das Recht auf Meinungsäußerung und Mitbestimmung gibt Kindern die Möglichkeit, bei sie betreffenden Entscheidungen gehört zu werden."),
        ("Was garantiert das Recht auf Gesundheit für Kinder?", new[] { "Zugang zu medizinischer Versorgung und gesunder Ernährung", "Unbegrenzten Konsum von Süßigkeiten", "Keine Impfungen" }, "Zugang zu medizinischer Versorgung und gesunder Ernährung",
            "Das Recht auf Gesundheit sichert Kindern Zugang zu medizinischer Versorgung, sauberem Wasser und ausreichender Ernährung."),
        ("Was bedeutet das Recht auf Spiel und Freizeit?", new[] { "Kinder dürfen spielen, sich erholen und an Freizeitaktivitäten teilnehmen", "Kinder müssen den ganzen Tag arbeiten", "Kinder dürfen nie draußen spielen" }, "Kinder dürfen spielen, sich erholen und an Freizeitaktivitäten teilnehmen",
            "Das Recht auf Spiel und Freizeit erkennt an, dass Spielen, Erholung und Freizeit wichtig für die Entwicklung von Kindern sind."),
        ("Was schützt das Recht auf Gleichbehandlung (Diskriminierungsverbot)?", new[] { "Kinder dürfen nicht wegen Herkunft, Geschlecht oder Religion benachteiligt werden", "Nur bestimmte Kinder haben Rechte", "Mädchen haben weniger Rechte als Jungen" }, "Kinder dürfen nicht wegen Herkunft, Geschlecht oder Religion benachteiligt werden",
            "Das Diskriminierungsverbot stellt sicher, dass alle Kinder unabhängig von Herkunft, Geschlecht oder Religion dieselben Rechte haben."),
        ("Wer trägt laut Kinderrechtskonvention die Hauptverantwortung, Kinderrechte umzusetzen?", new[] { "In erster Linie die Staaten, die die Konvention unterschrieben haben", "Nur die Kinder selbst", "Niemand ist dafür zuständig" }, "In erster Linie die Staaten, die die Konvention unterschrieben haben",
            "Die Vertragsstaaten der Kinderrechtskonvention verpflichten sich, die darin festgelegten Rechte in ihrem Land umzusetzen."),
        ("Was versteht man unter dem \"Kindeswohl\" als Leitprinzip der Kinderrechtskonvention?", new[] { "Bei allen Entscheidungen soll das Wohl des Kindes vorrangig berücksichtigt werden", "Nur wirtschaftliche Interessen zählen", "Kinder haben kein eigenes Wohl" }, "Bei allen Entscheidungen soll das Wohl des Kindes vorrangig berücksichtigt werden",
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
        ("Welches Recht schützt Kinder speziell vor Kinderarbeit und Ausbeutung?", new[] { "Das Recht auf Schutz vor wirtschaftlicher Ausbeutung", "Das Recht auf ein eigenes Unternehmen", "Das Recht, überall zu arbeiten" }, "Das Recht auf Schutz vor wirtschaftlicher Ausbeutung",
            "Das Recht auf Schutz vor wirtschaftlicher Ausbeutung bewahrt Kinder vor gefährlicher oder ausbeuterischer Arbeit."),
        ("Warum ist das Recht auf Bildung besonders wichtig für Mädchen weltweit?", new[] { "In manchen Regionen haben Mädchen seltener Zugang zu Schulbildung als Jungen", "Mädchen brauchen laut Konvention keine Bildung", "Bildung ist nur für Jungen vorgesehen" }, "In manchen Regionen haben Mädchen seltener Zugang zu Schulbildung als Jungen",
            "In manchen Regionen der Welt haben Mädchen immer noch seltener Zugang zu Schulbildung als Jungen - das Recht auf Bildung gilt aber für alle Kinder gleichermaßen."),
        ("Was garantiert das Recht auf einen Namen und eine Staatsangehörigkeit ab der Geburt?", new[] { "Jedes Kind soll rechtlich anerkannt und registriert werden", "Nur manche Kinder dürfen einen Namen tragen", "Ein Name ist für Kinder nicht wichtig" }, "Jedes Kind soll rechtlich anerkannt und registriert werden",
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
        ("Was bedeutet \"Verbraucherschutz\" beim Thema Ernährung?", new[] { "Kundinnen und Kunden vor gesundheitsschädlichen oder falsch gekennzeichneten Lebensmitteln schützen", "Lebensmittel möglichst teuer machen", "Nur ausländische Lebensmittel verbieten" },
            "Kundinnen und Kunden vor gesundheitsschädlichen oder falsch gekennzeichneten Lebensmitteln schützen", "Verbraucherschutz sorgt z.B. durch Kontrollen und Kennzeichnungspflichten dafür, dass Lebensmittel sicher und ehrlich beworben sind."),
        ("Was versteht man unter \"Überfluss und Mangel\" bei der weltweiten Ernährung?", new[] { "In manchen Regionen der Welt gibt es zu viel Nahrung, in anderen zu wenig", "Überall auf der Welt gibt es gleich viel zu essen", "Mangel bedeutet, dass Essen schlecht schmeckt" },
            "In manchen Regionen der Welt gibt es zu viel Nahrung, in anderen zu wenig", "Weltweit ist Nahrung ungleich verteilt: In manchen Ländern werden Lebensmittel verschwendet, in anderen herrscht Hunger."),
        ("Was bedeutet \"regionale und saisonale\" Ernährung?", new[] { "Lebensmittel aus der Umgebung essen, die gerade Saison haben", "Nur Lebensmittel aus dem Ausland essen", "Jeden Tag genau dasselbe essen" },
            "Lebensmittel aus der Umgebung essen, die gerade Saison haben", "Regionale und saisonale Ernährung bedeutet, Obst und Gemüse aus der Nähe zu essen, wenn sie gerade natürlich reifen - das spart oft Transportwege."),
        ("Warum ist Lebensmittelverschwendung ein Problem?", new[] { "Wertvolle Ressourcen (Wasser, Anbaufläche, Energie) werden umsonst verbraucht", "Weil dadurch mehr Menschen satt werden", "Weil weggeworfenes Essen automatisch neue Pflanzen wachsen lässt" },
            "Wertvolle Ressourcen (Wasser, Anbaufläche, Energie) werden umsonst verbraucht", "Für die Herstellung von Lebensmitteln werden Wasser, Fläche und Energie verbraucht - wird das Essen weggeworfen, war das alles umsonst."),
        ("Was bedeutet \"ökologische Landwirtschaft\" (Bio-Landbau)?", new[] { "Anbau ohne chemisch-synthetische Pestizide und Düngemittel", "Anbau mit möglichst viel Chemie", "Anbau nur in Gewächshäusern" },
            "Anbau ohne chemisch-synthetische Pestizide und Düngemittel", "Ökologische Landwirtschaft verzichtet bewusst auf chemisch-synthetische Pestizide und Düngemittel und setzt auf natürliche Methoden."),
        ("Was ist eine ausgewogene Ernährung?", new[] { "Eine Mischung aus verschiedenen Lebensmittelgruppen wie Obst, Gemüse, Getreide und Eiweiß", "Nur Süßigkeiten essen", "Nur ein einziges Lebensmittel essen" },
            "Eine Mischung aus verschiedenen Lebensmittelgruppen wie Obst, Gemüse, Getreide und Eiweiß", "Eine ausgewogene Ernährung besteht aus einer abwechslungsreichen Mischung verschiedener Lebensmittelgruppen."),
        ("Warum ist Trinkwasserqualität ein wichtiges Ernährungsthema weltweit?", new[] { "Sauberes Wasser ist überlebenswichtig, aber nicht überall verfügbar", "Wasser hat nichts mit Ernährung zu tun", "Alle Menschen haben automatisch sauberes Wasser" },
            "Sauberes Wasser ist überlebenswichtig, aber nicht überall verfügbar", "Sauberes Trinkwasser ist lebensnotwendig, doch weltweit haben nicht alle Menschen gleichermaßen Zugang dazu."),
        ("Was versteht man unter einem \"ökologischen Fußabdruck\" bei der Ernährung?", new[] { "Wie stark die Umwelt durch die eigene Ernährung belastet wird", "Die Schuhgröße eines Menschen", "Die Anzahl der Mahlzeiten pro Tag" },
            "Wie stark die Umwelt durch die eigene Ernährung belastet wird", "Der ökologische Fußabdruck der Ernährung zeigt, wie stark Anbau, Transport und Verzehr von Lebensmitteln die Umwelt belasten."),
        ("Warum wird der Fleischkonsum oft kritisch diskutiert?", new[] { "Tierhaltung braucht viel Fläche, Wasser und verursacht Treibhausgase", "Fleisch hat keinerlei Umweltwirkung", "Fleisch wächst ohne Ressourcen" },
            "Tierhaltung braucht viel Fläche, Wasser und verursacht Treibhausgase", "Die Haltung von Nutztieren benötigt viel Fläche und Wasser und trägt zu Treibhausgasemissionen bei."),
        ("Was bedeutet \"Welternährungsproblem\"?", new[] { "Trotz genug produzierter Nahrung weltweit hungern viele Menschen wegen ungleicher Verteilung", "Es gibt weltweit zu wenig Nahrung, um alle zu ernähren", "Alle Menschen der Welt haben genug zu essen" },
            "Trotz genug produzierter Nahrung weltweit hungern viele Menschen wegen ungleicher Verteilung", "Weltweit wird genug Nahrung produziert, doch durch ungleiche Verteilung hungern trotzdem viele Menschen."),
        ("Was ist der Unterschied zwischen konventioneller und ökologischer Landwirtschaft beim Düngen?", new[] { "Ökologische Landwirtschaft nutzt v.a. natürlichen Dünger statt chemisch-synthetischen", "Beide nutzen identische Methoden", "Ökolandwirtschaft düngt gar nicht" },
            "Ökologische Landwirtschaft nutzt v.a. natürlichen Dünger statt chemisch-synthetischen", "Ökologische Landwirtschaft setzt vor allem auf natürlichen Dünger, konventionelle Landwirtschaft nutzt oft zusätzlich chemisch-synthetischen Dünger."),
        ("Was bewirken Transportwege (\"Lebensmittel-Kilometer\") bei importierten Lebensmitteln?", new[] { "Lange Transportwege verursachen zusätzliche Treibhausgase", "Sie machen Lebensmittel automatisch gesünder", "Sie haben keinerlei Umweltwirkung" },
            "Lange Transportwege verursachen zusätzliche Treibhausgase", "Je weiter ein Lebensmittel transportiert wird, desto mehr Treibhausgase entstehen in der Regel dabei."),
        ("Was ist ein Vorteil des Mindesthaltbarkeitsdatums (MHD) für Verbraucher?", new[] { "Es gibt eine Orientierung, bis wann ein Lebensmittel bei richtiger Lagerung mindestens gut bleibt", "Nach dem MHD ist das Lebensmittel sofort giftig", "Es zeigt den Preis des Lebensmittels" },
            "Es gibt eine Orientierung, bis wann ein Lebensmittel bei richtiger Lagerung mindestens gut bleibt", "Das MHD gibt an, bis wann ein Lebensmittel bei richtiger Lagerung mindestens seine Qualität behält - danach ist es oft noch genießbar."),
        ("Warum spielt Fairtrade auch bei Lebensmitteln wie Kaffee oder Kakao eine Rolle?", new[] { "Es sichert Erzeugerinnen und Erzeugern in ärmeren Ländern faire Preise", "Es macht Lebensmittel automatisch billiger", "Es hat nichts mit Ernährung zu tun" },
            "Es sichert Erzeugerinnen und Erzeugern in ärmeren Ländern faire Preise", "Fairtrade-Siegel bei Kaffee oder Kakao garantieren den Erzeugerinnen und Erzeugern faire Mindestpreise."),
        ("Was bedeutet \"Monokultur\" in der Landwirtschaft?", new[] { "Über lange Zeit wird auf einer Fläche nur eine einzige Pflanzenart angebaut", "Auf einer Fläche wachsen viele verschiedene Pflanzenarten gleichzeitig", "Es wird gar nichts angebaut" },
            "Über lange Zeit wird auf einer Fläche nur eine einzige Pflanzenart angebaut", "Bei Monokultur wird über lange Zeit immer nur eine einzige Pflanzenart auf derselben Fläche angebaut."),
        ("Welches Problem kann Monokultur für den Boden verursachen?", new[] { "Der Boden wird einseitig ausgelaugt und weniger fruchtbar", "Der Boden wird automatisch fruchtbarer", "Es gibt keinerlei Auswirkungen" },
            "Der Boden wird einseitig ausgelaugt und weniger fruchtbar", "Monokultur entzieht dem Boden immer dieselben Nährstoffe, wodurch er auf Dauer ausgelaugt wird."),
        ("Was sind Grundnahrungsmittel?", new[] { "Lebensmittel, die einen Großteil der täglichen Ernährung einer Bevölkerung ausmachen, z.B. Reis, Brot, Kartoffeln", "Nur seltene Delikatessen", "Nur Süßwaren" },
            "Lebensmittel, die einen Großteil der täglichen Ernährung einer Bevölkerung ausmachen, z.B. Reis, Brot, Kartoffeln", "Grundnahrungsmittel wie Reis, Brot oder Kartoffeln decken einen Großteil des täglichen Energiebedarfs vieler Menschen."),
        ("Warum ist Reis für einen großen Teil der Weltbevölkerung ein besonders wichtiges Grundnahrungsmittel?", new[] { "Er lässt sich in vielen Klimazonen anbauen und ernährt sehr viele Menschen", "Er wächst nur in Deutschland", "Er wird nirgendwo mehr angebaut" },
            "Er lässt sich in vielen Klimazonen anbauen und ernährt sehr viele Menschen", "Reis lässt sich in vielen warmen, feuchten Regionen anbauen und ist für einen sehr großen Teil der Weltbevölkerung Hauptnahrungsmittel."),
        ("Was kann jede Person selbst tun, um Lebensmittelverschwendung zu Hause zu verringern?", new[] { "Einkäufe planen und Reste sinnvoll weiterverwenden", "Möglichst viel einkaufen und oft wegwerfen", "Nie mehr einkaufen gehen" },
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
        ("Warum entstand eine der ersten Hochkulturen der Geschichte gerade am Nil in Ägypten?", new[] { "Die jährliche Nilüberschwemmung machte das Land fruchtbar für Ackerbau", "Am Nil gab es überhaupt kein Wasser", "Der Nil war zu gefährlich, um dort zu siedeln" }, "Die jährliche Nilüberschwemmung machte das Land fruchtbar für Ackerbau",
            "Die regelmäßige Überschwemmung des Nils lagerte fruchtbaren Schlamm ab, was zuverlässigen Ackerbau und damit eine frühe Hochkultur ermöglichte."),
        ("Wozu nutzten die alten Ägypter aufwendige Bewässerungssysteme am Nil?", new[] { "Um Wasser gezielt auf die Felder zu leiten", "Um den Fluss trockenzulegen", "Um Boote zu verbrennen" }, "Um Wasser gezielt auf die Felder zu leiten",
            "Kanäle und Bewässerungssysteme leiteten Nilwasser gezielt auf die Felder, auch abseits der direkten Uferzone."),
        ("Warum kann Wasser heute zwischen manchen Staaten zu einem Konfliktfaktor werden?", new[] { "Flüsse fließen durch mehrere Länder, die sich um die Nutzung streiten können", "Wasser ist überall unbegrenzt vorhanden", "Wasser hat keinerlei wirtschaftliche Bedeutung" }, "Flüsse fließen durch mehrere Länder, die sich um die Nutzung streiten können",
            "Grenzüberschreitende Flüsse wie der Nil werden von mehreren Staaten genutzt, was zu Konflikten um Wasserrechte führen kann."),
        ("Wofür wird Wasser als Wirtschaftsfaktor neben der Landwirtschaft noch genutzt?", new[] { "Für Transport (Schifffahrt) und Energiegewinnung (Wasserkraft)", "Nur zum Blumengießen", "Wasser hat keine wirtschaftliche Bedeutung" }, "Für Transport (Schifffahrt) und Energiegewinnung (Wasserkraft)",
            "Flüsse und Meere dienen als Transportwege für Schiffe und liefern über Wasserkraftwerke Energie."),
        ("Was ist ein Beispiel dafür, dass Wasser auch ein Freizeitfaktor ist?", new[] { "Schwimmen, Segeln oder Angeln an Seen und Flüssen", "Wasser wird nie für Freizeit genutzt", "Nur zum Putzen verwendet" }, "Schwimmen, Segeln oder Angeln an Seen und Flüssen",
            "Seen, Flüsse und Meere werden von vielen Menschen zum Baden, Segeln, Angeln oder für andere Freizeitaktivitäten genutzt."),
        ("Wie formt Wasser über lange Zeiträume Küsten und Landschaften?", new[] { "Durch Erosion, also das Abtragen von Gestein und Boden", "Wasser hat keinerlei Einfluss auf Landschaften", "Nur durch Regen in der Wüste" }, "Durch Erosion, also das Abtragen von Gestein und Boden",
            "Wellen, Strömungen und fließendes Wasser tragen über lange Zeit Gestein und Boden ab und formen so Küsten und Täler."),
        ("Was versteht man unter Wasserknappheit?", new[] { "In einer Region steht nicht genug sauberes Wasser für den Bedarf zur Verfügung", "Es gibt überall auf der Welt zu viel Wasser", "Ein anderes Wort für Überschwemmung" }, "In einer Region steht nicht genug sauberes Wasser für den Bedarf zur Verfügung",
            "Wasserknappheit bedeutet, dass in einer Region nicht ausreichend sauberes Trinkwasser für Menschen, Landwirtschaft und Industrie vorhanden ist."),
        ("Warum war der Nil für die alten Ägypter wichtiger als Regen?", new[] { "In Ägypten regnet es nur sehr selten, der Nil lieferte zuverlässig Wasser", "In Ägypten regnete es jeden Tag stark", "Regen war für den Ackerbau schädlich" }, "In Ägypten regnet es nur sehr selten, der Nil lieferte zuverlässig Wasser",
            "Ägypten liegt größtenteils in der Wüste mit sehr wenig Niederschlag - ohne den Nil wäre dauerhafter Ackerbau dort kaum möglich gewesen."),
        ("Was ist ein Wasserkraftwerk?", new[] { "Eine Anlage, die die Bewegungsenergie von Wasser in elektrischen Strom umwandelt", "Ein Gebäude zum Trinkwasser abfüllen", "Ein Ort, an dem nur geschwommen wird" }, "Eine Anlage, die die Bewegungsenergie von Wasser in elektrischen Strom umwandelt",
            "In Wasserkraftwerken treibt strömendes oder fallendes Wasser Turbinen an, die daraus elektrischen Strom erzeugen."),
        ("Warum ist der Zugang zu sauberem Trinkwasser weltweit ungleich verteilt?", new[] { "Klima, Geografie und wirtschaftliche Mittel unterscheiden sich stark zwischen Regionen", "Alle Länder der Welt haben exakt gleich viel Wasser", "Trinkwasser ist überall unbegrenzt kostenlos verfügbar" }, "Klima, Geografie und wirtschaftliche Mittel unterscheiden sich stark zwischen Regionen",
            "Trockene Klimazonen, geografische Lage und fehlende Infrastruktur führen dazu, dass sauberes Trinkwasser weltweit sehr ungleich verteilt ist."),
        ("Was zeigt das Beispiel Ägypten über den Zusammenhang von Wasser und der Entstehung früher Staaten?", new[] { "Zuverlässige Wasserversorgung ermöglichte Ackerbau, Vorratshaltung und damit eine organisierte Gesellschaft", "Wasser hatte für die Entstehung von Staaten keine Bedeutung", "Staaten entstanden nur in wasserlosen Wüstenregionen" }, "Zuverlässige Wasserversorgung ermöglichte Ackerbau, Vorratshaltung und damit eine organisierte Gesellschaft",
            "Verlässliche Ernten durch Bewässerung schufen Nahrungsüberschüsse, die Arbeitsteilung, Handel und eine zentrale Verwaltung wie im alten Ägypten ermöglichten."),
        ("Was ist eine mögliche Folge, wenn zwei Länder um dasselbe Flusswasser konkurrieren?", new[] { "Politische Spannungen oder Verhandlungen über die gerechte Wasserverteilung", "Der Fluss verschwindet automatisch", "Es entstehen keinerlei Probleme" }, "Politische Spannungen oder Verhandlungen über die gerechte Wasserverteilung",
            "Wenn mehrere Staaten von einem Fluss abhängig sind, kann die Nutzung zu politischen Spannungen führen, die oft durch Verhandlungen gelöst werden müssen."),
        ("Was passiert mit einer Küstenlinie, wenn Wellen über lange Zeit weiches Gestein abtragen?", new[] { "Es können Buchten, Klippen oder Strände entstehen", "Die Küstenlinie bleibt für immer exakt gleich", "Das Meer verschwindet komplett" }, "Es können Buchten, Klippen oder Strände entstehen",
            "Die Erosionskraft des Meeres formt über sehr lange Zeiträume charakteristische Küstenformen wie Buchten, Klippen oder Sandstrände."),
        ("Was bedeutet \"Bewässerungsfeldbau\", wie ihn die alten Ägypter betrieben?", new[] { "Felder werden gezielt künstlich mit Wasser aus Kanälen versorgt", "Felder werden nur vom Regen bewässert", "Felder werden absichtlich trockengelegt" }, "Felder werden gezielt künstlich mit Wasser aus Kanälen versorgt",
            "Beim Bewässerungsfeldbau wird Wasser über Kanäle und Gräben gezielt zu den Feldern geleitet, unabhängig vom Regen."),
        ("Warum kann übermäßige Wasserentnahme aus einem Fluss für Anwohner flussabwärts problematisch sein?", new[] { "Ihnen bleibt dann weniger Wasser für Landwirtschaft und Alltag übrig", "Es hat keinerlei Auswirkungen flussabwärts", "Der Fluss wird dadurch automatisch breiter" }, "Ihnen bleibt dann weniger Wasser für Landwirtschaft und Alltag übrig",
            "Wird flussaufwärts sehr viel Wasser entnommen, kann flussabwärts weniger Wasser für Landwirtschaft, Trinkwasser und Industrie übrig bleiben."),
        ("Welche Rolle spielten Nilüberschwemmungen für die Kalenderentwicklung im alten Ägypten?", new[] { "Der Zeitpunkt der Überschwemmung half, ein frühes Kalendersystem zu entwickeln", "Überschwemmungen hatten nichts mit dem Kalender zu tun", "Es gab in Ägypten nie einen Kalender" }, "Der Zeitpunkt der Überschwemmung half, ein frühes Kalendersystem zu entwickeln",
            "Da die Nilüberschwemmung regelmäßig zur gleichen Jahreszeit auftrat, orientierten sich die alten Ägypter bei ihrem Kalender daran."),
        ("Was ist ein Stausee?", new[] { "Ein künstlich angelegter See hinter einem Damm, der Wasser speichert", "Ein natürlicher See ohne jeden menschlichen Einfluss", "Ein anderes Wort für Ozean" }, "Ein künstlich angelegter See hinter einem Damm, der Wasser speichert",
            "Ein Stausee entsteht, wenn ein Damm einen Fluss aufstaut, um Wasser für Bewässerung, Trinkwasser oder Stromerzeugung zu speichern."),
        ("Wofür wird der Assuan-Staudamm am Nil in Ägypten heute unter anderem genutzt?", new[] { "Für Hochwasserschutz und Stromerzeugung", "Nur zum Baden für Touristen", "Er hat keinerlei Funktion" }, "Für Hochwasserschutz und Stromerzeugung",
            "Der Assuan-Staudamm reguliert seit dem 20. Jahrhundert die Nilüberschwemmungen und erzeugt zugleich elektrischen Strom."),
        ("Warum ist der sparsame Umgang mit Trinkwasser auch in wasserreichen Ländern wie Deutschland sinnvoll?", new[] { "Die Aufbereitung von Trinkwasser kostet Energie und Ressourcen", "Wasser sparen hat überhaupt keinen Nutzen", "In Deutschland gibt es unendlich viel Wasser" }, "Die Aufbereitung von Trinkwasser kostet Energie und Ressourcen",
            "Auch in Ländern mit viel Niederschlag verbraucht die Aufbereitung und Verteilung von Trinkwasser Energie und Ressourcen, weshalb Sparen sinnvoll bleibt."),
        ("Was verbindet die Themen \"Wasser als Konfliktfaktor\" und \"Wasser als Wirtschaftsfaktor\" miteinander?", new[] { "Beide zeigen, wie wichtig die begrenzte Ressource Wasser für Gesellschaften ist", "Beide Themen haben nichts miteinander zu tun", "Wasser ist niemals wirtschaftlich oder politisch bedeutsam" }, "Beide zeigen, wie wichtig die begrenzte Ressource Wasser für Gesellschaften ist",
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
        ("Wie viele Einwohner hatte die antike Stadt Rom auf ihrem Höhepunkt schätzungsweise?", new[] { "Über eine Million", "Nur etwa 500", "Etwa 50" }, "Über eine Million",
            "Das antike Rom war mit schätzungsweise über einer Million Einwohnern eine der größten Städte der antiken Welt."),
        ("Was war das Forum Romanum im antiken Rom?", new[] { "Das zentrale, öffentliche Platz- und Marktgebiet der Stadt", "Ein privates Wohnhaus", "Ein Fluss außerhalb der Stadt" }, "Das zentrale, öffentliche Platz- und Marktgebiet der Stadt",
            "Das Forum Romanum war das politische, religiöse und wirtschaftliche Zentrum Roms mit Tempeln, Märkten und öffentlichen Gebäuden."),
        ("Wie versorgten die Römer ihre Großstadt zuverlässig mit Trinkwasser?", new[] { "Durch Aquädukte, die Wasser über weite Strecken in die Stadt leiteten", "Sie hatten überhaupt kein fließendes Wasser", "Nur durch Regenwasser in Eimern" }, "Durch Aquädukte, die Wasser über weite Strecken in die Stadt leiteten",
            "Römische Aquädukte transportierten Wasser oft über viele Kilometer aus den Bergen in die Stadt Rom."),
        ("Was waren die Thermen im antiken Rom?", new[] { "Öffentliche Badeanlagen, die auch als soziale Treffpunkte dienten", "Private Gärten der Kaiser", "Getreidespeicher" }, "Öffentliche Badeanlagen, die auch als soziale Treffpunkte dienten",
            "Die Thermen waren große öffentliche Bäder, in denen sich die Menschen wuschen, entspannten und trafen."),
        ("Was ist ein typisches Merkmal moderner Großstädte wie dem Großraum Berlin?", new[] { "Hohe Bevölkerungsdichte und vielfältiges kulturelles Angebot", "Fast keine Einwohner", "Ausschließlich landwirtschaftliche Flächen" }, "Hohe Bevölkerungsdichte und vielfältiges kulturelles Angebot",
            "Moderne Großstädte wie Berlin zeichnen sich durch hohe Bevölkerungsdichte, vielfältige Kultur, Wirtschaft und Infrastruktur aus."),
        ("Was versteht man unter \"Verkehrsverdichtung\" in Großstädten?", new[] { "Viele Fahrzeuge auf engem Raum, oft mit Staus verbunden", "Es gibt in der Stadt überhaupt keinen Verkehr", "Straßen werden ständig breiter gebaut" }, "Viele Fahrzeuge auf engem Raum, oft mit Staus verbunden",
            "Verkehrsverdichtung entsteht, wenn viele Menschen und Fahrzeuge auf begrenztem städtischem Raum unterwegs sind, was oft zu Staus führt."),
        ("Was ist ein Vorteil von kultureller Vielfalt in einer Großstadt?", new[] { "Unterschiedliche Perspektiven, Ideen und Angebote bereichern das Zusammenleben", "Vielfalt hat ausschließlich Nachteile", "Städte mit Vielfalt haben weniger Möglichkeiten" }, "Unterschiedliche Perspektiven, Ideen und Angebote bereichern das Zusammenleben",
            "Kulturelle Vielfalt bringt unterschiedliche Ideen, Restaurants, Feste und Perspektiven zusammen und kann eine Stadt lebendiger machen."),
        ("Was ist ein Beispiel für Umweltbelastung durch eine Großstadt?", new[] { "Luftverschmutzung durch viele Fahrzeuge und Industrie", "Städte verursachen niemals Umweltprobleme", "Nur ländliche Gebiete verursachen Umweltbelastung" }, "Luftverschmutzung durch viele Fahrzeuge und Industrie",
            "Der hohe Verkehrs- und Energieverbrauch in Großstädten führt häufig zu Luftverschmutzung und anderen Umweltbelastungen."),
        ("Was war die \"Insula\" im antiken Rom?", new[] { "Ein mehrstöckiges Mietshaus für die einfache Stadtbevölkerung", "Ein Tempel für die Götter", "Ein römisches Kriegsschiff" }, "Ein mehrstöckiges Mietshaus für die einfache Stadtbevölkerung",
            "Insulae waren mehrstöckige Wohnhäuser, in denen die Mehrheit der einfachen römischen Stadtbevölkerung dicht gedrängt lebte."),
        ("Warum gilt Innovation als eine Chance großer Städte?", new[] { "Viele Menschen, Unternehmen und Ideen treffen auf engem Raum zusammen", "Innovation entsteht nur außerhalb von Städten", "Städte verhindern grundsätzlich neue Ideen" }, "Viele Menschen, Unternehmen und Ideen treffen auf engem Raum zusammen",
            "Die Nähe vieler Menschen, Firmen, Universitäten und Ideen in Großstädten begünstigt oft neue Erfindungen und Entwicklungen."),
        ("Was regelte das römische Straßennetz für den Alltag der Stadt?", new[] { "Transport von Waren, Truppen und Menschen zwischen Städten und Regionen", "Es diente ausschließlich religiösen Prozessionen", "Straßen wurden im antiken Rom nicht gebaut" }, "Transport von Waren, Truppen und Menschen zwischen Städten und Regionen",
            "Das gut ausgebaute römische Straßennetz ermöglichte Handel, Truppenbewegungen und Reisen im gesamten Römischen Reich."),
        ("Was ist eine typische Herausforderung bei Wohnraum in modernen Großstädten?", new[] { "Wohnungen sind oft knapp und teuer", "Wohnraum ist in Städten immer im Überfluss vorhanden", "Wohnungen kosten in Städten grundsätzlich nichts" }, "Wohnungen sind oft knapp und teuer",
            "Durch die hohe Nachfrage in Großstädten sind Wohnungen häufig knapper und teurer als in ländlichen Gebieten."),
        ("Was war das Kolosseum im antiken Rom?", new[] { "Ein großes Amphitheater für öffentliche Unterhaltung wie Gladiatorenkämpfe", "Ein privates Wohnhaus für Bauern", "Ein Getreidespeicher außerhalb der Stadt" }, "Ein großes Amphitheater für öffentliche Unterhaltung wie Gladiatorenkämpfe",
            "Das Kolosseum bot Platz für tausende Zuschauer bei öffentlichen Veranstaltungen wie Gladiatorenkämpfen."),
        ("Wie unterscheidet sich der öffentliche Nahverkehr in Großstädten oft von dem auf dem Land?", new[] { "In Großstädten gibt es meist ein dichteres Netz an Bus, Bahn und U-Bahn", "Es gibt keinerlei Unterschied", "Auf dem Land ist der Nahverkehr immer dichter" }, "In Großstädten gibt es meist ein dichteres Netz an Bus, Bahn und U-Bahn",
            "Aufgrund der hohen Bevölkerungsdichte lohnt sich in Großstädten meist ein dichteres öffentliches Verkehrsnetz als auf dem Land."),
        ("Was bedeutet \"Ghettoisierung\" oder soziale Trennung in manchen Stadtvierteln?", new[] { "Bestimmte Bevölkerungsgruppen leben räumlich stark getrennt voneinander", "Alle Stadtviertel sind automatisch sozial gleich gemischt", "Ein anderes Wort für Grünflächen" }, "Bestimmte Bevölkerungsgruppen leben räumlich stark getrennt voneinander",
            "In manchen Städten konzentrieren sich bestimmte Bevölkerungsgruppen in einzelnen Vierteln, was als Herausforderung für den sozialen Zusammenhalt gilt."),
        ("Warum brauchten römische Großstädte ein organisiertes System zur Getreideversorgung?", new[] { "So viele Menschen konnten sich nicht mehr selbst mit Nahrung versorgen", "Es gab in Rom keine hungrigen Menschen", "Getreide wurde nur für Tiere gebraucht" }, "So viele Menschen konnten sich nicht mehr selbst mit Nahrung versorgen",
            "Bei über einer Million Einwohnern musste Getreide organisiert aus den Provinzen des Reiches nach Rom transportiert werden."),
        ("Was ist ein Vorteil von Grünflächen und Parks in modernen Großstädten?", new[] { "Sie verbessern Luftqualität und bieten Erholungsraum", "Sie schaden der Umwelt", "Sie werden in Städten grundsätzlich nicht benötigt" }, "Sie verbessern Luftqualität und bieten Erholungsraum",
            "Parks und Grünflächen verbessern das Stadtklima, filtern Luft und bieten Bewohnerinnen und Bewohnern Erholungsmöglichkeiten."),
        ("Was bezeichnete man im antiken Rom als \"Brot und Spiele\" (panem et circenses)?", new[] { "Kostenlose Getreideverteilung und öffentliche Unterhaltung zur Zufriedenstellung der Bevölkerung", "Ein Sportwettkampf ohne Zuschauer", "Ein privates Familienessen" }, "Kostenlose Getreideverteilung und öffentliche Unterhaltung zur Zufriedenstellung der Bevölkerung",
            "Mit kostenlosem Getreide und öffentlichen Unterhaltungsveranstaltungen versuchten römische Herrscher, die Stadtbevölkerung zufriedenzustellen."),
        ("Was ist eine mögliche Chance kultureller Vielfalt für die lokale Wirtschaft einer Großstadt?", new[] { "Vielfältige Geschäfte, Restaurants und Dienstleistungen sprechen mehr Menschen an", "Vielfalt schadet immer der Wirtschaft", "Wirtschaftlich lohnt sich ausschließlich Einheitlichkeit" }, "Vielfältige Geschäfte, Restaurants und Dienstleistungen sprechen mehr Menschen an",
            "Ein vielfältiges Angebot an Geschäften, Restaurants und Dienstleistungen kann unterschiedliche Kundengruppen ansprechen und die lokale Wirtschaft stärken."),
        ("Warum werden Städte wie Rom in der Antike oft als Vorbild für spätere Stadtplanung genannt?", new[] { "Sie hatten organisierte Infrastruktur wie Straßen, Wasserversorgung und öffentliche Gebäude", "Antike Städte hatten überhaupt keine Planung", "Es gab keinerlei öffentliche Gebäude" }, "Sie hatten organisierte Infrastruktur wie Straßen, Wasserversorgung und öffentliche Gebäude",
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
        ("Wie weit erstreckte sich das Römische Reich auf seinem Höhepunkt ungefähr?", new[] { "Rund um das gesamte Mittelmeer und weite Teile Europas", "Nur über die Stadt Rom selbst", "Nur über Nordeuropa" }, "Rund um das gesamte Mittelmeer und weite Teile Europas",
            "Das Römische Reich umfasste auf seinem Höhepunkt Gebiete rund um das gesamte Mittelmeer sowie große Teile West- und Südeuropas."),
        ("Was ist die Europäische Union (EU)?", new[] { "Ein politischer und wirtschaftlicher Zusammenschluss europäischer Staaten", "Ein einzelnes europäisches Land", "Ein Sportverband" }, "Ein politischer und wirtschaftlicher Zusammenschluss europäischer Staaten",
            "Die EU ist ein Zusammenschluss europäischer Staaten, die politisch und wirtschaftlich eng zusammenarbeiten."),
        ("Welche gemeinsame Währung nutzen viele, aber nicht alle EU-Mitgliedsstaaten?", new[] { "Den Euro", "Den Dollar", "Das Pfund" }, "Den Euro",
            "Der Euro ist die gemeinsame Währung vieler, aber nicht aller EU-Mitgliedsstaaten (die sogenannte Eurozone)."),
        ("Was ermöglicht der freie Personenverkehr innerhalb großer Teile der EU (Schengen-Raum)?", new[] { "Reisen zwischen vielen Mitgliedstaaten meist ohne Passkontrollen an der Grenze", "Es gibt in der EU keinerlei Reisefreiheit", "Jeder Grenzübertritt braucht ein Visum" }, "Reisen zwischen vielen Mitgliedstaaten meist ohne Passkontrollen an der Grenze",
            "Im Schengen-Raum können Menschen zwischen vielen Mitgliedstaaten in der Regel ohne Passkontrollen an der Binnengrenze reisen."),
        ("Was war die Berliner Mauer?", new[] { "Eine Grenzbefestigung, die von 1961 bis 1989 Ost- und West-Berlin trennte", "Eine antike römische Stadtmauer", "Eine touristische Attraktion ohne historische Bedeutung" }, "Eine Grenzbefestigung, die von 1961 bis 1989 Ost- und West-Berlin trennte",
            "Die Berliner Mauer trennte von 1961 bis 1989 Ost- und West-Berlin und wurde zum Symbol der deutschen und europäischen Teilung."),
        ("Was geschah mit Deutschland nach dem Zweiten Weltkrieg zunächst?", new[] { "Es wurde in verschiedene Besatzungszonen geteilt, später in BRD und DDR", "Es blieb völlig unverändert", "Es wurde sofort Teil der EU" }, "Es wurde in verschiedene Besatzungszonen geteilt, später in BRD und DDR",
            "Nach 1945 wurde Deutschland zunächst in Besatzungszonen geteilt, aus denen später die Bundesrepublik Deutschland und die DDR entstanden."),
        ("Was verbindet das Römische Reich und die heutige EU als historischen Vergleich?", new[] { "Beide verbanden viele verschiedene Regionen/Völker unter einem gemeinsamen politischen Rahmen", "Beide bestanden nur aus einer einzigen Stadt", "Beide existieren zur gleichen Zeit" }, "Beide verbanden viele verschiedene Regionen/Völker unter einem gemeinsamen politischen Rahmen",
            "Sowohl das Römische Reich als auch die EU verbanden sehr unterschiedliche Regionen unter einem gemeinsamen politischen und rechtlichen Rahmen - wenn auch auf ganz unterschiedliche Weise."),
        ("Wie viele Mitgliedstaaten hat die Europäische Union ungefähr (Stand nach dem Brexit)?", new[] { "27", "5", "100" }, "27",
            "Nach dem Austritt Großbritanniens (Brexit) 2020 hat die EU 27 Mitgliedstaaten."),
        ("Was bedeutet \"Binnenmarkt\" in der EU?", new[] { "Waren, Dienstleistungen, Kapital und Personen können sich weitgehend frei zwischen Mitgliedstaaten bewegen", "Jeder Staat handelt komplett isoliert für sich", "Es gibt in der EU keinerlei Handel" }, "Waren, Dienstleistungen, Kapital und Personen können sich weitgehend frei zwischen Mitgliedstaaten bewegen",
            "Der EU-Binnenmarkt erlaubt den weitgehend freien Austausch von Waren, Dienstleistungen, Kapital und Personen zwischen den Mitgliedstaaten."),
        ("Warum wird der 9. November 1989 auch europäisch als wichtiges Datum gesehen?", new[] { "Der Mauerfall gilt als Symbol für das Ende der Teilung Europas im Kalten Krieg", "An diesem Tag wurde die EU gegründet", "Es war der Beginn des Zweiten Weltkriegs" }, "Der Mauerfall gilt als Symbol für das Ende der Teilung Europas im Kalten Krieg",
            "Der Fall der Berliner Mauer wird oft als Symbol für das Ende der politischen Teilung Europas im Kalten Krieg gesehen."),
        ("Was ist eine Klimazone?", new[] { "Ein Gebiet mit ähnlichen, typischen Wetterbedingungen über das Jahr", "Ein anderes Wort für Staatsgrenze", "Eine Art politischer Vertrag" }, "Ein Gebiet mit ähnlichen, typischen Wetterbedingungen über das Jahr",
            "Klimazonen fassen Regionen mit ähnlichen typischen Temperatur- und Niederschlagsverhältnissen zusammen, z.B. mediterran oder gemäßigt."),
        ("Welche Klimazone prägt weite Teile Südeuropas rund um das Mittelmeer?", new[] { "Das mediterrane Klima mit trockenen, warmen Sommern", "Ein tropisches Regenwaldklima", "Ein arktisches Eisklima" }, "Das mediterrane Klima mit trockenen, warmen Sommern",
            "Das mediterrane Klima am Mittelmeer ist geprägt von trockenen, warmen Sommern und milden, feuchteren Wintern."),
        ("Wie wird der Bundestag gemeinsam mit den Institutionen der EU beschrieben, wenn es um Gesetzgebung geht?", new[] { "Nationale Parlamente und EU-Institutionen wirken bei bestimmten Themen zusammen", "Nationale Parlamente haben mit der EU nichts zu tun", "Nur die EU darf über alles allein entscheiden" }, "Nationale Parlamente und EU-Institutionen wirken bei bestimmten Themen zusammen",
            "In vielen Politikbereichen arbeiten nationale Parlamente wie der Bundestag und EU-Institutionen wie das Europaparlament zusammen."),
        ("Was ist das Europäische Parlament?", new[] { "Die von EU-Bürgerinnen und -Bürgern direkt gewählte Vertretung auf EU-Ebene", "Ein Parlament nur für Deutschland", "Ein Gericht für Verkehrsdelikte" }, "Die von EU-Bürgerinnen und -Bürgern direkt gewählte Vertretung auf EU-Ebene",
            "Das Europäische Parlament wird direkt von den Bürgerinnen und Bürgern der EU-Mitgliedstaaten gewählt und wirkt an EU-Gesetzen mit."),
        ("Was symbolisiert der Begriff \"Europa - grenzenlos?\" im Vergleich zur deutschen Teilung während des Kalten Krieges?", new[] { "Den Kontrast zwischen früherer strikter Grenzziehung und heutiger europäischer Reisefreiheit", "Dass es in Europa niemals Grenzen gab", "Dass Grenzen heute strenger sind als je zuvor" }, "Den Kontrast zwischen früherer strikter Grenzziehung und heutiger europäischer Reisefreiheit",
            "Die einst streng bewachte innerdeutsche Grenze steht im starken Kontrast zur heutigen weitgehenden Reisefreiheit innerhalb der EU."),
        ("Was ist ein Vorteil der EU-Mitgliedschaft für den Handel zwischen den Ländern?", new[] { "Weniger Zölle und einfachere Regeln zwischen Mitgliedstaaten", "Handel zwischen EU-Ländern ist grundsätzlich verboten", "Jedes Produkt braucht eine Sondererlaubnis pro Land" }, "Weniger Zölle und einfachere Regeln zwischen Mitgliedstaaten",
            "Durch den EU-Binnenmarkt entfallen viele Zölle und bürokratische Hürden beim Handel zwischen Mitgliedstaaten."),
        ("Was hatten das Römische Reich und moderne europäische Staaten gemeinsam in Bezug auf Straßen und Infrastruktur?", new[] { "Beide bauten überregionale Verkehrsnetze für Handel und Austausch aus", "Beide bauten überhaupt keine Straßen", "Straßenbau spielte in beiden keine Rolle" }, "Beide bauten überregionale Verkehrsnetze für Handel und Austausch aus",
            "Sowohl das antike Straßennetz Roms als auch moderne europäische Verkehrsnetze (Autobahnen, Schienen) fördern Handel und Austausch über Regionen hinweg."),
        ("Was ist ein Unterschied zwischen dem Römischen Reich und der heutigen EU?", new[] { "Das Römische Reich wurde militärisch erobert, die EU beruht auf freiwilligem Beitritt der Mitgliedstaaten", "Beide beruhten auf exakt denselben Prinzipien", "Beide bestehen aus genau denselben Ländern" }, "Das Römische Reich wurde militärisch erobert, die EU beruht auf freiwilligem Beitritt der Mitgliedstaaten",
            "Anders als das durch Eroberung entstandene Römische Reich basiert die EU auf dem freiwilligen politischen Zusammenschluss unabhängiger Staaten."),
        ("Warum ist die deutsche Wiedervereinigung 1990 auch ein europäisches Ereignis?", new[] { "Sie markierte das Ende der europäischen Teilung im Kalten Krieg und stärkte die europäische Zusammenarbeit", "Sie betraf ausschließlich innerdeutsche Angelegenheiten", "Sie hatte keinerlei Auswirkungen auf Europa" }, "Sie markierte das Ende der europäischen Teilung im Kalten Krieg und stärkte die europäische Zusammenarbeit",
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
        ("Was ist ein möglicher Nachteil von Massentourismus für die Umwelt?", new[] { "Überlastung von Natur, mehr Müll und höherer Ressourcenverbrauch", "Massentourismus hat keinerlei Umweltwirkung", "Die Umwelt profitiert automatisch von mehr Touristen" }, "Überlastung von Natur, mehr Müll und höherer Ressourcenverbrauch",
            "Sehr viele Touristinnen und Touristen an einem Ort können zu Umweltbelastung, mehr Abfall und höherem Ressourcenverbrauch führen."),
        ("Was ist ein Vorteil von Tourismus für eine Region?", new[] { "Er kann Arbeitsplätze und Einkommen für die lokale Bevölkerung schaffen", "Tourismus bringt einer Region grundsätzlich nur Nachteile", "Tourismus verhindert jede wirtschaftliche Entwicklung" }, "Er kann Arbeitsplätze und Einkommen für die lokale Bevölkerung schaffen",
            "Tourismus kann Arbeitsplätze in Hotels, Gastronomie und Handel schaffen und der lokalen Bevölkerung Einkommen bringen."),
        ("Was zeichnete die berühmten Seidenstraßen-Handelsrouten historisch aus?", new[] { "Sie verbanden Asien und Europa für den Warenaustausch über weite Strecken", "Sie verliefen ausschließlich innerhalb einer einzigen Stadt", "Sie wurden nur für Kriege genutzt" }, "Sie verbanden Asien und Europa für den Warenaustausch über weite Strecken",
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
        ("Was zeichnet die Nord- und Ostseeküste Deutschlands geografisch aus?", new[] { "Flaches Küstenland mit Häfen, Watt und Inseln", "Hohe Gebirgsketten direkt am Meer", "Wüstenlandschaft" }, "Flaches Küstenland mit Häfen, Watt und Inseln",
            "Die deutsche Nord- und Ostseeküste ist geprägt von flachem Küstenland, Wattenmeer, Häfen und vorgelagerten Inseln."),
        ("Was können Reisende tun, um die Umweltbelastung durch ihre Reisen zu verringern?", new[] { "Umweltfreundlichere Verkehrsmittel wählen und Region/Kultur respektvoll behandeln", "Möglichst viel Müll am Reiseziel hinterlassen", "Ausschließlich mit dem Flugzeug auf kurze Strecken reisen" }, "Umweltfreundlichere Verkehrsmittel wählen und Region/Kultur respektvoll behandeln",
            "Die Wahl umweltfreundlicherer Verkehrsmittel und ein respektvoller Umgang mit Natur und Kultur vor Ort können die negativen Folgen von Reisen verringern."),
        ("Was war ein wichtiger Antrieb für Entdeckungsreisen der europäischen Seefahrer im 15./16. Jahrhundert?", new[] { "Die Suche nach neuen Seewegen für den Gewürzhandel", "Reines Interesse an Strandurlaub", "Die Flucht vor Massentourismus" }, "Die Suche nach neuen Seewegen für den Gewürzhandel",
            "Viele Entdeckungsreisen wie die portugiesischer und spanischer Seefahrer zielten auf neue Seewege für den lukrativen Gewürzhandel mit Asien."),
        ("Wie hat sich die durchschnittliche Reisedauer zwischen Kontinenten seit dem Mittelalter verändert?", new[] { "Sie hat sich durch moderne Verkehrsmittel drastisch verkürzt", "Sie ist exakt gleich geblieben", "Sie hat sich deutlich verlängert" }, "Sie hat sich durch moderne Verkehrsmittel drastisch verkürzt",
            "Was früher Wochen oder Monate dauerte (Schiffsreisen), ist mit modernen Flugzeugen oft in wenigen Stunden möglich."),
        ("Was ist ein Grund dafür, dass Reisen heute für viel mehr Menschen möglich ist als früher?", new[] { "Günstigere und schnellere Verkehrsmittel sowie höherer allgemeiner Wohlstand", "Reisen ist heute komplizierter als früher", "Es gibt heute weniger Verkehrsmittel als früher" }, "Günstigere und schnellere Verkehrsmittel sowie höherer allgemeiner Wohlstand",
            "Günstigere Flugpreise, bessere Infrastruktur und gestiegener Wohlstand machen Reisen heute für deutlich mehr Menschen zugänglich als in früheren Jahrhunderten."),
        ("Welche Rolle spielten venezianische Kaufleute wie Marco Polo für den Handel zwischen Europa und Asien im Mittelalter?", new[] { "Sie erkundeten und beschrieben neue Handelsrouten und brachten Wissen über ferne Länder nach Europa", "Sie reisten ausschließlich innerhalb Venedigs", "Sie hatten mit Handel nichts zu tun" }, "Sie erkundeten und beschrieben neue Handelsrouten und brachten Wissen über ferne Länder nach Europa",
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
        ("Was ist ein zentraler Unterschied zwischen der athenischen und der heutigen deutschen Demokratie?", new[] { "Heute dürfen alle erwachsenen Staatsbürgerinnen und Staatsbürger unabhängig von Geschlecht wählen", "In Athen durften auch Kinder abstimmen", "Es gibt keinerlei Unterschiede" }, "Heute dürfen alle erwachsenen Staatsbürgerinnen und Staatsbürger unabhängig von Geschlecht wählen",
            "Im Gegensatz zur athenischen Demokratie gilt das Wahlrecht heute unabhängig vom Geschlecht für alle erwachsenen Staatsbürgerinnen und Staatsbürger."),
        ("Was ist der Unterschied zwischen direkter und repräsentativer (indirekter) Demokratie?", new[] { "Bei direkter Demokratie stimmt das Volk selbst über Sachfragen ab, bei repräsentativer wählt es Vertreter", "Beide Formen bedeuten exakt dasselbe", "Repräsentative Demokratie kennt keine Wahlen" }, "Bei direkter Demokratie stimmt das Volk selbst über Sachfragen ab, bei repräsentativer wählt es Vertreter",
            "In der direkten Demokratie (wie im antiken Athen) stimmt das Volk direkt ab, in der repräsentativen Demokratie (wie in Deutschland) wählt es Abgeordnete, die entscheiden."),
        ("Was ist eine politische Partei?", new[] { "Eine Organisation von Menschen mit ähnlichen politischen Zielen, die sich zur Wahl stellt", "Ein anderes Wort für Parlament", "Eine sportliche Mannschaft" }, "Eine Organisation von Menschen mit ähnlichen politischen Zielen, die sich zur Wahl stellt",
            "Parteien bündeln Menschen mit ähnlichen politischen Zielen und Vorstellungen und treten gemeinsam zu Wahlen an."),
        ("Was ist ein Parlament?", new[] { "Die gewählte Volksvertretung, die Gesetze berät und beschließt", "Ein Gerichtsgebäude", "Ein Ort, an dem nur der Bundeskanzler entscheidet" }, "Die gewählte Volksvertretung, die Gesetze berät und beschließt",
            "Ein Parlament (z.B. der Bundestag) besteht aus gewählten Abgeordneten, die Gesetze beraten, ändern und beschließen."),
        ("Was ist ein Klassenrat in der Schule ein Beispiel für?", new[] { "Mitbestimmung im Alltag durch gemeinsame Beratung und Entscheidung", "Eine Form der Diktatur", "Eine reine Vorlesung ohne Mitsprache" }, "Mitbestimmung im Alltag durch gemeinsame Beratung und Entscheidung",
            "Im Klassenrat können Schülerinnen und Schüler gemeinsam Themen besprechen und mitentscheiden - ein alltagsnahes Beispiel für Mitbestimmung."),
        ("Was bedeutet \"Wahlrecht\" in einer modernen Demokratie?", new[] { "Das Recht, bei Wahlen die eigene Stimme abzugeben", "Die Pflicht, für eine bestimmte Partei zu stimmen", "Ein Vorrecht nur für bestimmte Berufsgruppen" }, "Das Recht, bei Wahlen die eigene Stimme abzugeben",
            "Das Wahlrecht gibt Bürgerinnen und Bürgern das Recht, bei politischen Wahlen ihre Stimme abzugeben."),
        ("Was ist ein Beispiel für einen Interessenkonflikt im lokalen Umfeld, wie er in einer Demokratie gelöst werden muss?", new[] { "Streit um die Nutzung einer Fläche, z.B. für einen Spielplatz oder einen Parkplatz", "Zwei Personen mögen dasselbe Buch", "Ein Streit ohne jede Lösungsmöglichkeit" }, "Streit um die Nutzung einer Fläche, z.B. für einen Spielplatz oder einen Parkplatz",
            "Interessenkonflikte, etwa um die Nutzung einer Fläche für Spielplatz oder Parkplatz, müssen in einer Demokratie durch Diskussion und Abstimmung gelöst werden."),
        ("Was bedeutet \"Mehrheitsprinzip\" bei demokratischen Abstimmungen?", new[] { "Die Meinung, die die meisten Stimmen erhält, setzt sich in der Regel durch", "Nur eine einzige Person entscheidet immer allein", "Abstimmungsergebnisse werden zufällig ausgelost" }, "Die Meinung, die die meisten Stimmen erhält, setzt sich in der Regel durch",
            "Nach dem Mehrheitsprinzip setzt sich bei einer Abstimmung in der Regel die Position durch, die die meisten Stimmen erhalten hat."),
        ("Warum ist der Minderheitenschutz in einer Demokratie trotz Mehrheitsprinzip wichtig?", new[] { "Auch die Rechte und Interessen von Menschen in der Minderheit müssen geachtet werden", "Minderheiten haben in einer Demokratie keine Rechte", "Minderheitenschutz widerspricht grundsätzlich der Demokratie" }, "Auch die Rechte und Interessen von Menschen in der Minderheit müssen geachtet werden",
            "Damit die Mehrheit nicht über die Rechte Einzelner hinweggeht, schützt eine funktionierende Demokratie auch die Interessen von Minderheiten."),
        ("Was unterscheidet eine Demokratie grundsätzlich von einer Diktatur?", new[] { "In einer Demokratie kann die Bevölkerung die Regierung wählen und kontrollieren", "In einer Diktatur haben alle Bürger gleich viel Mitspracherecht", "Beide Regierungsformen sind identisch" }, "In einer Demokratie kann die Bevölkerung die Regierung wählen und kontrollieren",
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
        ("Warum gilt Gleichberechtigung als wichtiges Ziel moderner Demokratien im Unterschied zur athenischen Demokratie?", new[] { "Heute sollen alle Menschen unabhängig von Geschlecht oder Herkunft gleiche politische Rechte haben", "Gleichberechtigung war in Athen bereits vollständig verwirklicht", "Gleichberechtigung spielt in Demokratien keine Rolle" }, "Heute sollen alle Menschen unabhängig von Geschlecht oder Herkunft gleiche politische Rechte haben",
            "Anders als im antiken Athen, wo nur ein Teil der Bevölkerung Rechte hatte, streben moderne Demokratien nach gleichen politischen Rechten für alle Menschen."),
        ("Was bedeutet das Losverfahren, das im antiken Athen für manche öffentliche Ämter genutzt wurde?", new[] { "Bestimmte Ämter wurden per Zufall unter den Bürgern verlost statt gewählt", "Alle Ämter wurden vererbt", "Ämter wurden ausschließlich durch Reichtum vergeben" }, "Bestimmte Ämter wurden per Zufall unter den Bürgern verlost statt gewählt",
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
        ("Was ist das Grundgesetz?", new[] { "Die Verfassung der Bundesrepublik Deutschland", "Ein einfaches Gesetz zum Straßenverkehr", "Eine Regel nur für Berlin" },
            "Die Verfassung der Bundesrepublik Deutschland", "Das Grundgesetz von 1949 ist die Verfassung Deutschlands und steht über allen anderen Gesetzen."),
        ("Welcher Artikel des Grundgesetzes garantiert die Würde des Menschen?", new[] { "Artikel 1", "Artikel 20", "Artikel 100" }, "Artikel 1",
            "Artikel 1 des Grundgesetzes lautet: \"Die Würde des Menschen ist unantastbar.\""),
        ("Seit wann gilt das Grundgesetz für ganz Deutschland (nach der Wiedervereinigung)?", new[] { "Seit 1990", "Seit 1949", "Seit 1945" }, "Seit 1990",
            "Das Grundgesetz galt ab 1949 zunächst nur für die Bundesrepublik (Westdeutschland) und gilt seit der Wiedervereinigung 1990 für ganz Deutschland."),
        ("Was bedeutet \"Gewaltenteilung\" im Grundgesetz?", new[] { "Staatsmacht ist auf Gesetzgebung, Regierung und Gerichte aufgeteilt", "Nur eine Person entscheidet über alles", "Es gibt gar keine Regeln"}, "Staatsmacht ist auf Gesetzgebung, Regierung und Gerichte aufgeteilt",
            "Die Gewaltenteilung verteilt die Macht auf Legislative (Parlament), Exekutive (Regierung) und Judikative (Gerichte), damit keine zu mächtig wird."),
        ("Was sind laut Grundgesetz die tragenden Prinzipien des deutschen Staates?", new[] { "Demokratie, Rechtsstaat, Sozialstaat, Bundesstaat", "Ein Kaiser entscheidet allein", "Es gibt gar keine festen Prinzipien" }, "Demokratie, Rechtsstaat, Sozialstaat, Bundesstaat",
            "Artikel 20 GG legt fest, dass Deutschland ein demokratischer, sozialer Rechtsstaat und Bundesstaat ist."),
        ("In welchem Jahr wurde das Grundgesetz verkündet?", new[] { "1949", "1918", "1990" }, "1949",
            "Das Grundgesetz wurde am 23. Mai 1949 verkündet und trat als Verfassung der Bundesrepublik Deutschland in Kraft."),
        ("Was garantiert Artikel 3 des Grundgesetzes?", new[] { "Die Gleichheit aller Menschen vor dem Gesetz", "Das Recht auf ein eigenes Auto", "Die Steuerpflicht" }, "Die Gleichheit aller Menschen vor dem Gesetz",
            "Artikel 3 des Grundgesetzes garantiert die Gleichheit aller Menschen vor dem Gesetz und verbietet Diskriminierung."),
        ("Was schützt Artikel 5 des Grundgesetzes besonders?", new[] { "Meinungsfreiheit und Pressefreiheit", "Nur das Eigentum", "Nur den Straßenverkehr" }, "Meinungsfreiheit und Pressefreiheit",
            "Artikel 5 des Grundgesetzes schützt die Meinungsfreiheit, die Pressefreiheit und die Freiheit von Kunst und Wissenschaft."),
        ("Was ist eine \"Ewigkeitsklausel\" im Grundgesetz (Art. 79 Abs. 3)?", new[] { "Bestimmte Grundprinzipien wie die Menschenwürde dürfen nie geändert werden", "Das Grundgesetz gilt nur für 100 Jahre", "Alle Artikel können beliebig geändert werden" }, "Bestimmte Grundprinzipien wie die Menschenwürde dürfen nie geändert werden",
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
        ("Was regelt Artikel 20a des Grundgesetzes seit 1994?", new[] { "Den Schutz der natürlichen Lebensgrundlagen (Umweltschutz)", "Das Recht auf ein Auto", "Die Steuerhöhe" }, "Den Schutz der natürlichen Lebensgrundlagen (Umweltschutz)",
            "Seit 1994 verpflichtet Artikel 20a den Staat, die natürlichen Lebensgrundlagen auch für künftige Generationen zu schützen."),
        ("Was ist der Unterschied zwischen Grundgesetz und einfachem Gesetz?", new[] { "Das Grundgesetz steht als Verfassung über allen einfachen Gesetzen", "Beide stehen auf derselben Stufe", "Einfache Gesetze stehen über dem Grundgesetz" }, "Das Grundgesetz steht als Verfassung über allen einfachen Gesetzen",
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
        ("Was beschreibt der einfache Wirtschaftskreislauf zwischen Haushalten und Unternehmen?", new[] { "Haushalte bieten Arbeitskraft, Unternehmen zahlen Lohn dafür", "Nur der Staat verteilt Geld", "Unternehmen bekommen nichts von Haushalten" },
            "Haushalte bieten Arbeitskraft, Unternehmen zahlen Lohn dafür",
            "Im einfachen Wirtschaftskreislauf tauschen private Haushalte und Unternehmen Arbeitskraft/Güter gegen Geld (Lohn/Erlöse)."),
        ("Was passiert bei Inflation?", new[] { "Die Preise steigen allgemein, Geld verliert an Wert", "Die Preise sinken dauerhaft", "Nichts ändert sich" },
            "Die Preise steigen allgemein, Geld verliert an Wert", "Bei Inflation steigt das allgemeine Preisniveau, wodurch man sich für denselben Geldbetrag weniger leisten kann."),
        ("Welche Rolle spielt der Staat zusätzlich im erweiterten Wirtschaftskreislauf?", new[] { "Er erhebt Steuern und zahlt z.B. für Schulen und Straßen", "Er hat gar keine Rolle", "Er verbietet jeden Handel" },
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
        ("Was bedeutet \"Lohn\" im Wirtschaftskreislauf?", new[] { "Bezahlung, die Beschäftigte für ihre Arbeitskraft erhalten", "Steuern, die der Staat einzieht", "Der Preis für Rohstoffe" },
            "Bezahlung, die Beschäftigte für ihre Arbeitskraft erhalten", "Der Lohn ist die Bezahlung, die Beschäftigte von Unternehmen für ihre geleistete Arbeit erhalten."),
        ("Was passiert bei einer Wirtschaftskrise häufig mit der Arbeitslosigkeit?", new[] { "Sie steigt meist an", "Sie sinkt immer", "Sie bleibt unverändert" },
            "Sie steigt meist an", "In Wirtschaftskrisen produzieren und verkaufen Unternehmen oft weniger, wodurch häufig Arbeitsplätze wegfallen und die Arbeitslosigkeit steigt."),
        ("Was bedeutet \"Sparen\" für private Haushalte im Wirtschaftskreislauf?", new[] { "Einen Teil des Einkommens nicht ausgeben, sondern zurücklegen", "Das gesamte Einkommen sofort ausgeben", "Kein Einkommen erhalten" },
            "Einen Teil des Einkommens nicht ausgeben, sondern zurücklegen", "Beim Sparen legen private Haushalte einen Teil ihres Einkommens zurück, statt ihn sofort auszugeben."),
        ("Was ist ein \"Kredit\"?", new[] { "Geliehenes Geld, das mit Zinsen zurückgezahlt werden muss", "Geschenktes Geld ohne Rückzahlung", "Eine Steuer" },
            "Geliehenes Geld, das mit Zinsen zurückgezahlt werden muss", "Ein Kredit ist Geld, das man sich z.B. von einer Bank leiht und später mit Zinsen zurückzahlen muss."),
        ("Was bedeutet \"Zinsen\" bei einem Kredit oder Sparguthaben?", new[] { "Ein zusätzlicher Betrag, der für das Verleihen oder Anlegen von Geld gezahlt wird", "Ein fester Betrag, den man einmalig zahlt", "Eine staatliche Strafe" },
            "Ein zusätzlicher Betrag, der für das Verleihen oder Anlegen von Geld gezahlt wird", "Zinsen sind der Preis dafür, dass jemand Geld verleiht oder anlegt - sie werden meist als Prozentsatz berechnet."),
        ("Was ist der Unterschied zwischen Brutto- und Nettolohn?", new[] { "Brutto ist der Lohn vor Abzügen, Netto der Betrag nach Steuern und Abgaben", "Beides ist immer identisch", "Netto ist immer höher als Brutto" },
            "Brutto ist der Lohn vor Abzügen, Netto der Betrag nach Steuern und Abgaben", "Der Bruttolohn ist der volle Lohn vor Steuern und Sozialabgaben, der Nettolohn der Betrag, der danach übrig bleibt."),
        ("Wofür verwendet der Staat die Einnahmen aus Steuern typischerweise?", new[] { "Für öffentliche Aufgaben wie Schulen, Straßen, Gesundheit und Sicherheit", "Nur für private Zwecke von Politikern", "Steuern werden nie verwendet" },
            "Für öffentliche Aufgaben wie Schulen, Straßen, Gesundheit und Sicherheit", "Mit Steuereinnahmen finanziert der Staat öffentliche Aufgaben wie Bildung, Infrastruktur, Gesundheit und Sicherheit."),
        ("Was bedeutet \"Marktwirtschaft\"?", new[] { "Preise und Produktion regeln sich vor allem über Angebot und Nachfrage", "Der Staat legt alle Preise fest", "Es gibt keinerlei Handel" },
            "Preise und Produktion regeln sich vor allem über Angebot und Nachfrage", "In einer Marktwirtschaft bestimmen vor allem Angebot und Nachfrage, was produziert wird und zu welchem Preis."),
        ("Was bedeutet \"soziale Marktwirtschaft\" in Deutschland?", new[] { "Freier Markt kombiniert mit staatlicher sozialer Absicherung", "Ausschließlich staatlich geplante Wirtschaft", "Wirtschaft ganz ohne staatliche Regeln" },
            "Freier Markt kombiniert mit staatlicher sozialer Absicherung", "Die soziale Marktwirtschaft verbindet freien Wettbewerb mit staatlicher sozialer Absicherung, z.B. durch Sozialversicherungen."),
        ("Was passiert mit Preisen, wenn das Angebot eines Produkts stark steigt, die Nachfrage aber gleich bleibt?", new[] { "Der Preis sinkt meist", "Der Preis steigt meist", "Der Preis bleibt garantiert gleich" },
            "Der Preis sinkt meist", "Steigt das Angebot bei gleichbleibender Nachfrage, sinkt in der Marktwirtschaft meist der Preis."),
        ("Was ist die Aufgabe von Banken im Wirtschaftskreislauf?", new[] { "Sie verwalten Geld, vergeben Kredite und ermöglichen Zahlungsverkehr", "Sie produzieren nur Lebensmittel", "Sie erheben Steuern für den Staat" },
            "Sie verwalten Geld, vergeben Kredite und ermöglichen Zahlungsverkehr", "Banken verwalten Spareinlagen, vergeben Kredite an Haushalte und Unternehmen und wickeln den Zahlungsverkehr ab."),
        ("Was bedeutet \"Export\" für die Wirtschaft eines Landes?", new[] { "Waren und Dienstleistungen werden ins Ausland verkauft", "Waren werden nur im eigenen Land verbraucht", "Es findet gar kein Handel statt" },
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
        ("Was ist eine wichtige Aufgabe von Medien in einer Demokratie?", new[] { "Die Öffentlichkeit informieren und die Regierung kontrollieren", "Nur Werbung zu zeigen", "Immer nur eine Meinung zu vertreten" },
            "Die Öffentlichkeit informieren und die Regierung kontrollieren",
            "Medien werden oft als \"vierte Gewalt\" bezeichnet, weil sie die Öffentlichkeit informieren und Politik/Wirtschaft kritisch beobachten."),
        ("Was sollte man tun, bevor man eine Nachricht aus dem Internet weiterverbreitet?", new[] { "Die Quelle und Fakten prüfen", "Sofort teilen, ohne nachzudenken", "Nur die Überschrift lesen" },
            "Die Quelle und Fakten prüfen", "Um Falschmeldungen (Fake News) nicht zu verbreiten, sollte man Quelle und Fakten kritisch prüfen."),
        ("Was unterscheidet einen seriösen Nachrichtensender von einer unseriösen Quelle?", new[] { "Er nennt Quellen und trennt Meinung von Fakten", "Er hat die reißerischste Überschrift", "Er postet am schnellsten"}, "Er nennt Quellen und trennt Meinung von Fakten",
            "Seriöser Journalismus recherchiert sorgfältig, nennt Quellen und kennzeichnet klar, was Fakten und was Meinung ist."),
        ("Warum nennt man Medien manchmal die \"vierte Gewalt\" im Staat?", new[] { "Weil sie Politik und Wirtschaft öffentlich kontrollieren", "Weil sie über Gesetze abstimmen dürfen", "Weil sie Gerichtsurteile fällen" }, "Weil sie Politik und Wirtschaft öffentlich kontrollieren",
            "Neben Legislative, Exekutive und Judikative gelten Medien als \"vierte Gewalt\", weil sie durch Berichterstattung Machtmissbrauch aufdecken können."),
        ("Was unterscheidet soziale Medien von klassischen Nachrichtenmedien?", new[] { "Bei sozialen Medien kann im Prinzip jeder selbst Inhalte veröffentlichen", "Soziale Medien werden immer von Journalisten geprüft", "Es gibt keinen Unterschied" }, "Bei sozialen Medien kann im Prinzip jeder selbst Inhalte veröffentlichen",
            "Anders als bei klassischen Medien mit Redaktion kann bei sozialen Medien jede Person Inhalte posten - das erhöht das Risiko ungeprüfter Falschinformationen."),
        ("Was bedeutet \"Pressefreiheit\"?", new[] { "Medien dürfen frei und unabhängig vom Staat berichten", "Nur der Staat darf über Medien berichten", "Zeitungen kosten kein Geld" }, "Medien dürfen frei und unabhängig vom Staat berichten",
            "Pressefreiheit bedeutet, dass Medien unabhängig und ohne staatliche Zensur berichten dürfen."),
        ("Was ist ein \"Boulevardmedium\"?", new[] { "Ein Medium, das oft reißerisch und stark vereinfachend über Themen berichtet", "Ein rein wissenschaftliches Fachmagazin", "Ein staatliches Amtsblatt" }, "Ein Medium, das oft reißerisch und stark vereinfachend über Themen berichtet",
            "Boulevardmedien berichten oft besonders reißerisch, emotional und vereinfachend, um viele Leserinnen und Leser anzusprechen."),
        ("Was bedeutet \"Fake News\"?", new[] { "Bewusst verbreitete Falschmeldungen, die wie echte Nachrichten aussehen", "Nachrichten, die immer wahr sind", "Nachrichten, die nur im Fernsehen laufen" }, "Bewusst verbreitete Falschmeldungen, die wie echte Nachrichten aussehen",
            "Fake News sind bewusst erfundene oder verfälschte Meldungen, die wie echte Nachrichten wirken sollen."),
        ("Woran erkennt man oft eine unseriöse Quelle im Internet?", new[] { "Fehlende Quellenangaben und reißerische Übertreibungen", "Ein bekanntes Impressum mit Kontaktadresse", "Sachliche, nachprüfbare Fakten" }, "Fehlende Quellenangaben und reißerische Übertreibungen",
            "Unseriöse Quellen liefern oft keine überprüfbaren Quellen und übertreiben oder dramatisieren stark."),
        ("Was bedeutet \"Filterblase\" in sozialen Medien?", new[] { "Man bekommt hauptsächlich Inhalte gezeigt, die der eigenen Meinung entsprechen", "Man sieht automatisch alle Meinungen gleichermaßen", "Soziale Medien zeigen niemals persönliche Inhalte" }, "Man bekommt hauptsächlich Inhalte gezeigt, die der eigenen Meinung entsprechen",
            "In einer Filterblase bekommt man vor allem Inhalte angezeigt, die zur eigenen Meinung passen, wodurch andere Sichtweisen seltener vorkommen."),
        ("Was ist ein \"Algorithmus\" in sozialen Medien?", new[] { "Eine Regel, nach der eine Software auswählt, welche Inhalte angezeigt werden", "Ein Mensch, der jeden Beitrag persönlich prüft", "Ein Gesetz gegen Falschnachrichten" }, "Eine Regel, nach der eine Software auswählt, welche Inhalte angezeigt werden",
            "Algorithmen entscheiden automatisiert, welche Beiträge einer Nutzerin oder einem Nutzer angezeigt werden."),
        ("Was sollte man tun, bevor man einen Kommentar im Internet abschickt?", new[] { "Überlegen, ob der Kommentar respektvoll und wahr ist", "Sofort alles unüberlegt posten", "Immer möglichst beleidigend schreiben" }, "Überlegen, ob der Kommentar respektvoll und wahr ist",
            "Vor dem Absenden eines Kommentars sollte man kurz überlegen, ob er respektvoll, fair und inhaltlich richtig ist."),
        ("Was bedeutet \"Cybermobbing\"?", new[] { "Wiederholtes Beleidigen, Bedrohen oder Bloßstellen einer Person über digitale Medien", "Ein freundlicher Chat zwischen Freunden", "Ein technischer Fehler im Internet" }, "Wiederholtes Beleidigen, Bedrohen oder Bloßstellen einer Person über digitale Medien",
            "Cybermobbing bezeichnet wiederholtes Beleidigen, Bedrohen oder Bloßstellen einer Person über das Internet oder digitale Geräte."),
        ("Warum ist es wichtig, im Internet mehrere Quellen zu einer Nachricht zu vergleichen?", new[] { "Um Fehler oder bewusste Falschinformationen einzelner Quellen zu erkennen", "Weil alle Quellen sowieso immer identisch berichten", "Es ist nicht wichtig" }, "Um Fehler oder bewusste Falschinformationen einzelner Quellen zu erkennen",
            "Der Vergleich mehrerer Quellen hilft, Fehler oder bewusste Falschinformationen einzelner Berichte zu erkennen."),
        ("Was ist der öffentlich-rechtliche Rundfunk (z.B. ARD, ZDF) in Deutschland?", new[] { "Ein durch Rundfunkbeitrag finanziertes, unabhängiges Mediensystem mit Informationsauftrag", "Ein rein privates Gewinn-Unternehmen", "Ein staatlich kontrolliertes Propagandamedium" }, "Ein durch Rundfunkbeitrag finanziertes, unabhängiges Mediensystem mit Informationsauftrag",
            "ARD und ZDF werden über den Rundfunkbeitrag finanziert und sind zu unabhängiger, ausgewogener Berichterstattung verpflichtet."),
        ("Was bedeutet Medienkompetenz?", new[] { "Die Fähigkeit, Medieninhalte kritisch einzuordnen und sicher zu nutzen", "Möglichst viele Stunden am Handy zu verbringen", "Nur Inhalte zu glauben, die viele Likes haben" }, "Die Fähigkeit, Medieninhalte kritisch einzuordnen und sicher zu nutzen",
            "Medienkompetenz bedeutet, Medieninhalte kritisch beurteilen und Medien verantwortungsvoll nutzen zu können."),
        ("Warum kennzeichnen seriöse Medien manchmal Beiträge als \"Meinung\" oder \"Kommentar\"?", new[] { "Um Fakten klar von persönlichen Einschätzungen zu trennen", "Um Leser bewusst zu verwirren", "Weil das gesetzlich verboten ist" }, "Um Fakten klar von persönlichen Einschätzungen zu trennen",
            "Die Kennzeichnung als Meinung oder Kommentar hilft, sachliche Fakten klar von persönlichen Einschätzungen zu unterscheiden."),
        ("Was kann übermäßiger Konsum von sozialen Medien bei Kindern und Jugendlichen begünstigen?", new[] { "Schlafmangel und Konzentrationsprobleme", "Automatisch bessere Schulnoten", "Keinerlei Auswirkungen" }, "Schlafmangel und Konzentrationsprobleme",
            "Zu viel Bildschirmzeit, besonders spätabends, kann Schlaf und Konzentrationsfähigkeit von Kindern und Jugendlichen beeinträchtigen."),
        ("Was sollte man tun, wenn man online gemobbt wird oder Mobbing beobachtet?", new[] { "Sich Erwachsenen oder Vertrauenspersonen anvertrauen und Beweise sichern", "Es niemandem erzählen", "Selbst zurückmobben" }, "Sich Erwachsenen oder Vertrauenspersonen anvertrauen und Beweise sichern",
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
}
