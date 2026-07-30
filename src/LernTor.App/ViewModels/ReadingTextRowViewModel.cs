using CommunityToolkit.Mvvm.ComponentModel;
using LernTor.Core.Models;

namespace LernTor.App.ViewModels;

/// <summary>
/// Eine Zeile der Lesetext-Verwaltung im Eltern-Bereich. Bildet eingebaute und eigene Texte
/// einheitlich ab, damit die Eltern EINE Liste sehen statt zweier getrennter Bereiche - vorher
/// waren die 63 eingebauten Stücke im Eltern-Bereich überhaupt nicht sichtbar, und ein frisch
/// eingetragener eigener Text ließ sich nicht gezielt aufrufen.
/// </summary>
public sealed partial class ReadingTextRowViewModel : ObservableObject
{
    private readonly Action<ReadingTextRowViewModel> _onVisibilityChanged;

    public ReadingTextRowViewModel(
        ReadingPiece piece,
        bool isHidden,
        bool isPinned,
        Action<ReadingTextRowViewModel> onVisibilityChanged)
    {
        Piece = piece;
        _onVisibilityChanged = onVisibilityChanged;
        isVisible = !isHidden;
        this.isPinned = isPinned;
    }

    public ReadingPiece Piece { get; }

    public string Key => Piece.Key;

    public string Title => Piece.Title;

    public string Author => string.IsNullOrWhiteSpace(Piece.Author) ? "—" : Piece.Author;

    public bool IsCustom => Piece.IsCustom;

    /// <summary>Herkunft als Klartext-Chip - Eltern müssen sehen, was sie bearbeiten dürfen.</summary>
    public string OriginLabel => Piece.IsCustom ? "Eigener Text" : "Eingebaut";

    /// <summary>In welchen Sprachen der Text vorliegt (eigene Texte haben oft nur eine).</summary>
    public string LanguagesLabel
    {
        get
        {
            var languages = new List<string>();
            if (Piece.HasText("De")) languages.Add("DE");
            if (Piece.HasText("Tr")) languages.Add("TR");
            if (Piece.HasText("En")) languages.Add("EN");
            return languages.Count == 0 ? "—" : string.Join(" · ", languages);
        }
    }

    /// <summary>Erste Zeile des deutschen Textes als Vorschau, damit man Titel zuordnen kann.</summary>
    public string Preview
    {
        get
        {
            var text = Piece.TextDe;
            if (string.IsNullOrWhiteSpace(text)) text = Piece.TextTr;
            if (string.IsNullOrWhiteSpace(text)) text = Piece.TextEn;
            if (string.IsNullOrWhiteSpace(text)) return string.Empty;

            var oneLine = text.Replace('\n', ' ').Replace('\r', ' ').Trim();
            return oneLine.Length <= 90 ? oneLine : oneLine[..90] + "…";
        }
    }

    /// <summary>Sichtbar = darf im Vorlese-Bereich vorkommen.</summary>
    [ObservableProperty]
    private bool isVisible;

    /// <summary>Angeheftet = steht jeden Tag als erster Text da, bis es wieder gelöst wird.</summary>
    [ObservableProperty]
    private bool isPinned;

    partial void OnIsVisibleChanged(bool value) => _onVisibilityChanged(this);
}
