using System.Runtime.CompilerServices;

// SubjectTimeAnalyzer.Classify ist bewusst internal - die Einordnungsregel gehört zur
// Auswertung, nicht zur öffentlichen Schnittstelle. Direkt testbar muss sie trotzdem sein:
// über Analyze() liesse sich die Mindestmenge-Grenze nur mit kuenstlich gebauten Datensaetzen
// pruefen, und die Grenzfaelle (4 vs. 5 Messungen) waeren dort nicht mehr erkennbar.
[assembly: InternalsVisibleTo("LernTor.Tests")]
