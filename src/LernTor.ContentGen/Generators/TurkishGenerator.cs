using LernTor.Core.Enums;
using LernTor.Core.Models;

namespace LernTor.ContentGen.Generators;

/// <summary>
/// Türkisch-Aufgabengenerator für bilinguale/herkunftssprachliche Lerner: Zeitformen, Wortschatz,
/// Ekler (Suffixe) für Klasse 6, sowie Satzglieder, Fiilimsi und Rechtschreibung für Klasse 9.
/// </summary>
public sealed class TurkishGenerator : ExerciseGeneratorBase
{
    public override Subject Subject => Subject.Tuerkisch;

    protected override IReadOnlyDictionary<GradeLevel, IReadOnlyList<TopicFactory>> TopicsByGrade { get; } =
        new Dictionary<GradeLevel, IReadOnlyList<TopicFactory>>
        {
            [GradeLevel.Klasse6] = new List<TopicFactory>
            {
                SimdikiZaman,
                GecmisZaman,
                EsAnlamli,
                ZitAnlamli,
                DogaVeCevre,
                AileVeGunlukYasam,
                OkulVeToplum,
                TurkiyeKulturu
            },
            [GradeLevel.Klasse9] = new List<TopicFactory>
            {
                CumleOgeleri,
                GelecekZaman,
                YazimKurallari,
                FiilimsiTuru,
                KimlikVeGelecek,
                TarihVeGelenekler,
                TurkiyeCografyasi
            }
        };

    private static readonly (string Fiil, string Simdiki)[] SimdikiZamanBeispiele =
    {
        ("gitmek", "gidiyor"), ("okumak", "okuyor"), ("yazmak", "yazıyor"),
        ("oynamak", "oynuyor"), ("koşmak", "koşuyor"), ("gelmek", "geliyor"),
        ("içmek", "içiyor"), ("görmek", "görüyor"), ("bilmek", "biliyor"),
        ("sevmek", "seviyor"), ("gülmek", "gülüyor"), ("ağlamak", "ağlıyor"),
        ("uyumak", "uyuyor"), ("konuşmak", "konuşuyor"), ("düşünmek", "düşünüyor"),
        ("beklemek", "bekliyor"), ("çalışmak", "çalışıyor"), ("dinlemek", "dinliyor"),
        ("anlamak", "anlıyor"), ("yemek", "yiyor")
    };

    private static QuizQuestion SimdikiZaman(Random r)
    {
        var v = SimdikiZamanBeispiele[r.Next(SimdikiZamanBeispiele.Length)];

        return new QuizQuestion
        {
            Id = NewId(),
            Subject = Subject.Tuerkisch,
            GradeLevel = GradeLevel.Klasse6,
            Topic = "Şimdiki Zaman (Präsens)",
            Type = QuestionType.OpenText,
            Prompt = $"\"{v.Fiil}\" fiilinin (o/she/it için) şimdiki zaman hâlini yaz. (Beispiel: gelmek -> geliyor)",
            CorrectAnswers = new[] { v.Simdiki },
            Explanation = $"\"{v.Fiil}\" -> \"{v.Simdiki}\". Şimdiki zaman \"-yor\" eki ile kurulur.",
            HelpHint = "Şimdiki zaman (Präsens) her zaman \"-yor\" ekiyle kurulur, kelime köküne göre ünlü uyumu değişir (gid-iyor, oku-yor)."
        };
    }

    private static readonly (string Fiil, string Gecmis)[] GecmisZamanBeispiele =
    {
        ("gelmek", "geldi"), ("almak", "aldı"), ("görmek", "gördü"),
        ("okumak", "okudu"), ("yazmak", "yazdı"), ("gitmek", "gitti"),
        ("içmek", "içti"), ("bilmek", "bildi"), ("sevmek", "sevdi"),
        ("gülmek", "güldü"), ("ağlamak", "ağladı"), ("uyumak", "uyudu"),
        ("konuşmak", "konuştu"), ("düşünmek", "düşündü"), ("beklemek", "bekledi"),
        ("çalışmak", "çalıştı"), ("dinlemek", "dinledi"), ("anlamak", "anladı"),
        ("yemek", "yedi"), ("oynamak", "oynadı")
    };

    private static QuizQuestion GecmisZaman(Random r)
    {
        var v = GecmisZamanBeispiele[r.Next(GecmisZamanBeispiele.Length)];

        return new QuizQuestion
        {
            Id = NewId(),
            Subject = Subject.Tuerkisch,
            GradeLevel = GradeLevel.Klasse6,
            Topic = "Geçmiş Zaman (Präteritum/-di'li geçmiş)",
            Type = QuestionType.OpenText,
            Prompt = $"\"{v.Fiil}\" fiilinin (o/she/it için) -di'li geçmiş zaman hâlini yaz.",
            CorrectAnswers = new[] { v.Gecmis },
            Explanation = $"\"{v.Fiil}\" -> \"{v.Gecmis}\". -di'li geçmiş zaman, görülen/kesin geçmişi anlatır.",
            HelpHint = "-di'li geçmiş zaman eki (-di/-dı/-du/-dü ya da -ti/-tı/-tu/-tü) ünlü ve ünsüz uyumuna göre değişir."
        };
    }

    private static readonly (string Kelime, string EsAnlam, string[] Yanlislar)[] EsAnlamliListe =
    {
        ("mutlu", "sevinçli", new[] { "üzgün", "yorgun", "kızgın" }),
        ("büyük", "iri", new[] { "küçük", "az", "kısa" }),
        ("güzel", "hoş", new[] { "çirkin", "kötü", "sıkıcı" }),
        ("hızlı", "çabuk", new[] { "yavaş", "ağır", "durgun" }),
        ("akıllı", "zeki", new[] { "aptal", "tembel", "yorgun" }),
        ("üzgün", "kederli", new[] { "mutlu", "neşeli", "sakin" }),
        ("cesur", "yürekli", new[] { "korkak", "çekingen", "tembel" }),
        ("yorgun", "bitkin", new[] { "dinç", "zinde", "canlı" }),
        ("kolay", "basit", new[] { "zor", "karmaşık", "güç" }),
        ("zengin", "varlıklı", new[] { "fakir", "yoksul", "muhtaç" }),
        ("temiz", "pak", new[] { "kirli", "pis", "bulaşık" }),
        ("sessiz", "sakin", new[] { "gürültülü", "yüksek sesli", "hareketli" }),
        ("cömert", "eli açık", new[] { "cimri", "pinti", "hasis" }),
        ("korkak", "ürkek", new[] { "cesur", "yiğit", "atılgan" }),
        ("tembel", "uyuşuk", new[] { "çalışkan", "gayretli", "hareketli" }),
        ("güçlü", "kuvvetli", new[] { "zayıf", "güçsüz", "halsiz" }),
        ("neşeli", "şen", new[] { "üzgün", "kederli", "asık suratlı" }),
        ("eski", "köhne", new[] { "yeni", "modern", "taze" }),
        ("basit", "yalın", new[] { "karmaşık", "zor", "girift" }),
        ("ünlü", "meşhur", new[] { "tanınmamış", "bilinmeyen", "sıradan" })
    };

    private static QuizQuestion EsAnlamli(Random r)
    {
        var e = EsAnlamliListe[r.Next(EsAnlamliListe.Length)];
        var optionen = new[] { e.EsAnlam }.Concat(e.Yanlislar).OrderBy(_ => r.Next()).ToArray();

        return new QuizQuestion
        {
            Id = NewId(),
            Subject = Subject.Tuerkisch,
            GradeLevel = GradeLevel.Klasse6,
            Topic = "Eş Anlamlı Kelimeler (Synonyme)",
            Type = QuestionType.MultipleChoice,
            Prompt = $"\"{e.Kelime}\" kelimesinin eş anlamlısı hangisidir?",
            Options = optionen,
            CorrectAnswers = new[] { e.EsAnlam },
            Explanation = $"\"{e.Kelime}\" ile \"{e.EsAnlam}\" aynı ya da çok benzer anlama gelir (eş anlamlı kelimeler).",
            HelpHint = "Eş anlamlı (synonym) kelimeler aynı ya da çok benzer bir anlama gelir - cümle içinde birbirinin yerine kullanılabilirler."
        };
    }

    private static readonly (string Kelime, string ZitAnlam, string[] Yanlislar)[] ZitAnlamliListe =
    {
        ("sıcak", "soğuk", new[] { "ılık", "nemli", "kuru" }),
        ("uzun", "kısa", new[] { "geniş", "dar", "derin" }),
        ("kolay", "zor", new[] { "basit", "hızlı", "yavaş" }),
        ("erken", "geç", new[] { "yakın", "uzak", "yeni" }),
        ("kalın", "ince", new[] { "sert", "yumuşak", "ağır" }),
        ("büyük", "küçük", new[] { "orta", "iri", "kocaman" }),
        ("hızlı", "yavaş", new[] { "seri", "çevik", "atik" }),
        ("güzel", "çirkin", new[] { "hoş", "şık", "sevimli" }),
        ("temiz", "kirli", new[] { "pak", "düzenli", "bakımlı" }),
        ("açık", "kapalı", new[] { "aralık", "yarım", "geniş" }),
        ("yukarı", "aşağı", new[] { "yan", "ileri", "geri" }),
        ("ileri", "geri", new[] { "yukarı", "yan", "aşağı" }),
        ("dolu", "boş", new[] { "yarım", "az", "hafif" }),
        ("ağır", "hafif", new[] { "kalın", "büyük", "sert" }),
        ("zengin", "fakir", new[] { "varlıklı", "cömert", "tok" }),
        ("genç", "yaşlı", new[] { "küçük", "olgun", "deneyimli" }),
        ("gündüz", "gece", new[] { "sabah", "akşam", "öğle" }),
        ("iyi", "kötü", new[] { "güzel", "hoş", "mükemmel" }),
        ("mutlu", "mutsuz", new[] { "üzgün", "sinirli", "yorgun" }),
        ("sabah", "akşam", new[] { "öğle", "gece", "gündüz" })
    };

    private static QuizQuestion ZitAnlamli(Random r)
    {
        var z = ZitAnlamliListe[r.Next(ZitAnlamliListe.Length)];
        var optionen = new[] { z.ZitAnlam }.Concat(z.Yanlislar).OrderBy(_ => r.Next()).ToArray();

        return new QuizQuestion
        {
            Id = NewId(),
            Subject = Subject.Tuerkisch,
            GradeLevel = GradeLevel.Klasse6,
            Topic = "Zıt Anlamlı Kelimeler (Antonyme)",
            Type = QuestionType.MultipleChoice,
            Prompt = $"\"{z.Kelime}\" kelimesinin zıt (karşıt) anlamlısı hangisidir?",
            Options = optionen,
            CorrectAnswers = new[] { z.ZitAnlam },
            Explanation = $"\"{z.Kelime}\" kelimesinin karşıtı \"{z.ZitAnlam}\"dır.",
            HelpHint = "Zıt anlamlı (Antonym) kelimeler tam tersi bir anlam taşır - dikkat: sadece \"biraz farklı\" olan kelimeler zıt anlamlı sayılmaz."
        };
    }

    private static readonly (string TurkceKelime, string Almanca, string[] Yanlislar)[] DogaCevreListe =
    {
        ("orman", "Wald", new[] { "Berg", "Feld", "Wüste" }),
        ("nehir", "Fluss", new[] { "See", "Meer", "Brunnen" }),
        ("çevre kirliliği", "Umweltverschmutzung", new[] { "Umweltschutz", "Naturschutz", "Klimawandel" }),
        ("geri dönüşüm", "Recycling", new[] { "Müllabfuhr", "Umweltschutz", "Naturschutz" }),
        ("hayvan türü", "Tierart", new[] { "Pflanzenart", "Lebensraum", "Ökosystem" }),
        ("iklim değişikliği", "Klimawandel", new[] { "Umweltverschmutzung", "Naturschutz", "Wetterbericht" }),
        ("deniz", "Meer", new[] { "Teich", "Bach", "Brunnen" }),
        ("dağ", "Berg", new[] { "Tal", "Hügel", "Wüste" }),
        ("göl", "See", new[] { "Meer", "Fluss", "Teich" }),
        ("hava kirliliği", "Luftverschmutzung", new[] { "Wasserverschmutzung", "Umweltschutz", "Lärmbelastung" }),
        ("güneş enerjisi", "Sonnenenergie", new[] { "Windenergie", "Wasserkraft", "Kernenergie" }),
        ("yenilenebilir enerji", "Erneuerbare Energie", new[] { "Fossile Energie", "Atomenergie", "Kohleenergie" }),
        ("sera etkisi", "Treibhauseffekt", new[] { "Ozonloch", "Klimawandel", "Luftverschmutzung" }),
        ("çöl", "Wüste", new[] { "Steppe", "Savanne", "Tundra" }),
        ("yağmur ormanı", "Regenwald", new[] { "Nadelwald", "Laubwald", "Mischwald" }),
        ("doğal kaynak", "natürliche Ressource", new[] { "künstliche Ressource", "Rohstoffmangel", "Energiequelle" }),
        ("ekosistem", "Ökosystem", new[] { "Lebensraum", "Nahrungskette", "Biotop" }),
        ("biyoçeşitlilik", "Artenvielfalt", new[] { "Umweltschutz", "Naturschutz", "Tierschutz" }),
        ("nesli tükenmekte olan tür", "vom Aussterben bedrohte Art", new[] { "geschützte Art", "seltene Art", "wilde Art" }),
        ("su tasarrufu", "Wassersparen", new[] { "Wasserverschmutzung", "Wasserversorgung", "Wasserkraft" })
    };

    private static QuizQuestion DogaVeCevre(Random r)
    {
        var d = DogaCevreListe[r.Next(DogaCevreListe.Length)];
        var optionen = new[] { d.Almanca }.Concat(d.Yanlislar).OrderBy(_ => r.Next()).ToArray();

        return new QuizQuestion
        {
            Id = NewId(),
            Subject = Subject.Tuerkisch,
            GradeLevel = GradeLevel.Klasse6,
            Topic = "Doğa ve Çevre (Natur und Umwelt) – Wortschatz",
            Type = QuestionType.MultipleChoice,
            Prompt = $"\"{d.TurkceKelime}\" kelimesinin Almancası hangisidir?",
            Options = optionen,
            CorrectAnswers = new[] { d.Almanca },
            Explanation = $"\"{d.TurkceKelime}\" Almanca \"{d.Almanca}\" demektir.",
            HelpHint = "Doğa ve çevre kelimeleri günlük hayatta sık kullanılır - anlamını Almanca karşılığıyla eşleştirmeye çalış."
        };
    }

    private static readonly (string TurkceKelime, string Almanca, string[] Yanlislar)[] AileGunlukListe =
    {
        ("aile", "Familie", new[] { "Freund", "Nachbar", "Verwandter" }),
        ("kardeş", "Geschwister", new[] { "Eltern", "Großeltern", "Cousin/Cousine" }),
        ("arkadaş", "Freund/Freundin", new[] { "Fremder", "Lehrer", "Nachbar" }),
        ("buluşmak", "sich treffen", new[] { "sich streiten", "sich verstecken", "sich verabschieden" }),
        ("günlük rutin", "Tagesablauf", new[] { "Wochenende", "Ferienplan", "Stundenplan" }),
        ("ev işleri", "Hausarbeiten", new[] { "Hausaufgaben", "Haustiere", "Hausordnung" }),
        ("harçlık", "Taschengeld", new[] { "Gehalt", "Geschenk", "Ersparnis" }),
        ("yemek tarifi", "Rezept", new[] { "Speisekarte", "Einkaufsliste", "Kochbuch" }),
        ("alışveriş yapmak", "einkaufen", new[] { "kochen", "aufräumen", "putzen" }),
        ("oda", "Zimmer", new[] { "Haus", "Garten", "Wohnung" }),
        ("yol tarifi", "Wegbeschreibung", new[] { "Stadtplan", "Verkehrsschild", "Landkarte" }),
        ("ulaşım aracı", "Verkehrsmittel", new[] { "Fahrschein", "Bahnhof", "Straße" }),
        ("okul yolu", "Schulweg", new[] { "Schulhof", "Schulbus", "Schulranzen" }),
        ("komşuluk", "Nachbarschaft", new[] { "Freundschaft", "Verwandtschaft", "Gemeinschaft" }),
        ("hobi", "Hobby", new[] { "Beruf", "Pflicht", "Hausaufgabe" }),
        ("spor yapmak", "Sport treiben", new[] { "Musik hören", "fernsehen", "lesen" }),
        ("kıyafet", "Kleidung", new[] { "Schuhe", "Schmuck", "Tasche" }),
        ("misafir", "Gast", new[] { "Nachbar", "Fremder", "Verwandter" }),
        ("doğum günü", "Geburtstag", new[] { "Jahrestag", "Feiertag", "Ferientag" }),
        ("aile büyükleri", "Familienälteste (Großeltern etc.)", new[] { "kleine Geschwister", "entfernte Verwandte", "Nachbarn" })
    };

    private static QuizQuestion AileVeGunlukYasam(Random r)
    {
        var d = AileGunlukListe[r.Next(AileGunlukListe.Length)];
        var optionen = new[] { d.Almanca }.Concat(d.Yanlislar).OrderBy(_ => r.Next()).ToArray();

        return new QuizQuestion
        {
            Id = NewId(), Subject = Subject.Tuerkisch, GradeLevel = GradeLevel.Klasse6,
            Topic = "Aile ve Günlük Yaşam (Familie und Alltag) – Wortschatz", Type = QuestionType.MultipleChoice,
            Prompt = $"\"{d.TurkceKelime}\" kelimesinin Almancası hangisidir?",
            Options = optionen, CorrectAnswers = new[] { d.Almanca },
            Explanation = $"\"{d.TurkceKelime}\" Almanca \"{d.Almanca}\" demektir.",
            HelpHint = "Aile ve günlük yaşamla ilgili kelimeler: aile, kardeş, ev işleri, harçlık, alışveriş."
        };
    }

    private static readonly (string TurkceKelime, string Almanca, string[] Yanlislar)[] OkulToplumListe =
    {
        ("okul", "Schule", new[] { "Bibliothek", "Turnhalle", "Kindergarten" }),
        ("öğretmen", "Lehrer/in", new[] { "Schüler/in", "Direktor/in", "Hausmeister/in" }),
        ("ders programı", "Stundenplan", new[] { "Zeugnis", "Hausaufgabenheft", "Klassenbuch" }),
        ("sınıf arkadaşı", "Klassenkamerad/in", new[] { "Nachbar/in", "Geschwister", "Lehrer/in" }),
        ("teneffüs", "Pause", new[] { "Unterricht", "Prüfung", "Ferien" }),
        ("kural", "Regel", new[] { "Regal", "Vorschlag", "Meinung" }),
        ("millet", "Nation/Volk", new[] { "Stadt", "Familie", "Klasse" }),
        ("dil", "Sprache", new[] { "Zunge (nur anatomisch)", "Wort", "Buchstabe" }),
        ("kültürel çeşitlilik", "kulturelle Vielfalt", new[] { "kulturelle Einheit", "Sprachbarriere", "Traditionsverlust" }),
        ("ödev yapmak", "Hausaufgaben machen", new[] { "Hausaufgaben vergessen", "Hausaufgaben abschreiben", "Hausaufgaben verlieren" }),
        ("okula gitmek", "zur Schule gehen", new[] { "von der Schule kommen", "die Schule verlassen", "die Schule schwänzen" }),
        ("sınav", "Prüfung", new[] { "Ferien", "Unterrichtsstunde", "Zeugnis" }),
        ("meslek", "Beruf", new[] { "Hobby", "Schulfach", "Freizeit" }),
        ("vatandaş", "Bürger/in", new[] { "Ausländer/in", "Tourist/in", "Gast" }),
        ("toplum", "Gesellschaft", new[] { "Familie", "Klasse", "Nachbarschaft" }),
        ("saygı göstermek", "Respekt zeigen", new[] { "ignorieren", "sich streiten", "sich beschweren" }),
        ("arkadaşlık kurmak", "Freundschaft schließen", new[] { "sich streiten", "sich verstecken", "sich distanzieren" }),
        ("okul müdürü", "Schulleiter/in", new[] { "Klassenlehrer/in", "Hausmeister/in", "Sekretär/in" }),
        ("ders kitabı", "Schulbuch", new[] { "Tagebuch", "Kochbuch", "Wörterbuch" }),
        ("birlikte yaşamak", "zusammenleben", new[] { "alleine leben", "wegziehen", "sich trennen" })
    };

    private static QuizQuestion OkulVeToplum(Random r)
    {
        var d = OkulToplumListe[r.Next(OkulToplumListe.Length)];
        var optionen = new[] { d.Almanca }.Concat(d.Yanlislar).OrderBy(_ => r.Next()).ToArray();

        return new QuizQuestion
        {
            Id = NewId(), Subject = Subject.Tuerkisch, GradeLevel = GradeLevel.Klasse6,
            Topic = "Okul ve Toplum (Schule und Gesellschaft) – Wortschatz", Type = QuestionType.MultipleChoice,
            Prompt = $"\"{d.TurkceKelime}\" kelimesinin Almancası hangisidir?",
            Options = optionen, CorrectAnswers = new[] { d.Almanca },
            Explanation = $"\"{d.TurkceKelime}\" Almanca \"{d.Almanca}\" demektir.",
            HelpHint = "Okul ve toplumla ilgili kelimeler: öğretmen, sınıf arkadaşı, kural, toplum, saygı göstermek."
        };
    }

    private static readonly (string Frage, string[] Optionen, string Antwort, string Erklaerung)[] TurkiyeKulturuListe =
    {
        ("Türkiye'nin başkenti neresidir?", new[] { "Ankara", "İstanbul", "İzmir" }, "Ankara", "Türkiye'nin başkenti Ankara'dır, en büyük şehri ise İstanbul'dur."),
        ("Ramazan Bayramı ne zaman kutlanır?", new[] { "Ramazan ayının sonunda", "Yaz aylarında her zaman", "Yılbaşında" }, "Ramazan ayının sonunda", "Ramazan Bayramı, bir aylık oruç ayı olan Ramazan'ın sonunda kutlanır."),
        ("Kurban Bayramı'nda geleneksel olarak ne yapılır?", new[] { "Kurban kesilir ve paylaşılır", "Sadece tatil yapılır", "Okullar açılır" }, "Kurban kesilir ve paylaşılır", "Kurban Bayramı'nda kurban kesilir ve et ihtiyaç sahipleriyle paylaşılır."),
        ("Türkiye'nin en büyük şehri hangisidir (nüfusa göre)?", new[] { "İstanbul", "Ankara", "Bursa" }, "İstanbul", "İstanbul, nüfus bakımından Türkiye'nin en büyük şehridir."),
        ("İstanbul hangi iki kıtayı birbirine bağlar?", new[] { "Avrupa ve Asya", "Afrika ve Asya", "Avrupa ve Afrika" }, "Avrupa ve Asya", "İstanbul, Avrupa ve Asya kıtaları arasında köprü niteliğindedir."),
        ("Boğaziçi (Bosporus) neyi ayırır?", new[] { "İstanbul'un Avrupa ve Asya yakasını", "Karadeniz ve Akdeniz'i tamamen", "Türkiye ve Yunanistan'ı" }, "İstanbul'un Avrupa ve Asya yakasını", "Boğaziçi, İstanbul'un Avrupa yakası ile Asya yakasını birbirinden ayırır."),
        ("Türkiye'de resmi dil hangisidir?", new[] { "Türkçe", "Arapça", "Kürtçe" }, "Türkçe", "Türkiye'nin resmi dili Türkçedir."),
        ("23 Nisan hangi bayramla ilgilidir?", new[] { "Ulusal Egemenlik ve Çocuk Bayramı", "Cumhuriyet Bayramı", "Zafer Bayramı" }, "Ulusal Egemenlik ve Çocuk Bayramı", "23 Nisan, Ulusal Egemenlik ve Çocuk Bayramı olarak kutlanır."),
        ("29 Ekim hangi önemli günü kutlar?", new[] { "Cumhuriyet Bayramı", "Çocuk Bayramı", "Zafer Bayramı" }, "Cumhuriyet Bayramı", "29 Ekim, Türkiye Cumhuriyeti'nin ilan edildiği gün olan Cumhuriyet Bayramı'dır."),
        ("Türk mutfağının ünlü bir tatlısı hangisidir?", new[] { "Baklava", "Tiramisu", "Croissant" }, "Baklava", "Baklava, Türk mutfağının dünyaca ünlü tatlılarından biridir."),
        ("Nazar boncuğu neyi simgeler (halk inanışına göre)?", new[] { "Kötü bakışlardan/nazardan korunmayı", "Bolluk ve bereketi", "Uğursuzluğu" }, "Kötü bakışlardan/nazardan korunmayı", "Nazar boncuğu, halk inanışına göre kötü bakışlardan/nazardan koruduğuna inanılan bir semboldür."),
        ("Türkiye'de yaygın bir geleneksel içecek hangisidir?", new[] { "Çay", "Kola", "Meyve suyu" }, "Çay", "Çay, Türkiye'de günlük hayatta en yaygın tüketilen içeceklerden biridir."),
        ("Anadolu ne anlama gelir (coğrafi olarak)?", new[] { "Türkiye'nin Asya kıtasındaki büyük yarımadası", "Türkiye'nin başkenti", "İstanbul'un bir semti" }, "Türkiye'nin Asya kıtasındaki büyük yarımadası", "Anadolu, Türkiye'nin Asya kıtasında yer alan büyük yarımadasıdır."),
        ("Karadeniz Türkiye'nin hangi bölgesinde yer alır?", new[] { "Kuzeyinde", "Güneyinde", "Batısında" }, "Kuzeyinde", "Karadeniz, Türkiye'nin kuzeyinde yer alır."),
        ("Akdeniz Türkiye'nin hangi bölgesinde yer alır?", new[] { "Güneyinde", "Kuzeyinde", "Doğusunda" }, "Güneyinde", "Akdeniz, Türkiye'nin güneyinde yer alır."),
        ("Kapadokya, hangi doğal oluşumuyla ünlüdür?", new[] { "Peri bacaları (ilginç kaya oluşumları)", "Yüksek dağlar", "Büyük göller" }, "Peri bacaları (ilginç kaya oluşumları)", "Kapadokya, peri bacaları adı verilen ilginç kaya oluşumlarıyla ünlüdür."),
        ("Pamukkale hangi doğal özelliğiyle ünlüdür?", new[] { "Beyaz travertenler ve termal sular", "Kum tepeleri", "Volkanik dağlar" }, "Beyaz travertenler ve termal sular", "Pamukkale, beyaz travertenleri ve termal sularıyla ünlüdür."),
        ("Türk halk müziğinde sıkça kullanılan bir çalgı hangisidir?", new[] { "Bağlama (saz)", "Gitar", "Piyano" }, "Bağlama (saz)", "Bağlama (saz), Türk halk müziğinde sıkça kullanılan geleneksel bir çalgıdır."),
        ("Türkiye'de misafirperverlik geleneği neyi ifade eder?", new[] { "Misafirlere karşı gösterilen konukseverlik ve saygı", "Misafirlerden uzak durmayı", "Sadece akrabaları ağırlamayı" }, "Misafirlere karşı gösterilen konukseverlik ve saygı", "Misafirperverlik, Türk kültüründe misafirlere gösterilen konukseverlik ve saygıyı ifade eder."),
        ("Hıdırellez hangi mevsimle ilişkili bir halk bayramıdır?", new[] { "İlkbahar", "Kış", "Sonbahar" }, "İlkbahar", "Hıdırellez, ilkbaharın gelişini kutlayan geleneksel bir halk bayramıdır.")
    };

    private static QuizQuestion TurkiyeKulturu(Random r)
    {
        var f = TurkiyeKulturuListe[r.Next(TurkiyeKulturuListe.Length)];
        return new QuizQuestion
        {
            Id = NewId(), Subject = Subject.Tuerkisch, GradeLevel = GradeLevel.Klasse6,
            Topic = "Türk Kültürü ve Gelenekleri (Kultur und Traditionen)", Type = QuestionType.MultipleChoice,
            Prompt = f.Frage, Options = f.Optionen, CorrectAnswers = new[] { f.Antwort }, Explanation = f.Erklaerung,
            HelpHint = "Türkiye'nin başkenti Ankara, en büyük şehri İstanbul'dur. Önemli bayramlar: Ramazan, Kurban, 23 Nisan, 29 Ekim."
        };
    }

    private static readonly (string Cumle, string Oge, string Cevap)[] CumleOgeleriListe =
    {
        ("Ali topu attı.", "Yüklem", "attı"),
        ("Ali topu attı.", "Özne", "Ali"),
        ("Annem bana kitap aldı.", "Nesne", "kitap"),
        ("Öğretmen sınıfta ders anlattı.", "Yer Tamlayıcısı (Zarf Tümleci)", "sınıfta"),
        ("Çocuklar bahçede oynadı.", "Yer Tamlayıcısı (Zarf Tümleci)", "bahçede"),
        ("Annem bana kitap aldı.", "Özne", "Annem"),
        ("Öğretmen sınıfta ders anlattı.", "Yüklem", "anlattı"),
        ("Çocuklar bahçede oynadı.", "Özne", "Çocuklar"),
        ("Kedi süt içti.", "Yüklem", "içti"),
        ("Kedi süt içti.", "Nesne", "süt"),
        ("Babam arabayı yıkadı.", "Nesne", "arabayı"),
        ("Babam arabayı yıkadı.", "Özne", "Babam"),
        ("Kardeşim akşam eve geldi.", "Zaman Tamlayıcısı (Zarf Tümleci)", "akşam"),
        ("Kardeşim akşam eve geldi.", "Yer Tamlayıcısı (Zarf Tümleci)", "eve"),
        ("Ayşe dün mektup yazdı.", "Zaman Tamlayıcısı (Zarf Tümleci)", "dün"),
        ("Ayşe dün mektup yazdı.", "Nesne", "mektup"),
        ("Öğrenciler parkta top oynadı.", "Yer Tamlayıcısı (Zarf Tümleci)", "parkta"),
        ("Öğrenciler parkta top oynadı.", "Nesne", "top"),
        ("Anne mutfakta yemek pişirdi.", "Yer Tamlayıcısı (Zarf Tümleci)", "mutfakta"),
        ("Anne mutfakta yemek pişirdi.", "Nesne", "yemek")
    };

    private static QuizQuestion CumleOgeleri(Random r)
    {
        var c = CumleOgeleriListe[r.Next(CumleOgeleriListe.Length)];

        return new QuizQuestion
        {
            Id = NewId(),
            Subject = Subject.Tuerkisch,
            GradeLevel = GradeLevel.Klasse9,
            Topic = "Cümlenin Ögeleri (Satzglieder)",
            Type = QuestionType.OpenText,
            Prompt = $"Cümle: \"{c.Cumle}\" – Bu cümlenin {c.Oge}'i nedir?",
            CorrectAnswers = new[] { c.Cevap },
            Explanation = $"\"{c.Cumle}\" cümlesinde {c.Oge}: \"{c.Cevap}\".",
            HelpHint = "Özne (kim/ne yapıyor?), Yüklem (eylem/fiil), Nesne (eylemin etkilediği şey), Yer/Zaman Tamlayıcısı (nerede/ne zaman?)."
        };
    }

    private static readonly (string Fiil, string Gelecek)[] GelecekZamanBeispiele =
    {
        ("gitmek", "gidecek"), ("gelmek", "gelecek"), ("okumak", "okuyacak"),
        ("yazmak", "yazacak"), ("oynamak", "oynayacak"),
        ("içmek", "içecek"), ("görmek", "görecek"), ("bilmek", "bilecek"),
        ("sevmek", "sevecek"), ("gülmek", "gülecek"), ("ağlamak", "ağlayacak"),
        ("uyumak", "uyuyacak"), ("konuşmak", "konuşacak"), ("düşünmek", "düşünecek"),
        ("beklemek", "bekleyecek"), ("çalışmak", "çalışacak"), ("dinlemek", "dinleyecek"),
        ("anlamak", "anlayacak"), ("yemek", "yiyecek"), ("almak", "alacak")
    };

    private static QuizQuestion GelecekZaman(Random r)
    {
        var v = GelecekZamanBeispiele[r.Next(GelecekZamanBeispiele.Length)];

        return new QuizQuestion
        {
            Id = NewId(),
            Subject = Subject.Tuerkisch,
            GradeLevel = GradeLevel.Klasse9,
            Topic = "Gelecek Zaman (Futur)",
            Type = QuestionType.OpenText,
            Prompt = $"\"{v.Fiil}\" fiilinin (o/she/it için) gelecek zaman (-ecek/-acak) hâlini yaz.",
            CorrectAnswers = new[] { v.Gelecek },
            Explanation = $"\"{v.Fiil}\" -> \"{v.Gelecek}\". Gelecek zaman \"-ecek/-acak\" eki ile kurulur.",
            HelpHint = "Gelecek zaman (Futur) her zaman \"-ecek/-acak\" ekiyle kurulur - kelime kökünün son ünlüsüne göre \"e\" veya \"a\" seçilir."
        };
    }

    private static readonly (string SatzMitLuecke, string Loesung, string Regel)[] YazimBeispiele =
    {
        ("Yarın okula gid___im.", "eceğ", "Gelecek zaman eki ünlü ile başlayan ekten önce yumuşar: gid-eceğ-im."),
        ("Kitab___ okudum.", "ı", "\"Kitap\" kelimesi ünlüyle başlayan ek aldığında p -> b yumuşamasına uğrar: kitab-ı."),
        ("Ali'___ gördüm.", "yi", "Özel isimlere gelen ekler kesme işareti ile ayrılır: Ali'yi."),
        ("Renkli kalem___ getir.", "i", "Belirtili nesne \"-i\" hâl ekini alır: kalem-i."),
        ("Öğretmen___ soruyu sordu.", "e", "Yönelme (-e/-a) hâl eki, ünsüz yumuşamasına uğramayan kelimelere doğrudan eklenir: öğretmen-e."),
        ("Yarın parka gid___im.", "eceğ", "Gelecek zaman ekinin sonundaki \"k\", ünlüyle başlayan kişi ekinden önce \"ğ\"ye yumuşar: gid-eceğ-im."),
        ("Kitabı yarın oku___ım.", "yacağ", "Ünlüyle biten fiile gelecek zaman eki \"y\" kaynaştırma harfiyle eklenir, sondaki \"k\" de ünlüden önce yumuşar: oku-yacağ-ım."),
        ("Yarın erken kalk___ım.", "acağ", "Gelecek zaman ekinin sonundaki \"k\", ünlüyle başlayan kişi ekinden önce \"ğ\"ye yumuşar: kalk-acağ-ım."),
        ("Dolab___ açtım.", "ı", "\"Dolap\" kelimesi ünlüyle başlayan ek aldığında p -> b yumuşamasına uğrar: dolab-ı."),
        ("Ağac___ çok büyük.", "ı", "\"Ağaç\" kelimesi ünlüyle başlayan ek aldığında ç -> c yumuşamasına uğrar: ağac-ı."),
        ("Kağıd___ buruştu.", "ı", "\"Kağıt\" kelimesi ünlüyle başlayan ek aldığında t -> d yumuşamasına uğrar: kağıd-ı."),
        ("Zeynep'___ aradım.", "i", "Özel isimlere gelen ekler kesme işareti ile ayrılır: Zeynep'i."),
        ("İstanbul'___ gittik.", "a", "Özel isimlere gelen ekler kesme işareti ile ayrılır: İstanbul'a."),
        ("Mehmet'___ kitap verdim.", "e", "Özel isimlere gelen ekler kesme işareti ile ayrılır: Mehmet'e."),
        ("Yeni çanta___ aldım.", "yı", "Belirtili nesne \"-yı\" hâl ekini alır (ünlüyle biten kelimeye kaynaştırma harfi \"y\" ile): çanta-yı."),
        ("Kırmızı elma___ ye.", "yı", "Belirtili nesne hâl eki ünlüyle biten kelimeye \"y\" kaynaştırma harfiyle eklenir: elma-yı."),
        ("Büyük top___ getir.", "u", "Belirtili nesne \"-u\" hâl ekini alır (ünlü uyumuna göre): top-u."),
        ("Anne___ çiçek getirdim.", "ye", "Yönelme hâl eki ünlüyle biten kelimeye \"y\" kaynaştırma harfiyle eklenir: anne-ye."),
        ("Okul___ gidiyorum.", "a", "Yönelme (-e/-a) hâl eki ünsüz yumuşamasına uğramayan kelimelere doğrudan eklenir: okul-a."),
        ("Doktor___ gittik.", "a", "Yönelme (-e/-a) hâl eki, ünlü uyumuna göre kelimeye doğrudan eklenir: doktor-a.")
    };

    private static QuizQuestion YazimKurallari(Random r)
    {
        var y = YazimBeispiele[r.Next(YazimBeispiele.Length)];

        return new QuizQuestion
        {
            Id = NewId(),
            Subject = Subject.Tuerkisch,
            GradeLevel = GradeLevel.Klasse9,
            Topic = "Yazım Kuralları (Rechtschreibung)",
            Type = QuestionType.OpenText,
            Prompt = $"Boşluğa gelmesi gereken eki yaz: \"{y.SatzMitLuecke}\"",
            CorrectAnswers = new[] { y.Loesung },
            Explanation = y.Regel,
            HelpHint = "Türkçede ünsüz yumuşaması (p→b, ç→c, t→d, k→ğ) ve ünlü uyumu, ek eklenirken kelimenin son sesine göre değişir."
        };
    }

    private static readonly (string Cumle, string Fiilimsi, string Tur)[] FiilimsiListe =
    {
        ("Koşan çocuk düştü.", "Koşan", "Sıfat-fiil (Partizip)"),
        ("Okumadan sınava girdi.", "Okumadan", "Zarf-fiil (Adverbialpartizip)"),
        ("Yüzmek çok eğlencelidir.", "Yüzmek", "İsim-fiil (Verbalnomen)"),
        ("Gülen yüzüyle herkesi mutlu etti.", "Gülen", "Sıfat-fiil (Partizip)"),
        ("Eve gelir gelmez uyudu.", "gelir gelmez", "Zarf-fiil (Adverbialpartizip)"),
        ("Uçan kuşu izledik.", "Uçan", "Sıfat-fiil (Partizip)"),
        ("Yazmak benim hobimdir.", "Yazmak", "İsim-fiil (Verbalnomen)"),
        ("Konuşurken gülümsedi.", "Konuşurken", "Zarf-fiil (Adverbialpartizip)"),
        ("Kırılan cam yerlere düştü.", "Kırılan", "Sıfat-fiil (Partizip)"),
        ("Okumak çok önemlidir.", "Okumak", "İsim-fiil (Verbalnomen)"),
        ("Eve varınca telefon etti.", "varınca", "Zarf-fiil (Adverbialpartizip)"),
        ("Gelen misafirleri karşıladık.", "Gelen", "Sıfat-fiil (Partizip)"),
        ("Yüzmeyi çok seviyorum.", "Yüzmeyi", "İsim-fiil (Verbalnomen)"),
        ("Düşünmeden konuştu.", "Düşünmeden", "Zarf-fiil (Adverbialpartizip)"),
        ("Ağlayan bebek uyudu.", "Ağlayan", "Sıfat-fiil (Partizip)"),
        ("Koşmak sağlık için iyidir.", "Koşmak", "İsim-fiil (Verbalnomen)"),
        ("Kapıyı açar açmaz içeri girdi.", "açar açmaz", "Zarf-fiil (Adverbialpartizip)"),
        ("Uyuyan çocuğu uyandırmadık.", "Uyuyan", "Sıfat-fiil (Partizip)"),
        ("Yazmak zaman alır.", "Yazmak", "İsim-fiil (Verbalnomen)"),
        ("Gülerek bize baktı.", "Gülerek", "Zarf-fiil (Adverbialpartizip)")
    };

    private static QuizQuestion FiilimsiTuru(Random r)
    {
        var f = FiilimsiListe[r.Next(FiilimsiListe.Length)];
        var optionen = new[] { "Sıfat-fiil (Partizip)", "Zarf-fiil (Adverbialpartizip)", "İsim-fiil (Verbalnomen)" };

        return new QuizQuestion
        {
            Id = NewId(),
            Subject = Subject.Tuerkisch,
            GradeLevel = GradeLevel.Klasse9,
            Topic = "Fiilimsi (Partizip/Verbalnomen)",
            Type = QuestionType.MultipleChoice,
            Prompt = $"Cümle: \"{f.Cumle}\" – \"{f.Fiilimsi}\" hangi fiilimsi türüdür?",
            Options = optionen,
            CorrectAnswers = new[] { f.Tur },
            Explanation = $"\"{f.Fiilimsi}\" bir {f.Tur} örneğidir.",
            HelpHint = "Sıfat-fiil bir ismi niteler (koşan çocuk), zarf-fiil bir eylemi nasıl/ne zaman yapıldığını anlatır (okumadan), isim-fiil eylemi isim gibi kullanır (yüzmek)."
        };
    }

    private static readonly (string TurkceKelime, string Almanca, string[] Yanlislar)[] KimlikGelecekListe =
    {
        ("gelecek planı", "Zukunftsplan", new[] { "Vergangenheit", "Tatil planı", "Schulplan" }),
        ("iki dillilik", "Zweisprachigkeit", new[] { "Einsprachigkeit", "Sprachlosigkeit", "Fremdsprache" }),
        ("kimlik", "Identität", new[] { "Kennkarte (nur Dokument)", "Charakter", "Persönlichkeit (nur äußerlich)" }),
        ("göç etmek", "auswandern/migrieren", new[] { "reisen", "zurückkehren", "besuchen" }),
        ("hayal kurmak", "träumen (von der Zukunft)", new[] { "sich erinnern", "sich fürchten", "sich langweilen" }),
        ("kendine güven", "Selbstvertrauen", new[] { "Selbstzweifel", "Bescheidenheit", "Unsicherheit" }),
        ("rol model", "Vorbild", new[] { "Schauspieler", "Rolle im Theater", "Anführer" }),
        ("iki kültür arasında yaşamak", "zwischen zwei Kulturen leben", new[] { "nur in einer Kultur leben", "keine Kultur haben", "eine Kultur ablehnen" }),
        ("meslek seçimi", "Berufswahl", new[] { "Schulfachwahl", "Hobbywahl", "Studienort" }),
        ("başarı", "Erfolg", new[] { "Misserfolg", "Zufall", "Glück" }),
        ("hedef belirlemek", "sich ein Ziel setzen", new[] { "ein Ziel vergessen", "kein Ziel haben", "ein Ziel ablehnen" }),
        ("özgüven kazanmak", "Selbstvertrauen gewinnen", new[] { "Selbstvertrauen verlieren", "sich verstecken", "sich zurückziehen" }),
        ("akran baskısı", "Gruppenzwang", new[] { "Elterndruck", "Lehrerdruck", "Notendruck" }),
        ("zorluklarla başa çıkmak", "mit Schwierigkeiten umgehen", new[] { "Schwierigkeiten ignorieren", "aufgeben", "sich beschweren" }),
        ("kişisel gelişim", "persönliche Entwicklung", new[] { "Schulnote", "Freizeitaktivität", "Berufserfahrung" }),
        ("aidiyet duygusu", "Zugehörigkeitsgefühl", new[] { "Fremdheitsgefühl", "Gleichgültigkeit", "Einsamkeit" }),
        ("önyargı", "Vorurteil", new[] { "Meinung ohne Bewertung", "Tatsache", "Beweis" }),
        ("karar vermek", "eine Entscheidung treffen", new[] { "eine Entscheidung vermeiden", "eine Entscheidung vergessen", "eine Entscheidung ablehnen" }),
        ("gurbet", "Fremde/Ausland (fern von der Heimat)", new[] { "Heimat", "Nachbarschaft", "Verwandtschaft" }),
        ("kültürel kimlik", "kulturelle Identität", new[] { "kulturelle Verwirrung", "kulturelle Ablehnung", "kulturelle Isolation" })
    };

    private static QuizQuestion KimlikVeGelecek(Random r)
    {
        var d = KimlikGelecekListe[r.Next(KimlikGelecekListe.Length)];
        var optionen = new[] { d.Almanca }.Concat(d.Yanlislar).OrderBy(_ => r.Next()).ToArray();

        return new QuizQuestion
        {
            Id = NewId(), Subject = Subject.Tuerkisch, GradeLevel = GradeLevel.Klasse9,
            Topic = "Kimlik ve Gelecek (Identität und Zukunft) – Wortschatz", Type = QuestionType.MultipleChoice,
            Prompt = $"\"{d.TurkceKelime}\" kelimesinin Almancası hangisidir?",
            Options = optionen, CorrectAnswers = new[] { d.Almanca },
            Explanation = $"\"{d.TurkceKelime}\" Almanca \"{d.Almanca}\" demektir.",
            HelpHint = "Kimlik ve gelecekle ilgili kelimeler: gelecek planı, iki dillilik, göç etmek, kendine güven, hedef belirlemek."
        };
    }

    private static readonly (string Frage, string[] Optionen, string Antwort, string Erklaerung)[] TarihGelenekListe =
    {
        ("Türkiye Cumhuriyeti hangi yıl kuruldu?", new[] { "1923", "1918", "1938" }, "1923", "Türkiye Cumhuriyeti 29 Ekim 1923'te ilan edildi."),
        ("Türkiye Cumhuriyeti'nin kurucusu kimdir?", new[] { "Mustafa Kemal Atatürk", "Süleyman Demirel", "İsmet İnönü" }, "Mustafa Kemal Atatürk", "Türkiye Cumhuriyeti'nin kurucusu Mustafa Kemal Atatürk'tür."),
        ("Atatürk hangi yıl vefat etti?", new[] { "1938", "1923", "1950" }, "1938", "Mustafa Kemal Atatürk 1938 yılında vefat etti."),
        ("Osmanlı İmparatorluğu ne zaman sona erdi?", new[] { "1922 (saltanatın kaldırılmasıyla)", "1850", "1980" }, "1922 (saltanatın kaldırılmasıyla)", "Osmanlı İmparatorluğu, 1922'de saltanatın kaldırılmasıyla sona erdi."),
        ("Cumhuriyet öncesi Türkiye hangi imparatorluğun bir parçasıydı?", new[] { "Osmanlı İmparatorluğu", "Roma İmparatorluğu", "Bizans İmparatorluğu (doğrudan devam olarak değil)" }, "Osmanlı İmparatorluğu", "Cumhuriyet öncesinde bugünkü Türkiye toprakları Osmanlı İmparatorluğu'na aitti."),
        ("Atatürk'ün yaptığı önemli reformlardan biri hangisidir?", new[] { "Latin alfabesine geçiş", "Osmanlıcayı zorunlu kılmak", "Eğitimi yasaklamak" }, "Latin alfabesine geçiş", "Atatürk döneminde 1928'de Latin alfabesine geçildi."),
        ("Kurtuluş Savaşı hangi yıllar arasında gerçekleşti?", new[] { "1919-1922", "1939-1945", "1950-1955" }, "1919-1922", "Kurtuluş Savaşı 1919-1922 yılları arasında gerçekleşti."),
        ("İstanbul'un fethi hangi yıl gerçekleşti?", new[] { "1453", "1923", "1071" }, "1453", "İstanbul, 1453 yılında Osmanlılar tarafından fethedildi."),
        ("İstanbul'u fetheden Osmanlı padişahı kimdir?", new[] { "II. Mehmet (Fatih Sultan Mehmet)", "Kanuni Sultan Süleyman", "Yavuz Sultan Selim" }, "II. Mehmet (Fatih Sultan Mehmet)", "İstanbul'u fetheden padişah II. Mehmet, yani Fatih Sultan Mehmet'tir."),
        ("Türkiye'de kadınlara seçme ve seçilme hakkı hangi dönemde tanındı?", new[] { "Cumhuriyetin ilk yıllarında (1930'larda)", "Osmanlı döneminde", "2000'li yıllarda" }, "Cumhuriyetin ilk yıllarında (1930'larda)", "Kadınlara seçme ve seçilme hakkı Cumhuriyetin ilk yıllarında, 1930'larda tanındı."),
        ("Ankara neden Türkiye'nin başkenti seçildi?", new[] { "Kurtuluş Savaşı'nın merkezi ve stratejik açıdan güvenli konumu nedeniyle", "En kalabalık şehir olduğu için", "Deniz kıyısında olduğu için" }, "Kurtuluş Savaşı'nın merkezi ve stratejik açıdan güvenli konumu nedeniyle", "Ankara, Kurtuluş Savaşı'nın merkezi olması ve stratejik konumu nedeniyle başkent seçildi."),
        ("1071 Malazgirt Savaşı'nın önemi nedir?", new[] { "Türklerin Anadolu'ya yerleşmesinin başlangıcı sayılır", "Cumhuriyetin kuruluşudur", "Osmanlı'nın sonu sayılır" }, "Türklerin Anadolu'ya yerleşmesinin başlangıcı sayılır", "Malazgirt Savaşı, Türklerin Anadolu'ya yerleşmesinin başlangıcı olarak kabul edilir."),
        ("Selçuklu Devleti'nden sonra Anadolu'da hangi büyük devlet kuruldu?", new[] { "Osmanlı İmparatorluğu", "Bizans İmparatorluğu", "Roma İmparatorluğu" }, "Osmanlı İmparatorluğu", "Selçuklu Devleti'nden sonra Anadolu'da Osmanlı İmparatorluğu kuruldu."),
        ("Türkiye'nin resmi bayramlarından biri olan Zafer Bayramı hangi tarihi olayı anar?", new[] { "Kurtuluş Savaşı'nın kazanılmasını (30 Ağustos)", "Cumhuriyetin ilanını", "Atatürk'ün doğumunu" }, "Kurtuluş Savaşı'nın kazanılmasını (30 Ağustos)", "Zafer Bayramı, 30 Ağustos'ta Kurtuluş Savaşı'nın kazanılmasını anar."),
        ("Atatürk ilkelerinden biri hangisidir?", new[] { "Laiklik", "Monarşi", "Feodalizm" }, "Laiklik", "Laiklik, Atatürk'ün altı ilkesinden (Atatürk ilkeleri) biridir."),
        ("Osmanlı İmparatorluğu'nun başkenti neresiydi (fetihten sonra)?", new[] { "İstanbul", "Ankara", "İzmir" }, "İstanbul", "1453'teki fetihten sonra İstanbul, Osmanlı İmparatorluğu'nun başkenti oldu."),
        ("Türkiye'de eğitim hangi Atatürk reformuyla laik hâle getirildi?", new[] { "Tevhid-i Tedrisat Kanunu (Öğretim Birliği Yasası)", "Latin alfabesinin kabulü", "Kadınlara oy hakkı verilmesi" }, "Tevhid-i Tedrisat Kanunu (Öğretim Birliği Yasası)", "Tevhid-i Tedrisat Kanunu ile eğitim tek elden ve laik bir sisteme bağlandı."),
        ("Cumhuriyet Bayramı hangi tarihte kutlanır?", new[] { "29 Ekim", "23 Nisan", "30 Ağustos" }, "29 Ekim", "Cumhuriyet Bayramı, Cumhuriyetin ilan edildiği 29 Ekim'de kutlanır."),
        ("Atatürk'ün \"Yurtta sulh, cihanda sulh\" sözü ne anlama gelir?", new[] { "Ülke içinde ve dünyada barış", "Ülke içinde savaş, dünyada barış", "Sadece askeri güç önemlidir" }, "Ülke içinde ve dünyada barış", "Bu söz, hem ülke içinde hem de dünyada barışın önemini vurgular."),
        ("Türkiye Cumhuriyeti'nin ilk cumhurbaşkanı kimdir?", new[] { "Mustafa Kemal Atatürk", "İsmet İnönü", "Celal Bayar" }, "Mustafa Kemal Atatürk", "Mustafa Kemal Atatürk, Türkiye Cumhuriyeti'nin ilk cumhurbaşkanıdır.")
    };

    private static QuizQuestion TarihVeGelenekler(Random r)
    {
        var f = TarihGelenekListe[r.Next(TarihGelenekListe.Length)];
        return new QuizQuestion
        {
            Id = NewId(), Subject = Subject.Tuerkisch, GradeLevel = GradeLevel.Klasse9,
            Topic = "Türk Tarihi ve Gelenekleri (Geschichte und Traditionen)", Type = QuestionType.MultipleChoice,
            Prompt = f.Frage, Options = f.Optionen, CorrectAnswers = new[] { f.Antwort }, Explanation = f.Erklaerung,
            HelpHint = "Türkiye Cumhuriyeti 1923'te Atatürk tarafından kuruldu. Önemli tarihler: 1453 (İstanbul'un fethi), 1919-1922 (Kurtuluş Savaşı), 29 Ekim (Cumhuriyet Bayramı)."
        };
    }

    private static readonly (string Frage, string[] Optionen, string Antwort, string Erklaerung)[] TurkiyeCografyasiListe =
    {
        ("Türkiye'nin sınırları içinde tamamen yer alan en uzun nehri hangisidir?", new[] { "Kızılırmak", "Fırat", "Sakarya" }, "Kızılırmak", "Kızılırmak, tamamen Türkiye sınırları içinde akan en uzun nehirdir."),
        ("Türkiye kaç kıtaya yakın/bağlantılıdır (coğrafi konumu itibariyle)?", new[] { "İki kıtaya (Avrupa ve Asya)", "Üç kıtaya", "Sadece Asya'ya" }, "İki kıtaya (Avrupa ve Asya)", "Türkiye topraklarının küçük bir kısmı Avrupa'da, büyük kısmı ise Asya'dadır."),
        ("Türkiye'nin en yüksek dağı hangisidir?", new[] { "Ağrı Dağı", "Uludağ", "Erciyes Dağı" }, "Ağrı Dağı", "Ağrı Dağı, Türkiye'nin en yüksek dağıdır."),
        ("Ege Bölgesi hangi denize kıyısı vardır?", new[] { "Ege Denizi", "Karadeniz", "Akdeniz" }, "Ege Denizi", "Ege Bölgesi, adından da anlaşılacağı gibi Ege Denizi'ne kıyıdır."),
        ("Türkiye'nin güneydoğusunda hangi coğrafi bölge yer alır?", new[] { "Güneydoğu Anadolu Bölgesi", "Karadeniz Bölgesi", "Marmara Bölgesi" }, "Güneydoğu Anadolu Bölgesi", "Güneydoğu Anadolu Bölgesi, Türkiye'nin güneydoğusunda yer alır."),
        ("Marmara Bölgesi'nde yer alan büyük deniz hangisidir?", new[] { "Marmara Denizi", "Van Gölü", "Tuz Gölü" }, "Marmara Denizi", "Marmara Denizi, Marmara Bölgesi'nde yer alır ve bölgeye adını verir."),
        ("Türkiye'nin en büyük gölü hangisidir?", new[] { "Van Gölü", "Tuz Gölü", "Beyşehir Gölü" }, "Van Gölü", "Van Gölü, Türkiye'nin en büyük gölüdür."),
        ("Kapadokya hangi bölgede yer alır?", new[] { "İç Anadolu Bölgesi", "Karadeniz Bölgesi", "Akdeniz Bölgesi" }, "İç Anadolu Bölgesi", "Kapadokya, İç Anadolu Bölgesi'nde yer alır."),
        ("Türkiye'nin turizm açısından önemli kıyı şeridi hangi bölgelerdedir?", new[] { "Ege ve Akdeniz kıyıları", "Sadece Karadeniz kıyıları", "Sadece İç Anadolu" }, "Ege ve Akdeniz kıyıları", "Ege ve Akdeniz kıyıları, Türkiye'nin turizm açısından en önemli bölgeleridir."),
        ("Boğazlar (İstanbul ve Çanakkale Boğazı) hangi denizleri birbirine bağlar?", new[] { "Karadeniz'i Akdeniz'e (Marmara üzerinden)", "Ege Denizi'ni Kızıldeniz'e", "Atlantik'i Pasifik'e" }, "Karadeniz'i Akdeniz'e (Marmara üzerinden)", "İstanbul ve Çanakkale Boğazları, Marmara Denizi üzerinden Karadeniz'i Akdeniz'e bağlar."),
        ("Türkiye'de kış turizmiyle bilinen bir merkez hangisidir?", new[] { "Uludağ", "Bodrum", "Antalya" }, "Uludağ", "Uludağ, Türkiye'de kayak ve kış turizmiyle bilinen önemli bir merkezdir."),
        ("Doğu Anadolu Bölgesi'nin iklimi genel olarak nasıldır?", new[] { "Karasal, kışları çok soğuk", "Ilıman, kışları ılık", "Tropikal, her mevsim sıcak" }, "Karasal, kışları çok soğuk", "Doğu Anadolu Bölgesi'nde sert bir karasal iklim hâkimdir, kışlar çok soğuk geçer."),
        ("Türkiye'nin sahip olduğu doğal afet risklerinden biri hangisidir?", new[] { "Deprem", "Volkanik patlama her bölgede sık", "Kasırga sık görülür" }, "Deprem", "Türkiye, jeolojik konumu nedeniyle önemli bir deprem riski taşır."),
        ("Türkiye hangi deprem kuşağında yer alır?", new[] { "Alp-Himalaya deprem kuşağı", "Pasifik Ateş Çemberi", "Deprem riski taşımaz" }, "Alp-Himalaya deprem kuşağı", "Türkiye, dünyanın önemli deprem kuşaklarından biri olan Alp-Himalaya kuşağında yer alır."),
        ("Karadeniz Bölgesi'nin ekonomisinde önemli bir tarım ürünü hangisidir?", new[] { "Çay ve fındık", "Zeytin", "Pamuk" }, "Çay ve fındık", "Karadeniz Bölgesi, çay ve fındık üretimiyle bilinir."),
        ("Ege Bölgesi'nde yaygın olarak yetiştirilen bir tarım ürünü hangisidir?", new[] { "Zeytin ve incir", "Çay", "Muz" }, "Zeytin ve incir", "Ege Bölgesi'nde zeytin ve incir yaygın olarak yetiştirilir."),
        ("Güneydoğu Anadolu Projesi (GAP) hangi amaçla geliştirilmiştir?", new[] { "Bölgenin sulama ve enerji ihtiyacını karşılamak", "Turizmi geliştirmek", "Sadece demiryolu yapmak" }, "Bölgenin sulama ve enerji ihtiyacını karşılamak", "GAP, Güneydoğu Anadolu Bölgesi'nin sulama ve enerji ihtiyacını karşılamak amacıyla geliştirilmiştir."),
        ("Türkiye'nin komşu ülkelerinden biri hangisidir?", new[] { "Yunanistan", "İtalya", "Fransa" }, "Yunanistan", "Yunanistan, Türkiye'nin batı komşularından biridir."),
        ("Anadolu'nun ortasında yer alan büyük tuzlu göl hangisidir?", new[] { "Tuz Gölü", "Van Gölü", "Eğirdir Gölü" }, "Tuz Gölü", "Tuz Gölü, İç Anadolu'da yer alan büyük ve tuzlu bir göldür."),
        ("Pamukkale hangi coğrafi bölgede yer alır?", new[] { "Ege Bölgesi", "Karadeniz Bölgesi", "Doğu Anadolu Bölgesi" }, "Ege Bölgesi", "Pamukkale, Ege Bölgesi'nde, Denizli ilinde yer alır.")
    };

    private static QuizQuestion TurkiyeCografyasi(Random r)
    {
        var f = TurkiyeCografyasiListe[r.Next(TurkiyeCografyasiListe.Length)];
        return new QuizQuestion
        {
            Id = NewId(), Subject = Subject.Tuerkisch, GradeLevel = GradeLevel.Klasse9,
            Topic = "Türkiye'nin Coğrafyası (Geografie der Türkei)", Type = QuestionType.MultipleChoice,
            Prompt = f.Frage, Options = f.Optionen, CorrectAnswers = new[] { f.Antwort }, Explanation = f.Erklaerung,
            HelpHint = "Türkiye Avrupa ve Asya arasında yer alır. Bölgeler: Karadeniz (kuzey), Akdeniz (güney), Ege (batı), İç Anadolu (orta), Doğu ve Güneydoğu Anadolu."
        };
    }
}
