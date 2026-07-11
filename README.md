# LernTor

Eine kindersichere Windows-Kiosk-Lern-App: Nach dem Login startet LernTor im Vollbild und
sperrt den PC, bis das Kind News gelesen, Übungsaufgaben in den freigeschalteten Fächern
bearbeitet und das Abschlussquiz mit mindestens 50% bestanden hat. Erst dann wird der PC
freigegeben. Eltern legen im Eltern-Bereich fest, welche der 12 verfügbaren Fächer für den
Tag/das Profil überhaupt aktiv sind (siehe "Bereiche deaktivieren").

Zielgruppe: deutsch-türkische Kinder (ca. 10–15 Jahre) in Berlin, Lehrplan-Themen orientiert
am Berliner Rahmenlehrplan, Klasse 6 und 9 (siehe [docs/CURRICULUM.md](docs/CURRICULUM.md)).

## Profile

Mehrere Kinder am selben PC wählen beim Start ihr eigenes Profil aus einem Kachel-Dashboard: jede
Kachel zeigt den selbst gewählten Avatar (Emoji, beim Anlegen aus einer Auswahl wählbar - bewusst
Emoji statt Bilddateien: keine Assets zu pflegen, rendert nativ), einen Fortschrittsring für die
heutige Lernsession (grün + Häkchen, wenn heute schon freigeschaltet wurde) und Name/Alter/Klasse
auf sanft rotierenden Pastell-Hintergründen. Darüber steht eine tageszeitabhängige Begrüßung
(morgens/nachmittags/abends, DE/TR). Über "Neues Profil ➕" wird ein weiteres Profil mit eigener
Klassenstufe (6 oder 9) und Avatar angelegt. Jedes Profil hat seinen eigenen Fortschritt, sein
eigenes Abschlussquiz-Ergebnis und sein eigenes Aktivitätsprotokoll (im Eltern-Bereich per Dropdown
auswählbar). Beim ersten Start werden automatisch zwei Beispielprofile angelegt. Nach
Auswahl/Erstellung wird der Profilname dauerhaft oben rechts im Kiosk-Fenster angezeigt (👤 Name),
damit jederzeit erkennbar ist, welches Kind gerade angemeldet ist.

## Ablauf für das Kind

1. **Profil wählen**
2. **Lesen** (Pflicht, nicht überspringbar) – täglich wechselndes Stück aus einem festen Pool von
   60 Texten (siehe `LernTor.Core/Services/ReadingContentProvider.cs`), das **gleichzeitig auf
   Deutsch, Türkisch und Englisch nebeneinander** angezeigt wird, damit dasselbe Stück in allen drei
   Sprachen gelesen werden kann. Über Sprach-Tabs lässt sich alternativ eine einzelne Sprache größer
   und mit mehr Zeilenabstand anzeigen; ein **Vorlesen-Button** liest den Text der gewählten Sprache
   über die Windows-eigene Sprachausgabe vor (`System.Speech`, komplett offline - eine türkische
   Stimme ist auf Windows oft nicht vorinstalliert und muss ggf. über Einstellungen → Zeit und
   Sprache nachinstalliert werden; ohne passende Stimme liest die Standardstimme). Soll **laut
   vorgelesen** werden. Ein 5-Minuten-Timer läuft; "Weiter" ist erst danach nutzbar - es gibt bewusst
   keine Überspringen-Funktion. Ob tatsächlich (laut) gelesen wurde, kann technisch nicht geprüft
   werden – das ist eine bekannte, akzeptierte Grenze dieser Funktion. Der Pool besteht aus zwei
   Hälften:
   - **30 literarische/Allgemeinwissen-Stücke**: gemeinfreie Gedichte/Texte (Goethe, Heine, Yunus
     Emre, Schiller, Rilke, Robert Frost, William Blake, Lewis Carroll, u.a.) sowie kurze
     "kurz erklärt"-Sachtexte (Berlin, Sonnensystem, Menschenrechte, EU, ...). Fremdsprachige
     Originale werden mit eigenen, freien Übertragungen (keine fremden Übersetzungen) begleitet.
   - **30 Pop-Kultur-Kurzgeschichten**: eigens verfasste, kindgerechte Abenteuergeschichten rund um
     Minecraft, Minecraft Legends, Super Mario, AntonCraft, Geometry Dash, Rock'n'Roll, Pac-Man,
     Kirby, Batman, One Punch Man, Dragon Ball, One Piece und Backrooms. Diese Texte referenzieren
     die Marken/Figuren nur namentlich als Thema; Handlung, Dialoge und Wortlaut sind komplett neu
     und zitieren kein geschütztes Spiel-/Comic-/Anime-Material.
   (Eine Anbindung an lyrikline.org wurde bewusst nicht umgesetzt: aus dieser Entwicklungsumgebung war
   der Zugriff auf die Seite nicht möglich, und ihre zeitgenössischen Gedichte/Übersetzungen sind
   urheberrechtlich geschützt und dürften ohne Erlaubnis nicht in die App übernommen werden.)
3. **News** (Pflicht) – kuratierte RSS-Artikel mit Fokus Berlin/Deutschland und Istanbul/Samsun/Ünye/Türkei
   (Berlin-Lokalnachrichten werden garantiert einbezogen), je 1-2 Verständnisfragen pro Artikel. Eine
   Marker-Leiste im Kopf zeigt je Artikel einen Kreis (grün mit ✓ = Fragen beantwortet, auch aus einer
   früheren Session desselben Tages nach Absturz/Neustart; lila = aktueller Artikel; grau = offen).
4. **Fachbereiche** (alle nicht von den Eltern deaktivierten, Klasse 6/9): Mathematik, Deutsch, Türkisch,
   Englisch, Biologie, Chemie, Physik, Gesellschaftswissenschaften (Gewi), Politik, Geografie, Ethik,
   Medienbildung (ITG) – siehe [docs/CURRICULUM.md](docs/CURRICULUM.md) für die genauen Themen je Fach.
   Bei offenen Mathematik-Aufgaben steht ein Taschenrechner zur Verfügung; Aufgaben mit hinterlegtem
   Tipp zeigen einen abrufbaren Formel-/Vorgehens-Hinweis (verrät nicht die Lösung).
5. **Abschlussquiz** – gemischte Fragen aus allen aktiven Fächern (Anzahl passt sich automatisch an, wie
   viele Fächer aktiv sind), ≥50% richtig → PC wird freigeschaltet. Bei Nichtbestehen werden gezielt mehr
   Fragen aus den schwachen Fächern gestellt. Antwortoptionen bei Multiple-Choice-Fragen werden bei jeder
   Anzeige neu gemischt, damit die richtige Antwort nicht immer an derselben Stelle steht.

Fortschritt wird laufend in einer lokalen SQLite-Datenbank gespeichert (`%LOCALAPPDATA%\LernTor\lerntor.db`),
ein Absturz oder Neustart verliert also keinen Fortschritt.

## Architektur & Technologie-Entscheidung

**.NET 8 + WPF**, kein WinUI 3, kein Electron. Begründung: Die sicherheitskritischste Anforderung
(Kiosk-Sperre: Task-Manager deaktivieren, Windows-Taste/Alt+Tab abfangen) ist mit klassischen
Win32-APIs (P/Invoke) am zuverlässigsten umsetzbar, und WPF hat dafür die geringste Reibung bei
gleichzeitig moderner, gut stylebarer UI. Details siehe Projekt-Chatverlauf/Architektur-Entscheidung.

```
src/
  LernTor.Core         Domänenmodelle, Fortschritts-/Scoring-Logik (plattformunabhängig)
  LernTor.ContentGen   Regelbasierte Aufgabengeneratoren (Mathe/Deutsch/Türkisch/NaWi) + Quiz-Composer
  LernTor.News         RSS-Ingestion, Vereinfachung, Verständnisfragen-Generierung
  LernTor.Data         SQLite-Persistenz (EF Core): Fortschritt, Aktivitätsprotokoll, Einstellungen,
                       eigene (elternerstellte) Aufgaben
  LernTor.Security     Kiosk-Lock (Keyboard-Hook), Task-Manager-Policy, Autostart, Admin-Auth
  LernTor.App          WPF-UI (Kiosk-Shell, alle Bildschirme, DE/TR-Lokalisierung)
  LernTor.Installer     Inno-Setup-Skript + PowerShell-Autostart-Helfer
tests/
  LernTor.Tests        xUnit-Tests für Core/ContentGen/News
```

## Kiosk-Sperre: was sie tut und was sie NICHT tut

Umgesetzt ist ein **Soft-Lock**: Vollbild-Topmost-Fenster ohne Rahmen, globaler Keyboard-Hook
(blockiert Windows-Taste, Alt+Tab, Strg+Esc, Alt+Esc, Alt+F4) und eine Registry-Richtlinie, die den
Task-Manager deaktiviert, solange LernTor läuft. Start erfolgt über einen Windows-Autostart-Task
(Anmeldung-Trigger), **kein** Shell-Ersatz (`explorer.exe` bleibt unangetastet).

**Bewusste Grenze:** Strg+Alt+Entf (Secure Attention Sequence) kann von keiner Windows-Anwendung im
User-Modus abgefangen werden – das ist ein OS-Schutzmechanismus. Die Task-Manager-Registry-Sperre
greift aber auch dann, wenn ein Kind versucht, darüber den Task-Manager zu öffnen (es erscheint eine
"vom Administrator deaktiviert"-Meldung). Ein sehr technisch versiertes Kind mit Admin-Wissen könnte
dies dennoch umgehen – für den Einsatzzweck (Motivation/Struktur für 10-15-Jährige) ist das eine
angemessene, dokumentierte Grenze. Ein härterer Shell-Ersatz-Modus wäre als spätere Erweiterung möglich,
birgt aber das Risiko, den Desktop bei einem App-Absturz komplett unbenutzbar zu machen, und wurde
deshalb bewusst nicht umgesetzt.

## Motivation & Gamification

- **Belohnungs-Sterne**: +2 fürs Lesen, +2 für News, +1 je abgeschlossenem Fach, +5 fürs bestandene
  Abschlussquiz - nur echte Abschlüsse zählen, von Eltern deaktivierte (übersprungene) Fächer geben
  keine Sterne. Heute verdiente Sterne und der Gesamtstand erscheinen auf dem Abschluss-Bildschirm;
  der über alle Tage gesammelte Gesamtstand (⭐) steht auf der Profil-Kachel.
- **Etappen-Leiste**: oben im Kiosk-Fenster zeigen vier Chips (Lesen → News → Fächer x/y → Quiz)
  jederzeit, wo das Kind in der heutigen Session steht - erledigt = grün mit Häkchen, aktuell = lila.
- **Konfettiregen**: beim bestandenen Abschlussquiz fällt einmalig Konfetti in den App-Farben über
  den Ergebnis-Bildschirm (einmalige Animation, keine Dauerschleife - keine bleibende CPU-Last im
  Kiosk-Betrieb).
- Bewusst KEINE Tages-Streaks: ein verpasster Tag soll kein schlechtes Gewissen erzeugen - Sterne
  können nur wachsen, nie verfallen.

## Eltern-Features

- Zahnrad-Symbol (unten rechts, dezent) öffnet den passwortgeschützten Eltern-Bereich.
- Erststart: Admin-Passwort selbst festlegen (PBKDF2-Hash, kein Klartext gespeichert).
- Fachbereiche einzeln deaktivieren, Klassenstufe (6/9) einstellen.
- Aktivitätsprotokoll: alle beantworteten Aufgaben + Quiz-Ergebnisse einsehbar.
- "Sofort freischalten": Notfall-Override, überspringt den restlichen Ablauf.
- "Alle Daten zurücksetzen…" (Gefahrenzone): löscht alle Profile/Fortschritte/Einstellungen aus der
  App heraus, mit Ja/Nein-Bestätigung. Vorher ging das nur manuell über das Löschen von `lerntor.db`.
- **Eigene Aufgaben** ("Eigene Aufgaben (z.B. von der Lehrkraft)"): Formular zum manuellen Anlegen
  eigener Fragen (Fach, Klassenstufe, Fragetyp, Frage, Antwortoptionen, richtige Antwort(en),
  Erklärung, optionaler Tipp) - z.B. für aktuelle Hausaufgaben oder Themen, die die Lehrkraft gerade
  durchnimmt. Gespeichert in der lokalen DB (`CustomQuestionRepository`/`CustomQuestionEntity`,
  Tabelle `CustomQuestions`), unabhängig von den generierten Aufgaben. Diese Aufgaben werden **additiv**
  zu den generierten Übungsaufgaben (passendes Fach + Klassenstufe) und zum Abschlussquiz
  hinzugefügt - sie ersetzen die Generatoren nicht. Löschbar über den "Löschen"-Button je Eintrag.

**Bewusst keine Zeitlimit-Funktion**: Es gibt kein tägliches Zeitlimit für die Lernsession - die
Kinder sollen so viel Zeit erhalten, wie sie zum Durcharbeiten von Lesen/Fächern/Abschlussquiz
benötigen. Das ist eine bewusste Design-Entscheidung, kein technisches Versäumnis.

## Bekannte Grenzen / nächste Schritte

- **KI-Funktionen: komplett lokal, keine Cloud-Anbindung.** LernTor nutzt an keiner Stelle einen
  Cloud-KI-Dienst - das lokal geladene Sprachmodell ist die einzige KI-Anbindung, für zwei Features:
  automatisches Einlesen von Lehrer-Unterlagen (PDF/Word → Fragenvorschläge) und den KI-Lernchat für
  Kinder (siehe "Eltern-Features" bzw. weiter unten). Beide teilen sich dieselbe Infrastruktur:
  - **`LocalLlmModelHost`** (`src/LernTor.ContentGen/Llm/LocalLlmModelHost.cs`) lädt das GGUF-Modell
    über [LLamaSharp](https://github.com/SciSharp/LLamaSharp) (llama.cpp-Bindings, Pakete `LLamaSharp` +
    `LLamaSharp.Backend.Cpu`, reines CPU-Backend ohne CUDA-Abhängigkeit) genau einmal und hält es im
    Speicher, statt es bei jedem Aufruf neu zu laden.
  - **Modell-Katalog + automatischer Download** (`LocalLlmModelCatalog.cs`): Eltern wählen im
    Eltern-Bereich per Dropdown eines der kuratierten Modelle; die Datei wird beim ersten Gebrauch
    automatisch nach `%LOCALAPPDATA%\LernTor\models\` heruntergeladen (einmalig Internet nötig, danach
    komplett offline; jedes Modell hat seine eigene Datei, ein Wechsel lädt also nur einmal herunter).
    - **Standard: [Qwen2.5-7B-Instruct](https://huggingface.co/Qwen/Qwen2.5-7B-Instruct-GGUF)**
      (Q4_K_M, ~4,7 GB, Apache-2.0): als einzige Familie im permissiv lizenzierten, CPU-tauglichen
      7B-Bereich offiziell stark in Deutsch UND Türkisch (29+ Sprachen), deutlich besseres Reasoning
      und Erklärqualität als das 3B - und dieselbe Modellfamilie, gegen die Prompt-Format und
      AntiPrompts-Stoppwörter in dieser Codebasis bereits erprobt sind. Verworfen: Mistral-7B
      (Türkisch schwach), Llama-3.1-8B (Lizenz + Türkisch mittelmäßig), Gemma-2-9B (Lizenz),
      Qwen2.5-14B (~9 GB Q4 → auf Familien-CPUs nur 2-4 Token/s, zu langsam für den Kinder-Chat).
    - **Alternative: Qwen2.5-3B-Instruct** (Q4_K_M, ~2 GB) als "Leicht & schnell"-Option für ältere
      PCs, auf denen das 7B zu langsam antwortet.
    **Die Hugging-Face-Download-URLs sind aus der Entwicklungsumgebung heraus nicht verifizierbar**
    (huggingface.co ist von dort blockiert, 403 per curl bestätigt - dieselbe Einschränkung wie zuvor bei
    docs.cloud.google.com) und müssen beim ersten echten Test auf einem Windows-Rechner mit Internetzugang
    überprüft werden; schlägt der Download fehl, zeigt die App eine klare Fehlermeldung (inkl. Modellname
    und Größe) und Eltern können ein anderes Modell wählen oder manuell eine `.gguf`-Datei über den
    Datei-Dialog hinterlegen (eigene Datei hat Vorrang vor dem Katalog).
  - **API-Verifizierung**: `LLamaSharp` 0.27.0 wurde real von nuget.org heruntergeladen und die
    kompilierte DLL per CLR-Metadaten-Analyse geprüft (nicht nur aus Dokumentation/Training geraten):
    `ModelParams(string modelPath)`-Konstruktor mit settable `ContextSize`/`GpuLayerCount`-Properties,
    statisches `LLamaWeights.LoadFromFile(ModelParams)`,
    `StatelessExecutor(LLamaWeights, IContextParams, ILogger?)`-Konstruktor (baut sich seinen
    Inferenz-Kontext laut Metadaten-Analyse intern selbst auf - ein separat per
    `LLamaWeights.CreateContext(...)` erzeugter Kontext wird gar nicht entgegengenommen) sowie
    `InferAsync(string, InferenceParams?, CancellationToken)`, das laut TypeRef-Analyse
    `IAsyncEnumerable<string>` liefert (per `await foreach` zum vollständigen Antworttext
    zusammengesetzt). `LLamaWeights` implementiert `IDisposable`.
- **Automatisches Einlesen von Lehrer-Unterlagen** (`src/LernTor.ContentGen/TeacherImport/`): PDF
  (`PdfPigTextExtractor`) bzw. .docx (`OpenXmlWordTextExtractor`, kein altes binäres .doc) → Fließtext →
  `LocalLlmQuestionSuggester` stellt eine Frage nach strukturiertem JSON mit Quizfragen. Ergebnisse sind
  immer nur Vorschläge (`ExtractedQuestionDraft`, mit `SourceExcerpt` = Textstelle im Original) - Eltern
  müssen im Eltern-Bereich jeden Vorschlag einzeln über "Übernehmen" bestätigen oder "Verwerfen", bevor
  er via `CustomQuestionRepository.AddAsync` gespeichert wird. Keine automatische Übernahme ohne
  menschliche Kontrolle. Der manuelle Editor (siehe "Eltern-Features") funktioniert unabhängig davon
  weiterhin normal.
  - **Inline-Editing**: alle Felder eines Vorschlags (Thema, Frage, Optionen, richtige Antworten,
    Erklärung, Tipp) sind direkt in der Vorschlagskarte korrigierbar, bevor übernommen wird -
    "Übernehmen" validiert (Frage/Antwort vorhanden, bei Auswahlfragen mindestens 2 Optionen und die
    richtige Antwort wörtlich darunter) und zeigt Fehler direkt an der Karte. Die Belegstelle aus dem
    Dokument bleibt bewusst nicht editierbar - sie ist die Prüf-Referenz, kein Inhalt.
- **KI-Lernchat für Kinder** (`src/LernTor.App/Controls/QuestionCard.xaml`, "🤖 KI fragen"-Button):
  Kinder können zu jeder Aufgabe - in News, Übungen und im Abschlussquiz gleichermaßen, da alle drei
  dieselbe `QuestionCard` verwenden - dem KI-Assistenten Rückfragen stellen, so wie sie einen
  Taschenrechner benutzen würden: als Werkzeug, mit dem man sich auseinandersetzt, nicht als reinen
  Lösungsautomaten. Der Prompt (`HomeworkChatPromptBuilder`, `src/LernTor.ContentGen/HomeworkChat/`)
  bekommt bewusst die richtige Antwort und die Erklärung der Aufgabe MIT in den Kontext - ohne sie zu
  kennen, könnte die KI nur raten statt wirklich zu helfen. Die Leitplanke ist eine reine
  Prompt-Anweisung: die fertige Antwort nicht schon in der ersten Nachricht preiszugeben, sondern zuerst
  durch Rückfragen und Denkanstöße zu helfen - erst wenn das Kind es mehrfach probiert hat oder
  ausdrücklich danach fragt, soll die KI die Lösung erklären.
- **News-Quellen**: kuratierte RSS-Feeds (siehe `LernTor.News/NewsFeedSource.cs`), inklusive einer
  KI-/Technik-Quelle (heise online) und einer Herabstufung (nicht Ausfilterung) von Artikeln mit
  verstörenden Themen (Krieg, Gewaltverbrechen, ...) über `SensitiveKeywords`. RSS-URLs von
  Nachrichtenseiten ändern sich gelegentlich – nicht erreichbare Feeds werden übersprungen, sollten aber
  gelegentlich geprüft werden. Inzwischen auf einem echten Windows-Rechner getestet und bestätigt
  funktionsfähig (inkl. der neuen heise.de-Quelle).
- **Vereinfachung der Artikeltexte** ist aktuell regelbasiert (kein LLM). Ein lokales LLM (Phi-3, Gemma 2,
  Llama 3.1 über Ollama) ließe sich über das `ITextSimplifier`-Interface in `LernTor.News` als zweite
  Implementierung ergänzen, ohne den Rest der App anzufassen.
- Wird inzwischen auch auf einem echten Windows-Rechner getestet (nicht nur über den CI-Workflow
  `.github/workflows/build.yml` auf `windows-latest`), da diese Entwicklungsumgebung selbst kein
  Windows bereitstellt.

## Schnellstart (Entwicklung)

```powershell
git clone <repo>
cd HeroKid
dotnet restore LernTor.sln
dotnet build LernTor.sln
$env:LERNTOR_SKIP_LOCK = "1"   # WICHTIG für die Entwicklung: verhindert die Kiosk-Sperre!
dotnet run --project src/LernTor.App
```

Vollständige Build-/Installationsanleitung: [docs/BUILD.md](docs/BUILD.md).
