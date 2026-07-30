using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Threading;
using LernTor.App.ViewModels;

namespace LernTor.App.Views;

public partial class ParentSettingsWindow : Window
{
    private readonly ParentSettingsViewModel _viewModel;

    public ParentSettingsWindow(ParentSettingsViewModel viewModel)
    {
        InitializeComponent();

        _viewModel = viewModel;
        DataContext = _viewModel;
        _viewModel.RequestClose += () => Dispatcher.Invoke(() =>
        {
            // Ueber "Speichern"/"Schliessen" im Fenster: die Rueckfrage ist dann schon erledigt
            // bzw. bewusst uebersprungen.
            _closeConfirmed = true;
            Close();
        });

        Loaded += async (_, _) => await _viewModel.InitializeAsync();
        Closing += ParentSettingsWindow_Closing;
    }

    /// <summary>Verhindert eine zweite Rueckfrage, wenn der Schliessvorgang schon geklaert ist.</summary>
    private bool _closeConfirmed;

    /// <summary>
    /// Rueckfrage beim Schliessen ueber das X, wenn ungespeicherte Aenderungen offen sind.
    /// Ohne sie gingen Presets, Zeiten und eigene Tipp-Texte stillschweigend verloren - im
    /// Familienbetrieb ist genau das mehrfach passiert.
    /// </summary>
    private void ParentSettingsWindow_Closing(object? sender, System.ComponentModel.CancelEventArgs e)
    {
        if (_closeConfirmed || !_viewModel.HasUnsavedChanges)
        {
            return;
        }

        var answer = MessageBox.Show(
            this,
            "Es gibt ungespeicherte Änderungen.\n\nJa = speichern und schließen\nNein = ohne Speichern schließen\nAbbrechen = im Eltern-Bereich bleiben",
            "Änderungen speichern?",
            MessageBoxButton.YesNoCancel,
            MessageBoxImage.Question,
            MessageBoxResult.Yes);

        switch (answer)
        {
            case MessageBoxResult.Yes:
                // Speichern ist asynchron, das Fenster darf also erst danach zugehen. Der laufende
                // Schliessvorgang wird abgebrochen und spaeter neu ausgeloest - Close() DARF hier
                // nicht direkt aufgerufen werden, WPF wirft dann eine InvalidOperationException
                // ("... while a Window is closing").
                e.Cancel = true;
                _ = SaveThenCloseAsync();
                break;

            case MessageBoxResult.Cancel:
                e.Cancel = true;
                break;

            // Nein: nichts abbrechen, der laufende Schliessvorgang laeuft einfach durch. Auch hier
            // bewusst kein Close() - der Vorgang ist ja schon unterwegs.
            default:
                _closeConfirmed = true;
                break;
        }
    }

    /// <summary>
    /// Speichert und schließt danach. Läuft bewusst erst NACH dem abgebrochenen Schließvorgang an
    /// (<see cref="Dispatcher"/>-Durchlauf abwarten), weil <see cref="Window.Close"/> währenddessen
    /// nicht erlaubt ist. Das Schließen selbst übernimmt der RequestClose-Handler oben, den
    /// <c>SaveAsync</c> am Ende auslöst.
    /// </summary>
    private async Task SaveThenCloseAsync()
    {
        // Leerer Callback mit niedriger Priorität = "warte, bis WPF mit dem Schließvorgang fertig
        // ist". Bewusst InvokeAsync (Instanzmethode) statt des statischen Dispatcher.Yield, damit
        // die Auflösung nicht an der Color-Color-Regel hängt.
        await Dispatcher.InvokeAsync(() => { }, DispatcherPriority.Background);
        await _viewModel.SaveCommand.ExecuteAsync(null);
    }

    private async void LoginButton_Click(object sender, RoutedEventArgs e)
    {
        await _viewModel.LoginCommand.ExecuteAsync(PasswordInput.Password);
        PasswordInput.Clear();
    }

    private void UnlockAndExitButton_Click(object sender, RoutedEventArgs e)
    {
        _viewModel.UnlockAndExitCommand.Execute(PasswordInput.Password);
        PasswordInput.Clear();
    }

    /// <summary>PasswordBox.Password kann aus Sicherheitsgründen nicht gebunden werden, daher lässt
    /// sich Enter hier nicht per KeyBinding/Command lösen wie bei normalen TextBoxen - stattdessen
    /// wird direkt der bestehende Login-Klick-Handler wiederverwendet.</summary>
    private async void PasswordInput_KeyDown(object sender, KeyEventArgs e)
    {
        if (e.Key == Key.Return)
        {
            await _viewModel.LoginCommand.ExecuteAsync(PasswordInput.Password);
            PasswordInput.Clear();
        }
    }
}
