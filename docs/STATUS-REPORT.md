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
| **Tipptrainer** | ✅ Fertig | 11 reguläre Lektionen + 1 profil-spezifische Abschluss-Lektion (Emirhan ODER Batuhan, je nach Profilname), nur Deutsch/QWERTZ, 35% Mindestgenauigkeit |

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
| **Deutsch** | `GermanGenerator.cs` | 12 | 13 | 25 | Drama-Analyse abgedeckt; Novelle/Parabel fehlen noch |
| **Türkisch** | `TurkishGenerator.cs` | 8 | 7 | 15 | Klasse 9: fehlt Alltag/Konsum, Gesellschaft, Berufswelt |
| **Englisch** | `EnglischGenerator.cs` | 7 | 6 | 13 | Klasse 9: fehlt Alltag/Konsum, Bewerbung, Kultur/Historie |
| **Biologie** | `BiologieGenerator.cs` | 4 | 8 | 12 | Klasse 6: fehlt Zelle, Lebensräume/Nahrungsketten |
| **Chemie** | `ChemieGenerator.cs` | 5 | 9 | 14 | Klasse 9 komplett (inkl. organischer Chemie); Klasse 6: fehlt Periodensystem-Basics, Gase, Wasser, Salze |
| **Physik** | `PhysikGenerator.cs` | 6 | 7 | 13 | Klasse 6: fehlt Thermik, Kraft/Wechselwirkung, mech./therm. Energie |
| **Geschichte** | `GeschichteGenerator.cs` | 3 | 6 | 9 | Klasse 9: fehlt nur Feindbilder/Propaganda (Bonusmodul) |
| **Gewi** | `GewiGenerator.cs` | 9 | 3 | 12 | Klasse 6 komplett (6/6); Klasse 9 auf Kernthemen fokussiert |
| **Politik** | `PolitikGenerator.cs` | 3 | 8 | 11 | Klasse 9 komplett (6/6, inkl. EU/Willensbildung/int. Konflikte); Klasse 6 nur Kernthemen |
| **Geografie** | `GeoGenerator.cs` | 3 | 9 | 12 | Klasse 9 komplett (6/6, inkl. Ressourcen/Klimaschutz/Globalisierung/Europa); Klasse 6 nur Kernthemen |
| **Ethik** | `EthikGenerator.cs` | 3 | 10 | 13 | Klasse 9 komplett (6/6); Klasse 6: fehlt Identität, Freiheit, Gerechtigkeit |
| **Kunst** | `KunstGenerator.cs` | 4 | 6 | 10 | ✅ |
| **Musik** | `MusikGenerator.cs` | 5 | 6 | 11 | ✅ |
| **ITG** | `ItgGenerator.cs` | 3 | 3 | 6 | Standardsoftware bewusst weggelassen (nicht quizbar) |

**Gesamt: 202 Topics × ~20 Fragen = ~4.000 Fragen im Pool**

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

| Fach | Fehlende RLP-Themenfelder | Bewertung |
|------|---------------------------|-----------|
| **Chemie Klasse 6** | Periodensystem (Klasse-6-Niveau), Gase, Wasser, Salze | Klasse 9 ist komplett (inkl. organischer Chemie - das ist NICHT mehr ausgeklammert) |
| **Physik Klasse 6** | Thermisches Verhalten, Kraft/Wechselwirkung, Mech. Energie, Therm. Energie | Nur 4 der zugehörigen RLP-Themenfelder; Basis-Themen (Aggregatzustände, Stromkreis, Magnetismus, Optik) decken Alltagswissen ab |
| **Geografie Klasse 6** | Risikoräume, Migration/Bevölkerung, tropischer Regenwald, Armut/Reichtum (Kl.6-Niveau) | Klasse 9 ist komplett (Ressourcen, Klimaschutz, Globalisierung, Europa alle vorhanden) |
| **Politik Klasse 6** | Armut/Reichtum (Kl.6), globalisierte Welt, Migration, Rechtsstaat | Klasse 9 ist komplett (Willensbildung/Medien, int. Konflikte, Friedenssicherung, EU alle vorhanden) |
| **Ethik Klasse 6** | Identität/Rolle, Freiheit/Verantwortung (Kl.6), Gerechtigkeit (Kl.6) | Klasse 9 ist komplett (alle 6 RLP-Themenfelder vorhanden) |
| **Englisch/Türkisch Klasse 9** | Alltag/Konsum, Bewerbung/Berufswelt, Kultur (Klasse-9-Niveau) | Kernthemen (Identität, Gesellschaft/Medien, Umwelt bzw. Traditionen, Geografie) sind drin |
| **Türkisch generell** | RLP gliedert in 4 Themenfelder pro Stufe; Generator ist überwiegend grammatikorientiert (Zeiten, Satzglieder, Rechtschreibung) | Funktionell für Quiz nutzbar, mehrere Wortschatz-Themenfelder inzwischen ergänzt, aber nicht 1:1 RLP-abgebildet |
| **Deutsch** | Literarische Textanalyse: Novelle, Parabel | Drama-Analyse ist inzwischen abgedeckt (`DramaAufbau`, `Figurencharakterisierung`) |
| **Biologie Klasse 6** | Die Zelle, Lebensräume/Nahrungsketten | Klasse 9 ist komplett (6/6 Themenfelder) |
| **Geschichte Klasse 9** | Feindbilder/Propaganda (Bonusmodul) | 6 von 7 Themenfeldern abgedeckt |

---

## 3. Offene To-Dos / Known Gaps

### 3.1 Technische Schulden & Bugs

| Priorität | Thema | Details |
|-----------|-------|---------|
| 🔴 **Hoch** | **EF Core Migrations fehlen** | Nutzt `EnsureCreated()` + `SqliteSchemaUpdater` (nur additive Änderungen). Bei Spalten-Umbennungen/Entfernung → manuelles DB-Löschen nötig. |
| 🔴 **Hoch** | **Installer Signing (EV-Zertifikat)** | Ohne Signatur → SmartScreen-Warnung bei Endnutzern. |
| 🟡 **Mittel** | **TTS Türkisch** | Piper-Stimmen primär Deutsch/Englisch; Türkisch fehlt/experimentell. Fallback: Windows SAPI (oft keine TR-Stimme installiert). |
| 🟡 **Mittel** | **Offline-Erst-Installation LLM** | Model-Download (~2-4 GB) passiert erst bei erstem Nutzen. Kein Pre-Bundle im Installer. |
| 🟡 **Mittel** | **Eltern-Export/Backup** | Nur "Alle Daten zurücksetzen", kein Export/Import von Profilen/Fortschritt. |
| 🟢 **Niedrig** | **Multi-Device Sync** | Nicht vorgesehen (lokal-only, SQLite). |
| 🟢 **Niedrig** | **News-Feed-URLs pflegen** | RSS-Endpunkte ändern sich gelegentlich; in `NewsFeedSource.cs` kuratiert, aber nicht automatisiert geprüft. |

### 3.2 UX / Pädagogische Lücken

| Priorität | Thema | Details |
|-----------|-------|---------|
| 🟡 **Mittel** | **Lesestufen-Texte** | 60 Texte (30 literarisch + 30 Pop-Kultur) – aber keine Klassiker-Abdeckung für Klasse 9 (z. B. Goethe/Schiller/Fontane nur rudimentär). |
| 🟡 **Mittel** | **Mathe: Offene Eingabe vs. MC** | Aktuell nur Multiple-Choice; offene Zahleneingabe wäre besser (aber schwerer zu validieren). |
| 🟢 **Niedrig** | **Gamification: Streaks** | Bewusst weggelassen (kein Druck bei verpassten Tagen), aber könnte optional ergänzt werden. |
| 🟢 **Niedrig** | **Eltern: Wochenziel-Übersicht** | Wochenbericht existiert, aber keine Zielsetzung (z. B. "3 Fächer diese Woche"). |

### 3.3 Content-Erweiterung (Nice-to-have)

*Chemie/Politik/Geografie/Ethik Klasse 9 sind entgegen früherer Fassungen dieses Berichts bereits
vollständig - die reale Restlücke liegt bei Klasse 6 und ein paar Klasse-9-Nischenthemen:*

| Fach | Fehlende Topics (RLP) | Aufwand |
|------|----------------------|---------|
| Chemie Kl.6 | Periodensystem (Kl.6-Niveau), Gase, Wasser, Salze | ~4 Topics × 20 Fragen = 80 Q |
| Physik Kl.6 | Thermik, Kraft, Energie, Wärme | ~4 Topics × 20 = 80 Q |
| Biologie Kl.6 | Zelle, Lebensräume/Nahrungsketten | ~2 Topics × 20 = 40 Q |
| Geografie Kl.6 | Risikoräume, Migration, Regenwald, Armut (Kl.6) | ~4 Topics × 20 = 80 Q |
| Politik Kl.6 | Armut (Kl.6), Globalisierte Welt, Migration, Rechtsstaat | ~4 Topics × 20 = 80 Q |
| Ethik Kl.6 | Identität, Freiheit, Gerechtigkeit (Kl.6-Niveau) | ~3 Topics × 20 = 60 Q |
| Englisch/Türkisch Kl.9 | Alltag/Konsum, Bewerbung/Berufswelt, Kultur | ~5 Topics × 20 = 100 Q |
| Deutsch | Novelle, Parabel | ~2 Topics × 20 = 40 Q |
| Geschichte Kl.9 | Feindbilder/Propaganda (Bonusmodul) | ~1 Topic × 20 = 20 Q |

**Gesamt Content-Lücke: ~580 Fragen** (bei 202 existierenden Topics/~4.000 Fragen = ~14%
Erweiterungsmöglichkeit, deutlich kleiner als frühere Schätzungen).

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

### Sprint 1: Produktionsreife (2-3 Wochen)
1. **EV-Zertifikat besorgen & Installer signieren** (SmartScreen)
2. **Türkische TTS-Stimme** recherchieren (Piper Community / eSpeak-NG Fallback)
3. **Feed-URL-Healthcheck** (kleines Skript, wöchentlich via Cron/GitHub Action)
4. **Eltern-Export/Import** (JSON-Backup von Profilen + Fortschritt)

### Sprint 2: Content-Runde 1 (2 Wochen)
5. Chemie Klasse 6: Periodensystem-Basics, Gase, Wasser, Salze (4 Topics)
6. Physik Klasse 6: Thermik, Kraft, Energie, Wärme (4 Topics)
7. Biologie Klasse 6: Zelle, Lebensräume/Nahrungsketten (2 Topics)

### Sprint 3: Content-Runde 2 (2 Wochen)
8. Politik/Geografie Klasse 6: Armut, Globalisierte Welt, Migration, Rechtsstaat, Risikoräume, Regenwald (bis zu 8 Topics)
9. Ethik Klasse 6: Identität, Freiheit, Gerechtigkeit (3 Topics)
10. Englisch/Türkisch Klasse 9: Alltag/Konsum, Bewerbung (bis zu 4 Topics)

### Sprint 4: Polish (1-2 Wochen)
11. Mathe: Offene Zahleneingabe (neuer Fragetyp)
12. Lesetexte Klasse 9: Klassiker-Ergänzung (Goethe, Schiller, Fontane)
13. Optionale Streaks (ein/ausschaltbar im Eltern-Bereich)
14. Deutsch: Novelle/Parabel; Geschichte Klasse 9: Feindbilder/Propaganda (Bonusmodul)

---

## 7. Fazit

**Die App ist funktionskomplett für den Kern-Zweck:**

> Kind loggt sich ein → **Lesen** (2 Texte, 3 Sprachen, Vorlesen) → **Tippen** (11 Lektionen + persönlicher Abschluss) → **News** (~22 Artikel: 1 pro Feed aus 22 RSS-Quellen + tägliches Finanzwissen-Erklärstück, altersgerecht) → **Fächer** (bis zu 15 aktive Fächer, ~20 Fragen/Topic, keine Wiederholung bei richtig) → **Abschlussquiz** (dynamisch verteilt, ≥50% = PC frei) → Eltern steuern Fächer/Noten/LLM/Belohnungen, sehen Wochenbericht.

**Abdeckungsgrad RLP:** deutlich höher als frühere Fassungen dieses Berichts annahmen - Chemie,
Politik, Geografie und Ethik Klasse 9 sind inzwischen vollständig (6/6 RLP-Themenfelder). Die
verbleibende Lücke liegt überwiegend bei Klasse-6-Grundthemen (Physik, Chemie, Biologie, Geografie,
Politik, Ethik) sowie ein paar Klasse-9-Nischenthemen (Englisch/Türkisch Alltag/Bewerbung, Deutsch
Novelle/Parabel) - dazu bewusst ausgeklammerte Bereiche (Sport, WAT, Standardsoftware).

**Blocker für Produktions-Rollout:** Nur **Installer-Signing (EV-Zertifikat)**. Alles andere ist "Qualität/Content", kein Blocker.

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