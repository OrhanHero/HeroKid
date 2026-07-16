# Fach- und Stage-System

Dieses Dokument beschreibt die Lernstufen und Fächerlogik der App. Die zwei wichtigsten Quellen der
Wahrheit sind:

- [ProgressGateService.cs](../src/LernTor.Core/Services/ProgressGateService.cs) für die Reihenfolge
  der Lernstufen
- [LearningStageSubjects.cs](../src/LernTor.Core/Services/LearningStageSubjects.cs) für die
  Zuordnung von Fachstufen zu `Subject`

## Reihenfolge der Lernstufen

Die heutige Session läuft in dieser Reihenfolge:

1. Willkommen
2. Lesen
3. Tippen
4. News
5. Fachbereiche
6. Abschlussquiz
7. Freigeschaltet

`ProgressGateService.GetNextStage()` und `CanEnterStage()` arbeiten beide mit dieser Reihenfolge.
Wenn ein Bereich von Eltern deaktiviert wurde, wird er beim Durchlaufen übersprungen und als bereits
erledigt behandelt.

## Fachbereiche

Die Fachstufen sind über `LearningStageSubjects.Map` den Fach-Enums zugeordnet. Das ist die einzige
Stelle, an der die App wissen muss, welche `LearningStage` zu welchem `Subject` gehört. Diese
Zuordnung wird von mehreren Stellen genutzt:

- `ProgressGateService` für die Freischaltlogik
- `MainViewModel` für Navigation und Session-Leiste
- `QuizComposer` für das Abschlussquiz
- Eltern-Bereich für Deaktivierung einzelner Fächer

Aktuell gibt es 15 Fachbereiche:

- Mathematik
- Deutsch
- Türkisch
- Englisch
- Biologie
- Chemie
- Physik
- Geschichte
- Gewi
- Politik
- Geografie
- Ethik
- Kunst
- Musik
- ITG

`News` ist bewusst kein Fach, sondern eine eigene Pflichtstufe. `Tippen` ist ebenfalls eine eigene
Lernstufe und kein curriculares Fach.

## Curriculum und Generatoren

Die Fachthemen selbst werden in [docs/CURRICULUM.md](CURRICULUM.md) dokumentiert. Dort steht, welche
Themen je Fach und Klassenstufe aktuell als Generatoren vorhanden sind.

Für neue Themen gilt:

1. Neue Fragefunktion im passenden Generator ergänzen.
2. Die Funktion in `TopicsByGrade` eintragen.
3. Einen Test ergänzen, der die richtige Bewertung prüft.

Für ein komplett neues Fach kommen zusätzlich `Subject`, `LearningStage`, `LearningStageSubjects`,
`ProgressGateService`, `QuizComposer`, die WPF-Anzeige und die Lokalisierung dazu.

## Praktische Auswirkung im UI

Die Fortschrittsleiste im Kiosk-Fenster spiegelt diese Logik direkt wider. Deshalb ist die
Dokumentation der Stage-Reihenfolge auch für UI-Änderungen wichtig: Wenn ein neuer Bereich
hinzukommt oder die Reihenfolge geändert wird, müssen Navigation, Progress-Gate und die
Session-Anzeige gemeinsam angepasst werden.
