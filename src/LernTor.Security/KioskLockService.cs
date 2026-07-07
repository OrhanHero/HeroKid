namespace LernTor.Security;

/// <summary>
/// Fasst alle Soft-Lock-Maßnahmen zusammen: Keyboard-Hook (Win-Taste/Alt+Tab/Ctrl+Esc)
/// + Task-Manager-Registry-Sperre. Fensterbezogene Maßnahmen (Vollbild, Topmost, kein
/// Fensterrahmen) werden im WPF-Fenster selbst gesetzt (LernTor.App/Views/MainWindow).
/// </summary>
public sealed class KioskLockService : IDisposable
{
    private readonly KioskKeyboardHook _keyboardHook = new();
    private bool _isLocked;

    public bool IsLocked => _isLocked;

    public void Lock()
    {
        if (_isLocked)
        {
            return;
        }

        _keyboardHook.Install();
        TaskManagerPolicy.Disable();
        _isLocked = true;
    }

    public void Unlock()
    {
        if (!_isLocked)
        {
            return;
        }

        _keyboardHook.Uninstall();
        TaskManagerPolicy.Enable();
        _isLocked = false;
    }

    public void Dispose()
    {
        Unlock();
        _keyboardHook.Dispose();
    }
}
