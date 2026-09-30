# LernTor 3.0: Design, Technik, Umgebung, Funktion – Plan

Stand: 30.09.2026. Dieser Plan beschreibt die nächste große Version und den Fahrplan danach. Er
schließt an [`NAECHSTES-LEVEL.md`](NAECHSTES-LEVEL.md) (Version 2.0, umgesetzt) an; die
laufende Arbeitsliste steht in [`NAECHSTE-SCHRITTE.md`](NAECHSTE-SCHRITTE.md).

Wie bei 2.0 gilt: **jeder Schritt einzeln** bauen, lokal testen (die ganze Solution baut unter
Linux, siehe `CLAUDE.md`), einchecken und von der CI auf `windows-latest` bestätigen lassen. Ein
roter Schritt wird repariert, bevor der nächste beginnt. **Dieser Plan liegt auf GitHub, bevor
die Umsetzung beginnt**, und wird nach jedem Schritt im Abschnitt „Stand der Umsetzung“
nachgeführt.

### Stand der Umsetzung

| Schritt | Stand |
|---|---|
| 0 — Release 2.1.0 (Antwortprüfung, +660 Fragen) | ⏳ Release-Lauf gestartet 30.09.2026; Version in `Directory.Build.props`/`setup.iss` auf 2.1.0 |
| 1 — Sehen, was man baut (Bildschirmfotos, Kontrast, Regeln) | 🔨 Bildschirmfotos in der CI (`DesignScreenshotTests`, Artefakt „Design-Vorschau“); Kontrasttest und Preflight-Regeln folgen mit Schritt 2, weil sie die Design-Tokens brauchen |
| 2 — Design-Fundament | offen |
| 3 — Acht Designs | offen |
| 4 — Design-Auswahl für die Kinder | offen |
| 5 — Designs als Belohnung | offen |
| 6 — Entwicklungsumgebung | offen |
| 7 — Dokumentation | offen |
| 8 — Release 3.0.0 | offen |

---

## Ausgangslage

Version 2.0 hat das Fundament modern gemacht: .NET 10 LTS, zentrale Paketverwaltung, CI unter
Windows und Linux, Release per Knopfdruck, Meisterschaft je Thema, 25 Abzeichen. Am 30.09.2026
kamen 660 Fragen dazu, und drei Fehler in der Antwortprüfung wurden behoben (offene Antworten
wurden mit „enthält“ geprüft, die binomischen Formeln hatten falsche Lösungen, die
Groß-/Kleinschreibung wurde nicht geprüft).

Was fehlt, sieht man sofort: **LernTor hat genau ein Aussehen.** Alle 30 Ansichten verwenden
dieselben neun Farben, fest eingebunden (297 `StaticResource`-Verweise), eine Schriftart und
feste Schriftgrößen (505 Stellen). Es gibt keinen Dunkelmodus, keine größere Schrift für
müde Augen und nichts, was die Kinder an „ihrer“ App selbst einstellen können – außer dem
Avatar-Emoji.

Der Plan für 2.0 hat einen Neuanstrich bewusst ausgeschlossen: *„Das Aussehen lässt sich hier
nicht am Bildschirm prüfen.“* Das stimmt weiterhin – die Entwicklung läuft ohne Windows-Bildschirm.
Deshalb beginnt 3.0 **nicht** mit Farben, sondern damit, das Aussehen prüfbar zu machen
(Schritt 1). Erst danach wird umgebaut.

## Was wir uns von anderen Apps abschauen

| Vorbild | Was dort gut ist | Was LernTor davon übernimmt |
|---|---|---|
| **Duolingo** | Dunkelmodus, Design folgt dem System | Design „Nacht“, **„wie Windows“** und **„abends automatisch dunkel“** |
| **ANTON** | Kinder gestalten ihren Bereich selbst (Avatar, Hintergründe), Belohnungen schalten etwas frei | **Design-Galerie** je Kind, **Designs als Belohnung** für Abzeichen |
| **Windows 11 / Fluent** | Einheitliche Design-Tokens, Kontrast-Designs für Barrierefreiheit | **Design-Tokens** statt fester Farben, Design **„Hoher Kontrast“** |
| **Kindle / Apple Bücher / Leserlich-Modi** | Schriftart und Textgröße wählbar, „gut lesbare“ Schrift | **Schriftwahl** (Standard, Gut lesbar, Verspielt) und **Textgröße** (100/110/120 %) |
| **WCAG 2.2** (Barrierefreiheitsstandard) | Mindestkontrast 4,5 : 1 für Text | **Automatischer Kontrasttest** für jedes Design |
| **Storybook / Chromatic** (Web-Entwicklung) | Jede Ansicht in jedem Design als Bild, bei jeder Änderung | **Bildschirmfotos aller Designs in der CI** |

**Bewusst nicht übernommen:**

- **Keine Vereins-, Marken- oder Länderdesigns** (keine Galatasaray-, Hertha- oder Flaggenfarben
  mit Namen). Die Designs heißen nach Natur und Stimmung.
- **Keine heruntergeladenen Schriftdateien.** Nur Schriften, die jedes Windows 10/11 mitbringt
  (Segoe UI, Verdana, Comic Sans MS). Kein Lizenzthema, kein Netzzugriff.
- **Kein Umbau der Layouts.** 3.0 ändert Farben, Schrift und Größe – nicht, wo was steht. Die
  Bedienung, an die die Kinder gewöhnt sind, bleibt.
- **Nichts, was verfällt** (wie bei 2.0): ein freigeschaltetes Design bleibt freigeschaltet.
- **Keine Cloud, keine Konten, keine Werbung, kein Tracking.** Die Design-Wahl liegt in der
  lokalen Datenbank beim Profil.

---

## Schritt 0 — Release 2.1.0

Bevor 3.0 beginnt, bekommen die Kinder die Korrekturen vom 30.09.2026: die strengere
Antwortprüfung (sonst lässt sich weiter raten), die richtigen binomischen Formeln (sonst bekommt
Batuhan für richtige Rechnungen Fehler) und die 660 neuen Fragen.

- Version 2.1.0 in `Directory.Build.props` und `setup.iss`, Release über
  Actions → Release → Run workflow.
- Fertig, wenn: das Release mit ZIP und Prüfsumme auf GitHub liegt.

## Schritt 1 — Sehen, was man baut

Die Voraussetzung für alles Weitere. Ohne diesen Schritt wäre jeder Farbumbau ein Blindflug.

1. **Bildschirmfotos in der CI.** Ein neuer UI-Test rendert die wichtigsten Ansichten
   (Profilwahl, Startseite, Übung mit Frage, Geschafft-Bildschirm, Mein Fortschritt) mit
   Beispieldaten in **1366 × 768** (der kleinste Bildschirm, für den LernTor gebaut ist) als PNG –
   zuerst im heutigen Aussehen, ab Schritt 3 in jedem Design und jeder Textgröße. Die CI lädt die
   Bilder als Artefakt **„Design-Vorschau“** hoch. Die Familie kann sie unter Actions ansehen,
   ohne etwas zu installieren.
2. **Kontrasttest.** Jedes Design ist ein Datensatz in `LernTor.Core`. Ein Unit-Test rechnet für
   jede Kombination aus Text- und Hintergrundfarbe das Kontrastverhältnis nach WCAG 2.2 aus:
   mindestens **4,5 : 1** für Text, **3 : 1** für große Schrift und Bedienelemente. Ein Design,
   das das nicht schafft, kommt nicht durch die CI.
3. **Regeln in `scripts/preflight.py`:** keine `StaticResource` auf einen Design-Pinsel (sonst
   wechselt diese Stelle die Farbe nicht mit) und keine fest eingetragenen Farbwerte (`#RRGGBB`)
   in Ansichten.

Fertig, wenn: die CI ein Artefakt mit Bildern erzeugt und die Tests grün sind.

## Schritt 2 — Design-Fundament

1. **Design-Tokens statt Farben.** `Colors.xaml` beschreibt heute Farben („Lila“), künftig
   Rollen: Hintergrund, Fläche, Text, gedämpfter Text, Primär, Primär dunkel, Akzent, Erfolg,
   Fehler, vier Kachelfarben, Rahmen, Fortschrittsspur, Fächerfarben. Die Schlüssel bleiben gleich
   (`PrimaryBrush` usw.), damit keine Ansicht umbenannt werden muss.
2. **Alle Verweise dynamisch.** Die 297 `StaticResource`-Verweise auf Design-Pinsel werden zu
   `DynamicResource`. Dann ändert sich die Farbe auch in Ansichten, die schon offen sind.
3. **`ThemeService`** (App) legt das gewählte Design als letztes Wörterbuch in
   `Application.Resources` und tauscht es zur Laufzeit. Die Designdaten kommen aus Core, damit
   Kontrasttest und App dieselben Werte sehen.
4. **Schriftart als Ressource** (`AppFontFamily`) statt „Segoe UI“ in jedem Stil.
5. **Textgröße** über eine Skalierung des ganzen Fensterinhalts (100/110/120 %) statt über 505
   einzelne Schriftgrößen. Die Bildschirmfotos zeigen, ob bei 120 % auf 1366 × 768 etwas
   abgeschnitten wird.

Fertig, wenn: das heutige Aussehen pixelgleich bleibt (Vergleich der Bildschirmfotos aus Schritt
1) und ein Umschalten im Test die Farben einer offenen Ansicht ändert.

## Schritt 3 — Acht Designs

| Design | Stimmung | Frei |
|---|---|---|
| **Lavendel** | das heutige Aussehen (Standard) | ✅ |
| **Ozean** | Blau- und Türkistöne | ✅ |
| **Wald** | Grüntöne, ruhig | ✅ |
| **Sonnenuntergang** | Orange und Koralle, warm | ✅ |
| **Bonbon** | Rosa und Mint, verspielt | ✅ |
| **Nacht** | dunkel, augenschonend am Abend | ✅ |
| **Hoher Kontrast** | Schwarz/Weiß/Gelb, für schlechte Sicht und helle Räume | ✅ |
| **Galaxie** | dunkel mit Violett und Gold | 🔒 Belohnung (Schritt 5) |

Jedes Design mit deutschem und türkischem Namen, jedes durch den Kontrasttest, jedes auf den
Bildschirmfotos.

## Schritt 4 — Design-Auswahl für die Kinder

- **🎨-Knopf auf der Startseite** öffnet eine Galerie: jede Karte zeigt eine kleine Vorschau
  (Hintergrund, Karte, Knopf, Text) im jeweiligen Design. Ein Klick wechselt sofort.
- **Schrift:** Standard (Segoe UI), Gut lesbar (Verdana), Verspielt (Comic Sans MS).
- **Textgröße:** 100 %, 110 %, 120 %.
- **Automatisch:** „wie Windows“ (folgt dem hellen/dunklen Modus des Systems) und „abends
  automatisch dunkel“ (ab 19 Uhr Design „Nacht“).
- Alles **pro Profil** gespeichert (eigene Spalten, eigene Speichermethode im Repository wie
  `SetPinnedReadingTextAsync` – nicht über `ProfileSettings`, weil das Kind es selbst einstellt).
  Beim Profilwechsel wird das Design des Kindes geladen; die Profilwahl selbst zeigt das
  Standard-Design.
- **Eltern-Bereich:** zeigt je Profil das gewählte Design und kann es zurücksetzen.
- Deutsch und Türkisch, wie alle Texte.

## Schritt 5 — Designs als Belohnung

Anlehnung an ANTON: Abzeichen schalten etwas frei, das man sieht. **Galaxie** wird mit einem
bestehenden Abzeichen für Ausdauer freigeschaltet (welches, steht nach Durchsicht von
`AchievementCatalog` im Umsetzungs-Commit). Freigeschaltet bleibt freigeschaltet – die Regel
„nichts verfällt“ gilt weiter. Gesperrte Designs zeigen in der Galerie ein Schloss und den
Satz, wie man sie bekommt.

## Schritt 6 — Entwicklungsumgebung

Damit jede neue Sitzung – ob Mensch oder KI – sofort bauen und testen kann:

1. **SessionStart-Hook** für Claude Code im Web (`.claude/settings.json` +
   `scripts/session-start.sh`): installiert `dotnet-sdk-10.0` aus dem Ubuntu-Archiv, wenn es
   fehlt, und stellt die Pakete wieder her. Bisher musste das in jeder Sitzung von Hand passieren.
2. **Devcontainer** (`.devcontainer/devcontainer.json`) für VS Code und GitHub Codespaces mit
   .NET 10 und Python für die Prüfskripte.
3. **`.editorconfig`**: Einrückung, Zeilenenden und Namensregeln, wie sie im Code schon gelten.

## Schritt 7 — Dokumentation

- **Neu: `docs/DESIGN.md`** – die Tokens und ihre Bedeutung, wie man ein Design hinzufügt, welche
  Kontrastwerte gelten, wo die Bildschirmfotos liegen.
- **`CLAUDE.md`**: Design-Regeln (nur `DynamicResource` auf Design-Pinsel, keine Hex-Farben in
  Ansichten, neue Designs nur mit Kontrasttest).
- **`TESTPLAN.md`**: Sichtprüfung der Designs am echten Bildschirm.
- **`README.md`**, **`NAECHSTE-SCHRITTE.md`**, **`BUILD.md`** (Umgebung), dieser Plan (Stand).

## Schritt 8 — Release 3.0.0

Version 3.0.0, Release über den Knopf in Actions, Testplan für die Familie.

---

## Risiken und wie der Plan ihnen begegnet

| Risiko | Gegenmaßnahme |
|---|---|
| Eine Ansicht sieht in einem Design kaputt aus, und niemand merkt es | Bildschirmfotos jeder Ansicht in jedem Design (Schritt 1) |
| Text ist auf einem Hintergrund schlecht lesbar | Kontrasttest nach WCAG 2.2, sonst rote CI (Schritt 1) |
| Eine Stelle wechselt die Farbe nicht mit | Preflight-Regel gegen `StaticResource` auf Design-Pinsel und gegen Hex-Farben |
| Bei 120 % Textgröße rutscht der Freischalten-Knopf aus dem Bild | Bildschirmfotos in 1366 × 768 bei 120 %; der Geschafft-Bildschirm scrollt seit 2.0 |
| Ein XAML-Laufzeitfehler (z. B. `DynamicResource` an einer Stelle, die ihn nicht erlaubt) | `XamlLoadTests` und Render-Tests mit Daten (siehe `CLAUDE.md`, „item templates“) |
| Das Kind stellt sich ein unlesbares Design ein | Es gibt keine unlesbaren Designs (Kontrasttest); Eltern können zurücksetzen |

---

## Fahrplan danach (Zukunft)

| Wann | Was | Warum |
|---|---|---|
| **Oktober 2026** | Version 3.0 (dieser Plan) | Design, Barrierefreiheit, Umgebung |
| **nach 3–4 Wochen Betrieb** | Auswertung: Tageslänge, Abschlussquiz-Schwelle, Fehlerprotokoll | Entscheidet, ob ein Zeitbudget in Minuten nötig ist ([`NAECHSTE-SCHRITTE.md`](NAECHSTE-SCHRITTE.md), Abschnitt 3) |
| **vor dem 10.11.2026** | 2.1.0 oder 3.0 installiert | .NET 8 läuft aus (erledigt durch 2.0, nur Installation fehlt) |
| **Winter 2026/27** | Fragen gegenlesen lassen (Türkisch, Englisch), Lehrplan-Lücken schließen: Biologie Kl. 5/6 (Wasserkreislauf, Pflanzen/Tiere, Ernährung), Mathe Kl. 9 Wahrscheinlichkeit | Qualität vor Menge |
| **Frühjahr 2027** | Ferien 2027/28 und Feiertage 2028 in `SchoolCalendar.cs` | Der Kalender endet mit den Sommerferien 2027 |
| **vor Sommer 2027** | Klasse-7-Pools nach Emirhans neuem Stundenplan prüfen (Kunst 80, Politik 100 Fragen) | Klassenwechsel |
| **Sommer 2027** | Klassenstufen wechseln, neue Stundenpläne eintragen | Schuljahr 2027/28 |
| **offen, Entscheidung der Familie** | Elternbericht als PDF, Modul „Gaming & Creator“, Auto-Update mit signiertem Installer | siehe [`NAECHSTE-SCHRITTE.md`](NAECHSTE-SCHRITTE.md), Abschnitt 6 |
| **November 2028** | Wechsel auf .NET 12 LTS | .NET 10 läuft aus; Dependabot meldet sich vorher |
