using System.Windows;
using System.Windows.Input;
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
    private async void ParentSettingsWindow_Closing(object? sender, System.ComponentModel.CancelEventArgs e)
    {
        if (_closeConfirmed || !_viewModel.HasUnsavedChanges)
        {
            return;
        }

        e.Cancel = true;

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
                // SaveAsync loest selbst RequestClose aus und setzt dabei _closeConfirmed.
                await _viewModel.SaveCommand.ExecuteAsync(null);
                break;
            case MessageBoxResult.No:
                _closeConfirmed = true;
                Close();
                break;
            // Abbrechen: Fenster bleibt offen, e.Cancel bleibt true.
        }
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
