namespace LernTor.Core.Services;

/// <summary>
/// Statische, kuratierte Lerninhalte des KI-Bereichs ("KI verstehen und sicher nutzen").
/// Bewusst als fest einkompilierte C#-Daten statt JSON-Dateien - dasselbe Muster wie
/// ReadingContentProvider/TypingContentProvider: komplett offline, typsicher, keine
/// Lade-/Parse-Fehlerpfade im Kiosk, und Erweiterungen laufen wie bei den Fach-Generatoren
/// über normale Code-Reviews. Beide Sprachen (DE/TR) stecken direkt im Datensatz, damit der
/// Sprachwechsel ohne Neuladen funktioniert.
///
/// Tonfall der Texte: auf Augenhöhe, ohne Fachchinesisch, ohne Angstmache - KI wird als
/// Werkzeug entmystifiziert (Taschenrechner/Bibliothekar, kein magisches Wesen).
/// </summary>
public static class KiContentService
{
    /// <summary>Ein Lernmodul des KI-Bereichs (z.B. "Was ist KI?") mit mehreren kurzen Abschnitten.</summary>
    public sealed record KiModule(string Id, string Icon, string TitleDe, string TitleTr, IReadOnlyList<KiSection> Sections);

    /// <summary>Ein Abschnitt eines Moduls: kleine Überschrift + wenige Sätze Fließtext.</summary>
    public sealed record KiSection(string HeadingDe, string HeadingTr, string BodyDe, string BodyTr);

    public static IReadOnlyList<KiModule> GetModules() => Modules;

    private static readonly IReadOnlyList<KiModule> Modules = new List<KiModule>
    {
        new KiModule(
            "was-ist-ki", "🤖",
            "Was ist KI?", "Yapay zeka nedir?",
            new List<KiSection>
            {
                new(
                    "Ein Werkzeug, kein Wesen", "Bir araç, bir canlı değil",
                    "Eine KI ist ein Computerprogramm - so wie ein Taschenrechner, nur für Sprache, " +
                    "Bilder oder Musik. Sie denkt nicht, sie fühlt nichts und sie will nichts. " +
                    "Sie rechnet nur sehr schnell aus, welche Antwort am wahrscheinlichsten passt.",
                    "Yapay zeka bir bilgisayar programıdır - hesap makinesi gibi, ama dil, resim " +
                    "veya müzik için. Düşünmez, hissetmez ve bir şey istemez. Sadece hangi cevabın " +
                    "en olası olduğunu çok hızlı hesaplar."),
                new(
                    "Wie lernt eine KI?", "Yapay zeka nasıl öğrenir?",
                    "Eine Sprach-KI hat riesige Mengen Text gelesen - Bücher, Webseiten, Artikel. " +
                    "Daraus hat sie Muster gelernt: Welche Wörter folgen oft aufeinander? Wenn du ihr " +
                    "eine Frage stellst, setzt sie Wort für Wort die wahrscheinlichste Fortsetzung " +
                    "zusammen. Deshalb klingt sie schlau - auch wenn sie mal Unsinn erzählt.",
                    "Bir dil yapay zekası devasa miktarda metin okumuştur - kitaplar, web siteleri, " +
                    "makaleler. Bunlardan kalıplar öğrenmiştir: Hangi kelimeler sıkça art arda gelir? " +
                    "Ona bir soru sorduğunda, kelime kelime en olası devamı oluşturur. Bu yüzden " +
                    "akıllı gibi görünür - bazen saçmalasa bile."),
                new(
                    "Der Bibliothekar-Vergleich", "Kütüphaneci benzetmesi",
                    "Stell dir einen Bibliothekar vor, der Millionen Bücher gelesen hat, aber kein " +
                    "einziges dabei hat. Er antwortet aus dem Gedächtnis - meistens richtig, manchmal " +
                    "verwechselt er etwas. Genau so arbeitet eine KI: Sie schlägt nichts nach, sie " +
                    "erinnert sich an Muster. Nachschlagen und Prüfen bleibt dein Job.",
                    "Milyonlarca kitap okumuş ama yanında tek kitap olmayan bir kütüphaneci düşün. " +
                    "Hafızasından cevap verir - çoğunlukla doğru, bazen karıştırır. Yapay zeka da " +
                    "aynen böyle çalışır: Hiçbir şeye bakmaz, kalıpları hatırlar. Kontrol etmek " +
                    "senin görevindir.")
            }),
        new KiModule(
            "ki-im-alltag", "📱",
            "KI im Alltag", "Günlük hayatta yapay zeka",
            new List<KiSection>
            {
                new(
                    "Du benutzt schon lange KI", "Zaten uzun süredir yapay zeka kullanıyorsun",
                    "Die Handykamera, die Gesichter scharf stellt. Die Videoplattform, die dir das " +
                    "nächste Video vorschlägt. Die Autokorrektur, die deine Tippfehler ausbessert. " +
                    "Der Übersetzer für Deutsch und Türkisch. Das alles ist KI - meist unsichtbar " +
                    "eingebaut.",
                    "Yüzleri netleştiren telefon kamerası. Sana bir sonraki videoyu öneren video " +
                    "platformu. Yazım hatalarını düzelten otomatik düzeltme. Almanca-Türkçe çeviri " +
                    "uygulaması. Bunların hepsi yapay zeka - çoğu zaman görünmez şekilde yerleştirilmiş."),
                new(
                    "Empfehlungen wollen dich halten", "Öneriler seni tutmak ister",
                    "Wenn eine App dir Videos oder Beiträge vorschlägt, hat sie ein Ziel: dass du " +
                    "möglichst lange bleibst. Sie zeigt dir nicht das Wichtigste, sondern das, was " +
                    "dich am längsten fesselt. Wer das weiß, kann bewusster entscheiden, wann Schluss ist.",
                    "Bir uygulama sana video veya gönderi önerdiğinde bir amacı vardır: mümkün " +
                    "olduğunca uzun kalman. Sana en önemliyi değil, seni en çok bağlayanı gösterir. " +
                    "Bunu bilen, ne zaman duracağına daha bilinçli karar verebilir."),
                new(
                    "KI in Spielen", "Oyunlarda yapay zeka",
                    "Gegner, die auf deine Züge reagieren, oder Welten, die sich anpassen - auch in " +
                    "Spielen steckt KI. Der Unterschied zu einer Chat-KI: Spiel-KI folgt oft festen " +
                    "Regeln, die Entwickler eingebaut haben. Beide wirken lebendig, sind aber Programme.",
                    "Hamlelerine tepki veren rakipler veya kendini uyarlayan dünyalar - oyunlarda da " +
                    "yapay zeka vardır. Sohbet yapay zekasından farkı: Oyun yapay zekası çoğunlukla " +
                    "geliştiricilerin koyduğu sabit kurallara uyar. İkisi de canlı gibi görünür ama " +
                    "programdır.")
            }),
        new KiModule(
            "sicher-mit-ki", "🛡️",
            "Sicher mit KI", "Yapay zekayla güvende",
            new List<KiSection>
            {
                new(
                    "Halluzinationen: sicher klingender Unsinn", "Halüsinasyonlar: emin görünen saçmalık",
                    "Eine KI kann Dinge erfinden und sie trotzdem völlig überzeugt aufschreiben - " +
                    "erfundene Jahreszahlen, falsche Namen, Bücher, die es nicht gibt. Das nennt man " +
                    "Halluzination. Es ist ein technischer Fehler, keine Lüge - aber für dich heißt " +
                    "es: Wichtige Fakten immer in einer zweiten Quelle prüfen.",
                    "Yapay zeka bir şeyler uydurabilir ve yine de bunları tamamen emin bir şekilde " +
                    "yazabilir - uydurma yıllar, yanlış isimler, var olmayan kitaplar. Buna " +
                    "halüsinasyon denir. Bu teknik bir hatadır, yalan değil - ama senin için anlamı " +
                    "şu: Önemli bilgileri her zaman ikinci bir kaynaktan kontrol et."),
                new(
                    "Bias: Schieflagen aus den Daten", "Önyargı: verilerden gelen çarpıklıklar",
                    "Eine KI lernt aus Texten, die Menschen geschrieben haben - mit allen Vorurteilen " +
                    "darin. Wenn in den Trainingsdaten z.B. Ärzte fast immer Männer sind, schlägt die " +
                    "KI eher Männer vor. Das ist kein böser Wille, sondern ein Spiegel der Daten. " +
                    "Deshalb gilt: KI-Antworten sind eine Meinung aus Mustern, nicht die Wahrheit.",
                    "Yapay zeka, insanların yazdığı metinlerden öğrenir - içindeki tüm önyargılarla " +
                    "birlikte. Örneğin eğitim verilerinde doktorlar neredeyse hep erkekse, yapay zeka " +
                    "da erkekleri önerir. Bu kötü niyet değil, verilerin aynasıdır. Bu yüzden: Yapay " +
                    "zeka cevapları kalıplardan çıkan bir görüştür, gerçeğin kendisi değil."),
                new(
                    "Deine Daten gehören dir", "Verilerin sana aittir",
                    "Gib einer KI im Internet nie deinen vollen Namen, deine Adresse, deine Schule " +
                    "oder Fotos von dir. Alles, was du eintippst, kann gespeichert werden. Unsere " +
                    "LernTor-KI läuft übrigens komplett auf diesem PC - sie schickt nichts ins " +
                    "Internet. Bei Online-KIs weißt du das nie so genau.",
                    "İnternetteki bir yapay zekaya asla tam adını, adresini, okulunu veya " +
                    "fotoğraflarını verme. Yazdığın her şey kaydedilebilir. Bu arada LernTor'un " +
                    "yapay zekası tamamen bu bilgisayarda çalışır - internete hiçbir şey göndermez. " +
                    "Online yapay zekalarda bunu asla tam bilemezsin."),
                new(
                    "Echt oder KI-gemacht?", "Gerçek mi, yapay zeka yapımı mı?",
                    "KI kann täuschend echte Bilder, Stimmen und Videos erzeugen. Achte auf Details: " +
                    "seltsame Hände oder Ohren, verschwommene Schrift im Hintergrund, zu glatte Haut, " +
                    "Licht aus unmöglichen Richtungen. Und frag dich immer: Wer hat das gepostet - " +
                    "und warum? Im Zweifel: nicht weiterleiten.",
                    "Yapay zeka gerçeğinden ayırt edilemeyen resimler, sesler ve videolar üretebilir. " +
                    "Detaylara dikkat et: tuhaf eller veya kulaklar, arka planda bulanık yazılar, " +
                    "fazla pürüzsüz cilt, imkansız yönlerden gelen ışık. Ve her zaman kendine sor: " +
                    "Bunu kim paylaştı - ve neden? Şüphedeysen: iletme.")
            })
    };
}
