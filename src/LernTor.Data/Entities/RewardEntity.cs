namespace LernTor.Data.Entities;

/// <summary>
/// Eine von den Eltern angelegte Belohnung, die Kinder mit gesammelten Sternen einlösen können
/// (z.B. "30 Minuten extra Spielzeit" für 20 ⭐). Global für alle Profile - die Sterne-Kosten
/// zahlt jedes Kind aus dem eigenen Sterne-Konto. Gibt den Sternen einen echten Gegenwert;
/// bewusst weiterhin ohne Streaks/Verfall (Sterne können nur wachsen oder eingelöst werden).
/// </summary>
public sealed class RewardEntity
{
    public int Id { get; set; }
    public string Emoji { get; set; } = "🎁";
    public string Title { get; set; } = string.Empty;
    public int StarCost { get; set; }
    public DateTimeOffset CreatedAt { get; set; }
}

/// <summary>
/// Eine eingelöste Belohnung (pro Profil) - als Schnappschuss von Titel/Kosten, damit die
/// Historie auch nach dem Löschen oder Ändern der Belohnung korrekt bleibt. Die Eltern sehen
/// die Einlösungen im Eltern-Bereich und lösen sie in der echten Welt ein.
/// </summary>
public sealed class RewardRedemptionEntity
{
    public int Id { get; set; }
    public string ProfileId { get; set; } = string.Empty;
    public string RewardTitle { get; set; } = string.Empty;
    public string RewardEmoji { get; set; } = string.Empty;
    public int StarCost { get; set; }
    public DateTimeOffset RedeemedAt { get; set; }
}
