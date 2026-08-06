# Führerschein Klasse B

Eigener Lernbereich mit drei Unterbereichen, angelehnt an den Aufbau von fahrschule24.de.
Erreichbar als eigene Etappe (`LearningStage.Fuehrerschein`) zwischen KI-Bereich und
Abschlussquiz.

| Unterbereich | Stand | Inhalt |
|---|---|---|
| **Verkehrszeichen** | ✅ fertig | 77 Zeichen in fünf Gruppen (42 im Original, 35 nachgezeichnet), Karteikarten und Quiz, tägliche Challenge |
| **Theoriefragen** | 🔜 Stufe 2 | Eigene Fragen zu den 14 amtlichen Sachgebieten, Prüfungssimulation, Schwachstellen-Trainer |
| **Theorie-Kurs** | 🔜 Stufe 3 | Erklärseiten mit Zeichnungen, Quiz, Lernstandskontrolle |

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

**Was übernommen ist und was nicht.** Aus der ADAC-Übersicht ist die **Zeichengeometrie der
Schilder selbst** entnommen — also genau das, was als amtliches Werk gemeinfrei ist. Nicht
entnommen sind Layout, Satz, Erklärtexte und die Zusammenstellung der Broschüre; das ist die
Leistung des Herausgebers und durch dessen Copyright-Vermerk geschützt. Der Extraktor liest
ausschließlich Pfad- und Farboperatoren, keine Schrift und keine Seitengestaltung.

Die Erklär- und Merktexte in diesem Bereich sind selbst geschrieben. Die 35 noch nicht
bestätigten Zeichen sind weiterhin aus der Verordnungsbeschreibung nachgezeichnet
(`SignPictograms`).

**Der amtliche Fragenkatalog ist NICHT frei.** Die offiziellen Theorie-Prüfungsfragen gehören der
TÜV|DEKRA arge tp 21; kommerzielle Lern-Apps lizenzieren sie. Sie dürfen hier nicht hinein.
Stufe 2 bringt deshalb **eigene** Fragen zu denselben 14 amtlichen Sachgebieten aus StVO und
StVZO — zum Lernen gleichwertig, aber nicht wortgleich mit der Prüfung. Wer wortgleiche Fragen
will, muss sie über den vorhandenen Eltern-Import selbst eintragen.

**Keine Videos.** LernTor ist vollständig offline. Der Theorie-Kurs bekommt stattdessen
bebilderte Erklärseiten mit denselben gezeichneten Zeichen.

## Original oder nachgezeichnet

**42 der 77 Zeichen liegen im Original vor**, aus der amtlichen Übersicht gewonnen
(`scripts/extract-signs-from-pdf.py`, Ergebnis in `TrafficSignArtwork.cs`). Sie tragen die
echten RAL-Verkehrsfarben aus der Vorlage: `#E3000F` Verkehrsrot, `#005DAA` Verkehrsblau,
`#FFED00` Verkehrsgelb. Die übrigen 35 behalten ihre nachgezeichnete Fassung.

**Warum nicht alle?** Die Zuordnung Bild→Name läuft über die Lesereihenfolge der Vorlage
(Bildraster links, Namensliste rechts) — und die verrutscht stellenweise. Aufgenommen wird
deshalb nur, was zwei maschinelle Prüfungen besteht:

1. Die äußere Kontur passt zur erwarteten Grundform (Flächeninhalt im Verhältnis zum
   umschließenden Rechteck: Dreieck ≈ 0,5, Kreis ≈ 0,79, Rechteck ≈ 1,0).
2. Die erwartete Randfarbe kommt im Zeichen vor.

Das ist keine Förmlichkeit. Beim **Wendeverbot (VZ 272)** kam ein *blaues* Schild heraus, wo ein
roter Kreis stehen muss; beim **Überholverbot (VZ 276)** fehlte jedes Rot. Diese Fälle sind
aussortiert und behalten ihre gezeichnete Fassung — lieber ein vereinfachtes richtiges Schild
als ein originalgetreues falsches. `TrafficSignArtworkTests` hält fest, dass sie nicht
stillschweigend zurückkehren.

**Eine Falle beim Extrahieren:** der Extraktor liest nur Zeichenpfade, **keine Schrift**. Bei
VZ 108-10 („Gefälle 10 %") und VZ 274-50 („50") steht die Aussage aber in der Zahl — als
Original allein wären das ein leeres Dreieck und ein leerer roter Kreis. `TrafficSignVisual`
legt die Aufschrift deshalb über die Originalzeichnung.

**Wie mehr Zeichen dazukommen:** Prüfregeln in der Auswertung nachschärfen oder die
Fehlzuordnungen von Hand richtigstellen, dann das Skript erneut laufen lassen. Die Architektur
steht; es ist nur noch Datenpflege.

## Wie die nachgezeichneten Zeichen entstehen

Keine Bilddateien — die App kann nichts nachladen, und ein Ordner mit 77 PNGs wäre bei jeder
Änderung ein Binär-Diff.

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

## Eltern-Einstellungen (pro Profil)

| Einstellung | Standard | Wirkung |
|---|---|---|
| Bereich anzeigen | an | Aus = die Etappe wird übersprungen |
| Zeichen in der Challenge | 5 | Presets 3/5/8/10 |
| Zeichengruppen | alle fünf | Einzeln abwählbar; abgewählte kommen weder im Quiz noch in der Challenge vor |
| Lernstand zurücksetzen | — | Nur die Verkehrszeichen; Sterne und übriger Fortschritt bleiben |

Zusätzlich gibt es den globalen Fächer-Schalter (`AppSettings.DisabledSubjects`), der wie bei
allen Fächern für beide Kinder zugleich gilt. **Beide Schalter zählen: aus ist aus.**

Sind alle Gruppen abgewählt, fällt der Bereich auf den vollen Katalog zurück — ein leerer
Bildschirm, aus dem man nicht mehr herauskommt, wäre die schlechtere Antwort.

## Persistenz

`TrafficSignProgressEntity` (Tabelle `TrafficSignProgress`), Schlüssel `ProfileId|SignNumber`.
Die **Zeichennummer ist der Schlüssel des Lernstands** — sie darf sich nicht ändern, sonst
verlieren die Kinder ihren Fortschritt.

Am Profil kamen drei Spalten dazu. `DrivingAreaDisabled` ist bewusst **invertiert** benannt: der
additive Schema-Abgleich gibt neuen Spalten in bestehenden Zeilen `DEFAULT 0`. Bei einem Feld
`DrivingAreaEnabled` wäre der Bereich damit für alle vorhandenen Profile stillschweigend AUS
gewesen.

## Erweitern

Ein Zeichen mehr: Eintrag in der passenden `TrafficSignCatalog.*`-Datei, fertig. Braucht es ein
neues Piktogramm, kommt eine Konstante in `SignPictograms` dazu.

`TrafficSignRenderTests` (UiTests) lässt **WPF selbst** jeden Pfad parsen und zeichnet den ganzen
Katalog einmal durch. Das ist der eigentliche Schutz: in Core ist ein Pfad nur ein String, ein
Tippfehler darin fällt dort nicht auf.
