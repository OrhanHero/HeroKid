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
        ("oynamak", "oynuyor"), ("koşmak", "koşuyor"), ("gelmek", "geliyor")
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
        ("okumak", "okudu"), ("yazmak", "yazdı"), ("gitmek", "gitti")
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
        ("hızlı", "çabuk", new[] { "yavaş", "ağır", "durgun" })
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
        ("erken", "geç", new[] { "yakın", "uzak", "yeni" })
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
        ("iklim değişikliği", "Klimawandel", new[] { "Umweltverschmutzung", "Naturschutz", "Wetterbericht" })
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
        ("Öğretmen sınıfta ders anlattı.", "Yer Tamlayıcısı (Zarf Tümleci)", "sınıfta")
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
        ("yazmak", "yazacak"), ("oynamak", "oynayacak")
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
        ("Renkli kalem___ getir.", "i", "Belirtili nesne \"-i\" hâl ekini alır: kalem-i.")
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
        ("Yüzmek çok eğlencelidir.", "Yüzmek", "İsim-fiil (Verbalnomen)")
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
