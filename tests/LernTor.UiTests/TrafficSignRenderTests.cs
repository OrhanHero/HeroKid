using System.Windows;
using System.Windows.Media;
using LernTor.App.Controls;
using LernTor.Core.Services;
using Xunit;

namespace LernTor.UiTests;

/// <summary>
/// Prüft die handgeschriebenen Geometriepfade des Verkehrszeichen-Katalogs mit dem echten
/// WPF-Parser.
///
/// <para>Der Katalog beschreibt rund 75 Zeichen über Pfad-Strings. Ein Tippfehler darin ist
/// in Core nicht erkennbar - dort ist es einfach ein String. Erst <c>Geometry.Parse</c> sagt,
/// ob daraus eine Figur wird. Die Zeichenfläche fängt einen kaputten Pfad zwar ab (sie lässt
/// das Piktogramm dann weg, statt die Ansicht mitzureißen), aber ein Verkehrszeichen ohne Bild
/// ist im Quiz wertlos - deshalb muss es hier auffallen und nicht vor dem Kind.</para>
/// </summary>
public sealed class TrafficSignRenderTests
{
    [Fact]
    public void Alle_Piktogramme_sind_gueltige_Geometrien()
    {
        var kaputt = new List<string>();

        foreach (var sign in TrafficSignCatalog.All)
        {
            foreach (var (label, pathData) in new[]
                     {
                         ("PathData", sign.PathData),
                         ("OverlayPathData", sign.OverlayPathData)
                     })
            {
                if (string.IsNullOrWhiteSpace(pathData))
                {
                    continue;
                }

                try
                {
                    var geometry = Geometry.Parse(pathData);

                    // Ein syntaktisch gültiger, aber leerer Pfad wäre genauso unbrauchbar.
                    if (geometry.Bounds.IsEmpty || geometry.Bounds.Width <= 0)
                    {
                        kaputt.Add($"{sign.Number} {sign.Name}: {label} ergibt eine leere Figur.");
                    }
                }
                catch (FormatException ex)
                {
                    kaputt.Add($"{sign.Number} {sign.Name}: {label} ist kein gültiger Pfad ({ex.Message}).");
                }
            }
        }

        Assert.Empty(kaputt);
    }

    [Fact]
    public void Piktogramme_bleiben_im_Zeichenfeld()
    {
        // Alle Pfade rechnen in einem Feld von 0..100. Wer darüber hinausragt, wird am Rand
        // des Schildes abgeschnitten - das fällt beim Schreiben des Pfades nicht auf.
        const double Toleranz = 6.0;
        var ausserhalb = new List<string>();

        foreach (var sign in TrafficSignCatalog.All)
        {
            foreach (var (label, pathData) in new[]
                     {
                         ("PathData", sign.PathData),
                         ("OverlayPathData", sign.OverlayPathData)
                     })
            {
                if (string.IsNullOrWhiteSpace(pathData))
                {
                    continue;
                }

                var bounds = Geometry.Parse(pathData).Bounds;

                if (bounds.Left < -Toleranz || bounds.Top < -Toleranz
                    || bounds.Right > 100 + Toleranz || bounds.Bottom > 100 + Toleranz)
                {
                    ausserhalb.Add($"{sign.Number} {sign.Name}: {label} liegt bei {bounds}.");
                }
            }
        }

        Assert.Empty(ausserhalb);
    }

    [WpfFact]
    public void Jedes_Zeichen_laesst_sich_zeichnen()
    {
        // Ein vollständiger Layout- und Render-Durchlauf über den ganzen Katalog: fängt
        // Fehler, die erst beim tatsächlichen Zeichnen auftreten (ungültige Farbe, Pen mit
        // Strichstärke 0 bei gesetzter Linie).
        foreach (var sign in TrafficSignCatalog.All)
        {
            var flaeche = new TrafficSignVisual { Sign = sign, Width = 120, Height = 120 };

            flaeche.Measure(new Size(120, 120));
            flaeche.Arrange(new Rect(0, 0, 120, 120));

            var bitmap = new System.Windows.Media.Imaging.RenderTargetBitmap(
                120, 120, 96, 96, PixelFormats.Pbgra32);
            bitmap.Render(flaeche);

            Assert.True(flaeche.IsMeasureValid, $"Zeichen {sign.Number} ({sign.Name}) konnte nicht gezeichnet werden.");
        }
    }
}
