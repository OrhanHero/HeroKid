using LernTor.Core.Enums;
using LernTor.Core.Models;

namespace LernTor.ContentGen.Generators;

/// <summary>
/// Türkisch-Aufgabengenerator für bilinguale/herkunftssprachliche Lerner: Zeitformen, Wortschatz,
/// Ekler (Suffixe) für Klasse 6, birleşik zamanlar, Deyimler/Atasözleri, Noktalama und Metin
/// türleri für Klasse 7, sowie Satzglieder, Fiilimsi und Rechtschreibung für Klasse 9.
///
/// <para><b>Erweitert am 28.09.2026</b> um je fünf Themen für Klasse 6 (Çoğul eki, Hâl ekleri,
/// Soru eki, Sayılar/Günler/Aylar, Kısa metin anlama) und Klasse 9 (Söz sanatları, Ses
/// olayları, Sözcükte anlam, Cümle türleri, Türk edebiyatı). Grund: Türkisch ist bei der
/// Fächerauswahl nach Stundenplan jeden Tag dabei, und die alten Pools waren nach 5-7 Wochen
/// einmal durch (scripts/pool-reichweite.py).</para>
///
/// <para><b>Erweitert am 30.09.2026</b> um fünf Themen für Klasse 6 (İyelik ekleri, Geniş zaman,
/// Emir kipi, Vücut ve sağlık, Sıfatlarda karşılaştırma), sieben für Klasse 7 (Zarflar, Zamirler,
/// Şart kipi, Gereklilik kipi, Yapım ekleri/Birleşik kelimeler, Hikâye unsurları, Berlin'de günlük
/// yaşam) und fünf für Klasse 9 (Fiil çatısı, Ek fiil, Paragrafta anlam, Bağlaçlar/Edatlar,
/// Anlatım bozuklukları): Klasse 6 360, Klasse 7 260, Klasse 9 400 Fragen.</para>
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
                TurkiyeKulturu,
                CogulEki,
                HalEkleri,
                SoruEki,
                SayilarVeZaman,
                KisaMetinAnlama,
                IyelikEkleri,
                GenisZaman,
                EmirKipiVeRica,
                VucutVeSaglik,
                SifatlardaKarsilastirma
            },
            [GradeLevel.Klasse7] = new List<TopicFactory>
            {
                SimdikiZamaninHikayesi,
                BelirsizGecmisZaman,
                DeyimlerVeAtasozleri,
                NoktalamaIsaretleri,
                MetinTurleri,
                MedyaVeIletisim,
                ZarflarK7,
                ZamirlerK7,
                SartKipiK7,
                GereklilikKipiK7,
                YapimEkleriVeBirlesikKelimelerK7,
                HikayeUnsurlariK7,
                BerlindeGunlukYasamK7
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
                SchuleUndBerufswelt,
                SozSanatlari,
                SesOlaylari,
                SozcukteAnlam,
                CumleTurleri,
                TurkEdebiyati,
                FiilCatisi,
                EkFiil,
                ParagraftaAnlam,
                BaglaclarVeEdatlar,
                AnlatimBozukluklari
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

    // ================= Klasse 6: Erweiterung 28.09.2026 =================
    // Türkisch ist durch die Fächerauswahl nach Stundenplan JEDEN Tag dabei - der alte Pool
    // (160 Fragen) war rechnerisch nach gut fünf Wochen einmal durch (scripts/pool-reichweite.py).

    private static readonly (string Kelime, string Cogul)[] CogulEkiBeispiele =
    {
        ("kitap", "kitaplar"), ("ağaç", "ağaçlar"), ("ev", "evler"), ("göz", "gözler"),
        ("çocuk", "çocuklar"), ("öğretmen", "öğretmenler"), ("kuş", "kuşlar"), ("çiçek", "çiçekler"),
        ("masa", "masalar"), ("kalem", "kalemler"), ("okul", "okullar"), ("köprü", "köprüler"),
        ("arkadaş", "arkadaşlar"), ("gün", "günler"), ("yol", "yollar"), ("şehir", "şehirler"),
        ("bulut", "bulutlar"), ("kedi", "kediler"), ("top", "toplar"), ("soru", "sorular")
    };

    private static QuizQuestion CogulEki(Random r)
    {
        var v = CogulEkiBeispiele[r.Next(CogulEkiBeispiele.Length)];
        var kalin = v.Cogul.EndsWith("lar", StringComparison.Ordinal);

        return new QuizQuestion
        {
            Id = NewId(), Subject = Subject.Tuerkisch, GradeLevel = GradeLevel.Klasse6,
            Topic = "Çoğul Eki -ler/-lar (Plural)", Type = QuestionType.OpenText,
            Prompt = $"\"{v.Kelime}\" kelimesinin çoğul hâlini yaz. (Beispiel: kapı -> kapılar)",
            CorrectAnswers = new[] { v.Cogul },
            Explanation = kalin
                ? $"\"{v.Kelime}\" kelimesinin son ünlüsü kalındır (a, ı, o, u) - bu yüzden -lar gelir: {v.Cogul}."
                : $"\"{v.Kelime}\" kelimesinin son ünlüsü incedir (e, i, ö, ü) - bu yüzden -ler gelir: {v.Cogul}.",
            HelpHint = "Büyük ünlü uyumu: son ünlü a, ı, o, u ise -lar; e, i, ö, ü ise -ler (okul-lar, ev-ler)."
        };
    }

    private static readonly (string Frage, string[] Optionen, string Antwort, string Erklaerung)[] HalEkleriListe =
    {
        ("Boşluğa hangisi gelmeli? \"Her sabah okul___ gidiyorum.\"", new[] { "okula", "okulda", "okuldan" }, "okula", "Nereye? sorusunun cevabı yönelme hâli (-e/-a) ile verilir: okula gidiyorum."),
        ("Boşluğa hangisi gelmeli? \"Kitabım çanta___ duruyor.\"", new[] { "çantada", "çantaya", "çantadan" }, "çantada", "Nerede? sorusunun cevabı bulunma hâli (-de/-da) ile verilir: çantada duruyor."),
        ("Boşluğa hangisi gelmeli? \"Babam iş___ yeni döndü.\"", new[] { "işten", "işte", "işe" }, "işten", "Nereden? sorusunun cevabı ayrılma hâli (-den/-dan, -ten/-tan) ile verilir: işten döndü."),
        ("Boşluğa hangisi gelmeli? \"Bu akşam sinema___ gidelim mi?\"", new[] { "sinemaya", "sinemada", "sinemadan" }, "sinemaya", "Nereye gidiyoruz? Yönelme hâli: sinema + y + a = sinemaya (ünlüyle biten kelimede araya -y- girer)."),
        ("Boşluğa hangisi gelmeli? \"Kediler bahçe___ oynuyor.\"", new[] { "bahçede", "bahçeye", "bahçeden" }, "bahçede", "Nerede oynuyorlar? Bulunma hâli: bahçede."),
        ("Boşluğa hangisi gelmeli? \"Tren Berlin___ saat üçte kalktı.\"", new[] { "Berlin'den", "Berlin'de", "Berlin'e" }, "Berlin'den", "Tren nereden kalktı? Ayrılma hâli: Berlin'den. Özel isimlere gelen ek kesme işaretiyle ayrılır."),
        ("Boşluğa hangisi gelmeli? \"Yazın dedemler___ kalacağız.\"", new[] { "dedemlerde", "dedemlere", "dedemlerden" }, "dedemlerde", "Nerede kalacağız? Bulunma hâli: dedemlerde."),
        ("Boşluğa hangisi gelmeli? \"Sıcak ekmek fırın___ yeni çıktı.\"", new[] { "fırından", "fırında", "fırına" }, "fırından", "Ekmek nereden çıktı? Ayrılma hâli: fırından."),
        ("Boşluğa hangisi gelmeli? \"Dün akşam park___ uzun süre oturduk.\"", new[] { "parkta", "parka", "parktan" }, "parkta", "Nerede oturduk? Bulunma hâli. \"park\" sert ünsüzle (k) bittiği için -da değil -ta gelir: parkta."),
        ("Boşluğa hangisi gelmeli? \"Mektubu arkadaşım___ gönderdim.\"", new[] { "arkadaşıma", "arkadaşımda", "arkadaşımdan" }, "arkadaşıma", "Kime gönderdim? Yönelme hâli: arkadaşıma."),
        ("Boşluğa hangisi gelmeli? \"Ali okul___ eve yürüyerek dönüyor.\"", new[] { "okuldan", "okulda", "okula" }, "okuldan", "Ali nereden dönüyor? Ayrılma hâli: okuldan eve."),
        ("Boşluğa hangisi gelmeli? \"Toplantı saat beş___ başlıyor.\"", new[] { "beşte", "beşe", "beşten" }, "beşte", "Ne zaman? Saat bildirirken bulunma hâli kullanılır: saat beşte. \"beş\" ş ile bittiği için -de değil -te gelir."),
        ("Boşluğa hangisi gelmeli? \"Kitapları raf___ koy, lütfen.\"", new[] { "rafa", "rafta", "raftan" }, "rafa", "Nereye koyuyoruz? Yönelme hâli: rafa."),
        ("Boşluğa hangisi gelmeli? \"Kuşlar ağaç___ uçup gitti.\"", new[] { "ağaçtan", "ağaçta", "ağaca" }, "ağaçtan", "Kuşlar nereden uçup gitti? Ayrılma hâli: ağaçtan."),
        ("Boşluğa hangisi gelmeli? \"Deniz___ yüzmeyi çok seviyorum.\"", new[] { "Denizde", "Denize", "Denizden" }, "Denizde", "Nerede yüzüyorum? Bulunma hâli: denizde."),
        ("Boşluğa hangisi gelmeli? \"Okul çıkışı kütüphane___ uğradım.\"", new[] { "kütüphaneye", "kütüphanede", "kütüphaneden" }, "kütüphaneye", "\"uğramak\" fiili yönelme hâli ister: kütüphaneye uğradım."),
        ("Boşluğa hangisi gelmeli? \"Bu soruyu öğretmen___ sor.\"", new[] { "öğretmene", "öğretmende", "öğretmenden" }, "öğretmene", "Kime soruyorsun? Yönelme hâli: öğretmene sor."),
        ("Boşluğa hangisi gelmeli? \"Kardeşim bu yıl ilkokul___ başladı.\"", new[] { "ilkokula", "ilkokulda", "ilkokuldan" }, "ilkokula", "\"başlamak\" fiili yönelme hâli ister: ilkokula başladı."),
        ("Boşluğa hangisi gelmeli? \"Sabahları duş___ sonra kahvaltı yaparım.\"", new[] { "duştan", "duşta", "duşa" }, "duştan", "\"sonra\" kelimesi ayrılma hâli ister: duştan sonra, dersten sonra."),
        ("Boşluğa hangisi gelmeli? \"Hafta sonu Hamburg___ gideceğiz.\"", new[] { "Hamburg'a", "Hamburg'da", "Hamburg'dan" }, "Hamburg'a", "Nereye gideceğiz? Yönelme hâli: Hamburg'a. Şehir adına gelen ek kesme işaretiyle ayrılır.")
    };

    private static QuizQuestion HalEkleri(Random r)
    {
        var f = HalEkleriListe[r.Next(HalEkleriListe.Length)];
        return new QuizQuestion
        {
            Id = NewId(), Subject = Subject.Tuerkisch, GradeLevel = GradeLevel.Klasse6,
            Topic = "Hâl Ekleri -e/-de/-den (Wohin, wo, woher)", Type = QuestionType.MultipleChoice,
            Prompt = f.Frage, Options = f.Optionen, CorrectAnswers = new[] { f.Antwort }, Explanation = f.Erklaerung,
            HelpHint = "Nereye? -> -e/-a (okula). Nerede? -> -de/-da (okulda). Nereden? -> -den/-dan (okuldan). Sert ünsüzden sonra -te/-ta, -ten/-tan."
        };
    }

    private static readonly (string Frage, string[] Optionen, string Antwort, string Erklaerung)[] SoruEkiListe =
    {
        ("Boşluğa hangisi gelmeli? \"Yarın okula gelecek ___?\"", new[] { "misin", "mısın", "musun" }, "misin", "Son ünlü \"e\" (gelecek) - soru eki \"mi\" olur, kişi eki eklenir: gelecek misin?"),
        ("Boşluğa hangisi gelmeli? \"Bu kitap senin ___?\"", new[] { "mi", "mı", "mu" }, "mi", "Son ünlü \"i\" (senin) - soru eki \"mi\" olur ve ayrı yazılır."),
        ("Boşluğa hangisi gelmeli? \"Kapı açık ___?\"", new[] { "mı", "mi", "mu" }, "mı", "Son ünlü \"ı\" (açık) - soru eki \"mı\" olur."),
        ("Boşluğa hangisi gelmeli? \"Çay soğuk ___?\"", new[] { "mu", "mü", "mı" }, "mu", "Son ünlü \"u\" (soğuk) - soru eki \"mu\" olur."),
        ("Boşluğa hangisi gelmeli? \"Hava güzel ___?\"", new[] { "mi", "mı", "mü" }, "mi", "Son ünlü \"e\" (güzel) - soru eki \"mi\" olur."),
        ("Boşluğa hangisi gelmeli? \"Ödevini bitirdin ___?\"", new[] { "mi", "mı", "mu" }, "mi", "Son ünlü \"i\" (bitirdin) - soru eki \"mi\" olur."),
        ("Boşluğa hangisi gelmeli? \"Siz de futbol oynuyor ___?\"", new[] { "musunuz", "misiniz", "mısınız" }, "musunuz", "Son ünlü \"o\" (oynuyor) - soru eki \"mu\" olur, \"siz\" için -sunuz eklenir: oynuyor musunuz?"),
        ("Boşluğa hangisi gelmeli? \"Bu ekmek taze ___?\"", new[] { "mi", "mı", "mu" }, "mi", "Son ünlü \"e\" (taze) - soru eki \"mi\" olur."),
        ("Boşluğa hangisi gelmeli? \"Kuşlar göçtü ___?\"", new[] { "mü", "mu", "mi" }, "mü", "Son ünlü \"ü\" (göçtü) - soru eki \"mü\" olur."),
        ("Boşluğa hangisi gelmeli? \"Köpeğin büyük ___?\"", new[] { "mü", "mu", "mı" }, "mü", "Son ünlü \"ü\" (büyük) - soru eki \"mü\" olur."),
        ("Boşluğa hangisi gelmeli? \"Onlar tatile gitti ___?\"", new[] { "mi", "mı", "mü" }, "mi", "Son ünlü \"i\" (gitti) - soru eki \"mi\" olur."),
        ("Boşluğa hangisi gelmeli? \"Sınav zor ___?\"", new[] { "mu", "mı", "mü" }, "mu", "Son ünlü \"o\" (zor) - soru eki \"mu\" olur. o ve u'dan sonra hep \"mu\" gelir."),
        ("Boşluğa hangisi gelmeli? \"Sen Türkçe biliyor ___?\"", new[] { "musun", "misin", "mısın" }, "musun", "Son ünlü \"o\" (biliyor) - soru eki \"mu\" olur, \"sen\" için -sun eklenir: biliyor musun?"),
        ("Boşluğa hangisi gelmeli? \"Pencere kapalı ___?\"", new[] { "mı", "mi", "mu" }, "mı", "Son ünlü \"ı\" (kapalı) - soru eki \"mı\" olur."),
        ("Boşluğa hangisi gelmeli? \"Bu şarkıyı duydun ___?\"", new[] { "mu", "mü", "mi" }, "mu", "Son ünlü \"u\" (duydun) - soru eki \"mu\" olur."),
        ("Boşluğa hangisi gelmeli? \"Yemek hazır ___?\"", new[] { "mı", "mi", "mu" }, "mı", "Son ünlü \"ı\" (hazır) - soru eki \"mı\" olur."),
        ("Boşluğa hangisi gelmeli? \"Ben haklı ___?\"", new[] { "mıyım", "miyim", "muyum" }, "mıyım", "Son ünlü \"ı\" (haklı) - soru eki \"mı\" olur, \"ben\" için -yım eklenir: haklı mıyım?"),
        ("Boşluğa hangisi gelmeli? \"Bugün hava sıcak ___?\"", new[] { "mı", "mi", "mu" }, "mı", "Son ünlü \"a\" (sıcak) - soru eki \"mı\" olur. a ve ı'dan sonra hep \"mı\" gelir."),
        ("Boşluğa hangisi gelmeli? \"Çocuklar uyudu ___?\"", new[] { "mu", "mü", "mı" }, "mu", "Son ünlü \"u\" (uyudu) - soru eki \"mu\" olur."),
        ("Boşluğa hangisi gelmeli? \"Bu yol doğru ___?\"", new[] { "mu", "mı", "mi" }, "mu", "Son ünlü \"u\" (doğru) - soru eki \"mu\" olur.")
    };

    private static QuizQuestion SoruEki(Random r)
    {
        var f = SoruEkiListe[r.Next(SoruEkiListe.Length)];
        return new QuizQuestion
        {
            Id = NewId(), Subject = Subject.Tuerkisch, GradeLevel = GradeLevel.Klasse6,
            Topic = "Soru Eki mi/mı/mu/mü (Fragepartikel)", Type = QuestionType.MultipleChoice,
            Prompt = f.Frage, Options = f.Optionen, CorrectAnswers = new[] { f.Antwort }, Explanation = f.Erklaerung,
            HelpHint = "Soru eki her zaman AYRI yazılır ve son ünlüye uyar: e/i -> mi, a/ı -> mı, o/u -> mu, ö/ü -> mü."
        };
    }

    private static readonly (string TurkceKelime, string Almanca, string[] Yanlislar)[] SayilarZamanListe =
    {
        ("pazartesi", "Montag", new[] { "Dienstag", "Mittwoch", "Sonntag" }),
        ("salı", "Dienstag", new[] { "Montag", "Donnerstag", "Freitag" }),
        ("çarşamba", "Mittwoch", new[] { "Dienstag", "Donnerstag", "Samstag" }),
        ("perşembe", "Donnerstag", new[] { "Mittwoch", "Freitag", "Dienstag" }),
        ("cuma", "Freitag", new[] { "Samstag", "Donnerstag", "Montag" }),
        ("cumartesi", "Samstag", new[] { "Freitag", "Sonntag", "Montag" }),
        ("pazar", "Sonntag", new[] { "Samstag", "Montag", "Freitag" }),
        ("ocak", "Januar", new[] { "Februar", "Juni", "Dezember" }),
        ("şubat", "Februar", new[] { "Januar", "März", "Oktober" }),
        ("nisan", "April", new[] { "August", "Mai", "März" }),
        ("ağustos", "August", new[] { "April", "Juli", "Oktober" }),
        ("ekim", "Oktober", new[] { "November", "September", "August" }),
        ("aralık", "Dezember", new[] { "November", "Januar", "Oktober" }),
        ("kırk beş", "45", new[] { "54", "35", "40" }),
        ("yetmiş iki", "72", new[] { "27", "62", "82" }),
        ("doksan", "90", new[] { "80", "19", "70" }),
        ("yüz on", "110", new[] { "101", "111", "210" }),
        ("iki bin yirmi altı", "2026", new[] { "2016", "2062", "2006" }),
        ("saat üç buçuk", "halb vier (3:30)", new[] { "halb drei (2:30)", "Viertel nach drei (3:15)", "Viertel vor drei (2:45)" }),
        ("sabah", "Morgen", new[] { "Abend", "Mittag", "Nacht" })
    };

    private static QuizQuestion SayilarVeZaman(Random r)
    {
        var d = SayilarZamanListe[r.Next(SayilarZamanListe.Length)];
        var optionen = new[] { d.Almanca }.Concat(d.Yanlislar).OrderBy(_ => r.Next()).ToArray();

        return new QuizQuestion
        {
            Id = NewId(), Subject = Subject.Tuerkisch, GradeLevel = GradeLevel.Klasse6,
            Topic = "Sayılar, Günler ve Aylar (Zahlen und Zeit) – Wortschatz", Type = QuestionType.MultipleChoice,
            Prompt = $"\"{d.TurkceKelime}\" Almanca hangisidir?",
            Options = optionen, CorrectAnswers = new[] { d.Almanca },
            Explanation = $"\"{d.TurkceKelime}\" Almanca \"{d.Almanca}\" demektir.",
            HelpHint = "Sayılar: on (10), yirmi (20), otuz (30), kırk (40), elli (50), altmış (60), yetmiş (70), seksen (80), doksan (90), yüz (100). \"buçuk\" = halb (üç buçuk = 3:30)."
        };
    }

    private static readonly (string Frage, string[] Optionen, string Antwort, string Erklaerung)[] KisaMetinListe =
    {
        ("Metin: \"Elif her cumartesi babaannesini ziyaret eder. Babaannesi ona börek yapar ve birlikte çay içerler.\" Soru: Elif babaannesini ne zaman ziyaret eder?", new[] { "Her cumartesi", "Her pazar", "Her cuma akşamı" }, "Her cumartesi", "Metnin ilk cümlesi: \"Elif her cumartesi babaannesini ziyaret eder.\""),
        ("Metin: \"Mert sabah otobüsü kaçırdı. Bu yüzden okula yürüyerek gitti ve derse beş dakika geç kaldı.\" Soru: Mert neden derse geç kaldı?", new[] { "Otobüsü kaçırdığı için", "Ödevini unuttuğu için", "Yağmur yağdığı için" }, "Otobüsü kaçırdığı için", "\"Bu yüzden\" sözü sebebi gösterir: otobüsü kaçırdığı için yürümek zorunda kaldı."),
        ("Metin: \"Zeynep'in sınıfında 24 öğrenci var. Bugün üç öğrenci hasta olduğu için okula gelmedi.\" Soru: Bugün sınıfta kaç öğrenci var?", new[] { "21", "24", "27" }, "21", "24 öğrenciden 3'ü gelmedi: 24 - 3 = 21."),
        ("Metin: \"Kaan Berlin'de doğdu. Annesi İzmirli, babası ise Trabzonlu. Yazları hep Türkiye'ye giderler.\" Soru: Kaan nerede doğdu?", new[] { "Berlin'de", "İzmir'de", "Trabzon'da" }, "Berlin'de", "İlk cümle: \"Kaan Berlin'de doğdu.\" İzmir ve Trabzon anne ile babanın memleketi."),
        ("Metin: \"Ayşe kütüphaneden üç kitap aldı. İkisini bir haftada okudu, üçüncüsünü henüz bitirmedi.\" Soru: Ayşe kaç kitabı okuyup bitirdi?", new[] { "İki", "Üç", "Bir" }, "İki", "Üç kitaptan ikisini okudu, üçüncüsü henüz bitmedi."),
        ("Metin: \"Parkta bir köpek kayboldu. Çocuklar onu bulup tasmasındaki numarayı aradılar. Sahibi çok sevindi.\" Soru: Çocuklar köpeğin sahibine nasıl ulaştı?", new[] { "Tasmadaki numarayı arayarak", "Mahallede kapı kapı dolaşarak", "Veterinere götürüp sorarak" }, "Tasmadaki numarayı arayarak", "Metinde: \"tasmasındaki numarayı aradılar.\""),
        ("Metin: \"Deniz, matematik sınavından önce üç gün boyunca her akşam bir saat çalıştı. Sınavdan iyi not aldı.\" Soru: Metne göre Deniz iyi notu neye borçlu?", new[] { "Düzenli çalışmasına", "Öğretmenin yardımına", "Soruların kolaylığına" }, "Düzenli çalışmasına", "Metin yalnızca Deniz'in üç gün boyunca her akşam çalıştığını anlatıyor - öğretmenden veya kolay sorulardan söz etmiyor."),
        ("Metin: \"Hava tahminine göre yarın Berlin'de yağmur yağacak ve sıcaklık on derece olacak.\" Soru: Yarın dışarı çıkarken ne almak mantıklıdır?", new[] { "Şemsiye", "Güneş kremi", "Mayo" }, "Şemsiye", "Yağmur yağacağı için şemsiye almak mantıklıdır."),
        ("Metin: \"Emre futbol takımında kaleci. Geçen maçta iki penaltı kurtardı ve takımı 1-0 kazandı.\" Soru: Emre takımda hangi görevi yapıyor?", new[] { "Kaleci", "Forvet", "Hakem" }, "Kaleci", "İlk cümle: \"Emre futbol takımında kaleci.\""),
        ("Metin: \"Selin'in doğum günü 14 Mart'ta. Bu yıl arkadaşlarını bowling oynamaya davet etti.\" Soru: Selin doğum gününde ne yapacak?", new[] { "Bowling oynayacak", "Sinemaya gidecek", "Yüzmeye gidecek" }, "Bowling oynayacak", "Metinde: \"arkadaşlarını bowling oynamaya davet etti.\""),
        ("Metin: \"Burak telefonla çok oyun oynuyordu. Annesiyle konuştuktan sonra günde en fazla bir saat oynamaya karar verdi.\" Soru: Burak ne karar verdi?", new[] { "Oyun süresini sınırlamaya", "Yeni bir oyun almaya", "Telefonu tamamen bırakmaya" }, "Oyun süresini sınırlamaya", "\"Günde en fazla bir saat\" oynamak, oyun süresini sınırlamak demektir - tamamen bırakmak değil."),
        ("Metin: \"Leyla ile Can kardeştir. Leyla on iki, Can ise dokuz yaşındadır.\" Soru: Can, Leyla'dan kaç yaş küçüktür?", new[] { "Üç", "İki", "Dört" }, "Üç", "12 - 9 = 3. Can üç yaş küçüktür."),
        ("Metin: \"Okulun bahçesine yeni ağaçlar dikildi. Her sınıf bir ağaçtan sorumlu ve onu sulamak zorunda.\" Soru: Sınıfların görevi nedir?", new[] { "Bir ağacı sulamak", "Bahçeyi süpürmek", "Çiçek satmak" }, "Bir ağacı sulamak", "Metinde: \"Her sınıf bir ağaçtan sorumlu ve onu sulamak zorunda.\""),
        ("Metin: \"Murat Türkçe ve Almanca konuşuyor. Okulda İngilizce de öğreniyor.\" Soru: Murat toplam kaç dil konuşuyor ya da öğreniyor?", new[] { "Üç", "İki", "Dört" }, "Üç", "Türkçe, Almanca ve İngilizce - toplam üç dil."),
        ("Metin: \"Market sabah sekizde açılıyor ve akşam sekizde kapanıyor. Pazar günleri ise kapalı.\" Soru: Pazar günü markete gidilirse ne olur?", new[] { "Market kapalıdır", "Market geç açılır", "Market erken kapanır" }, "Market kapalıdır", "Son cümle: \"Pazar günleri ise kapalı.\""),
        ("Metin: \"Nehir'in kedisi Pamuk bembeyazdır. Adını da bu yüzden Pamuk koymuşlar.\" Soru: Kediye neden Pamuk adı verilmiş?", new[] { "Rengi beyaz olduğu için", "Çok yumuşak olduğu için", "Çok küçük olduğu için" }, "Rengi beyaz olduğu için", "\"Bu yüzden\" önceki cümleye bağlanır: kedi bembeyaz olduğu için adı Pamuk."),
        ("Metin: \"Sınıf gezisinde müzeye gittik. Rehber bize eski Mısır'dan kalma bir mumya gösterdi.\" Soru: Rehber ne gösterdi?", new[] { "Bir mumya", "Bir dinozor iskeleti", "Eski bir tablo" }, "Bir mumya", "Metinde: \"eski Mısır'dan kalma bir mumya gösterdi.\""),
        ("Metin: \"Yusuf bisikletiyle okula giderken kask takar. Babası ona bunun çok önemli olduğunu söyledi.\" Soru: Yusuf'un kask takmasının asıl sebebi nedir?", new[] { "Güvenliği için", "Moda olduğu için", "Hava soğuk olduğu için" }, "Güvenliği için", "Kask, düşme ve kazalarda başı korur - babasının \"çok önemli\" demesinin sebebi budur."),
        ("Metin: \"Bugün pazartesi. Ödevin teslim tarihi yarından sonraki gün.\" Soru: Ödev hangi gün teslim edilecek?", new[] { "Çarşamba", "Salı", "Perşembe" }, "Çarşamba", "Pazartesi -> yarın salı -> yarından sonraki gün çarşamba."),
        ("Metin: \"Ece'nin annesi hemşire, babası ise otobüs şoförü. Ece büyüyünce doktor olmak istiyor.\" Soru: Ece'nin annesinin mesleği nedir?", new[] { "Hemşire", "Doktor", "Şoför" }, "Hemşire", "Metinde: \"Ece'nin annesi hemşire\". Doktor olmak isteyen Ece'nin kendisi.")
    };

    private static QuizQuestion KisaMetinAnlama(Random r)
    {
        var f = KisaMetinListe[r.Next(KisaMetinListe.Length)];
        return new QuizQuestion
        {
            Id = NewId(), Subject = Subject.Tuerkisch, GradeLevel = GradeLevel.Klasse6,
            Topic = "Kısa Metin Anlama (Leseverstehen)", Type = QuestionType.MultipleChoice,
            Prompt = f.Frage, Options = f.Optionen, CorrectAnswers = new[] { f.Antwort }, Explanation = f.Erklaerung,
            HelpHint = "Önce soruyu oku, sonra cevabı metinde ara. Cevap çoğu zaman metindeki bir cümlede açıkça yazılıdır."
        };
    }

    // ================= Klasse 9: Erweiterung 28.09.2026 =================
    // Wie bei Klasse 6: Türkisch ist täglich dabei, der alte Pool (200 Fragen) reichte
    // rechnerisch knapp sieben Wochen. Neu: Söz sanatları, Ses olayları, Sözcükte anlam,
    // Cümle türleri, Türk edebiyatından yazarlar.

    private static readonly (string Frage, string[] Optionen, string Antwort, string Erklaerung)[] SozSanatlariListe =
    {
        ("\"Kız, ay gibi güzeldi.\" cümlesinde hangi söz sanatı vardır?", new[] { "Benzetme", "Kişileştirme", "Abartma" }, "Benzetme", "Kızın güzelliği \"gibi\" edatıyla aya benzetiliyor - benzetme (teşbih)."),
        ("\"Rüzgâr pencereye vurup ağlıyordu.\" cümlesinde hangi söz sanatı vardır?", new[] { "Kişileştirme", "Benzetme", "Abartma" }, "Kişileştirme", "Ağlamak insana özgüdür; rüzgâra insan özelliği verilmiş - kişileştirme (teşhis)."),
        ("\"Öyle çok ağladı ki gözyaşları sel oldu.\" cümlesinde hangi söz sanatı vardır?", new[] { "Abartma", "Tezat", "Konuşturma" }, "Abartma", "Gözyaşlarının sel olması gerçekte mümkün değildir; durum olduğundan çok büyük anlatılıyor - abartma (mübalağa)."),
        ("\"Ağaç bana 'Beni kesme!' dedi.\" cümlesinde hangi söz sanatı vardır?", new[] { "Konuşturma", "Benzetme", "Abartma" }, "Konuşturma", "İnsan dışındaki bir varlık (ağaç) konuşturuluyor - konuşturma (intak)."),
        ("\"Gülerken ağlıyordu içim.\" cümlesinde hangi söz sanatı vardır?", new[] { "Tezat", "Benzetme", "Konuşturma" }, "Tezat", "Gülmek ve ağlamak karşıt kavramlardır ve bir arada kullanılmış - tezat (karşıtlık)."),
        ("\"Onun kalbi taş gibi sertti.\" cümlesinde hangi söz sanatı vardır?", new[] { "Benzetme", "Tezat", "Konuşturma" }, "Benzetme", "Kalp \"gibi\" ile taşa benzetiliyor - benzetme."),
        ("\"Güneş, sabah bize gülümsedi.\" cümlesinde hangi söz sanatı vardır?", new[] { "Kişileştirme", "Tezat", "Abartma" }, "Kişileştirme", "Gülümsemek insana özgüdür; güneşe insan özelliği verilmiş - kişileştirme."),
        ("\"Bin kere söyledim sana!\" cümlesinde hangi söz sanatı vardır?", new[] { "Abartma", "Benzetme", "Tezat" }, "Abartma", "Gerçekten bin kere söylenmemiştir; sayı bilerek büyütülmüş - abartma."),
        ("\"Zengin fakir, herkes oradaydı.\" cümlesinde hangi söz sanatı vardır?", new[] { "Tezat", "Abartma", "Benzetme" }, "Tezat", "Zengin ve fakir karşıt kavramlardır - tezat."),
        ("\"Aslan gibi güçlü bir adamdı.\" cümlesinde hangi söz sanatı vardır?", new[] { "Benzetme", "Kişileştirme", "Tezat" }, "Benzetme", "Adam \"gibi\" edatıyla aslana benzetiliyor - benzetme."),
        ("\"Şehir sabah uykusundan uyandı.\" cümlesinde hangi söz sanatı vardır?", new[] { "Kişileştirme", "Benzetme", "Tezat" }, "Kişileştirme", "Uyumak ve uyanmak canlılara özgüdür; şehre insan özelliği verilmiş - kişileştirme."),
        ("\"Kedi bana dönüp 'Bana mama ver' dedi.\" cümlesinde hangi söz sanatı vardır?", new[] { "Konuşturma", "Tezat", "Abartma" }, "Konuşturma", "Hayvan insan gibi konuşturuluyor - konuşturma."),
        ("\"Açlıktan ölüyorum!\" cümlesinde hangi söz sanatı vardır?", new[] { "Abartma", "Tezat", "Benzetme" }, "Abartma", "Konuşan kişi gerçekte ölmek üzere değildir; açlığını büyüterek anlatıyor - abartma."),
        ("\"Gece gündüz, yaz kış demeden çalıştı.\" cümlesinde hangi söz sanatı vardır?", new[] { "Tezat", "Konuşturma", "Kişileştirme" }, "Tezat", "Gece-gündüz ve yaz-kış karşıt kavram çiftleridir - tezat."),
        ("\"Dağlar başını eğmiş, bizi selamlıyordu.\" cümlesinde hangi söz sanatı vardır?", new[] { "Kişileştirme", "Tezat", "Benzetme" }, "Kişileştirme", "Selamlamak insana özgü bir davranıştır; dağlara insan özelliği verilmiş - kişileştirme."),
        ("\"Gözlerin yıldız gibi parlıyor.\" cümlesinde hangi söz sanatı vardır?", new[] { "Benzetme", "Abartma", "Konuşturma" }, "Benzetme", "Gözler \"gibi\" edatıyla yıldızlara benzetiliyor - benzetme."),
        ("\"Kalem, 'Beni kullanmayı unutma' diye seslendi.\" cümlesinde hangi söz sanatı vardır?", new[] { "Konuşturma", "Benzetme", "Tezat" }, "Konuşturma", "Cansız bir varlık (kalem) konuşturuluyor - konuşturma."),
        ("\"Dünyanın yükünü omuzlarımda taşıyorum.\" cümlesinde hangi söz sanatı vardır?", new[] { "Abartma", "Tezat", "Konuşturma" }, "Abartma", "Kimse dünyanın yükünü taşıyamaz; sorumluluk olduğundan çok büyük anlatılıyor - abartma."),
        ("\"Hem sevinçli hem hüzünlüydü o gün.\" cümlesinde hangi söz sanatı vardır?", new[] { "Tezat", "Benzetme", "Abartma" }, "Tezat", "Sevinç ve hüzün karşıt duygulardır ve bir arada kullanılmış - tezat."),
        ("\"Bulutlar pamuk gibi yumuşacıktı.\" cümlesinde hangi söz sanatı vardır?", new[] { "Benzetme", "Kişileştirme", "Tezat" }, "Benzetme", "Bulutlar \"gibi\" edatıyla pamuğa benzetiliyor - benzetme.")
    };

    private static QuizQuestion SozSanatlari(Random r)
    {
        var f = SozSanatlariListe[r.Next(SozSanatlariListe.Length)];
        return new QuizQuestion
        {
            Id = NewId(), Subject = Subject.Tuerkisch, GradeLevel = GradeLevel.Klasse9,
            Topic = "Söz Sanatları (Stilmittel)", Type = QuestionType.MultipleChoice,
            Prompt = f.Frage, Options = f.Optionen, CorrectAnswers = new[] { f.Antwort }, Explanation = f.Erklaerung,
            HelpHint = "Benzetme: \"gibi\" ile karşılaştırma. Kişileştirme: insana ait özellik başka varlığa verilir. Konuşturma: insan dışı varlık konuşur. Abartma: olduğundan büyük anlatma. Tezat: karşıt kavramlar bir arada."
        };
    }

    private static readonly (string Frage, string[] Optionen, string Antwort, string Erklaerung)[] SesOlaylariListe =
    {
        ("\"kitap + ı -> kitabı\" örneğinde hangi ses olayı vardır?", new[] { "Ünsüz yumuşaması", "Ünlü düşmesi", "Kaynaştırma" }, "Ünsüz yumuşaması", "Sert ünsüz \"p\", ünlüyle başlayan ek alınca yumuşayıp \"b\" olur: kitap -> kitabı."),
        ("\"ağız + ı -> ağzı\" örneğinde hangi ses olayı vardır?", new[] { "Ünlü düşmesi", "Ünlü daralması", "Ünsüz benzeşmesi" }, "Ünlü düşmesi", "İkinci hecedeki dar ünlü \"ı\", ünlüyle başlayan ek alınca düşer: ağız -> ağzı."),
        ("\"sokak + da -> sokakta\" örneğinde hangi ses olayı vardır?", new[] { "Ünsüz benzeşmesi", "Ünsüz yumuşaması", "Ünlü düşmesi" }, "Ünsüz benzeşmesi", "Sert ünsüzle (k) biten kelimeye gelen ekin \"d\"si sertleşip \"t\" olur: sokakta."),
        ("\"başla + yor -> başlıyor\" örneğinde hangi ses olayı vardır?", new[] { "Ünlü daralması", "Kaynaştırma", "Ünlü düşmesi" }, "Ünlü daralması", "Geniş ünlü \"a\", -yor eki gelince daralıp \"ı\" olur: başla- -> başlıyor."),
        ("\"kapı + ı -> kapıyı\" örneğinde hangi ses olayı vardır?", new[] { "Kaynaştırma", "Ünsüz benzeşmesi", "Ünlü daralması" }, "Kaynaştırma", "İki ünlü yan yana gelmesin diye araya kaynaştırma ünsüzü \"y\" girer: kapı-y-ı."),
        ("\"renk + i -> rengi\" örneğinde hangi ses olayı vardır?", new[] { "Ünsüz yumuşaması", "Ünsüz benzeşmesi", "Ünlü daralması" }, "Ünsüz yumuşaması", "\"nk\" ile biten kelimede \"k\", ünlüyle başlayan ek gelince \"g\" olur: renk -> rengi."),
        ("\"burun + u -> burnu\" örneğinde hangi ses olayı vardır?", new[] { "Ünlü düşmesi", "Ünsüz yumuşaması", "Kaynaştırma" }, "Ünlü düşmesi", "İkinci hecedeki \"u\" düşer: burun -> burnu."),
        ("\"kitap + da -> kitapta\" örneğinde hangi ses olayı vardır?", new[] { "Ünsüz benzeşmesi", "Ünlü daralması", "Kaynaştırma" }, "Ünsüz benzeşmesi", "Sert \"p\"den sonra ekin \"d\"si \"t\" olur: kitapta. (Fıstıkçı Şahap kuralı: f, s, t, k, ç, ş, h, p)"),
        ("\"ara + yor -> arıyor\" örneğinde hangi ses olayı vardır?", new[] { "Ünlü daralması", "Ünsüz yumuşaması", "Ünlü düşmesi" }, "Ünlü daralması", "Geniş \"a\", -yor ekinden önce daralıp \"ı\" olur: arıyor."),
        ("\"araba + a -> arabaya\" örneğinde hangi ses olayı vardır?", new[] { "Kaynaştırma", "Ünlü düşmesi", "Ünsüz yumuşaması" }, "Kaynaştırma", "İki ünlü arasına \"y\" girer: araba-y-a."),
        ("\"ağaç + ı -> ağacı\" örneğinde hangi ses olayı vardır?", new[] { "Ünsüz yumuşaması", "Kaynaştırma", "Ünlü daralması" }, "Ünsüz yumuşaması", "Sert \"ç\" yumuşayıp \"c\" olur: ağaç -> ağacı."),
        ("\"omuz + u -> omzu\" örneğinde hangi ses olayı vardır?", new[] { "Ünlü düşmesi", "Ünsüz benzeşmesi", "Kaynaştırma" }, "Ünlü düşmesi", "İkinci hecedeki \"u\" düşer: omuz -> omzu."),
        ("\"git + di -> gitti\" örneğinde hangi ses olayı vardır?", new[] { "Ünsüz benzeşmesi", "Ünsüz yumuşaması", "Ünlü düşmesi" }, "Ünsüz benzeşmesi", "Sert \"t\"den sonra ekin \"d\"si \"t\" olur: gitti."),
        ("\"bekle + yor -> bekliyor\" örneğinde hangi ses olayı vardır?", new[] { "Ünlü daralması", "Ünsüz benzeşmesi", "Kaynaştırma" }, "Ünlü daralması", "Geniş \"e\", -yor ekinden önce daralıp \"i\" olur: bekliyor."),
        ("\"kedi + i -> kediyi\" örneğinde hangi ses olayı vardır?", new[] { "Kaynaştırma", "Ünlü daralması", "Ünsüz benzeşmesi" }, "Kaynaştırma", "İki ünlü arasına \"y\" girer: kedi-y-i."),
        ("\"dolap + ı -> dolabı\" örneğinde hangi ses olayı vardır?", new[] { "Ünsüz yumuşaması", "Ünlü düşmesi", "Ünlü daralması" }, "Ünsüz yumuşaması", "Sert \"p\" yumuşayıp \"b\" olur: dolap -> dolabı."),
        ("\"oğul + u -> oğlu\" örneğinde hangi ses olayı vardır?", new[] { "Ünlü düşmesi", "Kaynaştırma", "Ünsüz yumuşaması" }, "Ünlü düşmesi", "İkinci hecedeki \"u\" düşer: oğul -> oğlu."),
        ("\"süt + cü -> sütçü\" örneğinde hangi ses olayı vardır?", new[] { "Ünsüz benzeşmesi", "Kaynaştırma", "Ünlü daralması" }, "Ünsüz benzeşmesi", "Sert \"t\"den sonra ekin \"c\"si \"ç\" olur: sütçü."),
        ("\"ye + yor -> yiyor\" örneğinde hangi ses olayı vardır?", new[] { "Ünlü daralması", "Ünlü düşmesi", "Ünsüz yumuşaması" }, "Ünlü daralması", "\"e\", -yor ekinden önce daralıp \"i\" olur: yiyor."),
        ("\"su + u -> suyu\" örneğinde hangi ses olayı vardır?", new[] { "Kaynaştırma", "Ünsüz benzeşmesi", "Ünlü düşmesi" }, "Kaynaştırma", "İki ünlü arasına \"y\" girer: su-y-u.")
    };

    private static QuizQuestion SesOlaylari(Random r)
    {
        var f = SesOlaylariListe[r.Next(SesOlaylariListe.Length)];
        return new QuizQuestion
        {
            Id = NewId(), Subject = Subject.Tuerkisch, GradeLevel = GradeLevel.Klasse9,
            Topic = "Ses Olayları (Lautveränderungen)", Type = QuestionType.MultipleChoice,
            Prompt = f.Frage, Options = f.Optionen, CorrectAnswers = new[] { f.Antwort }, Explanation = f.Erklaerung,
            HelpHint = "Yumuşama: p,ç,t,k -> b,c,d,ğ/g. Benzeşme: sert ünsüzden sonra d->t, c->ç. Ünlü düşmesi: ağız->ağzı. Daralma: a/e -> ı/i (-yor önünde). Kaynaştırma: iki ünlü arasına y, n, s, ş."
        };
    }

    private static readonly (string Frage, string[] Optionen, string Antwort, string Erklaerung)[] SozcukteAnlamListe =
    {
        ("\"Çok soğuk bir insandı, kimseyle konuşmazdı.\" cümlesinde \"soğuk\" hangi anlamda kullanılmıştır?", new[] { "Mecaz anlam", "Gerçek anlam", "Terim anlam" }, "Mecaz anlam", "Burada \"soğuk\" sıcaklık değil, \"ilgisiz, mesafeli\" demektir - mecaz anlam."),
        ("\"Bu kış hava çok soğuk.\" cümlesinde \"soğuk\" hangi anlamda kullanılmıştır?", new[] { "Gerçek anlam", "Mecaz anlam", "Terim anlam" }, "Gerçek anlam", "Havanın düşük sıcaklığı anlatılıyor - kelimenin ilk ve temel anlamı, gerçek anlam."),
        ("\"Üçgenin iç açıları toplamı 180 derecedir.\" cümlesinde \"açı\" hangi anlamda kullanılmıştır?", new[] { "Terim anlam", "Gerçek anlam", "Mecaz anlam" }, "Terim anlam", "\"Açı\" burada matematiğe özgü bir kavramdır - terim anlam."),
        ("\"Olaya farklı bir açıdan bakmalısın.\" cümlesinde \"açı\" hangi anlamda kullanılmıştır?", new[] { "Mecaz anlam", "Terim anlam", "Gerçek anlam" }, "Mecaz anlam", "Burada \"açı\" geometrik değil, \"bakış tarzı\" demektir - mecaz anlam."),
        ("\"Tatlı bir sesi vardı.\" cümlesinde \"tatlı\" hangi anlamda kullanılmıştır?", new[] { "Mecaz anlam", "Gerçek anlam", "Terim anlam" }, "Mecaz anlam", "Sesin tadı olmaz; \"tatlı\" burada \"hoşa giden\" demektir - mecaz anlam."),
        ("\"Annem tatlı olarak baklava yaptı.\" cümlesinde \"tatlı\" hangi anlamda kullanılmıştır?", new[] { "Gerçek anlam", "Mecaz anlam", "Terim anlam" }, "Gerçek anlam", "Şekerli yiyecek anlatılıyor - gerçek anlam."),
        ("\"Hücre, canlıların en küçük yapı birimidir.\" cümlesinde \"hücre\" hangi anlamda kullanılmıştır?", new[] { "Terim anlam", "Mecaz anlam", "Gerçek anlam" }, "Terim anlam", "\"Hücre\" burada biyolojiye özgü bir kavramdır - terim anlam."),
        ("\"Taş kalpli biriydi, kimseye acımazdı.\" cümlesinde \"taş\" hangi anlamda kullanılmıştır?", new[] { "Mecaz anlam", "Gerçek anlam", "Terim anlam" }, "Mecaz anlam", "\"Taş kalpli\" duygusuz, acımasız demektir - mecaz anlam."),
        ("\"Yola düşen taşı kenara çektik.\" cümlesinde \"taş\" hangi anlamda kullanılmıştır?", new[] { "Gerçek anlam", "Terim anlam", "Mecaz anlam" }, "Gerçek anlam", "Gerçek bir taş parçası anlatılıyor - gerçek anlam."),
        ("\"Cümlenin yüklemini bul.\" cümlesinde \"yüklem\" hangi anlamda kullanılmıştır?", new[] { "Terim anlam", "Gerçek anlam", "Mecaz anlam" }, "Terim anlam", "\"Yüklem\" dil bilgisine özgü bir kavramdır - terim anlam."),
        ("\"Bu işin başına artık sen geçeceksin.\" cümlesinde \"baş\" hangi anlamda kullanılmıştır?", new[] { "Mecaz anlam", "Gerçek anlam", "Terim anlam" }, "Mecaz anlam", "\"İşin başına geçmek\" yönetmek demektir, vücudun bir bölümü değil - mecaz anlam."),
        ("\"Başım çok ağrıyor.\" cümlesinde \"baş\" hangi anlamda kullanılmıştır?", new[] { "Gerçek anlam", "Mecaz anlam", "Terim anlam" }, "Gerçek anlam", "Vücudun bir bölümü olan baş anlatılıyor - gerçek anlam."),
        ("\"Bu problemi çözmek için bir denklem kur.\" cümlesinde \"denklem\" hangi anlamda kullanılmıştır?", new[] { "Terim anlam", "Mecaz anlam", "Gerçek anlam" }, "Terim anlam", "\"Denklem\" matematiğe özgü bir kavramdır - terim anlam."),
        ("\"Arkadaşına çok ağır sözler söyledi.\" cümlesinde \"ağır\" hangi anlamda kullanılmıştır?", new[] { "Mecaz anlam", "Gerçek anlam", "Terim anlam" }, "Mecaz anlam", "Sözlerin ağırlığı ölçülmez; \"ağır\" burada \"kırıcı\" demektir - mecaz anlam."),
        ("\"Bu çanta çok ağır, taşıyamıyorum.\" cümlesinde \"ağır\" hangi anlamda kullanılmıştır?", new[] { "Gerçek anlam", "Terim anlam", "Mecaz anlam" }, "Gerçek anlam", "Çantanın ağırlığı anlatılıyor - gerçek anlam."),
        ("\"Mıknatısın iki kutbu vardır.\" cümlesinde \"kutup\" hangi anlamda kullanılmıştır?", new[] { "Terim anlam", "Gerçek anlam", "Mecaz anlam" }, "Terim anlam", "\"Kutup\" burada fiziğe özgü bir kavramdır - terim anlam."),
        ("\"Kardeşimin çok keskin bir zekâsı var.\" cümlesinde \"keskin\" hangi anlamda kullanılmıştır?", new[] { "Mecaz anlam", "Gerçek anlam", "Terim anlam" }, "Mecaz anlam", "Zekâ kesmez; \"keskin\" burada \"hızlı kavrayan\" demektir - mecaz anlam."),
        ("\"Bıçak çok keskin, dikkat et.\" cümlesinde \"keskin\" hangi anlamda kullanılmıştır?", new[] { "Gerçek anlam", "Mecaz anlam", "Terim anlam" }, "Gerçek anlam", "Bıçağın iyi kesmesi anlatılıyor - gerçek anlam."),
        ("\"Bir sayının karesini hesapla.\" cümlesinde \"kare\" hangi anlamda kullanılmıştır?", new[] { "Terim anlam", "Mecaz anlam", "Gerçek anlam" }, "Terim anlam", "\"Bir sayının karesi\" matematiğe özgü bir kavramdır - terim anlam."),
        ("\"Bizi çok sıcak bir şekilde karşıladılar.\" cümlesinde \"sıcak\" hangi anlamda kullanılmıştır?", new[] { "Mecaz anlam", "Gerçek anlam", "Terim anlam" }, "Mecaz anlam", "\"Sıcak karşılama\" samimi, içten karşılama demektir - mecaz anlam.")
    };

    private static QuizQuestion SozcukteAnlam(Random r)
    {
        var f = SozcukteAnlamListe[r.Next(SozcukteAnlamListe.Length)];
        return new QuizQuestion
        {
            Id = NewId(), Subject = Subject.Tuerkisch, GradeLevel = GradeLevel.Klasse9,
            Topic = "Sözcükte Anlam: Gerçek, Mecaz, Terim (Wortbedeutung)", Type = QuestionType.MultipleChoice,
            Prompt = f.Frage, Options = f.Optionen, CorrectAnswers = new[] { f.Antwort }, Explanation = f.Erklaerung,
            HelpHint = "Gerçek anlam: kelimenin ilk, temel anlamı. Mecaz anlam: gerçek anlamından uzaklaşmış yeni anlam (soğuk insan). Terim anlam: bir bilim, sanat ya da meslek dalına özgü anlam (açı, hücre)."
        };
    }

    private static readonly (string Frage, string[] Optionen, string Antwort, string Erklaerung)[] CumleTurleriListe =
    {
        ("\"Hava bugün çok güzel.\" cümlesi yüklemine göre hangi türdür?", new[] { "İsim cümlesi", "Fiil cümlesi", "Soru cümlesi" }, "İsim cümlesi", "Yüklem \"güzel\" bir isimdir (sıfat) - isim cümlesi."),
        ("\"Çocuklar parkta oynadı.\" cümlesi yüklemine göre hangi türdür?", new[] { "Fiil cümlesi", "İsim cümlesi", "Soru cümlesi" }, "Fiil cümlesi", "Yüklem \"oynadı\" çekimli bir fiildir - fiil cümlesi."),
        ("\"Kardeşim öğretmendir.\" cümlesi yüklemine göre hangi türdür?", new[] { "İsim cümlesi", "Fiil cümlesi", "Bağlı cümle" }, "İsim cümlesi", "Yüklem \"öğretmendir\" isim soylu bir kelimedir - isim cümlesi."),
        ("\"Yarın erken kalkacağım.\" cümlesi yüklemine göre hangi türdür?", new[] { "Fiil cümlesi", "İsim cümlesi", "Sıralı cümle" }, "Fiil cümlesi", "Yüklem \"kalkacağım\" çekimli bir fiildir - fiil cümlesi."),
        ("\"Bu kitap çok ilginç.\" cümlesi yüklemine göre hangi türdür?", new[] { "İsim cümlesi", "Fiil cümlesi", "Soru cümlesi" }, "İsim cümlesi", "Yüklem \"ilginç\" bir isimdir (sıfat) - isim cümlesi."),
        ("\"Annem mutfakta yemek pişiriyor.\" cümlesi yüklemine göre hangi türdür?", new[] { "Fiil cümlesi", "İsim cümlesi", "Bağlı cümle" }, "Fiil cümlesi", "Yüklem \"pişiriyor\" çekimli bir fiildir - fiil cümlesi."),
        ("\"Güneş doğdu, kuşlar ötmeye başladı.\" cümlesi yapısına göre hangi türdür?", new[] { "Sıralı cümle", "Basit cümle", "Birleşik cümle" }, "Sıralı cümle", "Virgülle ayrılmış iki bağımsız yargı var - sıralı cümle."),
        ("\"Eve geldim ama kimse yoktu.\" cümlesi yapısına göre hangi türdür?", new[] { "Bağlı cümle", "Sıralı cümle", "Basit cümle" }, "Bağlı cümle", "İki yargı \"ama\" bağlacıyla bağlanmış - bağlı cümle."),
        ("\"Öğretmen sınıfa girince herkes sustu.\" cümlesi yapısına göre hangi türdür?", new[] { "Birleşik cümle", "Sıralı cümle", "Bağlı cümle" }, "Birleşik cümle", "\"girince\" bir fiilimsidir ve yan cümle kurar; asıl yargı \"herkes sustu\" - birleşik cümle."),
        ("\"Dün akşam arkadaşımla sinemaya gittim.\" cümlesi yapısına göre hangi türdür?", new[] { "Basit cümle", "Birleşik cümle", "Sıralı cümle" }, "Basit cümle", "Tek bir yargı (gittim) var, fiilimsi yok - basit cümle."),
        ("\"Ders çalıştım, sonra dışarı çıktım.\" cümlesi yapısına göre hangi türdür?", new[] { "Sıralı cümle", "Birleşik cümle", "Basit cümle" }, "Sıralı cümle", "İki bağımsız yargı virgülle sıralanmış - sıralı cümle."),
        ("\"Yağmur yağdığı için maç ertelendi.\" cümlesi yapısına göre hangi türdür?", new[] { "Birleşik cümle", "Bağlı cümle", "Sıralı cümle" }, "Birleşik cümle", "\"yağdığı için\" yan cümledir (fiilimsi), asıl yargı \"maç ertelendi\" - birleşik cümle."),
        ("\"Hem ders çalıştı hem de odasını topladı.\" cümlesi yapısına göre hangi türdür?", new[] { "Bağlı cümle", "Basit cümle", "Birleşik cümle" }, "Bağlı cümle", "İki yargı \"hem... hem de\" bağlacıyla bağlanmış - bağlı cümle."),
        ("\"Kedi süt içti.\" cümlesi yapısına göre hangi türdür?", new[] { "Basit cümle", "Sıralı cümle", "Bağlı cümle" }, "Basit cümle", "Tek yüklem, tek yargı - basit cümle."),
        ("\"Kapıyı açtım ve içeri girdim.\" cümlesi yapısına göre hangi türdür?", new[] { "Bağlı cümle", "Birleşik cümle", "Basit cümle" }, "Bağlı cümle", "İki yargı \"ve\" bağlacıyla bağlanmış - bağlı cümle."),
        ("\"Okula gelmeden önce kahvaltı yaptım.\" cümlesi yapısına göre hangi türdür?", new[] { "Birleşik cümle", "Sıralı cümle", "Basit cümle" }, "Birleşik cümle", "\"gelmeden önce\" fiilimsiyle kurulmuş bir yan cümledir - birleşik cümle."),
        ("\"Bu filmi hiç izlemedim.\" cümlesi anlamına göre hangi türdür?", new[] { "Olumsuz cümle", "Olumlu cümle", "Soru cümlesi" }, "Olumsuz cümle", "Yüklem \"izlemedim\" olumsuzluk eki (-me) taşıyor - olumsuz cümle."),
        ("\"Yarın bize gelecek misin?\" cümlesi anlamına göre hangi türdür?", new[] { "Soru cümlesi", "Olumlu cümle", "Olumsuz cümle" }, "Soru cümlesi", "Soru eki \"mi\" ile bilgi isteniyor - soru cümlesi."),
        ("\"Bu soruyu bilmeyen yoktur.\" cümlesi anlamına göre hangi türdür?", new[] { "Olumlu cümle", "Olumsuz cümle", "Soru cümlesi" }, "Olumlu cümle", "Yapıca olumsuz görünür (yoktur), ama anlamca herkesin bildiğini söyler - anlamca olumlu cümle."),
        ("\"Toplantıya hiç kimse gelmedi.\" cümlesi anlamına göre hangi türdür?", new[] { "Olumsuz cümle", "Soru cümlesi", "Olumlu cümle" }, "Olumsuz cümle", "Gelmenin gerçekleşmediği söyleniyor - olumsuz cümle.")
    };

    private static QuizQuestion CumleTurleri(Random r)
    {
        var f = CumleTurleriListe[r.Next(CumleTurleriListe.Length)];
        return new QuizQuestion
        {
            Id = NewId(), Subject = Subject.Tuerkisch, GradeLevel = GradeLevel.Klasse9,
            Topic = "Cümle Türleri (Satzarten)", Type = QuestionType.MultipleChoice,
            Prompt = f.Frage, Options = f.Optionen, CorrectAnswers = new[] { f.Antwort }, Explanation = f.Erklaerung,
            HelpHint = "Yüklemine göre: isim / fiil cümlesi. Yapısına göre: basit (tek yargı), birleşik (fiilimsili yan cümle), sıralı (virgülle), bağlı (ve, ama, hem... hem). Anlamına göre: olumlu, olumsuz, soru."
        };
    }

    private static readonly (string Frage, string[] Optionen, string Antwort, string Erklaerung)[] TurkEdebiyatiListe =
    {
        ("\"İnce Memed\" romanının yazarı kimdir?", new[] { "Yaşar Kemal", "Orhan Pamuk", "Aziz Nesin" }, "Yaşar Kemal", "\"İnce Memed\", Çukurova'da geçen bir eşkıya romanıdır ve Yaşar Kemal'in en tanınmış eseridir."),
        ("2006'da Nobel Edebiyat Ödülü'nü kazanan Türk yazar kimdir?", new[] { "Orhan Pamuk", "Yaşar Kemal", "Nazım Hikmet" }, "Orhan Pamuk", "Orhan Pamuk, 2006'da Nobel Edebiyat Ödülü'nü alan ilk Türk yazardır."),
        ("İstiklal Marşı'nın şairi kimdir?", new[] { "Mehmet Akif Ersoy", "Nazım Hikmet Ran", "Yunus Emre" }, "Mehmet Akif Ersoy", "İstiklal Marşı'nı Mehmet Akif Ersoy yazdı; 1921'de milli marş olarak kabul edildi."),
        ("Sabahattin Ali'nin \"Kürk Mantolu Madonna\" romanı büyük ölçüde hangi şehirde geçer?", new[] { "Berlin", "Paris", "Viyana" }, "Berlin", "Romanın kahramanı Raif Efendi gençliğinde Berlin'de yaşar; aşk hikâyesi orada geçer."),
        ("\"Kürk Mantolu Madonna\" kimin eseridir?", new[] { "Sabahattin Ali", "Sait Faik", "Reşat Nuri Güntekin" }, "Sabahattin Ali", "\"Kürk Mantolu Madonna\" (1943) Sabahattin Ali'nin romanıdır ve bugün de çok okunur."),
        ("\"Çalıkuşu\" romanının yazarı kimdir?", new[] { "Reşat Nuri Güntekin", "Halide Edib Adıvar", "Sabahattin Ali" }, "Reşat Nuri Güntekin", "\"Çalıkuşu\", öğretmen Feride'nin Anadolu'daki hayatını anlatan Reşat Nuri Güntekin romanıdır."),
        ("Mizah öyküleriyle tanınan, \"Zübük\" romanının yazarı kimdir?", new[] { "Aziz Nesin", "Orhan Veli", "Yaşar Kemal" }, "Aziz Nesin", "Aziz Nesin, Türk edebiyatının en tanınmış mizah yazarıdır; \"Zübük\" onun eseridir."),
        ("Yunus Emre en çok hangi tür şiirleriyle tanınır?", new[] { "Tasavvufi halk şiiri", "Serbest vezinli şiir", "Mizahi hiciv şiiri" }, "Tasavvufi halk şiiri", "Yunus Emre (13.-14. yüzyıl) sevgi ve hoşgörüyü anlatan tasavvufi şiirleriyle tanınır; sade bir Türkçe kullanmıştır."),
        ("\"Kaşağı\" öyküsünün yazarı kimdir?", new[] { "Ömer Seyfettin", "Sait Faik", "Aziz Nesin" }, "Ömer Seyfettin", "\"Kaşağı\", yalan ve vicdan azabını anlatan bir Ömer Seyfettin öyküsüdür."),
        ("Nasreddin Hoca hangi türle özdeşleşmiştir?", new[] { "Fıkra", "Roman", "Destan" }, "Fıkra", "Nasreddin Hoca, güldürürken düşündüren kısa fıkralarıyla bilinir."),
        ("\"Memleketimden İnsan Manzaraları\" kimin eseridir?", new[] { "Nazım Hikmet", "Orhan Veli", "Cahit Sıtkı" }, "Nazım Hikmet", "\"Memleketimden İnsan Manzaraları\", Nazım Hikmet'in uzun destansı şiiridir."),
        ("Garip akımının öncülerinden olan şair kimdir?", new[] { "Orhan Veli Kanık", "Mehmet Akif Ersoy", "Karacaoğlan" }, "Orhan Veli Kanık", "Orhan Veli, Garip akımıyla şiire günlük dili ve sıradan insanı getirdi."),
        ("\"Uzun ince bir yoldayım\" dizesiyle tanınan halk ozanı kimdir?", new[] { "Âşık Veysel", "Karacaoğlan", "Yunus Emre" }, "Âşık Veysel", "Âşık Veysel (1894-1973), sazı ve sade diliyle tanınan bir halk ozanıdır."),
        ("\"Dede Korkut Hikâyeleri\" hangi türe örnektir?", new[] { "Destansı halk hikâyesi", "Modern psikolojik roman", "Tiyatro oyunu" }, "Destansı halk hikâyesi", "Dede Korkut Hikâyeleri, Oğuz Türklerinin hayatını anlatan destansı halk hikâyeleridir."),
        ("\"Otuz Beş Yaş\" şiirinin şairi kimdir?", new[] { "Cahit Sıtkı Tarancı", "Nazım Hikmet", "Orhan Veli Kanık" }, "Cahit Sıtkı Tarancı", "\"Yaş otuz beş, yolun yarısı eder\" dizesiyle başlayan şiir Cahit Sıtkı Tarancı'nındır."),
        ("\"Ateşten Gömlek\" romanının yazarı kimdir?", new[] { "Halide Edib Adıvar", "Reşat Nuri Güntekin", "Ömer Seyfettin" }, "Halide Edib Adıvar", "\"Ateşten Gömlek\", Kurtuluş Savaşı'nı anlatan bir Halide Edib Adıvar romanıdır."),
        ("Öyküleriyle tanınan ve \"Semaver\"i yazan yazar kimdir?", new[] { "Sait Faik", "Ömer Seyfettin", "Sabahattin Ali" }, "Sait Faik", "Sait Faik Abasıyanık, İstanbul'daki sıradan insanları anlatan öyküleriyle tanınır; \"Semaver\" onun ilk kitabıdır."),
        ("Almanya'da yaşayan, Almanca yazan ve Türkiye kökenli olan yazar hangisidir?", new[] { "Emine Sevgi Özdamar", "Halide Edib Adıvar", "Reşat Nuri Güntekin" }, "Emine Sevgi Özdamar", "Emine Sevgi Özdamar, eserlerini Almanca yazar ve Almanya'da önemli edebiyat ödülleri almıştır."),
        ("17. yüzyılın ünlü halk şairi, koşmalarıyla tanınan kimdir?", new[] { "Karacaoğlan", "Nazım Hikmet", "Orhan Pamuk" }, "Karacaoğlan", "Karacaoğlan, 17. yüzyıl halk edebiyatının en ünlü âşıklarından biridir; doğa ve sevgi konulu koşmalar yazmıştır."),
        ("\"Kar\" ve \"Benim Adım Kırmızı\" romanlarının yazarı kimdir?", new[] { "Orhan Pamuk", "Yaşar Kemal", "Elif Şafak" }, "Orhan Pamuk", "Her iki roman da Orhan Pamuk'a aittir.")
    };

    private static QuizQuestion TurkEdebiyati(Random r)
    {
        var f = TurkEdebiyatiListe[r.Next(TurkEdebiyatiListe.Length)];
        return new QuizQuestion
        {
            Id = NewId(), Subject = Subject.Tuerkisch, GradeLevel = GradeLevel.Klasse9,
            Topic = "Türk Edebiyatından Yazarlar ve Eserler (Literatur)", Type = QuestionType.MultipleChoice,
            Prompt = f.Frage, Options = f.Optionen, CorrectAnswers = new[] { f.Antwort }, Explanation = f.Erklaerung,
            HelpHint = "Halk edebiyatı: Yunus Emre, Karacaoğlan, Âşık Veysel, Nasreddin Hoca. Roman: Yaşar Kemal, Orhan Pamuk, Sabahattin Ali, Reşat Nuri, Halide Edib. Öykü: Ömer Seyfettin, Sait Faik. Şiir: Nazım Hikmet, Orhan Veli, Mehmet Akif."
        };
    }


    // ================= Klasse 6: Erweiterung 30.09.2026 =================
    // Fünf weitere Themen (100 Fragen): İyelik ekleri, Geniş zaman, Emir kipi/Rica,
    // Vücut ve sağlık, Sıfatlarda karşılaştırma (daha/en/kadar).

    private static readonly (string Kisi, string Kelime, string Iyelik, string Aciklama)[] IyelikEkleriBeispiele =
    {
        ("benim", "kitap", "kitabım", "\"benim\" -> -ım. \"kitap\" p ile biter; ünlüyle başlayan ek gelince p -> b olur (ünsüz yumuşaması): kitabım."),
        ("senin", "ağaç", "ağacın", "\"senin\" -> -ın. \"ağaç\" ç ile biter; ünlüyle başlayan ek gelince ç -> c olur: ağacın."),
        ("onun", "araba", "arabası", "\"onun\" -> ünlüyle biten kelimede -sı/-si/-su/-sü gelir. Son ünlü a: arabası."),
        ("bizim", "okul", "okulumuz", "\"bizim\" -> -ımız/-imiz/-umuz/-ümüz. Son ünlü u olduğu için: okulumuz."),
        ("sizin", "ev", "eviniz", "\"sizin\" -> -ınız/-iniz/-unuz/-ünüz. Son ünlü e olduğu için: eviniz."),
        ("benim", "köpek", "köpeğim", "\"benim\" -> -im. \"köpek\" k ile biter; ünlüyle başlayan ek gelince k -> ğ olur: köpeğim."),
        ("benim", "oda", "odam", "\"oda\" ünlüyle biter - bu yüzden araya ünlü girmez, sadece -m eklenir: odam."),
        ("senin", "anne", "annen", "\"anne\" ünlüyle biter - \"senin\" için sadece -n eklenir: annen."),
        ("onun", "baba", "babası", "\"baba\" ünlüyle biter - \"onun\" için -sı eklenir: babası."),
        ("bizim", "kardeş", "kardeşimiz", "\"bizim\" -> -imiz. Son ünlü e olduğu için: kardeşimiz."),
        ("sizin", "öğretmen", "öğretmeniniz", "\"sizin\" -> -iniz. Son ünlü e olduğu için: öğretmeniniz."),
        ("benim", "göz", "gözüm", "\"benim\" -> -üm. Son ünlü ö olduğu için ek -üm olur: gözüm."),
        ("senin", "top", "topun", "\"senin\" -> -un. Son ünlü o olduğu için ek -un olur: topun."),
        ("bizim", "çanta", "çantamız", "\"çanta\" ünlüyle biter - \"bizim\" için -mız eklenir: çantamız."),
        ("onun", "telefon", "telefonu", "\"telefon\" ünsüzle biter - \"onun\" için sadece -u eklenir (son ünlü o): telefonu."),
        ("benim", "defter", "defterim", "\"benim\" -> -im. Son ünlü e olduğu için: defterim."),
        ("bizim", "sokak", "sokağımız", "\"bizim\" -> -ımız. \"sokak\" k ile biter; ünlüyle başlayan ek gelince k -> ğ olur: sokağımız."),
        ("sizin", "kedi", "kediniz", "\"kedi\" ünlüyle biter - \"sizin\" için sadece -niz eklenir: kediniz."),
        ("senin", "gömlek", "gömleğin", "\"senin\" -> -in. \"gömlek\" k ile biter, k -> ğ olur: gömleğin."),
        ("onun", "su", "suyu", "\"su\" bir istisnadır: \"onun\" için -su değil, araya y girer: suyu (su-yu).")
    };

    private static QuizQuestion IyelikEkleri(Random r)
    {
        var v = IyelikEkleriBeispiele[r.Next(IyelikEkleriBeispiele.Length)];

        return new QuizQuestion
        {
            Id = NewId(), Subject = Subject.Tuerkisch, GradeLevel = GradeLevel.Klasse6,
            Topic = "İyelik Ekleri (Possessivsuffixe: mein, dein, sein …)", Type = QuestionType.OpenText,
            Prompt = $"İyelik ekini ekle ve kelimeyi yaz: \"{v.Kisi} {v.Kelime}___\" (Beispiel: benim ev___ -> evim)",
            CorrectAnswers = new[] { v.Iyelik },
            Explanation = v.Aciklama,
            HelpHint = "benim -> -(ı)m, senin -> -(ı)n, onun -> -(s)ı, bizim -> -(ı)mız, sizin -> -(ı)nız. Ünlü uyumuna dikkat (ı/i/u/ü); p, ç, k ünlüden önce b, c, ğ olur (kitabım, ağacım, köpeğim)."
        };
    }

    private static readonly (string Fiil, string Genis, string Kural)[] GenisZamanBeispiele =
    {
        ("almak", "alır", "\"almak\" tek hecelidir ama -ar almaz: -ır alan 13 istisnadan biridir (al-ır)."),
        ("bilmek", "bilir", "\"bilmek\" tek heceli istisnalardandır: -er değil -ir alır (bil-ir)."),
        ("gelmek", "gelir", "\"gelmek\" tek heceli istisnalardandır: -er değil -ir alır (gel-ir)."),
        ("görmek", "görür", "\"görmek\" tek heceli istisnalardandır: -er değil -ür alır (gör-ür)."),
        ("olmak", "olur", "\"olmak\" tek heceli istisnalardandır: -ar değil -ur alır (ol-ur)."),
        ("kalmak", "kalır", "\"kalmak\" tek heceli istisnalardandır: -ar değil -ır alır (kal-ır)."),
        ("vermek", "verir", "\"vermek\" tek heceli istisnalardandır: -er değil -ir alır (ver-ir)."),
        ("bulmak", "bulur", "\"bulmak\" tek heceli istisnalardandır: -ar değil -ur alır (bul-ur)."),
        ("gitmek", "gider", "\"gitmek\" tek heceli, istisna değil: -er alır. Ünlüden önce t -> d olur: gid-er."),
        ("yapmak", "yapar", "\"yapmak\" tek hecelidir ve istisna değildir: -ar alır (yap-ar)."),
        ("içmek", "içer", "\"içmek\" tek hecelidir ve istisna değildir: -er alır (iç-er)."),
        ("sevmek", "sever", "\"sevmek\" tek hecelidir ve istisna değildir: -er alır (sev-er)."),
        ("yazmak", "yazar", "\"yazmak\" tek hecelidir ve istisna değildir: -ar alır (yaz-ar)."),
        ("gülmek", "güler", "\"gülmek\" tek hecelidir ve istisna değildir: -er alır (gül-er)."),
        ("çalışmak", "çalışır", "\"çalışmak\" çok hecelidir: çok heceli kökler -ır/-ir/-ur/-ür alır (çalış-ır)."),
        ("düşünmek", "düşünür", "\"düşünmek\" çok hecelidir: son ünlü ü olduğu için -ür alır (düşün-ür)."),
        ("okumak", "okur", "\"oku-\" ünlüyle biter: geniş zamanda sadece -r eklenir (oku-r)."),
        ("oynamak", "oynar", "\"oyna-\" ünlüyle biter: geniş zamanda sadece -r eklenir (oyna-r)."),
        ("uyumak", "uyur", "\"uyu-\" ünlüyle biter: geniş zamanda sadece -r eklenir (uyu-r)."),
        ("dinlemek", "dinler", "\"dinle-\" ünlüyle biter: geniş zamanda sadece -r eklenir (dinle-r).")
    };

    private static QuizQuestion GenisZaman(Random r)
    {
        var v = GenisZamanBeispiele[r.Next(GenisZamanBeispiele.Length)];

        return new QuizQuestion
        {
            Id = NewId(), Subject = Subject.Tuerkisch, GradeLevel = GradeLevel.Klasse6,
            Topic = "Geniş Zaman (Aorist: Gewohnheiten, Allgemeines)", Type = QuestionType.OpenText,
            Prompt = $"\"{v.Fiil}\" fiilinin (o için) geniş zaman hâlini yaz. (Beispiel: koşmak -> koşar)",
            CorrectAnswers = new[] { v.Genis },
            Explanation = $"\"{v.Fiil}\" -> \"{v.Genis}\". {v.Kural} Geniş zaman alışkanlıkları ve her zaman doğru olanı anlatır: \"Her sabah süt içerim.\"",
            HelpHint = "Ünlüyle biten kök + -r (oku-r). Tek heceli kök çoğunlukla -ar/-er (yap-ar, gid-er); ama al, bil, bul, dur, gel, gör, kal, ol, öl, san, var, ver, vur -ır/-ir/-ur/-ür alır. Çok heceli kök: -ır/-ir/-ur/-ür (çalış-ır)."
        };
    }

    private static readonly (string Frage, string[] Optionen, string Antwort, string Erklaerung)[] EmirKipiListe =
    {
        ("Boşluğa hangisi gelmeli? \"Buraya ___, sana bir şey göstereceğim!\" (sen - gelmek)", new[] { "gelin", "gel", "geldi" }, "gel", "\"sen\" için emir kipinde ek yoktur, sadece fiil kökü kullanılır: gel!"),
        ("Boşluğa hangisi gelmeli? \"Buyurun, lütfen ___!\" (siz - oturmak)", new[] { "otursun", "oturdu", "oturun" }, "oturun", "\"siz\" (ve kibar hitap) için -ın/-in/-un/-ün eklenir. Son ünlü u: otur-un."),
        ("Boşluğa hangisi gelmeli? \"Ödevini hemen ___!\" (sen - yapmak)", new[] { "yap", "yapın", "yaptın" }, "yap", "\"sen\" için emir = fiil kökü: yap! \"yapın\" birden fazla kişiye ya da kibar hitaptır."),
        ("Boşluğa hangisi gelmeli? \"Çocuklar, beni iyi ___!\" (siz - dinlemek)", new[] { "dinlesin", "dinleyin", "dinledi" }, "dinleyin", "Kök ünlüyle bitiyor (dinle-): -in ekinden önce araya -y- girer: dinle-y-in."),
        ("Boşluğa hangisi gelmeli? \"Ali de bizimle ___!\" (o - gelmek)", new[] { "gelin", "geldi", "gelsin" }, "gelsin", "3. tekil kişi (o) için emir eki -sın/-sin/-sun/-sün'dür: gel-sin."),
        ("Hangisi kibar bir ricadır? (Bitte reich mir das Salz.)", new[] { "Tuzu uzattın mı?", "Tuzu uzatır mısın?", "Tuzu uzatacak mısın?" }, "Tuzu uzatır mısın?", "Geniş zaman + soru eki (-r mısın?) kibar bir rica yapar. \"uzattın mı\" geçmişi, \"uzatacak mısın\" geleceği sorar."),
        ("Öğretmenine hangisini söylemek en kibarıdır?", new[] { "Pencereyi aç!", "Pencereyi açtınız mı?", "Pencereyi açar mısınız lütfen?" }, "Pencereyi açar mısınız lütfen?", "Büyüklere \"siz\" ile, -r mısınız sorusu ve \"lütfen\" ile rica edilir. \"Aç!\" kaba bir emirdir."),
        ("Boşluğa hangisi gelmeli? \"Topla evin içinde ___!\" (sen - oynamamak)", new[] { "oynamaz", "oynama", "oynamadı" }, "oynama", "Olumsuz emir: kök + -ma/-me. \"sen\" için başka ek gelmez: oyna-ma!"),
        ("Boşluğa hangisi gelmeli? \"Lütfen koridorda ___!\" (siz - bağırmamak)", new[] { "bağırmasın", "bağırmıyor", "bağırmayın" }, "bağırmayın", "\"siz\" için olumsuz emir: kök + -ma + y + ın: bağır-ma-y-ın."),
        ("Boşluğa hangisi gelmeli? \"Adınızı kâğıdın üstüne ___.\" (siz - yazmak)", new[] { "yazın", "yazsın", "yazdı" }, "yazın", "\"siz\" için -ın/-in/-un/-ün. Son ünlü a olduğu için: yaz-ın."),
        ("Boşluğa hangisi gelmeli? \"Fotoğraf çekiyorum, lütfen ___!\" (siz - gülümsemek)", new[] { "gülümsesin", "gülümseyin", "gülümsedi" }, "gülümseyin", "Kök ünlüyle bitiyor (gülümse-): araya -y- girer: gülümse-y-in."),
        ("Boşluğa hangisi gelmeli? \"Bu kitabı mutlaka ___, çok güzel!\" (sen - okumak)", new[] { "okudu", "okuyun", "oku" }, "oku", "\"sen\" için emir = sadece fiil kökü: oku! \"okuyun\" ise \"siz\" içindir."),
        ("Boşluğa hangisi gelmeli? \"Sebzelerini de ___!\" (sen - yemek)", new[] { "ye", "yiyin", "yedi" }, "ye", "\"yemek\" fiilinin kökü \"ye-\"dir; \"sen\" için emir: ye! (\"siz\" için: yiyin)."),
        ("Hangisi bir emir cümlesidir?", new[] { "Dişlerini fırçaladın.", "Dişlerini fırçala!", "Dişlerini fırçalıyor." }, "Dişlerini fırçala!", "\"fırçala!\" fiil köküdür ve birine bir şey yapmasını söyler - emir. Diğerleri geçmiş ve şimdiki zaman anlatır."),
        ("Boşluğa hangisi gelmeli? \"Hoş geldiniz, içeri ___!\" (siz - girmek)", new[] { "girsin", "girin", "girdi" }, "girin", "Misafirlere kibarca \"siz\" formu kullanılır: gir-in."),
        ("Boşluğa hangisi gelmeli? \"Çocuklar bahçede ___, biz de çay içelim.\" (onlar - oynamak)", new[] { "oynayın", "oynasınlar", "oynadılar" }, "oynasınlar", "3. çoğul kişi (onlar) için emir: -sınlar/-sinler: oyna-sınlar."),
        ("Hangisi kibar bir yardım isteğidir?", new[] { "Bana yardım etmedin.", "Bana yardım ettin mi?", "Bana yardım edebilir misin?" }, "Bana yardım edebilir misin?", "\"-ebilir misin?\" (kannst du …?) kibar bir rica kalıbıdır. Diğer iki cümle rica değil, geçmişle ilgili."),
        ("Birine bir şey uzatırken ya da ikram ederken (bitte schön) ne dersin?", new[] { "Afiyet olsun.", "Geçmiş olsun.", "Buyurun." }, "Buyurun.", "\"Buyurun\" = bitte schön (beim Anbieten). \"Afiyet olsun\" = guten Appetit, \"Geçmiş olsun\" = gute Besserung."),
        ("Hangisi olumsuz bir emirdir?", new[] { "Telefonla oynamadı.", "Telefonla oyna!", "Telefonla oynama!" }, "Telefonla oynama!", "Olumsuz emir -ma/-me ile kurulur: oyna-ma! \"oynamadı\" olumsuz geçmiş zamandır, emir değil."),
        ("\"Pencereyi açın.\" cümlesini daha kibar yapmak için hangi kelime eklenir?", new[] { "hemen", "lütfen", "şimdi" }, "lütfen", "\"lütfen\" = bitte. \"hemen\" (sofort) ve \"şimdi\" (jetzt) cümleyi kibar değil, daha acele yapar.")
    };

    private static QuizQuestion EmirKipiVeRica(Random r)
    {
        var f = EmirKipiListe[r.Next(EmirKipiListe.Length)];
        return new QuizQuestion
        {
            Id = NewId(), Subject = Subject.Tuerkisch, GradeLevel = GradeLevel.Klasse6,
            Topic = "Emir Kipi ve Rica (Imperativ und höfliche Bitten)", Type = QuestionType.MultipleChoice,
            Prompt = f.Frage, Options = f.Optionen.OrderBy(_ => r.Next()).ToArray(), CorrectAnswers = new[] { f.Antwort }, Explanation = f.Erklaerung,
            HelpHint = "sen: sadece kök (gel!). siz: -ın/-in/-un/-ün (gelin!). o: -sın/-sin (gelsin). Olumsuz: -ma/-me (gelme!). Kibar rica: -r mısın? / -ebilir misin? + lütfen."
        };
    }

    private static readonly (string TurkceKelime, string Almanca, string[] Yanlislar)[] VucutSaglikListe =
    {
        ("baş ağrısı", "Kopfschmerzen", new[] { "Bauchschmerzen", "Zahnschmerzen", "Halsschmerzen" }),
        ("diş", "Zahn", new[] { "Zunge", "Lippe", "Kinn" }),
        ("omuz", "Schulter", new[] { "Ellbogen", "Knie", "Rücken" }),
        ("diz", "Knie", new[] { "Knöchel", "Hüfte", "Zeh" }),
        ("parmak", "Finger", new[] { "Handgelenk", "Daumen", "Nagel" }),
        ("mide", "Magen", new[] { "Lunge", "Leber", "Herz" }),
        ("kalp", "Herz", new[] { "Niere", "Magen", "Lunge" }),
        ("ateş (hastalıkta)", "Fieber", new[] { "Husten", "Schnupfen", "Übelkeit" }),
        ("öksürük", "Husten", new[] { "Niesen", "Fieber", "Schnupfen" }),
        ("nezle", "Schnupfen", new[] { "Husten", "Grippe", "Fieber" }),
        ("eczane", "Apotheke", new[] { "Arztpraxis", "Krankenhaus", "Drogerie" }),
        ("hastane", "Krankenhaus", new[] { "Apotheke", "Arztpraxis", "Pflegeheim" }),
        ("ilaç", "Medikament", new[] { "Rezept", "Pflaster", "Verband" }),
        ("reçete", "Rezept (vom Arzt)", new[] { "Medikament", "Überweisung", "Krankschreibung" }),
        ("muayene", "Untersuchung", new[] { "Behandlung", "Impfung", "Operation" }),
        ("aşı", "Impfung", new[] { "Spritze", "Tablette", "Salbe" }),
        ("sağlıklı beslenme", "gesunde Ernährung", new[] { "regelmäßige Bewegung", "ausreichender Schlaf", "gründliche Hygiene" }),
        ("dirsek", "Ellbogen", new[] { "Schulter", "Handgelenk", "Knöchel" }),
        ("boğaz ağrısı", "Halsschmerzen", new[] { "Ohrenschmerzen", "Kopfschmerzen", "Rückenschmerzen" }),
        ("el bileği", "Handgelenk", new[] { "Ellbogen", "Fingerknöchel", "Unterarm" })
    };

    private static QuizQuestion VucutVeSaglik(Random r)
    {
        var d = VucutSaglikListe[r.Next(VucutSaglikListe.Length)];
        var optionen = new[] { d.Almanca }.Concat(d.Yanlislar).OrderBy(_ => r.Next()).ToArray();

        return new QuizQuestion
        {
            Id = NewId(), Subject = Subject.Tuerkisch, GradeLevel = GradeLevel.Klasse6,
            Topic = "Vücut ve Sağlık (Körper und Gesundheit) – Wortschatz", Type = QuestionType.MultipleChoice,
            Prompt = $"\"{d.TurkceKelime}\" kelimesinin Almancası hangisidir?",
            Options = optionen, CorrectAnswers = new[] { d.Almanca },
            Explanation = $"\"{d.TurkceKelime}\" Almanca \"{d.Almanca}\" demektir.",
            HelpHint = "Doktorda işe yarar: \"Başım ağrıyor\" (Ich habe Kopfschmerzen), \"Ateşim var\" (Ich habe Fieber). İlaç eczaneden, reçeteyle alınır."
        };
    }

    private static readonly (string Frage, string[] Optionen, string Antwort, string Erklaerung)[] KarsilastirmaListe =
    {
        ("Boşluğa hangisi gelmeli? \"Ahmet, Can___ daha uzun.\"", new[] { "Can'a", "Can'dan", "Can'da" }, "Can'dan", "Karşılaştırmada, karşılaştırılan kişi ayrılma hâli (-den/-dan) alır: Can'dan daha uzun (größer als Can)."),
        ("Boşluğa hangisi gelmeli? \"Sınıfımızın ___ hızlı koşucusu Elif'tir.\"", new[] { "daha", "kadar", "en" }, "en", "Bir grubun içinde birinci olanı \"en\" gösterir (Superlativ): en hızlı = am schnellsten."),
        ("Boşluğa hangisi gelmeli? \"Berlin, Hamburg'dan ___ kalabalıktır.\"", new[] { "en", "daha", "kadar" }, "daha", "\"-den ... daha\" iki şeyi karşılaştırır (Komparativ): Hamburg'dan daha kalabalık = voller als Hamburg."),
        ("Boşluğa hangisi gelmeli? \"Ben de senin ___ güçlüyüm.\"", new[] { "daha", "en", "kadar" }, "kadar", "\"kadar\" eşitliği anlatır: senin kadar güçlü = so stark wie du."),
        ("Boşluğa hangisi gelmeli? \"Fil, fareden daha ___.\"", new[] { "küçüktür", "ağırdır", "hafiftir" }, "ağırdır", "Fil, fareden çok daha büyüktür, bu yüzden daha ağırdır."),
        ("Hangisi iki şeyi karşılaştırır? (Komparativ: schöner)", new[] { "en güzel", "çok güzel", "daha güzel" }, "daha güzel", "\"daha\" = mehr/-er: daha güzel = schöner. \"en güzel\" Superlativ, \"çok güzel\" sadece \"sehr schön\" demektir."),
        ("Hangisi en üstünlük bildirir? (Superlativ: am süßesten)", new[] { "daha tatlı", "en tatlı", "pek tatlı" }, "en tatlı", "\"en\" = am meisten / der, die, das ...-ste: en tatlı = am süßesten."),
        ("Hangi cümle doğrudur?", new[] { "Kedim köpeğime daha küçük.", "Kedim köpeğimde daha küçük.", "Kedim köpeğimden daha küçük." }, "Kedim köpeğimden daha küçük.", "Karşılaştırılan kelime -den/-dan alır: köpeğimden daha küçük (kleiner als mein Hund)."),
        ("Boşluğa hangisi gelmeli? \"Everest, dünyanın ___ yüksek dağıdır.\"", new[] { "kadar", "en", "daha" }, "en", "Dünyadaki bütün dağlar arasında birinci: en yüksek dağ (der höchste Berg)."),
        ("Boşluğa hangisi gelmeli? \"Kardeşim benden iki yaş ___.\"", new[] { "küçüğüm", "küçükten", "küçük" }, "küçük", "\"benden\" karşılaştırmayı zaten gösterir; sıfat ek almaz: benden iki yaş küçük."),
        ("Boşluğa hangisi gelmeli? \"Bugün dünden ___ soğuk.\"", new[] { "daha", "kadar", "en" }, "daha", "\"dünden\" (ayrılma hâli) iki günü karşılaştırır, ardından \"daha\" gelir: dünden daha soğuk."),
        ("Hangisi \"so groß wie ein Haus\" demektir?", new[] { "evden büyük", "ev kadar büyük", "en büyük ev" }, "ev kadar büyük", "\"kadar\" = so ... wie: ev kadar büyük. \"evden büyük\" = größer als ein Haus."),
        ("Boşluğa hangisi gelmeli? \"Bu soru öbüründen ___.\" (leichter)", new[] { "en kolay", "daha kolay", "çok kolay" }, "daha kolay", "\"öbüründen\" ile iki soru karşılaştırılıyor - Komparativ: daha kolay = leichter."),
        ("Hangi cümle yanlıştır?", new[] { "Bu oda en büyük odadır.", "Bu oda daha büyükten.", "Bu oda çok büyüktür." }, "Bu oda daha büyükten.", "-den eki sıfata değil, karşılaştırılan şeye gelir. Doğrusu: \"Bu oda ondan daha büyük.\""),
        ("Leyla ile Deniz aynı boyda. Hangisi doğrudur?", new[] { "Leyla, Deniz'den daha uzun.", "Leyla, Deniz kadar uzun.", "Leyla, en uzun kişidir." }, "Leyla, Deniz kadar uzun.", "Aynı boydalar - eşitlik \"kadar\" ile anlatılır: Deniz kadar uzun (so groß wie Deniz)."),
        ("Hangisi \"der schnellste Zug\" demektir?", new[] { "daha hızlı tren", "hızlı tren kadar", "en hızlı tren" }, "en hızlı tren", "Superlativ \"en\" ile kurulur: en hızlı tren. \"daha hızlı tren\" = schnellerer Zug."),
        ("Boşluğa hangisi gelmeli? \"Çay, kahve___ daha ucuz.\"", new[] { "kahveye", "kahveden", "kahvede" }, "kahveden", "Karşılaştırılan şey ayrılma hâli alır: kahveden daha ucuz (billiger als Kaffee)."),
        ("\"Babam annemden daha erken kalkar.\" Kim daha erken kalkar?", new[] { "Annem", "İkisi aynı saatte", "Babam" }, "Babam", "\"-den\" alan kelime (annemden) karşılaştırılan kişidir; daha erken kalkan \"babam\"dır."),
        ("\"Pizza, döner kadar lezzetli.\" cümlesi ne anlatır?", new[] { "Pizza daha lezzetli", "İkisi eşit derecede lezzetli", "Döner daha lezzetli" }, "İkisi eşit derecede lezzetli", "\"kadar\" eşitliği gösterir: pizza da döner de aynı derecede lezzetli."),
        ("\"Mehmet sınıftaki en uzun öğrenci.\" Buna göre hangisi doğrudur?", new[] { "Sınıfta Mehmet'ten daha kısa öğrenci yok.", "Mehmet sınıfın en kısa öğrencisi.", "Sınıfta Mehmet'ten daha uzun öğrenci yok." }, "Sınıfta Mehmet'ten daha uzun öğrenci yok.", "\"en uzun\" = der Größte: Sınıfta ondan daha uzun kimse yoktur.")
    };

    private static QuizQuestion SifatlardaKarsilastirma(Random r)
    {
        var f = KarsilastirmaListe[r.Next(KarsilastirmaListe.Length)];
        return new QuizQuestion
        {
            Id = NewId(), Subject = Subject.Tuerkisch, GradeLevel = GradeLevel.Klasse6,
            Topic = "Sıfatlarda Karşılaştırma: daha, en, kadar (Komparativ/Superlativ)", Type = QuestionType.MultipleChoice,
            Prompt = f.Frage, Options = f.Optionen.OrderBy(_ => r.Next()).ToArray(), CorrectAnswers = new[] { f.Antwort }, Explanation = f.Erklaerung,
            HelpHint = "daha = -er (daha büyük = größer), en = am ...-sten (en büyük = am größten), kadar = so ... wie. Verglichenes bekommt -den/-dan: Ali'den daha büyük."
        };
    }


    // ================= Klasse 7: Erweiterung 30.09.2026 =================
    // Türkisch ist durch die Fächerauswahl nach Stundenplan JEDEN Tag dabei - der K7-Pool hatte nur
    // 120 Fragen. Neu: Zarflar, Zamirler, Şart kipi, Gereklilik kipi, Yapım ekleri/Birleşik kelimeler,
    // Hikâye unsurları und Alltagswortschatz "Berlin'de günlük yaşam".

    private static readonly (string Frage, string[] Optionen, string Antwort, string Erklaerung)[] ZarflarListe =
    {
        ("\"Yarın Hamburg'a gideceğiz.\" cümlesindeki zaman zarfı hangisidir?", new[] { "Hamburg'a", "Yarın", "gideceğiz" }, "Yarın",
            "\"Yarın\" fiilin NE ZAMAN yapılacağını bildirir, bu yüzden zaman zarfıdır. \"Hamburg'a\" ek almış bir isimdir (yönelme hâli)."),
        ("\"Kedi yukarı çıktı.\" cümlesindeki yer-yön zarfı hangisidir?", new[] { "Kedi", "çıktı", "yukarı" }, "yukarı",
            "\"yukarı\" hareketin YÖNÜNÜ bildirir ve ek almamıştır - bu yüzden yer-yön zarfıdır (içeri, dışarı, aşağı, ileri, geri gibi)."),
        ("\"Ali sessizce odaya girdi.\" cümlesinde \"sessizce\" hangi tür zarftır?", new[] { "Zaman zarfı", "Durum zarfı", "Miktar zarfı" }, "Durum zarfı",
            "\"Nasıl girdi?\" sorusunun cevabı \"sessizce\"dir. Fiilin NASIL yapıldığını bildiren zarflar durum zarfıdır."),
        ("\"Bu yaz çok sıcak oldu.\" cümlesinde \"çok\" hangi tür zarftır?", new[] { "Durum zarfı", "Miktar zarfı", "Soru zarfı" }, "Miktar zarfı",
            "\"çok\" sıfatın (sıcak) derecesini, yani NE KADAR sıcak olduğunu bildirir. Bu tür zarflara miktar (azlık-çokluk) zarfı denir."),
        ("\"Tatile ne zaman gidiyorsunuz?\" cümlesinde \"ne zaman\" hangi tür zarftır?", new[] { "Zaman zarfı", "Durum zarfı", "Soru zarfı" }, "Soru zarfı",
            "\"ne zaman\" zamanı SORAR, bir zaman bildirmez. Soru yoluyla fiili niteleyen zarflar soru zarfıdır (nasıl, niçin, ne zaman, ne kadar)."),
        ("\"Öğretmen bizi dışarı çağırdı.\" cümlesindeki yer-yön zarfı hangisidir?", new[] { "dışarı", "bizi", "Öğretmen" }, "dışarı",
            "\"dışarı\" ek almadan fiilin yönünü bildirir. Dikkat: \"dışarıda\" gibi ek alınca artık zarf değil, hâl eki almış bir isim olur."),
        ("\"Dedem her sabah erken kalkar.\" cümlesinde \"erken\" hangi tür zarftır?", new[] { "Yer-yön zarfı", "Miktar zarfı", "Zaman zarfı" }, "Zaman zarfı",
            "\"Ne zaman kalkar?\" - \"erken\". Erken, geç, şimdi, dün, yarın gibi kelimeler zaman zarfıdır."),
        ("\"Kardeşim yemeğini yavaş yavaş yedi.\" cümlesindeki durum zarfı hangisidir?", new[] { "yemeğini", "Kardeşim", "yavaş yavaş" }, "yavaş yavaş",
            "\"Nasıl yedi?\" - \"yavaş yavaş\". İkilemeler de fiilin nasıl yapıldığını anlatınca durum zarfı olur."),
        ("\"Bu film biraz uzun.\" cümlesinde \"biraz\" hangi tür zarftır?", new[] { "Durum zarfı", "Zaman zarfı", "Miktar zarfı" }, "Miktar zarfı",
            "\"biraz\" sıfatın (uzun) derecesini azaltır. Az, biraz, çok, pek, daha, en, oldukça miktar zarflarıdır."),
        ("Zarflar hangi kelimelerin anlamını niteler ya da derecesini belirtir?", new[] { "Yalnızca isimleri, zamirleri ve özel adları", "Fiilleri, sıfatları ve başka zarfları", "Sadece bağlaçları ve edatları" }, "Fiilleri, sıfatları ve başka zarfları",
            "Zarflar fiilleri (hızlı koştu), sıfatları (çok güzel) ve başka zarfları (daha hızlı) niteler. İsimleri niteleyen kelimeler ise sıfattır."),
        ("\"Neden bu kadar geç kaldın?\" cümlesinde \"Neden\" hangi tür zarftır?", new[] { "Soru zarfı", "Zaman zarfı", "Durum zarfı" }, "Soru zarfı",
            "\"Neden\" fiilin sebebini SORAR. Neden, niçin, nasıl, ne zaman, ne kadar soru zarflarıdır."),
        ("\"Bu sabah otobüs geç geldi.\" cümlesinde \"geç\" hangi tür zarftır?", new[] { "Durum zarfı", "Miktar zarfı", "Zaman zarfı" }, "Zaman zarfı",
            "\"Otobüs ne zaman geldi?\" - \"geç\". Erken ve geç, fiilin zamanını bildirdikleri için zaman zarfıdır."),
        ("\"Merdivenden aşağı indik.\" cümlesinde \"aşağı\" hangi tür zarftır?", new[] { "Yer-yön zarfı", "Durum zarfı", "Zaman zarfı" }, "Yer-yön zarfı",
            "\"aşağı\" ek almadan hareketin yönünü bildirir. Aşağı, yukarı, içeri, dışarı, ileri, geri yer-yön zarflarıdır."),
        ("Hangisi bir zaman zarfıdır?", new[] { "şimdi", "yukarı", "nasıl" }, "şimdi",
            "\"şimdi\" fiilin zamanını bildirir. \"yukarı\" yer-yön zarfı, \"nasıl\" ise soru zarfıdır."),
        ("Hangisi bir soru zarfıdır?", new[] { "hızla", "nasıl", "dışarı" }, "nasıl",
            "\"nasıl\" fiilin nasıl yapıldığını sorar (Nasıl geldin?). \"hızla\" durum zarfı, \"dışarı\" yer-yön zarfıdır."),
        ("Hangisi bir miktar (azlık-çokluk) zarfıdır?", new[] { "ileri", "akşam", "oldukça" }, "oldukça",
            "\"oldukça\" bir niteliğin derecesini bildirir (oldukça zor). \"ileri\" yer-yön, \"akşam\" zaman zarfıdır."),
        ("Hangi cümlede \"güzel\" kelimesi zarf olarak kullanılmıştır?", new[] { "Güzel bir gün geçirdik.", "Bu elbise çok güzel.", "Kardeşim güzel konuşur." }, "Kardeşim güzel konuşur.",
            "\"Nasıl konuşur?\" - \"güzel\". Burada \"güzel\" fiili niteler, yani zarftır. \"Güzel bir gün\"de ise ismi (gün) niteleyen bir sıfattır."),
        ("Hangi cümlede \"hızlı\" kelimesi zarf olarak kullanılmıştır?", new[] { "Hızlı bir araba aldık.", "Tren çok hızlı gidiyor.", "Hızlı adam yoruldu." }, "Tren çok hızlı gidiyor.",
            "\"Nasıl gidiyor?\" - \"hızlı\". Fiili nitelediği için zarftır. Diğer cümlelerde \"hızlı\" bir ismi (araba, adam) niteleyen sıfattır."),
        ("\"Akşam eve dönünce ödevimi yaptım.\" cümlesindeki zaman zarfı hangisidir?", new[] { "eve", "ödevimi", "Akşam" }, "Akşam",
            "\"Ne zaman yaptım?\" - \"Akşam\". Ek almadan zaman bildiren \"akşam, sabah, gece\" zaman zarfıdır; \"eve\" ise yönelme hâli almış bir isimdir."),
        ("\"Kitabı nasıl buldun?\" cümlesindeki soru zarfı hangisidir?", new[] { "Kitabı", "nasıl", "buldun" }, "nasıl",
            "\"nasıl\" fiilin (buldun) durumunu sorar. Bu yüzden soru zarfıdır.")
    };

    private static QuizQuestion ZarflarK7(Random r)
    {
        var f = ZarflarListe[r.Next(ZarflarListe.Length)];
        return new QuizQuestion
        {
            Id = NewId(), Subject = Subject.Tuerkisch, GradeLevel = GradeLevel.Klasse7,
            Topic = "Zarflar (Adverbien)", Type = QuestionType.MultipleChoice,
            Prompt = f.Frage, Options = f.Optionen, CorrectAnswers = new[] { f.Antwort }, Explanation = f.Erklaerung,
            HelpHint = "Zarf fiili, sıfatı ya da başka bir zarfı niteler. Ne zaman? -> zaman zarfı (yarın). Nasıl? -> durum zarfı (sessizce). Nereye? (eksiz) -> yer-yön zarfı (dışarı). Ne kadar? -> miktar zarfı (çok). Soru soran -> soru zarfı (nasıl)."
        };
    }

    private static readonly (string Frage, string[] Optionen, string Antwort, string Erklaerung)[] ZamirlerListe =
    {
        ("\"Biz yarın müzeye gidiyoruz.\" cümlesindeki kişi zamiri hangisidir?", new[] { "müzeye", "Biz", "yarın" }, "Biz",
            "\"Biz\" konuşan kişileri gösterir ve bir ismin yerini tutar. Kişi zamirleri: ben, sen, o, biz, siz, onlar."),
        ("\"Bunu kim yaptı?\" cümlesinde \"kim\" hangi tür zamirdir?", new[] { "Kişi zamiri", "Soru zamiri", "İşaret zamiri" }, "Soru zamiri",
            "\"kim\" bir ismin yerine geçer ve onu SORAR. Soru zamirleri: kim, ne, hangisi, kaçı, nere(si)."),
        ("\"Şunu bana uzatır mısın?\" cümlesinde \"Şunu\" hangi tür zamirdir?", new[] { "Kişi zamiri", "Soru zamiri", "İşaret zamiri" }, "İşaret zamiri",
            "\"şu\" bir varlığı işaret ederek onun adının yerini tutar. Ek aldığı ve yanında isim olmadığı için işaret zamiridir."),
        ("Hangi cümlede \"bu\" kelimesi zamir olarak kullanılmıştır?", new[] { "Bu kalem benim.", "Bu benim kalemim.", "Bu çanta çok ağır." }, "Bu benim kalemim.",
            "\"Bu benim kalemim.\" cümlesinde \"bu\" tek başına bir varlığın yerini tutar - zamirdir. \"Bu kalem\", \"bu çanta\"da ise ismin önünde durup onu gösterir; o zaman işaret sıfatıdır."),
        ("\"ben\" zamirinin çoğulu hangisidir?", new[] { "siz", "biz", "onlar" }, "biz",
            "Tekil ben - sen - o, çoğul biz - siz - onlar. \"ben\"in çoğulu \"biz\"dir."),
        ("\"o\" kişi zamirinin çoğulu hangisidir?", new[] { "sizler", "bizler", "onlar" }, "onlar",
            "\"o\" (er/sie/es) çoğul olunca \"onlar\" (sie) olur."),
        ("Almancadaki \"ihr\" zamiri Türkçede hangisiyle karşılanır?", new[] { "siz", "biz", "onlar" }, "siz",
            "\"ihr\" = siz. \"siz\" aynı zamanda kibar hitap olan \"Sie\" anlamında da kullanılır."),
        ("\"Herkes bahçede toplandı.\" cümlesinde \"Herkes\" hangi tür zamirdir?", new[] { "Belgisiz zamir", "Kişi zamiri", "Soru zamiri" }, "Belgisiz zamir",
            "\"Herkes\" kimlerin toplandığını tam olarak belirtmez. Belli olmayan varlıkların yerini tutan zamirler belgisiz zamirdir (herkes, biri, bazıları, hiçbiri)."),
        ("\"Ödevini kendin yap!\" cümlesinde \"kendin\" hangi tür zamirdir?", new[] { "İşaret zamiri", "Dönüşlülük zamiri", "Belgisiz zamir" }, "Dönüşlülük zamiri",
            "\"kendi\" zamiri işi yapan kişiyi vurgular ve ona geri döner. Bu yüzden dönüşlülük zamiri denir (kendim, kendin, kendisi)."),
        ("Boşluğa hangi zamir gelmeli? \"___ Berlin'de yaşıyorum.\"", new[] { "Sen", "Biz", "Ben" }, "Ben",
            "Fiildeki \"-um\" kişi eki birinci tekil kişiyi gösterir (yaşıyor-um). Bu yüzden özne \"Ben\" olmalıdır."),
        ("Boşluğa hangi zamir gelmeli? \"___ hangi okula gidiyorsunuz?\"", new[] { "Siz", "Biz", "Onlar" }, "Siz",
            "\"-sunuz\" kişi eki ikinci çoğul kişiyi gösterir (gidiyor-sunuz). Özne \"Siz\" olmalıdır."),
        ("Boşluğa hangi zamir gelmeli? \"___ dün maçı kazandılar.\"", new[] { "Bizler", "Onlar", "Sizler" }, "Onlar",
            "\"-lar\" kişi eki üçüncü çoğul kişiyi gösterir (kazandı-lar). Özne \"Onlar\" olmalıdır."),
        ("\"Masadaki kitaplardan hangisi senin?\" cümlesinde \"hangisi\" hangi tür zamirdir?", new[] { "İşaret zamiri", "Soru zamiri", "Kişi zamiri" }, "Soru zamiri",
            "\"hangisi\" kitaplardan birinin yerini tutar ve onu sorar - soru zamiridir."),
        ("\"Bunlar çok eski fotoğraflar.\" cümlesinde \"Bunlar\" hangi tür zamirdir?", new[] { "Soru zamiri", "İşaret zamiri", "Kişi zamiri" }, "İşaret zamiri",
            "\"Bunlar\" fotoğrafları işaret eder ve onların yerini tutar. \"bu\" çoğul eki alınca da işaret zamiri olarak kalır."),
        ("Zamir nedir?", new[] { "İsmi niteleyen kelime", "Fiile gelen zaman eki", "İsmin yerini tutan kelime" }, "İsmin yerini tutan kelime",
            "Zamir (Pronomen) bir ismin yerine kullanılır: \"Ayşe geldi.\" -> \"O geldi.\" İsmi niteleyen kelime ise sıfattır."),
        ("\"Bazıları erken geldi.\" cümlesinde \"Bazıları\" hangi tür zamirdir?", new[] { "Kişi zamiri", "Belgisiz zamir", "İşaret zamiri" }, "Belgisiz zamir",
            "\"Bazıları\" kimlerin geldiğini açıkça söylemez. Belirsiz varlıkların yerini tuttuğu için belgisiz zamirdir."),
        ("Hangi cümlede \"o\" bir kişi zamiridir?", new[] { "O ev çok eski.", "O kalemi bana ver.", "O çok iyi futbol oynar." }, "O çok iyi futbol oynar.",
            "\"O çok iyi futbol oynar.\" cümlesinde \"o\" bir kişinin (er/sie) yerini tutar. \"O ev\", \"o kalem\"de ise ismin önünde işaret sıfatıdır."),
        ("\"kim\" soru zamirine yönelme hâli eki (-e) gelince hangisi olur?", new[] { "kimi", "kime", "kimde" }, "kime",
            "kim + e = kime (Wem?). \"kimi\" belirtme hâli (Wen?), \"kimde\" bulunma hâlidir (Bei wem?)."),
        ("\"ben\" zamirine yönelme hâli eki (-e) gelince hangisi olur?", new[] { "bana", "bene", "beni" }, "bana",
            "\"ben\" ve \"sen\" yönelme hâlinde düzensizdir: ben + e = bana, sen + e = sana. \"beni\" ise belirtme hâlidir."),
        ("\"biz\" zamirine ilgi eki gelince hangisi olur?", new[] { "bizin", "bizim", "bizün" }, "bizim",
            "\"biz\" ve \"ben\" ilgi ekini \"-im\" olarak alır: bizim, benim. Diğerleri düzenlidir: senin, onun, sizin.")
    };

    private static QuizQuestion ZamirlerK7(Random r)
    {
        var f = ZamirlerListe[r.Next(ZamirlerListe.Length)];
        return new QuizQuestion
        {
            Id = NewId(), Subject = Subject.Tuerkisch, GradeLevel = GradeLevel.Klasse7,
            Topic = "Zamirler (Pronomen)", Type = QuestionType.MultipleChoice,
            Prompt = f.Frage, Options = f.Optionen, CorrectAnswers = new[] { f.Antwort }, Explanation = f.Erklaerung,
            HelpHint = "Zamir ismin yerini tutar. Kişi: ben, sen, o, biz, siz, onlar. İşaret: bu, şu, o (yanında isim yoksa). Soru: kim, ne, hangisi. Belgisiz: herkes, biri, bazıları. Dönüşlülük: kendi."
        };
    }

    private static readonly (string Fiil, string Sart)[] SartKipiBeispiele =
    {
        ("gelmek", "gelse"), ("almak", "alsa"), ("gitmek", "gitse"), ("okumak", "okusa"),
        ("yazmak", "yazsa"), ("görmek", "görse"), ("içmek", "içse"), ("bilmek", "bilse"),
        ("çalışmak", "çalışsa"), ("oynamak", "oynasa"), ("uyumak", "uyusa"), ("konuşmak", "konuşsa"),
        ("düşünmek", "düşünse"), ("beklemek", "beklese"), ("dinlemek", "dinlese"), ("anlamak", "anlasa"),
        ("yemek", "yese"), ("istemek", "istese"), ("koşmak", "koşsa"), ("kazanmak", "kazansa")
    };

    private static QuizQuestion SartKipiK7(Random r)
    {
        var v = SartKipiBeispiele[r.Next(SartKipiBeispiele.Length)];

        return new QuizQuestion
        {
            Id = NewId(), Subject = Subject.Tuerkisch, GradeLevel = GradeLevel.Klasse7,
            Topic = "Şart Kipi -se/-sa (Bedingung: wenn/falls)", Type = QuestionType.OpenText,
            Prompt = $"\"{v.Fiil}\" fiilinin (o için) şart kipi hâlini yaz. (Beispiel: bakmak -> baksa)",
            CorrectAnswers = new[] { v.Sart },
            Explanation = $"\"{v.Fiil}\" -> \"{v.Sart}\". Şart kipi fiil köküne \"-se/-sa\" eklenerek kurulur. Kökteki son ünlü e, i, ö, ü ise " +
                          "\"-se\", a, ı, o, u ise \"-sa\" gelir (büyük ünlü uyumu). Almancada çoğu zaman \"wenn/falls\" ile karşılanır.",
            HelpHint = "Şart kipi = fiil kökü + -se/-sa: gel-se, oku-sa. Örnek: \"Keşke Ali bugün gelse!\" (Wenn Ali doch heute käme!)"
        };
    }

    private static readonly (string Fiil, string Gereklilik)[] GereklilikKipiBeispiele =
    {
        ("okumak", "okumalı"), ("yazmak", "yazmalı"), ("çalışmak", "çalışmalı"), ("öğrenmek", "öğrenmeli"),
        ("dinlemek", "dinlemeli"), ("sormak", "sormalı"), ("tekrarlamak", "tekrarlamalı"), ("beklemek", "beklemeli"),
        ("uyumak", "uyumalı"), ("gelmek", "gelmeli"), ("gitmek", "gitmeli"), ("yemek", "yemeli"),
        ("içmek", "içmeli"), ("temizlemek", "temizlemeli"), ("yıkamak", "yıkamalı"), ("söylemek", "söylemeli"),
        ("düşünmek", "düşünmeli"), ("koşmak", "koşmalı"), ("bitirmek", "bitirmeli"), ("toplamak", "toplamalı")
    };

    private static QuizQuestion GereklilikKipiK7(Random r)
    {
        var v = GereklilikKipiBeispiele[r.Next(GereklilikKipiBeispiele.Length)];

        return new QuizQuestion
        {
            Id = NewId(), Subject = Subject.Tuerkisch, GradeLevel = GradeLevel.Klasse7,
            Topic = "Gereklilik Kipi -meli/-malı (müssen/sollen)", Type = QuestionType.OpenText,
            Prompt = $"\"{v.Fiil}\" fiilinin (o için) gereklilik kipi hâlini yaz. (Beispiel: bakmak -> bakmalı)",
            CorrectAnswers = new[] { v.Gereklilik },
            Explanation = $"\"{v.Fiil}\" -> \"{v.Gereklilik}\". Gereklilik kipi fiil köküne \"-meli/-malı\" eklenerek kurulur. Kökteki son ünlü " +
                          "e, i, ö, ü ise \"-meli\", a, ı, o, u ise \"-malı\" gelir. Anlamı Almancadaki \"muss/soll\" gibidir: bir işin yapılması gerekir.",
            HelpHint = "Gereklilik kipi = fiil kökü + -meli/-malı: öğren-meli, oku-malı. Örnek: \"Her gün kitap okumalı.\" (Er/Sie sollte jeden Tag lesen.)"
        };
    }

    private static readonly (string Frage, string[] Optionen, string Antwort, string Erklaerung)[] YapimEkleriListe =
    {
        ("Balık satan kişiye ne denir?", new[] { "balıklı", "balıkçı", "balıksız" }, "balıkçı",
            "\"-cı/-ci/-çı/-çi\" yapım eki meslek bildiren yeni kelimeler türetir: balık + çı = balıkçı. \"k\" sert ünsüz olduğu için \"c\" değil \"ç\" gelir."),
        ("\"gözlük\" kelimesindeki yapım eki hangisidir?", new[] { "-ük", "-lü", "-lük" }, "-lük",
            "göz + lük = gözlük. \"-lik/-lık/-luk/-lük\" eki bir işe yarayan eşya adları türetir (tuzluk, kitaplık)."),
        ("\"tuzsuz\" kelimesi ne anlama gelir?", new[] { "İçinde çok tuz olan", "İçinde tuz olmayan", "Tuz satan kişi" }, "İçinde tuz olmayan",
            "\"-sız/-siz/-suz/-süz\" eki yokluk bildirir: tuz + suz = tuzsuz (ohne Salz)."),
        ("\"Berlinli\" kelimesi ne anlama gelir?", new[] { "Berlin'e kısa süreliğine gelen turist", "Berlin'de doğup büyüyen, oralı kişi", "Berlin'de satılan bir tür yiyecek" }, "Berlin'de doğup büyüyen, oralı kişi",
            "\"-lı/-li/-lu/-lü\" eki bir yere ait olmayı bildirir: Berlin + li = Berlinli (Berliner), İzmir + li = İzmirli."),
        ("Hangisi birleşik bir kelimedir?", new[] { "kitaplık", "ayakkabı", "gözlükçü" }, "ayakkabı",
            "\"ayakkabı\" iki kelimeden oluşur: ayak + kabı. Birleşik kelimeler iki ya da daha fazla kelimenin birleşmesiyle oluşur; \"kitaplık\" ve \"gözlükçü\" ise ek alarak türemiştir."),
        ("\"bilgisayar\" kelimesi hangi parçalardan oluşur?", new[] { "bil + gisayar", "bilgis + ayar", "bilgi + sayar" }, "bilgi + sayar",
            "bilgisayar = bilgi + sayar (Computer, wörtlich \"Informationszähler\"). İki kelime birleşerek yeni bir anlam kazanmıştır."),
        ("Hangisinde \"-lık/-lik\" eki bir eşya adı (raf/dolap) yapmıştır?", new[] { "kitaplık", "iyilik", "çocukluk" }, "kitaplık",
            "\"kitaplık\" kitapların konulduğu rafı bildirir. \"iyilik\" ve \"çocukluk\" ise soyut kavramlardır - aynı ek farklı anlamlar katabilir."),
        ("\"sütçü\" kelimesindeki \"-çü\" eki ne anlam katar?", new[] { "Bir şeyin olmadığını", "Meslek (o işi yapan kişi)", "Bir yerden olmayı" }, "Meslek (o işi yapan kişi)",
            "süt + çü = sütçü (Milchmann). \"-cı/-ci/-cu/-cü\" ve sert ünsüzden sonra \"-çı/-çi/-çu/-çü\" meslek ya da uğraş bildirir."),
        ("Hangi kelimede anlamı değiştirmeyen bir çekim eki vardır?", new[] { "kitapçı", "kitaplar", "kitaplık" }, "kitaplar",
            "Çoğul eki \"-lar\" bir çekim ekidir: kelimenin anlamı aynı kalır, sadece sayısı değişir. \"-çı\" ve \"-lık\" ise yeni kelime türeten yapım ekleridir."),
        ("\"akıllı\" kelimesinin zıt anlamlısı hangi ekle kurulur?", new[] { "akıllılık", "akılsız", "akılcı" }, "akılsız",
            "\"-lı\" eki varlık, \"-sız\" eki yokluk bildirir: akıllı <-> akılsız, şekerli <-> şekersiz."),
        ("Hangisi yapım eki almış türemiş bir kelimedir?", new[] { "yollar", "yolda", "yolcu" }, "yolcu",
            "yol + cu = yolcu (Reisender): yeni anlamlı bir kelime türemiştir. \"yollar\" ve \"yolda\" ise çekim eki almıştır."),
        ("\"şekerli\" kelimesinin kökü hangisidir?", new[] { "şek", "şeker", "şekerle" }, "şeker",
            "Kök, kelimenin eklerden arındırılmış en küçük anlamlı parçasıdır: şeker + li = şekerli."),
        ("Hangisi birleşik kelime DEĞİLDİR?", new[] { "buzdolabı", "kahverengi", "çiçekçi" }, "çiçekçi",
            "\"çiçekçi\" = çiçek + çi, yani ek alarak türemiştir. \"buzdolabı\" (buz + dolabı) ve \"kahverengi\" (kahve + rengi) iki kelimeden oluşur."),
        ("\"-sız/-siz\" eki kelimeye hangi anlamı katar?", new[] { "Varlık (bir şeyin olması)", "Yokluk (bir şeyin olmaması)", "Meslek (bir işi yapmak)" }, "Yokluk (bir şeyin olmaması)",
            "\"-sız/-siz/-suz/-süz\" yokluk bildirir: susuz, parasız, evsiz. Tersi \"-lı/-li\" varlık bildirir."),
        ("\"kitaplık\" kelimesi ne anlama gelir?", new[] { "Kitap satan kişinin çalıştığı dükkân", "Çok sayıda kitap okumuş kişi", "Kitapların konulduğu raf ya da dolap" }, "Kitapların konulduğu raf ya da dolap",
            "kitap + lık = kitaplık (Bücherregal). Kitap satılan dükkân ise \"kitapçı\"dır."),
        ("Hangi kelime \"göz\" köküne iki yapım eki gelerek oluşmuştur?", new[] { "gözlük", "gözlükçü", "gözler" }, "gözlükçü",
            "göz + lük = gözlük, gözlük + çü = gözlükçü (Optiker). Bir kelimeye birden fazla yapım eki gelebilir."),
        ("\"cumartesi\" nasıl bir kelimedir?", new[] { "Türemiş kelime (cuma + -si eki)", "Birleşik kelime (cuma + ertesi)", "Basit kelime (hiç ek almamış)" }, "Birleşik kelime (cuma + ertesi)",
            "cumartesi = cuma + ertesi (der Tag nach Freitag). İki kelime birleşirken bir ses düşmüştür."),
        ("Boşluğa hangisi gelmeli? \"Annem çayını ___ içer, hiç şeker koymaz.\"", new[] { "şekerli", "şekerci", "şekersiz" }, "şekersiz",
            "\"hiç şeker koymaz\" yokluk anlatır, bu yüzden \"-siz\" ekli \"şekersiz\" gelmelidir."),
        ("Hangi kelime bir kişinin nereli olduğunu bildirir?", new[] { "Adana'da", "Adanalı", "Adana'ya" }, "Adanalı",
            "\"-lı/-li\" eki bir yere ait olmayı bildirir: Adanalı = Adana'dan olan kişi. Diğerleri hâl eki almış yer adlarıdır."),
        ("\"denizci\" kelimesi ne anlama gelir?", new[] { "Denizde yüzmeyi çok seven turist", "Denizde çalışan, gemicilik yapan kişi", "Denize yakın bir yerde oturan kişi" }, "Denizde çalışan, gemicilik yapan kişi",
            "deniz + ci = denizci (Seemann). \"-ci\" eki burada meslek bildirir.")
    };

    private static QuizQuestion YapimEkleriVeBirlesikKelimelerK7(Random r)
    {
        var f = YapimEkleriListe[r.Next(YapimEkleriListe.Length)];
        return new QuizQuestion
        {
            Id = NewId(), Subject = Subject.Tuerkisch, GradeLevel = GradeLevel.Klasse7,
            Topic = "Yapım Ekleri ve Birleşik Kelimeler (Wortbildung)", Type = QuestionType.MultipleChoice,
            Prompt = f.Frage, Options = f.Optionen, CorrectAnswers = new[] { f.Antwort }, Explanation = f.Erklaerung,
            HelpHint = "Yapım eki yeni kelime türetir: -ci (meslek: balıkçı), -lik (eşya: gözlük), -li (sahip/nereli: tuzlu, Berlinli), -siz (yokluk: tuzsuz). Birleşik kelime = iki kelime (ayakkabı, bilgisayar)."
        };
    }

    private static readonly (string Frage, string[] Optionen, string Antwort, string Erklaerung)[] HikayeUnsurlariListe =
    {
        ("Bir hikâyenin dört temel unsuru hangileridir?", new[] { "Olay, kişiler, yer ve zaman", "Başlık, sayfa, yazar ve yayınevi", "Giriş, dipnot, kaynak ve özet" }, "Olay, kişiler, yer ve zaman",
            "Her hikâyede bir olay yaşanır; bu olay belli kişilerin başından belli bir yerde ve zamanda geçer. Bunlar hikâyenin dört temel unsurudur."),
        ("\"Geçen yaz Elif, Antalya'daki kampta kaybolan köpeğini aradı.\" cümlesinde yer unsuru hangisidir?", new[] { "Geçen yaz", "Antalya'daki kampta", "Elif" }, "Antalya'daki kampta",
            "\"Nerede?\" sorusunun cevabı yer unsurunu verir: Antalya'daki kampta."),
        ("\"Geçen yaz Elif, Antalya'daki kampta kaybolan köpeğini aradı.\" cümlesinde zaman unsuru hangisidir?", new[] { "Elif", "Antalya'daki kampta", "Geçen yaz" }, "Geçen yaz",
            "\"Ne zaman?\" sorusunun cevabı zaman unsurunu verir: Geçen yaz."),
        ("\"Geçen yaz Elif, Antalya'daki kampta kaybolan köpeğini aradı.\" cümlesinde ana kahraman kimdir?", new[] { "Elif", "kamp", "Antalya" }, "Elif",
            "Olayı yaşayan ve harekete geçen kişi Elif'tir (köpeğini arayan odur). Olayın merkezindeki kişi ana kahramandır."),
        ("Hikâyede olayların birbirine bağlı biçimde sıralanmasına ne denir?", new[] { "Ana fikir", "Olay örgüsü", "Başlık" }, "Olay örgüsü",
            "Olay örgüsü (Handlung) olayların sebep-sonuç ilişkisiyle birbirine bağlanarak sıralanmasıdır."),
        ("Hikâyenin bölümleri doğru sırayla hangisidir?", new[] { "Sonuç - Giriş - Gelişme", "Giriş - Gelişme - Sonuç", "Gelişme - Sonuç - Giriş" }, "Giriş - Gelişme - Sonuç",
            "Hikâye girişle başlar (kişi ve yer tanıtılır), gelişmede olay ilerler, sonuçta sorun çözülür."),
        ("Hikâyede merakın ve heyecanın en yüksek olduğu bölüm hangisidir?", new[] { "Giriş bölümü", "Sonuç bölümü", "Gelişme bölümü" }, "Gelişme bölümü",
            "Gelişme bölümünde sorun büyür, olaylar düğümlenir - okurun merakı en çok burada artar."),
        ("Kahramanların ve olayın geçtiği yerin tanıtıldığı bölüm hangisidir?", new[] { "Giriş bölümü", "Sonuç bölümü", "Gelişme bölümü" }, "Giriş bölümü",
            "Giriş bölümü okuru hikâyeye hazırlar: kim, nerede, ne zaman sorularının cevabı genellikle burada verilir."),
        ("Sorunun çözüldüğü ve hikâyenin bittiği bölüm hangisidir?", new[] { "Gelişme bölümü", "Sonuç bölümü", "Giriş bölümü" }, "Sonuç bölümü",
            "Sonuç bölümünde düğüm çözülür ve hikâye tamamlanır."),
        ("Yazarın okura vermek istediği temel düşünceye ne denir?", new[] { "Ana fikir", "Olay örgüsü", "Yardımcı kişi" }, "Ana fikir",
            "Ana fikir (Kernaussage), metnin okura iletmek istediği asıl mesajdır - örneğin \"Dostluk her şeyden değerlidir.\""),
        ("Hikâyede olayları okura aktaran \"ses\"e ne denir?", new[] { "Okuyucu", "Anlatıcı", "Dinleyici" }, "Anlatıcı",
            "Anlatıcı (Erzähler) olayları anlatan sestir. Yazar ile anlatıcı her zaman aynı kişi değildir."),
        ("\"Ben o gün çok korkmuştum.\" diye başlayan bir hikâyede anlatıcı nasıldır?", new[] { "Anlatıcı yoktur, sadece diyalog", "Birinci kişi anlatıcı (ben)", "Üçüncü kişi anlatıcı (o)" }, "Birinci kişi anlatıcı (ben)",
            "Anlatıcı olayları \"ben\" diye kendi başından geçmiş gibi anlatıyor. Bu, birinci kişi anlatıcıdır (Ich-Erzähler)."),
        ("\"Ali kapıyı açtı ve şaşırdı.\" diye anlatılan bir hikâyede anlatıcı nasıldır?", new[] { "Birinci kişi anlatıcı (ben)", "İkinci kişi anlatıcı (sen)", "Üçüncü kişi anlatıcı (o)" }, "Üçüncü kişi anlatıcı (o)",
            "Anlatıcı olayları dışarıdan, \"o\" diye anlatıyor. Bu, üçüncü kişi anlatıcıdır (Er/Sie-Erzähler)."),
        ("Hikâyede kahramanın bir engel ya da başka bir kişiyle yaşadığı mücadeleye ne denir?", new[] { "Diyalog", "Çatışma", "Tasvir" }, "Çatışma",
            "Çatışma (Konflikt) hikâyeyi ilerleten gerilimdir: iyi ile kötü, kahraman ile bir engel arasındaki mücadele."),
        ("Hikâyede kişilerin karşılıklı konuşmalarına ne denir?", new[] { "Özet", "Diyalog", "Başlık" }, "Diyalog",
            "Diyalog (Dialog) kişiler arasındaki konuşmadır; yazıda konuşma çizgisi ya da tırnak işaretiyle gösterilir."),
        ("Bir yerin ya da bir kişinin görünüşünün ayrıntılı anlatılmasına ne denir?", new[] { "Olay örgüsü", "Ana fikir", "Betimleme (tasvir)" }, "Betimleme (tasvir)",
            "Betimleme, okurun gözünde canlandırabilmesi için bir yeri ya da kişiyi renk, ses ve görüntüleriyle ayrıntılı anlatmaktır."),
        ("Masallarda zaman genellikle nasıl verilir?", new[] { "Tam tarih verilir: 12 Mart 2025", "Belirsizdir: \"Evvel zaman içinde\"", "Sadece saat verilir: sabah yedide" }, "Belirsizdir: \"Evvel zaman içinde\"",
            "Masallarda zaman ve yer belirsizdir: \"Evvel zaman içinde, kalbur saman içinde...\" Hikâyelerde ise zaman daha belirgindir."),
        ("Hikâyede olayların yaşandığı çevreye ne denir?", new[] { "Zaman", "Mekân (yer)", "Kahraman" }, "Mekân (yer)",
            "Olayların geçtiği yer mekândır (Schauplatz) - örneğin bir okul, bir köy ya da Berlin'de bir park."),
        ("Olaylarda başrol oynayan ana kişiye ne denir?", new[] { "Yardımcı kişi", "Anlatıcı", "Başkahraman" }, "Başkahraman",
            "Başkahraman (Hauptfigur) olayların merkezindeki kişidir. Ona yardım eden ya da karşı çıkan kişiler yardımcı kişilerdir."),
        ("Masallarda konuşan hayvanlar ve devler hangi özelliği gösterir?", new[] { "Gerçekçilik", "Olağanüstülük", "Güncellik" }, "Olağanüstülük",
            "Masallarda gerçek hayatta olmayan varlıklar ve olaylar bulunur - buna olağanüstülük denir. Hikâyede ise olaylar gerçek hayatta olabilecek türdendir.")
    };

    private static QuizQuestion HikayeUnsurlariK7(Random r)
    {
        var f = HikayeUnsurlariListe[r.Next(HikayeUnsurlariListe.Length)];
        return new QuizQuestion
        {
            Id = NewId(), Subject = Subject.Tuerkisch, GradeLevel = GradeLevel.Klasse7,
            Topic = "Hikâye Unsurları (Elemente einer Erzählung)", Type = QuestionType.MultipleChoice,
            Prompt = f.Frage, Options = f.Optionen, CorrectAnswers = new[] { f.Antwort }, Explanation = f.Erklaerung,
            HelpHint = "Hikâye unsurları: olay, kişiler (kahramanlar), yer (mekân), zaman. Bölümler: giriş - gelişme - sonuç. Anlatıcı \"ben\" diyorsa birinci kişi, \"o\" diyorsa üçüncü kişidir."
        };
    }

    private static readonly (string TurkceKelime, string Almanca, string[] Yanlislar)[] BerlinGunlukYasamListe =
    {
        ("doktor randevusu", "Arzttermin", new[] { "Arztpraxis", "Arztbrief", "Krankmeldung" }),
        ("eczane", "Apotheke", new[] { "Arztpraxis", "Drogerie", "Krankenhaus" }),
        ("reçete", "Rezept (vom Arzt)", new[] { "Überweisung (zum Facharzt)", "Krankschreibung", "Quittung" }),
        ("bekleme salonu", "Wartezimmer", new[] { "Sprechzimmer", "Empfang", "Warteschlange" }),
        ("aşı", "Impfung", new[] { "Tablette", "Verband", "Salbe" }),
        ("acil servis", "Notaufnahme", new[] { "Rettungswagen", "Intensivstation", "Wartezimmer" }),
        ("durak", "Haltestelle", new[] { "Bahnsteig", "Fahrplan", "Kreuzung" }),
        ("aktarma yapmak", "umsteigen", new[] { "aussteigen", "einsteigen", "abfahren" }),
        ("aylık bilet", "Monatskarte", new[] { "Tageskarte", "Einzelfahrschein", "Schülerausweis" }),
        ("gecikme", "Verspätung", new[] { "Abfahrt", "Ankunft", "Umleitung" }),
        ("bilet kontrolü", "Fahrkartenkontrolle", new[] { "Fahrkartenautomat", "Gepäckkontrolle", "Passkontrolle" }),
        ("randevu almak", "einen Termin vereinbaren", new[] { "einen Termin absagen", "einen Antrag stellen", "eine Frage stellen" }),
        ("başvuru formu", "Antragsformular", new[] { "Anmeldebestätigung", "Kontoauszug", "Stundenplan" }),
        ("kimlik kartı", "Personalausweis", new[] { "Führerschein", "Krankenkassenkarte", "Reisepass" }),
        ("kira", "Miete", new[] { "Nebenkosten", "Kaution", "Rechnung" }),
        ("ev sahibi", "Vermieter", new[] { "Mieter", "Hausmeister", "Nachbar" }),
        ("tercüman", "Dolmetscher", new[] { "Berater", "Sachbearbeiter", "Vorleser" }),
        ("iki dilli", "zweisprachig", new[] { "einsprachig", "fremdsprachig", "mehrteilig" }),
        ("ana dil", "Muttersprache", new[] { "Fremdsprache", "Amtssprache", "Zweitsprache" }),
        ("bisiklet yolu", "Radweg", new[] { "Gehweg", "Zebrastreifen", "Busspur" })
    };

    private static QuizQuestion BerlindeGunlukYasamK7(Random r)
    {
        var d = BerlinGunlukYasamListe[r.Next(BerlinGunlukYasamListe.Length)];
        var optionen = new[] { d.Almanca }.Concat(d.Yanlislar).OrderBy(_ => r.Next()).ToArray();

        return new QuizQuestion
        {
            Id = NewId(), Subject = Subject.Tuerkisch, GradeLevel = GradeLevel.Klasse7,
            Topic = "Berlin'de Günlük Yaşam – Wortschatz (Arzt, Verkehr, Behörde)", Type = QuestionType.MultipleChoice,
            Prompt = $"\"{d.TurkceKelime}\" kelimesinin Almancası hangisidir?",
            Options = optionen, CorrectAnswers = new[] { d.Almanca },
            Explanation = $"\"{d.TurkceKelime}\" Almanca \"{d.Almanca}\" demektir. İki dilli yaşarken bu kelimeler doktorda, BVG'de ya da Bürgeramt'ta çok işe yarar.",
            HelpHint = "Günlük yaşam kelimeleri: doktor randevusu, eczane, reçete, durak, aktarma yapmak, aylık bilet, randevu almak, başvuru formu, kimlik kartı, kira."
        };
    }

    private static readonly (string Frage, string[] Optionen, string Antwort, string Erklaerung)[] FiilCatisiListe =
    {
        ("\"Mektuplar dün postaya verildi.\" cümlesinin yüklemi öznesine göre hangi çatıdadır?", new[] { "Edilgen (Passiv)", "Etken (Aktiv)", "İşteş (reziprok)" }, "Edilgen (Passiv)", "\"Ver-il-di\": \"-il\" edilgenlik ekidir. İşi yapan belli değil; \"mektuplar\" sözde öznedir."),
        ("\"Annem sabah erkenden ekmek aldı.\" cümlesinin yüklemi öznesine göre hangi çatıdadır?", new[] { "Edilgen (Passiv)", "Dönüşlü (reflexiv)", "Etken (Aktiv)" }, "Etken (Aktiv)", "İşi yapan özne (annem) cümlede açıkça belli - etken çatı."),
        ("\"Maç yarın akşam oynanacak.\" cümlesinin yüklemi öznesine göre hangi çatıdadır?", new[] { "Dönüşlü (reflexiv)", "Edilgen (Passiv)", "Etken (Aktiv)" }, "Edilgen (Passiv)", "\"Oyna-n-acak\": \"-n\" edilgenlik ekidir. Maçı kimin oynayacağı söylenmiyor."),
        ("\"Küçük kardeşim kendi kendine giyindi.\" cümlesinin yüklemi öznesine göre hangi çatıdadır?", new[] { "İşteş (reziprok)", "Etken (Aktiv)", "Dönüşlü (reflexiv)" }, "Dönüşlü (reflexiv)", "\"Giy-in-di\": Özne işi kendi üzerine yapıyor (kendini giydirdi) - dönüşlü çatı."),
        ("\"Tatilde kuzenimle her gün mesajlaştık.\" cümlesinin yüklemi öznesine göre hangi çatıdadır?", new[] { "İşteş (reziprok)", "Edilgen (Passiv)", "Dönüşlü (reflexiv)" }, "İşteş (reziprok)", "\"Mesaj-la-ş-tık\": \"-ş\" eki işin karşılıklı yapıldığını gösterir - işteş çatı."),
        ("\"Okulun bahçesi dün temizlendi.\" cümlesinin yüklemi öznesine göre hangi çatıdadır?", new[] { "Dönüşlü (reflexiv)", "Edilgen (Passiv)", "İşteş (reziprok)" }, "Edilgen (Passiv)", "Bahçe kendini temizleyemez; işi yapan belli değil. \"Temiz-le-n-di\" edilgen çatıdadır."),
        ("Aşağıdaki fiillerden hangisi geçişsizdir (nesne almaz)?", new[] { "okumak", "uyumak", "yazmak" }, "uyumak", "\"Neyi uyudun?\" diye sorulamaz, bu yüzden \"uyumak\" nesne almaz. \"Neyi okudun?\" ve \"Neyi yazdın?\" ise sorulabilir."),
        ("Aşağıdaki fiillerden hangisi geçişlidir (nesne alabilir)?", new[] { "içmek", "gelmek", "gülmek" }, "içmek", "\"Neyi içtin? - Suyu.\" sorusu sorulabilir, yani \"içmek\" nesne alır. \"Gelmek\" ve \"gülmek\" nesne almaz."),
        ("\"Ali topu kaleye attı.\" cümlesinde \"attı\" fiili nesne durumuna göre nasıldır?", new[] { "Geçişsiz (intransitiv)", "Edilgen (Passiv)", "Geçişli (transitiv)" }, "Geçişli (transitiv)", "\"Neyi attı? - Topu.\" Fiil bir nesne (topu) almış - geçişli fiil."),
        ("\"Bebek bütün gece rahat uyudu.\" cümlesinde \"uyudu\" fiili nesne durumuna göre nasıldır?", new[] { "Geçişsiz (intransitiv)", "Geçişli (transitiv)", "Dönüşlü (reflexiv)" }, "Geçişsiz (intransitiv)", "\"Uyumak\" nesne alamaz (\"neyi uyudu?\" sorulamaz) - geçişsiz fiil."),
        ("\"Uyumak\" fiilinden türetilen ve nesne alabilen fiil hangisidir?", new[] { "uyuşmak", "uyunmak", "uyutmak" }, "uyutmak", "\"-t\" eki geçişsiz fiili geçişli yapar: \"Annem bebeği uyuttu.\" (Neyi/kimi uyuttu? - Bebeği.)"),
        ("\"Babam arabayı tamirciye yaptırdı.\" cümlesinde tamir işini asıl kim yapar?", new[] { "Babam", "Tamirci", "Araba" }, "Tamirci", "\"Yap-tır-dı\": \"-tır\" ettirgen ekidir. Babam işi kendisi yapmadı, tamirciye yaptırdı."),
        ("\"Arabalar dün yıkandı.\" cümlesinde \"yıkandı\" neden edilgendir?", new[] { "Arabalar birbirini karşılıklı olarak yıkadı", "Arabalar bu işi kendi kendine yaptı", "Arabaları kimin yıkadığı belli değil" }, "Arabaları kimin yıkadığı belli değil", "Arabalar kendini yıkayamaz. İşi yapan kişi söylenmiyor - bu edilgen çatının özelliğidir."),
        ("Hangi cümlenin yüklemi edilgen çatılıdır?", new[] { "Kapı gürültüyle açıldı.", "Kapıyı sabah erkenden açtım.", "Kapıyı kardeşim açtı." }, "Kapı gürültüyle açıldı.", "\"Aç-ıl-dı\": Kapıyı kimin açtığı belli değil. Diğer iki cümlede işi yapan (ben, kardeşim) belli."),
        ("Hangi cümlenin yüklemi işteş çatılıdır?", new[] { "Arkadaşım beni bayramda aradı.", "Arkadaşlar bayramda kucaklaştı.", "Bayram için evler temizlendi." }, "Arkadaşlar bayramda kucaklaştı.", "\"Kucak-la-ş-tı\": \"-ş\" eki işin karşılıklı yapıldığını gösterir - işteş çatı."),
        ("\"-ş\" eki fiile genellikle hangi anlamı katar?", new[] { "İşi başkasına yaptırma anlamı", "Birlikte ya da karşılıklı yapma", "İşi yapanın belli olmaması" }, "Birlikte ya da karşılıklı yapma", "\"Görüşmek, yazışmak, koşuşmak\": \"-ş\" eki işteş çatı kurar, iş birlikte ya da karşılıklı yapılır."),
        ("Edilgen çatılı fiiller genellikle hangi eklerle kurulur?", new[] { "-l / -n (okun-, sevil-)", "-ş (görüş-, yazış-)", "-dır / -t (yaptır-, uyut-)" }, "-l / -n (okun-, sevil-)", "Edilgenlik \"-l\" ya da \"-n\" ekiyle yapılır: sev-il-mek, oku-n-mak. \"-ş\" işteş, \"-dır/-t\" ettirgen ekidir."),
        ("\"Öğretmen ödevleri kontrol etti.\" cümlesinin edilgen biçimi hangisidir?", new[] { "Ödevleri kontrol ettirdi.", "Ödevler kontrol etti.", "Ödevler kontrol edildi." }, "Ödevler kontrol edildi.", "Edilgen cümlede işi yapan (öğretmen) çıkar, fiil \"-il\" eki alır: \"ed-il-di\". \"Ettirdi\" ise ettirgendir."),
        ("\"Sınav sonuçları açıklandı.\" cümlesinde sözde özne hangisidir?", new[] { "Sınav sonuçları", "açıklandı", "Öğretmenler" }, "Sınav sonuçları", "Edilgen cümlede işi yapan belli değildir. \"Açıklanan ne?\" sorusunun cevabı \"sınav sonuçları\" - sözde özne."),
        ("\"Kuzenim Berlin'e yeni taşındı.\" cümlesinde \"taşındı\" fiili nesne durumuna göre nasıldır?", new[] { "Geçişli (transitiv)", "Ettirgen (kausativ)", "Geçişsiz (intransitiv)" }, "Geçişsiz (intransitiv)", "\"Taşınmak\" (umziehen) burada nesne almaz: \"Neyi taşındı?\" sorulamaz - geçişsiz fiil.")
    };

    private static QuizQuestion FiilCatisi(Random r)
    {
        var f = FiilCatisiListe[r.Next(FiilCatisiListe.Length)];
        return new QuizQuestion
        {
            Id = NewId(), Subject = Subject.Tuerkisch, GradeLevel = GradeLevel.Klasse9,
            Topic = "Fiil Çatısı (Aktiv/Passiv, transitiv/intransitiv)", Type = QuestionType.MultipleChoice,
            Prompt = f.Frage, Options = f.Optionen, CorrectAnswers = new[] { f.Antwort }, Explanation = f.Erklaerung,
            HelpHint = "Öznesine göre: etken (özne belli), edilgen (-l/-n, işi yapan belli değil), dönüşlü (kendine yapar), işteş (-ş, karşılıklı). Nesnesine göre: geçişli (\"neyi?\" sorulabilir), geçişsiz (sorulamaz)."
        };
    }

    private static readonly (string Frage, string[] Optionen, string Antwort, string Erklaerung)[] EkFiilListe =
    {
        ("Dün havanın soğuk olduğunu kendin yaşadın. Hangisini söylersin?", new[] { "Hava dün çok soğukmuş.", "Hava dün çok soğuktu.", "Hava dün çok soğuksa." }, "Hava dün çok soğuktu.", "Ek fiilin hikâyesi (-dı/idi) kendi yaşadığın, gördüğün geçmişi anlatır: soğuk-tu."),
        ("Konserin çok güzel olduğunu arkadaşından duydun. Duyduğunu nasıl aktarırsın?", new[] { "Konser çok güzeldi.", "Konser çok güzelse.", "Konser çok güzelmiş." }, "Konser çok güzelmiş.", "Ek fiilin rivayeti (-mış/imiş) başkasından duyulan bilgiyi anlatır: güzel-miş."),
        ("\"Annem ___.\" cümlesinde \"öğretmen\" sözcüğünü ek fiille yüklem yapan biçim hangisidir?", new[] { "öğretmendir", "öğretmenler", "öğretmeni" }, "öğretmendir", "Ek fiil \"-dir\" ismi yüklem yapar: Annem öğretmen-dir."),
        ("Ek fiilli bir yüklemin olumsuzu hangi sözcükle yapılır?", new[] { "yok", "değil", "ne" }, "değil", "İsim cümlesi \"değil\" ile olumsuz olur: Hasta değilim. \"Yok\" ise \"var\"ın karşıtıdır."),
        ("\"Sen çok yorgunsun.\" cümlesinin olumsuzu hangisidir?", new[] { "Sen çok yorgunsun değil.", "Sen çok yorgun değilsin.", "Sen çok yorgun yoksun." }, "Sen çok yorgun değilsin.", "Kişi eki \"değil\" sözcüğüne gelir: yorgun değil-sin."),
        ("\"Geliyordu\" sözcüğünde ek fiil hangi görevi yapar?", new[] { "Basit zamanı birleşik zamana çevirir", "İsmi yüklem yaparak cümle kurar", "Fiili edilgen çatıya dönüştürür" }, "Basit zamanı birleşik zamana çevirir", "Geli-yor (şimdiki zaman) + du (ek fiilin hikâyesi) = şimdiki zamanın hikâyesi, yani birleşik zaman."),
        ("\"Yarın hava güzel ise pikniğe gideriz.\" cümlesinde \"ise\" nedir?", new[] { "Ek fiilin rivayeti", "Sebep bildiren bağlaç", "Ek fiilin şart kipi" }, "Ek fiilin şart kipi", "\"İse\" (-se/-sa) ek fiilin şart kipidir ve bir koşul bildirir: Hava güzelse..."),
        ("\"Güzel ise\" ifadesinin bitişik yazılışı hangisidir?", new[] { "güzelse", "güzelyse", "güzeliyse" }, "güzelse", "Ünsüzle biten isme \"-se/-sa\" doğrudan eklenir: güzel-se. \"Y\" yalnızca ünlüden sonra gelir."),
        ("\"Hasta idi\" ifadesinin bitişik yazılışı hangisidir?", new[] { "hastadı", "hastaidi", "hastaydı" }, "hastaydı", "Ünlüyle biten sözcükte \"idi\"nin \"i\"si düşer, yerine \"y\" kaynaştırma harfi gelir: hasta-y-dı."),
        ("\"Öğrenci imiş\" ifadesinin bitişik yazılışı hangisidir?", new[] { "öğrencimiş", "öğrenciymiş", "öğrenciyimiş" }, "öğrenciymiş", "Ünlüyle biten sözcükte \"imiş\"in \"i\"si düşer, \"y\" gelir: öğrenci-y-miş."),
        ("\"Ben Berlinli___.\" boşluğa hangi ek gelir?", new[] { "-sin", "-dir", "-yim" }, "-yim", "1. tekil kişi (ben) ünlüyle biten sözcüğe \"y\" ile eklenir: Berlinli-y-im."),
        ("\"Dedem İzmirli___.\" cümlesinde 3. tekil kişi için hangi ek fiil kullanılabilir?", new[] { "-dir", "-yiz", "-siniz" }, "-dir", "3. tekil kişide ek fiil \"-dir\"dir: İzmirli-dir. Günlük dilde çoğu zaman düşer: Dedem İzmirli."),
        ("\"Siz çok iyi futbolcu___.\" boşluğa hangi ek gelir?", new[] { "-yuz", "-sunuz", "-dur" }, "-sunuz", "2. çoğul kişi (siz): futbolcu-sunuz. Ek, son ünlü \"u\" olduğu için \"-sunuz\" biçimini alır."),
        ("Ek fiilin kaç kipi (zamanı) vardır?", new[] { "İki: yalnızca -di ve -miş", "Dört: -dır, -di, -miş, -se", "Üç: -dır, -di ve -miş" }, "Dört: -dır, -di, -miş, -se", "Ek fiilin dört kipi vardır: geniş zaman (-dır), hikâye (-di), rivayet (-miş), şart (-se)."),
        ("\"Çocukken çok utangaçmışım.\" cümlesinde \"-mış\" hangi anlamı verir?", new[] { "Sonradan öğrenilen ya da duyulan durum", "Kendi gözüyle görülen bir geçmiş", "Gelecekte gerçekleşecek bir durum" }, "Sonradan öğrenilen ya da duyulan durum", "Rivayet (-miş) duyulan ya da sonradan fark edilen durumu anlatır: Kişi bunu büyüklerinden duymuştur."),
        ("Hangi cümlenin yüklemi ek fiil almıştır?", new[] { "Babam işe gitti.", "Babam kahvaltı yaptı.", "Babam bir mühendisti." }, "Babam bir mühendisti.", "\"Mühendis-ti\": İsim, ek fiilin hikâyesiyle yüklem olmuş. Diğer yüklemler çekimli fiildir."),
        ("\"Sınav zor değildi.\" cümlesinde \"değildi\" nasıl oluşmuştur?", new[] { "değil + idi (ek fiilin hikâyesi)", "değil + imiş (ek fiilin rivayeti)", "değil + ise (ek fiilin şartı)" }, "değil + idi (ek fiilin hikâyesi)", "\"Değil\" sözcüğüne ek fiilin hikâyesi (idi) gelmiştir: değil-di."),
        ("\"Evde ise\" ifadesinin bitişik yazılışı hangisidir?", new[] { "evdese", "evdeise", "evdeyse" }, "evdeyse", "Ünlüyle biten sözcükte \"ise\"nin \"i\"si düşer, \"y\" gelir: evde-y-se."),
        ("Ek fiilin görevlerinden biri hangisidir?", new[] { "Fiillerden yeni isimler türetmek", "İsim soylu sözcükleri yüklem yapmak", "Sözcükleri çoğul hâle getirmek" }, "İsim soylu sözcükleri yüklem yapmak", "Ek fiil isimleri yüklem yapar (öğrenci-yim) ve basit zamanlı fiillerden birleşik zaman kurar (geliyor-du)."),
        ("\"Ahmet hastaymış.\" cümlesini söyleyen kişi ne anlatmak ister?", new[] { "Ahmet'in hasta olduğunu kendi gözüyle görmüş", "Ahmet'in yarın hasta olacağını düşünüyor", "Ahmet'in hasta olduğunu başkasından duymuş" }, "Ahmet'in hasta olduğunu başkasından duymuş", "\"-mış\" (rivayet) duyulan bilgiyi anlatır. Kendi gördüğünü anlatsaydı \"Ahmet hastaydı.\" derdi.")
    };

    private static QuizQuestion EkFiil(Random r)
    {
        var f = EkFiilListe[r.Next(EkFiilListe.Length)];
        return new QuizQuestion
        {
            Id = NewId(), Subject = Subject.Tuerkisch, GradeLevel = GradeLevel.Klasse9,
            Topic = "Ek Fiil (Kopula: -dır, -dı, -mış, -sa)", Type = QuestionType.MultipleChoice,
            Prompt = f.Frage, Options = f.Optionen, CorrectAnswers = new[] { f.Antwort }, Explanation = f.Erklaerung,
            HelpHint = "Ek fiil (i-mek) ismi yüklem yapar ve dört kipi vardır: -dır (geniş zaman), -dı/idi (hikâye, yaşanan), -mış/imiş (rivayet, duyulan), -sa/ise (şart). Olumsuzu \"değil\"; ünlüden sonra \"y\" gelir."
        };
    }

    private static readonly (string Frage, string[] Optionen, string Antwort, string Erklaerung)[] ParagraftaAnlamListe =
    {
        ("\"Kitap okumak insanın hayal gücünü geliştirir. Okuyan çocuk farklı dünyaları tanır, yeni kelimeler öğrenir ve kendini daha iyi ifade eder.\" Bu paragrafın ana düşüncesi nedir?", new[] { "Düzenli kitap okumak insanı geliştirir.", "Çocuklar yalnızca masal kitabı okumalı.", "Kelimeleri ezberlemek çok zordur." }, "Düzenli kitap okumak insanı geliştirir.", "Paragrafın bütün cümleleri okumanın insana kattıklarını anlatıyor - verilmek istenen mesaj budur."),
        ("\"Berlin'de her yıl binlerce kişi bisikletle işe gider. Bisiklet yolları çoğaldıkça trafik azalıyor, hava daha temiz oluyor.\" Bu paragrafın konusu nedir?", new[] { "Berlin'deki müzeler", "Berlin'de bisiklet kullanımı", "Toplu taşıma fiyatları" }, "Berlin'de bisiklet kullanımı", "Konu, paragrafta \"neyden söz edildiği\"dir. Her cümle Berlin'de bisikletle ilgili."),
        ("\"Kuşlar sonbaharda sıcak ülkelere göç eder. Leylekler Afrika'ya kadar uçar ve ilkbaharda yine aynı yuvaya döner.\" Bu paragrafa en uygun başlık hangisidir?", new[] { "Kışın Kar Yağışı", "Kuşların Uzun Yolculuğu", "Evcil Hayvan Bakımı" }, "Kuşların Uzun Yolculuğu", "Başlık paragrafın konusunu kısaca özetler. Paragraf kuşların göçünü, yani uzun yolculuklarını anlatıyor."),
        ("\"Spor yapmak sağlığımız için önemlidir. Düzenli spor yapan kişi daha iyi uyur, kalbi güçlenir ve kendini daha mutlu hisseder.\" Hangisi bu paragrafın yardımcı düşüncelerinden biridir?", new[] { "Spor yapan kişi daha iyi uyur.", "Spor yapmak yalnızca gençler içindir.", "Futbol en eski spordur." }, "Spor yapan kişi daha iyi uyur.", "Yardımcı düşünceler ana düşünceyi destekler. \"Daha iyi uyur\" sporun faydasını gösteren bir örnektir."),
        ("\"Dedem Almanya'ya 1970'lerde geldi. Önce bir fabrikada çalıştı, dil bilmediği için çok zorlandı. Yıllar içinde Almanca öğrendi ve kendi dükkânını açtı.\" Paragrafta ağırlıklı olarak ne anlatılıyor?", new[] { "Fabrikaların çalışma saatleri", "Almanca dilinin tarihi", "Dedenin Almanya'daki hayatı" }, "Dedenin Almanya'daki hayatı", "Bütün cümleler dedenin Almanya'daki yaşamını sırayla anlatıyor."),
        ("\"Bir arkadaşın sana kötü davrandığında hemen kızma. Belki o gün zor bir gün geçiriyordur. Önce onu dinle, sonra konuş.\" Yazar ne öneriyor?", new[] { "Kızmadan önce karşımızdakini anlamayı", "Kötü davranan arkadaşla hiç konuşmamayı", "Zor günlerde evden hiç dışarı çıkmamayı" }, "Kızmadan önce karşımızdakini anlamayı", "Yazar \"önce dinle, sonra konuş\" diyerek karşımızdakini anlamaya çalışmayı öğütlüyor."),
        ("\"Türk mutfağında zeytinyağlı yemekler önemli bir yer tutar. Özellikle Ege'de fasulye, enginar ve dolma zeytinyağıyla pişirilir.\" Bu paragrafın konusu nedir?", new[] { "Ege'deki tatil yerleri", "Soğuk içecekler", "Zeytinyağlı yemekler" }, "Zeytinyağlı yemekler", "Paragraf baştan sona zeytinyağlı yemeklerden söz ediyor; Ege yalnızca örnek bölgedir."),
        ("\"(I) Kediler çok temiz hayvanlardır. (II) Günün büyük kısmında tüylerini yalarlar. (III) Köpekler ise insanlara çok bağlıdır. (IV) Kumlarını da düzenli kullanırlar.\" Hangi cümle düşüncenin akışını bozar?", new[] { "II. cümle", "III. cümle", "IV. cümle" }, "III. cümle", "Paragraf kedilerin temizliğini anlatıyor. III. cümle konuyu köpeklere kaydırdığı için akışı bozar."),
        ("\"(I) Bayramda sabah erkenden kalkarız. (II) Yeni kıyafetlerimizi giyeriz. (III) Büyüklerimizin elini öperiz. (IV) Kış mevsiminde günler kısadır.\" Hangi cümle düşüncenin akışını bozar?", new[] { "II. cümle", "I. cümle", "IV. cümle" }, "IV. cümle", "Paragraf bayram sabahını anlatıyor. IV. cümle mevsimlerden söz ettiği için konunun dışında kalır."),
        ("\"Karagöz ile Hacivat, Türk gölge oyununun iki ünlü kahramanıdır. Deriden yapılan renkli figürler bir perdenin arkasından ışıkla oynatılır.\" Bu paragrafa en uygun başlık hangisidir?", new[] { "Perdedeki Gölgeler", "Deri Ayakkabı Yapımı", "Işığın Hızı" }, "Perdedeki Gölgeler", "Paragraf gölge oyununu anlatıyor; \"Perdedeki Gölgeler\" bu konuyu en iyi özetleyen başlıktır."),
        ("\"Pazar günü saat 10'da okul bahçesinde çöp toplama etkinliği yapılacak. Eldiven ve poşetler okuldan verilecek. Tüm öğrenciler ve aileleri davetlidir.\" Bu metnin amacı nedir?", new[] { "Geçmişte yaşanan bir anıyı anlatmak", "Bir etkinliğe davet etmek", "Bir kitabı eleştirmek" }, "Bir etkinliğe davet etmek", "Metin tarih, saat ve yer bildirip herkesi çağırıyor - bir duyuru ve davettir."),
        ("Ana düşünce bir paragrafta çoğunlukla nerede bulunur?", new[] { "Her zaman ortadaki cümlede", "Çoğunlukla ilk ya da son cümlede", "Yalnızca başlığın içinde" }, "Çoğunlukla ilk ya da son cümlede", "Ana düşünce çoğu zaman giriş cümlesinde ya da sonuç (son) cümlesinde yer alır; ama her paragrafta aynı yerde olmak zorunda değildir."),
        ("Konu ile ana düşünce arasındaki fark nedir?", new[] { "Konu neyin anlatıldığıdır, ana düşünce verilen mesajdır.", "Konu ve ana düşünce her zaman tamamen aynı şeydir.", "Ana düşünce metnin başlığıdır, konu ise son cümlesidir." }, "Konu neyin anlatıldığıdır, ana düşünce verilen mesajdır.", "Konu \"Metin neyden söz ediyor?\" sorusunun, ana düşünce \"Yazar bize ne demek istiyor?\" sorusunun cevabıdır."),
        ("\"Futbol yalnızca bir oyun değildir. Takım olarak oynamayı, kaybetmeyi kabul etmeyi ve rakibe saygı duymayı öğretir.\" Bu paragrafın ana düşüncesi nedir?", new[] { "Futbolda en önemli şey her maçı kazanmaktır.", "Futbol maçları çok uzun sürer.", "Futbol çocuklara önemli değerler öğretir." }, "Futbol çocuklara önemli değerler öğretir.", "Paragraf futbolun takım ruhu, saygı ve kaybetmeyi kabul etme gibi değerler kazandırdığını anlatıyor."),
        ("\"Uçak Antalya'ya indiğinde kalbim hızla çarpıyordu. Bir yıldır görmediğim babaannemi birazdan kucaklayacaktım.\" Anlatıcı ne hissediyor?", new[] { "Heyecan ve özlem", "Korku ve öfke", "Can sıkıntısı" }, "Heyecan ve özlem", "\"Kalbim hızla çarpıyordu\" heyecanı, \"bir yıldır görmediğim\" özlemi gösteriyor."),
        ("\"Plastik poşetler doğada yüzlerce yıl yok olmaz. Denize karışan poşetleri kaplumbağalar yiyecek sanır. Alışverişe giderken bez çanta kullanmak bu sorunu azaltır.\" Paragrafta hangi çözüm önerilir?", new[] { "Balık yemek", "Denize daha az girmek", "Bez çanta kullanmak" }, "Bez çanta kullanmak", "Son cümle sorunu ve çözümü açıkça söylüyor: Plastik poşet yerine bez çanta kullanmak."),
        ("\"Mehmet her sabah 6'da kalkıp gazete dağıtıyor. Kazandığı parayla yeni bir bisiklet almak istiyor. Şimdiden paranın yarısını biriktirdi.\" Hangisi bu paragraftan çıkarılamaz?", new[] { "Mehmet her sabah çok erken saatte kalkıyor.", "Mehmet bisikleti babasından hediye aldı.", "Mehmet para biriktiriyor." }, "Mehmet bisikleti babasından hediye aldı.", "Mehmet bisikleti henüz almadı, parasını kendisi biriktiriyor. Hediye bilgisi paragrafta yok."),
        ("Hangisi bir paragrafın giriş cümlesi olabilir?", new[] { "Bu yüzden o güzel kış gününü hiç unutmadım.", "Ayrıca annem de bizimle geldi.", "Kışın en sevdiğim etkinlik kızak kaymaktır." }, "Kışın en sevdiğim etkinlik kızak kaymaktır.", "\"Bu yüzden\" ve \"Ayrıca\" önceki bir cümleye bağlanır, bu yüzden giriş cümlesi olamaz. Giriş cümlesi tek başına anlaşılır."),
        ("Hangisi bir paragrafın sonuç cümlesi olabilir?", new[] { "Kısacası sağlıklı beslenmek hepimizin elindedir.", "Sabah kahvaltısı günün ilk öğünüdür.", "Mesela ben her gün meyve yerim." }, "Kısacası sağlıklı beslenmek hepimizin elindedir.", "\"Kısacası\" anlatılanları toparlar ve sonuca bağlar - sonuç cümlesinin tipik işaretidir."),
        ("\"Bazı insanlar iki dil bilmenin çocukların kafasını karıştırdığını düşünür. Oysa araştırmalar iki dilli çocukların problem çözmede daha başarılı olduğunu gösteriyor.\" Yazar hangi görüşü savunuyor?", new[] { "Çocuklar yalnızca tek bir dil öğrenmelidir.", "İki dilli büyümek çocuğa avantaj sağlar.", "Araştırmalar her zaman yanlış çıkar." }, "İki dilli büyümek çocuğa avantaj sağlar.", "\"Oysa\" sözcüğüyle yazar ilk görüşe karşı çıkıyor ve iki dilliliğin faydalı olduğunu savunuyor.")
    };

    private static QuizQuestion ParagraftaAnlam(Random r)
    {
        var f = ParagraftaAnlamListe[r.Next(ParagraftaAnlamListe.Length)];
        return new QuizQuestion
        {
            Id = NewId(), Subject = Subject.Tuerkisch, GradeLevel = GradeLevel.Klasse9,
            Topic = "Paragrafta Anlam (Thema, Hauptgedanke)", Type = QuestionType.MultipleChoice,
            Prompt = f.Frage, Options = f.Optionen, CorrectAnswers = new[] { f.Antwort }, Explanation = f.Erklaerung,
            HelpHint = "Konu: Metin neyden söz ediyor? Ana düşünce: Yazar ne demek istiyor (mesaj)? Yardımcı düşünceler ana düşünceyi destekler. \"Bu yüzden, ayrıca\" ile başlayan cümle giriş olamaz; \"kısacası\" sonuç bildirir."
        };
    }

    private static readonly (string Frage, string[] Optionen, string Antwort, string Erklaerung)[] BaglaclarEdatlarListe =
    {
        ("\"Ben ___ sinemaya gelmek istiyorum.\" (auch) Boşluğa hangisi gelir?", new[] { "da", "de", "te" }, "de", "\"Auch\" anlamındaki bağlaç ayrı yazılır ve önceki sözcüğün son ünlüsüne uyar: Ben de. Bağlaç \"de\" hiçbir zaman \"te\" olmaz."),
        ("Hangi cümlede \"de\" doğru yazılmıştır?", new[] { "Kardeşim de bizimle geldi.", "Kardeşimde bizimle geldi.", "Kardeşim-de bizimle geldi." }, "Kardeşim de bizimle geldi.", "Burada \"de\" \"auch\" anlamında bir bağlaçtır ve ayrı yazılır. Bitişik \"kardeşimde\" \"bei meinem Bruder\" demektir."),
        ("\"Kitabın evde kaldı.\" cümlesindeki \"-de\" nedir?", new[] { "Bağlaç, ayrı yazılması gerekir", "Soru eki, ayrı yazılması gerekir", "Bulunma hâl eki, bitişik yazılır" }, "Bulunma hâl eki, bitişik yazılır", "\"Nerede kaldı? - Evde.\" Yer bildiren \"-de\" bulunma hâl ekidir ve sözcüğe bitişik yazılır."),
        ("Hangi cümlede yazım yanlışı vardır?", new[] { "Sende mi geleceksin?", "Sen de mi geleceksin?", "Sende kalem var mı?" }, "Sende mi geleceksin?", "\"Du auch?\" anlamında \"de\" bağlaçtır ve ayrı yazılmalı: Sen de mi geleceksin? \"Sende kalem var mı?\" ise doğrudur (bei dir)."),
        ("Hangisi doğru yazılmıştır?", new[] { "Duydumki yarın okul tatil.", "Duydum-ki yarın okul tatil.", "Duydum ki yarın okul tatil." }, "Duydum ki yarın okul tatil.", "Bağlaç olan \"ki\" her zaman ayrı yazılır: Duydum ki..."),
        ("Hangi cümlede \"-ki\" ek olduğu için bitişik yazılır?", new[] { "Öyle yorgunum ki uyuyacağım.", "Masadaki kitabı bana ver.", "Biliyorum ki haklısın." }, "Masadaki kitabı bana ver.", "\"Masa-da-ki\": \"-ki\" burada \"masada olan\" anlamı veren bir ektir ve bitişik yazılır. Diğer ikisinde \"ki\" bağlaçtır."),
        ("\"Bugün okula gitmedim ___ hastaydım.\" Boşluğa hangisi gelir?", new[] { "ile", "ama", "çünkü" }, "çünkü", "\"Çünkü\" sebep bildirir: Neden gitmedim? Hastaydım."),
        ("\"Maçı izlemek istedim ___ elektrikler kesildi.\" Boşluğa hangisi gelir?", new[] { "ama", "çünkü", "ve" }, "ama", "İki cümle arasında karşıtlık var (istedim - olmadı). Karşıtlık \"ama\" ile bağlanır."),
        ("\"Hafta sonu sinemaya ___ parka gideceğiz.\" Boşluğa hangisi gelir?", new[] { "hem de", "ya da", "ne de" }, "ya da", "\"Ya da\" iki seçenekten birini bildirir: ya sinema ya park."),
        ("\"Ne ödevini yaptı ___ odasını topladı.\" Boşluğa hangisi gelir?", new[] { "hem de", "ne de", "ya da" }, "ne de", "\"Ne... ne (de)\" iki işin de yapılmadığını bildirir; yüklem olumlu kalır."),
        ("Hangisi bir edattır (ilgeç)?", new[] { "gibi", "ama", "ve" }, "gibi", "Edatlar tek başına anlamı olmayan, isimle birlikte anlam kazanan sözcüklerdir: senin gibi. \"Ama\" ve \"çünkü\" bağlaçtır."),
        ("\"Annem için bir hediye aldım.\" cümlesinde \"için\" hangi görevdedir?", new[] { "Bağlaç (karşıtlık bildirir)", "Hâl eki", "Edat (amaç/yarar)" }, "Edat (amaç/yarar)", "\"Kimin için?\" - \"için\" bir edattır ve burada amaç/yarar bildirir."),
        ("\"Ali ile Ayşe okula gitti.\" cümlesinde \"ile\" hangi görevdedir?", new[] { "Bağlaç, \"ve\" yerine kullanılmış", "Edat, araç anlamı katmış", "Hâl eki, bitişik yazılmalı" }, "Bağlaç, \"ve\" yerine kullanılmış", "\"Ali ve Ayşe okula gitti.\" denebildiği için \"ile\" burada bağlaçtır. \"Otobüs ile\" gibi araç bildirirse edattır."),
        ("\"Otobüsle okula gidiyorum.\" cümlesinde \"-le\" hangi anlamı katar?", new[] { "Sebep (neden?)", "Zaman (ne zaman?)", "Araç (ne ile?)" }, "Araç (ne ile?)", "\"Neyle gidiyorsun? - Otobüsle.\" \"İle\" burada araç bildiren bir edattır ve bitişik yazılmıştır."),
        ("\"Sabaha kadar ders çalıştı.\" cümlesinde \"kadar\" hangi anlamı verir?", new[] { "Benzerlik", "Zaman sınırı", "Sebep" }, "Zaman sınırı", "\"Ne zamana kadar?\" \"Kadar\" burada zaman sınırı bildiren bir edattır (bis zum Morgen)."),
        ("\"Hava çok soğuktu, ___ dışarı çıktık.\" Boşluğa hangisi gelir?", new[] { "çünkü", "ya da", "yine de" }, "yine de", "Soğuğa rağmen dışarı çıkıldı - karşıtlık bildiren \"yine de\" uygundur."),
        ("Hangi cümlede \"da\" bulunma hâl ekidir?", new[] { "Arkadaşım parkta bekliyor.", "Arkadaşım da bizi okul önünde bekliyor.", "O da bizimle gelsin." }, "Arkadaşım parkta bekliyor.", "\"Nerede? - Parkta.\" Yer bildiren \"-ta\" bulunma hâl ekidir ve bitişik yazılır. Diğerlerinde \"da\" bağlaçtır."),
        ("\"Bu soruyu senden başka kimse çözemedi.\" cümlesindeki edat hangisidir?", new[] { "kimse", "başka", "çözemedi" }, "başka", "\"-den başka\" bir edattır ve \"hariç\" anlamı verir: senden başka."),
        ("\"Berlin'e göre İstanbul daha kalabalıktır.\" cümlesinde \"göre\" hangi anlamı katar?", new[] { "Birlikte olma durumu", "Karşılaştırma", "Amaç ve hedef" }, "Karşılaştırma", "\"-e göre\" edatı burada iki şehri karşılaştırıyor."),
        ("Bağlaç olan \"de/da\"yı ekten ayırmanın kolay yolu nedir?", new[] { "Cümleden çıkarınca anlam bozulmazsa bağlaçtır", "Sözcüğün sonu ünlüyse her zaman bağlaçtır", "Cümle soru ise her zaman ayrı yazılır" }, "Cümleden çıkarınca anlam bozulmazsa bağlaçtır", "\"Ben de geldim\" -> \"Ben geldim\": Cümle bozulmaz, \"de\" bağlaçtır. \"Evde kaldı\" -> \"Ev kaldı\": Anlam bozulur, \"-de\" ektir.")
    };

    private static QuizQuestion BaglaclarVeEdatlar(Random r)
    {
        var f = BaglaclarEdatlarListe[r.Next(BaglaclarEdatlarListe.Length)];
        return new QuizQuestion
        {
            Id = NewId(), Subject = Subject.Tuerkisch, GradeLevel = GradeLevel.Klasse9,
            Topic = "Bağlaçlar ve Edatlar (Konjunktionen/Postpositionen)", Type = QuestionType.MultipleChoice,
            Prompt = f.Frage, Options = f.Optionen, CorrectAnswers = new[] { f.Antwort }, Explanation = f.Erklaerung,
            HelpHint = "Bağlaç \"de/da\" (auch) ve \"ki\" ayrı yazılır; hâl eki \"-de/-da\" ve \"-ki\" eki (masadaki) bitişiktir. Bağlaçlar: ve, ama, çünkü, ya da, ne... ne. Edatlar: gibi, için, kadar, göre, ile, -den başka."
        };
    }

    private static readonly (string Frage, string[] Optionen, string Antwort, string Erklaerung)[] AnlatimBozukluklariListe =
    {
        ("Hangi cümlede gereksiz sözcük kullanılmıştır?", new[] { "Okula yeniden gittim.", "Okuldan eve döndüm.", "Okula geri döndüm." }, "Okula geri döndüm.", "\"Dönmek\" zaten geri gelmek demektir; \"geri\" sözcüğü gereksizdir."),
        ("\"Kötü bir kâbus gördüm.\" cümlesi neden hatalıdır?", new[] { "Kâbus zaten kötü rüya, \"kötü\" gereksiz", "\"Gördüm\" yerine \"yaptım\" denmeliydi", "Cümlenin sonuna soru işareti gerekir" }, "Kâbus zaten kötü rüya, \"kötü\" gereksiz", "Kâbus korkutucu, kötü bir rüyadır. \"Kötü\" aynı anlamı tekrar ettiği için gereksizdir."),
        ("Hangi cümle anlatım bakımından doğrudur?", new[] { "Toplantıya yaklaşık elli kişi kadar katıldı.", "Toplantıya tahminen yaklaşık elli kişi katıldı.", "Toplantıya yaklaşık elli kişi katıldı." }, "Toplantıya yaklaşık elli kişi katıldı.", "\"Yaklaşık\", \"kadar\" ve \"tahminen\" aynı anlamı taşır; biri yeterlidir."),
        ("Hangi cümlede sözcük yanlış anlamda kullanılmıştır?", new[] { "Bu mağazada fiyatlar çok düşük.", "Bu mağazada fiyatlar çok ucuz.", "Bu mağazada ürünler çok ucuz." }, "Bu mağazada fiyatlar çok ucuz.", "Ürün ucuz ya da pahalı olur; fiyat ise düşük ya da yüksek olur."),
        ("\"Bu konu hakkında pek çok kitaplar okudum.\" cümlesinin doğrusu hangisidir?", new[] { "Bu konu hakkında pek çok kitaplar okudu.", "Bu konuyu hakkında pek çok kitap okudum.", "Bu konu hakkında pek çok kitap okudum." }, "Bu konu hakkında pek çok kitap okudum.", "\"Pek çok\" zaten çokluk bildirir; ardından gelen isim çoğul eki almaz: pek çok kitap."),
        ("\"Ben ve kardeşim parka gitti.\" cümlesindeki bozukluk nedir?", new[] { "Özne ile yüklem kişi bakımından uyumsuz", "Cümlede gereksiz bir sözcük kullanılmış", "Bir sözcük yanlış anlamda kullanılmış" }, "Özne ile yüklem kişi bakımından uyumsuz", "\"Ben ve kardeşim\" = biz. Yüklem de 1. çoğul olmalı: Ben ve kardeşim parka gittik."),
        ("\"Maalesef sınavı kazandım.\" cümlesindeki bozukluk nedir?", new[] { "Cümlede yüklem eksik bırakılmış", "\"Maalesef\" sevinçli bir olayla kullanılmış", "Özne ile yüklem kişi bakımından uyumsuz" }, "\"Maalesef\" sevinçli bir olayla kullanılmış", "\"Maalesef\" üzüntü bildirir; sınavı kazanmak sevindirici olduğu için sözcük yanlış seçilmiştir."),
        ("Hangi cümlede çoğul eki yanlış kullanılmıştır?", new[] { "Bahçedeki bütün çocuklar oynuyor.", "Bahçede beş çocuklar oynuyor.", "Bahçede beş tane çocuk oynuyor." }, "Bahçede beş çocuklar oynuyor.", "Sayı sözcüğünden sonra isim çoğul eki almaz: beş çocuk."),
        ("\"Bu film bence kesinlikle belki güzeldir.\" cümlesindeki bozukluk nedir?", new[] { "\"Kesinlikle\" ile \"belki\" çelişiyor", "\"Film\" sözcüğü yanlış yazılmış", "Cümlede hiç yüklem bulunmuyor" }, "\"Kesinlikle\" ile \"belki\" çelişiyor", "\"Kesinlikle\" kesinlik, \"belki\" ise olasılık bildirir; ikisi aynı cümlede birbiriyle çelişir."),
        ("\"Ne ödevini yaptı ne de odasını toplamadı.\" cümlesindeki bozukluk nedir?", new[] { "Cümlede özne hiç kullanılmamış", "\"Ödev\" sözcüğü yanlış anlamda kullanılmış", "\"Ne... ne\" olumsuz yüklemle kullanılmış" }, "\"Ne... ne\" olumsuz yüklemle kullanılmış", "\"Ne... ne\" bağlacı zaten olumsuzluk verir; yüklem olumlu olmalı: ...ne de odasını topladı."),
        ("\"Otobüste yer olmadığı için ayakta oturdum.\" cümlesindeki bozukluk nedir?", new[] { "\"Ayakta\" ile \"oturdum\" anlamca çelişiyor", "\"Otobüste\" sözcüğü gereksiz kullanılmış", "Cümlede bağlaç yanlış yerde kullanılmış" }, "\"Ayakta\" ile \"oturdum\" anlamca çelişiyor", "Ayakta durulur, oturulmaz. Doğrusu: \"...ayakta durdum.\""),
        ("\"Kahvaltıda peynir, zeytin ve çay içtim.\" cümlesindeki bozukluk nedir?", new[] { "Virgüllerin yerine \"ve\" yazılması gerekiyor", "\"Kahvaltıda\" sözcüğü ayrı yazılmalı", "\"İçtim\" yüklemi peynire ve zeytine uymuyor" }, "\"İçtim\" yüklemi peynire ve zeytine uymuyor", "Peynir ve zeytin içilmez, yenir. Doğrusu: \"Kahvaltıda peynir ve zeytin yedim, çay içtim.\""),
        ("Hangi cümlede anlamca çelişki vardır?", new[] { "Bu güzel haberi duyunca çok sevindim.", "Bu kötü haberi duyunca çok sevindim.", "Bu haberi duyunca çok üzüldüm." }, "Bu kötü haberi duyunca çok sevindim.", "Kötü bir haber insanı sevindirmez; \"kötü\" ile \"sevindim\" birbiriyle çelişir."),
        ("\"Siz de bizimle geliyor musun?\" cümlesinin doğrusu hangisidir?", new[] { "Sen de bizimle geliyor musunuz?", "Siz de bizimle geliyorlar mı?", "Siz de bizimle geliyor musunuz?" }, "Siz de bizimle geliyor musunuz?", "Özne \"siz\" ise yüklem de 2. çoğul kişi eki almalıdır: geliyor musunuz."),
        ("\"Tüm öğrenciler sınıfa geldi ama bazıları gelmedi.\" cümlesindeki bozukluk nedir?", new[] { "\"Tüm\" ile \"bazıları gelmedi\" çelişiyor", "\"Sınıfa\" sözcüğünde yazım yanlışı var", "\"Ama\" yerine \"çünkü\" kullanılmalı" }, "\"Tüm\" ile \"bazıları gelmedi\" çelişiyor", "Hepsi geldiyse bazılarının gelmemesi mümkün değildir - mantık hatası."),
        ("Hangi cümlede gereksiz sözcük vardır?", new[] { "Toplantıda fikir alışverişi yaptık.", "Toplantıda karşılıklı fikir alışverişi yaptık.", "Toplantıda her öğrenci kendi fikrini tek tek söyledi." }, "Toplantıda karşılıklı fikir alışverişi yaptık.", "\"Alışveriş\" zaten karşılıklıdır (alış + veriş); \"karşılıklı\" gereksizdir."),
        ("\"Hiç kimse bu soruyu çözebildi.\" cümlesinin doğrusu hangisidir?", new[] { "Hiç kimse bu soruyu çözebildi mi.", "Hiç kimse bu soruyu çözemedi.", "Hiç kimseler bu soruyu çözebildi." }, "Hiç kimse bu soruyu çözemedi.", "\"Hiç kimse\" olumsuz yüklem ister: Hiç kimse çözemedi."),
        ("\"Geçen yıl Türkiye'ye gideceğiz.\" cümlesindeki bozukluk nedir?", new[] { "Zaman sözcüğü ile yüklem uyuşmuyor", "Özel ismin kesme işareti yanlış", "Cümlede gereksiz sözcük kullanılmış" }, "Zaman sözcüğü ile yüklem uyuşmuyor", "\"Geçen yıl\" geçmişi, \"gideceğiz\" geleceği bildirir. Doğrusu: \"Geçen yıl Türkiye'ye gittik.\""),
        ("\"Bu sabah saat 7'de sabahleyin kalktım.\" cümlesinde hangi sözcük gereksizdir?", new[] { "saat", "kalktım", "sabahleyin" }, "sabahleyin", "\"Bu sabah\" zaten zamanı bildiriyor; \"sabahleyin\" aynı bilgiyi tekrar eder."),
        ("\"Bu diziyi izlemenizi hiç önermem, çünkü çok güzel.\" cümlesindeki bozukluk nedir?", new[] { "Önermeme ile gerekçe birbiriyle çelişiyor", "\"Çünkü\" yerine \"ve\" kullanılmalı", "\"Diziyi\" sözcüğü gereksiz kullanılmış" }, "Önermeme ile gerekçe birbiriyle çelişiyor", "Güzel bir dizi önerilir; \"önermem\" ile \"çok güzel\" gerekçesi mantıkça çelişir.")
    };

    private static QuizQuestion AnlatimBozukluklari(Random r)
    {
        var f = AnlatimBozukluklariListe[r.Next(AnlatimBozukluklariListe.Length)];
        return new QuizQuestion
        {
            Id = NewId(), Subject = Subject.Tuerkisch, GradeLevel = GradeLevel.Klasse9,
            Topic = "Anlatım Bozuklukları (Ausdrucksfehler)", Type = QuestionType.MultipleChoice,
            Prompt = f.Frage, Options = f.Optionen, CorrectAnswers = new[] { f.Antwort }, Explanation = f.Erklaerung,
            HelpHint = "Sık görülen bozukluklar: gereksiz sözcük (geri dönmek), yanlış anlamda sözcük (fiyat ucuz), özne-yüklem uyumsuzluğu (ben ve kardeşim gittik), çelişki (kesinlikle belki), çokluk sözcüğünden sonra çoğul eki (beş çocuklar)."
        };
    }
}
