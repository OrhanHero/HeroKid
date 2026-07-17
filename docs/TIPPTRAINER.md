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

Der Trainer besteht aus 12 regulären Lektionen + 1 profil-spezifischer Abschluss-Lektion:

### Reguläre Lektionen (für alle Profile gleich)
1. **Grundreihe** (3 Lektionen): ASDF JKL; → erste Wörter → Kombinationen
2. **Oberreihe** (2 Lektionen): QWERTZUIOPÜ+ → Wörter
3. **Unterreihe** (2 Lektionen): YXCVBNM,.- → Wörter
4. **Zahlenreihe** (2 Lektionen): 1234567890 → Kombinationen (Telefonnummern, PLZ, Daten)
5. **Wörter & Silben** (1 Lektion): Häufige deutsche Wörter
6. **Sätze** (2 Lektionen): Einfache Sätze → Sätze mit Zahlen & Zeichen

### Profil-spezifische Abschluss-Lektion (Lektion 7)
Nach allen regulären Lektionen wird automatisch die passende Abschluss-Lektion freigeschaltet:
- **Emirhan Kahraman** (Klasse 6): Persönlicher Steckbrief-Text mit Name, Geburtsdatum (09.05.2014), Adresse (Weichselstraße 41, 12045 Berlin), Telefon (0173 2085640), Eltern (Orhan & Zehra Kahraman), Schule (Lemgo-Grundschule, 6c), Lieblingsessen (Nudeln mit Sahnesoße und Schnitzel), Hobbys (Fußball, Freunde treffen)
- **Batuhan Kahraman** (Klasse 9): Persönlicher Steckbrief-Text mit Name, Geburtsdatum (16.08.2011), gleiche Adresse, Telefon (01522 8467854), gleiche Eltern, Schule (Robert-Koch-Gymnasium, 9a), Lieblingsessen (Pommes und Nuggets), Hobbys (Fußball, Freunde treffen)

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
