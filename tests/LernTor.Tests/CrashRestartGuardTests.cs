using LernTor.Core.Services;
using Xunit;

namespace LernTor.Tests;

public sealed class CrashRestartGuardTests : IDisposable
{
    private readonly string _stateFilePath = Path.Combine(
        Path.GetTempPath(), "lerntor-crashguard-tests", Guid.NewGuid().ToString("N"), "crash-restarts.txt");

    private readonly CrashRestartGuard _guard;

    public CrashRestartGuardTests()
    {
        _guard = new CrashRestartGuard(_stateFilePath);
    }

    public void Dispose()
    {
        try
        {
            Directory.Delete(Path.GetDirectoryName(_stateFilePath)!, recursive: true);
        }
        catch (IOException)
        {
            // Deckt auch DirectoryNotFoundException ab - Aufräumen ist Best-Effort.
        }
    }

    [Fact]
    public void Allows_first_restart()
    {
        Assert.True(_guard.TryRegisterRestart());
    }

    [Fact]
    public void Blocks_fourth_restart_within_window()
    {
        var start = DateTimeOffset.UtcNow;

        Assert.True(_guard.TryRegisterRestart(start));
        Assert.True(_guard.TryRegisterRestart(start + TimeSpan.FromMinutes(1)));
        Assert.True(_guard.TryRegisterRestart(start + TimeSpan.FromMinutes(2)));
        Assert.False(_guard.TryRegisterRestart(start + TimeSpan.FromMinutes(3)));
    }

    [Fact]
    public void Allows_restart_again_once_old_entries_left_the_window()
    {
        var start = DateTimeOffset.UtcNow;

        Assert.True(_guard.TryRegisterRestart(start));
        Assert.True(_guard.TryRegisterRestart(start + TimeSpan.FromMinutes(1)));
        Assert.True(_guard.TryRegisterRestart(start + TimeSpan.FromMinutes(2)));
        Assert.False(_guard.TryRegisterRestart(start + TimeSpan.FromMinutes(3)));

        // 13 Minuten nach dem ersten Crash sind alle drei Einträge älter als das 10-Minuten-Fenster.
        Assert.True(_guard.TryRegisterRestart(start + TimeSpan.FromMinutes(13)));
    }

    [Fact]
    public void Unparseable_lines_in_state_file_are_ignored()
    {
        Directory.CreateDirectory(Path.GetDirectoryName(_stateFilePath)!);
        File.WriteAllLines(_stateFilePath, new[] { "kaputt", "", "auch-kein-datum" });

        Assert.True(_guard.TryRegisterRestart());
    }
}
