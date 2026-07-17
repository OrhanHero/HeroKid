# CLAUDE.md

This file provides guidance to Claude Code (claude.ai/code) when working with code in this repository.

## What this is

LernTor is a Windows kiosk learning app for kids: after Windows login it takes over the screen
(soft kiosk lock) until the child reads news, works through exercises in whichever subjects
parents left enabled, and passes a final quiz (default ≥50%, threshold configurable per profile in
parent settings). Only then is the PC unlocked. Target
audience: German-Turkish kids (~10-15) in Berlin; content follows the Berlin Rahmenlehrplan for
Klasse 6 and 9. Full behavioral spec lives in `README.md`; curriculum topic mapping in
`docs/CURRICULUM.md`; build/install steps in `docs/BUILD.md`.

**Environment constraint**: this repo is often developed from a Linux sandbox with no .NET SDK
and no Windows, so nothing here can actually be compiled or run locally in that environment.
`LernTor.App` and `LernTor.Security` require Windows (WPF, Win32 P/Invoke) and won't build on
Linux/macOS even with the SDK installed. **The GitHub Actions workflow
(`.github/workflows/build.yml`, runs on `windows-latest`) is the real build/test verification** —
after pushing, check the workflow run rather than assuming local compilation succeeded. Several
real bugs in this codebase's history were only caught this way (see "Hard-won gotchas" below).

## Commands

```powershell
# Restore + build everything
dotnet restore LernTor.sln
dotnet build LernTor.sln

# Run a single test project (all tests)
dotnet test tests/LernTor.Tests/LernTor.Tests.csproj

# Run one test by name/filter
dotnet test tests/LernTor.Tests/LernTor.Tests.csproj --filter "FullyQualifiedName~MathGeneratorTests"

# Run the app WITHOUT triggering the kiosk lock (essential for dev — otherwise it locks the dev machine)
$env:LERNTOR_SKIP_LOCK = "1"
dotnet run --project src/LernTor.App
# (the lock is also auto-skipped when a debugger is attached, e.g. F5 in Visual Studio)

# Self-contained release build
dotnet publish src/LernTor.App/LernTor.App.csproj --configuration Release --runtime win-x64 --self-contained true --output publish/win-x64

# Installer (requires Inno Setup's iscc.exe)
iscc src\LernTor.Installer\setup.iss
```

Local SQLite DB lives at `%LOCALAPPDATA%\LernTor\lerntor.db`. The app uses EF Core's
`EnsureCreated()` plus `SqliteSchemaUpdater.Update()` on startup — **additive** schema changes
(new tables, new columns, new indexes) are applied automatically to an existing DB, derived at
runtime from `GenerateCreateScript()` (real EF migrations would need the `dotnet ef` tooling on
every schema change, which this SDK-less dev environment cannot run). Deleting the DB manually is
only needed for the rare non-additive change: removed/renamed columns, or reinterpreting existing
values (e.g. reordering an enum that was persisted numerically — persist enums as strings to
avoid this, see `JsonOptions.Default`).

## Architecture

Seven projects, dependency graph flows one direction (`Core` has no dependencies, `App` depends on
everything):

```
LernTor.Core         net8.0, no deps       — models, enums, ProgressGateService, ScoringService
LernTor.ContentGen    → Core               — rule-based per-subject question generators + QuizComposer
LernTor.News          → Core               — RSS ingestion, simplification, comprehension questions
LernTor.Data          → Core               — EF Core/SQLite repositories
LernTor.Security      → Core (net8.0-windows) — kiosk keyboard hook, task-manager policy, autostart, admin auth
LernTor.App           → all of the above (net8.0-windows, WPF) — the actual UI
LernTor.Installer                          — Inno Setup script + PowerShell autostart helper (not a .csproj)
```

### Content generators (`LernTor.ContentGen/Generators`)

Every subject in the `Subject` enum (`LernTor.Core.Enums.Subject` — currently 14 curriculum
subjects plus `News`, which has no generator) is one `ExerciseGeneratorBase` subclass. Each exposes
a `TopicsByGrade: IReadOnlyDictionary<GradeLevel, IReadOnlyList<TopicFactory>>` — a topic is just a
`Random -> QuizQuestion` delegate, backed by a curated fixed array of ~20
question/answer/explanation tuples per topic (Math is the exception: it generates fresh numbers
instead of picking from a fixed pool; ~20 is the target pool size everywhere else so a profile
doesn't exhaust a topic quickly once "never repeat a correctly-answered prompt" — see below —
kicks in). `ExerciseGeneratorBase.Generate()` handles de-duplication itself: it retries on a
colliding `Prompt` text (bounded attempts) before falling back to allowing repeats if a topic's
pool is smaller than the requested count. Adding a new topic to an existing subject only requires a
new private method + an entry in `TopicsByGrade` — no changes needed anywhere else (`QuizComposer`,
ViewModels, tests) since they all iterate generically. Adding a whole new *subject* additionally
touches `Subject`/`LearningStage` enums, `LearningStageSubjects.Map`,
`ProgressGateService.SequentialOrder`, the default generator list in `QuizComposer()`,
`SubjectToTitleConverter`, `ParentSettingsViewModel`'s toggle list, and DE/TR `Translations` — see
any `Add <Subject> as a new subject` commit for the exact file list.

Since curated pools are finite, three repositories in `LernTor.Data` cooperate to keep questions
feeling fresh across sessions, all funneled into the `recentlySeenPrompts` parameter threaded
through `ExerciseGeneratorBase.Generate()`/`QuizComposer`: `ActivityLogRepository.GetRecentPromptsAsync`
supplies a rolling freshness window of recently-asked prompts; `MasteredPromptRepository` tracks
prompts a profile has *ever* answered correctly and excludes them permanently (falls back to
allowing repeats only once a topic's entire pool is mastered); `ReviewQuestionRepository` is the
"Fehler-Kartei" — prompts answered *incorrectly* are snapshotted (not regenerated, since generated
questions aren't reproducible the next day) and resurface first on later days, marked 🔁, until
answered correctly twice in a row. `MainViewModel` (App) is what assembles and combines these three
sources before calling into `ContentGen`.

`QuizComposer.ComposeFinalQuiz` builds the final quiz dynamically: it excludes generators for
subjects in the caller-supplied `disabledSubjects` set, then divides `targetTotalQuestions` (default
22) across however many subjects remain active, so the quiz doesn't balloon when many subjects are
enabled and doesn't include subjects the child never practiced. News questions get capped at
roughly a third of the target.

### Stage navigation (`LernTor.Core.Services.LearningStageSubjects`)

`LearningStage` (an ordered enum: Willkommen → News → one entry per subject → Abschlussquiz →
Freigeschaltet) and `Subject` are two separate enums because News/Abschlussquiz/meta-stages have no
associated subject. `LearningStageSubjects.Map` is the **single source of truth** mapping
subject-stages to subjects — both `ProgressGateService` (Core, decides what's unlocked) and
`MainViewModel` (App, decides what to render) read from it. If you add a subject, update this map,
`ProgressGateService.SequentialOrder`, and the default generator list in `QuizComposer()` — nothing
else needs to know about the new stage.

### App layer (WPF, MVVM)

`MainViewModel` is the navigation host: it owns `CurrentViewModel` (bound via
`MainWindow`'s `ContentControl`, resolved through per-type `DataTemplate`s) and swaps it based on
`LearningStage`. Child ViewModels (`ProfileSelectionViewModel`, `WelcomeViewModel`, `NewsViewModel`,
`ExerciseViewModel`, `FinalQuizViewModel`, `ResultViewModel`) are plain objects constructed with
callback delegates (not a mediator/messenger) — e.g. `ExerciseViewModel` takes an
`onSubjectCompleted` action and calls it when done. `QuestionAnswerViewModel` is the one shared
"answer this question" component reused by News, Exercise, and FinalQuiz views via
`Controls/QuestionCard.xaml`.

Everything is scoped per `StudentProfile` (multiple kids, one PC): `ProgressRepository`,
`ActivityLogRepository` take a `profileId`. `AppSettings` (admin password, disabled subjects, time
limit) stays global/shared across profiles; grade level (`GradeLevel`) is per-profile, not global.

Localization (`LernTor.App.Localization.LocalizationService`) is a hand-rolled singleton with a
string indexer over `Translations.Map` (DE/TR), bound in XAML via
`{Binding Source={x:Static loc:LocalizationService.Instance}, Path=[Some_Key]}`. Switching
`CurrentLanguage` raises `PropertyChanged("Item[]")` so every indexer binding re-evaluates.

### Local LLM (`LernTor.ContentGen/Llm`, `/HomeworkChat`, `/TeacherImport`)

LernTor's only AI is a locally-loaded GGUF model via [LLamaSharp](https://github.com/SciSharp/LLamaSharp)
(CPU-only backend, no cloud calls anywhere) — `LocalLlmModelHost` loads it once and keeps it in
memory. Two unrelated features share this one host: `LocalLlmQuestionSuggester` (turns
teacher-uploaded PDF/Word documents, extracted via `PdfPigTextExtractor`/`OpenXmlWordTextExtractor`,
into draft `CustomQuestion`s a parent must approve) and `LocalLlmHomeworkHelpChatService` (the
kid-facing "🤖 KI fragen" chat in `QuestionCard.xaml`, shared by News/Exercise/FinalQuiz). Model
files are chosen from `LocalLlmModelCatalog` and auto-downloaded to `%LOCALAPPDATA%\LernTor\models\`
on first use via a dedicated `HttpClient` with no timeout (the shared app `HttpClient`'s default
100s timeout previously aborted multi-GB downloads mid-stream — a real bug, not hypothetical).

## Hard-won gotchas (don't reintroduce these)

- **`net8.0-windows` + `UseWPF=true` does NOT get the same implicit global usings as plain
  `net8.0`.** Plain SDK projects (Core/ContentGen/News/Data) get `System.Net.Http` for free; the
  WPF project does not — add `using System.Net.Http;` explicitly wherever `HttpClient` is used in
  `LernTor.App`. This only surfaced as a CI compile error, not locally.
- **`StaticResource` cannot resolve across sibling `ResourceDictionary` files merged at a common
  parent.** `Styles.xaml` referencing brushes from `Colors.xaml` only works if `Styles.xaml` merges
  `Colors.xaml` itself (`ResourceDictionary.MergedDictionaries` inside `Styles.xaml`) — being merged
  together only in `App.xaml` is not enough. This compiles fine (resolution happens at XAML *load*
  time) but throws `XamlParseException` at runtime before any window renders — symptom is a blank
  white window that immediately closes with no visible error.
- **`Run.Text` has `BindsTwoWayByDefault`** (unlike `TextBlock.Text`/`Button.Content`). Any
  `<Run Text="{Binding SomeReadOnlyProperty}" />` without explicit `Mode=OneWay` throws
  `XamlParseException` → `InvalidOperationException` the moment that page renders.
- **EF Core's Sqlite provider cannot translate `OrderBy`/`OrderByDescending` on a `DateTimeOffset`
  column** (`NotSupportedException` at query execution, not at compile time). Fetch rows first
  (`ToListAsync()`), then sort in memory.
- Kiosk hardening (`KioskLockService.Lock()`) must never let one measure's failure crash the app —
  Group Policy/antivirus can deny the `DisableTaskMgr` registry write on some machines. Each
  measure (keyboard hook, task-manager policy) is attempted independently and failures are
  collected as warnings, not thrown.
- Enum values serialized via `System.Text.Json` default to numeric encoding — reordering/adding
  enum members then silently reinterprets old saved data. `LernTor.Data.JsonOptions.Default`
  (a shared `JsonSerializerOptions` with `JsonStringEnumConverter`) is used for anything persisting
  `Subject`/enum collections (`DisabledSubjects`, `CompletedExerciseSubjects`, etc.) for this reason.
- A global `DispatcherUnhandledException`/`AppDomain.UnhandledException` handler in `App.xaml.cs`
  shows a `MessageBox` and guards against re-entrancy (a fatal error during shutdown must not spawn
  a stack of duplicate dialogs) — this is how prior startup crashes were actually diagnosed.
- LLamaSharp's `StatelessExecutor.InferAsync` does not stop on its own when a prompt ends with an
  open turn like `"Assistent:"` — without `InferenceParams.AntiPrompts` stop sequences, it keeps
  completing the text and hallucinates the rest of the conversation (both the child's next messages
  and further AI replies) instead of returning after one answer. `LocalLlmHomeworkHelpChatService`
  sets `AntiPrompts = ["\nKind:", "Kind:", "\nAssistent:"]` and additionally trims the raw output at
  the first such marker as a defensive backstop.
