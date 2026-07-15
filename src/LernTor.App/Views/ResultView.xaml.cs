using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using LernTor.App.ViewModels;

namespace LernTor.App.Views;

public partial class ResultView : UserControl
{
    /// <summary>Konfetti-Farben passend zur App-Palette (Colors.xaml).</summary>
    private static readonly Color[] ConfettiColors =
    {
        Color.FromRgb(0x6C, 0x5C, 0xE7), // Primary (Lila)
        Color.FromRgb(0xFD, 0xCB, 0x6E), // Accent (Gold)
        Color.FromRgb(0x00, 0xB8, 0x94), // Success (Grün)
        Color.FromRgb(0xE8, 0x43, 0x93), // Turkish (Pink)
        Color.FromRgb(0x09, 0x84, 0xE3)  // Math (Blau)
    };

    public ResultView()
    {
        InitializeComponent();
    }

    private void ResultView_Loaded(object sender, RoutedEventArgs e)
    {
        if (DataContext is ResultViewModel { Passed: true })
        {
            SpawnConfetti();
        }
    }

    /// <summary>
    /// Einmaliger Konfettiregen beim bestandenen Abschluss: ~70 kleine Rechtecke fallen mit
    /// zufälliger Farbe/Größe/Drehung von oben durch, jedes entfernt sich nach seiner Animation
    /// selbst aus dem Canvas - keine Dauerschleife, keine bleibende CPU-Last im Kiosk-Betrieb.
    /// </summary>
    private void SpawnConfetti()
    {
        var random = new Random();
        var width = ActualWidth > 0 ? ActualWidth : 1200;
        var height = ActualHeight > 0 ? ActualHeight : 800;

        for (var i = 0; i < 70; i++)
        {
            var size = random.Next(8, 16);
            var color = ConfettiColors[random.Next(ConfettiColors.Length)];
            var rotate = new RotateTransform(random.Next(360));
            var piece = new Rectangle
            {
                Width = size,
                Height = size * 0.6,
                RadiusX = 2,
                RadiusY = 2,
                Fill = new SolidColorBrush(color),
                RenderTransform = rotate,
                RenderTransformOrigin = new Point(0.5, 0.5)
            };

            Canvas.SetLeft(piece, random.NextDouble() * width);
            Canvas.SetTop(piece, -30);
            ConfettiCanvas.Children.Add(piece);

            var fall = new DoubleAnimation
            {
                From = -30,
                To = height + 40,
                Duration = TimeSpan.FromSeconds(2.2 + random.NextDouble() * 1.8),
                BeginTime = TimeSpan.FromMilliseconds(random.Next(0, 900)),
                EasingFunction = new QuadraticEase { EasingMode = EasingMode.EaseIn }
            };
            var capturedPiece = piece;
            fall.Completed += (_, _) => ConfettiCanvas.Children.Remove(capturedPiece);
            piece.BeginAnimation(Canvas.TopProperty, fall);

            var spin = new DoubleAnimation
            {
                By = random.Next(2) == 0 ? 540 : -540,
                Duration = TimeSpan.FromSeconds(3),
                BeginTime = fall.BeginTime
            };
            rotate.BeginAnimation(RotateTransform.AngleProperty, spin);
        }
    }
}
