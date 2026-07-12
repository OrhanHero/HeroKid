using System.Text.RegularExpressions;
using LernTor.Core.Logging;
using Microsoft.EntityFrameworkCore;

namespace LernTor.Data;

/// <summary>
/// Gleicht das Schema einer BESTEHENDEN lerntor.db beim App-Start mit dem aktuellen EF-Modell ab:
/// fehlende Tabellen werden angelegt, fehlende Spalten per <c>ALTER TABLE ... ADD COLUMN</c>
/// ergänzt, Indizes idempotent (IF NOT EXISTS) nachgezogen. Damit entfällt das bisherige
/// "nach jedem Schema-Update bitte lerntor.db löschen" - Profile, Sterne und Fortschritte
/// überleben App-Updates.
///
/// <para><b>Warum kein klassisches EF-Migrations-Setup:</b> Migrationen müssen bei jeder
/// Schema-Änderung mit dem <c>dotnet ef</c>-Tooling GENERIERT werden - diese Codebasis wird aber
/// aus einer Umgebung ohne .NET SDK entwickelt (siehe CLAUDE.md), handgeschriebene Migrationen
/// samt ModelSnapshot driften dann zwangsläufig vom echten Modell weg. Dieser Abgleich leitet
/// sich stattdessen zur Laufzeit direkt aus dem Modell ab (<c>GenerateCreateScript()</c>) und
/// kann daher nie veralten.</para>
///
/// <para><b>Grenzen (bewusst):</b> Nur ADDITIVE Änderungen werden übernommen - neue Tabellen,
/// neue Spalten, neue Indizes. Entfernte/umbenannte Spalten bleiben als tote Spalten stehen
/// (harmlos), und eine Umdeutung vorhandener Werte (z.B. Enum-Umsortierung bei numerischer
/// Speicherung) kann kein Schema-Abgleich der Welt heilen - dagegen hilft weiterhin die Regel,
/// Enums als Strings zu persistieren (siehe JsonOptions.Default / SavedArticleEntity).</para>
/// </summary>
public static class SqliteSchemaUpdater
{
    /// <summary>Führt den Abgleich aus und liefert die angewendeten Änderungen (leer = Schema
    /// war aktuell). Änderungen werden zusätzlich ins Fehlerprotokoll (AppLog) geschrieben.</summary>
    public static IReadOnlyList<string> Update(LernTorDbContext db)
    {
        var applied = new List<string>();

        // Das vollständige CREATE-Skript des AKTUELLEN Modells ist die einzige Wahrheitsquelle -
        // daraus werden Soll-Tabellen, Soll-Spalten (inkl. Typ/NOT NULL) und Indizes gelesen.
        var script = db.Database.GenerateCreateScript();
        var statements = script
            .Split(';')
            .Select(s => s.Trim())
            .Where(s => s.Length > 0)
            .ToList();

        var existingTables = QuerySingleColumn(db,
            "SELECT name FROM sqlite_master WHERE type = 'table'");

        foreach (var statement in statements.Where(s => s.StartsWith("CREATE TABLE", StringComparison.OrdinalIgnoreCase)))
        {
            var tableName = Regex.Match(statement, "CREATE TABLE \"([^\"]+)\"").Groups[1].Value;

            if (!existingTables.Contains(tableName, StringComparer.OrdinalIgnoreCase))
            {
                db.Database.ExecuteSqlRaw(statement);
                applied.Add($"Tabelle \"{tableName}\" angelegt");
                continue;
            }

            var existingColumns = QuerySingleColumn(db, $"SELECT name FROM pragma_table_info('{tableName}')");

            foreach (var column in ParseColumnDefinitions(statement))
            {
                if (existingColumns.Contains(column.Name, StringComparer.OrdinalIgnoreCase))
                {
                    continue;
                }

                db.Database.ExecuteSqlRaw($"ALTER TABLE \"{tableName}\" ADD COLUMN {column.AddColumnSql}");
                applied.Add($"Spalte \"{tableName}\".\"{column.Name}\" ergänzt");
            }
        }

        // Indizes zum Schluss (nachdem alle Tabellen/Spalten existieren), idempotent per
        // IF NOT EXISTS - deckt sowohl Indizes neuer Tabellen als auch neue Indizes auf
        // bestehenden Tabellen ab.
        foreach (var statement in statements.Where(s => s.StartsWith("CREATE ", StringComparison.OrdinalIgnoreCase)
                                                        && s.Contains("INDEX", StringComparison.OrdinalIgnoreCase)
                                                        && !s.StartsWith("CREATE TABLE", StringComparison.OrdinalIgnoreCase)))
        {
            var idempotent = Regex.Replace(statement, @"^CREATE (UNIQUE )?INDEX ",
                "CREATE $1INDEX IF NOT EXISTS ", RegexOptions.IgnoreCase);
            db.Database.ExecuteSqlRaw(idempotent);
        }

        foreach (var change in applied)
        {
            AppLog.Info("Datenbank", $"Schema-Update: {change}");
        }

        return applied;
    }

    private sealed record ColumnDefinition(string Name, string AddColumnSql);

    /// <summary>
    /// Zerlegt den Spaltenblock eines CREATE-TABLE-Statements in einzelne Spaltendefinitionen.
    /// Tabellen-Constraints (CONSTRAINT/FOREIGN KEY/...) werden übersprungen; Inline-PK-Zusätze
    /// werden entfernt (die PK-Spalte "Id" existiert in Alt-Tabellen ohnehin immer). Für
    /// NOT-NULL-Spalten ohne DEFAULT wird ein typgerechter DEFAULT ergänzt - SQLite verlangt
    /// ihn bei ADD COLUMN auf gefüllten Tabellen und befüllt damit zugleich die Altzeilen.
    /// </summary>
    private static List<ColumnDefinition> ParseColumnDefinitions(string createTableStatement)
    {
        var open = createTableStatement.IndexOf('(');
        var close = createTableStatement.LastIndexOf(')');
        var body = createTableStatement[(open + 1)..close];

        var columns = new List<ColumnDefinition>();

        foreach (var part in SplitTopLevel(body))
        {
            var definition = part.Trim();
            if (!definition.StartsWith('"'))
            {
                continue; // CONSTRAINT/FOREIGN KEY/... - keine Spalte.
            }

            var name = definition[1..definition.IndexOf('"', 1)];

            // Inline-PK-Deklaration entfernen (kommt bei ADD COLUMN nie zum Einsatz, s.o.).
            var addSql = Regex.Replace(definition,
                @"\s+CONSTRAINT\s+""[^""]+""\s+PRIMARY KEY(\s+AUTOINCREMENT)?", string.Empty,
                RegexOptions.IgnoreCase);

            if (addSql.Contains("NOT NULL", StringComparison.OrdinalIgnoreCase) &&
                !addSql.Contains("DEFAULT", StringComparison.OrdinalIgnoreCase))
            {
                var type = definition.Split(' ', StringSplitOptions.RemoveEmptyEntries)
                    .Skip(1).FirstOrDefault()?.ToUpperInvariant() ?? "TEXT";
                var defaultValue = type is "INTEGER" or "REAL" or "NUMERIC" ? "0" : "''";
                addSql += $" DEFAULT {defaultValue}";
            }

            columns.Add(new ColumnDefinition(name, addSql));
        }

        return columns;
    }

    /// <summary>Trennt am Komma, aber nur auf oberster Klammer-Ebene (Typen wie NUMERIC(10,2)
    /// enthalten selbst Kommas).</summary>
    private static IEnumerable<string> SplitTopLevel(string body)
    {
        var depth = 0;
        var start = 0;
        for (var i = 0; i < body.Length; i++)
        {
            switch (body[i])
            {
                case '(': depth++; break;
                case ')': depth--; break;
                case ',' when depth == 0:
                    yield return body[start..i];
                    start = i + 1;
                    break;
            }
        }

        yield return body[start..];
    }

    private static List<string> QuerySingleColumn(LernTorDbContext db, string sql)
    {
        var results = new List<string>();
        var connection = db.Database.GetDbConnection();

        var wasOpen = connection.State == System.Data.ConnectionState.Open;
        if (!wasOpen)
        {
            connection.Open();
        }

        try
        {
            using var command = connection.CreateCommand();
            command.CommandText = sql;
            using var reader = command.ExecuteReader();
            while (reader.Read())
            {
                results.Add(reader.GetString(0));
            }
        }
        finally
        {
            if (!wasOpen)
            {
                connection.Close();
            }
        }

        return results;
    }
}
