# Lehrplan-Zuordnung (Berliner Rahmenlehrplan, Klasse 6 & 9)

Diese Übersicht zeigt, welche Themen die Generatoren in `src/LernTor.ContentGen/Generators`
aktuell abdecken. Es handelt sich um eine **repräsentative Auswahl** zentraler Themen je Fach und
Klassenstufe, nicht um eine vollständige 1:1-Abbildung des kompletten Rahmenlehrplans. Die
Architektur (ein `TopicFactory`-Delegate pro Thema in `ExerciseGeneratorBase`) ist bewusst so
gebaut, dass weitere Themen einfach als zusätzliche private Methode + Eintrag in `TopicsByGrade`
ergänzt werden können.

Der Tipptrainer ist eine eigene Lernstufe vor den News und gehört nicht zum Fachcurriculum.
**Nur deutsche Sprache & QWERTZ-Layout** (keine türkischen/englischen Wörter mehr).
Profil-spezifische Abschluss-Lektionen: Emirhan (Klasse 6) und Batuhan (Klasse 9) tippen jeweils
ihren persönlichen Steckbrief-Text.

**Poolgröße je Thema**: Jedes Thema wird von einer festen Liste kuratierter Beispiele bedient
(außer Mathematik, das echte Zahlenwerte würfelt statt aus einer festen Liste zu ziehen - dort ist
die Zahl der möglichen Aufgaben pro Thema praktisch unbegrenzt). Der Zielwert für diese Listen ist
**20 Beispiele pro Thema**: Bei zu kleinen Pools (ursprünglich nur 2-4, später 5 Beispiele) griff die
Wiederholungs-Vermeidung in `ExerciseGeneratorBase.Generate` schnell ins Leere, und dasselbe Kind
sah dieselben Fragen bereits nach 1-2 Tagen wieder. Alle 14 Fächer mit fester Beispiel-Liste
(Deutsch, Englisch, Türkisch, ITG, Politik, Physik, Biologie, Chemie, Geografie, Gewi, Ethik, Kunst,
Musik, Geschichte) sind inzwischen auf diesen Zielwert gebracht.

## Mathematik (`MathGenerator.cs`)

| Klasse 6 | Klasse 9 |
|---|---|
| Bruchrechnung – Addition | Lineare Gleichungen |
| Bruchrechnung – Multiplikation | Lineare Funktionen |
| Prozentrechnung – Prozentwert | Quadratische Gleichungen (pq-Formel) |
| Negative Zahlen | Satz des Pythagoras |
| Flächen- und Umfangsberechnung | Zinsrechnung |
| Maßstab | Binomische Formeln |
| Wahrscheinlichkeit bei Zufallsexperimenten | Mittelwert und Median (Statistik) |
| Volumen von Quadern | Trigonometrie im rechtwinkligen Dreieck |
| Bruch-Dezimalzahl-Umwandlung | Satz des Thales |
| Direkt proportionale Zuordnungen | Volumen von Pyramide, Kegel und Kugel |
| Kongruenzabbildungen | Lineare Gleichungssysteme |
| Kombinatorik (systematisches Zählen) | Quadratische Funktionen (Scheitelpunkt) |
| | Exponentielles Wachstum |
| | Potenzgesetze |

## Deutsch (`GermanGenerator.cs`)

| Klasse 6 | Klasse 9 |
|---|---|
| Wortarten | Aktiv und Passiv |
| Zeitformen (Tempus) | Satzgefüge und Konjunktionen |
| Satzglieder | Kommasetzung |
| Groß- und Kleinschreibung | "dass" oder "das" |
| Steigerung von Adjektiven | Wortarten (vertieft) |
| Satzarten | Textsorten unterscheiden |
| Wortbildung | Aufbau eines Dramas |
| Balladen und Jugendbücher | Figurencharakterisierung |
| Sach- und Gebrauchstexte auswerten | Argumentation und Quellenkritik |
| Texte in medialer Form (Wiki, E-Mail, TV) | Filmanalyse |
| Schreibformen | Rede, Debatte und Bewerbung |
| Gesprächsformen und Präsentieren | Satzbau und Sprachwissen |
| | Wortbedeutung und Sprachwandel |

## Türkisch (`TurkishGenerator.cs`)

| Klasse 6 | Klasse 9 |
|---|---|
| Şimdiki Zaman (Präsens) | Cümlenin Ögeleri (Satzglieder) |
| Geçmiş Zaman (Präteritum/-di'li geçmiş) | Gelecek Zaman (Futur) |
| Eş Anlamlı Kelimeler (Synonyme) | Yazım Kuralları (Rechtschreibung) |
| Zıt Anlamlı Kelimeler (Antonyme) | Fiilimsi (Partizip/Verbalnomen) |
| Doğa ve Çevre (Natur und Umwelt) – Wortschatz | Kimlik ve Gelecek (Identität und Zukunft) – Wortschatz |
| Aile ve Günlük Yaşam (Familie und Alltag) – Wortschatz | Türk Tarihi ve Gelenekleri (Geschichte und Traditionen) |
| Okul ve Toplum (Schule und Gesellschaft) – Wortschatz | Türkiye'nin Coğrafyası (Geografie der Türkei) |
| Türk Kültürü ve Gelenekleri (Kultur und Traditionen) | |

## Physik (`PhysikGenerator.cs`)

| Klasse 6 | Klasse 9 |
|---|---|
| Aggregatzustände | Ohmsches Gesetz |
| Einfacher Stromkreis | Energieerhaltung |
| Magnetismus | Newtonsche Gesetze |
| Von den Sinnen zum Messen | Magnetfelder und elektromagnetische Induktion |
| Welt des Großen – Welt des Kleinen (Optik und Weltraum) | Gleichförmige und beschleunigte Bewegungen (Kinematik) |
| Bewegung zu Wasser, zu Lande und in der Luft (Bionik) | Radioaktivität und Kernphysik |
| | Schwingungen, Wellen und optische Geräte |

## Chemie (`ChemieGenerator.cs`)

| Klasse 6 | Klasse 9 |
|---|---|
| Stoffgemische trennen | Atommodell |
| Verbrennung | Chemische Reaktionen |
| Säuren und Laugen | Periodensystem |
| Metalle und ihre Eigenschaften | Klare Verhältnisse – Stöchiometrie |
| Stoffe im Alltag | Säuren und Laugen – echt ätzend |
| Das Periodensystem der Elemente – Übersicht und Werkzeug | Kohlenwasserstoffe – vom Campinggas zum Superbenzin |
| Gase – zwischen lebensnotwendig und gefährlich | Alkohole – vom Holzgeist zum Glycerin |
| Wasser – eine Verbindung | Organische Säuren – Salatsauce, Entkalker & Co |
| Salze – Gegensätze ziehen sich an | Ester – Vielfalt der Produkte aus Alkoholen und Säuren |

Organische Chemie (Kohlenwasserstoffe, Alkohole, organische Säuren, Ester) ist entgegen einer
früheren Design-Notiz in dieser Datei **inzwischen doch implementiert** (Klasse 9) - die Notiz war
nicht mehr aktuell und wurde entfernt (siehe Abschnitt "Abgleich mit dem offiziellen
Rahmenlehrplan" weiter unten).

## Biologie (`BiologieGenerator.cs`)

| Klasse 6 | Klasse 9 |
|---|---|
| Der menschliche Körper | Zellbiologie |
| Fotosynthese | Vererbung (Genetik) |
| Wirbeltierklassen | Ökosysteme |
| Pubertät und Entwicklung | Gesundheit und Krankheit (Immunologie) |
| Die Zelle | Bau und Funktion des Nervensystems |
| Lebensräume und ihre Bewohner (Nahrungsketten) | Sucht und Suchtprävention |
| | Vererbung beim Menschen (Humangenetik) |
| | Evolution – Theorien und Stammesgeschichte |

## Englisch (`EnglischGenerator.cs`)

| Klasse 6 | Klasse 9 |
|---|---|
| Simple Present vs. Present Progressive | Simple Past vs. Present Perfect |
| Unregelmäßige Pluralformen | Conditional Sentences (Type 1) |
| Question Words | Passive Voice |
| Individuum und Lebenswelt: Alltag und Familie | Identität, Lebensentwürfe und Zukunft |
| Gesellschaft: Schule und Zusammenleben | Gesellschaft, Medien und Vielfalt |
| Kultur und historischer Hintergrund | Umwelt und Nachhaltigkeit |
| Natur und Umwelt | |

## Gesellschaftswissenschaften / Gewi (`GewiGenerator.cs`)

| Klasse 6 | Klasse 9 |
|---|---|
| Geschichtliche Epochen | Grundgesetz |
| Kartenkunde und Himmelsrichtungen | Wirtschaftskreislauf |
| Kinderrechte | Medien und Gesellschaft |
| Ernährung – wie werden Menschen satt? | |
| Wasser – nur Natur oder in Menschenhand? | |
| Stadt und städtische Vielfalt | |
| Europa – grenzenlos? | |
| Tourismus und Mobilität – schneller, weiter, klüger? | |
| Demokratie und Mitbestimmung | |

## Politik (`PolitikGenerator.cs`)

| Klasse 6 | Klasse 9 |
|---|---|
| Was ist Demokratie? | Gewaltenteilung |
| Berlin und seine Bezirke | Bundestag und Bundesrat |
| Wahlrecht | Wahlsystem |
| | Soziale Marktwirtschaft |
| | Demokratie in Deutschland: Willensbildung, Medien und Gefährdungen |
| | Konflikte und Konfliktlösungen: internationale Akteure |
| | Friedenssicherung und Entwicklungspolitik |
| | Europa in der Welt: Die Europäische Union |

## Geografie (`GeoGenerator.cs`)

| Klasse 6 | Klasse 9 |
|---|---|
| Kontinente und Ozeane | Plattentektonik |
| Klimazonen | Klimawandel |
| Deutschland: Bundesländer | Verstädterung |
| | Armut und Reichtum weltweit |
| | Umgang mit Ressourcen: Energie und Rohstoffe |
| | Umgang mit Ressourcen: Landwirtschaft und Boden |
| | Klimaschutz: Internationale Konflikte und Lösungen |
| | Wirtschaftliche Verflechtungen und Globalisierung |
| | Europa in der Welt (naturräumliche und wirtschaftliche Vielfalt) |

## Ethik (`EthikGenerator.cs`)

| Klasse 6 | Klasse 9 |
|---|---|
| Werte und Regeln | Verantwortung und Pflicht |
| Freundschaft und Konflikte | Meinungsfreiheit und Grenzen |
| Weltreligionen | Digitale Ethik |
| | Recht und Gerechtigkeit |
| | Wer bin ich? - Identität und Rolle |
| | Wie frei bin ich? - Freiheit und Verantwortung |
| | Was ist gerecht? - Gerechtigkeitstheorien vertieft |
| | Was ist der Mensch? - Mensch und Gemeinschaft |
| | Was soll ich tun? - Handeln und Moral |
| | Worauf kann ich vertrauen? - Wissen und Glauben |

## Kunst (`KunstGenerator.cs`)

| Klasse 6 | Klasse 9 |
|---|---|
| Kunstwerke wahrnehmen und beschreiben | Kunst als Intervention und Mahnung |
| Material, Körper und Raum | Medienkunst und bildhaftes Gestalten |
| Medien und Verfahren | Architektur, Raum und Design |
| Kunst und meine Lebenswelt | Materialästhetik und Transformation |
| | Inszenierung und Kuration |
| | Kulturelle Identität und Vielfalt |

## Musik (`MusikGenerator.cs`)

| Klasse 6 | Klasse 9 |
|---|---|
| Grundlagen der Musik | Harmonielehre und Partiturlesen |
| Form und Gestaltung | Komposition und Satzweisen |
| Gattungen und Genres | Medien und digitale Produktion |
| Wirkung und Funktion | Gattungen und Genres der Musikgeschichte |
| Musik im kulturellen Kontext | Filmmusik und Programmmusik |
| | Musik im kulturellen und gesellschaftlichen Kontext |

## Geschichte (`GeschichteGenerator.cs`)

| Klasse 6 | Klasse 9 |
|---|---|
| Epochenüberblick: Mittelalter, Frühe Neuzeit, Revolutionen | Demokratie und Diktatur |
| Armut und Reichtum, Migrationen | Der Kalte Krieg und die geteilte Welt |
| Juden, Christen und Muslime | Konflikte und Konfliktlösungen |
| | Europa in der Welt |
| | Völkermorde und Massengewalt |
| | Die Welt nach dem Kalten Krieg (1989-1991) |

## Medienbildung / ITG (`ItgGenerator.cs`)

| Klasse 6 | Klasse 9 |
|---|---|
| Datenschutz-Grundlagen | Cybermobbing |
| Sichere Passwörter | Fake News erkennen |
| Urheberrecht im Internet | Algorithmen-Grundbegriff |

## News (`LernTor.News`)

22 kuratierte, kostenlose RSS-Quellen (siehe `CuratedNewsFeeds.All` in `NewsFeedSource.cs`) -
öffentlich-rechtlich, Nachrichtenagenturen, Bezirks-/Landesparlament, Hersteller-Feeds, bewusst
keine Boulevardquellen:

- **Deutschland/Berlin**: tagesschau.de, Deutsche Welle, rbb24 Berlin, Tagesspiegel Berlin,
  Berliner Morgenpost, Bezirksamt Neukölln, Bezirksamt Friedrichshain-Kreuzberg, Abgeordnetenhaus
  Berlin, fluter.de, Bundesregierung kompakt/Pressemitteilungen, BMBFSFJ
- **Türkei**: Anadolu Ajansı, TRT Haber, DW Türkçe (bewusst seriöse Agenturen/Sender statt
  Boulevardquellen)
- **KI/Technik**: heise online, IT Boltwise
- **Spiele**: GameStar, Nintendo.de News, PlayStation Blog DE, Xbox News DE, Steam News

`RssNewsService.LoadCuratedArticlesAsync` übernimmt aus **jedem Feed genau den neuesten Artikel** -
keine Quoten-/Prioritäts-Rankings mehr, das Ergebnis bleibt dadurch bewusst klein und stabil
(typischerweise ~22 Artikel statt z.B. 71, wenn mehrere Artikel pro Feed genommen würden). Zusätzlich
hängt sich täglich EIN rotierendes, kuratiertes Finanzwissen-Erklärstück an (`FinanceKnowledgeArticles`
- dafür gibt es keinen verlässlichen RSS-Feed, siehe unten). Ab einem Profil-Alter von ≤ 9 Jahren
werden Artikel mit `SensitiveKeywords` (Krieg, Gewaltverbrechen, Suizid, ... inkl. türkischsprachiger
Entsprechungen) komplett ausgefiltert statt nur herabgestuft ("keine Angstmache" gilt für die
Jüngsten strikt); ab 10 Jahren bleibt das mildere Herabstufen im Ranking. Pro Artikel wird automatisch
GENAU EINE Verständnisfrage erzeugt: ein Lückentext aus der Zusammenfassung mit ausgeblendetem
Schlüsselwort (`HeuristicComprehensionQuestionGenerator`) - frühere Fragetypen (Regionsfrage,
Schlüsselwort-aus-Überschrift-Frage) wurden auf Nutzerwunsch entfernt, weil sie ohne echtes Lesen
lösbar waren. Gibt die Zusammenfassung keinen Lückentext her, bleibt der Artikel ohne Frage.

**Wie die App an Nachrichten kommt**: `RssNewsService` lädt bei jedem Aufruf des News-Bereichs live
die RSS-Feeds aller obigen Quellen per `HttpClient` (kein Cache, keine gespeicherten Artikel) - die
Inhalte sind also tatsächlich tagesaktuell, nicht vorproduziert. Nicht erreichbare Feeds werden
einzeln übersprungen, ohne den Ladevorgang der übrigen Feeds abzubrechen (ein Tages-Archiv dient als
Offline-Rückfall, siehe README "Tages-Archiv & Offline-Rückfall").

## Abschlussquiz-Zusammenstellung bei vielen Fächern

`QuizComposer.ComposeFinalQuiz` verteilt die Fragenzahl dynamisch auf alle nicht deaktivierten
Fächer, damit das Quiz bei 12 möglichen Fächern nicht auf 60+ Fragen anwächst: Sind z.B. nur 4
Fächer aktiv, bekommt jedes davon mehr Fragen; sind alle 12 aktiv, entsprechend weniger pro Fach.
Eltern steuern über "Bereiche deaktivieren" im Eltern-Bereich, welche Fächer täglich überhaupt
Teil des Ablaufs und des Abschlussquiz sind.

## Erweiterung um weitere Themen

Neues Thema hinzufügen (Beispiel Mathematik):

1. Neue private `static QuizQuestion MeinNeuesThema(Random r) { ... }`-Methode in `MathGenerator.cs`.
2. Methode in die passende Liste (`TopicsByGrade[GradeLevel.KlasseX]`) eintragen.
3. Test in `tests/LernTor.Tests` ergänzen, der prüft, dass die eigene Musterlösung als richtig erkannt wird.

Kein Codeänderung an `ExerciseGeneratorBase`, `QuizComposer` oder den ViewModels nötig – die
Zufallsauswahl und Quiz-Zusammenstellung funktioniert automatisch mit neuen Themen.

**Neues Fach hinzufügen** (wie kürzlich `KunstGenerator.cs` und `MusikGenerator.cs`):
1. Neuen `Subject`-Enum-Wert in `LernTor.Core.Enums.Subject` ergänzen.
2. Neuen Generator (`SubjectNameGenerator.cs`) nach dem Muster von `KunstGenerator.cs`/`MusikGenerator.cs` anlegen (Subclass von `ExerciseGeneratorBase`, `TopicsByGrade` für Klasse 6/9 befüllen).
3. In `LearningStageSubjects.Map` (Core) das Fach den passenden `LearningStage`-Einträgen zuordnen.
4. In `ProgressGateService.SequentialOrder` (Core) den Stage-Reihenfolge-Eintrag ergänzen.
5. In `QuizComposer` (ContentGen) den Generator in der Default-Liste registrieren.
6. `SubjectToTitleConverter` (App) für Anzeigetitel (DE/TR) erweitern.
7. `ParentSettingsViewModel` (App) um Toggle für das neue Fach erweitern.
8. `Translations` (App) um DE/TR-Texte für das Fach und seine Topics erweitern.

Keine Änderungen an `ExerciseGeneratorBase`, `QuizComposer`-Logik oder ViewModels nötig – die
Navigation und Quiz-Zusammenstellung funktioniert automatisch mit neuen Generatoren.

## Abgleich mit dem offiziellen Rahmenlehrplan 1-10 (kompakt)

Die obige Themenauswahl wurde mit der offiziellen Broschüre "Rahmenlehrplan 1-10 kompakt"
(Senatsverwaltung für Bildung, Berlin, 1. Auflage 2017) abgeglichen, inklusive der zuvor noch nicht
gelesenen Abschnitte zu Mathematik, Chemie, Physik, Türkisch, Geografie, Ethik, Politische Bildung,
Gesellschaftswissenschaften 5/6 und Informatik. Die Broschüre selbst ist urheberrechtlich geschützt,
darf laut Impressum aber "für die Zwecke der Schule" verwendet werden – sie diente hier nur als
interne Orientierung, es wurden keine Textpassagen übernommen.

**Als direkte Folge dieses Abgleichs wurden neun neue Themen ergänzt** (jeweils mit echten
Beispielaufgaben inkl. Erklärung/HelpHint, kein reiner Dokumentations-Platzhalter):

- **Mathematik**: "Wahrscheinlichkeit bei Zufallsexperimenten" (Klasse 6) und "Mittelwert und Median
  (Statistik)" (Klasse 9) schließen die zuvor komplett fehlende Leitidee **"Daten und Zufall"**.
- **Chemie**: "Metalle und ihre Eigenschaften" (Klasse 6) deckt das zuvor fehlende Themenfeld Metalle ab.
- **Deutsch**: "Textsorten unterscheiden" (Klasse 9) deckt die RLP-Textsorten (Bericht, Kommentar,
  Reportage, Leserbrief, Erörterung), die zuvor nur in Klasse 5/6-Form (Wortart/Satzart) vorkamen.
- **Türkisch**: "Doğa ve Çevre (Natur und Umwelt) – Wortschatz" (Klasse 6) bringt erstmals einen
  themenorientierten (statt rein grammatikorientierten) Wortschatz-Topic ein, passend zum
  RLP-Themenfeld "Natur und Umwelt".
- **Gesellschaftswissenschaften/Gewi**: "Ernährung – wie werden Menschen satt?" (Klasse 6) deckt das
  RLP-Themenfeld "Ernährung" der Gewi-5/6-Doppeljahrgangsstufe ab.
- **Geografie**: "Armut und Reichtum weltweit" (Klasse 9) deckt das gleichnamige RLP-Themenfeld ab.
- **Physik**: "Magnetfelder und elektromagnetische Induktion" (Klasse 9) deckt das RLP-Themenfeld
  "Magnetfelder und elektromagnetische Induktion" (Doppeljahrgangsstufe 9/10) ab.
- **Politik**: "Soziale Marktwirtschaft" (Klasse 9) deckt das gleichnamige RLP-Themenfeld
  (Doppeljahrgangsstufe 9/10) ab.
- **Ethik**: "Recht und Gerechtigkeit" (Klasse 9) deckt das gleichnamige RLP-Themenfeld ab, das zuvor
  nur indirekt über "Meinungsfreiheit und Grenzen" gestreift wurde.

**Als direkte Folge der Erweiterung um neue Fächer wurden zwei komplett neue Fach-Generatoren ergänzt**:

- **Kunst** (`KunstGenerator.cs`): 4 Themen für Klasse 6 (Kunstwerke wahrnehmen, Material/Körper/Raum, Medien/Verfahren, Kunst und Lebenswelt) und 6 Themen für Klasse 9 (Intervention/Mahnung, Medienkunst, Architektur/Design, Materialästhetik/Transformation, Inszenierung/Kuration, Kulturelle Identität/Vielfalt) – deckt zentrale Inhaltsbereiche des RLP Kunst ab (Wahrnehmen, Gestalten, Kommunizieren, Kontextualisieren).
- **Musik** (`MusikGenerator.cs`): 5 Themen für Klasse 6 (Grundlagen, Form/Gestaltung, Gattungen/Genres, Wirkung/Funktion, Kultureller Kontext) und 6 Themen für Klasse 9 (Harmonielehre/Partitur, Komposition/Satzweisen, Medien/Digitale Produktion, Musikgeschichte, Filmmusik/Programmmusik, Gesellschaftlicher Kontext) – deckt die RLP-Themenfelder Grundlagen, Form, Gattungen, Wirkung und kultureller Kontext ab.

**Bewusst nicht übernommene/verbleibende Unterschiede** (kein technischer Mangel, sondern
Simplifizierungen dieser App gegenüber dem vollständigen RLP):

- **Biologie**: Der RLP führt Biologie als eigenständiges Fach erst ab Doppeljahrgangsstufe 7/8; in
  5/6 ist es Teil des integrierten Fachs Naturwissenschaften. Unsere Klasse-6-Themen (menschlicher
  Körper, Fotosynthese, Wirbeltierklassen) sind trotzdem sinnvoll, da die App Bio/Chemie/Physik aus
  Vereinfachungsgründen als getrennte Fächer ab Klasse 6 führt.
- **Türkisch**: Der RLP gliedert das Fach primär in kommunikative Themenfelder (Individuum und
  Gesellschaft, Gesellschaft und öffentliches Leben, Kultur und historischer Hintergrund, Natur und
  Umwelt) statt nach Grammatikthemen. `TurkishGenerator.cs` bleibt überwiegend grammatikorientiert
  (Zeiten, Satzglieder, Rechtschreibung), da das für automatisch geprüfbare Übungsfragen
  praktikabler ist - das neue Wortschatz-Thema ist ein erster Schritt in Richtung RLP-Themenfelder,
  kein vollständiger Umbau.
- **Informatik/ITG**: Das RLP-Themenfeld "Standardsoftware" (praktischer Umgang mit
  Textverarbeitung/Tabellenkalkulation) lässt sich kaum als automatisch auswertbare Quizfrage
  abbilden und bleibt daher unberücksichtigt; die übrigen Themenfelder (Informatiksysteme, Leben in
  vernetzten Systemen) sind über die bestehenden Themen (Datenschutz, Cybermobbing, Fake News,
  Algorithmen) plausibel abgedeckt.
- **Deutsch, Politische Bildung**: Drama-Analyse ist inzwischen abgedeckt (`DramaAufbau`,
  `Figurencharakterisierung`), Novelle/Parabel sowie tiefergehende Sozialkunde-Themen (z.B.
  Migration/Bevölkerung, Rechtsstaat) sind bewusst nicht 1:1 übernommen - eine vollständige
  Abdeckung aller RLP-Themenfelder ist weiterhin nicht das Ziel dieser App (siehe Hinweis am Anfang
  dieser Datei).

Dieser Abgleich verbessert die Abdeckung gezielt, ist aber weiterhin keine vollständige 1:1-Analyse
aller RLP-Themenfelder für alle zehn Jahrgangsstufen - die App konzentriert sich bewusst auf Klasse 6
und 9.

## Vollständiger Content-Plan-Abgleich nach Profil (Emirhan / Batuhan)

Quelle: internes Word-Dokument "LernTor Native - vollständiger und lückenloser Rahmenlehrplan
Berlin" mit den kompletten Themenfeldern für die zwei realen Ziel-Profile dieser App:

- **Emirhan** (geb. 09.05.2014, 6. Klasse, Niveaustufe D) → entspricht `GradeLevel.Klasse6`
- **Batuhan** (geb. 16.08.2011, 9. Klasse, Niveaustufe G) → entspricht `GradeLevel.Klasse9`

Der Haken-Status je Themenfeld wurde **nicht aus dem Gedächtnis geschätzt**, sondern direkt aus dem
aktuellen Code abgeleitet: für jedes Fach wurde die `TopicsByGrade`-Liste des jeweiligen Generators
in `src/LernTor.ContentGen/Generators/*.cs` mit dem passenden `GradeLevel` gegen die Themenfelder des
Dokuments abgeglichen. `- [x]` heißt: es existiert ein Topic (echte Beispielaufgaben inkl.
Erklärung/HelpHint), das dieses Themenfeld inhaltlich abdeckt. `- [ ]` heißt: (noch) kein
entsprechendes Topic vorhanden - unabhängig davon, ob das Fach selbst schon existiert oder nicht.
Bei Fächern, die es in der App noch gar nicht gibt (Sport, WAT, Naturwissenschaften
WP 7-10), sind entsprechend alle Themenfelder offen.

### Emirhan (Klasse 6, Niveaustufe D)

#### Deutsch (`GermanGenerator.cs`)

- [x] Vertiefung von Lese-/Schreibstrategien sowie sicheres Anwenden von Rechtschreibregeln (→ `GrossKleinschreibung`, `AdjektivSteigerung`)
- [x] Literarische Texte: Balladen sowie Kinder- und Jugendbücher (→ `Balladen`)
- [x] Sach- und Gebrauchstexte: Interviews, Zeitungsartikel, Grafiken auswerten (→ `SachtexteAuswerten`)
- [x] Texte in anderer medialer Form: Infosendungen, TV-Serien, Wikis, E-Mails (→ `MedialeTexte`)
- [x] Schreibformen: Schreibplan, formelle Briefe, Erzählungen, Berichte, Lesetagebücher, Parallelgedichte (→ `Schreibformen`)
- [x] Gesprächsformen/Redebeiträge: Diskussionen, Interviews, Präsentationen (→ `Gespraechsformen`)
- [x] Struktur und Wirkung von Sprache: Wortarten, Satzarten, Wortbildung (→ `Wortarten`, `Satzarten`, `Wortbildung`)

#### Mathematik (`MathGenerator.cs`)

- [x] 2.1 Zahlen und Operationen (Bruch-/Dezimalrechnung) (→ `BruchAddition`, `BruchMultiplikation`, `BruchDezimalUmwandlung`)
- [x] 2.2 Größen und Messen (Flächeninhalt/Volumen, Rechtecke/Quader) (→ `RechteckFlaeche`, `QuaderVolumen`)
- [x] 2.3 Raum und Form (Kongruenzabbildungen) (→ `Kongruenzabbildungen`)
- [x] 2.4 Gleichungen und Funktionen (Terme, proportionale Zuordnungen) (→ `ProportionaleZuordnung`)
- [x] 2.5 Daten und Zufall (Kombinatorik, relative Häufigkeit) (→ `Kombinatorik`, `WahrscheinlichkeitWuerfel`)

#### Englisch (`EnglischGenerator.cs`)

- [x] Themenfeld 1: Individuum und Lebenswelt (→ `AlltagUndFamilie`)
- [x] Themenfeld 2: Gesellschaft und öffentliches Leben (→ `SchuleUndGesellschaft`)
- [x] Themenfeld 3: Kultur und historischer Hintergrund (→ `KulturUndTraditionen`)
- [x] Themenfeld 4: Natur und Umwelt (→ `NaturUndUmwelt`)

#### Türkisch (`TurkishGenerator.cs`)

- [x] Themenfeld 1: Individuum und Lebenswelt (→ `AileVeGunlukYasam`)
- [x] Themenfeld 2: Gesellschaft und öffentliches Leben (→ `OkulVeToplum`)
- [x] Themenfeld 3: Kultur und historischer Hintergrund (→ `TurkiyeKulturu`)
- [x] Themenfeld 4: Natur und Umwelt (→ `DogaVeCevre`)

#### Naturwissenschaften 5/6 (kein eigenes Fach - verteilt auf Physik/Chemie/Biologie)

- [x] 3.1 Von den Sinnen zum Messen (→ Physik: `MessenUndSinne`)
- [x] 3.2 Stoffe im Alltag (→ Chemie: `StoffeImAlltag`)
- [ ] 3.3 Die Sonne als Energiequelle (Wasserkreislauf, Treibhauseffekt-Modell)
- [x] 3.4 Welt des Großen – Welt des Kleinen (→ Physik: `OptikUndWeltraum`)
- [ ] 3.5 Pflanzen – Tiere – Lebensräume (Winterschlaf, Frühblüher, Verbreitungsstrategien)
- [x] 3.6 Bewegung zu Wasser, zu Lande und in der Luft (Bionik) (→ Physik: `BewegungUndBionik`)
- [ ] 3.7 Körper und Gesundheit (Ernährungspyramide, Suchtprävention explizit)
- [x] 3.8 Sexualerziehung (→ Biologie: `PubertaetUndEntwicklung`)
- [x] 3.9 Technik (einfacher Stromkreis, Leiter/Nichtleiter) (→ Physik: `Stromkreis`)

#### Gesellschaftswissenschaften 5/6 / Gewi (`GewiGenerator.cs`)

- [x] 3.1 Ernährung – wie werden Menschen satt? (→ `Ernaehrung`)
- [x] 3.2 Wasser – nur Natur oder in Menschenhand? (→ `WasserAlsRessource`)
- [x] 3.3 Stadt und städtische Vielfalt (→ `StadtUndVielfalt`)
- [x] 3.4 Europa – grenzenlos? (→ `EuropaGrenzenlos`)
- [x] 3.5 Tourismus und Mobilität (→ `TourismusUndMobilitaet`)
- [x] 3.6 Demokratie und Mitbestimmung (→ `DemokratieUndMitbestimmung`)

#### Biologie (`BiologieGenerator.cs`)

- [x] 3.1 Die Zelle – kleinste Funktionseinheit des Lebendigen (→ `Zelle`)
- [x] 3.2 Lebensräume und ihre Bewohner (Nahrungsketten) (→ `LebensraeumeUndNahrungsketten`)
- [x] 3.3 Stoffwechsel des Menschen (→ `MenschlicheOrgane`)
- [x] 3.4 Sexualität, Fortpflanzung und Entwicklung (→ `PubertaetUndEntwicklung`)

#### Chemie (`ChemieGenerator.cs`)

- [x] 3.1 Faszination Chemie – Feuer, Schall und Rauch (→ `Verbrennung`)
- [x] 3.2 Das Periodensystem der Elemente – Übersicht und Werkzeug (Klasse-6-Niveau) (→ `PeriodensystemGrundlagen`)
- [x] 3.3 Gase – zwischen lebensnotwendig und gefährlich (→ `Gase`)
- [x] 3.4 Wasser – eine Verbindung (→ `Wasser`)
- [x] 3.5 Salze – Gegensätze ziehen sich an (→ `Salze`)
- [x] 3.6 Metalle – Schätze der Erde (→ `MetalleEigenschaften`)

#### Physik (`PhysikGenerator.cs`)

- [ ] 3.1 Thermisches Verhalten von Körpern (Wärmeausdehnung)
- [ ] 3.2 Wechselwirkung und Kraft
- [ ] 3.3 Mechanische Energie und Arbeit
- [ ] 3.4 Thermische Energie und Wärme

#### Geschichte (`GeschichteGenerator.cs`)

- [x] 3.1 Basismodule 7/8: Epochenüberblick (Mittelalter, Frühe Neuzeit, Revolutionen) (→ `Epochenueberblick`)
- [x] 3.2 Armut und Reichtum / Migrationen (→ `ArmutUndReichtumMigration`)
- [x] 3.3 Wahlmodule: z.B. Juden, Christen und Muslime (→ `JudenChristenMuslime`)

#### Geografie (`GeoGenerator.cs`)

- [ ] 3.1 Leben in Risikoräumen (Naturgefahren)
- [ ] 3.2 Migration und Bevölkerung (Flucht, Landflucht)
- [ ] 3.3 Vielfalt der Erde (tropischer Regenwald)
- [ ] 3.4 Armut und Reichtum (Klasse-6-Niveau)

#### Politische Bildung (`PolitikGenerator.cs`)

- [ ] 3.1 Armut und Reichtum (Klasse-6-Niveau)
- [ ] 3.2 Leben in einer globalisierten Welt
- [ ] 3.3 Migration und Bevölkerung
- [ ] 3.4 Leben in einem Rechtsstaat (Klassenregeln, Jugendschutz, Kinderrechte)

#### Ethik (`EthikGenerator.cs`)

- [ ] 3.1 Wer bin ich? – Identität und Rolle
- [ ] 3.2 Wie frei bin ich? – Freiheit und Verantwortung (Klasse-6-Niveau)
- [ ] 3.3 Was ist gerecht? – Recht und Gerechtigkeit (Klasse-6-Niveau)
- [x] 3.4 Was ist der Mensch? – Mensch und Gemeinschaft (Toleranz, Konflikte) (→ `Freundschaft`)

#### Kunst (`KunstGenerator.cs`)

- [x] 14.1 Inhaltsbereich: Kunstwerke (Konzepte 5/6) (→ `KunstwerkeWahrnehmen`)
- [x] 14.2 Inhaltsbereich: Material, Körper und Raum (→ `MaterialKoerperRaum`)
- [x] 14.3 Inhaltsbereich: Medien (→ `MedienUndVerfahren`)
- [x] 14.4 Inhaltsbereich: Individuelle Erfahrungen (Alltag und Lebenswelt) (→ `KunstUndLebenswelt`)
- [ ] 14.5 Inhaltsbereich: Verfahren und Werkzeuge (Gestaltungspraxis)

#### Musik (`MusikGenerator.cs`)

- [x] Themenfeld 1: Grundlagen der Musik (→ `GrundlagenDerMusik`)
- [x] Themenfeld 2: Form und Gestaltung (→ `FormUndGestaltung`)
- [x] Themenfeld 3: Gattungen und Genres (→ `GattungenUndGenres`)
- [x] Themenfeld 4: Wirkung und Funktion (→ `WirkungUndFunktion`)
- [x] Themenfeld 5: Musik im kulturellen Kontext (→ `MusikImKulturellenKontext`)

#### Sport - *Fach existiert noch nicht in der App*

- [ ] 16.1 Bewegungsfeld 1: Laufen, Springen, Werfen, Stoßen (Leichtathletik)
- [ ] 16.2 Bewegungsfeld 2: Spielen (Kleine Spiele & Sportspiele)
- [ ] 16.3 Bewegungsfeld 3: Bewegen an Geräten (Turnen & Parkour)
- [ ] 16.4 Optionales Bewegungsfeld: Kämpfen nach Regeln

#### ITG (`ItgGenerator.cs`)

- [ ] Themenfeld: Standardsoftware
- [x] Themenfeld: Informatiksysteme (Datenschutz-Grundlagen, sichere Passwörter) (→ `Datenschutz`, `SicherePasswoerter`)
- [x] Themenfeld: Leben in und mit vernetzten Systemen (Urheberrecht) (→ `Urheberrecht`)

### Batuhan (Klasse 9, Niveaustufe G)

#### Deutsch (`GermanGenerator.cs`)

- [x] 1.1 Literarische Texte erschließen (Analyse & Interpretation) (→ `DramaAufbau`, `Figurencharakterisierung`)
- [x] 1.2 Sach- und Gebrauchstexte auswerten (→ `Quellenkritik`)
- [x] 1.3 Texte in unterschiedlicher medialer Form (Filmanalyse) (→ `Filmanalyse`)
- [x] 1.4 Texte verfassen (Schreibformen & Argumentation) (→ `Textsorten`, `RedeUndBewerbung`)
- [x] 1.5 Gesprächsformen und Rhetorik (Mündlichkeit) (→ `RedeUndBewerbung`)
- [x] 1.6 Sprachwissen und Grammatik (Sprachbewusstheit) (→ `Satzbau`, `Wortbedeutung9`, `Konjunktionen`, `AktivPassiv`, `Kommasetzung`, `DassOderDas`)

#### Mathematik (`MathGenerator.cs`)

- [x] 1. Zahlen und Operationen (reelle Zahlen, Potenzgesetze) (→ `Potenzgesetze`)
- [x] 2. Größen und Messen (Trigonometrie, Körperberechnungen) (→ `Trigonometrie`, `PyramideKegelKugelVolumen`)
- [x] 3. Raum und Form (Satz des Thales/Pythagoras) (→ `SatzDesThales`, `SatzDesPythagoras`)
- [x] 4. Gleichungen und Funktionen (LGS, quadratische/Exponentialfunktionen) (→ `LinearesGleichungssystem`, `QuadratischeFunktionMerkmale`, `Exponentialfunktion`)
- [ ] 5. Daten und Zufall (Stochastik: mehrstufige Wahrscheinlichkeiten, Baumdiagramme)
- [x] 6. Wahlpflichtmodul A: Wachstumsprozesse (→ `Exponentialfunktion`)
- [ ] 7. Wahlpflichtmodul B: Darstellende Geometrie (Zweitafelprojektion)

#### Englisch (`EnglischGenerator.cs`)

- [x] 3.1 Persönlichkeit, Identität und Zukunft (→ `IdentitaetUndZukunft`)
- [ ] 3.2 Alltag, Konsum und Wohnwelt (Werbung, Verbraucherschutz)
- [x] 3.3 Gesellschaftliches Zusammenleben und Medien (→ `GesellschaftUndMedien`)
- [ ] 3.4 Schule, Ausbildung und Arbeitswelt (Bewerbung)
- [ ] 3.5 Kultur und historischer Hintergrund (Klasse-9-Niveau)
- [x] 3.6 Natur, Umwelt und Ökologie (→ `UmweltUndNachhaltigkeit`)

#### Türkisch (`TurkishGenerator.cs`)

- [x] 4.1 Identität, Lebenswelt und Migration (→ `KimlikVeGelecek`)
- [ ] 4.2 Alltag, Konsum und türkische Kultur
- [ ] 4.3 Gesellschaft und öffentliches Leben (Klasse-9-Niveau)
- [ ] 4.4 Schule, Ausbildung und Berufswelt
- [x] 4.5 Traditionen und historischer Hintergrund (→ `TarihVeGelenekler`)
- [x] 4.6 Natur, Umwelt und Regionen der Türkei (→ `TurkiyeCografyasi`)

#### Biologie (`BiologieGenerator.cs`)

- [x] 5.1 Gesundheit und Krankheit (Infektionsbiologie & Immunologie) (→ `Immunsystem`)
- [x] 5.2 Bau und Funktion des Nervensystems (Neurobiologie) (→ `Nervensystem`)
- [x] 5.3 Sucht und Suchtprävention (→ `SuchtUndSuchtpraevention`)
- [x] 5.4 Zelluläre Grundlagen der Vererbung (Mitose/Meiose) (→ `Humangenetik`)
- [x] 5.5 Vererbung beim Menschen (Humangenetik & Genmutationen) (→ `Humangenetik`)
- [x] 5.6 Evolution – Theorien und Stammesgeschichte (→ `Evolution`)

#### Chemie (`ChemieGenerator.cs`)

- [x] 6.1 Klare Verhältnisse – Quantitative Betrachtungen (Stöchiometrie) (→ `Stoechiometrie`)
- [x] 6.2 Säuren und Laugen – echt ätzend (→ `SaeureBaseVertieft`)
- [x] 6.3 Kohlenwasserstoffe – vom Campinggas zum Superbenzin (→ `Kohlenwasserstoffe`)
- [x] 6.4 Alkohole – vom Holzgeist zum Glycerin (→ `Alkohole`)
- [x] 6.5 Organische Säuren – Salatsauce, Entkalker & Co (→ `OrganischeSaeuren`)
- [x] 6.6 Ester – Vielfalt der Produkte aus Alkoholen und Säuren (→ `Ester`)

#### Physik (`PhysikGenerator.cs`)

- [x] 7.1 Gleichförmige und beschleunigte Bewegungen (Kinematik) (→ `Kinematik`)
- [x] 7.2 Kraft und Beschleunigung (Dynamik) (→ `NewtonscheGesetze`)
- [x] 7.3 Magnetfelder und elektromagnetische Induktion (→ `MagnetfelderInduktion`)
- [x] 7.4 Radioaktivität und Kernphysik (→ `RadioaktivitaetUndKernphysik`)
- [x] 7.5 Energieumwandlungen in Natur und Technik (→ `Energieerhaltung`)
- [x] 7.6 Schwingungen, Wellen und Optische Geräte (→ `SchwingungenWellenOptik`)

#### Geschichte (`GeschichteGenerator.cs`)

- [x] 8.1 Basismodul 1: Demokratie und Diktatur (→ `DemokratieUndDiktatur`)
- [x] 8.2 Basismodul 2: Der Kalte Krieg und die geteilte Welt (→ `KalterKrieg`)
- [x] 8.3 Verbundmodul 1: Konflikte und Konfliktlösungen (Nahost-Fallanalyse) (→ `KonflikteUndKonfliktloesungen`)
- [x] 8.4 Verbundmodul 2: Europa in der Welt (globalhistorischer Vergleich) (→ `EuropaInDerWelt`)
- [x] 8.5 Wahlmodul-Fokus A: Völkermorde und Massengewalt (→ `VoelkermordeUndMassengewalt`)
- [x] 8.6 Wahlmodul-Fokus B: Die Welt nach dem Kalten Krieg (1989-1991) (→ `WeltNachDemKaltenKrieg`)
- [ ] 8.7 (Bonus) Wahlmodul-Fokus C: Feindbilder und Propaganda

#### Geografie (`GeoGenerator.cs`)

- [x] 9.1 Umgang mit Ressourcen: Energie und Rohstoffe (→ `RessourcenEnergie`)
- [x] 9.2 Umgang mit Ressourcen: Landwirtschaft und Boden (→ `LandwirtschaftUndBoden`)
- [x] 9.3 Das Klimasystem und der Klimawandel (→ `Klimawandel`)
- [x] 9.4 Klimaschutz: Internationale Konflikte und Lösungen (→ `KlimaschutzInternational`)
- [x] 9.5 Wirtschaftliche Verflechtungen und Globalisierung (→ `WirtschaftlicheVerflechtung`)
- [x] 9.6 Europa in der Welt (Verbundmodul) (→ `EuropaWirtschaftsraum`)

#### Politische Bildung (`PolitikGenerator.cs`)

- [x] 10.1 Demokratie in Deutschland: Prinzipien und Institutionen (→ `Gewaltenteilung`, `BundestagBundesrat`)
- [x] 10.2 Demokratie in Deutschland: Willensbildung, Medien und Gefährdungen (→ `WillensbildungUndMedien`)
- [x] 10.3 Konflikte und Konfliktlösungen: internationale Akteure (→ `KonflikteInternationaleAkteure`)
- [x] 10.4 Friedenssicherung und Entwicklungspolitik (→ `FriedenssicherungUndEntwicklungspolitik`)
- [x] 10.5 Soziale Marktwirtschaft im Spannungsfeld (→ `SozialeMarktwirtschaft`)
- [x] 10.6 Europa in der Welt: Die Europäische Union (→ `EuropaeischeUnion`)

#### Ethik (`EthikGenerator.cs`)

- [x] 11.1 Wer bin ich? – Identität und Rolle (Kant/Autonomie, Gender) (→ `IdentitaetUndRolle`)
- [x] 11.2 Wie frei bin ich? – Freiheit und Verantwortung (→ `Verantwortung`, `FreiheitPhilosophisch`)
- [x] 11.3 Was ist gerecht? – Recht und Gerechtigkeit (→ `RechtUndGerechtigkeit`, `GerechtigkeitstheorienVertieft`)
- [x] 11.4 Was ist der Mensch? – Mensch und Gemeinschaft (Hobbes/Rousseau) (→ `MenschUndGemeinschaft`)
- [x] 11.5 Was soll ich tun? – Handeln und Moral (Dilemmata, Pflichtethik/Utilitarismus) (→ `HandelnUndMoral`)
- [x] 11.6 Worauf kann ich vertrauen? – Wissen und Glauben (→ `WissenUndGlauben`)

#### Kunst (`KunstGenerator.cs`)

- [x] 12.1 Kunst als Intervention und Mahnung (→ `KunstAlsInterventionUndMahnung`)
- [x] 12.2 Medienkunst und Bildhaftes Gestalten (→ `MedienkunstUndBildhaftesGestalten`)
- [x] 12.3 Architektur, Raum und Design (→ `ArchitekturRaumUndDesign`)
- [x] 12.4 Materialästhetik und Transformation (→ `MaterialaesthetikUndTransformation`)
- [x] 12.5 Inszenierung und Kuration (→ `InszenierungUndKuration`)
- [x] 12.6 Kulturelle Identität und Vielfalt (→ `KulturelleIdentitaetUndVielfalt`)

#### Musik (`MusikGenerator.cs`)

- [x] 13.1 Grundlagen der Musik: Harmonielehre und Partiturlesen (→ `HarmonielehreUndPartiturlesen`)
- [x] 13.2 Form und Gestaltung: Komposition und Satzweisen (→ `KompositionUndSatzweisen`)
- [x] 13.3 Medien und digitale Produktion (→ `MedienUndDigitaleProduktion`)
- [x] 13.4 Gattungen und Genres der Musikgeschichte (→ `GattungenDerMusikgeschichte`)
- [x] 13.5 Wirkung und Funktion: Filmmusik und Programmmusik (→ `FilmmusikUndProgrammmusik`)
- [x] 13.6 Musik im kulturellen und gesellschaftlichen Kontext

#### Sport - *Fach existiert noch nicht in der App*

- [ ] 14.1 Laufen, Springen, Werfen (Leichtathletik & Biomechanik)
- [ ] 14.2 Spielen (Sportspiele & Taktik)
- [ ] 14.3 Bewegen an Geräten (Turnen & Parkour)
- [ ] 14.4 Kämpfen nach Regeln (Zweikampf & Fair Play)
- [ ] 14.5 Bewegungsfolgen gestalten (Tanz, Akrobatik, Rhythmus)
- [ ] 14.6 Trainingsmethodik und Fitness

#### ITG (`ItgGenerator.cs`)

- [ ] Themenfeld: Standardsoftware
- [ ] Themenfeld: Informatiksysteme
- [x] Themenfeld: Leben in und mit vernetzten Systemen (Cybermobbing, Fake News) (→ `Cybermobbing`, `FakeNewsErkennen`)

#### Wirtschaft-Arbeit-Technik (WAT) - *Fach existiert noch nicht in der App*

- [ ] 16.1 Ernährung und Konsum aus regionaler und globaler Sicht (P9)
- [ ] 16.2 Unternehmerisches Handeln (P10)
- [ ] 16.3 Berufs- und Lebenswegplanung: Erkunden und Entscheiden (P11)
- [ ] 16.4 Gestaltung komplexer Projekte und Technikbewertung (P12)
- [ ] 16.5 Computergesteuerte Fertigung und Automatisierung (Wahlpflicht WP5)
- [ ] 16.6 Mobilität und Energieversorgung der Zukunft (Wahlpflicht WP7)

#### Naturwissenschaften (WP 7-10) - *Fach existiert noch nicht in der App*

- [ ] 17.1 Klima im Wandel
- [ ] 17.2 Energieversorgung der Menschheit
- [ ] 17.3 Kondensate zum Essen und Verpacken (Polymerchemie)
- [ ] 17.4 Nahrung für die Welt
- [ ] 17.5 Information und Kommunikation (Neurobiologie/Informatik)
- [ ] 17.6 Bauen, Wohnen und Bionik

### Übergreifende Themen (Basiscurricula, gemeinsam für beide Profile)

Die folgenden Punkte sind laut Rahmenlehrplan **fächerübergreifende Bildungsziele** (Teil B), keine
eigenständigen Quiz-Fächer - sie werden hier nur zur Vollständigkeit dokumentiert und absichtlich
**nicht** als Haken-Liste geführt, da sie sich nicht 1:1 in einzelne Quizfragen-Topics übersetzen
lassen: Sprach- und Medienbildung, Demokratiebildung, Nachhaltige Entwicklung, Kulturelle/
Interkulturelle Bildung, Inklusion und Vielfalt, Gesundheitsförderung, Berufs- und
Studienorientierung, Europabildung, Gewaltprävention, Gleichstellung/Gender Mainstreaming,
Mobilitätsbildung/Verkehrserziehung, Sexualerziehung, Verbraucherbildung. In der Praxis fließen
einzelne dieser Aspekte bereits implizit in bestehende Topics ein (z.B. Verbraucherbildung in
Gewi/`Ernaehrung`, Sexualerziehung in Biologie/`PubertaetUndEntwicklung`, Gewaltprävention in
Ethik/`Freundschaft`).

---

## 📋 Aktueller Implementierungsstand (Stand: 2026-07-17)

### ✅ Vollständig implementierte Fach-Generatoren (15 Fächer)

| Fach | Generator | Klasse 6 Topics | Klasse 9 Topics | Gesamt |
|------|-----------|----------------|----------------|--------|
| Mathematik | `MathGenerator.cs` | 12 | 14 | 26 |
| Deutsch | `GermanGenerator.cs` | 12 | 13 | 25 |
| Türkisch | `TurkishGenerator.cs` | 8 | 7 | 15 |
| Englisch | `EnglischGenerator.cs` | 7 | 6 | 13 |
| Biologie | `BiologieGenerator.cs` | 6 | 8 | 14 |
| Chemie | `ChemieGenerator.cs` | 9 | 9 | 18 |
| Physik | `PhysikGenerator.cs` | 6 | 7 | 13 |
| Geschichte | `GeschichteGenerator.cs` | 3 | 6 | 9 |
| Gewi | `GewiGenerator.cs` | 9 | 3 | 12 |
| Politik | `PolitikGenerator.cs` | 3 | 8 | 11 |
| Geografie | `GeoGenerator.cs` | 3 | 9 | 12 |
| Ethik | `EthikGenerator.cs` | 3 | 10 | 13 |
| Kunst | `KunstGenerator.cs` | 4 | 6 | 10 |
| Musik | `MusikGenerator.cs` | 5 | 6 | 11 |
| ITG | `ItgGenerator.cs` | 3 | 3 | 6 |

**Total: 208 Topics** (je Topic ~20 kuratierte Fragen → ~4.150 Fragen im Pool; Mathematik generiert
zusätzlich echte Zahlenwerte statt aus einer festen Liste zu ziehen, dort ist die Zahl möglicher
Aufgaben pro Topic praktisch unbegrenzt). Die Fach-Detailtabellen weiter oben in dieser Datei sowie
der Haken-Abgleich weiter unten wurden direkt aus `TopicsByGrade` in den Generator-Dateien
abgeleitet, nicht aus dem Gedächtnis geschätzt.

### 📰 News / RSS-Feeds (`LernTor.News/CuratedNewsFeeds.cs`)

**22 kuratierte RSS-Quellen** (keine Boulevard, ausschließlich öffentlich-rechtlich, Agenturen,
etablierte Regionalzeitungen, Hersteller-Feeds), gruppiert nach `DefaultCategory` (nur ein
Ausgangspunkt - `NewsCategoryClassifier` ordnet den tatsächlichen Artikeltext zusätzlich per
Schlüsselwort um, z.B. landet eine Minecraft-Meldung von tagesschau.de trotzdem in 🎮 Spiele):

| Kategorie | Feeds (Anzahl) | Quellen |
|-----------|----------------|---------|
| **Berlin** | 5 | rbb24, Tagesspiegel, Berliner Morgenpost, Bezirksamt Neukölln, Bezirksamt Friedrichshain-Kreuzberg |
| **Türkei** | 3 | Anadolu Ajansı, TRT Haber, DW Türkçe |
| **Spiele** | 5 | GameStar, Nintendo.de News, PlayStation Blog DE, Xbox News DE, Steam News |
| **KI / Technik** | 1 | IT Boltwise (heise online läuft kategorisiert als Deutschland, deckt aber KI-Themen mit ab) |
| **Deutschland / Allgemein** | 7 | tagesschau.de, Abgeordnetenhaus Berlin, fluter.de, heise online, Bundesregierung kompakt, Bundesregierung Pressemitteilungen, BMBFSFJ |
| **Welt / International** | 1 | Deutsche Welle |
| **Finanzen** (kein Feed) | 1 tägliches Erklärstück | rotierendes, kuratiertes Finanzwissen (`FinanceKnowledgeArticles`, Fix) |

**News-Auswahl-Logik** (in `RssNewsService.LoadCuratedArticlesAsync`) - bewusst einfach gehalten,
keine Quoten/Prioritäts-Rankings mehr:

```csharp
foreach (feed in CuratedNewsFeeds.All)   // 22 Feeds
    → genau der neueste Artikel dieses Feeds
    → bei ≤9 Jahren: komplett übersprungen, falls SensitiveKeywords treffen
    → nicht erreichbare Feeds werden einzeln übersprungen (Fehlerprotokoll)

+ 1 FinanceKnowledgeArticles.GetForDate(heute)   // täglich fix, kein Feed nötig

→ typischerweise ~22-23 Artikel/Tag (weniger, wenn Feeds ausfallen)
```

**Altersfilter** (Parameter `childAge`):
- **≤ 9 Jahre**: Artikel mit *SensitiveKeywords* werden **hart ausgefiltert** (keine Angstmache)
- **≥ 10 Jahre / null**: *SensitiveKeywords* nur **Ranking-Penalty** (nach unten stufen)

### ⚙️ Vollständig implementierte Kern-Komponenten

| Komponente | Status | Details |
|------------|--------|---------|
| **Core-Domain** | ✅ | Enums, Models, `ProgressGateService`, `ScoringService`, `LearningStageSubjects.Map` |
| **ContentGen** | ✅ | 14 Generatoren, `QuizComposer` (dynamische Quiz-Zusammensetzung), Review-/Mastered-Logik |
| **News** | ✅ | RSS-Loading, Vereinfachung, Verständnisfragen, Kategorisierung, Glossar, Bezirks-Erkennung |
| **Data (EF Core SQLite)** | ✅ | Repositories: Progress, ActivityLog, MasteredPrompt, ReviewQuestion, CustomQuestion, Settings, Rewards |
| **Security (Kiosk)** | ✅ | Keyboard-Hook, TaskMgr-Policy, Autostart, Admin-Auth |
| **App (WPF/MVVM)** | ✅ | MainVM, alle Views (ProfileSelection, Welcome, News, Exercise, FinalQuiz, Result, ParentSettings), QuestionCard, KI-Chat, TTS (Piper), Lehrer-Import (PDF/Word → KI → Entwürfe), Belohnungen, Wochenbericht |
| **Localization** | ✅ | DE/TR, String-Indexer, Live-Switch via `PropertyChanged("Item[]")` |
| **Local LLM** | ✅ | LLamaSharp, GGUF-Autodownload (~2-4 GB), 2 Features: Lehrer-Import + KI-Hausaufgaben-Chat |

### ❌ Bewusst nicht umgesetzt / Fehlend

| Thema | Grund |
|-------|-------|
| **Sport, Kunst-Praxis, Musik-Praxis, WAT, NaWi-WP 7-10** | RLP-Themenfelder lassen sich schlecht als Quiz abbilden (Bewegung, Gestalten, Musizieren, Werkstatt) |
| **Naturwissenschaften 5/6 (integriert)** | Nur über Physik/Chemie/Bio-Themen abgedeckt, kein eigenes Fach |
| **Standardsoftware (ITG)** | Textverarbeitung/Tabellenkalkulation nicht quizbar → bewusst weggelassen |
| **Literarische Textanalyse: Novelle, Parabel** | Drama ist inzwischen abgedeckt (`DramaAufbau`, `Figurencharakterisierung`); Novelle/Parabel bleiben offen |
| **Migration/Bevölkerung (Geo/Politik 9)** | Im RLP vorhanden, nicht 1:1 als Topic implementiert |
| **Kunst/Musik: "Verfahren und Werkzeuge" / "Standardsoftware" (ITG)** | Nur theoretisch (Wissen), keine praktische Übung möglich |
| **Offline-Erst-Installation des LLM** | Model-Download passiert erst bei erstem Nutzen (~2-4 GB), kein Pre-Bundle im Installer |
| **TTS für Türkisch** | Piper-Stimmen primär Deutsch/Englisch; Türkisch fehlt/experimentell |
| **Eltern-Export/Backup der Daten** | Nur "Alle Daten zurücksetzen", kein Export/Import von Profilen/Fortschritt |
| **Multi-Device Sync** | Nicht vorgesehen (lokal-only, SQLite) |
| **EF Core Migrations** | Nutzt `EnsureCreated()` + `SqliteSchemaUpdater` (additive Schema-Änderungen only) |
| **Installer-Signing (EV-Zertifikat)** | Für echte Distribution nötig (SmartScreen) |

---

### 🎯 Zusammenfassung

**App ist funktionskomplett für den Kern-Zweck:**

1. Kind loggt sich ein
2. **News lesen** (~22 Artikel: 1 pro Feed aus 22 RSS-Quellen + 1 Finanzwissen-Erklärstück; Berlin/Türkei/KI/Spiele feste Quellen, altersgerecht gefiltert)
3. **Übungen** in bis zu 15 Fächern (je nach Eltern-Einstellung aktiv, Klasse 6/9, ~20 Fragen/Topic, keine Wiederholung bei richtig)
4. **Abschlussquiz** (dynamisch verteilt auf aktive Fächer, ≥50% = PC frei)
5. Eltern steuern Fächer, Noten, LLM, Belohnungen, sehen Wochenbericht

**Die 3 neuen Generatoren (Kunst, Musik, Geschichte-Klasse-9-Themen) sind voll in die Pipeline integriert:**
- `LearningStageSubjects.Map` → Subject-Zuordnung
- `ProgressGateService.SequentialOrder` → Stufen-Reihenfolge
- `QuizComposer` → dynamische Quiz-Zusammensetzung
- `SubjectToTitleConverter` → Anzeigetitel
- `ParentSettingsViewModel.ToggleableSubjects` → Eltern-Toggles
- `Translations` (DE/TR) → UI-Texte
- `CURRICULUM.md` → Dokumentation
