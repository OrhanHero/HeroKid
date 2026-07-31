namespace LernTor.Core.Models;

/// <summary>
/// Wer einen Eintrag angelegt hat - gilt für Klausurtermine ebenso wie für Hausaufgaben.
///
/// <para>Die Unterscheidung ist keine Buchhaltung, sondern eine Zugriffsregel: Kinder dürfen
/// eigene Einträge ändern und löschen, Einträge der Eltern nur ansehen. Sonst wäre "Eintrag
/// löschen" der bequeme Weg, dem Lernen zu entgehen. Durchgesetzt wird sie in den Repositories
/// (<c>DeleteAsChildAsync</c>/<c>UpdateAsChildAsync</c>), nicht nur an der Oberfläche - ein
/// ausgeblendeter Knopf ist keine Zugriffskontrolle.</para>
///
/// <para>Wird als STRING gespeichert, ein Umsortieren des Enums deutet bestehende Einträge also
/// nicht um.</para>
/// </summary>
public enum EntryAuthor
{
    /// <summary>Von den Eltern angelegt - für das Kind schreibgeschützt.</summary>
    Eltern,

    /// <summary>Vom Kind selbst angelegt. Es darf den Eintrag auch wieder ändern und löschen.</summary>
    Kind
}
