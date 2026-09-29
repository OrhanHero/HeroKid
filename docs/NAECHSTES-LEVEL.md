# LernTor aufs nächste Level: Plan

Stand: 29.09.2026. Dieser Plan beschreibt, was als Nächstes gebaut wird, damit LernTor modern
bleibt und die nächsten Jahre ohne Umbau übersteht. Er ergänzt die Arbeitsliste in
[`NAECHSTE-SCHRITTE.md`](NAECHSTE-SCHRITTE.md) und die Begründungen in [`PLAN.md`](PLAN.md).

Umgesetzt wird **in der Reihenfolge unten, jeder Schritt einzeln**: bauen, testen, einchecken und
von der CI auf `windows-latest` bestätigen lassen. Ein Schritt, der dort rot wird, wird repariert,
bevor der nächste beginnt.

### Stand der Umsetzung

| Schritt | Stand |
|---|---|
| 1 — .NET 10 LTS | ✅ umgesetzt 29.09.2026 |
| 2 — Fragenpools | ✅ umgesetzt 29.09.2026 (+140 Fragen) |
| 3 — Fehler-Kartei | ✅ umgesetzt 29.09.2026 (anders als geplant, siehe dort) |
| 4 — Meisterschaft je Thema | ✅ umgesetzt 29.09.2026 |
| 5 — Abzeichen | ✅ umgesetzt 29.09.2026 (25 Abzeichen) |
| 6 — Sicherheit und Wartbarkeit | ✅ umgesetzt 29.09.2026 |
| 7 — Betrieb und Weitergabe | ✅ umgesetzt 29.09.2026 |
| 8 — Dokumentation | ✅ umgesetzt 29.09.2026 |

Offen ist nur, was eine CI nicht kann: **die Familie muss es sehen** (unten und im
[Testplan](TESTPLAN.md#neu-seit-29092026-version-20)).

---

## Ausgangslage

LernTor ist schon weit: 17 Fächer mit rund 4.900 Fragen, Stundenplan-Auswahl, Wiederholung nach
Abständen (7/30/90 Tage), Fehler-Kartei, Sterne und Belohnungen, Wochenziel, Lernverlauf,
HTML-Elternbericht, Tipptrainer, Diktat, Führerschein, Erste Hilfe und ein lokales Sprachmodell.
910 Unit-Tests laufen grün.

Zwei Dinge drängen:

1. **.NET 8 läuft am 10.11.2026 aus.** Danach gibt es für die Laufzeit, auf der LernTor steht,
   keine Sicherheitsupdates mehr. Die App läuft auf einem Familien-PC mit Internet (News,
   Wetter, Modell-Download). Das ist in sechs Wochen.
2. **Einige Fragenpools reichen nicht über den Winter.** Englisch Klasse 6 reicht knapp 8 Wochen,
   Musik Klasse 6 gut 8, Englisch Klasse 9 genau 10 (gerechnet mit
   `scripts/pool-reichweite.py`). Danach kommen nur noch Wiederholungen.

Dazu fehlen Dinge, die gute Lern-Apps heute haben und die Kinder dort gewohnt sind.

## Was wir uns von anderen Lern-Apps abschauen

| Vorbild | Was dort gut ist | Was LernTor davon übernimmt |
|---|---|---|
| **ANTON** (Berliner Rahmenlehrplan, in Schulen verbreitet) | Abzeichen für echte Leistungen, sichtbarer Lernstand je Thema | **Abzeichen** (Schritt 5), **Meisterschaft je Thema** (Schritt 4) |
| **Khan Academy** | Vier Stufen je Fähigkeit: *versucht → vertraut → sicher → gemeistert* | **Meisterschaftsstufen** je Thema, für Kind und Eltern sichtbar |
| **Anki** | Karten kommen wieder, wenn sie fällig sind, und man sieht, wie viele heute fällig sind | **Fehler-Kartei-Zahl, die stimmt**: „Aus deiner Fehler-Kartei heute dran: 3“ (Schritt 3) |
| **Duolingo** | Kleine Erfolge feiern, Fortschritt ständig sichtbar | Abzeichen mit Feier auf dem Geschafft-Bildschirm |
| **sofatutor / simpleclub** | Eltern sehen auf einen Blick, wo das Kind steht | Meisterschaft je Fach im Elternbericht |

**Bewusst nicht übernommen**, weil es gegen die Grundsätze dieser App geht:

- **Keine Ranglisten und kein Vergleich unter Geschwistern.** Das bleibt eine Eltern-Entscheidung
  im Bericht, wie bisher.
- **Nichts, was verfällt.** Kein Serien-Verlust wie bei Duolingo, keine Leben, die ausgehen. Ein
  Abzeichen bleibt für immer; Sterne können nur wachsen oder eingelöst werden.
- **Keine Cloud, keine Konten, keine Werbung, kein Tracking.** Alles bleibt auf dem PC.
- **Kein kompletter Neuanstrich (Fluent-Design, Dunkelmodus).** Das Aussehen lässt sich hier nicht
  am Bildschirm prüfen. Ein Umbau aller 30 Ansichten ohne Sichtkontrolle wäre genau die
  Fehlerklasse, die bisher nur beim Hinsehen gefunden wurde (siehe `PLAN.md`, Phase 0).
- **Kein neuer Wiederholungs-Algorithmus (FSRS).** Die festen Abstände funktionieren; FSRS lohnt
  erst mit viel mehr Wiederholungen pro Frage, als ein Kind hier je sammelt.
- **Kein Auto-Update.** Das wäre der erste Netzzugriff der App außer News und Wetter und braucht
  eine Entscheidung der Familie. Vorbereitet wird nur der Weg dahin (siehe Schritt 7).

---

## Schritt 1 — Fundament: .NET 10 LTS

**Warum:** .NET 8 bekommt ab dem 10.11.2026 keine Sicherheitsupdates mehr. .NET 10 ist die
aktuelle Langzeitversion mit Unterstützung bis November 2028.

**Was:**
- Alle Projekte von `net8.0`/`net8.0-windows` auf `net10.0`/`net10.0-windows`.
- Alle Microsoft-Pakete auf die passende 10.0.x-Version (EF Core, Extensions, System.Speech,
  Syndication). `Microsoft.Win32.Registry` fällt weg, das ist seit Jahren im Framework enthalten.
- **Zentrale Paketverwaltung** (`Directory.Packages.props`): jede Paketversion steht genau einmal.
  Das verhindert die Art Versionskonflikt, die schon einmal die Wiederherstellung gebrochen hat
  (NU1605 bei `Microsoft.Extensions.Logging.Abstractions`).
- **Gemeinsame Build-Einstellungen** (`Directory.Build.props`): Nullable, ImplicitUsings,
  Sprachversion stehen einmal statt in acht Projektdateien.
- **`global.json`** legt die SDK-Version fest, damit ein Build in zwei Jahren noch dasselbe
  Ergebnis liefert.
- **Dependabot** prüft monatlich NuGet-Pakete und GitHub-Actions auf neue Versionen und
  Sicherheitslücken und schlägt Updates als Pull Request vor.
- **CI:** .NET 10, NuGet-Cache, und ein zusätzlicher schneller Linux-Lauf (Bauen und Unit-Tests in
  etwa zwei Minuten), der parallel zum vollständigen Windows-Lauf Rückmeldung gibt.

**Neu entdeckt:** Die ganze Solution, auch die WPF-App, lässt sich in der Linux-Entwicklungsumgebung
bauen (`dotnet-sdk-10.0` aus dem Ubuntu-Archiv, `-p:EnableWindowsTargeting=true`), und die 910
Unit-Tests laufen dort in fünf Sekunden. Bisher galt das als unmöglich; jeder Tippfehler kostete eine
CI-Runde von acht Minuten. `CLAUDE.md` und `BUILD.md` werden entsprechend angepasst. Nur die
UI-Tests brauchen weiterhin Windows.

**Fertig, wenn:** CI auf Windows grün ist (Build, 910+ Unit-Tests, UI-Smoke-Tests, Publish,
Prüfung der KI-Bibliotheken) und das Artefakt auf .NET 10 läuft.

**Risiko:** gering bis mittel. WPF und EF Core haben zwischen 8 und 10 kleine Brüche; die
UI-Smoke-Tests laden alle Ansichten und starten die echte EXE. Die Datenbank bleibt, wie sie ist:
das Schema wird nur ergänzt, nie umgebaut.

## Schritt 2 — Fragenpools für den Winter

**Warum:** Englisch und Musik Klasse 6 und Englisch Klasse 9 sind die ersten Fächer, die leer
laufen.

**Was:** mit dem Skill `fach-pool`, je Thema 20 Fragen mit Erklärung, Längen-Bias um 35 %:
- **Englisch Klasse 6: +60 Fragen** (drei Themen, Berliner Rahmenlehrplan Niveau A1/A2)
- **Musik Klasse 6: +40 Fragen** (zwei Themen)
- **Englisch Klasse 9: +40 Fragen** (zwei Themen, Niveau B1)

**Fertig, wenn:** `pool-reichweite.py` für alle drei mindestens 10 Wochen zeigt und ein Test die
Poolgröße festhält.

## Schritt 3 — Fehler-Kartei: eine Zahl, die stimmt (nach Anki)

> **Beim Umsetzen festgestellt:** Die Startseite zeigte die Fehler-Kartei schon („🔁 Von früher
> noch offen: 12“); der Punkt in `NAECHSTE-SCHRITTE.md` war veraltet. Die Zahl stimmte aber seit
> der Fächerauswahl nach Stundenplan nicht mehr: gezählt wurden alle offenen Einträge, auch in
> Fächern, die heute gar nicht dran sind, und ohne die Obergrenze von drei je Fach. Ein Kind las
> „12“ und bekam vier. Schritt 3 macht die Zahl deshalb **richtig**, statt eine zweite Zeile
> hinzuzufügen. Eine Zeile für fällige Wiederholungen nach Abstand (7/30/90 Tage) gibt es bewusst
> nicht: diese Fragen dürfen wieder kommen, werden aber nicht bevorzugt gezogen – eine Zahl dafür
> wäre ein Versprechen, das der Ablauf nicht hält.

**Was:** `ReviewForecast` (Core) zählt nur die Fächer, die heute geübt werden, und höchstens
`PerSubjectCap` (3) je Fach – dieselbe Konstante, mit der der Aufgabenablauf die Fragen zieht.
Die Zeile heißt jetzt „🔁 Aus deiner Fehler-Kartei heute dran: 4“.

**Fertig, wenn:** die Zählung per Test abgesichert ist und die Zeile nur erscheint, wenn es etwas
zu zeigen gibt (unverändert: `ShowDueReviews`).

## Schritt 4 — Meisterschaft je Thema (nach Khan Academy)

**Warum:** Das Kind sieht bisher nur Sterne und eine Trefferquote je Fach. Ob es ein Thema
*beherrscht*, sieht niemand.

**Was:** Eine Regel in Core (`TopicMastery`), die aus dem Aktivitätsprotokoll und den gemeisterten
Fragen für jedes Thema eine von vier Stufen ableitet:

| Stufe | Bedeutung |
|---|---|
| 🌱 Angefangen | weniger als 5 Antworten |
| 📘 Vertraut | genug Antworten, Quote unter 70 % |
| ✅ Sicher | Quote ab 70 % in den letzten Antworten |
| 🏆 Gemeistert | Quote ab 90 % **und** Fragen haben mindestens eine Wiederholung nach Abstand bestanden |

Angezeigt wird das in einer neuen Ansicht **„Mein Fortschritt“** (erreichbar von der Startseite)
und als Abschnitt im Elternbericht.

**Fertig, wenn:** die Stufenregel mit Grenzfällen getestet ist und die Ansicht in den
XAML-Ladetests steckt.

## Schritt 5 — Abzeichen (nach ANTON und Duolingo)

**Warum:** Sterne zählen nur. Abzeichen erzählen, *was* man geschafft hat: das erste gemeisterte
Thema, 100 richtige Mathe-Aufgaben, die leere Fehler-Kartei, alle Verkehrszeichen.

**Was:**
- Ein fester Katalog von rund 25 Abzeichen in Core, jedes mit einer prüfbaren Bedingung über
  Daten, die es schon gibt (Aktivitätsprotokoll, Quizversuche, Tipptrainer, Verkehrszeichen,
  Theorie, Fehler-Kartei, Lerntage).
- Eine neue Tabelle für freigeschaltete Abzeichen mit Datum. Sie gehört zum Profil und wird damit
  automatisch beim Löschen eines Profils und beim Zurücksetzen mit entfernt (die Tabellenlisten
  kommen seit dem 28.09. aus dem EF-Modell).
- **Nichts geht verloren:** ein Abzeichen wird nie wieder entzogen.
- Neue Abzeichen erscheinen auf dem Geschafft-Bildschirm („🏅 Neues Abzeichen!“), alle zusammen in
  „Mein Fortschritt“, gesperrte grau mit dem Hinweis, wie man sie bekommt.
- Abzeichen, die an Leistung hängen, nicht an Anwesenheit: keine Abzeichen für
  „30 Tage am Stück“.

**Fertig, wenn:** jede Bedingung einzeln getestet ist, das Freischalten doppelte Einträge verhindert
und die Tabelle im Sicherungs-/Zurücksetzen-Test mitläuft.

## Schritt 6 — Sicherheit und Wartbarkeit

- **Eltern-Passwort:** PBKDF2 mit 600.000 statt 210.000 Durchläufen (OWASP-Empfehlung seit 2023).
  Vorhandene Passwörter bleiben gültig und werden beim nächsten erfolgreichen Anmelden unbemerkt
  auf die neue Stärke umgestellt.
- **`UpdateSettingsAsync` mit einem Einstellungs-Objekt statt 20 Positionsparametern**
  (`PLAN.md` 4.1). Der Fehler vom 07.08., bei dem das Anheften eines Lesetexts sechs Einstellungen
  zurücksetzte, wird damit vom Compiler verhindert statt von einer Vorab-Prüfung.

## Schritt 7 — Betrieb und Weitergabe

- **Release-Workflow:** Ein Git-Tag `v*` baut die App und legt sie als ZIP an ein GitHub-Release.
  Das ist die Voraussetzung für ein späteres Auto-Update, schaltet aber selbst noch nichts ein.
- **Systeminfo im Eltern-Bereich:** App-Version, .NET-Version, Datenbankgröße, letzte Sicherung.
  Das ist die erste Frage bei jedem Problem.

## Schritt 8 — Dokumentation

README, `NAECHSTE-SCHRITTE.md`, `TESTPLAN.md` (neue Prüfpunkte für Abzeichen, Meisterschaft,
Fehler-Kartei-Zeile), `BUILD.md` (.NET 10, lokaler Build) und `CLAUDE.md` werden nachgezogen.

---

## Was die Familie danach prüfen sollte

Diese Punkte kann keine CI sehen, nur ein Mensch am Bildschirm:

- Sieht „Mein Fortschritt“ auf dem echten Bildschirm gut aus, ist alles lesbar?
- Freuen sich die Kinder über die Abzeichen, oder stören sie?
- Stimmen die neuen Englisch- und Musikfragen? (Gegenlesen durch jemanden mit gutem Englisch.)
- Läuft die App nach dem Update auf .NET 10 wie vorher, auch KI-Chat und Vorlesen?
