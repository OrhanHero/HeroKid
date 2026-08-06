# Weiterer Plan für LernTor

Stand: 06.08.2026. Reine Planung — nichts hiervon ist umgesetzt.

Dieser Plan ordnet nach **Risiko für die Familie**, nicht nach technischer Eleganz. Die App wird
täglich von zwei Kindern benutzt; was sie am ehesten kaputtmacht oder ihnen Arbeit vernichtet,
steht oben.

## Wo das Projekt steht

| Größe | Wert |
|---|---|
| Code | 326 `.cs`-Dateien, ~56.600 Zeilen, 30 XAML-Ansichten |
| Fächer | 17 mit eigenem Generator, ~5.030 kuratierte Fragen |
| Etappen | 20 (`LearningStage`), davon 17 Fach-Etappen |
| Tests | ~765, plus 14 statische Prüfungen in `scripts/preflight.py` |
| Bereiche | Lesen, Tippen, Schreiben, News, 15 Schulfächer, KI-Bereich, Erste Hilfe, Führerschein (3 Unterbereiche), Abschlussquiz |
| Verteilung | ZIP-Artefakt aus GitHub Actions, kein Installer |
| Kalender | Ferien bis 14.08.2027, Feiertage bis 26.12.2027 |

**Der wichtigste Satz über den Stand:** Kein Mensch hat die App je von Anfang bis Ende
durchgespielt. Alles, was bisher gefunden wurde — das falsche STOP-Schild, das doppelte „STOP",
die unbrauchbaren vektorisierten Zeichen — kam aus dem Hinsehen, nicht aus Tests.

---

## Phase 0 — Erst benutzen, dann bauen

**Sperrt alles andere.** Solange niemand die App durchgespielt hat, ist jede weitere Arbeit ein
Bauen ins Ungewisse. Die Prüfliste unten ist so gebaut, dass sie an einem Abend abzuarbeiten ist.

### Prüfliste für den ersten vollständigen Durchlauf

Vorbereitung: `%LOCALAPPDATA%\LernTor\lerntor.db` vorher sichern. Ohne `LERNTOR_SKIP_LOCK=1`
starten, um den echten Kiosk zu sehen — aber nur, wenn eine zweite Person am Rechner ist.

**Start und Rahmen**
- [ ] App startet ohne Fehlermeldung, Profilauswahl zeigt beide Kinder
- [ ] Uhr oben läuft, Etappenleiste zeigt die richtige aktuelle Etappe
- [ ] Ferien-Kachel rechts zeigt „Sommerferien – noch X Tage" (bis 22.08.2026)
- [ ] Planer-Knopf unten links ist sichtbar, sobald eine Etappe läuft

**Die Etappen der Reihe nach**
- [ ] Lesen: Text erscheint, Mindestzeit läuft, Weiter erst danach frei
- [ ] Tippen: Tastatur wird angezeigt, Genauigkeit wird gemessen
- [ ] Schreiben / Diktat: Vorlesefunktion spricht (Piper oder SAPI)
- [ ] News: Artikel laden — **und was passiert ohne Internet?**
- [ ] Je Fach: 6 Aufgaben, Erklärung nach falscher Antwort, Weiter-Zwang greift
- [ ] KI-Bereich: Module lesbar, Checkliste beantwortbar
- [ ] **Erste Hilfe: alle acht Themen einmal gesehen?** (nie von einem Menschen gesehen)
- [ ] Führerschein: Challenge, Karteikarten, Quiz — **alle 77 Schilder einmal ansehen**
- [ ] **Theoriefragen: Mehrfachauswahl bedienbar? Prüfungssimulation vollständig?** (nie gesehen)
- [ ] **Theorie-Kurs: alle 14 Lektionen lesbar, Kontrolle startbar?** (nie gesehen)
- [ ] Abschlussquiz: 20 Fragen, Auswertung, Freischaltung

**Der Planer-Knopf** (neu, nie benutzt)
- [ ] Mitten in einer Aufgabe öffnen → Hausaufgaben und Klausuren erscheinen
- [ ] „Zurück zum Lernen" → **dieselbe** halb beantwortete Aufgabe steht wieder da
- [ ] Die Mindestzeit-Uhr ist während des Planers **nicht** weitergelaufen

**Eltern-Bereich**
- [ ] Passwort greift, alle Reiter öffnen sich
- [ ] Ferien-/Feiertagsliste vollständig und richtig sortiert
- [ ] Alle Schalter (Fächer, Führerschein, Erste Hilfe) wirken beim nächsten Start
- [ ] Sicherung exportieren → Datei entsteht und ist größer als 0 Byte
- [ ] Wochenbericht/Heatmap zeigt die eben erzeugten Daten

**Kiosk** (nur mit zweiter Person am Rechner)
- [ ] Alt+Tab, Win, Win+Tab, Strg+Esc, Alt+F4 — kommt man raus?
- [ ] Fenster über die Alt-Tab-Vorschau schließen — bleibt es offen?
- [ ] Nach Bestehen: Sperre wirklich weg, Desktop erreichbar

**Ergebnis:** eine Liste konkreter Beobachtungen. Die ist mehr wert als jedes statische Audit.

---

## Phase 1 — Datensicherheit

### 1.1 Sicherung und Wiederherstellung im Ganzen testen — **höchste Priorität**

**Warum:** `AutoBackupPolicyTests` prüft elf Dinge — Dateinamen, Aufbewahrung, Fingerabdruck.
**Kein einziger Test schreibt eine Sicherung und liest sie zurück.** Genau der Pfad, der die Daten
der Kinder bewegt (Verbindungs-Pool schließen, Datei ersetzen, Schema nachziehen), ist ungeprüft.

**Woran man merkt, dass es fehlt:** Gar nicht — bis die Wiederherstellung gebraucht wird.

**Umfang:** ein Testfall gegen eine echte Temp-Datenbank: Profile und Fortschritt anlegen,
exportieren, Daten zerstören, wiederherstellen, prüfen dass alles wieder da ist. Dazu ein zweiter
mit einer Sicherung aus einem **älteren Schema**, damit der `SqliteSchemaUpdater`-Pfad mitläuft.

**Risiko, wenn man es lässt:** Zwei Kinder verlieren ihren gesamten Lernstand, und zwar in dem
Moment, in dem man ihn retten wollte.

### 1.2 `PRAGMA integrity_check` im Eltern-Bereich

Ein Knopf „Datenbank prüfen" mit Klartextergebnis. Eine beschädigte SQLite-Datei zeigt sich sonst
erst als scheinbar zufälliger Absturz.

### 1.3 Wiederherstellung dokumentieren

Eine halbe Seite in `docs/BUILD.md`: Wo liegen die Sicherungen, wie spielt man sie ein, was tun,
wenn die App gar nicht mehr startet. Für den Fall, dass niemand hier ist, der es aus dem Code lesen
kann.

---

## Phase 2 — Inhaltliche Tragfähigkeit

### 2.1 Pool-Erschöpfung ausrechnen statt schätzen

**Warum:** Drei Mechanismen halten Fragen zurück — richtig beantwortete (`MasteredPrompt`), kürzlich
gesehene (`ActivityLog`), plus die Fehler-Kartei. Bei 6 Aufgaben/Tag ist rechnerisch bestimmbar,
nach wie vielen Tagen ein Fach nur noch Wiederholungen liefert. **Bisher weiß das niemand.**

**Umfang:** ein Skript `scripts/pool-reichweite.py`, das je Fach und Klassenstufe die Poolgröße
gegen die Tagesrate hält und eine Tabelle ausgibt. Ergebnis sagt, wo Fragen fehlen — statt zu raten.

**Erwartung:** Fächer mit 200 Fragen über drei Stufen sind dünner, als sie aussehen.

### 2.2 Abschlussquiz entdünnen

**Warum:** 20 Fragen ÷ 17 Fächer = eine Frage pro Fach. Das Quiz prüft nicht mehr, es tastet an.

**Drei Wege, in aufsteigendem Aufwand:**
1. Zielzahl auf 25–30 anheben (eine Zeile, aber längerer Tag)
2. Nur Fächer nehmen, die das Kind **heute tatsächlich geübt** hat
3. Gewichtung nach Schwäche — die Daten dafür liegen bereits in `AdaptiveTopicWeighting`

Weg 2 ist der richtige: er hält den Tag kurz und prüft trotzdem das Geübte.

### 2.3 Antwortlängen-Bias im Blick behalten

Läuft automatisch (`check-answer-length-bias.py`, Gate 60 %). Kein Handlungsbedarf, nur nicht
vergessen, wenn neue Fragen dazukommen.

---

## Phase 3 — Zeitliche Tragfähigkeit

### 3.1 Schuljahreswechsel

**Warum:** `GradeLevel` wird einmal beim Anlegen gesetzt. Am 24.08.2026 beginnt für beide Kinder
ein neues Schuljahr; im Sommer 2027 werden sie Klasse 7 und Klasse 10. Es gibt keinen Weg dorthin
außer „Profil neu anlegen" — was den Fortschritt kostet.

**Umfang:** Klassenstufe im Eltern-Bereich änderbar machen (`UpdateSettingsAsync` kann es fast
schon), plus ein Hinweis im Eltern-Bereich, wenn der Schulstart aus dem Kalender vorbei ist.

**Fällig:** vor August 2027, besser gleich.

### 3.2 Kalender verlängern

Ferien enden am 14.08.2027, Feiertage am 26.12.2027. Der Eltern-Bereich sagt das an — es muss nur
jemand hinsehen. **Fällig: Frühjahr 2027.**

### 3.3 Mitternacht während einer Sitzung

**Warum:** Der Tagesfortschritt hängt an `DateTime.Today`. Was passiert, wenn ein Kind um 23:58
anfängt? Ungeprüft.

**Umfang:** ein Test, der die Tagesgrenze simuliert; falls das Verhalten falsch ist, den Stichtag
beim Sitzungsstart einfrieren.

---

## Phase 4 — Wartbarkeit

### 4.1 Feature-Schalter vereinheitlichen

**Warum:** `DrivingAreaDisabled` und `ErsteHilfeDisabled` sind zwei Einzelspalten mit derselben
Invertierungs-Falle (`DEFAULT 0` heißt „an"). Beim dritten Modul wird jemand die Invertierung
vergessen, und der Bereich ist für alle vorhandenen Profile still aus.

**Umfang:** eine Spalte `DisabledModulesJson` (wie `DisabledSignCategories`), ein Enum, ein
Zugriffspunkt. Migration der zwei vorhandenen Spalten.

**Nebeneffekt:** Der Preflight-Check für Fach-Verdrahtung wird einfacher.

### 4.2 CI-Push-Problem

**Warum:** Seit dem 06.08.2026 lösen Pushes auf dieses Repo **keine Workflow-Läufe** aus. Workflow
aktiv, Trigger passend, Kontingent frei — GitHub verschluckt das Ereignis. Behelf: manuell per
`workflow_dispatch`.

**Umfang:** beobachten, ob es sich von selbst legt. Wenn nicht: einen zweiten Trigger
(`schedule` alle 6 h auf `master`) als Netz. In CLAUDE.md steht bereits, dass `head_sha` zu
vergleichen ist statt der Farbe des letzten Laufs.

### 4.3 Preflight weiter schärfen

Kandidaten, jeder aus einem echten Fehler dieser Codebasis geboren:
- Fest verdrahtete Zählungen in Tests, die bei neuen Fächern veralten (`Assert.Equal(17, …)`)
- Von Hand gepflegte Generator-/Ansichts-Listen in Tests (der `MathGeneratorTests`-Fall)
- `[ObservableProperty]`-Felder, deren Name mit Großbuchstaben beginnt (erzeugt stillschweigend
  eine kaputte Property)

### 4.4 Doku aufräumen

`README.md` ist auf über 400 Zeilen gewachsen und beschreibt inzwischen 20 Bereiche. Sinnvoller
Schnitt: README als Überblick, Details in `docs/` je Bereich — das Muster gibt es mit
`FUEHRERSCHEIN.md` und `TIPPTRAINER.md` schon.

---

## Phase 5 — Ideen, bewusst nachrangig

Nichts hiervon ist nötig. Wenn Phase 0–4 stehen, ist die App fertig genug.

- **Fehler-Kartei sichtbar machen**: Die Kinder sehen nicht, wie viele Fragen wiederkommen.
- **Elternbericht per PDF** statt nur am Bildschirm.
- **Zweiter Rechner**: Sicherung auf USB, damit ein Kind auch am Zweitgerät weiterlernen kann.
- **Lesetexte aus eigenen Büchern** einscannen (der Teacher-Import kann PDF schon).

---

## Was NICHT gemacht wird, und warum

- **Vollständiges Threading-Audit.** Kein einziger Deadlock ist je aufgetreten. Ohne Symptom ist
  das Suchen teuer und ergebnisarm.
- **SQL-Injection-Audit.** Es gibt genau einen Roh-SQL-Aufruf: `VACUUM INTO` mit einem Dateipfad
  aus dem Speichern-Dialog, Hochkommata verdoppelt. Kein Angriffsweg. Die EF1002-Warnung bleibt
  als Lärm stehen oder wird mit Begründung unterdrückt — ein Audit-Kapitel ist sie nicht.
- **Kiosk-Härtung gegen neue Windows-Shortcuts.** Sinnvoll, aber nur an einem echten
  Windows-Rechner prüfbar. Gehört zum lokalen Agenten, nicht hierher.
- **LLM-Benchmarking auf schwacher Hardware.** Erst messen, wenn es auf dem Familienrechner
  tatsächlich zu langsam ist. Vorher optimiert man ins Blaue.
- **Mehr Fächer, mehr Module.** Der Tagesablauf hat 20 Etappen. Jede weitere verlängert den Tag
  für zwei Kinder, die schon zur Schule gehen.

---

## Reihenfolge

```
Phase 0  App durchspielen                  ← sperrt alles andere
   │
   ├─ Phase 1  Sicherung/Wiederherstellung  ← höchstes Risiko
   │
   ├─ Phase 2  Pool-Reichweite, Abschlussquiz
   │
   ├─ Phase 3  Schuljahreswechsel (vor Aug 2027), Kalender (Frühjahr 2027)
   │
   └─ Phase 4  Feature-Schalter, CI, Preflight, Doku
```

## Prüfkommandos

```bash
python3 scripts/preflight.py                  # 14 statische Prüfungen
python3 scripts/check-answer-length-bias.py   # Gate 60 % je Generator
dotnet test tests/LernTor.Tests/LernTor.Tests.csproj
```

CI: `windows-latest`, ~7 Minuten. **Immer `head_sha` gegen den eigenen Commit prüfen** — ein
grüner letzter Lauf heißt nicht, dass der eigene Commit gebaut wurde.
