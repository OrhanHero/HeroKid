# LernTor 3.1: Funktion – Plan

Stand: 30.09.2026. Dieser Plan schließt an [`NAECHSTES-LEVEL-3.md`](NAECHSTES-LEVEL-3.md) an.
Version 3.0 brachte Design, Technik und Umgebung. Die Familie hat sie am 30.09.2026 installiert
und zurückgemeldet: „alles funktioniert“. Version 3.1 bringt den vierten Teil des Auftrags vom
30.09.2026, **Funktion**: Dinge, die die Kinder beim Lernen jeden Tag bemerken.

Es gelten dieselben Regeln wie bei 2.0 und 3.0:
- **Jeder Schritt einzeln:** bauen, lokal testen, einchecken und von der CI auf `windows-latest` bestätigen lassen. Ein roter Schritt wird repariert, bevor der nächste beginnt.
- **Erst der Plan:** Er liegt auf GitHub, bevor die Umsetzung beginnt.
- **Laufender Stand:** Nach jedem Schritt wird der Abschnitt „Stand der Umsetzung“ nachgeführt.

### Stand der Umsetzung

| Schritt | Stand |
|---|---|
| 1 — 🔊 Jede Frage vorlesen | ⏳ geplant |
| 2 — 📅 Lernkalender in „Mein Fortschritt“ | ⏳ geplant |
| 3 — Zwei neue Designs als Belohnung | ⏳ geplant |
| 4 — Lehrplan-Lücken schließen | ⏳ geplant |
| 5 — Schulkalender 2027/28 | ⏳ geplant |
| 6 — Dokumentation, Version 3.1.0, Release | ⏳ geplant |

---

## Ausgangslage

- **Vorlesen gibt es schon, aber nur an zwei Stellen.** `TextToSpeechService` liest offline vor: mit den Piper-Stimmen für Deutsch, Türkisch und Englisch (einmaliger Download im Eltern-Bereich), sonst mit der Windows-Stimme. Genutzt wird das nur für die Etappe „Vorlesen“ und für Diktate. Eine Übungsfrage vorlesen lassen kann ein Kind nicht, und es kann auch nicht hören, wie der englische Satz richtig klingt.
- **„Mein Fortschritt“ zeigt, *was* ein Kind kann, aber nicht, *wann* es gelernt hat.** Die Lerntage werden nur für die Abzeichen gezählt.
- **Nur ein Design ist eine Belohnung** (Galaxie mit „📅 Zehn Lerntage“). Die übrigen 24 Abzeichen schalten nichts frei, das man sieht.
- **Lehrplan-Lücken** aus dem Fahrplan von 3.0:
  - Mathe Klasse 9 hat keine mehrstufigen Zufallsversuche (Baumdiagramm, Pfadregeln).
  - Biologie Klasse 6 hat weder „Ernährung und Verdauung“ noch „Blütenpflanzen“.
  - In Klasse 7 sind Kunst (80 Fragen) und Politik (100 Fragen) die dünnsten Pools.
- **Der Schulkalender endet mit den Sommerferien 2027.** Danach kennt die Fächerwahl nach Stundenplan keine Ferien mehr.

## Was bewusst nicht dazukommt

- **Keine Lernserie („Streak“), die reißen kann.** Der Lernkalender zählt Tage, die bleiben. Ein verpasster Tag nimmt nichts weg. Das ist dieselbe Regel wie bei den Abzeichen („nichts verfällt“).
- **Keine Online-Stimmen.** Vorgelesen wird nur mit Stimmen, die auf dem PC liegen. Es gibt keinen neuen Netzzugriff.
- **Kein Auto-Update und kein Modul „Gaming & Creator“.** Beides bleibt eine Entscheidung der Familie ([`NAECHSTE-SCHRITTE.md`](NAECHSTE-SCHRITTE.md), Abschnitt 6).
- **Keine Änderung an Bedienung und Layout,** außer den neuen Knöpfen und dem Kalender.

---

## Schritt 1 — 🔊 Jede Frage vorlesen

- **Ein 🔊-Knopf an jeder Frage** in Übung, Nachrichten und Abschlussquiz. Er liest die Frage und die Antwortmöglichkeiten vor. Ein zweiter Klick hält an. Beim Weiterblättern hört das Vorlesen auf.
- **Die richtige Sprache für jeden Teil.** Die Englisch- und Türkischfragen sind gemischt, zum Beispiel: *Setze die richtige Form ein: „She ___ to school every day.“* Die deutsche Anweisung liest die deutsche Stimme, den englischen Satz die englische.
  - `SpeechSegmenter` (Core) zerlegt dafür den Text in Abschnitte mit Sprache: was in Anführungszeichen steht, gehört zur Sprache des Fachs, und für den Rest entscheiden typische Wörter.
  - Ein Test läuft über alle Generatoren. Er prüft, dass kein Abschnitt leer ist, dass keine Anführungszeichen mitgelesen werden und dass bekannte Beispiele in der richtigen Sprache landen.
- **Nach dem Antworten** liest 🔊 an der Lösung die richtige Antwort vor. So hört ein Kind, wie der englische oder türkische Satz richtig klingt.
- **Nur mit passender Stimme.** Fehlt für eine Sprache jede Stimme (weder Piper noch Windows), erscheint der Knopf bei dieser Frage nicht. Eine deutsche Stimme, die Englisch vorliest, brächte falsche Aussprache bei.
- **Diktate bleiben, wie sie sind.** Dort ist das Vorlesen die Aufgabe, und der Satz darf nicht sichtbar werden.
- Fertig, wenn:
  - Segmenter-Tests und Render-Test sind grün.
  - Der Knopf ist auf den Bildschirmfotos zu sehen.
  - Der Testplan hat einen Punkt zum Anhören am echten PC.

## Schritt 2 — 📅 Lernkalender in „Mein Fortschritt“

- **Die letzten 12 Wochen als Kalender** (Mo–So):
  - Tage mit mindestens einer Antwort sind in der Erfolgsfarbe markiert.
  - Schulfreie Tage (Wochenende, Feiertag, Ferien aus `SchoolCalendar`) sind hell.
  - Künftige Tage bleiben leer.
- **Darüber steht ein Satz:** „42 Lerntage insgesamt · 11 in den letzten vier Wochen“. Es gibt keine Serie und keinen roten Tag.
- **Die Berechnung liegt in Core:** `LearningCalendar` rechnet aus denselben Antworten, die schon für die Abzeichen geladen werden.
- **Tests:** Unit-Tests mit festen Daten und ein Render-Test mit Daten (siehe `CLAUDE.md`, „item templates“). Dazu kommt ein Bildschirmfoto in jedem Design.

## Schritt 3 — Zwei neue Designs als Belohnung

| Design | Stimmung | Frei mit |
|---|---|---|
| **🧊 Gletscher** | hell, kühles Eisblau | „🚀 Fünfhundert richtig“ (`richtig-500`) |
| **🌋 Vulkan** | dunkel, Lava-Orange | „🏆 Erstes gemeistertes Thema“ (`meister-1`) |

- Beide bestehen den Kontrasttest (WCAG 2.2 AA) und erscheinen auf den Bildschirmfotos.
- Die Galerie zeigt bei gesperrten Designs, wie man sie bekommt. Das funktioniert seit 3.0.
- Die Ids werden nie umbenannt, denn sie sind beim Profil gespeichert.

## Schritt 4 — Lehrplan-Lücken schließen

| Fach | Neu | Pool vorher → nachher |
|---|---|---|
| Mathe Kl. 9 | Mehrstufige Zufallsversuche: Baumdiagramm, Pfadregeln (rechnet mit frischen Zahlen) | Thema neu |
| Biologie Kl. 6 | „Ernährung und Verdauung“, „Blütenpflanzen“ (je 20) | 120 → 160 |
| Kunst Kl. 7 | zwei Themen je 20 | 80 → 120 |
| Politik Kl. 7 | zwei Themen je 20 | 100 → 140 |

Die Qualitätssicherung ist dieselbe wie bei allen Fragen:
- Der Längen-Bias-Wächter muss bestehen.
- Offene Antworten gehen durch den Schrotflinten-Test von `OpenTextAnswerMatcherTests`.
- `PoolReichweiteTests` hält die neuen Poolgrößen fest.
- `CURRICULUM.md` wird nachgeführt.

## Schritt 5 — Schulkalender 2027/28

Die Ferientermine hat die Senatsverwaltung für Bildung, Jugend und Familie veröffentlicht („Ferientermine“ auf berlin.de):

| Was | Von | Bis |
|---|---|---|
| Herbstferien | Mo 11.10.2027 | Sa 23.10.2027 |
| Weihnachtsferien | Mi 22.12.2027 | Fr 31.12.2027 |
| Winterferien | Mo 31.01.2028 | Sa 05.02.2028 |
| Osterferien | Mo 10.04.2028 | Sa 22.04.2028 |
| Unterrichtsfreier Tag | Fr 26.05.2028 | |
| Pfingstferien | Do 01.06.2028 | Fr 02.06.2028 |
| Sommerferien | Sa 01.07.2028 | Sa 12.08.2028 |

- **Feiertage 2028:**
  - Neujahr, Internationaler Frauentag (8.3.) und Tag der Arbeit
  - Karfreitag 14.4., Ostermontag 17.4., Christi Himmelfahrt 25.5. und Pfingstmontag 5.6. (Ostersonntag ist der 16.4.2028)
  - Tag der Deutschen Einheit sowie 1. und 2. Weihnachtsfeiertag
- **Nachprüfen:** Die Seite war aus der Entwicklungsumgebung nicht direkt abrufbar. Die Termine stammen aus den Suchergebnissen von berlin.de und stimmen mit einer zweiten Quelle und den Wochentagen überein. Die Familie gleicht sie einmal mit berlin.de ab (Testplan).
- **Tests:** Sie prüfen die neuen Einträge und dass der Kalender lückenlos bis zu den Sommerferien 2028 reicht.

## Schritt 6 — Dokumentation, Version 3.1.0, Release

- `README.md`, `NAECHSTE-SCHRITTE.md`, `TESTPLAN.md` (neue Punkte V.3.1-x), `DESIGN.md` (zehn Designs), `CURRICULUM.md`, `CLAUDE.md` (Vorlesen: Sprachabschnitte), dieser Plan.
- Version 3.1.0 in `Directory.Build.props` und `setup.iss`.
- Nach grüner CI und dem Merge: Release über Actions → Release → Run workflow.

---

## Risiken und wie der Plan ihnen begegnet

| Risiko | Gegenmaßnahme |
|---|---|
| Die deutsche Stimme liest Englisch vor und bringt falsche Aussprache bei | Knopf nur, wenn für jeden Abschnitt eine passende Stimme da ist |
| Das Vorlesen läuft nach dem Weiterblättern weiter | Anhalten beim Fragenwechsel und beim Etappenwechsel (wie beim Diktat) |
| Der Segmenter zerlegt eine Frage falsch | Test über alle Generatoren; im schlimmsten Fall klingt ein Abschnitt in der falschen Sprache, die Frage bleibt lösbar |
| Der Kalender macht Druck („Serie gerissen“) | Es gibt keine Serie, nur gezählte Tage; schulfreie Tage sind nicht „verpasst“ |
| Ein neues Design ist schlecht lesbar | Kontrasttest, sonst rote CI; Bildschirmfotos |
| Ein falscher Ferientermin | Quelle genannt, Test auf Wochentage, Abgleich durch die Familie |
