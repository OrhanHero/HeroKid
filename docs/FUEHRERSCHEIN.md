# Führerschein Klasse B

Eigener Lernbereich mit drei Unterbereichen, angelehnt an den Aufbau von fahrschule24.de.
Erreichbar als eigene Etappe (`LearningStage.Fuehrerschein`) zwischen KI-Bereich und
Abschlussquiz.

| Unterbereich | Stand | Inhalt |
|---|---|---|
| **Verkehrszeichen** | ✅ fertig | 77 Zeichen in fünf Gruppen, alle als echte Bilddatei (Wikimedia Commons, gemeinfrei), Karteikarten und Quiz, tägliche Challenge |
| **Theoriefragen** | ✅ fertig | 65 eigene Fragen zu den 14 amtlichen Sachgebieten, Prüfungssimulation nach Fehlerpunkten, Schwachstellen-Trainer |
| **Theorie-Kurs** | ✅ fertig | 14 bebilderte Lektionen (eine je Sachgebiet) mit Merksätzen und Lernstandskontrolle |

## Was Pflicht ist und was nicht

Nur die **tägliche Challenge** ist Pflicht: fünf Zeichen, etwa eine Minute. Ohne sie schaltet
der "Weiter"-Knopf nicht frei. Karteikarten und Gruppen-Quiz sind freiwillig und jederzeit
erreichbar.

Das ist Absicht. Ein Bereich, der sich wie eine zweite Schule anfühlt, wird nicht benutzt; einer,
der jeden Tag eine Minute kostet, schon. Die Anzahl ist im Eltern-Bereich pro Kind einstellbar
(3/5/8/10).

## Rechtliches

**Die Verkehrszeichen sind gemeinfrei.** Sie stehen in StVO Anlage 1–3, also in einer
Verordnung, und sind damit amtliches Werk (§ 5 UrhG). Form, Farbe und Bedeutung darf jeder
nachbauen.

**Was übernommen ist und was nicht.** Die Bilddateien selbst kommen von Wikimedia Commons, dort
unter `{{PD-GermanGov}}` (amtliches Werk, § 5 UrhG) freigegeben — je Zeichen einzeln geprüft, nicht
pauschal angenommen. Herkunft und Lizenzvermerk je Nummer stehen in
`src/LernTor.App/Assets/Verkehrszeichen/QUELLEN.md`. Nicht übernommen ist die Zusammenstellung
einer fremden Broschüre wie der ADAC-Übersicht (deren Layout und Auswahl ist geschützt) — diese
App bezieht die Zeichen direkt von der Quelle, nicht über eine Broschüre.

Die Erklär- und Merktexte in diesem Bereich sind selbst geschrieben.

**Der amtliche Fragenkatalog ist NICHT frei.** Die offiziellen Theorie-Prüfungsfragen gehören der
TÜV|DEKRA arge tp 21; kommerzielle Lern-Apps lizenzieren sie. Sie dürfen hier nicht hinein.
Die 65 Fragen in `DrivingTheoryCatalog` sind deshalb **selbst geschrieben** und decken dieselben
14 amtlichen Sachgebiete aus StVO und StVZO ab — zum Lernen gleichwertig, aber nicht wortgleich
mit der Prüfung. Wer wortgleiche Fragen will, muss sie über den vorhandenen Eltern-Import selbst
eintragen.

**Keine Videos.** LernTor ist vollständig offline. Der Theorie-Kurs bekommt stattdessen
bebilderte Erklärseiten mit denselben Zeichenbildern.

## Original-Bilddateien

**Alle 77 Zeichen liegen als echte Bilddatei vor** (PNG, aus Wikimedia Commons, siehe
`src/LernTor.App/Assets/Verkehrszeichen/QUELLEN.md` für Quelle und Lizenz je Nummer). Vorherige
Fassungen dieser App zeichneten die Zeichen selbst nach (handgezogene Geometriepfade) oder
gewannen sie maschinell aus einer ADAC-PDF (`scripts/extract-signs-from-pdf.py`, damals mit
Fehlzuordnungen bei VZ 272/276 — die Skripte bleiben im Repo als Dokumentation des Wegs, sind
aber nicht mehr der Weg, wie die App an ihre Zeichen kommt). Echte Zeichen sehen schlicht besser
aus als beides.

**Warum als eingebettete Ressource statt loser PNG-Dateien:** die App ist vollständig offline und
darf zur Laufzeit nichts nachladen. `LernTor.App.csproj` bindet die Bilder als `Resource` ein -
das kompiliert sie in die Assembly selbst statt sie lose neben die EXE zu legen, sie überstehen
also den Single-File-Publish unverändert (siehe `IncludeNativeLibrariesForSelfExtract` in
CLAUDE.md für eine verwandte, aber andere Falle - die betrifft native DLLs, nicht verwaltete
Ressourcen wie diese hier).

**Warum PNG statt SVG-in-WPF:** WPF zeichnet SVG nicht selbst. Ein Zusatzpaket (SharpVectors) oder
ein SVG→XAML-Build-Schritt hätte dieselbe Fehlerklasse riskiert, die dieses Projekt laut CLAUDE.md
schon zweimal getroffen hat - XAML/Ressourcen, die sauber kompilieren und erst beim Anzeigen
werfen. Ein fertiges Rasterbild bei 960 px Breite hat genug Reserve für die größte Darstellung
(200 px im Quiz) und ist trivial zu laden.

**Der nachgezeichnete Pfad bleibt als Rückfallebene stehen** (`SignPictograms`, `TrafficSign.PathData`/
`Text`) - für den Fall, dass für eine Nummer je keine Bilddatei mitgeliefert würde. Er kommt im
Normalbetrieb nie zum Zug: `TrafficSignVisual` prüft zuerst auf eine Bilddatei
(`TrafficSignImages`), und `TrafficSignCatalogTests`/`TrafficSignRenderTests` halten fest, dass für
jede der 77 Katalognummern tatsächlich eine da ist.

**Aufschrift nicht doppelt:** 16 Zeichen zeigen eine Zahl oder einen Ort schon in der Bilddatei
selbst (z.B. VZ 274-50 die "50", VZ 108-10/110-10 "10 %", VZ 314 das "P", VZ 310/311 einen
Beispielort). `TrafficSignImages.ZeichnetAufschriftSelbst` listet sie, damit
`TrafficSignVisual` `TrafficSign.Text` für genau diese NICHT zusätzlich übers Bild zeichnet -
sonst stünde die Zahl doppelt. Der Text bleibt trotzdem in den Katalogdaten stehen, er ist die
fachlich richtige Aufschrift für den Rückfallpfad.

## Wie der Rückfallpfad entsteht

Nur relevant, falls für eine Nummer je keine Bilddatei mitgeliefert würde - im Normalbetrieb
zeichnet `TrafficSignVisual` immer die echte Bilddatei (siehe oben).

- `SignPictograms` (Core): 58 Geometriepfade in einem gedachten Feld von **0..100** in beiden
  Richtungen. Benannte Konstanten, weil dieselbe Figur auf mehreren Zeichen sitzt — das Fahrrad
  auf VZ 138 (Warnung), 237 (Radweg) und 254 (Verbot).
- `TrafficSign` (Core): Grundform, Farben, Aufschrift, bis zu **zwei** Pfad-Ebenen mit je
  eigener Farbe und Strichstärke. Zwei Ebenen reichen für alles: gestrichener Pfeilschaft plus
  gefüllte Spitze, roter Balken über blauem Grund, graues Auto plus rotes Auto.
- `TrafficSignVisual` (App): ein `FrameworkElement` mit eigenem `OnRender`. Bewusst **kein**
  XAML-Template — als XAML bräuchte das ein Dutzend Trigger je Grundform und würde in genau die
  Fehlerklasse laufen, die dieses Projekt zweimal getroffen hat: XAML, das sauber kompiliert und
  erst beim Anzeigen wirft.

Die Grundform wird zweimal gezeichnet: außen in der Randfarbe, darüber eine verkleinerte Kopie
in der Füllfarbe. Der sichtbare Rand ist also der Rest der äußeren Form — das funktioniert für
Kreis, Dreieck, Achteck und Raute gleichermaßen, ohne für jede Form eine eigene Innenkontur zu
rechnen.

**Drei Zeichen bestehen aus zwei Sinnbildern** (VZ 240 Geh-/Radweg, 260 Kraftfahrzeuge, 276
Überholverbot). Sie haben eigene, kleinere Pfade — die vollen Einzelfiguren übereinandergelegt
ergäben nur einen schwarzen Klecks.

## Wann ein Zeichen als gekonnt gilt

**Zweimal hintereinander richtig** (`TrafficSignProgress.MasteredStreak`) — dieselbe Regel wie
in der Fehler-Kartei, damit die Kinder nicht zwei Vorstellungen von "sitzt" lernen. Ein Fehler
setzt die Serie auf **null**, nicht auf eins weniger: ein gerade verwechseltes Zeichen sitzt
nicht mehr fast.

Zwei statt einmal, weil bei vier Antwortmöglichkeiten jeder vierte Rateversuch trifft — zweimal
hintereinander zu raten gelingt nur in einem von sechzehn Fällen.

**Die Karteikarten zählen bewusst nicht in den Lernstand.** "Wusste ich" drückt sich jeder gern,
und beim Blick auf die Rückseite meint man ohnehin, es gewusst zu haben. Gewertet wird nur das
Quiz, wo die Antwort vor der Auflösung feststeht. Die Karten sind zum Lernen da, das Quiz zum
Prüfen.

## Die tägliche Challenge

`DailySignChallenge.ForDay` leitet die Auswahl aus **Profil-Kennung und Datum** ab, nicht aus dem
Zufallsgenerator:

- Nach einem App-Neustart sind es dieselben Zeichen — sonst startet ein Kind so lange neu, bis
  leichte kommen.
- Beide Kinder bekommen am selben Tag verschiedene — Abschreiben bringt nichts.
- Noch nicht Gekonntes kommt zuerst; gekonnte Zeichen füllen nur auf, wenn nicht genug offene
  übrig sind.

Bewusst **nicht** `string.GetHashCode()` als Startwert: dessen Ergebnis ist in .NET pro
Prozessstart zufällig verwürfelt, die Auswahl wäre nach jedem Neustart eine andere — genau das,
was hier nicht passieren darf.

## Antwortlängen-Falle

Die Kinder haben in diesem Projekt schon einmal ausgenutzt, dass die längste Antwort die richtige
ist (siehe `scripts/check-answer-length-bias.py`). Im Zeichen-Quiz kann das Muster gar nicht
entstehen: alle vier Optionen sind echte Zeichennamen aus demselben Katalog, und welcher davon
richtig ist, entscheidet das gezeigte Bild. `SignQuizBuilderTests` hält fest, dass das so bleibt.

Die Ablenker kommen aus **derselben Gruppe** wie das gefragte Zeichen. Sonst wäre die Form schon
die halbe Antwort: wer ein rotes Dreieck sieht, könnte alles Blaue ausschließen, ohne das Zeichen
zu kennen.

## Theoriefragen

### Warum die Fragen nicht wie ein Quiz funktionieren

Die echte Prüfung unterscheidet sich in zwei Punkten von einem Ein-aus-vier-Quiz, und an beiden
scheitern Prüflinge, die nur Quiz geübt haben:

- **Mehrere Antworten können richtig sein.** Wer eine von zwei richtigen ankreuzt, hat die Frage
  falsch — Teilpunkte gibt es nicht. 10 der 65 Fragen sind Mehrfachfragen.
- **Gezählt werden Fehler*punkte*, nicht Fragen.** Jede Frage wiegt 2 bis 5 Punkte, je nachdem,
  wie gefährlich der Irrtum wäre.

`TheoryExamRules` bildet das ab: bestanden ist, wer **höchstens 10 Fehlerpunkte** hat **und**
nicht zwei 5-Punkte-Fragen verhauen hat. Beide Bedingungen zählen einzeln — bei 30 Fragen kann
man also 28 richtig haben und trotzdem durchfallen. Eine Prozentanzeige würde genau das
verschleiern, deshalb steht auf dem Ergebnisbildschirm die Punktzahl groß und keine Prozentzahl.

### Warum die Antworten gemischt werden

Im Katalog steht die richtige Antwort **bewusst an erster Stelle** — so ist eine Frage beim
Schreiben und beim späteren Nachlesen sofort zu erfassen. Genau so angezeigt wäre sie wertlos:
"immer die erste ankreuzen" hätte volle Punktzahl gegeben.

`TheoryQuestionPresenter.Present` ist die **einzige** Stelle, an der eine Frage in eine
anzeigbare Form kommt, und mischt dabei. Keine Ansicht kann es vergessen, weil man an die
Antworten sonst gar nicht herankommt. `TheoryQuestionPresenterTests` prüft, dass die richtigen
Positionen mitwandern und die richtige Antwort nicht immer auf Platz eins landet.

Die zweite Falle (längste Antwort = richtige) ist mit 49 % gemessen und in
`DrivingTheoryCatalogTests` bei 60 % gedeckelt. Auf 25 % zu drücken wäre falsch: "die längste ist
nie die richtige" wäre das nächste ausnutzbare Muster.

### Üben, Prüfung, Schwachstellen

| Modus | Rückmeldung | Fragen |
|---|---|---|
| Sachgebiet üben | sofort, mit Begründung; "Weiter" muss gedrückt werden | alle des Gebiets, noch nicht sitzende zuerst |
| Prüfungssimulation | **keine** bis zum Ende | 30, über alle Sachgebiete gestreut |
| Schwachstellen-Trainer | sofort, mit Begründung | 10, bevorzugt aus schwachen Gebieten |

Die Prüfung gibt zwischendurch bewusst keine Auflösung — sonst wäre es keine Simulation, sondern
eine lange Übungsrunde. Die Auswertung kommt am Ende dafür vollständig: **jede falsche Frage
einzeln**, mit der eigenen Antwort, der richtigen und der Begründung. Das ist der Ertrag des
Durchlaufs; die Punktzahl allein bringt niemanden weiter.

`TheoryExamComposer.ComposeExam` streut reihum über die Sachgebiete, statt zufällig aus dem Topf
zu ziehen. Sonst kämen an einem Tag zwölf Vorfahrt-Fragen und am nächsten keine — wer nur einen
Ausschnitt übt, hält sich für weiter, als er ist.

### Was als Schwachstelle gilt

Ein Sachgebiet ist eine Schwachstelle, wenn es **belastbar gemessen** (mindestens 4 beantwortete
Fragen) **und** unter 70 % ist. Zwei falsche Antworten machen noch keine Schwachstelle; solange
zu wenig geübt wurde, schickt der Trainer einen gemischten Satz und sagt das auch.

Gemessen wird der **Jetzt-Zustand**, nicht der Durchschnitt der Historie: eine Frage zählt als
richtig, wenn sie gerade sitzt (zweimal hintereinander richtig, `TheoryProgress.MasteredStreak`
— dieselbe Schwelle wie bei den Zeichen). Über alles je Beantwortete zu mitteln würde ein Gebiet
noch monatelang als Schwachstelle führen, nachdem das Kind es längst kann, und der Trainer würde
weiter Fragen daraus schicken statt zum nächsten Problem zu gehen.

## Theorie-Kurs

Vierzehn Lektionen, eine je Sachgebiet, in der Reihenfolge des Enums `DrivingTheoryTopic` - die
Enum-Reihenfolge **ist** die Kursreihenfolge. Jede Lektion hat eine Einleitung, vier Abschnitte
und drei Merksätze; Abschnitte mit einem passenden Verkehrszeichen zeigen es daneben.

### Der Kurs bringt keine eigenen Fragen mit

Die Lernstandskontrolle am Ende einer Lektion zieht bis zu fünf Fragen aus
`DrivingTheoryCatalog` zum selben Sachgebiet. Ein zweiter Fragensatz wäre doppelte Pflege und
würde bei jeder Änderung auseinanderlaufen - und die Kinder würden im Kurs etwas anderes üben als
in der Prüfungssimulation.

Aus demselben Grund **zählen die Antworten der Kontrolle in den Theorie-Lernstand**: es sind
dieselben Fragen. Sie hier nicht mitzuzählen hieße, dass eine im Kurs gemeisterte Frage im
Schwachstellen-Trainer weiter als ungekonnt geführt wird.

### Gelesen und geschafft sind zwei Dinge

| Stand | Bedeutung |
|---|---|
| offen | noch nie geöffnet |
| gelesen | "Fertig gelesen" gedrückt oder eine Kontrolle versucht |
| geschafft | Kontrolle mit mindestens 70 % bestanden |

Nur "geschafft" zu speichern hieße, dass eine gelesene Lektion nach einer verpatzten Kontrolle
wieder aussieht wie nie geöffnet - und das Kind fängt entnervt von vorn an. Gespeichert wird
außerdem der **beste** Versuch, nicht der letzte: ein aus Neugier gestarteter und abgebrochener
zweiter Anlauf soll das erste Ergebnis nicht verderben.

Die Kontrolle ist **freiwillig**. Wer nur lesen will, drückt "Fertig gelesen". Eine erzwungene
Prüfung nach jedem Text macht aus einem Nachschlagewerk eine Schulstunde, und dann wird nichts
mehr nachgeschlagen. Aus demselben Grund ist die Kursreihenfolge eine Empfehlung und keine
Sperre: wer in der Fahrschule gerade Vorfahrt hat, soll Vorfahrt lesen können, ohne sich vorher
durch dreizehn andere zu klicken.

### Rechtliches gilt hier genauso

Alle Kurstexte sind selbst geschrieben und geben StVO und StVZO in eigenen Worten wieder. Für
Erklärseiten gilt dasselbe wie für die Fragen: der amtliche Fragenkatalog gehört der TÜV|DEKRA
arge tp 21 und kommt nicht in die App. Videos gibt es keine - LernTor ist vollständig offline,
und die Zeichenbilder liegen ohnehin schon im Programm.

## Eltern-Einstellungen (pro Profil)

| Einstellung | Standard | Wirkung |
|---|---|---|
| Bereich anzeigen | an | Aus = die Etappe wird übersprungen |
| Zeichen in der Challenge | 5 | Presets 3/5/8/10 |
| Zeichengruppen | alle fünf | Einzeln abwählbar; abgewählte kommen weder im Quiz noch in der Challenge vor |
| Verkehrszeichen-Lernstand zurücksetzen | — | Nur die Verkehrszeichen; Sterne und übriger Fortschritt bleiben |
| Theorie-Lernstand zurücksetzen | — | Gelernte Fragen und Prüfungshistorie; die Zeichen bleiben unangetastet |
| Kurs-Lernstand zurücksetzen | — | Lektionen wieder auf ungelesen; die Fragen bleiben, weil die Kontrollen in denselben Fragen-Lernstand zählen |

Zusätzlich gibt es den globalen Fächer-Schalter (`AppSettings.DisabledSubjects`), der wie bei
allen Fächern für beide Kinder zugleich gilt. **Beide Schalter zählen: aus ist aus.**

Sind alle Gruppen abgewählt, fällt der Bereich auf den vollen Katalog zurück — ein leerer
Bildschirm, aus dem man nicht mehr herauskommt, wäre die schlechtere Antwort.

## Persistenz

`TrafficSignProgressEntity` (Tabelle `TrafficSignProgress`), Schlüssel `ProfileId|SignNumber`.
Die **Zeichennummer ist der Schlüssel des Lernstands** — sie darf sich nicht ändern, sonst
verlieren die Kinder ihren Fortschritt.

`TheoryAnswerEntity` (Tabelle `TheoryAnswers`), Schlüssel `ProfileId|QuestionId`, und
`TheoryExamRunEntity` (Tabelle `TheoryExamRuns`) für die Prüfungsdurchläufe.

Zwei Dinge stehen dort bewusst **nicht** drin:

- **Das Sachgebiet einer Frage.** Es steht im Katalog und wird von dort geholt. Zweimal
  gespeichert hieße, dass beide Stellen auseinanderlaufen, sobald eine Frage umsortiert wird.
  Fragen, die es im Katalog nicht mehr gibt, fallen dadurch von selbst aus der Auswertung.
- **Bestanden/durchgefallen.** Wird aus den gespeicherten Zahlen neu berechnet. Ein gespeichertes
  Häkchen würde alte Läufe nach einer Regeländerung anders bewerten als die Zahlen daneben.

`CourseLessonProgressEntity` (Tabelle `CourseLessonProgress`), Schlüssel `ProfileId|LessonId`,
mit getrenntem `ReadAt` und `PassedAt` (siehe oben) und dem besten Versuch in `BestPercent`.

Am Profil kamen drei Spalten dazu. `DrivingAreaDisabled` ist bewusst **invertiert** benannt: der
additive Schema-Abgleich gibt neuen Spalten in bestehenden Zeilen `DEFAULT 0`. Bei einem Feld
`DrivingAreaEnabled` wäre der Bereich damit für alle vorhandenen Profile stillschweigend AUS
gewesen.

## Erweitern

Ein Zeichen mehr: Bilddatei (PNG, gemeinfrei, z.B. von Wikimedia Commons) unter
`src/LernTor.App/Assets/Verkehrszeichen/<nummer>.png` ablegen, Quelle und Lizenz in `QUELLEN.md`
im selben Ordner eintragen, dazu ein Eintrag in der passenden `TrafficSignCatalog.*`-Datei -
fertig, `TrafficSignImages` findet die Datei automatisch über die Nummer. Zeigt die Bilddatei
schon eine Aufschrift (Zahl, Ort), zusätzlich die Nummer in
`TrafficSignImages.BildEnthaeltAufschrift` eintragen, sonst steht der Text doppelt.

`TrafficSignRenderTests` (UiTests) lässt **WPF selbst** jeden Pfad des Rückfallpfads parsen und
zeichnet den ganzen Katalog einmal durch - das ist der eigentliche Schutz für den Rückfallpfad:
in Core ist ein Pfad nur ein String, ein Tippfehler darin fällt dort nicht auf. Ein zweiter Test
im selben Projekt hält fest, dass jede Katalognummer eine Bilddatei hat und keine Bilddatei ohne
Katalogeintrag herumliegt.
