using LernTor.Core.Enums;
using LernTor.Core.Models;

namespace LernTor.ContentGen.Generators;

/// <summary>Ethik nach Berliner Rahmenlehrplan, Klasse 6 und Klasse 9.</summary>
public sealed class EthikGenerator : ExerciseGeneratorBase
{
    public override Subject Subject => Subject.Ethik;

    protected override IReadOnlyDictionary<GradeLevel, IReadOnlyList<TopicFactory>> TopicsByGrade { get; } =
        new Dictionary<GradeLevel, IReadOnlyList<TopicFactory>>
        {
            [GradeLevel.Klasse6] = new List<TopicFactory> { WerteRegeln, Freundschaft, Weltreligionen },
            [GradeLevel.Klasse9] = new List<TopicFactory> { Verantwortung, Meinungsfreiheit, DigitaleEthik, RechtUndGerechtigkeit }
        };

    private static readonly (string Frage, string[] Optionen, string Antwort, string Erklaerung)[] WerteListe =
    {
        ("Warum braucht eine Gemeinschaft (z.B. eine Schulklasse) Regeln?", new[] { "Damit alle fair und sicher miteinander leben können", "Damit nur einer bestimmen darf", "Regeln sind nicht nötig" },
            "Damit alle fair und sicher miteinander leben können", "Regeln schaffen Orientierung und schützen davor, dass Einzelne benachteiligt oder verletzt werden."),
        ("Was bedeutet \"Toleranz\"?", new[] { "Andere Meinungen und Lebensweisen respektieren, auch wenn man sie nicht teilt", "Alles gut finden müssen", "Nur die eigene Meinung gelten lassen" },
            "Andere Meinungen und Lebensweisen respektieren, auch wenn man sie nicht teilt",
            "Toleranz heißt, Andersartigkeit auszuhalten und zu respektieren, ohne sie zwingend selbst zu übernehmen.")
    };

    private static QuizQuestion WerteRegeln(Random r)
    {
        var f = WerteListe[r.Next(WerteListe.Length)];
        return new QuizQuestion
        {
            Id = NewId(), Subject = Subject.Ethik, GradeLevel = GradeLevel.Klasse6,
            Topic = "Werte und Regeln", Type = QuestionType.MultipleChoice,
            Prompt = f.Frage, Options = f.Optionen, CorrectAnswers = new[] { f.Antwort }, Explanation = f.Erklaerung,
            HelpHint = "Regeln schützen ein faires, sicheres Miteinander; Toleranz heißt, andere Meinungen zu respektieren, ohne sie teilen zu müssen."
        };
    }

    private static readonly (string Frage, string[] Optionen, string Antwort, string Erklaerung)[] FreundschaftListe =
    {
        ("Was ist ein wichtiges Merkmal echter Freundschaft?", new[] { "Gegenseitiges Vertrauen und Ehrlichkeit", "Immer der gleichen Meinung sein", "Sich nie streiten" },
            "Gegenseitiges Vertrauen und Ehrlichkeit", "Vertrauen und Ehrlichkeit sind zentrale Grundlagen einer stabilen Freundschaft, auch wenn man mal streitet."),
        ("Wie löst man einen Streit mit einem Freund/einer Freundin am besten?", new[] { "Ruhig miteinander reden und zuhören", "Den Kontakt sofort abbrechen", "Beleidigen, bis man Recht bekommt" },
            "Ruhig miteinander reden und zuhören", "Ein offenes, ruhiges Gespräch, in dem beide Seiten zuhören, hilft meist mehr als Streit oder Rückzug.")
    };

    private static QuizQuestion Freundschaft(Random r)
    {
        var f = FreundschaftListe[r.Next(FreundschaftListe.Length)];
        return new QuizQuestion
        {
            Id = NewId(), Subject = Subject.Ethik, GradeLevel = GradeLevel.Klasse6,
            Topic = "Freundschaft und Konflikte", Type = QuestionType.MultipleChoice,
            Prompt = f.Frage, Options = f.Optionen, CorrectAnswers = new[] { f.Antwort }, Explanation = f.Erklaerung,
            HelpHint = "Vertrauen und Ehrlichkeit tragen Freundschaften; Konflikte löst man am besten durch ruhiges Reden und Zuhören."
        };
    }

    private static readonly (string Frage, string[] Optionen, string Antwort, string Erklaerung)[] ReligionenListe =
    {
        ("In welcher Stadt steht die Kaaba, ein zentrales Heiligtum des Islam?", new[] { "Mekka", "Jerusalem", "Rom" }, "Mekka",
            "Die Kaaba in Mekka (Saudi-Arabien) ist das zentrale Heiligtum des Islam, zu dem Muslime pilgern (Hadsch)."),
        ("Welches Fest feiern Christen zur Erinnerung an die Geburt von Jesus?", new[] { "Weihnachten", "Ostern", "Pfingsten" }, "Weihnachten",
            "Weihnachten erinnert im Christentum an die Geburt von Jesus Christus."),
        ("Welches jüdische Fest erinnert an den Auszug aus Ägypten?", new[] { "Pessach", "Chanukka", "Jom Kippur" }, "Pessach",
            "Pessach (Passah) erinnert im Judentum an die Befreiung aus der Sklaverei in Ägypten.")
    };

    private static QuizQuestion Weltreligionen(Random r)
    {
        var f = ReligionenListe[r.Next(ReligionenListe.Length)];
        return new QuizQuestion
        {
            Id = NewId(), Subject = Subject.Ethik, GradeLevel = GradeLevel.Klasse6,
            Topic = "Weltreligionen", Type = QuestionType.MultipleChoice,
            Prompt = f.Frage, Options = f.Optionen, CorrectAnswers = new[] { f.Antwort }, Explanation = f.Erklaerung,
            HelpHint = "Denk an zentrale Orte und Feste der Weltreligionen: Mekka (Islam), Weihnachten (Christentum), Pessach (Judentum)."
        };
    }

    private static readonly (string Frage, string[] Optionen, string Antwort, string Erklaerung)[] VerantwortungListe =
    {
        ("Was bedeutet es, \"Verantwortung zu übernehmen\"?", new[] { "Für die Folgen des eigenen Handelns einzustehen", "Nie Fehler zuzugeben", "Andere für alles verantwortlich zu machen" },
            "Für die Folgen des eigenen Handelns einzustehen", "Verantwortung übernehmen heißt, zu den Konsequenzen des eigenen Tuns zu stehen, statt sie abzuwälzen."),
        ("Was ist der Unterschied zwischen einer Pflicht und einer freiwilligen Handlung?", new[] { "Eine Pflicht ist verbindlich vorgeschrieben, Freiwilligkeit nicht", "Es gibt keinen Unterschied", "Pflichten sind immer freiwillig" },
            "Eine Pflicht ist verbindlich vorgeschrieben, Freiwilligkeit nicht", "Pflichten (z.B. Schulpflicht) sind verbindlich, während freiwillige Handlungen aus eigenem Antrieb erfolgen.")
    };

    private static QuizQuestion Verantwortung(Random r)
    {
        var f = VerantwortungListe[r.Next(VerantwortungListe.Length)];
        return new QuizQuestion
        {
            Id = NewId(), Subject = Subject.Ethik, GradeLevel = GradeLevel.Klasse9,
            Topic = "Verantwortung und Pflicht", Type = QuestionType.MultipleChoice,
            Prompt = f.Frage, Options = f.Optionen, CorrectAnswers = new[] { f.Antwort }, Explanation = f.Erklaerung,
            HelpHint = "Verantwortung übernehmen heißt: für die Folgen des eigenen Handelns einstehen. Pflichten sind verbindlich, Freiwilligkeit nicht."
        };
    }

    private static readonly (string Frage, string[] Optionen, string Antwort, string Erklaerung)[] MeinungsfreiheitListe =
    {
        ("Was garantiert die Meinungsfreiheit im Grundgesetz (Artikel 5)?", new[] { "Jeder darf seine Meinung frei äußern und verbreiten", "Man darf alles ungestraft sagen, egal was", "Nur Journalisten dürfen ihre Meinung sagen" },
            "Jeder darf seine Meinung frei äußern und verbreiten",
            "Artikel 5 GG schützt die freie Meinungsäußerung - sie hat aber Grenzen, z.B. bei Beleidigung oder Volksverhetzung."),
        ("Wo findet Meinungsfreiheit ihre Grenzen?", new[] { "Bei Beleidigung, Verleumdung oder Volksverhetzung", "Sie hat keinerlei Grenzen", "Nur im Internet" },
            "Bei Beleidigung, Verleumdung oder Volksverhetzung", "Auch die Meinungsfreiheit ist nicht grenzenlos: Sie endet dort, wo andere Rechtsgüter wie die Menschenwürde verletzt werden.")
    };

    private static QuizQuestion Meinungsfreiheit(Random r)
    {
        var f = MeinungsfreiheitListe[r.Next(MeinungsfreiheitListe.Length)];
        return new QuizQuestion
        {
            Id = NewId(), Subject = Subject.Ethik, GradeLevel = GradeLevel.Klasse9,
            Topic = "Meinungsfreiheit und Grenzen", Type = QuestionType.MultipleChoice,
            Prompt = f.Frage, Options = f.Optionen, CorrectAnswers = new[] { f.Antwort }, Explanation = f.Erklaerung,
            HelpHint = "Artikel 5 GG schützt freie Meinungsäußerung - sie endet aber dort, wo sie andere Rechte verletzt (z.B. Beleidigung, Volksverhetzung)."
        };
    }

    private static readonly (string Frage, string[] Optionen, string Antwort, string Erklaerung)[] DigitalListe =
    {
        ("Was sollte man tun, wenn man online gemobbt wird (Cybermobbing)?", new[] { "Beweise sichern und eine Vertrauensperson informieren", "Es für sich behalten und ignorieren", "Selbst zurück-mobben" },
            "Beweise sichern und eine Vertrauensperson informieren", "Screenshots sichern und sich Eltern, Lehrkräften oder Beratungsstellen anzuvertrauen ist der empfohlene Weg."),
        ("Was ist ethisch problematisch an manchen \"Fake News\"?", new[] { "Sie verbreiten bewusst Falschinformationen, die Menschen täuschen", "Sie sind immer sofort als falsch erkennbar", "Sie schaden niemandem" },
            "Sie verbreiten bewusst Falschinformationen, die Menschen täuschen", "Fake News manipulieren gezielt die Meinung von Menschen und untergraben Vertrauen in echte Informationen.")
    };

    private static QuizQuestion DigitaleEthik(Random r)
    {
        var f = DigitalListe[r.Next(DigitalListe.Length)];
        return new QuizQuestion
        {
            Id = NewId(), Subject = Subject.Ethik, GradeLevel = GradeLevel.Klasse9,
            Topic = "Digitale Ethik", Type = QuestionType.MultipleChoice,
            Prompt = f.Frage, Options = f.Optionen, CorrectAnswers = new[] { f.Antwort }, Explanation = f.Erklaerung,
            HelpHint = "Bei Cybermobbing: Beweise sichern und Vertrauensperson informieren. Fake News täuschen bewusst - Quelle immer prüfen."
        };
    }

    private static readonly (string Frage, string[] Optionen, string Antwort, string Erklaerung)[] GerechtigkeitListe =
    {
        ("Was bedeutet \"Gerechtigkeit\" ganz allgemein?", new[] { "Jedem das geben, was ihm/ihr zusteht (unter Berücksichtigung fairer Kriterien)", "Allen exakt dasselbe geben, egal um welche Situation es geht", "Nur den Stärkeren geben, was sie wollen" },
            "Jedem das geben, was ihm/ihr zusteht (unter Berücksichtigung fairer Kriterien)",
            "Gerechtigkeit bedeutet nicht zwingend \"alle bekommen gleich viel\", sondern dass Verteilung/Behandlung nach fairen, nachvollziehbaren Kriterien erfolgt."),
        ("Was ist der Unterschied zwischen \"Gleichheit\" und \"Gerechtigkeit\"?", new[] { "Gleichheit heißt \"alle bekommen dasselbe\", Gerechtigkeit berücksichtigt auch unterschiedliche Bedürfnisse/Voraussetzungen", "Es gibt keinen Unterschied", "Gerechtigkeit bedeutet immer strengere Strafen" },
            "Gleichheit heißt \"alle bekommen dasselbe\", Gerechtigkeit berücksichtigt auch unterschiedliche Bedürfnisse/Voraussetzungen",
            "Gerechte Verteilung kann bedeuten, unterschiedliche Startbedingungen auszugleichen, statt allen exakt gleich viel zu geben."),
        ("Welchem Zweck dient eine Strafe für eine Straftat aus ethischer Sicht (u.a.)?", new[] { "Wiedergutmachung, Abschreckung und Schutz der Gemeinschaft", "Nur der Rache", "Sie hat keinen erkennbaren Zweck" },
            "Wiedergutmachung, Abschreckung und Schutz der Gemeinschaft",
            "Strafen sollen u.a. Unrecht ausgleichen, künftige Taten verhindern (Abschreckung) und die Gemeinschaft schützen - reine Rache ist ethisch umstritten.")
    };

    private static QuizQuestion RechtUndGerechtigkeit(Random r)
    {
        var f = GerechtigkeitListe[r.Next(GerechtigkeitListe.Length)];
        return new QuizQuestion
        {
            Id = NewId(), Subject = Subject.Ethik, GradeLevel = GradeLevel.Klasse9,
            Topic = "Recht und Gerechtigkeit", Type = QuestionType.MultipleChoice,
            Prompt = f.Frage, Options = f.Optionen, CorrectAnswers = new[] { f.Antwort }, Explanation = f.Erklaerung,
            HelpHint = "Gerechtigkeit ist nicht dasselbe wie \"alle bekommen gleich viel\" - sie berücksichtigt faire Kriterien und unterschiedliche Voraussetzungen."
        };
    }
}
