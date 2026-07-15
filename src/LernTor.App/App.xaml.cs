using System.Net.Http;
using System.Threading.Tasks;
using System.Windows;
using LernTor.App.Services;
using LernTor.App.ViewModels;
using LernTor.App.Views;
using LernTor.ContentGen;
using LernTor.ContentGen.HomeworkChat;
using LernTor.ContentGen.Llm;
using LernTor.ContentGen.TeacherImport;
using LernTor.Core.Logging;
using LernTor.Core.Services;
using LernTor.Data;
using LernTor.Data.Repositories;
using LernTor.News;
using LernTor.Security;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace LernTor.App;

public partial class App : Application
{
    private IHost? _host;
    private bool _isFatallyShuttingDown;

    public IServiceProvider Services => _host!.Services;

    protected override async void OnStartup(StartupEventArgs e)
    {
        base.OnStartup(e);

        // Ohne diese Handler stirbt eine WPF-App bei einer unbehandelten Exception beim Start
        // kommentarlos (weißer Bildschirm, dann Prozessende) - genau das soll hiermit sichtbar werden.
        // _isFatallyShuttingDown verhindert, dass währenddessen jeder weitere Layout-/Render-Durchlauf
        // dieselbe Exception erneut auslöst und ein ganzer Stapel von Dialogen aufpoppt.
        DispatcherUnhandledException += (_, args) =>
        {
            args.Handled = true;
            if (_isFatallyShuttingDown)
            {
                return;
            }

            _isFatallyShuttingDown = true;
            // Zusätzlich zur MessageBox in die Log-Datei: der Dialog ist nach dem Wegklicken
            // verloren, die Datei nicht (siehe AppLog / Fehlerprotokoll im Eltern-Bereich).
            AppLog.Error("App", "Unbehandelter UI-Fehler, App wird beendet", args.Exception);
            MessageBox.Show(
                $"LernTor ist auf einen unerwarteten Fehler gestoßen und muss beendet werden:\n\n{args.Exception}",
                "LernTor - Fehler", MessageBoxButton.OK, MessageBoxImage.Error);
            Shutdown(1);
        };
        AppDomain.CurrentDomain.UnhandledException += (_, args) =>
        {
            if (_isFatallyShuttingDown)
            {
                return;
            }

            _isFatallyShuttingDown = true;
            AppLog.Error("App", "Unbehandelter Hintergrund-Fehler, App wird beendet", args.ExceptionObject as Exception);
            MessageBox.Show(
                $"LernTor ist auf einen unerwarteten Fehler gestoßen und muss beendet werden:\n\n{args.ExceptionObject}",
                "LernTor - Fehler", MessageBoxButton.OK, MessageBoxImage.Error);
        };

        try
        {
            AppLog.Info("App", $"LernTor startet (Version {typeof(App).Assembly.GetName().Version})");
            await StartUpInternalAsync(e);
        }
        catch (Exception ex)
        {
            AppLog.Error("App", "Start fehlgeschlagen", ex);
            MessageBox.Show(
                $"LernTor konnte nicht gestartet werden:\n\n{ex}",
                "LernTor - Startfehler", MessageBoxButton.OK, MessageBoxImage.Error);
            Shutdown(1);
        }
    }

    private async Task StartUpInternalAsync(StartupEventArgs e)
    {
        // Wird vom Installer (Inno Setup) bzw. Deinstaller aufgerufen, um den Autostart-Task
        // zu registrieren/entfernen, ohne die Kiosk-UI zu starten.
        if (e.Args.Contains("--register-autostart"))
        {
            AutostartService.Register(Environment.ProcessPath ?? Environment.GetCommandLineArgs()[0]);
            Shutdown();
            return;
        }

        if (e.Args.Contains("--unregister-autostart"))
        {
            AutostartService.Unregister();
            Shutdown();
            return;
        }

        _host = Host.CreateDefaultBuilder()
            .ConfigureServices((_, services) =>
            {
                // Singleton statt Scoped: einfache Single-User-Desktop-App ohne parallele Requests,
                // ViewModels (Singletons) greifen direkt auf die Repositories zu.
                services.AddDbContext<LernTorDbContext>(options =>
                    options.UseSqlite($"Data Source={LernTorDbContext.GetDefaultDbPath()}"),
                    ServiceLifetime.Singleton);

                services.AddSingleton<HttpClient>();

                // Wetter-Widget im News-Bereich (Open-Meteo, kostenlos/ohne Schlüssel; bei
                // Fehlschlag bleibt das Widget einfach ausgeblendet).
                services.AddSingleton<WeatherService>();
                services.AddSingleton<ITextSimplifier, RuleBasedTextSimplifier>();
                services.AddSingleton<IComprehensionQuestionGenerator, HeuristicComprehensionQuestionGenerator>();
                services.AddSingleton<RssNewsService>();

                services.AddSingleton<QuizComposer>();
                services.AddSingleton<ProgressGateService>();
                services.AddSingleton<ScoringService>();

                services.AddSingleton<ProgressRepository>();
                services.AddSingleton<ActivityLogRepository>();
                services.AddSingleton<SettingsRepository>();
                services.AddSingleton<StudentProfileRepository>();
                services.AddSingleton<DatabaseMaintenanceRepository>();
                services.AddSingleton<CustomQuestionRepository>();
                services.AddSingleton<ReviewQuestionRepository>();
                services.AddSingleton<MasteredPromptRepository>();
                services.AddSingleton<ArchivedArticleRepository>();
                services.AddSingleton<RewardRepository>();
                services.AddSingleton<TypingProgressRepository>();
                services.AddSingleton<TypingExerciseService>();

                services.AddSingleton<KioskLockService>();

                // Gemeinsame LLM-Infrastruktur (siehe README): komplett lokal, keine Cloud-Anbindung.
                // LocalLlmOptions wird von ParentSettingsViewModel beim Laden der Einstellungen befüllt, da
                // die DI-Container schon vor dem Laden der AppSettings aus der DB aufgebaut werden.
                // LocalLlmModelHost lädt das Modell nur einmal (lädt bei Bedarf sogar automatisch ein
                // Standardmodell herunter) und wird sowohl vom Lehrer-Import als auch vom KI-Lernchat
                // genutzt, damit beide Features es nicht unabhängig voneinander doppelt im RAM halten.
                services.AddSingleton<LocalLlmOptions>();
                services.AddSingleton<LocalLlmModelHost>();

                // Automatisches Einlesen von Lehrer-Unterlagen.
                services.AddSingleton<ITeacherDocumentTextExtractor, PdfPigTextExtractor>();
                services.AddSingleton<ITeacherDocumentTextExtractor, OpenXmlWordTextExtractor>();
                services.AddSingleton<ITeacherQuestionSuggester, LocalLlmQuestionSuggester>();
                services.AddSingleton<TeacherDocumentImportService>();

                // KI-Lernchat für Kinder (siehe README).
                services.AddSingleton<IHomeworkHelpChatService, LocalLlmHomeworkHelpChatService>();

                // Vorlesefunktion im Lesen-Abschnitt (komplett offline): natürliche Piper-Stimmen,
                // sofern im Eltern-Bereich heruntergeladen, sonst Windows-SAPI als Rückfall.
                // Singleton, damit die Sprachausgabe nur einmal initialisiert wird; Host-Dispose
                // räumt sie beim Beenden ab.
                services.AddSingleton<PiperTtsEngine>();
                services.AddSingleton<TextToSpeechService>();

                services.AddSingleton<MainViewModel>();
                services.AddSingleton<MainWindow>();

                services.AddTransient<ParentSettingsViewModel>();
                services.AddTransient<ParentSettingsWindow>();
            })
            .Build();

        await _host.StartAsync();

        using (var scope = _host.Services.CreateScope())
        {
            var db = scope.ServiceProvider.GetRequiredService<LernTorDbContext>();
            // Frische Installation: EnsureCreated legt das komplette Schema an. Bestehende DB:
            // EnsureCreated tut nichts - der SchemaUpdater ergänzt dann fehlende Tabellen/Spalten,
            // damit Profile & Fortschritte App-Updates überleben (kein DB-Löschen mehr nötig).
            await db.Database.EnsureCreatedAsync();
            SqliteSchemaUpdater.Update(db);
        }

        var mainWindow = _host.Services.GetRequiredService<MainWindow>();
        mainWindow.Show();

        var shouldSkipKioskLock =
            Environment.GetEnvironmentVariable("LERNTOR_SKIP_LOCK") == "1" ||
            System.Diagnostics.Debugger.IsAttached;

        if (!shouldSkipKioskLock)
        {
            _host.Services.GetRequiredService<KioskLockService>().Lock();
        }

        var mainViewModel = _host.Services.GetRequiredService<MainViewModel>();
        await mainViewModel.InitializeAsync();
    }

    protected override async void OnExit(ExitEventArgs e)
    {
        if (_host is not null)
        {
            _host.Services.GetService<KioskLockService>()?.Unlock();
            await _host.StopAsync();
            _host.Dispose();
        }

        base.OnExit(e);
    }
}
