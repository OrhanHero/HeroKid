using System.Windows;
using System.Windows.Media;

namespace LernTor.App.Controls;

/// <summary>
/// Leichtgewichtiger Fortschrittsring (Kreisbogen) für die Profil-Kacheln: zeichnet direkt in
/// <see cref="OnRender"/> statt über Path/ArcSegment-Bindings - kein Template, kein Layout-Overhead,
/// gut geeignet für mehrere Instanzen auf einem Auswahl-Screen. Fraction 0..1; bei 0 wird nur der
/// dezente Hintergrundring gezeichnet, bei 1 ein voller Kreis.
/// </summary>
public sealed class ProgressArc : FrameworkElement
{
    public static readonly DependencyProperty FractionProperty = DependencyProperty.Register(
        nameof(Fraction), typeof(double), typeof(ProgressArc),
        new FrameworkPropertyMetadata(0d, FrameworkPropertyMetadataOptions.AffectsRender));

    public static readonly DependencyProperty StrokeProperty = DependencyProperty.Register(
        nameof(Stroke), typeof(Brush), typeof(ProgressArc),
        new FrameworkPropertyMetadata(Brushes.MediumPurple, FrameworkPropertyMetadataOptions.AffectsRender));

    public static readonly DependencyProperty TrackBrushProperty = DependencyProperty.Register(
        nameof(TrackBrush), typeof(Brush), typeof(ProgressArc),
        new FrameworkPropertyMetadata(Brushes.Gainsboro, FrameworkPropertyMetadataOptions.AffectsRender));

    public static readonly DependencyProperty StrokeThicknessProperty = DependencyProperty.Register(
        nameof(StrokeThickness), typeof(double), typeof(ProgressArc),
        new FrameworkPropertyMetadata(6d, FrameworkPropertyMetadataOptions.AffectsRender));

    public double Fraction
    {
        get => (double)GetValue(FractionProperty);
        set => SetValue(FractionProperty, value);
    }

    public Brush Stroke
    {
        get => (Brush)GetValue(StrokeProperty);
        set => SetValue(StrokeProperty, value);
    }

    public Brush TrackBrush
    {
        get => (Brush)GetValue(TrackBrushProperty);
        set => SetValue(TrackBrushProperty, value);
    }

    public double StrokeThickness
    {
        get => (double)GetValue(StrokeThicknessProperty);
        set => SetValue(StrokeThicknessProperty, value);
    }

    protected override void OnRender(DrawingContext drawingContext)
    {
        var thickness = StrokeThickness;
        var radius = (Math.Min(ActualWidth, ActualHeight) - thickness) / 2;
        if (radius <= 0)
        {
            return;
        }

        var center = new Point(ActualWidth / 2, ActualHeight / 2);

        // Dezenter Hintergrundring, damit auch "0% geschafft" als Ring erkennbar bleibt.
        drawingContext.DrawEllipse(null, new Pen(TrackBrush, thickness), center, radius, radius);

        var fraction = Math.Clamp(Fraction, 0d, 1d);
        if (fraction <= 0)
        {
            return;
        }

        var pen = new Pen(Stroke, thickness) { StartLineCap = PenLineCap.Round, EndLineCap = PenLineCap.Round };

        if (fraction >= 1)
        {
            drawingContext.DrawEllipse(null, pen, center, radius, radius);
            return;
        }

        // Bogen startet oben (12 Uhr) und läuft im Uhrzeigersinn.
        var startAngle = -Math.PI / 2;
        var endAngle = startAngle + fraction * 2 * Math.PI;
        var startPoint = new Point(center.X + radius * Math.Cos(startAngle), center.Y + radius * Math.Sin(startAngle));
        var endPoint = new Point(center.X + radius * Math.Cos(endAngle), center.Y + radius * Math.Sin(endAngle));

        var geometry = new StreamGeometry();
        using (var context = geometry.Open())
        {
            context.BeginFigure(startPoint, isFilled: false, isClosed: false);
            context.ArcTo(endPoint, new Size(radius, radius), 0,
                isLargeArc: fraction > 0.5, SweepDirection.Clockwise, isStroked: true, isSmoothJoin: false);
        }

        geometry.Freeze();
        drawingContext.DrawGeometry(null, pen, geometry);
    }
}
