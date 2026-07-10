using System.Collections.Specialized;
using System.Windows;
using System.Windows.Controls;
using LernTor.App.ViewModels;

namespace LernTor.App.Controls;

public partial class QuestionCard : UserControl
{
    public QuestionCard()
    {
        InitializeComponent();
        DataContextChanged += OnDataContextChanged;
    }

    /// <summary>
    /// QuestionCard wird über eine DataTemplate-Bindung wiederverwendet (z.B. "Nächste Aufgabe") -
    /// derselbe Visual-Tree bekommt dabei eine neue QuestionAnswerViewModel-Instanz mit eigener
    /// ChatMessages-Collection. Ohne Um-Hängen des Handlers würde nach der ersten Frage nur noch die
    /// (dann verwaiste) alte Collection beobachtet.
    /// </summary>
    private void OnDataContextChanged(object sender, DependencyPropertyChangedEventArgs e)
    {
        if (e.OldValue is QuestionAnswerViewModel oldViewModel)
        {
            oldViewModel.ChatMessages.CollectionChanged -= ChatMessages_CollectionChanged;
        }

        if (e.NewValue is QuestionAnswerViewModel newViewModel)
        {
            newViewModel.ChatMessages.CollectionChanged += ChatMessages_CollectionChanged;
        }
    }

    /// <summary>Scrollt den Chatverlauf automatisch zur neuesten Nachricht (Kind-Frage oder KI-Antwort).</summary>
    private void ChatMessages_CollectionChanged(object? sender, NotifyCollectionChangedEventArgs e)
    {
        Dispatcher.BeginInvoke(() => ChatScrollViewer.ScrollToEnd());
    }

    /// <summary>
    /// Fügt das angeklickte türkische Sonderzeichen an der aktuellen Cursor-Position im
    /// Antwortfeld ein, statt es nur ans Ende anzuhängen.
    /// </summary>
    private void TurkishCharacterButton_Click(object sender, RoutedEventArgs e)
    {
        if (sender is not Button { Tag: string character })
        {
            return;
        }

        var caretIndex = AnswerTextBox.SelectionStart;
        AnswerTextBox.Text = AnswerTextBox.Text.Insert(caretIndex, character);
        AnswerTextBox.SelectionStart = caretIndex + character.Length;
        AnswerTextBox.Focus();
    }

    /// <summary>Übernimmt das aktuelle Taschenrechner-Ergebnis an der Cursor-Position im Antwortfeld.</summary>
    private void Calculator_InsertRequested(object? sender, string result)
    {
        var caretIndex = AnswerTextBox.SelectionStart;
        AnswerTextBox.Text = AnswerTextBox.Text.Insert(caretIndex, result);
        AnswerTextBox.SelectionStart = caretIndex + result.Length;
        AnswerTextBox.Focus();
    }
}
