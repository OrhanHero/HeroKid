# LernTor – Status-Quo-Bericht (Stand: 2026-07-30, zweite Fassung)

> **Hinweis**: Dieser Bericht basiert auf Code-Analyse. Die App läuft nur unter Windows (WPF + Win32 P/Invoke). Build-Verifikation erfolgt via GitHub Actions (`.github/workflows/build.yml` auf `windows-latest`).

---

## 1. Gesamtfortschritt

| Bereich | Status | Details |
|---------|--------|---------|
| **Core Domain** | ✅ Fertig | Enums, Models, `ProgressGateService`, `ScoringService`, `LearningStageSubjects.Map` |
| **ContentGen (Generatoren)** | ✅ Fertig | 16 Fach-Generatoren (15 RLP-Fächer + KI-Wissen), `QuizComposer`, Review-/Mastered-Logik |
| **News (RSS + Aufbereitung)** | ✅ Fertig | RSS-Loading, Vereinfachung, Verständnisfragen, Kategorisierung, Glossar, Bezirks-Erkennung |
| **Data (EF Core SQLite)** | ✅ Fertig | Repositories: Progress, ActivityLog, MasteredPrompt, ReviewQuestion, CustomQuestion, Settings, Rewards, TypingProgress |
| **Security (Kiosk)** | ✅ Fertig | Keyboard-Hook (inkl. Win+Kombos), TaskMgr-Policy, NoWinKeys-Policy, Autostart, Admin-Auth, Vordergrund-Wächter + Closing-Sperre in `MainWindow` |
| **App (WPF/MVVM)** | ✅ Fertig | MainVM, alle Views (ProfileSelection, Welcome, News, Exercise, FinalQuiz, Result, ParentSettings), QuestionCard, KI-Chat, TTS (Piper), Lehrer-Import (PDF/Word → KI → Entwürfe), Belohnungen, Wochenbericht |
| **Localization** | ✅ Fertig | DE/TR, String-Indexer, Live-Switch via `PropertyChanged("Item[]")` |
| **Local LLM** | ✅ Fertig | LLamaSharp, GGUF-Autodownload (~2-4 GB), 2 Features: Lehrer-Import + KI-Hausaufgaben-Chat |
| **Tipptrainer** | ✅ Fertig | 11 reguläre Lektionen + 1 profil-spezifische Abschluss-Lektion, nur Deutsch/QWERTZ, Mindestgenauigkeit pro Profil einstellbar (Presets 25/50/75/100%, Standard 25%). Die Zieltexte der beiden letzten Lektionen können Eltern selbst schreiben (max. 200 Zeichen, `TypingTextOverrides`) |
| **Vokabeltrainer** | ✅ Fertig | Eigene Wortlisten je Profil für Englisch und Türkisch, Massen-Einfügen, beide Abfragerichtungen, Spaced Repetition 7/30/90 Tage |

---

## 2. Fach-Abdeckung nach Berliner Rahmenlehrplan (Klasse 6 / 7 / 9)

**Legende**: ✅ = Topic mit ~20 kuratierten Fragen vorhanden, ⚠️ = Topic existiert aber unvollständig (< 20 Fragen), ❌ = Topic fehlt ganz, — = Fach nicht in App

> **Doppeljahrgänge**: Der RLP ist in Doppeljahrgangsstufen gegliedert (7/8, 9/10). Die Pools
> folgen dem - Klasse 7 deckt 7/8 ab, Klasse 9 deckt 9/10 ab. Profile mit Klasse 8 bzw. 10 sind
> wählbar und greifen über die Übergangsregel in `ExerciseGeneratorBase.Generate` automatisch auf
> den passenden Pool zu (siehe `GradeLevel`).

### ✅ Vollständig implementierte Fächer (15/17 RLP-Fächer) + KI-Wissen

*Diese Tabelle wurde direkt aus `TopicsByGrade` in den Generator-Dateien abgeleitet (siehe
[docs/CURRICULUM.md](CURRICULUM.md) für die Themen-Detailtabellen und den vollständigen
RLP-Haken-Abgleich), nicht geschätzt.*

| Fach | Generator | K6 | K7 | K9 | Gesamt | Abdeckung RLP-Themenfelder |
|------|-----------|---:|---:|---:|-------:|---------------------------|
| **Mathematik** | `MathGenerator.cs` | 12 | 9 | 14 | 35 | Klasse 9: fehlt nur Stochastik-Baumdiagramme und darstellende Geometrie (Nischenthemen) |
| **Deutsch** | `GermanGenerator.cs` | 12 | 6 | 15 | 33 | ✅ komplett (inkl. Drama-Analyse, Novelle, Parabel) |
| **Türkisch** | `TurkishGenerator.cs` | 8 | 6 | 10 | 24 | ✅ komplett (alle 4 kommunikativen RLP-Themenfelder je Stufe) |
| **Chemie** | `ChemieGenerator.cs` | 9 | 6 | 9 | 24 | ✅ komplett |
| **Physik** | `PhysikGenerator.cs` | 10 | 6 | 7 | 23 | ✅ komplett |
| **Englisch** | `EnglischGenerator.cs` | 7 | 6 | 9 | 22 | ✅ komplett |
| **Biologie** | `BiologieGenerator.cs` | 6 | 6 | 8 | 20 | ✅ komplett |
| **Politik** | `PolitikGenerator.cs` | 7 | 4 | 8 | 19 | ✅ komplett |
| **Geografie** | `GeoGenerator.cs` | 7 | 3 | 9 | 19 | ✅ komplett |
| **Ethik** | `EthikGenerator.cs` | 6 | 3 | 10 | 19 | ✅ komplett |
| **Gewi** | `GewiGenerator.cs` | 9 | 6 | 3 | 18 | Klasse 6 komplett; Klasse 9 auf Kernthemen fokussiert (Fach läuft dort in Geschichte/Geo/Politik aus) |
| **Geschichte** | `GeschichteGenerator.cs` | 3 | 6 | 7 | 16 | ✅ komplett (inkl. Feindbilder/Propaganda-Bonusmodul) |
| **Musik** | `MusikGenerator.cs` | 5 | 4 | 6 | 15 | ✅ komplett |
| **Kunst** | `KunstGenerator.cs` | 4 | 4 | 6 | 14 | ✅ komplett |
| **ITG** | `ItgGenerator.cs` | 3 | 4 | 3 | 10 | Standardsoftware bewusst weggelassen (nicht quizbar) |
| **KI-Wissen** | `KiWissenGenerator.cs` | 4 | – | 4 | 8 | Kein RLP-Fach, sondern eigener Bereich mit fünf Lernmodulen (siehe `KiContentService`); Klasse 7/8 nutzen den Klasse-6-Pool |

**Gesamt: 319 Topics × ~20 Fragen ≈ 6.380 Fragen im Pool** (Mathematik würfelt zusätzlich echte
Zahlenwerte, dort ist der Pool praktisch unbegrenzt).

> Die Zahlen sind aus `TopicsByGrade` in den Generator-Dateien ausgezählt, nicht geschätzt. Frühere
> Fassungen dieses Berichts nannten 124 bzw. 232 Topics; der Klasse-7-Sprint und der KI-Bereich sind
> seither dazugekommen.

---

### ❌ Nicht implementierte RLP-Fächer (3/17)

| Fach | Grund |
|------|-------|
| **Sport** | Bewegung/Gestalten/Musizieren lassen sich nicht als Quiz abbilden |
| **WAT (Wirtschaft-Arbeit-Technik)** | Werkstatt/Projektarbeit nicht quizbar; Berufsorientierung nur theoretisch möglich |
| **Naturwissenschaften WP 7-10** | Wahlpflichtfach, Überschneidung mit Bio/Chemie/Physik/Informatik |

---

### ⚠️ Bewusste Lücken bei implementierten Fächern

Keine bekannten RLP-Lücken mehr bei den 15 implementierten Fächern (Stand nach Abschluss der
Deutsch- und Geschichte-Ergänzung). Verbleibende Einschränkungen sind bewusste Design-Entscheidungen
(siehe Abschnitt 4) statt fehlender Inhalte.

---

## 3. Offene To-Dos / Known Gaps

### 3.1 Technische Schulden & Bugs

| Priorität | Thema | Details |
|-----------|-------|---------|
| 🔴 **Hoch** | **EF Core Migrations fehlen** | Nutzt `EnsureCreated()` + `SqliteSchemaUpdater` (nur additive Änderungen). Bei Spalten-Umbennungen/Entfernung → manuelles DB-Löschen nötig. |
| ⏸️ **Nicht relevant** | **Installer Signing (EV-Zertifikat)** | Die Familie installiert aus dem ZIP-Artefakt des CI-Laufs, nicht über den Inno-Setup-Installer (Nutzer-Entscheidung 2026-07-30). Ohne Weitergabe an Dritte gibt es keine SmartScreen-Hürde zu lösen. |
| ✅ **Erledigt/gut genug** | **TTS Türkisch** | Aktuelle Piper-Stimme ist gut genug, bleibt vorerst unangetastet (Nutzer-Entscheidung). |
| 🟡 **Mittel** | **Offline-Erst-Installation LLM** | Model-Download (~2-4 GB) passiert erst bei erstem Nutzen. Kein Pre-Bundle im Installer. |
| ✅ **Erledigt** | **Eltern-Export/Backup** | Sicherung erstellen/wiederherstellen im Eltern-Bereich: Export als konsistente .db-Datei (`VACUUM INTO`), Import ersetzt die aktive DB nach Bestätigung (App-Neustart, Schema-Abgleich macht alte Sicherungen kompatibel). |
| 🟢 **Niedrig** | **Multi-Device Sync** | Nicht vorgesehen (lokal-only, SQLite). |
| ✅ **Erledigt** | **Kiosk-Ausbruch über Alt+Tab / Win+Tab** | Drei unabhängige Schichten: Vordergrund-Wächter (300ms-`DispatcherTimer`, vergleicht Prozess-IDs), `MainWindow.Closing`-Sperre solange `KioskLockService.IsLocked` (fängt den X-Button in der Windows-11-Alt+Tab-Vorschau, der ein reines `WM_CLOSE` ohne Tastendruck schickt) und Win-Kombo-Blockade im `KioskKeyboardHook` + `NoWinKeys`-Policy (`WindowsHotkeyPolicy`) gegen den Ausbruch via neuem virtuellem Desktop. |
| ✅ **Erledigt** | **Tipptrainer übte eine Taste, die es auf QWERTZ nicht gibt** | Die Grundreihen-Lektionen ließen "asdf jkl;" tippen - das ist die US-Belegung, auf einer deutschen Tastatur liegt dort das Ö. Beim Nachprüfen fielen zwei weitere Fehler in derselben Datei auf: Y und Z waren in der Finger-Zuordnung vertauscht (QWERTY statt QWERTZ), Komma und Punkt hingen beide am kleinen Finger, und zwei Lektionen hatten hartkodierte Finger-Listen, die kürzer waren als ihr Zieltext. Alle Listen werden jetzt aus dem Text abgeleitet. |
| ✅ **Erledigt** | **Tipptrainer verlangte faktisch 100%** | `TypingExerciseService.CheckInput` prüfte zusätzlich `correctChars >= targetText.Length` und hat damit das Eltern-Preset (25/50/75/100%) stillschweigend überschrieben. Die Zusatzbedingung ist entfernt - jetzt gilt nur noch die eingestellte Mindestgenauigkeit. |
| ✅ **Erledigt** | **Lehrer-Import scheiterte an LLamaSharp-Nativebibliotheken** | Der Single-File-Publish hat `llama.dll`/`ggml*.dll` in die exe eingebettet, wo LLamaSharps eigene Pfadsuche sie nicht findet (`The type initializer for 'LLama.Native.NativeApi' threw an exception`). Ein MSBuild-Target hält `runtimes/win-x64/native/` als lose Dateien daneben. **Erfordert eine Neuinstallation der App**, nicht nur ein Update der DB. |
| ✅ **Erledigt** | **Kinder nutzten den Längen-Bias der Antworten aus** | In 13 Generatoren war die richtige Antwort zu 70-93% die längste Option - die Kinder haben ohne Lesen die längste angeklickt. `scripts/balance-answer-lengths.py` hat die Distraktoren positionsbasiert angeglichen, `scripts/check-answer-length-bias.py` hält den Anteil in der CI dauerhaft unter 60% (Ist-Wert: ~35%). Bewusst **nicht** 0%, weil eine "die längste ist nie richtig"-Regel genauso ausnutzbar wäre. |
| ✅ **Erledigt** | **News-Feed-URLs pflegen** | Wöchentlicher automatischer Healthcheck (`.github/workflows/feed-healthcheck.yml` + `scripts/check-feeds.py`): prüft alle URLs aus `NewsFeedSource.cs` montags, Lauf wird rot bei totem Feed. Zusätzlich 48h-Offline-Cache pro Feed in der App (`FeedCache`). |

### 3.2 UX / Pädagogische Lücken

| Priorität | Thema | Details |
|-----------|-------|---------|
| ✅ **Erledigt** | **Lesestufen-Texte** | 63 Texte (33 literarisch/Allgemeinwissen + 30 Pop-Kultur), inkl. Klassiker-Ergänzung Goethe (Erlkönig), Schiller (Die Bürgschaft) und Fontane (Herr von Ribbeck auf Ribbeck im Havelland). |
| ✅ **Erledigt** | **Mathe: Offene Eingabe vs. MC** | Alle rechnerischen Topics nutzen `QuestionType.OpenText` (offene Zahleneingabe). Nur `Kongruenzabbildungen` und `Satz des Thales` bleiben bewusst Multiple-Choice: konzeptuelle Fragen mit Satz-Antworten, eine offene Eingabe wäre dort nicht sinnvoll validierbar. |
| ✅ **Erledigt** | **Gamification: Streaks** | Optional umgesetzt: 🔥-Lernserie auf dem Willkommensbildschirm (`StreakCalculator`), Standard AUS und von Eltern einschaltbar. Bewusst reine Anzeige - keine Strafen/Erinnerungen bei verpassten Tagen, ein noch nicht gelernter heutiger Tag bricht die Serie nicht. |
| ✅ **Erledigt** | **Eltern: Wochenziel** | Pro Profil einstellbar (3-7 Lerntage, Standard aus, `WeeklyGoalCalculator`). Reine Anzeige wie die Streaks - ein verfehltes Ziel kostet nichts. Anders als eine Serie zerbricht es nicht an einem einzigen verpassten Tag; ist es rechnerisch nicht mehr erreichbar, wird bewusst nichts Mahnendes angezeigt. Woche beginnt am Montag. |
| ✅ **Erledigt** | **Antworttempo sichtbar machen** | Das Protokoll speicherte nur *was*, nicht *wie schnell* geantwortet wurde - im Richtig/Falsch-Bericht ist Raten damit unsichtbar (bei drei Optionen liegt Raten in einem Drittel der Fälle richtig). `AnswerPaceAnalyzer` zeigt "X von Y Antworten unter 3 Sekunden" plus Warnzeile ab einem Drittel. Bewusst Hinweis statt Sperre: wer das Antworten blockiert, bestraft auch das Kind, das die Antwort sofort weiß. Median statt Mittelwert (eine Pause verzerrt sonst alles), Alt-Einträge ohne Messung werden übersprungen. |
| ✅ **Erledigt** | **Fehler-Kartei sichtbar machen** | Sie arbeitete unsichtbar im Hintergrund. Der Willkommensbildschirm zeigt jetzt vorher "🔁 Von früher noch offen: 3" - die Kinder sehen, dass Fehler wiederkommen, statt zu verschwinden. |
| ✅ **Erledigt** | **Lesetext-Verwaltung** | Eine Liste über eingebaute *und* eigene Texte (`ReadingTextRowViewModel`): ausblenden, anheften (📌 = jeden Tag erster Text, pro Profil), eigene Texte bearbeiten. Der Schlüssel hängt an der Datenbank-Id, nicht am Titel - Umbenennen löst eine Anheftung deshalb nicht. Ausblenden und Löschen lösen eine bestehende Anheftung automatisch. |
| ✅ **Erledigt** | **Nachrichtenquellen wählbar** | Alle 22 Quellen einzeln an/aus (`AppSettings.DisabledNewsFeeds`). Alle abgeschaltet = wieder alle aktiv. |
| ✅ **Erledigt** | **Eigene Aufgaben wurden nie abgefragt** | Regression aus dem Vokabel-Commit: `.Take()` stand vor `.OrderBy(zufall)`, also wurden immer die ersten N genommen - und das sind die generierten Aufgaben. Die eigenen standen dahinter und fielen weg. Die Zusammenstellung folgt jetzt einer Rangfolge (Fehler-Kartei → Vokabeln → eigene Aufgaben vollständig → generierte füllen auf). |
| ✅ **Erledigt** | **Dokument-Import hing endlos** | Der komplette Dokumenttext ging in ein 4096-Token-Fenster; ein mehrseitiges PDF sprengt das um ein Vielfaches, und auf der CPU rechnet das Modell dann stundenlang. Jetzt auf 6000 Zeichen gekürzt (an Satzgrenze), MaxTokens halbiert, AntiPrompts ergänzt, Abbrechen-Knopf und Laufzeitanzeige. |
| ✅ **Erledigt** | **Tipptrainer: Ausstieg aus der Übung** | Zurück-Knopf zur Lektionsübersicht; der Abbruch speichert nichts, der Bestwert bleibt. |
| ✅ **Erledigt** | **Eigene Lesetexte der Eltern** | Pro Profil (`CustomReadingTextRepository`). Ein eigener Text belegt den **ersten** der beiden Tagesplätze, mehrere wechseln sich täglich ab - würden sie sich unter die 63 eingebauten mischen, käme ein Gedicht für nächste Woche erst in zwei Monaten dran. Die drei Sprachfelder sind einzeln optional; leere Sprachen zeigen einen Hinweis statt einer leeren Spalte. |
| ✅ **Erledigt** | **Vokabeltrainer (Englisch/Türkisch)** | Massen-Einfügen einer Liste (`VocabularyParser` akzeptiert `=`, `;`, Tabulator und Bindestrich-mit-Leerzeichen; ein Bindestrich *im* Wort trennt nicht). Läuft in den bestehenden Fächern mit und ersetzt dort bis zur Hälfte der Aufgaben, statt eine eigene Lernstufe zu sein - Vokabeln *sind* der Kern dieser Fächer. Abwechselnd beide Abfragerichtungen, deterministisch je Vokabel und Tag. Eigene Wiederholungs-Steuerung (7/30/90 Tage): anders als bei der Fehler-Kartei verschwindet eine Vokabel nie, sie gehört zum Wortschatz. |
| ✅ **Erledigt** | **KI-Bereich als eigenes Fach** | `KiContentService` (Core) liefert fünf Lernmodule mit DE/TR-Texten, `KiWissenGenerator` die zugehörigen Quizfragen. Vollständig offline - kein einziger externer API-Aufruf. |
| ✅ **Erledigt** | **KI als Werkzeug, nicht als Lebensberater** | Zwei Module tragen diese Botschaft: "KI richtig nutzen" (erst selbst denken, gezielt fragen, nachprüfen, nicht abschreiben - inkl. der drei Prüffragen nach jeder Antwort) und "Wo KI nicht hingehört" (sie kennt dich nicht, ist kein Freund, kein Arzt, kein Schiedsrichter; bei Streit/Angst/Mobbing/Krankheit sind Menschen zuständig, Nummer gegen Kummer 116 111). Beide werden abgefragt, nicht nur gelesen. Zusätzlich ein dauerhafter Hinweis unter dem "🤖 KI fragen"-Knopf in jeder Aufgabe. |
| ✅ **Erledigt** | **Zeit-/Umfangs-Settings im Eltern-Bereich** | Lesen, News und Fächer haben jetzt einstellbare Zeit- und Umfangsgrenzen sowie einen Ferien-/Pausenmodus - alles ohne neuen Build änderbar. |
| ✅ **Erledigt** | **Klasse-7-Pools in Kunst/Musik/ITG** | Waren mit je 2 Topics die dünnsten Pools. Jetzt je 4 Topics: Kunst um "Bild des Menschen" und "Bild der Dinge", Musik um Instrumentenkunde und Musizieren/Zusammenspiel, ITG um Hardware/Netzwerke und IT-Sicherheit erweitert. |

### 3.3 Content-Erweiterung (Nice-to-have)

Nach Abschluss von Sprint 2, 3 und 4 sind alle Klasse-6-Grundthemen (Naturwissenschaften +
Gesellschaftswissenschaften), Chemie/Politik/Geografie/Ethik/Englisch/Türkisch Klasse 9 sowie die
letzten beiden Deutsch/Geschichte-Lücken (Novelle, Parabel, Feindbilder/Propaganda) vollständig.
Content vollständig - keine offenen RLP-Lücken mehr bei den 15 implementierten Fächern (siehe
`BiologieGenerator.cs`, `ChemieGenerator.cs`, `PhysikGenerator.cs`, `GeoGenerator.cs`,
`PolitikGenerator.cs`, `EthikGenerator.cs`, `EnglischGenerator.cs`, `TurkishGenerator.cs`,
`GermanGenerator.cs`, `GeschichteGenerator.cs`). Weitere Content-Arbeit wäre nur noch Polish
(siehe 3.2) oder bewusst ausgeklammerte Bereiche (siehe Abschnitt 4).

---

## 4. Architektur-Entscheidungen (Bestätigt & Dokumentiert)

| Entscheidung | Status | Begründung |
|--------------|--------|------------|
| **.NET 8 + WPF** (kein WinUI 3, kein Electron) | ✅ Final | Win32 P/Invoke für Kiosk-Lock zuverlässigster Weg; WPF gut stylebar |
| **Soft-Kiosk** (Vollbild + Keyboard-Hook + TaskMgr-Registry) | ✅ Final | Strg+Alt+Entf nicht abfangbar (OS-Schutz); für 10-15 J. angemessen |
| **Kein Zeitlimit** | ✅ Final | Bewusste Design-Entscheidung: Kinder sollen Zeit zum Lesen/Lernen haben |
| **Enums als Strings persistieren** (JsonStringEnumConverter) | ✅ Final | Verhindert Stillbruch bei Enum-Reihenfolge-Änderungen |
| **Additive Schema-Updates only** | ✅ Final | Keine EF Migrations-Toolchain nötig; `SqliteSchemaUpdater` reicht |
| **Lokal-only, keine Cloud/Telemetrie** | ✅ Final | Datenschutz, Offline-Fähigkeit, DSGVO-konform |
| **LLamaSharp (CPU-only, GGUF)** | ✅ Final | Keine CUDA-Abhängigkeit; Qwen2.5-7B-Instruct (Apache-2.0) stark in DE/TR |
| **RSS live laden (kein Cache)** | ✅ Final | Tagesaktuelle News; Offline-Fallback via Tagesarchiv (7 Tage) |
| **Doppeljahrgangs-Pools statt Pool pro Klasse** | ✅ Final | Der RLP ist selbst in Doppeljahrgängen (7/8, 9/10) organisiert. Klasse 8/10 sind wählbar, nutzen aber über die Übergangsregel den Pool der unteren Stufe - Eltern tragen die echte Klasse ein, wir pflegen halb so viele Pools |
| **Längen-Bias auf ~35% statt 0%** | ✅ Final | Ein hartes "längste Antwort ist nie richtig" wäre invers genauso ausnutzbar. ~35% liegt nahe am Zufall bei 3-4 Optionen |
| **`scripts/preflight.py` statt Compiler** | ✅ Final | In der SDK-losen Entwicklungsumgebung ist die CI der einzige echte Compiler. Preflight kodiert die dokumentierten Fallstricke (Klammerbalance, `Run.Text`-Bindings, `HttpClient`-using, Shutdown/Unlock, Subject-Verdrahtung, EF-`DateTimeOffset`-Sortierung, Generator-Konsistenz) als ausführbare Prüfungen und fängt sie vor dem Push |

---

## 5. Test-Abdeckung

| Test-Projekt | Tests | Abdeckung |
|--------------|-------|-----------|
| `LernTor.Tests` (xUnit) | 200 `[Fact]`/`[Theory]` | Core (ProgressGate, Scoring, Streaks, Spaced Repetition), ContentGen (alle 16 Generatoren: Musterlösungen prüfen, Klasse-7-Pools ohne Rückfall, Doppeljahrgangs-Regel, Sicherheitsnetz „jede Stufe liefert in jedem Fach Aufgaben"), News (RSS-Parser RDF/Atom/RSS2, FeedCache, Vereinfachung, Kategorisierung, Glossar, Finanzwissen), Data (Repositories gegen echte SQLite-Dateien) |
| `LernTor.UiTests` (xUnit, net8.0-windows) | 5 `[Fact]`/`[Theory]` (davon eine Theory über alle Views) | XAML-Load-Tests (jede View mit App-Ressourcen instanziieren + Layout - fängt die XamlParseException-Klasse) + Prozess-Smoke-Test (echte exe startet, Hauptfenster erscheint, kein Fehlerdialog) - läuft im selben windows-latest-CI-Lauf |
| Integrationstests | 0 | Manuell auf Windows getestet (siehe docs/PILOT-CHECKLISTE.md) |
| `scripts/preflight.py` | 9 Prüfungen | Kein Test-Framework, sondern ein statischer Vorab-Check für die SDK-lose Entwicklungsumgebung: Klammerbalance (.cs), XAML-Wohlgeformtheit, `Run.Text` ohne `Mode=OneWay`, `HttpClient` ohne `using System.Net.Http;`, `Shutdown()` ohne vorheriges `Unlock()`, Subject-Verdrahtung, Generator-Konsistenz (`TopicsByGrade` ↔ Methoden), EF-`DateTimeOffset`-Sortierung, Konfigurationsdateien. Hat direkt beim ersten Lauf einen echten Bug gefunden (tote Bindings in `TypingExerciseView.xaml`). |

**CI**: `.github/workflows/build.yml` baut auf `windows-latest` → Artefakt hochladen → manueller Smoke-Test auf Windows empfohlen. Zusätzlich läuft `scripts/check-answer-length-bias.py` als Gate (Schwelle 60% je Generator).

**Vor jedem Push** (siehe `.claude/skills/push-check/SKILL.md`): `python3 scripts/preflight.py` und
`python3 scripts/check-answer-length-bias.py` - beide müssen grün sein, danach Push und
CI-Lauf prüfen. Lokal kompilieren geht in dieser Umgebung nicht.

---

## 6. Nächste Meilensteine (Vorschlag)

> **Priorisierung (Nutzer-Entscheidung 2026-07-17):** Installer-Signing (EV-Zertifikat) wird bewusst
> auf ganz zuletzt verschoben - erst wenn die finale Version erreicht ist. Türkische TTS-Stimme ist
> aktuell gut genug und bleibt vorerst unangetastet. Fokus liegt auf den Content-Lücken (Sprint 2/3).

### Sprint 2: Content-Runde 1 - Klasse 6 Naturwissenschaften ✅ ABGESCHLOSSEN
5. ~~Biologie Klasse 6: Zelle, Lebensräume/Nahrungsketten (2 Topics)~~ ✅ erledigt (`Zelle`, `LebensraeumeUndNahrungsketten` in `BiologieGenerator.cs`)
6. ~~Chemie Klasse 6: Periodensystem-Basics, Gase, Wasser, Salze (4 Topics)~~ ✅ erledigt (`PeriodensystemGrundlagen`, `Gase`, `Wasser`, `Salze` in `ChemieGenerator.cs`)
7. ~~Physik Klasse 6: Thermik, Kraft, Energie, Wärme (4 Topics)~~ ✅ erledigt (`WaermeausdehnungKoerper`, `WechselwirkungUndKraft`, `MechanischeEnergieUndArbeit`, `ThermischeEnergieUndWaerme` in `PhysikGenerator.cs`)

### Sprint 3: Content-Runde 2 ✅ ABGESCHLOSSEN
8. ~~Geografie Klasse 6: Risikoräume, Migration/Bevölkerung, Regenwald, Armut (4 Topics)~~ ✅ erledigt (`RisikoraeumeNaturgefahren`, `MigrationUndBevoelkerung`, `TropischerRegenwald`, `ArmutUndReichtumKlasse6` in `GeoGenerator.cs`)
9. ~~Politik Klasse 6: Armut, Globalisierte Welt, Migration, Rechtsstaat (4 Topics)~~ ✅ erledigt (`ArmutUndReichtumPolitik`, `GlobalisierteWelt`, `MigrationPolitik`, `LebenImRechtsstaat` in `PolitikGenerator.cs`)
10. ~~Ethik Klasse 6: Identität, Freiheit, Gerechtigkeit (3 Topics)~~ ✅ erledigt (`IdentitaetUndRolleKlasse6`, `FreiheitUndVerantwortungKlasse6`, `RechtUndGerechtigkeitKlasse6` in `EthikGenerator.cs`)
11. ~~Englisch Klasse 9: Alltag/Konsum, Bewerbung, Kultur (3 Topics)~~ ✅ erledigt (`AlltagUndKonsum`, `SchuleUndArbeitswelt`, `KulturUndHistorischerHintergrund` in `EnglischGenerator.cs`)
12. ~~Türkisch Klasse 9: Alltag/Konsum, Gesellschaft, Berufswelt (3 Topics)~~ ✅ erledigt (`AlltagUndKonsum`, `GesellschaftUndOeffentlichesLeben`, `SchuleUndBerufswelt` in `TurkishGenerator.cs`)
16. ~~Deutsch: Novelle/Parabel; Geschichte Klasse 9: Feindbilder/Propaganda (Bonusmodul)~~ ✅ erledigt (`Novelle`, `Parabel` in `GermanGenerator.cs`; `FeindbilderUndPropaganda` in `GeschichteGenerator.cs`)

### Später (nach finaler Version)
- **EV-Zertifikat besorgen & Installer signieren** (SmartScreen)
- ~~**Feed-URL-Healthcheck**~~ ✅ erledigt (wöchentliche GitHub Action `feed-healthcheck.yml`)
- ~~**Eltern-Export/Import**~~ ✅ erledigt (DB-Sicherung im Eltern-Bereich, siehe 3.1)

### Sprint 5: Klasse 7 + Doppeljahrgänge ✅ ABGESCHLOSSEN
17. ~~Klasse-7-Pools für alle 15 Fächer~~ ✅ erledigt (79 neue Topics, ~1.580 Fragen - inklusive der Nacharbeit in Kunst, Musik und ITG)
18. ~~Klasse 8 und 10 wählbar machen~~ ✅ erledigt (`GradeLevel.Klasse8`/`Klasse10`, Übergangsregel greift auf 7er- bzw. 9er-Pool; dabei fiel auf, dass `KidNewsMetadata` für alle Stufen außer 6 und 9 leere Einordnungstexte lieferte - behoben)

### Sprint 6: Familien-Feedback aus dem Pilotbetrieb ✅ ABGESCHLOSSEN
19. ~~Zeit-/Umfangs-Settings + Ferienmodus im Eltern-Bereich~~ ✅ erledigt
20. ~~Tipptrainer-Bestehensschwelle greift wirklich~~ ✅ erledigt (siehe 3.1)
21. ~~Lehrer-Import (PDF) reparieren~~ ✅ erledigt (LLamaSharp-Natives, siehe 3.1)
22. ~~Längen-Bias der Antworten entschärfen~~ ✅ erledigt (13 Generatoren, CI-Gate)
23. ~~KI-Bereich als eigenes Fach~~ ✅ erledigt
24. ~~Kiosk-Ausbruch über Alt+Tab und Win+Tab schließen~~ ✅ erledigt (drei Schichten, siehe 3.1)

### Sprint 7: Steuerung und eigene Inhalte ✅ ABGESCHLOSSEN
25. ~~Antwortzeit mitschreiben und im Elternbericht auswerten~~ ✅ erledigt (`AnswerPaceAnalyzer`)
26. ~~Fehler-Kartei im Willkommensbildschirm sichtbar machen~~ ✅ erledigt
27. ~~Eigene Lesetexte pro Profil~~ ✅ erledigt (`CustomReadingTextRepository`)
28. ~~Vokabeltrainer für Englisch/Türkisch~~ ✅ erledigt (`VocabularyRepository`, `VocabularyParser`)
29. ~~Wochenziel pro Profil~~ ✅ erledigt (`WeeklyGoalCalculator`)
30. ~~QWERTZ-Fehler im Tipptrainer (jkl; statt jklö, Y/Z vertauscht, zu kurze Finger-Listen)~~ ✅ erledigt
31. ~~Eigene Tipp-Texte für die beiden letzten Lektionen~~ ✅ erledigt (`TypingTextOverrides`)

### Sprint 4: Polish (1-2 Wochen)
13. ~~Mathe: Offene Zahleneingabe (neuer Fragetyp)~~ ✅ bereits umgesetzt (alle rechnerischen Topics in `MathGenerator.cs` nutzen `OpenText`; `Kongruenzabbildungen`/`SatzDesThales` bleiben als konzeptuelle Fragen bewusst Multiple-Choice)
14. ~~Lesetexte Klasse 9: Klassiker-Ergänzung (Goethe, Schiller, Fontane)~~ ✅ erledigt (`Erlkönig`, `Die Bürgschaft`, `Herr von Ribbeck auf Ribbeck im Havelland` in `ReadingContentProvider.cs`)
15. ~~Optionale Streaks (ein/ausschaltbar im Eltern-Bereich)~~ ✅ erledigt (`AppSettings.StreaksEnabled` + `StreakCalculator`, 🔥-Anzeige im Willkommensbildschirm ab 2 Tagen, Standard aus)
16. ~~Schwierigkeitsstufen pro Profil im Eltern-Bereich (Tipptrainer-Mindestgenauigkeit, Abschlussquiz-Schwellenwerte für 1./2. Versuch)~~ ✅ erledigt (`StudentProfile.TypingMinAccuracy`/`QuizFirstAttemptThreshold`/`QuizRetryThreshold`, Presets als `TabPillButton`-Gruppen im Eltern-Bereich - kein neuer Build mehr nötig, um diese Hürden zu ändern)

---

## 7. Fazit

**Die App ist funktionskomplett für den Kern-Zweck:**

> Kind loggt sich ein → **Lesen** (2 Texte, 3 Sprachen, Vorlesen) → **Tippen** (11 Lektionen + persönlicher Abschluss) → **News** (~22 Artikel: 1 pro Feed aus 22 RSS-Quellen + tägliches Finanzwissen-Erklärstück, altersgerecht) → **Fächer** (bis zu 16 aktive Fächer inkl. KI-Bereich, ~20 Fragen/Topic; Richtiges pausiert per Spaced Repetition 7/30/90 Tage und kehrt zur Auffrischung zurück) → **Abschlussquiz** (dynamisch verteilt, Bestehensschwelle pro Profil einstellbar, Standard ≥50% = PC frei) → Eltern steuern Fächer/Klassenstufe/Zeitgrenzen/Ferienmodus/LLM/Belohnungen/Schwierigkeitsstufen, sehen Wochenbericht.

**Abdeckungsgrad RLP:** Alle 15 implementierten Fach-Generatoren decken ihre RLP-Themenfelder für
Klasse 6, 7 und 9 ab (319 Topics, ~6.380 Fragen im Pool), dazu kommt der KI-Bereich als eigenes,
nicht-curriculares Fach. Über die Doppeljahrgangs-Regel sind damit alle fünf wählbaren Klassenstufen
(6, 7, 8, 9, 10) versorgt. Es gibt keine offene RLP-Content-Lücke mehr - auch die zuletzt dünnen
Klasse-7-Pools von Kunst, Musik und ITG stehen jetzt bei je vier Themen. Bewusst ausgeklammert
bleiben Sport, WAT und Standardsoftware - siehe Abschnitt 2.

**Kiosk-Härtung:** Nach drei Runden Familien-Feedback sind Alt+Tab (inkl. X-Button der Windows-11-
Vorschau) und Win+Tab/neuer virtueller Desktop geschlossen. Was in Software ehrlicherweise nicht zu
lösen ist - Strg+Alt+Entf → "Abmelden"/"Benutzer wechseln", ein zweites Windows-Konto, USB-Boot,
BIOS - ist organisatorisch in `docs/PILOT-CHECKLISTE.md` abgedeckt.

**Blocker für Produktions-Rollout:** Keiner. Die Familie installiert aus dem ZIP-Artefakt des
GitHub-Actions-Laufs, nicht über den Inno-Setup-Installer - das Installer-Signing (EV-Zertifikat)
ist damit gegenstandslos, solange das so bleibt. Alles andere ist "Qualität/Content".

---

## 8. Schnell-Check für neuen Entwickler

```powershell
# 1. Repo klonen
git clone <repo>
cd HeroKid-Lokal

# 2. Build prüfen (lokal nur auf Windows!)
dotnet restore LernTor.sln
dotnet build LernTor.sln

# 3. Tests laufen
dotnet test tests/LernTor.Tests/LernTor.Tests.csproj

# 4. Ohne Kiosk-Lock starten (Entwicklung!)
$env:LERNTOR_SKIP_LOCK = "1"
dotnet run --project src/LernTor.App

# 5. Release-Build + Installer
dotnet publish src/LernTor.App/LernTor.App.csproj -c Release -r win-x64 --self-contained true -o publish/win-x64
iscc src\LernTor.Installer\setup.iss
```

**Datenbank**: `%LOCALAPPDATA%\LernTor\lerntor.db` (SQLite, manuell löschbar für Reset)
**Logs**: `%LOCALAPPDATA%\LernTor\logs\lerntor-YYYY-MM-DD.log`
**Models**: `%LOCALAPPDATA%\LernTor\models\` (GGUF, ~2-4 GB)

---

*Bericht erstellt durch Code-Analyse (kein Laufzeit-Test). Für echte Verifikation: Windows-Rechner + GitHub Actions Build-Artefakt nutzen.*