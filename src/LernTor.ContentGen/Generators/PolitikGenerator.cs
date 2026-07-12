using LernTor.Core.Enums;
using LernTor.Core.Models;

namespace LernTor.ContentGen.Generators;

/// <summary>Politik/politische Bildung nach Berliner Rahmenlehrplan, Klasse 6 und Klasse 9.</summary>
public sealed class PolitikGenerator : ExerciseGeneratorBase
{
    public override Subject Subject => Subject.Politik;

    protected override IReadOnlyDictionary<GradeLevel, IReadOnlyList<TopicFactory>> TopicsByGrade { get; } =
        new Dictionary<GradeLevel, IReadOnlyList<TopicFactory>>
        {
            [GradeLevel.Klasse6] = new List<TopicFactory> { Demokratie, BerlinBezirke, Wahlrecht },
            [GradeLevel.Klasse9] = new List<TopicFactory> { Gewaltenteilung, BundestagBundesrat, Wahlsystem, SozialeMarktwirtschaft }
        };

    private static readonly (string Frage, string[] Optionen, string Antwort, string Erklaerung)[] DemokratieListe =
    {
        ("Was bedeutet \"Demokratie\" ganz einfach erklärt?", new[] { "Herrschaft des Volkes - alle dürfen mitbestimmen", "Herrschaft eines einzelnen Königs", "Herrschaft des Militärs" },
            "Herrschaft des Volkes - alle dürfen mitbestimmen", "\"Demokratie\" kommt aus dem Griechischen und bedeutet \"Volksherrschaft\": Die Bürger bestimmen durch Wahlen mit."),
        ("Was ist ein wichtiges Merkmal einer Demokratie?", new[] { "Freie Wahlen und Meinungsfreiheit", "Nur eine erlaubte Partei", "Keine Gerichte" }, "Freie Wahlen und Meinungsfreiheit",
            "Freie, geheime Wahlen und die Freiheit, seine Meinung zu äußern, gehören zu den Grundpfeilern einer Demokratie."),
        ("Was bedeutet \"Gewaltenteilung\" in einer Demokratie ganz einfach?", new[] { "Macht wird auf mehrere unabhängige Institutionen verteilt", "Eine Person hat die gesamte Macht", "Es gibt gar keine Regeln" }, "Macht wird auf mehrere unabhängige Institutionen verteilt",
            "Gewaltenteilung verhindert, dass eine einzelne Person oder Gruppe zu viel Macht bekommt."),
        ("Warum ist eine freie Presse wichtig für eine Demokratie?", new[] { "Sie informiert die Bürger und kontrolliert die Mächtigen", "Sie darf nur Positives über die Regierung schreiben", "Sie ist unwichtig" }, "Sie informiert die Bürger und kontrolliert die Mächtigen",
            "Eine freie Presse berichtet unabhängig und deckt Missstände auf - das nennt man auch \"vierte Gewalt\"."),
        ("Was ist das Gegenteil einer Demokratie, in der eine Person oder Gruppe allein und unkontrolliert herrscht?", new[] { "Diktatur", "Monarchie mit Wahlrecht", "Bundesrepublik" }, "Diktatur",
            "In einer Diktatur trifft eine Person oder kleine Gruppe die Entscheidungen ohne freie Wahlen oder Kontrolle."),
        ("Was ist eine \"Wahl\" in einer Demokratie?", new[] { "Die Möglichkeit, durch Abstimmen zu entscheiden, wer regieren soll", "Eine Pflichtveranstaltung ohne echte Entscheidung", "Ein Fest ohne politische Bedeutung" }, "Die Möglichkeit, durch Abstimmen zu entscheiden, wer regieren soll",
            "Bei Wahlen entscheiden die Bürgerinnen und Bürger, welche Personen oder Parteien sie im Parlament/in der Regierung vertreten sollen."),
        ("Was ist eine \"Opposition\" im Parlament?", new[] { "Die Parteien, die nicht in der Regierung sind und diese kontrollieren", "Die regierende Partei selbst", "Die Gerichte" }, "Die Parteien, die nicht in der Regierung sind und diese kontrollieren",
            "Die Opposition übt Kritik an der Regierung und kontrolliert deren Arbeit - ein wichtiger Teil der Demokratie."),
        ("Warum ist Meinungsfreiheit wichtig in einer Demokratie?", new[] { "Jeder darf seine Meinung frei äußern und auch die Regierung kritisieren", "Nur die Regierung darf ihre Meinung sagen", "Meinungsfreiheit ist in Demokratien verboten" }, "Jeder darf seine Meinung frei äußern und auch die Regierung kritisieren",
            "Meinungsfreiheit erlaubt offene Debatten und Kritik an der Politik - ohne sie könnte Machtmissbrauch nicht aufgedeckt werden."),
        ("Was bedeutet \"Mehrheitsentscheidung\"?", new[] { "Die Meinung, die mehr Stimmen bekommt, setzt sich durch", "Die Meinung einer einzelnen Person zählt immer am meisten", "Es wird gar nicht abgestimmt" }, "Die Meinung, die mehr Stimmen bekommt, setzt sich durch",
            "Bei einer Mehrheitsentscheidung gewinnt die Option, für die die meisten Stimmen abgegeben wurden."),
        ("Was ist ein \"Rechtsstaat\"?", new[] { "Ein Staat, in dem sich alle - auch die Regierung - an die Gesetze halten müssen", "Ein Staat ohne jegliche Gesetze", "Ein Staat, in dem nur die Regierung Gesetze befolgen muss" }, "Ein Staat, in dem sich alle - auch die Regierung - an die Gesetze halten müssen",
            "Im Rechtsstaat gilt: Niemand steht über dem Gesetz, auch nicht Politikerinnen und Politiker."),
        ("Was können Bürgerinnen und Bürger tun, wenn sie mit einer politischen Entscheidung nicht einverstanden sind?", new[] { "Friedlich demonstrieren oder eine Petition einreichen", "Nur schweigen und nichts unternehmen", "Die Entscheidung heimlich sabotieren" }, "Friedlich demonstrieren oder eine Petition einreichen",
            "Demonstrationsrecht und das Recht auf Petitionen sind wichtige Wege, um politischen Unmut friedlich auszudrücken."),
        ("Was ist eine \"Petition\"?", new[] { "Eine schriftliche Bitte oder Beschwerde an ein Parlament oder eine Behörde", "Ein Gesetz, das sofort in Kraft tritt", "Ein Gerichtsurteil" }, "Eine schriftliche Bitte oder Beschwerde an ein Parlament oder eine Behörde",
            "Mit einer Petition können Bürgerinnen und Bürger ein Anliegen offiziell an ein Parlament oder eine Behörde richten."),
        ("Was bedeutet \"Minderheitenschutz\" in einer Demokratie?", new[] { "Auch Menschen mit einer anderen Meinung als die Mehrheit haben geschützte Rechte", "Nur die Mehrheit hat überhaupt Rechte", "Minderheiten dürfen nicht wählen" }, "Auch Menschen mit einer anderen Meinung als die Mehrheit haben geschützte Rechte",
            "Demokratie bedeutet nicht nur \"die Mehrheit entscheidet\" - die Rechte von Minderheiten müssen dabei ebenfalls geschützt bleiben."),
        ("Was ist eine \"Koalition\" in der Politik?", new[] { "Ein Zusammenschluss mehrerer Parteien, um gemeinsam eine Regierung zu bilden", "Eine einzelne, besonders große Partei", "Ein anderes Wort für Opposition" }, "Ein Zusammenschluss mehrerer Parteien, um gemeinsam eine Regierung zu bilden",
            "Erreicht keine Partei allein die Mehrheit, schließen sich oft mehrere Parteien zu einer Koalition zusammen, um gemeinsam zu regieren."),
        ("Warum gibt es in einer Demokratie meist mehrere Parteien zur Auswahl?", new[] { "Damit unterschiedliche Meinungen und Interessen vertreten werden können", "Weil eine einzige Partei gesetzlich verboten ist", "Damit niemand wählen gehen muss" }, "Damit unterschiedliche Meinungen und Interessen vertreten werden können",
            "Verschiedene Parteien stehen für unterschiedliche politische Ideen - Wählerinnen und Wähler können so zwischen echten Alternativen entscheiden."),
        ("Was ist ein \"Verein\", und warum ist er ein Beispiel für Mitbestimmung in der Gesellschaft?", new[] { "Ein freiwilliger Zusammenschluss von Menschen, der z.B. demokratisch seinen Vorstand wählt", "Eine staatliche Behörde", "Eine Firma, die Gewinn machen muss" }, "Ein freiwilliger Zusammenschluss von Menschen, der z.B. demokratisch seinen Vorstand wählt",
            "Vereine organisieren sich oft selbst demokratisch, z.B. durch die Wahl eines Vorstands in der Mitgliederversammlung."),
        ("Was bedeutet das Wort \"Grundgesetz\" für Deutschland?", new[] { "Die Verfassung, das oberste Gesetz Deutschlands", "Ein einfaches Gesetz zum Straßenverkehr", "Eine Regel nur für Schulen" }, "Die Verfassung, das oberste Gesetz Deutschlands",
            "Das Grundgesetz steht über allen anderen Gesetzen und legt die Grundordnung des deutschen Staates fest."),
        ("Was ist eine \"Demonstration\"?", new[] { "Eine öffentliche, meist friedliche Versammlung, um eine Meinung oder Forderung zu zeigen", "Ein geheimes Treffen ohne Öffentlichkeit", "Eine Art Gerichtsverhandlung" }, "Eine öffentliche, meist friedliche Versammlung, um eine Meinung oder Forderung zu zeigen",
            "Das Versammlungsrecht erlaubt es, öffentlich und friedlich für oder gegen politische Themen zu demonstrieren."),
        ("Warum ist es wichtig, dass sich auch Kinder und Jugendliche für Politik interessieren?", new[] { "Weil politische Entscheidungen auch ihre eigene Zukunft betreffen", "Weil das gesetzlich vorgeschrieben ist", "Weil Politik nur Erwachsene betrifft" }, "Weil politische Entscheidungen auch ihre eigene Zukunft betreffen",
            "Themen wie Bildung, Umwelt oder Digitalisierung werden politisch entschieden und wirken sich direkt auf das Leben junger Menschen aus."),
        ("Was unterscheidet eine repräsentative Demokratie (wie in Deutschland) von direkter Demokratie?", new[] { "In der repräsentativen Demokratie wählt man Vertreter, die entscheiden; direkte Demokratie lässt das Volk direkt abstimmen", "Beide Formen sind exakt identisch", "In der repräsentativen Demokratie gibt es gar keine Wahlen" }, "In der repräsentativen Demokratie wählt man Vertreter, die entscheiden; direkte Demokratie lässt das Volk direkt abstimmen",
            "In Deutschland (repräsentative Demokratie) entscheiden gewählte Abgeordnete stellvertretend, während bei direkter Demokratie (z.B. Volksabstimmung) das Volk selbst über einzelne Fragen abstimmt.")
    };

    private static QuizQuestion Demokratie(Random r)
    {
        var f = DemokratieListe[r.Next(DemokratieListe.Length)];
        return new QuizQuestion
        {
            Id = NewId(), Subject = Subject.Politik, GradeLevel = GradeLevel.Klasse6,
            Topic = "Was ist Demokratie?", Type = QuestionType.MultipleChoice,
            Prompt = f.Frage, Options = f.Optionen, CorrectAnswers = new[] { f.Antwort }, Explanation = f.Erklaerung,
            HelpHint = "\"Demokratie\" = \"Volksherrschaft\" (griechisch) - freie Wahlen und Meinungsfreiheit gehören zu ihren Grundpfeilern."
        };
    }

    private static readonly (string Frage, string[] Optionen, string Antwort, string Erklaerung)[] BezirkeListe =
    {
        ("Wie viele Bezirke hat Berlin?", new[] { "12", "8", "20" }, "12", "Berlin ist in 12 Bezirke aufgeteilt, z.B. Mitte, Pankow, Neukölln oder Spandau."),
        ("Wer leitet einen Berliner Bezirk?", new[] { "Ein Bezirksbürgermeister/eine Bezirksbürgermeisterin", "Der Bundeskanzler", "Ein König" }, "Ein Bezirksbürgermeister/eine Bezirksbürgermeisterin",
            "Jeder Berliner Bezirk hat ein eigenes Bezirksamt, das von einem Bezirksbürgermeister bzw. einer Bezirksbürgermeisterin geleitet wird."),
        ("Wie heißt das Berliner Stadtparlament, das für ganz Berlin zuständig ist?", new[] { "Abgeordnetenhaus", "Bundestag", "Stadtrat" }, "Abgeordnetenhaus",
            "Das Abgeordnetenhaus von Berlin ist das Landesparlament und beschließt Gesetze für ganz Berlin."),
        ("Wer ist der/die Regierungschef/in von ganz Berlin?", new[] { "Regierender Bürgermeister/Regierende Bürgermeisterin", "Bundeskanzler/in", "Bezirksbürgermeister/in" }, "Regierender Bürgermeister/Regierende Bürgermeisterin",
            "Berlin wird von einem/einer Regierenden Bürgermeister/in geleitet - er/sie ist zugleich Regierungschef/in des Bundeslandes Berlin."),
        ("Berlin ist gleichzeitig eine Stadt und ein...?", new[] { "Bundesland", "Bezirk", "Dorf" }, "Bundesland",
            "Berlin ist ein Stadtstaat: Stadt und Bundesland zugleich, so wie Hamburg und Bremen."),
        ("In welchem Bezirk liegen der Fernsehturm und der Alexanderplatz?", new[] { "Mitte", "Spandau", "Neukölln" }, "Mitte",
            "Der Fernsehturm und der Alexanderplatz liegen im historischen Zentrum Berlins, im Bezirk Mitte."),
        ("In welchem Bezirk liegt das Brandenburger Tor?", new[] { "Mitte", "Pankow", "Reinickendorf" }, "Mitte",
            "Das Brandenburger Tor steht ebenfalls im Bezirk Mitte, ganz in der Nähe des Reichstagsgebäudes."),
        ("In welchem Bezirk liegt der bekannte Kurfürstendamm (\"Ku'damm\")?", new[] { "Charlottenburg-Wilmersdorf", "Lichtenberg", "Marzahn-Hellersdorf" }, "Charlottenburg-Wilmersdorf",
            "Der Kurfürstendamm ist eine bekannte Einkaufsstraße im Bezirk Charlottenburg-Wilmersdorf."),
        ("In welchem Bezirk steht die historische Zitadelle Spandau?", new[] { "Spandau", "Treptow-Köpenick", "Tempelhof-Schöneberg" }, "Spandau",
            "Die Zitadelle Spandau ist eine der besterhaltenen Renaissance-Festungen Europas und liegt naheliegend im Bezirk Spandau."),
        ("In welchem Bezirk liegt das Tempelhofer Feld (ehemaliger Flughafen)?", new[] { "Tempelhof-Schöneberg", "Steglitz-Zehlendorf", "Friedrichshain-Kreuzberg" }, "Tempelhof-Schöneberg",
            "Das Tempelhofer Feld, heute eine große Grünfläche, war der frühere Flughafen Tempelhof im Bezirk Tempelhof-Schöneberg."),
        ("In welchem Bezirk lag der frühere Flughafen Tegel?", new[] { "Reinickendorf", "Mitte", "Neukölln" }, "Reinickendorf",
            "Der ehemalige Flughafen Berlin-Tegel liegt im Bezirk Reinickendorf."),
        ("In welchem Bezirk befindet sich die East Side Gallery (bemalte Mauerreste)?", new[] { "Friedrichshain-Kreuzberg", "Pankow", "Spandau" }, "Friedrichshain-Kreuzberg",
            "Die East Side Gallery, ein Stück bemalter Berliner Mauer, verläuft im Bezirk Friedrichshain-Kreuzberg."),
        ("In welchem Bezirk liegt der Müggelsee, der größte See Berlins?", new[] { "Treptow-Köpenick", "Charlottenburg-Wilmersdorf", "Lichtenberg" }, "Treptow-Köpenick",
            "Der Müggelsee, Berlins größter See, liegt im Bezirk Treptow-Köpenick."),
        ("Welcher Berliner Bezirk ist flächenmäßig der größte?", new[] { "Treptow-Köpenick", "Mitte", "Friedrichshain-Kreuzberg" }, "Treptow-Köpenick",
            "Treptow-Köpenick ist mit viel Wald- und Wasserfläche der flächenmäßig größte der 12 Berliner Bezirke."),
        ("Welcher Bezirk ist bekannt für seinen multikulturellen Wochenmarkt am Maybachufer?", new[] { "Neukölln", "Marzahn-Hellersdorf", "Steglitz-Zehlendorf" }, "Neukölln",
            "Der Markt am Maybachufer in Neukölln ist bekannt für sein multikulturelles Angebot, u.a. den sogenannten Türkenmarkt."),
        ("In welchem heutigen Bezirk liegt der Ortsteil Prenzlauer Berg?", new[] { "Pankow", "Mitte", "Reinickendorf" }, "Pankow",
            "Prenzlauer Berg wurde 2001 zusammen mit Weißensee und dem alten Pankow zum neuen Bezirk Pankow zusammengelegt."),
        ("Bei der Bezirksreform 2001 wurden Wedding und Tiergarten mit dem historischen Zentrum zu welchem neuen Bezirk zusammengelegt?", new[] { "Mitte", "Neukölln", "Spandau" }, "Mitte",
            "2001 wurden mehrere kleinere Bezirke zusammengelegt - Wedding und Tiergarten wurden Teil des neuen, größeren Bezirks Mitte."),
        ("Wie nennt man die kleineren Verwaltungseinheiten innerhalb eines Berliner Bezirks, z.B. Wedding oder Prenzlauer Berg?", new[] { "Ortsteile", "Bundesländer", "Landkreise" }, "Ortsteile",
            "Jeder Bezirk gliedert sich in mehrere Ortsteile - historisch gewachsene Stadtteile mit eigenem Namen und Charakter."),
        ("In welchem Bezirk steht das Olympiastadion (erbaut für die Olympischen Spiele 1936)?", new[] { "Charlottenburg-Wilmersdorf", "Lichtenberg", "Treptow-Köpenick" }, "Charlottenburg-Wilmersdorf",
            "Das Olympiastadion liegt im Ortsteil Westend, der zum Bezirk Charlottenburg-Wilmersdorf gehört."),
        ("In welchem Bezirk befinden sich die \"Gärten der Welt\", ein großer Themenpark mit Gärten aus aller Welt?", new[] { "Marzahn-Hellersdorf", "Steglitz-Zehlendorf", "Friedrichshain-Kreuzberg" }, "Marzahn-Hellersdorf",
            "Die Gärten der Welt liegen im Bezirk Marzahn-Hellersdorf und zeigen Gartenkunst aus verschiedenen Ländern und Kulturen.")
    };

    private static QuizQuestion BerlinBezirke(Random r)
    {
        var f = BezirkeListe[r.Next(BezirkeListe.Length)];
        return new QuizQuestion
        {
            Id = NewId(), Subject = Subject.Politik, GradeLevel = GradeLevel.Klasse6,
            Topic = "Berlin und seine Bezirke", Type = QuestionType.MultipleChoice,
            Prompt = f.Frage, Options = f.Optionen, CorrectAnswers = new[] { f.Antwort }, Explanation = f.Erklaerung,
            HelpHint = "Berlin hat 12 Bezirke, jeder mit eigenem Bezirksamt und Bezirksbürgermeister/in."
        };
    }

    private static QuizQuestion Wahlrecht(Random r)
    {
        var varianten = new (string Frage, string[] Optionen, string Antwort, string Erklaerung)[]
        {
            ("Ab welchem Alter darf man in Deutschland bei der Bundestagswahl wählen (aktives Wahlrecht)?", new[] { "18 Jahre", "16 Jahre", "21 Jahre" }, "18 Jahre",
                "Bei der Bundestagswahl darf ab 18 Jahren gewählt werden (bei manchen Kommunal-/Landtagswahlen z.T. schon ab 16)."),
            ("Ab welchem Alter darf man sich in Deutschland als Bundestagsabgeordnete/r wählen lassen (passives Wahlrecht)?", new[] { "18 Jahre", "25 Jahre", "30 Jahre" }, "18 Jahre",
                "Wählbar in den Bundestag ist, wer volljährig ist - also ab 18 Jahren, genau wie beim aktiven Wahlrecht."),
            ("Wie oft finden in Deutschland regulär Bundestagswahlen statt?", new[] { "Alle 4 Jahre", "Jedes Jahr", "Alle 10 Jahre" }, "Alle 4 Jahre",
                "Der Bundestag wird regulär alle vier Jahre neu gewählt."),
            ("Was bedeutet \"allgemeines Wahlrecht\"?", new[] { "Grundsätzlich darf jede/r Bürger/in ab einem bestimmten Alter wählen", "Nur bestimmte Berufsgruppen dürfen wählen", "Nur Männer dürfen wählen" }, "Grundsätzlich darf jede/r Bürger/in ab einem bestimmten Alter wählen",
                "\"Allgemein\" bedeutet: Es gibt keine Einschränkungen nach Geschlecht, Herkunft oder Vermögen - nur das Alter zählt."),
            ("Was macht man, wenn man am Wahltag nicht ins Wahllokal gehen kann, aber trotzdem wählen möchte?", new[] { "Briefwahl beantragen", "Man darf dann gar nicht wählen", "Ein Nachbar wählt für einen mit" }, "Briefwahl beantragen",
                "Mit der Briefwahl kann man vor dem Wahltag per Post wählen, wenn man am Wahltag selbst nicht ins Wahllokal kann."),
            ("Muss man in Deutschland zur Wahl gehen (Wahlpflicht)?", new[] { "Nein, wählen ist ein Recht, keine Pflicht", "Ja, es ist gesetzlich Pflicht", "Nur bei der Bundestagswahl ist es Pflicht" }, "Nein, wählen ist ein Recht, keine Pflicht",
                "In Deutschland gibt es keine Wahlpflicht - wählen zu gehen ist ein Recht, das man auch nicht wahrnehmen kann."),
            ("Was ist eine \"Wahlbenachrichtigung\"?", new[] { "Ein Schreiben, das informiert, wo und wann man wählen kann", "Der amtliche Stimmzettel selbst", "Eine Einladung zum Wahlkampf" }, "Ein Schreiben, das informiert, wo und wann man wählen kann",
                "Vor einer Wahl erhalten alle Wahlberechtigten eine Wahlbenachrichtigung mit Ort und Zeit ihres Wahllokals."),
            ("Was macht man in der Wahlkabine?", new[] { "Man kreuzt geheim und ungestört seine Stimme auf dem Stimmzettel an", "Man diskutiert laut mit anderen über die Wahl", "Man zeigt anderen, wen man wählt" }, "Man kreuzt geheim und ungestört seine Stimme auf dem Stimmzettel an",
                "Die Wahlkabine garantiert das Wahlgeheimnis: Niemand kann sehen, wie man abgestimmt hat."),
            ("Wer zählt am Wahlabend die abgegebenen Stimmen aus?", new[] { "Ehrenamtliche Wahlhelferinnen und Wahlhelfer", "Nur die Bundeskanzlerin/der Bundeskanzler persönlich", "Ein einzelner Computer ohne Aufsicht" }, "Ehrenamtliche Wahlhelferinnen und Wahlhelfer",
                "Die Stimmenauszählung übernehmen ehrenamtliche Wahlhelferinnen und Wahlhelfer öffentlich und nachvollziehbar."),
            ("Was passiert mit einem falsch oder mehrdeutig ausgefüllten Stimmzettel?", new[] { "Er wird als ungültig gewertet und zählt nicht", "Er zählt automatisch doppelt", "Er wird der Regierungspartei zugerechnet" }, "Er wird als ungültig gewertet und zählt nicht",
                "Ungültige Stimmzettel (z.B. mit mehreren Kreuzen) fließen nicht in das Wahlergebnis ein."),
            ("Was bedeutet \"Wahlbeteiligung\"?", new[] { "Der Anteil der Wahlberechtigten, die tatsächlich gewählt haben", "Die Anzahl der Parteien bei einer Wahl", "Die Anzahl der Wahlplakate" }, "Der Anteil der Wahlberechtigten, die tatsächlich gewählt haben",
                "Die Wahlbeteiligung zeigt, wie viel Prozent aller Wahlberechtigten von ihrem Wahlrecht Gebrauch gemacht haben."),
            ("Ab welchem Alter darf man in Berlin bei der Wahl zum Abgeordnetenhaus wählen?", new[] { "16 Jahre", "18 Jahre", "21 Jahre" }, "16 Jahre",
                "Seit einer Verfassungsänderung dürfen in Berlin bereits 16-Jährige das Abgeordnetenhaus und die Bezirksverordnetenversammlung mitwählen."),
            ("Was ist ein \"Stimmzettel\"?", new[] { "Das Papier, auf dem man bei einer Wahl seine Stimme(n) abgibt", "Ein Werbeplakat einer Partei", "Ein Brief des Bundeskanzlers" }, "Das Papier, auf dem man bei einer Wahl seine Stimme(n) abgibt",
                "Auf dem Stimmzettel kreuzt man in der Wahlkabine die gewünschte(n) Partei(en)/Person(en) an."),
            ("Was versteht man unter \"Wahlkampf\"?", new[] { "Die Zeit vor einer Wahl, in der Parteien um Stimmen werben", "Ein Streit zwischen Wählern im Wahllokal", "Eine sportliche Veranstaltung" }, "Die Zeit vor einer Wahl, in der Parteien um Stimmen werben",
                "Im Wahlkampf stellen Parteien ihre Programme vor, z.B. durch Plakate, Reden und Fernsehdebatten."),
            ("Was ist ein \"Wahlkreis\"?", new[] { "Ein geografisch abgegrenztes Gebiet, das eine Person direkt in den Bundestag entsenden kann", "Ein anderes Wort für Bundesland", "Ein Fachbegriff für eine politische Partei" }, "Ein geografisch abgegrenztes Gebiet, das eine Person direkt in den Bundestag entsenden kann",
                "Deutschland ist in Wahlkreise eingeteilt - in jedem Wahlkreis zieht die Person mit den meisten Erststimmen direkt in den Bundestag ein."),
            ("Warum ist das Wahlgeheimnis (geheime Wahl) wichtig?", new[] { "Damit niemand unter Druck gesetzt werden kann, in eine bestimmte Richtung zu wählen", "Damit Wahlergebnisse geheim bleiben und nie veröffentlicht werden", "Weil das Zählen sonst zu lange dauern würde" }, "Damit niemand unter Druck gesetzt werden kann, in eine bestimmte Richtung zu wählen",
                "Das Wahlgeheimnis schützt davor, dass jemand wegen seiner Wahlentscheidung bedroht oder unter Druck gesetzt wird."),
            ("Was können unabhängige Wahlbeobachter tun?", new[] { "Prüfen, ob eine Wahl fair und ordnungsgemäß abläuft", "Selbst über das Wahlergebnis entscheiden", "Stimmen für andere Personen abgeben" }, "Prüfen, ob eine Wahl fair und ordnungsgemäß abläuft",
                "Wahlbeobachter kontrollieren den ordnungsgemäßen Ablauf einer Wahl und tragen so zu Vertrauen in das Ergebnis bei."),
            ("Was passiert mit der Stimme einer Person, die nicht zur Wahl geht?", new[] { "Sie wird bei der Auszählung nicht berücksichtigt", "Sie wird automatisch der stärksten Partei zugerechnet", "Die Wahl wird deswegen ungültig" }, "Sie wird bei der Auszählung nicht berücksichtigt",
                "Wer nicht wählen geht, gibt keine Stimme ab - diese Person beeinflusst das Ergebnis dadurch nicht mit."),
            ("Was ist neben dem Mindestalter eine weitere Voraussetzung für das Wahlrecht bei der Bundestagswahl?", new[] { "Die deutsche Staatsangehörigkeit", "Ein bestimmtes Einkommen", "Ein abgeschlossenes Studium" }, "Die deutsche Staatsangehörigkeit",
                "Bei der Bundestagswahl dürfen grundsätzlich nur deutsche Staatsbürgerinnen und Staatsbürger ab 18 Jahren wählen."),
            ("Ab welchem Alter dürfen Jugendliche in Deutschland seit einer Wahlrechtsreform bei der Wahl zum Europäischen Parlament mitwählen?", new[] { "16 Jahre", "21 Jahre", "25 Jahre" }, "16 Jahre",
                "Seit einer Reform des Europawahlrechts dürfen in Deutschland bereits 16-Jährige bei der Europawahl mitbestimmen."),
        };
        var f = varianten[r.Next(varianten.Length)];
        return new QuizQuestion
        {
            Id = NewId(), Subject = Subject.Politik, GradeLevel = GradeLevel.Klasse6,
            Topic = "Wahlrecht", Type = QuestionType.MultipleChoice,
            Prompt = f.Frage, Options = f.Optionen, CorrectAnswers = new[] { f.Antwort }, Explanation = f.Erklaerung,
            HelpHint = "Sowohl aktives (wählen) als auch passives (sich wählen lassen) Wahlrecht zum Bundestag beginnen mit der Volljährigkeit (18 Jahre)."
        };
    }

    private static readonly (string Frage, string[] Optionen, string Antwort, string Erklaerung)[] GewaltenteilungListe =
    {
        ("Welche drei Gewalten gibt es nach dem Prinzip der Gewaltenteilung?", new[] { "Legislative, Exekutive, Judikative", "Regierung, Opposition, Volk", "Bund, Land, Gemeinde" },
            "Legislative, Exekutive, Judikative", "Die drei Staatsgewalten sind: Legislative (Gesetzgebung), Exekutive (Ausführung/Regierung) und Judikative (Rechtsprechung/Gerichte)."),
        ("Wer übt die Judikative (rechtsprechende Gewalt) aus?", new[] { "Die Gerichte", "Der Bundestag", "Die Bundesregierung" }, "Die Gerichte",
            "Die Judikative liegt bei den unabhängigen Gerichten, die Gesetze auslegen und Recht sprechen."),
        ("Wer übt in Deutschland die Exekutive (ausführende Gewalt) hauptsächlich aus?", new[] { "Die Regierung und Verwaltung", "Die Gerichte", "Der Bundestag allein" }, "Die Regierung und Verwaltung",
            "Die Exekutive setzt Gesetze um - dazu gehören Bundesregierung, Ministerien und Behörden."),
        ("Warum ist es wichtig, dass die drei Gewalten voneinander unabhängig sind?", new[] { "Damit keine Gewalt zu viel Macht bekommt und sich alle gegenseitig kontrollieren", "Damit alles schneller geht", "Das ist nicht wichtig" }, "Damit keine Gewalt zu viel Macht bekommt und sich alle gegenseitig kontrollieren",
            "Die gegenseitige Kontrolle (\"Checks and Balances\") verhindert Machtmissbrauch."),
        ("Wer beschließt in Deutschland die Gesetze (Legislative)?", new[] { "Bundestag und Bundesrat", "Die Bundesregierung allein", "Die Gerichte" }, "Bundestag und Bundesrat",
            "Gesetze werden vom Bundestag beschlossen, bei vielen Gesetzen wirkt auch der Bundesrat mit."),
        ("Wer entwickelte das Prinzip der Gewaltenteilung, das viele moderne Demokratien prägt?", new[] { "Der französische Philosoph Montesquieu", "Ein deutscher Bundeskanzler", "Ein Berliner Bürgermeister" }, "Der französische Philosoph Montesquieu",
            "Der Philosoph Montesquieu beschrieb im 18. Jahrhundert das Prinzip, Staatsmacht auf mehrere unabhängige Gewalten zu verteilen."),
        ("Welches Gericht in Deutschland kann prüfen, ob ein Gesetz gegen das Grundgesetz verstößt?", new[] { "Das Bundesverfassungsgericht", "Das Amtsgericht", "Der Bundestag selbst" }, "Das Bundesverfassungsgericht",
            "Das Bundesverfassungsgericht überprüft Gesetze auf ihre Vereinbarkeit mit dem Grundgesetz."),
        ("Wo hat das Bundesverfassungsgericht seinen Sitz?", new[] { "Karlsruhe", "Berlin", "München" }, "Karlsruhe",
            "Anders als viele andere zentrale Institutionen sitzt das Bundesverfassungsgericht nicht in Berlin, sondern in Karlsruhe."),
        ("Warum sind Richterinnen und Richter in Deutschland unabhängig?", new[] { "Damit sie frei von politischem Druck entscheiden können", "Damit sie sich nicht an Gesetze halten müssen", "Damit sie von der Regierung Weisungen bekommen" }, "Damit sie frei von politischem Druck entscheiden können",
            "Richterliche Unabhängigkeit bedeutet: Urteile dürfen nicht von Politik oder Regierung beeinflusst werden."),
        ("Was ist ein \"Misstrauensvotum\" im Bundestag?", new[] { "Eine Abstimmung, mit der der Bundestag der Regierungschefin/dem Regierungschef das Vertrauen entziehen kann", "Eine Wahl zum Bundespräsidenten", "Eine Abstimmung über ein neues Gesetz" }, "Eine Abstimmung, mit der der Bundestag der Regierungschefin/dem Regierungschef das Vertrauen entziehen kann",
            "Mit einem Misstrauensvotum kann der Bundestag zeigen, dass er der Bundeskanzlerin/dem Bundeskanzler nicht mehr vertraut."),
        ("Was passiert bei einem erfolgreichen \"konstruktiven\" Misstrauensvotum in Deutschland?", new[] { "Gleichzeitig wird eine neue Regierungschefin/ein neuer Regierungschef gewählt", "Der Bundestag löst sich sofort komplett auf", "Es passiert gar nichts weiter" }, "Gleichzeitig wird eine neue Regierungschefin/ein neuer Regierungschef gewählt",
            "Das \"konstruktive\" Misstrauensvotum verlangt, dass der Bundestag gleichzeitig eine Nachfolgerin/einen Nachfolger wählt - Regierungswechsel ohne Machtvakuum."),
        ("Welche Rolle spielt der Föderalismus zusätzlich zur klassischen Gewaltenteilung in Deutschland?", new[] { "Er verteilt Macht zusätzlich zwischen Bund und Bundesländern", "Er schafft eine einzige zentrale Macht ohne Länder", "Er betrifft nur die Gerichte" }, "Er verteilt Macht zusätzlich zwischen Bund und Bundesländern",
            "Neben der Gewaltenteilung sorgt der Föderalismus für eine zusätzliche Machtverteilung zwischen Bundesebene und den 16 Bundesländern."),
        ("Was bedeutet \"Immunität\" für Bundestagsabgeordnete?", new[] { "Sie können während ihrer Amtszeit ohne Zustimmung des Bundestags grundsätzlich nicht strafrechtlich verfolgt werden", "Sie dürfen niemals kritisiert werden", "Sie müssen keine Steuern zahlen" }, "Sie können während ihrer Amtszeit ohne Zustimmung des Bundestags grundsätzlich nicht strafrechtlich verfolgt werden",
            "Die Immunität soll Abgeordnete vor politisch motivierter Strafverfolgung schützen - der Bundestag kann sie aber aufheben."),
        ("Was ist die Hauptaufgabe der Bundespräsidentin bzw. des Bundespräsidenten?", new[] { "Das Land nach außen repräsentieren und Gesetze formal unterzeichnen", "Alle Gesetze selbst schreiben", "Die Bundeswehr im Alltag befehligen" }, "Das Land nach außen repräsentieren und Gesetze formal unterzeichnen",
            "Das Amt des Bundespräsidenten ist vor allem repräsentativ - er/sie unterschreibt Gesetze und vertritt Deutschland offiziell."),
        ("Was unterscheidet Verwaltungsgerichte von Zivilgerichten (vereinfacht)?", new[] { "Verwaltungsgerichte entscheiden über Streit mit Behörden, Zivilgerichte über Streit zwischen Bürgern", "Beide sind exakt dasselbe Gericht", "Verwaltungsgerichte gibt es in Deutschland nicht" }, "Verwaltungsgerichte entscheiden über Streit mit Behörden, Zivilgerichte über Streit zwischen Bürgern",
            "Verwaltungsgerichte klären z.B. Streit über Behördenentscheidungen, während Zivilgerichte private Streitigkeiten zwischen Bürgerinnen und Bürgern verhandeln."),
        ("Was bedeutet \"Gewaltenverschränkung\" im deutschen System (vereinfacht)?", new[] { "Die drei Gewalten sind nicht komplett getrennt, sondern wirken teils zusammen", "Die drei Gewalten haben überhaupt keinen Kontakt zueinander", "Es gibt in Deutschland nur eine einzige Gewalt" }, "Die drei Gewalten sind nicht komplett getrennt, sondern wirken teils zusammen",
            "Beim Bundesrat etwa wirken Vertreter der Länderregierungen (Exekutive) an der Bundesgesetzgebung (Legislative) mit."),
        ("Warum sitzen im Bundesrat Vertreter der Länderregierungen, obwohl er an Gesetzen mitwirkt?", new[] { "Damit die Landesregierungen ihre Interessen direkt bei Bundesgesetzen einbringen können", "Weil das ein reiner Zufall der Geschichte ist", "Weil der Bundestag das so verlangt hat, um Macht abzugeben" }, "Damit die Landesregierungen ihre Interessen direkt bei Bundesgesetzen einbringen können",
            "Der Bundesrat gibt den Bundesländern eine direkte Stimme bei Bundesgesetzen, die sie betreffen."),
        ("Was passiert, wenn das Bundesverfassungsgericht ein Gesetz für verfassungswidrig erklärt?", new[] { "Das Gesetz verliert seine Gültigkeit bzw. muss angepasst werden", "Das Gesetz gilt trotzdem unverändert weiter", "Der Bundestag wird automatisch aufgelöst" }, "Das Gesetz verliert seine Gültigkeit bzw. muss angepasst werden",
            "Ein für verfassungswidrig erklärtes Gesetz kann nicht bestehen bleiben - es muss geändert oder aufgehoben werden."),
        ("Warum gilt Gewaltenteilung als wichtiger Schutz für die Bürgerinnen und Bürger?", new[] { "Weil keine einzelne Institution unkontrolliert über sie bestimmen kann", "Weil dadurch alles schneller entschieden wird", "Weil es dadurch überhaupt keine Regeln mehr gibt" }, "Weil keine einzelne Institution unkontrolliert über sie bestimmen kann",
            "Durch die gegenseitige Kontrolle der drei Gewalten wird Machtmissbrauch erschwert - das schützt letztlich die Rechte der Bürgerinnen und Bürger."),
        ("Wie lange ist die reguläre Amtszeit von Richterinnen und Richtern am Bundesverfassungsgericht?", new[] { "12 Jahre (ohne Wiederwahl)", "4 Jahre", "Lebenslang" }, "12 Jahre (ohne Wiederwahl)",
            "Verfassungsrichterinnen und -richter werden für 12 Jahre gewählt und können danach nicht wiedergewählt werden - das soll ihre Unabhängigkeit stärken.")
    };

    private static QuizQuestion Gewaltenteilung(Random r)
    {
        var f = GewaltenteilungListe[r.Next(GewaltenteilungListe.Length)];
        return new QuizQuestion
        {
            Id = NewId(), Subject = Subject.Politik, GradeLevel = GradeLevel.Klasse9,
            Topic = "Gewaltenteilung", Type = QuestionType.MultipleChoice,
            Prompt = f.Frage, Options = f.Optionen, CorrectAnswers = new[] { f.Antwort }, Explanation = f.Erklaerung,
            HelpHint = "Die drei Gewalten: Legislative (Gesetzgebung), Exekutive (Regierung/Verwaltung), Judikative (Gerichte)."
        };
    }

    private static readonly (string Frage, string[] Optionen, string Antwort, string Erklaerung)[] BundestagListe =
    {
        ("Was ist die Hauptaufgabe des Bundestags?", new[] { "Gesetze beschließen und die Regierung kontrollieren", "Gerichtsurteile fällen", "Die Polizei leiten" },
            "Gesetze beschließen und die Regierung kontrollieren", "Der Bundestag ist das direkt gewählte Parlament Deutschlands und beschließt Bundesgesetze."),
        ("Was vertritt der Bundesrat?", new[] { "Die Interessen der 16 Bundesländer", "Nur die Interessen Berlins", "Die Vereinten Nationen" }, "Die Interessen der 16 Bundesländer",
            "Der Bundesrat ist die Vertretung der Bundesländer auf Bundesebene und wirkt bei vielen Gesetzen mit."),
        ("Wie wird der Bundeskanzler/die Bundeskanzlerin gewählt?", new[] { "Vom Bundestag gewählt", "Direkt vom Volk gewählt", "Vom Bundespräsidenten bestimmt, ohne Wahl" }, "Vom Bundestag gewählt",
            "Anders als z.B. in den USA wählt in Deutschland nicht das Volk direkt, sondern der Bundestag den Bundeskanzler/die Bundeskanzlerin."),
        ("Wo tagt der Deutsche Bundestag?", new[] { "Im Reichstagsgebäude in Berlin", "Im Schloss Bellevue", "Im Roten Rathaus" }, "Im Reichstagsgebäude in Berlin",
            "Der Bundestag tagt im Reichstagsgebäude, das für Besucher teilweise zugänglich ist (u.a. die Kuppel)."),
        ("Was ist eine \"Fraktion\" im Bundestag?", new[] { "Der Zusammenschluss der Abgeordneten einer Partei", "Ein einzelner Abgeordneter", "Ein Gerichtssaal" }, "Der Zusammenschluss der Abgeordneten einer Partei",
            "Abgeordnete derselben Partei bilden im Bundestag eine Fraktion und stimmen oft gemeinsam ab."),
        ("Wer leitet die Sitzungen des Bundestags?", new[] { "Die Bundestagspräsidentin/der Bundestagspräsident", "Die Bundeskanzlerin/der Bundeskanzler", "Der Bundespräsident allein" }, "Die Bundestagspräsidentin/der Bundestagspräsident",
            "Die Bundestagspräsidentin bzw. der Bundestagspräsident leitet die Plenarsitzungen und vertritt den Bundestag nach außen."),
        ("Wie nennt man die kleineren Arbeitsgruppen im Bundestag, die sich mit einzelnen Themenbereichen wie Gesundheit oder Bildung befassen?", new[] { "Ausschüsse", "Bundesländer", "Wahlkreise" }, "Ausschüsse",
            "In Ausschüssen arbeiten Abgeordnete Gesetzentwürfe im Detail aus, bevor sie im Plenum abgestimmt werden."),
        ("Wie nennt man den Zeitraum zwischen zwei Bundestagswahlen?", new[] { "Wahlperiode (Legislaturperiode)", "Amnestie", "Koalitionsvertrag" }, "Wahlperiode (Legislaturperiode)",
            "Die Wahlperiode (auch Legislaturperiode genannt) dauert in der Regel vier Jahre - so lange arbeitet ein gewählter Bundestag."),
        ("Wie viele Lesungen durchläuft ein Gesetzentwurf im Bundestag normalerweise, bevor über ihn abschließend abgestimmt wird?", new[] { "In der Regel drei Lesungen", "Immer nur eine einzige Lesung", "Mindestens zehn Lesungen" }, "In der Regel drei Lesungen",
            "Gesetzentwürfe werden meist in drei Lesungen beraten und teils in Ausschüssen überarbeitet, bevor die finale Abstimmung erfolgt."),
        ("Was ist ein \"Untersuchungsausschuss\" im Bundestag?", new[] { "Ein Gremium, das mutmaßliche Missstände oder Skandale genauer untersucht", "Ein Ausschuss, der nur Gesetzestexte Korrektur liest", "Ein Gericht, das Strafen verhängt" }, "Ein Gremium, das mutmaßliche Missstände oder Skandale genauer untersucht",
            "Ein Untersuchungsausschuss klärt öffentlich Sachverhalte auf, z.B. bei Verdacht auf Fehlverhalten in der Regierung."),
        ("Womit befasst sich der Petitionsausschuss des Bundestags?", new[] { "Eingaben und Beschwerden von Bürgerinnen und Bürgern", "Ausschließlich internationalen Verträgen", "Der Vergabe von Bundesverdienstorden" }, "Eingaben und Beschwerden von Bürgerinnen und Bürgern",
            "Der Petitionsausschuss prüft Bitten und Beschwerden, die Bürgerinnen und Bürger direkt an den Bundestag richten."),
        ("In welchem Gebäude finden die Bundestagssitzungen im Plenum statt?", new[] { "Im Reichstagsgebäude", "Im Schloss Bellevue", "Im Roten Rathaus" }, "Im Reichstagsgebäude",
            "Das Reichstagsgebäude in Berlin ist der Sitz des Deutschen Bundestags und für Besucherinnen und Besucher teilweise zugänglich."),
        ("Wie groß ist der Deutsche Bundestag seit der Wahlrechtsreform von 2023 in etwa begrenzt?", new[] { "Auf rund 630 Sitze", "Auf genau 100 Sitze", "Auf über 1000 Sitze" }, "Auf rund 630 Sitze",
            "Um ein stetig wachsendes Parlament zu begrenzen, wurde die Sitzzahl des Bundestags auf eine feste Obergrenze von rund 630 Sitzen gedeckelt."),
        ("Was können einzelne Abgeordnete im Bundestag stellen, um die Regierung offiziell zu befragen?", new[] { "Eine parlamentarische Anfrage", "Ein Gerichtsurteil", "Eine Volksabstimmung" }, "Eine parlamentarische Anfrage",
            "Mit parlamentarischen Anfragen können Abgeordnete von der Regierung Auskünfte zu konkreten Themen verlangen."),
        ("Was bedeutet \"Fraktionsdisziplin\" im Bundestag, obwohl sie rechtlich nicht bindend ist?", new[] { "Abgeordnete einer Fraktion stimmen in der Praxis oft gemeinsam ab", "Alle Abgeordneten müssen exakt dieselbe Kleidung tragen", "Abgeordnete dürfen im Plenum nicht sprechen" }, "Abgeordnete einer Fraktion stimmen in der Praxis oft gemeinsam ab",
            "Auch ohne gesetzliche Pflicht stimmen Mitglieder einer Fraktion aus politischer Abstimmung untereinander meist einheitlich ab."),
        ("Wer war die erste Frau, die Bundestagspräsidentin wurde?", new[] { "Annemarie Renger", "Angela Merkel", "Rita Süssmuth" }, "Annemarie Renger",
            "Annemarie Renger wurde 1972 als erste Frau Bundestagspräsidentin."),
        ("Was ist eine \"namentliche Abstimmung\" im Bundestag?", new[] { "Eine Abstimmung, bei der für jede Person einzeln festgehalten wird, wie sie gestimmt hat", "Eine geheime Abstimmung ohne jede Dokumentation", "Eine Abstimmung nur der Bundestagspräsidentin allein" }, "Eine Abstimmung, bei der für jede Person einzeln festgehalten wird, wie sie gestimmt hat",
            "Bei wichtigen Entscheidungen wird oft namentlich abgestimmt, damit das Abstimmungsverhalten jeder Abgeordneten/jedes Abgeordneten öffentlich nachvollziehbar ist."),
        ("Was können Oppositionsfraktionen im Bundestag u.a. als Minderheitenrecht verlangen?", new[] { "Die Einsetzung eines Untersuchungsausschusses", "Die sofortige Auflösung der Regierung ohne Abstimmung", "Ein Verbot der Regierungspartei" }, "Die Einsetzung eines Untersuchungsausschusses",
            "Auch mit einer bestimmten Mindestzahl an Stimmen kann die Opposition die Einsetzung eines Untersuchungsausschusses erzwingen."),
        ("Wie wird eine neue Bundeskanzlerin bzw. ein neuer Bundeskanzler nach einer Bundestagswahl formal bestimmt?", new[] { "Der Bundestag wählt sie/ihn, meist auf Vorschlag der Bundespräsidentin/des Bundespräsidenten", "Das Volk wählt sie/ihn direkt", "Die vorherige Regierung bestimmt die Nachfolge selbst" }, "Der Bundestag wählt sie/ihn, meist auf Vorschlag der Bundespräsidentin/des Bundespräsidenten",
            "Anders als in Präsidialsystemen wählt in Deutschland der Bundestag die Regierungschefin/den Regierungschef."),
        ("Wie nennt man die zeitliche Begrenzung, die Rednerinnen und Rednern im Bundestags-Plenum meist gesetzt wird?", new[] { "Redezeit", "Wahlperiode", "Fraktionsdisziplin" }, "Redezeit",
            "Damit Debatten im Plenum geordnet ablaufen, wird die Redezeit der einzelnen Abgeordneten meist begrenzt.")
    };

    private static QuizQuestion BundestagBundesrat(Random r)
    {
        var f = BundestagListe[r.Next(BundestagListe.Length)];
        return new QuizQuestion
        {
            Id = NewId(), Subject = Subject.Politik, GradeLevel = GradeLevel.Klasse9,
            Topic = "Bundestag und Bundesrat", Type = QuestionType.MultipleChoice,
            Prompt = f.Frage, Options = f.Optionen, CorrectAnswers = new[] { f.Antwort }, Explanation = f.Erklaerung,
            HelpHint = "Der Bundestag ist das direkt gewählte Parlament (Gesetze, Regierungskontrolle); der Bundesrat vertritt die 16 Bundesländer."
        };
    }

    private static readonly (string Frage, string[] Optionen, string Antwort, string Erklaerung)[] WahlsystemListe =
    {
        ("Bei der Bundestagswahl hat jeder Wähler zwei Stimmen. Wofür ist die Zweitstimme wichtig?", new[] { "Sie entscheidet über die Sitzverteilung der Parteien im Bundestag", "Sie wählt nur den Bundeskanzler direkt", "Sie zählt nicht" },
            "Sie entscheidet über die Sitzverteilung der Parteien im Bundestag",
            "Die Zweitstimme (Parteistimme) ist entscheidend für das Kräfteverhältnis der Parteien im Bundestag."),
        ("Wie nennt man eine Wahl, bei der niemand sehen kann, wen man gewählt hat?", new[] { "Geheime Wahl", "Öffentliche Wahl", "Offene Wahl" }, "Geheime Wahl",
            "In Deutschland ist die Wahl laut Grundgesetz u.a. geheim: Niemand darf erfahren, wen man gewählt hat."),
        ("Was bedeutet \"gleiches Wahlrecht\"?", new[] { "Jede Stimme zählt gleich viel", "Nur Reiche haben mehr Stimmen", "Nur Ältere haben mehr Stimmen" }, "Jede Stimme zählt gleich viel",
            "\"Gleich\" bedeutet: Die Stimme jeder wahlberechtigten Person hat dasselbe Gewicht, unabhängig von Einkommen oder Status."),
        ("Was bedeutet die \"Fünf-Prozent-Hürde\" bei der Bundestagswahl?", new[] { "Eine Partei braucht mindestens 5% der Stimmen, um in den Bundestag einzuziehen", "Jede Partei bekommt automatisch 5% der Sitze", "Nur 5 Parteien dürfen antreten" }, "Eine Partei braucht mindestens 5% der Stimmen, um in den Bundestag einzuziehen",
            "Die 5%-Hürde soll eine zu starke Zersplitterung des Parlaments in viele Kleinstparteien verhindern."),
        ("Was ist die Erststimme bei der Bundestagswahl?", new[] { "Sie wählt die/den Kandidat/in des eigenen Wahlkreises direkt", "Sie entscheidet über die Sitzverteilung der Parteien", "Sie wählt den Bundeskanzler direkt" }, "Sie wählt die/den Kandidat/in des eigenen Wahlkreises direkt",
            "Mit der Erststimme wird eine Person direkt im eigenen Wahlkreis gewählt, mit der Zweitstimme eine Partei."),
        ("Wie nennt man das deutsche Wahlsystem, das Persönlichkeitswahl (Erststimme) und Verhältniswahl (Zweitstimme) verbindet?", new[] { "Personalisierte Verhältniswahl", "Reine Mehrheitswahl", "Losverfahren" }, "Personalisierte Verhältniswahl",
            "Das deutsche System kombiniert die direkte Wahl von Personen im Wahlkreis mit der proportionalen Verteilung nach Parteistimmen."),
        ("Was passiert nach einer Bundestagswahl, wenn keine Partei allein die Mehrheit der Sitze hat?", new[] { "Mehrere Parteien schließen sich meist zu einer Koalition zusammen", "Die Wahl wird automatisch für ungültig erklärt", "Es gibt dann gar keine Regierung mehr" }, "Mehrere Parteien schließen sich meist zu einer Koalition zusammen",
            "Ohne absolute Mehrheit einer einzelnen Partei einigen sich meist mehrere Parteien in Koalitionsverhandlungen auf eine gemeinsame Regierung."),
        ("Was ist eine \"Minderheitsregierung\"?", new[] { "Eine Regierung, die im Parlament keine eigene Mehrheit hat", "Eine Regierung nur aus kleinen Parteien", "Eine Regierung, die von Minderjährigen gewählt wurde" }, "Eine Regierung, die im Parlament keine eigene Mehrheit hat",
            "Eine Minderheitsregierung braucht für jedes Gesetz wechselnde Mehrheiten, da sie allein keine feste Mehrheit im Parlament hat."),
        ("Was ist eine \"vorgezogene Neuwahl\"?", new[] { "Eine Bundestagswahl, die früher als nach den regulären vier Jahren stattfindet", "Eine Wahl, die um vier Jahre verschoben wird", "Eine zweite, wiederholte Auszählung derselben Wahl" }, "Eine Bundestagswahl, die früher als nach den regulären vier Jahren stattfindet",
            "Zerbricht z.B. eine Regierungskoalition vorzeitig, kann es zu einer vorgezogenen Neuwahl vor Ablauf der regulären Wahlperiode kommen."),
        ("Was zeigen \"Hochrechnungen\" am Wahlabend, bevor alle Stimmen ausgezählt sind?", new[] { "Eine vorläufige Schätzung des Wahlergebnisses", "Das endgültige, amtliche Endergebnis", "Die Wahlergebnisse der nächsten Wahl" }, "Eine vorläufige Schätzung des Wahlergebnisses",
            "Hochrechnungen basieren auf einem Teil der ausgezählten Stimmen und geben einen ersten, noch vorläufigen Trend an."),
        ("Wofür nutzen Parteien Wahlplakate im Wahlkampf?", new[] { "Um ihre politischen Ziele und Kandidatinnen/Kandidaten bekannt zu machen", "Um geheime Absprachen zu treffen", "Um Wählerinnen und Wähler vom Wählen abzuhalten" }, "Um ihre politischen Ziele und Kandidatinnen/Kandidaten bekannt zu machen",
            "Wahlplakate sind ein klassisches Mittel, um im öffentlichen Raum für die eigene Partei und ihre Themen zu werben."),
        ("Was war ein Ziel der Bundestagswahlrechtsreform von 2023?", new[] { "Die Größe des Bundestags dauerhaft zu begrenzen", "Die Wahlpflicht in Deutschland einzuführen", "Die 5%-Hürde komplett abzuschaffen" }, "Die Größe des Bundestags dauerhaft zu begrenzen",
            "Der Bundestag war in den Jahrzehnten zuvor immer weiter gewachsen - die Reform sollte die Sitzzahl auf eine feste Obergrenze deckeln."),
        ("Was passiert mit Stimmen für eine Partei, die an der 5%-Hürde scheitert?", new[] { "Sie fließen nicht in die Sitzverteilung des Bundestags ein", "Sie werden automatisch der stärksten Partei zugerechnet", "Die Wahl muss deswegen wiederholt werden" }, "Sie fließen nicht in die Sitzverteilung des Bundestags ein",
            "Parteien unter 5% ziehen in der Regel nicht in den Bundestag ein - ihre Stimmen wirken sich nicht auf die Sitzverteilung aus."),
        ("Welche Ausnahme von der 5%-Hürde gibt es, wenn eine Partei genügend Direktmandate gewinnt (Grundmandatsklausel)?", new[] { "Ab mindestens 3 gewonnenen Direktmandaten zieht die Partei trotzdem entsprechend ihrem Zweitstimmenanteil ein", "Es gibt überhaupt keine Ausnahme von der 5%-Hürde", "Ab einem einzigen Direktmandat ist die Hürde für die Partei hinfällig" }, "Ab mindestens 3 gewonnenen Direktmandaten zieht die Partei trotzdem entsprechend ihrem Zweitstimmenanteil ein",
            "Die sogenannte Grundmandatsklausel erlaubt Parteien mit mindestens drei gewonnenen Wahlkreisen den Einzug, auch unterhalb der 5%-Marke."),
        ("Was bedeutet es, wenn eine Person über die Landesliste einer Partei in den Bundestag einzieht?", new[] { "Sie zieht über die Zweitstimme/Parteiliste ein, nicht durch direkten Sieg im Wahlkreis", "Sie hat im eigenen Wahlkreis die meisten Erststimmen gewonnen", "Sie wurde vom Bundespräsidenten persönlich ernannt" }, "Sie zieht über die Zweitstimme/Parteiliste ein, nicht durch direkten Sieg im Wahlkreis",
            "Neben den direkt gewählten Wahlkreiskandidaten ziehen weitere Abgeordnete über die Landeslisten ihrer Partei entsprechend dem Zweitstimmenergebnis ein."),
        ("Wie nennt man eine Person, die im eigenen Wahlkreis die meisten Erststimmen erhält?", new[] { "Direktkandidatin/Direktkandidat (Wahlkreisgewinnerin/-gewinner)", "Fraktionsvorsitzende/r", "Bundestagspräsidentin/-präsident" }, "Direktkandidatin/Direktkandidat (Wahlkreisgewinnerin/-gewinner)",
            "Wer im eigenen Wahlkreis die meisten Erststimmen bekommt, zieht als Direktkandidatin/Direktkandidat direkt in den Bundestag ein."),
        ("Was unterscheidet ein reines Mehrheitswahlsystem von einem Verhältniswahlsystem (vereinfacht)?", new[] { "Beim Mehrheitswahlsystem gewinnt, wer die meisten Stimmen hat; beim Verhältniswahlsystem spiegelt sich der Stimmenanteil in der Sitzverteilung", "Beide Systeme führen immer zum exakt gleichen Ergebnis", "Verhältniswahl kennt keine Parteien" }, "Beim Mehrheitswahlsystem gewinnt, wer die meisten Stimmen hat; beim Verhältniswahlsystem spiegelt sich der Stimmenanteil in der Sitzverteilung",
            "In reinen Mehrheitswahlsystemen (z.B. Großbritannien) reicht oft die relative Mehrheit im Wahlkreis; Verhältniswahl bildet die Stimmenanteile proportional in Sitzen ab."),
        ("Warum ist es für kleine Parteien besonders wichtig, die 5%-Hürde zu erreichen?", new[] { "Nur so ziehen sie überhaupt mit Sitzen in den Bundestag ein", "Die Hürde betrifft ausschließlich sehr große Parteien", "Kleine Parteien sind von der Hürde automatisch befreit" }, "Nur so ziehen sie überhaupt mit Sitzen in den Bundestag ein",
            "Ohne mindestens 5% der Zweitstimmen (oder ausreichend Direktmandate) bekommt eine Partei in der Regel keine Sitze im Bundestag."),
        ("Wie heißt das mathematische Verfahren, mit dem in Deutschland Bundestagssitze proportional auf Parteien verteilt werden?", new[] { "Sainte-Laguë/Schepers-Verfahren", "Das Würfelverfahren", "Das Alphabet-Verfahren" }, "Sainte-Laguë/Schepers-Verfahren",
            "Das Sainte-Laguë/Schepers-Verfahren rechnet Zweitstimmenanteile möglichst gerecht in eine Sitzverteilung um."),
        ("Wer bestimmt normalerweise, welche Personen auf der Landesliste einer Partei stehen und in welcher Reihenfolge?", new[] { "Die Partei selbst, meist durch eine Wahl auf einem Parteitag", "Der Bundespräsident allein", "Das Bundesverfassungsgericht" }, "Die Partei selbst, meist durch eine Wahl auf einem Parteitag",
            "Parteimitglieder wählen auf Parteitagen oder Vertreterversammlungen die Reihenfolge ihrer Landesliste demokratisch selbst.")
    };

    private static QuizQuestion Wahlsystem(Random r)
    {
        var f = WahlsystemListe[r.Next(WahlsystemListe.Length)];
        return new QuizQuestion
        {
            Id = NewId(), Subject = Subject.Politik, GradeLevel = GradeLevel.Klasse9,
            Topic = "Wahlsystem", Type = QuestionType.MultipleChoice,
            Prompt = f.Frage, Options = f.Optionen, CorrectAnswers = new[] { f.Antwort }, Explanation = f.Erklaerung,
            HelpHint = "Bei der Bundestagswahl entscheidet die Zweitstimme über die Sitzverteilung der Parteien; Wahlen in Deutschland sind u.a. geheim."
        };
    }

    private static readonly (string Frage, string[] Optionen, string Antwort, string Erklaerung)[] SozialeMarktwirtschaftListe =
    {
        ("Was ist das Grundprinzip der Sozialen Marktwirtschaft in Deutschland?", new[] { "Freier Markt und Wettbewerb, kombiniert mit sozialer Absicherung durch den Staat", "Der Staat bestimmt alle Preise und Löhne", "Es gibt keinerlei staatliche Regeln für die Wirtschaft" },
            "Freier Markt und Wettbewerb, kombiniert mit sozialer Absicherung durch den Staat",
            "Die Soziale Marktwirtschaft verbindet freien Wettbewerb (Angebot und Nachfrage) mit sozialem Ausgleich, z.B. durch Sozialversicherungen und Mindestlohn."),
        ("Welche Aufgabe hat der Staat in der Sozialen Marktwirtschaft?", new[] { "Er setzt Regeln (z.B. gegen Monopole) und sichert soziale Absicherung ab", "Er verbietet jeglichen privaten Handel", "Er hat gar keine Aufgabe in der Wirtschaft" },
            "Er setzt Regeln (z.B. gegen Monopole) und sichert soziale Absicherung ab",
            "Der Staat greift regelnd ein (z.B. Kartellrecht gegen Monopole) und sorgt für ein soziales Netz (Kranken-, Renten-, Arbeitslosenversicherung)."),
        ("Was gehört in Deutschland zum sozialen Sicherungssystem?", new[] { "Kranken-, Renten- und Arbeitslosenversicherung", "Nur die private Altersvorsorge", "Ausschließlich Steuern auf Lebensmittel" },
            "Kranken-, Renten- und Arbeitslosenversicherung", "Die gesetzlichen Sozialversicherungen (u.a. Kranken-, Renten-, Arbeitslosen- und Pflegeversicherung) bilden das soziale Sicherungsnetz."),
        ("Was ist der Mindestlohn?", new[] { "Der niedrigste erlaubte Stundenlohn in Deutschland", "Das höchste erlaubte Gehalt", "Eine freiwillige Zahlung von Firmen" },
            "Der niedrigste erlaubte Stundenlohn in Deutschland", "Der gesetzliche Mindestlohn legt fest, wie viel pro Stunde mindestens gezahlt werden muss."),
        ("Was passiert bei starkem Wettbewerb zwischen Firmen normalerweise mit den Preisen?", new[] { "Sie tendieren eher dazu, niedrig zu bleiben", "Sie steigen automatisch stark", "Wettbewerb hat keinen Einfluss auf Preise" },
            "Sie tendieren eher dazu, niedrig zu bleiben", "Konkurrieren mehrere Anbieter um Kunden, versuchen sie oft, mit guten Preisen oder Qualität zu überzeugen."),
        ("Wer gilt als einer der \"Väter\" der Sozialen Marktwirtschaft in Deutschland nach dem Zweiten Weltkrieg?", new[] { "Ludwig Erhard", "Konrad Adenauer allein", "Otto von Bismarck" }, "Ludwig Erhard",
            "Ludwig Erhard gilt als einer der wichtigsten Architekten der Sozialen Marktwirtschaft in der jungen Bundesrepublik."),
        ("Was ist die Aufgabe des Bundeskartellamts?", new[] { "Es verhindert, dass einzelne Unternehmen den Markt unfair beherrschen", "Es setzt die Löhne aller Berufe fest", "Es druckt neues Geld" }, "Es verhindert, dass einzelne Unternehmen den Markt unfair beherrschen",
            "Das Kartellamt überwacht den Wettbewerb und greift z.B. ein, wenn Unternehmen unerlaubte Preisabsprachen treffen."),
        ("Was sind \"Tarifverhandlungen\"?", new[] { "Verhandlungen zwischen Gewerkschaften und Arbeitgebern über Löhne und Arbeitsbedingungen", "Verhandlungen über Zölle zwischen Ländern", "Gespräche zwischen Parteien vor einer Wahl" }, "Verhandlungen zwischen Gewerkschaften und Arbeitgebern über Löhne und Arbeitsbedingungen",
            "In Tarifverhandlungen einigen sich Gewerkschaften und Arbeitgeberverbände auf Löhne, Arbeitszeiten und weitere Bedingungen."),
        ("Was ist eine \"Gewerkschaft\"?", new[] { "Eine Organisation, die die Interessen von Arbeitnehmerinnen und Arbeitnehmern vertritt", "Eine staatliche Steuerbehörde", "Ein Zusammenschluss von Unternehmerinnen und Unternehmern" }, "Eine Organisation, die die Interessen von Arbeitnehmerinnen und Arbeitnehmern vertritt",
            "Gewerkschaften setzen sich gebündelt für bessere Löhne und Arbeitsbedingungen ihrer Mitglieder ein."),
        ("Was bedeutet \"Steuerprogression\" in Deutschland (vereinfacht)?", new[] { "Wer mehr verdient, zahlt einen höheren Steuersatz", "Alle zahlen exakt denselben Steuersatz", "Wer mehr verdient, zahlt gar keine Steuern" }, "Wer mehr verdient, zahlt einen höheren Steuersatz",
            "Die progressive Einkommensteuer soll höhere Einkommen stärker an der Finanzierung des Gemeinwesens beteiligen."),
        ("Wie heißt die aktuelle Grundsicherung für Arbeitsuchende in Deutschland (frühere Bezeichnung: Hartz IV)?", new[] { "Bürgergeld", "Kindergeld", "Wohngeld" }, "Bürgergeld",
            "Seit einer Reform heißt die Grundsicherung für Arbeitsuchende in Deutschland \"Bürgergeld\"."),
        ("Wie wird die gesetzliche Rente in Deutschland im sogenannten Umlageverfahren grundsätzlich finanziert?", new[] { "Die arbeitende Generation zahlt mit ihren Beiträgen die Renten der aktuellen Rentnerinnen und Rentner", "Jede Person spart ihr ganzes Leben nur für sich selbst", "Der Staat druckt für Renten einfach neues Geld" }, "Die arbeitende Generation zahlt mit ihren Beiträgen die Renten der aktuellen Rentnerinnen und Rentner",
            "Im Umlageverfahren finanzieren die Beiträge der aktuell Beschäftigten direkt die Renten der heutigen Rentnerinnen und Rentner."),
        ("Was schützt der Verbraucherschutz in Deutschland u.a.?", new[] { "Käuferinnen und Käufer vor unsicheren oder irreführend beworbenen Produkten", "Ausschließlich große Unternehmen vor Konkurrenz", "Nur den Staat vor Steuerausfällen" }, "Käuferinnen und Käufer vor unsicheren oder irreführend beworbenen Produkten",
            "Verbraucherschutzregeln sorgen z.B. für Produktsicherheit, klare Kennzeichnung und Widerrufsrechte beim Einkauf."),
        ("Warum spricht man in Deutschland zunehmend von \"sozialer UND ökologischer Marktwirtschaft\"?", new[] { "Weil Umweltschutz zunehmend als weiteres Ziel der Wirtschaftsordnung gilt", "Weil der Umweltschutz komplett abgeschafft wurde", "Weil das Wort \"sozial\" nicht mehr verwendet werden darf" }, "Weil Umweltschutz zunehmend als weiteres Ziel der Wirtschaftsordnung gilt",
            "Neben sozialem Ausgleich gewinnt der Schutz von Umwelt und Klima als Leitidee der Wirtschaftspolitik an Bedeutung."),
        ("Was bedeutet \"Wirtschaftswachstum\", gemessen z.B. am Bruttoinlandsprodukt (BIP)?", new[] { "Die Wirtschaftsleistung eines Landes nimmt in einem Zeitraum zu", "Die Bevölkerung eines Landes wird kleiner", "Ein Land druckt mehr Geld" }, "Die Wirtschaftsleistung eines Landes nimmt in einem Zeitraum zu",
            "Das BIP misst den Wert aller in einem Land produzierten Güter und Dienstleistungen - steigt es, spricht man von Wirtschaftswachstum."),
        ("Welche Institution ist für die gemeinsame Geldpolitik und Preisstabilität im Euro-Raum zuständig?", new[] { "Die Europäische Zentralbank (EZB)", "Das Bundeskartellamt", "Die Bundesagentur für Arbeit" }, "Die Europäische Zentralbank (EZB)",
            "Die EZB steuert u.a. die Leitzinsen im Euro-Raum, um die Inflation stabil zu halten."),
        ("Was passiert, wenn ein einzelnes Unternehmen ein Monopol hat (einziger Anbieter am Markt)?", new[] { "Es kann Preise ohne Konkurrenzdruck bestimmen, was Kundinnen und Kunden benachteiligen kann", "Der Preis fällt automatisch auf null", "Andere Unternehmen dürfen automatisch günstiger anbieten" }, "Es kann Preise ohne Konkurrenzdruck bestimmen, was Kundinnen und Kunden benachteiligen kann",
            "Ohne Konkurrenz fehlt der Anreiz, Preise niedrig zu halten - deshalb greift bei Monopolverdacht das Kartellamt ein."),
        ("Was passiert mit dem gesetzlichen Mindestlohn in Deutschland im Lauf der Zeit typischerweise?", new[] { "Er wird regelmäßig angepasst/angehoben", "Er bleibt für immer exakt gleich hoch", "Er wird jedes Jahr abgeschafft und neu eingeführt" }, "Er wird regelmäßig angepasst/angehoben",
            "Eine unabhängige Mindestlohnkommission prüft regelmäßig und schlägt Anpassungen des Mindestlohns vor."),
        ("Was ist ein \"Streik\"?", new[] { "Eine organisierte Arbeitsniederlegung, um z.B. bessere Löhne durchzusetzen", "Ein gesetzliches Verbot zu arbeiten", "Ein Fest zum Firmenjubiläum" }, "Eine organisierte Arbeitsniederlegung, um z.B. bessere Löhne durchzusetzen",
            "Mit einem Streik legen Arbeitnehmerinnen und Arbeitnehmer organisiert die Arbeit nieder, um Forderungen wie höhere Löhne durchzusetzen."),
        ("Was ist eine Aufgabe der Bundesagentur für Arbeit?", new[] { "Sie unterstützt bei der Jobsuche und zahlt z.B. Arbeitslosengeld", "Sie legt die Steuersätze für alle Bürger fest", "Sie kontrolliert Gerichtsverfahren" }, "Sie unterstützt bei der Jobsuche und zahlt z.B. Arbeitslosengeld",
            "Die Bundesagentur für Arbeit vermittelt Arbeitsplätze, berät und zahlt u.a. Arbeitslosengeld an Berechtigte.")
    };

    private static QuizQuestion SozialeMarktwirtschaft(Random r)
    {
        var f = SozialeMarktwirtschaftListe[r.Next(SozialeMarktwirtschaftListe.Length)];
        return new QuizQuestion
        {
            Id = NewId(), Subject = Subject.Politik, GradeLevel = GradeLevel.Klasse9,
            Topic = "Soziale Marktwirtschaft", Type = QuestionType.MultipleChoice,
            Prompt = f.Frage, Options = f.Optionen, CorrectAnswers = new[] { f.Antwort }, Explanation = f.Erklaerung,
            HelpHint = "Soziale Marktwirtschaft = freier Wettbewerb + staatliche Regeln/soziale Absicherung (Sozialversicherungen)."
        };
    }
}
