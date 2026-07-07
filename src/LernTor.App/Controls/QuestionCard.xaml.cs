using System.Windows;
using System.Windows.Controls;

namespace LernTor.App.Controls;

public partial class QuestionCard : UserControl
{
    public QuestionCard()
    {
        InitializeComponent();
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
}
