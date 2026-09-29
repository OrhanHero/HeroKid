using System.Security.Cryptography;
using LernTor.Security;
using Xunit;

namespace LernTor.UiTests;

/// <summary>
/// Eltern-Passwort (PBKDF2). Steht in diesem Projekt, weil LernTor.Security net10.0-windows ist
/// und nur hier referenziert wird - die Tests selbst brauchen kein WPF.
/// </summary>
public sealed class AdminAuthServiceTests
{
    [Fact]
    public void Neues_Passwort_wird_mit_aktueller_Staerke_gespeichert_und_geprueft()
    {
        var (hash, salt) = AdminAuthService.HashPassword("geheim123");

        Assert.StartsWith($"pbkdf2-sha256:{AdminAuthService.CurrentIterations}:", salt);
        Assert.True(AdminAuthService.Verify("geheim123", hash, salt));
        Assert.False(AdminAuthService.Verify("geheim124", hash, salt));
        Assert.False(AdminAuthService.NeedsRehash(salt));
    }

    [Fact]
    public void Passwort_aus_der_Zeit_vor_der_Umstellung_bleibt_gueltig_und_wird_als_veraltet_erkannt()
    {
        // Genau so, wie der alte Code gespeichert hat: reines Base64-Salt, 210.000 Durchläufe.
        var salt = RandomNumberGenerator.GetBytes(16);
        var hash = Rfc2898DeriveBytes.Pbkdf2("vorher", salt, 210_000, HashAlgorithmName.SHA256, 32);
        var gespeicherterHash = Convert.ToBase64String(hash);
        var gespeichertesSalt = Convert.ToBase64String(salt);

        Assert.True(AdminAuthService.Verify("vorher", gespeicherterHash, gespeichertesSalt));
        Assert.False(AdminAuthService.Verify("anders", gespeicherterHash, gespeichertesSalt));
        Assert.True(AdminAuthService.NeedsRehash(gespeichertesSalt));
    }

    [Theory]
    [InlineData("", "")]
    [InlineData("!!kein-base64!!", "pbkdf2-sha256:600000:AAAAAAAAAAAAAAAAAAAAAA==")]
    [InlineData("AAAA", "pbkdf2-sha256:abc:AAAA")]
    [InlineData("AAAA", "pbkdf2-sha256:0:AAAA")]
    [InlineData("AAAA", "pbkdf2-sha256:600000")]
    public void Kaputte_Eintraege_werden_abgelehnt_statt_zu_werfen(string hash, string salt)
    {
        Assert.False(AdminAuthService.Verify("irgendwas", hash, salt));
        Assert.False(AdminAuthService.NeedsRehash(salt));
    }
}
