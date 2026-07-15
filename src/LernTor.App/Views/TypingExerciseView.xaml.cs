using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using LernTor.App.ViewModels;

namespace LernTor.App.Views;

/// <summary>Code-behind für Tipp-Übung: Tastatur-Events, Fokus-Management.</summary>
public partial class TypingExerciseView : UserControl
{
    public TypingExerciseView()
    {
        InitializeComponent();
        Loaded += OnLoaded;
    }

    private void OnLoaded(object sender, RoutedEventArgs e)
    {
        // Initialen Zieltext rendern
        if (DataContext is TypingExerciseViewModel vm)
        {
            UpdateTargetTextDisplay(vm);
        }
        // Fokus auf Eingabefeld
        InputBox.Focus();
    }

    private void InputBox_PreviewKeyDown(object sender, KeyEventArgs e)
    {
        if (DataContext is not TypingExerciseViewModel vm || vm.IsCompleted)
        {
            e.Handled = true;
            return;
        }

        // Enter: Nur wenn Lektion abgeschlossen (Enter wird im TextBox nicht durchgereicht)
        if (e.Key == Key.Enter)
        {
            e.Handled = true;
            if (vm.IsCompleted)
            {
                // Weiter-Button im ResultOverlay ist sichtbar, Continue-Command wird durch Button getriggert
            }
            return;
        }

        // Backspace: Erlauben, aber nur wenn noch Zeichen da sind
        if (e.Key == Key.Back)
        {
            if (vm.CurrentInput.Length == 0)
            {
                e.Handled = true;
            }
            return;
        }

        // Andere Tasten: Normale Verarbeitung durch TextBox
    }

    private void InputBox_TextChanged(object sender, TextChangedEventArgs e)
    {
        if (DataContext is TypingExerciseViewModel vm)
        {
            UpdateTargetTextDisplay(vm);
            // Cursor-Position am Ende halten
            InputBox.CaretIndex = vm.CurrentInput.Length;
        }
    }

    private void UpdateTargetTextDisplay(TypingExerciseViewModel vm)
    {
        // Einfaches Rendering: TargetText in TextBlock, wir nutzen Inlines für Highlighting
        // Für WPF vereinfacht: Nutzen wir einen RichTextBox-ähnlichen Ansatz mit TextBlock Inlines
        TargetTextBlock.Inlines.Clear();

        var target = vm.Lesson.TargetText;
        var input = vm.CurrentInput;
        int pos = vm.CurrentPosition;

        for (int i = 0; i < target.Length; i++)
        {
            char c = target[i];
            var run = new Run(c.ToString())
            {
                FontFamily = new FontFamily("Consolas, Cascadia Code, monospace"),
                FontSize = 20,
                FontWeight = FontWeights.Normal
            };

            if (i < pos)
            {
                // Bereits getippt
                bool correct = i < vm.CurrentInput.Length && i < target.Length && vm.CurrentInput[i] == target[i];
                run.Foreground = correct ? new System.Windows.Media.SolidColorBrush((System.Windows.Media.Color)System.Windows.Media.ColorConverter.ConvertFromString("#4ADE80"))
                                          : new System.Windows.Media.SolidColorBrush((System.Windows.Media.Color)System.Windows.Media.ColorConverter.ConvertFromString("#FF6B6B"));
                run.FontWeight = FontWeights.Bold;
            }
            else if (i == pos)
            {
                // Aktuelle Position: hervorgehoben
                run.Background = new System.Windows.Media.SolidColorBrush((System.Windows.Media.Color)System.Windows.Media.ColorConverter.ConvertFromString("#FFD700"));
                run.FontWeight = FontWeights.Bold;
            }
            else
            {
                // Noch nicht getippt
                run.Foreground = new System.Windows.Media.SolidColorBrush((System.Windows.Media.Color)System.Windows.Media.ColorConverter.ConvertFromString("#888888"));
            }

            TargetTextBlock.Inlines.Add(run);
        }
    }
}