namespace LernTor.Security;

/// <summary>
/// Fasst alle Soft-Lock-Maßnahmen zusammen: Keyboard-Hook (Win-Taste/Alt+Tab/Ctrl+Esc)
/// + Task-Manager-Registry-Sperre. Fensterbezogene Maßnahmen (Vollbild, Topmost, kein
/// Fensterrahmen) werden im WPF-Fenster selbst gesetzt (LernTor.App/Views/MainWindow).
///
/// Jede Maßnahme wird unabhängig von den anderen versucht: Auf manchen Rechnern verweigert
/// Gruppenrichtlinie/Virenschutz den Registry-Zugriff für die Task-Manager-Sperre (dieser
/// Registry-Pfad ist ein bekanntes Ziel für Malware und wird deshalb oft überwacht/blockiert).
/// Ein Fehlschlag einer einzelnen Maßnahme darf die App nicht unbenutzbar machen - die
/// übrigen Maßnahmen greifen trotzdem.
/// </summary>
public sealed class KioskLockService : IDisposable
{
    private readonly KioskKeyboardHook _keyboardHook = new();
    private bool _isLocked;
    private bool _taskManagerPolicyActive;

    public bool IsLocked => _isLocked;

    /// <summary>Warnungen zu Härtungsmaßnahmen, die auf diesem Rechner nicht angewendet werden konnten.</summary>
    public IReadOnlyList<string> Warnings { get; private set; } = Array.Empty<string>();

    public void Lock()
    {
        if (_isLocked)
        {
            return;
        }

        var warnings = new List<string>();

        try
        {
            _keyboardHook.Install();
        }
        catch (Exception ex)
        {
            warnings.Add($"Tastatur-Sperre (Win-Taste/Alt+Tab) konnte nicht aktiviert werden: {ex.Message}");
        }

        try
        {
            TaskManagerPolicy.Disable();
            _taskManagerPolicyActive = true;
        }
        catch (Exception ex)
        {
            warnings.Add($"Task-Manager-Sperre konnte nicht aktiviert werden (Gruppenrichtlinie/Virenschutz?): {ex.Message}");
        }

        Warnings = warnings;
        _isLocked = true;
    }

    public void Unlock()
    {
        if (!_isLocked)
        {
            return;
        }

        try
        {
            _keyboardHook.Uninstall();
        }
        catch
        {
            // Beim Entsperren nie eine Exception werfen - der PC muss in jedem Fall nutzbar bleiben.
        }

        if (_taskManagerPolicyActive)
        {
            try
            {
                TaskManagerPolicy.Enable();
                _taskManagerPolicyActive = false;
            }
            catch
            {
                // Siehe oben.
            }
        }

        _isLocked = false;
    }

    public void Dispose()
    {
        Unlock();
        _keyboardHook.Dispose();
    }
}
