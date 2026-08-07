# CLAUDE.md

This file provides guidance to Claude Code (claude.ai/code) when working with code in this repository.

## What this is

LernTor is a Windows kiosk learning app for kids: after Windows login it takes over the screen
(soft kiosk lock) until the child reads news, works through exercises in whichever subjects
parents left enabled, and passes a final quiz (default ≥50%, threshold configurable per profile in
parent settings). Only then is the PC unlocked. Target
audience: German-Turkish kids (~10-15) in Berlin; content follows the Berlin Rahmenlehrplan for
Klasse 6 and 9. Full behavioral spec lives in `README.md`; curriculum topic mapping in
`docs/CURRICULUM.md`; build/install steps in `docs/BUILD.md`; stage/subject wiring in
`docs/FAECHER-SYSTEM.md`; typing trainer details in `docs/TIPPTRAINER.md`; the driving-licence area
(traffic signs, own vector rendering, licensing constraints) in `docs/FUEHRERSCHEIN.md`.

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

# Self-contained release build (single-file, ReadyToRun — see docs/BUILD.md for why PublishTrimmed is deliberately not used)
dotnet publish src/LernTor.App/LernTor.App.csproj --configuration Release --runtime win-x64 --self-contained true --output publish/win-x64 -p:PublishSingleFile=true -p:PublishReadyToRun=true

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

Every subject in the `Subject` enum (`LernTor.Core.Enums.Subject` — currently 15 curriculum
subjects plus `News` and `Tippen`, neither of which has a generator: News questions come from
`LernTor.News` instead, and typing exercises come from the separate `TypingContentProvider`/
`TypingExerciseService`, not `ExerciseGeneratorBase`) is one `ExerciseGeneratorBase` subclass. Each exposes
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
supplies a rolling freshness window of recently-asked prompts; `MasteredPromptRepository` implements
spaced repetition (`SpacedRepetitionSchedule` in Core): a correctly answered prompt is excluded
until it comes due again after growing intervals (stage 1 = 7d, 2 = 30d, 3+ = 90d) — passing a
due review bumps the stage, failing one deletes the mastery so the Fehler-Kartei takes over
(legacy rows with `NextDueAt = NULL` count as due); `ReviewQuestionRepository` is the
"Fehler-Kartei" — prompts answered *incorrectly* are snapshotted (not regenerated, since generated
questions aren't reproducible the next day) and resurface first on later days, marked 🔁, until
answered correctly twice in a row. `MainViewModel` (App) is what assembles and combines these three
sources before calling into `ContentGen`.

`QuizComposer.ComposeFinalQuiz` builds the final quiz dynamically: it excludes generators for
subjects in the caller-supplied `disabledSubjects` set, then divides `targetTotalQuestions` (default
20) across however many subjects remain active, so the quiz doesn't balloon when many subjects are
enabled and doesn't include subjects the child never practiced. News questions get capped at
roughly a third of the target. `MainViewModel` calls this twice per day at most: once for the first
attempt (target 20) and, only if that attempt's score is below the profile's configured threshold,
once more for a retry weighted toward weak subjects (target 15, via `ComposeRetryExercises`).

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
`ActivityLogRepository` take a `profileId`. `AppSettings` (admin password, disabled subjects, LLM
model choice) stays global/shared across profiles; `GradeLevel` and the difficulty thresholds
(`TypingMinAccuracy`, `QuizFirstAttemptThreshold`, `QuizRetryThreshold` — parent-configurable presets,
no rebuild needed to change them) live on `StudentProfile` itself, per-profile, not global.
`StudentProfileRepository.UpdateSettingsAsync` is currently the only way to edit an existing
profile's fields after creation — `GradeLevel`/`Name`/etc. otherwise only get set once at creation.

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
- **`pack://application:,,,/Path` only resolves an embedded `Resource` when the EXECUTING
  assembly (the process's entry assembly) is also the assembly the resource is compiled into.**
  That's true for `LernTor.exe` itself, but not for `LernTor.UiTests`: there the test host is the
  entry assembly and `LernTor.dll` (with the traffic-sign PNGs under
  `Assets/Verkehrszeichen/`, see `TrafficSignImages`) is only referenced. The short form silently
  finds nothing (throws `IOException`, easy to swallow in a catch block) instead of erroring
  loudly — symptom was every sign quietly falling back to its old hand-drawn pictogram, with all
  tests still green, only caught by actually reading the rendered pixels. Fix: always use the
  explicit assembly form, `pack://application:,,,/LernTor;component/Path`, which resolves
  correctly regardless of which assembly is the entry point.
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
- **Never publish with `-p:IncludeNativeLibrariesForSelfExtract=true`.** LLamaSharp resolves
  `llama.dll`/`ggml-*.dll` by *file path relative to the exe*, not through the normal .NET loader.
  With that flag the natives go into the single-file bundle and get self-extracted to a temp folder
  where LLamaSharp's own lookup can't see them — the app starts fine, only the AI chat and the
  teacher import fail with `The type initializer for 'LLama.Native.NativeApi' threw an exception`.
  This bit the family **twice**: the first fix added an MSBuild target setting
  `ExcludeFromSingleFile` on the native files, but the publish command re-enabled the flag on the
  command line, which wins over anything the project file does. The flag is now gone from CI and
  `docs/BUILD.md`, `IncludeNativeLibrariesForSelfExtract` is pinned to `false` in
  `LernTor.App.csproj`, and the CI workflow **verifies after publishing** that `llama.dll` plus at
  least one `ggml-*.dll` really are loose files.
  **Verified layout** (CI log of run `65ac1a6`, LLamaSharp 0.27): the backend ships one folder per
  CPU variant — `runtimes/win-x64/native/{noavx,avx,avx2,avx512}/` each containing `llama.dll`,
  `ggml.dll`, `ggml-base.dll`, `ggml-cpu.dll`, `mtmd.dll`, plus a `runtimes/win-arm64/native/` set.
  They are **not** flattened into the publish root, so any check for them must recurse — a first
  version of the CI check looked only in the root and produced a false failure. Don't "tidy up"
  the `runtimes/` folder next to the exe, and don't ship only the exe: without that folder the AI
  features are dead.
- **Never set a WPF property both as an attribute and as a child element.** Writing
  `<Button Style="{StaticResource X}" …>` *and* `<Button.Style>…</Button.Style>` on the same
  element fails with `MC3024: 'Style' property has already been set and can be set only once` —
  a compile error, so it only surfaces in CI from this SDK-less environment. It happens most often
  when retrofitting a `Style.Trigger` onto a button that already had a `StaticResource` style.
  `scripts/preflight.py` now checks for this (`xaml-doppelter-style`).
- **Repository tests that back onto a real SQLite temp file must wrap the `File.Delete` in their
  `Dispose` in a `try`/`catch`.** `Microsoft.Data.Sqlite` pools connections, so the file handle can
  still be held after the `DbContext` is disposed — `File.Delete` then throws `IOException: The
  process cannot access the file`, and xUnit reports that as a *test failure* even though the test
  itself passed. Only reproduces on the Windows CI runner, never on a quick local reading of the
  code. Existing repository tests (`ReviewQuestionRepositoryTests`, `CustomReadingTextRepositoryTests`)
  show the pattern; the OS cleans the temp directory anyway.
- Kiosk hardening (`KioskLockService.Lock()`) must never let one measure's failure crash the app —
  Group Policy/antivirus can deny the `DisableTaskMgr` registry write on some machines. Each
  measure (keyboard hook, task-manager policy) is attempted independently and failures are
  collected as warnings, not thrown.
- The `WH_KEYBOARD_LL` hook (`KioskKeyboardHook`) that blocks Win/Alt+Tab/Alt+Esc/Ctrl+Esc/Alt+F4
  is not 100% reliable on every machine (timing, Group Policy, edge cases) — kids escaping the
  kiosk via Alt+Tab was a real bug. `MainWindow` therefore runs a second, independent line of
  defense: a `DispatcherTimer` polling `GetForegroundWindow()` every 300ms that reclaims focus
  whenever the foreground window belongs to a **different process** — this catches Alt+Tab, Win+D,
  taskbar clicks, etc. regardless of whether the keyboard hook caught the specific key combo. It
  compares process IDs (not window handles) so the Eltern-Bereich window (same process) is left
  alone. **Neither of those stopped the app from actually closing**, though: Windows 11's Alt+Tab
  switcher renders a close ("X") button directly on each window's thumbnail, letting a child close
  LernTor via a plain `WM_CLOSE` without any key combo or focus change involved — invisible to both
  measures above. `MainWindow.Closing` therefore cancels the close outright whenever
  `KioskLockService.IsLocked` is still true. Every intentional shutdown path (daily unlock,
  Eltern-Bereich-Sofortentsperrung, Werkseinstellungen, Sicherung wiederherstellen) must call
  `KioskLockService.Unlock()` *before* `Application.Current.Shutdown()` — forgetting that on a new
  shutdown path makes `MainWindow.Closing` block it.
- Blocking the bare Windows key (`isWindowsKey` in `KioskKeyboardHook`) does **not** block Win+combo
  shortcuts — a real bug let kids open Task View via Win+Tab and create a brand-new virtual
  desktop (LernTor doesn't exist there → full unrestricted PC access). Swallowing the Win keydown
  message doesn't stop Windows from tracking the key as physically held (`GetAsyncKeyState` still
  reports it down), so the shell recognizes the combo anyway — the hook now explicitly checks
  `winPressed` (like `altPressed`/`ctrlPressed`) and blocks Win+Tab/D/E/R/X/I and
  Win+Ctrl+Left/Right/F4 (virtual-desktop switch/close) individually. As a second, independent
  layer (some Win+combos, especially Task View, are recognized by the shell in ways a low-level
  hook alone can't reliably catch on every Windows build), `KioskLockService` also sets the
  `NoWinKeys` registry policy (`WindowsHotkeyPolicy`, same pattern as `TaskManagerPolicy`/
  `DisableTaskMgr`) for the duration of the lock.
- **`_` as a lambda parameter kills the `_ = SomethingAsync()` fire-and-forget idiom.** This
  codebase writes `_ = SomeAsync()` everywhere for deliberately unawaited tasks. Inside a lambda
  whose parameter is also named `_`, that line stops being a discard and becomes an *assignment to
  the parameter* — `error CS0029: Cannot implicitly convert type 'Task' to 'int'`. Only surfaces in
  CI. Give the unused parameter a real name instead; `scripts/preflight.py` now checks for it
  (`lambda-verwerfen`).
- **Appending a method "to the end of the file" puts it in the LAST class of that file, not
  the one you meant.** `TypingExerciseViewModel.cs` holds three classes; a scripted edit that
  inserted `PauseStage`/`ResumeStage` before the final `}` landed them in `KeyboardKeyViewModel`,
  so the class that *declared* `IPausableStage` never implemented it (`CS0535`). Brace balance
  stays correct, the diff reads fine, and only the compiler notices — eight minutes of CI.
  `scripts/preflight.py` now checks that a class declaring a known interface implements its
  members inside its own brace range (`schnittstelle-fehlt`).
- **`[RelayCommand]` needs `using CommunityToolkit.Mvvm.Input;`, which is a DIFFERENT namespace
  from `[ObservableProperty]`'s `CommunityToolkit.Mvvm.ComponentModel`.** A file that already
  uses `[ObservableProperty]` looks like it has the toolkit imported, so adding the first
  `[RelayCommand]` to it fails with `CS0246` on `RelayCommandAttribute` — which reads like a
  missing package reference rather than a missing using. `scripts/preflight.py` checks it
  (`toolkit-using`).
- **A green CI badge on the last run does not mean YOUR commit was built.** A push to `master`
  once produced no workflow run at all — the workflow stayed `active`, the trigger matched
  (`branches: ["**"]`), and Actions was not out of quota; GitHub simply swallowed the push event.
  Always compare `head_sha` against the commit you pushed, not just the newest run's colour. A
  missing run can be started by hand with `actions_run_trigger`/`run_workflow` on `master`.
- **A lambda inside a `struct` member cannot touch the struct's own fields/properties**
  (`CS1673`). `public IEnumerable<string> CorrectAnswers => CorrectIndices.Select(i => Options[i]);`
  reads perfectly and compiles fine in a `record` (class) — in a `readonly record struct` it is a
  compile error, because the lambda would have to capture `this`. Only surfaces in CI from this
  SDK-less environment; it cost a full round on `TheoryQuestionPresenter.PresentedQuestion`. The
  fix is always the same: copy the members into locals *before* the lambda. `scripts/preflight.py`
  now checks for it (`struct-lambda`).
- **A tuple element named `Rest` is a compile error** (`CS8126: Tuple element name 'Rest' is
  disallowed at any position`). `ValueTuple` uses `Rest` internally for tuples with eight or more
  fields, so the name is reserved *everywhere* — including in a two-element tuple like
  `(DayOfWeek? Tag, string Rest)`. It reads perfectly, and in this German-commented codebase "Rest"
  is also the obvious word for "the remainder of the line", so nothing about it looks wrong.
  Only the compiler notices, and this environment has none — it cost a full CI round on
  `TimetableTextParser.TagAbtrennen`. Same trap for `ToString`/`Equals`/`GetHashCode`/`GetType`
  (inherited from `object`). `scripts/preflight.py` now checks for it (`tupel-feldname`).
- **WPF does not render flag emoji.** A flag like 🇹🇷 is two Regional Indicator code points
  (U+1F1E6–U+1F1FF) that a browser or phone composes into a flag; WPF/Segoe UI Emoji does not
  compose them and draws the two *letters* instead — the timetable tile showed a bare "TR" in
  front of Türkisch, and the news categories had been showing "DE"/"TR" for much longer without
  anyone noticing. Compiles, all tests green, only visible in a screenshot of the running app.
  Use a non-flag emoji for anything language- or country-flavoured.
  `scripts/preflight.py` now checks for it (`flaggen-emoji`) — including inside comments, so
  don't paste a flag into an explanatory comment either.
- **Static field initializers across `partial` class files have no defined order.** Building an
  aggregate field from arrays declared in sibling partial files (`TrafficSignCatalog`) can read
  them before they are populated — the compiler flags it as `CS8604`, a *warning*, so the build
  still passes and the breakage would be a runtime `NullReferenceException` or a silently empty
  catalog. Wrap the aggregate in `Lazy<T>` so it is built on first access, after the static
  constructor has run.
- Enum values serialized via `System.Text.Json` default to numeric encoding — reordering/adding
  enum members then silently reinterprets old saved data. `LernTor.Data.JsonOptions.Default`
  (a shared `JsonSerializerOptions` with `JsonStringEnumConverter`) is used for anything persisting
  `Subject`/enum collections (`DisabledSubjects`, `CompletedExerciseSubjects`, etc.) for this reason.
- Global exception handlers in `App.xaml.cs` (`DispatcherUnhandledException`,
  `AppDomain.UnhandledException`, `TaskScheduler.UnobservedTaskException`) funnel into one
  re-entrancy-guarded `HandleFatalException`: dev mode (`LERNTOR_SKIP_LOCK=1`/debugger) shows a
  `MessageBox` — this is how prior startup crashes were actually diagnosed — while kiosk mode logs
  silently and auto-restarts, bounded by `CrashRestartGuard` (Core; max 3 restarts per 10 min,
  persisted in `%LOCALAPPDATA%\LernTor\crash-restarts.txt`) so a crash-on-startup bug degrades to
  the soft-lock desktop instead of an infinite restart loop. Unobserved task exceptions are
  logged + `SetObserved()` only — never treated as fatal, never trigger a restart.
- LLamaSharp's `StatelessExecutor.InferAsync` does not stop on its own when a prompt ends with an
  open turn like `"Assistent:"` — without `InferenceParams.AntiPrompts` stop sequences, it keeps
  completing the text and hallucinates the rest of the conversation (both the child's next messages
  and further AI replies) instead of returning after one answer. `LocalLlmHomeworkHelpChatService`
  sets `AntiPrompts = ["\nKind:", "Kind:", "\nAssistent:"]` and additionally trims the raw output at
  the first such marker as a defensive backstop.
