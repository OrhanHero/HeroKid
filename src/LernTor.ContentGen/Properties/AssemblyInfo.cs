using System.Runtime.CompilerServices;

// Der Antwort-Parser des Lehrer-Imports (LlmResponseParser) ist bewusst internal - er gehört zur
// LLM-Anbindung, nicht zur öffentlichen Schnittstelle. Getestet werden muss er trotzdem: die
// Reparatur abgeschnittener Antworten lässt sich sonst nur über einen echten Modelllauf prüfen.
[assembly: InternalsVisibleTo("LernTor.Tests")]
