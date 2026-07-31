using Xunit;

// Diese Testklassen duerfen NICHT parallel laufen.
//
// xUnit behandelt jede Testklasse als eigene Kollektion und fuehrt Kollektionen standardmaessig
// parallel aus. Hier teilen sich aber alle Tests zwei Dinge, die es nur EINMAL pro Prozess bzw.
// pro Benutzerkonto gibt:
//
//   - das WPF-Application-Objekt: XamlLoadTests baut es auf, StartupSmokeTests startet zusaetzlich
//     die echte LernTor.exe. Faehrt eines davon herunter, scheitern die anderen mit
//     "The Application object is being shut down".
//   - die Datenbank unter %LOCALAPPDATA%\LernTor: rufen zwei Instanzen gleichzeitig
//     EnsureCreatedAsync auf einer frischen Datei auf, sehen beide ein leeres Schema, beide legen
//     die Tabellen an - und die zweite scheitert mit "table ActivityLog already exists".
//
// Genau das ist auf dem CI-Runner passiert: 14 von 19 Tests rot, obwohl an keinem davon etwas
// kaputt war. Ein Fehlschlag, der von der Ausfuehrungsreihenfolge abhaengt, ist schlimmer als
// gar kein Test - er kostet jedes Mal eine Runde Ursachensuche am falschen Ende.
[assembly: CollectionBehavior(DisableTestParallelization = true)]
