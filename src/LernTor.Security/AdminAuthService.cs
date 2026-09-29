using System.Globalization;
using System.Security.Cryptography;

namespace LernTor.Security;

/// <summary>
/// PBKDF2-basiertes Hashing für das Eltern-Admin-Passwort (zum Überspringen/Konfigurieren).
/// Es wird nie ein Klartext-Passwort gespeichert.
///
/// <para><b>Versioniertes Salt-Feld (seit 29.09.2026).</b> Die Zahl der Durchläufe steht jetzt
/// mit im gespeicherten Salt: <c>pbkdf2-sha256:600000:&lt;Salt&gt;</c>. Ältere Einträge sind
/// reines Base64 ohne Präfix und wurden mit 210.000 Durchläufen erzeugt - sie bleiben gültig.
/// Nach dem nächsten erfolgreichen Anmelden meldet <see cref="NeedsRehash"/> sie als veraltet,
/// und der Eltern-Bereich speichert das Passwort unbemerkt mit der aktuellen Stärke neu. So lässt
/// sich die Zahl später wieder anheben, ohne dass jemand sein Passwort neu setzen muss.</para>
///
/// <para>600.000 Durchläufe mit SHA-256 entsprechen der OWASP-Empfehlung seit 2023 (vorher
/// 210.000 für SHA-512 bzw. 310.000 für SHA-256).</para>
/// </summary>
public static class AdminAuthService
{
    private const int SaltSizeBytes = 16;
    private const int HashSizeBytes = 32;

    /// <summary>Aktuelle Stärke für neu gespeicherte Passwörter.</summary>
    public const int CurrentIterations = 600_000;

    /// <summary>Stärke der Einträge ohne Präfix (bis 29.09.2026).</summary>
    public const int LegacyIterations = 210_000;

    private const string Prefix = "pbkdf2-sha256:";

    public static (string Hash, string Salt) HashPassword(string plainPassword) =>
        HashPassword(plainPassword, CurrentIterations);

    /// <summary>Mit ausdrücklicher Stärke - für Tests, die ältere Einträge nachstellen.</summary>
    public static (string Hash, string Salt) HashPassword(string plainPassword, int iterations)
    {
        var salt = RandomNumberGenerator.GetBytes(SaltSizeBytes);
        var hash = Rfc2898DeriveBytes.Pbkdf2(plainPassword, salt, iterations, HashAlgorithmName.SHA256, HashSizeBytes);

        var saltField = iterations == LegacyIterations
            ? Convert.ToBase64String(salt)
            : $"{Prefix}{iterations.ToString(CultureInfo.InvariantCulture)}:{Convert.ToBase64String(salt)}";

        return (Convert.ToBase64String(hash), saltField);
    }

    public static bool Verify(string plainPassword, string storedHash, string storedSalt)
    {
        if (string.IsNullOrEmpty(storedHash) || !TryParseSalt(storedSalt, out var salt, out var iterations))
        {
            return false;
        }

        byte[] expectedHash;
        try
        {
            expectedHash = Convert.FromBase64String(storedHash);
        }
        catch (FormatException)
        {
            return false;
        }

        var actualHash = Rfc2898DeriveBytes.Pbkdf2(plainPassword, salt, iterations, HashAlgorithmName.SHA256, expectedHash.Length);
        return CryptographicOperations.FixedTimeEquals(actualHash, expectedHash);
    }

    /// <summary>
    /// Ob ein gespeichertes Passwort schwächer ist als <see cref="CurrentIterations"/>. Nur nach
    /// einem ERFOLGREICHEN <see cref="Verify"/> auswerten - dann ist das Klartext-Passwort
    /// bekannt und kann neu gehasht werden.
    /// </summary>
    public static bool NeedsRehash(string storedSalt) =>
        TryParseSalt(storedSalt, out _, out var iterations) && iterations < CurrentIterations;

    private static bool TryParseSalt(string? storedSalt, out byte[] salt, out int iterations)
    {
        salt = Array.Empty<byte>();
        iterations = 0;

        if (string.IsNullOrEmpty(storedSalt))
        {
            return false;
        }

        string base64;
        if (storedSalt.StartsWith(Prefix, StringComparison.Ordinal))
        {
            var teile = storedSalt[Prefix.Length..].Split(':', 2);
            if (teile.Length != 2
                || !int.TryParse(teile[0], NumberStyles.None, CultureInfo.InvariantCulture, out iterations)
                || iterations <= 0)
            {
                return false;
            }

            base64 = teile[1];
        }
        else
        {
            iterations = LegacyIterations;
            base64 = storedSalt;
        }

        try
        {
            salt = Convert.FromBase64String(base64);
            return salt.Length > 0;
        }
        catch (FormatException)
        {
            return false;
        }
    }
}
