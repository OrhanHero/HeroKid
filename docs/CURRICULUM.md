# Lehrplan-Zuordnung (Berliner Rahmenlehrplan, Klasse 6 & 9)

Diese Übersicht zeigt, welche Themen die Generatoren in `src/LernTor.ContentGen/Generators`
aktuell abdecken. Es handelt sich um eine **repräsentative Auswahl** zentraler Themen je Fach und
Klassenstufe, nicht um eine vollständige 1:1-Abbildung des kompletten Rahmenlehrplans. Die
Architektur (ein `TopicFactory`-Delegate pro Thema in `ExerciseGeneratorBase`) ist bewusst so
gebaut, dass weitere Themen einfach als zusätzliche private Methode + Eintrag in `TopicsByGrade`
ergänzt werden können.

## Mathematik (`MathGenerator.cs`)

| Klasse 6 | Klasse 9 |
|---|---|
| Bruchrechnung: Addition | Lineare Gleichungen |
| Bruchrechnung: Multiplikation | Lineare Funktionen (Steigung/Achsenabschnitt) |
| Prozentrechnung (Prozentwert) | Quadratische Gleichungen (pq-Formel) |
| Negative Zahlen | Satz des Pythagoras |
| Flächen-/Umfangsberechnung (Rechteck) | Zinsrechnung (einfache Zinsen) |
| Maßstab | Binomische Formeln |

## Deutsch (`GermanGenerator.cs`)

| Klasse 6 | Klasse 9 |
|---|---|
| Wortarten (Nomen/Verb/Adjektiv) | Aktiv und Passiv |
| Zeitformen (Präsens/Präteritum/Perfekt) | Satzgefüge und Konjunktionen |
| Satzglieder (Subjekt/Prädikat/Objekt) | Kommasetzung |
| Groß- und Kleinschreibung | "dass" oder "das" |
| Steigerung von Adjektiven | Wortarten vertieft (Adverb/Präposition/Konjunktion) |

## Türkisch (`TurkishGenerator.cs`)

| Klasse 6 | Klasse 9 |
|---|---|
| Şimdiki Zaman (Präsens) | Cümlenin Ögeleri (Satzglieder) |
| Geçmiş Zaman (Präteritum) | Gelecek Zaman (Futur) |
| Eş Anlamlı Kelimeler (Synonyme) | Yazım Kuralları (Rechtschreibung) |
| Zıt Anlamlı Kelimeler (Antonyme) | Fiilimsi (Partizip/Verbalnomen) |

## Naturwissenschaften (`ScienceGenerator.cs`, kombiniert Physik/Chemie/Biologie)

| Klasse 6 | Klasse 9 |
|---|---|
| Aggregatzustände (Physik) | Atommodell (Chemie) |
| Einfacher Stromkreis (Physik) | Ohmsches Gesetz (Physik) |
| Der menschliche Körper (Biologie) | Zellbiologie |
| Fotosynthese (Biologie) | Chemische Reaktionen |
| Magnetismus (Physik) | Vererbung/Genetik |

## News (`LernTor.News`)

Kuratierte, kostenlose RSS-Quellen (siehe `NewsFeedSource.cs`):

- **Deutschland/Berlin**: tagesschau.de, rbb24 Berlin
- **Türkei/Istanbul/Samsun/Ünye**: Hürriyet, Sabah

Artikel werden nach Schlüsselwörtern priorisiert (Berlin, Deutschland, Istanbul, Samsun, Ünye,
Türkei) und auf die 5-8 relevantesten/aktuellsten reduziert. Pro Artikel werden automatisch zwei
Verständnisfragen erzeugt (Herkunfts-/Regionsfrage + Schlüsselwort-Frage aus der Überschrift).

## Erweiterung um weitere Themen

Neues Thema hinzufügen (Beispiel Mathematik):

1. Neue private `static QuizQuestion MeinNeuesThema(Random r) { ... }`-Methode in `MathGenerator.cs`.
2. Methode in die passende Liste (`TopicsByGrade[GradeLevel.KlasseX]`) eintragen.
3. Test in `tests/LernTor.Tests` ergänzen, der prüft, dass die eigene Musterlösung als richtig erkannt wird.

Kein Codeänderung an `ExerciseGeneratorBase`, `QuizComposer` oder den ViewModels nötig – die
Zufallsauswahl und Quiz-Zusammenstellung funktioniert automatisch mit neuen Themen.
