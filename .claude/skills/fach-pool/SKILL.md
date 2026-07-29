---
name: fach-pool
description: Aufgaben-Themenpool für ein Fach und eine Klassenstufe anlegen oder erweitern (z.B. "Klasse 7 Biologie", "neues Fach Musik"). Nutzen bei allen Änderungen an den kuratierten Fragenpools in LernTor.ContentGen/Generators.
---

# Themenpool für ein Fach anlegen

## Struktur

Jedes Fach ist eine `ExerciseGeneratorBase`-Unterklasse in
`src/LernTor.ContentGen/Generators/<Fach>Generator.cs` mit
`TopicsByGrade: IReadOnlyDictionary<GradeLevel, IReadOnlyList<TopicFactory>>`.
Ein Thema ist eine `static QuizQuestion Methode(Random r)`, die aus einem festen Array von
~20 Tupeln zieht. Neue Themen brauchen nur: privates Array + Methode + Eintrag in `TopicsByGrade`.

Zielumfang pro Klassenstufe: **5-6 Themen à ~20 Fragen**. Kleinere Pools erschöpfen sich schnell,
weil korrekt beantwortete Aufgaben per Spaced Repetition für 7/30/90 Tage pausieren.

## Inhaltliche Regeln

- **Curriculum**: Berliner Rahmenlehrplan der jeweiligen Jahrgangsstufe. Zielgruppe sind
  deutsch-türkische Kinder (10-15) in Berlin - Beispiele aus deren Lebenswelt wirken besser als
  abstrakte Lehrbuchfälle.
- **Jede Frage braucht `Explanation`** (erklärt das *Warum*, nicht nur die Lösung) und einen
  `HelpHint`, der die Regel dahinter zusammenfasst.
- **Distraktoren müssen längen-balanciert sein.** Kinder haben nachweislich das Muster
  "die längste Antwort ist die richtige" ausgenutzt - Altbestände lagen bei 70-93%. Falsche
  Optionen daher inhaltlich plausibel *und* ähnlich lang formulieren, nicht als offensichtlich
  absurde Kurzsätze.
- **Offene Eingabe** (`QuestionType.OpenText`) für alles Rechen- und Formbare (Grammatikformen,
  Rechnungen); Multiple Choice für Verständnis- und Wissensfragen.

## Ablauf

1. Themen und Fragen im Generator ergänzen (Doc-Kommentar der Klasse mitziehen).
2. Bias prüfen - **muss unter 60% liegen**, Ziel ~35%:
   ```bash
   python3 scripts/check-answer-length-bias.py
   ```
   Liegt der Wert zu hoch, Distraktoren nachschärfen. Für Altbestände gibt es
   `scripts/balance-answer-lengths.py --target=35 <datei>` (verlängert automatisch den jeweils
   längsten falschen Distraktor bei einem Teil der Fragen). Bewusst **nicht** auf 0% senken -
   sonst wird "nimm nie die längste" zum nächsten ausnutzbaren Muster.
3. `docs/CURRICULUM.md`: Tabelle des Fachs um die Spalte/Zeilen erweitern.
4. Test in `tests/LernTor.Tests/` ergänzen - für eine neue Klassenstufe die Zusicherung, dass
   sie einen eigenen Pool hat und nicht auf eine niedrigere Stufe zurückfällt.
5. Über den `push-check`-Skill absichern und veröffentlichen.

## Neues Fach (statt nur neue Klassenstufe)

Zusätzlich zu pflegen - `scripts/preflight.py` prüft diese Verdrahtung mit:
`Subject`- und `LearningStage`-Enums, `LearningStageSubjects.Map`,
`ProgressGateService.SequentialOrder`, Generatorliste im `QuizComposer`-Konstruktor,
`SubjectToTitleConverter`, Fächer-Umschalter im `ParentSettingsViewModel`,
DE/TR-Einträge in `Translations`.

## Übergangsregel bei fehlender Klassenstufe

`ExerciseGeneratorBase.Generate` fällt automatisch auf die nächstniedrigere vorhandene Stufe
zurück. Ein Fach ohne Klasse-7-Pool liefert also Klasse-6-Aufgaben statt gar nichts - deshalb
sind Teillieferungen unbedenklich.
