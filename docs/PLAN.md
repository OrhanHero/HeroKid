# Weiterer Plan für LernTor

Stand: 07.08.2026. Reine Planung — nichts hiervon ist umgesetzt.

Dieser Plan ordnet nach **Risiko für die Familie**, nicht nach technischer Eleganz. Die App wird
täglich von zwei Kindern benutzt; was sie am ehesten kaputtmacht oder ihnen Arbeit vernichtet,
steht oben.

## Wo das Projekt steht

| Größe | Wert |
|---|---|
| Code | 345 `.cs`-Dateien, ~59.300 Zeilen, 30 XAML-Ansichten *(gemessen 07.08.2026)* |
| Fächer | 17 mit eigenem Generator, **4.671 Frage-Tupel** in 337 Themen *(gezählt, nicht geschätzt)* |
| Etappen | 20 (`LearningStage`), davon 17 Fach-Etappen |
| Tests | **601 Testmethoden** (+223 `InlineData`-Fälle) in 71 Dateien, plus 16 statische Prüfungen |
| Bereiche | Lesen, Tippen, Schreiben, News, 15 Schulfächer, KI-Bereich, Erste Hilfe, Führerschein (3 Unterbereiche), Abschlussquiz |
| Verteilung | ZIP-Artefakt aus GitHub Actions, kein Installer |
| Kalender | Ferien bis 14.08.2027, Feiertage bis 26.12.2027 |
| Stundenplan | pro Profil, eigenes Zeitraster je Schule (seit 07.08.2026) |

**Der wichtigste Satz über den Stand:** Kein Mensch hat die App je von Anfang bis Ende
durchgespielt. Alles, was bisher gefunden wurde — das falsche STOP-Schild, das doppelte „STOP",
die unbrauchbaren vektorisierten Zeichen, zuletzt das „TR" statt der türkischen Flagge — kam aus
dem Hinsehen, nicht aus Tests. 601 Testmethoden und 16 statische Prüfungen haben **keinen
einzigen** davon gefunden. Das ist kein Vorwurf an die Tests; es ist der Grund, warum Phase 0
alles andere sperrt.

**Das ausführliche, nach Rollen geordnete Testprotokoll steht in [`TESTPLAN.md`](TESTPLAN.md)** —
fünf Rollen (die beiden Kinder, Elternteil, Kind-das-rauswill, Notfall), weil sich Fehler an der
ABSICHT zeigen und nicht am Knopf.

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

## Phase 1.0 — Drei Fehler, die schon jetzt in der App stecken

**Am 07.08.2026 durch gezielte Analyse gefunden und jeweils im Code nachgeprüft.** Sie stehen vor
allem anderen in Phase 1, weil sie Daten verlieren bzw. Einstellungen still zurücksetzen — nicht,
weil sie laut sind. Keiner von ihnen wird durch die 601 Tests oder die 16 statischen Prüfungen
erfasst.

### 1.0.1 Einen Lesetext anzuheften setzt sechs andere Einstellungen zurück — **verifiziert**

`ParentSettingsViewModel.SetPinnedReadingTextAsync` (`:2388`) ruft
`StudentProfileRepository.UpdateSettingsAsync` (`:117`, **20 Parameter + CancellationToken**) mit
nur **14 Argumenten** auf, rein positional. Alles danach fällt auf die Defaults der Signatur:

| Einstellung | wird still auf | Folge |
|---|---|---|
| `NewsArticleCount` | 0 → 12 | eingestellte Artikelzahl weg |
| `NewsFilterStrictness` | `Normal` | ein auf „Streng" gestellter Jugendschutzfilter wird **gelockert** |
| `DrivingAreaEnabled` | `true` | abgeschalteter Führerschein-Bereich ist **wieder an** |
| `ErsteHilfeEnabled` | `true` | abgeschaltete Erste Hilfe ist **wieder an** |
| `DrivingChallengeSignCount` | 0 → 5 | Challenge-Größe weg |
| `DisabledSignCategories` | leer | **alle** Schilderkategorien wieder aktiv |

Die Methode ist ein Voll-Überschreiber ohne Patch-Semantik. Ein vergessenes Argument ist damit
**Datenverlust ohne Compilerfehler** — und weil viele Parameter denselben Typ (`int`, `bool`)
haben, verschiebt ein neu eingefügter Parameter still die Bedeutung aller folgenden.

**Behebung, in dieser Reihenfolge:** (1) den fehlenden Aufruf reparieren; (2) `UpdateSettingsAsync`
auf ein **Einstellungs-Objekt** statt 20 Positionsparameter umstellen, damit derselbe Fehler
strukturell nicht mehr möglich ist; (3) eine Preflight-Prüfung, die Aufrufe mit weniger Argumenten
als Pflichtparametern meldet.

### 1.0.2 `HasCompletedTyping` und `HasCompletedWriting` werden nie gespeichert — **verifiziert**

`StudentProgress` hat drei Merker (`:18`, `:23`, `:28`), `ProgressEntity` hat **nur
`HasCompletedReading`** (`:10`), und `ProgressRepository` bildet auch nur diesen ab (`:39`, `:63`).
`ProgressGateService:88` liest `HasCompletedTyping`, `MainViewModel:1145` setzt es — nach einem
Neustart ist es wieder `false`. Ein Kind, dem mitten in der Sitzung der PC abstürzt, macht den
Tipptrainer noch einmal.

**Behebung:** zwei Spalten ergänzen (additiv, `SqliteSchemaUpdater` zieht das nach) und beim
Testen gezielt nachstellen — das ist Punkt 4.12 in [`TESTPLAN.md`](TESTPLAN.md).

### 1.0.3 Core und App sind sich über abgeschaltete Bereiche nicht einig

`ProgressGateService:98` kennt nur die **globale** Menge `AppSettings.DisabledSubjects`;
`MainViewModel.IsSubjectDisabled` (`:523-536`) kennt zusätzlich die **profilbezogenen** Schalter
`DrivingAreaEnabled` / `ErsteHilfeEnabled`. Die Fortschrittsanzeige (`:479-480`) rechnet ebenfalls
nur mit der globalen Menge — profilweise abgeschaltete Bereiche blähen den Nenner „n/m" auf.
Weiter ist `ProgressGateService.CanEnterStage` (`:51-72`) **toter Code**: nur Tests rufen es auf.

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

### 2.0 Zeitbudget statt Etappenzahl — **die wichtigste Änderung, und zugleich eine Warnung**

> Status: **nur Planung.** Umgesetzt wird das frühestens, wenn die neuen Stundenpläne für das
> Schuljahr 2026/27 da sind (ab 24.08.2026) — die Fächerauswahl soll sich am Stundenplan
> orientieren, und den gibt es vorher nicht.

**Das Problem.** Die App hat 20 Etappen. Jede einzelne war für sich eine gute Idee. Zusammen sind
sie die einzige Art, wie dieses Projekt scheitern kann: nicht durch einen Fehler, sondern dadurch,
dass der Tag so lang wird, dass die Kinder ihn zu hassen anfangen. Das zeigt sich nicht in einem
Testlauf, sondern an einem Dienstag im November. Bisher begegnet die App dem Wachstum nur
verwaltend — Eltern können Bereiche abschalten, Aufgabenzahlen senken, Mindestzeiten kürzen. Das
sind alles Stellschrauben an einer Struktur, deren Grundgröße **die Anzahl der Bereiche** ist. Und
die wächst mit jedem Modul.

**Die Umkehr.** Nicht mehr „alle Bereiche, jeder etwas kürzer", sondern: die Eltern stellen ein
**Zeitbudget** ein (z. B. 45 Minuten), und die App entscheidet, was heute hineinpasst. Die Fächer
**rotieren über die Woche**, statt jeden Tag alle vorzukommen — Montag Mathe/Deutsch/Bio, Dienstag
Physik/Geschichte/Englisch. Damit ist die Tageslänge eine **eingestellte Größe** statt einer Folge
davon, wie viele Module es gerade gibt. Ein neues Modul verlängert den Tag dann nicht mehr; es
konkurriert um Platz. Das ist der eigentliche Punkt.

**Was schon da liegt** (nichts davon muss neu erfunden werden):
- `AdaptiveTopicWeighting` weiß, welche Themen am nötigsten sind — das ist die Auswahlregel.
- `ExamEntry.LearningWeight` zieht Fächer vor Klausuren nach vorn — muss im Budget Vorrang haben.
- Die Mindestzeiten je Etappe (`ExerciseSecondsPerQuestion`, `NewsSecondsPerArticle`,
  `ReadingMinutes`) sind bereits pro Profil einstellbar und ergeben zusammen eine **Schätzung**
  der Dauer je Bereich.
- `ActivityLog` hat Zeitstempel je Antwort — daraus lässt sich die **tatsächliche** Dauer je Fach
  messen, statt sie zu schätzen. Das ist der Unterschied zwischen einem Budget, das stimmt, und
  einem, das nur so heißt.
- Der **Stundenplan** (seit 08/2026) sagt, welche Fächer ein Kind an diesem Tag überhaupt hatte.

**Skizze, nicht Bauplan:**
1. `StudentProfile.DailyTimeBudgetMinutes` (0 = aus, dann bleibt alles wie heute — der Umstieg
   muss abschaltbar sein, sonst ist er nicht testbar).
2. Ein `DayPlanner` in Core: bekommt Budget, gemessene Ø-Dauer je Bereich, Schwächen, anstehende
   Klausuren und den Stundenplan des Tages; gibt die Liste der heutigen Etappen zurück. Rein
   rechnend, ohne Datenbank — also vollständig prüfbar, so wie `ProgressGateService`.
3. Feste Etappen bleiben außerhalb des Budgets oder bekommen einen kleinen Fixanteil (Vorlesen,
   News, Abschlussquiz). Über das Abschlussquiz muss dabei mitentschieden werden — siehe 2.2,
   die beiden Punkte hängen zusammen.
4. Eine **Rotationsgarantie**: kein Fach darf länger als N Tage ausfallen, sonst frisst die
   Schwächen-Gewichtung schwache Fächer auf und starke verschwinden ganz.

**Kopplung an den Stundenplan** — der eigentlich neue Teil und der Grund für das Warten:
Fächer, die das Kind heute **in der Schule hatte**, am selben Tag zu üben, ist die naheliegende
Regel; Fächer, die **morgen** dran sind, vorzubereiten, die zweite. Welche der beiden besser ist,
lässt sich nicht am Schreibtisch entscheiden — dafür brauchen wir die echten Pläne und ein paar
Wochen Erfahrung damit. Deshalb steht hier bewusst kein fertiger Algorithmus.

**Offene Fragen, die vor dem Bauen zu klären sind:**
- Was passiert, wenn das Budget aufgebraucht ist, das Kind aber mitten in einem Fach steckt?
  (Abschneiden ist respektlos, Überziehen macht das Budget wertlos — vermutlich: laufendes Fach
  zu Ende, dann Schluss.)
- Zählt die Zeit im Eltern-Planer, im KI-Chat, im Führerschein-Bereich mit?
- Was ist an einem Tag, an dem ein Kind schnell ist? Endet der Tag früher, oder kommt mehr?
  (Früher enden ist die ehrlichere Antwort und der eigentliche Anreiz.)
- Wie sieht das Kind, **warum** heute Physik dran ist und nicht Mathe? Ohne Begründung fühlt sich
  Rotation wie Willkür an — der Klausur-Lernplan hat genau dieses Problem schon einmal gelöst.

**Risiko, klar benannt:** das ist der tiefste Eingriff in den Ablauf, den die App bisher gesehen
hat. `ProgressGateService.SequentialOrder`, `LearningStage`, `SessionSteps` und der gesamte
Fortschrittsspeicher gehen heute davon aus, dass die Etappenliste **fest** ist. Vor dem ersten
Handgriff gehört Phase 1 (Sicherung/Wiederherstellung) nachweislich abgeschlossen.

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

### 5.1 Modul „Gaming & Creator-Insider" (`MedienGaming`)

**Die Idee:** Ein Fach über die Welt, in der die Kinder ohnehin sind — Minecraft, Roblox,
Fortnite, EA FC, YouTube und Twitch —, das dabei Verbraucherschutz und Medienkompetenz vermittelt.
Der Reiz liegt darin, dass ein Kind sich beim Lesen wie ein **Entlarver** fühlt, nicht wie ein
Schüler.

**Warum es einen eigenen Eintrag verdient:** Es ist der erste Vorschlag, der ein
*Motivations*problem angeht statt einer Wissenslücke. Alle anderen Bereiche muss man wollen; diesen
klickt ein Zwölfjähriger von selbst an.

#### Was neu wäre und was nicht

Gegen den Bestand geprüft:

| Themenblock | Befund |
|---|---|
| **Geld-Fallen** (Lootbox, In-Game-Währung, FOMO, Pay-to-Win) | **Neu.** `Gewi.KonsumUndVerantwortung` hat Impulskauf, Werbung, Influencer-Marketing, Schuldenfalle — benachbart, nicht dasselbe. Der stärkste Teil. |
| **Creator-Handwerk** (Hook, Thumbnail, Schnitt, Musikrechte) | **Überwiegend neu.** Musikrechte deckt `Itg.Urheberrecht` bereits ab (20 Fragen inkl. CC-Lizenzen, Plagiat) — darauf verweisen statt wiederholen. |
| **Wie Spiele denken** (Zufall, Spawn-Regeln, Ping vs. Skill, prozedurale Generierung) | **Halb neu.** `Itg.Algorithmen`, `Itg.AlgorithmenUndDigitaleWerkzeuge`, `Itg.HardwareUndNetzwerke` decken die Grundlagen ab; Ping und Pay-to-Win nicht. |
| **Account & Umgangston** (2FA, Skin-Phishing, Voice-Chat-Toxizität) | **Kaum neu.** `Itg.Cybermobbing` (20 Fragen, inkl. Anonymitätseffekt), Phishing in `Itg.Datenschutz`, Passwörter in `Itg.SicherePasswoerter`. Nur 2FA und Account-Diebstahl wegen Skins fehlen. |

#### Zuschnitt

Ein Fach mit Profil-Schalter (Muster Erste Hilfe), **~80 Fragen in vier Themen**:

1. Geld-Fallen (~25) — der Kern
2. Creator-Handwerk (~20)
3. Wie Spiele denken (~20)
4. Account und Umgangston (~15, bewusst klein)

Dazu **zehn gaming-bezogene Fragen zusätzlich** in `Itg.SicherePasswoerter` und `Itg.Cybermobbing`
statt eines fünften Blocks — dort sucht man sie später auch.

#### Drei Regeln, ohne die das Modul verrottet

**1. Mechanismen lehren, Spiele nur als Beispiel.** V-Bucks-Preise, Fortnite-Shop-Mechaniken und
Pack-Wahrscheinlichkeiten stimmen in achtzehn Monaten zur Hälfte nicht mehr — und **kein Test kann
das merken**. „Was ist eine Lootbox und warum wirkt sie?" hält. „Wie viel kostet Skin X" hält nicht.

**2. Keine Namen von Creatorn in Fragetexten.** Kanäle hören auf zu posten, ändern ihr Format oder
geraten in Ungnade. Eine Frage über eine benannte Person altert schlechter als jede Preisangabe.
Formate ja („Minecraft-Let's-Play", „Comic-Dub", „Shorts"), Personen nein.

**3. Kein moralischer Ton.** Ein Modul, das nach „Spiele sind böse" klingt, verliert einen
Fünfzehnjährigen bei Frage zwei. Die Haltung ist: *So funktioniert die Masche — jetzt kennst du sie.*

#### Was zuerst geklärt werden muss

Die Familie schaut regelmäßig vier Kanäle:
`@tinymacdude`, `@KoreanComic`, `@al1craft`, `@justmehabibi`.

Die sind wertvoller Rohstoff, aber **hier nicht auswertbar** — diese Entwicklungsumgebung hat kein
Netz, und aus Kanalnamen auf Inhalte zu schließen wäre geraten. Gebraucht wird je Kanal, von der
Familie oder einem Agenten mit Netzzugang:

- Welches **Spiel/Format** (Minecraft-Bau, Roblox-Obby, Comic-Vertonung, Fußball-Packs …)?
- Welche **Monetarisierung** ist im Kanal sichtbar (Sponsoring, Affiliate-Links, eigener Shop,
  Glücksspiel-nahe Formate wie Pack-Openings)?
- Welche **Redaktionstricks** sind auffällig (Hook-Länge, Thumbnail-Stil, Cliffhanger, Titel)?

Daraus werden Fragen über *Muster*, nicht über *Kanäle*. Der Sinn: Die Kinder erkennen im
Lieblingsvideo wieder, was sie in der Frage gelesen haben — ohne dass der Kanalname im Text steht.

#### Nebenwirkung

Ein 18. Fach macht das Abschlussquiz zu 20 ÷ 18 Fragen. **Phase 2.2 wird damit Voraussetzung,
nicht Option.**

#### Was NICHT gebaut wird

**Keine „Wahl-" oder „Belohnungs-Etappe".** Die App kennt nur verpflichtend-in-Reihenfolge oder
pro Profil abgeschaltet. Eine dritte Art zu bauen hieße `ProgressGateService`, `SequentialOrder`,
`SessionSteps` und die Gate-Logik anzufassen — das wäre der teure Teil, nicht die Fragen. Und als
Belohnung fürs Durchhalten würde das Modul die Kinder dazu bringen, den Rest zu hetzen: genau das
Verhalten, gegen das die Anti-Durchklick-Regeln gebaut sind. Die Freiwilligkeit entsteht über den
Inhalt, nicht über die Mechanik.

### 5.2 Kleinere Ideen

- **Fehler-Kartei sichtbar machen**: Die Kinder sehen nicht, wie viele Fragen wiederkommen.
- **Elternbericht per PDF** statt nur am Bildschirm.
- **Zweiter Rechner**: Sicherung auf USB, damit ein Kind auch am Zweitgerät weiterlernen kann.
- **Lesetexte aus eigenen Büchern** einscannen (der Teacher-Import kann PDF schon).

### 5.3 Sammelstelle für weitere Ideen

Neue Vorschläge kommen hierher, bevor sie bewertet sind. Für jeden gilt dieselbe Prüfung, die
sich bisher jedes Mal gelohnt hat:

1. **Gibt es das schon?** Themenlisten der 17 Generatoren gegenlesen — bei vier von fünf bisherigen
   Vorschlägen war der größere Teil bereits vorhanden.
2. **Verlängert es den Lerntag?** Ein neues Fach heißt eine neue Etappe für zwei Kinder, die schon
   zur Schule gehen.
3. **Altert der Inhalt?** Verkehrszeichen halten Jahrzehnte, Spielpreise achtzehn Monate.
4. **Braucht es neue Mechanik oder nur neue Fragen?** Nur Fragen ist ein Tag Arbeit, neue Mechanik
   eine Woche.

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
  für zwei Kinder, die schon zur Schule gehen. Solange die Tageslänge aus der Anzahl der Bereiche
  folgt, ist jede weitere Idee ein Preis, den die Kinder zahlen — deshalb steht das Zeitbudget
  (2.0) vor jedem neuen Modul und nicht dahinter.

---

## Reihenfolge

```
Phase 0  App durchspielen (TESTPLAN.md)    ← sperrt alles andere
   │
   ├─ Phase 1.0 Drei verifizierte Fehler     ← Datenverlust, sofort
   │
   ├─ Phase 1  Sicherung/Wiederherstellung  ← höchstes Risiko
   │
   ├─ Phase 2  Zeitbudget (2.0, ab 24.08.2026), Pool-Reichweite, Abschlussquiz
   │
   ├─ Phase 3  Schuljahreswechsel (vor Aug 2027), Kalender (Frühjahr 2027)
   │
   └─ Phase 4  Feature-Schalter, CI, Preflight, Doku
```

## Prüfkommandos

```bash
python3 scripts/preflight.py                  # 16 statische Prüfungen
python3 scripts/check-answer-length-bias.py   # Gate 60 % je Generator
dotnet test tests/LernTor.Tests/LernTor.Tests.csproj
```

CI: `windows-latest`, ~7 Minuten. **Immer `head_sha` gegen den eigenen Commit prüfen** — ein
grüner letzter Lauf heißt nicht, dass der eigene Commit gebaut wurde.
