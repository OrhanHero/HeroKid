# Designs in LernTor

Seit Version 3.0 (30.09.2026) kann jedes Kind sein Design, seine Schrift und seine Textgröße
selbst wählen („🎨 Mein Design“ auf der Startseite). Diese Seite beschreibt, wie das technisch
aufgebaut ist, welche Regeln gelten und wie man ein Design hinzufügt. Den Plan dazu enthält
[`NAECHSTES-LEVEL-3.md`](NAECHSTES-LEVEL-3.md).

## Was die Kinder sehen

| Wahl | Möglichkeiten |
|---|---|
| **Design** | 💜 Lavendel (Standard), 🌊 Ozean, 🌲 Wald, 🌅 Sonnenuntergang, 🍬 Bonbon, 🌙 Nacht (dunkel), 🔲 Hoher Kontrast, 🌌 Galaxie (dunkel, 🔒 wird frei mit dem Abzeichen „📅 Zehn Lerntage“) |
| **Schrift** | Standard (Segoe UI), Gut lesbar (Verdana), Verspielt (Comic Sans MS) – alles Schriften, die jedes Windows mitbringt |
| **Textgröße** | 100 %, 110 %, 120 % |
| **Automatisch dunkel** | „Wie Windows“ (folgt dem hellen/dunklen Modus in den Windows-Einstellungen), „Abends ab 19 Uhr dunkel“ (bis 6 Uhr) – dann gilt „Nacht“, außer das gewählte Design ist schon dunkel |

- Jede Änderung wirkt **sofort** und wird beim Profil gespeichert.
- Die **Profilwahl** zeigt immer das Standard-Design, weil sie allen Kindern gehört.
- Der **Eltern-Bereich** zeigt ebenfalls immer das Standard-Design. Er nennt je Profil die Wahl des Kindes und kann sie auf den Standard zurücksetzen.
- Ist ein gewähltes Design gesperrt, etwa nach dem Einspielen einer älteren Sicherung ohne das Abzeichen, gilt Lavendel.

## Aufbau

```
LernTor.Core/Design                         LernTor.App
─────────────────────────────────           ────────────────────────────────────────
DesignPalette     22 Farbrollen (#RRGGBB)   Resources/Colors.xaml   Standardwerte = Lavendel
DesignTheme       Id, Name DE/TR, dunkel?   Services/ThemeService   legt das Design als letztes
DesignThemeCatalog  die acht Designs                                Wörterbuch in App.Resources
DesignResourceKeys  Rolle → Ressourcenschlüssel                     (DynamicResource wechselt mit)
DesignContrastRules WCAG-Regeln             ViewModels/DesignPickerViewModel   die Galerie
DesignPreferences   Wahl des Kindes         Views/DesignPickerView
DesignSelection     was gerade gilt         MainViewModel: anwenden bei Profilstart und
ColorContrast       Kontrast nach WCAG 2.2  jedem Etappenwechsel
```

- **Rollen statt Farben.** Eine Ansicht fragt nie „Lila“, sondern „Hauptfarbe“ (`PrimaryBrush`), „Fläche“ (`CardBrush`), „Schrift auf Farbflächen“ (`OnColorBrush`) usw. Die Schlüssel sind die alten Namen aus `Colors.xaml`, deshalb musste keine Ansicht umbenannt werden.
- **Nur `DynamicResource`.** `ThemeService` tauscht zur Laufzeit ein Wörterbuch aus. Eine Stelle mit `StaticResource` behielte die alte Farbe. `scripts/preflight.py` meldet das (`design-static`).
- **Keine festen Farben in Ansichten.** `#RRGGBB` und `"White"` in `Views/` und `Controls/` meldet ebenfalls `preflight.py` (`design-hexfarbe`). Ausnahme sind die Fingerfarben der Tipp-Tastatur: Das sind feste Lernfarben, kein Design.
- **Textgröße** ist eine Skalierung des ganzen Fensterinhalts (`LayoutTransform` in `MainWindow`), nicht 505 einzelne Schriftgrößen.
- **Schrift** ist die Ressource `AppFontFamily`. Fenster und Stile holen sie dynamisch.
- **Speicherung:** fünf Spalten in `Profiles` (`DesignThemeId`, `DesignFont`, `DesignTextScalePercent`, `DesignFollowWindows`, `DesignDarkInEvening`), geschrieben nur von `StudentProfileRepository.SetDesignAsync`. Die Spalten sind bewusst **nicht** in `ProfileSettings`, sonst überschriebe „Speichern“ im Eltern-Bereich die Wahl des Kindes. Alte Datenbanken bekommen die Spalten automatisch (`SqliteSchemaUpdater`), leere Werte bedeuten Standard.

## Die Farbrollen

| Rolle | Schlüssel | Wofür |
|---|---|---|
| Background | `BackgroundBrush` | Fensterhintergrund |
| Surface | `CardBrush` | Karten, Flächen |
| TextPrimary | `TextPrimaryBrush` | normaler Text |
| TextSecondary | `TextSecondaryBrush` | gedämpfter Text, zweitrangige Knöpfe |
| Primary / PrimaryDark | `PrimaryBrush` / `PrimaryDarkBrush` | Hauptfarbe, Hover |
| OnColor | `OnColorBrush` | Schrift auf Hauptfarbe, Erfolg, Fehler, zweitrangigen Knöpfen |
| Accent | `AccentBrush` | Hervorhebungsfläche (Tipp-Kasten, Sprach-Etiketten) mit normalem Text |
| Focus | `FocusBrush` | Tastatur-Fokusring, neue Abzeichen |
| Success / Error | `SuccessBrush` / `ErrorBrush` | richtig/falsch – als Schrift und als Fläche |
| TileLavender/Sand/Mint/Rose | `Tile…Brush` | Kachelflächen mit normalem Text |
| ProgressTrack | `ProgressTrackBrush` | Hintergrund von Fortschrittsbalken |
| InputBorder | `InputBorderBrush` | Rahmen von Eingabefeldern |
| Math/German/Turkish/Science/News | `…Brush` | Fächerfarben (große Schrift, Grafik) |

## Kontrastregeln (WCAG 2.2, Stufe AA)

`DesignContrastRules` prüft jedes Design, `DesignThemeCatalogTests` lässt die CI rot werden, wenn eine Regel verletzt ist:

| Mindestens | Vordergrund | auf |
|---|---|---|
| **4,5 : 1** | TextPrimary | Background, Surface, Accent, alle vier Kacheln |
| **4,5 : 1** | TextSecondary | Background, Surface |
| **4,5 : 1** | OnColor | Primary, PrimaryDark, Success, Error, TextSecondary |
| **4,5 : 1** | Primary, Success, Error (als Schrift) | Background, Surface |
| **3 : 1** | Fächerfarben, Focus | Background, Surface |

Das bisherige Aussehen fiel am 30.09.2026 durch. Grün erreichte auf Weiß 2,5 : 1, Orange 2,2 : 1, der gelbe Fokusring 1,5 : 1. Lavendel hat deshalb dunklere Grün-, Rot- und Orangetöne und eine Spur dunklere Hauptfarbe.

## Ein Design hinzufügen

1. In `DesignThemeCatalog.All` einen Eintrag anhängen: eine neue Id aus Kleinbuchstaben (nie eine bestehende umbenennen, sie ist beim Profil gespeichert), Emoji (keine Flagge, siehe `CLAUDE.md`), Name und Beschreibung auf Deutsch und Türkisch, `IsDark` und alle 22 Rollen. Vergisst man eine Rolle, schlägt der Compiler an (`required`).
2. Die Farben so wählen, dass die Kontrastregeln erfüllt sind: `dotnet test --filter DesignThemeCatalogTests` nennt jeden Verstoß mit Wert.
3. Soll ein Abzeichen es freischalten, `UnlockAchievementId` setzen. Der Test prüft, dass es das Abzeichen gibt.
4. Den Test `Katalog_hat_acht_Designs…` auf die neue Anzahl anpassen.
5. Nach dem Push die **Bildschirmfotos** ansehen (siehe unten).

## Bildschirmfotos (Artefakt „Design-Vorschau“)

Die Entwicklung läuft ohne Windows-Bildschirm. Deshalb rendert `DesignScreenshotTests` in der CI
Profilwahl, Startseite, Übung (Auswahl und offene Antwort), den Geschafft-Bildschirm, „Mein
Fortschritt“ und die Design-Galerie in **jedem Design** in 1366 × 768 als PNG. Dazu kommt Lavendel in
**120 %** mit der Schrift „Gut lesbar“, weil dort am ehesten etwas abgeschnitten wird. Die Bilder
liegen bei jedem CI-Lauf unter **Actions → Lauf → Artifacts → „Design-Vorschau“** (30 Tage).
Dateiname: `<design>_<ansicht>.png`.

Der Test prüft nur, dass jede Ansicht zeichnet und nicht leer ist. **Ob es gut aussieht, entscheidet
ein Mensch.** Nach jeder sichtbaren Änderung die Bilder durchblättern.

## Bekannte Grenzen

- Die Kästchen von `CheckBox`/`RadioButton` und die Klappliste von `ComboBox`/`DatePicker` (nur in den Planer-Dialogen) bleiben im Windows-Stil. Die Beschriftungen folgen dem Design (implizite Stile in `Styles.xaml`).
- Eine Ansicht, die beim Umschalten schon offen ist, wechselt die Farbe über `DynamicResource`. WPF benachrichtigt dafür aber nur Elemente in einem Fenster. Das prüft der Testplan am echten Bildschirm (V.3-2), nicht die CI.
- Die Farben der Tipp-Tastatur (Finger) und der Verkehrszeichen sind fest. Sie sind Lerninhalt, kein Design.
