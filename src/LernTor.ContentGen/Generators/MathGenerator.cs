using LernTor.Core.Enums;
using LernTor.Core.Models;

namespace LernTor.ContentGen.Generators;

/// <summary>
/// Mathematik-Aufgabengenerator nach Berliner Rahmenlehrplan, Klasse 6 (Grundschule/ISS)
/// und Klasse 9 (Gymnasium). Jede Aufgabe wird mit zufälligen, aber sinnvoll begrenzten
/// Werten neu erzeugt und liefert einen vollständigen Lösungsweg.
/// </summary>
public sealed class MathGenerator : ExerciseGeneratorBase
{
    public override Subject Subject => Subject.Mathematik;

    protected override IReadOnlyDictionary<GradeLevel, IReadOnlyList<TopicFactory>> TopicsByGrade { get; } =
        new Dictionary<GradeLevel, IReadOnlyList<TopicFactory>>
        {
            [GradeLevel.Klasse6] = new List<TopicFactory>
            {
                BruchAddition,
                BruchMultiplikation,
                ProzentGrundwert,
                NegativeZahlenAddition,
                RechteckFlaeche,
                Massstab,
                WahrscheinlichkeitWuerfel,
                QuaderVolumen,
                BruchDezimalUmwandlung,
                ProportionaleZuordnung,
                Kongruenzabbildungen,
                Kombinatorik
            },
            [GradeLevel.Klasse7] = new List<TopicFactory>
            {
                RationaleZahlenRechnen,
                ProzentwertBerechnen,
                EinfacheZinsen,
                TermeZusammenfassen,
                EinfacheGleichungen,
                DreisatzProportional,
                WinkelBerechnen,
                FlaechenVielecke,
                WahrscheinlichkeitUrne
            },
            [GradeLevel.Klasse9] = new List<TopicFactory>
            {
                LineareGleichung,
                LineareFunktion,
                QuadratischeGleichung,
                SatzDesPythagoras,
                Zinsrechnung,
                BinomischeFormel,
                MittelwertUndMedian,
                Trigonometrie,
                SatzDesThales,
                PyramideKegelKugelVolumen,
                LinearesGleichungssystem,
                QuadratischeFunktionMerkmale,
                Exponentialfunktion,
                Potenzgesetze
            }
        };

    private static int Gcd(int a, int b) => b == 0 ? Math.Abs(a) : Gcd(b, a % b);

    private static (int Zaehler, int Nenner) Reduce(int zaehler, int nenner)
    {
        var g = Gcd(zaehler, nenner);
        if (g == 0) g = 1;
        if (nenner < 0) { nenner = -nenner; zaehler = -zaehler; }
        return (zaehler / g, nenner / g);
    }

    private static QuizQuestion BruchAddition(Random r)
    {
        int n1 = r.Next(2, 9), n2 = r.Next(2, 9);
        int z1 = r.Next(1, n1), z2 = r.Next(1, n2);

        int commonDenominator = n1 * n2 / Gcd(n1, n2);
        int zaehlerSumme = z1 * (commonDenominator / n1) + z2 * (commonDenominator / n2);
        var (rz, rn) = Reduce(zaehlerSumme, commonDenominator);

        string ergebnis = rn == 1 ? $"{rz}" : $"{rz}/{rn}";

        return new QuizQuestion
        {
            Id = NewId(),
            Subject = Subject.Mathematik,
            GradeLevel = GradeLevel.Klasse6,
            Topic = "Bruchrechnung – Addition",
            Type = QuestionType.OpenText,
            Prompt = $"Berechne: {z1}/{n1} + {z2}/{n2} = ? (Gib den gekürzten Bruch als z/n oder ganze Zahl an)",
            CorrectAnswers = new[] { ergebnis },
            Explanation = $"Gemeinsamer Nenner ist {commonDenominator}. " +
                          $"{z1}/{n1} = {z1 * (commonDenominator / n1)}/{commonDenominator}, " +
                          $"{z2}/{n2} = {z2 * (commonDenominator / n2)}/{commonDenominator}. " +
                          $"Zähler addieren: {zaehlerSumme}/{commonDenominator} = {ergebnis} (gekürzt).",
            HelpHint = "Erst beide Brüche auf denselben (gemeinsamen) Nenner bringen, dann die Zähler addieren, am Ende kürzen."
        };
    }

    private static QuizQuestion BruchMultiplikation(Random r)
    {
        int z1 = r.Next(1, 8), n1 = r.Next(z1 + 1, z1 + 6);
        int z2 = r.Next(1, 8), n2 = r.Next(z2 + 1, z2 + 6);

        var (rz, rn) = Reduce(z1 * z2, n1 * n2);
        string ergebnis = rn == 1 ? $"{rz}" : $"{rz}/{rn}";

        return new QuizQuestion
        {
            Id = NewId(),
            Subject = Subject.Mathematik,
            GradeLevel = GradeLevel.Klasse6,
            Topic = "Bruchrechnung – Multiplikation",
            Type = QuestionType.OpenText,
            Prompt = $"Berechne: {z1}/{n1} · {z2}/{n2} = ? (gekürzt als z/n)",
            CorrectAnswers = new[] { ergebnis },
            Explanation = $"Bei der Multiplikation von Brüchen werden Zähler mal Zähler und Nenner mal Nenner gerechnet: " +
                          $"({z1}·{z2})/({n1}·{n2}) = {z1 * z2}/{n1 * n2} = {ergebnis} (gekürzt).",
            HelpHint = "Zähler mal Zähler, Nenner mal Nenner - danach das Ergebnis kürzen, falls möglich."
        };
    }

    private static QuizQuestion ProzentGrundwert(Random r)
    {
        int[] prozentwerte = { 5, 10, 20, 25, 30, 40, 50, 75 };
        int prozent = prozentwerte[r.Next(prozentwerte.Length)];
        int grundwert = r.Next(2, 41) * 10;
        int ergebnis = grundwert * prozent / 100;

        return new QuizQuestion
        {
            Id = NewId(),
            Subject = Subject.Mathematik,
            GradeLevel = GradeLevel.Klasse6,
            Topic = "Prozentrechnung – Prozentwert",
            Type = QuestionType.OpenText,
            Prompt = $"Wie viel sind {prozent}% von {grundwert}?",
            CorrectAnswers = new[] { ergebnis.ToString() },
            Explanation = $"Prozentwert = Grundwert · Prozentsatz / 100 = {grundwert} · {prozent} / 100 = {ergebnis}.",
            HelpHint = "Formel: Prozentwert = Grundwert · Prozentsatz / 100."
        };
    }

    private static QuizQuestion NegativeZahlenAddition(Random r)
    {
        int a = r.Next(-20, 21);
        int b = r.Next(-20, 21);
        int ergebnis = a + b;
        string bTerm = b < 0 ? $"({b})" : b.ToString();

        return new QuizQuestion
        {
            Id = NewId(),
            Subject = Subject.Mathematik,
            GradeLevel = GradeLevel.Klasse6,
            Topic = "Negative Zahlen",
            Type = QuestionType.OpenText,
            Prompt = $"Berechne: {a} + {bTerm} = ?",
            CorrectAnswers = new[] { ergebnis.ToString() },
            Explanation = b < 0
                ? $"Addition einer negativen Zahl ist wie Subtraktion: {a} + ({b}) = {a} - {Math.Abs(b)} = {ergebnis}."
                : $"{a} + {b} = {ergebnis}.",
            HelpHint = "Eine negative Zahl zu addieren ist dasselbe wie die positive Zahl zu subtrahieren."
        };
    }

    private static QuizQuestion RechteckFlaeche(Random r)
    {
        int laenge = r.Next(3, 21);
        int breite = r.Next(2, 16);
        int flaeche = laenge * breite;
        int umfang = 2 * (laenge + breite);

        return new QuizQuestion
        {
            Id = NewId(),
            Subject = Subject.Mathematik,
            GradeLevel = GradeLevel.Klasse6,
            Topic = "Flächen- und Umfangsberechnung",
            Type = QuestionType.OpenText,
            Prompt = $"Ein Rechteck ist {laenge} cm lang und {breite} cm breit. Wie groß ist der Flächeninhalt in cm²?",
            CorrectAnswers = new[] { flaeche.ToString() },
            Explanation = $"Flächeninhalt = Länge · Breite = {laenge} · {breite} = {flaeche} cm² " +
                          $"(Zum Vergleich: der Umfang wäre 2·({laenge}+{breite}) = {umfang} cm).",
            HelpHint = "Formel für den Flächeninhalt eines Rechtecks: Fläche = Länge · Breite."
        };
    }

    private static QuizQuestion Massstab(Random r)
    {
        int[] massstaebe = { 100, 1000, 10000, 25000, 50000, 100000 };
        int massstab = massstaebe[r.Next(massstaebe.Length)];
        int kartenCm = r.Next(2, 21);
        long realCm = (long)kartenCm * massstab;
        long realMeter = realCm / 100;

        return new QuizQuestion
        {
            Id = NewId(),
            Subject = Subject.Mathematik,
            GradeLevel = GradeLevel.Klasse6,
            Topic = "Maßstab",
            Type = QuestionType.OpenText,
            Prompt = $"Auf einer Karte im Maßstab 1:{massstab} misst du eine Strecke von {kartenCm} cm. " +
                     "Wie viele Meter sind das in Wirklichkeit?",
            CorrectAnswers = new[] { realMeter.ToString() },
            Explanation = $"Wirkliche Länge = Kartenlänge · Maßstabsfaktor = {kartenCm} cm · {massstab} = {realCm} cm = {realMeter} m.",
            HelpHint = "Wirkliche Länge = Kartenlänge · Maßstabsfaktor. Am Ende von cm in Meter umrechnen (durch 100 teilen)."
        };
    }

    private static QuizQuestion WahrscheinlichkeitWuerfel(Random r)
    {
        (string beschreibung, int[] favorable)[] ereignisse =
        {
            ("eine gerade Zahl", new[] { 2, 4, 6 }),
            ("eine ungerade Zahl", new[] { 1, 3, 5 }),
            ("die Zahl 6", new[] { 6 }),
            ("eine Zahl größer als 4", new[] { 5, 6 }),
            ("eine Zahl kleiner als 3", new[] { 1, 2 }),
            ("eine durch 3 teilbare Zahl", new[] { 3, 6 })
        };
        var (beschreibung, favorable) = ereignisse[r.Next(ereignisse.Length)];
        var (rz, rn) = Reduce(favorable.Length, 6);
        string ergebnis = $"{rz}/{rn}";

        return new QuizQuestion
        {
            Id = NewId(),
            Subject = Subject.Mathematik,
            GradeLevel = GradeLevel.Klasse6,
            Topic = "Wahrscheinlichkeit bei Zufallsexperimenten",
            Type = QuestionType.OpenText,
            Prompt = $"Du würfelst einmal mit einem normalen Würfel (Zahlen 1 bis 6). Wie groß ist die " +
                     $"Wahrscheinlichkeit, {beschreibung} zu würfeln? (Gib den gekürzten Bruch als z/n an)",
            CorrectAnswers = new[] { ergebnis },
            Explanation = $"Von den 6 möglichen Ergebnissen sind {favorable.Length} günstig " +
                          $"({string.Join(", ", favorable)}). Wahrscheinlichkeit = günstige Ergebnisse / " +
                          $"mögliche Ergebnisse = {favorable.Length}/6 = {ergebnis} (gekürzt).",
            HelpHint = "Wahrscheinlichkeit = Anzahl günstiger Ergebnisse / Anzahl aller möglichen Ergebnisse."
        };
    }

    private static QuizQuestion QuaderVolumen(Random r)
    {
        int laenge = r.Next(2, 16), breite = r.Next(2, 13), hoehe = r.Next(2, 11);
        int volumen = laenge * breite * hoehe;
        int oberflaeche = 2 * (laenge * breite + laenge * hoehe + breite * hoehe);

        return new QuizQuestion
        {
            Id = NewId(),
            Subject = Subject.Mathematik,
            GradeLevel = GradeLevel.Klasse6,
            Topic = "Volumen von Quadern",
            Type = QuestionType.OpenText,
            Prompt = $"Ein Quader ist {laenge} cm lang, {breite} cm breit und {hoehe} cm hoch. Wie groß ist sein Volumen in cm³?",
            CorrectAnswers = new[] { volumen.ToString() },
            Explanation = $"Volumen = Länge · Breite · Höhe = {laenge} · {breite} · {hoehe} = {volumen} cm³ (zum Vergleich: Oberfläche wäre {oberflaeche} cm²).",
            HelpHint = "Formel für das Volumen eines Quaders: Volumen = Länge · Breite · Höhe."
        };
    }

    private static QuizQuestion BruchDezimalUmwandlung(Random r)
    {
        int[] nenner = { 2, 4, 5, 8, 10, 20, 25, 50 };
        int n = nenner[r.Next(nenner.Length)];
        int z = r.Next(1, n);
        var (rz, rn) = Reduce(z, n);
        decimal dezimal = (decimal)z / n;
        string dezimalStr = dezimal.ToString(System.Globalization.CultureInfo.InvariantCulture);

        return new QuizQuestion
        {
            Id = NewId(),
            Subject = Subject.Mathematik,
            GradeLevel = GradeLevel.Klasse6,
            Topic = "Bruch-Dezimalzahl-Umwandlung",
            Type = QuestionType.OpenText,
            Prompt = $"Wandle den Bruch {z}/{n} in eine Dezimalzahl um (mit Punkt statt Komma, z.B. \"0.5\").",
            CorrectAnswers = new[] { dezimalStr },
            Explanation = $"{z}/{n} (gekürzt: {rz}/{rn}) = {dezimalStr} als Dezimalzahl.",
            HelpHint = "Ein Bruch z/n entspricht der Division z geteilt durch n."
        };
    }

    private static QuizQuestion ProportionaleZuordnung(Random r)
    {
        int einheitenpreis = r.Next(1, 10);
        int mengeGegeben = r.Next(2, 6);
        int preisGegeben = einheitenpreis * mengeGegeben;
        int mengeGesucht = r.Next(mengeGegeben + 1, mengeGegeben + 10);
        int preisGesucht = einheitenpreis * mengeGesucht;

        return new QuizQuestion
        {
            Id = NewId(),
            Subject = Subject.Mathematik,
            GradeLevel = GradeLevel.Klasse6,
            Topic = "Direkt proportionale Zuordnungen",
            Type = QuestionType.OpenText,
            Prompt = $"{mengeGegeben} kg Äpfel kosten {preisGegeben}€. Wie viel kosten {mengeGesucht} kg Äpfel (bei gleichem Preis pro kg)?",
            CorrectAnswers = new[] { preisGesucht.ToString() },
            Explanation = $"Preis pro kg = {preisGegeben}€ / {mengeGegeben} = {einheitenpreis}€. Für {mengeGesucht} kg: {einheitenpreis}€ · {mengeGesucht} = {preisGesucht}€.",
            HelpHint = "Bei direkt proportionalen Zuordnungen: erst den Wert für 1 Einheit berechnen, dann mit der gesuchten Menge multiplizieren."
        };
    }

    private static readonly (string Frage, string[] Optionen, string Antwort, string Erklaerung)[] KongruenzabbildungenListe =
    {
        ("Was versteht man unter einer Kongruenzabbildung?", new[] { "Eine Abbildung, bei der Form und Größe einer Figur gleich bleiben", "Eine Abbildung, bei der sich die Größe der Figur ändert", "Eine Abbildung, die nur Farben verändert" }, "Eine Abbildung, bei der Form und Größe einer Figur gleich bleiben",
            "Bei einer Kongruenzabbildung bleiben Form und Größe einer Figur unverändert."),
        ("Welche Kongruenzabbildung verschiebt eine Figur geradlinig, ohne sie zu drehen oder zu spiegeln?", new[] { "Verschiebung (Translation)", "Drehung (Rotation)", "Spiegelung" }, "Verschiebung (Translation)",
            "Bei einer Verschiebung (Translation) bewegt sich die Figur geradlinig, ohne sich zu drehen."),
        ("Welche Kongruenzabbildung dreht eine Figur um einen festen Punkt?", new[] { "Drehung (Rotation)", "Verschiebung", "Spiegelung" }, "Drehung (Rotation)",
            "Eine Drehung (Rotation) dreht die Figur um ein festes Drehzentrum."),
        ("Welche Kongruenzabbildung spiegelt eine Figur an einer Achse (Linie)?", new[] { "Achsenspiegelung", "Verschiebung", "Punktspiegelung" }, "Achsenspiegelung",
            "Eine Achsenspiegelung spiegelt die Figur an einer festen Linie, der Spiegelachse."),
        ("Was passiert mit den Seitenlängen einer Figur bei einer Kongruenzabbildung?", new[] { "Sie bleiben unverändert", "Sie verdoppeln sich immer", "Sie werden halbiert" }, "Sie bleiben unverändert",
            "Kongruenzabbildungen verändern die Seitenlängen einer Figur nicht."),
        ("Was passiert mit den Winkeln einer Figur bei einer Kongruenzabbildung?", new[] { "Sie bleiben unverändert", "Sie werden immer größer", "Sie werden immer kleiner" }, "Sie bleiben unverändert",
            "Auch die Winkel einer Figur bleiben bei Kongruenzabbildungen unverändert."),
        ("Was ist eine Punktspiegelung?", new[] { "Eine Spiegelung einer Figur an einem einzelnen festen Punkt", "Eine Spiegelung an einer Linie", "Eine reine Verschiebung ohne Drehung" }, "Eine Spiegelung einer Figur an einem einzelnen festen Punkt",
            "Bei einer Punktspiegelung wird die Figur an einem einzelnen festen Punkt gespiegelt."),
        ("Wie verändert sich eine Figur bei einer Verschiebung (Translation)?", new[] { "Sie bewegt sich geradlinig in eine bestimmte Richtung, ohne sich zu drehen", "Sie dreht sich um einen Punkt", "Sie wird gespiegelt" }, "Sie bewegt sich geradlinig in eine bestimmte Richtung, ohne sich zu drehen",
            "Eine Verschiebung bewegt die Figur geradlinig in eine bestimmte Richtung."),
        ("Was ist bei einer Drehung um 180° besonders?", new[] { "Sie entspricht einer Punktspiegelung am Drehzentrum", "Die Figur bleibt exakt an derselben Stelle wie vorher", "Die Figur wird automatisch größer" }, "Sie entspricht einer Punktspiegelung am Drehzentrum",
            "Eine Drehung um genau 180° ist gleichbedeutend mit einer Punktspiegelung am Drehzentrum."),
        ("Woran erkennt man, dass zwei Figuren kongruent sind?", new[] { "Sie stimmen in Form und Größe genau überein", "Sie haben dieselbe Farbe", "Sie liegen exakt übereinander, ohne dass man sie bewegen darf" }, "Sie stimmen in Form und Größe genau überein",
            "Kongruente Figuren stimmen in Form und Größe exakt überein."),
        ("Was bleibt bei einer Achsenspiegelung im Vergleich zum Original erhalten?", new[] { "Form und Größe, aber die Orientierung (Ausrichtung) dreht sich um", "Nur die Farbe", "Nichts bleibt erhalten" }, "Form und Größe, aber die Orientierung (Ausrichtung) dreht sich um",
            "Bei einer Achsenspiegelung bleiben Form und Größe erhalten, die Orientierung kehrt sich aber um."),
        ("Wie nennt man den Punkt, um den bei einer Drehung gedreht wird?", new[] { "Drehzentrum (Drehpunkt)", "Spiegelachse", "Verschiebungspfeil" }, "Drehzentrum (Drehpunkt)",
            "Der feste Punkt, um den gedreht wird, heißt Drehzentrum oder Drehpunkt."),
        ("Wie nennt man die Linie, an der bei einer Achsenspiegelung gespiegelt wird?", new[] { "Spiegelachse", "Drehzentrum", "Verschiebungsvektor" }, "Spiegelachse",
            "Die Linie, an der gespiegelt wird, heißt Spiegelachse."),
        ("Was beschreibt ein Verschiebungspfeil (Vektor) bei einer Translation?", new[] { "Richtung und Weite der Verschiebung", "Den Drehwinkel", "Die Spiegelachse" }, "Richtung und Weite der Verschiebung",
            "Ein Verschiebungspfeil gibt Richtung und Weite der Verschiebung an."),
        ("Warum bleiben bei allen Kongruenzabbildungen die Flächeninhalte gleich?", new[] { "Weil sich weder Form noch Größe der Figur verändern", "Weil sich die Form immer verändert", "Der Flächeninhalt verändert sich immer" }, "Weil sich weder Form noch Größe der Figur verändern",
            "Da Form und Größe unverändert bleiben, bleibt auch der Flächeninhalt gleich."),
        ("Was ist der Unterschied zwischen einer Achsenspiegelung und einer Punktspiegelung?", new[] { "Bei der Achsenspiegelung wird an einer Linie gespiegelt, bei der Punktspiegelung an einem einzelnen Punkt", "Es gibt keinen Unterschied", "Eine Punktspiegelung verändert die Größe der Figur" }, "Bei der Achsenspiegelung wird an einer Linie gespiegelt, bei der Punktspiegelung an einem einzelnen Punkt",
            "Achsenspiegelung nutzt eine Linie, Punktspiegelung einen einzelnen Punkt als Spiegelelement."),
        ("Kann man eine Drehung um 360° als besondere Kongruenzabbildung bezeichnen?", new[] { "Ja, die Figur landet genau wieder an ihrer Ausgangsposition", "Nein, 360° ist kein möglicher Drehwinkel", "Nein, die Figur verschwindet dabei" }, "Ja, die Figur landet genau wieder an ihrer Ausgangsposition",
            "Nach einer vollen Drehung um 360° liegt die Figur wieder genau in der Ausgangsposition."),
        ("Was passiert mit einer Figur, wenn man sie zweimal an derselben Achse spiegelt?", new[] { "Sie landet wieder in ihrer ursprünglichen Lage", "Sie wird doppelt so groß", "Sie wird halbiert" }, "Sie landet wieder in ihrer ursprünglichen Lage",
            "Zwei Spiegelungen an derselben Achse heben sich gegenseitig auf."),
        ("Welche Eigenschaft haben alle Kongruenzabbildungen gemeinsam?", new[] { "Sie verändern weder Form noch Größe einer Figur", "Sie verändern immer die Farbe der Figur", "Sie funktionieren nur bei Kreisen" }, "Sie verändern weder Form noch Größe einer Figur",
            "Allen Kongruenzabbildungen ist gemeinsam, dass Form und Größe erhalten bleiben."),
        ("Warum sind Kongruenzabbildungen im Alltag wichtig, z.B. beim Fliesenlegen oder bei Mustern?", new[] { "Weil gleiche Formen durch Verschieben, Drehen oder Spiegeln lückenlos angeordnet werden können", "Weil sie die Größe der Fliesen verändern", "Sie haben keinerlei praktischen Nutzen" }, "Weil gleiche Formen durch Verschieben, Drehen oder Spiegeln lückenlos angeordnet werden können",
            "Durch Verschieben, Drehen und Spiegeln lassen sich gleiche Formen lückenlos anordnen, z.B. bei Fliesenmustern.")
    };

    private static QuizQuestion Kongruenzabbildungen(Random r)
    {
        var f = KongruenzabbildungenListe[r.Next(KongruenzabbildungenListe.Length)];
        return new QuizQuestion
        {
            Id = NewId(), Subject = Subject.Mathematik, GradeLevel = GradeLevel.Klasse6,
            Topic = "Kongruenzabbildungen", Type = QuestionType.MultipleChoice,
            Prompt = f.Frage, Options = f.Optionen, CorrectAnswers = new[] { f.Antwort }, Explanation = f.Erklaerung,
            HelpHint = "Verschiebung = geradlinige Bewegung, Drehung = um einen Punkt drehen, Spiegelung = an Achse/Punkt spiegeln. Form und Größe bleiben immer gleich."
        };
    }

    private static QuizQuestion Kombinatorik(Random r)
    {
        int shirts = r.Next(2, 7);
        int hosen = r.Next(2, 6);
        int kombinationen = shirts * hosen;

        return new QuizQuestion
        {
            Id = NewId(),
            Subject = Subject.Mathematik,
            GradeLevel = GradeLevel.Klasse6,
            Topic = "Kombinatorik (systematisches Zählen)",
            Type = QuestionType.OpenText,
            Prompt = $"Du hast {shirts} verschiedene T-Shirts und {hosen} verschiedene Hosen. Wie viele verschiedene Kombinationen aus einem T-Shirt und einer Hose kannst du zusammenstellen?",
            CorrectAnswers = new[] { kombinationen.ToString() },
            Explanation = $"Für jedes der {shirts} T-Shirts gibt es {hosen} mögliche Hosen dazu: {shirts} · {hosen} = {kombinationen} Kombinationen.",
            HelpHint = "Zählprinzip: Anzahl Möglichkeiten für das erste Ding mal Anzahl Möglichkeiten für das zweite Ding."
        };
    }

    private static QuizQuestion LineareGleichung(Random r)
    {
        int x = r.Next(-10, 11);
        int a = r.Next(2, 9) * (r.Next(2) == 0 ? 1 : -1);
        int b = r.Next(-15, 16);
        int c = a * x + b;
        string bTerm = b < 0 ? $"- {Math.Abs(b)}" : $"+ {b}";

        return new QuizQuestion
        {
            Id = NewId(),
            Subject = Subject.Mathematik,
            GradeLevel = GradeLevel.Klasse9,
            Topic = "Lineare Gleichungen",
            Type = QuestionType.OpenText,
            Prompt = $"Löse die Gleichung nach x auf: {a}x {bTerm} = {c}",
            CorrectAnswers = new[] { x.ToString() },
            Explanation = $"{a}x {bTerm} = {c}  |  {(b < 0 ? "+" : "-")} {Math.Abs(b)}\n" +
                          $"{a}x = {c - b}  |  : {a}\n" +
                          $"x = {x}",
            HelpHint = "Erst die Zahl ohne x auf die andere Seite bringen (Gegenoperation), dann durch die Zahl vor dem x teilen."
        };
    }

    private static QuizQuestion LineareFunktion(Random r)
    {
        int steigung;
        do { steigung = r.Next(-5, 6); } while (steigung == 0);
        int achsenabschnitt = r.Next(-10, 11);
        int x = r.Next(-6, 7);
        int y = steigung * x + achsenabschnitt;

        return new QuizQuestion
        {
            Id = NewId(),
            Subject = Subject.Mathematik,
            GradeLevel = GradeLevel.Klasse9,
            Topic = "Lineare Funktionen",
            Type = QuestionType.OpenText,
            Prompt = $"Gegeben ist die lineare Funktion f(x) = {steigung}x {(achsenabschnitt >= 0 ? "+ " + achsenabschnitt : "- " + Math.Abs(achsenabschnitt))}. " +
                     $"Berechne f({x}).",
            CorrectAnswers = new[] { y.ToString() },
            Explanation = $"f({x}) = {steigung}·({x}) {(achsenabschnitt >= 0 ? "+ " + achsenabschnitt : "- " + Math.Abs(achsenabschnitt))} = {y}.",
            HelpHint = "Setze den gegebenen x-Wert in f(x) = m·x + b ein und rechne aus."
        };
    }

    private static QuizQuestion QuadratischeGleichung(Random r)
    {
        // Konstruiere x^2 + px + q = 0 aus zwei verschiedenen ganzzahligen Nullstellen, damit die pq-Formel "schöne" Lösungen liefert.
        int x1 = r.Next(-8, 9);
        int x2;
        do { x2 = r.Next(-8, 9); } while (x2 == x1);
        int p = -(x1 + x2);
        int q = x1 * x2;
        string pTerm = p == 0 ? "" : (p > 0 ? $" + {p}x" : $" - {Math.Abs(p)}x");
        string qTerm = q == 0 ? "" : (q > 0 ? $" + {q}" : $" - {Math.Abs(q)}");
        var loesungen = new[] { x1, x2 }.OrderBy(v => v).Select(v => v.ToString());

        return new QuizQuestion
        {
            Id = NewId(),
            Subject = Subject.Mathematik,
            GradeLevel = GradeLevel.Klasse9,
            Topic = "Quadratische Gleichungen (pq-Formel)",
            Type = QuestionType.OpenText,
            Prompt = $"Löse: x²{pTerm}{qTerm} = 0. Gib die kleinere Lösung zuerst an, getrennt mit Komma (z.B. -2,3).",
            CorrectAnswers = new[] { string.Join(",", loesungen) },
            Explanation = $"Mit der pq-Formel: x = -p/2 ± √((p/2)² - q), p={p}, q={q}. " +
                          $"Die Lösungen sind x₁={Math.Min(x1, x2)} und x₂={Math.Max(x1, x2)} " +
                          $"(Probe: x₁+x₂ = -p = {-p}, x₁·x₂ = q = {q}).",
            HelpHint = "pq-Formel: x = -p/2 ± √((p/2)² - q). Bringe die Gleichung zuerst in die Form x² + px + q = 0."
        };
    }

    private static QuizQuestion SatzDesPythagoras(Random r)
    {
        // Pythagoreische Tripel für "glatte" Ergebnisse.
        (int a, int b, int c)[] tripel = { (3, 4, 5), (6, 8, 10), (5, 12, 13), (8, 15, 17), (9, 12, 15), (7, 24, 25) };
        var (ka, kb, kc) = tripel[r.Next(tripel.Length)];

        return new QuizQuestion
        {
            Id = NewId(),
            Subject = Subject.Mathematik,
            GradeLevel = GradeLevel.Klasse9,
            Topic = "Satz des Pythagoras",
            Type = QuestionType.OpenText,
            Prompt = $"Ein rechtwinkliges Dreieck hat die Katheten a = {ka} cm und b = {kb} cm. " +
                     "Wie lang ist die Hypotenuse c (in cm)?",
            CorrectAnswers = new[] { kc.ToString() },
            Explanation = $"Nach dem Satz des Pythagoras gilt: a² + b² = c². " +
                          $"{ka}² + {kb}² = {ka * ka} + {kb * kb} = {ka * ka + kb * kb} = c². " +
                          $"Also c = √{ka * ka + kb * kb} = {kc} cm.",
            HelpHint = "Satz des Pythagoras: a² + b² = c² (c ist die Hypotenuse, die längste Seite). Am Ende die Wurzel ziehen."
        };
    }

    private static QuizQuestion Zinsrechnung(Random r)
    {
        int kapital = r.Next(2, 21) * 500;
        int zinssatz = new[] { 2, 3, 4, 5, 6 }[r.Next(5)];
        int jahre = r.Next(1, 5);
        int zinsen = kapital * zinssatz * jahre / 100;

        return new QuizQuestion
        {
            Id = NewId(),
            Subject = Subject.Mathematik,
            GradeLevel = GradeLevel.Klasse9,
            Topic = "Zinsrechnung",
            Type = QuestionType.OpenText,
            Prompt = $"Ein Kapital von {kapital}€ wird für {jahre} Jahre zu {zinssatz}% Zinsen pro Jahr angelegt " +
                     "(einfache Zinsrechnung, ohne Zinseszins). Wie viel Zinsen kommen insgesamt zusammen?",
            CorrectAnswers = new[] { zinsen.ToString() },
            Explanation = $"Zinsen = Kapital · Zinssatz · Jahre / 100 = {kapital} · {zinssatz} · {jahre} / 100 = {zinsen}€.",
            HelpHint = "Formel (einfache Zinsen, ohne Zinseszins): Zinsen = Kapital · Zinssatz · Jahre / 100."
        };
    }

    private static QuizQuestion BinomischeFormel(Random r)
    {
        int a = r.Next(1, 10);
        int b = r.Next(1, 10);
        bool plus = r.Next(2) == 0;
        int aa = a * a;
        int twoAb = 2 * a * b;
        int bb = b * b;

        string aufgabe = plus ? $"(x + {b})²" : $"(x - {b})²";
        string loesung = plus
            ? $"x² + {twoAb}x + {bb}"
            : $"x² - {twoAb}x + {bb}";

        return new QuizQuestion
        {
            Id = NewId(),
            Subject = Subject.Mathematik,
            GradeLevel = GradeLevel.Klasse9,
            Topic = "Binomische Formeln",
            Type = QuestionType.OpenText,
            Prompt = $"Multipliziere aus (1. bzw. 2. binomische Formel): {aufgabe} = ?",
            CorrectAnswers = new[] { loesung },
            Explanation = plus
                ? $"(x+{b})² = x² + 2·x·{b} + {b}² = x² + {twoAb}x + {bb}"
                : $"(x-{b})² = x² - 2·x·{b} + {b}² = x² - {twoAb}x + {bb}",
            HelpHint = "1./2. binomische Formel: (a±b)² = a² ± 2ab + b². Hier ist a = x."
        };
    }

    private static QuizQuestion MittelwertUndMedian(Random r)
    {
        int a, b, c, d, e, mean;
        int attempts = 0;
        do
        {
            a = r.Next(1, 20);
            b = r.Next(1, 20);
            c = r.Next(1, 20);
            d = r.Next(1, 20);
            mean = r.Next(3, 15);
            e = mean * 5 - (a + b + c + d);
            attempts++;
        } while ((e < 1 || e > 40) && attempts < 30);

        var werte = new[] { a, b, c, d, e };
        var sortiert = werte.OrderBy(v => v).ToArray();
        int median = sortiert[2];
        string wertliste = string.Join(", ", werte);
        bool frageNachMedian = r.Next(2) == 0;

        if (frageNachMedian)
        {
            return new QuizQuestion
            {
                Id = NewId(),
                Subject = Subject.Mathematik,
                GradeLevel = GradeLevel.Klasse9,
                Topic = "Mittelwert und Median (Statistik)",
                Type = QuestionType.OpenText,
                Prompt = $"Gegeben sind die Werte: {wertliste}. Wie lautet der Median (Zentralwert)?",
                CorrectAnswers = new[] { median.ToString() },
                Explanation = $"Sortiert: {string.Join(", ", sortiert)}. Bei 5 Werten (ungerade Anzahl) ist " +
                              $"der Median der mittlere Wert nach dem Sortieren: {median}.",
                HelpHint = "Erst die Werte der Größe nach sortieren, dann den mittleren Wert ablesen (bei ungerader Anzahl)."
            };
        }

        return new QuizQuestion
        {
            Id = NewId(),
            Subject = Subject.Mathematik,
            GradeLevel = GradeLevel.Klasse9,
            Topic = "Mittelwert und Median (Statistik)",
            Type = QuestionType.OpenText,
            Prompt = $"Gegeben sind die Werte: {wertliste}. Wie lautet der Mittelwert (arithmetisches Mittel)?",
            CorrectAnswers = new[] { mean.ToString() },
            Explanation = $"Mittelwert = Summe aller Werte / Anzahl der Werte = ({a} + {b} + {c} + {d} + {e}) / 5 " +
                          $"= {a + b + c + d + e} / 5 = {mean}.",
            HelpHint = "Mittelwert = Summe aller Werte / Anzahl der Werte."
        };
    }

    private static QuizQuestion Trigonometrie(Random r)
    {
        (int a, int b, int c)[] tripel = { (3, 4, 5), (6, 8, 10), (5, 12, 13), (8, 15, 17), (9, 12, 15), (7, 24, 25) };
        var (ka, kb, _) = tripel[r.Next(tripel.Length)];
        bool tauschen = r.Next(2) == 0;
        int gegenkathete = tauschen ? kb : ka;
        int ankathete = tauschen ? ka : kb;
        double tangens = (double)gegenkathete / ankathete;
        int winkelGrad = (int)Math.Round(Math.Atan(tangens) * 180 / Math.PI);

        return new QuizQuestion
        {
            Id = NewId(),
            Subject = Subject.Mathematik,
            GradeLevel = GradeLevel.Klasse9,
            Topic = "Trigonometrie im rechtwinkligen Dreieck",
            Type = QuestionType.OpenText,
            Prompt = $"In einem rechtwinkligen Dreieck ist die Gegenkathete zum gesuchten Winkel α gleich {gegenkathete} cm, die Ankathete gleich {ankathete} cm. Berechne α mit dem Tangens (gerundet auf ganze Grad, nur die Zahl angeben).",
            CorrectAnswers = new[] { winkelGrad.ToString() },
            Explanation = $"tan(α) = Gegenkathete/Ankathete = {gegenkathete}/{ankathete} ≈ {tangens:0.###}. " +
                          $"α = arctan({tangens:0.###}) ≈ {winkelGrad}°.",
            HelpHint = "Im rechtwinkligen Dreieck: tan(α) = Gegenkathete / Ankathete. Der gesuchte Winkel ist arctan davon."
        };
    }

    private static readonly (string Frage, string[] Optionen, string Antwort, string Erklaerung)[] SatzDesThalesListe =
    {
        ("Was besagt der Satz des Thales?", new[] { "Jeder Winkel im Halbkreis über dem Durchmesser ist ein rechter Winkel (90°)", "Jeder Winkel im Kreis ist automatisch 180°", "Ein Kreis hat immer den gleichen Umfang wie Durchmesser" }, "Jeder Winkel im Halbkreis über dem Durchmesser ist ein rechter Winkel (90°)",
            "Der Satz des Thales besagt, dass jeder Winkel im Halbkreis über dem Durchmesser 90° beträgt."),
        ("Was ist die Voraussetzung für den Satz des Thales?", new[] { "Ein Dreieck ist einem Halbkreis einbeschrieben, wobei der Durchmesser eine Dreiecksseite ist", "Das Dreieck muss gleichseitig sein", "Es muss kein Kreis vorhanden sein" }, "Ein Dreieck ist einem Halbkreis einbeschrieben, wobei der Durchmesser eine Dreiecksseite ist",
            "Der Satz des Thales gilt für Dreiecke, deren dritter Eckpunkt auf einem Halbkreis über dem Durchmesser liegt."),
        ("Wie nennt man den Kreis, auf dem beim Satz des Thales der dritte Eckpunkt eines rechtwinkligen Dreiecks liegt?", new[] { "Thaleskreis", "Umkreis eines Quadrats", "Inkreis eines Dreiecks" }, "Thaleskreis",
            "Dieser besondere Kreis heißt Thaleskreis."),
        ("Wofür kann der Satz des Thales praktisch genutzt werden?", new[] { "Um rechte Winkel ohne Geodreieck exakt zu konstruieren", "Um den Umfang eines Kreises zu berechnen", "Um die Fläche eines Quadrats zu berechnen" }, "Um rechte Winkel ohne Geodreieck exakt zu konstruieren",
            "Mit dem Satz des Thales lassen sich rechte Winkel exakt konstruieren, auch ohne Geodreieck."),
        ("Wo liegt der Mittelpunkt des Thaleskreises?", new[] { "Genau in der Mitte des Durchmessers (der Hypotenuse)", "Immer am rechten Winkel", "Außerhalb des Dreiecks" }, "Genau in der Mitte des Durchmessers (der Hypotenuse)",
            "Der Mittelpunkt des Thaleskreises liegt genau in der Mitte der Hypotenuse."),
        ("Was ist der Radius des Thaleskreises im Verhältnis zur Hypotenuse des rechtwinkligen Dreiecks?", new[] { "Der Radius ist halb so lang wie die Hypotenuse", "Der Radius ist doppelt so lang wie die Hypotenuse", "Radius und Hypotenuse sind immer gleich lang" }, "Der Radius ist halb so lang wie die Hypotenuse",
            "Der Radius des Thaleskreises entspricht der Hälfte der Hypotenuse (des Durchmessers)."),
        ("Was gilt für JEDEN Punkt auf dem Thaleskreis (außer den Endpunkten des Durchmessers)?", new[] { "Der Winkel zum Durchmesser ist immer 90°", "Der Abstand zum Mittelpunkt ist immer unterschiedlich", "Der Winkel ist immer 180°" }, "Der Winkel zum Durchmesser ist immer 90°",
            "Für jeden Punkt auf dem Thaleskreis ist der Winkel zu den Endpunkten des Durchmessers 90°."),
        ("Wie kann man mit dem Satz des Thales prüfen, ob ein Dreieck rechtwinklig ist?", new[] { "Man prüft, ob der Eckpunkt auf einem Halbkreis über der längsten Seite liegt", "Man misst nur die Fläche des Dreiecks", "Man zählt die Anzahl der Ecken" }, "Man prüft, ob der Eckpunkt auf einem Halbkreis über der längsten Seite liegt",
            "Liegt der dritte Eckpunkt auf dem Halbkreis über der längsten Seite, ist das Dreieck rechtwinklig."),
        ("Was unterscheidet den Satz des Thales vom Satz des Pythagoras?", new[] { "Thales beschreibt eine Winkelbeziehung im Halbkreis, Pythagoras eine Seitenbeziehung im rechtwinkligen Dreieck", "Beide Sätze sind identisch", "Pythagoras gilt nur für Kreise" }, "Thales beschreibt eine Winkelbeziehung im Halbkreis, Pythagoras eine Seitenbeziehung im rechtwinkligen Dreieck",
            "Thales beschreibt eine Winkelbeziehung (90° im Halbkreis), Pythagoras eine Beziehung zwischen den Seitenlängen."),
        ("Wie nennt man einen Kreis, der durch alle drei Eckpunkte eines Dreiecks verläuft?", new[] { "Umkreis", "Inkreis", "Thaleskreis (nur bei rechtwinkligen Dreiecken)" }, "Umkreis",
            "Der Kreis durch alle drei Eckpunkte eines Dreiecks heißt Umkreis."),
        ("Warum ist der Thaleskreis ein Spezialfall des Umkreises?", new[] { "Weil beim rechtwinkligen Dreieck der Durchmesser des Umkreises genau der Hypotenuse entspricht", "Weil der Thaleskreis nie ein Umkreis sein kann", "Weil der Umkreis immer kleiner als der Thaleskreis ist" }, "Weil beim rechtwinkligen Dreieck der Durchmesser des Umkreises genau der Hypotenuse entspricht",
            "Beim rechtwinkligen Dreieck ist der Umkreis-Durchmesser genau die Hypotenuse - das macht ihn zum Thaleskreis."),
        ("Wie kann man mithilfe des Satzes von Thales ein rechtwinkliges Dreieck konstruieren?", new[] { "Man zeichnet einen Halbkreis über einer Strecke und wählt einen beliebigen Punkt darauf als dritten Eckpunkt", "Man zeichnet ein beliebiges Dreieck ohne Kreis", "Man verdoppelt einfach eine Seite" }, "Man zeichnet einen Halbkreis über einer Strecke und wählt einen beliebigen Punkt darauf als dritten Eckpunkt",
            "Ein beliebiger Punkt auf dem Halbkreis über einer Strecke ergibt zusammen mit den Endpunkten immer ein rechtwinkliges Dreieck."),
        ("Was passiert mit dem rechten Winkel, wenn der dritte Eckpunkt genau auf einen Endpunkt des Durchmessers fällt?", new[] { "Dann entsteht kein echtes Dreieck mehr", "Der Winkel bleibt trotzdem exakt 90°", "Der Winkel wird automatisch 180°" }, "Dann entsteht kein echtes Dreieck mehr",
            "Fällt der dritte Punkt auf einen Endpunkt des Durchmessers, entsteht kein echtes Dreieck mehr."),
        ("Ist der Satz des Thales ein Spezialfall eines allgemeineren Kreiswinkelsatzes (Umfangswinkelsatz)?", new[] { "Ja, er ist ein Sonderfall für den Umfangswinkel über dem Durchmesser", "Nein, beide Sätze haben nichts miteinander zu tun", "Nein, der Satz des Thales gilt nur für Quadrate" }, "Ja, er ist ein Sonderfall für den Umfangswinkel über dem Durchmesser",
            "Der Satz des Thales ist ein Sonderfall des allgemeineren Umfangswinkelsatzes."),
        ("Warum ist der Satz des Thales in der Praxis (z.B. im Bauwesen) nützlich?", new[] { "Er ermöglicht das exakte Konstruieren rechter Winkel ohne spezielles Winkelmessgerät", "Er hilft nur beim Berechnen von Kreisumfängen", "Er hat in der Praxis keinerlei Nutzen" }, "Er ermöglicht das exakte Konstruieren rechter Winkel ohne spezielles Winkelmessgerät",
            "Rechte Winkel lassen sich mit dem Satz des Thales auch ohne spezielles Messgerät konstruieren."),
        ("Was gilt für die Hypotenuse eines im Thaleskreis liegenden rechtwinkligen Dreiecks?", new[] { "Sie ist immer der Durchmesser des Thaleskreises", "Sie ist immer der Radius", "Sie liegt außerhalb des Kreises" }, "Sie ist immer der Durchmesser des Thaleskreises",
            "Die Hypotenuse des rechtwinkligen Dreiecks ist immer der Durchmesser des Thaleskreises."),
        ("Welche geometrische Grundform wird beim Satz des Thales zwingend benötigt?", new[] { "Ein Halbkreis (bzw. Kreis) mit einem Durchmesser", "Ein Quadrat", "Ein regelmäßiges Sechseck" }, "Ein Halbkreis (bzw. Kreis) mit einem Durchmesser",
            "Der Satz des Thales braucht zwingend einen Halbkreis mit einem Durchmesser."),
        ("Was passiert mit dem rechten Winkel, wenn der dritte Eckpunkt näher am Rand oder näher an der Mitte des Halbkreises liegt?", new[] { "Er bleibt immer exakt 90°, unabhängig von der genauen Position auf dem Halbkreis", "Er wird größer, je näher der Punkt am Rand liegt", "Er wird kleiner, je näher der Punkt an der Mitte liegt" }, "Er bleibt immer exakt 90°, unabhängig von der genauen Position auf dem Halbkreis",
            "Unabhängig von der genauen Position auf dem Halbkreis bleibt der Winkel stets 90°."),
        ("Wie könnte man den Satz des Thales nutzen, um zu prüfen, ob drei gegebene Punkte ein rechtwinkliges Dreieck bilden?", new[] { "Prüfen, ob der Punkt mit dem rechten Winkel auf dem Halbkreis über der längsten Strecke liegt", "Nur die Farben der Punkte vergleichen", "Die Anzahl der Punkte zählen" }, "Prüfen, ob der Punkt mit dem rechten Winkel auf dem Halbkreis über der längsten Strecke liegt",
            "Man prüft, ob der vermutete rechte Winkel auf dem Halbkreis über der längsten Seite liegt."),
        ("Warum wird der Satz des Thales oft als Einstieg in Kreiswinkelsätze der Geometrie behandelt?", new[] { "Weil er einen besonders einfachen und anschaulichen Sonderfall darstellt", "Weil er der komplizierteste aller Kreiswinkelsätze ist", "Weil er nichts mit Kreisen zu tun hat" }, "Weil er einen besonders einfachen und anschaulichen Sonderfall darstellt",
            "Der Satz des Thales ist ein besonders einfacher, anschaulicher Sonderfall der Kreiswinkelsätze.")
    };

    private static QuizQuestion SatzDesThales(Random r)
    {
        var f = SatzDesThalesListe[r.Next(SatzDesThalesListe.Length)];
        return new QuizQuestion
        {
            Id = NewId(), Subject = Subject.Mathematik, GradeLevel = GradeLevel.Klasse9,
            Topic = "Satz des Thales", Type = QuestionType.MultipleChoice,
            Prompt = f.Frage, Options = f.Optionen, CorrectAnswers = new[] { f.Antwort }, Explanation = f.Erklaerung,
            HelpHint = "Satz des Thales: Jeder Winkel im Halbkreis über dem Durchmesser ist ein rechter Winkel (90°)."
        };
    }

    private static QuizQuestion PyramideKegelKugelVolumen(Random r)
    {
        int form = r.Next(3);

        if (form == 0)
        {
            int seite = r.Next(2, 10);
            int hoehe = r.Next(1, 7) * 3;
            int grundflaeche = seite * seite;
            int volumen = grundflaeche * hoehe / 3;

            return new QuizQuestion
            {
                Id = NewId(), Subject = Subject.Mathematik, GradeLevel = GradeLevel.Klasse9,
                Topic = "Volumen von Pyramide, Kegel und Kugel", Type = QuestionType.OpenText,
                Prompt = $"Eine quadratische Pyramide hat eine Grundkante von {seite} cm und eine Höhe von {hoehe} cm. Wie groß ist ihr Volumen in cm³? (Formel: V = 1/3 · Grundfläche · Höhe)",
                CorrectAnswers = new[] { volumen.ToString() },
                Explanation = $"Grundfläche = {seite}² = {grundflaeche} cm². Volumen = 1/3 · {grundflaeche} · {hoehe} = {volumen} cm³.",
                HelpHint = "Pyramidenvolumen: V = 1/3 · Grundfläche · Höhe."
            };
        }

        if (form == 1)
        {
            int radius = r.Next(2, 8);
            int hoeheKegel = r.Next(1, 7) * 3;
            int koeffizient = radius * radius * hoeheKegel / 3;

            return new QuizQuestion
            {
                Id = NewId(), Subject = Subject.Mathematik, GradeLevel = GradeLevel.Klasse9,
                Topic = "Volumen von Pyramide, Kegel und Kugel", Type = QuestionType.OpenText,
                Prompt = $"Ein Kegel hat den Radius r = {radius} cm und die Höhe h = {hoeheKegel} cm. Wie groß ist sein Volumen? (Formel: V = 1/3 · π · r² · h; gib das Ergebnis als Vielfaches von π an, z.B. \"12π\")",
                CorrectAnswers = new[] { $"{koeffizient}π" },
                Explanation = $"V = 1/3 · π · {radius}² · {hoeheKegel} = 1/3 · π · {radius * radius} · {hoeheKegel} = {koeffizient}π cm³.",
                HelpHint = "Kegelvolumen: V = 1/3 · π · r² · h. Radius immer zuerst quadrieren."
            };
        }

        int[] radien = { 3, 6, 9 };
        int rad = radien[r.Next(radien.Length)];
        int koeffKugel = 4 * rad * rad * rad / 3;

        return new QuizQuestion
        {
            Id = NewId(), Subject = Subject.Mathematik, GradeLevel = GradeLevel.Klasse9,
            Topic = "Volumen von Pyramide, Kegel und Kugel", Type = QuestionType.OpenText,
            Prompt = $"Eine Kugel hat den Radius r = {rad} cm. Wie groß ist ihr Volumen? (Formel: V = 4/3 · π · r³; gib das Ergebnis als Vielfaches von π an, z.B. \"36π\")",
            CorrectAnswers = new[] { $"{koeffKugel}π" },
            Explanation = $"V = 4/3 · π · {rad}³ = 4/3 · π · {rad * rad * rad} = {koeffKugel}π cm³.",
            HelpHint = "Kugelvolumen: V = 4/3 · π · r³."
        };
    }

    private static QuizQuestion LinearesGleichungssystem(Random r)
    {
        int x0 = r.Next(-6, 7), y0 = r.Next(-6, 7);
        int a = r.Next(1, 6), b = r.Next(1, 6), d = r.Next(1, 6), e = r.Next(1, 6);
        int c = a * x0 + b * y0;
        int f = d * x0 - e * y0;

        return new QuizQuestion
        {
            Id = NewId(),
            Subject = Subject.Mathematik,
            GradeLevel = GradeLevel.Klasse9,
            Topic = "Lineare Gleichungssysteme",
            Type = QuestionType.OpenText,
            Prompt = $"Löse das Gleichungssystem: {a}x + {b}y = {c} und {d}x - {e}y = {f}. Gib die Lösung als \"x={x0},y={y0}\"-Format an.",
            CorrectAnswers = new[] { $"x={x0},y={y0}" },
            Explanation = $"Einsetzen von x={x0} und y={y0} erfüllt beide Gleichungen: {a}·{x0} + {b}·{y0} = {c} und {d}·{x0} - {e}·{y0} = {f}.",
            HelpHint = "Löse z.B. durch Gleichsetzen, Einsetzen oder Additionsverfahren: eine Gleichung nach einer Variable auflösen und in die andere einsetzen."
        };
    }

    private static QuizQuestion QuadratischeFunktionMerkmale(Random r)
    {
        int h = r.Next(-6, 7), k = r.Next(-8, 9);
        string hTerm = h == 0 ? "x" : (h > 0 ? $"(x - {h})" : $"(x + {Math.Abs(h)})");
        string kTerm = k == 0 ? "" : (k > 0 ? $" + {k}" : $" - {Math.Abs(k)}");

        return new QuizQuestion
        {
            Id = NewId(),
            Subject = Subject.Mathematik,
            GradeLevel = GradeLevel.Klasse9,
            Topic = "Quadratische Funktionen (Scheitelpunkt)",
            Type = QuestionType.OpenText,
            Prompt = $"Gegeben ist die quadratische Funktion f(x) = {hTerm}²{kTerm} (Scheitelpunktform). Wie lautet der Scheitelpunkt? (Format: \"({h},{k})\")",
            CorrectAnswers = new[] { $"({h},{k})" },
            Explanation = $"In der Scheitelpunktform f(x) = (x-h)² + k liegt der Scheitelpunkt bei S({h}|{k}).",
            HelpHint = "In der Scheitelpunktform f(x) = (x-h)² + k liegt der Scheitelpunkt direkt bei S(h|k)."
        };
    }

    private static QuizQuestion Exponentialfunktion(Random r)
    {
        int start = r.Next(2, 21) * 10;
        int verdopplungen = r.Next(2, 6);
        long ergebnis = start;
        for (int i = 0; i < verdopplungen; i++)
        {
            ergebnis *= 2;
        }

        const int stundenProVerdopplung = 3;
        int stunden = verdopplungen * stundenProVerdopplung;

        return new QuizQuestion
        {
            Id = NewId(),
            Subject = Subject.Mathematik,
            GradeLevel = GradeLevel.Klasse9,
            Topic = "Exponentielles Wachstum",
            Type = QuestionType.OpenText,
            Prompt = $"Eine Bakterienkultur mit anfangs {start} Bakterien verdoppelt sich alle {stundenProVerdopplung} Stunden. Wie viele Bakterien sind es nach {stunden} Stunden ({verdopplungen} Verdopplungen)?",
            CorrectAnswers = new[] { ergebnis.ToString() },
            Explanation = $"Nach jeder Verdopplung wird die Anzahl mit 2 multipliziert: {start} · 2^{verdopplungen} = {ergebnis}. Das ist exponentielles Wachstum.",
            HelpHint = "Exponentielles Wachstum: Bestand nach n Verdopplungen = Anfangsbestand · 2^n."
        };
    }

    private static QuizQuestion Potenzgesetze(Random r)
    {
        int basis = r.Next(2, 6);
        int exp1 = r.Next(1, 6);
        int exp2 = r.Next(1, 6);

        if (r.Next(2) == 0)
        {
            int summe = exp1 + exp2;
            return new QuizQuestion
            {
                Id = NewId(), Subject = Subject.Mathematik, GradeLevel = GradeLevel.Klasse9,
                Topic = "Potenzgesetze", Type = QuestionType.OpenText,
                Prompt = $"Vereinfache mit den Potenzgesetzen: {basis}^{exp1} · {basis}^{exp2} = {basis}^? (Gib nur den Exponenten an)",
                CorrectAnswers = new[] { summe.ToString() },
                Explanation = $"Bei gleicher Basis werden bei der Multiplikation die Exponenten addiert: {basis}^{exp1} · {basis}^{exp2} = {basis}^({exp1}+{exp2}) = {basis}^{summe}.",
                HelpHint = "Potenzgesetz: a^m · a^n = a^(m+n) (gleiche Basis: Exponenten addieren)."
            };
        }

        int expGross = Math.Max(exp1, exp2) + r.Next(1, 4);
        int expKlein = Math.Min(exp1, exp2);
        int differenz = expGross - expKlein;

        return new QuizQuestion
        {
            Id = NewId(), Subject = Subject.Mathematik, GradeLevel = GradeLevel.Klasse9,
            Topic = "Potenzgesetze", Type = QuestionType.OpenText,
            Prompt = $"Vereinfache mit den Potenzgesetzen: {basis}^{expGross} : {basis}^{expKlein} = {basis}^? (Gib nur den Exponenten an)",
            CorrectAnswers = new[] { differenz.ToString() },
            Explanation = $"Bei gleicher Basis werden bei der Division die Exponenten subtrahiert: {basis}^{expGross} : {basis}^{expKlein} = {basis}^({expGross}-{expKlein}) = {basis}^{differenz}.",
            HelpHint = "Potenzgesetz: a^m : a^n = a^(m-n) (gleiche Basis: Exponenten subtrahieren)."
        };
    }

    // ============ Klasse 7 (Berliner RLP Sek I, Doppeljahrgang 7/8) ============

    private static QuizQuestion RationaleZahlenRechnen(Random r)
    {
        int a = r.Next(-12, 13);
        int b = r.Next(-12, 13);
        while (a == 0) a = r.Next(-12, 13);
        while (b == 0) b = r.Next(-12, 13);

        int op = r.Next(3);
        (string symbol, int ergebnis) = op switch
        {
            0 => ("+", a + b),
            1 => ("-", a - b),
            _ => ("·", a * b)
        };
        string bText = b < 0 ? $"({b})" : b.ToString();

        return new QuizQuestion
        {
            Id = NewId(),
            Subject = Subject.Mathematik,
            GradeLevel = GradeLevel.Klasse7,
            Topic = "Rationale Zahlen",
            Type = QuestionType.OpenText,
            Prompt = $"Berechne: {a} {symbol} {bText} = ?",
            CorrectAnswers = new[] { ergebnis.ToString() },
            Explanation = op switch
            {
                0 => $"{a} + {bText}: auf der Zahlengeraden von {a} um {Math.Abs(b)} nach {(b > 0 ? "rechts" : "links")} = {ergebnis}.",
                1 => $"{a} - {bText}: Minus einer negativen Zahl ist Plus bzw. auf der Zahlengeraden nach links gehen - Ergebnis {ergebnis}.",
                _ => $"{a} · {bText}: Beträge multiplizieren ({Math.Abs(a)}·{Math.Abs(b)}={Math.Abs(ergebnis)}); {(Math.Sign(a) * Math.Sign(b) > 0 ? "gleiche Vorzeichen → Plus" : "verschiedene Vorzeichen → Minus")} = {ergebnis}."
            },
            HelpHint = "Vorzeichenregeln: Minus mal Minus = Plus, Minus mal Plus = Minus. Beim Addieren/Subtrahieren hilft die Zahlengerade."
        };
    }

    private static QuizQuestion ProzentwertBerechnen(Random r)
    {
        int[] saetze = { 5, 10, 15, 20, 25, 30, 40, 50, 60, 75 };
        int prozentsatz = saetze[r.Next(saetze.Length)];
        int grundwert = r.Next(2, 21) * 20; // Vielfaches von 20 → Ergebnis mit 5er-Sätzen immer ganzzahlig
        int prozentwert = grundwert * prozentsatz / 100;

        return new QuizQuestion
        {
            Id = NewId(),
            Subject = Subject.Mathematik,
            GradeLevel = GradeLevel.Klasse7,
            Topic = "Prozentrechnung",
            Type = QuestionType.OpenText,
            Prompt = $"Eine Jacke kostet {grundwert} €. Sie ist um {prozentsatz}% reduziert. Wie viel Euro sparst du? (nur die Zahl)",
            CorrectAnswers = new[] { prozentwert.ToString() },
            Explanation = $"Prozentwert = Grundwert · Prozentsatz : 100 = {grundwert} · {prozentsatz} : 100 = {prozentwert} €.",
            HelpHint = "Prozentwert = Grundwert · Prozentsatz : 100. Tipp: 10% sind ein Zehntel des Grundwerts."
        };
    }

    private static QuizQuestion EinfacheZinsen(Random r)
    {
        int[] kapitale = { 200, 400, 500, 800, 1000, 1500, 2000, 2500 };
        int kapital = kapitale[r.Next(kapitale.Length)];
        int zinssatz = r.Next(2, 6);
        int zinsen = kapital * zinssatz / 100;

        return new QuizQuestion
        {
            Id = NewId(),
            Subject = Subject.Mathematik,
            GradeLevel = GradeLevel.Klasse7,
            Topic = "Zinsrechnung (einfach)",
            Type = QuestionType.OpenText,
            Prompt = $"Du legst {kapital} € für ein Jahr zu {zinssatz}% Zinsen an. Wie viel Euro Zinsen bekommst du nach einem Jahr? (nur die Zahl)",
            CorrectAnswers = new[] { zinsen.ToString() },
            Explanation = $"Zinsen = Kapital · Zinssatz : 100 = {kapital} · {zinssatz} : 100 = {zinsen} €.",
            HelpHint = "Zinsen für ein Jahr = Kapital · Zinssatz : 100 - das ist Prozentrechnung mit Geld."
        };
    }

    private static QuizQuestion TermeZusammenfassen(Random r)
    {
        int a = r.Next(2, 10);
        int b = r.Next(2, 10);
        int c = r.Next(1, Math.Min(a + b, 9));
        int ergebnis = a + b - c;

        return new QuizQuestion
        {
            Id = NewId(),
            Subject = Subject.Mathematik,
            GradeLevel = GradeLevel.Klasse7,
            Topic = "Terme zusammenfassen",
            Type = QuestionType.OpenText,
            Prompt = $"Fasse zusammen: {a}x + {b}x - {c}x. Wie viele x bleiben übrig? (nur die Zahl vor dem x)",
            CorrectAnswers = new[] { ergebnis.ToString() },
            Explanation = $"Gleichartige Terme werden über ihre Koeffizienten zusammengefasst: {a} + {b} - {c} = {ergebnis}, also {ergebnis}x.",
            HelpHint = "Nur die Zahlen vor dem x verrechnen (Koeffizienten) - das x bleibt stehen."
        };
    }

    private static QuizQuestion EinfacheGleichungen(Random r)
    {
        int x = r.Next(2, 13);
        int a = r.Next(2, 7);
        int b = r.Next(1, 21);
        int c = a * x + b;

        return new QuizQuestion
        {
            Id = NewId(),
            Subject = Subject.Mathematik,
            GradeLevel = GradeLevel.Klasse7,
            Topic = "Einfache Gleichungen",
            Type = QuestionType.OpenText,
            Prompt = $"Löse die Gleichung: {a}x + {b} = {c}. Wie groß ist x?",
            CorrectAnswers = new[] { x.ToString() },
            Explanation = $"Beide Seiten minus {b}: {a}x = {c - b}. Beide Seiten durch {a}: x = {x}. Probe: {a}·{x} + {b} = {c} ✓",
            HelpHint = "Erst die Zahl ohne x auf die andere Seite bringen (minus rechnen), dann durch die Zahl vor dem x teilen."
        };
    }

    private static QuizQuestion DreisatzProportional(Random r)
    {
        int einzelpreis = r.Next(2, 7);
        int menge1 = r.Next(2, 6);
        int menge2 = r.Next(menge1 + 1, 13);
        int preis1 = einzelpreis * menge1;
        int preis2 = einzelpreis * menge2;

        return new QuizQuestion
        {
            Id = NewId(),
            Subject = Subject.Mathematik,
            GradeLevel = GradeLevel.Klasse7,
            Topic = "Zuordnungen und Dreisatz",
            Type = QuestionType.OpenText,
            Prompt = $"{menge1} kg Äpfel kosten {preis1} €. Wie viel Euro kosten {menge2} kg? (nur die Zahl)",
            CorrectAnswers = new[] { preis2.ToString() },
            Explanation = $"Dreisatz: 1 kg kostet {preis1} : {menge1} = {einzelpreis} €. Also kosten {menge2} kg: {menge2} · {einzelpreis} = {preis2} €.",
            HelpHint = "Dreisatz bei proportionalen Zuordnungen: erst auf 1 Einheit herunterrechnen (teilen), dann auf die gesuchte Menge hochrechnen (malnehmen)."
        };
    }

    private static QuizQuestion WinkelBerechnen(Random r)
    {
        int variante = r.Next(3);

        if (variante == 0)
        {
            int alpha = r.Next(30, 91);
            int beta = r.Next(30, Math.Min(180 - alpha - 10, 100));
            int gamma = 180 - alpha - beta;

            return new QuizQuestion
            {
                Id = NewId(),
                Subject = Subject.Mathematik,
                GradeLevel = GradeLevel.Klasse7,
                Topic = "Winkel",
                Type = QuestionType.OpenText,
                Prompt = $"In einem Dreieck sind zwei Winkel bekannt: α = {alpha}° und β = {beta}°. Wie groß ist der dritte Winkel γ in Grad? (nur die Zahl)",
                CorrectAnswers = new[] { gamma.ToString() },
                Explanation = $"Die Winkelsumme im Dreieck beträgt 180°: γ = 180° - {alpha}° - {beta}° = {gamma}°.",
                HelpHint = "Winkelsumme im Dreieck: α + β + γ = 180°."
            };
        }

        if (variante == 1)
        {
            int alpha = r.Next(20, 161);
            int neben = 180 - alpha;

            return new QuizQuestion
            {
                Id = NewId(),
                Subject = Subject.Mathematik,
                GradeLevel = GradeLevel.Klasse7,
                Topic = "Winkel",
                Type = QuestionType.OpenText,
                Prompt = $"Zwei Geraden schneiden sich. Ein Winkel beträgt {alpha}°. Wie groß ist sein Nebenwinkel in Grad? (nur die Zahl)",
                CorrectAnswers = new[] { neben.ToString() },
                Explanation = $"Nebenwinkel ergänzen sich zu 180°: 180° - {alpha}° = {neben}°.",
                HelpHint = "Nebenwinkel liegen auf einer Geraden nebeneinander und ergeben zusammen 180°."
            };
        }

        int winkel = r.Next(20, 161);
        return new QuizQuestion
        {
            Id = NewId(),
            Subject = Subject.Mathematik,
            GradeLevel = GradeLevel.Klasse7,
            Topic = "Winkel",
            Type = QuestionType.OpenText,
            Prompt = $"Zwei Geraden schneiden sich. Ein Winkel beträgt {winkel}°. Wie groß ist sein Scheitelwinkel in Grad? (nur die Zahl)",
            CorrectAnswers = new[] { winkel.ToString() },
            Explanation = $"Scheitelwinkel liegen sich am Schnittpunkt gegenüber und sind immer gleich groß: {winkel}°.",
            HelpHint = "Scheitelwinkel liegen sich gegenüber und sind IMMER gleich groß."
        };
    }

    private static QuizQuestion FlaechenVielecke(Random r)
    {
        int variante = r.Next(3);

        if (variante == 0)
        {
            int grundseite = r.Next(2, 13) * 2; // gerade → Fläche ganzzahlig
            int hoehe = r.Next(2, 11);
            int flaeche = grundseite * hoehe / 2;

            return new QuizQuestion
            {
                Id = NewId(),
                Subject = Subject.Mathematik,
                GradeLevel = GradeLevel.Klasse7,
                Topic = "Flächen von Vielecken",
                Type = QuestionType.OpenText,
                Prompt = $"Ein Dreieck hat die Grundseite g = {grundseite} cm und die Höhe h = {hoehe} cm. Wie groß ist sein Flächeninhalt in cm²? (nur die Zahl)",
                CorrectAnswers = new[] { flaeche.ToString() },
                Explanation = $"A = g · h : 2 = {grundseite} · {hoehe} : 2 = {flaeche} cm².",
                HelpHint = "Dreiecksfläche: A = Grundseite · Höhe : 2."
            };
        }

        if (variante == 1)
        {
            int grundseite = r.Next(3, 13);
            int hoehe = r.Next(2, 11);
            int flaeche = grundseite * hoehe;

            return new QuizQuestion
            {
                Id = NewId(),
                Subject = Subject.Mathematik,
                GradeLevel = GradeLevel.Klasse7,
                Topic = "Flächen von Vielecken",
                Type = QuestionType.OpenText,
                Prompt = $"Ein Parallelogramm hat die Grundseite g = {grundseite} cm und die Höhe h = {hoehe} cm. Wie groß ist sein Flächeninhalt in cm²? (nur die Zahl)",
                CorrectAnswers = new[] { flaeche.ToString() },
                Explanation = $"A = g · h = {grundseite} · {hoehe} = {flaeche} cm².",
                HelpHint = "Parallelogrammfläche: A = Grundseite · Höhe (nicht die schräge Seite!)."
            };
        }

        int a = r.Next(3, 11);
        int c = a + r.Next(1, 6);
        if ((a + c) % 2 != 0) c++; // Summe gerade → Fläche ganzzahlig
        int h = r.Next(2, 9);
        int trapezflaeche = (a + c) * h / 2;

        return new QuizQuestion
        {
            Id = NewId(),
            Subject = Subject.Mathematik,
            GradeLevel = GradeLevel.Klasse7,
            Topic = "Flächen von Vielecken",
            Type = QuestionType.OpenText,
            Prompt = $"Ein Trapez hat die parallelen Seiten a = {a} cm und c = {c} cm sowie die Höhe h = {h} cm. Wie groß ist sein Flächeninhalt in cm²? (nur die Zahl)",
            CorrectAnswers = new[] { trapezflaeche.ToString() },
            Explanation = $"A = (a + c) : 2 · h = ({a} + {c}) : 2 · {h} = {(a + c) / 2} · {h} = {trapezflaeche} cm².",
            HelpHint = "Trapezfläche: Mittelwert der parallelen Seiten mal Höhe: A = (a + c) : 2 · h."
        };
    }

    private static QuizQuestion WahrscheinlichkeitUrne(Random r)
    {
        int rote = r.Next(1, 7);
        int blaue = r.Next(1, 7);
        var (z, n) = Reduce(rote, rote + blaue);
        string ergebnis = n == 1 ? $"{z}" : $"{z}/{n}";

        return new QuizQuestion
        {
            Id = NewId(),
            Subject = Subject.Mathematik,
            GradeLevel = GradeLevel.Klasse7,
            Topic = "Wahrscheinlichkeit (einstufig)",
            Type = QuestionType.OpenText,
            Prompt = $"In einer Urne liegen {rote} rote und {blaue} blaue Kugeln. Du ziehst eine Kugel, ohne hinzusehen. " +
                     $"Wie groß ist die Wahrscheinlichkeit für Rot? (gekürzter Bruch als z/n oder ganze Zahl)",
            CorrectAnswers = new[] { ergebnis },
            Explanation = $"P(Rot) = günstige : mögliche Ergebnisse = {rote} : {rote + blaue} = {ergebnis} (gekürzt).",
            HelpHint = "Wahrscheinlichkeit = Anzahl günstige Ergebnisse geteilt durch Anzahl aller möglichen Ergebnisse."
        };
    }
}
