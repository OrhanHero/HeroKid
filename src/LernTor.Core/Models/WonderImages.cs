namespace LernTor.Core.Models;

/// <summary>
/// Die Bilder der sieben Weltwunder der Antike als Ressourcen-Adressen.
///
/// <para><b>Alle sieben sind KI-generierte Rekonstruktionen</b>, von der Familie selbst erstellt
/// (siehe <c>src/LernTor.App/Assets/Weltwunder/QUELLEN.md</c>). Fotos kann es nicht geben: sechs
/// der sieben Bauwerke existieren nicht mehr. Deshalb steht an jedem Bild eine Unterschrift, die
/// genau das sagt - ein fotorealistisches Bild ohne diesen Hinweis würde mehr behaupten, als
/// irgendjemand weiß.</para>
///
/// <para><b>Die ausdrückliche Assembly-Form <c>/LernTor;component/...</c> ist Pflicht</b>, nicht
/// Geschmack: die Kurzform <c>pack://application:,,,/Pfad</c> findet eine eingebettete Ressource
/// nur, wenn die AUSFÜHRENDE Assembly zugleich die ist, in der die Ressource liegt. In den
/// UI-Tests ist das der Testhost, nicht <c>LernTor.dll</c> - dort fände die Kurzform still
/// nichts. Genau daran sind in diesem Projekt schon einmal alle Verkehrszeichen unbemerkt auf
/// den Rückfallpfad gerutscht, bei grünen Tests (siehe CLAUDE.md).</para>
/// </summary>
public static class WonderImages
{
    private const string Basis = "pack://application:,,,/LernTor;component/Assets/Weltwunder/";

    public static string PyramideGizeh => Basis + "pyramide-gizeh.png";

    public static string HaengendeGaerten => Basis + "haengende-gaerten.png";

    public static string Zeusstatue => Basis + "zeusstatue.png";

    public static string ArtemisTempel => Basis + "artemis-tempel.png";

    public static string Mausoleum => Basis + "mausoleum.png";

    public static string KolossRhodos => Basis + "koloss-rhodos.png";

    public static string LeuchtturmAlexandria => Basis + "leuchtturm-alexandria.png";

    /// <summary>
    /// Die Unterschrift, die unter jedem dieser Bilder steht.
    ///
    /// <para>Sie ist keine Höflichkeit, sondern der Kern des Themas: von sechs der sieben
    /// Bauwerke weiß niemand, wie sie wirklich aussahen. Wer ein Bild ohne diesen Satz zeigt,
    /// bringt den Kindern etwas bei, das er selbst nicht weiß.</para>
    /// </summary>
    public const string Hinweis = "🤖 KI-generierte Rekonstruktion - so könnte es ausgesehen haben. Niemand weiß es genau.";

    /// <summary>
    /// Sonderfall Koloss von Rhodos: das Bild zeigt ihn mit gespreizten Beinen über der
    /// Hafeneinfahrt. Das ist eine Legende aus Mittelalter und Neuzeit, keine antike
    /// Überlieferung - und genau das verneint eine Frage dieses Themas.
    ///
    /// <para>Das Bild bleibt trotzdem, weil es mit dieser Unterschrift die beste Lektion des
    /// ganzen Themas ist: ein Bild kann überzeugend aussehen und trotzdem falsch sein.</para>
    /// </summary>
    public const string HinweisLegende =
        "🤖 KI-generiert - und Achtung: dieses Bild zeigt die LEGENDE. Über der Hafeneinfahrt stand der Koloss in Wirklichkeit nicht.";
}
