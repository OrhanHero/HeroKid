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
            [GradeLevel.Klasse6] = new List<TopicFactory> { Demokratie, BerlinBezirke, Wahlrecht, ArmutUndReichtumPolitik, GlobalisierteWelt, MigrationPolitik, LebenImRechtsstaat },
            [GradeLevel.Klasse9] = new List<TopicFactory> { Gewaltenteilung, BundestagBundesrat, Wahlsystem, SozialeMarktwirtschaft, WillensbildungUndMedien, KonflikteInternationaleAkteure, FriedenssicherungUndEntwicklungspolitik, EuropaeischeUnion }
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

    private static readonly (string Frage, string[] Optionen, string Antwort, string Erklaerung)[] WillensbildungListe =
    {
        ("Was bedeutet \"politische Willensbildung\"?", new[] { "Der Prozess, in dem sich Meinungen und Forderungen zu politischen Positionen entwickeln", "Ein anderes Wort für Gerichtsverfahren", "Ein Begriff, der nur die Regierung selbst betrifft" }, "Der Prozess, in dem sich Meinungen und Forderungen zu politischen Positionen entwickeln",
            "Politische Willensbildung beschreibt, wie sich aus individuellen Meinungen über Diskussion, Medien und Parteien politische Positionen und Mehrheiten formen."),
        ("Welche Rolle spielen Parteien bei der politischen Willensbildung?", new[] { "Sie bündeln unterschiedliche Meinungen zu politischen Programmen und stellen sich zur Wahl", "Parteien haben mit Willensbildung nichts zu tun", "Parteien entscheiden immer allein, ohne die Bevölkerung einzubeziehen" }, "Sie bündeln unterschiedliche Meinungen zu politischen Programmen und stellen sich zur Wahl",
            "Parteien bündeln verschiedene politische Ansichten zu Programmen, mit denen sie um Wählerstimmen werben und so zur politischen Willensbildung beitragen."),
        ("Welche Rolle spielen Interessengruppen (z.B. Verbände) in der Politik?", new[] { "Sie vertreten die Anliegen bestimmter gesellschaftlicher Gruppen gegenüber der Politik", "Sie haben in demokratischen Prozessen keinerlei Einfluss", "Sie ersetzen offiziell das Parlament" }, "Sie vertreten die Anliegen bestimmter gesellschaftlicher Gruppen gegenüber der Politik",
            "Interessengruppen wie Wirtschafts- oder Umweltverbände setzen sich für die Anliegen ihrer Mitglieder ein und versuchen, politische Entscheidungen zu beeinflussen."),
        ("Was ist die \"Kontrollfunktion\" der Massenmedien in einer Demokratie?", new[] { "Sie decken Missstände auf und beobachten kritisch das Handeln von Politik und Wirtschaft", "Sie sollen ausschließlich positive Nachrichten über die Regierung verbreiten", "Sie haben keinerlei Aufgabe gegenüber der Politik" }, "Sie decken Missstände auf und beobachten kritisch das Handeln von Politik und Wirtschaft",
            "Medien werden oft als \"vierte Gewalt\" bezeichnet, weil sie durch kritische Berichterstattung Politik und Wirtschaft kontrollieren und Missstände aufdecken können."),
        ("Was ist ein möglicher Interessenkonflikt bei der Berichterstattung sozialer Netzwerke über Politik?", new[] { "Plattformen können wirtschaftliche Interessen (z.B. Reichweite, Werbeeinnahmen) verfolgen, die die Auswahl der Inhalte beeinflussen", "Soziale Netzwerke haben grundsätzlich niemals eigene wirtschaftliche Interessen", "Politische Inhalte werden in sozialen Netzwerken immer völlig neutral dargestellt" }, "Plattformen können wirtschaftliche Interessen (z.B. Reichweite, Werbeeinnahmen) verfolgen, die die Auswahl der Inhalte beeinflussen",
            "Da soziale Netzwerke oft durch Werbung finanziert werden, können wirtschaftliche Interessen die Auswahl und Priorisierung politischer Inhalte beeinflussen."),
        ("Was bedeutet \"wehrhafte Demokratie\" (streitbare Demokratie)?", new[] { "Der demokratische Staat darf sich aktiv gegen Kräfte schützen, die die Demokratie selbst abschaffen wollen", "Ein Staat, der keine Meinungsfreiheit erlaubt", "Ein anderes Wort für Diktatur" }, "Der demokratische Staat darf sich aktiv gegen Kräfte schützen, die die Demokratie selbst abschaffen wollen",
            "Das Konzept der wehrhaften Demokratie erlaubt es dem Staat, sich gezielt gegen Bestrebungen zu schützen, die freiheitliche demokratische Grundordnung zu beseitigen."),
        ("Was ist ein Beispiel für ein Instrument der wehrhaften Demokratie in Deutschland?", new[] { "Die Möglichkeit, verfassungsfeindliche Parteien vom Bundesverfassungsgericht verbieten zu lassen", "Das Verbot jeglicher Opposition", "Die automatische Absetzung jeder gewählten Regierung" }, "Die Möglichkeit, verfassungsfeindliche Parteien vom Bundesverfassungsgericht verbieten zu lassen",
            "Das Bundesverfassungsgericht kann Parteien verbieten, die nachweislich die freiheitliche demokratische Grundordnung beseitigen wollen - ein zentrales Instrument der wehrhaften Demokratie."),
        ("Was versteht man unter \"politischem Extremismus\"?", new[] { "Politische Positionen, die die demokratische Grundordnung ablehnen oder bekämpfen wollen", "Jede besonders engagierte politische Meinung", "Ein anderes Wort für Opposition" }, "Politische Positionen, die die demokratische Grundordnung ablehnen oder bekämpfen wollen",
            "Extremistische Positionen lehnen zentrale demokratische Prinzipien wie Meinungsfreiheit, Gewaltenteilung oder Menschenrechte grundsätzlich ab."),
        ("Was ist eine mögliche Ursache für \"Parteienverdrossenheit\" in der Bevölkerung?", new[] { "Enttäuschung über nicht eingehaltene Versprechen oder das Gefühl, nicht gehört zu werden", "Parteienverdrossenheit gibt es in Demokratien grundsätzlich nicht", "Zu viel Vertrauen der Bevölkerung in alle Parteien gleichermaßen" }, "Enttäuschung über nicht eingehaltene Versprechen oder das Gefühl, nicht gehört zu werden",
            "Parteienverdrossenheit entsteht häufig durch Enttäuschung über Politik, das Gefühl mangelnder Mitbestimmung oder wahrgenommene Skandale."),
        ("Warum ist Medienkompetenz wichtig, um Einflussversuche auf die politische Meinungsbildung zu erkennen?", new[] { "Sie hilft, Quellen kritisch zu prüfen und Manipulationsversuche zu erkennen", "Medienkompetenz hat mit politischer Meinungsbildung nichts zu tun", "Medienkompetenz macht jede kritische Prüfung von Inhalten überflüssig" }, "Sie hilft, Quellen kritisch zu prüfen und Manipulationsversuche zu erkennen",
            "Wer Medieninhalte kritisch einordnen kann, erkennt eher, wenn Informationen einseitig dargestellt oder gezielt manipuliert werden."),
        ("Was ist eine \"Filterblase\" im Zusammenhang mit sozialen Netzwerken und politischer Meinungsbildung?", new[] { "Ein Zustand, in dem man vor allem Inhalte sieht, die der eigenen Meinung entsprechen", "Ein technisches Gerät zur Internetfilterung", "Ein anderes Wort für Wahlkampf" }, "Ein Zustand, in dem man vor allem Inhalte sieht, die der eigenen Meinung entsprechen",
            "In einer Filterblase bekommen Nutzerinnen und Nutzer durch Algorithmen vor allem Inhalte gezeigt, die zur eigenen Meinung passen - andere Sichtweisen kommen seltener vor."),
        ("Was kann eine Folge von zunehmender politischer Polarisierung in einer Gesellschaft sein?", new[] { "Erschwerte Kompromissfindung und wachsende gesellschaftliche Spaltung", "Automatisch mehr gegenseitiges Verständnis zwischen politischen Lagern", "Polarisierung hat auf demokratische Prozesse keinerlei Auswirkung" }, "Erschwerte Kompromissfindung und wachsende gesellschaftliche Spaltung",
            "Starke politische Polarisierung kann dazu führen, dass Kompromisse schwerer zu finden sind und sich gesellschaftliche Gräben vertiefen."),
        ("Was ist ein Verfassungsschutz in Deutschland?", new[] { "Eine Behörde, die Bestrebungen gegen die freiheitliche demokratische Grundordnung beobachtet", "Eine Organisation, die ausschließlich Gebäude vor Einbrüchen schützt", "Ein anderes Wort für die Polizei im Straßenverkehr" }, "Eine Behörde, die Bestrebungen gegen die freiheitliche demokratische Grundordnung beobachtet",
            "Der Verfassungsschutz beobachtet extremistische und verfassungsfeindliche Bestrebungen, um die demokratische Grundordnung zu schützen."),
        ("Was ist ein Grund, warum Desinformation (bewusste Falschinformation) als Gefahr für Demokratien gilt?", new[] { "Sie kann die öffentliche Meinungsbildung gezielt verzerren und Vertrauen in Institutionen untergraben", "Desinformation hat auf demokratische Prozesse keinerlei Einfluss", "Desinformation stärkt automatisch das Vertrauen in demokratische Institutionen" }, "Sie kann die öffentliche Meinungsbildung gezielt verzerren und Vertrauen in Institutionen untergraben",
            "Gezielte Desinformation kann die öffentliche Debatte verzerren und das Vertrauen der Bevölkerung in demokratische Institutionen und Medien schwächen."),
        ("Was ist der öffentlich-rechtliche Rundfunk (z.B. ARD, ZDF) in seiner Funktion für die politische Willensbildung?", new[] { "Ein durch Rundfunkbeitrag finanziertes Mediensystem mit dem Auftrag zu ausgewogener, unabhängiger Information", "Ein rein privates, gewinnorientiertes Unternehmen ohne besonderen Informationsauftrag", "Eine direkte Abteilung der Bundesregierung" }, "Ein durch Rundfunkbeitrag finanziertes Mediensystem mit dem Auftrag zu ausgewogener, unabhängiger Information",
            "Der öffentlich-rechtliche Rundfunk soll unabhängig von Regierung und wirtschaftlichen Interessen ausgewogen informieren und so die demokratische Meinungsbildung unterstützen."),
        ("Warum werden Talkshows und politische Diskussionsformate oft als wichtig für die politische Willensbildung angesehen?", new[] { "Sie bieten eine Plattform, auf der unterschiedliche politische Positionen öffentlich diskutiert werden", "Sie haben mit politischer Meinungsbildung nichts zu tun", "Sie zeigen ausschließlich eine einzige politische Meinung" }, "Sie bieten eine Plattform, auf der unterschiedliche politische Positionen öffentlich diskutiert werden",
            "Politische Diskussionsformate ermöglichen den öffentlichen Austausch unterschiedlicher Positionen und tragen so zur Meinungsbildung der Zuschauerinnen und Zuschauer bei."),
        ("Was können Bürgerinnen und Bürger tun, um Interessengruppen und Verbänden gegenüber eine eigene, informierte Position zu entwickeln?", new[] { "Verschiedene Quellen und Positionen kritisch miteinander vergleichen", "Sich ausschließlich auf eine einzige Quelle verlassen", "Politische Themen grundsätzlich ignorieren" }, "Verschiedene Quellen und Positionen kritisch miteinander vergleichen",
            "Der Vergleich unterschiedlicher Quellen und Positionen hilft, sich ein eigenständiges, informiertes Bild zu politischen Themen zu machen, statt einseitigen Interessen unreflektiert zu folgen."),
        ("Was ist ein Kennzeichen von Verschwörungstheorien, die gezielt politische Meinungsbildung gefährden können?", new[] { "Sie behaupten oft geheime, unbewiesene Absichten hinter komplexen Ereignissen, ohne belastbare Beweise", "Sie beruhen immer auf nachprüfbaren, wissenschaftlich anerkannten Fakten", "Sie haben mit politischer Meinungsbildung nichts zu tun" }, "Sie behaupten oft geheime, unbewiesene Absichten hinter komplexen Ereignissen, ohne belastbare Beweise",
            "Verschwörungstheorien unterstellen oft ohne stichhaltige Beweise geheime Absichten hinter Ereignissen und können dadurch das Vertrauen in demokratische Institutionen untergraben."),
        ("Was für eine Rolle spielt die Vielfalt der Medienlandschaft (unterschiedliche Zeitungen, Sender, Plattformen) für die Demokratie?", new[] { "Sie ermöglicht den Zugang zu unterschiedlichen Perspektiven und verhindert eine einseitige Meinungsmacht", "Medienvielfalt ist für Demokratien bedeutungslos", "Nur eine einzige Informationsquelle wäre für eine Demokratie ausreichend" }, "Sie ermöglicht den Zugang zu unterschiedlichen Perspektiven und verhindert eine einseitige Meinungsmacht",
            "Eine vielfältige Medienlandschaft verhindert, dass eine einzelne Quelle die öffentliche Meinung dominiert, und ermöglicht Bürgerinnen und Bürgern einen Vergleich unterschiedlicher Perspektiven."),
        ("Was ist ein Beispiel dafür, wie der Staat gegen Bestrebungen gegen die freiheitliche demokratische Grundordnung vorgehen kann, ohne die Meinungsfreiheit generell einzuschränken?", new[] { "Gezielte rechtliche Maßnahmen gegen konkret verfassungsfeindliche Organisationen oder Handlungen", "Ein generelles Verbot jeder kritischen Meinungsäußerung", "Die Abschaffung aller Parteien" }, "Gezielte rechtliche Maßnahmen gegen konkret verfassungsfeindliche Organisationen oder Handlungen",
            "Die wehrhafte Demokratie richtet sich gezielt gegen konkret verfassungsfeindliche Organisationen und Handlungen, nicht gegen Meinungsfreiheit oder legitime Kritik im Allgemeinen.")
    };

    private static QuizQuestion WillensbildungUndMedien(Random r)
    {
        var f = WillensbildungListe[r.Next(WillensbildungListe.Length)];
        return new QuizQuestion
        {
            Id = NewId(), Subject = Subject.Politik, GradeLevel = GradeLevel.Klasse9,
            Topic = "Demokratie in Deutschland: Willensbildung, Medien und Gefährdungen", Type = QuestionType.MultipleChoice,
            Prompt = f.Frage, Options = f.Optionen, CorrectAnswers = new[] { f.Antwort }, Explanation = f.Erklaerung,
            HelpHint = "Parteien, Interessengruppen und Medien prägen die politische Willensbildung; die \"wehrhafte Demokratie\" erlaubt gezielte Maßnahmen (z.B. Parteiverbote) gegen verfassungsfeindliche Bestrebungen, ohne die allgemeine Meinungsfreiheit einzuschränken."
        };
    }

    private static readonly (string Frage, string[] Optionen, string Antwort, string Erklaerung)[] KonflikteAkteureListe =
    {
        ("Was ist ein \"Failed State\" (gescheiterter Staat)?", new[] { "Ein Staat, dessen Regierung grundlegende Aufgaben wie Sicherheit und Ordnung nicht mehr gewährleisten kann", "Ein besonders wirtschaftlich erfolgreicher Staat", "Ein anderes Wort für eine parlamentarische Demokratie" }, "Ein Staat, dessen Regierung grundlegende Aufgaben wie Sicherheit und Ordnung nicht mehr gewährleisten kann",
            "Ein Failed State verliert die Kontrolle über zentrale staatliche Funktionen wie Sicherheit, Recht und Verwaltung, oft begleitet von Gewalt und Instabilität."),
        ("Was ist eine grundlegende Definition von \"Terrorismus\"?", new[] { "Gezielte Gewaltanwendung, um durch Angst und Einschüchterung politische Ziele zu erreichen", "Ein anderes Wort für jede Form von politischem Protest", "Eine Form der parlamentarischen Opposition" }, "Gezielte Gewaltanwendung, um durch Angst und Einschüchterung politische Ziele zu erreichen",
            "Terrorismus nutzt gezielt Gewalt und die dadurch verbreitete Angst, um politische, religiöse oder ideologische Ziele durchzusetzen."),
        ("Welche Aufgabe haben die Vereinten Nationen (UN) im internationalen Konfliktmanagement?", new[] { "Vermittlung, Friedenssicherung und internationale Zusammenarbeit zwischen Staaten", "Die alleinige militärische Kontrolle über alle Staaten der Welt", "Ausschließlich wirtschaftliche Fragen ohne jeden politischen Bezug" }, "Vermittlung, Friedenssicherung und internationale Zusammenarbeit zwischen Staaten",
            "Die UN setzen sich u.a. durch Vermittlung, Friedensmissionen und internationale Zusammenarbeit für die Lösung und Vorbeugung von Konflikten ein."),
        ("Was sind Nichtregierungsorganisationen (NGOs) im Kontext internationaler Konflikte?", new[] { "Unabhängige Organisationen, die sich z.B. für Menschenrechte oder humanitäre Hilfe einsetzen", "Staatliche Behörden einzelner Regierungen", "Ein anderes Wort für die Vereinten Nationen" }, "Unabhängige Organisationen, die sich z.B. für Menschenrechte oder humanitäre Hilfe einsetzen",
            "NGOs wie Ärzte ohne Grenzen oder Amnesty International engagieren sich unabhängig von Regierungen für humanitäre Hilfe, Menschenrechte oder Friedensarbeit."),
        ("Was ist die Aufgabe der EU als internationaler Akteur der Friedenssicherung?", new[] { "Wirtschaftliche und politische Zusammenarbeit zwischen Mitgliedstaaten fördern, um Konflikte in Europa zu verhindern", "Ausschließlich militärische Interventionen weltweit durchzuführen", "Sie hat mit Friedenssicherung nichts zu tun" }, "Wirtschaftliche und politische Zusammenarbeit zwischen Mitgliedstaaten fördern, um Konflikte in Europa zu verhindern",
            "Die enge wirtschaftliche und politische Verflechtung der EU-Mitgliedstaaten gilt als wichtiger Beitrag zur Friedenssicherung in Europa nach den Weltkriegen."),
        ("Was ist die Aufgabe der NATO als internationaler Akteur?", new[] { "Ein Verteidigungsbündnis, das sich gegenseitigen Beistand seiner Mitgliedstaaten verpflichtet", "Eine rein wirtschaftliche Freihandelszone", "Ein internationales Gericht für Zivilrecht" }, "Ein Verteidigungsbündnis, das sich gegenseitigen Beistand seiner Mitgliedstaaten verpflichtet",
            "Die NATO ist ein Verteidigungsbündnis, dessen Mitgliedstaaten sich im Verteidigungsfall gegenseitigen Beistand zusagen."),
        ("Was ist ein Beispiel für die Ursachen eines internationalen Konflikts?", new[] { "Streit um Territorium, Ressourcen, Macht oder ethnisch-religiöse Spannungen", "Konflikte entstehen historisch immer völlig grundlos", "Ausschließlich sportliche Wettkämpfe zwischen Staaten" }, "Streit um Territorium, Ressourcen, Macht oder ethnisch-religiöse Spannungen",
            "Internationale Konflikte entstehen oft durch eine Mischung aus Territorialstreitigkeiten, Ressourcenkonkurrenz, Machtinteressen oder ethnisch-religiösen Spannungen."),
        ("Was ist eine \"Konfliktanalyse\"?", new[] { "Die systematische Untersuchung von Gegenstand, Ursachen und beteiligten Akteuren eines Konflikts", "Eine rein militärische Strategie zur Kriegsführung", "Ein Begriff, der ausschließlich in der Mathematik verwendet wird" }, "Die systematische Untersuchung von Gegenstand, Ursachen und beteiligten Akteuren eines Konflikts",
            "Eine Konfliktanalyse untersucht strukturiert, worum es in einem Konflikt geht, welche Ursachen dahinterstehen und welche Akteure beteiligt sind."),
        ("Was können \"Blauhelm-Missionen\" der Vereinten Nationen bewirken?", new[] { "Sie sollen Frieden sichern helfen, z.B. durch Überwachung von Waffenstillständen", "Sie führen ausschließlich eigene Angriffskriege", "Sie haben mit Friedenssicherung nichts zu tun" }, "Sie sollen Frieden sichern helfen, z.B. durch Überwachung von Waffenstillständen",
            "UN-Friedenstruppen (Blauhelme) werden in Krisengebieten eingesetzt, um z.B. Waffenstillstände zu überwachen und zur Stabilisierung beizutragen."),
        ("Was für eine Rolle können regionale Organisationen (z.B. die Afrikanische Union) bei internationalen Konflikten spielen?", new[] { "Sie können bei der Vermittlung und Lösung regionaler Konflikte mitwirken", "Regionale Organisationen haben bei Konflikten grundsätzlich keinerlei Rolle", "Sie ersetzen offiziell die Vereinten Nationen" }, "Sie können bei der Vermittlung und Lösung regionaler Konflikte mitwirken",
            "Regionale Organisationen wie die Afrikanische Union oder die Arabische Liga engagieren sich oft bei der Vermittlung und Lösung von Konflikten in ihrer jeweiligen Region."),
        ("Was ist ein Grund, warum internationale Konflikte oft schwer zu lösen sind?", new[] { "Mehrere Akteure mit unterschiedlichen, teils widersprüchlichen Interessen sind beteiligt", "Internationale Konflikte haben grundsätzlich immer eine einzige, einfache Lösung", "An internationalen Konflikten ist üblicherweise nur ein einziger Akteur beteiligt" }, "Mehrere Akteure mit unterschiedlichen, teils widersprüchlichen Interessen sind beteiligt",
            "Da an internationalen Konflikten oft mehrere Staaten und Akteure mit unterschiedlichen Interessen beteiligt sind, gestaltet sich eine für alle akzeptable Lösung häufig schwierig."),
        ("Was versteht man unter \"humanitärer Hilfe\" in Konfliktregionen?", new[] { "Unterstützung mit Nahrung, medizinischer Versorgung und Schutz für notleidende Menschen", "Ausschließlich militärische Unterstützung einer Konfliktpartei", "Wirtschaftliche Sanktionen gegen eine Konfliktpartei" }, "Unterstützung mit Nahrung, medizinischer Versorgung und Schutz für notleidende Menschen",
            "Humanitäre Hilfe versorgt Menschen in Konfliktregionen mit lebensnotwendigen Gütern wie Nahrung, Wasser, medizinischer Versorgung und Schutz."),
        ("Was ist ein Beispiel für ein diplomatisches Mittel zur Konfliktlösung?", new[] { "Verhandlungen und Vermittlungsgespräche zwischen den Konfliktparteien", "Der sofortige militärische Angriff auf eine Konfliktpartei", "Der vollständige Abbruch jeglicher Kommunikation" }, "Verhandlungen und Vermittlungsgespräche zwischen den Konfliktparteien",
            "Diplomatische Verhandlungen und Vermittlung gelten als grundlegendes, gewaltfreies Mittel zur Beilegung internationaler Konflikte."),
        ("Was sind internationale Sanktionen als Mittel der Konfliktbearbeitung?", new[] { "Wirtschaftliche oder politische Druckmittel gegen einen Staat oder Akteur, um sein Verhalten zu ändern", "Eine Form militärischer Zusammenarbeit zwischen Staaten", "Ein anderes Wort für Entwicklungshilfe" }, "Wirtschaftliche oder politische Druckmittel gegen einen Staat oder Akteur, um sein Verhalten zu ändern",
            "Sanktionen wie Handelsbeschränkungen sollen politischen oder wirtschaftlichen Druck ausüben, um ein bestimmtes Verhalten zu ändern, ohne militärisch einzugreifen."),
        ("Was ist ein Grund, warum Terrorismus häufig auch als internationale Herausforderung (nicht nur nationales Problem) betrachtet wird?", new[] { "Terroristische Netzwerke agieren oft über Landesgrenzen hinweg", "Terrorismus betrifft grundsätzlich immer nur ein einziges Land", "Terrorismus hat keinerlei internationale Dimension" }, "Terroristische Netzwerke agieren oft über Landesgrenzen hinweg",
            "Da terroristische Netzwerke häufig grenzüberschreitend agieren, erfordert ihre Bekämpfung oft internationale Zusammenarbeit zwischen mehreren Staaten."),
        ("Was ist eine \"Krisenregion\"?", new[] { "Ein Gebiet mit anhaltenden politischen, militärischen oder gesellschaftlichen Spannungen und Konflikten", "Ein besonders friedliches und stabiles Gebiet", "Ein Gebiet ohne jegliche Bevölkerung" }, "Ein Gebiet mit anhaltenden politischen, militärischen oder gesellschaftlichen Spannungen und Konflikten",
            "Krisenregionen sind geprägt von andauernden politischen, militärischen oder gesellschaftlichen Spannungen, die oft internationale Aufmerksamkeit erfordern."),
        ("Was können internationale Gerichte, wie der Internationale Strafgerichtshof (IStGH), im Zusammenhang mit Konflikten leisten?", new[] { "Verantwortliche für schwere Kriegsverbrechen oder Völkermord rechtlich zur Verantwortung ziehen", "Sie haben mit internationalen Konflikten überhaupt nichts zu tun", "Sie können ausschließlich zivile Streitigkeiten innerhalb eines Landes verhandeln" }, "Verantwortliche für schwere Kriegsverbrechen oder Völkermord rechtlich zur Verantwortung ziehen",
            "Der Internationale Strafgerichtshof kann Einzelpersonen wegen schwerer Verbrechen wie Kriegsverbrechen oder Völkermord völkerrechtlich zur Verantwortung ziehen."),
        ("Was ist ein Grund, warum die Zusammenarbeit zwischen UN, EU, NATO und NGOs bei Konflikten oft sinnvoll ist?", new[] { "Unterschiedliche Akteure können sich mit ihren jeweiligen Stärken (Diplomatie, Sicherheit, humanitäre Hilfe) ergänzen", "Diese Organisationen verfolgen immer exakt dieselben Ziele und Methoden", "Zusammenarbeit zwischen diesen Organisationen ist grundsätzlich nicht möglich" }, "Unterschiedliche Akteure können sich mit ihren jeweiligen Stärken (Diplomatie, Sicherheit, humanitäre Hilfe) ergänzen",
            "Da UN, EU, NATO und NGOs unterschiedliche Stärken haben (z.B. Diplomatie, militärische Absicherung, humanitäre Hilfe), kann ihre koordinierte Zusammenarbeit Konfliktlösungen wirksamer machen."),
        ("Was ist ein zentrales Ziel bei der Analyse der \"Selbstverständnisse und Ziele\" internationaler Akteure wie der UN oder NATO?", new[] { "Zu verstehen, welche Werte und Zwecke die jeweilige Organisation offiziell verfolgt", "Ausschließlich den Namen der Organisation auswendig zu lernen", "Die Finanzierung der Organisation im Detail zu berechnen" }, "Zu verstehen, welche Werte und Zwecke die jeweilige Organisation offiziell verfolgt",
            "Das Verständnis von Selbstverständnis und Zielen internationaler Organisationen hilft einzuordnen, wie und warum sie in Konflikten handeln."),
        ("Was ist der UN-Sicherheitsrat?", new[] { "Das zentrale UN-Gremium, das über Maßnahmen zur Wahrung des Weltfriedens entscheidet", "Ein Gremium, das ausschließlich über das UN-Budget entscheidet", "Ein anderes Wort für die Generalversammlung der UN" }, "Das zentrale UN-Gremium, das über Maßnahmen zur Wahrung des Weltfriedens entscheidet",
            "Der UN-Sicherheitsrat trifft zentrale Entscheidungen zur internationalen Friedenssicherung, u.a. über Sanktionen oder Friedensmissionen.")
    };

    private static QuizQuestion KonflikteInternationaleAkteure(Random r)
    {
        var f = KonflikteAkteureListe[r.Next(KonflikteAkteureListe.Length)];
        return new QuizQuestion
        {
            Id = NewId(), Subject = Subject.Politik, GradeLevel = GradeLevel.Klasse9,
            Topic = "Konflikte und Konfliktlösungen: internationale Akteure", Type = QuestionType.MultipleChoice,
            Prompt = f.Frage, Options = f.Optionen, CorrectAnswers = new[] { f.Antwort }, Explanation = f.Erklaerung,
            HelpHint = "Internationale Akteure wie UN, EU, NATO und NGOs verfolgen unterschiedliche Ziele (Friedenssicherung, wirtschaftliche Zusammenarbeit, Verteidigung, humanitäre Hilfe) und ergänzen sich bei der Bearbeitung von Konflikten, Terrorismus und Failed States."
        };
    }

    private static readonly (string Frage, string[] Optionen, string Antwort, string Erklaerung)[] FriedenssicherungListe =
    {
        ("Was versteht man unter \"ziviler Konfliktbearbeitung\"?", new[] { "Gewaltfreie Methoden wie Vermittlung und Dialog zur Lösung von Konflikten", "Ausschließlich militärische Mittel zur Konfliktlösung", "Ein anderes Wort für einen Waffenstillstand" }, "Gewaltfreie Methoden wie Vermittlung und Dialog zur Lösung von Konflikten",
            "Zivile Konfliktbearbeitung setzt auf gewaltfreie Instrumente wie Vermittlung, Dialogprozesse und Versöhnungsarbeit statt militärischer Mittel."),
        ("Was ist ein Beispiel für ein Instrument ziviler Konfliktbearbeitung?", new[] { "Vermittlungsgespräche zwischen verfeindeten Gruppen durch neutrale Dritte", "Ein militärischer Angriff auf eine Konfliktpartei", "Wirtschaftliche Sanktionen ohne jeden Dialog" }, "Vermittlungsgespräche zwischen verfeindeten Gruppen durch neutrale Dritte",
            "Neutrale Vermittler helfen bei zivilen Konfliktbearbeitungsprozessen dabei, zwischen verfeindeten Gruppen einen Dialog zu ermöglichen."),
        ("Was ist \"Entwicklungspolitik\"?", new[] { "Politische Maßnahmen und Programme zur Unterstützung der wirtschaftlichen und sozialen Entwicklung ärmerer Länder", "Ein anderes Wort für Verteidigungspolitik", "Eine rein innenpolitische Angelegenheit ohne internationalen Bezug" }, "Politische Maßnahmen und Programme zur Unterstützung der wirtschaftlichen und sozialen Entwicklung ärmerer Länder",
            "Entwicklungspolitik umfasst staatliche und internationale Maßnahmen, die ärmere Länder bei wirtschaftlicher und sozialer Entwicklung unterstützen sollen."),
        ("Wie kann nachhaltige Entwicklungspolitik zur Friedenssicherung beitragen?", new[] { "Sie kann Armut und soziale Ungleichheit verringern, die oft Konfliktursachen sind", "Entwicklungspolitik hat mit Friedenssicherung grundsätzlich nichts zu tun", "Sie verschärft Konflikte in jedem Fall zusätzlich" }, "Sie kann Armut und soziale Ungleichheit verringern, die oft Konfliktursachen sind",
            "Da Armut und soziale Ungleichheit häufig zu Konflikten beitragen, kann nachhaltige Entwicklungspolitik präventiv zur Friedenssicherung beitragen."),
        ("Was ist die außenpolitische Rolle Deutschlands im internationalen Vergleich oft beschrieben?", new[] { "Als wirtschaftlich starkes Land mit Engagement in internationalen Organisationen wie UN und EU", "Als Land, das sich aus jeglicher internationalen Politik komplett heraushält", "Als Land ohne jede außenpolitische Bedeutung" }, "Als wirtschaftlich starkes Land mit Engagement in internationalen Organisationen wie UN und EU",
            "Deutschland gilt als wirtschaftlich bedeutendes Land, das sich in internationalen Organisationen wie den Vereinten Nationen und der EU aktiv einbringt."),
        ("Was ist ein Beispiel für zivile Friedensfachkräfte (z.B. entsendet von NGOs)?", new[] { "Fachleute, die vor Ort in Krisengebieten bei Dialog, Versöhnung und Wiederaufbau helfen", "Ausschließlich bewaffnete Soldaten im Auslandseinsatz", "Ein anderes Wort für Diplomatinnen und Diplomaten" }, "Fachleute, die vor Ort in Krisengebieten bei Dialog, Versöhnung und Wiederaufbau helfen",
            "Zivile Friedensfachkräfte unterstützen vor Ort in Krisenregionen gewaltfrei bei Dialogprozessen, Versöhnung und dem Wiederaufbau der Gesellschaft."),
        ("Was ist eine Herausforderung für Entwicklungspolitik, wenn Hilfe in Krisengebiete geliefert wird?", new[] { "Sicherzustellen, dass Hilfe tatsächlich bei den betroffenen Menschen ankommt, statt z.B. durch Korruption verlorenzugehen", "Krisengebiete benötigen grundsätzlich keinerlei Entwicklungshilfe", "Entwicklungshilfe erreicht immer automatisch alle Betroffenen ohne jedes Problem" }, "Sicherzustellen, dass Hilfe tatsächlich bei den betroffenen Menschen ankommt, statt z.B. durch Korruption verlorenzugehen",
            "Eine zentrale Herausforderung der Entwicklungspolitik ist, dass Hilfsgelder und -güter tatsächlich bei den bedürftigen Menschen ankommen und nicht durch Korruption oder Misswirtschaft verloren gehen."),
        ("Was kann langfristige Bildungsförderung in einem von Konflikten betroffenen Land bewirken?", new[] { "Sie kann wirtschaftliche Perspektiven verbessern und langfristig zur Stabilität beitragen", "Bildungsförderung hat auf gesellschaftliche Stabilität keinerlei Einfluss", "Sie verschärft gesellschaftliche Konflikte grundsätzlich zusätzlich" }, "Sie kann wirtschaftliche Perspektiven verbessern und langfristig zur Stabilität beitragen",
            "Bessere Bildungschancen können wirtschaftliche Perspektiven eröffnen und dadurch langfristig zu größerer gesellschaftlicher Stabilität beitragen."),
        ("Was ist ein Ziel von Wiederaufbauprogrammen nach einem bewaffneten Konflikt?", new[] { "Zerstörte Infrastruktur und gesellschaftliche Strukturen wiederherzustellen", "Zerstörungen bewusst bestehen zu lassen", "Ausschließlich neue militärische Anlagen zu errichten" }, "Zerstörte Infrastruktur und gesellschaftliche Strukturen wiederherzustellen",
            "Wiederaufbauprogramme sollen nach einem Konflikt zerstörte Infrastruktur, Wirtschaft und gesellschaftliche Strukturen wiederherstellen und so zur Stabilisierung beitragen."),
        ("Was ist ein Beispiel für eine Rolle, die eine zivile Friedenshelferin bzw. ein ziviler Friedenshelfer in einem Krisengebiet übernehmen könnte?", new[] { "Vermittlung zwischen verfeindeten lokalen Gruppen und Unterstützung beim Dialogaufbau", "Die Leitung eines militärischen Kampfeinsatzes", "Die alleinige politische Führung des Krisenlandes" }, "Vermittlung zwischen verfeindeten lokalen Gruppen und Unterstützung beim Dialogaufbau",
            "Zivile Friedenshelferinnen und -helfer unterstützen oft dabei, zwischen verfeindeten lokalen Gruppen Vertrauen und Dialog aufzubauen."),
        ("Was ist ein Zielkonflikt, der bei Entwicklungspolitik häufig diskutiert wird?", new[] { "Kurzfristige wirtschaftliche Interessen der Geberländer gegenüber den langfristigen Bedürfnissen der Empfängerländer", "Es gibt bei der Entwicklungspolitik grundsätzlich keinerlei Zielkonflikte", "Geber- und Empfängerländer haben immer exakt identische Interessen" }, "Kurzfristige wirtschaftliche Interessen der Geberländer gegenüber den langfristigen Bedürfnissen der Empfängerländer",
            "Ein häufig diskutierter Zielkonflikt in der Entwicklungspolitik ist die Frage, ob Hilfe eher eigenen wirtschaftlichen Interessen der Geberländer oder den tatsächlichen langfristigen Bedürfnissen der Empfängerländer dient."),
        ("Was ist ein Vorteil präventiver Entwicklungszusammenarbeit gegenüber reiner Krisenreaktion?", new[] { "Konflikte können idealerweise verhindert werden, statt erst nach ihrem Ausbruch zu reagieren", "Präventive Maßnahmen sind grundsätzlich immer wirkungslos", "Krisenreaktion ist immer effektiver als Prävention" }, "Konflikte können idealerweise verhindert werden, statt erst nach ihrem Ausbruch zu reagieren",
            "Präventive Entwicklungszusammenarbeit setzt an möglichen Konfliktursachen wie Armut oder Ungleichheit an, bevor ein Konflikt überhaupt ausbricht."),
        ("Was ist eine typische Aufgabe der Gesellschaft für Internationale Zusammenarbeit (GIZ) als deutsche Institution?", new[] { "Sie setzt im Auftrag der Bundesregierung Entwicklungszusammenarbeitsprojekte in anderen Ländern um", "Sie ist ausschließlich für die Verteidigungspolitik zuständig", "Sie hat mit Entwicklungspolitik nichts zu tun" }, "Sie setzt im Auftrag der Bundesregierung Entwicklungszusammenarbeitsprojekte in anderen Ländern um",
            "Die GIZ setzt im Auftrag der Bundesregierung praktische Projekte der internationalen Zusammenarbeit und Entwicklungspolitik um."),
        ("Was können Bildungsprojekte, Gesundheitsversorgung und Infrastrukturaufbau in der Entwicklungszusammenarbeit gemeinsam bewirken?", new[] { "Sie verbessern grundlegende Lebensbedingungen und können so gesellschaftliche Stabilität fördern", "Sie haben keinerlei Zusammenhang mit gesellschaftlicher Stabilität", "Sie verschlechtern grundsätzlich die Lebensbedingungen vor Ort" }, "Sie verbessern grundlegende Lebensbedingungen und können so gesellschaftliche Stabilität fördern",
            "Verbesserte Bildung, Gesundheitsversorgung und Infrastruktur tragen zu besseren Lebensbedingungen bei, was wiederum gesellschaftliche Stabilität begünstigen kann."),
        ("Was ist eine Kritik, die manchmal an internationaler Entwicklungshilfe geäußert wird?", new[] { "Sie könne in manchen Fällen Abhängigkeiten schaffen, statt eigenständige Entwicklung zu fördern", "Entwicklungshilfe wird niemals kritisch diskutiert", "Entwicklungshilfe hat historisch niemals irgendwelche Nebenwirkungen gehabt" }, "Sie könne in manchen Fällen Abhängigkeiten schaffen, statt eigenständige Entwicklung zu fördern",
            "Kritikerinnen und Kritiker bemängeln teils, dass bestimmte Formen der Entwicklungshilfe langfristige Abhängigkeiten schaffen könnten, statt eigenständige wirtschaftliche Entwicklung ausreichend zu fördern."),
        ("Was zeigt der Perspektivenwechsel in die Rolle einer zivilen Friedenshelferin bzw. eines zivilen Friedenshelfers besonders gut?", new[] { "Welche konkreten Handlungsspielräume und Herausforderungen in einem Krisengebiet bestehen", "Dass Friedensarbeit in der Praxis überhaupt keine Herausforderungen mit sich bringt", "Dass zivile Friedensarbeit dieselben Aufgaben wie militärische Einsätze hat" }, "Welche konkreten Handlungsspielräume und Herausforderungen in einem Krisengebiet bestehen",
            "Ein Perspektivenwechsel in die Rolle einer Friedensfachkraft verdeutlicht die praktischen Handlungsmöglichkeiten und Grenzen ziviler Friedensarbeit vor Ort."),
        ("Was ist ein Beispiel für die außenpolitische Verantwortung, die Deutschland aufgrund seiner Geschichte oft zugeschrieben wird?", new[] { "Besonderes Engagement für Menschenrechte, internationale Zusammenarbeit und Konfliktprävention", "Ein bewusster Rückzug aus jeglicher internationaler Verantwortung", "Ausschließlich wirtschaftliche Eigeninteressen ohne jede weitere Verantwortung" }, "Besonderes Engagement für Menschenrechte, internationale Zusammenarbeit und Konfliktprävention",
            "Aufgrund seiner Geschichte wird Deutschland oft eine besondere außenpolitische Verantwortung zugeschrieben, sich für Menschenrechte, internationale Zusammenarbeit und Konfliktprävention einzusetzen."),
        ("Was ist ein Beispiel dafür, wie Entwicklungspolitik und Friedenssicherung zusammenhängen können?", new[] { "Wirtschaftliche Perspektiven durch Entwicklungsprojekte können Fluchtursachen und Konfliktpotenzial verringern", "Entwicklungspolitik und Friedenssicherung sind komplett unabhängige, unverbundene Politikbereiche", "Entwicklungsprojekte erhöhen immer automatisch das Konfliktrisiko" }, "Wirtschaftliche Perspektiven durch Entwicklungsprojekte können Fluchtursachen und Konfliktpotenzial verringern",
            "Indem Entwicklungsprojekte wirtschaftliche Perspektiven vor Ort schaffen, können sie langfristig sowohl Fluchtursachen als auch Konfliktpotenzial verringern."),
        ("Was ist ein Beispiel für multilaterale Entwicklungszusammenarbeit im Unterschied zu bilateraler Hilfe?", new[] { "Mehrere Geberländer und internationale Organisationen finanzieren gemeinsam Entwicklungsprojekte", "Nur ein einzelnes Geberland unterstützt direkt ein einzelnes Empfängerland", "Multilaterale Zusammenarbeit existiert in der Entwicklungspolitik nicht" }, "Mehrere Geberländer und internationale Organisationen finanzieren gemeinsam Entwicklungsprojekte",
            "Bei multilateraler Entwicklungszusammenarbeit bündeln mehrere Geberländer und internationale Organisationen (z.B. über die Weltbank) ihre Mittel für gemeinsame Projekte."),
        ("Was ist ein Ziel der Vereinten Nationen mit den \"Nachhaltigkeitszielen\" (Sustainable Development Goals) im Bereich Frieden?", new[] { "Frieden, Gerechtigkeit und starke Institutionen weltweit zu fördern", "Ausschließlich wirtschaftliches Wachstum ohne jeden weiteren Bezug", "Die Abschaffung aller internationalen Organisationen" }, "Frieden, Gerechtigkeit und starke Institutionen weltweit zu fördern",
            "Eines der 17 UN-Nachhaltigkeitsziele (SDG 16) widmet sich explizit Frieden, Gerechtigkeit und starken, verantwortungsvollen Institutionen weltweit.")
    };

    private static QuizQuestion FriedenssicherungUndEntwicklungspolitik(Random r)
    {
        var f = FriedenssicherungListe[r.Next(FriedenssicherungListe.Length)];
        return new QuizQuestion
        {
            Id = NewId(), Subject = Subject.Politik, GradeLevel = GradeLevel.Klasse9,
            Topic = "Friedenssicherung und Entwicklungspolitik", Type = QuestionType.MultipleChoice,
            Prompt = f.Frage, Options = f.Optionen, CorrectAnswers = new[] { f.Antwort }, Explanation = f.Erklaerung,
            HelpHint = "Zivile Konfliktbearbeitung setzt auf gewaltfreie Mittel wie Vermittlung und Dialog; nachhaltige Entwicklungspolitik kann Armut und Konfliktursachen verringern und so präventiv zur Friedenssicherung beitragen."
        };
    }

    private static readonly (string Frage, string[] Optionen, string Antwort, string Erklaerung)[] EuropaeischeUnionListe =
    {
        ("Was ist der europäische Integrationsprozess?", new[] { "Die schrittweise politische, wirtschaftliche und rechtliche Annäherung europäischer Staaten seit dem Zweiten Weltkrieg", "Ein einmaliges, abgeschlossenes Ereignis ohne weitere Entwicklung", "Ein Begriff, der ausschließlich die deutsche Wiedervereinigung beschreibt" }, "Die schrittweise politische, wirtschaftliche und rechtliche Annäherung europäischer Staaten seit dem Zweiten Weltkrieg",
            "Der europäische Integrationsprozess beschreibt die seit den 1950er-Jahren fortschreitende politische, wirtschaftliche und rechtliche Zusammenarbeit europäischer Staaten, die zur heutigen EU führte."),
        ("Was ist das Europäische Parlament?", new[] { "Die von den Bürgerinnen und Bürgern der EU direkt gewählte Vertretung auf EU-Ebene", "Ein Gericht für Verkehrsdelikte", "Ein rein beratendes Gremium ohne jede gesetzgeberische Funktion" }, "Die von den Bürgerinnen und Bürgern der EU direkt gewählte Vertretung auf EU-Ebene",
            "Das Europäische Parlament wird direkt von den EU-Bürgerinnen und -Bürgern gewählt und wirkt maßgeblich an der EU-Gesetzgebung mit."),
        ("Wer schlägt im EU-Gesetzgebungsverfahren typischerweise neue Gesetze vor?", new[] { "Die Europäische Kommission", "Ausschließlich einzelne Bürgerinnen und Bürger direkt", "Der Europäische Gerichtshof" }, "Die Europäische Kommission",
            "Die Europäische Kommission hat im EU-Gesetzgebungsverfahren meist das alleinige Initiativrecht, Gesetzesvorschläge einzubringen."),
        ("Wer entscheidet im EU-Gesetzgebungsverfahren letztlich über die meisten Gesetzesvorschläge?", new[] { "Das Europäische Parlament und der Rat der Europäischen Union gemeinsam", "Ausschließlich die Europäische Kommission allein", "Ausschließlich der Europäische Gerichtshof" }, "Das Europäische Parlament und der Rat der Europäischen Union gemeinsam",
            "Im ordentlichen Gesetzgebungsverfahren entscheiden Europäisches Parlament und Rat der EU (Vertretung der Mitgliedstaaten) gemeinsam über die meisten EU-Gesetze."),
        ("Was bedeutet \"demokratische Legitimation\" der EU?", new[] { "Die EU-Institutionen leiten ihre Macht letztlich von den Bürgerinnen und Bürgern bzw. gewählten Regierungen der Mitgliedstaaten ab", "Die EU trifft alle Entscheidungen völlig unabhängig von demokratischen Prozessen", "Ein Begriff, der mit der EU nichts zu tun hat" }, "Die EU-Institutionen leiten ihre Macht letztlich von den Bürgerinnen und Bürgern bzw. gewählten Regierungen der Mitgliedstaaten ab",
            "Demokratische Legitimation bedeutet, dass die Macht der EU-Institutionen letztlich auf demokratischen Wahlen der Bürgerinnen und Bürger sowie der Mitgliedstaaten beruht."),
        ("Was ist eine \"Europäische Bürgerinitiative\"?", new[] { "Ein Instrument, mit dem Bürgerinnen und Bürger die EU-Kommission auffordern können, sich mit einem bestimmten Thema zu befassen", "Ein Gesetz, das automatisch ohne jede Abstimmung in Kraft tritt", "Ein Begriff, der ausschließlich einzelne Mitgliedstaaten betrifft" }, "Ein Instrument, mit dem Bürgerinnen und Bürger die EU-Kommission auffordern können, sich mit einem bestimmten Thema zu befassen",
            "Mit einer Europäischen Bürgerinitiative können Bürgerinnen und Bürger bei ausreichender Unterstützung die EU-Kommission auffordern, sich mit einem bestimmten Anliegen zu befassen."),
        ("Wie viele Unterschriften sind für eine erfolgreiche Europäische Bürgerinitiative ungefähr notwendig?", new[] { "Rund eine Million aus mehreren Mitgliedstaaten", "Nur 100 Unterschriften", "Über eine Milliarde Unterschriften" }, "Rund eine Million aus mehreren Mitgliedstaaten",
            "Eine Europäische Bürgerinitiative benötigt etwa eine Million Unterschriften, die aus einer Mindestanzahl von Mitgliedstaaten stammen müssen."),
        ("Was bedeutet \"europäische Identität\" für Bürgerinnen und Bürger unterschiedlicher Mitgliedstaaten?", new[] { "Ein Gefühl der gemeinsamen Zugehörigkeit zu Europa trotz nationaler und kultureller Unterschiede", "Dass alle EU-Bürgerinnen und -Bürger dieselbe Sprache sprechen müssen", "Ein Begriff ohne jede politische Bedeutung" }, "Ein Gefühl der gemeinsamen Zugehörigkeit zu Europa trotz nationaler und kultureller Unterschiede",
            "Europäische Identität beschreibt ein gemeinsames Zugehörigkeitsgefühl der Bürgerinnen und Bürger zu Europa, das neben der jeweiligen nationalen Identität besteht."),
        ("Was ist mit der \"Finalität der EU\" (Bundesstaat vs. Staatenbund) gemeint?", new[] { "Die offene Frage, wie eng die politische Integration der EU langfristig werden soll", "Ein bereits abschließend entschiedenes, unveränderliches Ergebnis", "Ein Begriff, der ausschließlich die Wirtschaftspolitik betrifft" }, "Die offene Frage, wie eng die politische Integration der EU langfristig werden soll",
            "Die Debatte um die \"Finalität\" der EU dreht sich um die grundsätzliche, bislang ungeklärte Frage, ob sich die EU langfristig zu einem Bundesstaat entwickeln oder eher ein Staatenbund souveräner Länder bleiben soll."),
        ("Was ist ein Argument für eine engere politische Integration der EU (hin zu einem Bundesstaat)?", new[] { "Gemeinsame Herausforderungen (z.B. Klimawandel, Sicherheit) ließen sich effizienter gemeinsam lösen", "Eine engere Integration hätte grundsätzlich keinerlei Vorteile", "Nationale Alleingänge seien in jedem Fall wirksamer als gemeinsames Handeln" }, "Gemeinsame Herausforderungen (z.B. Klimawandel, Sicherheit) ließen sich effizienter gemeinsam lösen",
            "Befürworterinnen und Befürworter engerer Integration argumentieren, dass große grenzüberschreitende Herausforderungen gemeinsam wirksamer angegangen werden können."),
        ("Was ist ein Argument gegen eine zu enge politische Integration der EU?", new[] { "Sorge um den Verlust nationaler Souveränität und demokratischer Mitbestimmung auf nationaler Ebene", "Es gibt gegen engere Integration grundsätzlich keinerlei Argumente", "Engere Integration würde automatisch alle nationalen Interessen vollständig berücksichtigen" }, "Sorge um den Verlust nationaler Souveränität und demokratischer Mitbestimmung auf nationaler Ebene",
            "Kritikerinnen und Kritiker befürchten bei zu enger Integration einen Verlust nationaler Entscheidungsspielräume und demokratischer Mitbestimmung auf Ebene der einzelnen Staaten."),
        ("Was ist der Europäische Rat (nicht zu verwechseln mit dem Rat der EU)?", new[] { "Das Gremium der Staats- und Regierungschefs der Mitgliedstaaten, das die politischen Leitlinien der EU festlegt", "Ein Gericht, das Rechtsstreitigkeiten zwischen Bürgern entscheidet", "Ein anderes Wort für das Europäische Parlament" }, "Das Gremium der Staats- und Regierungschefs der Mitgliedstaaten, das die politischen Leitlinien der EU festlegt",
            "Im Europäischen Rat legen die Staats- und Regierungschefs der Mitgliedstaaten die grundlegenden politischen Leitlinien und Prioritäten der EU fest."),
        ("Was ist der Europäische Gerichtshof (EuGH)?", new[] { "Das oberste Gericht der EU, das über die Auslegung und Anwendung des EU-Rechts entscheidet", "Ein Gremium zur Wahl des EU-Kommissionspräsidenten", "Ein anderes Wort für den Rat der Europäischen Union" }, "Das oberste Gericht der EU, das über die Auslegung und Anwendung des EU-Rechts entscheidet",
            "Der Europäische Gerichtshof stellt sicher, dass EU-Recht in allen Mitgliedstaaten einheitlich ausgelegt und angewendet wird."),
        ("Was zeigt die Debatte um die EU-Migrationspolitik häufig über unterschiedliche Interessen der Mitgliedstaaten?", new[] { "Mitgliedstaaten haben teils unterschiedliche Positionen zur Verteilung von Verantwortung und Solidarität", "Alle Mitgliedstaaten vertreten in dieser Frage immer exakt dieselbe Position", "Migrationspolitik ist in der EU historisch niemals umstritten gewesen" }, "Mitgliedstaaten haben teils unterschiedliche Positionen zur Verteilung von Verantwortung und Solidarität",
            "Die EU-Migrationspolitik zeigt exemplarisch, wie unterschiedlich Mitgliedstaaten Fragen der Verantwortungsteilung und Solidarität bewerten können."),
        ("Was können Bürgerinnen und Bürger über eine Europäische Bürgerinitiative konkret erreichen wollen, z.B. bei Umweltthemen?", new[] { "Die EU-Kommission dazu bewegen, einen Gesetzesvorschlag zu einem bestimmten Umweltthema zu prüfen", "Direkt und ohne jedes weitere Verfahren ein neues EU-Gesetz erlassen", "Ausschließlich eine einzelne nationale Regierung zur Reaktion zwingen" }, "Die EU-Kommission dazu bewegen, einen Gesetzesvorschlag zu einem bestimmten Umweltthema zu prüfen",
            "Eine erfolgreiche Bürgerinitiative fordert die Kommission auf, sich mit einem Thema zu befassen - sie erlässt aber nicht automatisch selbst ein neues Gesetz."),
        ("Was ist ein Beispiel für gemeinsame europäische Politik im Bereich Migration?", new[] { "Absprachen zur Verteilung von Geflüchteten oder zum gemeinsamen Grenzschutz zwischen Mitgliedstaaten", "Jeder Mitgliedstaat handelt bei Migration völlig unabhängig ohne jede Abstimmung", "Migrationspolitik liegt ausschließlich bei den Vereinten Nationen" }, "Absprachen zur Verteilung von Geflüchteten oder zum gemeinsamen Grenzschutz zwischen Mitgliedstaaten",
            "Die EU versucht u.a. durch Absprachen zur Verteilung von Geflüchteten und zum gemeinsamen Außengrenzschutz eine koordinierte Migrationspolitik zu gestalten."),
        ("Was ist ein Grund, warum die Frage nach der \"Finalität\" der EU politisch kontrovers diskutiert wird?", new[] { "Mitgliedstaaten haben unterschiedliche Vorstellungen davon, wie viel Souveränität sie an die EU abgeben wollen", "Alle Mitgliedstaaten haben zu dieser Frage historisch immer dieselbe Meinung vertreten", "Die Frage der Finalität ist rein technischer Natur ohne politische Bedeutung" }, "Mitgliedstaaten haben unterschiedliche Vorstellungen davon, wie viel Souveränität sie an die EU abgeben wollen",
            "Da Mitgliedstaaten unterschiedlich viel nationale Souveränität an die EU abgeben möchten, bleibt die Frage nach der langfristigen Ausgestaltung (Finalität) der EU politisch umstritten."),
        ("Was ist ein Beispiel für praktische Partizipationsmöglichkeiten von EU-Bürgerinnen und -Bürgern neben Wahlen zum Europäischen Parlament?", new[] { "Die Nutzung der Europäischen Bürgerinitiative oder die Teilnahme an öffentlichen EU-Konsultationen", "EU-Bürgerinnen und -Bürger haben außerhalb von Wahlen keinerlei Mitwirkungsmöglichkeiten", "Nur nationale Regierungschefs dürfen sich an EU-Prozessen beteiligen" }, "Die Nutzung der Europäischen Bürgerinitiative oder die Teilnahme an öffentlichen EU-Konsultationen",
            "Neben der Wahl zum Europäischen Parlament können sich Bürgerinnen und Bürger z.B. über Europäische Bürgerinitiativen oder öffentliche Konsultationen der EU-Institutionen einbringen."),
        ("Wie viele Mitgliedstaaten hat die Europäische Union ungefähr (Stand nach dem Brexit)?", new[] { "27", "12", "50" }, "27",
            "Nach dem Austritt Großbritanniens (Brexit) im Jahr 2020 hat die EU 27 Mitgliedstaaten."),
        ("Was ist der EU-Binnenmarkt?", new[] { "Ein gemeinsamer Wirtschaftsraum, in dem Waren, Dienstleistungen, Kapital und Personen sich weitgehend frei bewegen können", "Ein Markt, der ausschließlich für ein einziges Mitgliedsland gilt", "Ein Begriff, der nur den Handel mit Nicht-EU-Ländern beschreibt" }, "Ein gemeinsamer Wirtschaftsraum, in dem Waren, Dienstleistungen, Kapital und Personen sich weitgehend frei bewegen können",
            "Der EU-Binnenmarkt ermöglicht den weitgehend freien Austausch von Waren, Dienstleistungen, Kapital und Personen zwischen den Mitgliedstaaten.")
    };

    private static QuizQuestion EuropaeischeUnion(Random r)
    {
        var f = EuropaeischeUnionListe[r.Next(EuropaeischeUnionListe.Length)];
        return new QuizQuestion
        {
            Id = NewId(), Subject = Subject.Politik, GradeLevel = GradeLevel.Klasse9,
            Topic = "Europa in der Welt: Die Europäische Union", Type = QuestionType.MultipleChoice,
            Prompt = f.Frage, Options = f.Optionen, CorrectAnswers = new[] { f.Antwort }, Explanation = f.Erklaerung,
            HelpHint = "Im EU-Gesetzgebungsverfahren schlägt die Kommission Gesetze vor, über die Parlament und Rat gemeinsam entscheiden; die Debatte um die \"Finalität\" (Bundesstaat vs. Staatenbund) und Instrumente wie die Europäische Bürgerinitiative prägen die europäische Integration."
        };
    }

    private static readonly (string Frage, string[] Optionen, string Antwort, string Erklaerung)[] ArmutReichtumPolitikListe =
    {
        ("Was ist der Sozialstaat, einfach erklärt?", new[] { "Ein Staat, der Menschen in Not finanziell und sozial unterstützt", "Ein Staat, der sich nicht um seine Bürger kümmert", "Ein anderes Wort für Diktatur" }, "Ein Staat, der Menschen in Not finanziell und sozial unterstützt",
            "Der Sozialstaat sichert Menschen in schwierigen Lebenslagen ab, z.B. bei Arbeitslosigkeit, Krankheit oder Armut."),
        ("Was ist der Mindestlohn?", new[] { "Der gesetzlich niedrigste Stundenlohn, den Arbeitgeber zahlen müssen", "Der höchste erlaubte Stundenlohn in Deutschland", "Ein freiwilliges Trinkgeld für Angestellte" }, "Der gesetzlich niedrigste Stundenlohn, den Arbeitgeber zahlen müssen",
            "Der Mindestlohn legt gesetzlich fest, wie wenig Arbeitgeber ihren Angestellten pro Stunde mindestens zahlen dürfen."),
        ("Was ist \"Kinderarmut\" in Deutschland?", new[] { "Wenn Kinder in Familien aufwachsen, die nur sehr wenig Geld zur Verfügung haben", "Ein anderes Wort für Kinderarbeit", "Ein Zustand, der in Deutschland gar nicht existiert" }, "Wenn Kinder in Familien aufwachsen, die nur sehr wenig Geld zur Verfügung haben",
            "Kinderarmut bedeutet, dass Familien mit Kindern nur über sehr geringe finanzielle Mittel verfügen, was den Alltag der Kinder einschränkt."),
        ("Was ist die \"Tafel\" in Deutschland?", new[] { "Eine Organisation, die überschüssige Lebensmittel an bedürftige Menschen verteilt", "Eine staatliche Behörde für Steuerangelegenheiten", "Ein anderes Wort für Klassenzimmer" }, "Eine Organisation, die überschüssige Lebensmittel an bedürftige Menschen verteilt",
            "Die Tafeln sammeln überschüssige, aber noch genießbare Lebensmittel und geben sie kostengünstig oder kostenlos an Bedürftige weiter."),
        ("Was bedeutet \"Umverteilung\" durch Steuern, vereinfacht?", new[] { "Wer mehr verdient, zahlt mehr Steuern - davon werden u.a. Unterstützungsleistungen finanziert", "Alle zahlen exakt denselben Steuerbetrag", "Steuern werden ausschließlich an reiche Menschen ausgezahlt" }, "Wer mehr verdient, zahlt mehr Steuern - davon werden u.a. Unterstützungsleistungen finanziert",
            "Durch progressive Steuern zahlen Menschen mit höherem Einkommen anteilig mehr - mit diesem Geld werden u.a. Sozialleistungen finanziert."),
        ("Was ist Wohngeld?", new[] { "Eine staatliche Unterstützung für Menschen mit geringem Einkommen bei den Mietkosten", "Eine Steuer auf den Kauf einer Wohnung", "Ein Bonus für den Kauf von Möbeln" }, "Eine staatliche Unterstützung für Menschen mit geringem Einkommen bei den Mietkosten",
            "Wohngeld hilft Menschen mit geringem Einkommen, sich die Miete für eine Wohnung leisten zu können."),
        ("Was ist Bürgergeld?", new[] { "Eine staatliche Grundsicherung für Menschen ohne ausreichendes eigenes Einkommen", "Ein Bonus, den jeder Bürger unabhängig vom Einkommen erhält", "Eine Steuer, die alle Bürger zahlen müssen" }, "Eine staatliche Grundsicherung für Menschen ohne ausreichendes eigenes Einkommen",
            "Das Bürgergeld sichert Menschen ohne ausreichendes eigenes Einkommen ein finanzielles Existenzminimum."),
        ("Warum haben manche Kinder in Deutschland weniger Chancen auf einen guten Schulabschluss?", new[] { "Finanzielle Not der Familie kann z.B. Nachhilfe, Ausflüge oder ruhiges Lernen erschweren", "Alle Kinder haben in Deutschland exakt dieselben Startbedingungen", "Schulnoten hängen ausschließlich vom Zufall ab" }, "Finanzielle Not der Familie kann z.B. Nachhilfe, Ausflüge oder ruhiges Lernen erschweren",
            "Finanzielle und familiäre Rahmenbedingungen beeinflussen, wie gut Kinder in der Schule unterstützt werden können."),
        ("Was kann die Politik tun, um Kinderarmut zu bekämpfen?", new[] { "Zum Beispiel finanzielle Unterstützung für Familien und kostenlose Bildungsangebote schaffen", "Kinderarmut lässt sich politisch grundsätzlich nicht beeinflussen", "Nur die betroffenen Familien selbst können etwas tun" }, "Zum Beispiel finanzielle Unterstützung für Familien und kostenlose Bildungsangebote schaffen",
            "Politische Maßnahmen wie Familienleistungen, kostenlose Schulmaterialien oder Ganztagsbetreuung können Kinderarmut verringern."),
        ("Was ist ein Beispiel für eine soziale Einrichtung, die Familien mit wenig Geld unterstützt?", new[] { "Zum Beispiel eine Suppenküche, Kleiderkammer oder ein Familienzentrum", "Ein privates Luxushotel", "Eine Bank für Wertpapierhandel" }, "Zum Beispiel eine Suppenküche, Kleiderkammer oder ein Familienzentrum",
            "Solche Einrichtungen bieten bedürftigen Familien praktische Unterstützung im Alltag, von Essen bis Kleidung."),
        ("Was versteht man unter \"Chancengleichheit\"?", new[] { "Alle Menschen sollen unabhängig von ihrer Herkunft die gleichen Möglichkeiten im Leben haben", "Nur reiche Menschen sollen gute Chancen bekommen", "Chancengleichheit bedeutet, dass alle exakt dasselbe Einkommen haben" }, "Alle Menschen sollen unabhängig von ihrer Herkunft die gleichen Möglichkeiten im Leben haben",
            "Chancengleichheit bedeutet, dass der Erfolg im Leben nicht von der Herkunft, sondern möglichst von den eigenen Fähigkeiten abhängen soll."),
        ("Was ist ein Grund, warum Chancengleichheit in der Praxis oft nicht vollständig erreicht wird?", new[] { "Unterschiedliche finanzielle und familiäre Startbedingungen wirken sich auf Bildungschancen aus", "Alle Kinder starten grundsätzlich mit exakt denselben Bedingungen", "Chancengleichheit ist gesetzlich verboten" }, "Unterschiedliche finanzielle und familiäre Startbedingungen wirken sich auf Bildungschancen aus",
            "Trotz des Ziels der Chancengleichheit beeinflussen reale Unterschiede in Familie und Einkommen oft die tatsächlichen Bildungschancen."),
        ("Was ist \"Elterngeld\"?", new[] { "Eine staatliche finanzielle Unterstützung für Eltern in der ersten Zeit nach der Geburt eines Kindes", "Ein Bonus, den Kinder direkt selbst erhalten", "Eine Steuer auf die Geburt eines Kindes" }, "Eine staatliche finanzielle Unterstützung für Eltern in der ersten Zeit nach der Geburt eines Kindes",
            "Elterngeld unterstützt Eltern finanziell, wenn sie nach der Geburt eines Kindes weniger oder gar nicht arbeiten."),
        ("Warum gibt es in Deutschland eine gesetzliche Kranken- und Rentenversicherung?", new[] { "Damit alle Menschen im Krankheits- oder Rentenfall abgesichert sind, unabhängig vom eigenen Vermögen", "Damit nur reiche Menschen im Alter abgesichert sind", "Diese Versicherungen sind in Deutschland verboten" }, "Damit alle Menschen im Krankheits- oder Rentenfall abgesichert sind, unabhängig vom eigenen Vermögen",
            "Die gesetzlichen Versicherungen sollen sicherstellen, dass niemand allein wegen Krankheit oder Alter in existenzielle Not gerät."),
        ("Was ist ein Beispiel für eine Ungleichheit zwischen Arm und Reich, die man im Alltag beobachten kann?", new[] { "Zum Beispiel unterschiedlicher Zugang zu Nachhilfe, Freizeitangeboten oder Wohnraum", "In Deutschland gibt es überhaupt keine Ungleichheiten mehr", "Alle Menschen haben immer denselben Wohnraum" }, "Zum Beispiel unterschiedlicher Zugang zu Nachhilfe, Freizeitangeboten oder Wohnraum",
            "Finanzielle Unterschiede zeigen sich im Alltag oft beim Zugang zu Bildung, Freizeit und Wohnsituation."),
        ("Welche Unterstützung kann eine Familie mit geringem Einkommen für Vereinssport oder Ausflüge in Deutschland beantragen?", new[] { "Leistungen aus dem \"Bildungs- und Teilhabepaket\"", "Es gibt dafür keinerlei staatliche Unterstützung", "Nur eine private Spende von Nachbarn" }, "Leistungen aus dem \"Bildungs- und Teilhabepaket\"",
            "Das Bildungs- und Teilhabepaket unterstützt Kinder aus einkommensschwachen Familien z.B. bei Vereinsbeiträgen, Nachhilfe oder Ausflügen."),
        ("Was ist ein Ziel der Politik, wenn sie den Mindestlohn erhöht?", new[] { "Menschen mit niedrigem Einkommen sollen von ihrer Arbeit besser leben können", "Unternehmen sollen dadurch weniger Gewinn machen dürfen", "Es gibt dabei überhaupt kein politisches Ziel" }, "Menschen mit niedrigem Einkommen sollen von ihrer Arbeit besser leben können",
            "Eine Erhöhung des Mindestlohns soll sicherstellen, dass Menschen mit einfachen Jobs von ihrem Einkommen besser leben können."),
        ("Warum wird in Deutschland politisch oft über die \"Vermögensverteilung\" diskutiert?", new[] { "Weil ein kleiner Teil der Bevölkerung einen großen Teil des Gesamtvermögens besitzt", "Weil alle Menschen in Deutschland exakt gleich viel besitzen", "Vermögensverteilung ist in Deutschland kein politisches Thema" }, "Weil ein kleiner Teil der Bevölkerung einen großen Teil des Gesamtvermögens besitzt",
            "In Deutschland ist Vermögen sehr ungleich verteilt - ein vergleichsweise kleiner Teil der Bevölkerung besitzt einen großen Anteil des Gesamtvermögens."),
        ("Was ist eine Grundsicherung im Alter?", new[] { "Eine staatliche Unterstützung für ältere Menschen, deren Rente nicht zum Leben reicht", "Eine zusätzliche Steuer für Rentnerinnen und Rentner", "Ein Bonus, den nur besonders reiche Rentner erhalten" }, "Eine staatliche Unterstützung für ältere Menschen, deren Rente nicht zum Leben reicht",
            "Reicht die eigene Rente nicht für ein Existenzminimum, springt die Grundsicherung im Alter unterstützend ein."),
        ("Warum ist Armutsbekämpfung ein wichtiges politisches Thema in einer Demokratie?", new[] { "Weil soziale Gerechtigkeit und gleiche Teilhabechancen als wichtige gesellschaftliche Ziele gelten", "Weil Armut in einer Demokratie per Gesetz gar nicht existieren darf", "Armutsbekämpfung ist in Demokratien kein relevantes Thema" }, "Weil soziale Gerechtigkeit und gleiche Teilhabechancen als wichtige gesellschaftliche Ziele gelten",
            "Demokratische Gesellschaften streben soziale Gerechtigkeit an - Armutsbekämpfung gehört deshalb zu den zentralen politischen Aufgaben.")
    };

    private static QuizQuestion ArmutUndReichtumPolitik(Random r)
    {
        var f = ArmutReichtumPolitikListe[r.Next(ArmutReichtumPolitikListe.Length)];
        return new QuizQuestion
        {
            Id = NewId(), Subject = Subject.Politik, GradeLevel = GradeLevel.Klasse6,
            Topic = "Armut und Reichtum (Klasse-6-Niveau)", Type = QuestionType.MultipleChoice,
            Prompt = f.Frage, Options = f.Optionen, CorrectAnswers = new[] { f.Antwort }, Explanation = f.Erklaerung,
            HelpHint = "Der deutsche Sozialstaat federt Armut über Leistungen wie Bürgergeld, Wohngeld und Bildungs-/Teilhabepaket ab - finanziert über Steuern nach dem Prinzip: wer mehr verdient, zahlt mehr."
        };
    }

    private static readonly (string Frage, string[] Optionen, string Antwort, string Erklaerung)[] GlobalisierteWeltListe =
    {
        ("Was bedeutet \"Globalisierung\" einfach erklärt?", new[] { "Die zunehmende weltweite Vernetzung von Ländern in Wirtschaft, Kultur und Kommunikation", "Der komplette Rückzug aller Länder voneinander", "Ein anderes Wort für Klimawandel" }, "Die zunehmende weltweite Vernetzung von Ländern in Wirtschaft, Kultur und Kommunikation",
            "Globalisierung beschreibt, wie Länder weltweit wirtschaftlich, kulturell und über Kommunikation immer enger miteinander verbunden werden."),
        ("Was ist ein Beispiel für Globalisierung im Alltag?", new[] { "Kleidung oder Elektronik, die in vielen verschiedenen Ländern hergestellt wird", "Ein Produkt, das ausschließlich im eigenen Dorf hergestellt und verkauft wird", "Ein Brief, der per Post verschickt wird" }, "Kleidung oder Elektronik, die in vielen verschiedenen Ländern hergestellt wird",
            "Viele Alltagsprodukte durchlaufen heute mehrere Produktionsschritte in unterschiedlichen Ländern, bevor sie im Laden landen."),
        ("Warum werden viele Produkte, die wir kaufen, in unterschiedlichen Ländern produziert?", new[] { "Weil einzelne Produktionsschritte dort günstiger oder effizienter erledigt werden können", "Weil es gesetzlich vorgeschrieben ist, Produkte in mehreren Ländern herzustellen", "Aus rein zufälligen Gründen ohne wirtschaftlichen Hintergrund" }, "Weil einzelne Produktionsschritte dort günstiger oder effizienter erledigt werden können",
            "Unternehmen verteilen Produktionsschritte oft dorthin, wo sie am günstigsten oder effizientesten umgesetzt werden können."),
        ("Was ist eine globale \"Lieferkette\", einfach erklärt?", new[] { "Der Weg eines Produkts von den Rohstoffen bis zum fertigen Produkt im Laden, oft über mehrere Länder", "Eine Kette aus Metall, die Waren im Laden sichert", "Ein anderes Wort für Zollgebühr" }, "Der Weg eines Produkts von den Rohstoffen bis zum fertigen Produkt im Laden, oft über mehrere Länder",
            "Eine Lieferkette umfasst alle Schritte von der Rohstoffgewinnung über die Herstellung bis zum fertigen Produkt im Regal - oft über viele Länder verteilt."),
        ("Was ist ein Vorteil der Globalisierung?", new[] { "Menschen weltweit haben Zugang zu mehr Produkten, Wissen und Austausch", "Alle Länder sind dadurch komplett voneinander isoliert", "Globalisierung hat ausschließlich Nachteile" }, "Menschen weltweit haben Zugang zu mehr Produkten, Wissen und Austausch",
            "Durch weltweite Vernetzung profitieren Menschen von einem größeren Angebot an Produkten, Wissen und kulturellem Austausch."),
        ("Was ist ein möglicher Nachteil der Globalisierung?", new[] { "Ungleiche Arbeitsbedingungen oder Umweltbelastung in manchen Produktionsländern", "Globalisierung hat ausschließlich Vorteile ohne jeden Nachteil", "Weniger internationale Kommunikation" }, "Ungleiche Arbeitsbedingungen oder Umweltbelastung in manchen Produktionsländern",
            "Kritisiert wird oft, dass Produktion in manchen Ländern zu schlechten Arbeitsbedingungen oder Umweltschäden führt."),
        ("Was ist das Internet ein gutes Beispiel für in Bezug auf globale Vernetzung?", new[] { "Menschen weltweit können sich in Echtzeit austauschen und Informationen teilen", "Das Internet verbindet ausschließlich Menschen innerhalb eines Landes", "Das Internet hat mit Globalisierung nichts zu tun" }, "Menschen weltweit können sich in Echtzeit austauschen und Informationen teilen",
            "Das Internet ermöglicht sofortigen Austausch von Informationen und Kommunikation über Ländergrenzen hinweg."),
        ("Warum sprechen viele Menschen weltweit heute Englisch als gemeinsame Sprache?", new[] { "Englisch dient international oft als gemeinsame Verständigungssprache in Wirtschaft und Reisen", "Englisch ist die einzige Sprache, die weltweit gesetzlich erlaubt ist", "Alle anderen Sprachen der Welt sind ausgestorben" }, "Englisch dient international oft als gemeinsame Verständigungssprache in Wirtschaft und Reisen",
            "Englisch hat sich international als häufig genutzte gemeinsame Sprache etabliert, etwa in Wirtschaft, Wissenschaft und Tourismus."),
        ("Was ist ein multinationaler Konzern?", new[] { "Ein Unternehmen, das in vielen Ländern der Welt tätig ist", "Ein Unternehmen, das nur in einem einzigen Dorf verkauft", "Eine staatliche Behörde für internationale Beziehungen" }, "Ein Unternehmen, das in vielen Ländern der Welt tätig ist",
            "Multinationale Konzerne produzieren und verkaufen ihre Produkte in vielen verschiedenen Ländern weltweit."),
        ("Was können weltweite Handelsabkommen zwischen Ländern regeln?", new[] { "Zum Beispiel Zölle, Handelsregeln und wirtschaftliche Zusammenarbeit", "Ausschließlich die Farbe von Nationalflaggen", "Sie regeln überhaupt nichts Konkretes" }, "Zum Beispiel Zölle, Handelsregeln und wirtschaftliche Zusammenarbeit",
            "Handelsabkommen legen fest, wie Länder wirtschaftlich zusammenarbeiten, z.B. bei Zöllen und Handelsbestimmungen."),
        ("Was ist ein Grund, warum manche Menschen Globalisierung kritisch sehen?", new[] { "Sie befürchten, dass lokale Kulturen, Arbeitsplätze oder Umweltstandards darunter leiden", "Kritiker befürchten zu wenig internationalen Handel", "Es gibt an der Globalisierung überhaupt keine Kritikpunkte" }, "Sie befürchten, dass lokale Kulturen, Arbeitsplätze oder Umweltstandards darunter leiden",
            "Kritikerinnen und Kritiker sorgen sich z.B. um den Verlust lokaler Arbeitsplätze, Kulturen oder um niedrigere Umweltstandards in manchen Ländern."),
        ("Was bedeutet \"Interdependenz\" zwischen Ländern in einer globalisierten Welt, vereinfacht?", new[] { "Länder sind wirtschaftlich und politisch stark voneinander abhängig", "Länder sind vollkommen unabhängig voneinander", "Ein anderes Wort für Kriegszustand zwischen Ländern" }, "Länder sind wirtschaftlich und politisch stark voneinander abhängig",
            "Interdependenz bedeutet gegenseitige Abhängigkeit - Entscheidungen und Ereignisse in einem Land wirken sich oft auf andere Länder aus."),
        ("Was kann passieren, wenn ein wichtiger Rohstoff in einem Land knapp wird, das viele andere Länder beliefert?", new[] { "Die Preise können weltweit steigen und andere Länder sind betroffen", "Andere Länder bemerken davon überhaupt nichts", "Der Rohstoff wird dadurch automatisch billiger" }, "Die Preise können weltweit steigen und andere Länder sind betroffen",
            "In einer vernetzten Weltwirtschaft wirken sich Engpässe in einem Land oft auf Preise und Versorgung in vielen anderen Ländern aus."),
        ("Warum ist internationale Zusammenarbeit bei globalen Problemen wie dem Klimawandel wichtig?", new[] { "Weil solche Probleme nicht von einem einzelnen Land allein gelöst werden können", "Weil der Klimawandel nur ein einziges Land betrifft", "Internationale Zusammenarbeit ist bei globalen Problemen unnötig" }, "Weil solche Probleme nicht von einem einzelnen Land allein gelöst werden können",
            "Globale Herausforderungen wie der Klimawandel erfordern gemeinsames Handeln vieler Länder, da sie Ländergrenzen überschreiten."),
        ("Was ist ein Beispiel für eine internationale Organisation, die die Zusammenarbeit zwischen Staaten fördert?", new[] { "Die Vereinten Nationen (UN)", "Ein einzelner privater Handwerksbetrieb", "Ein regionaler Sportverein" }, "Die Vereinten Nationen (UN)",
            "Die Vereinten Nationen sind eine zentrale internationale Organisation, die Zusammenarbeit zwischen fast allen Staaten der Welt fördert."),
        ("Was versteht man unter kultureller Globalisierung, z.B. bei Musik oder Filmen?", new[] { "Kulturelle Trends und Produkte verbreiten sich heute sehr schnell über Ländergrenzen hinweg", "Kultur bleibt in einer globalisierten Welt streng auf ein einzelnes Land beschränkt", "Kulturelle Globalisierung betrifft nur die Landwirtschaft" }, "Kulturelle Trends und Produkte verbreiten sich heute sehr schnell über Ländergrenzen hinweg",
            "Musik, Filme und andere kulturelle Trends verbreiten sich dank globaler Vernetzung heute besonders schnell weltweit."),
        ("Warum kann Globalisierung sowohl reiche als auch arme Länder wirtschaftlich betreffen?", new[] { "Wirtschaftliche Entwicklungen und Krisen wirken sich heute oft weltweit aus, nicht nur lokal", "Wirtschaftliche Ereignisse bleiben immer streng auf ein einziges Land begrenzt", "Nur reiche Länder sind von wirtschaftlichen Ereignissen betroffen" }, "Wirtschaftliche Entwicklungen und Krisen wirken sich heute oft weltweit aus, nicht nur lokal",
            "Durch enge wirtschaftliche Verflechtung können sich Krisen oder Entwicklungen in einem Land weltweit auf andere Länder auswirken."),
        ("Was ist ein Vorteil davon, dass Menschen weltweit heute leichter miteinander kommunizieren können?", new[] { "Wissen, Ideen und Kulturen können sich schneller austauschen", "Kommunikation über Ländergrenzen hinweg ist grundsätzlich verboten", "Es gibt dadurch keinerlei Vorteile" }, "Wissen, Ideen und Kulturen können sich schneller austauschen",
            "Schnellere und einfachere Kommunikation ermöglicht einen regen Austausch von Wissen, Ideen und kulturellen Einflüssen weltweit."),
        ("Was können Verbraucherinnen und Verbraucher tun, um verantwortungsvoller in einer globalisierten Welt einzukaufen?", new[] { "Zum Beispiel auf faire und nachhaltige Produktion achten", "Es gibt für Verbraucher keinerlei Einflussmöglichkeiten", "Ausschließlich die günstigsten Produkte kaufen, egal woher" }, "Zum Beispiel auf faire und nachhaltige Produktion achten",
            "Durch bewussten Konsum, z.B. mit Fairtrade- oder Umweltsiegeln, können Verbraucherinnen und Verbraucher globale Produktionsbedingungen mitbeeinflussen."),
        ("Warum wird die Globalisierung oft mit steigendem Warenverkehr per Schiff, Flugzeug und LKW in Verbindung gebracht?", new[] { "Weil viele Produkte heute über weite Strecken zwischen Produktions- und Verkaufsländern transportiert werden", "Weil Warentransport in einer globalisierten Welt komplett verboten ist", "Weil alle Waren heute ausschließlich lokal produziert und verkauft werden" }, "Weil viele Produkte heute über weite Strecken zwischen Produktions- und Verkaufsländern transportiert werden",
            "Globaler Handel bedeutet, dass Waren oft weite Strecken zwischen Produktions- und Verkaufsländern zurücklegen müssen.")
    };

    private static QuizQuestion GlobalisierteWelt(Random r)
    {
        var f = GlobalisierteWeltListe[r.Next(GlobalisierteWeltListe.Length)];
        return new QuizQuestion
        {
            Id = NewId(), Subject = Subject.Politik, GradeLevel = GradeLevel.Klasse6,
            Topic = "Leben in einer globalisierten Welt", Type = QuestionType.MultipleChoice,
            Prompt = f.Frage, Options = f.Optionen, CorrectAnswers = new[] { f.Antwort }, Explanation = f.Erklaerung,
            HelpHint = "Globalisierung vernetzt Länder wirtschaftlich, kulturell und über Kommunikation - das bringt Vorteile (Austausch, Angebot), aber auch Herausforderungen (Arbeitsbedingungen, Umwelt, Interdependenz)."
        };
    }

    private static readonly (string Frage, string[] Optionen, string Antwort, string Erklaerung)[] MigrationPolitikListe =
    {
        ("Was garantiert das Grundgesetz Menschen, die in Deutschland politisch verfolgt werden?", new[] { "Das Recht auf Asyl", "Das Recht auf ein kostenloses Auto", "Das Recht, sofort die deutsche Staatsbürgerschaft zu erhalten" }, "Das Recht auf Asyl",
            "Das Grundgesetz garantiert politisch Verfolgten grundsätzlich das Recht, in Deutschland Asyl zu beantragen."),
        ("Was ist die Staatsbürgerschaft (Staatsangehörigkeit)?", new[] { "Die rechtliche Zugehörigkeit einer Person zu einem bestimmten Staat mit bestimmten Rechten und Pflichten", "Ein anderes Wort für Wohnort", "Ein Reisedokument für den Urlaub" }, "Die rechtliche Zugehörigkeit einer Person zu einem bestimmten Staat mit bestimmten Rechten und Pflichten",
            "Die Staatsbürgerschaft regelt rechtlich, zu welchem Staat eine Person gehört, inklusive Rechten wie dem Wahlrecht und Pflichten."),
        ("Wie kann man z.B. die deutsche Staatsbürgerschaft durch Einbürgerung erhalten?", new[] { "Durch Erfüllung bestimmter Voraussetzungen wie Aufenthaltsdauer, Sprachkenntnisse und eigenständige Lebensunterhaltssicherung", "Automatisch nach einem einzigen Tag in Deutschland", "Durch reinen Zufall per Losverfahren" }, "Durch Erfüllung bestimmter Voraussetzungen wie Aufenthaltsdauer, Sprachkenntnisse und eigenständige Lebensunterhaltssicherung",
            "Einbürgerung setzt in der Regel u.a. eine bestimmte Aufenthaltsdauer, ausreichende Sprachkenntnisse und eigenständige Existenzsicherung voraus."),
        ("Was bedeutet \"doppelte Staatsbürgerschaft\"?", new[] { "Eine Person besitzt gleichzeitig die Staatsangehörigkeit von zwei Ländern", "Eine Person hat gar keine Staatsangehörigkeit", "Ein Land hat zwei verschiedene Regierungen" }, "Eine Person besitzt gleichzeitig die Staatsangehörigkeit von zwei Ländern",
            "Bei doppelter Staatsbürgerschaft gehört eine Person rechtlich gleichzeitig zwei verschiedenen Staaten an."),
        ("Was ist ein Integrationskurs?", new[] { "Ein staatlich gefördertes Angebot, das Zugewanderten Sprache und Grundwissen über Deutschland vermittelt", "Ein Kurs, der ausschließlich Sport unterrichtet", "Ein Kurs, den nur deutsche Staatsbürger besuchen dürfen" }, "Ein staatlich gefördertes Angebot, das Zugewanderten Sprache und Grundwissen über Deutschland vermittelt",
            "Integrationskurse vermitteln Zugewanderten Deutschkenntnisse sowie Grundwissen über Gesellschaft, Kultur und Rechtsordnung."),
        ("Wer entscheidet in Deutschland letztlich über die Migrationspolitik?", new[] { "Die gewählten politischen Vertreterinnen und Vertreter in Bundestag und Regierung", "Ausschließlich internationale Konzerne", "Migrationspolitik wird per Losentscheid festgelegt" }, "Die gewählten politischen Vertreterinnen und Vertreter in Bundestag und Regierung",
            "Wie in anderen Politikbereichen entscheiden die demokratisch gewählten Vertretungen über die Ausgestaltung der Migrationspolitik."),
        ("Was ist ein politischer Streitpunkt bei der Migrationspolitik, über den Parteien oft unterschiedlich diskutieren?", new[] { "Zum Beispiel wie viele Menschen aufgenommen werden sollen oder wie Integration gestaltet wird", "Ob Deutschland ein Land ist oder nicht", "Ob die deutsche Sprache existiert" }, "Zum Beispiel wie viele Menschen aufgenommen werden sollen oder wie Integration gestaltet wird",
            "Fragen zur Zahl der Aufnahmen, zur Ausgestaltung der Integration oder zur Verteilung sind typische politische Streitpunkte."),
        ("Was bedeutet \"kommunale Integrationsarbeit\"?", new[] { "Maßnahmen von Städten und Gemeinden, um Zugewanderten das Ankommen vor Ort zu erleichtern", "Ein Verbot für Städte, sich um Migrationsthemen zu kümmern", "Ausschließlich internationale Verträge zwischen Staaten" }, "Maßnahmen von Städten und Gemeinden, um Zugewanderten das Ankommen vor Ort zu erleichtern",
            "Städte und Gemeinden unterstützen mit eigenen Angeboten - z.B. Beratung oder Sprachkursen - die Integration vor Ort."),
        ("Was ist ein Ausländerbeirat bzw. Migrationsrat in manchen Kommunen?", new[] { "Ein Gremium, das die Interessen von Migrantinnen und Migranten in der Kommunalpolitik vertritt", "Eine Behörde, die ausschließlich Reisepässe ausstellt", "Ein privater Verein ohne politische Funktion" }, "Ein Gremium, das die Interessen von Migrantinnen und Migranten in der Kommunalpolitik vertritt",
            "Solche Beiräte sollen die Perspektiven von Migrantinnen und Migranten in lokale politische Entscheidungen einbringen."),
        ("Dürfen alle in Deutschland lebenden Menschen bei Bundestagswahlen wählen?", new[] { "Nein, nur Personen mit deutscher Staatsbürgerschaft", "Ja, ausnahmslos jede Person, die in Deutschland wohnt", "Nein, nur Personen über 30 Jahre" }, "Nein, nur Personen mit deutscher Staatsbürgerschaft",
            "Das Wahlrecht bei Bundestagswahlen ist an die deutsche Staatsbürgerschaft geknüpft, unabhängig von der Wohndauer in Deutschland."),
        ("Warum ist die politische Teilhabe von Migrantinnen und Migranten ein wichtiges gesellschaftliches Thema?", new[] { "Damit auch ihre Interessen in politischen Entscheidungen berücksichtigt werden", "Weil Migranten grundsätzlich kein Interesse an Politik haben", "Politische Teilhabe betrifft ausschließlich Staatsbürgerinnen ohne Migrationsgeschichte" }, "Damit auch ihre Interessen in politischen Entscheidungen berücksichtigt werden",
            "Politische Teilhabe stellt sicher, dass auch die Anliegen von Menschen mit Migrationsgeschichte gehört und berücksichtigt werden."),
        ("Was bedeutet \"Diskriminierung\" im Zusammenhang mit Migration?", new[] { "Die ungerechte Benachteiligung von Menschen aufgrund ihrer Herkunft", "Eine besondere Förderung von Migrantinnen und Migranten", "Ein anderes Wort für Einbürgerung" }, "Die ungerechte Benachteiligung von Menschen aufgrund ihrer Herkunft",
            "Diskriminierung bedeutet, Menschen ungerecht zu behandeln, z.B. wegen ihrer Herkunft, statt sie nach ihren Fähigkeiten zu beurteilen."),
        ("Was tut das Allgemeine Gleichbehandlungsgesetz (AGG) in Deutschland?", new[] { "Es schützt Menschen u.a. vor Diskriminierung wegen ihrer Herkunft", "Es erlaubt gezielt die Benachteiligung bestimmter Gruppen", "Es regelt ausschließlich Verkehrsregeln" }, "Es schützt Menschen u.a. vor Diskriminierung wegen ihrer Herkunft",
            "Das AGG soll Benachteiligungen z.B. aufgrund von Herkunft, Geschlecht oder Religion verhindern."),
        ("Was versteht man unter \"Multikulturalismus\" als politisches Konzept?", new[] { "Das Zusammenleben verschiedener Kulturen in einer Gesellschaft mit gegenseitigem Respekt", "Die vollständige Abschaffung aller kulturellen Unterschiede", "Ein anderes Wort für Einwanderungsverbot" }, "Das Zusammenleben verschiedener Kulturen in einer Gesellschaft mit gegenseitigem Respekt",
            "Multikulturalismus beschreibt das Ideal eines respektvollen Zusammenlebens unterschiedlicher kultureller Gruppen in einer Gesellschaft."),
        ("Warum ist Migration in Deutschland ein zentrales Thema in Wahlkämpfen?", new[] { "Weil sie viele Bereiche wie Arbeitsmarkt, Bildung und Wohnungspolitik betrifft", "Migration hat mit Wahlkämpfen überhaupt nichts zu tun", "Weil es in Deutschland keine Migration gibt" }, "Weil sie viele Bereiche wie Arbeitsmarkt, Bildung und Wohnungspolitik betrifft",
            "Migration wirkt sich auf viele Politikbereiche aus, weshalb sie häufig ein zentrales Wahlkampfthema ist."),
        ("Was ist eine Duldung im deutschen Aufenthaltsrecht?", new[] { "Eine vorübergehende Aussetzung der Abschiebung ohne dauerhaftes Aufenthaltsrecht", "Ein dauerhaftes, unbefristetes Aufenthaltsrecht", "Ein anderes Wort für Staatsbürgerschaft" }, "Eine vorübergehende Aussetzung der Abschiebung ohne dauerhaftes Aufenthaltsrecht",
            "Eine Duldung bedeutet, dass eine Abschiebung vorübergehend ausgesetzt wird, ohne dass ein gesichertes Aufenthaltsrecht besteht."),
        ("Was macht das Bundesamt für Migration und Flüchtlinge (BAMF)?", new[] { "Es bearbeitet u.a. Asylanträge in Deutschland", "Es organisiert ausschließlich Auslandsreisen für Bürger", "Es ist für den Straßenverkehr zuständig" }, "Es bearbeitet u.a. Asylanträge in Deutschland",
            "Das BAMF ist die zentrale deutsche Behörde, die u.a. Asylanträge prüft und über Asylverfahren entscheidet."),
        ("Was ist ein politisches Argument für eine geregelte Zuwanderung von Fachkräften nach Deutschland?", new[] { "Deutschland braucht in vielen Branchen zusätzliche Arbeitskräfte", "Deutschland hat in allen Branchen bereits zu viele Arbeitskräfte", "Fachkräftezuwanderung ist in Deutschland gesetzlich verboten" }, "Deutschland braucht in vielen Branchen zusätzliche Arbeitskräfte",
            "In vielen Berufsfeldern fehlen in Deutschland Fachkräfte, weshalb geregelte Zuwanderung politisch oft befürwortet wird."),
        ("Warum ist die Verteilung von Geflüchteten auf verschiedene Bundesländer politisch geregelt?", new[] { "Damit die Aufnahme und Versorgung gerecht auf ganz Deutschland verteilt wird", "Damit ausschließlich ein einziges Bundesland zuständig ist", "Diese Verteilung ist gesetzlich nicht geregelt" }, "Damit die Aufnahme und Versorgung gerecht auf ganz Deutschland verteilt wird",
            "Ein festgelegter Verteilungsschlüssel sorgt dafür, dass die Aufnahme von Geflüchteten fair auf alle Bundesländer verteilt wird."),
        ("Was bedeutet \"gesellschaftlicher Zusammenhalt\" im Zusammenhang mit Zuwanderung?", new[] { "Das friedliche und respektvolle Zusammenleben unterschiedlicher Bevölkerungsgruppen", "Die vollständige Trennung verschiedener Bevölkerungsgruppen", "Ein anderes Wort für Grenzkontrollen" }, "Das friedliche und respektvolle Zusammenleben unterschiedlicher Bevölkerungsgruppen",
            "Gesellschaftlicher Zusammenhalt beschreibt das Ziel, dass Menschen unterschiedlicher Herkunft friedlich und respektvoll zusammenleben.")
    };

    private static QuizQuestion MigrationPolitik(Random r)
    {
        var f = MigrationPolitikListe[r.Next(MigrationPolitikListe.Length)];
        return new QuizQuestion
        {
            Id = NewId(), Subject = Subject.Politik, GradeLevel = GradeLevel.Klasse6,
            Topic = "Migration und Bevölkerung", Type = QuestionType.MultipleChoice,
            Prompt = f.Frage, Options = f.Optionen, CorrectAnswers = new[] { f.Antwort }, Explanation = f.Erklaerung,
            HelpHint = "Grundgesetz garantiert Asylrecht; Staatsbürgerschaft, Einbürgerung, Integration und politische Teilhabe (nur mit deutscher Staatsbürgerschaft wahlberechtigt) sind zentrale politische Begriffe rund um Migration."
        };
    }

    private static readonly (string Frage, string[] Optionen, string Antwort, string Erklaerung)[] RechtsstaatListe =
    {
        ("Was bedeutet \"Rechtsstaat\" einfach erklärt?", new[] { "Ein Staat, in dem sich alle - auch die Regierung - an geltende Gesetze halten müssen", "Ein Staat ohne jegliche Gesetze", "Ein Staat, in dem nur die Regierung Gesetze befolgen muss" }, "Ein Staat, in dem sich alle - auch die Regierung - an geltende Gesetze halten müssen",
            "Im Rechtsstaat gelten Gesetze für alle gleichermaßen - auch für die Regierung selbst, die sich nicht darüber hinwegsetzen darf."),
        ("Warum sind Klassenregeln in der Schule ein kleines Beispiel für Rechtsstaatlichkeit?", new[] { "Sie gelten für alle gleichermaßen und sichern ein faires Miteinander", "Sie gelten nur für einzelne, ausgewählte Schülerinnen und Schüler", "Klassenregeln haben mit Rechtsstaatlichkeit nichts zu tun" }, "Sie gelten für alle gleichermaßen und sichern ein faires Miteinander",
            "Wie Gesetze im Staat gelten Klassenregeln für alle gleichermaßen und sorgen für ein geordnetes, faires Miteinander."),
        ("Was sind Kinderrechte?", new[] { "Grundlegende Rechte, die speziell den Schutz und die Bedürfnisse von Kindern sichern", "Rechte, die ausschließlich Erwachsene betreffen", "Ein anderes Wort für Hausaufgaben" }, "Grundlegende Rechte, die speziell den Schutz und die Bedürfnisse von Kindern sichern",
            "Kinderrechte sichern besonders die Bedürfnisse von Kindern ab, z.B. Schutz, Bildung und Mitbestimmung."),
        ("In welchem internationalen Dokument sind die Kinderrechte weltweit festgelegt?", new[] { "In der UN-Kinderrechtskonvention", "Im Grundgesetz eines einzelnen Landes", "In den Olympischen Statuten" }, "In der UN-Kinderrechtskonvention",
            "Die UN-Kinderrechtskonvention legt weltweit gültige Grundrechte für Kinder fest, die fast alle Staaten unterzeichnet haben."),
        ("Was ist ein Beispiel für ein wichtiges Kinderrecht?", new[] { "Das Recht auf Bildung, Schutz vor Gewalt oder das Recht auf Mitbestimmung", "Das Recht, keine Schule besuchen zu müssen", "Das Recht, uneingeschränkt jeden Film zu sehen" }, "Das Recht auf Bildung, Schutz vor Gewalt oder das Recht auf Mitbestimmung",
            "Zu den zentralen Kinderrechten gehören u.a. das Recht auf Bildung, auf Schutz vor Gewalt und auf Mitbestimmung bei sie betreffenden Fragen."),
        ("Was regelt der Jugendschutz in Deutschland z.B. bei Filmen und Computerspielen?", new[] { "Ab welchem Alter bestimmte Inhalte für Kinder und Jugendliche freigegeben sind", "Welche Filme grundsätzlich verboten sind, egal für welches Alter", "Wie viel ein Kinoticket kosten darf" }, "Ab welchem Alter bestimmte Inhalte für Kinder und Jugendliche freigegeben sind",
            "Altersfreigaben (z.B. von der USK) legen fest, ab welchem Alter bestimmte Filme oder Spiele für Kinder und Jugendliche geeignet sind."),
        ("Warum gibt es Altersbeschränkungen beim Kauf von Alkohol oder Zigaretten?", new[] { "Um Kinder und Jugendliche vor gesundheitsschädlichen Produkten zu schützen", "Weil diese Produkte für Erwachsene verboten sind", "Altersbeschränkungen gibt es dafür in Deutschland nicht" }, "Um Kinder und Jugendliche vor gesundheitsschädlichen Produkten zu schützen",
            "Jugendschutzregeln sollen verhindern, dass Kinder und Jugendliche zu früh Zugang zu gesundheitsschädlichen Produkten wie Alkohol erhalten."),
        ("Was ist das Jugendschutzgesetz?", new[] { "Ein Gesetz, das Kinder und Jugendliche z.B. vor bestimmten Medieninhalten, Alkohol oder späten Ausgehzeiten schützt", "Ein Gesetz, das ausschließlich Erwachsene betrifft", "Ein Gesetz zur Regelung von Straßenverkehr" }, "Ein Gesetz, das Kinder und Jugendliche z.B. vor bestimmten Medieninhalten, Alkohol oder späten Ausgehzeiten schützt",
            "Das Jugendschutzgesetz regelt u.a. Altersgrenzen für Alkohol, Ausgehzeiten und den Zugang zu bestimmten Medieninhalten."),
        ("Was bedeutet \"Gewaltenteilung\" als Grundprinzip eines Rechtsstaats, ganz einfach?", new[] { "Gesetzgebung, Regierung und Gerichte sind voneinander getrennt und kontrollieren sich gegenseitig", "Eine einzige Person entscheidet über alle Staatsgewalten", "Gewaltenteilung bedeutet, dass es keine Gerichte gibt" }, "Gesetzgebung, Regierung und Gerichte sind voneinander getrennt und kontrollieren sich gegenseitig",
            "Durch die Trennung von Legislative, Exekutive und Judikative kontrollieren sich die Staatsgewalten gegenseitig und verhindern Machtmissbrauch."),
        ("Warum darf niemand - auch nicht die Regierung - über dem Gesetz stehen?", new[] { "Damit Macht nicht missbraucht werden kann und alle gleich behandelt werden", "Damit die Regierung machen kann, was sie möchte", "Diese Regel gilt nur für einfache Bürgerinnen und Bürger" }, "Damit Macht nicht missbraucht werden kann und alle gleich behandelt werden",
            "Gilt das Gesetz für alle gleichermaßen, wird Machtmissbrauch verhindert und Gleichbehandlung gesichert."),
        ("Was passiert, wenn jemand gegen ein Gesetz verstößt, in einem Rechtsstaat?", new[] { "Ein unabhängiges Gericht entscheidet über eine mögliche Strafe", "Die Regierung entscheidet allein und ohne Gericht über die Strafe", "Es passiert grundsätzlich gar nichts" }, "Ein unabhängiges Gericht entscheidet über eine mögliche Strafe",
            "In einem Rechtsstaat entscheiden unabhängige Gerichte über Schuld und Strafe, nicht die Regierung selbst."),
        ("Was bedeutet \"Unschuldsvermutung\"?", new[] { "Eine Person gilt so lange als unschuldig, bis ihre Schuld gerichtlich bewiesen ist", "Jede angeklagte Person gilt automatisch als schuldig", "Ein anderes Wort für Gewaltenteilung" }, "Eine Person gilt so lange als unschuldig, bis ihre Schuld gerichtlich bewiesen ist",
            "Die Unschuldsvermutung schützt Angeklagte davor, vorschnell als schuldig behandelt zu werden, bevor ein Gericht entschieden hat."),
        ("Warum haben auch Kinder das Recht, bei sie betreffenden Entscheidungen gehört zu werden?", new[] { "Das ist ein Grundprinzip der Kinderrechte (Recht auf Mitbestimmung/Beteiligung)", "Kinder haben grundsätzlich kein Recht auf Mitsprache", "Nur Erwachsene dürfen über Kinder betreffende Fragen entscheiden" }, "Das ist ein Grundprinzip der Kinderrechte (Recht auf Mitbestimmung/Beteiligung)",
            "Die UN-Kinderrechtskonvention sieht vor, dass die Meinung von Kindern bei sie betreffenden Entscheidungen berücksichtigt werden soll."),
        ("Was ist eine Kinderbeauftragte bzw. ein Kinderbeauftragter?", new[] { "Eine Person oder Stelle, die sich besonders für die Rechte und Interessen von Kindern einsetzt", "Ein anderes Wort für Schulleitung", "Eine Person, die ausschließlich Prüfungen abnimmt" }, "Eine Person oder Stelle, die sich besonders für die Rechte und Interessen von Kindern einsetzt",
            "Kinderbeauftragte setzen sich politisch und gesellschaftlich gezielt für die Wahrung der Kinderrechte ein."),
        ("Was bedeutet \"Schulpflicht\" in Deutschland?", new[] { "Alle Kinder müssen für eine bestimmte Zeit verpflichtend eine Schule besuchen", "Schule ist in Deutschland komplett freiwillig", "Nur Kinder aus bestimmten Familien müssen zur Schule gehen" }, "Alle Kinder müssen für eine bestimmte Zeit verpflichtend eine Schule besuchen",
            "Die Schulpflicht verpflichtet alle Kinder in Deutschland, für eine gesetzlich festgelegte Zeit die Schule zu besuchen."),
        ("Warum ist die Schulpflicht auch eine Umsetzung des Rechts auf Bildung?", new[] { "Sie stellt sicher, dass wirklich alle Kinder Zugang zu Bildung bekommen", "Sie verhindert, dass Kinder überhaupt lernen dürfen", "Schulpflicht hat mit dem Recht auf Bildung nichts zu tun" }, "Sie stellt sicher, dass wirklich alle Kinder Zugang zu Bildung bekommen",
            "Indem die Schulpflicht für alle Kinder gilt, wird das Recht auf Bildung praktisch für jedes Kind umgesetzt."),
        ("Was ist Kinderarbeit und warum ist sie in Deutschland weitgehend verboten?", new[] { "Arbeit von Kindern unter dem gesetzlichen Mindestalter - verboten, um Kindheit, Gesundheit und Bildung zu schützen", "Ein anderes Wort für Hausaufgaben", "Kinderarbeit ist in Deutschland ausdrücklich erlaubt und gefördert" }, "Arbeit von Kindern unter dem gesetzlichen Mindestalter - verboten, um Kindheit, Gesundheit und Bildung zu schützen",
            "Das Verbot von Kinderarbeit soll sicherstellen, dass Kinder ihre Kindheit, Gesundheit und schulische Bildung nicht durch Erwerbsarbeit verlieren."),
        ("Was können Kinder und Jugendliche tun, wenn ihre Rechte verletzt werden?", new[] { "Sich an Vertrauenspersonen, Beratungsstellen oder Kinderschutzorganisationen wenden", "Es gibt für Kinder keinerlei Möglichkeit, sich zu wehren", "Nur die Polizei darf in solchen Fällen kontaktiert werden" }, "Sich an Vertrauenspersonen, Beratungsstellen oder Kinderschutzorganisationen wenden",
            "Bei Rechtsverletzungen können sich Kinder an Eltern, Lehrkräfte, Beratungsstellen oder spezielle Kinderschutzorganisationen wenden."),
        ("Was ist eine Klassensprecherin bzw. ein Klassensprecher ein Beispiel für?", new[] { "Ein kleines Beispiel für demokratische Mitbestimmung von Kindern", "Ein Beispiel für eine Diktatur in der Klasse", "Eine rein zufällige, bedeutungslose Position" }, "Ein kleines Beispiel für demokratische Mitbestimmung von Kindern",
            "Die Wahl einer Klassensprecherin oder eines Klassensprechers ist ein praktisches, kleines Beispiel für demokratische Mitbestimmung."),
        ("Warum gelten Gesetze in einem Rechtsstaat für alle Menschen gleichermaßen?", new[] { "Damit niemand wegen seiner Herkunft, seines Status oder seiner Macht bevorzugt oder benachteiligt wird", "Damit mächtige Personen bevorzugt behandelt werden können", "Gesetze gelten in einem Rechtsstaat nur für einfache Bürgerinnen und Bürger" }, "Damit niemand wegen seiner Herkunft, seines Status oder seiner Macht bevorzugt oder benachteiligt wird",
            "Die Gleichheit vor dem Gesetz ist ein Kernprinzip des Rechtsstaats und verhindert Bevorzugung oder Benachteiligung Einzelner.")
    };

    private static QuizQuestion LebenImRechtsstaat(Random r)
    {
        var f = RechtsstaatListe[r.Next(RechtsstaatListe.Length)];
        return new QuizQuestion
        {
            Id = NewId(), Subject = Subject.Politik, GradeLevel = GradeLevel.Klasse6,
            Topic = "Leben in einem Rechtsstaat (Klassenregeln, Jugendschutz, Kinderrechte)", Type = QuestionType.MultipleChoice,
            Prompt = f.Frage, Options = f.Optionen, CorrectAnswers = new[] { f.Antwort }, Explanation = f.Erklaerung,
            HelpHint = "Im Rechtsstaat gelten Gesetze für alle gleich, auch für die Regierung (Gewaltenteilung); Kinderrechte (UN-Konvention) und Jugendschutz sichern speziell die Bedürfnisse von Kindern ab."
        };
    }
}
