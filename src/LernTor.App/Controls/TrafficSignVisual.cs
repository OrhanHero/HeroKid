using System.Globalization;
using System.Windows;
using System.Windows.Media;
using LernTor.Core.Enums;
using LernTor.Core.Models;

namespace LernTor.App.Controls;

/// <summary>
/// Zeichnet ein Verkehrszeichen.
///
/// <para><b>Warum direkt in <see cref="OnRender"/> statt als XAML-Template:</b> ein Zeichen ist
/// eine Handvoll Geometrien, deren Farben und Formen von den Daten abhängen. Als XAML bräuchte
/// das ein Dutzend Trigger und Converter je Grundform - und würde damit genau in die
/// Fehlerklasse laufen, die dieses Projekt schon zweimal getroffen hat: XAML, das sauber
/// kompiliert und erst beim Anzeigen wirft. Hier ist alles gewöhnlicher C#-Code, den der
/// Compiler prüft.</para>
///
/// <para>Alle Geometrien rechnen in einem Feld von 0..100 und werden auf das kleinere Maß des
/// verfügbaren Platzes skaliert - dasselbe Zeichen funktioniert damit als 60-Pixel-Kachel in der
/// Übersicht und als 300-Pixel-Bild im Quiz.</para>
/// </summary>
public sealed class TrafficSignVisual : FrameworkElement
{
    /// <summary>Kantenlänge des gedachten Zeichenfelds.</summary>
    private const double Feld = 100.0;

    /// <summary>Dünne neutrale Außenlinie. Ohne sie wären Zeichen mit weißem Rand (VZ 306
    /// Vorfahrtstraße) auf dem hellen Hintergrund der App unsichtbar.</summary>
    private static readonly Pen Aussenlinie = CreateFrozenPen(Color.FromRgb(0x9A, 0x9A, 0x9A), 0.8);

    private static readonly Typeface Schrift =
        new(new FontFamily("Segoe UI"), FontStyles.Normal, FontWeights.Bold, FontStretches.Normal);

    public static readonly DependencyProperty SignProperty = DependencyProperty.Register(
        nameof(Sign),
        typeof(TrafficSign),
        typeof(TrafficSignVisual),
        new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.AffectsRender));

    public TrafficSign? Sign
    {
        get => (TrafficSign?)GetValue(SignProperty);
        set => SetValue(SignProperty, value);
    }

    protected override void OnRender(DrawingContext dc)
    {
        var sign = Sign;
        if (sign is null)
        {
            return;
        }

        var seite = Math.Min(ActualWidth, ActualHeight);
        if (seite <= 0)
        {
            return;
        }

        // Quadratisch und mittig - das Zeichen soll bei jedem Seitenverhältnis rund bleiben.
        var links = (ActualWidth - seite) / 2;
        var oben = (ActualHeight - seite) / 2;
        var skalierung = seite / Feld;

        dc.PushTransform(new TranslateTransform(links, oben));
        dc.PushTransform(new ScaleTransform(skalierung, skalierung));

        try
        {
            DrawShape(dc, sign);
            DrawPath(dc, sign.PathData, sign.PathColor, sign.PathStrokeThickness);
            DrawPath(dc, sign.OverlayPathData, sign.OverlayColor, sign.OverlayStrokeThickness);
            DrawText(dc, sign);
        }
        finally
        {
            dc.Pop();
            dc.Pop();
        }
    }

    /// <summary>
    /// Grundform: außen in der Randfarbe, darüber eine verkleinerte Kopie in der Füllfarbe. Der
    /// sichtbare Rand ist also der Rest der äußeren Form - das funktioniert für Kreis, Dreieck,
    /// Achteck und Raute gleichermaßen, ohne für jede Form eine eigene Innenkontur zu rechnen.
    /// </summary>
    private static void DrawShape(DrawingContext dc, TrafficSign sign)
    {
        var aussen = ShapeGeometry(sign.Shape, 1.0);

        if (!IstTransparent(sign.BorderColor))
        {
            dc.DrawGeometry(BrushFor(sign.BorderColor), Aussenlinie, aussen);
        }

        if (IstTransparent(sign.FillColor))
        {
            return;
        }

        var innen = ShapeGeometry(sign.Shape, InnenAnteil(sign.Shape));
        dc.DrawGeometry(BrushFor(sign.FillColor), null, innen);
    }

    /// <summary>
    /// Wie groß die innere Fläche im Verhältnis zur äußeren ist - der Rest ist der Rand. Die
    /// Werte sind an den amtlichen Zeichen abgelesen: das Dreieck der Gefahrzeichen hat einen
    /// auffällig breiteren Rand als ein Richtzeichen-Rechteck.
    /// </summary>
    private static double InnenAnteil(SignShape shape) => shape switch
    {
        SignShape.DreieckSpitzeOben => 0.72,
        SignShape.DreieckSpitzeUnten => 0.72,
        SignShape.Kreis => 0.80,
        SignShape.Achteck => 0.86,
        SignShape.Raute => 0.84,
        _ => 0.92
    };

    /// <summary>
    /// Die Grundform im 0..100-Feld, um den Mittelpunkt herum auf <paramref name="anteil"/>
    /// verkleinert.
    /// </summary>
    private static Geometry ShapeGeometry(SignShape shape, double anteil)
    {
        Geometry geometry = shape switch
        {
            SignShape.Kreis => new EllipseGeometry(new Point(50, 50), 48, 48),
            SignShape.DreieckSpitzeOben => Polygon(new Point(50, 5), new Point(96, 85), new Point(4, 85)),
            SignShape.DreieckSpitzeUnten => Polygon(new Point(4, 15), new Point(96, 15), new Point(50, 95)),
            SignShape.Raute => Polygon(new Point(50, 3), new Point(97, 50), new Point(50, 97), new Point(3, 50)),
            SignShape.Achteck => Octagon(),
            SignShape.Rechteck => new RectangleGeometry(new Rect(2, 22, 96, 56), 3, 3),
            _ => new RectangleGeometry(new Rect(4, 4, 92, 92), 4, 4)
        };

        if (Math.Abs(anteil - 1.0) > 0.0001)
        {
            geometry = geometry.Clone();
            geometry.Transform = new ScaleTransform(anteil, anteil, 50, 50);
        }

        geometry.Freeze();
        return geometry;
    }

    private static Geometry Polygon(params Point[] points)
    {
        var figure = new PathFigure { StartPoint = points[0], IsClosed = true, IsFilled = true };
        for (var i = 1; i < points.Length; i++)
        {
            figure.Segments.Add(new LineSegment(points[i], true));
        }

        return new PathGeometry(new[] { figure });
    }

    private static Geometry Octagon()
    {
        var punkte = new Point[8];
        for (var i = 0; i < 8; i++)
        {
            // Um 22,5° gedreht, damit oben und unten eine waagerechte Kante liegt - so sieht
            // ein Stoppschild aus, nicht wie ein auf der Spitze stehendes Achteck.
            var winkel = (Math.PI / 4 * i) + (Math.PI / 8);
            punkte[i] = new Point(50 + (48 * Math.Cos(winkel)), 50 + (48 * Math.Sin(winkel)));
        }

        return Polygon(punkte);
    }

    private static void DrawPath(DrawingContext dc, string? pathData, string color, double strokeThickness)
    {
        if (string.IsNullOrWhiteSpace(pathData) || IstTransparent(color))
        {
            return;
        }

        Geometry geometry;
        try
        {
            geometry = Geometry.Parse(pathData);
        }
        catch (FormatException)
        {
            // Ein kaputter Pfad darf nicht die ganze Ansicht mitreißen - lieber ein Zeichen
            // ohne Piktogramm als ein Quiz, das beim Anzeigen abstürzt. Der Katalogtest
            // (TrafficSignCatalogTests) fängt so etwas ohnehin vor dem Ausliefern ab.
            return;
        }

        geometry.Freeze();

        if (strokeThickness > 0)
        {
            var pen = new Pen(BrushFor(color), strokeThickness)
            {
                StartLineCap = PenLineCap.Round,
                EndLineCap = PenLineCap.Round,
                LineJoin = PenLineJoin.Round
            };
            pen.Freeze();
            dc.DrawGeometry(null, pen, geometry);
            return;
        }

        dc.DrawGeometry(BrushFor(color), null, geometry);
    }

    /// <summary>
    /// Aufschrift, auf die Breite des Zeichens eingepasst. "50" und "Anlieger frei" müssen beide
    /// hineinpassen, ohne dass im Katalog je eine Schriftgröße steht.
    /// </summary>
    private static void DrawText(DrawingContext dc, TrafficSign sign)
    {
        if (string.IsNullOrWhiteSpace(sign.Text))
        {
            return;
        }

        var text = new FormattedText(
            sign.Text,
            CultureInfo.GetCultureInfo("de-DE"),
            FlowDirection.LeftToRight,
            Schrift,
            40,
            BrushFor(sign.TextColor),
            1.0);

        var verfuegbar = TextBreite(sign.Shape);
        var faktor = Math.Min(1.0, verfuegbar / Math.Max(text.Width, 0.01));

        text.SetFontSize(40 * faktor);

        // Beim Dreieck sitzt der Text tiefer: oben läuft die Form spitz zu.
        var mitteY = sign.Shape == SignShape.DreieckSpitzeOben ? 58.0 : 50.0;

        dc.DrawText(text, new Point(50 - (text.Width / 2), mitteY - (text.Height / 2)));
    }

    private static double TextBreite(SignShape shape) => shape switch
    {
        SignShape.DreieckSpitzeOben => 46,
        SignShape.DreieckSpitzeUnten => 52,
        SignShape.Kreis => 62,
        SignShape.Achteck => 66,
        SignShape.Raute => 56,
        SignShape.Rechteck => 88,
        _ => 82
    };

    private static bool IstTransparent(string color) =>
        string.IsNullOrWhiteSpace(color) || string.Equals(color, SignColors.Transparent, StringComparison.OrdinalIgnoreCase);

    private static Brush BrushFor(string color)
    {
        var brush = new SolidColorBrush((Color)ColorConverter.ConvertFromString(color));
        brush.Freeze();
        return brush;
    }

    private static Pen CreateFrozenPen(Color color, double thickness)
    {
        var brush = new SolidColorBrush(color);
        brush.Freeze();
        var pen = new Pen(brush, thickness);
        pen.Freeze();
        return pen;
    }
}
