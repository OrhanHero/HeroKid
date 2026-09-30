# Lehrplan-Zuordnung (Berliner Rahmenlehrplan, Klasse 6 / 7 / 9 + Doppeljahrgänge 8 & 10)

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

**Klassenstufen und Doppeljahrgänge**: Der Berliner Rahmenlehrplan ist in Doppeljahrgangsstufen
gegliedert. Die Aufgabenpools folgen dieser Gliederung: **Klasse 7 deckt inhaltlich 7/8 ab,
Klasse 9 deckt 9/10 ab.** Eltern können im Profil trotzdem die tatsächliche Klasse (6, 7, 8, 9
oder 10) eintragen - für Klasse 8 und 10 gibt es bewusst keine eigenen Pools, stattdessen greift
die Übergangsregel in `ExerciseGeneratorBase.Generate` auf die nächstniedrigere vorhandene Stufe
zu und trifft damit automatisch den passenden Doppeljahrgang (8 → Klasse-7-Pool, 10 →
Klasse-9-Pool, also der Stoff, der auch zum MSA führt). Die Tabellen unten zeigen deshalb drei
Spalten (Klasse 6 / 7 / 9), decken über diese Regel aber alle fünf wählbaren Stufen ab.

## Mathematik (`MathGenerator.cs`)

| Klasse 6 | Klasse 7 | Klasse 9 |
|---|---|---|
| Bruchrechnung – Addition | Rationale Zahlen | Lineare Gleichungen |
| Bruchrechnung – Multiplikation | Prozentrechnung | Lineare Funktionen |
| Prozentrechnung – Prozentwert | Zinsrechnung (einfach) | Quadratische Gleichungen (pq-Formel) |
| Negative Zahlen | Terme zusammenfassen | Satz des Pythagoras |
| Flächen- und Umfangsberechnung | Einfache Gleichungen | Zinsrechnung |
| Maßstab | Zuordnungen und Dreisatz | Binomische Formeln |
| Wahrscheinlichkeit bei Zufallsexperimenten | Winkel (Winkelsumme, Neben-/Scheitelwinkel) | Mittelwert und Median (Statistik) |
| Volumen von Quadern | Flächen von Vielecken (Dreieck/Parallelogramm/Trapez) | Trigonometrie im rechtwinkligen Dreieck |
| Bruch-Dezimalzahl-Umwandlung | Wahrscheinlichkeit (einstufig, Urne) | Satz des Thales |
| Direkt proportionale Zuordnungen | | Volumen von Pyramide, Kegel und Kugel |
| Kongruenzabbildungen | | Lineare Gleichungssysteme |
| Kombinatorik (systematisches Zählen) | | Quadratische Funktionen (Scheitelpunkt) |
| | | Exponentielles Wachstum |
| | | Potenzgesetze |
| | | Mehrstufige Zufallsversuche (Baumdiagramm, Pfadregeln) – seit 3.1 |

> **Klasse 7 (neu, im Aufbau):** Mathematik hat als erstes Fach einen eigenen Klasse-7-Pool
> (9 generative Themen nach RLP Sek I, Doppeljahrgang 7/8). Alle anderen Fächer fallen für
> Klasse-7-Profile übergangsweise auf ihren Klasse-6-Pool zurück (Wiederholung des zuletzt
> Gelernten, siehe `ExerciseGeneratorBase.Generate`) - der Klasse-7-Content wird Fach für Fach
> ergänzt. News-Vereinfachung behandelt Klasse 7 wie Klasse 9 (mild vereinfacht).

## Deutsch (`GermanGenerator.cs`)

| Klasse 6 | Klasse 7 | Klasse 9 |
|---|---|---|
| Wortarten | Konjunktiv und indirekte Rede | Aktiv und Passiv |
| Zeitformen (Tempus) | Adverbialsätze | Satzgefüge und Konjunktionen |
| Satzglieder | Sprachliche Bilder (Stilmittel) | Kommasetzung |
| Groß- und Kleinschreibung | Inhaltsangabe | "dass" oder "das" |
| Steigerung von Adjektiven | Argumentieren und Erörtern | Wortarten (vertieft) |
| Satzarten | Kurzgeschichten verstehen | Textsorten unterscheiden |
| Wortbildung | **Plusquamperfekt (Vorvergangenheit)** *(neu 30.09.2026)* | Aufbau eines Dramas |
| Balladen und Jugendbücher | **Adverbiale Bestimmungen** *(neu)* | Figurencharakterisierung |
| Sach- und Gebrauchstexte auswerten | **Attribute (Beifügungen)** *(neu)* | Argumentation und Quellenkritik |
| Texte in medialer Form (Wiki, E-Mail, TV) |  | Filmanalyse |
| Schreibformen |  | Rede, Debatte und Bewerbung |
| Gesprächsformen und Präsentieren |  | Satzbau und Sprachwissen |
|  |  | Wortbedeutung und Sprachwandel |
|  |  | Novelle |
|  |  | Parabel |

## Türkisch (`TurkishGenerator.cs`)

| Klasse 6 | Klasse 7 | Klasse 9 |
|---|---|---|
| Şimdiki Zaman (Präsens) | Şimdiki Zamanın Hikâyesi (-yordu) | Cümlenin Ögeleri (Satzglieder) |
| Geçmiş Zaman (Präteritum/-di'li geçmiş) | Belirsiz Geçmiş Zaman (-miş'li geçmiş) | Gelecek Zaman (Futur) |
| Eş Anlamlı Kelimeler (Synonyme) | Deyimler ve Atasözleri | Yazım Kuralları (Rechtschreibung) |
| Zıt Anlamlı Kelimeler (Antonyme) | Noktalama İşaretleri | Fiilimsi (Partizip/Verbalnomen) |
| Doğa ve Çevre (Natur und Umwelt) – Wortschatz | Metin Türleri (Textsorten) | Kimlik ve Gelecek (Identität und Zukunft) – Wortschatz |
| Aile ve Günlük Yaşam (Familie und Alltag) – Wortschatz | Medya ve İletişim – Wortschatz | Türk Tarihi ve Gelenekleri (Geschichte und Traditionen) |
| Okul ve Toplum (Schule und Gesellschaft) – Wortschatz | **Zarflar (Adverbien)** *(neu 30.09.2026)* | Türkiye'nin Coğrafyası (Geografie der Türkei) |
| Türk Kültürü ve Gelenekleri (Kultur und Traditionen) | **Zamirler (Pronomen)** *(neu)* | Alltag, Konsum und türkische Kultur – Wortschatz |
| Çoğul Eki -ler/-lar (Plural) *(neu 28.09.2026)* | **Şart Kipi -se/-sa (wenn/falls)** *(neu)* | Gesellschaft und öffentliches Leben (Klasse-9-Niveau) – Wortschatz |
| Hâl Ekleri -e/-de/-den (Wohin, wo, woher) *(neu)* | **Gereklilik Kipi -meli/-malı (müssen/sollen)** *(neu)* | Schule, Ausbildung und Berufswelt – Wortschatz |
| Soru Eki mi/mı/mu/mü (Fragepartikel) *(neu)* | **Yapım Ekleri ve Birleşik Kelimeler (Wortbildung)** *(neu)* | Söz Sanatları (Stilmittel) *(neu 28.09.2026)* |
| Sayılar, Günler ve Aylar (Zahlen und Zeit) – Wortschatz *(neu)* | **Hikâye Unsurları (Elemente einer Erzählung)** *(neu)* | Ses Olayları (Lautveränderungen) *(neu)* |
| Kısa Metin Anlama (Leseverstehen) *(neu)* | **Berlin'de Günlük Yaşam (Arzt, Verkehr, Behörde) – Wortschatz** *(neu)* | Sözcükte Anlam: Gerçek, Mecaz, Terim *(neu)* |
| **İyelik Ekleri (Possessivsuffixe)** *(neu 30.09.2026)* |  | Cümle Türleri (Satzarten) *(neu)* |
| **Geniş Zaman (Aorist)** *(neu)* |  | Türk Edebiyatından Yazarlar ve Eserler *(neu)* |
| **Emir Kipi ve Rica (Imperativ, Bitten)** *(neu)* |  | **Fiil Çatısı (Aktiv/Passiv, transitiv/intransitiv)** *(neu 30.09.2026)* |
| **Vücut ve Sağlık (Körper und Gesundheit) – Wortschatz** *(neu)* |  | **Ek Fiil (Kopula -dır/-dı/-mış/-sa)** *(neu)* |
| **Sıfatlarda Karşılaştırma: daha, en, kadar** *(neu)* |  | **Paragrafta Anlam (Thema, Hauptgedanke)** *(neu)* |
|  |  | **Bağlaçlar ve Edatlar (de/da, ki, ile …)** *(neu)* |
|  |  | **Anlatım Bozuklukları (Ausdrucksfehler)** *(neu)* |

Die Erweiterung vom 28.09.2026 (+100 Fragen je Stufe) folgt aus `scripts/pool-reichweite.py`:
Türkisch ist bei der Fächerauswahl nach Stundenplan jeden Tag dabei und war mit 160 bzw. 200
Fragen nach 5–7 Wochen einmal durch; jetzt reicht es für Klasse 6 knapp 9, für Klasse 9 zehn
Wochen. Die zweite Erweiterung vom 30.09.2026 bringt Klasse 6 auf 360 (12 Wochen), Klasse 7 auf
260 und Klasse 9 auf 400 Fragen (13,3 Wochen).

## Physik (`PhysikGenerator.cs`)

| Klasse 6 | Klasse 7 | Klasse 9 |
|---|---|---|
| Aggregatzustände | Optik: Licht, Schatten, Spiegel und Linsen | Ohmsches Gesetz |
| Einfacher Stromkreis | Kraft und Bewegung | Energieerhaltung |
| Magnetismus | Druck und Auftrieb | Newtonsche Gesetze |
| Von den Sinnen zum Messen | Wärmelehre: Temperatur und Wärmeübertragung | Magnetfelder und elektromagnetische Induktion |
| Welt des Großen – Welt des Kleinen (Optik und Weltraum) | Energieformen und Energieumwandlung | Gleichförmige und beschleunigte Bewegungen (Kinematik) |
| Bewegung zu Wasser, zu Lande und in der Luft (Bionik) | Elektrizität: Stromkreis und Wirkungen | Radioaktivität und Kernphysik |
| Thermisches Verhalten von Körpern (Wärmeausdehnung) | | Schwingungen, Wellen und optische Geräte |
| Wechselwirkung und Kraft | | |
| Mechanische Energie und Arbeit | | |
| Thermische Energie und Wärme | | |
| Die Sonne als Energiequelle (Wasserkreislauf, Treibhauseffekt) – seit 3.2 | | |

## Chemie (`ChemieGenerator.cs`)

| Klasse 6 | Klasse 7 | Klasse 9 |
|---|---|---|
| Stoffgemische trennen | Stoffe und ihre Eigenschaften | Atommodell |
| Verbrennung | Stofftrennverfahren | Chemische Reaktionen |
| Säuren und Laugen | Die chemische Reaktion | Periodensystem |
| Metalle und ihre Eigenschaften | Luft und Verbrennung | Klare Verhältnisse – Stöchiometrie |
| Stoffe im Alltag | Wasser, Lösungen und pH-Wert | Säuren und Laugen – echt ätzend |
| Das Periodensystem der Elemente – Übersicht und Werkzeug | Metalle und Korrosion | Kohlenwasserstoffe – vom Campinggas zum Superbenzin |
| Gase – zwischen lebensnotwendig und gefährlich | | Alkohole – vom Holzgeist zum Glycerin |
| Wasser – eine Verbindung | | Organische Säuren – Salatsauce, Entkalker & Co |
| Salze – Gegensätze ziehen sich an | | Ester – Vielfalt der Produkte aus Alkoholen und Säuren |

Organische Chemie (Kohlenwasserstoffe, Alkohole, organische Säuren, Ester) ist entgegen einer
früheren Design-Notiz in dieser Datei **inzwischen doch implementiert** (Klasse 9) - die Notiz war
nicht mehr aktuell und wurde entfernt (siehe Abschnitt "Abgleich mit dem offiziellen
Rahmenlehrplan" weiter unten).

## Biologie (`BiologieGenerator.cs`)

| Klasse 6 | Klasse 7 | Klasse 9 |
|---|---|---|
| Der menschliche Körper | Zelle und Zellteilung | Zellbiologie |
| Fotosynthese | Fotosynthese und Zellatmung | Vererbung (Genetik) |
| Wirbeltierklassen | Sinnesorgane und Reizverarbeitung | Ökosysteme |
| Pubertät und Entwicklung | Blut und Blutkreislauf | Gesundheit und Krankheit (Immunologie) |
| Die Zelle | Ökosystem Wald | Bau und Funktion des Nervensystems |
| Lebensräume und ihre Bewohner (Nahrungsketten) | Angepasstheit an Lebensräume | Sucht und Suchtprävention |
| Ernährung und Verdauung (seit 3.1) | | Vererbung beim Menschen (Humangenetik) |
| Blütenpflanzen: Blüte, Bestäubung, Samen (seit 3.1) | | Evolution – Theorien und Stammesgeschichte |
| Überwintern: Winterschlaf, Winterruhe, Winterstarre, Frühblüher (seit 3.2) | | |
| Gesund leben und Sucht vorbeugen (seit 3.2) | | |

> **Vokabeln**: Zusätzlich zu den Themenpools können Eltern im Eltern-Bereich eigene Wortlisten
> für Englisch und Türkisch hinterlegen (`VocabularyRepository`). Diese Vokabeln laufen im
> jeweiligen Fach mit und ersetzen dort bis zur Hälfte der generierten Aufgaben - sie verlängern
> den Tag also nicht. Abgefragt wird abwechselnd in beide Richtungen, mit eigener
> Wiederholungssteuerung (7/30/90 Tage, siehe `SpacedRepetitionSchedule`). Anders als bei der
> Fehler-Kartei verschwindet eine Vokabel nie aus dem Bestand.

## Englisch (`EnglischGenerator.cs`)

| Klasse 6 | Klasse 7 | Klasse 9 |
|---|---|---|
| Simple Present vs. Present Progressive | Simple Past vs. Past Progressive | Simple Past vs. Present Perfect |
| Unregelmäßige Pluralformen | going-to-Future vs. will-Future | Conditional Sentences (Type 1) |
| Question Words | Steigerung von Adjektiven (Comparison) | Passive Voice |
| Individuum und Lebenswelt: Alltag und Familie | some/any und much/many | Identität, Lebensentwürfe und Zukunft |
| Gesellschaft: Schule und Zusammenleben | Freizeit und Reisen (Wortschatz) | Gesellschaft, Medien und Vielfalt |
| Kultur und historischer Hintergrund | Großbritannien (Landeskunde) | Umwelt und Nachhaltigkeit |
| Natur und Umwelt | **Adverbs of manner** *(neu 30.09.2026)* | Alltag, Konsum und Wohnwelt (Werbung, Verbraucherschutz) |
| **Simple Past: regelmäßige und unregelmäßige Verben** *(neu 29.09.2026)* | **Question tags** *(neu)* | Schule, Ausbildung und Arbeitswelt (Bewerbung) |
| **Possessivbegleiter und Objektpronomen** *(neu)* | **Modalverben: must, mustn't, needn't, have to** *(neu)* | Kultur und historischer Hintergrund (Klasse-9-Niveau) |
| **Präpositionen in, on, at** *(neu)* | **Wortschatz: Freundschaft, Handy und Medien** *(neu)* | **Second Conditional** *(neu 29.09.2026)* |
|  |  | **Relative Clauses** *(neu)* |

## Gesellschaftswissenschaften / Gewi (`GewiGenerator.cs`)

| Klasse 6 | Klasse 7 | Klasse 9 |
|---|---|---|
| Geschichtliche Epochen | Armut und Gerechtigkeit | Grundgesetz |
| Kartenkunde und Himmelsrichtungen | Europa und die Europäische Union | Wirtschaftskreislauf |
| Kinderrechte | Migration und Vielfalt | Medien und Gesellschaft |
| Ernährung – wie werden Menschen satt? | Konsum und Verantwortung |  |
| Wasser – nur Natur oder in Menschenhand? | Medien und digitales Leben |  |
| Stadt und städtische Vielfalt | Nachhaltigkeit und Klima |  |
| Europa – grenzenlos? | **Islamische Welt im Mittelalter und Osmanisches Reich** *(neu 30.09.2026)* |  |
| Tourismus und Mobilität – schneller, weiter, klüger? | **Demokratie in Berlin: Land, Bezirk und Beteiligung** *(neu)* |  |
| Demokratie und Mitbestimmung | **Leben auf der Burg und im Kloster** *(neu)* |  |

## Politik (`PolitikGenerator.cs`)

| Klasse 6 | Klasse 7 | Klasse 9 |
|---|---|---|
| Was ist Demokratie? | Mitbestimmung und Engagement | Gewaltenteilung |
| Berlin und seine Bezirke | Rechtsstaat und Jugendrecht | Bundestag und Bundesrat |
| Wahlrecht | Parteien, Wahlen und Föderalismus | Wahlsystem |
| Armut und Reichtum (Klasse-6-Niveau) | Menschenrechte und internationale Politik | Soziale Marktwirtschaft |
| Leben in einer globalisierten Welt | Medien, Meinung und Öffentlichkeit (seit 3.1) | Demokratie in Deutschland: Willensbildung, Medien und Gefährdungen |
| Migration und Bevölkerung | Geld, Konsum und Verbraucherschutz (seit 3.1) | Konflikte und Konfliktlösungen: internationale Akteure |
| Leben in einem Rechtsstaat (Klassenregeln, Jugendschutz, Kinderrechte) |  |  |
|  |  | Friedenssicherung und Entwicklungspolitik |
|  |  | Europa in der Welt: Die Europäische Union |

## Geografie (`GeoGenerator.cs`)

| Klasse 6 | Klasse 7 | Klasse 9 |
|---|---|---|
| Kontinente und Ozeane | Klimazonen und Vegetationszonen | Plattentektonik |
| Klimazonen | Stadt- und Raumentwicklung | Klimawandel |
| Deutschland: Bundesländer | Wasser, Meere und Ressourcennutzung | Verstädterung |
| Leben in Risikoräumen (Naturgefahren) | Europa: Räume, Grenzen und Vielfalt | Armut und Reichtum weltweit |
| Migration und Bevölkerung (Flucht, Landflucht) | Landwirtschaft und Ernährung | Umgang mit Ressourcen: Energie und Rohstoffe |
| Vielfalt der Erde (tropischer Regenwald) | Naturgefahren und Naturrisiken | Umgang mit Ressourcen: Landwirtschaft und Boden |
| Armut und Reichtum (Klasse-6-Niveau) |  | Klimaschutz: Internationale Konflikte und Lösungen |
|  |  | Wirtschaftliche Verflechtungen und Globalisierung |
|  |  | Europa in der Welt (naturräumliche und wirtschaftliche Vielfalt) |

## Ethik (`EthikGenerator.cs`)

| Klasse 6 | Klasse 7 | Klasse 9 |
|---|---|---|
| Werte und Regeln | Freundschaft, Identität und Respekt | Verantwortung und Pflicht |
| Freundschaft und Konflikte | Weltreligionen und Zusammenleben | Meinungsfreiheit und Grenzen |
| Weltreligionen | Gerechtigkeit, Werte und Verantwortung | Digitale Ethik |
| Wer bin ich? – Identität und Rolle | Medien, Wahrheit und Verantwortung | Recht und Gerechtigkeit |
| Wie frei bin ich? – Freiheit und Verantwortung (Klasse-6-Niveau) | Tier- und Umweltethik | Wer bin ich? - Identität und Rolle |
| Was ist gerecht? – Recht und Gerechtigkeit (Klasse-6-Niveau) | Konflikt, Gewalt und Zivilcourage | Wie frei bin ich? - Freiheit und Verantwortung |
|  |  | Was ist gerecht? - Gerechtigkeitstheorien vertieft |
|  |  | Was ist der Mensch? - Mensch und Gemeinschaft |
|  |  | Was soll ich tun? - Handeln und Moral |
|  |  | Worauf kann ich vertrauen? - Wissen und Glauben |

## Kunst (`KunstGenerator.cs`)

| Klasse 6 | Klasse 7 | Klasse 9 |
|---|---|---|
| Kunstwerke wahrnehmen und beschreiben | Perspektive, Farbe und Bildaufbau | Kunst als Intervention und Mahnung |
| Material, Körper und Raum | Kunstepochen und Bildmedien | Medienkunst und bildhaftes Gestalten |
| Medien und Verfahren | Bild des Menschen: Figur, Porträt und Inszenierung | Architektur, Raum und Design |
| Kunst und meine Lebenswelt | Bild der Dinge: Objekt, Plastik und Design | Materialästhetik und Transformation |
|  | Comic und Bildgeschichte (seit 3.1) | Inszenierung und Kuration |
|  | Fotografie und Druckgrafik (seit 3.1) | Kulturelle Identität und Vielfalt |

## Musik (`MusikGenerator.cs`)

| Klasse 6 | Klasse 7 | Klasse 9 |
|---|---|---|
| Grundlagen der Musik | Musiklehre, Medien und Urheberrecht | Harmonielehre und Partiturlesen |
| Form und Gestaltung | Musikepochen und Stilrichtungen | Komposition und Satzweisen |
| Gattungen und Genres | Instrumentenkunde und Klangfarbe | Medien und digitale Produktion |
| Wirkung und Funktion | Musizieren: Rhythmus, Notation und Zusammenspiel | Gattungen und Genres der Musikgeschichte |
| Musik im kulturellen Kontext | **Tonleitern, Intervalle und Tonarten** *(neu 30.09.2026)* | Filmmusik und Programmmusik |
| **Notenwerte, Pausen und Takt** *(neu 29.09.2026)* | **Komponisten: Bach, Händel, Mozart, Beethoven** *(neu)* | Musik im kulturellen und gesellschaftlichen Kontext |
| **Stimme, Gesang und Chor** *(neu)* | **Musik der Welt: Türkei und andere Kulturen** *(neu)* |  |

## Geschichte (`GeschichteGenerator.cs`)

| Klasse 6 | Klasse 7 | Klasse 9 |
|---|---|---|
| Epochenüberblick: Mittelalter, Frühe Neuzeit, Revolutionen | Mittelalter: Lebenswelten | Demokratie und Diktatur |
| Armut und Reichtum, Migrationen | Entdeckungen und Kolonialismus | Der Kalte Krieg und die geteilte Welt |
| Juden, Christen und Muslime | Reformation und Glaubensspaltung | Konflikte und Konfliktlösungen |
|  | Absolutismus und Aufklärung | Europa in der Welt |
|  | Französische Revolution und ihre Folgen | Völkermorde und Massengewalt |
|  | Industrialisierung und soziale Frage | Die Welt nach dem Kalten Krieg (1989-1991) |
|  |  | Feindbilder und Propaganda |

## Medienbildung / ITG (`ItgGenerator.cs`)

| Klasse 6 | Klasse 7 | Klasse 9 |
|---|---|---|
| Datenschutz-Grundlagen | Algorithmen, Hardware und sicheres Arbeiten | Cybermobbing |
| Sichere Passwörter | Daten, Medien und digitale Werkzeuge | Fake News erkennen |
| Urheberrecht im Internet | Hardware, Netzwerke und Internet | Algorithmen-Grundbegriff |
|  | IT-Sicherheit und digitale Verantwortung | Informatiksysteme: Aufbau, Daten und Netze – seit 3.2 |

## KI-Bereich (`KiWissenGenerator.cs` + `KiContentService`)

> Kein Rahmenlehrplan-Fach, sondern KI-/Medienkompetenz als eigener Modulbereich: erst fünf
> Lernmodule (Texte in `KiContentService`, DE/TR), dann die "KI-Checkliste" als normale Übung.
> Klasse 7 hat seit 30.09.2026 einen eigenen Pool (vorher Rückfall auf Klasse 6). Distraktoren sind bewusst
> längen-balanciert (siehe `scripts/check-answer-length-bias.py`).
>
> Die Lernmodule bauen aufeinander auf: **Was ist KI?** (Werkzeug, kein Wesen) → **KI im Alltag**
> (wo sie schon überall steckt) → **Sicher mit KI** (Halluzinationen, Bias, Daten, Deepfakes) →
> **KI richtig nutzen** (die Arbeitsweise: erst selbst denken, gezielt fragen, nachprüfen, nicht
> abschreiben) → **Wo KI nicht hingehört** (sie kennt dich nicht, ist kein Freund, kein Arzt, kein
> Schiedsrichter; bei echten Sorgen sind Menschen zuständig - inklusive der Nummer gegen Kummer
> 116 111).
>
> Die letzten beiden Module sind die inhaltliche Mitte des Bereichs: KI als Werkzeug beherrschen,
> **ohne sie zur Referenz fürs eigene Leben zu machen**. Beide werden auch abgefragt - ein reiner
> Lesetext würde durchgeklickt. Zusätzlich steht unter dem "🤖 KI fragen"-Knopf in jeder Aufgabe
> dauerhaft der Hinweis, dass die KI sich irren kann und man die Aufgabe erst selbst versuchen
> soll (`Exercise_AiDisclaimer`) - dort, wo das Kind die KI tatsächlich benutzt.

| Klasse 6 | Klasse 7 | Klasse 9 |
|---|---|---|
| Wie KI funktioniert | **Wie eine KI lernt (Trainingsdaten)** *(neu 30.09.2026)* | Halluzinationen und Fakten-Check |
| KI im Alltag | **KI oder feste Regel?** *(neu)* | Bias und Verantwortung |
| KI-Checkliste: Sicher nutzen | **Falschmeldungen erkennen** *(neu)* | Wo KI nicht hingehört |
| KI richtig nutzen | **Daten und Privatsphäre** *(neu)* | Deepfakes und Datenschutz |

## News (`LernTor.News`)

44 kuratierte, kostenlose RSS-Quellen (siehe `CuratedNewsFeeds.All` in `NewsFeedSource.cs`) -
öffentlich-rechtlich, Nachrichtenagenturen, Bezirks-/Landesparlament, Forschungs- und
Hersteller-Feeds, bewusst keine Boulevardquellen. 27 deutsch, 9 türkisch, 8 englisch; die
vollständige Aufstellung nach Rubriken steht weiter unten unter "📰 News / RSS-Feeds".

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
- **Musik** (`MusikGenerator.cs`): 7 Themen für Klasse 6 (Grundlagen, Form/Gestaltung, Gattungen/Genres, Wirkung/Funktion, Kultureller Kontext, seit 29.09.2026 auch Notenwerte/Takt und Stimme/Gesang) und 6 Themen für Klasse 9 (Harmonielehre/Partitur, Komposition/Satzweisen, Medien/Digitale Produktion, Musikgeschichte, Filmmusik/Programmmusik, Gesellschaftlicher Kontext) – deckt die RLP-Themenfelder Grundlagen, Form, Gattungen, Wirkung und kultureller Kontext ab.

**Bewusst nicht übernommene/verbleibende Unterschiede** (kein technischer Mangel, sondern
Simplifizierungen dieser App gegenüber dem vollständigen RLP):

- **Biologie**: Der RLP führt Biologie als eigenständiges Fach erst ab Doppeljahrgangsstufe 7/8; in
  5/6 ist es Teil des integrierten Fachs Naturwissenschaften. Unsere Klasse-6-Themen (menschlicher
  Körper, Fotosynthese, Wirbeltierklassen) sind trotzdem sinnvoll, da die App Bio/Chemie/Physik aus
  Vereinfachungsgründen als getrennte Fächer ab Klasse 6 führt.
- **Türkisch**: Der RLP gliedert das Fach primär in kommunikative Themenfelder statt nach
  Grammatikthemen. `TurkishGenerator.cs` deckt inzwischen alle 4 kommunikativen Themenfelder
  (Individuum und Gesellschaft, Gesellschaft und öffentliches Leben, Kultur und historischer
  Hintergrund, Natur und Umwelt) beider Klassenstufen zusätzlich zu den grammatikorientierten
  Themen (Zeiten, Satzglieder, Rechtschreibung) ab - kein vollständiger Umbau, aber die RLP-
  Themenfelder sind jetzt genauso vollständig abgebildet wie die Grammatik.
- **Informatik/ITG**: Das RLP-Themenfeld "Standardsoftware" (praktischer Umgang mit
  Textverarbeitung/Tabellenkalkulation) lässt sich kaum als automatisch auswertbare Quizfrage
  abbilden und bleibt daher unberücksichtigt; die übrigen Themenfelder (Informatiksysteme, Leben in
  vernetzten Systemen) sind über die bestehenden Themen (Datenschutz, Cybermobbing, Fake News,
  Algorithmen) plausibel abgedeckt.
- **Deutsch**: Drama-Analyse (`DramaAufbau`, `Figurencharakterisierung`) sowie Novelle und Parabel
  (`Novelle`, `Parabel`) sind inzwischen abgedeckt - eine vollständige Abdeckung aller RLP-
  Themenfelder ist weiterhin nicht das Ziel dieser App (siehe Hinweis am Anfang dieser Datei).

Dieser Abgleich verbessert die Abdeckung gezielt, ist aber weiterhin keine vollständige 1:1-Analyse
aller RLP-Themenfelder für alle zehn Jahrgangsstufen - die App konzentriert sich bewusst auf Klasse 6
und 9.

## Vollständiger Abgleich mit dem Rahmenlehrplan

**Seit 3.2 (30.09.2026) steht der Abgleich im Code:** `RahmenlehrplanKatalog`
(`src/LernTor.ContentGen/Curriculum`) führt jedes Themenfeld je Fach und Doppeljahrgangsstufe mit
den Themen, die es üben. `RahmenlehrplanAbdeckungTests` prüft jede Zuordnung gegen die Generatoren,
und die Übersicht [`RAHMENLEHRPLAN.md`](RAHMENLEHRPLAN.md) wird daraus erzeugt. Die früheren
Hand-Checklisten für Emirhan (Klasse 6) und Batuhan (Klasse 9), abgeleitet aus dem Familien-Dokument
„LernTor Native – vollständiger und lückenloser Rahmenlehrplan Berlin“, sind vollständig in den
Katalog übernommen (186 Themenfelder, alle Zuordnungen geprüft).

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

| Fach | Generator | Klasse 6 | Klasse 7 | Klasse 9 | Gesamt |
|------|-----------|---------|---------|---------|--------|
| Mathematik | `MathGenerator.cs` | 12 | 9 | 14 | 35 |
| Deutsch | `GermanGenerator.cs` | 13 | 6 | 16 | 35 |
| Türkisch | `TurkishGenerator.cs` | 13 | 6 | 15 | 34 |
| Englisch | `EnglischGenerator.cs` | 10 | 6 | 11 | 27 |
| Biologie | `BiologieGenerator.cs` | 6 | 6 | 8 | 20 |
| Chemie | `ChemieGenerator.cs` | 9 | 6 | 9 | 24 |
| Physik | `PhysikGenerator.cs` | 10 | 6 | 7 | 23 |
| Geschichte | `GeschichteGenerator.cs` | 4 | 7 | 8 | 19 |
| Gewi | `GewiGenerator.cs` | 9 | 6 | 3 | 18 |
| Politik | `PolitikGenerator.cs` | 7 | 4 | 8 | 19 |
| Geografie | `GeoGenerator.cs` | 7 | 6 | 9 | 22 |
| Ethik | `EthikGenerator.cs` | 6 | 6 | 10 | 22 |
| Kunst | `KunstGenerator.cs` | 4 | 4 | 6 | 14 |
| Musik | `MusikGenerator.cs` | 7 | 4 | 6 | 17 |
| ITG | `ItgGenerator.cs` | 3 | 4 | 3 | 10 |
| KI-Wissen | `KiWissenGenerator.cs` | 4 | – (fällt auf 6 zurück) | 4 | 8 |

**Total: 347 Topics** (Stand 29.09.2026; Klasse 6: 124, Klasse 7: 86, Klasse 9: 137). Je Topic ~20
kuratierte Fragen; Mathematik generiert echte Zahlenwerte statt aus einer festen Liste zu ziehen,
dort ist die Zahl möglicher Aufgaben pro Topic praktisch unbegrenzt. Die Tabelle ist mit einem
kleinen Skript direkt aus `TopicsByGrade` in den Generator-Dateien gezählt, nicht geschätzt.
Wie lange ein Pool bei der Übung nach Stundenplan reicht, rechnet `scripts/pool-reichweite.py` aus.

### 📰 News / RSS-Feeds (`LernTor.News/NewsFeedSource.cs`)

**44 kuratierte RSS-Quellen** (kein Boulevard, ausschließlich öffentlich-rechtlich, Agenturen,
etablierte Regionalzeitungen, Forschungs- und Hersteller-Feeds) - 27 deutsch, 9 türkisch, 8
englisch. Pro Tag wird nur ein Teil davon abgefragt (`RssNewsService.SelectFeedsForDay`, siehe
Tagesrotation weiter unten); gruppiert ist die Liste nach `DefaultCategory`, was nur der
Ausgangspunkt ist - `NewsCategoryClassifier` ordnet den tatsächlichen Artikeltext zusätzlich per
Schlüsselwort um, z.B. landet eine Minecraft-Meldung von tagesschau.de trotzdem in 🎮 Spiele:

| Kategorie | Feeds (Anzahl) | Quellen |
|-----------|----------------|---------|
| **Deutschland** | 10 | tagesschau.de, Deutschlandfunk Nachrichten, ZDF logo! Kindernachrichten, Abgeordnetenhaus Berlin, fluter.de, Umweltbundesamt, heise online, Bundesregierung kompakt, Bundesregierung Pressemitteilungen, BMBFSFJ |
| **Türkei** | 8 | Anadolu Ajansı, Anadolu Ajansı Bilim-Teknoloji, TRT Haber, TRT Haber Bilim ve Teknoloji, TRT Haber Eğitim, DW Türkçe, BBC News Türkçe, Euronews Türkçe |
| **Wissen** | 7 | Spektrum.de, MDR Wissen, wissenschaft.de, BBC Science & Environment, NASA Breaking News, ESA Space News, ScienceDaily |
| **Berlin** | 6 | rbb24 Berlin, Tagesspiegel Berlin, Berliner Morgenpost, Bezirksamt Neukölln, Bezirksamt Friedrichshain-Kreuzberg, Bezirksamt Mitte |
| **Welt** | 5 | Deutsche Welle, BBC Newsround, BBC News World, DW English, Euronews English |
| **Spiele** | 5 | GameStar, Nintendo.de News, PlayStation Blog DE, Xbox News DE, Steam News |
| **Sport** | 2 | Sportschau, Anadolu Ajansı Spor |
| **KI / Technik** | 1 | IT Boltwise (heise online läuft kategorisiert als Deutschland, deckt aber KI-Themen mit ab) |
| **Finanzen** (kein Feed) | 1 tägliches Erklärstück | rotierendes, kuratiertes Finanzwissen (`FinanceKnowledgeArticles`, Fix) |

Diese Tabelle wurde aus `NewsFeedSource.All` erzeugt, nicht aus dem Gedächtnis geschätzt - die
vorherige Fassung stand noch auf 22 Quellen und listete die Rubriken Wissen und Sport gar nicht.
Ob die URLs auch leben, prüft `scripts/check-feeds.py` (wöchentlich per GitHub Action).

**News-Auswahl-Logik** (in `RssNewsService.LoadCuratedArticlesAsync`) - bewusst einfach gehalten,
keine Quoten/Prioritäts-Rankings mehr:

```csharp
foreach (feed in SelectFeedsForDay(heute))   // Tagesausschnitt aus 44 Feeds
    → genau der neueste Artikel dieses Feeds
    → bei ≤9 Jahren: komplett übersprungen, falls SensitiveKeywords treffen
    → nicht erreichbare Feeds werden einzeln übersprungen (Fehlerprotokoll)

+ 1 FinanceKnowledgeArticles.GetForDate(heute)   // täglich fix, kein Feed nötig

→ so viele Artikel wie im Eltern-Bereich eingestellt (StudentProfile.NewsArticleCount,
  Standard 12) + 1 Finanzstück; weniger, wenn Feeds ausfallen
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
3. **Übungen** in bis zu 15 Fächern (je nach Eltern-Einstellung aktiv, Klasse 6/9, ~20 Fragen/Topic; richtig beantwortete Aufgaben pausieren per Spaced Repetition 7/30/90 Tage und kehren dann zur Auffrischung zurück)
4. **Abschlussquiz** (dynamisch verteilt auf aktive Fächer, Bestehensschwelle pro Profil einstellbar, Standard ≥50% = PC frei)
5. Eltern steuern Fächer, Noten, LLM, Belohnungen, sehen Wochenbericht

**Die 3 neuen Generatoren (Kunst, Musik, Geschichte-Klasse-9-Themen) sind voll in die Pipeline integriert:**
- `LearningStageSubjects.Map` → Subject-Zuordnung
- `ProgressGateService.SequentialOrder` → Stufen-Reihenfolge
- `QuizComposer` → dynamische Quiz-Zusammensetzung
- `SubjectToTitleConverter` → Anzeigetitel
- `ParentSettingsViewModel.ToggleableSubjects` → Eltern-Toggles
- `Translations` (DE/TR) → UI-Texte
- `CURRICULUM.md` → Dokumentation
