using LernTor.Core.Enums;

namespace LernTor.Core.Services;

/// <summary>
/// Alles, woran die Abzeichen gemessen werden - ein Schnappschuss aus Daten, die es ohnehin gibt
/// (Aktivitätsprotokoll, Quizversuche, Tipptrainer, Verkehrszeichen, Theorie, Meisterschaft).
/// Die App füllt ihn, <see cref="AchievementCatalog"/> wertet ihn aus; so bleibt jede Bedingung
/// ohne Datenbank und ohne WPF testbar.
/// </summary>
public sealed record AchievementFacts
{
    public int CorrectAnswers { get; init; }

    public IReadOnlyDictionary<Subject, int> CorrectBySubject { get; init; } = new Dictionary<Subject, int>();

    /// <summary>Verschiedene Tage mit mindestens einer Antwort - GESAMT, keine Serie. Ein
    /// verpasster Tag nimmt nichts weg.</summary>
    public int LearningDays { get; init; }

    /// <summary>Richtige Antworten auf Fragen aus der Fehler-Kartei („🔁“): Fehler, die das Kind
    /// beim zweiten Anlauf richtig hatte.</summary>
    public int CorrectedMistakes { get; init; }

    /// <summary>Themen auf der Stufe „sicher“ oder „gemeistert“.</summary>
    public int SecureTopics { get; init; }

    public int MasteredTopics { get; init; }

    public int PassedFinalQuizzes { get; init; }

    /// <summary>Abschlussquizze mit 100 %.</summary>
    public int PerfectFinalQuizzes { get; init; }

    public int TypingLessonsCompleted { get; init; }

    public int TrafficSignsMastered { get; init; }

    /// <summary>Zahl der Zeichen, die das Kind überhaupt üben kann (Katalog ohne abgewählte
    /// Gruppen). 0 = Führerschein-Bereich nicht verfügbar.</summary>
    public int TrafficSignsAvailable { get; init; }

    public int TheoryExamsPassed { get; init; }

    public int CorrectIn(Subject subject) => CorrectBySubject.GetValueOrDefault(subject);
}

/// <summary>Ein Abzeichen.</summary>
/// <param name="Id">Stabile Kennung - wird gespeichert, darf sich nie ändern.</param>
/// <param name="Area">Gehört das Abzeichen zu einem abschaltbaren Bereich (Führerschein, Erste
/// Hilfe, KI, Tippen …), wird es ausgeblendet, solange der Bereich für das Kind aus ist - ein
/// Abzeichen für etwas, das man nicht üben darf, wäre ein leeres Versprechen.</param>
public sealed record Achievement(
    string Id,
    string Emoji,
    string TitleDe,
    string TitleTr,
    string HowToDe,
    string HowToTr,
    Subject? Area,
    Func<AchievementFacts, bool> IsEarned);

/// <summary>
/// Der feste Katalog der Abzeichen - nach dem Vorbild von ANTON und Duolingo, aber nach den
/// Grundsätzen dieser App:
///
/// <list type="bullet">
/// <item><b>Leistung statt Anwesenheit.</b> Kein „30 Tage am Stück“. Lerntage zählen gesamt,
/// ein verpasster Tag nimmt nichts weg.</item>
/// <item><b>Nichts verfällt.</b> Ein einmal freigeschaltetes Abzeichen wird gespeichert und nie
/// entzogen, auch wenn eine Bedingung später nicht mehr erfüllt ist (etwa weil ein Thema von
/// „gemeistert“ zurückfällt).</item>
/// <item><b>Kein Vergleich.</b> Abzeichen sind nur für das Kind selbst sichtbar.</item>
/// </list>
///
/// <para>Die Kennungen sind gespeichert (<c>UnlockedAchievements</c>) und dürfen sich nicht
/// ändern; neue Abzeichen werden angehängt. Texte stehen hier zweisprachig, damit Katalog und
/// Übersetzung nicht auseinanderlaufen können.</para>
/// </summary>
public static class AchievementCatalog
{
    public static IReadOnlyList<Achievement> All { get; } = new List<Achievement>
    {
        // --- Richtige Antworten insgesamt ---
        new("richtig-10", "👣", "Erste Schritte", "İlk adımlar",
            "10 Aufgaben richtig lösen", "10 görevi doğru çöz",
            null, f => f.CorrectAnswers >= 10),
        new("richtig-100", "💯", "Hundert richtig", "Yüz doğru",
            "100 Aufgaben richtig lösen", "100 görevi doğru çöz",
            null, f => f.CorrectAnswers >= 100),
        new("richtig-500", "🚀", "Fünfhundert richtig", "Beş yüz doğru",
            "500 Aufgaben richtig lösen", "500 görevi doğru çöz",
            null, f => f.CorrectAnswers >= 500),
        new("richtig-1000", "🌟", "Tausend richtig", "Bin doğru",
            "1000 Aufgaben richtig lösen", "1000 görevi doğru çöz",
            null, f => f.CorrectAnswers >= 1000),

        // --- Lerntage (gesamt, keine Serie) ---
        new("lerntage-10", "📅", "Zehn Lerntage", "On öğrenme günü",
            "An 10 verschiedenen Tagen lernen", "10 farklı günde öğren",
            null, f => f.LearningDays >= 10),
        new("lerntage-50", "🗓️", "Fünfzig Lerntage", "Elli öğrenme günü",
            "An 50 verschiedenen Tagen lernen", "50 farklı günde öğren",
            null, f => f.LearningDays >= 50),
        new("lerntage-100", "🏅", "Hundert Lerntage", "Yüz öğrenme günü",
            "An 100 verschiedenen Tagen lernen", "100 farklı günde öğren",
            null, f => f.LearningDays >= 100),

        // --- Fehler-Kartei ---
        new("fehler-10", "🔁", "Aus Fehlern gelernt", "Hatalardan öğrendin",
            "10 Fragen aus der Fehler-Kartei beim zweiten Mal richtig haben", "Hata kutusundaki 10 soruyu ikinci seferde doğru cevapla",
            null, f => f.CorrectedMistakes >= 10),
        new("fehler-50", "🧗", "Dranbleiber", "Pes etmeyen",
            "50 Fragen aus der Fehler-Kartei beim zweiten Mal richtig haben", "Hata kutusundaki 50 soruyu ikinci seferde doğru cevapla",
            null, f => f.CorrectedMistakes >= 50),

        // --- Meisterschaft (TopicMasteryCalculator) ---
        new("sicher-1", "✅", "Erstes sicheres Thema", "İlk emin olduğun konu",
            "Ein Thema auf „sicher“ bringen", "Bir konuyu „emin“ seviyesine getir",
            null, f => f.SecureTopics >= 1),
        new("sicher-10", "📚", "Zehn sichere Themen", "On emin konu",
            "10 Themen auf „sicher“ oder besser bringen", "10 konuyu „emin“ veya daha iyi seviyeye getir",
            null, f => f.SecureTopics >= 10),
        new("meister-1", "🏆", "Erstes gemeistertes Thema", "İlk ustalaştığın konu",
            "Ein Thema meistern", "Bir konuda ustalaş",
            null, f => f.MasteredTopics >= 1),
        new("meister-10", "👑", "Zehn gemeisterte Themen", "On konuda ustalık",
            "10 Themen meistern", "10 konuda ustalaş",
            null, f => f.MasteredTopics >= 10),

        // --- Fächer ---
        new("mathe-200", "🧮", "Mathe-Ass", "Matematik ası",
            "200 Mathe-Aufgaben richtig lösen", "200 matematik görevini doğru çöz",
            Subject.Mathematik, f => f.CorrectIn(Subject.Mathematik) >= 200),
        new("dreisprachig", "🗣️", "Dreisprachig", "Üç dilli",
            "Je 25 Aufgaben in Deutsch, Türkisch und Englisch richtig lösen", "Almanca, Türkçe ve İngilizcede 25'er görevi doğru çöz",
            null, f => f.CorrectIn(Subject.Deutsch) >= 25 && f.CorrectIn(Subject.Tuerkisch) >= 25 && f.CorrectIn(Subject.Englisch) >= 25),
        new("allrounder", "🌈", "Allrounder", "Her şeyden anlayan",
            "In 8 verschiedenen Fächern je 20 Aufgaben richtig lösen", "8 farklı derste 20'şer görevi doğru çöz",
            null, f => f.CorrectBySubject.Count(e => TopicMasteryCalculator.CountsForMastery(e.Key) && e.Value >= 20) >= 8),
        new("forscher", "🔬", "Forscher", "Araştırmacı",
            "In Biologie, Chemie und Physik je 20 Aufgaben richtig lösen", "Biyoloji, kimya ve fizikte 20'şer görevi doğru çöz",
            null, f => f.CorrectIn(Subject.Biologie) >= 20 && f.CorrectIn(Subject.Chemie) >= 20 && f.CorrectIn(Subject.Physik) >= 20),
        new("ki-kenner", "🤖", "KI-Kenner", "Yapay zeka uzmanı",
            "30 Aufgaben im KI-Bereich richtig lösen", "Yapay zeka bölümünde 30 görevi doğru çöz",
            Subject.KiWissen, f => f.CorrectIn(Subject.KiWissen) >= 30),
        new("ersthelfer", "🚑", "Ersthelfer", "İlk yardımcı",
            "30 Aufgaben in Erster Hilfe richtig lösen", "İlk yardımda 30 görevi doğru çöz",
            Subject.ErsteHilfe, f => f.CorrectIn(Subject.ErsteHilfe) >= 30),

        // --- Abschlussquiz ---
        new("quiz-10", "🎓", "Zehn Abschlussquiz bestanden", "On final sınavı geçildi",
            "10 Abschlussquiz bestehen", "10 final sınavını geç",
            null, f => f.PassedFinalQuizzes >= 10),
        new("quiz-perfekt", "✨", "Fehlerfrei", "Hatasız",
            "Ein Abschlussquiz mit 100 % bestehen", "Bir final sınavını %100 ile geç",
            null, f => f.PerfectFinalQuizzes >= 1),

        // --- Tippen ---
        new("tippen-5", "⌨️", "Tipp-Starter", "Klavye başlangıcı",
            "5 Lektionen im Tipptrainer schaffen", "Klavye eğitiminde 5 dersi tamamla",
            Subject.Tippen, f => f.TypingLessonsCompleted >= 5),
        new("tippen-20", "⚡", "Tastatur-Profi", "Klavye ustası",
            "20 Lektionen im Tipptrainer schaffen", "Klavye eğitiminde 20 dersi tamamla",
            Subject.Tippen, f => f.TypingLessonsCompleted >= 20),

        // --- Führerschein ---
        new("schilder-alle", "🚦", "Schilder-Kenner", "Trafik işareti uzmanı",
            "Alle Verkehrszeichen sicher können", "Tüm trafik işaretlerini iyice öğren",
            Subject.Fuehrerschein, f => f.TrafficSignsAvailable > 0 && f.TrafficSignsMastered >= f.TrafficSignsAvailable),
        new("theorie-bestanden", "🚗", "Theorie bestanden", "Teoriyi geçtin",
            "Eine Theorieprüfung (Simulation) bestehen", "Bir teori sınavı simülasyonunu geç",
            Subject.Fuehrerschein, f => f.TheoryExamsPassed >= 1),
    };

    /// <summary>Alle Abzeichen, deren Bedingung gerade erfüllt ist.</summary>
    public static IReadOnlyList<Achievement> Earned(AchievementFacts facts)
    {
        ArgumentNullException.ThrowIfNull(facts);
        return All.Where(abzeichen => abzeichen.IsEarned(facts)).ToList();
    }

    /// <summary>
    /// Was jetzt NEU dazukommt: erfüllt, aber noch nicht gespeichert. Genau diese Abzeichen
    /// zeigt der Geschafft-Bildschirm als „Neues Abzeichen!“ und genau diese werden gespeichert.
    /// </summary>
    public static IReadOnlyList<Achievement> NewlyEarned(AchievementFacts facts, IReadOnlySet<string> alreadyUnlocked)
    {
        ArgumentNullException.ThrowIfNull(alreadyUnlocked);
        return Earned(facts).Where(abzeichen => !alreadyUnlocked.Contains(abzeichen.Id)).ToList();
    }

    public static Achievement? Find(string id) => All.FirstOrDefault(abzeichen => abzeichen.Id == id);

    /// <summary>
    /// Füllt den Teil der Fakten, der aus den Antworten und der Meisterschaft folgt. Den Rest
    /// (Quiz, Tippen, Führerschein) ergänzt die App mit <c>with { … }</c>.
    /// </summary>
    public static AchievementFacts FromAnswers(
        IEnumerable<MasteryAnswer> answers, IReadOnlyList<TopicMasteryStatus> topics)
    {
        ArgumentNullException.ThrowIfNull(answers);
        ArgumentNullException.ThrowIfNull(topics);

        var liste = answers as IReadOnlyCollection<MasteryAnswer> ?? answers.ToList();
        var richtige = liste.Where(antwort => antwort.WasCorrect).ToList();

        return new AchievementFacts
        {
            CorrectAnswers = richtige.Count,
            CorrectBySubject = richtige
                .GroupBy(antwort => antwort.Subject)
                .ToDictionary(gruppe => gruppe.Key, gruppe => gruppe.Count()),
            LearningDays = liste
                .Select(antwort => DateOnly.FromDateTime(antwort.Timestamp.LocalDateTime))
                .Distinct()
                .Count(),
            CorrectedMistakes = richtige.Count(antwort =>
                (antwort.Topic ?? string.Empty).StartsWith(TopicMasteryCalculator.ReviewTopicPrefix, StringComparison.Ordinal)),
            SecureTopics = topics.Count(thema => thema.Level >= MasteryLevel.Sicher),
            MasteredTopics = topics.Count(thema => thema.Level == MasteryLevel.Gemeistert),
        };
    }
}
