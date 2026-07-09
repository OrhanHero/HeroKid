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
                WahrscheinlichkeitWuerfel
            },
            [GradeLevel.Klasse9] = new List<TopicFactory>
            {
                LineareGleichung,
                LineareFunktion,
                QuadratischeGleichung,
                SatzDesPythagoras,
                Zinsrechnung,
                BinomischeFormel,
                MittelwertUndMedian
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
}
