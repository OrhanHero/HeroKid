using LernTor.Core.Enums;
using LernTor.Core.Models;

namespace LernTor.Core.Services;

public static partial class DrivingTheoryCatalog
{
    /// <summary>
    /// Sachgebiete 8-14: ruhender Verkehr, andere Verkehrsteilnehmer, besondere Situationen,
    /// Unfall und Panne, Umwelt, Fahrzeugtechnik, Beförderung und Anhänger.
    /// </summary>
    private static readonly TheoryQuestion[] Verhalten =
    {
        // ---------------------------------------------- Ruhender Verkehr
        new()
        {
            Id = "rv-01", Topic = DrivingTheoryTopic.RuhenderVerkehr, Points = 3,
            SignNumber = "286",
            Prompt = "Was ist beim eingeschränkten Haltverbot (VZ 286) erlaubt?",
            Options = new[]
            {
                "Halten bis zu drei Minuten zum Ein- und Aussteigen oder Beladen",
                "Überhaupt nichts - hier ist weder Halten noch Parken gestattet",
                "Parken bis zu einer Stunde, danach muss umgeparkt werden",
                "Parken für beliebige Zeit, sofern eine Parkscheibe ausliegt"
            },
            CorrectIndices = new[] { 0 },
            Explanation = "Ein Strich = ein Verbot, nämlich Parken. Kurzes Halten zum Ein- und Aussteigen bleibt erlaubt. Beim absoluten Haltverbot (zwei Striche, VZ 283) ist auch das verboten."
        },
        new()
        {
            Id = "rv-02", Topic = DrivingTheoryTopic.RuhenderVerkehr, Points = 3,
            Prompt = "Wo darfst du nicht parken? (Mehrere Antworten richtig)",
            Options = new[]
            {
                "Vor und hinter Kreuzungen im Bereich von 5 Metern",
                "Vor Grundstückseinfahrten",
                "Auf Fahrradschutzstreifen",
                "In Einbahnstraßen auf der linken Seite"
            },
            CorrectIndices = new[] { 0, 1, 2 },
            Explanation = "In Einbahnstraßen ist links parken ausdrücklich erlaubt - das ist die Ausnahme, die viele nicht kennen. Die anderen drei sind klassische Parkverstöße."
        },
        new()
        {
            Id = "rv-03", Topic = DrivingTheoryTopic.RuhenderVerkehr, Points = 2,
            Prompt = "Was ist der Unterschied zwischen Halten und Parken?",
            Options = new[]
            {
                "Parken ist, wer länger als drei Minuten steht oder sein Fahrzeug verlässt",
                "Halten ist nur mit laufendem Motor und angeschaltetem Warnblinker erlaubt",
                "Zwischen Halten und Parken gibt es rechtlich keinerlei Unterschied",
                "Parken ist ausschließlich auf gekennzeichneten Parkplätzen möglich"
            },
            CorrectIndices = new[] { 0 },
            Explanation = "Drei Minuten oder Fahrzeug verlassen - dann ist es Parken. Wer aussteigt, um kurz etwas abzugeben, parkt also, auch wenn es schnell geht."
        },

        // ---------------------------------------------- Andere Verkehrsteilnehmer
        new()
        {
            Id = "av-01", Topic = DrivingTheoryTopic.AndereVerkehrsteilnehmer, Points = 5,
            SignNumber = "136-10",
            Prompt = "Du siehst am Straßenrand spielende Kinder. Wie verhältst du dich?",
            Options = new[]
            {
                "Geschwindigkeit deutlich verringern und bremsbereit sein",
                "Hupen und normal weiterfahren",
                "Nur ausweichen, wenn eines auf die Straße läuft",
                "Beschleunigen, um schnell vorbei zu sein"
            },
            CorrectIndices = new[] { 0 },
            Explanation = "Kinder können Entfernung und Geschwindigkeit nicht zuverlässig einschätzen und rennen ohne zu schauen los. Hier hilft nur, vorher langsam zu sein - reagieren kommt zu spät."
        },
        new()
        {
            Id = "av-02", Topic = DrivingTheoryTopic.AndereVerkehrsteilnehmer, Points = 5,
            Prompt = "Ein LKW biegt vor dir nach rechts ab. Worauf musst du achten?",
            Options = new[]
            {
                "Er schwenkt hinten aus und hat einen toten Winkel - nicht rechts daneben halten",
                "Er fährt langsamer, deshalb kannst du gefahrlos rechts an ihm vorbeifahren",
                "Nichts Besonderes - LKW haben große Spiegel und sehen alles neben sich",
                "Du hast Vorrang, weil du geradeaus fährst und er abbiegen will"
            },
            CorrectIndices = new[] { 0 },
            Explanation = "Der Abbiege-Unfall zwischen LKW und Radfahrenden ist einer der tödlichsten überhaupt. Neben einem abbiegenden LKW zu stehen ist die gefährlichste Position im Straßenverkehr."
        },
        new()
        {
            Id = "av-03", Topic = DrivingTheoryTopic.AndereVerkehrsteilnehmer, Points = 4,
            Prompt = "Ein Linienbus will von der Haltestelle abfahren und blinkt. Was gilt?",
            Options = new[]
            {
                "Du musst ihm das Abfahren ermöglichen, notfalls warten",
                "Du hast Vorfahrt, weil du auf der Fahrbahn bist",
                "Nur wenn er noch nicht angefahren ist",
                "Du musst hupen, damit er wartet"
            },
            CorrectIndices = new[] { 0 },
            Explanation = "Bussen an Haltestellen ist das Abfahren zu ermöglichen. Und solange ein Bus mit Warnblinker an der Haltestelle steht, darf nur Schrittgeschwindigkeit gefahren werden."
        },
        new()
        {
            Id = "av-04", Topic = DrivingTheoryTopic.AndereVerkehrsteilnehmer, Points = 4,
            Prompt = "Warum sind ältere Menschen im Straßenverkehr besonders gefährdet?",
            Options = new[]
            {
                "Sie brauchen länger zum Überqueren und hören oder sehen oft schlechter",
                "Sie halten sich seltener an die Verkehrsregeln als jüngere Menschen",
                "Sie sind grundsätzlich unaufmerksam und schwer einzuschätzen",
                "Sie sind im Straßenverkehr nicht stärker gefährdet als andere"
            },
            CorrectIndices = new[] { 0 },
            Explanation = "Es geht um Tempo und Wahrnehmung, nicht um Regeltreue. Wer an einer Ampel losläuft, die kurz darauf umspringt, braucht Zeit - keine Hupe."
        },

        // ---------------------------------------------- Besondere Situationen
        new()
        {
            Id = "bs-01", Topic = DrivingTheoryTopic.BesondereSituationen, Points = 5,
            SignNumber = "201-50",
            Prompt = "Du näherst dich einem Bahnübergang mit Andreaskreuz. Was gilt?",
            Options = new[]
            {
                "Der Schienenverkehr hat Vorrang - notfalls warten",
                "Rechts vor links wie an jeder Kreuzung",
                "Du darfst fahren, solange keine Schranke unten ist",
                "Züge müssen an Kreuzungen bremsen"
            },
            CorrectIndices = new[] { 0 },
            Explanation = "Ein Zug braucht bis zum Stehen bis zu einem Kilometer und kann nicht ausweichen. Das Andreaskreuz ist das einzige Zeichen in Kreuzform - damit es unverwechselbar bleibt."
        },
        new()
        {
            Id = "bs-02", Topic = DrivingTheoryTopic.BesondereSituationen, Points = 5,
            Prompt = "Auf der Autobahn staut es sich. Wo bildest du die Rettungsgasse?",
            Options = new[]
            {
                "Zwischen dem äußersten linken und dem daneben liegenden Fahrstreifen",
                "Immer genau in der Mitte der Fahrbahn, unabhängig von der Spurzahl",
                "Auf dem Standstreifen, den alle Fahrzeuge dafür frei lassen",
                "Erst dann, wenn ein Einsatzfahrzeug tatsächlich zu hören ist"
            },
            CorrectIndices = new[] { 0 },
            Explanation = "Immer zwischen ganz links und dem Rest - unabhängig von der Fahrstreifenzahl. Und sofort beim Stocken, nicht erst beim Martinshorn: dann ist es zu spät zum Rangieren."
        },
        new()
        {
            Id = "bs-03", Topic = DrivingTheoryTopic.BesondereSituationen, Points = 4,
            Prompt = "Wann musst du in Deutschland Winterreifen fahren?",
            Options = new[]
            {
                "Bei Glatteis, Schneeglätte, Schneematsch, Eis- oder Reifglätte",
                "Immer von Oktober bis Ostern, unabhängig vom tatsächlichen Wetter",
                "Nur auf Autobahnen und autobahnähnlich ausgebauten Straßen",
                "Winterreifen sind in Deutschland grundsätzlich freiwillig"
            },
            CorrectIndices = new[] { 0 },
            Explanation = "Die Pflicht hängt an den Verhältnissen, nicht am Kalender - deshalb \"situative Winterreifenpflicht\". Die Faustregel Oktober bis Ostern ist ein Merksatz, kein Gesetz."
        },
        new()
        {
            Id = "bs-04", Topic = DrivingTheoryTopic.BesondereSituationen, Points = 4,
            Prompt = "Wann darfst du die Nebelschlussleuchte einschalten?",
            Options = new[]
            {
                "Nur bei Sichtweite unter 50 Metern",
                "Immer bei Nebel",
                "Bei Regen und Nebel",
                "Wenn es dunkel wird"
            },
            CorrectIndices = new[] { 0 },
            Explanation = "Unter 50 m Sichtweite - sonst blendet sie den Hintermann. Sie ist so hell wie eine Bremsleuchte und bei mäßigem Nebel eine Gefahr statt einer Hilfe."
        },
        new()
        {
            Id = "bs-05", Topic = DrivingTheoryTopic.BesondereSituationen, Points = 4,
            Prompt = "Es hat lange nicht geregnet und fängt jetzt an. Warum ist das besonders gefährlich?",
            Options = new[]
            {
                "Öl und Gummiabrieb auf der Fahrbahn bilden mit dem Wasser einen rutschigen Film",
                "Weil die Scheibenwischer erst nach einigen Minuten sauber wischen",
                "Weil die Reifen erst nass werden müssen, bevor sie richtig greifen",
                "Nach langer Trockenheit ist einsetzender Regen ungefährlich"
            },
            CorrectIndices = new[] { 0 },
            Explanation = "Die ersten Minuten nach langer Trockenheit sind die rutschigsten. Später wäscht der Regen die Fahrbahn frei - genau dann fühlen sich viele sicherer, obwohl es vorher gefährlicher war."
        },

        // ---------------------------------------------- Unfall und Panne
        new()
        {
            Id = "up-01", Topic = DrivingTheoryTopic.UnfallUndPanne, Points = 5,
            Prompt = "Du kommst als Erster zu einem Unfall. In welcher Reihenfolge handelst du?",
            Options = new[]
            {
                "Absichern, Notruf 112, Erste Hilfe leisten",
                "Erste Hilfe, dann absichern, dann Notruf",
                "Notruf, dann warten bis Hilfe kommt",
                "Fotos machen für die Versicherung, dann Notruf"
            },
            CorrectIndices = new[] { 0 },
            Explanation = "Erst absichern - Warnblinker, Weste, Warndreieck. Wer ungesichert hilft, wird selbst zum zweiten Unfall. Danach Notruf, dann helfen."
        },
        new()
        {
            Id = "up-02", Topic = DrivingTheoryTopic.UnfallUndPanne, Points = 4,
            Prompt = "In welcher Entfernung stellst du das Warndreieck auf?",
            Options = new[]
            {
                "Innerorts etwa 50 m, außerorts etwa 100 m, auf der Autobahn etwa 200 m",
                "Überall gleich 50 Meter, unabhängig von der Straßenart",
                "Unmittelbar hinter dem Fahrzeug, damit es sichtbar bleibt",
                "Überall gleich 100 Meter, auch innerhalb geschlossener Ortschaften"
            },
            CorrectIndices = new[] { 0 },
            Explanation = "Je schneller der Verkehr, desto weiter weg - der Hintermann braucht die Strecke zum Reagieren. Auf der Autobahn hinter Kurven und Kuppen eher noch weiter."
        },
        new()
        {
            Id = "up-03", Topic = DrivingTheoryTopic.UnfallUndPanne, Points = 4,
            Prompt = "Was gehört zur gesetzlichen Ausstattung eines PKW? (Mehrere Antworten richtig)",
            Options = new[]
            {
                "Warndreieck",
                "Verbandkasten",
                "Warnweste",
                "Feuerlöscher"
            },
            CorrectIndices = new[] { 0, 1, 2 },
            Explanation = "Dreieck, Verbandkasten und mindestens eine Warnweste sind Pflicht. Ein Feuerlöscher ist in Deutschland freiwillig - in manchen Nachbarländern nicht."
        },
        new()
        {
            Id = "up-04", Topic = DrivingTheoryTopic.UnfallUndPanne, Points = 5,
            Prompt = "Du bist an einem Unfall beteiligt, es gibt nur Blechschaden und der andere will einfach weiterfahren. Was gilt?",
            Options = new[]
            {
                "Du musst deine Personalien angeben und darfst dich nicht unerlaubt entfernen",
                "Bei reinem Blechschaden darf jeder Beteiligte den Ort einfach verlassen",
                "Ein Zettel mit der Telefonnummer an der Windschutzscheibe genügt",
                "Warten muss nur, wer den Unfall nachweislich verursacht hat"
            },
            CorrectIndices = new[] { 0 },
            Explanation = "Unerlaubtes Entfernen vom Unfallort ist eine Straftat, nicht nur eine Ordnungswidrigkeit - auch bei Blechschaden. Ein Zettel reicht nicht, man muss eine angemessene Zeit warten oder die Polizei verständigen."
        },

        // ---------------------------------------------- Umwelt und Sparsamkeit
        new()
        {
            Id = "us-01", Topic = DrivingTheoryTopic.UmweltUndSparsamkeit, Points = 3,
            Prompt = "Wie fährst du sparsam und umweltschonend? (Mehrere Antworten richtig)",
            Options = new[]
            {
                "Früh hochschalten und niedrigtourig fahren",
                "Vorausschauend fahren und Bremsen vermeiden",
                "Unnötigen Ballast und Dachträger entfernen",
                "Den Motor im Stand warmlaufen lassen"
            },
            CorrectIndices = new[] { 0, 1, 2 },
            Explanation = "Motor warmlaufen lassen ist verboten und schadet dem Motor obendrein - er wird im Stand langsamer warm als beim Fahren."
        },
        new()
        {
            Id = "us-02", Topic = DrivingTheoryTopic.UmweltUndSparsamkeit, Points = 2,
            Prompt = "Wie wirkt sich zu niedriger Reifendruck aus?",
            Options = new[]
            {
                "Höherer Verbrauch, stärkerer Verschleiß und längerer Bremsweg",
                "Nur der Fahrkomfort leidet, die Sicherheit bleibt unverändert",
                "Der Verbrauch sinkt, weil der Reifen weicher abrollt",
                "Gar nicht, solange die Reifen nicht sichtbar platt sind"
            },
            CorrectIndices = new[] { 0 },
            Explanation = "Weiche Reifen walken, verbrauchen mehr und halten schlechter. Reifendruck ist die billigste Sicherheitsmaßnahme überhaupt."
        },
        new()
        {
            Id = "us-03", Topic = DrivingTheoryTopic.UmweltUndSparsamkeit, Points = 2,
            Prompt = "Wo verbraucht ein Auto pro Kilometer am meisten?",
            Options = new[]
            {
                "Auf Kurzstrecken mit kaltem Motor",
                "Auf der Autobahn bei gleichmäßiger Fahrt",
                "Auf der Landstraße",
                "Der Verbrauch ist überall gleich"
            },
            CorrectIndices = new[] { 0 },
            Explanation = "Ein kalter Motor verbraucht ein Vielfaches. Die ersten Kilometer sind die teuersten und die schmutzigsten - für kurze Wege ist das Rad fast immer die bessere Wahl."
        },

        // ---------------------------------------------- Fahrzeugtechnik und Sicherheit
        new()
        {
            Id = "ft-01", Topic = DrivingTheoryTopic.FahrzeugtechnikUndSicherheit, Points = 5,
            Prompt = "Wie tief muss das Profil deiner Reifen mindestens sein?",
            Options = new[] { "1,6 Millimeter", "3 Millimeter", "1 Millimeter", "0,8 Millimeter" },
            CorrectIndices = new[] { 0 },
            Explanation = "1,6 mm ist das gesetzliche Minimum - Fachleute empfehlen bei Sommerreifen 3 mm, bei Winterreifen 4 mm. Bei Nässe entscheidet das Profil darüber, ob der Reifen noch greift oder aufschwimmt."
        },
        new()
        {
            Id = "ft-02", Topic = DrivingTheoryTopic.FahrzeugtechnikUndSicherheit, Points = 5,
            Prompt = "Wer muss sich im Auto anschnallen?",
            Options = new[]
            {
                "Alle Insassen, auf jedem Sitzplatz mit Gurt",
                "Nur Fahrer und Beifahrer",
                "Nur auf den Vordersitzen und der Autobahn",
                "Erwachsene dürfen selbst entscheiden"
            },
            CorrectIndices = new[] { 0 },
            Explanation = "Alle, überall. Ein ungesicherter Mitfahrer auf der Rückbank wird bei einem Aufprall zum Geschoss und kann die Person vor sich erschlagen."
        },
        new()
        {
            Id = "ft-03", Topic = DrivingTheoryTopic.FahrzeugtechnikUndSicherheit, Points = 4,
            Prompt = "Bis wann brauchen Kinder einen Kindersitz?",
            Options = new[]
            {
                "Bis 12 Jahre oder 150 cm Körpergröße - was zuerst eintritt",
                "Bis zum vollendeten 10. Lebensjahr, unabhängig von der Größe",
                "Bis zum Schuleintritt, also etwa bis zum 6. Lebensjahr",
                "Bis 140 cm Körpergröße, unabhängig vom Alter des Kindes"
            },
            CorrectIndices = new[] { 0 },
            Explanation = "Wer eine der beiden Grenzen erreicht, darf ohne Sitz fahren. Der Gurt ist für Erwachsenenkörper gebaut - bei kleineren Menschen läuft er über den Hals statt über die Schulter."
        },
        new()
        {
            Id = "ft-04", Topic = DrivingTheoryTopic.FahrzeugtechnikUndSicherheit, Points = 4,
            Prompt = "Die Bremsflüssigkeit wurde lange nicht gewechselt. Was ist die Gefahr?",
            Options = new[]
            {
                "Sie zieht Wasser, kocht bei starker Belastung und die Bremse versagt",
                "Sie wird mit der Zeit dickflüssig, dadurch packt die Bremse zu hart zu",
                "Gar nichts - Bremsflüssigkeit hält ein ganzes Autoleben lang",
                "Nur die Farbe verändert sich, die Wirkung bleibt unverändert"
            },
            CorrectIndices = new[] { 0 },
            Explanation = "Bremsflüssigkeit ist hygroskopisch - sie zieht Wasser aus der Luft. Bei einer langen Bergabfahrt kann sie dann Dampfblasen bilden, und das Pedal geht ins Leere. Deshalb alle zwei Jahre wechseln."
        },
        new()
        {
            Id = "ft-05", Topic = DrivingTheoryTopic.FahrzeugtechnikUndSicherheit, Points = 3,
            Prompt = "Was macht ABS?",
            Options = new[]
            {
                "Es verhindert das Blockieren der Räder, sodass man beim Bremsen lenken kann",
                "Es verkürzt den Bremsweg auf jedem Untergrund deutlich und zuverlässig",
                "Es bremst selbsttätig, sobald ein Hindernis vor dem Fahrzeug auftaucht",
                "Es hält den eingestellten Abstand zum vorausfahrenden Fahrzeug"
            },
            CorrectIndices = new[] { 0 },
            Explanation = "ABS erhält die Lenkfähigkeit - das ist sein Zweck, nicht der kürzere Bremsweg. Auf Schotter oder Schnee kann der Weg sogar länger werden, dafür kann man ausweichen."
        },

        // ---------------------------------------------- Beförderung und Anhänger
        new()
        {
            Id = "ba-01", Topic = DrivingTheoryTopic.BefoerderungUndAnhaenger, Points = 5,
            Prompt = "Du transportierst Getränkekisten im Kofferraum. Was gilt?",
            Options = new[]
            {
                "Die Ladung muss gegen Verrutschen gesichert sein",
                "Im Kofferraum ist Sicherung nicht nötig",
                "Es reicht, langsam zu fahren",
                "Nur schwere Ladung über 50 kg muss gesichert werden"
            },
            CorrectIndices = new[] { 0 },
            Explanation = "Bei einer Vollbremsung aus 50 km/h wiegt eine 20-kg-Kiste wie eine halbe Tonne. Ungesicherte Ladung fliegt durch die Rückenlehne in den Innenraum."
        },
        new()
        {
            Id = "ba-02", Topic = DrivingTheoryTopic.BefoerderungUndAnhaenger, Points = 3,
            Prompt = "Wie schnell darfst du außerorts mit Anhänger fahren, wenn nichts anderes ausgeschildert ist?",
            Options = new[] { "80 km/h", "100 km/h", "60 km/h", "Wie ohne Anhänger" },
            CorrectIndices = new[] { 0 },
            Explanation = "80 km/h für Gespanne, auch auf der Autobahn. Mit besonderer Zulassung (Tempo-100-Plakette) sind 100 km/h möglich, aber nur unter strengen Bedingungen."
        },
        new()
        {
            Id = "ba-03", Topic = DrivingTheoryTopic.BefoerderungUndAnhaenger, Points = 4,
            Prompt = "Was ändert sich beim Fahren mit Anhänger? (Mehrere Antworten richtig)",
            Options = new[]
            {
                "Der Bremsweg wird länger",
                "Das Gespann kann bei hohem Tempo ins Schlingern geraten",
                "Beim Rückwärtsfahren lenkt der Anhänger entgegengesetzt",
                "Die Kurven werden enger zu fahren"
            },
            CorrectIndices = new[] { 0, 1, 2 },
            Explanation = "Kurven müssen WEITER gefahren werden, nicht enger - der Anhänger schneidet die Kurve innen. Wer wie gewohnt abbiegt, nimmt den Bordstein mit."
        },
        new()
        {
            Id = "ba-04", Topic = DrivingTheoryTopic.BefoerderungUndAnhaenger, Points = 3,
            Prompt = "Wie viele Personen darfst du mitnehmen?",
            Options = new[]
            {
                "So viele, wie es zugelassene Sitzplätze mit Gurt gibt",
                "So viele, wie ins Auto passen",
                "Höchstens vier, unabhängig vom Fahrzeug",
                "Beliebig viele auf kurzen Strecken"
            },
            CorrectIndices = new[] { 0 },
            Explanation = "Die Zahl steht im Fahrzeugschein. Mehr Menschen als Sitzplätze ist nicht nur verboten, sondern bei einem Unfall lebensgefährlich für alle im Fahrzeug."
        }
    };
}
