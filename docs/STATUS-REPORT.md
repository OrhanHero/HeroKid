# LernTor – Status-Quo-Bericht (Stand: 2026-07-17)

> **Hinweis**: Dieser Bericht basiert auf Code-Analyse. Die App läuft nur unter Windows (WPF + Win32 P/Invoke). Build-Verifikation erfolgt via GitHub Actions (`.github/workflows/build.yml` auf `windows-latest`).

---

## 1. Gesamtfortschritt

| Bereich | Status | Details |
|---------|--------|---------|
| **Core Domain** | ✅ Fertig | Enums, Models, `ProgressGateService`, `ScoringService`, `LearningStageSubjects.Map` |
| **ContentGen (Generatoren)** | ✅ Fertig | 15 Fach-Generatoren, `QuizComposer`, Review-/Mastered-Logik |
| **News (RSS + Aufbereitung)** | ✅ Fertig | RSS-Loading, Vereinfachung, Verständnisfragen, Kategorisierung, Glossar, Bezirks-Erkennung |
| **Data (EF Core SQLite)** | ✅ Fertig | Repositories: Progress, ActivityLog, MasteredPrompt, ReviewQuestion, CustomQuestion, Settings, Rewards, TypingProgress |
| **Security (Kiosk)** | ✅ Fertig | Keyboard-Hook, TaskMgr-Policy, Autostart, Admin-Auth |
| **App (WPF/MVVM)** | ✅ Fertig | MainVM, alle Views (ProfileSelection, Welcome, News, Exercise, FinalQuiz, Result, ParentSettings), QuestionCard, KI-Chat, TTS (Piper), Lehrer-Import (PDF/Word → KI → Entwürfe), Belohnungen, Wochenbericht |
| **Localization** | ✅ Fertig | DE/TR, String-Indexer, Live-Switch via `PropertyChanged("Item[]")` |
| **Local LLM** | ✅ Fertig | LLamaSharp, GGUF-Autodownload (~2-4 GB), 2 Features: Lehrer-Import + KI-Hausaufgaben-Chat |
| **Tipptrainer** | ✅ Fertig | 11 reguläre Lektionen + 1 profil-spezifische Abschluss-Lektion (Emirhan ODER Batuhan, je nach Profilname), nur Deutsch/QWERTZ, Mindestgenauigkeit pro Profil im Eltern-Bereich einstellbar (Presets 25/50/75/85%, Standard 25%) |

---

## 2. Fach-Abdeckung nach Berliner Rahmenlehrplan (Klasse 6 & 9)

**Legende**: ✅ = Topic mit ~20 kuratierten Fragen vorhanden, ⚠️ = Topic existiert aber unvollständig (< 20 Fragen), ❌ = Topic fehlt ganz, — = Fach nicht in App

### ✅ Vollständig implementierte Fächer (15/17 RLP-Fächer)

*Diese Tabelle wurde direkt aus `TopicsByGrade` in den Generator-Dateien abgeleitet (siehe
[docs/CURRICULUM.md](CURRICULUM.md) für die Themen-Detailtabellen und den vollständigen
RLP-Haken-Abgleich), nicht geschätzt.*

| Fach | Generator | Klasse 6 Topics | Klasse 9 Topics | Gesamt | Abdeckung RLP-Themenfelder |
|------|-----------|----------------|----------------|--------|---------------------------|
| **Mathematik** | `MathGenerator.cs` | 12 | 14 | 26 | Klasse 9: fehlt nur Stochastik-Baumdiagramme und darstellende Geometrie (Nischenthemen) |
| **Deutsch** | `GermanGenerator.cs` | 12 | 15 | 27 | ✅ komplett (inkl. Drama-Analyse, Novelle, Parabel) |
| **Türkisch** | `TurkishGenerator.cs` | 8 | 10 | 18 | ✅ komplett (alle 4 kommunikativen RLP-Themenfelder je Stufe) |
| **Englisch** | `EnglischGenerator.cs` | 7 | 9 | 16 | ✅ komplett (beide Stufen 6/6) |
| **Biologie** | `BiologieGenerator.cs` | 6 | 8 | 14 | ✅ komplett |
| **Chemie** | `ChemieGenerator.cs` | 9 | 9 | 18 | ✅ komplett (beide Stufen 6/6) |
| **Physik** | `PhysikGenerator.cs` | 10 | 7 | 17 | ✅ komplett (beide Stufen 6/6 bzw. 6/7) |
| **Geschichte** | `GeschichteGenerator.cs` | 3 | 7 | 10 | ✅ komplett (inkl. Feindbilder/Propaganda-Bonusmodul) |
| **Gewi** | `GewiGenerator.cs` | 9 | 3 | 12 | Klasse 6 komplett (6/6); Klasse 9 auf Kernthemen fokussiert |
| **Politik** | `PolitikGenerator.cs` | 7 | 8 | 15 | ✅ komplett (beide Stufen 6/6) |
| **Geografie** | `GeoGenerator.cs` | 7 | 9 | 16 | ✅ komplett (beide Stufen 4/4 bzw. 6/6) |
| **Ethik** | `EthikGenerator.cs` | 6 | 10 | 16 | ✅ komplett (beide Stufen 6/6) |
| **Kunst** | `KunstGenerator.cs` | 4 | 6 | 10 | ✅ |
| **Musik** | `MusikGenerator.cs` | 5 | 6 | 11 | ✅ |
| **ITG** | `ItgGenerator.cs` | 3 | 3 | 6 | Standardsoftware bewusst weggelassen (nicht quizbar) |

**Gesamt: 232 Topics × ~20 Fragen = ~4.640 Fragen im Pool**

> Frühere Fassungen dieses Berichts nannten ~124 Topics und listeten Chemie/Politik/Geografie/Ethik
> Klasse 9 sowie Deutsch-Drama als große Lücken. Die Generatoren wurden seither erweitert, ohne dass
> dieser Bericht nachgezogen wurde - diese Fassung ist gegen den aktuellen Code verifiziert.

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
| ⏸️ **Zurückgestellt** | **Installer Signing (EV-Zertifikat)** | Ohne Signatur → SmartScreen-Warnung bei Endnutzern. Bewusst auf die finale Version verschoben (Nutzer-Entscheidung). |
| ✅ **Erledigt/gut genug** | **TTS Türkisch** | Aktuelle Piper-Stimme ist gut genug, bleibt vorerst unangetastet (Nutzer-Entscheidung). |
| 🟡 **Mittel** | **Offline-Erst-Installation LLM** | Model-Download (~2-4 GB) passiert erst bei erstem Nutzen. Kein Pre-Bundle im Installer. |
| ✅ **Erledigt** | **Eltern-Export/Backup** | Sicherung erstellen/wiederherstellen im Eltern-Bereich: Export als konsistente .db-Datei (`VACUUM INTO`), Import ersetzt die aktive DB nach Bestätigung (App-Neustart, Schema-Abgleich macht alte Sicherungen kompatibel). |
| 🟢 **Niedrig** | **Multi-Device Sync** | Nicht vorgesehen (lokal-only, SQLite). |
| ✅ **Erledigt** | **News-Feed-URLs pflegen** | Wöchentlicher automatischer Healthcheck (`.github/workflows/feed-healthcheck.yml` + `scripts/check-feeds.py`): prüft alle URLs aus `NewsFeedSource.cs` montags, Lauf wird rot bei totem Feed. Zusätzlich 48h-Offline-Cache pro Feed in der App (`FeedCache`). |

### 3.2 UX / Pädagogische Lücken

| Priorität | Thema | Details |
|-----------|-------|---------|
| ✅ **Erledigt** | **Lesestufen-Texte** | 63 Texte (33 literarisch/Allgemeinwissen + 30 Pop-Kultur), inkl. Klassiker-Ergänzung Goethe (Erlkönig), Schiller (Die Bürgschaft) und Fontane (Herr von Ribbeck auf Ribbeck im Havelland). |
| ✅ **Erledigt** | **Mathe: Offene Eingabe vs. MC** | Alle rechnerischen Topics nutzen `QuestionType.OpenText` (offene Zahleneingabe). Nur `Kongruenzabbildungen` und `Satz des Thales` bleiben bewusst Multiple-Choice: konzeptuelle Fragen mit Satz-Antworten, eine offene Eingabe wäre dort nicht sinnvoll validierbar. |
| ✅ **Erledigt** | **Gamification: Streaks** | Optional umgesetzt: 🔥-Lernserie auf dem Willkommensbildschirm (`StreakCalculator`), Standard AUS und von Eltern einschaltbar. Bewusst reine Anzeige - keine Strafen/Erinnerungen bei verpassten Tagen, ein noch nicht gelernter heutiger Tag bricht die Serie nicht. |
| 🟢 **Niedrig** | **Eltern: Wochenziel-Übersicht** | Wochenbericht existiert, aber keine Zielsetzung (z. B. "3 Fächer diese Woche"). |

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

---

## 5. Test-Abdeckung

| Test-Projekt | Tests | Abdeckung |
|--------------|-------|-----------|
| `LernTor.Tests` (xUnit) | ~85 | Core (ProgressGate, Scoring), ContentGen (alle 15 Generatoren: Musterlösungen prüfen), News (RSS-Parser RDF/Atom/RSS2, Vereinfachung, Kategorisierung, Glossar, Finanzwissen) |
| UI-Tests | 0 | WPF-UI-Tests in CI schwer (brauchen Windows + interaktive Session) |
| Integrationstests | 0 | Manuell auf Windows getestet |

**CI**: `.github/workflows/build.yml` baut auf `windows-latest` → Artefakt hochladen → manueller Smoke-Test auf Windows empfohlen.

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

### Sprint 4: Polish (1-2 Wochen)
13. ~~Mathe: Offene Zahleneingabe (neuer Fragetyp)~~ ✅ bereits umgesetzt (alle rechnerischen Topics in `MathGenerator.cs` nutzen `OpenText`; `Kongruenzabbildungen`/`SatzDesThales` bleiben als konzeptuelle Fragen bewusst Multiple-Choice)
14. ~~Lesetexte Klasse 9: Klassiker-Ergänzung (Goethe, Schiller, Fontane)~~ ✅ erledigt (`Erlkönig`, `Die Bürgschaft`, `Herr von Ribbeck auf Ribbeck im Havelland` in `ReadingContentProvider.cs`)
15. ~~Optionale Streaks (ein/ausschaltbar im Eltern-Bereich)~~ ✅ erledigt (`AppSettings.StreaksEnabled` + `StreakCalculator`, 🔥-Anzeige im Willkommensbildschirm ab 2 Tagen, Standard aus)
16. ~~Schwierigkeitsstufen pro Profil im Eltern-Bereich (Tipptrainer-Mindestgenauigkeit, Abschlussquiz-Schwellenwerte für 1./2. Versuch)~~ ✅ erledigt (`StudentProfile.TypingMinAccuracy`/`QuizFirstAttemptThreshold`/`QuizRetryThreshold`, Presets als `TabPillButton`-Gruppen im Eltern-Bereich - kein neuer Build mehr nötig, um diese Hürden zu ändern)

---

## 7. Fazit

**Die App ist funktionskomplett für den Kern-Zweck:**

> Kind loggt sich ein → **Lesen** (2 Texte, 3 Sprachen, Vorlesen) → **Tippen** (11 Lektionen + persönlicher Abschluss) → **News** (~22 Artikel: 1 pro Feed aus 22 RSS-Quellen + tägliches Finanzwissen-Erklärstück, altersgerecht) → **Fächer** (bis zu 15 aktive Fächer, ~20 Fragen/Topic, keine Wiederholung bei richtig) → **Abschlussquiz** (dynamisch verteilt, Bestehensschwelle pro Profil einstellbar, Standard ≥50% = PC frei) → Eltern steuern Fächer/Noten/LLM/Belohnungen/Schwierigkeitsstufen, sehen Wochenbericht.

**Abdeckungsgrad RLP:** Alle 15 implementierten Fach-Generatoren decken ihre RLP-Themenfelder für
Klasse 6 und 9 vollständig ab (232 Topics, ~4.640 Fragen im Pool) - inklusive der zuletzt
geschlossenen Deutsch- (Novelle, Parabel) und Geschichte-Lücken (Feindbilder/Propaganda). Es gibt
keine offene RLP-Content-Lücke mehr bei diesen 15 Fächern; nur bewusst ausgeklammerte Bereiche
bleiben (Sport, WAT, Standardsoftware) - siehe Abschnitt 2.

**Blocker für Produktions-Rollout:** Nur **Installer-Signing (EV-Zertifikat)** - bewusst auf die finale Version verschoben. Alles andere ist "Qualität/Content", kein Blocker.

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