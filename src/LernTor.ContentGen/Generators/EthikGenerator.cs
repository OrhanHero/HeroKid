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
            "Toleranz heißt, Andersartigkeit auszuhalten und zu respektieren, ohne sie zwingend selbst zu übernehmen."),
        ("Was versteht man unter einem \"Wert\" (z.B. Ehrlichkeit, Respekt)?", new[] { "Eine Vorstellung davon, was im Leben wichtig und erstrebenswert ist", "Eine Zahl auf einem Preisschild", "Eine feste gesetzliche Vorschrift" },
            "Eine Vorstellung davon, was im Leben wichtig und erstrebenswert ist", "Werte wie Ehrlichkeit oder Respekt leiten unser Handeln, auch wenn sie - anders als Gesetze - nicht erzwingbar sind."),
        ("Warum kann es zwischen Regeln und dem eigenen Gewissen manchmal einen Konflikt geben?", new[] { "Weil eine Regel im Einzelfall dem eigenen Wertempfinden widersprechen kann", "Weil Regeln und Gewissen immer exakt übereinstimmen", "Weil es kein Gewissen gibt" },
            "Weil eine Regel im Einzelfall dem eigenen Wertempfinden widersprechen kann", "Manchmal fühlt sich eine Regel im Einzelfall ungerecht an - dann muss man abwägen, wie man verantwortungsvoll handelt."),
        ("Was bedeutet \"Respekt\" im Umgang mit anderen Menschen?", new[] { "Andere wertschätzend behandeln, auch bei Unterschieden", "Nur Menschen respektieren, die genauso denken wie man selbst", "Andere Meinungen laut niedermachen" },
            "Andere wertschätzend behandeln, auch bei Unterschieden", "Respekt bedeutet, andere Menschen unabhängig von Unterschieden wertschätzend und fair zu behandeln.")
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
            "Ruhig miteinander reden und zuhören", "Ein offenes, ruhiges Gespräch, in dem beide Seiten zuhören, hilft meist mehr als Streit oder Rückzug."),
        ("Was bedeutet \"Empathie\" in einer Freundschaft?", new[] { "Sich in die Gefühle des anderen hineinversetzen können", "Immer nur an sich selbst denken", "Nie über Gefühle sprechen" },
            "Sich in die Gefühle des anderen hineinversetzen können", "Empathie heißt, die Gefühle und die Sichtweise einer anderen Person nachzuvollziehen - das stärkt Freundschaften."),
        ("Was ist ein Zeichen von Loyalität unter Freunden?", new[] { "Füreinander einstehen, auch wenn es unbequem ist", "Nur bei gutem Wetter füreinander da sein", "Geheimnisse sofort an alle weitererzählen" },
            "Füreinander einstehen, auch wenn es unbequem ist", "Loyalität bedeutet, zu einem Freund/einer Freundin zu stehen, auch in schwierigen Situationen, und Vertrauen nicht zu missbrauchen."),
        ("Was sollte man tun, wenn ein Freund/eine Freundin von anderen gemobbt wird?", new[] { "Sich schützend dazustellen und Hilfe holen", "Wegschauen, damit man selbst nicht betroffen ist", "Beim Mobbing mitmachen" },
            "Sich schützend dazustellen und Hilfe holen", "Wegschauen lässt Mobbing weitergehen - echte Freundschaft zeigt sich darin, Unterstützung zu geben und ggf. eine Vertrauensperson einzubeziehen.")
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
            "Pessach (Passah) erinnert im Judentum an die Befreiung aus der Sklaverei in Ägypten."),
        ("Wie heißt das heilige Buch des Islam?", new[] { "Der Koran", "Die Bibel", "Die Tora" }, "Der Koran",
            "Der Koran gilt im Islam als das durch den Propheten Mohammed offenbarte heilige Buch."),
        ("Wie heißt der Fastenmonat im Islam?", new[] { "Ramadan", "Advent", "Chanukka" }, "Ramadan",
            "Im Ramadan fasten gläubige Musliminnen und Muslime von Sonnenaufgang bis Sonnenuntergang.")
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
            "Eine Pflicht ist verbindlich vorgeschrieben, Freiwilligkeit nicht", "Pflichten (z.B. Schulpflicht) sind verbindlich, während freiwillige Handlungen aus eigenem Antrieb erfolgen."),
        ("Was bedeutet \"Eigenverantwortung\"?", new[] { "Für die eigenen Entscheidungen selbst geradestehen", "Immer auf andere warten, die entscheiden", "Verantwortung komplett ablehnen" },
            "Für die eigenen Entscheidungen selbst geradestehen", "Eigenverantwortung bedeutet, selbst Entscheidungen zu treffen und auch für deren Folgen einzustehen."),
        ("Warum ist es verantwortungsvoll, Zusagen (Versprechen) einzuhalten?", new[] { "Weil andere sich darauf verlassen und Vertrauen entsteht", "Weil man sonst bestraft werden muss", "Zusagen haben keine Bedeutung" },
            "Weil andere sich darauf verlassen und Vertrauen entsteht", "Wer Zusagen einhält, zeigt Verlässlichkeit - das schafft Vertrauen zwischen Menschen."),
        ("Was bedeutet Verantwortung gegenüber der Umwelt?", new[] { "Ressourcen schonen und Rücksicht auf künftige Generationen nehmen", "Nur an den eigenen Vorteil im Moment denken", "Die Umwelt betrifft niemanden persönlich" },
            "Ressourcen schonen und Rücksicht auf künftige Generationen nehmen", "Verantwortungsvolles Handeln bedeutet u.a., Ressourcen sparsam zu nutzen, damit auch künftige Generationen gut leben können.")
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
            "Bei Beleidigung, Verleumdung oder Volksverhetzung", "Auch die Meinungsfreiheit ist nicht grenzenlos: Sie endet dort, wo andere Rechtsgüter wie die Menschenwürde verletzt werden."),
        ("Warum ist Meinungsfreiheit wichtig für eine Demokratie?", new[] { "Weil unterschiedliche Sichtweisen offen diskutiert werden können", "Weil so nur eine Meinung erlaubt ist", "Weil sie Kritik an der Regierung verbietet" },
            "Weil unterschiedliche Sichtweisen offen diskutiert werden können", "Meinungsfreiheit erlaubt offenen Streit um die beste Lösung und Kritik an der Regierung - beides ist zentral für Demokratie."),
        ("Was ist der Unterschied zwischen einer Meinung und einer Tatsache?", new[] { "Eine Tatsache ist beweisbar, eine Meinung ist eine persönliche Einschätzung", "Beides ist immer dasselbe", "Meinungen sind immer wahr" },
            "Eine Tatsache ist beweisbar, eine Meinung ist eine persönliche Einschätzung", "Tatsachen lassen sich überprüfen (z.B. durch Fakten/Belege), während Meinungen persönliche Bewertungen sind."),
        ("Was bedeutet \"Zensur\" und warum ist sie in einer Demokratie problematisch?", new[] { "Staatliche Unterdrückung von Meinungen - widerspricht der freien Meinungsäußerung", "Zensur bedeutet, Bücher besonders zu fördern", "Zensur gibt es in Demokratien überhaupt nicht" },
            "Staatliche Unterdrückung von Meinungen - widerspricht der freien Meinungsäußerung", "Zensur unterdrückt unliebsame Meinungen und widerspricht damit dem demokratischen Grundrecht auf freie Meinungsäußerung.")
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
            "Sie verbreiten bewusst Falschinformationen, die Menschen täuschen", "Fake News manipulieren gezielt die Meinung von Menschen und untergraben Vertrauen in echte Informationen."),
        ("Warum sollte man vor dem Posten eines Fotos von Freunden im Internet um Erlaubnis fragen?", new[] { "Weil jeder selbst über eigene Bilder entscheiden darf (Recht am eigenen Bild)", "Weil das gesetzlich nicht geregelt ist", "Weil es egal ist, was andere darüber denken" },
            "Weil jeder selbst über eigene Bilder entscheiden darf (Recht am eigenen Bild)", "Das \"Recht am eigenen Bild\" bedeutet: Niemand darf ohne Zustimmung Fotos von einer Person veröffentlichen."),
        ("Was ist ein verantwortungsvoller Umgang mit den eigenen Daten im Internet?", new[] { "Nur wenige, gut überlegte persönliche Daten preisgeben", "Alle privaten Informationen öffentlich teilen", "Passwörter mit Freunden teilen" },
            "Nur wenige, gut überlegte persönliche Daten preisgeben", "Sparsamer Umgang mit persönlichen Daten schützt vor Missbrauch, Betrug und ungewollter Weitergabe an Dritte."),
        ("Warum ist es ethisch wichtig, im Internet höflich zu bleiben, obwohl man anonym schreiben könnte?", new[] { "Weil auch hinter anonymen Nutzernamen echte Menschen mit Gefühlen stehen", "Weil Anonymität automatisch alles erlaubt", "Weil es im Internet keine Konsequenzen gibt" },
            "Weil auch hinter anonymen Nutzernamen echte Menschen mit Gefühlen stehen", "Anonymität im Netz ändert nichts daran, dass am anderen Ende ein echter Mensch verletzt werden kann - Respekt gilt auch online.")
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
            "Strafen sollen u.a. Unrecht ausgleichen, künftige Taten verhindern (Abschreckung) und die Gemeinschaft schützen - reine Rache ist ethisch umstritten."),
        ("Was bedeutet \"Chancengleichheit\" in einer gerechten Gesellschaft?", new[] { "Alle sollen unabhängig von Herkunft ähnliche Möglichkeiten haben", "Alle bekommen automatisch dasselbe Ergebnis", "Nur Reiche sollen Chancen bekommen" },
            "Alle sollen unabhängig von Herkunft ähnliche Möglichkeiten haben", "Chancengleichheit bedeutet, dass z.B. Bildungschancen nicht von Herkunft oder Einkommen der Eltern abhängen sollten."),
        ("Warum gilt \"Unschuldsvermutung\" als wichtiges Rechtsprinzip?", new[] { "Jeder gilt bis zum Beweis der Schuld als unschuldig", "Jeder gilt automatisch als schuldig, bis er sich rechtfertigt", "Das Prinzip betrifft nur Erwachsene" },
            "Jeder gilt bis zum Beweis der Schuld als unschuldig", "Die Unschuldsvermutung schützt Angeklagte davor, vorschnell verurteilt zu werden, bevor ein Gericht die Schuld zweifelsfrei festgestellt hat.")
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
