using System.IO;
using System.Windows;
using System.Windows.Controls;
using LernTor.App.Views;
using LernTor.Data;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace LernTor.UiTests;

/// <summary>
/// Lädt den echten Eltern-Bereich mit denselben Dienst-Registrierungen wie die App
/// (<see cref="LernTor.App.App.RegisterServices"/>), aber mit einer Wegwerf-Datenbank.
///
/// <para>Vorher lud kein Test dieses Fenster: <c>XamlLoadTests</c> nimmt nur Ansichten ohne
/// Parameter, und der Start-Test öffnet nur das Hauptfenster. Ein XAML-Laufzeitfehler im
/// Eltern-Bereich wäre also erst den Eltern aufgefallen - im größten Fenster der App.</para>
/// </summary>
public sealed class ParentSettingsWindowTests
{
    private static void EnsureAppResourcesLoaded()
    {
        if (Application.Current is null)
        {
            var app = new LernTor.App.App();
            app.InitializeComponent();
        }
    }

    [WpfFact]
    public void Eltern_Bereich_laedt_und_jede_Sprungmarke_hat_ihr_Ziel()
    {
        EnsureAppResourcesLoaded();

        var ordner = Path.Combine(Path.GetTempPath(), "lerntor-eltern-" + Guid.NewGuid().ToString("N"));
        Directory.CreateDirectory(ordner);
        var services = new ServiceCollection();
        // Das Logging stellt in der App der Host (Host.CreateDefaultBuilder) - Repositories wie
        // TrafficSignProgressRepository brauchen ILogger<T>.
        services.AddLogging();
        LernTor.App.App.RegisterServices(services, Path.Combine(ordner, "test.db"), Path.Combine(ordner, "sicherungen"));

        using (var provider = services.BuildServiceProvider())
        {
            provider.GetRequiredService<LernTorDbContext>().Database.EnsureCreated();

            // Der Konstruktor lädt das komplette XAML (InitializeComponent) - hier fiele ein
            // Laufzeitfehler auf.
            var fenster = provider.GetRequiredService<ParentSettingsWindow>();

            var sprungmarken = Assert.IsType<StackPanel>(fenster.FindName("Sprungmarken"));
            var ziele = sprungmarken.Children.OfType<Button>().Select(knopf => knopf.Tag as string).ToList();

            Assert.True(ziele.Count >= 10, $"nur {ziele.Count} Sprungmarken");
            foreach (var ziel in ziele)
            {
                Assert.False(string.IsNullOrEmpty(ziel), "Sprungmarke ohne Ziel");
                Assert.IsAssignableFrom<FrameworkElement>(fenster.FindName(ziel!));
            }

            // Die Ziele stehen in der Reihenfolge der Seite - sonst springt "weiter unten" nach oben.
            var inhalt = Assert.IsType<StackPanel>(fenster.FindName("EinstellungenInhalt"));
            var reihenfolge = ziele.Select(ziel => inhalt.Children.IndexOf((UIElement)fenster.FindName(ziel!)!)).ToList();
            Assert.Equal(reihenfolge.OrderBy(i => i), reihenfolge);

            // Bewusst KEIN fenster.Close(): das Fenster wurde nie gezeigt, und Schließen beendet
            // bei ShutdownMode.OnLastWindowClose die geteilte Test-Application - danach schlug
            // jeder weitere UI-Test mit "Cannot create more than one Application" fehl (CI
            // 30.09.2026, 30 rote Tests).
        }

        try
        {
            Directory.Delete(ordner, recursive: true);
        }
        catch (IOException)
        {
            // Microsoft.Data.Sqlite hält die Datei im Verbindungspool noch offen (CLAUDE.md);
            // den Temp-Ordner räumt das System auf.
        }
        catch (UnauthorizedAccessException)
        {
        }
    }
}
