using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;

namespace LernTor.Data;

/// <summary>
/// Ein kurzer Fingerabdruck des aktuellen EF-Modells, um zu erkennen, ob sich das Schema seit dem
/// letzten Start verändert hat - <b>bevor</b> <see cref="SqliteSchemaUpdater"/> etwas daran ändert.
///
/// <para>Warum überhaupt: der Schema-Abgleich kann nur additive Änderungen (neue Tabellen/Spalten).
/// Eine umbenannte Spalte oder umgedeutete Werte kann er nicht heilen - dort bliebe nur
/// "lerntor.db löschen", und damit wären alle Fortschritte weg. Wer weiß, dass gleich etwas am
/// Schema passiert, kann vorher sichern (siehe <c>AutoBackupService</c>); erst danach ist es zu
/// spät, weil die Sicherung dann schon den neuen Stand enthielte.</para>
///
/// <para>Der Fingerabdruck liegt bewusst in einer <b>Datei neben der Datenbank</b> und nicht in
/// der Settings-Tabelle: eine Spalte dafür müsste selbst erst vom Schema-Abgleich angelegt
/// werden, und man müsste sie lesen, bevor es sie gibt.</para>
/// </summary>
public static class SchemaFingerprint
{
    /// <summary>Länge des Hex-Ausschnitts - 16 Zeichen sind für einen Gleich/Ungleich-Vergleich
    /// mehr als genug und bleiben im Fehlerprotokoll lesbar.</summary>
    public const int Length = 16;

    /// <summary>
    /// Berechnet den Fingerabdruck aus dem CREATE-Skript des Modells
    /// (<c>db.Database.GenerateCreateScript()</c>). Whitespace wird vorher vereinheitlicht, damit
    /// eine reine Formatierungsänderung von EF nicht als Schemaänderung durchgeht.
    /// </summary>
    public static string Compute(string createScript)
    {
        var normalized = Regex.Replace(createScript ?? string.Empty, @"\s+", " ").Trim();
        var hash = SHA256.HashData(Encoding.UTF8.GetBytes(normalized));

        return Convert.ToHexString(hash)[..Length].ToLowerInvariant();
    }

    /// <summary>Liest den zuletzt gespeicherten Fingerabdruck; null = noch keiner vorhanden
    /// (frische Installation oder erster Start mit dieser Funktion).</summary>
    public static string? Read(string filePath)
    {
        try
        {
            if (!File.Exists(filePath))
            {
                return null;
            }

            var content = File.ReadAllText(filePath).Trim();
            return content.Length == 0 ? null : content;
        }
        catch (IOException)
        {
            // Ein unlesbarer Fingerabdruck darf den App-Start nicht kippen - er führt dann
            // schlimmstenfalls zu einer Sicherung zu viel, und das ist die harmlose Richtung.
            return null;
        }
    }

    public static void Write(string filePath, string fingerprint)
    {
        var directory = Path.GetDirectoryName(filePath);
        if (!string.IsNullOrEmpty(directory))
        {
            Directory.CreateDirectory(directory);
        }

        File.WriteAllText(filePath, fingerprint);
    }
}
