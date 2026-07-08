# Lehrplan-Zuordnung (Berliner Rahmenlehrplan, Klasse 6 & 9)

Diese Übersicht zeigt, welche Themen die Generatoren in `src/LernTor.ContentGen/Generators`
aktuell abdecken. Es handelt sich um eine **repräsentative Auswahl** zentraler Themen je Fach und
Klassenstufe, nicht um eine vollständige 1:1-Abbildung des kompletten Rahmenlehrplans. Die
Architektur (ein `TopicFactory`-Delegate pro Thema in `ExerciseGeneratorBase`) ist bewusst so
gebaut, dass weitere Themen einfach als zusätzliche private Methode + Eintrag in `TopicsByGrade`
ergänzt werden können.

## Mathematik (`MathGenerator.cs`)

| Klasse 6 | Klasse 9 |
|---|---|
| Bruchrechnung: Addition | Lineare Gleichungen |
| Bruchrechnung: Multiplikation | Lineare Funktionen (Steigung/Achsenabschnitt) |
| Prozentrechnung (Prozentwert) | Quadratische Gleichungen (pq-Formel) |
| Negative Zahlen | Satz des Pythagoras |
| Flächen-/Umfangsberechnung (Rechteck) | Zinsrechnung (einfache Zinsen) |
| Maßstab | Binomische Formeln |

## Deutsch (`GermanGenerator.cs`)

| Klasse 6 | Klasse 9 |
|---|---|
| Wortarten (Nomen/Verb/Adjektiv) | Aktiv und Passiv |
| Zeitformen (Präsens/Präteritum/Perfekt) | Satzgefüge und Konjunktionen |
| Satzglieder (Subjekt/Prädikat/Objekt) | Kommasetzung |
| Groß- und Kleinschreibung | "dass" oder "das" |
| Steigerung von Adjektiven | Wortarten vertieft (Adverb/Präposition/Konjunktion) |

## Türkisch (`TurkishGenerator.cs`)

| Klasse 6 | Klasse 9 |
|---|---|
| Şimdiki Zaman (Präsens) | Cümlenin Ögeleri (Satzglieder) |
| Geçmiş Zaman (Präteritum) | Gelecek Zaman (Futur) |
| Eş Anlamlı Kelimeler (Synonyme) | Yazım Kuralları (Rechtschreibung) |
| Zıt Anlamlı Kelimeler (Antonyme) | Fiilimsi (Partizip/Verbalnomen) |

## Physik (`PhysikGenerator.cs`)

| Klasse 6 | Klasse 9 |
|---|---|
| Aggregatzustände | Ohmsches Gesetz |
| Einfacher Stromkreis | Energieerhaltung |
| Magnetismus | Newtonsche Gesetze |

## Chemie (`ChemieGenerator.cs`)

| Klasse 6 | Klasse 9 |
|---|---|
| Stoffgemische trennen | Atommodell |
| Verbrennung | Chemische Reaktionen |
| Säuren und Laugen | Periodensystem |

## Biologie (`BiologieGenerator.cs`)

| Klasse 6 | Klasse 9 |
|---|---|
| Der menschliche Körper | Zellbiologie |
| Fotosynthese | Vererbung/Genetik |
| Wirbeltierklassen | Ökosysteme |

## Englisch (`EnglischGenerator.cs`)

| Klasse 6 | Klasse 9 |
|---|---|
| Simple Present vs. Present Progressive | Simple Past vs. Present Perfect |
| Unregelmäßige Pluralformen | Conditional Sentences (Type 1) |
| Question Words | Passive Voice |

## Gesellschaftswissenschaften / Gewi (`GewiGenerator.cs`)

| Klasse 6 | Klasse 9 |
|---|---|
| Geschichtliche Epochen | Grundgesetz |
| Kartenkunde und Himmelsrichtungen | Wirtschaftskreislauf |
| Kinderrechte | Medien und Gesellschaft |

## Politik (`PolitikGenerator.cs`)

| Klasse 6 | Klasse 9 |
|---|---|
| Was ist Demokratie? | Gewaltenteilung |
| Berlin und seine Bezirke | Bundestag und Bundesrat |
| Wahlrecht | Wahlsystem |

## Geografie (`GeoGenerator.cs`)

| Klasse 6 | Klasse 9 |
|---|---|
| Kontinente und Ozeane | Plattentektonik |
| Klimazonen | Klimawandel |
| Deutschland: Bundesländer | Verstädterung |

## Ethik (`EthikGenerator.cs`)

| Klasse 6 | Klasse 9 |
|---|---|
| Werte und Regeln | Verantwortung und Pflicht |
| Freundschaft und Konflikte | Meinungsfreiheit und Grenzen |
| Weltreligionen | Digitale Ethik |

## Medienbildung / ITG (`ItgGenerator.cs`)

| Klasse 6 | Klasse 9 |
|---|---|
| Datenschutz-Grundlagen | Cybermobbing |
| Sichere Passwörter | Fake News erkennen |
| Urheberrecht im Internet | Algorithmen-Grundbegriff |

## News (`LernTor.News`)

Kuratierte, kostenlose RSS-Quellen (siehe `NewsFeedSource.cs`):

- **Deutschland/Berlin**: tagesschau.de, rbb24 Berlin, Tagesspiegel Berlin
- **Türkei/Istanbul/Samsun/Ünye**: Hürriyet, Sabah
- **Technik/KI**: heise online (deckt digitale/KI-Themen ab, die für Jugendliche zunehmend
  alltagsrelevant sind)

Artikel werden nach Schlüsselwörtern priorisiert (Berlin, Deutschland, Istanbul, Samsun, Ünye,
Türkei, sowie KI/Künstliche Intelligenz/ChatGPT/Digital) und auf die 5-8 relevantesten/aktuellsten
reduziert. Berlin-Lokalnachrichten sind explizit sehr wichtig: mindestens 2 (bzw. ein Drittel von
`targetCount`) aktuelle Berlin-Artikel werden garantiert aufgenommen, bevor die restlichen Plätze
nach allgemeiner Priorität aufgefüllt werden. Artikel mit verstörenden Themen (Krieg, Gewaltverbrechen,
Suizid, ...) werden nicht hart ausgefiltert, aber deutlich nach unten gestuft (`SensitiveKeywords` in
`NewsFeedSource.cs`), damit harmlosere, altersgerechtere Artikel bevorzugt ausgewählt werden. Artikel,
die inhaltlich (Titel) doppelt vorkommen - z.B. weil zwei Feeds dieselbe Meldung führen - werden vorher
herausgefiltert. Pro Artikel werden automatisch zwei Verständnisfragen erzeugt (Herkunfts-/Regionsfrage
+ Schlüsselwort-Frage aus der Überschrift).

**Wie die App an Nachrichten kommt**: `RssNewsService` lädt bei jedem Aufruf des News-Bereichs live
die RSS-Feeds aller obigen Quellen per `HttpClient` (kein Cache, keine gespeicherten Artikel) - die
Inhalte sind also tatsächlich tagesaktuell, nicht vorproduziert. Nicht erreichbare Feeds werden
einzeln übersprungen, ohne den Ladevorgang der übrigen Feeds abzubrechen.

## Abschlussquiz-Zusammenstellung bei vielen Fächern

`QuizComposer.ComposeFinalQuiz` verteilt die Fragenzahl dynamisch auf alle nicht deaktivierten
Fächer, damit das Quiz bei 12 möglichen Fächern nicht auf 60+ Fragen anwächst: Sind z.B. nur 4
Fächer aktiv, bekommt jedes davon mehr Fragen; sind alle 12 aktiv, entsprechend weniger pro Fach.
Eltern steuern über "Bereiche deaktivieren" im Eltern-Bereich, welche Fächer täglich überhaupt
Teil des Ablaufs und des Abschlussquiz sind.

## Erweiterung um weitere Themen

Neues Thema hinzufügen (Beispiel Mathematik):

1. Neue private `static QuizQuestion MeinNeuesThema(Random r) { ... }`-Methode in `MathGenerator.cs`.
2. Methode in die passende Liste (`TopicsByGrade[GradeLevel.KlasseX]`) eintragen.
3. Test in `tests/LernTor.Tests` ergänzen, der prüft, dass die eigene Musterlösung als richtig erkannt wird.

Kein Codeänderung an `ExerciseGeneratorBase`, `QuizComposer` oder den ViewModels nötig – die
Zufallsauswahl und Quiz-Zusammenstellung funktioniert automatisch mit neuen Themen.

## Abgleich mit dem offiziellen Rahmenlehrplan 1-10 (kompakt)

Die obige Themenauswahl wurde stichprobenartig mit der offiziellen Broschüre "Rahmenlehrplan 1-10
kompakt" (Senatsverwaltung für Bildung, Berlin, 1. Auflage 2017) abgeglichen. Die Broschüre selbst
ist urheberrechtlich geschützt, darf laut Impressum aber "für die Zwecke der Schule" verwendet
werden – sie diente hier nur als interne Orientierung, es wurden keine Textpassagen übernommen.
Ergebnis des Abgleichs, mit konkreten Ideen für künftige Themenerweiterungen:

- **Mathematik**: Die offizielle Leitidee **"Daten und Zufall"** (Statistik, Wahrscheinlichkeiten,
  Zählstrategien) fehlt in `MathGenerator.cs` bisher komplett – die anderen vier Leitideen (Zahlen
  und Operationen, Größen und Messen, Raum und Form, Gleichungen und Funktionen) sind über die
  bestehenden Themen gut abgedeckt. Naheliegende Ergänzung: ein Themenfeld "Mittelwert/Median" oder
  "Wahrscheinlichkeit beim Würfeln" für Klasse 6/9.
- **Chemie**: Der RLP nennt zusätzlich die Themenfelder Salze, Metalle sowie (9/10) organische Chemie
  (Kohlenwasserstoffe, Alkohole, organische Säuren, Ester) – aktuell in `ChemieGenerator.cs` nicht
  vertreten. Die vorhandenen Themen (Stoffgemische, Verbrennung, Säuren/Laugen, Atommodell,
  Reaktionen, Periodensystem) decken den Kern aber ab.
- **Biologie**: Der RLP führt Biologie als eigenständiges Fach erst ab Doppeljahrgangsstufe 7/8; in
  5/6 ist es Teil des integrierten Fachs Naturwissenschaften. Unsere Klasse-6-Themen (menschlicher
  Körper, Fotosynthese, Wirbeltierklassen) sind trotzdem sinnvoll, da die App Bio/Chemie/Physik aus
  Vereinfachungsgründen als getrennte Fächer ab Klasse 6 führt.
- **Türkisch**: Der RLP gliedert das Fach in kommunikative Themenfelder (Individuum und Gesellschaft,
  Gesellschaft und öffentliches Leben, Kultur und historischer Hintergrund, Natur und Umwelt) statt
  nach Grammatikthemen. `TurkishGenerator.cs` ist bewusst grammatikorientiert (Zeiten, Satzglieder,
  Rechtschreibung), was für automatisch geprüfbare Übungsfragen praktikabler ist – künftige
  Themen könnten aber zusätzlich Wortschatz zu Alltag/Kultur/Umwelt abdecken, um näher am RLP zu sein.
- **Geografie, Ethik, Politische Bildung, Gesellschaftswissenschaften 5/6, Physik, Informatik**: Die
  vorhandenen Themen bilden jeweils eine plausible Teilmenge der RLP-Themenfelder ab (z.B.
  Geografie: Klimazonen/Klimawandel und Kontinente/Verstädterung passen zu "Klimawandel und
  Klimaschutz" bzw. "Vielfalt der Erde"); eine vollständige 1:1-Abdeckung aller RLP-Themenfelder ist
  weiterhin nicht das Ziel dieser App (siehe Hinweis am Anfang dieser Datei).

Dieser Abgleich ist eine Momentaufnahme, keine abgeschlossene Analyse – die Broschüre deckt alle
Fächer und Jahrgangsstufen 1-10 ab, während die App sich auf Klasse 6 und 9 konzentriert.
