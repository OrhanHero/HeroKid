# Lehrplan-Zuordnung (Berliner Rahmenlehrplan, Klasse 6 & 9)

Diese Übersicht zeigt, welche Themen die Generatoren in `src/LernTor.ContentGen/Generators`
aktuell abdecken. Es handelt sich um eine **repräsentative Auswahl** zentraler Themen je Fach und
Klassenstufe, nicht um eine vollständige 1:1-Abbildung des kompletten Rahmenlehrplans. Die
Architektur (ein `TopicFactory`-Delegate pro Thema in `ExerciseGeneratorBase`) ist bewusst so
gebaut, dass weitere Themen einfach als zusätzliche private Methode + Eintrag in `TopicsByGrade`
ergänzt werden können.

**Poolgröße je Thema**: Jedes Thema wird von einer festen Liste kuratierter Beispiele bedient
(außer Mathematik, das echte Zahlenwerte würfelt statt aus einer festen Liste zu ziehen - dort ist
die Zahl der möglichen Aufgaben pro Thema praktisch unbegrenzt). Der Zielwert für diese Listen ist
**20 Beispiele pro Thema**: Bei zu kleinen Pools (ursprünglich nur 2-4, später 5 Beispiele) griff die
Wiederholungs-Vermeidung in `ExerciseGeneratorBase.Generate` schnell ins Leere, und dasselbe Kind
sah dieselben Fragen bereits nach 1-2 Tagen wieder. Alle 11 Fächer mit fester Beispiel-Liste
(Deutsch, Englisch, Türkisch, ITG, Politik, Physik, Biologie, Chemie, Geografie, Gewi, Ethik) sind
inzwischen auf diesen Zielwert gebracht.

## Mathematik (`MathGenerator.cs`)

| Klasse 6 | Klasse 9 |
|---|---|
| Bruchrechnung: Addition | Lineare Gleichungen |
| Bruchrechnung: Multiplikation | Lineare Funktionen (Steigung/Achsenabschnitt) |
| Prozentrechnung (Prozentwert) | Quadratische Gleichungen (pq-Formel) |
| Negative Zahlen | Satz des Pythagoras |
| Flächen-/Umfangsberechnung (Rechteck) | Zinsrechnung (einfache Zinsen) |
| Maßstab | Binomische Formeln |
| Wahrscheinlichkeit bei Zufallsexperimenten | Mittelwert und Median (Statistik) |

## Deutsch (`GermanGenerator.cs`)

| Klasse 6 | Klasse 9 |
|---|---|
| Wortarten (Nomen/Verb/Adjektiv) | Aktiv und Passiv |
| Zeitformen (Präsens/Präteritum/Perfekt) | Satzgefüge und Konjunktionen |
| Satzglieder (Subjekt/Prädikat/Objekt) | Kommasetzung |
| Groß- und Kleinschreibung | "dass" oder "das" |
| Steigerung von Adjektiven | Wortarten vertieft (Adverb/Präposition/Konjunktion) |
| | Textsorten unterscheiden (Bericht/Kommentar/Reportage/...) |

## Türkisch (`TurkishGenerator.cs`)

| Klasse 6 | Klasse 9 |
|---|---|
| Şimdiki Zaman (Präsens) | Cümlenin Ögeleri (Satzglieder) |
| Geçmiş Zaman (Präteritum) | Gelecek Zaman (Futur) |
| Eş Anlamlı Kelimeler (Synonyme) | Yazım Kuralları (Rechtschreibung) |
| Zıt Anlamlı Kelimeler (Antonyme) | Fiilimsi (Partizip/Verbalnomen) |
| Doğa ve Çevre (Natur und Umwelt) – Wortschatz | |

## Physik (`PhysikGenerator.cs`)

| Klasse 6 | Klasse 9 |
|---|---|
| Aggregatzustände | Ohmsches Gesetz |
| Einfacher Stromkreis | Energieerhaltung |
| Magnetismus | Newtonsche Gesetze |
| | Magnetfelder und elektromagnetische Induktion |

## Chemie (`ChemieGenerator.cs`)

| Klasse 6 | Klasse 9 |
|---|---|
| Stoffgemische trennen | Atommodell |
| Verbrennung | Chemische Reaktionen |
| Säuren und Laugen | Periodensystem |
| Metalle und ihre Eigenschaften | |

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
| Ernährung – wie werden Menschen satt? | |

## Politik (`PolitikGenerator.cs`)

| Klasse 6 | Klasse 9 |
|---|---|
| Was ist Demokratie? | Gewaltenteilung |
| Berlin und seine Bezirke | Bundestag und Bundesrat |
| Wahlrecht | Wahlsystem |
| | Soziale Marktwirtschaft |

## Geografie (`GeoGenerator.cs`)

| Klasse 6 | Klasse 9 |
|---|---|
| Kontinente und Ozeane | Plattentektonik |
| Klimazonen | Klimawandel |
| Deutschland: Bundesländer | Verstädterung |
| | Armut und Reichtum weltweit |

## Ethik (`EthikGenerator.cs`)

| Klasse 6 | Klasse 9 |
|---|---|
| Werte und Regeln | Verantwortung und Pflicht |
| Freundschaft und Konflikte | Meinungsfreiheit und Grenzen |
| Weltreligionen | Digitale Ethik |
| | Recht und Gerechtigkeit |

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

Die obige Themenauswahl wurde mit der offiziellen Broschüre "Rahmenlehrplan 1-10 kompakt"
(Senatsverwaltung für Bildung, Berlin, 1. Auflage 2017) abgeglichen, inklusive der zuvor noch nicht
gelesenen Abschnitte zu Mathematik, Chemie, Physik, Türkisch, Geografie, Ethik, Politische Bildung,
Gesellschaftswissenschaften 5/6 und Informatik. Die Broschüre selbst ist urheberrechtlich geschützt,
darf laut Impressum aber "für die Zwecke der Schule" verwendet werden – sie diente hier nur als
interne Orientierung, es wurden keine Textpassagen übernommen.

**Als direkte Folge dieses Abgleichs wurden neun neue Themen ergänzt** (jeweils mit echten
Beispielaufgaben inkl. Erklärung/HelpHint, kein reiner Dokumentations-Platzhalter):

- **Mathematik**: "Wahrscheinlichkeit bei Zufallsexperimenten" (Klasse 6) und "Mittelwert und Median
  (Statistik)" (Klasse 9) schließen die zuvor komplett fehlende Leitidee **"Daten und Zufall"**.
- **Chemie**: "Metalle und ihre Eigenschaften" (Klasse 6) deckt das zuvor fehlende Themenfeld Metalle ab.
- **Deutsch**: "Textsorten unterscheiden" (Klasse 9) deckt die RLP-Textsorten (Bericht, Kommentar,
  Reportage, Leserbrief, Erörterung), die zuvor nur in Klasse 5/6-Form (Wortart/Satzart) vorkamen.
- **Türkisch**: "Doğa ve Çevre (Natur und Umwelt) – Wortschatz" (Klasse 6) bringt erstmals einen
  themenorientierten (statt rein grammatikorientierten) Wortschatz-Topic ein, passend zum
  RLP-Themenfeld "Natur und Umwelt".
- **Gesellschaftswissenschaften/Gewi**: "Ernährung – wie werden Menschen satt?" (Klasse 6) deckt das
  RLP-Themenfeld "Ernährung" der Gewi-5/6-Doppeljahrgangsstufe ab.
- **Geografie**: "Armut und Reichtum weltweit" (Klasse 9) deckt das gleichnamige RLP-Themenfeld ab.
- **Physik**: "Magnetfelder und elektromagnetische Induktion" (Klasse 9) deckt das RLP-Themenfeld
  "Magnetfelder und elektromagnetische Induktion" (Doppeljahrgangsstufe 9/10) ab.
- **Politik**: "Soziale Marktwirtschaft" (Klasse 9) deckt das gleichnamige RLP-Themenfeld
  (Doppeljahrgangsstufe 9/10) ab.
- **Ethik**: "Recht und Gerechtigkeit" (Klasse 9) deckt das gleichnamige RLP-Themenfeld ab, das zuvor
  nur indirekt über "Meinungsfreiheit und Grenzen" gestreift wurde.

**Bewusst nicht übernommene/verbleibende Unterschiede** (kein technischer Mangel, sondern
Simplifizierungen dieser App gegenüber dem vollständigen RLP):

- **Chemie**: organische Chemie (Kohlenwasserstoffe, Alkohole, organische Säuren, Ester,
  Doppeljahrgangsstufe 9/10) bleibt bewusst ausgeklammert - eher Grundschul-fernes Spezialwissen für
  den Zielaltersbereich dieser App.
- **Biologie**: Der RLP führt Biologie als eigenständiges Fach erst ab Doppeljahrgangsstufe 7/8; in
  5/6 ist es Teil des integrierten Fachs Naturwissenschaften. Unsere Klasse-6-Themen (menschlicher
  Körper, Fotosynthese, Wirbeltierklassen) sind trotzdem sinnvoll, da die App Bio/Chemie/Physik aus
  Vereinfachungsgründen als getrennte Fächer ab Klasse 6 führt.
- **Türkisch**: Der RLP gliedert das Fach primär in kommunikative Themenfelder (Individuum und
  Gesellschaft, Gesellschaft und öffentliches Leben, Kultur und historischer Hintergrund, Natur und
  Umwelt) statt nach Grammatikthemen. `TurkishGenerator.cs` bleibt überwiegend grammatikorientiert
  (Zeiten, Satzglieder, Rechtschreibung), da das für automatisch geprüfbare Übungsfragen
  praktikabler ist - das neue Wortschatz-Thema ist ein erster Schritt in Richtung RLP-Themenfelder,
  kein vollständiger Umbau.
- **Informatik/ITG**: Das RLP-Themenfeld "Standardsoftware" (praktischer Umgang mit
  Textverarbeitung/Tabellenkalkulation) lässt sich kaum als automatisch auswertbare Quizfrage
  abbilden und bleibt daher unberücksichtigt; die übrigen Themenfelder (Informatiksysteme, Leben in
  vernetzten Systemen) sind über die bestehenden Themen (Datenschutz, Cybermobbing, Fake News,
  Algorithmen) plausibel abgedeckt.
- **Deutsch, Geografie, Politische Bildung**: Die RLP-Themenfelder für literarische Textanalyse
  (Drama, Novelle, Parabel), tiefergehende Sozialkunde-Themen (z.B. Migration/Bevölkerung,
  Rechtsstaat) und weitere Geografie-Themenfelder (z.B. Tourismus/Mobilität) sind bewusst nicht
  1:1 übernommen - eine vollständige Abdeckung aller RLP-Themenfelder ist weiterhin nicht das Ziel
  dieser App (siehe Hinweis am Anfang dieser Datei), die repräsentative Auswahl wurde aber durch
  diesen Abgleich gezielt um die auffälligsten Lücken erweitert.

Dieser Abgleich verbessert die Abdeckung gezielt, ist aber weiterhin keine vollständige 1:1-Analyse
aller RLP-Themenfelder für alle zehn Jahrgangsstufen - die App konzentriert sich bewusst auf Klasse 6
und 9.
