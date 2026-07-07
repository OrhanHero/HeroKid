using Microsoft.Win32;

namespace LernTor.Security;

/// <summary>
/// Deaktiviert den Task-Manager per Windows-Registry-Richtlinie, solange der Kiosk-Modus aktiv ist.
/// Dies ist die Gegenmaßnahme dafür, dass Strg+Alt+Entf nicht von Anwendungen abgefangen werden kann:
/// Der Nutzer kann über Strg+Alt+Entf zwar den Sperrbildschirm/das Sicherheitsmenü öffnen, der
/// Task-Manager selbst zeigt dann aber eine "von Ihrem Administrator deaktiviert"-Meldung.
/// </summary>
public static class TaskManagerPolicy
{
    private const string PolicyKeyPath = @"Software\Microsoft\Windows\CurrentVersion\Policies\System";
    private const string ValueName = "DisableTaskMgr";

    public static void Disable()
    {
        using var key = Registry.CurrentUser.CreateSubKey(PolicyKeyPath, writable: true);
        key.SetValue(ValueName, 1, RegistryValueKind.DWord);
    }

    public static void Enable()
    {
        using var key = Registry.CurrentUser.OpenSubKey(PolicyKeyPath, writable: true);
        key?.DeleteValue(ValueName, throwOnMissingValue: false);
    }

    public static bool IsDisabled()
    {
        using var key = Registry.CurrentUser.OpenSubKey(PolicyKeyPath, writable: false);
        var value = key?.GetValue(ValueName);
        return value is int i && i == 1;
    }
}
