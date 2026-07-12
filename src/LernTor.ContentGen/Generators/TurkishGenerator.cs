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
                DogaVeCevre
            },
            [GradeLevel.Klasse9] = new List<TopicFactory>
            {
                CumleOgeleri,
                GelecekZaman,
                YazimKurallari,
                FiilimsiTuru
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
}
