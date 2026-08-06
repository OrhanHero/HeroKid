using LernTor.Core.Enums;
using LernTor.Core.Models;

namespace LernTor.Core.Services;

public static partial class DrivingCourseCatalog
{
    /// <summary>Die zweiten sieben Lektionen: stehen, andere, Wetter, Pannen, Technik, Ladung.</summary>
    private static readonly DrivingCourseLesson[] Praxis =
    {
        new()
        {
            Id = "kurs-parken",
            Topic = DrivingTheoryTopic.RuhenderVerkehr,
            Title = "Halten und Parken",
            Intro = "Der Unterschied zwischen Halten und Parken entscheidet, ob ein Schild für dich "
                  + "gilt - und die meisten Knöllchen entstehen daraus, dass jemand ihn nicht kannte.",
            Sections = new CourseSection[]
            {
                new("Halten, Parken, Anhalten",
                    "Anhalten ist erzwungen, etwa an der roten Ampel - dafür gilt kein Schild. Halten ist "
                  + "freiwillig und dauert bis zu drei Minuten. Wer länger stehenbleibt oder das Fahrzeug "
                  + "verlässt, parkt. Das eingeschränkte Haltverbot verbietet nur das Parken: kurz halten, "
                  + "ein- und aussteigen und be- oder entladen bleibt erlaubt.",
                    "286"),
                new("Absolutes Haltverbot",
                    "Hier ist auch das kurze Halten verboten. Zwei schräge Striche im roten Kreis - das "
                  + "eingeschränkte hat nur einen. Ein guter Merksatz: ein Strich heißt eingeschränkt, "
                  + "zwei Striche heißen gar nicht.",
                    "283"),
                new("Wo nie geparkt wird",
                    "Fünf Meter vor und hinter Kreuzungen und Einmündungen. Vor Bordsteinabsenkungen, "
                  + "denn dort steigen Rollstuhlfahrer und Kinderwagen auf die Straße. Fünfzehn Meter vor "
                  + "und hinter einem Haltestellenschild. Vor Feuerwehrzufahrten. Und niemals in zweiter "
                  + "Reihe."),
                new("Parken mit Scheibe und in der Nacht",
                    "Wo eine Parkscheibe verlangt wird, stellst du die nächste halbe Stunde ein, nicht "
                  + "die genaue Ankunftszeit. Bei Dunkelheit muss ein Auto, das nicht gut zu sehen ist, "
                  + "beleuchtet abgestellt werden - innerorts genügt sonst die Straßenbeleuchtung.",
                    "314")
            },
            KeyPoints = new[]
            {
                "Über drei Minuten oder aussteigen: das ist Parken, nicht Halten.",
                "Ein Strich = eingeschränktes Haltverbot, zwei Striche = absolutes.",
                "Fünf Meter Abstand zu Kreuzungen, fünfzehn zu Haltestellen."
            }
        },
        new()
        {
            Id = "kurs-andere",
            Topic = DrivingTheoryTopic.AndereVerkehrsteilnehmer,
            Title = "Die anderen einschätzen",
            Intro = "Ein Auto verhält sich vorhersehbar. Ein sechsjähriges Kind nicht. Der wichtigste "
                  + "Teil des Fahrens ist, zu erkennen, wer sich gleich anders verhalten wird, als du "
                  + "denkst.",
            Sections = new CourseSection[]
            {
                new("Kinder tun das Unerwartete",
                    "Kinder können Entfernung und Geschwindigkeit noch nicht zuverlässig einschätzen und "
                  + "sind leicht abgelenkt - ein Ball ist wichtiger als ein Auto. Wo Kinder am Straßenrand "
                  + "sind, gehst du vom Gas und bist bremsbereit. Dasselbe gilt an Schulen, Spielplätzen "
                  + "und Bushaltestellen.",
                    "136-10"),
                new("Ältere Menschen und Fußgänger",
                    "Ältere Fußgänger brauchen länger und hören schlechter. Am Zebrastreifen musst du "
                  + "ihnen das Überqueren ermöglichen - also anhalten, wenn jemand erkennbar hinüber "
                  + "will. Überholen ist unmittelbar vor und auf einem Fußgängerüberweg verboten.",
                    "350-10"),
                new("Radfahrende",
                    "Sie schwanken beim Anfahren, weichen Schlaglöchern und geöffneten Türen aus und sind "
                  + "schneller, als sie aussehen - ein E-Bike fährt 25 km/h. Beim Rechtsabbiegen ist der "
                  + "Schulterblick Pflicht: genau dort passieren die schweren Unfälle.",
                    "138-10"),
                new("Lkw und der tote Winkel",
                    "Ein Lkw hat rechts neben sich einen Bereich, in dem der Fahrer niemanden sieht. Wenn "
                  + "du dem Fahrer im Spiegel nicht ins Gesicht sehen kannst, sieht er dich auch nicht. "
                  + "Außerdem schwenkt ein Lkw beim Abbiegen aus - niemals rechts neben ihm stehenbleiben, "
                  + "wenn er blinkt.")
            },
            KeyPoints = new[]
            {
                "Bei Kindern am Straßenrand: vom Gas und bremsbereit.",
                "Am Zebrastreifen anhalten, wenn jemand erkennbar hinüber will - und dort nie überholen.",
                "Wer den Lkw-Fahrer nicht im Spiegel sieht, wird von ihm nicht gesehen."
            }
        },
        new()
        {
            Id = "kurs-situationen",
            Topic = DrivingTheoryTopic.BesondereSituationen,
            Title = "Wetter, Dunkelheit, Bahnübergang",
            Intro = "Dieselbe Straße ist bei Regen, Nebel oder Nacht eine andere Straße. Und ein paar "
                  + "Orte haben eigene Regeln, weil dort Fehler nicht verzeihbar sind.",
            Sections = new CourseSection[]
            {
                new("Nässe, Aquaplaning, Glätte",
                    "Bei Regen wird der Bremsweg länger, und bei stehendem Wasser kann der Reifen "
                  + "aufschwimmen - dann lenkt und bremst nichts mehr. Richtig ist: vom Gas gehen, "
                  + "Lenkrad gerade halten, nicht bremsen. Brücken und Waldstücke vereisen zuerst, weil "
                  + "sie von unten und ohne Sonne auskühlen.",
                    "114"),
                new("Nebel und Dunkelheit",
                    "Die Nebelschlussleuchte darf erst unter 50 Metern Sichtweite an - sie blendet stark. "
                  + "Bei Dunkelheit fährst du mit Abblendlicht so, dass du innerhalb der beleuchteten "
                  + "Strecke anhalten kannst; das sind bei Abblendlicht oft weniger als 100 km/h. "
                  + "Fernlicht wird abgeblendet, sobald Gegenverkehr kommt oder du auffährst."),
                new("Bahnübergang",
                    "Das Andreaskreuz bedeutet: Schienenfahrzeuge haben Vorrang, immer. Vor einem "
                  + "Bahnübergang wird nicht überholt, und wer ihn nicht vollständig überqueren kann, "
                  + "fährt gar nicht erst drauf. Bleibt das Auto liegen: aussteigen, alle raus, weg vom "
                  + "Gleis - dann erst den Notruf.",
                    "201-50"),
                new("Autobahn und Rettungsgasse",
                    "Beim Auffahren beschleunigst du auf dem Beschleunigungsstreifen auf das Tempo des "
                  + "fließenden Verkehrs und ordnest dich in eine Lücke ein - der fließende Verkehr hat "
                  + "Vorrang. Sobald es stockt, bildest du die Rettungsgasse: zwischen dem linken und "
                  + "dem daneben liegenden Fahrstreifen, sofort und nicht erst, wenn das Blaulicht zu "
                  + "hören ist.",
                    "124")
            },
            KeyPoints = new[]
            {
                "Bei Aquaplaning: vom Gas, Lenkrad gerade, nicht bremsen.",
                "Nebelschlussleuchte erst unter 50 Metern Sichtweite.",
                "Rettungsgasse zwischen dem linken und dem Fahrstreifen daneben - sofort beim Stocken."
            }
        },
        new()
        {
            Id = "kurs-unfall",
            Topic = DrivingTheoryTopic.UnfallUndPanne,
            Title = "Unfall und Panne",
            Intro = "An einer Unfallstelle zählt die Reihenfolge, und sie ist immer dieselbe: erst "
                  + "sichern, dann melden, dann helfen. Wer sofort losrennt, wird selbst zum zweiten Opfer.",
            Sections = new CourseSection[]
            {
                new("Absichern kommt zuerst",
                    "Warnblinkanlage an, Warnweste anziehen - noch im Auto -, dann aussteigen und das "
                  + "Warndreieck aufstellen. Innerorts etwa 50 Meter vorher, auf der Landstraße 100 "
                  + "Meter, auf der Autobahn 200 Meter. Hinter der Leitplanke ist der einzig sichere "
                  + "Platz zum Warten.",
                    "600"),
                new("Notruf 112",
                    "Die 112 gilt in ganz Europa und ist kostenlos. Wichtig ist nicht ein auswendig "
                  + "gelernter Meldespruch, sondern dass du wo, was und wie viele Verletzte sagst - und "
                  + "dass du nicht auflegst, bevor die Leitstelle es sagt. Sie stellt die Fragen."),
                new("Helfen ist Pflicht",
                    "Unterlassene Hilfeleistung ist eine Straftat. Du musst nicht perfekt sein: Ansprechen, "
                  + "Decke drüber, jemanden aus dem Gefahrenbereich ziehen und bei der Leitstelle bleiben "
                  + "ist bereits Hilfe. Einen Helm nimmst du nur ab, wenn es zum Atmen nötig ist - dann "
                  + "aber unbedingt."),
                new("Panne und Abschleppen",
                    "Dieselbe Sicherungsreihenfolge wie beim Unfall. Beim Abschleppen müssen beide "
                  + "Fahrzeuge die Warnblinkanlage einschalten, und auf die Autobahn darf nur bis zur "
                  + "nächsten Ausfahrt abgeschleppt werden. Abschleppen ist eine Notlösung, kein "
                  + "Transportweg.")
            },
            KeyPoints = new[]
            {
                "Reihenfolge: absichern, melden, helfen.",
                "Warndreieck: 50 m innerorts, 100 m Landstraße, 200 m Autobahn.",
                "Beim Notruf nicht auflegen - die Leitstelle beendet das Gespräch."
            }
        },
        new()
        {
            Id = "kurs-umwelt",
            Topic = DrivingTheoryTopic.UmweltUndSparsamkeit,
            Title = "Sparsam fahren",
            Intro = "Umweltbewusstes Fahren ist fester Bestandteil der Prüfung - und nebenbei das "
                  + "Einzige am Autofahren, das direkt Geld spart.",
            Sections = new CourseSection[]
            {
                new("Früh hochschalten, niedertourig fahren",
                    "Schalte früh in den nächsthöheren Gang, etwa bei 2000 Umdrehungen, und fahre so weit "
                  + "wie möglich im höchsten sinnvollen Gang. Niedrige Drehzahl heißt weniger Verbrauch "
                  + "und weniger Lärm. Beim Verzögern lässt du den Gang drin und gehst vom Gas: moderne "
                  + "Motoren bekommen dann gar keinen Kraftstoff."),
                new("Vorausschauen spart mehr als jede Technik",
                    "Wer früh erkennt, dass die Ampel rot wird, und ausrollt, verbraucht nichts, statt "
                  + "erst zu beschleunigen und dann zu bremsen. Bremsen heißt immer: Kraftstoff, den du "
                  + "schon bezahlt hast, in Wärme verwandeln."),
                new("Reifendruck und Ballast",
                    "Zu niedriger Reifendruck kostet Sprit und Reifen - und verlängert den Bremsweg. "
                  + "Dachgepäckträger und Fahrradträger gehören ab, sobald sie nicht gebraucht werden; "
                  + "sie kosten auch leer spürbar Verbrauch."),
                new("Motor aus statt Leerlauf",
                    "Im Stand verbraucht ein Motor Kraftstoff, ohne dass sich etwas bewegt. Bei "
                  + "absehbaren Wartezeiten - Bahnübergang, längere Ampel, Stau - stellst du ihn ab. "
                  + "Und ein kalter Motor wird nicht im Stand warm, sondern beim vorsichtigen Losfahren.")
            },
            KeyPoints = new[]
            {
                "Früh hochschalten und niedertourig fahren.",
                "Vorausschauen und ausrollen spart mehr als jede Spartechnik.",
                "Bei absehbarer Wartezeit den Motor abstellen - warmlaufen lassen bringt nichts."
            }
        },
        new()
        {
            Id = "kurs-technik",
            Topic = DrivingTheoryTopic.FahrzeugtechnikUndSicherheit,
            Title = "Technik, die dich schützt",
            Intro = "Ein Auto ist nur so sicher wie seine Reifen und so gut sichtbar wie seine Lampen. "
                  + "Beides kannst du selbst prüfen, ohne Werkstatt.",
            Sections = new CourseSection[]
            {
                new("Reifen",
                    "Die gesetzliche Mindestprofiltiefe beträgt 1,6 Millimeter. Sinnvoll sind mehr: unter "
                  + "3 Millimetern lässt die Wirkung auf nasser Straße deutlich nach. Der Reifendruck "
                  + "steht in der Tankklappe oder im Türrahmen und wird am kalten Reifen geprüft. Bei "
                  + "Schnee, Eis und Glätte ist Winterbereifung Pflicht - situativ, nicht nach Datum."),
                new("Bremsen und Fahrhilfen",
                    "ABS verhindert das Blockieren der Räder, damit du beim Vollbremsen noch lenken "
                  + "kannst - der Bremsweg wird dadurch nicht unbedingt kürzer. ESP greift ein, wenn das "
                  + "Auto auszubrechen droht. Beide sind Hilfen, keine Physik-Ausschalter: auf Eis "
                  + "hilft auch ESP nicht mehr weiter."),
                new("Licht",
                    "Abblendlicht bei Dämmerung, Regen, Nebel und Dunkelheit. Tagfahrlicht ersetzt es "
                  + "nicht, denn es beleuchtet nur nach vorn und lässt das Rücklicht dunkel - bei Regen "
                  + "bist du damit von hinten fast unsichtbar. Die Warnblinkanlage ist für Gefahr, nicht "
                  + "fürs Parken in zweiter Reihe.",
                    "131"),
                new("Gurt und Kopfstütze",
                    "Der Gurt muss straff und flach anliegen, ohne Verdrehung, und gehört über das "
                  + "Becken, nicht über den Bauch. Die Kopfstütze wird so eingestellt, dass ihre Oberkante "
                  + "etwa auf Höhe des Scheitels steht - zu tief eingestellt verstärkt sie ein "
                  + "Schleudertrauma, statt es zu verhindern.")
            },
            KeyPoints = new[]
            {
                "Mindestprofiltiefe 1,6 mm - unter 3 mm wird es auf nasser Straße kritisch.",
                "ABS hält dich lenkfähig, es verkürzt den Bremsweg nicht zwangsläufig.",
                "Tagfahrlicht ersetzt kein Abblendlicht - hinten bleibt es dunkel."
            }
        },
        new()
        {
            Id = "kurs-ladung",
            Topic = DrivingTheoryTopic.BefoerderungUndAnhaenger,
            Title = "Menschen, Ladung, Anhänger",
            Intro = "Alles, was du mitnimmst, verändert das Fahrverhalten - und alles, was nicht "
                  + "gesichert ist, wird bei einer Vollbremsung zum Geschoss.",
            Sections = new CourseSection[]
            {
                new("Kinder im Auto",
                    "Kinder unter 12 Jahren, die kleiner als 150 Zentimeter sind, brauchen einen "
                  + "Kindersitz. Ein rückwärts gerichteter Sitz darf nie auf einen Beifahrersitz mit "
                  + "aktivem Airbag - der Airbag würde ihn mit voller Wucht treffen. Verantwortlich ist "
                  + "der Fahrer, nicht die Eltern auf der Rückbank."),
                new("Ladung sichern",
                    "Ladung muss so verstaut sein, dass sie bei Vollbremsung, Ausweichen und schlechter "
                  + "Fahrbahn nicht verrutscht. Schwere Teile gehören nach unten und nach vorn, direkt "
                  + "hinter die Rückenlehne. Eine lose Getränkekiste im Kofferraum wiegt bei Tempo 50 in "
                  + "der Wirkung ein Vielfaches ihres Gewichts."),
                new("Was übersteht, muss gekennzeichnet sein",
                    "Ragt Ladung mehr als einen Meter über das Rückleuchtenende hinaus, gehört eine rote "
                  + "Fahne oder ein rotes Schild ans Ende. Bei Dunkelheit zusätzlich eine rote Leuchte. "
                  + "Nach vorn darf Ladung überhaupt nur in engen Grenzen hinausragen."),
                new("Mit Anhänger",
                    "Ein Anhänger verlängert das Gespann und verlängert damit auch Überholvorgänge, "
                  + "Bremswege und den Platzbedarf beim Abbiegen. Ohne besondere Zulassung gelten "
                  + "außerorts 80 km/h. Die Stützlast an der Kupplung muss stimmen: zu wenig Last vorn "
                  + "lässt das Gespann bei höherem Tempo aufschaukeln - dann hilft nur, vom Gas zu gehen "
                  + "und nicht zu lenken.")
            },
            KeyPoints = new[]
            {
                "Kindersitz bis 12 Jahre oder 150 cm - der Fahrer ist verantwortlich.",
                "Schwere Ladung nach unten und direkt hinter die Rückenlehne.",
                "Schaukelt das Gespann: vom Gas, nicht gegenlenken."
            }
        }
    };
}
