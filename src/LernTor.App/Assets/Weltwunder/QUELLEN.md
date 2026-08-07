# Bilder der sieben Weltwunder der Antike

**Alle sieben Bilder sind KI-generierte Rekonstruktionen**, erstellt von der Familie selbst.
Damit gibt es keine fremden Bildrechte - anders als bei Fotos, die immer dem Fotografen gehören.

## Warum es keine Fotos sein können

**Sechs der sieben antiken Weltwunder existieren nicht mehr.** Vom Koloss von Rhodos, den
Hängenden Gärten, der Zeusstatue, dem Artemis-Tempel, dem Mausoleum und dem Leuchtturm von
Alexandria gibt es kein einziges Foto auf der Welt. Jede Abbildung - in jedem Buch, auf jeder
Website, auch die hier - ist eine Vorstellung davon, wie es ausgesehen haben könnte.

Nur die Große Pyramide von Gizeh steht noch.

**Deshalb steht an jedem Bild in der App, dass es eine KI-generierte Rekonstruktion ist.** Ein
fotorealistisches Bild ohne diesen Hinweis würde mehr behaupten, als irgendjemand weiß - und
genau das ist der Punkt, den das Thema selbst den Kindern beibringen will: Wer beschreibt,
entscheidet mit.

## Dateien

Erwartete Dateinamen in diesem Ordner (PNG, Breite mindestens 960 px):

| Datei | Bauwerk | Heute |
|---|---|---|
| `pyramide-gizeh.png` | Große Pyramide von Gizeh | steht noch |
| `haengende-gaerten.png` | Hängende Gärten von Babylon | verschwunden, Existenz umstritten |
| `zeusstatue.png` | Zeusstatue des Phidias in Olympia | verschwunden |
| `artemis-tempel.png` | Tempel der Artemis in Ephesos (Türkei) | verschwunden |
| `mausoleum.png` | Mausoleum von Halikarnassos (Bodrum, Türkei) | verschwunden |
| `koloss-rhodos.png` | Koloss von Rhodos | verschwunden |
| `leuchtturm-alexandria.png` | Leuchtturm (Pharos) von Alexandria | verschwunden |

Fehlt eine Datei, wird an der betreffenden Stelle einfach kein Bild angezeigt - die App bricht
deswegen nicht ab (dasselbe Verhalten wie bei `TrafficSignImages`).

## Sachliche Anmerkungen zu einzelnen Bildern

Beim Ansehen der ersten Fassung sind drei Dinge aufgefallen; zwei davon wurden nachgebessert:

- **Pyramide** - die erste Fassung zeigte einen Wagen mit Rädern. Die Blöcke wurden auf
  Schlitten über angefeuchteten Sand gezogen; das Rad kam dafür nicht zum Einsatz. **Korrigiert:**
  das jetzige Bild zeigt Schlitten und das Anfeuchten des Sandes.
- **Artemis-Tempel** - die erste Fassung hatte korinthische Kapitelle (Akanthusblätter). Der
  Artemis-Tempel war **ionisch**. **Korrigiert:** das jetzige Bild zeigt ionische Voluten.
- **Koloss von Rhodos** - das Bild zeigt die Statue mit gespreizten Beinen über der
  Hafeneinfahrt. **Das ist eine Legende aus Mittelalter und Neuzeit**, keine antike Überlieferung;
  technisch wäre es damals kaum möglich gewesen. Eine Frage im Thema verneint das ausdrücklich.
  Das Bild bleibt trotzdem - aber nur mit dem Hinweis, dass es die *Legende* zeigt. So wird aus
  dem Widerspruch die beste Lektion des Themas: ein Bild kann überzeugend aussehen und trotzdem
  falsch sein.

## Einbindung

Die Dateien werden über `<Resource Include="Assets\Weltwunder\*.png" />` in
`LernTor.App.csproj` in die Assembly eingebettet - nicht als lose Dateien neben der EXE. Sie
überstehen damit den Single-File-Publish unverändert, und die App bleibt vollständig offline;
zur Laufzeit wird nichts nachgeladen.
