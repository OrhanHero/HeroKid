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

## Navigationsfluss

1. Das Dashboard startet nach der Lesestufe.
2. Jede Lektion öffnet die eigentliche Übung.
3. Nach Abschluss zeigt die Komplett-Ansicht den Weiter-Button für die nächste Lektion.
4. Sind alle Lektionen fertig, zeigt das Dashboard das Glückwunsch-Panel mit dem Button
   „Weiter zu News“.
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
- Das Abschluss-Panel im Dashboard ist nur sichtbar, wenn wirklich alle Lektionen abgeschlossen
  sind.
- Für die News-Navigation wird der bestehende Stage-Wechsel benutzt, damit das Verhalten konsistent
  mit dem restlichen App-Flow bleibt.
