using System.Runtime.InteropServices;

namespace LernTor.Security;

/// <summary>
/// Globaler Low-Level-Keyboard-Hook (WH_KEYBOARD_LL), der die Windows-Taste, Alt+Tab,
/// Strg+Esc und Alt+Esc unterdrückt, solange der Kiosk-Modus aktiv ist.
/// Wichtig: Strg+Alt+Entf (Secure Attention Sequence) kann von KEINER Anwendung im
/// User-Modus abgefangen werden – das ist ein bewusster OS-Schutz von Windows.
/// Als Gegenmaßnahme dafür wird zusätzlich <see cref="TaskManagerPolicy"/> gesetzt.
/// </summary>
public sealed class KioskKeyboardHook : IDisposable
{
    private const int WH_KEYBOARD_LL = 13;
    private const int WM_KEYDOWN = 0x0100;
    private const int WM_SYSKEYDOWN = 0x0104;

    private const int VK_LWIN = 0x5B;
    private const int VK_RWIN = 0x5C;
    private const int VK_TAB = 0x09;
    private const int VK_ESCAPE = 0x1B;
    private const int VK_MENU = 0x12; // Alt
    private const int VK_CONTROL = 0x11;
    private const int VK_F4 = 0x73;

    private delegate nint LowLevelKeyboardProc(int nCode, nint wParam, nint lParam);

    [DllImport("user32.dll", SetLastError = true)]
    private static extern nint SetWindowsHookEx(int idHook, LowLevelKeyboardProc lpfn, nint hMod, uint dwThreadId);

    [DllImport("user32.dll", SetLastError = true)]
    [return: MarshalAs(UnmanagedType.Bool)]
    private static extern bool UnhookWindowsHookEx(nint hhk);

    [DllImport("user32.dll", SetLastError = true)]
    private static extern nint CallNextHookEx(nint hhk, int nCode, nint wParam, nint lParam);

    [DllImport("user32.dll")]
    private static extern short GetAsyncKeyState(int vKey);

    // Gehalten als Feld, damit der Delegate nicht vom GC eingesammelt wird, solange der Hook aktiv ist.
    private readonly LowLevelKeyboardProc _proc;
    private nint _hookHandle;

    public KioskKeyboardHook()
    {
        _proc = HookCallback;
    }

    public bool IsActive => _hookHandle != nint.Zero;

    public void Install()
    {
        if (IsActive)
        {
            return;
        }

        using var currentProcess = System.Diagnostics.Process.GetCurrentProcess();
        using var currentModule = currentProcess.MainModule!;
        _hookHandle = SetWindowsHookEx(
            WH_KEYBOARD_LL,
            _proc,
            Marshal.GetHINSTANCE(typeof(KioskKeyboardHook).Module),
            0);

        if (_hookHandle == nint.Zero)
        {
            throw new InvalidOperationException(
                $"Keyboard-Hook konnte nicht installiert werden (Win32-Fehler {Marshal.GetLastWin32Error()}).");
        }
    }

    public void Uninstall()
    {
        if (IsActive)
        {
            UnhookWindowsHookEx(_hookHandle);
            _hookHandle = nint.Zero;
        }
    }

    private nint HookCallback(int nCode, nint wParam, nint lParam)
    {
        if (nCode >= 0 && (wParam == WM_KEYDOWN || wParam == WM_SYSKEYDOWN))
        {
            int vkCode = Marshal.ReadInt32(lParam);
            bool altPressed = (GetAsyncKeyState(VK_MENU) & 0x8000) != 0;
            bool ctrlPressed = (GetAsyncKeyState(VK_CONTROL) & 0x8000) != 0;

            bool isWindowsKey = vkCode is VK_LWIN or VK_RWIN;
            bool isAltTab = altPressed && vkCode == VK_TAB;
            bool isAltEsc = altPressed && vkCode == VK_ESCAPE;
            bool isCtrlEsc = ctrlPressed && vkCode == VK_ESCAPE;
            bool isAltF4 = altPressed && vkCode == VK_F4;

            if (isWindowsKey || isAltTab || isAltEsc || isCtrlEsc || isAltF4)
            {
                // Taste verschlucken statt weiterzuleiten -> Windows reagiert nicht darauf.
                return (nint)1;
            }
        }

        return CallNextHookEx(_hookHandle, nCode, wParam, lParam);
    }

    public void Dispose() => Uninstall();
}
