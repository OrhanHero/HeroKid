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
            "Andere wertschätzend behandeln, auch bei Unterschieden", "Respekt bedeutet, andere Menschen unabhängig von Unterschieden wertschätzend und fair zu behandeln."),
        ("Was bedeutet \"Ehrlichkeit\" als Wert?", new[] { "Die Wahrheit sagen, auch wenn es unangenehm ist", "Nur sagen, was andere hören wollen", "Lügen, wenn es einem nützt" },
            "Die Wahrheit sagen, auch wenn es unangenehm ist", "Ehrlichkeit bedeutet, auch bei unangenehmen Themen bei der Wahrheit zu bleiben, statt zu beschönigen oder zu lügen."),
        ("Was versteht man unter \"Fairness\"?", new[] { "Andere gerecht und nach denselben Regeln behandeln", "Nur sich selbst Vorteile verschaffen", "Regeln nur für andere gelten lassen" },
            "Andere gerecht und nach denselben Regeln behandeln", "Fairness bedeutet, für alle dieselben Regeln gelten zu lassen und niemanden ungerecht zu bevorzugen oder zu benachteiligen."),
        ("Was bedeutet \"Zivilcourage\"?", new[] { "Sich trauen, für Recht und Gerechtigkeit einzutreten, auch gegen Widerstand", "Immer nur zuschauen", "Sich niemals einmischen" },
            "Sich trauen, für Recht und Gerechtigkeit einzutreten, auch gegen Widerstand", "Zivilcourage bedeutet, sich auch bei Widerstand oder Risiko für Recht und Gerechtigkeit einzusetzen."),
        ("Was bedeutet \"Solidarität\"?", new[] { "Sich mit anderen verbunden fühlen und ihnen in schwierigen Situationen helfen", "Nur an sich selbst denken", "Andere im Stich lassen" },
            "Sich mit anderen verbunden fühlen und ihnen in schwierigen Situationen helfen", "Solidarität bedeutet, sich mit anderen verbunden zu fühlen und ihnen in schwierigen Lagen beizustehen."),
        ("Warum ist Vertrauen wichtig für eine Gemeinschaft?", new[] { "Es ermöglicht ein sicheres Miteinander ohne ständiges Misstrauen", "Vertrauen ist unwichtig", "Vertrauen macht Regeln überflüssig" },
            "Es ermöglicht ein sicheres Miteinander ohne ständiges Misstrauen", "Vertrauen erlaubt ein entspanntes Miteinander, in dem man sich nicht ständig gegenseitig misstrauen muss."),
        ("Was bedeutet \"Verantwortungsbewusstsein\" im Alltag?", new[] { "Über die Folgen des eigenen Handelns nachdenken", "Nie über Konsequenzen nachdenken", "Immer andere die Konsequenzen tragen lassen" },
            "Über die Folgen des eigenen Handelns nachdenken", "Verantwortungsbewusstsein zeigt sich darin, die möglichen Folgen des eigenen Handelns vorher zu bedenken."),
        ("Was ist der Unterschied zwischen einem Wert und einem Gesetz?", new[] { "Werte leiten das Handeln freiwillig, Gesetze sind staatlich vorgeschrieben und durchsetzbar", "Beides ist völlig identisch", "Werte sind immer strenger als Gesetze" },
            "Werte leiten das Handeln freiwillig, Gesetze sind staatlich vorgeschrieben und durchsetzbar", "Werte wirken über die innere Überzeugung, während Gesetze verbindliche, staatlich durchsetzbare Regeln sind."),
        ("Was bedeutet \"Rücksichtnahme\"?", new[] { "Auf die Bedürfnisse und Gefühle anderer achten", "Nur die eigenen Wünsche durchsetzen", "Andere komplett ignorieren" },
            "Auf die Bedürfnisse und Gefühle anderer achten", "Rücksichtnahme bedeutet, das eigene Verhalten auch an den Bedürfnissen und Gefühlen anderer auszurichten."),
        ("Warum ist es wichtig, Fehler zugeben zu können?", new[] { "Weil man daraus lernen und Vertrauen wiederherstellen kann", "Weil Fehler niemals passieren dürfen", "Weil man Fehler immer anderen zuschieben sollte" },
            "Weil man daraus lernen und Vertrauen wiederherstellen kann", "Wer Fehler zugibt, kann daraus lernen und das Vertrauen anderer eher zurückgewinnen, als sie zu vertuschen."),
        ("Was bedeutet \"Gerechtigkeitssinn\"?", new[] { "Ein Gespür dafür, was fair und was unfair ist", "Immer nur die eigene Meinung als gerecht ansehen", "Gerechtigkeit interessiert einen nicht" },
            "Ein Gespür dafür, was fair und was unfair ist", "Ein ausgeprägter Gerechtigkeitssinn hilft, faire von unfairen Situationen zu unterscheiden."),
        ("Was ist ein Wertekonflikt?", new[] { "Wenn zwei wichtige Werte im Widerspruch zueinander stehen und man abwägen muss", "Wenn alle Werte immer perfekt zusammenpassen", "Ein Streit ohne jeden ethischen Bezug" },
            "Wenn zwei wichtige Werte im Widerspruch zueinander stehen und man abwägen muss", "Ein Wertekonflikt entsteht, wenn z.B. Ehrlichkeit und Rücksichtnahme im Einzelfall gegeneinander abgewogen werden müssen."),
        ("Wie kann man Regeln in einer Klasse gemeinsam sinnvoll gestalten?", new[] { "Indem alle Beteiligten mitreden und Regeln gemeinsam vereinbart werden", "Indem nur eine Person allein entscheidet", "Indem es gar keine Regeln gibt" },
            "Indem alle Beteiligten mitreden und Regeln gemeinsam vereinbart werden", "Gemeinsam erarbeitete Regeln werden meist eher akzeptiert, weil sich alle Beteiligten einbringen konnten."),
        ("Was bedeutet Dankbarkeit als Wert?", new[] { "Wertschätzen, was man von anderen bekommt oder erlebt", "Nie zufrieden sein", "Immer mehr fordern, ohne etwas zu schätzen" },
            "Wertschätzen, was man von anderen bekommt oder erlebt", "Dankbarkeit bedeutet, bewusst wahrzunehmen und wertzuschätzen, was man von anderen oder im Leben bekommt."),
        ("Warum sind Werte in unterschiedlichen Kulturen manchmal verschieden gewichtet?", new[] { "Weil Geschichte, Religion und Traditionen das Wertesystem einer Gesellschaft prägen", "Weil es überall auf der Welt exakt dieselben Werte gibt", "Weil Werte keinen kulturellen Hintergrund haben" },
            "Weil Geschichte, Religion und Traditionen das Wertesystem einer Gesellschaft prägen", "Geschichte, Religion und Traditionen prägen, welche Werte in einer Gesellschaft besonders wichtig genommen werden."),
        ("Was bedeutet \"Zivilcourage zeigen\", wenn man Unrecht beobachtet?", new[] { "Eingreifen oder Hilfe holen, statt wegzusehen", "Immer schweigen und nichts unternehmen", "Das Unrecht selbst noch verstärken" },
            "Eingreifen oder Hilfe holen, statt wegzusehen", "Zivilcourage zeigt sich darin, bei beobachtetem Unrecht einzugreifen oder gezielt Hilfe zu holen, statt wegzuschauen.")
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
            "Sich schützend dazustellen und Hilfe holen", "Wegschauen lässt Mobbing weitergehen - echte Freundschaft zeigt sich darin, Unterstützung zu geben und ggf. eine Vertrauensperson einzubeziehen."),
        ("Was bedeutet gegenseitiger Respekt in einer Freundschaft?", new[] { "Die Meinungen und Grenzen des anderen ernst nehmen", "Immer eigene Wünsche durchsetzen", "Die Gefühle des anderen ignorieren" },
            "Die Meinungen und Grenzen des anderen ernst nehmen", "Respekt in einer Freundschaft zeigt sich darin, Meinungen und Grenzen des anderen ernst zu nehmen."),
        ("Warum sind gemeinsame Erlebnisse wichtig für eine Freundschaft?", new[] { "Sie stärken Vertrauen und Verbundenheit", "Sie sind völlig unwichtig", "Sie schwächen die Beziehung" },
            "Sie stärken Vertrauen und Verbundenheit", "Gemeinsame Erlebnisse schaffen geteilte Erinnerungen und stärken Vertrauen und Verbundenheit."),
        ("Was ist ein Zeichen von Neid statt echter Freude für einen Freund/eine Freundin?", new[] { "Sich über den Erfolg des anderen zu ärgern statt sich mitzufreuen", "Sich ehrlich mitzufreuen", "Den anderen zu unterstützen" },
            "Sich über den Erfolg des anderen zu ärgern statt sich mitzufreuen", "Neid zeigt sich darin, sich über den Erfolg eines Freundes zu ärgern, statt sich ehrlich mitzufreuen."),
        ("Warum ist es wichtig, in einer Freundschaft auch \"Nein\" sagen zu können?", new[] { "Um eigene Grenzen zu wahren und ehrlich zu bleiben", "Weil man nie widersprechen darf", "Weil Freundschaft bedeutet, alles mitzumachen" },
            "Um eigene Grenzen zu wahren und ehrlich zu bleiben", "Auch in Freundschaften ist es wichtig, eigene Grenzen zu wahren und ehrlich zu bleiben, statt allem zuzustimmen."),
        ("Was kann helfen, wenn zwei Freunde unterschiedlicher Meinung sind?", new[] { "Beide Sichtweisen anhören und einen Kompromiss suchen", "Sofort die Freundschaft beenden", "Nur eine Meinung als richtig gelten lassen" },
            "Beide Sichtweisen anhören und einen Kompromiss suchen", "Beide Sichtweisen anzuhören und gemeinsam nach einem Kompromiss zu suchen, hilft, Konflikte zu lösen."),
        ("Was bedeutet \"Verlässlichkeit\" unter Freunden?", new[] { "Absprachen einhalten und für den anderen da sein", "Versprechen regelmäßig brechen", "Nur da sein, wenn es einem selbst nützt" },
            "Absprachen einhalten und für den anderen da sein", "Verlässliche Freunde halten Absprachen ein und sind auch in schwierigen Momenten füreinander da."),
        ("Warum kann Gruppenzwang eine Freundschaft gefährden?", new[] { "Weil man sich unter Druck gesetzt fühlt, gegen eigene Werte zu handeln", "Weil Gruppenzwang immer positiv ist", "Weil es Gruppenzwang gar nicht gibt" },
            "Weil man sich unter Druck gesetzt fühlt, gegen eigene Werte zu handeln", "Gruppenzwang kann dazu führen, gegen die eigenen Werte zu handeln, was echte Freundschaften belasten kann."),
        ("Was sollte man tun, wenn ein Freund/eine Freundin einen bittet, bei etwas Unehrlichem mitzumachen?", new[] { "Ehrlich Nein sagen und die eigene Haltung erklären", "Immer sofort mitmachen", "Die Freundschaft sofort beenden, ohne zu reden" },
            "Ehrlich Nein sagen und die eigene Haltung erklären", "Ehrlich Nein zu sagen und die eigene Haltung zu erklären, ist verantwortungsvoller, als unehrlich mitzumachen oder wortlos die Freundschaft zu beenden."),
        ("Was bedeutet es, in einer Freundschaft \"authentisch\" zu sein?", new[] { "Man selbst sein, statt sich für andere zu verstellen", "Sich immer komplett anzupassen", "Nie die eigene Meinung zeigen" },
            "Man selbst sein, statt sich für andere zu verstellen", "Authentisch zu sein bedeutet, in der Freundschaft man selbst zu bleiben, statt sich ständig zu verstellen."),
        ("Warum ist Kommunikation wichtig, um Missverständnisse unter Freunden zu klären?", new[] { "Nur im Gespräch können Missverständnisse aufgeklärt werden", "Schweigen klärt Missverständnisse automatisch", "Kommunikation verschlimmert Konflikte immer" },
            "Nur im Gespräch können Missverständnisse aufgeklärt werden", "Offene Kommunikation ermöglicht es, Missverständnisse zu klären, die sonst zu unnötigen Konflikten führen könnten."),
        ("Was ist ein Unterschied zwischen einer Bekanntschaft und einer engen Freundschaft?", new[] { "Enge Freundschaft beruht auf tieferem Vertrauen und gegenseitiger Unterstützung", "Es gibt keinen Unterschied", "Bekanntschaften sind immer enger als Freundschaften" },
            "Enge Freundschaft beruht auf tieferem Vertrauen und gegenseitiger Unterstützung", "Enge Freundschaften zeichnen sich durch tieferes Vertrauen und stärkere gegenseitige Unterstützung aus als lockere Bekanntschaften."),
        ("Wie kann man sich entschuldigen, wenn man einen Freund/eine Freundin verletzt hat?", new[] { "Ehrlich den Fehler eingestehen und aufrichtig um Entschuldigung bitten", "Den Vorfall einfach ignorieren", "Dem anderen die Schuld geben" },
            "Ehrlich den Fehler eingestehen und aufrichtig um Entschuldigung bitten", "Eine aufrichtige Entschuldigung setzt voraus, den eigenen Fehler ehrlich einzugestehen, statt ihn zu ignorieren oder abzuwälzen."),
        ("Was bedeutet \"Grenzen respektieren\" in einer Freundschaft?", new[] { "Akzeptieren, wenn der andere etwas nicht möchte", "Den anderen so lange überreden, bis er nachgibt", "Grenzen anderer bewusst ignorieren" },
            "Akzeptieren, wenn der andere etwas nicht möchte", "Grenzen zu respektieren heißt, die Entscheidung des anderen zu akzeptieren, statt ihn zu etwas zu drängen."),
        ("Warum kann es guttun, auch Freundschaften mit unterschiedlichen Interessen zu haben?", new[] { "Man lernt neue Perspektiven und Ideen kennen", "Es führt automatisch zu Streit", "Unterschiedliche Interessen sind immer schädlich" },
            "Man lernt neue Perspektiven und Ideen kennen", "Freundschaften mit unterschiedlichen Interessen eröffnen neue Perspektiven und erweitern den eigenen Horizont."),
        ("Was ist wichtig, wenn man merkt, dass eine Freundschaft einem nicht mehr guttut?", new[] { "Ehrlich mit sich selbst und dem anderen darüber sprechen", "Einfach kommentarlos verschwinden", "Den anderen öffentlich schlechtmachen" },
            "Ehrlich mit sich selbst und dem anderen darüber sprechen", "Ein ehrliches Gespräch mit sich selbst und dem anderen ist meist fairer als plötzliches Verschwinden oder Bloßstellen.")
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
            "Im Ramadan fasten gläubige Musliminnen und Muslime von Sonnenaufgang bis Sonnenuntergang."),
        ("Wie heißt das heilige Buch des Christentums?", new[] { "Die Bibel", "Der Koran", "Die Tora" }, "Die Bibel",
            "Die Bibel, bestehend aus Altem und Neuem Testament, ist die heilige Schrift des Christentums."),
        ("Wie heißt das heilige Buch des Judentums?", new[] { "Die Tora", "Der Koran", "Die Bibel" }, "Die Tora",
            "Die Tora, die fünf Bücher Mose, bildet den zentralen heiligen Text des Judentums."),
        ("Welches Fest feiern Christen zur Erinnerung an die Auferstehung von Jesus?", new[] { "Ostern", "Weihnachten", "Pfingsten" }, "Ostern",
            "Ostern erinnert im Christentum an die Auferstehung von Jesus Christus."),
        ("Wie heißt das jüdische Lichterfest im Winter?", new[] { "Chanukka", "Ramadan", "Ostern" }, "Chanukka",
            "Chanukka ist ein achttägiges jüdisches Lichterfest, das im Winter gefeiert wird."),
        ("In welcher Stadt liegt die Klagemauer, eine wichtige heilige Stätte des Judentums?", new[] { "Jerusalem", "Mekka", "Rom" }, "Jerusalem",
            "Die Klagemauer in Jerusalem ist eine der heiligsten Stätten des Judentums."),
        ("Wie nennt man das Gebetshaus der Muslime?", new[] { "Moschee", "Synagoge", "Kirche" }, "Moschee",
            "Muslime versammeln sich zum gemeinsamen Gebet in der Moschee."),
        ("Wie nennt man das Gebetshaus der Juden?", new[] { "Synagoge", "Moschee", "Kirche" }, "Synagoge",
            "Die Synagoge ist der zentrale Versammlungs- und Gebetsort der jüdischen Gemeinde."),
        ("Wie viele Weltreligionen zählt man häufig zu den größten (Christentum, Islam, Hinduismus, Buddhismus, Judentum)?", new[] { "5 große Weltreligionen", "Nur 1", "Über 100" }, "5 große Weltreligionen",
            "Christentum, Islam, Hinduismus, Buddhismus und Judentum zählen zu den fünf am häufigsten genannten Weltreligionen."),
        ("Welche Religion glaubt an Wiedergeburt (Reinkarnation) als zentrales Element, z.B. der Hinduismus und Buddhismus?", new[] { "Hinduismus und Buddhismus", "Christentum und Judentum", "Islam und Christentum" }, "Hinduismus und Buddhismus",
            "Im Hinduismus und Buddhismus spielt der Glaube an Wiedergeburt (Reinkarnation) eine zentrale Rolle."),
        ("Wer gilt im Buddhismus als zentrale Lehrfigur?", new[] { "Buddha (Siddhartha Gautama)", "Jesus Christus", "Der Prophet Mohammed" }, "Buddha (Siddhartha Gautama)",
            "Siddhartha Gautama, bekannt als Buddha, gilt als Begründer der buddhistischen Lehre."),
        ("Was bedeutet der Begriff \"interreligiöser Dialog\"?", new[] { "Der respektvolle Austausch zwischen Angehörigen verschiedener Religionen", "Ein Streit zwischen Religionen", "Das Verbot, über Religion zu sprechen" }, "Der respektvolle Austausch zwischen Angehörigen verschiedener Religionen",
            "Interreligiöser Dialog bedeutet den respektvollen Austausch und das gegenseitige Kennenlernen zwischen Angehörigen unterschiedlicher Religionen."),
        ("Was verbindet Christentum, Judentum und Islam als gemeinsame Wurzel?", new[] { "Sie glauben alle an einen einzigen Gott (monotheistische Religionen)", "Sie haben überhaupt keine Gemeinsamkeiten", "Sie glauben an viele verschiedene Götter" }, "Sie glauben alle an einen einzigen Gott (monotheistische Religionen)",
            "Christentum, Judentum und Islam sind monotheistische Religionen, die alle an einen einzigen Gott glauben."),
        ("Was bedeutet Religionsfreiheit, wie sie z.B. im deutschen Grundgesetz verankert ist?", new[] { "Jeder darf seine Religion frei wählen, ausüben oder auch keine haben", "Nur eine Religion ist im Land erlaubt", "Religion ist gesetzlich verboten" }, "Jeder darf seine Religion frei wählen, ausüben oder auch keine haben",
            "Religionsfreiheit garantiert jedem Menschen, seine Religion frei zu wählen, auszuüben oder auch religionslos zu sein."),
        ("Welches Symbol steht für das Christentum?", new[] { "Das Kreuz", "Der Halbmond", "Der Davidstern" }, "Das Kreuz",
            "Das Kreuz erinnert an die Kreuzigung Jesu und ist das zentrale Symbol des Christentums."),
        ("Welches Symbol steht für das Judentum?", new[] { "Der Davidstern (Magen David)", "Das Kreuz", "Der Halbmond" }, "Der Davidstern (Magen David)",
            "Der Davidstern (Magen David) ist ein zentrales Symbol des Judentums.")
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
            "Ressourcen schonen und Rücksicht auf künftige Generationen nehmen", "Verantwortungsvolles Handeln bedeutet u.a., Ressourcen sparsam zu nutzen, damit auch künftige Generationen gut leben können."),
        ("Was bedeutet \"soziale Verantwortung\" von Unternehmen?", new[] { "Auch auf faire Arbeitsbedingungen und Umweltschutz zu achten, nicht nur auf Gewinn", "Nur maximalen Gewinn zu erzielen", "Verantwortung betrifft nur den Staat" },
            "Auch auf faire Arbeitsbedingungen und Umweltschutz zu achten, nicht nur auf Gewinn", "Soziale Verantwortung bedeutet für Unternehmen, neben Gewinn auch faire Arbeitsbedingungen und Umweltschutz im Blick zu behalten."),
        ("Was bedeutet Verantwortung gegenüber sich selbst?", new[] { "Auf die eigene Gesundheit und Entwicklung achten", "Sich selbst komplett vernachlässigen", "Nur an andere denken, nie an sich selbst" },
            "Auf die eigene Gesundheit und Entwicklung achten", "Verantwortung gegenüber sich selbst bedeutet, gut auf die eigene Gesundheit, Bildung und Entwicklung zu achten."),
        ("Warum trägt man in einer Gruppe (z.B. Team, Klasse) auch Mitverantwortung?", new[] { "Weil das eigene Handeln Auswirkungen auf die ganze Gruppe haben kann", "Weil man in einer Gruppe keinerlei Verantwortung trägt", "Weil nur eine einzelne Person je verantwortlich ist" },
            "Weil das eigene Handeln Auswirkungen auf die ganze Gruppe haben kann", "In einer Gruppe wirkt sich das eigene Verhalten oft auf alle anderen aus - deshalb trägt jede und jeder Mitverantwortung."),
        ("Was ist der Unterschied zwischen moralischer und rechtlicher Verantwortung?", new[] { "Moralische Verantwortung beruht auf dem eigenen Gewissen, rechtliche auf Gesetzen", "Beides ist identisch", "Es gibt nur rechtliche Verantwortung" },
            "Moralische Verantwortung beruht auf dem eigenen Gewissen, rechtliche auf Gesetzen", "Moralische Verantwortung entsteht aus dem eigenen Gewissen, während rechtliche Verantwortung durch Gesetze festgelegt und einklagbar ist."),
        ("Warum ist es verantwortungsvoll, Konsequenzen des eigenen Handelns vorher zu bedenken?", new[] { "Um mögliche Schäden für sich und andere zu vermeiden", "Weil Konsequenzen nie eintreten", "Weil Nachdenken unnötig ist" },
            "Um mögliche Schäden für sich und andere zu vermeiden", "Wer Konsequenzen vorher bedenkt, kann mögliche Schäden für sich selbst und andere eher vermeiden."),
        ("Was bedeutet intergenerationelle Verantwortung (Verantwortung gegenüber künftigen Generationen)?", new[] { "Heute so handeln, dass auch kommende Generationen gut leben können", "Nur an die eigene Generation denken", "Zukünftige Generationen betreffen einen nicht" },
            "Heute so handeln, dass auch kommende Generationen gut leben können", "Intergenerationelle Verantwortung bedeutet, heutige Entscheidungen so zu treffen, dass auch künftige Generationen gut leben können."),
        ("Was bedeutet \"Verantwortung teilen\" in einem Team?", new[] { "Aufgaben und Konsequenzen gemeinsam tragen", "Nur eine Person soll alles allein verantworten", "Verantwortung kann man niemals teilen" },
            "Aufgaben und Konsequenzen gemeinsam tragen", "Geteilte Verantwortung bedeutet, dass Aufgaben und ihre Konsequenzen gemeinsam im Team getragen werden."),
        ("Warum ist Verlässlichkeit ein Teil von Verantwortungsbewusstsein?", new[] { "Weil andere sich auf eingehaltene Zusagen verlassen können müssen", "Weil Zusagen ohnehin bedeutungslos sind", "Weil Verlässlichkeit nichts mit Verantwortung zu tun hat" },
            "Weil andere sich auf eingehaltene Zusagen verlassen können müssen", "Verlässlichkeit ist Teil von Verantwortung, weil andere sich darauf verlassen können müssen, dass Zusagen eingehalten werden."),
        ("Was bedeutet es, für einen Fehler geradezustehen, statt ihn zu vertuschen?", new[] { "Verantwortung zu übernehmen und aus dem Fehler zu lernen", "Den Fehler jemand anderem zuzuschieben", "Den Fehler zu verheimlichen" },
            "Verantwortung zu übernehmen und aus dem Fehler zu lernen", "Für einen Fehler geradezustehen bedeutet, ihn einzugestehen, Verantwortung zu übernehmen und daraus zu lernen."),
        ("Warum tragen Erwachsene mehr rechtliche Verantwortung als Kinder?", new[] { "Weil sie als voll geschäftsfähig gelten und die Tragweite von Handlungen besser einschätzen können", "Weil Kinder mehr Verantwortung tragen als Erwachsene", "Weil es keinen Unterschied gibt" },
            "Weil sie als voll geschäftsfähig gelten und die Tragweite von Handlungen besser einschätzen können", "Erwachsene gelten als voll geschäftsfähig und werden rechtlich stärker für ihr Handeln zur Verantwortung gezogen als Kinder."),
        ("Was bedeutet ökologische Verantwortung im Alltag?", new[] { "Bewusst mit Ressourcen wie Wasser, Strom und Müll umzugehen", "Ressourcen unbegrenzt zu verschwenden", "Umweltfragen komplett zu ignorieren" },
            "Bewusst mit Ressourcen wie Wasser, Strom und Müll umzugehen", "Ökologische Verantwortung zeigt sich im Alltag z.B. im bewussten Umgang mit Wasser, Strom und Müll."),
        ("Warum ist es verantwortungsvoll, um Hilfe zu bitten, wenn man selbst nicht mehr weiterweiß?", new[] { "Weil man so Probleme lösen kann, statt sie zu verschlimmern", "Weil man niemals Hilfe suchen sollte", "Weil Hilfe suchen ein Zeichen von Schwäche ist, das man vermeiden muss" },
            "Weil man so Probleme lösen kann, statt sie zu verschlimmern", "Rechtzeitig um Hilfe zu bitten, ist verantwortungsvoll, weil es hilft, Probleme zu lösen, statt sie zu verschlimmern."),
        ("Was bedeutet \"Vorbildfunktion\" z.B. von älteren Geschwistern oder Klassensprecherinnen?", new[] { "Das eigene Verhalten kann als Orientierung für andere dienen", "Das eigene Verhalten hat niemals Einfluss auf andere", "Vorbilder tragen keine besondere Verantwortung" },
            "Das eigene Verhalten kann als Orientierung für andere dienen", "Wer eine Vorbildfunktion hat, sollte sich bewusst sein, dass das eigene Verhalten anderen als Orientierung dient."),
        ("Was ist ein Beispiel für verantwortungsvollen Umgang mit dem eigenen Taschengeld?", new[] { "Einen Teil zu sparen und Ausgaben zu überlegen", "Alles sofort und unüberlegt auszugeben", "Taschengeld hat nichts mit Verantwortung zu tun" },
            "Einen Teil zu sparen und Ausgaben zu überlegen", "Verantwortungsvoller Umgang mit Taschengeld zeigt sich darin, einen Teil zu sparen und Ausgaben bewusst zu planen."),
        ("Warum ist Verantwortung ein zentrales Thema beim Führerschein oder anderen Berechtigungen?", new[] { "Weil man mit der Berechtigung auch Pflichten und mögliche Konsequenzen für andere übernimmt", "Weil Berechtigungen keinerlei Verantwortung mit sich bringen", "Weil nur der Staat dafür verantwortlich ist" },
            "Weil man mit der Berechtigung auch Pflichten und mögliche Konsequenzen für andere übernimmt", "Mit besonderen Berechtigungen wie dem Führerschein übernimmt man auch Pflichten und Verantwortung für mögliche Folgen für andere.")
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
            "Staatliche Unterdrückung von Meinungen - widerspricht der freien Meinungsäußerung", "Zensur unterdrückt unliebsame Meinungen und widerspricht damit dem demokratischen Grundrecht auf freie Meinungsäußerung."),
        ("Was ist der Unterschied zwischen Kritik und Beleidigung?", new[] { "Kritik bezieht sich sachlich auf eine Handlung, Beleidigung greift die Person herabwürdigend an", "Beides ist rechtlich und ethisch identisch", "Kritik ist immer verboten" },
            "Kritik bezieht sich sachlich auf eine Handlung, Beleidigung greift die Person herabwürdigend an", "Sachliche Kritik bezieht sich auf eine Handlung oder Aussage, während eine Beleidigung die Person selbst herabwürdigt."),
        ("Warum ist Pressefreiheit eng mit Meinungsfreiheit verbunden?", new[] { "Weil Medien Informationen und Meinungen frei verbreiten können müssen", "Weil Medien keine Meinungen äußern dürfen", "Weil Pressefreiheit nichts mit Meinungsfreiheit zu tun hat" },
            "Weil Medien Informationen und Meinungen frei verbreiten können müssen", "Pressefreiheit ermöglicht es Medien, frei über Themen zu berichten und Meinungen zu verbreiten - eine wichtige Ergänzung zur Meinungsfreiheit."),
        ("Was bedeutet \"Hate Speech\" (Hassrede)?", new[] { "Menschenverachtende, oft diskriminierende Äußerungen gegen bestimmte Gruppen", "Jede Form von Kritik", "Ein anderes Wort für Meinungsfreiheit" },
            "Menschenverachtende, oft diskriminierende Äußerungen gegen bestimmte Gruppen", "Hate Speech bezeichnet menschenverachtende, oft diskriminierende Äußerungen gegen bestimmte Personen oder Gruppen."),
        ("Warum ist Volksverhetzung in Deutschland strafbar, obwohl es Meinungsfreiheit gibt?", new[] { "Weil sie zu Hass und Gewalt gegen Bevölkerungsgruppen aufstachelt und Menschenwürde verletzt", "Weil jede kritische Aussage automatisch strafbar ist", "Weil es in Deutschland keine Meinungsfreiheit gibt" },
            "Weil sie zu Hass und Gewalt gegen Bevölkerungsgruppen aufstachelt und Menschenwürde verletzt", "Volksverhetzung überschreitet die Grenzen der Meinungsfreiheit, weil sie zu Hass und Gewalt aufstachelt und die Menschenwürde verletzt."),
        ("Was bedeutet \"Diskursfähigkeit\" in einer Demokratie?", new[] { "Unterschiedliche Meinungen respektvoll austauschen und diskutieren können", "Andere Meinungen grundsätzlich ablehnen", "Nur eine einzige Meinung zulassen" },
            "Unterschiedliche Meinungen respektvoll austauschen und diskutieren können", "Diskursfähigkeit bedeutet, unterschiedliche Standpunkte respektvoll austauschen und gemeinsam diskutieren zu können."),
        ("Warum ist es wichtig, auch unbequeme Meinungen anzuhören?", new[] { "Um verschiedene Perspektiven zu verstehen und die eigene Meinung zu überprüfen", "Weil man unbequeme Meinungen sofort übernehmen muss", "Weil andere Meinungen grundsätzlich falsch sind" },
            "Um verschiedene Perspektiven zu verstehen und die eigene Meinung zu überprüfen", "Auch unbequeme Meinungen anzuhören hilft, verschiedene Perspektiven zu verstehen und die eigene Position kritisch zu überprüfen."),
        ("Was ist der Unterschied zwischen Satire und einer Falschbehauptung?", new[] { "Satire übertreibt bewusst erkennbar, eine Falschbehauptung gibt sich als Wahrheit aus", "Beides ist identisch", "Satire ist immer verboten" },
            "Satire übertreibt bewusst erkennbar, eine Falschbehauptung gibt sich als Wahrheit aus", "Satire übertreibt bewusst erkennbar zur Kritik oder Unterhaltung, während eine Falschbehauptung sich fälschlich als Tatsache ausgibt."),
        ("Warum genießt Kunst (z.B. Karikaturen) in Deutschland besonderen Schutz?", new[] { "Weil auch die Kunstfreiheit im Grundgesetz verankert ist", "Weil Kunst keinerlei Regeln unterliegt", "Weil Kunst grundsätzlich verboten ist" },
            "Weil auch die Kunstfreiheit im Grundgesetz verankert ist", "Artikel 5 des Grundgesetzes schützt neben der Meinungsfreiheit auch ausdrücklich die Freiheit von Kunst und Wissenschaft."),
        ("Was bedeutet \"Cancel Culture\" in der öffentlichen Diskussion (vereinfacht)?", new[] { "Personen wegen umstrittener Äußerungen öffentlich boykottieren oder ausschließen", "Ein Gesetz zum Schutz der Meinungsfreiheit", "Ein anderes Wort für Zensur durch den Staat" }, "Personen wegen umstrittener Äußerungen öffentlich boykottieren oder ausschließen",
            "Cancel Culture beschreibt, wenn Personen wegen umstrittener Äußerungen öffentlich stark kritisiert oder ausgeschlossen werden."),
        ("Warum sollte man in einer Debatte Argumente statt persönlicher Angriffe nutzen?", new[] { "Weil Argumente die Diskussion sachlich voranbringen, persönliche Angriffe sie eskalieren lassen", "Weil persönliche Angriffe immer überzeugender sind", "Weil Argumente in Debatten keine Rolle spielen" },
            "Weil Argumente die Diskussion sachlich voranbringen, persönliche Angriffe sie eskalieren lassen", "Sachliche Argumente bringen eine Debatte inhaltlich voran, während persönliche Angriffe sie oft nur eskalieren lassen."),
        ("Was bedeutet Meinungspluralismus in einer Gesellschaft?", new[] { "Es existieren viele unterschiedliche Meinungen nebeneinander", "Alle müssen dieselbe Meinung haben", "Nur eine einzige Meinung ist erlaubt" }, "Es existieren viele unterschiedliche Meinungen nebeneinander",
            "Meinungspluralismus bedeutet, dass in einer Gesellschaft viele unterschiedliche Meinungen nebeneinander bestehen dürfen."),
        ("Warum kann Meinungsfreiheit im Internet besonders anspruchsvoll zu handhaben sein?", new[] { "Weil Inhalte sich extrem schnell und weit verbreiten können, auch Falschinformationen", "Weil im Internet ohnehin niemand seine Meinung äußert", "Weil das Internet keinerlei Regeln kennt" },
            "Weil Inhalte sich extrem schnell und weit verbreiten können, auch Falschinformationen", "Im Internet verbreiten sich Inhalte extrem schnell und weit - das gilt leider auch für Falschinformationen."),
        ("Was versteht man unter dem Begriff \"Whistleblower\"?", new[] { "Eine Person, die Missstände oder Rechtsverstöße öffentlich aufdeckt", "Ein Politiker, der niemals Kritik übt", "Ein Journalist, der nie recherchiert" }, "Eine Person, die Missstände oder Rechtsverstöße öffentlich aufdeckt",
            "Whistleblower decken oft unter persönlichem Risiko Missstände oder Rechtsverstöße auf, die sonst verborgen blieben."),
        ("Was bedeutet der Grundsatz \"Ich kann deine Meinung ablehnen, aber dein Recht verteidigen, sie zu äußern\"?", new[] { "Man kann inhaltlich widersprechen und trotzdem das Recht auf freie Meinungsäußerung respektieren", "Man darf nur der eigenen Meinung ein Recht auf Äußerung zugestehen", "Jede andere Meinung muss verboten werden" },
            "Man kann inhaltlich widersprechen und trotzdem das Recht auf freie Meinungsäußerung respektieren", "Dieser Grundsatz zeigt, dass man einer Meinung inhaltlich widersprechen und trotzdem das Recht, sie zu äußern, respektieren kann."),
        ("Was ist ein sinnvoller erster Schritt, bevor man eine steile Behauptung online teilt?", new[] { "Prüfen, ob es dazu verlässliche, unabhängige Quellen gibt", "Sofort teilen, damit man der Erste ist", "Die Behauptung ungeprüft für wahr halten" }, "Prüfen, ob es dazu verlässliche, unabhängige Quellen gibt",
            "Vor dem Teilen steiler Behauptungen sollte man prüfen, ob es dazu verlässliche, unabhängige Quellen gibt, um keine Falschinformationen zu verbreiten.")
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
            "Weil auch hinter anonymen Nutzernamen echte Menschen mit Gefühlen stehen", "Anonymität im Netz ändert nichts daran, dass am anderen Ende ein echter Mensch verletzt werden kann - Respekt gilt auch online."),
        ("Was ist ein starkes, sicheres Passwort?", new[] { "Eine lange Kombination aus Buchstaben, Zahlen und Sonderzeichen, die schwer zu erraten ist", "Der eigene Name oder Geburtstag", "Immer dasselbe einfache Passwort für alles" }, "Eine lange Kombination aus Buchstaben, Zahlen und Sonderzeichen, die schwer zu erraten ist",
            "Ein sicheres Passwort ist lang, kombiniert Buchstaben, Zahlen und Sonderzeichen und ist schwer zu erraten."),
        ("Warum sollte man nicht mit Fremden aus dem Internet persönliche Treffen ohne Wissen der Eltern vereinbaren?", new[] { "Weil man online nie sicher wissen kann, wer wirklich dahintersteckt", "Weil das Internet immer sicher ist", "Weil Fremde im Internet automatisch vertrauenswürdig sind" }, "Weil man online nie sicher wissen kann, wer wirklich dahintersteckt",
            "Im Internet lässt sich nie sicher überprüfen, wer wirklich hinter einem Profil steckt - deshalb sollten Treffen nie ohne Wissen der Eltern vereinbart werden."),
        ("Was bedeutet \"Datenschutz\"?", new[] { "Persönliche Daten vor unbefugtem Zugriff und Missbrauch schützen", "Alle Daten öffentlich zugänglich machen", "Daten haben keinen Wert und müssen nicht geschützt werden" }, "Persönliche Daten vor unbefugtem Zugriff und Missbrauch schützen",
            "Datenschutz soll persönliche Daten vor unbefugtem Zugriff, Weitergabe oder Missbrauch schützen."),
        ("Warum sollte man bei Online-Spielen und Apps genau prüfen, welche Berechtigungen man erteilt?", new[] { "Weil manche Apps unnötig viele persönliche Daten sammeln wollen", "Weil Berechtigungen keinerlei Bedeutung haben", "Weil man immer alle Berechtigungen ungeprüft erlauben sollte" }, "Weil manche Apps unnötig viele persönliche Daten sammeln wollen",
            "Manche Apps fordern mehr Berechtigungen an, als sie eigentlich für ihre Funktion brauchen - deshalb lohnt sich ein kritischer Blick."),
        ("Was ist ethisch bedenklich an übermäßigem Teilen (\"Sharing\") privater Momente anderer ohne Erlaubnis?", new[] { "Es verletzt die Privatsphäre und Selbstbestimmung der abgebildeten Person", "Es ist niemals problematisch", "Es betrifft nur die eigene Privatsphäre" }, "Es verletzt die Privatsphäre und Selbstbestimmung der abgebildeten Person",
            "Das Teilen privater Momente anderer ohne Erlaubnis verletzt deren Recht am eigenen Bild und ihre Privatsphäre."),
        ("Was versteht man unter digitaler Nachhaltigkeit?", new[] { "Geräte und Online-Dienste bewusst und ressourcenschonend nutzen", "Möglichst viele Geräte gleichzeitig verschwenderisch nutzen", "Digitale Technik hat keinerlei Umweltwirkung" }, "Geräte und Online-Dienste bewusst und ressourcenschonend nutzen",
            "Digitale Nachhaltigkeit bedeutet, Geräte und Online-Dienste bewusst und möglichst ressourcenschonend zu nutzen."),
        ("Warum ist übermäßiger Bildschirmkonsum bei Kindern und Jugendlichen ein ethisches Diskussionsthema?", new[] { "Weil er Schlaf, Bewegung und soziale Kontakte beeinträchtigen kann", "Weil Bildschirmzeit niemals Auswirkungen hat", "Weil mehr Bildschirmzeit automatisch gesünder ist" }, "Weil er Schlaf, Bewegung und soziale Kontakte beeinträchtigen kann",
            "Zu viel Bildschirmzeit kann Schlaf, Bewegung und direkte soziale Kontakte von Kindern und Jugendlichen beeinträchtigen."),
        ("Was ist ein \"Deepfake\"?", new[] { "Eine mit künstlicher Intelligenz täuschend echt gefälschte Foto-, Audio- oder Videodatei", "Ein besonders sicheres Passwort", "Ein echtes, unbearbeitetes Video" }, "Eine mit künstlicher Intelligenz täuschend echt gefälschte Foto-, Audio- oder Videodatei",
            "Ein Deepfake wird mit künstlicher Intelligenz erstellt und wirkt täuschend echt, obwohl der Inhalt gefälscht ist."),
        ("Warum können Deepfakes ethisch problematisch sein?", new[] { "Sie können Menschen Dinge sagen oder tun lassen, die nie wirklich passiert sind, und so täuschen", "Sie sind immer klar als Fälschung erkennbar", "Sie haben keinerlei Auswirkung auf das Vertrauen in Medien" }, "Sie können Menschen Dinge sagen oder tun lassen, die nie wirklich passiert sind, und so täuschen",
            "Deepfakes können Personen Aussagen oder Handlungen unterstellen, die nie stattgefunden haben, und so gezielt täuschen."),
        ("Was bedeutet \"digitale Selbstbestimmung\"?", new[] { "Selbst entscheiden können, welche eigenen Daten wie genutzt werden", "Andere entscheiden immer über die eigenen Daten", "Digitale Daten gehören automatisch den Unternehmen" }, "Selbst entscheiden können, welche eigenen Daten wie genutzt werden",
            "Digitale Selbstbestimmung bedeutet, selbst kontrollieren zu können, welche eigenen Daten wie und von wem genutzt werden."),
        ("Warum ist es wichtig, KI-generierte Inhalte (Texte, Bilder) kritisch zu hinterfragen?", new[] { "Weil sie fehlerhaft, veraltet oder erfunden sein können", "Weil KI-Inhalte immer fehlerfrei und wahr sind", "Weil KI-Inhalte niemals hinterfragt werden dürfen" }, "Weil sie fehlerhaft, veraltet oder erfunden sein können",
            "KI-generierte Inhalte können fehlerhaft, veraltet oder schlicht erfunden sein - deshalb sollte man sie stets kritisch prüfen."),
        ("Was ist \"Sexting\" und warum ist es besonders unter Minderjährigen riskant?", new[] { "Das Versenden intimer Bilder, die sich unkontrolliert weiterverbreiten und missbraucht werden können", "Ein harmloses Spiel ohne jedes Risiko", "Eine staatlich geförderte Aktivität" }, "Das Versenden intimer Bilder, die sich unkontrolliert weiterverbreiten und missbraucht werden können",
            "Beim Sexting versendete intime Bilder können sich unkontrolliert weiterverbreiten und später missbraucht werden - das macht es besonders riskant."),
        ("Was sollte man tun, wenn man im Internet auf illegale oder verstörende Inhalte stößt?", new[] { "Die Seite verlassen und eine Vertrauensperson informieren, ggf. melden", "Die Inhalte an möglichst viele Freunde weiterleiten", "Nichts unternehmen, weil es niemanden interessiert" }, "Die Seite verlassen und eine Vertrauensperson informieren, ggf. melden",
            "Bei illegalen oder verstörenden Inhalten sollte man die Seite verlassen, eine Vertrauensperson informieren und den Inhalt ggf. melden."),
        ("Warum ist digitale Zivilcourage wichtig, wenn man Mobbing in einer Gruppe/einem Chat beobachtet?", new[] { "Weil aktives Eingreifen oder Melden Betroffenen hilft und Mobbing eindämmen kann", "Weil Zuschauen ohne Handeln immer die beste Lösung ist", "Weil digitale Zivilcourage nichts bewirkt" }, "Weil aktives Eingreifen oder Melden Betroffenen hilft und Mobbing eindämmen kann",
            "Aktives Eingreifen oder Melden von Cybermobbing hilft Betroffenen und kann weiteres Mobbing eindämmen."),
        ("Was ist ein verantwortungsvoller Umgang mit Künstlicher Intelligenz als Lernhilfe?", new[] { "Sie zum Verstehen nutzen, statt Aufgaben unreflektiert kopieren zu lassen", "Alle Aufgaben blind von der KI lösen lassen, ohne selbst zu denken", "KI komplett meiden, egal wofür" }, "Sie zum Verstehen nutzen, statt Aufgaben unreflektiert kopieren zu lassen",
            "KI sollte beim Verstehen von Themen helfen, statt Aufgaben blind und unreflektiert zu übernehmen.")
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
            "Jeder gilt bis zum Beweis der Schuld als unschuldig", "Die Unschuldsvermutung schützt Angeklagte davor, vorschnell verurteilt zu werden, bevor ein Gericht die Schuld zweifelsfrei festgestellt hat."),
        ("Was bedeutet \"Verteilungsgerechtigkeit\"?", new[] { "Güter und Chancen nach fairen Kriterien in der Gesellschaft verteilen", "Alles nur an eine einzige Person verteilen", "Gerechtigkeit hat nichts mit Verteilung zu tun" }, "Güter und Chancen nach fairen Kriterien in der Gesellschaft verteilen",
            "Verteilungsgerechtigkeit fragt danach, wie Güter, Einkommen und Chancen nach fairen Kriterien in einer Gesellschaft verteilt werden sollten."),
        ("Was bedeutet \"Verfahrensgerechtigkeit\"?", new[] { "Ein faires, nachvollziehbares Verfahren zur Entscheidungsfindung, unabhängig vom Ergebnis", "Nur das Ergebnis zählt, der Weg dorthin ist egal", "Verfahren spielen für Gerechtigkeit keine Rolle" }, "Ein faires, nachvollziehbares Verfahren zur Entscheidungsfindung, unabhängig vom Ergebnis",
            "Verfahrensgerechtigkeit bedeutet, dass der Weg zu einer Entscheidung fair und nachvollziehbar ist, unabhängig vom konkreten Ergebnis."),
        ("Was ist der Sinn der Gewaltenteilung (Legislative, Exekutive, Judikative) für Gerechtigkeit im Staat?", new[] { "Sie verhindert, dass eine einzelne Institution zu viel Macht bekommt", "Sie sorgt dafür, dass nur eine Institution alles entscheidet", "Sie hat nichts mit Gerechtigkeit zu tun" }, "Sie verhindert, dass eine einzelne Institution zu viel Macht bekommt",
            "Die Gewaltenteilung verteilt Macht auf mehrere unabhängige Institutionen und verhindert so Machtmissbrauch."),
        ("Was bedeutet der Grundsatz \"Gleichheit vor dem Gesetz\"?", new[] { "Gesetze gelten für alle Menschen gleichermaßen, unabhängig von Herkunft oder Status", "Gesetze gelten nur für bestimmte Gruppen", "Reiche Menschen stehen über dem Gesetz" }, "Gesetze gelten für alle Menschen gleichermaßen, unabhängig von Herkunft oder Status",
            "Gleichheit vor dem Gesetz bedeutet, dass dieselben Regeln für alle Menschen gelten, unabhängig von Herkunft, Status oder Vermögen."),
        ("Warum ist ein unabhängiges Gericht wichtig für Gerechtigkeit?", new[] { "Weil es frei von politischem Druck über Recht und Unrecht entscheiden kann", "Weil Gerichte immer von der Regierung kontrolliert werden sollten", "Weil Unabhängigkeit für Gerichte unwichtig ist" }, "Weil es frei von politischem Druck über Recht und Unrecht entscheiden kann",
            "Unabhängige Gerichte können frei von politischem Einfluss über Recht und Unrecht entscheiden, was faire Urteile ermöglicht."),
        ("Was versteht man unter \"Restorative Justice\" (Wiedergutmachungs-orientierter Gerechtigkeit)?", new[] { "Der Fokus liegt auf Wiedergutmachung und Versöhnung zwischen Täter und Opfer", "Der Fokus liegt ausschließlich auf möglichst harter Bestrafung", "Täter und Opfer haben dabei keinerlei Rolle" }, "Der Fokus liegt auf Wiedergutmachung und Versöhnung zwischen Täter und Opfer",
            "Restorative Justice stellt Wiedergutmachung und Versöhnung zwischen Täter und Opfer in den Mittelpunkt, statt nur auf Strafe zu setzen."),
        ("Warum gilt Kinderarmut als Gerechtigkeitsproblem?", new[] { "Weil Kinder sich ihre Startbedingungen nicht aussuchen können, diese aber ihre Chancen prägen", "Weil Kinder für ihre Armut selbst verantwortlich sind", "Weil Armut keinerlei Einfluss auf Chancen hat" }, "Weil Kinder sich ihre Startbedingungen nicht aussuchen können, diese aber ihre Chancen prägen",
            "Kinder können sich ihre Familienverhältnisse nicht aussuchen, doch diese Startbedingungen beeinflussen stark ihre späteren Chancen - das macht Kinderarmut zu einem Gerechtigkeitsproblem."),
        ("Was bedeutet \"positive Diskriminierung\" bzw. Förderung benachteiligter Gruppen?", new[] { "Gezielte Unterstützung, um bestehende Nachteile auszugleichen", "Eine Form der Benachteiligung ohne jeden Grund", "Das Gegenteil von Chancengleichheit" }, "Gezielte Unterstützung, um bestehende Nachteile auszugleichen",
            "Positive Diskriminierung meint gezielte Fördermaßnahmen, um bestehende Nachteile bestimmter Gruppen auszugleichen."),
        ("Was bedeutet der Rechtsgrundsatz \"audiatur et altera pars\" (die andere Seite soll gehört werden)?", new[] { "Beide Seiten eines Streits sollen vor einer Entscheidung gehört werden", "Nur eine Seite darf sich äußern", "Der Grundsatz betrifft nur Sportregeln" }, "Beide Seiten eines Streits sollen vor einer Entscheidung gehört werden",
            "Dieser Grundsatz verlangt, dass vor einer Entscheidung beide Seiten eines Streits gehört werden."),
        ("Warum ist der Zugang zu einem fairen Gerichtsverfahren ein wichtiges Gerechtigkeitsprinzip?", new[] { "Weil jede Person das Recht hat, sich gegen Vorwürfe zu verteidigen", "Weil nur wohlhabende Menschen ein Recht auf ein Verfahren haben sollten", "Weil Gerichtsverfahren für Gerechtigkeit unwichtig sind" }, "Weil jede Person das Recht hat, sich gegen Vorwürfe zu verteidigen",
            "Ein faires Gerichtsverfahren sichert jeder Person das Recht, sich gegen Vorwürfe angemessen zu verteidigen."),
        ("Was bedeutet \"Generationengerechtigkeit\"?", new[] { "Heutige Entscheidungen sollen auch künftige Generationen fair behandeln", "Nur die aktuelle Generation zählt", "Zukünftige Generationen haben keine Rechte" }, "Heutige Entscheidungen sollen auch künftige Generationen fair behandeln",
            "Generationengerechtigkeit bedeutet, heutige Entscheidungen so zu treffen, dass sie auch künftige Generationen fair behandeln."),
        ("Warum wird Steuerpolitik oft unter dem Aspekt der Gerechtigkeit diskutiert?", new[] { "Weil es um faire Verteilung von Lasten je nach wirtschaftlicher Leistungsfähigkeit geht", "Weil Steuern nichts mit Gerechtigkeit zu tun haben", "Weil alle Menschen unabhängig vom Einkommen exakt gleich viel zahlen müssen" }, "Weil es um faire Verteilung von Lasten je nach wirtschaftlicher Leistungsfähigkeit geht",
            "Bei Steuerpolitik geht es oft um die Frage, wie die finanzielle Last fair je nach wirtschaftlicher Leistungsfähigkeit verteilt werden soll."),
        ("Was ist ein Beispiel für globale Gerechtigkeit?", new[] { "Fairer Handel zwischen reichen und ärmeren Ländern", "Nur reiche Länder sollen von Handel profitieren", "Globale Gerechtigkeit betrifft nur ein einzelnes Land" }, "Fairer Handel zwischen reichen und ärmeren Ländern",
            "Fairer Handel, der auch ärmeren Ländern angemessene Preise sichert, ist ein Beispiel für globale Gerechtigkeit."),
        ("Warum ist der Jugendstrafvollzug in Deutschland anders geregelt als für Erwachsene?", new[] { "Weil bei Jugendlichen Erziehung und Resozialisierung im Vordergrund stehen", "Weil für Jugendliche keine Regeln gelten", "Weil Jugendliche automatisch härter bestraft werden" }, "Weil bei Jugendlichen Erziehung und Resozialisierung im Vordergrund stehen",
            "Im Jugendstrafrecht stehen Erziehung und Resozialisierung stärker im Vordergrund als bei Erwachsenen."),
        ("Was bedeutet der Grundsatz \"im Zweifel für den Angeklagten\" (in dubio pro reo)?", new[] { "Bei nicht eindeutig bewiesener Schuld darf niemand verurteilt werden", "Bei Zweifeln wird automatisch die härteste Strafe verhängt", "Der Grundsatz gilt nur für Zeugen" }, "Bei nicht eindeutig bewiesener Schuld darf niemand verurteilt werden",
            "Dieser Grundsatz besagt, dass bei verbleibenden Zweifeln an der Schuld zugunsten der angeklagten Person entschieden werden muss.")
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
