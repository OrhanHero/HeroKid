using LernTor.Core.Enums;
using LernTor.Core.Models;

namespace LernTor.Core.Services;

public static partial class TrafficSignCatalog
{
    /// <summary>
    /// Richtzeichen: sie geben Hinweise und regeln den Ablauf, meist blau und rechteckig. Die
    /// Vorfahrt-Zeichen (301/306/307) gehören hierher, obwohl sie wie Vorschriftzeichen wirken -
    /// eine beliebte Fangfrage.
    /// </summary>
    private static readonly TrafficSign[] Richt =
    {
        new()
        {
            Number = "301", Name = "Vorfahrt",
            Category = TrafficSignCategory.Richtzeichen, Shape = SignShape.DreieckSpitzeOben,
            PathData = SignPictograms.VorfahrtKreuz,
            Meaning = "An der nächsten Kreuzung oder Einmündung hat man Vorfahrt - aber nur dort, nicht darüber hinaus.",
            Hint = "Der dicke Balken ist deine Straße. Gilt nur für die eine nächste Kreuzung.",
            RelevantForBicycle = true
        },
        new()
        {
            Number = "306", Name = "Vorfahrtstraße",
            Category = TrafficSignCategory.Richtzeichen, Shape = SignShape.Raute,
            BorderColor = SignColors.Weiss, FillColor = SignColors.Gelb,
            Meaning = "Man hat Vorfahrt, und zwar an jeder Kreuzung, bis das Ende-Zeichen kommt.",
            Hint = "Gelbe Raute = du darfst zuerst, immer wieder, die ganze Straße entlang.",
            RelevantForBicycle = true
        },
        new()
        {
            Number = "307", Name = "Ende der Vorfahrtstraße",
            Category = TrafficSignCategory.Richtzeichen, Shape = SignShape.Raute,
            BorderColor = SignColors.Weiss, FillColor = SignColors.Gelb,
            PathData = SignPictograms.SchraegstrichKreuz, PathColor = "#7A7A7A", PathStrokeThickness = 6,
            Meaning = "Ab hier gilt die Vorfahrt nicht mehr. An der nächsten Kreuzung meist wieder rechts vor links.",
            Hint = "Vorsicht: nach diesem Schild darf plötzlich der von rechts zuerst - das wird oft übersehen.",
            RelevantForBicycle = true
        },
        new()
        {
            Number = "310", Name = "Ortstafel",
            Category = TrafficSignCategory.Richtzeichen, Shape = SignShape.Rechteck,
            BorderColor = SignColors.Schwarz, Text = "Berlin",
            Meaning = "Beginn einer geschlossenen Ortschaft. Ab hier gilt Tempo 50, auch ohne Tempo-Schild.",
            Hint = "Die Ortstafel IST das Tempolimit. Wer auf ein 50er-Schild wartet, wartet vergeblich.",
            RelevantForBicycle = false
        },
        new()
        {
            Number = "311", Name = "Ortstafel (Rückseite)",
            Category = TrafficSignCategory.Richtzeichen, Shape = SignShape.Rechteck,
            BorderColor = SignColors.Schwarz, Text = "Berlin",
            PathData = SignPictograms.SchraegstrichAbwaerts, PathColor = SignColors.Rot, PathStrokeThickness = 5,
            Meaning = "Ende der geschlossenen Ortschaft. Ab hier gilt außerorts Tempo 100, wenn nichts anderes steht.",
            Hint = "Der durchgestrichene Ortsname ist die Erlaubnis, wieder schneller zu fahren.",
            RelevantForBicycle = false
        },
        new()
        {
            Number = "314", Name = "Parken",
            Category = TrafficSignCategory.Richtzeichen, Shape = SignShape.Quadrat,
            BorderColor = SignColors.Blau, FillColor = SignColors.Blau,
            Text = "P", TextColor = SignColors.Weiss,
            Meaning = "Hier darf geparkt werden. Zusatzzeichen können Zeit oder Fahrzeugart einschränken.",
            Hint = "Das blaue P ist eine Erlaubnis, kein Gebot - parken muss niemand.",
            RelevantForBicycle = false
        },
        new()
        {
            Number = "325.1", Name = "Beginn eines verkehrsberuhigten Bereichs",
            Category = TrafficSignCategory.Richtzeichen, Shape = SignShape.Quadrat,
            BorderColor = SignColors.Blau, FillColor = SignColors.Blau,
            PathData = SignPictograms.Kinder, PathColor = SignColors.Weiss,
            Meaning = "Spielstraße: Schrittgeschwindigkeit, Fußgänger dürfen die ganze Straße nutzen und Kinder überall spielen.",
            Hint = "Schrittgeschwindigkeit heißt etwa 7 km/h - langsamer als die meisten denken. Parken nur auf markierten Flächen.",
            RelevantForBicycle = true
        },
        new()
        {
            Number = "325.2", Name = "Ende eines verkehrsberuhigten Bereichs",
            Category = TrafficSignCategory.Richtzeichen, Shape = SignShape.Quadrat,
            BorderColor = SignColors.Blau, FillColor = SignColors.Blau,
            PathData = SignPictograms.Kinder, PathColor = SignColors.Weiss,
            OverlayPathData = SignPictograms.SchraegstrichAbwaerts, OverlayColor = SignColors.Rot, OverlayStrokeThickness = 7,
            Meaning = "Ende der Spielstraße. Wer von hier auf die Straße einfährt, muss allen anderen Vorrang geben.",
            Hint = "Beim Verlassen ist man immer der Wartepflichtige - egal, wer von rechts kommt.",
            RelevantForBicycle = true
        },
        new()
        {
            Number = "330.1", Name = "Autobahn",
            Category = TrafficSignCategory.Richtzeichen, Shape = SignShape.Quadrat,
            BorderColor = SignColors.Blau, FillColor = SignColors.Blau,
            PathData = SignPictograms.Autobahn, PathColor = SignColors.Weiss,
            Meaning = "Beginn der Autobahn. Nur für Kraftfahrzeuge, die schneller als 60 km/h fahren können. Mindestens 60, Richtgeschwindigkeit 130.",
            Hint = "Fahrrad, Mofa und Fußgänger sind hier verboten - lebensgefährlich, nicht nur unerlaubt.",
            RelevantForBicycle = false
        },
        new()
        {
            Number = "330.2", Name = "Ende der Autobahn",
            Category = TrafficSignCategory.Richtzeichen, Shape = SignShape.Quadrat,
            BorderColor = SignColors.Blau, FillColor = SignColors.Blau,
            PathData = SignPictograms.Autobahn, PathColor = SignColors.Weiss,
            OverlayPathData = SignPictograms.SchraegstrichKreuz, OverlayColor = SignColors.Rot, OverlayStrokeThickness = 7,
            Meaning = "Ende der Autobahn. Die besonderen Regeln gelten nicht mehr; ab hier ist mit Gegenverkehr und Kreuzungen zu rechnen.",
            Hint = "Nach 200 km Autobahn fühlen sich 80 km/h wie Schritttempo an - genau da passieren die Unfälle.",
            RelevantForBicycle = false
        },
        new()
        {
            Number = "331.1", Name = "Kraftfahrstraße",
            Category = TrafficSignCategory.Richtzeichen, Shape = SignShape.Quadrat,
            BorderColor = SignColors.Blau, FillColor = SignColors.Blau,
            PathData = SignPictograms.PkwFrontal, PathColor = SignColors.Weiss,
            Meaning = "Nur für Kraftfahrzeuge, die schneller als 60 km/h fahren können. Anders als auf der Autobahn kann es Kreuzungen und Gegenverkehr geben.",
            Hint = "Das weiße Auto von vorn. Kein Tempolimit von selbst - aber auch keine getrennten Fahrbahnen.",
            RelevantForBicycle = false
        },
        new()
        {
            Number = "350-10", Name = "Fußgängerüberweg",
            Category = TrafficSignCategory.Richtzeichen, Shape = SignShape.Quadrat,
            BorderColor = SignColors.Blau, FillColor = SignColors.Blau,
            PathData = SignPictograms.Fussgaenger, PathColor = SignColors.Weiss,
            OverlayPathData = SignPictograms.Zebrastreifen, OverlayColor = SignColors.Weiss,
            Meaning = "Zebrastreifen. Fahrzeuge müssen Fußgängern und Rollstuhlfahrern das Überqueren ermöglichen.",
            Hint = "Am Zebrastreifen darf nicht überholt werden - auch nicht, wenn gerade niemand dasteht.",
            RelevantForBicycle = true
        },
        new()
        {
            Number = "357", Name = "Sackgasse",
            Category = TrafficSignCategory.Richtzeichen, Shape = SignShape.Quadrat,
            BorderColor = SignColors.Blau, FillColor = SignColors.Weiss,
            PathData = SignPictograms.Sackgasse, PathColor = SignColors.Rot,
            Meaning = "Die Straße hat keine Durchfahrt. Am Ende muss gewendet werden.",
            Hint = "Ist der Querbalken oben unterbrochen, kommen Radfahrer und Fußgänger trotzdem durch.",
            RelevantForBicycle = true
        }
    };

    /// <summary>
    /// Verkehrseinrichtungen sichern Baustellen und Gefahrstellen. Sie sind keine Schilder im
    /// engeren Sinn, sondern Absperrungen - stehen aber in der Prüfung mit drin.
    /// </summary>
    private static readonly TrafficSign[] Einrichtungen =
    {
        new()
        {
            Number = "600", Name = "Absperrschranke",
            Category = TrafficSignCategory.Verkehrseinrichtung, Shape = SignShape.Rechteck,
            BorderColor = SignColors.Schwarz,
            PathData = SignPictograms.BakenStreifen, PathColor = SignColors.Rot,
            Meaning = "Sperrt eine Straße oder Fahrspur vollständig ab. Dahinter darf nicht weitergefahren werden.",
            Hint = "Rot-weiß gestreift heißt überall: hier ist Schluss.",
            RelevantForBicycle = true
        },
        new()
        {
            Number = "605", Name = "Schraffenbake",
            Category = TrafficSignCategory.Verkehrseinrichtung, Shape = SignShape.Rechteck,
            BorderColor = SignColors.Schwarz,
            PathData = SignPictograms.BakenStreifen, PathColor = SignColors.Rot,
            Meaning = "Leitet den Verkehr an einer Engstelle vorbei. Die Streifen fallen zur Seite ab, an der man vorbeifahren muss.",
            Hint = "Die Schrägstreifen zeigen wie ein Pfeil, wohin du ausweichen sollst.",
            RelevantForBicycle = true
        },
        new()
        {
            Number = "610", Name = "Leitkegel",
            Category = TrafficSignCategory.Verkehrseinrichtung, Shape = SignShape.Rechteck,
            BorderColor = SignColors.Transparent, FillColor = SignColors.Transparent,
            PathData = SignPictograms.Leitkegel, PathColor = SignColors.Rot,
            Meaning = "Sperrt kurzfristig einen Bereich ab, etwa bei einer Unfallstelle oder Tagesbaustelle.",
            Hint = "Zwischen den Pylonen wird nicht durchgefahren, auch wenn Platz wäre.",
            RelevantForBicycle = true
        },
        new()
        {
            Number = "720", Name = "Grünpfeilschild",
            Category = TrafficSignCategory.Verkehrseinrichtung, Shape = SignShape.Quadrat,
            BorderColor = SignColors.Schwarz, FillColor = SignColors.Schwarz,
            PathData = SignPictograms.GruenpfeilSchaft, PathColor = SignColors.Gruen, PathStrokeThickness = 12,
            OverlayPathData = SignPictograms.GruenpfeilSpitze, OverlayColor = SignColors.Gruen,
            Meaning = "Bei Rot darf nach rechts abgebogen werden - aber erst nach vollständigem Halt an der Haltlinie und nur, wenn niemand behindert wird.",
            Hint = "Der Grünpfeil ist kein zweites Grün. Ohne Anhalten ist es ein Rotlichtverstoß.",
            RelevantForBicycle = true
        }
    };

    /// <summary>
    /// Zusatzzeichen stehen immer UNTER einem Hauptzeichen und verändern es. Allein gelesen
    /// bedeuten sie nichts - deshalb steht in der Bedeutung immer dazu, worauf sie sich beziehen.
    /// </summary>
    private static readonly TrafficSign[] Zusatz =
    {
        new()
        {
            Number = "1000-20", Name = "Richtung rechtsweisend",
            Category = TrafficSignCategory.Zusatzzeichen, Shape = SignShape.Rechteck,
            BorderColor = SignColors.Schwarz,
            PathData = SignPictograms.PfeilNachRechtsSchaft, PathStrokeThickness = 10,
            OverlayPathData = SignPictograms.PfeilNachRechtsSpitze, OverlayColor = SignColors.Schwarz,
            Meaning = "Das Zeichen darüber gilt für die Richtung, in die der Pfeil zeigt.",
            Hint = "Ohne den Pfeil würde das Hauptzeichen für alle Richtungen gelten.",
            RelevantForBicycle = true
        },
        new()
        {
            Number = "1000-10", Name = "Richtung linksweisend",
            Category = TrafficSignCategory.Zusatzzeichen, Shape = SignShape.Rechteck,
            BorderColor = SignColors.Schwarz,
            PathData = SignPictograms.PfeilNachLinksSchaft, PathStrokeThickness = 10,
            OverlayPathData = SignPictograms.PfeilNachLinksSpitze, OverlayColor = SignColors.Schwarz,
            Meaning = "Das Zeichen darüber gilt für die Richtung, in die der Pfeil zeigt - hier nach links.",
            Hint = "Derselbe Pfeil gibt es auch rechtsweisend. Die Richtung entscheidet, welcher Weg gemeint ist.",
            RelevantForBicycle = true
        },
        new()
        {
            Number = "1000-30", Name = "Beide Richtungen",
            Category = TrafficSignCategory.Zusatzzeichen, Shape = SignShape.Rechteck,
            BorderColor = SignColors.Schwarz,
            PathData = "M20,50 L80,50", PathStrokeThickness = 10,
            OverlayPathData = "M30,32 L12,50 L30,68 Z M70,32 L88,50 L70,68 Z", OverlayColor = SignColors.Schwarz,
            Meaning = "Das Zeichen darüber gilt in beide Richtungen, etwa bei einem Radweg für Gegenverkehr.",
            Hint = "Zwei Pfeilspitzen: hier kommt dir auf demselben Weg jemand entgegen.",
            RelevantForBicycle = true
        },
        new()
        {
            Number = "1004-30", Name = "Entfernungsangabe",
            Category = TrafficSignCategory.Zusatzzeichen, Shape = SignShape.Rechteck,
            BorderColor = SignColors.Schwarz, Text = "200 m",
            Meaning = "Das Zeichen darüber gilt erst in der angegebenen Entfernung - es ist eine Vorankündigung.",
            Hint = "Noch gilt es nicht. Erst nach der angegebenen Strecke wird es ernst.",
            RelevantForBicycle = true
        },
        new()
        {
            Number = "1001-30", Name = "Länge einer Strecke",
            Category = TrafficSignCategory.Zusatzzeichen, Shape = SignShape.Rechteck,
            BorderColor = SignColors.Schwarz, Text = "600 m",
            Meaning = "Das Zeichen darüber gilt für die angegebene Länge - danach endet es ohne weiteres Schild.",
            Hint = "Unterschied zur Entfernungsangabe: hier gilt es SOFORT, aber nur so weit.",
            RelevantForBicycle = true
        },
        new()
        {
            Number = "1020-30", Name = "Anlieger frei",
            Category = TrafficSignCategory.Zusatzzeichen, Shape = SignShape.Rechteck,
            BorderColor = SignColors.Schwarz, Text = "Anlieger frei",
            Meaning = "Das Verbot darüber gilt nicht für Menschen, die dort wohnen, arbeiten oder jemanden besuchen.",
            Hint = "Wer dort tatsächlich etwas zu tun hat, darf. Nur durchfahren, um abzukürzen, gilt nicht.",
            RelevantForBicycle = true
        },
        new()
        {
            Number = "1053-35", Name = "Bei Nässe",
            Category = TrafficSignCategory.Zusatzzeichen, Shape = SignShape.Rechteck,
            BorderColor = SignColors.Schwarz, Text = "bei Nässe",
            Meaning = "Das Zeichen darüber gilt nur, solange die Fahrbahn nass ist - nicht bei Nebel oder Schnee.",
            Hint = "Nass heißt: geschlossener Wasserfilm auf der Straße. Nicht schon, wenn es tröpfelt.",
            RelevantForBicycle = false
        }
    };
}
