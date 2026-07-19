using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Threading;
using LernTor.App.ViewModels;
using LernTor.Security;
using Microsoft.Extensions.DependencyInjection;

namespace LernTor.App.Views;

public partial class MainWindow : Window
{
    [DllImport("user32.dll")]
    private static extern nint GetForegroundWindow();

    [DllImport("user32.dll")]
    private static extern uint GetWindowThreadProcessId(nint hWnd, out uint lpdwProcessId);

    private readonly KioskLockService _kioskLockService;
    private readonly DispatcherTimer _foregroundWatchdog;

    public MainWindow(MainViewModel viewModel, KioskLockService kioskLockService)
    {
        InitializeComponent();
        DataContext = viewModel;
        _kioskLockService = kioskLockService;

        // Zweite, unabhängige Verteidigungslinie neben KioskKeyboardHook (WH_KEYBOARD_LL):
        // Kinder kommen z.B. über Alt+Tab (oder andere Wege, die der Keyboard-Hook auf
        // einzelnen Rechnern nicht abfängt - Gruppenrichtlinie/Timing/Edge-Cases) aus dem
        // Vollbild heraus. Statt jede denkbare Tastenkombination einzeln zu jagen, holt dieser
        // Watchdog das Fenster alle 300ms zurück in den Vordergrund, sobald der Fokus bei
        // einem FREMDEN Prozess liegt - der Eltern-Bereich (selbe Prozess-ID, eigenes Fenster)
        // bleibt davon unberührt, sonst könnten Eltern ihr eigenes Fenster nicht bedienen.
        _foregroundWatchdog = new DispatcherTimer(DispatcherPriority.Background)
        {
            Interval = TimeSpan.FromMilliseconds(300)
        };
        _foregroundWatchdog.Tick += (_, _) => ReclaimForegroundIfEscaped();
        _foregroundWatchdog.Start();

        Closed += (_, _) => _foregroundWatchdog.Stop();
        Closing += MainWindow_Closing;
    }

    /// <summary>
    /// Verhindert das eigentliche Schließen, solange der Kiosk gesperrt ist - unabhängig davon,
    /// WIE der Schließen-Befehl ausgelöst wurde. Realer Bug: Windows 11 zeigt im Alt+Tab-
    /// Umschalter ein "X" direkt auf jeder Fenster-Kachel, mit dem sich ein Fenster schließen
    /// lässt, OHNE dass zuvor überhaupt zu ihm gewechselt wird - der Keyboard-Hook (der nur
    /// Tastendrücke abfängt) und der Foreground-Watchdog (der nur den Fokus zurückholt) greifen
    /// hier beide nicht, weil kein Tastendruck und kein Fokuswechsel involviert ist, sondern ein
    /// direkter WM_CLOSE. Legitime Beendigungen (Freischaltung nach bestandenem Quiz, Eltern-
    /// Bereich-Sofortentsperrung, Werkseinstellungen, Sicherung wiederherstellen) rufen VOR
    /// Application.Shutdown() immer erst KioskLockService.Unlock() auf - IsLocked ist dann schon
    /// false und diese Fälle werden hier nicht blockiert.
    /// </summary>
    private void MainWindow_Closing(object? sender, System.ComponentModel.CancelEventArgs e)
    {
        if (!_kioskLockService.IsLocked)
        {
            return;
        }

        e.Cancel = true;
        Topmost = false;
        Topmost = true;
        Activate();
    }

    private void ReclaimForegroundIfEscaped()
    {
        if (!_kioskLockService.IsLocked)
        {
            return;
        }

        var foreground = GetForegroundWindow();
        if (foreground == nint.Zero)
        {
            return;
        }

        GetWindowThreadProcessId(foreground, out var foregroundProcessId);
        if (foregroundProcessId == (uint)Environment.ProcessId)
        {
            // Fokus liegt bereits bei uns oder einem eigenen Fenster (z.B. Eltern-Bereich) - nichts zu tun.
            return;
        }

        // Topmost aus/an erzwingt einen frischen Z-Order-Wechsel (reines Activate() reicht bei
        // manchen fremden Vordergrund-Fenstern - z.B. dem Alt+Tab-Task-Switcher - nicht aus).
        Topmost = false;
        Topmost = true;
        Activate();
    }

    private void ParentAccessButton_Click(object sender, RoutedEventArgs e)
    {
        var window = ((App)Application.Current).Services.GetRequiredService<ParentSettingsWindow>();
        window.Owner = this;

        if (window.DataContext is ParentSettingsViewModel parentViewModel && DataContext is MainViewModel mainViewModel)
        {
            parentViewModel.PreselectProfileId = mainViewModel.CurrentProfile?.Id;
        }

        window.ShowDialog();
    }
}
