using System.Security.Cryptography;

namespace LernTor.Security;

/// <summary>
/// PBKDF2-basiertes Hashing für das Eltern-Admin-Passwort (zum Überspringen/Konfigurieren).
/// Es wird nie ein Klartext-Passwort gespeichert.
/// </summary>
public static class AdminAuthService
{
    private const int SaltSizeBytes = 16;
    private const int HashSizeBytes = 32;
    private const int Iterations = 210_000;

    public static (string Hash, string Salt) HashPassword(string plainPassword)
    {
        var salt = RandomNumberGenerator.GetBytes(SaltSizeBytes);
        var hash = Rfc2898DeriveBytes.Pbkdf2(plainPassword, salt, Iterations, HashAlgorithmName.SHA256, HashSizeBytes);

        return (Convert.ToBase64String(hash), Convert.ToBase64String(salt));
    }

    public static bool Verify(string plainPassword, string storedHash, string storedSalt)
    {
        if (string.IsNullOrEmpty(storedHash) || string.IsNullOrEmpty(storedSalt))
        {
            return false;
        }

        var salt = Convert.FromBase64String(storedSalt);
        var expectedHash = Convert.FromBase64String(storedHash);
        var actualHash = Rfc2898DeriveBytes.Pbkdf2(plainPassword, salt, Iterations, HashAlgorithmName.SHA256, HashSizeBytes);

        return CryptographicOperations.FixedTimeEquals(actualHash, expectedHash);
    }
}
