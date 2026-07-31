namespace LernTor.Core.Services;

/// <summary>Warum eine automatische Sicherung gezogen wurde.</summary>
public enum AutoBackupReason
{
    /// <summary>Einmal am Tag beim ersten Start.</summary>
    Taeglich,

    /// <summary>Das Datenbankschema weicht vom letzten bekannten Stand ab - die wichtigste
    /// Sicherung überhaupt, sie entsteht VOR dem Schema-Abgleich.</summary>
    Schemaaenderung
}

/// <summary>Eine bereits vorhandene automatische Sicherung.</summary>
public readonly record struct AutoBackupFile(string FileName, DateTimeOffset CreatedAt)
{
    /// <summary>Sicherung, die wegen einer Schemaänderung entstanden ist (siehe Dateiname).</summary>
    public bool IsSchemaBackup =>
        FileName is not null &&
        FileName.Contains($"-{AutoBackupPolicy.SchemaTag}{AutoBackupPolicy.FileExtension}", StringComparison.OrdinalIgnoreCase);
}

/// <summary>
/// Wann automatisch gesichert wird und was davon aufgehoben bleibt.
///
/// <para>Hintergrund: die App verzichtet bewusst auf EF-Migrationen (siehe
/// <c>SqliteSchemaUpdater</c>) und gleicht das Schema zur Laufzeit ab - das kann aber nur
/// ADDITIVE Änderungen. Bei einer umbenannten Spalte oder umgedeuteten Werten bleibt als letzte
/// Möglichkeit "lerntor.db löschen", und damit sind Sterne, Fortschritte und Fehler-Kartei
/// beider Kinder weg. Genau davor schützt die automatische Sicherung: sie entsteht <b>bevor</b>
/// am Schema etwas verändert wird.</para>
///
/// <para>Die Sicherung von Hand (Eltern-Bereich → USB-Stick) bleibt wichtiger, weil sie auch
/// einen Plattendefekt übersteht - aber sie setzt voraus, dass jemand daran denkt. Diese hier
/// denkt für niemanden mit und liegt auf derselben Platte; sie deckt den häufigeren Fall ab:
/// ein App-Update, nach dem die Daten nicht mehr passen.</para>
/// </summary>
public static class AutoBackupPolicy
{
    /// <summary>So viele automatische Sicherungen bleiben liegen (plus die neueste
    /// Schema-Sicherung, siehe <see cref="Obsolete"/>).</summary>
    public const int KeepCount = 5;

    public const string FilePrefix = "lerntor-auto-";
    public const string FileExtension = ".db";
    public const string SchemaTag = "schema";
    public const string DailyTag = "taeglich";

    /// <summary>
    /// Dateiname mit Zeitpunkt und Grund. Gerechnet wird mit <c>DateTimeOffset.DateTime</c>, also
    /// der Wanduhrzeit des übergebenen Zeitpunkts - nicht mit <c>LocalDateTime</c>, das in die
    /// Zeitzone des ausführenden Rechners umrechnet. Im Betrieb ist das dasselbe (die App reicht
    /// <c>DateTimeOffset.Now</c> herein), aber ein Test wäre sonst davon abhängig, in welcher
    /// Zeitzone der Build-Rechner steht.
    /// </summary>
    public static string FileNameFor(DateTimeOffset when, AutoBackupReason reason) =>
        $"{FilePrefix}{when.DateTime:yyyy-MM-dd-HHmmss}-{Tag(reason)}{FileExtension}";

    private static string Tag(AutoBackupReason reason) =>
        reason == AutoBackupReason.Schemaaenderung ? SchemaTag : DailyTag;

    /// <summary>Erkennt eigene Dateien - fremde Dateien im Ordner werden nie gelöscht.</summary>
    public static bool IsOwnFile(string fileName) =>
        fileName.StartsWith(FilePrefix, StringComparison.OrdinalIgnoreCase) &&
        fileName.EndsWith(FileExtension, StringComparison.OrdinalIgnoreCase);

    /// <summary>
    /// Eine Schema-Sicherung ist immer fällig - sie ist der eigentliche Zweck der Übung. Die
    /// tägliche höchstens einmal pro Kalendertag, sonst zöge jeder Neustart eine weitere Kopie.
    /// </summary>
    public static bool IsDue(IEnumerable<AutoBackupFile> existing, DateTimeOffset now, AutoBackupReason reason)
    {
        if (reason == AutoBackupReason.Schemaaenderung)
        {
            return true;
        }

        var today = DateOnly.FromDateTime(now.DateTime);
        return !existing.Any(file => DateOnly.FromDateTime(file.CreatedAt.DateTime) == today);
    }

    /// <summary>
    /// Liefert die Dateinamen, die weg dürfen: alles außer den <paramref name="keep"/> neuesten -
    /// die <b>neueste Schema-Sicherung bleibt jedoch immer</b> erhalten, auch wenn sie längst aus
    /// den neuesten herausgerutscht ist. Sie ist der Stand direkt vor der letzten Schemaänderung
    /// und damit die einzige, die einen misslungenen Umbau noch rückgängig machen kann; lernt ein
    /// Kind danach eine Woche weiter, hätten fünf tägliche Sicherungen sie sonst verdrängt.
    /// </summary>
    public static IReadOnlyList<string> Obsolete(IEnumerable<AutoBackupFile> existing, int keep = KeepCount)
    {
        var files = existing
            .OrderByDescending(file => file.CreatedAt)
            .ThenByDescending(file => file.FileName, StringComparer.Ordinal)
            .ToList();

        var protectedNames = files
            .Take(Math.Max(1, keep))
            .Select(file => file.FileName)
            .ToHashSet(StringComparer.OrdinalIgnoreCase);

        var newestSchemaBackup = files
            .Where(file => file.IsSchemaBackup)
            .Select(file => file.FileName)
            .FirstOrDefault();

        if (newestSchemaBackup is not null)
        {
            protectedNames.Add(newestSchemaBackup);
        }

        return files
            .Where(file => !protectedNames.Contains(file.FileName))
            .Select(file => file.FileName)
            .ToList();
    }
}
