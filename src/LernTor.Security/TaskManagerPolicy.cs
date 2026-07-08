using System.Runtime.InteropServices;
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

    private const int HWND_BROADCAST = 0xffff;
    private const int WM_SETTINGCHANGE = 0x001A;
    private const int SMTO_ABORTIFHUNG = 0x0002;

    [DllImport("user32.dll", SetLastError = true, CharSet = CharSet.Auto)]
    private static extern IntPtr SendMessageTimeout(
        IntPtr hWnd, int msg, IntPtr wParam, string lParam, int fuFlags, int uTimeout, out IntPtr lpdwResult);

    public static void Disable()
    {
        using var key = Registry.CurrentUser.CreateSubKey(PolicyKeyPath, writable: true);
        key.SetValue(ValueName, 1, RegistryValueKind.DWord);
        BroadcastPolicyChange();
    }

    public static void Enable()
    {
        using var key = Registry.CurrentUser.OpenSubKey(PolicyKeyPath, writable: true);
        key?.DeleteValue(ValueName, throwOnMissingValue: false);
        BroadcastPolicyChange();
    }

    public static bool IsDisabled()
    {
        using var key = Registry.CurrentUser.OpenSubKey(PolicyKeyPath, writable: false);
        var value = key?.GetValue(ValueName);
        return value is int i && i == 1;
    }

    /// <summary>
    /// Windows/Explorer liest Richtlinien-Registrywerte wie <see cref="ValueName"/> nicht bei jedem
    /// Zugriff auf Task-Manager neu ein, sondern cached sie - normalerweise werden sie nur bei
    /// Anmeldung oder einem expliziten Policy-Refresh neu ausgewertet. Ohne diesen Broadcast bleibt
    /// der Task-Manager nach <see cref="Enable"/> so lange gesperrt, bis sich der Nutzer ab- und
    /// wieder anmeldet oder Explorer neu startet - genau das Verhalten, das beim Testen auffiel.
    /// </summary>
    private static void BroadcastPolicyChange()
    {
        SendMessageTimeout(new IntPtr(HWND_BROADCAST), WM_SETTINGCHANGE, IntPtr.Zero, "Policy", SMTO_ABORTIFHUNG, 5000, out _);
    }
}
