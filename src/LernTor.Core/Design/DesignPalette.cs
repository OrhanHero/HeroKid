namespace LernTor.Core.Design;

/// <summary>
/// Die Farben eines Designs, als Rollen statt als Farbnamen (docs/DESIGN.md). Jede Eigenschaft
/// ist ein Farbwert <c>#RRGGBB</c>. Die App legt daraus Pinsel mit festen Schlüsseln an
/// (<c>PrimaryBrush</c>, <c>CardBrush</c> …) - die Ansichten kennen nur diese Schlüssel.
///
/// <para>Alle Eigenschaften sind <c>required</c>: ein neues Design, das eine Rolle vergisst, ist
/// ein Compilerfehler statt einer Stelle, die im neuen Design plötzlich die Farbe des alten
/// behält. Welche Paare wie viel Kontrast brauchen, steht in <see cref="DesignContrastRules"/>.</para>
/// </summary>
public sealed record DesignPalette
{
    /// <summary>Fensterhintergrund.</summary>
    public required string Background { get; init; }

    /// <summary>Karten und Flächen auf dem Hintergrund (Schlüssel <c>CardBrush</c>).</summary>
    public required string Surface { get; init; }

    /// <summary>Normaler Text.</summary>
    public required string TextPrimary { get; init; }

    /// <summary>Gedämpfter Text: Untertitel, Hinweise.</summary>
    public required string TextSecondary { get; init; }

    /// <summary>Hauptfarbe: Knöpfe, Auswahl, Überschriften in Farbe.</summary>
    public required string Primary { get; init; }

    /// <summary>Hauptfarbe beim Darüberfahren.</summary>
    public required string PrimaryDark { get; init; }

    /// <summary>Schrift auf farbigen Flächen (Hauptfarbe, Erfolg, Fehler) - hell in hellen,
    /// dunkel in dunklen Designs.</summary>
    public required string OnColor { get; init; }

    /// <summary>Hervorhebungsfläche (Tipp-Kasten, Sprach-Etiketten) - trägt normalen Text.</summary>
    public required string Accent { get; init; }

    /// <summary>Tastatur-Fokusring und besondere Umrandungen.</summary>
    public required string Focus { get; init; }

    /// <summary>Richtig, geschafft - als Schrift und als Fläche.</summary>
    public required string Success { get; init; }

    /// <summary>Falsch, Warnung - als Schrift und als Fläche.</summary>
    public required string Error { get; init; }

    /// <summary>Kachelflächen (Profilwahl, Stufen) - tragen normalen Text.</summary>
    public required string TileLavender { get; init; }

    public required string TileSand { get; init; }

    public required string TileMint { get; init; }

    public required string TileRose { get; init; }

    /// <summary>Hintergrund von Fortschrittsbalken und -ringen.</summary>
    public required string ProgressTrack { get; init; }

    /// <summary>Rahmen von Eingabefeldern ohne Fokus.</summary>
    public required string InputBorder { get; init; }

    /// <summary>Fächerfarben (Überschrift der Übung, Fortschrittsansicht).</summary>
    public required string Math { get; init; }

    public required string German { get; init; }

    public required string Turkish { get; init; }

    public required string Science { get; init; }

    public required string News { get; init; }

    /// <summary>Alle Rollen mit Namen - für Kontrasttest, Ressourcen und Vorschau.</summary>
    public IReadOnlyDictionary<string, string> Roles => new Dictionary<string, string>
    {
        [nameof(Background)] = Background,
        [nameof(Surface)] = Surface,
        [nameof(TextPrimary)] = TextPrimary,
        [nameof(TextSecondary)] = TextSecondary,
        [nameof(Primary)] = Primary,
        [nameof(PrimaryDark)] = PrimaryDark,
        [nameof(OnColor)] = OnColor,
        [nameof(Accent)] = Accent,
        [nameof(Focus)] = Focus,
        [nameof(Success)] = Success,
        [nameof(Error)] = Error,
        [nameof(TileLavender)] = TileLavender,
        [nameof(TileSand)] = TileSand,
        [nameof(TileMint)] = TileMint,
        [nameof(TileRose)] = TileRose,
        [nameof(ProgressTrack)] = ProgressTrack,
        [nameof(InputBorder)] = InputBorder,
        [nameof(Math)] = Math,
        [nameof(German)] = German,
        [nameof(Turkish)] = Turkish,
        [nameof(Science)] = Science,
        [nameof(News)] = News,
    };
}
