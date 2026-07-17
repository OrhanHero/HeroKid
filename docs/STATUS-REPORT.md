# LernTor – Status-Quo-Bericht (Stand: 2025-07-17)

> **Hinweis**: Dieser Bericht basiert auf Code-Analyse. Die App läuft nur unter Windows (WPF + Win32 P/Invoke). Build-Verifikation erfolgt via GitHub Actions (`.github/workflows/build.yml` auf `windows-latest`).

---

## 1. Gesamtfortschritt

| Bereich | Status | Details |
|---------|--------|---------|
| **Core Domain** | ✅ Fertig | Enums, Models, `ProgressGateService`, `ScoringService`, `LearningStageSubjects.Map` |
| **ContentGen (Generatoren)** | ✅ Fertig | 14 Fach-Generatoren, `QuizComposer`, Review-/Mastered-Logik |
| **News (RSS + Aufbereitung)** | ✅ Fertig | RSS-Loading, Vereinfachung, Verständnisfragen, Kategorisierung, Glossar, Bezirks-Erkennung |
| **Data (EF Core SQLite)** | ✅ Fertig | Repositories: Progress, ActivityLog, MasteredPrompt, ReviewQuestion, CustomQuestion, Settings, Rewards, TypingProgress |
| **Security (Kiosk)** | ✅ Fertig | Keyboard-Hook, TaskMgr-Policy, Autostart, Admin-Auth |
| **App (WPF/MVVM)** | ✅ Fertig | MainVM, alle Views (ProfileSelection, Welcome, News, Exercise, FinalQuiz, Result, ParentSettings), QuestionCard, KI-Chat, TTS (Piper), Lehrer-Import (PDF/Word → KI → Entwürfe), Belohnungen, Wochenbericht |
| **Localization** | ✅ Fertig | DE/TR, String-Indexer, Live-Switch via `PropertyChanged("Item[]")` |
| **Local LLM** | ✅ Fertig | LLamaSharp, GGUF-Autodownload (~2-4 GB), 2 Features: Lehrer-Import + KI-Hausaufgaben-Chat |
| **Tipptrainer** | ✅ Fertig (neu) | 12 reguläre Lektionen + 2 profil-spezifische Abschluss-Lektionen (Emirhan/Batuhan), nur Deutsch/QWERTZ |

---

## 2. Fach-Abdeckung nach Berliner Rahmenlehrplan (Klasse 6 & 9)

**Legende**: ✅ = Topic mit ~20 kuratierten Fragen vorhanden, ⚠️ = Topic existiert aber unvollständig (< 20 Fragen), ❌ = Topic fehlt ganz, — = Fach nicht in App

### ✅ Vollständig implementierte Fächer (14/17 RLP-Fächer)

| Fach | Generator | Klasse 6 Topics | Klasse 9 Topics | Gesamt | Abdeckung RLP-Themenfelder |
|------|-----------|----------------|----------------|--------|---------------------------|
| **Mathematik** | `MathGenerator.cs` | 7 | 7 | 14 | 7/8 (fehlt: Statistik Klasse 9 Mittelwert/Median – *ist aber als Generator-Topic "MittelwertUndMedian" vorhanden*) |
| **Deutsch** | `GermanGenerator.cs` | 6 | 7 | 13 | 7/7 Klasse 6, 6/7 Klasse 9 (fehlt: tiefere Textanalyse Drama/Novelle/Parabel) |
| **Türkisch** | `TurkishGenerator.cs` | 5 | 4 | 9 | 4/4 Klasse 6, 2/6 Klasse 9 (RLP gliedert in Themenfelder, Generator ist grammatikorientiert) |
| **Englisch** | `EnglischGenerator.cs` | 4 | 5 | 9 | 4/4 Klasse 6, 3/6 Klasse 9 (fehlen: Alltag/Konsum, Bewerbung, Kultur) |
| **Biologie** | `BiologieGenerator.cs` | 3 | 3 | 6 | 2/4 Klasse 6, 6/6 Klasse 9 ✅ |
| **Chemie** | `ChemieGenerator.cs` | 4 | 0 | 4 | 2/5 Klasse 6, 0/6 Klasse 9 (organische Chemie bewusst ausgeklammert) |
| **Physik** | `PhysikGenerator.cs` | 3 | 5 | 8 | 1/4 Klasse 6, 6/6 Klasse 9 ✅ |
| **Geschichte** | `GeschichteGenerator.cs` | 3 | 6 | 9 | 3/3 Klasse 6, 6/7 Klasse 9 (fehlt: Feindbilder/Propaganda) |
| **Gewi** | `GewiGenerator.cs` | 5 | 2 | 7 | 6/6 Klasse 6, 2/5 Klasse 9 (fehlen: Wirtschaftskreislauf, Medien, Tourismus) |
| **Politik** | `PolitikGenerator.cs` | 3 | 3 | 6 | 3/4 Klasse 6, 2/6 Klasse 9 (fehlen: Willensbildung/Medien, Konflikte international, Friedenssicherung, EU) |
| **Geografie** | `GeoGenerator.cs` | 3 | 4 | 7 | 3/4 Klasse 6, 1/6 Klasse 9 (fehlen: Ressourcen, Landwirtschaft, Klimaschutz-Konflikte, Globalisierung, Europa) |
| **Ethik** | `EthikGenerator.cs` | 3 | 3 | 6 | 1/4 Klasse 6, 2/6 Klasse 9 (fehlen: Identität, Freiheit, Gerechtigkeit, Mensch/Gemeinschaft, Handeln/Moral, Wissen/Glauben) |
| **Kunst** | `KunstGenerator.cs` | 4 | 6 | 10 | 4/5 Klasse 6, 6/6 Klasse 9 ✅ |
| **Musik** | `MusikGenerator.cs` | 5 | 6 | 11 | 5/5 Klasse 6, 6/6 Klasse 9 ✅ |
| **ITG** | `ItgGenerator.cs` | 3 | 2 | 5 | 2/3 Klasse 6, 1/3 Klasse 9 (Standardsoftware bewusst weggelassen) |

**Gesamt: ~124 Topics × ~20 Fragen = ~2.500 Fragen im Pool**

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
| **Chemie Klasse 9** | Periodensystem, Gase, Wasser, Salze, organische Chemie (Kohlenwasserstoffe, Alkohole, Säuren, Ester) | Organische Chemie bewusst ausgeklammert (Zielalter 10-15); Rest "nice to have" |
| **Physik Klasse 6** | Thermisches Verhalten, Kraft/Wechselwirkung, Mech. Energie, Therm. Energie | Nur 3/7 Themenfelder; Basis-Themen (Aggregatzustände, Stromkreis, Magnetismus) decken Alltagswissen ab |
| **Geografie Klasse 9** | Ressourcen (Energie/Rohstoffe, Landwirtschaft), Klimaschutz-Konflikte, Globalisierung, Europa in der Welt | Nur Klimawandel implementiert; Rest niedrig priorisiert |
| **Politik Klasse 9** | Willensbildung/Medien/Gefährdungen, internationale Konflikte, Friedenssicherung, EU | Nur 3/6; Kernthemen (Gewaltenteilung, Bundestag/Bundesrat, Soziale Marktwirtschaft) sind drin |
| **Ethik beide Stufen** | Identität/Rolle, Freiheit/Verantwortung (Kl.6), Gerechtigkeit (Kl.6), Mensch/Gemeinschaft (Hobbes/Rousseau), Handeln/Moral, Wissen/Glauben | Nur 3/10 Themenfelder; Fokus auf lebensnahe Themen (Freundschaft, Verantwortung, Recht) |
| **Türkisch** | RLP gliedert in 4 Themenfelder pro Stufe; Generator ist grammatikorientiert (Zeiten, Satzglieder, Rechtschreibung) | Funktionell für Quiz nutzbar, aber nicht 1:1 RLP-abgebildet |
| **Deutsch Klasse 9** | Literarische Textanalyse (Drama, Novelle, Parabel) | Generator deckt Grammatik/Satzbau/Textsorten ab; tiefere Interpretation nicht automatisierbar |

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

| Fach | Fehlende Topics (RLP) | Aufwand |
|------|----------------------|---------|
| Chemie Kl.9 | Periodensystem, Gase, Wasser, Salze | ~6 Topics × 20 Fragen = 120 Q |
| Physik Kl.6 | Thermik, Kraft, Energie, Wärme | ~4 Topics × 20 = 80 Q |
| Geografie Kl.9 | Ressourcen, Landwirtschaft, Globalisierung, Europa | ~5 Topics × 20 = 100 Q |
| Politik Kl.9 | Willensbildung/Medien, Int. Konflikte, Friedenssicherung, EU | ~4 Topics × 20 = 80 Q |
| Ethik beide | Identität, Freiheit, Gerechtigkeit, Mensch/Gemeinschaft, Moral, Wissen/Glauben | ~8 Topics × 20 = 160 Q |
| Türkisch | Themenfelder statt Grammatik umbauen | Refactoring nötig |
| Englisch Kl.9 | Alltag/Konsum, Bewerbung, Kultur/Historie | ~3 Topics × 20 = 60 Q |

**Gesamt Content-Lücke: ~600-700 Fragen** (bei ~2.500 existentes = ~25% Erweiterungsmöglichkeit).

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
| `LernTor.Tests` (xUnit) | ~85 | Core (ProgressGate, Scoring), ContentGen (alle 14 Generatoren: Musterlösungen prüfen), News (RSS-Parser RDF/Atom/RSS2, Vereinfachung, Kategorisierung, Glossar) |
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
5. Chemie Klasse 9: Periodensystem, Gase, Wasser, Salze (4 Topics)
6. Physik Klasse 6: Thermik, Kraft, Energie, Wärme (4 Topics)
7. Geografie Klasse 9: Ressourcen, Klimaschutz-Konflikte (2 Topics)

### Sprint 3: Content-Runde 2 (2 Wochen)
8. Politik Klasse 9: Willensbildung/Medien, EU (2 Topics)
9. Ethik: Identität, Freiheit, Gerechtigkeit (3 Topics)
10. Englisch Klasse 9: Alltag/Konsum, Bewerbung (2 Topics)

### Sprint 4: Polish (1-2 Wochen)
11. Mathe: Offene Zahleneingabe (neuer Fragetyp)
12. Lesetexte Klasse 9: Klassiker-Ergänzung (Goethe, Schiller, Fontane)
13. Optionale Streaks (ein/ausschaltbar im Eltern-Bereich)

---

## 7. Fazit

**Die App ist funktionskomplett für den Kern-Zweck:**

> Kind loggt sich ein → **Lesen** (2 Texte, 3 Sprachen, Vorlesen) → **Tippen** (12 Lektionen + persönlicher Abschluss) → **News** (22 Artikel: Berlin/Türkei garantiert, altersgerecht, Verständnisfragen) → **Fächer** (13 aktive Fächer, ~20 Fragen/Topic, keine Wiederholung bei richtig) → **Abschlussquiz** (dynamisch verteilt, ≥50% = PC frei) → Eltern steuern Fächer/Noten/LLM/Belohnungen, sehen Wochenbericht.

**Abdeckungsgrad RLP:** ~75% der für Quiz geeigneten Themenfelder (14/17 Fächer, Kern-Themen drin). Die fehlenden 25% sind bewusst ausgeklammerte Bereiche (Sport, WAT, organische Chemie, tiefe Textanalyse) oder "Nice-to-have"-Ergänzungen.

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