using System.Collections.ObjectModel;
using System.Globalization;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using LernTor.Core.Models;
using LernTor.Core.Services;

namespace LernTor.App.ViewModels;

/// <summary>
/// Stundenplan-Verwaltung im Eltern-Bereich - je Profil ein Raster Montag bis Freitag mit den
/// Uhrzeiten der jeweiligen Schule.
///
/// <para><b>Warum ein Eingaberaster und kein PDF-Importeur:</b> die Stundenpläne der beiden
/// Schulen sehen völlig verschieden aus - der eine ist ein eingescannter Untis-Ausdruck, dessen
/// Textebene aus OCR stammt und sichtbare Lesefehler enthält, der andere eine Word-Tabelle mit
/// umbrochenen Zellen. Ein Importeur, der beide Formen selbst zu deuten versucht, müsste bei
/// jedem neuen Plan neu raten; ein still falsch übernommener Stundenplan ist schlechter als gar
/// keiner, weil niemand ihn nachprüft. Das Raster wird einmal je Halbjahr ausgefüllt und ist
/// danach nachweislich richtig.</para>
///
/// <para>Wer lieber schreibt als klickt, kann den Plan als Text einlesen
/// (<see cref="ApplyTimetableTextCommand"/>) - der füllt aber ebenfalls nur das Raster; erst
/// „Stundenplan speichern“ schreibt ihn weg. Nichts wird ungesehen übernommen.</para>
///
/// <para>Eigene Datei, kein Anhängen ans Ende der großen ViewModel-Datei: dort wäre der Code in
/// der letzten Klasse gelandet, nicht in der gemeinten (siehe CLAUDE.md).</para>
/// </summary>
public sealed partial class ParentSettingsViewModel
{
    /// <summary>Die zehn Zeilen des Rasters. Immer alle - eine Zeile, die erst erscheint, wenn
    /// man sie braucht, sucht man beim Eintragen der neunten Stunde vergeblich. Leere Zeilen
    /// fallen beim Speichern von selbst weg.</summary>
    public ObservableCollection<TimetableRowViewModel> TimetableRows { get; } = new();

    [ObservableProperty]
    private string timetableStatus = string.Empty;

    /// <summary>Meldungen aus dem Text-Einlesen: was nicht gedeutet werden konnte, steht hier im
    /// Klartext. Verschluckt wird nichts - eine übersprungene Zeile wäre eine Schulstunde, die im
    /// Plan des Kindes fehlt.</summary>
    [ObservableProperty]
    private string timetableWarnings = string.Empty;

    public bool HasTimetableWarnings => TimetableWarnings.Length > 0;

    partial void OnTimetableWarningsChanged(string value) => OnPropertyChanged(nameof(HasTimetableWarnings));

    /// <summary>Eingabefeld für den getippten Plan (siehe <see cref="TimetableTextParser"/>).</summary>
    [ObservableProperty]
    private string timetableText = string.Empty;

    /// <summary>Beispiel und Anleitung direkt am Feld - eine Formatbeschreibung, die man
    /// woanders nachlesen muss, liest niemand nach.</summary>
    public string TimetableTextHint =>
        "Eine Zeile je Wochentag, z. B.:\n" +
        "Mo: 1 Deutsch, 2 Mathe, 3 Sport (TH1), 6 Englisch\n" +
        "Di: 1 Musik, 2 Deutsch, 4 GeWi, 6 NaWi\n\n" +
        "Der Raum darf in Klammern dahinter stehen. „Einlesen“ füllt nur das Raster darüber – " +
        "gespeichert wird erst mit „Stundenplan speichern“.";

    private void EnsureTimetableRows()
    {
        if (TimetableRows.Count == Timetable.MaxPeriod)
        {
            return;
        }

        TimetableRows.Clear();
        for (var stunde = 1; stunde <= Timetable.MaxPeriod; stunde++)
        {
            TimetableRows.Add(new TimetableRowViewModel(stunde));
        }
    }

    /// <summary>Lädt den Plan des gewählten Profils ins Raster. Ohne Profil bleibt es leer,
    /// aber vorhanden - ein Raster, das erst nach einer Profilwahl erscheint, sieht aus wie ein
    /// Fehler.</summary>
    private async Task ReloadTimetableAsync()
    {
        EnsureTimetableRows();
        TimetableStatus = string.Empty;
        TimetableWarnings = string.Empty;

        foreach (var zeile in TimetableRows)
        {
            zeile.ClearSubjects();
        }

        var profil = SelectedProfile;
        var plan = profil is null
            ? Timetable.Empty
            : await _timetableRepo.GetForProfileAsync(profil.Id);

        foreach (var zeile in TimetableRows)
        {
            var raster = plan.PeriodOf(zeile.Period);
            zeile.StartText = raster?.Start.ToString("HH\\:mm", CultureInfo.InvariantCulture) ?? string.Empty;
            zeile.EndText = raster?.End.ToString("HH\\:mm", CultureInfo.InvariantCulture) ?? string.Empty;
        }

        foreach (var stunde in plan.Lessons)
        {
            var zeile = TimetableRows.FirstOrDefault(z => z.Period == stunde.Period);
            zeile?.SetText(stunde.Day, ZelleSchreiben(stunde));
        }
    }

    /// <summary>Wie eine Stunde im Raster dasteht: "Mathe" bzw. "Mathe (A012a)" - dieselbe
    /// Schreibweise, die beim Speichern wieder auseinandergenommen wird.</summary>
    private static string ZelleSchreiben(TimetableLesson stunde) =>
        string.IsNullOrWhiteSpace(stunde.Room) ? stunde.Subject : $"{stunde.Subject} ({stunde.Room})";

    [RelayCommand]
    private async Task SaveTimetableAsync()
    {
        var profil = SelectedProfile;
        if (profil is null)
        {
            TimetableStatus = "Bitte zuerst oben ein Profil auswählen.";
            return;
        }

        var raster = new List<TimetablePeriod>();
        var stunden = new List<TimetableLesson>();
        var unlesbareZeiten = new List<string>();

        foreach (var zeile in TimetableRows)
        {
            var von = ZeitLesen(zeile.StartText);
            var bis = ZeitLesen(zeile.EndText);

            if (von is not null && bis is not null)
            {
                raster.Add(new TimetablePeriod(zeile.Period, von.Value, bis.Value));
            }
            else if (!string.IsNullOrWhiteSpace(zeile.StartText) || !string.IsNullOrWhiteSpace(zeile.EndText))
            {
                // Eine halb ausgefuellte oder unlesbare Zeit wird gemeldet, nicht geraten: eine
                // erfundene Uhrzeit im Stundenplan eines Kindes ist schlimmer als eine fehlende.
                unlesbareZeiten.Add($"{zeile.Period}. Stunde");
            }

            foreach (var tag in Timetable.SchoolDays)
            {
                var (fach, raum) = TimetableTextParser.SplitRoom(zeile.TextFor(tag));
                if (fach.Length == 0)
                {
                    continue;
                }

                stunden.Add(new TimetableLesson(tag, zeile.Period, fach, null, raum));
            }
        }

        await _timetableRepo.ReplaceAsync(profil.Id, raster, stunden);

        TimetableStatus = $"Gespeichert: {stunden.Count} Stunden für {profil.Name}.";
        TimetableWarnings = unlesbareZeiten.Count == 0
            ? string.Empty
            : "Uhrzeit nicht gespeichert (erwartet wird „8:00“): " + string.Join(", ", unlesbareZeiten);
    }

    /// <summary>Leert die Fächer im Raster - die Uhrzeiten bleiben stehen, weil sich das
    /// Zeitraster einer Schule mit einem neuen Stundenplan nicht ändert. Gespeichert wird erst
    /// mit „Stundenplan speichern“, ein Fehlgriff ist also folgenlos.</summary>
    [RelayCommand]
    private void ClearTimetableSubjects()
    {
        EnsureTimetableRows();
        foreach (var zeile in TimetableRows)
        {
            zeile.ClearSubjects();
        }

        TimetableStatus = "Raster geleert – noch nicht gespeichert.";
        TimetableWarnings = string.Empty;
    }

    /// <summary>Setzt die Uhrzeiten auf das Ausgangsraster zurück - als Startpunkt, wenn eine
    /// Schule ihre Zeiten ändert und man nicht bei leeren Feldern anfangen will.</summary>
    [RelayCommand]
    private void ResetTimetablePeriods()
    {
        EnsureTimetableRows();
        foreach (var zeile in TimetableRows)
        {
            var vorgabe = Timetable.DefaultPeriods.FirstOrDefault(p => p.Number == zeile.Period);
            zeile.StartText = vorgabe?.Start.ToString("HH\\:mm", CultureInfo.InvariantCulture) ?? string.Empty;
            zeile.EndText = vorgabe?.End.ToString("HH\\:mm", CultureInfo.InvariantCulture) ?? string.Empty;
        }

        TimetableStatus = "Zeiten auf die Vorgabe gesetzt – bitte an die Schule anpassen und speichern.";
    }

    /// <summary>
    /// Liest den getippten Plan ins Raster. Bewusst NUR ins Raster: was eingelesen wurde, steht
    /// danach zum Nachsehen da und wird erst mit „Stundenplan speichern“ übernommen.
    /// </summary>
    [RelayCommand]
    private void ApplyTimetableText()
    {
        EnsureTimetableRows();

        var ergebnis = TimetableTextParser.Parse(TimetableText);

        if (!ergebnis.HasLessons && !ergebnis.HasWarnings)
        {
            TimetableStatus = "Nichts zum Einlesen gefunden.";
            TimetableWarnings = string.Empty;
            return;
        }

        if (ergebnis.HasLessons)
        {
            foreach (var zeile in TimetableRows)
            {
                zeile.ClearSubjects();
            }

            foreach (var stunde in ergebnis.Lessons)
            {
                var zeile = TimetableRows.FirstOrDefault(z => z.Period == stunde.Period);
                zeile?.SetText(stunde.Day, ZelleSchreiben(stunde));
            }
        }

        TimetableStatus = $"{ergebnis.Lessons.Count} Stunden ins Raster übernommen – noch nicht gespeichert.";
        TimetableWarnings = ergebnis.HasWarnings ? string.Join("\n", ergebnis.Warnings) : string.Empty;
    }

    /// <summary>"8:00", "08:00", "8.00" - alle drei sind dieselbe Uhrzeit und werden auch so
    /// gelesen. Alles andere gilt als unlesbar und wird gemeldet.</summary>
    private static TimeOnly? ZeitLesen(string? text)
    {
        var roh = text?.Trim().Replace('.', ':');
        if (string.IsNullOrEmpty(roh))
        {
            return null;
        }

        return TimeOnly.TryParseExact(roh, "H:mm", CultureInfo.InvariantCulture, DateTimeStyles.None, out var zeit) ||
               TimeOnly.TryParseExact(roh, "HH:mm", CultureInfo.InvariantCulture, DateTimeStyles.None, out zeit)
            ? zeit
            : null;
    }
}
