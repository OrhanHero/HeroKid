using System.IO;
using System.Windows.Media.Imaging;

namespace LernTor.App.Controls;

/// <summary>
/// Die echten Bilddateien der Verkehrszeichen (PNG, 960 px Breite, aus Wikimedia Commons - siehe
/// <c>docs/FUEHRERSCHEIN.md</c> für Quelle und Lizenz je Zeichen). Als <c>Resource</c>-Items in
/// <c>LernTor.App.csproj</c> eingebunden, landen sie im Single-File-Build als eingebettete
/// Ressource, nicht als lose Datei neben der EXE - kein Nachladen zur Laufzeit, die App bleibt
/// vollständig offline.
///
/// <para><b>Warum PNG statt SVG-in-WPF:</b> WPF kann SVG nicht selbst zeichnen. Ein Paket wie
/// SharpVectors oder ein Build-Schritt SVG→XAML-Geometrie hätte dieselbe Fehlerklasse riskiert,
/// die dieses Projekt laut CLAUDE.md schon zweimal getroffen hat - XAML/Ressourcen, die sauber
/// kompilieren und erst beim Anzeigen werfen. Ein fertiges Rasterbild bei 960 px hat dagegen
/// genug Reserve für die größte Darstellung (200 px im Quiz) und ist trivial zu laden.</para>
/// </summary>
internal static class TrafficSignImages
{
    /// <summary>
    /// Zeichen, deren Bilddatei die Aufschrift (Zahl/Ortsname) schon selbst zeigt. Für diese
    /// darf <see cref="LernTor.Core.Models.TrafficSign.Text"/> nicht zusätzlich übers Bild
    /// gezeichnet werden, sonst steht die Zahl doppelt. Als eigene Liste geführt statt über
    /// <c>TrafficSign.Text == null</c>: der Text bleibt in den Katalogdaten stehen (er ist
    /// weiterhin die fachlich richtige Aufschrift für den nachgezeichneten Rückfallpfad, falls
    /// für eine Nummer je keine Bilddatei vorläge), nur das RENDERN mit Bild lässt ihn aus.
    /// </summary>
    private static readonly HashSet<string> BildEnthaeltAufschrift = new()
    {
        // Zahl/Ort/Wort ist Teil der amtlichen Vorlage (siehe scratchpad-Manifest der
        // Bildrecherche, je Nummer per Commons-Beschreibung gegengeprüft): "206" "STOP",
        // "108-10"/"110-10" "10 %", "262" "5,5 t", "264" "2 m", "265" "3,8 m", "274-50" "50",
        // "274.1"/"274.2" "30"/"ZONE", "278-50" "50" (grau, durchgestrichen), "310"/"311"
        // Beispielort, "314" "P", "1001-30" Streckenlänge, "1004-30" Entfernung, "1020-30"
        // "Anlieger frei", "1053-35" "bei Nässe".
        //
        // "206" fehlte hier zunächst - der ersten Bildrecherche fiel nur auf, was als Zahl oder
        // Ort auffällt, nicht das fest eingebrannte Wort "STOP". Sichtbar wurde der Fehler erst
        // beim Rendern über den echten TrafficSignVisual-Code (doppeltes "STOP"), nicht beim
        // Betrachten der Bilddatei allein - ein Grund mehr, jede neue Bilddatei probeweise mit
        // TrafficSignVisual zu rendern statt nur die Datei selbst anzusehen.
        "206", "108-10", "110-10", "262", "264", "265", "274-50", "274.1", "274.2", "278-50",
        "310", "311", "314", "1001-30", "1004-30", "1020-30", "1053-35"
    };

    private static readonly Dictionary<string, BitmapImage?> Cache = new();

    public static bool ZeichnetAufschriftSelbst(string number) => BildEnthaeltAufschrift.Contains(number);

    /// <summary>Lädt die Bilddatei für eine Zeichennummer, oder <c>null</c>, wenn keine mitgeliefert
    /// wird - dann greift in <see cref="TrafficSignVisual"/> der nachgezeichnete Rückfallpfad.</summary>
    public static BitmapImage? For(string number)
    {
        if (Cache.TryGetValue(number, out var cached))
        {
            return cached;
        }

        BitmapImage? bild = null;
        try
        {
            // ";component/..." statt des kuerzeren "pack://application:,,,/Assets/...": Letzteres
            // loest nur auf, wenn die AUSFUEHRENDE Assembly (die Entry Assembly des Prozesses)
            // zugleich die Assembly mit den eingebetteten Ressourcen ist. Das stimmt fuer LernTor.exe
            // selbst, aber nicht fuer LernTor.UiTests: dort ist der Testhost die Entry Assembly und
            // LernTor.dll (mit den PNGs) nur referenziert - die kurze Form faende dort nichts, ohne
            // Ausnahme zu werfen, und der Rueckfallpfad wuerde still einspringen. Die explizite
            // Assembly-Form funktioniert in beiden Faellen.
            var uri = new Uri($"pack://application:,,,/LernTor;component/Assets/Verkehrszeichen/{number}.png", UriKind.Absolute);
            var quelle = new BitmapImage();
            quelle.BeginInit();
            quelle.UriSource = uri;
            quelle.CacheOption = BitmapCacheOption.OnLoad;
            quelle.EndInit();
            quelle.Freeze();
            bild = quelle;
        }
        catch (IOException)
        {
            // Keine Bilddatei fuer diese Nummer eingebettet - kein Absturz, nur der
            // nachgezeichnete Pfad kommt zum Einsatz. TrafficSignCatalogImageTests haelt fest,
            // dass das fuer keines der 77 Zeichen tatsaechlich passiert.
        }

        Cache[number] = bild;
        return bild;
    }
}
