# Tipptrainer

Der Tipptrainer ist eine eigene feste Lernstufe zwischen Lesen und News. Er besteht aus zwei
Oberflächen:

- [TypingDashboardView.xaml](../src/LernTor.App/Views/TypingDashboardView.xaml) zeigt alle Lektionen,
  Fortschritt, Sterne und das Abschluss-Panel, sobald alles erledigt ist.
- [TypingExerciseView.xaml](../src/LernTor.App/Views/TypingExerciseView.xaml) zeigt die eigentliche
  Übung mit Texteingabe, virtueller Tastatur, Zeit-/Fortschrittsanzeige und Abschluss-Button.

Die Steuerung läuft über die ViewModels:

- [TypingDashboardViewModel.cs](../src/LernTor.App/ViewModels/TypingDashboardViewModel.cs)
  verwaltet die Lektionen und bietet den Weiter-zur-News-Command für das Abschluss-Panel.
- [TypingExerciseViewModel.cs](../src/LernTor.App/ViewModels/TypingExerciseViewModel.cs)
  berechnet Live-Werte wie Position, Genauigkeit, WPM und Fortschritt.
- [MainViewModel.cs](../src/LernTor.App/ViewModels/MainViewModel.cs)
  entscheidet, ob nach dem Tipptrainer das Dashboard oder die News-Stufe angezeigt wird.

## Lektionsstruktur

Der Trainer besteht aus 11 regulären Lektionen + 1 profil-spezifischer Abschluss-Lektion (siehe
`TypingContentProvider.cs` für den Lektionspool):

### Reguläre Lektionen (für alle Profile gleich)
1. **Grundreihe** (3 Lektionen): ASDF JKL; → erste Wörter → Kombinationen
2. **Oberreihe** (2 Lektionen): QWERTZUIOPÜ+ → Wörter
3. **Unterreihe** (2 Lektionen): YXCVBNM,.- → Wörter
4. **Zahlenreihe** (2 Lektionen): 1234567890 → Kombinationen (Telefonnummern, PLZ, Daten)
5. **Wörter & Silben** (1 Lektion): Häufige deutsche Wörter
6. **Sätze** (1 Lektion): Einfache Sätze mit Satzzeichen (die zweite Sätze-Lektion mit Zahlen &
   Zeichen wurde entfernt, um Kinder nicht zu 5-6 Wiederholungen beim 10-Finger-Lernen zu zwingen)

Alle regulären Lektionen verlangen die im Eltern-Bereich pro Profil eingestellte Mindestgenauigkeit
(`StudentProfile.TypingMinAccuracy`, Presets 25/50/75/100%, Standard 25% - bewusst niedrig angesetzt,
damit das Weiterkommen nicht am 10-Finger-Erlernen selbst scheitert). `TypingExerciseService.CheckInput()`
nutzt diesen Wert statt der lesson-eigenen `TypingLesson.MinimumAccuracy` (die weiterhin als Fallback
dient, falls kein Profil-Wert übergeben wird) - so ist keine neue App-Version nötig, um die Hürde zu
verändern.

### Profil-spezifische Abschluss-Lektion (Lektion 12)
Nach allen regulären Lektionen wird automatisch GENAU EINE der beiden folgenden Abschluss-Lektionen
freigeschaltet (`TypingContentProvider.GetFinalLessonForProfile`, anhand des Profilnamens - "Emirhan"
im Namen → Emirhans Text, sonst Batuhans Text als Standard):
- **Emirhan Kahraman** (Klasse 6): kurzer persönlicher Steckbrief-Text (Name, Alter, Berlin, Schule
  6c, Lieblingsessen)
- **Batuhan Kahraman** (Klasse 9): kurzer persönlicher Steckbrief-Text (Name, Alter, Berlin, Schule
  9a, Lieblingsessen)

Beide Abschlusstexte wurden gekürzt (~110 Zeichen weniger als ursprünglich) und laufen ebenfalls mit
der pro Profil eingestellten Mindestgenauigkeit.

**Nur deutsches QWERTZ-Layout** – alle türkischen/englischen Lektionen wurden entfernt.

## Navigationsfluss

1. Das Dashboard startet nach der Lesestufe.
2. Jede Lektion öffnet die eigentliche Übung.
3. Nach Abschluss zeigt die Komplett-Ansicht den Weiter-Button für die nächste Lektion.
4. Sind alle Lektionen (inkl. persönlicher Abschluss-Lektion) fertig, zeigt das Dashboard das Glückwunsch-Panel mit dem Button „Weiter zu News“.
5. Dieser Button ruft denselben Stage-Wechsel auf, den das MainViewModel auch intern nutzt:
   `LearningStage.Tippen -> LearningStage.News`.

Damit ist die Navigation nicht mehr nur ein Text-Hinweis, sondern direkt aus dem Panel heraus
klickbar.

## Behobene WPF-Binding-Fallen

In [TypingExerciseView.xaml](../src/LernTor.App/Views/TypingExerciseView.xaml) wurden drei Bindings
auf `Mode=OneWay` umgestellt:

- `ProgressPercent` in der `ProgressBar.Value`
- `Typing_Words` im ersten `Run.Text`
- `Typing_Chars` im zweiten `Run.Text`

Der Grund: `Run.Text` hat in WPF ein ungewöhnliches Standardverhalten und ist nicht als
reibungsloses Read-Only-Target für normale Textanzeige gedacht. Ohne explizites `Mode=OneWay`
meldet WPF Binding-Fehler bzw. XAML-Ladefehler, obwohl die ViewModel-Werte korrekt sind. Die
Umstellung macht die Bindings eindeutig als reine Anzeige-Bindings lesbar und verhindert diese
Fehlerklasse.

## Praktische Hinweise

- Der Typing-Flow speichert Fortschritt pro Profil.
- Das Abschluss-Panel im Dashboard ist nur sichtbar, wenn wirklich alle Lektionen (inkl. profil-spezifischer Abschluss-Lektion) abgeschlossen sind.
- Für die News-Navigation wird der bestehende Stage-Wechsel benutzt, damit das Verhalten konsistent
  mit dem restlichen App-Flow bleibt.
