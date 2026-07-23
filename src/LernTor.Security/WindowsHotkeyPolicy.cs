using System.Runtime.InteropServices;
using Microsoft.Win32;

namespace LernTor.Security;

/// <summary>
/// Deaktiviert die Windows-Tasten-Hotkeys (Win+Tab/D/E/R/X/I/Pfeiltasten - Aufgabenansicht,
/// virtuelle Desktops, Explorer, Ausführen-Dialog, Schnellzugriffsmenü, Einstellungen) per
/// Gruppenrichtlinie, solange der Kiosk-Modus aktiv ist. Ergänzt <see cref="KioskKeyboardHook"/>:
/// Der Hook verschluckt Tastendrücke auf Nachrichtenebene, diese Richtlinie wirkt zusätzlich auf
/// Ebene der Shell selbst - manche Win-Tasten-Kombinationen (allen voran Win+Tab/Aufgabenansicht)
/// werden von Windows teils unabhängig vom normalen Tastatur-Hook erkannt, weshalb ein Hook allein
/// sie nicht zuverlässig blockiert (realer Bug: Kinder legten über Win+Tab einen neuen virtuellen
/// Desktop an und hatten dort vollen PC-Zugriff, obwohl der Hook lief).
/// </summary>
public static class WindowsHotkeyPolicy
{
    private const string PolicyKeyPath = @"Software\Microsoft\Windows\CurrentVersion\Policies\Explorer";
    private const string ValueName = "NoWinKeys";

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

    /// <summary>Siehe <see cref="TaskManagerPolicy.BroadcastPolicyChange"/> - dieselbe
    /// Cache-Problematik gilt für Explorer-Richtlinien allgemein.</summary>
    private static void BroadcastPolicyChange()
    {
        SendMessageTimeout(new IntPtr(HWND_BROADCAST), WM_SETTINGCHANGE, IntPtr.Zero, "Policy", SMTO_ABORTIFHUNG, 5000, out _);
    }
}
