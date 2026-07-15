using LernTor.Core.Enums;
using LernTor.Core.Models;

namespace LernTor.ContentGen.Generators;

/// <summary>Musik nach Berliner Rahmenlehrplan, Klasse 6 (Grundlagen/Form/Gattungen/Wirkung/Kultur) und Klasse 9 (vertieft, Harmonielehre bis gesellschaftlicher Kontext).</summary>
public sealed class MusikGenerator : ExerciseGeneratorBase
{
    public override Subject Subject => Subject.Musik;

    protected override IReadOnlyDictionary<GradeLevel, IReadOnlyList<TopicFactory>> TopicsByGrade { get; } =
        new Dictionary<GradeLevel, IReadOnlyList<TopicFactory>>
        {
            [GradeLevel.Klasse6] = new List<TopicFactory> { GrundlagenDerMusik, FormUndGestaltung, GattungenUndGenres, WirkungUndFunktion, MusikImKulturellenKontext },
            [GradeLevel.Klasse9] = new List<TopicFactory> { HarmonielehreUndPartiturlesen, KompositionUndSatzweisen, MedienUndDigitaleProduktion, GattungenDerMusikgeschichte, FilmmusikUndProgrammmusik, MusikImGesellschaftlichenKontext }
        };

    private static readonly (string Frage, string[] Optionen, string Antwort, string Erklaerung)[] GrundlagenDerMusikListe =
    {
        ("Was ist ein Halbtonschritt?", new[] { "Der kleinste Tonabstand im westlichen Tonsystem", "Ein Tonabstand von zwei ganzen Tönen", "Ein Begriff aus der Harmonielehre für laute Musik" },
            "Der kleinste Tonabstand im westlichen Tonsystem", "Der Halbtonschritt ist der kleinste in der westlichen Musik verwendete Tonabstand, z.B. von C zu Cis."),
        ("Was ist ein Ganztonschritt?", new[] { "Ein Tonabstand, der aus zwei Halbtonschritten besteht", "Der kleinste mögliche Tonabstand", "Ein anderes Wort für eine ganze Note (Notenwert)" },
            "Ein Tonabstand, der aus zwei Halbtonschritten besteht", "Ein Ganztonschritt entspricht zwei aneinandergereihten Halbtonschritten, z.B. von C zu D."),
        ("Wie viele Halbtonschritte umfasst eine Oktave im westlichen Tonsystem?", new[] { "12", "7", "8" },
            "12", "Eine Oktave ist im westlichen (chromatischen) Tonsystem in 12 gleich große Halbtonschritte unterteilt."),
        ("Was ist eine Tonleiter?", new[] { "Eine festgelegte Abfolge von Tönen in aufsteigender oder absteigender Reihenfolge", "Ein einzelner, isolierter Ton", "Ein Begriff aus der Notation von Rhythmen" },
            "Eine festgelegte Abfolge von Tönen in aufsteigender oder absteigender Reihenfolge", "Eine Tonleiter ordnet Töne nach einem bestimmten Muster aus Ganz- und Halbtonschritten in einer festen Reihenfolge an."),
        ("Was ist charakteristisch für den Aufbau einer Dur-Tonleiter?", new[] { "Eine feste Abfolge von Ganz- und Halbtonschritten (Halbtonschritte zwischen 3./4. und 7./8. Stufe)", "Es gibt keinerlei feste Abfolge, Dur-Tonleitern sind beliebig aufgebaut", "Sie besteht ausschließlich aus Halbtonschritten" },
            "Eine feste Abfolge von Ganz- und Halbtonschritten (Halbtonschritte zwischen 3./4. und 7./8. Stufe)", "Die Dur-Tonleiter folgt dem festen Muster Ganz-Ganz-Halb-Ganz-Ganz-Ganz-Halb."),
        ("Was bewirkt ein Vorzeichen (z.B. Kreuz oder b) vor einer Note?", new[] { "Es verändert die Tonhöhe der Note um einen Halbtonschritt", "Es verändert die Lautstärke der Note", "Es verändert ausschließlich die Notenlänge" },
            "Es verändert die Tonhöhe der Note um einen Halbtonschritt", "Vorzeichen wie das Kreuz (#) oder das b verändern die Tonhöhe einer Note jeweils um einen Halbtonschritt nach oben oder unten."),
        ("Was bewirkt ein Kreuz (#) vor einer Note?", new[] { "Es erhöht die Note um einen Halbtonschritt", "Es erniedrigt die Note um einen Halbtonschritt", "Es verlängert die Notendauer" },
            "Es erhöht die Note um einen Halbtonschritt", "Das Kreuz (#) erhöht die betroffene Note um einen Halbtonschritt, z.B. wird aus C ein Cis."),
        ("Was bewirkt ein b (Erniedrigungszeichen) vor einer Note?", new[] { "Es erniedrigt die Note um einen Halbtonschritt", "Es erhöht die Note um einen Halbtonschritt", "Es verkürzt die Notendauer" },
            "Es erniedrigt die Note um einen Halbtonschritt", "Das b erniedrigt die betroffene Note um einen Halbtonschritt, z.B. wird aus B (H) ein Bb (B)."),
        ("Welches Instrument zählt typischerweise zu den \"Band-Instrumenten\"?", new[] { "Die E-Gitarre", "Die Pauke", "Die Oboe" },
            "Die E-Gitarre", "Zu den klassischen Band-Instrumenten zählen z.B. E-Gitarre, Bass, Schlagzeug und Keyboard."),
        ("Zu welcher Instrumentengruppe des Orchesters gehört die Klarinette?", new[] { "Zu den Holzblasinstrumenten", "Zu den Blechblasinstrumenten", "Zu den Streichinstrumenten" },
            "Zu den Holzblasinstrumenten", "Die Klarinette gehört, wie z.B. auch Flöte und Oboe, zu den Holzblasinstrumenten des Orchesters."),
        ("Zu welcher Instrumentengruppe gehört die Gitarre?", new[] { "Zu den Saiteninstrumenten (Zupfinstrumente)", "Zu den Blechblasinstrumenten", "Zu den Schlaginstrumenten" },
            "Zu den Saiteninstrumenten (Zupfinstrumente)", "Die Gitarre gehört zu den Saiteninstrumenten und wird durch Zupfen der Saiten gespielt."),
        ("Zu welcher Instrumentengruppe des Orchesters gehört die Violine (Geige)?", new[] { "Zu den Streichinstrumenten", "Zu den Holzblasinstrumenten", "Zu den Schlaginstrumenten" },
            "Zu den Streichinstrumenten", "Die Violine gehört, wie Bratsche, Cello und Kontrabass, zu den Streichinstrumenten des Orchesters."),
        ("Was bedeutet die Dynamikbezeichnung \"forte\"?", new[] { "Laut spielen", "Leise spielen", "Schnell spielen" },
            "Laut spielen", "\"Forte\" ist eine italienische Vortragsbezeichnung und bedeutet, laut zu spielen."),
        ("Was bedeutet die Dynamikbezeichnung \"piano\"?", new[] { "Leise spielen", "Laut spielen", "Langsam spielen" },
            "Leise spielen", "\"Piano\" ist eine italienische Vortragsbezeichnung und bedeutet, leise zu spielen."),
        ("Was zeigt die Notation von Tonhöhen im Notensystem?", new[] { "Auf welcher Linie oder im welchem Zwischenraum eine Note steht, bestimmt ihre Tonhöhe", "Ausschließlich die Lautstärke eines Tons", "Ausschließlich das verwendete Instrument" },
            "Auf welcher Linie oder im welchem Zwischenraum eine Note steht, bestimmt ihre Tonhöhe", "Die Position einer Note auf den Linien oder in den Zwischenräumen des Notensystems zeigt ihre Tonhöhe an."),
        ("Was ist \"grafische Notation\" in der Musik?", new[] { "Eine freiere, oft symbolische Darstellung musikalischer Verläufe statt klassischer Notenschrift", "Eine ausschließlich digitale Notationsform", "Ein anderes Wort für ein Musikinstrument" },
            "Eine freiere, oft symbolische Darstellung musikalischer Verläufe statt klassischer Notenschrift", "Grafische Notation stellt musikalische Verläufe mit freieren, oft symbolischen Zeichen statt der klassischen Notenschrift dar."),
        ("Was bedeutet die Vortragsangabe \"crescendo\"?", new[] { "Allmählich lauter werden", "Allmählich leiser werden", "Allmählich schneller werden" },
            "Allmählich lauter werden", "\"Crescendo\" bedeutet, im Verlauf eines Musikstücks allmählich lauter zu werden."),
        ("Was bedeutet die Vortragsangabe \"decrescendo\" (oder \"diminuendo\")?", new[] { "Allmählich leiser werden", "Allmählich lauter werden", "Allmählich langsamer werden" },
            "Allmählich leiser werden", "\"Decrescendo\"/\"diminuendo\" bedeutet, im Verlauf eines Musikstücks allmählich leiser zu werden."),
        ("Warum ist das Wissen um Halb- und Ganztonschritte besonders für das Spielen der Gitarre wichtig?", new[] { "Weil sich Halb- und Ganztonschritte direkt in den Bünden auf dem Griffbrett widerspiegeln", "Weil die Gitarre als einziges Instrument keine Halbtonschritte kennt", "Weil dieses Wissen für Saiteninstrumente keine Rolle spielt" },
            "Weil sich Halb- und Ganztonschritte direkt in den Bünden auf dem Griffbrett widerspiegeln", "Jeder Bund auf dem Gitarrengriffbrett entspricht einem Halbtonschritt - das Wissen um Halb-/Ganztonschritte hilft direkt beim Greifen von Tönen und Akkorden."),
        ("Was ist der Unterschied zwischen einer Note und einer Pause in der Notation?", new[] { "Eine Note zeigt einen zu spielenden Ton, eine Pause zeigt eine Stille von bestimmter Dauer", "Beide Symbole bedeuten exakt dasselbe", "Eine Pause zeigt immer einen besonders lauten Ton an" },
            "Eine Note zeigt einen zu spielenden Ton, eine Pause zeigt eine Stille von bestimmter Dauer", "Während eine Note einen zu spielenden Ton mit bestimmter Tonhöhe und Dauer anzeigt, zeigt eine Pause eine Stille von festgelegter Dauer an.")
    };

    private static QuizQuestion GrundlagenDerMusik(Random r)
    {
        var f = GrundlagenDerMusikListe[r.Next(GrundlagenDerMusikListe.Length)];
        return new QuizQuestion
        {
            Id = NewId(), Subject = Subject.Musik, GradeLevel = GradeLevel.Klasse6,
            Topic = "Grundlagen der Musik", Type = QuestionType.MultipleChoice,
            Prompt = f.Frage, Options = f.Optionen, CorrectAnswers = new[] { f.Antwort }, Explanation = f.Erklaerung,
            HelpHint = "Halb- und Ganztonschritte bilden Tonleitern; Vorzeichen (Kreuz/b) verändern die Tonhöhe um einen Halbtonschritt; forte = laut, piano = leise."
        };
    }

    private static readonly (string Frage, string[] Optionen, string Antwort, string Erklaerung)[] FormUndGestaltungListe =
    {
        ("Was bedeutet \"Solo\" in der Musik?", new[] { "Ein einzelner Musiker/eine einzelne Musikerin spielt oder singt allein", "Alle Musiker spielen gemeinsam", "Ein Begriff für eine besonders laute Passage" },
            "Ein einzelner Musiker/eine einzelne Musikerin spielt oder singt allein", "Solo bezeichnet den Part, in dem eine einzelne Person allein spielt oder singt, oft hervorgehoben aus dem Gesamtklang."),
        ("Was bedeutet \"Tutti\"?", new[] { "Alle Musiker:innen spielen gemeinsam", "Nur eine einzelne Person spielt", "Ein Begriff für völlige Stille" },
            "Alle Musiker:innen spielen gemeinsam", "\"Tutti\" (italienisch für \"alle\") bezeichnet den Abschnitt, in dem alle Musiker:innen gemeinsam spielen - der Gegensatz zum Solo."),
        ("Was ist musikalische Gruppenimprovisation?", new[] { "Gemeinsames, spontanes Musizieren ohne vollständig festgelegte Vorgabe", "Das exakte Nachspielen eines vollständig notierten Stücks", "Ein Begriff aus der Musiktheorie ohne praktischen Bezug" },
            "Gemeinsames, spontanes Musizieren ohne vollständig festgelegte Vorgabe", "Bei der Gruppenimprovisation musizieren mehrere Personen gemeinsam spontan, oft nach einfachen Regeln, ohne dass jeder Ton vorher notiert wurde."),
        ("Was bedeutet \"Komponieren mit Rhythmusbausteinen\"?", new[] { "Ein Musikstück aus vorgegebenen, kurzen rhythmischen Mustern zusammenzusetzen", "Ein Musikstück ausschließlich aus einem einzigen, langen Ton zu bilden", "Ein Begriff, der nur bei elektronischer Musik verwendet wird" },
            "Ein Musikstück aus vorgegebenen, kurzen rhythmischen Mustern zusammenzusetzen", "Beim Komponieren mit Rhythmusbausteinen werden kurze, vorgegebene rhythmische Muster kombiniert, um ein eigenes Stück zu gestalten."),
        ("Was bedeutet \"Zweistimmigkeit\"?", new[] { "Zwei voneinander unabhängige, gleichzeitig erklingende Melodiestimmen", "Nur eine einzige Melodiestimme ohne Begleitung", "Ein Musikstück, das ausschließlich instrumental ist" },
            "Zwei voneinander unabhängige, gleichzeitig erklingende Melodiestimmen", "Zweistimmigkeit bedeutet, dass zwei eigenständige Melodiestimmen gleichzeitig erklingen."),
        ("Was bedeutet \"Imitation\" als musikalische Satzweise?", new[] { "Eine Stimme wiederholt (imitiert) eine musikalische Idee, die eine andere Stimme vorher vorgegeben hat", "Alle Stimmen spielen exakt zeitgleich denselben Ton", "Ein Begriff, der nur beim Singen, nie bei Instrumenten verwendet wird" },
            "Eine Stimme wiederholt (imitiert) eine musikalische Idee, die eine andere Stimme vorher vorgegeben hat", "Bei der Imitation greift eine Stimme eine musikalische Idee auf, die zuvor von einer anderen Stimme vorgegeben wurde."),
        ("Was ist die ABA-Form?", new[] { "Eine dreiteilige Form, bei der der erste Teil (A) nach einem kontrastierenden Mittelteil (B) wiederkehrt", "Eine Form, die aus nur einem einzigen, sich niemals wiederholenden Teil besteht", "Ein anderes Wort für eine Tonleiter" },
            "Eine dreiteilige Form, bei der der erste Teil (A) nach einem kontrastierenden Mittelteil (B) wiederkehrt", "Die ABA-Form besteht aus einem Anfangsteil (A), einem kontrastierenden Mittelteil (B) und der Wiederkehr des Anfangsteils (A)."),
        ("Was ist ein Rondo?", new[] { "Eine musikalische Form, bei der ein wiederkehrender Hauptteil (Refrain) mit wechselnden Zwischenteilen (Couplets) abwechselt", "Eine Form, die aus nur einem einzigen Teil besteht", "Ein anderes Wort für eine Tonleiter" },
            "Eine musikalische Form, bei der ein wiederkehrender Hauptteil (Refrain) mit wechselnden Zwischenteilen (Couplets) abwechselt", "Im Rondo wechselt ein wiederkehrender Hauptteil (Refrain) mit unterschiedlichen Zwischenteilen (Couplets) ab, z.B. A-B-A-C-A."),
        ("Was ist ein Kanon in der Musik?", new[] { "Ein Stück, bei dem mehrere Stimmen dieselbe Melodie zeitversetzt nacheinander singen/spielen", "Ein Stück, das ausschließlich von einer einzigen Stimme gesungen wird", "Ein anderes Wort für eine Tonleiter" },
            "Ein Stück, bei dem mehrere Stimmen dieselbe Melodie zeitversetzt nacheinander singen/spielen", "Bei einem Kanon singen oder spielen mehrere Stimmen dieselbe Melodie, beginnen aber zeitversetzt nacheinander."),
        ("Was ist ein bekanntes Beispiel für einen Kanon?", new[] { "\"Bruder Jakob\"", "Die deutsche Nationalhymne", "Ein Marsch von Johann Sebastian Bach" },
            "\"Bruder Jakob\"", "\"Bruder Jakob\" ist eines der bekanntesten Beispiele für einen einfachen, mehrstimmigen Kanon."),
        ("Was unterscheidet Solo von Tutti?", new[] { "Solo bedeutet, dass eine einzelne Person spielt, Tutti bedeutet, dass alle gemeinsam spielen", "Beide Begriffe bezeichnen exakt dasselbe", "Tutti bedeutet, dass niemand spielt" },
            "Solo bedeutet, dass eine einzelne Person spielt, Tutti bedeutet, dass alle gemeinsam spielen", "Solo und Tutti sind Gegensätze: Solo ist der Part einer einzelnen Person, Tutti der Abschnitt, in dem alle gemeinsam spielen."),
        ("Was bedeutet \"Vorspiel\" in einem Musikstück?", new[] { "Ein einleitender Abschnitt vor dem eigentlichen Hauptteil des Stücks", "Der letzte, abschließende Teil eines Stücks", "Ein anderes Wort für eine Pause" },
            "Ein einleitender Abschnitt vor dem eigentlichen Hauptteil des Stücks", "Das Vorspiel ist ein einleitender musikalischer Abschnitt, der dem eigentlichen Hauptteil eines Stücks vorausgeht."),
        ("Was bedeutet \"Zusammenspiel\" (Ensemble-Spiel)?", new[] { "Mehrere Musiker:innen spielen aufeinander abgestimmt gemeinsam ein Stück", "Nur eine einzelne Person spielt komplett allein", "Ein Begriff, der ausschließlich beim Singen verwendet wird" },
            "Mehrere Musiker:innen spielen aufeinander abgestimmt gemeinsam ein Stück", "Zusammenspiel bedeutet, dass mehrere Musiker:innen aufeinander abgestimmt gemeinsam musizieren."),
        ("Wie ist ein Rondo typischerweise strukturell aufgebaut?", new[] { "Ein wiederkehrender Refrain wechselt sich mit unterschiedlichen Couplets ab, z.B. A-B-A-C-A", "Es besteht aus nur einem einzigen, unveränderlichen Teil", "Es hat immer exakt zwei gleich lange Teile" },
            "Ein wiederkehrender Refrain wechselt sich mit unterschiedlichen Couplets ab, z.B. A-B-A-C-A", "Typisch für ein Rondo ist der Wechsel zwischen einem wiederkehrenden Refrain (A) und unterschiedlichen Zwischenteilen (B, C, ...)."),
        ("Was bedeutet die Wiederholung des A-Teils am Ende einer ABA-Form?", new[] { "Der Anfangsteil kehrt nach dem kontrastierenden Mittelteil zurück und rundet das Stück ab", "Der A-Teil wird komplett übersprungen", "Der A-Teil wird beim zweiten Mal völlig verändert gespielt" },
            "Der Anfangsteil kehrt nach dem kontrastierenden Mittelteil zurück und rundet das Stück ab", "Die Wiederkehr des A-Teils nach dem kontrastierenden B-Teil rundet die ABA-Form ab und schafft ein Gefühl von Vertrautheit und Abschluss."),
        ("Was ist ein Rhythmusbaustein?", new[] { "Ein kurzes, festgelegtes rhythmisches Muster, das als Baustein für eine Komposition dient", "Ein einzelner, isolierter Ton ohne jede rhythmische Bedeutung", "Ein Begriff aus der Harmonielehre" },
            "Ein kurzes, festgelegtes rhythmisches Muster, das als Baustein für eine Komposition dient", "Ein Rhythmusbaustein ist ein kurzes, wiederverwendbares rhythmisches Muster, das beim Komponieren als Baustein eingesetzt wird."),
        ("Warum eignet sich Gruppenimprovisation gut zum gemeinsamen Musizieren im Unterricht?", new[] { "Weil sie spontanes, kreatives Zusammenspiel ermöglicht, ohne dass jede Stimme vorher komponiert sein muss", "Weil sie ausschließlich Einzelspiel erlaubt", "Weil sie strengere Regeln als klassisches Notenspiel erfordert" },
            "Weil sie spontanes, kreatives Zusammenspiel ermöglicht, ohne dass jede Stimme vorher komponiert sein muss", "Gruppenimprovisation ermöglicht spontanes, gemeinsames Musizieren, ohne dass zuvor jede einzelne Stimme komponiert werden muss."),
        ("Was passiert beim Kanon-Prinzip, wenn eine zweite Stimme zeitversetzt einsetzt?", new[] { "Sie singt/spielt dieselbe Melodie wie die erste Stimme, beginnt aber später", "Sie singt/spielt eine völlig andere, unabhängige Melodie", "Sie pausiert vollständig, bis die erste Stimme fertig ist" },
            "Sie singt/spielt dieselbe Melodie wie die erste Stimme, beginnt aber später", "Beim Kanon-Prinzip setzt jede weitere Stimme mit derselben Melodie zeitversetzt (später) ein - dadurch entsteht Mehrstimmigkeit aus einer einzigen Melodie."),
        ("Was unterscheidet einen Kanon von einer einfachen Imitation?", new[] { "Beim Kanon wird die gesamte Melodie zeitversetzt exakt wiederholt, bei der Imitation reicht oft ein kürzerer musikalischer Gedanke", "Beide Begriffe bedeuten exakt dasselbe", "Ein Kanon enthält niemals eine Imitation" },
            "Beim Kanon wird die gesamte Melodie zeitversetzt exakt wiederholt, bei der Imitation reicht oft ein kürzerer musikalischer Gedanke", "Ein Kanon ist eine besondere, durchgängige Form der Imitation, bei der die komplette Melodie zeitversetzt wiederholt wird - Imitation kann sich allgemein auch auf kürzere musikalische Ideen beziehen."),
        ("Wie kann man eine ABA-Form von einem Rondo unterscheiden?", new[] { "Die ABA-Form hat genau drei Teile, ein Rondo hat mehrere wechselnde Zwischenteile um einen wiederkehrenden Refrain", "Beide Formen sind strukturell exakt identisch", "Ein Rondo hat niemals einen wiederkehrenden Teil" },
            "Die ABA-Form hat genau drei Teile, ein Rondo hat mehrere wechselnde Zwischenteile um einen wiederkehrenden Refrain", "Die ABA-Form besteht aus genau drei Teilen (A-B-A), während ein Rondo mehrere unterschiedliche Zwischenteile um einen wiederkehrenden Refrain herum anordnet (z.B. A-B-A-C-A).")
    };

    private static QuizQuestion FormUndGestaltung(Random r)
    {
        var f = FormUndGestaltungListe[r.Next(FormUndGestaltungListe.Length)];
        return new QuizQuestion
        {
            Id = NewId(), Subject = Subject.Musik, GradeLevel = GradeLevel.Klasse6,
            Topic = "Form und Gestaltung", Type = QuestionType.MultipleChoice,
            Prompt = f.Frage, Options = f.Optionen, CorrectAnswers = new[] { f.Antwort }, Explanation = f.Erklaerung,
            HelpHint = "Solo = einer spielt, Tutti = alle spielen. Kanon: dieselbe Melodie zeitversetzt. ABA-Form hat drei Teile, Rondo wechselt Refrain und Couplets."
        };
    }

    private static readonly (string Frage, string[] Optionen, string Antwort, string Erklaerung)[] GattungenUndGenresListe =
    {
        ("Was ist Kammermusik?", new[] { "Musik für eine kleine Besetzung, meist ein Instrument pro Stimme", "Musik ausschließlich für ein großes Orchester", "Ein anderes Wort für Popmusik" },
            "Musik für eine kleine Besetzung, meist ein Instrument pro Stimme", "Kammermusik wird von einer kleinen Besetzung gespielt, typischerweise mit nur einem Instrument pro Stimme, z.B. im Streichquartett."),
        ("Was ist Orchestermusik?", new[] { "Musik für eine große Besetzung verschiedener Instrumentengruppen", "Musik ausschließlich für ein einzelnes Instrument", "Ein anderes Wort für Kammermusik" },
            "Musik für eine große Besetzung verschiedener Instrumentengruppen", "Orchestermusik wird von einer großen Besetzung mit mehreren Instrumentengruppen (Streicher, Bläser, Schlagwerk) gespielt."),
        ("Was ist der zentrale Unterschied zwischen Kammermusik und Orchestermusik?", new[] { "Kammermusik hat eine kleine Besetzung mit einem Instrument pro Stimme, Orchestermusik eine große Besetzung mit mehreren Instrumenten je Stimme", "Beide Begriffe bezeichnen exakt dieselbe Besetzungsgröße", "Orchestermusik wird ausschließlich von einer einzigen Person gespielt" },
            "Kammermusik hat eine kleine Besetzung mit einem Instrument pro Stimme, Orchestermusik eine große Besetzung mit mehreren Instrumenten je Stimme", "Der wesentliche Unterschied liegt in der Besetzungsgröße: Kammermusik besetzt jede Stimme meist mit einem Instrument, Orchestermusik mit mehreren."),
        ("Was ist ein Sprechstück in der Musik?", new[] { "Ein Stück, bei dem Text rhythmisch gesprochen statt gesungen wird", "Ein Stück, das ausschließlich instrumental ist", "Ein Begriff aus der Harmonielehre" },
            "Ein Stück, bei dem Text rhythmisch gesprochen statt gesungen wird", "Bei einem Sprechstück wird der Text rhythmisch gesprochen statt melodisch gesungen."),
        ("Was ist ein Chorsatz?", new[] { "Eine mehrstimmige Komposition für mehrere Gesangsstimmen", "Ein Stück ausschließlich für ein einzelnes Soloinstrument", "Ein Begriff aus der Notation von Rhythmen" },
            "Eine mehrstimmige Komposition für mehrere Gesangsstimmen", "Ein Chorsatz ist eine mehrstimmige Komposition, bei der mehrere Gesangsstimmen zusammen musizieren."),
        ("Was bedeutet \"zweistimmiger Chorsatz\"?", new[] { "Zwei voneinander unabhängige Gesangsstimmen singen gemeinsam", "Nur eine einzige Gesangsstimme singt allein", "Ein Chorsatz, der ausschließlich instrumental begleitet wird" },
            "Zwei voneinander unabhängige Gesangsstimmen singen gemeinsam", "Ein zweistimmiger Chorsatz besteht aus zwei eigenständigen, gleichzeitig erklingenden Gesangsstimmen."),
        ("Was ist ein Streichquartett als Beispiel für Kammermusik?", new[] { "Eine Besetzung aus zwei Violinen, Bratsche und Cello", "Eine Besetzung aus vier Blechblasinstrumenten", "Ein vollständiges Symphonieorchester" },
            "Eine Besetzung aus zwei Violinen, Bratsche und Cello", "Das Streichquartett ist eine klassische Kammermusikbesetzung aus zwei Violinen, einer Bratsche und einem Cello."),
        ("Was ist ein Symphonieorchester?", new[] { "Eine große Orchesterbesetzung mit Streichern, Holz- und Blechbläsern sowie Schlagwerk", "Eine kleine Besetzung mit nur einem Instrument pro Stimme", "Ein anderes Wort für einen Chor" },
            "Eine große Orchesterbesetzung mit Streichern, Holz- und Blechbläsern sowie Schlagwerk", "Ein Symphonieorchester ist eine große Orchesterbesetzung, die alle wichtigen Instrumentengruppen (Streicher, Holz-/Blechbläser, Schlagwerk) umfasst."),
        ("Warum werden in der Musikkultur Lieder oft in verschiedenen Sprachen gesungen?", new[] { "Weil Musik über Sprachgrenzen hinweg kulturelle Vielfalt und Identität ausdrücken kann", "Weil es gesetzlich vorgeschrieben ist, Lieder nur in einer einzigen Sprache zu singen", "Weil andere Sprachen beim Singen technisch unmöglich sind" },
            "Weil Musik über Sprachgrenzen hinweg kulturelle Vielfalt und Identität ausdrücken kann", "Lieder in verschiedenen Sprachen spiegeln kulturelle Vielfalt wider und ermöglichen es, Identität und Herkunft musikalisch auszudrücken."),
        ("Was ist Vokalmusik allgemein?", new[] { "Musik, bei der die menschliche Singstimme im Mittelpunkt steht", "Musik, die ausschließlich von Instrumenten gespielt wird", "Ein anderes Wort für Orchestermusik" },
            "Musik, bei der die menschliche Singstimme im Mittelpunkt steht", "Vokalmusik stellt die menschliche Singstimme in den Mittelpunkt, im Gegensatz zur Instrumentalmusik."),
        ("Was ist Instrumentalmusik allgemein?", new[] { "Musik, die ausschließlich mit Instrumenten ohne Gesang gespielt wird", "Musik, die ausschließlich gesungen wird", "Ein anderes Wort für Kammermusik" },
            "Musik, die ausschließlich mit Instrumenten ohne Gesang gespielt wird", "Instrumentalmusik wird ausschließlich mit Instrumenten gespielt, ohne dass eine Gesangsstimme beteiligt ist."),
        ("Was ist ein modernes Beispiel für ein Sprechstück?", new[] { "Rap-Musik, bei der Texte rhythmisch gesprochen werden", "Eine klassische Opernarie", "Ein rein instrumentales Klavierstück" },
            "Rap-Musik, bei der Texte rhythmisch gesprochen werden", "Rap ist ein modernes Beispiel für ein Sprechstück, bei dem Texte rhythmisch gesprochen statt melodisch gesungen werden."),
        ("Was bedeutet \"a cappella\"?", new[] { "Gesang ganz ohne instrumentale Begleitung", "Gesang ausschließlich mit vollem Orchester", "Ein Begriff für besonders lautes Singen" },
            "Gesang ganz ohne instrumentale Begleitung", "\"A cappella\" bezeichnet Gesang, der komplett ohne instrumentale Begleitung ausgeführt wird."),
        ("Was ist ein Solist/eine Solistin in der Kammermusik?", new[] { "Eine Person, die einen hervorgehobenen Einzelpart innerhalb der kleinen Besetzung übernimmt", "Eine Person, die ausschließlich im Hintergrund unhörbar mitspielt", "Ein Begriff, der nur bei Orchestermusik verwendet wird" },
            "Eine Person, die einen hervorgehobenen Einzelpart innerhalb der kleinen Besetzung übernimmt", "Ein Solist/eine Solistin übernimmt auch innerhalb einer kleinen Kammermusikbesetzung einen besonders hervorgehobenen Part."),
        ("Was unterscheidet ein Klaviertrio von einem Streichquartett?", new[] { "Ein Klaviertrio besteht aus Klavier, Violine und Cello, ein Streichquartett aus vier Streichinstrumenten ohne Klavier", "Beide Besetzungen sind identisch", "Ein Klaviertrio besteht immer aus vier Instrumenten" },
            "Ein Klaviertrio besteht aus Klavier, Violine und Cello, ein Streichquartett aus vier Streichinstrumenten ohne Klavier", "Ein Klaviertrio besteht typischerweise aus Klavier, Violine und Cello, während ein Streichquartett aus vier reinen Streichinstrumenten ohne Klavier besteht."),
        ("Warum eignet sich Kammermusik gut für kleine, intime Aufführungsräume?", new[] { "Weil die kleine Besetzung auch in akustisch begrenzten Räumen gut zur Geltung kommt", "Weil Kammermusik grundsätzlich extrem laut gespielt werden muss", "Weil Kammermusik ausschließlich in großen Konzertsälen erlaubt ist" },
            "Weil die kleine Besetzung auch in akustisch begrenzten Räumen gut zur Geltung kommt", "Die kleine Besetzung der Kammermusik kommt auch in kleineren, intimeren Räumen gut zur Geltung, ohne ein großes Orchester zu benötigen."),
        ("Was ist die Aufgabe des Dirigenten/der Dirigentin in einem Orchester?", new[] { "Das gemeinsame Zusammenspiel des Orchesters zu koordinieren und zu leiten", "Ausschließlich ein einzelnes Solo-Instrument zu spielen", "Die Noten für das Orchester zu drucken" },
            "Das gemeinsame Zusammenspiel des Orchesters zu koordinieren und zu leiten", "Der Dirigent/die Dirigentin koordiniert und leitet das Zusammenspiel aller Musiker:innen im Orchester."),
        ("Was zeichnet einen zweistimmigen Chorsatz musikalisch gegenüber einem einstimmigen Lied aus?", new[] { "Zwei unabhängige, gleichzeitig erklingende Gesangsstimmen statt nur einer einzigen Melodiestimme", "Er wird ausschließlich instrumental begleitet", "Er besteht aus exakt vier Gesangsstimmen" },
            "Zwei unabhängige, gleichzeitig erklingende Gesangsstimmen statt nur einer einzigen Melodiestimme", "Im Gegensatz zu einem einstimmigen Lied bestehen bei einem zweistimmigen Chorsatz zwei unabhängige, gleichzeitig erklingende Gesangsstimmen."),
        ("Was ist der Unterschied zwischen einem Lied und einem reinen Instrumentalstück?", new[] { "Ein Lied enthält gesungenen Text, ein Instrumentalstück wird ausschließlich mit Instrumenten gespielt", "Beide Begriffe bezeichnen exakt dasselbe", "Ein Instrumentalstück enthält immer gesungenen Text" },
            "Ein Lied enthält gesungenen Text, ein Instrumentalstück wird ausschließlich mit Instrumenten gespielt", "Ein Lied verbindet Text und Melodie durch Gesang, während ein Instrumentalstück ganz ohne Gesangsstimme auskommt."),
        ("Was ist ein Beispiel für ein modernes \"Sprechstück\" außerhalb klassischer Musik?", new[] { "Ein Poetry-Slam-Beitrag mit rhythmisch gesprochenem Text", "Eine klassische Opernarie mit gesungenem Text", "Ein rein instrumentales Orchesterstück ohne Text" },
            "Ein Poetry-Slam-Beitrag mit rhythmisch gesprochenem Text", "Auch ein Poetry-Slam-Beitrag mit rhythmisch gesprochenem Text kann als moderne Form eines Sprechstücks verstanden werden.")
    };

    private static QuizQuestion GattungenUndGenres(Random r)
    {
        var f = GattungenUndGenresListe[r.Next(GattungenUndGenresListe.Length)];
        return new QuizQuestion
        {
            Id = NewId(), Subject = Subject.Musik, GradeLevel = GradeLevel.Klasse6,
            Topic = "Gattungen und Genres", Type = QuestionType.MultipleChoice,
            Prompt = f.Frage, Options = f.Optionen, CorrectAnswers = new[] { f.Antwort }, Explanation = f.Erklaerung,
            HelpHint = "Kammermusik = kleine Besetzung, Orchestermusik = große Besetzung. Vokalmusik braucht eine Singstimme, Instrumentalmusik kommt ohne Gesang aus."
        };
    }

    private static readonly (string Frage, string[] Optionen, string Antwort, string Erklaerung)[] WirkungUndFunktionListe =
    {
        ("Was bedeutet \"Programmmusik\"?", new[] { "Instrumentalmusik, die eine außermusikalische Geschichte, Idee oder ein Bild musikalisch darstellen will", "Musik, die ausschließlich nach einem festen Notenprogramm ohne jede Freiheit gespielt wird", "Ein anderes Wort für Popmusik" },
            "Instrumentalmusik, die eine außermusikalische Geschichte, Idee oder ein Bild musikalisch darstellen will", "Programmmusik ist Instrumentalmusik, die versucht, eine außermusikalische Geschichte, Idee oder ein Bild (z.B. eine Landschaft) musikalisch darzustellen."),
        ("Was bedeutet der Zusammenhang von Text und Musik in einem Song?", new[] { "Melodie, Rhythmus und Musik unterstützen und verstärken die Aussage des Textes", "Text und Musik stehen in einem Song immer völlig unabhängig voneinander", "Ein Song besteht laut Definition ausschließlich aus Text ohne Musik" },
            "Melodie, Rhythmus und Musik unterstützen und verstärken die Aussage des Textes", "In einem Song arbeiten Musik und Text zusammen - Melodie, Rhythmus und Klang können die inhaltliche Aussage des Textes unterstreichen und verstärken."),
        ("Was sind \"musikalische Idole/Vorbilder\"?", new[] { "Musiker:innen, die für andere durch ihre Musik oder ihren Stil zur Orientierung werden", "Ausschließlich historische Komponisten, die schon lange verstorben sind", "Ein Begriff, der nichts mit Musik zu tun hat" },
            "Musiker:innen, die für andere durch ihre Musik oder ihren Stil zur Orientierung werden", "Musikalische Idole/Vorbilder sind Musiker:innen, an deren Musik, Stil oder Auftreten sich andere - oft Jugendliche - orientieren."),
        ("Was bedeutet \"funktionale Musik\"?", new[] { "Musik, die einem bestimmten praktischen Zweck dient (z.B. Werbung, Tanz, Hymne)", "Musik, die ausschließlich zum reinen Zuhören ohne jeden weiteren Zweck komponiert wird", "Ein anderes Wort für Kammermusik" },
            "Musik, die einem bestimmten praktischen Zweck dient (z.B. Werbung, Tanz, Hymne)", "Funktionale Musik dient einem bestimmten praktischen Zweck, etwa um in der Werbung Aufmerksamkeit zu erzeugen oder zum Tanzen zu animieren."),
        ("Was ist die typische Funktion von Musik in der Werbung?", new[] { "Aufmerksamkeit zu erzeugen und positive Gefühle mit einem Produkt zu verbinden", "Ausschließlich neutrale Information ohne jede emotionale Wirkung zu vermitteln", "Werbemusik hat keinerlei erkennbare Funktion" },
            "Aufmerksamkeit zu erzeugen und positive Gefühle mit einem Produkt zu verbinden", "Musik in der Werbung soll Aufmerksamkeit erzeugen und positive Gefühle oder Assoziationen mit dem beworbenen Produkt verknüpfen."),
        ("Was ist eine Hymne?", new[] { "Ein feierliches Lied, das ein Land, eine Organisation oder ein Ereignis repräsentiert", "Ein rein instrumentales Tanzstück ohne jeden feierlichen Anlass", "Ein anderes Wort für ein Sprechstück" },
            "Ein feierliches Lied, das ein Land, eine Organisation oder ein Ereignis repräsentiert", "Eine Hymne ist ein feierliches Lied, das oft ein Land (Nationalhymne), eine Organisation oder ein besonderes Ereignis repräsentiert."),
        ("Was ist Poptanz-Musik als Beispiel für funktionale Musik?", new[] { "Musik, die gezielt zum Tanzen in einem bestimmten Rhythmus komponiert oder ausgewählt wird", "Musik, die ausschließlich zum stillen, konzentrierten Zuhören gedacht ist", "Ein anderes Wort für eine Nationalhymne" },
            "Musik, die gezielt zum Tanzen in einem bestimmten Rhythmus komponiert oder ausgewählt wird", "Poptanz-Musik wird gezielt so gestaltet oder ausgewählt, dass sie zum Tanzen in einem bestimmten Rhythmus anregt."),
        ("Wie kann Instrumentalmusik ohne Worte trotzdem eine Geschichte oder Stimmung transportieren?", new[] { "Durch musikalische Mittel wie Melodie, Rhythmus, Dynamik und Klangfarbe", "Instrumentalmusik kann grundsätzlich niemals eine Stimmung oder Geschichte transportieren", "Ausschließlich durch gesprochene Erklärungen neben der Musik" },
            "Durch musikalische Mittel wie Melodie, Rhythmus, Dynamik und Klangfarbe", "Auch ohne Worte kann Instrumentalmusik durch Melodie, Rhythmus, Dynamik und Klangfarbe Stimmungen erzeugen und Geschichten andeuten."),
        ("Was bedeutet \"Hörweise\" im musikalischen Sinn?", new[] { "Die individuelle Art und Weise, wie eine Person Musik wahrnimmt und darauf reagiert", "Ein technischer Begriff für die Lautstärke eines Musikstücks", "Ein anderes Wort für eine Tonleiter" },
            "Die individuelle Art und Weise, wie eine Person Musik wahrnimmt und darauf reagiert", "Hörweise beschreibt, wie individuell unterschiedlich Menschen Musik wahrnehmen, deuten und emotional darauf reagieren."),
        ("Warum unterscheiden sich musikalische Vorlieben oft innerhalb einer Lerngruppe?", new[] { "Weil persönlicher Geschmack, kultureller Hintergrund und Vorbilder die eigenen Vorlieben unterschiedlich prägen", "Weil alle Menschen von Natur aus exakt denselben Musikgeschmack haben müssten", "Weil musikalische Vorlieben gesetzlich vorgeschrieben sind" },
            "Weil persönlicher Geschmack, kultureller Hintergrund und Vorbilder die eigenen Vorlieben unterschiedlich prägen", "Persönlicher Geschmack, kultureller Hintergrund und musikalische Vorbilder prägen die individuellen musikalischen Vorlieben innerhalb einer Gruppe unterschiedlich."),
        ("Was ist ein Beispiel dafür, wie Musik in der Werbung gezielt Emotionen erzeugen soll?", new[] { "Ruhige, warme Musik bei einer Werbung für ein Familienprodukt, um Geborgenheit zu vermitteln", "Musik in der Werbung wird grundsätzlich zufällig und ohne jede Überlegung ausgewählt", "Werbemusik darf laut Definition niemals Emotionen ansprechen" },
            "Ruhige, warme Musik bei einer Werbung für ein Familienprodukt, um Geborgenheit zu vermitteln", "Werbemusik wird oft gezielt ausgewählt, um bestimmte Emotionen - wie Geborgenheit bei einem Familienprodukt - beim Publikum zu erzeugen."),
        ("Was bedeutet es, dass Text und Musik in einem Song \"zusammenwirken\"?", new[] { "Die musikalische Gestaltung (z.B. Tempo, Dynamik) unterstreicht die inhaltliche Aussage des Textes", "Text und Musik werden in einem Song immer komplett unabhängig voneinander komponiert", "Nur der Text hat in einem Song überhaupt eine Bedeutung" },
            "Die musikalische Gestaltung (z.B. Tempo, Dynamik) unterstreicht die inhaltliche Aussage des Textes", "Zusammenwirken bedeutet, dass musikalische Gestaltungsmittel wie Tempo oder Dynamik die inhaltliche Aussage des Textes verstärken oder unterstreichen."),
        ("Was ist eine Nationalhymne?", new[] { "Ein feierliches Lied, das ein Land offiziell repräsentiert", "Ein rein privates Lied ohne jede öffentliche Bedeutung", "Ein anderes Wort für ein Tanzlied" },
            "Ein feierliches Lied, das ein Land offiziell repräsentiert", "Eine Nationalhymne ist ein feierliches Lied, das offiziell ein Land repräsentiert, z.B. bei staatlichen Anlässen gespielt wird."),
        ("Was ist ein Unterschied zwischen einem Song mit Text und reiner Instrumentalmusik bezüglich der Wirkung?", new[] { "Ein Song mit Text kann eine konkrete inhaltliche Botschaft direkt vermitteln, Instrumentalmusik wirkt eher über Stimmung und Assoziation", "Beide Formen wirken immer exakt identisch", "Instrumentalmusik kann laut Definition niemals irgendeine Wirkung erzeugen" },
            "Ein Song mit Text kann eine konkrete inhaltliche Botschaft direkt vermitteln, Instrumentalmusik wirkt eher über Stimmung und Assoziation", "Ein Song mit Text kann eine konkrete Botschaft direkt aussprechen, während reine Instrumentalmusik vor allem über Stimmung, Klangfarbe und Assoziation wirkt."),
        ("Was bedeutet \"außermusikalisches Programm\" bei Programmmusik?", new[] { "Eine Geschichte, ein Bild oder eine Idee außerhalb der Musik selbst, die musikalisch dargestellt werden soll", "Ein rein technisches Notationsprogramm am Computer", "Ein Begriff, der ausschließlich bei elektronischer Musik verwendet wird" },
            "Eine Geschichte, ein Bild oder eine Idee außerhalb der Musik selbst, die musikalisch dargestellt werden soll", "Das \"außermusikalische Programm\" ist die außerhalb der Musik liegende Geschichte, das Bild oder die Idee, die ein Instrumentalstück musikalisch darstellen möchte."),
        ("Warum kann Musik als Vorbild/Idol für Jugendliche eine wichtige Rolle spielen?", new[] { "Weil musikalische Idole Werte, Stil und Identifikationsmöglichkeiten bieten können", "Weil musikalische Idole für Jugendliche grundsätzlich keinerlei Bedeutung haben", "Weil dies gesetzlich vorgeschrieben ist" },
            "Weil musikalische Idole Werte, Stil und Identifikationsmöglichkeiten bieten können", "Musikalische Idole können Jugendlichen Werte, Stil und Möglichkeiten zur Identifikation bieten, die über die reine Musik hinausgehen."),
        ("Was ist ein Beispiel für funktionale Musik im Alltag?", new[] { "Die Musik in einem Einkaufszentrum, die zum Verweilen anregen soll", "Ein rein privat gehörtes Konzertstück ohne jeden praktischen Zweck", "Ein Musikstück, das ausschließlich in einem Museum ausgestellt wird" },
            "Die Musik in einem Einkaufszentrum, die zum Verweilen anregen soll", "Hintergrundmusik in Einkaufszentren ist ein klassisches Beispiel für funktionale Musik, die gezielt eine bestimmte Wirkung (z.B. längeres Verweilen) erzeugen soll."),
        ("Wie kann ein Instrumentalstück eine Landschaft oder ein Ereignis musikalisch darstellen?", new[] { "Durch klangliche Mittel wie Tempo, Dynamik und Instrumentierung, die an das dargestellte Bild erinnern", "Ausschließlich durch gesungenen, beschreibenden Text", "Instrumentalmusik kann grundsätzlich keine Bilder oder Ereignisse darstellen" },
            "Durch klangliche Mittel wie Tempo, Dynamik und Instrumentierung, die an das dargestellte Bild erinnern", "Ein Instrumentalstück kann durch klangliche Mittel wie Tempo, Dynamik und Instrumentierung an eine Landschaft oder ein Ereignis erinnern, ohne dass Worte nötig sind."),
        ("Was bedeutet es, wenn Musik \"Stimmung transportiert\"?", new[] { "Die Musik erzeugt beim Hören ein bestimmtes Gefühl oder eine bestimmte Atmosphäre", "Musik hat grundsätzlich keinerlei Einfluss auf Gefühle", "Nur gesungene Musik kann laut Definition Stimmung transportieren" },
            "Die Musik erzeugt beim Hören ein bestimmtes Gefühl oder eine bestimmte Atmosphäre", "Musik \"transportiert Stimmung\", indem sie beim Hören ein bestimmtes Gefühl oder eine bestimmte Atmosphäre erzeugt."),
        ("Warum ist die Analyse von Text-Musik-Zusammenhängen bei Songs sinnvoll?", new[] { "Weil sie zeigt, wie musikalische Gestaltung die inhaltliche Aussage eines Textes verstärken kann", "Weil Text und Musik in einem Song ohnehin niemals zusammenhängen", "Weil eine solche Analyse für das Verständnis von Musik bedeutungslos ist" },
            "Weil sie zeigt, wie musikalische Gestaltung die inhaltliche Aussage eines Textes verstärken kann", "Die Analyse von Text-Musik-Zusammenhängen zeigt, wie musikalische Gestaltungsmittel die inhaltliche Aussage eines Songtextes gezielt verstärken können.")
    };

    private static QuizQuestion WirkungUndFunktion(Random r)
    {
        var f = WirkungUndFunktionListe[r.Next(WirkungUndFunktionListe.Length)];
        return new QuizQuestion
        {
            Id = NewId(), Subject = Subject.Musik, GradeLevel = GradeLevel.Klasse6,
            Topic = "Wirkung und Funktion", Type = QuestionType.MultipleChoice,
            Prompt = f.Frage, Options = f.Optionen, CorrectAnswers = new[] { f.Antwort }, Explanation = f.Erklaerung,
            HelpHint = "Programmmusik stellt außermusikalische Ideen dar; funktionale Musik dient einem Zweck (Werbung, Tanz, Hymne); Text und Musik wirken in Songs zusammen."
        };
    }

    private static readonly (string Frage, string[] Optionen, string Antwort, string Erklaerung)[] MusikImKulturellenKontextListe =
    {
        ("Was ist ein Konzert als musikalische Veranstaltungsform?", new[] { "Eine öffentliche Aufführung von Musik vor Publikum", "Ein rein privates, unhörbares Musikstück", "Ein Begriff aus der Notenschrift" },
            "Eine öffentliche Aufführung von Musik vor Publikum", "Ein Konzert ist eine öffentliche Aufführung von Musik, bei der Musiker:innen live vor Publikum spielen oder singen."),
        ("Was ist ein Bandauftritt?", new[] { "Die Live-Aufführung einer Musikgruppe (Band) vor Publikum", "Ein Begriff für eine rein technische Studioaufnahme", "Ein anderes Wort für ein einzelnes Musikstück in der Notenschrift" },
            "Die Live-Aufführung einer Musikgruppe (Band) vor Publikum", "Ein Bandauftritt ist die Live-Aufführung einer Musikgruppe vor Publikum, z.B. bei einem Konzert oder Festival."),
        ("Was bedeutet Urheberrecht im Zusammenhang mit Musik?", new[] { "Das Recht der Urheber:innen (z.B. Komponist:innen), über die Nutzung ihres Werks zu bestimmen", "Ein Recht, das ausschließlich für Bücher, nie für Musik gilt", "Ein Begriff ohne rechtliche Bedeutung" },
            "Das Recht der Urheber:innen (z.B. Komponist:innen), über die Nutzung ihres Werks zu bestimmen", "Das Urheberrecht schützt die Rechte der Urheber:innen eines Musikwerks und regelt, wer es wie nutzen darf."),
        ("Warum ist unerlaubtes Herunterladen von Musik oder Noten aus dem Internet problematisch?", new[] { "Weil es die Rechte der Urheber:innen verletzt und ihnen eine angemessene Vergütung entzieht", "Weil Musikdateien technisch gar nicht heruntergeladen werden können", "Weil das Herunterladen von Musik grundsätzlich nirgendwo legal möglich ist" },
            "Weil es die Rechte der Urheber:innen verletzt und ihnen eine angemessene Vergütung entzieht", "Unerlaubtes Herunterladen verletzt die Rechte der Urheber:innen und entzieht ihnen die ihnen zustehende Vergütung für ihr Werk."),
        ("Was ist eine Musikepoche?", new[] { "Ein historischer Zeitabschnitt mit typischen musikalischen Stilmerkmalen", "Ein einzelnes, isoliertes Musikstück", "Ein Begriff aus der Harmonielehre" },
            "Ein historischer Zeitabschnitt mit typischen musikalischen Stilmerkmalen", "Eine Musikepoche ist ein historischer Zeitabschnitt, der durch bestimmte typische musikalische Stilmerkmale gekennzeichnet ist."),
        ("Welche der folgenden ist eine bekannte Epoche der europäischen Musikgeschichte?", new[] { "Barock", "Steinzeit", "Digitalzeitalter der Raumfahrt" },
            "Barock", "Barock (ca. 1600-1750) ist eine bekannte Epoche der europäischen Musikgeschichte, gefolgt u.a. von Klassik und Romantik."),
        ("Wie hat sich Musik zu bestimmten Anlässen (z.B. Hochzeiten) im Laufe der Zeit verändert?", new[] { "Musikstile und Instrumentierung passen sich veränderten gesellschaftlichen Gewohnheiten und Vorlieben an", "Musik zu solchen Anlässen ist seit Jahrhunderten exakt identisch geblieben", "Musik spielt bei solchen Anlässen grundsätzlich keine Rolle" },
            "Musikstile und Instrumentierung passen sich veränderten gesellschaftlichen Gewohnheiten und Vorlieben an", "Musik zu bestimmten Anlässen wie Hochzeiten verändert sich mit der Zeit, weil sich gesellschaftliche Gewohnheiten, Technik und musikalische Vorlieben wandeln."),
        ("Was bedeutet \"Persönlichkeitsrecht\" im Zusammenhang mit einer Musikaufnahme?", new[] { "Das Recht einer Person, selbst zu bestimmen, wie ihre Stimme oder ihr Bild verwendet werden", "Ein Recht, das ausschließlich für schriftliche Texte gilt", "Ein Begriff ohne Bezug zu Musik" },
            "Das Recht einer Person, selbst zu bestimmen, wie ihre Stimme oder ihr Bild verwendet werden", "Das Persönlichkeitsrecht schützt u.a. das Recht einer Person, selbst zu bestimmen, wie ihre Stimme oder ihr Bild - etwa in einer Musikaufnahme - verwendet werden darf."),
        ("Was ist ein zentraler Unterschied zwischen einem Live-Konzert und einer Musikaufnahme?", new[] { "Ein Live-Konzert findet unmittelbar und einmalig vor Publikum statt, eine Aufnahme kann beliebig oft wiedergegeben werden", "Beide sind in jeder Hinsicht identisch", "Eine Aufnahme findet immer live vor Publikum statt" },
            "Ein Live-Konzert findet unmittelbar und einmalig vor Publikum statt, eine Aufnahme kann beliebig oft wiedergegeben werden", "Ein Live-Konzert ist ein einmaliges, unmittelbares Erlebnis, während eine Aufnahme beliebig oft und zu jeder Zeit wiedergegeben werden kann."),
        ("Was ist neben dem klassischen Konzert eine weitere typische musikalische Veranstaltungsform?", new[] { "Ein Musikfestival mit mehreren Bands/Künstler:innen", "Eine rein private, unhörbare Musikaufführung", "Ein Begriff aus der Notation" },
            "Ein Musikfestival mit mehreren Bands/Künstler:innen", "Neben einzelnen Konzerten sind Musikfestivals mit mehreren auftretenden Bands/Künstler:innen eine weitere verbreitete Veranstaltungsform."),
        ("Warum ist Musik oft ein wichtiger Teil gesellschaftlicher Veranstaltungen wie Hochzeiten oder Feiern?", new[] { "Weil sie Emotionen verstärken und die Atmosphäre eines Anlasses mitgestalten kann", "Weil Musik bei solchen Anlässen gesetzlich vorgeschrieben ist", "Weil Musik bei gesellschaftlichen Anlässen grundsätzlich keinerlei Bedeutung hat" },
            "Weil sie Emotionen verstärken und die Atmosphäre eines Anlasses mitgestalten kann", "Musik kann Emotionen verstärken und die Atmosphäre eines gesellschaftlichen Anlasses wie einer Hochzeit oder Feier maßgeblich mitgestalten."),
        ("Was bedeutet es, Noten oder Musik legal statt illegal aus dem Internet zu beziehen?", new[] { "Für die Nutzung zu bezahlen oder erlaubte, kostenlose Quellen zu nutzen, statt Urheberrechte zu verletzen", "Noten und Musik dürfen grundsätzlich nirgendwo legal bezogen werden", "Legalität spielt beim Musikdownload keine Rolle" },
            "Für die Nutzung zu bezahlen oder erlaubte, kostenlose Quellen zu nutzen, statt Urheberrechte zu verletzen", "Der legale Bezug von Noten oder Musik bedeutet, für die Nutzung zu bezahlen oder ausdrücklich erlaubte, kostenlose Quellen zu nutzen, statt die Rechte der Urheber:innen zu verletzen."),
        ("Was gilt als typisches Merkmal der Barockmusik?", new[] { "Reich verzierte Melodien und der durchgehende Generalbass", "Ausschließlich elektronisch erzeugte Klänge", "Vollständiger Verzicht auf jede Melodie" },
            "Reich verzierte Melodien und der durchgehende Generalbass", "Barockmusik ist u.a. durch reich verzierte Melodien und den durchgehenden Generalbass (eine begleitende Basslinie mit Akkorden) gekennzeichnet."),
        ("Was gilt als typisches Merkmal der Musik der Klassik (Musikepoche, ca. 1730-1820)?", new[] { "Klare, ausgewogene Formen und übersichtliche Melodien", "Ausschließlich elektronisch erzeugte Klänge", "Vollständiger Verzicht auf jede Form von Struktur" },
            "Klare, ausgewogene Formen und übersichtliche Melodien", "Die Musik der Klassik zeichnet sich durch klare, ausgewogene Formen und übersichtliche, gut nachvollziehbare Melodien aus."),
        ("Was gilt als typisches Merkmal der Musik der Romantik (Musikepoche, 19. Jahrhundert)?", new[] { "Starke Gefühlsbetontheit und oft größer besetzte Orchester", "Ausschließlich sehr kurze, einfache Melodien ohne jede Emotion", "Vollständiger Verzicht auf jedes Orchesterinstrument" },
            "Starke Gefühlsbetontheit und oft größer besetzte Orchester", "Die Musik der Romantik ist oft stark gefühlsbetont und wurde häufig für größer besetzte Orchester komponiert als in der vorherigen Klassik."),
        ("Was ist ein Beispiel für Musik, die zu einem bestimmten Anlass komponiert oder ausgewählt wird?", new[] { "Feierliche Musik bei einer Trauerfeier oder einem Fest", "Musik, die für keinerlei Anlass jemals ausgewählt wird", "Ein Begriff, der ausschließlich für Werbemusik gilt" },
            "Feierliche Musik bei einer Trauerfeier oder einem Fest", "Musik wird oft gezielt für bestimmte Anlässe wie Trauerfeiern oder Feste ausgewählt oder komponiert, um die Stimmung des Anlasses zu unterstreichen."),
        ("Warum verändert sich Musik im Laufe der Geschichte?", new[] { "Weil sich Gesellschaft, Technik und musikalischer Geschmack ständig weiterentwickeln", "Weil Musik seit ihrer Erfindung komplett unverändert geblieben ist", "Weil sich Musik nur durch Zufall verändert, ohne erkennbare Gründe" },
            "Weil sich Gesellschaft, Technik und musikalischer Geschmack ständig weiterentwickeln", "Musik verändert sich, weil sich Gesellschaft, Technik (z.B. neue Instrumente) und musikalischer Geschmack im Laufe der Zeit weiterentwickeln."),
        ("Was bedeutet \"musikalisches Erbe\"?", new[] { "Musikalische Werke und Traditionen, die von einer Generation an die nächste weitergegeben werden", "Ein rein finanzieller Begriff ohne Bezug zu Musik", "Ein Begriff, der ausschließlich Adelsfamilien betrifft" },
            "Musikalische Werke und Traditionen, die von einer Generation an die nächste weitergegeben werden", "Musikalisches Erbe umfasst Werke und Traditionen, die von einer Generation an die nächste weitergegeben und bewahrt werden."),
        ("Was ist ein Grund, warum Musiker:innen Rechte an ihren eigenen Werken haben sollten?", new[] { "Damit sie für ihre kreative Arbeit angemessen vergütet und anerkannt werden", "Weil Musik grundsätzlich niemandem gehören sollte", "Weil nur berühmte Musiker:innen ein Recht an ihren Werken haben sollten" },
            "Damit sie für ihre kreative Arbeit angemessen vergütet und anerkannt werden", "Urheberrechte sollen sicherstellen, dass Musiker:innen für ihre kreative Arbeit angemessen vergütet und als Urheber:innen anerkannt werden."),
        ("Wie unterscheidet sich das Musikhören heute typischerweise von früher (z.B. vor dem Internet)?", new[] { "Heute wird Musik oft über Streaming-Dienste jederzeit und überall abgerufen, früher oft über physische Tonträger wie Schallplatten", "Musikhören hat sich seit der Erfindung der Schallplatte nie verändert", "Musik konnte früher ausschließlich live gehört werden, nie aufgezeichnet" },
            "Heute wird Musik oft über Streaming-Dienste jederzeit und überall abgerufen, früher oft über physische Tonträger wie Schallplatten", "Während Musik früher oft über physische Tonträger wie Schallplatten gehört wurde, wird sie heute meist über Streaming-Dienste jederzeit und ortsunabhängig abgerufen.")
    };

    private static QuizQuestion MusikImKulturellenKontext(Random r)
    {
        var f = MusikImKulturellenKontextListe[r.Next(MusikImKulturellenKontextListe.Length)];
        return new QuizQuestion
        {
            Id = NewId(), Subject = Subject.Musik, GradeLevel = GradeLevel.Klasse6,
            Topic = "Musik im kulturellen Kontext", Type = QuestionType.MultipleChoice,
            Prompt = f.Frage, Options = f.Optionen, CorrectAnswers = new[] { f.Antwort }, Explanation = f.Erklaerung,
            HelpHint = "Urheberrecht schützt Musiker:innen; Musikepochen (Barock, Klassik, Romantik) haben je eigene Stilmerkmale; Musik zu Anlässen und das Musikhören verändern sich mit der Zeit."
        };
    }

    private static readonly (string Frage, string[] Optionen, string Antwort, string Erklaerung)[] HarmonielehreUndPartiturlesenListe =
    {
        ("Was ist ein Intervall in der Musik?", new[] { "Der Tonabstand zwischen zwei Tönen", "Ein einzelner, isolierter Ton", "Ein Begriff für die Lautstärke eines Musikstücks" },
            "Der Tonabstand zwischen zwei Tönen", "Ein Intervall bezeichnet den Tonabstand zwischen zwei Tönen, z.B. eine Terz oder eine Quinte."),
        ("Aus welchen Intervallen ist ein Dur-Dreiklang aufgebaut?", new[] { "Große Terz + kleine Terz (von unten nach oben)", "Kleine Terz + große Terz", "Ausschließlich aus zwei großen Terzen" },
            "Große Terz + kleine Terz (von unten nach oben)", "Ein Dur-Dreiklang besteht von unten nach oben aus einer großen Terz gefolgt von einer kleinen Terz."),
        ("Aus welchen Intervallen ist ein Moll-Dreiklang aufgebaut?", new[] { "Kleine Terz + große Terz (von unten nach oben)", "Große Terz + kleine Terz", "Ausschließlich aus zwei kleinen Terzen" },
            "Kleine Terz + große Terz (von unten nach oben)", "Ein Moll-Dreiklang besteht von unten nach oben aus einer kleinen Terz gefolgt von einer großen Terz."),
        ("Was ist die Tonika in einer Kadenz?", new[] { "Der Grundakkord einer Tonart, der Ruhe und Auflösung vermittelt", "Der am wenigsten stabile Akkord einer Tonart", "Ein Begriff aus der Notation von Rhythmen" },
            "Der Grundakkord einer Tonart, der Ruhe und Auflösung vermittelt", "Die Tonika ist der Grundakkord einer Tonart und vermittelt Ruhe sowie das Gefühl der Auflösung am Ende einer Kadenz."),
        ("Was ist die Dominante in einer Kadenz?", new[] { "Der Akkord auf der fünften Stufe, der Spannung erzeugt und zur Tonika hinführt", "Der Grundakkord einer Tonart", "Ein Begriff, der ausschließlich bei Moll-Tonarten verwendet wird" },
            "Der Akkord auf der fünften Stufe, der Spannung erzeugt und zur Tonika hinführt", "Die Dominante steht auf der fünften Stufe der Tonleiter, erzeugt Spannung und führt klanglich zurück zur Tonika."),
        ("Was ist die Subdominante in einer Kadenz?", new[] { "Der Akkord auf der vierten Stufe der Tonleiter", "Der Grundakkord einer Tonart", "Der Akkord auf der siebten Stufe der Tonleiter" },
            "Der Akkord auf der vierten Stufe der Tonleiter", "Die Subdominante steht auf der vierten Stufe der Tonleiter und leitet oft zur Dominante über."),
        ("Was ist eine einfache Kadenz (Tonika-Subdominante-Dominante-Tonika)?", new[] { "Eine grundlegende harmonische Abfolge, die einen klaren Schluss in einer Tonart erzeugt", "Eine Abfolge, die ausschließlich aus einem einzigen Akkord besteht", "Ein Begriff aus der Notation von Rhythmen" },
            "Eine grundlegende harmonische Abfolge, die einen klaren Schluss in einer Tonart erzeugt", "Die einfache Kadenz (Tonika-Subdominante-Dominante-Tonika) ist eine grundlegende harmonische Abfolge, die einen klaren, abgeschlossenen Klangschluss in einer Tonart erzeugt."),
        ("Was zeigt der Violinschlüssel in einer Partitur an?", new[] { "Er legt fest, auf welcher Linie der Ton g' liegt, und dient meist hohen Instrumenten/Stimmen", "Er zeigt ausschließlich die Lautstärke eines Stücks an", "Er legt die Tonart des gesamten Stücks fest" },
            "Er legt fest, auf welcher Linie der Ton g' liegt, und dient meist hohen Instrumenten/Stimmen", "Der Violinschlüssel (G-Schlüssel) legt fest, dass der Ton g' auf der zweiten Notenlinie liegt, und wird meist für höhere Instrumente und Stimmen verwendet."),
        ("Was zeigt der Bassschlüssel in einer Partitur an?", new[] { "Er legt fest, auf welcher Linie der Ton f liegt, und dient meist tiefen Instrumenten/Stimmen", "Er zeigt ausschließlich das Tempo eines Stücks an", "Er legt die Dynamik des gesamten Stücks fest" },
            "Er legt fest, auf welcher Linie der Ton f liegt, und dient meist tiefen Instrumenten/Stimmen", "Der Bassschlüssel (F-Schlüssel) legt fest, dass der Ton f auf der vierten Notenlinie liegt, und wird meist für tiefere Instrumente und Stimmen verwendet."),
        ("Was bedeutet \"Transposition\" in der Musik?", new[] { "Ein Musikstück in eine andere Tonart zu versetzen", "Ein Musikstück in ein anderes Tempo zu übertragen", "Ein Musikstück von einem Instrument auf ein anderes zu übertragen, ohne die Töne zu verändern" },
            "Ein Musikstück in eine andere Tonart zu versetzen", "Transposition bedeutet, ein Musikstück in eine andere Tonart zu versetzen, wobei die Intervalle zwischen den Tönen erhalten bleiben."),
        ("Was ist eine Partitur?", new[] { "Die Gesamtnotation eines mehrstimmigen Musikstücks mit allen Instrumental-/Gesangsstimmen übereinander", "Eine einzelne, isolierte Notenzeile für ein einzelnes Instrument", "Ein Begriff für ein rein mündlich überliefertes Musikstück" },
            "Die Gesamtnotation eines mehrstimmigen Musikstücks mit allen Instrumental-/Gesangsstimmen übereinander", "Eine Partitur zeigt die Gesamtnotation eines mehrstimmigen Musikstücks, bei der alle Instrumental- und Gesangsstimmen übereinander dargestellt werden."),
        ("Was zeigen die unterschiedlichen Notenzeilen einer Orchesterpartitur?", new[] { "Jede Zeile zeigt die Stimme eines bestimmten Instruments oder einer Instrumentengruppe", "Alle Zeilen zeigen immer exakt dieselbe Stimme", "Jede Zeile zeigt ausschließlich die Dynamik des Gesamtstücks" },
            "Jede Zeile zeigt die Stimme eines bestimmten Instruments oder einer Instrumentengruppe", "In einer Orchesterpartitur zeigt jede Notenzeile die Stimme eines bestimmten Instruments oder einer Instrumentengruppe."),
        ("Was ist ein typischer klanglicher Unterschied zwischen einem Dur- und einem Moll-Dreiklang?", new[] { "Dur klingt oft heller/fröhlicher, Moll oft dunkler/melancholischer", "Beide klingen immer exakt identisch", "Moll klingt grundsätzlich lauter als Dur" },
            "Dur klingt oft heller/fröhlicher, Moll oft dunkler/melancholischer", "Dur-Dreiklänge werden oft als heller und fröhlicher wahrgenommen, Moll-Dreiklänge oft als dunkler und melancholischer - eine verbreitete, wenn auch nicht absolute Hörgewohnheit."),
        ("Was ist eine Oktave als musikalisches Intervall?", new[] { "Der Abstand von einem Ton zum nächsten gleichnamigen Ton (z.B. C zu C)", "Der kleinstmögliche Tonabstand", "Ein Begriff, der nur bei Schlaginstrumenten verwendet wird" },
            "Der Abstand von einem Ton zum nächsten gleichnamigen Ton (z.B. C zu C)", "Eine Oktave ist der Tonabstand von einem Ton zum nächsten gleichnamigen Ton, z.B. von C zum nächsthöheren C."),
        ("Was ist eine Terz als musikalisches Intervall?", new[] { "Der Abstand von einem Ton zum übernächsten Ton der Tonleiter (z.B. C zu E)", "Der Abstand von einem Ton zum direkt benachbarten Ton", "Ein Begriff, der nur bei Blasinstrumenten verwendet wird" },
            "Der Abstand von einem Ton zum übernächsten Ton der Tonleiter (z.B. C zu E)", "Eine Terz ist der Tonabstand von einem Ton zum übernächsten Ton der Tonleiter, z.B. von C zu E."),
        ("Was ist eine Quinte als musikalisches Intervall?", new[] { "Der Abstand von einem Ton zum fünften Ton der Tonleiter (z.B. C zu G)", "Der Abstand von einem Ton zum direkt benachbarten Ton", "Ein Begriff, der ausschließlich bei Perkussionsinstrumenten verwendet wird" },
            "Der Abstand von einem Ton zum fünften Ton der Tonleiter (z.B. C zu G)", "Eine Quinte ist der Tonabstand von einem Ton zum fünften Ton der Tonleiter, z.B. von C zu G."),
        ("Wie kann man die Tonart eines Musikstücks anhand der Notenschrift bestimmen?", new[] { "Anhand der Anzahl und Art der Vorzeichen (Kreuze/b) am Beginn der Notenzeile", "Ausschließlich anhand der Lautstärke des Stücks", "Anhand der Anzahl der verwendeten Instrumente" },
            "Anhand der Anzahl und Art der Vorzeichen (Kreuze/b) am Beginn der Notenzeile", "Die Tonart eines Stücks lässt sich unter anderem anhand der Anzahl und Art der Vorzeichen (Kreuze oder b) zu Beginn der Notenzeile bestimmen."),
        ("Was bedeutet es, ein Musikstück \"eine Terz höher\" zu transponieren?", new[] { "Jeden Ton des Stücks um das Intervall einer Terz nach oben zu versetzen", "Das Stück um eine Terz zu verlangsamen", "Das Stück um eine Terz leiser zu spielen" },
            "Jeden Ton des Stücks um das Intervall einer Terz nach oben zu versetzen", "Ein Stück \"eine Terz höher\" zu transponieren bedeutet, jeden Ton um das Intervall einer Terz nach oben zu versetzen, während die Struktur der Melodie erhalten bleibt."),
        ("Wie wird das Tempo eines Musikstücks häufig in einer Partitur angegeben?", new[] { "Durch eine Metronomangabe (z.B. ♩ = 120) oder ein italienisches Tempowort (z.B. Allegro)", "Ausschließlich durch die Anzahl der verwendeten Instrumente", "Durch die Tonart des Stücks" },
            "Durch eine Metronomangabe (z.B. ♩ = 120) oder ein italienisches Tempowort (z.B. Allegro)", "Das Tempo wird in Partituren häufig durch eine Metronomangabe (Schläge pro Minute) oder ein italienisches Tempowort wie \"Allegro\" (schnell) angegeben."),
        ("Wie wird Dynamik (Lautstärkeverlauf) in einer Partitur oft angegeben?", new[] { "Durch Buchstaben wie p/f oder grafische Zeichen wie die Crescendo-/Decrescendo-Gabel", "Ausschließlich durch die Wahl der Tonart", "Durch die Anzahl der Notenzeilen" },
            "Durch Buchstaben wie p/f oder grafische Zeichen wie die Crescendo-/Decrescendo-Gabel", "Dynamik wird in Partituren häufig durch Buchstaben (p für piano, f für forte) oder grafische Zeichen wie die Crescendo-/Decrescendo-Gabel dargestellt.")
    };

    private static QuizQuestion HarmonielehreUndPartiturlesen(Random r)
    {
        var f = HarmonielehreUndPartiturlesenListe[r.Next(HarmonielehreUndPartiturlesenListe.Length)];
        return new QuizQuestion
        {
            Id = NewId(), Subject = Subject.Musik, GradeLevel = GradeLevel.Klasse9,
            Topic = "Harmonielehre und Partiturlesen", Type = QuestionType.MultipleChoice,
            Prompt = f.Frage, Options = f.Optionen, CorrectAnswers = new[] { f.Antwort }, Explanation = f.Erklaerung,
            HelpHint = "Kadenz: Tonika (Ruhe) - Subdominante (4. Stufe) - Dominante (5. Stufe, Spannung) - Tonika. Violinschlüssel für hohe, Bassschlüssel für tiefe Stimmen."
        };
    }

    private static readonly (string Frage, string[] Optionen, string Antwort, string Erklaerung)[] KompositionUndSatzweisenListe =
    {
        ("Was ist ein musikalisches Motiv?", new[] { "Eine kurze, prägnante musikalische Idee, die als Baustein eines Werks dient", "Ein vollständiges, mehrminütiges Musikstück", "Ein Begriff aus der Notation von Dynamik" },
            "Eine kurze, prägnante musikalische Idee, die als Baustein eines Werks dient", "Ein Motiv ist eine kurze, prägnante musikalische Idee (z.B. wenige Töne), die als Grundbaustein eines größeren Werks verwendet wird."),
        ("Was bedeutet \"Motivverarbeitung\"?", new[] { "Ein Motiv im Verlauf eines Stücks zu verändern, zu variieren oder weiterzuentwickeln", "Ein Motiv exakt unverändert immer wieder zu wiederholen", "Ein Begriff, der ausschließlich bei elektronischer Musik verwendet wird" },
            "Ein Motiv im Verlauf eines Stücks zu verändern, zu variieren oder weiterzuentwickeln", "Motivverarbeitung bedeutet, ein musikalisches Motiv im Verlauf eines Stücks zu verändern, zu variieren oder weiterzuentwickeln."),
        ("Was ist eine Suite als musikalische Form?", new[] { "Eine Abfolge mehrerer, meist tanzartiger Sätze mit unterschiedlichem Charakter", "Ein einzelner, sehr kurzer Ton ohne jede Struktur", "Ein anderes Wort für einen Kanon" },
            "Eine Abfolge mehrerer, meist tanzartiger Sätze mit unterschiedlichem Charakter", "Eine Suite besteht aus einer Abfolge mehrerer, oft tanzartiger Sätze mit jeweils unterschiedlichem Charakter."),
        ("Was ist ein Variationszyklus?", new[] { "Eine Reihe von Variationen, die ein gemeinsames musikalisches Thema immer wieder verändert darstellen", "Ein einzelnes, unverändertes Thema ohne jede Wiederholung", "Ein Begriff aus der Notation von Rhythmen" },
            "Eine Reihe von Variationen, die ein gemeinsames musikalisches Thema immer wieder verändert darstellen", "Ein Variationszyklus besteht aus mehreren Variationen, die ein gemeinsames Thema jeweils auf unterschiedliche Weise verändert darstellen."),
        ("Was ist eine Invention als Musikform?", new[] { "Ein meist zweistimmiges, kontrapunktisch gearbeitetes Übungs- oder Kompositionsstück", "Ein rein homophones Stück ohne jede Stimmenführung", "Ein anderes Wort für eine Symphonie" },
            "Ein meist zweistimmiges, kontrapunktisch gearbeitetes Übungs- oder Kompositionsstück", "Eine Invention ist ein meist zweistimmiges Stück, bei dem die Stimmen kontrapunktisch (in eigenständiger Stimmführung) miteinander verwoben werden - bekannt z.B. durch Johann Sebastian Bach."),
        ("Was ist eine Fuge?", new[] { "Ein polyphones Musikstück, bei dem ein Thema nacheinander in verschiedenen Stimmen imitierend eingeführt wird", "Ein rein homophones Musikstück ohne jede Stimmenführung", "Ein anderes Wort für eine Kadenz" },
            "Ein polyphones Musikstück, bei dem ein Thema nacheinander in verschiedenen Stimmen imitierend eingeführt wird", "Eine Fuge ist ein polyphones (mehrstimmiges) Stück, bei dem ein Thema nacheinander in verschiedenen Stimmen imitierend eingeführt und weiterentwickelt wird."),
        ("Wie ist die Sonatenhauptsatzform grob aufgebaut?", new[] { "Exposition - Durchführung - Reprise (oft mit abschließender Coda)", "Ausschließlich aus einem einzigen, unveränderlichen Teil", "Strophe - Refrain - Strophe - Refrain" },
            "Exposition - Durchführung - Reprise (oft mit abschließender Coda)", "Die Sonatenhauptsatzform gliedert sich klassisch in Exposition (Vorstellung der Themen), Durchführung (Verarbeitung der Themen) und Reprise (Wiederkehr der Themen), oft mit abschließender Coda."),
        ("Was passiert in der Exposition der Sonatenhauptsatzform?", new[] { "Die musikalischen Hauptthemen werden vorgestellt", "Die Themen werden intensiv motivisch verarbeitet und verändert", "Die Themen kehren am Ende unverändert zurück" },
            "Die musikalischen Hauptthemen werden vorgestellt", "In der Exposition werden die musikalischen Hauptthemen eines Sonatensatzes zum ersten Mal vorgestellt."),
        ("Was passiert in der Durchführung der Sonatenhauptsatzform?", new[] { "Die zuvor vorgestellten Themen werden motivisch intensiv verarbeitet und weiterentwickelt", "Die Themen werden zum ersten Mal überhaupt vorgestellt", "Es erklingt ausschließlich eine abschließende Coda" },
            "Die zuvor vorgestellten Themen werden motivisch intensiv verarbeitet und weiterentwickelt", "In der Durchführung werden die in der Exposition vorgestellten Themen motivisch intensiv verarbeitet, verändert und weiterentwickelt."),
        ("Was passiert in der Reprise der Sonatenhauptsatzform?", new[] { "Die Themen aus der Exposition kehren wieder, oft leicht verändert", "Die Themen werden zum ersten Mal überhaupt vorgestellt", "Es findet ausschließlich freie Improvisation statt" },
            "Die Themen aus der Exposition kehren wieder, oft leicht verändert", "In der Reprise kehren die Themen aus der Exposition wieder, oft in leicht veränderter Form, und runden den Satz ab."),
        ("Was ist eine Coda in einem Musikstück?", new[] { "Ein abschließender Teil, der ein Stück formal zum Ende bringt", "Der einleitende Teil eines Musikstücks", "Ein anderes Wort für die Exposition" },
            "Ein abschließender Teil, der ein Stück formal zum Ende bringt", "Die Coda ist ein abschließender Teil eines Musikstücks, der es formal zu einem klaren Ende bringt."),
        ("Was bedeutet \"Homophonie\" in der Musik?", new[] { "Eine Hauptstimme (Melodie) wird von untergeordneten Begleitstimmen (Akkorden) begleitet", "Mehrere gleichrangige, unabhängige Stimmen erklingen gleichzeitig", "Ein Begriff, der ausschließlich Sprechstücke betrifft" },
            "Eine Hauptstimme (Melodie) wird von untergeordneten Begleitstimmen (Akkorden) begleitet", "Bei Homophonie tritt eine Hauptstimme (Melodie) hervor, die von untergeordneten, meist akkordischen Begleitstimmen unterstützt wird."),
        ("Was bedeutet \"Polyphonie\" in der Musik?", new[] { "Mehrere gleichrangige, voneinander unabhängige Stimmen erklingen gleichzeitig", "Nur eine einzige Melodiestimme erklingt ohne jede Begleitung", "Ein Begriff, der ausschließlich bei elektronischer Musik verwendet wird" },
            "Mehrere gleichrangige, voneinander unabhängige Stimmen erklingen gleichzeitig", "Bei Polyphonie erklingen mehrere gleichrangige, voneinander unabhängige Stimmen gleichzeitig, wie z.B. in einer Fuge."),
        ("Was ist der zentrale Unterschied zwischen Homophonie und Polyphonie?", new[] { "Bei Homophonie dominiert eine Hauptstimme, bei Polyphonie sind mehrere Stimmen gleichrangig unabhängig", "Beide Begriffe bezeichnen exakt dasselbe Satzprinzip", "Polyphonie bedeutet, dass nur eine einzige Stimme erklingt" },
            "Bei Homophonie dominiert eine Hauptstimme, bei Polyphonie sind mehrere Stimmen gleichrangig unabhängig", "Homophonie ordnet eine Hauptstimme über begleitenden Akkorden an, während Polyphonie mehrere gleichrangige, unabhängige Stimmen miteinander verwebt."),
        ("Was bedeutet \"Arrangieren\" eines Musikstücks?", new[] { "Ein bestehendes Stück für eine andere Besetzung oder einen anderen Stil neu zu bearbeiten", "Ein Stück komplett neu zu komponieren, ohne jeden Bezug zum Original", "Ein Begriff, der ausschließlich das Aufnehmen im Studio bezeichnet" },
            "Ein bestehendes Stück für eine andere Besetzung oder einen anderen Stil neu zu bearbeiten", "Beim Arrangieren wird ein bestehendes Musikstück für eine andere Besetzung oder einen anderen musikalischen Stil neu bearbeitet."),
        ("Was ist eine komplexe Songform, wie sie in moderner Popmusik oft vorkommt?", new[] { "Eine Abfolge wie Strophe-Refrain-Strophe-Refrain-Bridge-Refrain", "Ausschließlich ein einziger, sich niemals wiederholender Teil", "Eine Form, die ausschließlich aus Pausen besteht" },
            "Eine Abfolge wie Strophe-Refrain-Strophe-Refrain-Bridge-Refrain", "Eine komplexe Songform in der modernen Popmusik besteht oft aus einer Abfolge wie Strophe-Refrain-Strophe-Refrain-Bridge-Refrain."),
        ("Was bedeutet Rhythmus- oder Melodieimprovisation im Kompositionsprozess?", new[] { "Spontanes Entwickeln rhythmischer oder melodischer Ideen als Ausgangspunkt für eine Komposition", "Ausschließlich das exakte Nachspielen einer bereits fertigen Komposition", "Ein Begriff, der mit Komposition nichts zu tun hat" },
            "Spontanes Entwickeln rhythmischer oder melodischer Ideen als Ausgangspunkt für eine Komposition", "Rhythmus- oder Melodieimprovisation bedeutet, spontan rhythmische oder melodische Ideen zu entwickeln, die als Ausgangspunkt für eine Komposition dienen können."),
        ("Was unterscheidet eine Fuge grundsätzlich von einem einfachen Kanon?", new[] { "Eine Fuge verarbeitet ihr Thema oft komplexer (z.B. mit Umkehrungen, Engführungen), ein Kanon wiederholt die Melodie meist unverändert zeitversetzt", "Beide Formen sind strukturell exakt identisch", "Ein Kanon ist grundsätzlich komplexer als eine Fuge" },
            "Eine Fuge verarbeitet ihr Thema oft komplexer (z.B. mit Umkehrungen, Engführungen), ein Kanon wiederholt die Melodie meist unverändert zeitversetzt", "Während ein Kanon eine Melodie meist unverändert zeitversetzt wiederholt, verarbeitet eine Fuge ihr Thema oft komplexer, z.B. durch Umkehrungen oder engführende Überlappungen."),
        ("Was unterscheidet eine Suite von einer klassischen Sonate?", new[] { "Eine Suite besteht meist aus mehreren tanzartigen Sätzen, eine Sonate folgt oft einer festen mehrsätzigen Form mit Sonatenhauptsatz", "Beide Begriffe bezeichnen exakt dieselbe Form", "Eine Sonate besteht immer aus genau einem einzigen Satz" },
            "Eine Suite besteht meist aus mehreren tanzartigen Sätzen, eine Sonate folgt oft einer festen mehrsätzigen Form mit Sonatenhauptsatz", "Eine Suite reiht meist mehrere tanzartige Sätze aneinander, während eine Sonate einer festen mehrsätzigen Form folgt, oft mit einem Sonatenhauptsatz als erstem Satz."),
        ("Warum ist Motivverarbeitung ein zentrales Prinzip vieler Kompositionen?", new[] { "Weil sie erlaubt, aus wenig musikalischem Material ein vielfältiges, zusammenhängendes Werk zu entwickeln", "Weil jedes Motiv in einem Werk nur genau einmal erklingen darf", "Weil Motivverarbeitung ausschließlich bei elektronischer Musik angewendet wird" },
            "Weil sie erlaubt, aus wenig musikalischem Material ein vielfältiges, zusammenhängendes Werk zu entwickeln", "Motivverarbeitung erlaubt es, aus einem kleinen musikalischen Baustein (Motiv) durch Variation und Entwicklung ein vielfältiges, aber zusammenhängendes Gesamtwerk zu schaffen.")
    };

    private static QuizQuestion KompositionUndSatzweisen(Random r)
    {
        var f = KompositionUndSatzweisenListe[r.Next(KompositionUndSatzweisenListe.Length)];
        return new QuizQuestion
        {
            Id = NewId(), Subject = Subject.Musik, GradeLevel = GradeLevel.Klasse9,
            Topic = "Komposition und Satzweisen", Type = QuestionType.MultipleChoice,
            Prompt = f.Frage, Options = f.Optionen, CorrectAnswers = new[] { f.Antwort }, Explanation = f.Erklaerung,
            HelpHint = "Sonatenhauptsatzform: Exposition (vorstellen) - Durchführung (verarbeiten) - Reprise (wiederkehren). Homophonie = eine Hauptstimme, Polyphonie = mehrere gleichrangige Stimmen."
        };
    }

    private static readonly (string Frage, string[] Optionen, string Antwort, string Erklaerung)[] MedienUndDigitaleProduktionListe =
    {
        ("Was ist eine Coverversion?", new[] { "Eine Neuaufnahme oder Neuinterpretation eines bereits existierenden Musikstücks durch andere Künstler:innen", "Das allererste Original eines Musikstücks", "Ein Begriff aus der Notation von Rhythmen" },
            "Eine Neuaufnahme oder Neuinterpretation eines bereits existierenden Musikstücks durch andere Künstler:innen", "Eine Coverversion ist eine Neuaufnahme oder Neuinterpretation eines bereits existierenden Songs durch andere Künstler:innen als die ursprünglichen Urheber:innen."),
        ("Was ist ein Remix?", new[] { "Eine veränderte, oft technisch neu bearbeitete Version eines bestehenden Musikstücks", "Ein komplett neues Stück ohne jeden Bezug zu einem bestehenden Original", "Ein Begriff aus der klassischen Notenschrift" },
            "Eine veränderte, oft technisch neu bearbeitete Version eines bestehenden Musikstücks", "Ein Remix verändert und bearbeitet ein bestehendes Musikstück technisch neu, z.B. durch neue Rhythmen, Klänge oder Arrangements."),
        ("Was ist der Unterschied zwischen einem Original und einer Bearbeitung (z.B. Cover oder Remix)?", new[] { "Das Original ist die erste, ursprüngliche Version, die Bearbeitung verändert oder interpretiert es neu", "Beide Begriffe bezeichnen exakt dasselbe Stück", "Eine Bearbeitung entsteht immer vor dem Original" },
            "Das Original ist die erste, ursprüngliche Version, die Bearbeitung verändert oder interpretiert es neu", "Das Original ist die erste, ursprüngliche Fassung eines Stücks, während eine Bearbeitung wie Cover oder Remix es später verändert oder neu interpretiert."),
        ("Was ist Autotune?", new[] { "Eine digitale Software, die Gesangsstimmen automatisch auf die richtige Tonhöhe korrigiert", "Ein akustisches Instrument aus der Klassik", "Ein Begriff aus der Notation von Dynamik" },
            "Eine digitale Software, die Gesangsstimmen automatisch auf die richtige Tonhöhe korrigiert", "Autotune ist eine digitale Software, die die Tonhöhe von Gesangsaufnahmen automatisch korrigiert oder auch bewusst künstlich verfremdet."),
        ("Warum kann der Einsatz von Autotune kritisch diskutiert werden?", new[] { "Weil er echte gesangliche Unzulänglichkeiten verdecken und ein verzerrtes Bild der tatsächlichen Gesangsleistung erzeugen kann", "Weil Autotune ausschließlich bei Instrumentalmusik eingesetzt wird", "Weil Autotune gesetzlich verboten ist" },
            "Weil er echte gesangliche Unzulänglichkeiten verdecken und ein verzerrtes Bild der tatsächlichen Gesangsleistung erzeugen kann", "Kritiker:innen bemängeln, dass Autotune echte gesangliche Schwächen verdecken und so ein verzerrtes Bild der tatsächlichen Leistung einer Sängerin/eines Sängers erzeugen kann."),
        ("Was bedeutet \"digitale Klangbearbeitung\"?", new[] { "Klänge mithilfe von Software nachträglich zu verändern, z.B. durch Hall, Delay oder Filter", "Ausschließlich das reine, unbearbeitete Aufnehmen von Klängen", "Ein Begriff, der nur bei klassischer Notenschrift verwendet wird" },
            "Klänge mithilfe von Software nachträglich zu verändern, z.B. durch Hall, Delay oder Filter", "Digitale Klangbearbeitung nutzt Software, um aufgenommene Klänge nachträglich zu verändern, z.B. durch Hall, Delay oder Filter-Effekte."),
        ("Wie wird der Computer heute typischerweise als Aufnahmewerkzeug genutzt?", new[] { "Über spezielle Musiksoftware (DAW), die Aufnahme, Bearbeitung und Mischung ermöglicht", "Ausschließlich zum Abspielen bereits fertiger Musikdateien", "Der Computer wird in der Musikproduktion grundsätzlich nicht eingesetzt" },
            "Über spezielle Musiksoftware (DAW), die Aufnahme, Bearbeitung und Mischung ermöglicht", "Der Computer wird über spezielle Musiksoftware (Digital Audio Workstations) zum Aufnehmen, Bearbeiten und Mischen von Musik genutzt."),
        ("Was ist Tontechnik allgemein?", new[] { "Die Lehre und Praxis von Aufnahme, Bearbeitung und Wiedergabe von Klang", "Ausschließlich die Lehre vom Notenlesen", "Ein Begriff aus der Harmonielehre" },
            "Die Lehre und Praxis von Aufnahme, Bearbeitung und Wiedergabe von Klang", "Tontechnik umfasst die Lehre und Praxis von Aufnahme, Bearbeitung und Wiedergabe von Klang, z.B. im Tonstudio."),
        ("Was ist ein Mikrofon in der Tontechnik?", new[] { "Ein Gerät, das Schallwellen in elektrische Signale umwandelt, um sie aufzunehmen", "Ein Gerät, das ausschließlich Bilder aufzeichnet", "Ein Instrument aus der Streicherfamilie" },
            "Ein Gerät, das Schallwellen in elektrische Signale umwandelt, um sie aufzunehmen", "Ein Mikrofon wandelt Schallwellen in elektrische Signale um, die dann aufgenommen, bearbeitet oder verstärkt werden können."),
        ("Was bedeutet \"Mischen\" (Mixing) einer Musikaufnahme?", new[] { "Die einzelnen aufgenommenen Spuren in Lautstärke, Klangfarbe und Position aufeinander abzustimmen", "Ausschließlich das physische Zusammenkleben von Tonbändern", "Ein Begriff, der nur beim Live-Konzert verwendet wird" },
            "Die einzelnen aufgenommenen Spuren in Lautstärke, Klangfarbe und Position aufeinander abzustimmen", "Beim Mischen (Mixing) werden die einzelnen aufgenommenen Spuren einer Produktion in Lautstärke, Klangfarbe und Position im Klangbild aufeinander abgestimmt."),
        ("Was bedeutet \"Mastering\" einer Musikproduktion, grob gesagt?", new[] { "Der letzte Bearbeitungsschritt, der den fertigen Mix klanglich optimiert und für die Veröffentlichung vorbereitet", "Der allererste Schritt, bevor überhaupt etwas aufgenommen wird", "Ein Begriff, der ausschließlich beim Notenschreiben verwendet wird" },
            "Der letzte Bearbeitungsschritt, der den fertigen Mix klanglich optimiert und für die Veröffentlichung vorbereitet", "Mastering ist der abschließende Bearbeitungsschritt, der den fertigen Mix klanglich optimiert und für die Veröffentlichung vorbereitet."),
        ("Was ist eine DAW (Digital Audio Workstation), vereinfacht erklärt?", new[] { "Eine Software zum Aufnehmen, Bearbeiten und Mischen von Musik am Computer", "Ein physisches Musikinstrument aus der Klassik", "Ein Begriff aus der klassischen Harmonielehre" },
            "Eine Software zum Aufnehmen, Bearbeiten und Mischen von Musik am Computer", "Eine DAW ist eine Software, mit der Musik am Computer aufgenommen, bearbeitet und gemischt werden kann."),
        ("Was unterscheidet eine Coverversion grundsätzlich von einem Remix?", new[] { "Eine Coverversion ist meist eine eigene Neuinterpretation (oft live/instrumental neu eingespielt), ein Remix bearbeitet meist die vorhandene Originalaufnahme technisch neu", "Beide Begriffe bezeichnen exakt dasselbe", "Ein Remix wird immer live ohne jede Technik erstellt" },
            "Eine Coverversion ist meist eine eigene Neuinterpretation (oft live/instrumental neu eingespielt), ein Remix bearbeitet meist die vorhandene Originalaufnahme technisch neu", "Eine Coverversion wird meist neu eingespielt und interpretiert, während ein Remix meist die vorhandene Originalaufnahme technisch neu bearbeitet."),
        ("Warum kann medial produzierte Musik die Wahrnehmung von Zuhörer:innen beeinflussen?", new[] { "Weil technische Bearbeitung (z.B. Autotune, Mixing) einen perfektionierten Eindruck erzeugen kann, der von der echten Live-Leistung abweicht", "Weil medial produzierte Musik grundsätzlich keinerlei Wirkung auf Zuhörer:innen hat", "Weil nur unbearbeitete Livemusik überhaupt eine Wirkung erzeugen kann" },
            "Weil technische Bearbeitung (z.B. Autotune, Mixing) einen perfektionierten Eindruck erzeugen kann, der von der echten Live-Leistung abweicht", "Technische Bearbeitung kann einen stark perfektionierten Eindruck erzeugen, der von der tatsächlichen Live-Leistung der Musiker:innen abweichen kann - ein Grund für kritische Reflexion."),
        ("Was ist ein Beispiel für digitale Klangbearbeitung?", new[] { "Das Hinzufügen von Hall (Reverb), um einen Klang räumlicher wirken zu lassen", "Das reine, unveränderte Abspielen einer Originalaufnahme", "Das handschriftliche Notieren einer Melodie" },
            "Das Hinzufügen von Hall (Reverb), um einen Klang räumlicher wirken zu lassen", "Das Hinzufügen von Hall (Reverb) ist ein klassisches Beispiel digitaler Klangbearbeitung, um einem Klang mehr räumliche Tiefe zu verleihen."),
        ("Was bedeutet es, wenn eine Gesangsaufnahme nachträglich stark digital \"korrigiert\" wird?", new[] { "Tonhöhen- oder Timing-Fehler werden nachträglich mit Software wie Autotune angepasst", "Die Aufnahme wird komplett neu und live ohne jede Technik eingesungen", "Die Aufnahme wird ausschließlich lauter gemacht" },
            "Tonhöhen- oder Timing-Fehler werden nachträglich mit Software wie Autotune angepasst", "Eine stark digital \"korrigierte\" Gesangsaufnahme wurde nachträglich mit Software wie Autotune in Tonhöhe oder Timing angepasst."),
        ("Was ist ein Sample in der digitalen Musikproduktion?", new[] { "Ein kurzer, aufgenommener Klangausschnitt, der in einer neuen Produktion wiederverwendet wird", "Ein komplett neu komponiertes, unabhängiges Musikstück", "Ein Begriff aus der klassischen Notenschrift" },
            "Ein kurzer, aufgenommener Klangausschnitt, der in einer neuen Produktion wiederverwendet wird", "Ein Sample ist ein kurzer, aufgenommener Klangausschnitt (z.B. aus einem anderen Song), der in einer neuen musikalischen Produktion wiederverwendet wird."),
        ("Warum sollte man medial produzierte Musik kritisch reflektieren, statt sie unhinterfragt zu übernehmen?", new[] { "Weil technische Bearbeitung ein Bild erzeugen kann, das von der tatsächlichen musikalischen Leistung abweicht", "Weil medial produzierte Musik grundsätzlich fehlerfrei und unproblematisch ist", "Weil kritisches Nachdenken über Musik generell überflüssig ist" },
            "Weil technische Bearbeitung ein Bild erzeugen kann, das von der tatsächlichen musikalischen Leistung abweicht", "Kritische Reflexion hilft zu erkennen, dass technische Bearbeitung (z.B. Autotune, aufwendiges Mixing) ein Bild erzeugen kann, das von der tatsächlichen musikalischen Leistung abweicht."),
        ("Was unterscheidet eine Studioaufnahme von einer Live-Aufnahme?", new[] { "Eine Studioaufnahme entsteht meist unter kontrollierten Bedingungen mit Nachbearbeitung, eine Live-Aufnahme entsteht unmittelbar vor Publikum", "Beide Aufnahmearten sind technisch und klanglich immer identisch", "Eine Live-Aufnahme wird immer nachträglich stärker bearbeitet als eine Studioaufnahme" },
            "Eine Studioaufnahme entsteht meist unter kontrollierten Bedingungen mit Nachbearbeitung, eine Live-Aufnahme entsteht unmittelbar vor Publikum", "Eine Studioaufnahme entsteht unter kontrollierten Bedingungen mit Möglichkeit zur Nachbearbeitung, während eine Live-Aufnahme das unmittelbare Geschehen vor Publikum festhält."),
        ("Was ist ein möglicher Vorteil digitaler Klangbearbeitung für Musiker:innen?", new[] { "Sie ermöglicht neue kreative Klanggestaltung, die akustisch allein nicht möglich wäre", "Digitale Klangbearbeitung hat für Musiker:innen ausschließlich Nachteile", "Sie ersetzt komplett die Notwendigkeit, ein Instrument zu spielen" },
            "Sie ermöglicht neue kreative Klanggestaltung, die akustisch allein nicht möglich wäre", "Digitale Klangbearbeitung eröffnet Musiker:innen neue kreative Möglichkeiten der Klanggestaltung, die mit rein akustischen Mitteln nicht erreichbar wären.")
    };

    private static QuizQuestion MedienUndDigitaleProduktion(Random r)
    {
        var f = MedienUndDigitaleProduktionListe[r.Next(MedienUndDigitaleProduktionListe.Length)];
        return new QuizQuestion
        {
            Id = NewId(), Subject = Subject.Musik, GradeLevel = GradeLevel.Klasse9,
            Topic = "Medien und digitale Produktion", Type = QuestionType.MultipleChoice,
            Prompt = f.Frage, Options = f.Optionen, CorrectAnswers = new[] { f.Antwort }, Explanation = f.Erklaerung,
            HelpHint = "Cover = neu interpretiert, Remix = technisch neu bearbeitet. Autotune korrigiert Tonhöhen digital - das kann die Wahrnehmung echter Leistung verzerren."
        };
    }

    private static readonly (string Frage, string[] Optionen, string Antwort, string Erklaerung)[] GattungenDerMusikgeschichteListe =
    {
        ("Was ist ein Volkslied?", new[] { "Ein oft mündlich überliefertes Lied ohne bekannten einzelnen Komponisten, das zur Volkskultur gehört", "Ein Lied, das ausschließlich von einem einzigen, bekannten klassischen Komponisten geschrieben wurde", "Ein Begriff aus der elektronischen Musik" },
            "Ein oft mündlich überliefertes Lied ohne bekannten einzelnen Komponisten, das zur Volkskultur gehört", "Volkslieder wurden oft mündlich über Generationen weitergegeben und haben meist keinen bekannten einzelnen Komponisten - sie gehören zur Volkskultur."),
        ("Was ist ein Kunstlied?", new[] { "Ein bewusst von einem einzelnen Komponisten komponiertes, oft anspruchsvolles Lied (häufig mit Klavierbegleitung)", "Ein Lied, das ausschließlich mündlich überliefert wurde", "Ein Begriff aus der elektronischen Musik" },
            "Ein bewusst von einem einzelnen Komponisten komponiertes, oft anspruchsvolles Lied (häufig mit Klavierbegleitung)", "Ein Kunstlied wurde bewusst von einem einzelnen Komponisten komponiert, oft mit anspruchsvoller Melodie und Klavierbegleitung, z.B. bei Franz Schubert."),
        ("Was ist ein Rezitativ in der Oper?", new[] { "Ein sprechgesangartiger Teil, der die Handlung vorantreibt", "Eine besonders melodische, gefühlvolle Solo-Gesangsnummer", "Ein rein instrumentaler Zwischenteil ohne jeden Text" },
            "Ein sprechgesangartiger Teil, der die Handlung vorantreibt", "Ein Rezitativ ist ein sprechgesangartiger, textnaher Teil einer Oper, der vor allem dazu dient, die Handlung voranzutreiben."),
        ("Was ist eine Arie in der Oper?", new[] { "Eine besonders melodische, oft gefühlvolle Solo-Gesangsnummer", "Ein rein sprechgesangartiger Handlungsteil", "Ein Begriff aus der Instrumentalmusik ohne Gesang" },
            "Eine besonders melodische, oft gefühlvolle Solo-Gesangsnummer", "Eine Arie ist eine besonders melodische, oft gefühlsbetonte Solo-Gesangsnummer, die Emotionen einer Figur ausdrückt."),
        ("Was ist eine Oper?", new[] { "Ein musikalisches Bühnenwerk, das eine Handlung durch Gesang, Orchester und szenische Darstellung erzählt", "Ein rein instrumentales Konzertstück ohne jede Handlung", "Ein Begriff aus der Popmusik" },
            "Ein musikalisches Bühnenwerk, das eine Handlung durch Gesang, Orchester und szenische Darstellung erzählt", "Eine Oper ist ein musikalisches Bühnenwerk, das eine Handlung durch Gesang, Orchestermusik und szenische Darstellung erzählt."),
        ("Was ist ein Musical?", new[] { "Ein Bühnenwerk, das Gesang, Tanz und gesprochenen Dialog miteinander verbindet, oft in populärerem Musikstil", "Ein rein klassisches Instrumentalstück ohne jede Handlung", "Ein Begriff aus der Kirchenmusik" },
            "Ein Bühnenwerk, das Gesang, Tanz und gesprochenen Dialog miteinander verbindet, oft in populärerem Musikstil", "Ein Musical verbindet Gesang, Tanz und gesprochenen Dialog, meist in einem populäreren Musikstil als die klassische Oper."),
        ("Was ist ein zentraler Unterschied zwischen Oper und Musical?", new[] { "Die Oper wird meist durchgehend gesungen (oft klassischer Musikstil), das Musical verbindet Gesang oft mit gesprochenem Dialog (oft populärerer Musikstil)", "Beide Formen sind musikalisch und inhaltlich identisch", "Ein Musical enthält niemals gesprochenen Text" },
            "Die Oper wird meist durchgehend gesungen (oft klassischer Musikstil), das Musical verbindet Gesang oft mit gesprochenem Dialog (oft populärerer Musikstil)", "Opern werden meist durchgehend im klassischen Stil gesungen, während Musicals Gesang oft mit gesprochenem Dialog verbinden und häufig einen populäreren Musikstil verwenden."),
        ("Was ist experimentelles Musiktheater (Performance)?", new[] { "Eine Bühnenform, die bewusst mit ungewöhnlichen, oft nicht-klassischen musikalisch-theatralen Mitteln experimentiert", "Eine streng klassische, unveränderliche Operninszenierung", "Ein Begriff aus der reinen Instrumentalmusik ohne Bühnenbezug" },
            "Eine Bühnenform, die bewusst mit ungewöhnlichen, oft nicht-klassischen musikalisch-theatralen Mitteln experimentiert", "Experimentelles Musiktheater (Performance) bricht bewusst mit klassischen Opern-/Theaterkonventionen und experimentiert mit ungewöhnlichen musikalisch-theatralen Mitteln."),
        ("Was ist eine Sinfonie?", new[] { "Ein groß angelegtes, meist mehrsätziges Orchesterwerk", "Ein kurzes Solostück für ein einzelnes Instrument", "Ein Begriff aus der Popmusik" },
            "Ein groß angelegtes, meist mehrsätziges Orchesterwerk", "Eine Sinfonie ist ein groß angelegtes, meist mehrsätziges Werk für Orchester."),
        ("Was ist eine Sinfonische Dichtung?", new[] { "Ein einsätziges Orchesterwerk, das ein außermusikalisches Programm (Geschichte, Bild) musikalisch darstellt", "Ein rein vokales Chorwerk ohne jedes Orchester", "Ein Begriff aus dem Jazz" },
            "Ein einsätziges Orchesterwerk, das ein außermusikalisches Programm (Geschichte, Bild) musikalisch darstellt", "Eine Sinfonische Dichtung ist ein meist einsätziges Orchesterwerk, das ein außermusikalisches Programm - etwa eine Geschichte oder ein Bild - musikalisch umsetzt."),
        ("Was ist ein Solokonzert als Musikform?", new[] { "Ein Werk, bei dem ein Soloinstrument im Wechselspiel mit einem Orchester im Mittelpunkt steht", "Ein Werk ausschließlich für ein einzelnes Soloinstrument ohne jede Begleitung", "Ein Begriff aus der Popmusik" },
            "Ein Werk, bei dem ein Soloinstrument im Wechselspiel mit einem Orchester im Mittelpunkt steht", "Ein Solokonzert stellt ein Soloinstrument in den Mittelpunkt, das im Wechselspiel mit einem begleitenden Orchester musiziert."),
        ("Was ist ein typisches stilistisches Merkmal des Jazz?", new[] { "Improvisation und ein charakteristischer Swing-Rhythmus", "Ausschließlich streng notierte, nie improvisierte Musik", "Vollständiger Verzicht auf jede Form von Rhythmus" },
            "Improvisation und ein charakteristischer Swing-Rhythmus", "Jazz zeichnet sich u.a. durch Improvisation und einen charakteristischen Swing-Rhythmus aus."),
        ("Was bedeutet \"Swing-Rhythmus\" im Jazz?", new[] { "Eine charakteristische, leicht \"schwingende\" rhythmische Betonung, die vom geraden Grundschlag abweicht", "Ein streng gerader, mechanischer Rhythmus ohne jede Betonungsverschiebung", "Ein Begriff, der ausschließlich in der Klassik verwendet wird" },
            "Eine charakteristische, leicht \"schwingende\" rhythmische Betonung, die vom geraden Grundschlag abweicht", "Der Swing-Rhythmus im Jazz erzeugt durch eine charakteristische, leicht \"schwingende\" Betonung ein typisches Groove-Gefühl, das vom geraden Grundschlag abweicht."),
        ("Was bedeutet Improvisation im Jazz?", new[] { "Spontanes, nicht vollständig vorher festgelegtes Musizieren innerhalb eines gegebenen musikalischen Rahmens", "Das exakte Nachspielen einer vollständig notierten Partitur", "Ein Begriff, der im Jazz keine Rolle spielt" },
            "Spontanes, nicht vollständig vorher festgelegtes Musizieren innerhalb eines gegebenen musikalischen Rahmens", "Improvisation im Jazz bedeutet, innerhalb eines gegebenen musikalischen Rahmens (z.B. Akkordfolge) spontan neue musikalische Ideen zu entwickeln."),
        ("Was sind \"Blue Notes\" im Jazz?", new[] { "Charakteristisch leicht \"verzogene\" (erniedrigte) Töne, die dem Jazz und Blues einen typischen Klangcharakter geben", "Töne, die ausschließlich in der Barockmusik verwendet werden", "Ein Begriff aus der Notation von Rhythmen" },
            "Charakteristisch leicht \"verzogene\" (erniedrigte) Töne, die dem Jazz und Blues einen typischen Klangcharakter geben", "Blue Notes sind charakteristisch leicht erniedrigte Töne (meist die 3., 5. oder 7. Stufe), die Jazz und Blues ihren typischen, oft melancholischen Klangcharakter geben."),
        ("Was ist ein typisches Merkmal der Rockmusik?", new[] { "Der starke Einsatz von E-Gitarren, Bass und Schlagzeug mit oft kraftvollem Rhythmus", "Der vollständige Verzicht auf jedes elektrisch verstärkte Instrument", "Ausschließlich rein akustische, unverstärkte Instrumente" },
            "Der starke Einsatz von E-Gitarren, Bass und Schlagzeug mit oft kraftvollem Rhythmus", "Rockmusik ist typischerweise geprägt vom starken Einsatz elektrisch verstärkter Instrumente wie E-Gitarre und Bass sowie Schlagzeug mit oft kraftvollem Rhythmus."),
        ("Was ist ein typisches Merkmal der Popmusik?", new[] { "Eingängige Melodien und oft einfache, klare Songstrukturen (z.B. Strophe-Refrain)", "Ausschließlich extrem komplexe, schwer eingängige Melodien", "Vollständiger Verzicht auf jede erkennbare Songstruktur" },
            "Eingängige Melodien und oft einfache, klare Songstrukturen (z.B. Strophe-Refrain)", "Popmusik zeichnet sich typischerweise durch eingängige Melodien und oft einfache, klare Songstrukturen wie Strophe und Refrain aus."),
        ("Was unterscheidet ein Rezitativ musikalisch von einer Arie?", new[] { "Ein Rezitativ ist sprechgesangartig und textnah, eine Arie ist melodisch ausgestaltet und gefühlsbetont", "Beide sind musikalisch exakt identisch", "Eine Arie ist immer sprechgesangartig, ein Rezitativ immer stark melodisch" },
            "Ein Rezitativ ist sprechgesangartig und textnah, eine Arie ist melodisch ausgestaltet und gefühlsbetont", "Während ein Rezitativ sprechgesangartig und textnah die Handlung vorantreibt, ist eine Arie melodisch ausgestaltet und drückt Gefühle aus."),
        ("Was unterscheidet ein Volkslied grundsätzlich von einem Kunstlied?", new[] { "Ein Volkslied ist meist mündlich überliefert ohne bekannten Komponisten, ein Kunstlied wurde bewusst von einem einzelnen Komponisten geschrieben", "Beide Liedarten sind in ihrer Entstehung identisch", "Ein Kunstlied wird immer mündlich überliefert" },
            "Ein Volkslied ist meist mündlich überliefert ohne bekannten Komponisten, ein Kunstlied wurde bewusst von einem einzelnen Komponisten geschrieben", "Ein Volkslied wurde meist mündlich überliefert und hat keinen bekannten Einzelkomponisten, während ein Kunstlied bewusst von einer einzelnen Person komponiert wurde."),
        ("Warum vergleicht man Oper und Musical oft hinsichtlich musikalischer, inhaltlicher und historischer Merkmale?", new[] { "Weil beide Bühnenformen Musik und Handlung verbinden, sich aber in Stil, Entstehungszeit und Publikum unterscheiden", "Weil beide Formen in jeder Hinsicht exakt identisch sind", "Weil ein Vergleich zwischen beiden Formen musikalisch keinen Sinn ergibt" },
            "Weil beide Bühnenformen Musik und Handlung verbinden, sich aber in Stil, Entstehungszeit und Publikum unterscheiden", "Oper und Musical verbinden beide Musik mit Bühnenhandlung, unterscheiden sich aber deutlich in musikalischem Stil, historischer Entstehungszeit und Zielpublikum - ein Vergleich macht diese Unterschiede sichtbar.")
    };

    private static QuizQuestion GattungenDerMusikgeschichte(Random r)
    {
        var f = GattungenDerMusikgeschichteListe[r.Next(GattungenDerMusikgeschichteListe.Length)];
        return new QuizQuestion
        {
            Id = NewId(), Subject = Subject.Musik, GradeLevel = GradeLevel.Klasse9,
            Topic = "Gattungen und Genres der Musikgeschichte", Type = QuestionType.MultipleChoice,
            Prompt = f.Frage, Options = f.Optionen, CorrectAnswers = new[] { f.Antwort }, Explanation = f.Erklaerung,
            HelpHint = "Rezitativ = sprechgesangartig, Arie = melodisch-gefühlvoll. Oper vs. Musical: klassischer Gesang vs. Gesang+Dialog. Jazz: Swing, Improvisation, Blue Notes."
        };
    }

    private static readonly (string Frage, string[] Optionen, string Antwort, string Erklaerung)[] FilmmusikUndProgrammmusikListe =
    {
        ("Was ist Filmmusik?", new[] { "Musik, die speziell zur Untermalung und Verstärkung der Handlung eines Films komponiert oder ausgewählt wird", "Musik, die ausschließlich unabhängig vom Film im Konzertsaal aufgeführt wird", "Ein Begriff aus der reinen Kammermusik" },
            "Musik, die speziell zur Untermalung und Verstärkung der Handlung eines Films komponiert oder ausgewählt wird", "Filmmusik wird speziell komponiert oder ausgewählt, um die Handlung, Stimmung und Emotionen eines Films musikalisch zu unterstützen."),
        ("Was ist die Leitmotivtechnik in der Filmmusik?", new[] { "Einer Figur, einem Ort oder einer Idee wird ein wiederkehrendes musikalisches Thema zugeordnet", "Jede Szene erhält ein komplett neues, niemals wiederkehrendes Thema", "Ein Begriff, der ausschließlich im Jazz verwendet wird" },
            "Einer Figur, einem Ort oder einer Idee wird ein wiederkehrendes musikalisches Thema zugeordnet", "Bei der Leitmotivtechnik erhält eine Figur, ein Ort oder eine Idee ein wiederkehrendes musikalisches Thema, das sie im gesamten Film erkennbar macht."),
        ("Was ist \"Underscoring\" in der Filmmusik?", new[] { "Musik, die dezent im Hintergrund läuft, um die Stimmung einer Szene zu unterstützen", "Musik, die ausschließlich lauter als der Dialog gespielt wird", "Ein Begriff aus der Notenschrift für Vorzeichen" },
            "Musik, die dezent im Hintergrund läuft, um die Stimmung einer Szene zu unterstützen", "Underscoring bezeichnet Filmmusik, die dezent im Hintergrund einer Szene läuft, um deren Stimmung zu unterstreichen, ohne in den Vordergrund zu treten."),
        ("Was ist \"Mood-Technik\" in der Filmmusik?", new[] { "Musik wird gezielt eingesetzt, um eine bestimmte Grundstimmung einer Szene zu erzeugen", "Musik wird ausschließlich zufällig ohne jede Überlegung zur Szene ausgewählt", "Ein Begriff, der ausschließlich Musical-Produktionen betrifft" },
            "Musik wird gezielt eingesetzt, um eine bestimmte Grundstimmung einer Szene zu erzeugen", "Bei der Mood-Technik wird Musik gezielt eingesetzt, um eine bestimmte emotionale Grundstimmung einer Filmszene zu erzeugen."),
        ("Was ist Tonmalerei?", new[] { "Musikalische Darstellung außermusikalischer Bilder oder Ereignisse durch klangliche Mittel", "Ausschließlich das Anmalen von Notenblättern mit Farbe", "Ein Begriff, der nur bei elektronischer Musik verwendet wird" },
            "Musikalische Darstellung außermusikalischer Bilder oder Ereignisse durch klangliche Mittel", "Tonmalerei stellt außermusikalische Bilder oder Ereignisse (z.B. ein Gewitter) durch klangliche musikalische Mittel dar."),
        ("Was ist Tonsymbolik?", new[] { "Bestimmte Töne oder Klänge stehen symbolisch für eine außermusikalische Bedeutung", "Ein Begriff, der ausschließlich die Lautstärke eines Stücks beschreibt", "Ein Synonym für eine Tonleiter" },
            "Bestimmte Töne oder Klänge stehen symbolisch für eine außermusikalische Bedeutung", "Bei der Tonsymbolik stehen bestimmte Töne oder Klänge symbolisch für eine außermusikalische Bedeutung, z.B. hohe Töne für Licht oder tiefe Töne für Bedrohung."),
        ("Was bedeutet ein \"musikalisches Signal\", z.B. eine Fanfare?", new[] { "Ein kurzes musikalisches Zeichen, das eine bestimmte Ankündigung oder Bedeutung transportiert", "Ein rein zufälliges, bedeutungsloses musikalisches Element", "Ein Begriff aus der Harmonielehre" },
            "Ein kurzes musikalisches Zeichen, das eine bestimmte Ankündigung oder Bedeutung transportiert", "Ein musikalisches Signal wie eine Fanfare ist ein kurzes, oft feierliches musikalisches Zeichen, das eine bestimmte Ankündigung oder Bedeutung transportiert."),
        ("Was ist das \"Wort-Ton-Verhältnis\" in einem Vokalstück?", new[] { "Das Zusammenspiel zwischen gesungenem Text und musikalischer Gestaltung", "Ausschließlich die Anzahl der Worte in einem Songtext", "Ein Begriff aus der reinen Instrumentalmusik" },
            "Das Zusammenspiel zwischen gesungenem Text und musikalischer Gestaltung", "Das Wort-Ton-Verhältnis beschreibt, wie gesungener Text und musikalische Gestaltung (Melodie, Rhythmus, Dynamik) in einem Vokalstück zusammenwirken."),
        ("Was ist ein Videoclip als Musikform?", new[] { "Eine visuelle, oft künstlerisch gestaltete Begleitung zu einem Musikstück", "Ein rein instrumentales Konzertstück ohne jedes Bildmaterial", "Ein Begriff aus der klassischen Notenschrift" },
            "Eine visuelle, oft künstlerisch gestaltete Begleitung zu einem Musikstück", "Ein Videoclip ist eine visuelle, oft künstlerisch gestaltete Begleitung zu einem Musikstück, die dessen Wirkung und Verbreitung unterstützt."),
        ("Was bedeutet \"Musik als Industriezweig\"?", new[] { "Musikproduktion, -vertrieb und -vermarktung als bedeutender wirtschaftlicher Wirtschaftsbereich", "Musik hat grundsätzlich keinerlei wirtschaftliche Bedeutung", "Ein Begriff, der ausschließlich klassische Musik betrifft" },
            "Musikproduktion, -vertrieb und -vermarktung als bedeutender wirtschaftlicher Wirtschaftsbereich", "\"Musik als Industriezweig\" betrachtet Produktion, Vertrieb und Vermarktung von Musik als bedeutenden wirtschaftlichen Wirtschaftsbereich."),
        ("Was ist ein Beispiel für politische Musik?", new[] { "Ein Protestsong, der gegen soziale Missstände oder für politische Veränderung eintritt", "Ein rein privates Wiegenlied ohne jeden politischen Bezug", "Ein Begriff, der ausschließlich Nationalhymnen bezeichnet" },
            "Ein Protestsong, der gegen soziale Missstände oder für politische Veränderung eintritt", "Ein Protestsong ist ein klassisches Beispiel politischer Musik, der gegen soziale Missstände protestiert oder für politische Veränderungen eintritt."),
        ("Was ist ein Beispiel für religiöse Musik?", new[] { "Ein Kirchenlied oder ein liturgischer Gesang im Gottesdienst", "Ein reiner Werbejingle für ein kommerzielles Produkt", "Ein rein instrumentales Jazz-Solostück" },
            "Ein Kirchenlied oder ein liturgischer Gesang im Gottesdienst", "Kirchenlieder oder liturgische Gesänge im Gottesdienst sind klassische Beispiele für religiöse Musik."),
        ("Was ist ein zentraler Unterschied zwischen Filmmusik und (konzertanter) Programmmusik?", new[] { "Filmmusik begleitet unmittelbar bewegte Bilder, Programmmusik wird meist unabhängig im Konzertsaal aufgeführt", "Beide Begriffe bezeichnen exakt dasselbe", "Programmmusik begleitet immer einen Film" },
            "Filmmusik begleitet unmittelbar bewegte Bilder, Programmmusik wird meist unabhängig im Konzertsaal aufgeführt", "Filmmusik ist unmittelbar mit bewegten Bildern verknüpft, während klassische Programmmusik meist eigenständig im Konzertsaal aufgeführt wird, obwohl sie ebenfalls außermusikalische Inhalte darstellt."),
        ("Wie kann Filmmusik die emotionale Wahrnehmung einer Filmszene steuern?", new[] { "Durch Tempo, Dynamik und Klangfarbe kann sie Spannung, Freude oder Angst gezielt verstärken", "Filmmusik hat grundsätzlich keinerlei Einfluss auf die Wahrnehmung einer Szene", "Ausschließlich durch gesprochene Kommentare, nie durch Musik selbst" },
            "Durch Tempo, Dynamik und Klangfarbe kann sie Spannung, Freude oder Angst gezielt verstärken", "Filmmusik kann durch Tempo, Dynamik und Klangfarbe gezielt Emotionen wie Spannung, Freude oder Angst bei den Zuschauenden verstärken."),
        ("Was ist ein Beispiel für Tonmalerei?", new[] { "Hohe, flatternde Töne, die Vogelgezwitscher musikalisch nachahmen", "Ein rein abstraktes Musikstück ohne jeden Bezug zu einem außermusikalischen Bild", "Ein Begriff aus der Notation von Vorzeichen" },
            "Hohe, flatternde Töne, die Vogelgezwitscher musikalisch nachahmen", "Hohe, flatternde Töne, die an Vogelgezwitscher erinnern, sind ein klassisches Beispiel für Tonmalerei."),
        ("Was bedeutet es, wenn Musik als \"Industriezweig\" mit wirtschaftlicher Dimension betrachtet wird?", new[] { "Musikproduktion und -vermarktung schaffen Arbeitsplätze und wirtschaftlichen Wert", "Musik hat in dieser Betrachtung ausschließlich künstlerischen, nie wirtschaftlichen Wert", "Dieser Begriff bezieht sich ausschließlich auf klassische Musik vor 1900" },
            "Musikproduktion und -vermarktung schaffen Arbeitsplätze und wirtschaftlichen Wert", "Als Industriezweig betrachtet schafft die Musikbranche - von Produktion bis Vermarktung - Arbeitsplätze und wirtschaftlichen Wert."),
        ("Warum ist die Musikindustrie wirtschaftlich bedeutsam?", new[] { "Weil sie Arbeitsplätze schafft und durch Verkauf, Streaming und Konzerte erhebliche Umsätze generiert", "Weil Musik grundsätzlich kostenlos und ohne jeden wirtschaftlichen Wert ist", "Weil die Musikindustrie ausschließlich von staatlichen Subventionen lebt" },
            "Weil sie Arbeitsplätze schafft und durch Verkauf, Streaming und Konzerte erhebliche Umsätze generiert", "Die Musikindustrie ist wirtschaftlich bedeutsam, weil sie Arbeitsplätze schafft und durch Verkauf, Streaming-Dienste und Konzerte erhebliche Umsätze generiert."),
        ("Was ist ein Beispiel für ein musikalisches Leitmotiv?", new[] { "Ein wiederkehrendes Thema, das im gesamten Film mit einer bestimmten Figur assoziiert wird", "Ein Thema, das in einem Film nur genau einmal erklingen darf", "Ein Begriff, der ausschließlich in der Popmusik verwendet wird" },
            "Ein wiederkehrendes Thema, das im gesamten Film mit einer bestimmten Figur assoziiert wird", "Ein Leitmotiv ist ein wiederkehrendes musikalisches Thema, das im gesamten Film mit einer bestimmten Figur, einem Ort oder einer Idee assoziiert wird."),
        ("Wie unterscheidet sich die Funktion von Musik in einem Videoclip von der in einem klassischen Konzertstück?", new[] { "Im Videoclip unterstützt Musik oft eine visuelle Erzählung, im Konzertstück steht die Musik meist für sich allein", "Beide Funktionen sind immer exakt identisch", "Ein Konzertstück wird immer mit einem begleitenden Videoclip aufgeführt" },
            "Im Videoclip unterstützt Musik oft eine visuelle Erzählung, im Konzertstück steht die Musik meist für sich allein", "Im Videoclip unterstützt und ergänzt Musik oft eine visuelle Erzählung, während sie in einem klassischen Konzertstück meist unabhängig für sich selbst wirkt."),
        ("Warum wird Musik manchmal gezielt für politische oder religiöse Zwecke eingesetzt?", new[] { "Weil sie Emotionen wecken und Gemeinschaftsgefühl oder Überzeugungen wirkungsvoll vermitteln kann", "Weil Musik für solche Zwecke gesetzlich vorgeschrieben ist", "Weil Musik in diesen Kontexten grundsätzlich keinerlei Wirkung entfaltet" },
            "Weil sie Emotionen wecken und Gemeinschaftsgefühl oder Überzeugungen wirkungsvoll vermitteln kann", "Musik kann gezielt für politische oder religiöse Zwecke eingesetzt werden, weil sie Emotionen weckt und Gemeinschaftsgefühl oder Überzeugungen wirkungsvoll vermitteln kann.")
    };

    private static QuizQuestion FilmmusikUndProgrammmusik(Random r)
    {
        var f = FilmmusikUndProgrammmusikListe[r.Next(FilmmusikUndProgrammmusikListe.Length)];
        return new QuizQuestion
        {
            Id = NewId(), Subject = Subject.Musik, GradeLevel = GradeLevel.Klasse9,
            Topic = "Filmmusik und Programmmusik", Type = QuestionType.MultipleChoice,
            Prompt = f.Frage, Options = f.Optionen, CorrectAnswers = new[] { f.Antwort }, Explanation = f.Erklaerung,
            HelpHint = "Leitmotiv = wiederkehrendes Thema für eine Figur; Tonmalerei stellt außermusikalische Bilder klanglich dar; Musik wirkt auch politisch, religiös und wirtschaftlich."
        };
    }

    private static readonly (string Frage, string[] Optionen, string Antwort, string Erklaerung)[] MusikImGesellschaftlichenKontextListe =
    {
        ("Was zeigt ein Epochenüberblick der Musikgeschichte typischerweise?", new[] { "Die grobe zeitliche Abfolge musikalischer Epochen wie Barock, Klassik, Romantik und Moderne", "Ausschließlich die Biografie eines einzigen Komponisten", "Ein Begriff, der ausschließlich die Gegenwart betrifft" },
            "Die grobe zeitliche Abfolge musikalischer Epochen wie Barock, Klassik, Romantik und Moderne", "Ein Epochenüberblick zeigt die grobe zeitliche Abfolge musikalischer Epochen, z.B. Barock, Klassik, Romantik und Moderne, mit ihren jeweiligen Stilmerkmalen."),
        ("Was ist ein Beispiel für einen aktuellen Musikberuf?", new[] { "Musikproduzent:in", "Ausschließlich Bäcker:in", "Ausschließlich Automechaniker:in" },
            "Musikproduzent:in", "Musikproduzent:in ist ein Beispiel für einen aktuellen Beruf in der Musikbranche, der Aufnahme, Gestaltung und Produktion von Musik umfasst."),
        ("Was bedeutet \"Geschlechterstereotype in der Musik\"?", new[] { "Vorurteile darüber, welche Instrumente, Genres oder Berufe angeblich zu einem bestimmten Geschlecht \"passen\"", "Eine gesetzlich festgelegte Regel, welches Geschlecht welches Instrument spielen darf", "Ein Begriff ohne Bezug zur Musikwelt" },
            "Vorurteile darüber, welche Instrumente, Genres oder Berufe angeblich zu einem bestimmten Geschlecht \"passen\"", "Geschlechterstereotype in der Musik sind Vorurteile darüber, welche Instrumente, Musikgenres oder Berufe angeblich besser zu einem bestimmten Geschlecht passen - etwa die Annahme, Dirigieren sei \"männlich\"."),
        ("Was ist eine Musikszene/Jugendkultur?", new[] { "Eine Gruppe mit gemeinsamem musikalischem Stil, Werten und oft eigenem äußeren Erscheinungsbild", "Eine staatliche Institution zur Musikförderung", "Ein Begriff, der ausschließlich klassische Musik betrifft" },
            "Eine Gruppe mit gemeinsamem musikalischem Stil, Werten und oft eigenem äußeren Erscheinungsbild", "Eine Musikszene/Jugendkultur ist eine Gruppe, die sich über einen gemeinsamen musikalischen Stil, gemeinsame Werte und oft ein eigenes äußeres Erscheinungsbild verbunden fühlt."),
        ("Was bedeutet \"Musik und Herrschaft\"?", new[] { "Musik wurde historisch oft genutzt, um die Macht und Bedeutung eines Herrschers/Regimes zu demonstrieren", "Musik hatte in der Geschichte niemals einen Bezug zu Macht oder Herrschaft", "Ein Begriff, der ausschließlich moderne Popmusik betrifft" },
            "Musik wurde historisch oft genutzt, um die Macht und Bedeutung eines Herrschers/Regimes zu demonstrieren", "Musik wurde historisch oft genutzt, um die Macht, den Reichtum und die Bedeutung eines Herrschers oder Regimes zu demonstrieren, z.B. durch aufwendige Hofmusik."),
        ("Was bedeutet \"Musik als Protest\"?", new[] { "Musik wird gezielt genutzt, um gegen Missstände zu protestieren oder gesellschaftliche Veränderung zu fordern", "Musik hat grundsätzlich keinerlei Bezug zu politischem Protest", "Ein Begriff, der ausschließlich klassische Musik betrifft" },
            "Musik wird gezielt genutzt, um gegen Missstände zu protestieren oder gesellschaftliche Veränderung zu fordern", "Musik als Protest wird gezielt eingesetzt, um gegen gesellschaftliche Missstände zu protestieren oder Veränderung einzufordern, z.B. in Protestsongs."),
        ("Was ist Weltmusik?", new[] { "Musik, die Elemente unterschiedlicher, oft nicht-westlicher musikalischer Traditionen einbezieht", "Musik, die ausschließlich aus einem einzigen Land stammt", "Ein Begriff, der ausschließlich klassische europäische Musik bezeichnet" },
            "Musik, die Elemente unterschiedlicher, oft nicht-westlicher musikalischer Traditionen einbezieht", "Weltmusik bezeichnet Musik, die Elemente unterschiedlicher, oft nicht-westlicher musikalischer Traditionen aus aller Welt einbezieht."),
        ("Was ist Ethno-Pop?", new[] { "Eine Musikrichtung, die traditionelle, ethnische Musikelemente mit modernen Popmusik-Stilmitteln verbindet", "Eine Musikrichtung, die ausschließlich klassische Instrumente ohne jede moderne Technik verwendet", "Ein Begriff aus der Barockmusik" },
            "Eine Musikrichtung, die traditionelle, ethnische Musikelemente mit modernen Popmusik-Stilmitteln verbindet", "Ethno-Pop verbindet traditionelle, ethnische Musikelemente mit modernen Popmusik-Stilmitteln zu einer neuen musikalischen Mischform."),
        ("Was bedeutet \"Musik und Globalisierung\"?", new[] { "Musikstile verbreiten sich heute weltweit schnell, u.a. über Streaming-Dienste und das Internet", "Musik bleibt trotz weltweiter Vernetzung ausschließlich lokal begrenzt", "Ein Begriff, der mit Musik nichts zu tun hat" },
            "Musikstile verbreiten sich heute weltweit schnell, u.a. über Streaming-Dienste und das Internet", "Durch Globalisierung und digitale Verbreitung (Streaming, Internet) verbreiten sich Musikstile heute schneller und weltweit als früher."),
        ("Was ist ein Beispiel für eine Jugendkultur mit eigener, unverwechselbarer Musikszene?", new[] { "Die Hip-Hop-Kultur", "Eine rein private Familienfeier ohne jeden musikalischen Bezug", "Ein staatliches Bildungsprogramm" },
            "Die Hip-Hop-Kultur", "Die Hip-Hop-Kultur ist ein bekanntes Beispiel für eine Jugendkultur mit eigener, unverwechselbarer Musikszene, eigenem Stil und eigenen Werten."),
        ("Warum können Geschlechterstereotype die Berufswahl im Musikbereich beeinflussen?", new[] { "Weil Vorurteile darüber, was \"typisch männlich/weiblich\" sei, die eigene Berufswahl unbewusst einschränken können", "Weil Geschlechterstereotype in der Musikwelt grundsätzlich keine Rolle spielen", "Weil es gesetzlich vorgeschrieben ist, welches Geschlecht welchen Musikberuf ausüben darf" },
            "Weil Vorurteile darüber, was \"typisch männlich/weiblich\" sei, die eigene Berufswahl unbewusst einschränken können", "Vorurteile darüber, welche Instrumente oder Berufe angeblich zu einem bestimmten Geschlecht passen, können die eigene Berufswahl unbewusst einschränken - z.B. wenn Mädchen seltener zum Dirigieren ermutigt werden."),
        ("Was macht ein Tontechniker/eine Tontechnikerin als Musikberuf?", new[] { "Er/sie nimmt Musik auf, bearbeitet und mischt sie technisch", "Er/sie schreibt ausschließlich klassische Kompositionen", "Er/sie unterrichtet ausschließlich Notenlesen an Schulen" },
            "Er/sie nimmt Musik auf, bearbeitet und mischt sie technisch", "Ein Tontechniker/eine Tontechnikerin ist für die technische Aufnahme, Bearbeitung und Mischung von Musik zuständig."),
        ("Was ist ein historisches Beispiel dafür, wie Herrscher Musik zur Machtdemonstration genutzt haben?", new[] { "Aufwendige Hofmusik und Hofkapellen an europäischen Fürstenhöfen", "Ausschließlich private, unhörbare Musik ohne jedes Publikum", "Ein Verbot jeglicher Musik am Hof" },
            "Aufwendige Hofmusik und Hofkapellen an europäischen Fürstenhöfen", "Aufwendige Hofmusik und eigene Hofkapellen an europäischen Fürstenhöfen dienten historisch oft dazu, Macht und Prestige des Herrschers zu demonstrieren."),
        ("Was ist ein historisches Beispiel für Musik als politisches Protestmittel?", new[] { "Protestsongs der Bürgerrechtsbewegung oder gegen Kriege", "Eine rein private Wiegenliedmusik ohne jeden politischen Bezug", "Eine klassische Sinfonie ohne jeden Text" },
            "Protestsongs der Bürgerrechtsbewegung oder gegen Kriege", "Protestsongs, etwa im Rahmen der Bürgerrechtsbewegung oder gegen Kriege, sind historische Beispiele für Musik als politisches Protestmittel."),
        ("Was bedeutet kulturelle Vermischung in der Weltmusik?", new[] { "Musikalische Elemente unterschiedlicher Kulturen werden miteinander kombiniert", "Musik bleibt in der Weltmusik immer streng auf eine einzige Kultur beschränkt", "Ein Begriff, der ausschließlich klassische Musik betrifft" },
            "Musikalische Elemente unterschiedlicher Kulturen werden miteinander kombiniert", "Kulturelle Vermischung in der Weltmusik bedeutet, dass musikalische Elemente unterschiedlicher Kulturen und Traditionen miteinander kombiniert werden."),
        ("Wie hängen Musik und Globalisierung zusammen?", new[] { "Streaming-Dienste und das Internet ermöglichen die schnelle weltweite Verbreitung von Musikstilen", "Musik verbreitet sich seit jeher ausschließlich lokal begrenzt", "Globalisierung hat auf Musik keinerlei Einfluss" },
            "Streaming-Dienste und das Internet ermöglichen die schnelle weltweite Verbreitung von Musikstilen", "Durch Streaming-Dienste und das Internet können sich Musikstile heute deutlich schneller und weltweiter verbreiten als vor der Globalisierung."),
        ("Was ist ein Beispiel für Ethno-Pop?", new[] { "Ein Popsong, der traditionelle Instrumente oder Melodien einer bestimmten Kultur mit modernen Popmusik-Elementen verbindet", "Ein rein klassisches Orchesterstück ohne jeden Popbezug", "Ein Begriff aus der reinen Kirchenmusik" },
            "Ein Popsong, der traditionelle Instrumente oder Melodien einer bestimmten Kultur mit modernen Popmusik-Elementen verbindet", "Ein Ethno-Pop-Song verbindet traditionelle Instrumente oder Melodien einer bestimmten Kultur mit modernen Popmusik-Elementen."),
        ("Warum verändern sich Musikszenen/Jugendkulturen im Lauf der Zeit?", new[] { "Weil sich gesellschaftliche Werte, Technik und musikalischer Geschmack ständig weiterentwickeln", "Weil Musikszenen gesetzlich alle paar Jahre neu festgelegt werden", "Weil sich Jugendkulturen seit ihrer Entstehung nie verändert haben" },
            "Weil sich gesellschaftliche Werte, Technik und musikalischer Geschmack ständig weiterentwickeln", "Musikszenen und Jugendkulturen verändern sich, weil sich gesellschaftliche Werte, Technik und musikalischer Geschmack im Lauf der Zeit ständig weiterentwickeln."),
        ("Was ist ein Beispiel für einen modernen Musikberuf im digitalen Bereich?", new[] { "Musikproduzent:in, die/der digitale Aufnahmen erstellt und bearbeitet", "Ausschließlich ein Beruf aus dem 18. Jahrhundert ohne jeden digitalen Bezug", "Ein Beruf, der mit Musik nichts zu tun hat" },
            "Musikproduzent:in, die/der digitale Aufnahmen erstellt und bearbeitet", "Musikproduzent:in ist ein Beispiel für einen modernen Musikberuf, der digitale Aufnahme- und Bearbeitungstechnik nutzt."),
        ("Warum ist es wichtig, Musikgeschichte im gesellschaftlichen Kontext zu betrachten, statt sie isoliert zu sehen?", new[] { "Weil Musik immer auch gesellschaftliche, politische und kulturelle Zusammenhänge widerspiegelt", "Weil Musikgeschichte grundsätzlich keinerlei Bezug zur Gesellschaft hat", "Weil eine gesellschaftliche Einordnung das Verständnis von Musik unmöglich macht" },
            "Weil Musik immer auch gesellschaftliche, politische und kulturelle Zusammenhänge widerspiegelt", "Musikgeschichte im gesellschaftlichen Kontext zu betrachten hilft zu verstehen, dass Musik immer auch gesellschaftliche, politische und kulturelle Zusammenhänge ihrer Zeit widerspiegelt.")
    };

    private static QuizQuestion MusikImGesellschaftlichenKontext(Random r)
    {
        var f = MusikImGesellschaftlichenKontextListe[r.Next(MusikImGesellschaftlichenKontextListe.Length)];
        return new QuizQuestion
        {
            Id = NewId(), Subject = Subject.Musik, GradeLevel = GradeLevel.Klasse9,
            Topic = "Musik im kulturellen und gesellschaftlichen Kontext", Type = QuestionType.MultipleChoice,
            Prompt = f.Frage, Options = f.Optionen, CorrectAnswers = new[] { f.Antwort }, Explanation = f.Erklaerung,
            HelpHint = "Musik spiegelt Gesellschaft wider: Herrschaft, Protest, Jugendkulturen, Geschlechterstereotype und Globalisierung prägen, wie und welche Musik entsteht."
        };
    }
}
