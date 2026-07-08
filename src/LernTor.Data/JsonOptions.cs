using System.Text.Json;
using System.Text.Json.Serialization;

namespace LernTor.Data;

/// <summary>
/// Serialisiert Enums (z.B. Subject) als Namen statt als Zahl. Sonst würde ein späteres
/// Hinzufügen/Entfernen von Enum-Werten (wie beim Aufteilen von Naturwissenschaften in
/// Biologie/Chemie/Physik) bereits gespeicherte Zahlenwerte stillschweigend auf die falschen
/// Enum-Member verschieben.
/// </summary>
public static class JsonOptions
{
    public static readonly JsonSerializerOptions Default = new()
    {
        Converters = { new JsonStringEnumConverter() }
    };
}
