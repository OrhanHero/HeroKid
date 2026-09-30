# Nächste Schritte

Stand: 30.09.2026. Diese Seite ist die Arbeitsliste. Was erledigt wurde, steht unten unter
[Erledigt am 29.09.2026](#erledigt-am-29092026-version-20) und
[Erledigt am 28.09.2026](#erledigt-am-28092026). Die Begründungen und die ältere Planung stehen in
[`PLAN.md`](PLAN.md) und [`NAECHSTES-LEVEL.md`](NAECHSTES-LEVEL.md).

Die Reihenfolge richtet sich nach dem, was die Kinder als Nächstes merken würden, nicht nach
technischer Eleganz.

> **Nächste Version (Plan vom 30.09.2026): 3.2 – Rahmenlehrplan**: jedes Themenfeld des Berliner
> Rahmenlehrplans für 5/6, 7/8 und 9/10 im Code verzeichnet und per Test mit den Fragen verknüpft,
> Klasse 7/8 und 9/10 vollständig, Rahmenlehrplan-Abdeckung im Eltern-Bericht:
> [`NAECHSTES-LEVEL-3-2.md`](NAECHSTES-LEVEL-3-2.md).
>
> **Neu (30.09.2026): Version 3.1 – Funktion** ([Release v3.1.0](https://github.com/OrhanHero/HeroKid/releases/tag/v3.1.0)) – jede Frage vorlesen (englische und
> türkische Sätze in der passenden Stimme), Lernkalender in „Mein Fortschritt“, zwei neue
> Belohnungs-Designs, Lehrplan-Lücken, Schulkalender 2027/28:
> [`NAECHSTES-LEVEL-3-1.md`](NAECHSTES-LEVEL-3-1.md).
>
> **Neu (30.09.2026): Version 3.0** ([Release v3.0.0](https://github.com/OrhanHero/HeroKid/releases/tag/v3.0.0)) –
> wählbare Designs (auch dunkel und mit hohem Kontrast), Schrift und Textgröße je Kind,
> Bildschirmfotos und Kontrasttests in der CI, Entwicklungsumgebung für jede Sitzung. Plan und
> Fahrplan bis 2028: [`NAECHSTES-LEVEL-3.md`](NAECHSTES-LEVEL-3.md), Aufbau: [`DESIGN.md`](DESIGN.md).
>
> **Neu (29.09.2026): Version 2.0** – .NET 10, Meisterschaft je Thema, Abzeichen, eine
> Fehler-Kartei-Zahl, die stimmt, größere Englisch- und Musikpools, stärkeres Eltern-Passwort,
> Systeminfo. Plan und Begründung: [`NAECHSTES-LEVEL.md`](NAECHSTES-LEVEL.md).

---

## 1. Jetzt: die Familie (kein Code nötig)

- [x] ~~Pull Request [OrhanHero/HeroKid#1](https://github.com/OrhanHero/HeroKid/pull/1) mergen
  und die neue Version installieren~~ – erledigt: Releases v2.0.0, v2.1.0 und
  [v3.0.0](https://github.com/OrhanHero/HeroKid/releases/tag/v3.0.0) liegen auf GitHub. Damit ist
  auch die Frist 10.11.2026 (Ende von .NET 8) erfüllt.
- [ ] **Vorher eine Sicherung auf USB** (Eltern-Bereich → „Sicherung erstellen…“). Das Update
  ergänzt eine Spalte und eine Tabelle in der Datenbank; die automatische `-schema.db`-Sicherung
  entsteht zwar von selbst, liegt aber auf derselben Platte.
- [ ] **Stundenpläne eintragen**: Text und Zeitraster aus
  [`STUNDENPLAENE-2026-27.md`](STUNDENPLAENE-2026-27.md) in den Eltern-Bereich einfügen.
- [ ] Bei Batuhan am Original nachsehen: **Ethik** nur Do 6? **Mathe** Fr 6/7 als Doppelstunde?
- [ ] Nach dem Eintragen **„🔍 Datenbank prüfen“** einmal drücken. Das Ergebnis muss ✅ sein.
- [x] **Version 3.0 installiert und angesehen** – Rückmeldung am 30.09.2026: „alles
  funktioniert“. Zum Nachschlagen: Testplan [V.2-1 bis V.3-8](TESTPLAN.md#neu-seit-30092026-version-21-und-30), dazu
  unter Actions das Artefakt „Design-Vorschau“ des letzten Laufs durchblättern.
- [ ] Die neuen Punkte im Testplan abhaken: [acht vom 28.09.](TESTPLAN.md#neu-seit-28092026) und
  [zehn für Version 2.0](TESTPLAN.md#neu-seit-29092026-version-20), zusammen etwa 45 Minuten.
  Am wichtigsten: **V.2** (Eltern-Passwort geht nach dem Update noch) und **V.7** (der Knopf
  „PC jetzt benutzen“ ist nach dem Abschlussquiz ohne Scrollen sichtbar).

## 2. In den nächsten Wochen: Türkisch-Pool vergrößern

`python3 scripts/pool-reichweite.py` zeigt, wie lange die Fragen je Fach bei der Übung nach
Stundenplan reichen:

| Kind | Fach | Fragen | reicht für |
|---|---|---:|---:|
| Emirhan | Türkisch (Kl. 6) | ~~160~~ ~~260~~ 360 | ~~5~~ ~~8,7~~ **12 Wochen** |
| Batuhan | Türkisch (Kl. 9) | ~~200~~ ~~300~~ 400 | ~~7~~ ~~10~~ **13,3 Wochen** |
| Emirhan | Englisch (Kl. 6) | ~~140~~ 200 | ~~8~~ **~11 Wochen** |
| Emirhan | Musik (Kl. 6) | ~~100~~ 140 | ~~8,3~~ **~11,7 Wochen** |
| Batuhan | Englisch (Kl. 9) | ~~180~~ 220 | ~~10~~ **~12 Wochen** |

Türkisch ist jeden Tag dabei und deshalb als erstes durch. Danach kommen nur noch fällige
Wiederholungen (7/30/90 Tage) und die Fehler-Kartei. Das ist nicht falsch, fühlt sich aber
nach „immer dasselbe“ an.

- [x] **Türkisch Klasse 6 und Klasse 9 um je 100 Fragen erweitert** (28.09.2026, je fünf
  Themen à 20; Längen-Bias 35 %). Reichweite jetzt: Emirhan ~8,7 Wochen (260 Fragen),
  Batuhan 10 Wochen (300 Fragen). Ein Test hält die Poolgröße fest.
- [x] **Englisch Klasse 6 +60, Musik Klasse 6 +40, Englisch Klasse 9 +40 Fragen** (29.09.2026,
  Simple Past, Pronomen, in/on/at, Notenwerte/Takt, Stimme/Gesang, Second Conditional, Relative
  Clauses). `PoolReichweiteTests` hält die Poolgrößen fest.
- [x] `pool-reichweite.py` erneut gelaufen: kein Fach unter 8 Wochen; am kürzesten reicht jetzt
  Türkisch Klasse 6 (8,7 Wochen, jeden Tag dabei).
- [x] **Türkisch ein zweites Mal erweitert** (30.09.2026): Klasse 6 +100 (İyelik ekleri, Geniş
  zaman, Emir kipi, Vücut ve sağlık, Karşılaştırma), Klasse 9 +100 (Fiil çatısı, Ek fiil,
  Paragrafta anlam, Bağlaçlar/Edatlar, Anlatım bozuklukları). Am kürzesten reicht jetzt Englisch
  Klasse 6 mit 11 Wochen.
- [ ] Die neuen Englischfragen von jemandem mit gutem Englisch gegenlesen lassen.

Die Türkisch-Fragen sollten von jemandem gegengelesen werden, der Türkisch als Muttersprache
spricht.

## 3. Nach 3–4 Wochen Betrieb: auswerten

- [ ] **Fehlerprotokoll** im Eltern-Bereich lesen, besonders Einträge mit
  `[Etappen]` (das Gate sah eine Etappe als offen, die übersprungen wurde) und `[Sitzung]`
  (Tageswechsel über Nacht).
- [ ] **Tageslänge**: Wie lange brauchen die Kinder jetzt, mit 5–6 statt 15 Fächern?
  Davon hängt ab, ob das **Zeitbudget in Minuten** (der offene Rest von Plan 2.0) überhaupt
  noch gebraucht wird. Ist der Tag kurz genug, bleibt es beim Stundenplan.
- [ ] **Abschlussquiz**: Mit 5–6 Fächern bekommt jedes Fach 3–4 Fragen statt einer. Passt die
  Bestehensschwelle noch, oder ist es zu leicht oder zu schwer geworden?
- [ ] Fragen die Kinder „warum kommt die Frage schon wieder?“ Dann gilt Abschnitt 2.

## 4. Fällige Termine

| Wann | Was | Wie |
|---|---|---|
| **Frühjahr 2028** | Ferien 2028/29 und Feiertage 2029 eintragen (seit 3.1 reicht der Kalender bis zu den Sommerferien 2028 bzw. bis 26.12.2028) | `SchoolCalendar.cs`, Daten von der Senatsverwaltung; der Eltern-Bereich zeigt das Enddatum an |
| **Sommer 2027** | Klassenstufe wechseln: Batuhan 9 → 10, Emirhan 6 → 7 | Eltern-Bereich → Profil → „Klassenstufe übernehmen“ (seit 28.09.2026; der Hinweis erscheint in den ersten vier Schulwochen von selbst) |
| **Sommer 2027** | Neue Stundenpläne 2027/28 eintragen | wie oben; `STUNDENPLAENE-2026-27.md` als Vorlage |
| **vor Sommer 2027** | Klasse-7-Pools nachprüfen, sobald Emirhans Stundenplan für Klasse 7 bekannt ist. Am 30.09.2026 schon ausgebaut: Türkisch 120 → 260, Englisch 120 → 200, Deutsch 120 → 180, Gewi 120 → 180, Musik 80 → 140, KI-Wissen eigener Pool mit 80. Mit 3.1 dazu: Kunst 80 → 120, Politik 80 → 120 (vorher hier irrtümlich mit 100 angegeben), Biologie Kl. 6 120 → 160 | `pool-reichweite.py` mit Emirhan als Klasse 7 laufen lassen (`KINDER` im Skript anpassen) |

## 5. Wartbarkeit (kein Zeitdruck)

Bewusst zurückgestellt. Diese Punkte sind Umbauten ohne sichtbaren Nutzen für die Kinder, und
jeder hat ein Risiko, das ein Test nur teilweise abdeckt.

- **4.1 Feature-Schalter vereinheitlichen**: `DrivingAreaDisabled`, `ErsteHilfeDisabled` und jetzt
  `TimetableSubjectsDisabled` sind drei Einzelspalten mit derselben Invertierungs-Falle. Beim
  vierten Schalter lohnt eine Spalte `DisabledModulesJson`. Dafür braucht es eine Migration der
  vorhandenen Werte, also genau die nicht-additive Änderung, die `SqliteSchemaUpdater` nicht kann.
- ~~**`UpdateSettingsAsync` auf ein Einstellungs-Objekt umstellen**~~ – erledigt 29.09.2026:
  `ProfileSettings` mit lauter `required`-Eigenschaften, ein vergessenes Feld ist ein
  Compilerfehler.
- **README aufteilen** (über 400 Zeilen): Überblick im README, Details je Bereich in `docs/`.

## 6. Ideen (nachrangig, siehe PLAN.md Phase 5)

- **Modul „Gaming & Creator“**: braucht zuerst Angaben der Familie zu den vier Kanälen
  (welches Spiel, welche Monetarisierung, welche Tricks). Ohne das keine Fragen. Außerdem gilt:
  mit der Auswahl nach Stundenplan hätte ein Fach, das auf keinem Plan steht, keinen Tag. Es
  müsste wie Türkisch fest dabei sein oder als eigener Bereich laufen.
- ~~Fehler-Kartei für die Kinder sichtbar machen~~ – war schon da; seit 29.09.2026 zählt die Zahl
  auf der Startseite nur noch, was heute wirklich drankommt (`ReviewForecast`).
- Elternbericht als PDF.
- Aus dem Pilot offen: Installer signieren (EV-Zertifikat), Entscheidung über Auto-Update.
  Ein Auto-Update wäre der erste Netzzugriff der App außer News und Wetter.

---

## Erledigt am 30.09.2026

| Was | Wo |
|---|---|
| **Version 3.1 – Funktion**: jede Frage vorlesen (englische/türkische Sätze in ihrer Stimme, Lösung anhören), Lernkalender in „Mein Fortschritt“, Designs 🏔️ Gletscher und 🌋 Vulkan als Belohnung, 120 neue Fragen (Biologie Kl. 6, Kunst/Politik Kl. 7) und mehrstufige Wahrscheinlichkeit (Mathe Kl. 9), Schulkalender bis Sommer 2028 | [`NAECHSTES-LEVEL-3-1.md`](NAECHSTES-LEVEL-3-1.md) |
| **Befunde aus dem Testlauf der Familie**: Emojis als Kästchen (neue Preflight-Regel `emoji-neu`), abgeschnittener Knopf im Passwort-Fenster, rohe Namen („Klasse10“), unbeschriftete Belohnungsfelder; Eltern-Bereich mit Inhaltsverzeichnis und erstmals von einem Test geladen | `ParentSettingsWindowTests` |
| **+660 Fragen**: Türkisch Kl. 6/7/9, Klasse 7 Englisch/Deutsch/Gewi/Musik, KI-Wissen Kl. 7 | Pull Request [OrhanHero/HeroKid#6](https://github.com/OrhanHero/HeroKid/pull/6) |
| **Fehler:** offene Antworten galten als richtig, sobald die Lösung *darin vorkam* – „25“ war richtig, wenn „5“ gesucht war, und wer mehrere Formen hintereinander tippte, lag immer richtig. Jetzt muss die Antwort der Lösung entsprechen (Einheiten, „x =“, Artikel, ganzer Lückensatz bleiben erlaubt) | `OpenTextAnswerMatcher` |
| **Fehler:** bei den binomischen Formeln waren 8 von 9 hinterlegten Lösungen falsch („(x + 9)² = x² + 36x + 81“). Wer richtig rechnete, bekam einen Fehler. Alte Karteikarten werden beim nächsten Abruf repariert | `BinomischeFormel` |
| **Fehler:** „Groß- und Kleinschreibung“ nahm „auto“ für „Auto“ an – die Schreibung, um die es ging, wurde nicht geprüft | `QuizQuestion.CaseSensitive` |
| Release-Workflow auch per Knopf (Actions → Release → Run workflow), Release 2.0.0 und 2.1.0 veröffentlicht | `release.yml` |
| **Version 3.0 – Designs**: acht Designs (auch dunkel und hoher Kontrast), Schrift und Textgröße je Kind, „wie Windows“/„abends dunkel“, Galaxie als Belohnung; alle Designs erfüllen WCAG 2.2 AA | [`DESIGN.md`](DESIGN.md), Pull Request [OrhanHero/HeroKid#8](https://github.com/OrhanHero/HeroKid/pull/8) |
| **Technik**: Bildschirmfotos jeder Ansicht in jedem Design in der CI, Kontrasttest, Preflight-Regeln für Design-Ressourcen | `DesignScreenshotTests`, `DesignThemeCatalogTests` |
| **Umgebung**: SessionStart-Hook (Cloud-Sitzungen bauen sofort), Devcontainer, `.editorconfig` | [`BUILD.md`](BUILD.md) |

## Erledigt am 29.09.2026 (Version 2.0)

Alles im Pull Request [OrhanHero/HeroKid#1](https://github.com/OrhanHero/HeroKid/pull/1), jeder
Schritt einzeln lokal gebaut und getestet (neu: die ganze Solution baut auch unter Linux) und von
der CI auf `windows-latest` bestätigt. Plan: [`NAECHSTES-LEVEL.md`](NAECHSTES-LEVEL.md).

| Schritt | Was | Wo |
|---|---|---|
| 1 | **.NET 10 LTS** (Support bis 11/2028), zentrale Paketversionen, `global.json`, Dependabot, schneller Linux-CI-Lauf | `Directory.Build.props`, `Directory.Packages.props` |
| 2 | **+140 Fragen**: Englisch Kl. 6 (+60), Englisch Kl. 9 (+40), Musik Kl. 6 (+40) – kein Fach mehr unter 8 Wochen | `EnglischGenerator`, `MusikGenerator` |
| 3 | Fehler-Kartei-Zahl auf der Startseite zählt nur, was **heute** drankommt | `ReviewForecast` |
| 4 | **🏆 Mein Fortschritt**: Meisterschaft je Thema in vier Stufen, auch im Elternbericht | `TopicMasteryCalculator` |
| 5 | **🏅 25 Abzeichen**, die nie verfallen | `AchievementCatalog`, Tabelle `UnlockedAchievements` |
| 6 | Eltern-Passwort mit 600.000 PBKDF2-Durchläufen (alte bleiben gültig); `UpdateSettingsAsync` mit Einstellungs-Objekt | `AdminAuthService`, `ProfileSettings` |
| 7 | Version 2.0.0, **Systeminfo** im Eltern-Bereich, Release-Workflow für Tags | `SystemInfoReport`, `release.yml` |
| — | Der Geschafft-Bildschirm scrollt bei Bedarf: der Freischalten-Knopf kann nicht mehr unter den Rand rutschen | `ResultView.xaml` |

## Erledigt am 28.09.2026

Alles im Pull Request [OrhanHero/HeroKid#1](https://github.com/OrhanHero/HeroKid/pull/1), jeder
Schritt einzeln durch die CI auf `windows-latest` gebaut und getestet.

| Plan | Was | Wo |
|---|---|---|
| 2.0 (Teil) | **Fächer nach Stundenplan**: nächster Schultag + Türkisch, NaWi reihum, Klausurfächer gehen vor; pro Kind abschaltbar; „Heute übst du für …“ auf der Startseite | `TimetableSubjectPlanner` |
| 2.2 | Abschlussquiz fragt nur die geübten Fächer ab (folgt aus 2.0) | `SubjectAvailability` |
| 4.2 | **CI wiederhergestellt** (war am 17.08. gelöscht worden) | `.github/workflows/build.yml` |
| 1.1 | Sicherung → Zerstören → Wiederherstellen als Test, auch mit älterem Schema | `BackupRestoreTests` |
| 1.2 | Knopf „🔍 Datenbank prüfen“ (`PRAGMA integrity_check`) | Eltern-Bereich |
| 1.3 | Notfall-Anleitung | [`WIEDERHERSTELLUNG.md`](WIEDERHERSTELLUNG.md) |
| — | **Fehler:** „Alle Daten zurücksetzen“ ließ 6 Tabellen stehen | Tabellenliste kommt jetzt aus dem EF-Modell |
| — | **Fehler:** „Profil löschen“ ließ Stundenplan, Fehler-Kartei, Vokabeln usw. verwaist liegen | ebenso |
| 2.1 | Pool-Reichweite ausgerechnet statt geschätzt | `scripts/pool-reichweite.py` |
| 3.1 | Klassenstufe im Eltern-Bereich änderbar, Hinweis zum Schuljahresbeginn | `SetGradeLevelAsync` |
| 3.3 | Über Nacht stehen gelassene Sitzung beginnt morgens neu (Grenze 4 Uhr) | `SessionDayRollover` |
| 4.1 (Teil) | `CanEnterStage` war toter Code, prüft jetzt jede Navigation mit (nicht sperrend, ins Log) | `MainViewModel` |
| 4.3 | Zwei neue Vorab-Prüfungen: `observable-grossbuchstabe`, `feste-fachzahl` | `scripts/preflight.py` |
| 2.1 (Folge) | **Türkisch-Pool +100 Fragen je Stufe** (Kl. 6: Çoğul eki, Hâl ekleri, Soru eki, Sayılar/Zaman, Kısa metin; Kl. 9: Söz sanatları, Ses olayları, Sözcükte anlam, Cümle türleri, Türk edebiyatı) | `TurkishGenerator` |
