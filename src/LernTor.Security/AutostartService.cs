using Microsoft.Win32.TaskScheduler;

namespace LernTor.Security;

/// <summary>
/// Registriert LernTor als geplante Aufgabe, die direkt nach dem Windows-Login startet
/// (Soft-Lock-Ansatz: kein Shell-Ersatz, daher bei Absturz automatisch normaler Desktop verfügbar).
/// </summary>
public static class AutostartService
{
    private const string TaskName = "LernTor Autostart";

    public static void Register(string executablePath)
    {
        using var taskService = TaskService.Instance;
        var taskDefinition = taskService.NewTask();
        taskDefinition.RegistrationInfo.Description = "Startet LernTor automatisch nach dem Login des Kindes.";
        taskDefinition.Triggers.Add(new LogonTrigger());
        taskDefinition.Actions.Add(new ExecAction(executablePath));
        taskDefinition.Principal.RunLevel = TaskRunLevel.Highest;
        taskDefinition.Settings.DisallowStartIfOnBatteries = false;
        taskDefinition.Settings.StopIfGoingOnBatteries = false;
        taskDefinition.Settings.ExecutionTimeLimit = TimeSpan.Zero; // keine automatische Beendigung

        taskService.RootFolder.RegisterTaskDefinition(TaskName, taskDefinition);
    }

    public static void Unregister()
    {
        using var taskService = TaskService.Instance;
        taskService.RootFolder.DeleteTask(TaskName, exceptionOnNotExists: false);
    }

    public static bool IsRegistered()
    {
        using var taskService = TaskService.Instance;
        return taskService.RootFolder.Tasks.Exists(TaskName);
    }
}
