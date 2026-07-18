using LernTor.Core.Enums;
using LernTor.Core.Models;

namespace LernTor.ContentGen.Generators;

/// <summary>
/// Türkisch-Aufgabengenerator für bilinguale/herkunftssprachliche Lerner: Zeitformen, Wortschatz,
/// Ekler (Suffixe) für Klasse 6, birleşik zamanlar, Deyimler/Atasözleri, Noktalama und Metin
/// türleri für Klasse 7, sowie Satzglieder, Fiilimsi und Rechtschreibung für Klasse 9.
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
            [GradeLevel.Klasse7] = new List<TopicFactory>
            {
                SimdikiZamaninHikayesi,
                BelirsizGecmisZaman,
                DeyimlerVeAtasozleri,
                NoktalamaIsaretleri,
                MetinTurleri,
                MedyaVeIletisim
            },
            [GradeLevel.Klasse9] = new List<TopicFactory>
            {
                CumleOgeleri,
                GelecekZaman,
                YazimKurallari,
                FiilimsiTuru,
                KimlikVeGelecek,
                TarihVeGelenekler,
                TurkiyeCografyasi,
                AlltagUndKonsum,
                GesellschaftUndOeffentlichesLeben,
                SchuleUndBerufswelt
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

    private static readonly (string TurkceKelime, string Almanca, string[] Yanlislar)[] AlltagKonsumListe =
    {
        ("alışveriş", "Einkaufen", new[] { "Reisen", "Kochen", "Aufräumen" }),
        ("indirim", "Rabatt", new[] { "Erhöhung", "Steuer", "Gebühr" }),
        ("fatura", "Rechnung", new[] { "Werbung", "Garantie", "Vertrag" }),
        ("tüketici", "Verbraucher", new[] { "Verkäufer", "Hersteller", "Lieferant" }),
        ("bütçe", "Budget/Haushaltsplan", new[] { "Sparbuch", "Gehalt", "Kredit" }),
        ("taksit", "Ratenzahlung", new[] { "Barzahlung", "Rabatt", "Steuer" }),
        ("marka", "Marke", new[] { "Produkt allgemein", "Preis", "Werbung" }),
        ("reklam", "Werbung", new[] { "Nachricht", "Zeitung", "Brief" }),
        ("iade etmek", "zurückgeben", new[] { "kaufen", "verkaufen", "bestellen" }),
        ("garanti", "Garantie", new[] { "Rechnung", "Rabatt", "Vertrag" }),
        ("çevrimiçi alışveriş", "Online-Einkauf", new[] { "Ladenbesuch", "Straßenmarkt", "Tauschhandel" }),
        ("kargo", "Lieferung/Versand", new[] { "Geschenk", "Einkaufstüte", "Rechnung" }),
        ("tasarruf etmek", "sparen", new[] { "ausgeben", "verschenken", "verlieren" }),
        ("geleneksel yemek", "traditionelles Gericht", new[] { "modernes Gericht", "Fastfood", "Süßigkeit" }),
        ("bayram", "Fest (religiös/national)", new[] { "gewöhnliches Wochenende", "Ferien allgemein", "Geburtstag" }),
        ("çarşı", "Markt/Basar", new[] { "modernes Einkaufszentrum", "Supermarkt", "Fabrik" }),
        ("nakit", "Bargeld", new[] { "Kreditkarte", "Scheck", "Kryptowährung" }),
        ("fiyat karşılaştırmak", "Preise vergleichen", new[] { "Preise erhöhen", "Preise verstecken", "Preise festlegen" }),
        ("tüketici hakları", "Verbraucherrechte", new[] { "Herstellerpflichten", "Steuerpflichten", "Handelsgesetze" }),
        ("israf", "Verschwendung", new[] { "Sparsamkeit", "Großzügigkeit", "Ordnung" })
    };

    private static QuizQuestion AlltagUndKonsum(Random r)
    {
        var d = AlltagKonsumListe[r.Next(AlltagKonsumListe.Length)];
        var optionen = new[] { d.Almanca }.Concat(d.Yanlislar).OrderBy(_ => r.Next()).ToArray();

        return new QuizQuestion
        {
            Id = NewId(), Subject = Subject.Tuerkisch, GradeLevel = GradeLevel.Klasse9,
            Topic = "Alltag, Konsum und türkische Kultur – Wortschatz", Type = QuestionType.MultipleChoice,
            Prompt = $"\"{d.TurkceKelime}\" kelimesinin Almancası hangisidir?",
            Options = optionen, CorrectAnswers = new[] { d.Almanca },
            Explanation = $"\"{d.TurkceKelime}\" Almanca \"{d.Almanca}\" demektir.",
            HelpHint = "Alışveriş ve tüketimle ilgili kelimeler: indirim, fatura, tüketici, tasarruf etmek, tüketici hakları."
        };
    }

    private static readonly (string TurkceKelime, string Almanca, string[] Yanlislar)[] ToplumsalYasamListe =
    {
        ("toplum", "Gesellschaft", new[] { "nur Familie", "Staat allgemein", "Verein" }),
        ("vatandaş", "Bürger/Staatsbürger", new[] { "Ausländer", "Tourist", "Gast" }),
        ("sivil toplum kuruluşu", "Nichtregierungsorganisation (NGO)", new[] { "Staatsbehörde", "Firma", "Partei" }),
        ("gönüllü çalışmak", "ehrenamtlich arbeiten", new[] { "bezahlt arbeiten", "studieren", "Urlaub machen" }),
        ("eşitlik", "Gleichheit", new[] { "Ungleichheit", "Wettbewerb", "Konkurrenz" }),
        ("ayrımcılık", "Diskriminierung", new[] { "Gleichbehandlung", "Zusammenarbeit", "Freundschaft" }),
        ("kamuoyu", "öffentliche Meinung", new[] { "private Meinung", "Regierungsmeinung", "Expertenmeinung" }),
        ("sorumluluk", "Verantwortung", new[] { "Freizeit", "Erlaubnis", "Zufall" }),
        ("dayanışma", "Solidarität", new[] { "Konkurrenz", "Gleichgültigkeit", "Distanz" }),
        ("toplumsal cinsiyet", "soziales Geschlecht (Gender)", new[] { "nur biologisches Geschlecht", "Alter", "Herkunft" }),
        ("hoşgörü", "Toleranz", new[] { "Intoleranz", "Gleichgültigkeit", "Misstrauen" }),
        ("katılım", "Teilnahme/Beteiligung", new[] { "Ablehnung", "Ausschluss", "Isolation" }),
        ("yerel yönetim", "Kommunalverwaltung", new[] { "Bundesregierung", "Weltregierung", "Firmenleitung" }),
        ("sosyal medya", "soziale Medien", new[] { "nur Zeitung", "nur Fernsehen", "nur Radio" }),
        ("kamu hizmeti", "öffentlicher Dienst", new[] { "Privatunternehmen", "nur Ehrenamt", "nur Militärdienst" }),
        ("göçmen", "Einwanderer/Migrant", new[] { "Tourist", "Einheimischer", "Botschafter" }),
        ("entegrasyon", "Integration", new[] { "Ausgrenzung", "Trennung", "Isolation" }),
        ("kültürel çeşitlilik", "kulturelle Vielfalt", new[] { "kulturelle Einheitlichkeit", "kulturelle Isolation", "kulturelle Überlegenheit" }),
        ("sosyal adalet", "soziale Gerechtigkeit", new[] { "soziale Ungleichheit", "wirtschaftliches Wachstum", "politische Macht" }),
        ("demokratik katılım", "demokratische Teilhabe", new[] { "autoritäre Herrschaft", "Monarchie", "Diktatur" })
    };

    private static QuizQuestion GesellschaftUndOeffentlichesLeben(Random r)
    {
        var d = ToplumsalYasamListe[r.Next(ToplumsalYasamListe.Length)];
        var optionen = new[] { d.Almanca }.Concat(d.Yanlislar).OrderBy(_ => r.Next()).ToArray();

        return new QuizQuestion
        {
            Id = NewId(), Subject = Subject.Tuerkisch, GradeLevel = GradeLevel.Klasse9,
            Topic = "Gesellschaft und öffentliches Leben (Klasse-9-Niveau) – Wortschatz", Type = QuestionType.MultipleChoice,
            Prompt = $"\"{d.TurkceKelime}\" kelimesinin Almancası hangisidir?",
            Options = optionen, CorrectAnswers = new[] { d.Almanca },
            Explanation = $"\"{d.TurkceKelime}\" Almanca \"{d.Almanca}\" demektir.",
            HelpHint = "Toplum ve kamusal yaşamla ilgili kelimeler: vatandaş, eşitlik, hoşgörü, entegrasyon, sosyal adalet."
        };
    }

    private static readonly (string TurkceKelime, string Almanca, string[] Yanlislar)[] BerufsweltListe =
    {
        ("meslek", "Beruf", new[] { "Hobby", "Schulfach", "Freizeit" }),
        ("staj", "Praktikum", new[] { "Urlaub", "Prüfung", "Ferienjob" }),
        ("iş başvurusu", "Bewerbung", new[] { "Arbeitsvertrag", "Kündigung", "Gehaltsabrechnung" }),
        ("özgeçmiş", "Lebenslauf", new[] { "Anschreiben", "Zeugnis", "Arbeitsvertrag" }),
        ("iş görüşmesi", "Vorstellungsgespräch", new[] { "Elternabend", "Prüfungsgespräch", "Beratungsgespräch" }),
        ("maaş", "Gehalt", new[] { "Urlaubsgeld", "Rente", "Stipendium" }),
        ("işveren", "Arbeitgeber", new[] { "Arbeitnehmer", "Arbeitsamt", "Gewerkschaft" }),
        ("işçi", "Arbeiter/Angestellter", new[] { "Arbeitgeber", "Chef", "Kunde" }),
        ("yetenek", "Fähigkeit/Talent", new[] { "Schwäche", "Fehler", "Note" }),
        ("meslek okulu", "Berufsschule", new[] { "Universität", "Grundschule", "Kindergarten" }),
        ("iş tecrübesi", "Berufserfahrung", new[] { "Schulzeugnis", "Freizeitaktivität", "Urlaubserfahrung" }),
        ("çıraklık", "Lehre/Ausbildung", new[] { "Studium", "Ferienjob", "Urlaub" }),
        ("kariyer", "Karriere/Laufbahn", new[] { "Hobby", "Freizeit", "Urlaub" }),
        ("işe alınmak", "eingestellt werden", new[] { "entlassen werden", "befördert werden", "gekündigt werden" }),
        ("işten çıkarılmak", "entlassen werden", new[] { "eingestellt werden", "befördert werden", "in Rente gehen" }),
        ("açık iş pozisyonu", "offene Stelle", new[] { "besetzte Stelle", "Ausbildungsplatz", "Praktikumsplatz" }),
        ("yarı zamanlı çalışmak", "Teilzeit arbeiten", new[] { "Vollzeit arbeiten", "gar nicht arbeiten", "ehrenamtlich arbeiten" }),
        ("mesleki eğitim", "Berufsausbildung", new[] { "Freizeitkurs", "Sprachkurs", "Musikunterricht" }),
        ("hedef meslek", "Wunschberuf/Zielberuf", new[] { "aktueller Beruf", "früherer Beruf", "Nebenjob" }),
        ("iş piyasası", "Arbeitsmarkt", new[] { "Wohnungsmarkt", "Aktienmarkt", "Lebensmittelmarkt" })
    };

    private static QuizQuestion SchuleUndBerufswelt(Random r)
    {
        var d = BerufsweltListe[r.Next(BerufsweltListe.Length)];
        var optionen = new[] { d.Almanca }.Concat(d.Yanlislar).OrderBy(_ => r.Next()).ToArray();

        return new QuizQuestion
        {
            Id = NewId(), Subject = Subject.Tuerkisch, GradeLevel = GradeLevel.Klasse9,
            Topic = "Schule, Ausbildung und Berufswelt – Wortschatz", Type = QuestionType.MultipleChoice,
            Prompt = $"\"{d.TurkceKelime}\" kelimesinin Almancası hangisidir?",
            Options = optionen, CorrectAnswers = new[] { d.Almanca },
            Explanation = $"\"{d.TurkceKelime}\" Almanca \"{d.Almanca}\" demektir.",
            HelpHint = "Meslek ve iş dünyasıyla ilgili kelimeler: iş başvurusu, özgeçmiş, iş görüşmesi, maaş, iş tecrübesi."
        };
    }

    // ----- Klasse 7 -----

    private static readonly (string Fiil, string Hikaye)[] SimdikiHikayeBeispiele =
    {
        ("gelmek", "geliyordu"), ("gitmek", "gidiyordu"), ("okumak", "okuyordu"),
        ("yazmak", "yazıyordu"), ("oynamak", "oynuyordu"), ("koşmak", "koşuyordu"),
        ("içmek", "içiyordu"), ("görmek", "görüyordu"), ("bilmek", "biliyordu"),
        ("sevmek", "seviyordu"), ("gülmek", "gülüyordu"), ("ağlamak", "ağlıyordu"),
        ("uyumak", "uyuyordu"), ("konuşmak", "konuşuyordu"), ("düşünmek", "düşünüyordu"),
        ("beklemek", "bekliyordu"), ("çalışmak", "çalışıyordu"), ("dinlemek", "dinliyordu"),
        ("anlamak", "anlıyordu"), ("yemek", "yiyordu")
    };

    private static QuizQuestion SimdikiZamaninHikayesi(Random r)
    {
        var v = SimdikiHikayeBeispiele[r.Next(SimdikiHikayeBeispiele.Length)];

        return new QuizQuestion
        {
            Id = NewId(), Subject = Subject.Tuerkisch, GradeLevel = GradeLevel.Klasse7,
            Topic = "Şimdiki Zamanın Hikâyesi (-yordu)", Type = QuestionType.OpenText,
            Prompt = $"\"{v.Fiil}\" fiilinin (o için) şimdiki zamanın hikâyesi hâlini yaz. (Beispiel: gelmek -> geliyordu)",
            CorrectAnswers = new[] { v.Hikaye },
            Explanation = $"\"{v.Fiil}\" -> \"{v.Hikaye}\". Şimdiki zamanın hikâyesi (-yordu), geçmişte sürmekte olan bir işi anlatır - " +
                          "Almanca karşılığı çoğu zaman Präteritum ya da \"war gerade dabei\" anlamıdır.",
            HelpHint = "Şimdiki zamanın hikâyesi \"-yor\" ekinin üzerine \"-du\" getirilerek kurulur (geliyor + du = geliyordu)."
        };
    }

    private static readonly (string Fiil, string Mis)[] BelirsizGecmisBeispiele =
    {
        ("gelmek", "gelmiş"), ("almak", "almış"), ("görmek", "görmüş"),
        ("okumak", "okumuş"), ("yazmak", "yazmış"), ("gitmek", "gitmiş"),
        ("içmek", "içmiş"), ("bilmek", "bilmiş"), ("sevmek", "sevmiş"),
        ("gülmek", "gülmüş"), ("ağlamak", "ağlamış"), ("uyumak", "uyumuş"),
        ("konuşmak", "konuşmuş"), ("düşünmek", "düşünmüş"), ("beklemek", "beklemiş"),
        ("çalışmak", "çalışmış"), ("dinlemek", "dinlemiş"), ("anlamak", "anlamış"),
        ("yemek", "yemiş"), ("oynamak", "oynamış")
    };

    private static QuizQuestion BelirsizGecmisZaman(Random r)
    {
        var v = BelirsizGecmisBeispiele[r.Next(BelirsizGecmisBeispiele.Length)];

        return new QuizQuestion
        {
            Id = NewId(), Subject = Subject.Tuerkisch, GradeLevel = GradeLevel.Klasse7,
            Topic = "Belirsiz Geçmiş Zaman (-miş'li geçmiş)", Type = QuestionType.OpenText,
            Prompt = $"\"{v.Fiil}\" fiilinin (o için) -miş'li geçmiş zaman hâlini yaz.",
            CorrectAnswers = new[] { v.Mis },
            Explanation = $"\"{v.Fiil}\" -> \"{v.Mis}\". -miş'li geçmiş zaman, duyulan ya da sonradan fark edilen geçmişi anlatır " +
                          "(başkasından duyduğumuz olaylar, masallar).",
            HelpHint = "-miş'li geçmiş zaman eki (-miş/-mış/-muş/-müş) ünlü uyumuna göre değişir ve görülmeyen/duyulan geçmişi anlatır - masallar hep bu zamanla anlatılır."
        };
    }

    private static readonly (string Frage, string[] Optionen, string Antwort, string Erklaerung)[] DeyimAtasozuListe =
    {
        ("\"Göz kulak olmak\" deyimi ne anlama gelir?", new[] { "Birine ya da bir şeye dikkat etmek, korumak", "Gözlük takmak", "Yüksek sesle şarkı söylemek" }, "Birine ya da bir şeye dikkat etmek, korumak",
            "\"Göz kulak olmak\" = auf jemanden/etwas aufpassen."),
        ("\"Ağzı kulaklarına varmak\" deyimi ne anlama gelir?", new[] { "Çok sevinmek", "Çok yemek yemek", "Yüksek sesle bağırmak" }, "Çok sevinmek",
            "\"Ağzı kulaklarına varmak\" = übers ganze Gesicht strahlen, sich sehr freuen."),
        ("\"Burnu havada olmak\" deyimi ne anlama gelir?", new[] { "Kibirli olmak, kendini beğenmek", "Nezle olmak", "Uçakla seyahat etmek" }, "Kibirli olmak, kendini beğenmek",
            "\"Burnu havada olmak\" = hochnäsig/eingebildet sein."),
        ("\"Kulak misafiri olmak\" deyimi ne anlama gelir?", new[] { "Bir konuşmayı istemeden dinlemek", "Misafirliğe gitmek", "Kulaklık takmak" }, "Bir konuşmayı istemeden dinlemek",
            "\"Kulak misafiri olmak\" = zufällig mithören."),
        ("\"İki gözü iki çeşme\" deyimi ne anlama gelir?", new[] { "Çok ağlamak", "Çok iyi görmek", "Su içmek istemek" }, "Çok ağlamak",
            "\"İki gözü iki çeşme (ağlamak)\" = bitterlich weinen."),
        ("\"Kolları sıvamak\" deyimi ne anlama gelir?", new[] { "Bir işe hazırlanıp başlamak", "Kıyafet ütülemek", "Spor yapmak" }, "Bir işe hazırlanıp başlamak",
            "\"Kolları sıvamak\" = die Ärmel hochkrempeln, sich an die Arbeit machen."),
        ("\"Kafa yormak\" deyimi ne anlama gelir?", new[] { "Bir konu üzerinde çok düşünmek", "Başı ağrımak", "Uyuyakalmak" }, "Bir konu üzerinde çok düşünmek",
            "\"Kafa yormak\" = sich über etwas den Kopf zerbrechen."),
        ("\"Etekleri zil çalmak\" deyimi ne anlama gelir?", new[] { "Çok sevinçli olmak", "Müzik aleti çalmak", "Yeni kıyafet almak" }, "Çok sevinçli olmak",
            "\"Etekleri zil çalmak\" = vor Freude strahlen."),
        ("\"Gözden düşmek\" deyimi ne anlama gelir?", new[] { "Değerini, itibarını kaybetmek", "Merdivenden düşmek", "Gözlüğünü kaybetmek" }, "Değerini, itibarını kaybetmek",
            "\"Gözden düşmek\" = an Ansehen verlieren, in Ungnade fallen."),
        ("\"Pire için yorgan yakmak\" deyimi ne anlama gelir?", new[] { "Küçük bir sorun yüzünden büyük zarara yol açmak", "Kamp ateşi yakmak", "Evi temizlemek" }, "Küçük bir sorun yüzünden büyük zarara yol açmak",
            "\"Pire için yorgan yakmak\" = wegen einer Kleinigkeit großen Schaden anrichten."),
        ("\"Damlaya damlaya göl olur\" atasözü ne anlatır?", new[] { "Küçük birikimler zamanla büyük değer oluşturur", "Yağmurlu havalarda dışarı çıkılmaz", "Göller damlalardan oluşmaz" }, "Küçük birikimler zamanla büyük değer oluşturur",
            "\"Damlaya damlaya göl olur\" = Kleinvieh macht auch Mist - kleine Ersparnisse summieren sich."),
        ("\"Ağaç yaşken eğilir\" atasözü ne anlatır?", new[] { "Eğitim küçük yaşta verilmelidir", "Ağaçlar rüzgarda eğilir", "Yaşlı ağaçlar daha değerlidir" }, "Eğitim küçük yaşta verilmelidir",
            "\"Ağaç yaşken eğilir\" = Was Hänschen nicht lernt, lernt Hans nimmermehr."),
        ("\"Bir elin nesi var, iki elin sesi var\" atasözü ne anlatır?", new[] { "Birlikte çalışmak tek başına çalışmaktan iyidir", "Alkışlamak kibarlıktır", "İki el bir elden hızlıdır" }, "Birlikte çalışmak tek başına çalışmaktan iyidir",
            "Bu atasözü iş birliğinin ve dayanışmanın gücünü anlatır."),
        ("\"Sakla samanı, gelir zamanı\" atasözü ne anlatır?", new[] { "Bugün gereksiz görünen şey ileride gerekli olabilir", "Saman hayvanlar için önemlidir", "Eski eşyalar çöpe atılmalıdır" }, "Bugün gereksiz görünen şey ileride gerekli olabilir",
            "Bu atasözü tutumlu olmayı ve ileriyi düşünmeyi öğütler."),
        ("\"Dost kara günde belli olur\" atasözü ne anlatır?", new[] { "Gerçek dostluk zor zamanlarda anlaşılır", "Dostlar her gün görüşmelidir", "Karanlıkta dost seçilmez" }, "Gerçek dostluk zor zamanlarda anlaşılır",
            "\"Dost kara günde belli olur\" = Freunde erkennt man in der Not."),
        ("\"Vakit nakittir\" atasözü ne anlatır?", new[] { "Zaman çok değerlidir, boşa harcanmamalıdır", "Para biriktirmek zordur", "Saat almak gereklidir" }, "Zaman çok değerlidir, boşa harcanmamalıdır",
            "\"Vakit nakittir\" = Zeit ist Geld."),
        ("\"Ayağını yorganına göre uzat\" atasözü ne anlatır?", new[] { "İmkânlarına göre yaşamak gerekir", "Uyurken düzgün yatmak gerekir", "Büyük yorgan almak gerekir" }, "İmkânlarına göre yaşamak gerekir",
            "Bu atasözü harcamalarını gelirine göre ayarlamayı öğütler."),
        ("\"Ne ekersen onu biçersin\" atasözü ne anlatır?", new[] { "Yaptıklarının karşılığını görürsün", "Çiftçilik zor bir meslektir", "Her tohum aynı ürünü verir" }, "Yaptıklarının karşılığını görürsün",
            "\"Ne ekersen onu biçersin\" = Wie man sät, so erntet man."),
        ("\"Akıl akıldan üstündür\" atasözü ne anlatır?", new[] { "Başkalarına danışmak her zaman faydalıdır", "Bazı insanlar hiç düşünmez", "Zeki insanlar yalnız çalışır" }, "Başkalarına danışmak her zaman faydalıdır",
            "Bu atasözü danışmanın ve farklı görüşler almanın değerini anlatır."),
        ("\"Taşıma suyla değirmen dönmez\" atasözü ne anlatır?", new[] { "Bir iş dışarıdan gelen desteklerle uzun süre yürümez", "Değirmenler artık kullanılmıyor", "Su taşımak yorucudur" }, "Bir iş dışarıdan gelen desteklerle uzun süre yürümez",
            "Bu atasözü kalıcı işlerin kendi kaynaklarıyla yürümesi gerektiğini anlatır.")
    };

    private static QuizQuestion DeyimlerVeAtasozleri(Random r)
    {
        var f = DeyimAtasozuListe[r.Next(DeyimAtasozuListe.Length)];
        return new QuizQuestion
        {
            Id = NewId(), Subject = Subject.Tuerkisch, GradeLevel = GradeLevel.Klasse7,
            Topic = "Deyimler ve Atasözleri", Type = QuestionType.MultipleChoice,
            Prompt = f.Frage, Options = f.Optionen, CorrectAnswers = new[] { f.Antwort }, Explanation = f.Erklaerung,
            HelpHint = "Deyimler mecazlı kalıp sözlerdir (göz kulak olmak), atasözleri ise öğüt veren eski sözlerdir (damlaya damlaya göl olur)."
        };
    }

    private static readonly (string Frage, string[] Optionen, string Antwort, string Erklaerung)[] NoktalamaListe =
    {
        ("Soru cümlelerinin sonuna hangi işaret konur?", new[] { "Soru işareti (?)", "Ünlem işareti (!)", "Nokta (.)" }, "Soru işareti (?)",
            "Soru bildiren cümlelerin sonuna soru işareti konur: \"Okula geldin mi?\""),
        ("Sevinç, korku ya da şaşkınlık bildiren cümlelerin sonuna hangi işaret konur?", new[] { "Ünlem işareti (!)", "Virgül (,)", "İki nokta (:)" }, "Ünlem işareti (!)",
            "Güçlü duygu bildiren cümleler ünlem işaretiyle biter: \"Ne güzel bir gün!\""),
        ("Tamamlanmış bir cümlenin sonuna hangi işaret konur?", new[] { "Nokta (.)", "Üç nokta (...)", "Noktalı virgül (;)" }, "Nokta (.)",
            "Anlamca tamamlanmış cümleler nokta ile biter."),
        ("Eş görevli kelimeleri ayırmak için hangi işaret kullanılır?", new[] { "Virgül (,)", "Kesme işareti (')", "Tırnak işareti (\" \")" }, "Virgül (,)",
            "Sıralanan eş görevli kelimeler virgülle ayrılır: \"Elma, armut ve kiraz aldım.\""),
        ("Açıklama ya da örnek vermeden önce hangi işaret kullanılır?", new[] { "İki nokta (:)", "Soru işareti (?)", "Ünlem işareti (!)" }, "İki nokta (:)",
            "Açıklama veya örneklerden önce iki nokta konur: \"Şunları al: defter, kalem, silgi.\""),
        ("Başkasından alınan sözler hangi işaret içinde gösterilir?", new[] { "Tırnak işareti (\" \")", "Virgül (,)", "Nokta (.)" }, "Tırnak işareti (\" \")",
            "Alıntı sözler tırnak içinde yazılır: Öğretmen \"Yarın sınav var.\" dedi."),
        ("Özel adlara gelen ekleri ayırmak için hangi işaret kullanılır?", new[] { "Kesme işareti (')", "Virgül (,)", "İki nokta (:)" }, "Kesme işareti (')",
            "Özel adlara gelen çekim ekleri kesme işaretiyle ayrılır: \"Berlin'de\", \"Ali'nin\"."),
        ("Tamamlanmamış, yarım bırakılan cümlelerin sonuna hangi işaret konur?", new[] { "Üç nokta (...)", "Nokta (.)", "Soru işareti (?)" }, "Üç nokta (...)",
            "Yarım bırakılan ifadelerin sonunda üç nokta bulunur: \"Keşke o gün...\""),
        ("\"Berlin_de yaşıyorum.\" cümlesinde boşluğa hangisi gelmelidir?", new[] { "Kesme işareti: Berlin'de", "Virgül: Berlin,de", "Hiçbir işaret gelmez: Berlinde" }, "Kesme işareti: Berlin'de",
            "Berlin özel ad olduğu için ek, kesme işaretiyle ayrılır: \"Berlin'de\"."),
        ("\"Yarın sınav var mı_\" cümlesinin sonuna hangi işaret gelmelidir?", new[] { "Soru işareti (?)", "Nokta (.)", "Ünlem işareti (!)" }, "Soru işareti (?)",
            "\"mı/mi\" soru eki cümleyi soru yapar - sonuna soru işareti konur."),
        ("\"Çantama defter_ kalem ve silgi koydum.\" cümlesinde boşluğa hangisi gelmelidir?", new[] { "Virgül (,)", "Nokta (.)", "İki nokta (:)" }, "Virgül (,)",
            "Sıralanan eş görevli kelimeler (defter, kalem, silgi) virgülle ayrılır."),
        ("\"İmdat_\" cümlesinin sonuna hangi işaret gelmelidir?", new[] { "Ünlem işareti (!)", "Soru işareti (?)", "Noktalı virgül (;)" }, "Ünlem işareti (!)",
            "Seslenme ve yardım çağrıları ünlemle biter: \"İmdat!\""),
        ("Konuşma metinlerinde satır başındaki konuşmaları göstermek için hangi işaret kullanılır?", new[] { "Konuşma çizgisi (-)", "Üç nokta (...)", "Kesme işareti (')" }, "Konuşma çizgisi (-)",
            "Karşılıklı konuşmalarda satır başına konuşma çizgisi konur."),
        ("\"Ali_nin çantası mavi.\" cümlesinde boşluğa hangisi gelmelidir?", new[] { "Kesme işareti: Ali'nin", "Virgül: Ali,nin", "İki nokta: Ali:nin" }, "Kesme işareti: Ali'nin",
            "Özel ad olan \"Ali\"ye gelen ek kesme işaretiyle ayrılır."),
        ("Cümle içinde arasöz ya da ek açıklama hangi işaretlerle gösterilebilir?", new[] { "Parantez ( ) ya da iki virgül arasında", "İki soru işareti arasında", "İki nokta üst üste arasında" }, "Parantez ( ) ya da iki virgül arasında",
            "Ek açıklamalar parantez içinde ya da iki virgül arasında verilir."),
        ("Tarihlerin gün, ay ve yıl bölümleri arasında hangi işaret kullanılır?", new[] { "Nokta (.)", "Virgül (,)", "Noktalı virgül (;)" }, "Nokta (.)",
            "Tarihler nokta ile yazılır: 23.04.1920."),
        ("Sıra bildiren sayılardan sonra hangi işaret konur?", new[] { "Nokta (.)", "Ünlem işareti (!)", "Tırnak işareti (\" \")" }, "Nokta (.)",
            "Sıra sayılarından sonra nokta konur: \"3. kat\" (üçüncü kat demektir)."),
        ("Virgülle ayrılmış örnekleri farklı gruplara ayırmak için hangi işaret kullanılır?", new[] { "Noktalı virgül (;)", "Ünlem işareti (!)", "Kesme işareti (')" }, "Noktalı virgül (;)",
            "Gruplar noktalı virgülle ayrılır: \"Elma, armut; ıspanak, pırasa aldım.\""),
        ("Kısaltmalardan sonra genellikle hangi işaret kullanılır?", new[] { "Nokta (.)", "Soru işareti (?)", "Üç nokta (...)" }, "Nokta (.)",
            "Çoğu kısaltmadan sonra nokta konur: \"Dr.\", \"Prof.\", \"vb.\""),
        ("\"Öğretmen şunları söyledi_ Yarın gezi var.\" cümlesinde boşluğa hangisi gelmelidir?", new[] { "İki nokta (:)", "Virgül (,)", "Soru işareti (?)" }, "İki nokta (:)",
            "Aktarılacak sözden önce iki nokta konur: \"Öğretmen şunları söyledi: Yarın gezi var.\"")
    };

    private static QuizQuestion NoktalamaIsaretleri(Random r)
    {
        var f = NoktalamaListe[r.Next(NoktalamaListe.Length)];
        return new QuizQuestion
        {
            Id = NewId(), Subject = Subject.Tuerkisch, GradeLevel = GradeLevel.Klasse7,
            Topic = "Noktalama İşaretleri", Type = QuestionType.MultipleChoice,
            Prompt = f.Frage, Options = f.Optionen, CorrectAnswers = new[] { f.Antwort }, Explanation = f.Erklaerung,
            HelpHint = "Nokta = cümle sonu, soru işareti = soru, ünlem = güçlü duygu, virgül = sıralama, kesme işareti = özel ada gelen ek (Berlin'de), iki nokta = açıklama öncesi."
        };
    }

    private static readonly (string Frage, string[] Optionen, string Antwort, string Erklaerung)[] MetinTurleriListe =
    {
        ("Olağanüstü olayların ve kahramanların anlatıldığı, \"bir varmış bir yokmuş\" diye başlayan metin türü hangisidir?", new[] { "Masal", "Haber yazısı", "Günlük" }, "Masal",
            "Masallar olağanüstü olayları anlatır ve -miş'li geçmiş zamanla kurulur."),
        ("Kahramanları genellikle hayvanlar olan ve ders veren kısa metin türü hangisidir?", new[] { "Fabl", "Biyografi", "Anı" }, "Fabl",
            "Fabllarda konuşan hayvanlar üzerinden ahlaki bir ders verilir (La Fontaine, Ezop)."),
        ("Bir kişinin hayatını başka birinin anlattığı metin türü hangisidir?", new[] { "Biyografi", "Otobiyografi", "Masal" }, "Biyografi",
            "Biyografi bir kişinin hayatını BAŞKASININ kaleminden anlatır."),
        ("Bir kişinin KENDİ hayatını anlattığı metin türü hangisidir?", new[] { "Otobiyografi", "Biyografi", "Fabl" }, "Otobiyografi",
            "Otobiyografide yazar kendi hayatını anlatır."),
        ("Günü gününe yazılan, tarih atılan kişisel metin türü hangisidir?", new[] { "Günlük", "Haber yazısı", "Deneme" }, "Günlük",
            "Günlük (Tagebuch), yaşananların günü gününe, tarih atılarak yazılmasıdır."),
        ("Yaşanmış olayların üzerinden zaman geçtikten sonra anlatıldığı metin türü hangisidir?", new[] { "Anı (Hatıra)", "Günlük", "Masal" }, "Anı (Hatıra)",
            "Anı, geçmişte yaşananların sonradan hatırlanarak yazılmasıdır - günlükten farkı budur."),
        ("Güncel olayları okuyucuya nesnel biçimde aktaran metin türü hangisidir?", new[] { "Haber yazısı", "Şiir", "Fabl" }, "Haber yazısı",
            "Haber yazısı 5N1K sorularına (ne, nerede, ne zaman, nasıl, neden, kim) cevap verir."),
        ("Duygu ve düşüncelerin dizeler hâlinde, ahenkli biçimde anlatıldığı tür hangisidir?", new[] { "Şiir", "Roman", "Haber yazısı" }, "Şiir",
            "Şiir dizelerden oluşur; ölçü, uyak ve ahenk önemlidir."),
        ("Sahnede oynanmak için yazılan, karşılıklı konuşmalara dayanan tür hangisidir?", new[] { "Tiyatro", "Günlük", "Biyografi" }, "Tiyatro",
            "Tiyatro metinleri sahnelenmek için yazılır ve diyaloglardan oluşur."),
        ("Uzun, geniş kadrolu ve ayrıntılı olay örgüsüne sahip kurmaca tür hangisidir?", new[] { "Roman", "Kısa hikâye", "Haber yazısı" }, "Roman",
            "Roman uzun soluklu bir kurmaca türüdür; çok sayıda kişi ve olay barındırır."),
        ("Yazarın bir konudaki kişisel görüşlerini kanıtlama kaygısı olmadan anlattığı tür hangisidir?", new[] { "Deneme", "Haber yazısı", "Masal" }, "Deneme",
            "Denemede yazar düşüncelerini serbestçe, sohbet havasında anlatır."),
        ("Bir milletin kahramanlıklarını anlatan çok eski, uzun manzum metin türü hangisidir?", new[] { "Destan", "Günlük", "Deneme" }, "Destan",
            "Destanlar (Ergenekon, Oğuz Kağan) milletlerin kahramanlık öykülerini anlatır."),
        ("Halk arasında anlatılan, gerçek olduğuna inanılan olağanüstü öyküler hangi türe girer?", new[] { "Efsane", "Biyografi", "Haber yazısı" }, "Efsane",
            "Efsaneler gerçek olduğuna inanılan, kuşaktan kuşağa aktarılan anlatılardır."),
        ("Kısa, yoğun ve tek bir olay çevresinde gelişen kurmaca tür hangisidir?", new[] { "Hikâye (öykü)", "Roman", "Destan" }, "Hikâye (öykü)",
            "Hikâye romandan kısadır; az kişi, tek olay ve dar zaman vardır."),
        ("Birine duygu, düşünce ve haber iletmek için yazılan metin türü hangisidir?", new[] { "Mektup", "Fabl", "Destan" }, "Mektup",
            "Mektup, uzaktaki birine hitap ederek yazılan kişisel bir metindir."),
        ("Masallar hangi zaman kipiyle anlatılır?", new[] { "-miş'li geçmiş zaman", "Şimdiki zaman", "Gelecek zaman" }, "-miş'li geçmiş zaman",
            "Masallar duyulan geçmiş zamanla anlatılır: \"Bir varmış, bir yokmuş...\""),
        ("Haber yazısının cevap vermesi beklenen sorular hangileridir?", new[] { "5N1K (ne, nerede, ne zaman, nasıl, neden, kim)", "Sadece \"kim?\"", "Sadece \"neden?\"" }, "5N1K (ne, nerede, ne zaman, nasıl, neden, kim)",
            "İyi bir haber 5N1K sorularının hepsine cevap verir."),
        ("Şiirde dize sonlarındaki ses benzerliğine ne denir?", new[] { "Uyak (kafiye)", "Paragraf", "Özet" }, "Uyak (kafiye)",
            "Uyak (kafiye), dize sonlarındaki ses benzerliğidir ve şiire ahenk katar."),
        ("Bir metnin türünü belirlerken öncelikle neye bakılır?", new[] { "Metnin amacına, biçimine ve anlatım özelliklerine", "Sadece metnin uzunluğuna", "Sadece yazarın adına" }, "Metnin amacına, biçimine ve anlatım özelliklerine",
            "Tür belirlenirken amaç (bilgilendirme/duygulandırma), biçim (dize/düzyazı) ve anlatım incelenir."),
        ("Fabl ile masal arasındaki en önemli fark nedir?", new[] { "Fablda kahramanlar hayvanlardır ve açık bir ders vardır", "Masallar her zaman gerçektir", "Fabllar çok uzundur" }, "Fablda kahramanlar hayvanlardır ve açık bir ders vardır",
            "Fablın kahramanları insan gibi davranan hayvanlardır ve sonunda ders (kıssadan hisse) verilir.")
    };

    private static QuizQuestion MetinTurleri(Random r)
    {
        var f = MetinTurleriListe[r.Next(MetinTurleriListe.Length)];
        return new QuizQuestion
        {
            Id = NewId(), Subject = Subject.Tuerkisch, GradeLevel = GradeLevel.Klasse7,
            Topic = "Metin Türleri", Type = QuestionType.MultipleChoice,
            Prompt = f.Frage, Options = f.Optionen, CorrectAnswers = new[] { f.Antwort }, Explanation = f.Erklaerung,
            HelpHint = "Masal = olağanüstü + -miş'li geçmiş, fabl = hayvanlar + ders, günlük = günü gününe, anı = sonradan, haber = 5N1K, şiir = dize/uyak."
        };
    }

    private static readonly (string TurkceKelime, string Almanca, string[] Yanlislar)[] MedyaIletisimListe =
    {
        ("gazete", "Zeitung", new[] { "Buch", "Brief", "Heft" }),
        ("dergi", "Zeitschrift", new[] { "Wörterbuch", "Plakat", "Rechnung" }),
        ("haber", "Nachricht", new[] { "Werbung", "Roman", "Gedicht" }),
        ("ekran", "Bildschirm", new[] { "Tastatur", "Drucker", "Lautsprecher" }),
        ("şifre", "Passwort", new[] { "Benutzername", "Adresse", "Unterschrift" }),
        ("kullanıcı", "Nutzer", new[] { "Verkäufer", "Nachbar", "Schüler" }),
        ("bağlantı", "Verbindung", new[] { "Trennung", "Rechnung", "Sendungssprecher" }),
        ("canlı yayın", "Live-Sendung", new[] { "Wiederholung", "Werbepause", "Aufzeichnung von gestern" }),
        ("reklam", "Werbung", new[] { "Nachrichtensendung", "Wettervorhersage", "Dokumentation" }),
        ("belgesel", "Dokumentarfilm", new[] { "Zeichentrickfilm", "Quizshow", "Seifenoper" }),
        ("manşet", "Schlagzeile", new[] { "Fußnote", "Inhaltsverzeichnis", "Impressum" }),
        ("muhabir", "Reporter", new[] { "Schauspieler", "Zuschauer", "Verleger" }),
        ("izleyici", "Zuschauer", new[] { "Moderator", "Kameramann", "Regisseur" }),
        ("okuyucu", "Leser", new[] { "Autor", "Drucker", "Verkäufer" }),
        ("yorum", "Kommentar", new[] { "Überschrift", "Seitenzahl", "Anzeige" }),
        ("paylaşmak", "teilen", new[] { "löschen", "drucken", "kaufen" }),
        ("indirmek", "herunterladen", new[] { "hochladen", "ausschalten", "verkaufen" }),
        ("yüklemek", "hochladen", new[] { "herunterladen", "abschreiben", "ausleihen" }),
        ("kaynak", "Quelle", new[] { "Meinung", "Gerücht", "Werbespot" }),
        ("sosyal medya", "soziale Medien", new[] { "Tageszeitung", "Radiosender", "Telefonbuch" })
    };

    private static QuizQuestion MedyaVeIletisim(Random r)
    {
        var d = MedyaIletisimListe[r.Next(MedyaIletisimListe.Length)];
        var optionen = new[] { d.Almanca }.Concat(d.Yanlislar).OrderBy(_ => r.Next()).ToArray();

        return new QuizQuestion
        {
            Id = NewId(), Subject = Subject.Tuerkisch, GradeLevel = GradeLevel.Klasse7,
            Topic = "Medya ve İletişim – Wortschatz", Type = QuestionType.MultipleChoice,
            Prompt = $"\"{d.TurkceKelime}\" kelimesinin Almancası hangisidir?",
            Options = optionen, CorrectAnswers = new[] { d.Almanca },
            Explanation = $"\"{d.TurkceKelime}\" Almanca \"{d.Almanca}\" demektir.",
            HelpHint = "Medya ve iletişim kelimeleri: gazete, haber, manşet, muhabir, kaynak, paylaşmak, indirmek/yüklemek."
        };
    }
}
