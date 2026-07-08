using LernTor.Core.Models;

namespace LernTor.Core.Services;

/// <summary>
/// Fester Pool an Gedichten/wichtigen Werken (gemeinfrei) für den täglichen Pflicht-Leseabschnitt.
/// Jedes Stück liegt parallel auf Deutsch, Türkisch und Englisch vor, damit es in der ReadingView
/// als drei Spalten nebeneinander gelesen werden kann. Übersetzungen ins Deutsche/Türkische bzw.
/// Englische, die im Original in einer anderen Sprache verfasst sind, sind eigene, freie
/// Übertragungen zum Vorlesen (keine Zitate bestehender fremder Übersetzungen), um Urheberrechte
/// Dritter (Übersetzer) nicht zu verletzen.
///
/// Rotiert nach Tag im Jahr, damit an einem Tag alle Aufrufe dasselbe Stück zeigen (egal wie oft die
/// App neu gestartet wird), am nächsten Tag aber ein neues dran ist.
/// </summary>
public static class ReadingContentProvider
{
    private static readonly IReadOnlyList<ReadingPiece> Pool = new List<ReadingPiece>
    {
        new()
        {
            Title = "Wandrers Nachtlied",
            Author = "Johann Wolfgang von Goethe",
            TextDe = "Über allen Gipfeln\n" +
                     "Ist Ruh,\n" +
                     "In allen Wipfeln\n" +
                     "Spürest du\n" +
                     "Kaum einen Hauch;\n" +
                     "Die Vögelein schweigen im Walde.\n" +
                     "Warte nur, balde\n" +
                     "Ruhest du auch.",
            TextEn = "Over all the peaks\n" +
                     "There is peace,\n" +
                     "In every treetop\n" +
                     "You feel\n" +
                     "Hardly a breath;\n" +
                     "The little birds fall silent in the forest.\n" +
                     "Just wait, soon\n" +
                     "You too will rest.",
            TextTr = "Bütün doruklarda\n" +
                     "Huzur var,\n" +
                     "Bütün ağaç tepelerinde\n" +
                     "Neredeyse hiç\n" +
                     "Bir esinti duymazsın;\n" +
                     "Kuşlar sessizleşti ormanda.\n" +
                     "Sabret biraz, yakında\n" +
                     "Sen de dinleneceksin."
        },
        new()
        {
            Title = "Die Lorelei (Auszug)",
            Author = "Heinrich Heine",
            TextDe = "Ich weiß nicht, was soll es bedeuten,\n" +
                     "Dass ich so traurig bin;\n" +
                     "Ein Märchen aus alten Zeiten,\n" +
                     "Das kommt mir nicht aus dem Sinn.\n\n" +
                     "Die Luft ist kühl und es dunkelt,\n" +
                     "Und ruhig fließt der Rhein;\n" +
                     "Der Gipfel des Berges funkelt\n" +
                     "Im Abendsonnenschein.",
            TextEn = "I don't know what it should mean,\n" +
                     "That I am so sad;\n" +
                     "A fairy tale from olden times,\n" +
                     "That will not leave my mind.\n\n" +
                     "The air is cool and it grows dark,\n" +
                     "And calmly flows the Rhine;\n" +
                     "The mountain's peak is glimmering\n" +
                     "In the evening sunshine.",
            TextTr = "Bilmiyorum ne anlama gelmeli,\n" +
                     "Neden bu kadar hüzünlüyüm;\n" +
                     "Eski zamanlardan bir masal,\n" +
                     "Aklımdan hiç çıkmıyor.\n\n" +
                     "Hava serin ve karanlık çöküyor,\n" +
                     "Ren nehri sakince akıyor;\n" +
                     "Dağın zirvesi parıldıyor\n" +
                     "Akşam güneşinin ışığında."
        },
        new()
        {
            Title = "İstiklal Marşı (ilk iki kıta)",
            Author = "Mehmet Akif Ersoy",
            TextTr = "Korkma, sönmez bu şafaklarda yüzen al sancak;\n" +
                     "Sönmeden yurdumun üstünde tüten en son ocak.\n" +
                     "O benim milletimin yıldızıdır, parlayacak;\n" +
                     "O benimdir, o benim milletimindir ancak.\n\n" +
                     "Çatma, kurban olayım, çehreni ey nazlı hilal!\n" +
                     "Kahraman ırkıma bir gül! Ne bu şiddet, bu celal?\n" +
                     "Sana olmaz dökülen kanlarımız sonra helal…\n" +
                     "Hakkıdır, Hakk'a tapan, milletimin istiklal!",
            TextDe = "Fürchte dich nicht, diese rote Fahne, die in der Morgendämmerung weht, wird nicht verlöschen;\n" +
                     "solange der letzte Rauch über meiner Heimat aufsteigt, wird sie nicht verlöschen.\n" +
                     "Sie ist der Stern meines Volkes, sie wird erstrahlen;\n" +
                     "sie gehört mir, sie gehört allein meinem Volk.\n\n" +
                     "Runzle nicht die Stirn, ich flehe dich an, du zarter Halbmond!\n" +
                     "Lächle meinem heldenhaften Volk zu! Warum diese Härte, dieser Zorn?\n" +
                     "Sonst wäre das für dich vergossene Blut meines Volkes umsonst gewesen…\n" +
                     "Die Freiheit ist das Recht meines Volkes, das an Gott glaubt!",
            TextEn = "Fear not! For the crimson flag that proudly ripples in this glorious dawn shall not fade,\n" +
                     "before the last hearth that is burning in my homeland is extinguished.\n" +
                     "That is the star of my nation, and it shall forever shine;\n" +
                     "it is mine, it belongs solely to my nation.\n\n" +
                     "Frown not, I beg you, oh coy crescent!\n" +
                     "Smile upon my heroic nation! Why this anger, why this rage?\n" +
                     "Otherwise, the blood shed for you by my people will not have been worthy...\n" +
                     "Freedom is the absolute right of my nation who worships God!"
        },
        new()
        {
            Title = "Gelin Tanış Olalım",
            Author = "Yunus Emre",
            TextTr = "Gelin tanış olalım,\n" +
                     "İşi kolay kılalım,\n" +
                     "Sevelim sevilelim,\n" +
                     "Dünya kimseye kalmaz.\n\n" +
                     "Ne var bu dünyanın işi,\n" +
                     "Ne şirin imiş bir bakışı,\n" +
                     "Anlar mı bilmez bunu,\n" +
                     "Meğer sohbetsiz kişi.",
            TextDe = "Kommt, lasst uns einander kennenlernen,\n" +
                     "lasst uns die Dinge leicht machen,\n" +
                     "lasst uns lieben und geliebt werden,\n" +
                     "diese Welt bleibt niemandem für immer.\n\n" +
                     "Was ist schon die Sache dieser Welt,\n" +
                     "wie süß ist doch ein einziger Blick,\n" +
                     "wer das nicht versteht,\n" +
                     "der hat wohl nie mit jemandem geredet.",
            TextEn = "Come, let us get to know one another,\n" +
                     "let us make things easy,\n" +
                     "let us love and be loved,\n" +
                     "this world remains with no one forever.\n\n" +
                     "What is the matter of this world,\n" +
                     "how sweet a single glance can be,\n" +
                     "whoever does not understand this\n" +
                     "must be someone who never truly talked with another."
        },
        new()
        {
            Title = "The Road Not Taken (Auszug)",
            Author = "Robert Frost",
            TextEn = "Two roads diverged in a yellow wood,\n" +
                     "And sorry I could not travel both\n" +
                     "And be one traveler, long I stood\n" +
                     "And looked down one as far as I could\n" +
                     "To where it bent in the undergrowth;\n\n" +
                     "Then took the other, as just as fair,\n" +
                     "And having perhaps the better claim,\n" +
                     "Because it was grassy and wanted wear;\n" +
                     "Though as for that the passing there\n" +
                     "Had worn them really about the same,",
            TextDe = "Zwei Wege trennten sich im gelben Wald,\n" +
                     "und da ich leider nicht beide gehen konnte\n" +
                     "als ein einzelner Reisender, stand ich lange\n" +
                     "und blickte den einen Weg hinab, so weit ich konnte,\n" +
                     "bis dahin, wo er sich im Unterholz verlor;\n\n" +
                     "Dann nahm ich den anderen, ebenso schön,\n" +
                     "und vielleicht mit dem besseren Anspruch,\n" +
                     "weil er grasbewachsen war und Abnutzung brauchte;\n" +
                     "obwohl, was das angeht, der Weg dort\n" +
                     "eigentlich genauso ausgetreten war,",
            TextTr = "Sarı bir ormanda iki yol ayrıldı,\n" +
                     "ve ne yazık ki ikisini birden gidemedim\n" +
                     "tek bir yolcu olarak, uzun süre durdum\n" +
                     "ve birinin ne kadar uzağa gittiğine baktım\n" +
                     "çalılıkların içinde kıvrıldığı yere kadar;\n\n" +
                     "Sonra diğerini seçtim, o da en az onun kadar güzeldi,\n" +
                     "belki de daha haklı bir tercihti,\n" +
                     "çünkü çimenliydi ve az kullanılmıştı;\n" +
                     "gerçi oradan geçenler açısından\n" +
                     "ikisi de neredeyse aynı derecede aşınmıştı,"
        },
        new()
        {
            Title = "The Tyger",
            Author = "William Blake",
            TextEn = "Tyger Tyger, burning bright,\n" +
                     "In the forests of the night;\n" +
                     "What immortal hand or eye,\n" +
                     "Could frame thy fearful symmetry?\n\n" +
                     "In what distant deeps or skies,\n" +
                     "Burnt the fire of thine eyes?\n" +
                     "On what wings dare he aspire?\n" +
                     "What the hand, dare seize the fire?",
            TextDe = "Tiger, Tiger, hell lodernd,\n" +
                     "in den Wäldern der Nacht;\n" +
                     "welche unsterbliche Hand oder welches Auge\n" +
                     "könnte deine furchterregende Symmetrie erschaffen?\n\n" +
                     "In welchen fernen Tiefen oder Himmeln\n" +
                     "brannte das Feuer deiner Augen?\n" +
                     "Auf welchen Schwingen wagte er sich zu erheben?\n" +
                     "Welche Hand wagte es, das Feuer zu ergreifen?",
            TextTr = "Kaplan, Kaplan, parıl parıl yanan,\n" +
                     "gecenin ormanlarında;\n" +
                     "hangi ölümsüz el ya da göz\n" +
                     "senin o korkunç simetrini yaratabilirdi?\n\n" +
                     "Hangi uzak derinliklerde ya da göklerde\n" +
                     "gözlerinin ateşi yandı?\n" +
                     "Hangi kanatlarla yükselmeye cesaret etti?\n" +
                     "Hangi el, ateşi tutmaya cesaret etti?"
        },
        new()
        {
            Title = "Grundgesetz für die Bundesrepublik Deutschland, Artikel 1",
            Author = "Deutscher Bundestag",
            TextDe = "(1) Die Würde des Menschen ist unantastbar. Sie zu achten und zu schützen ist " +
                     "Verpflichtung aller staatlichen Gewalt.\n\n" +
                     "(2) Das Deutsche Volk bekennt sich darum zu unverletzlichen und unveräußerlichen " +
                     "Menschenrechten als Grundlage jeder menschlichen Gemeinschaft, des Friedens und der " +
                     "Gerechtigkeit in der Welt.",
            TextEn = "(1) Human dignity shall be inviolable. To respect and protect it shall be the duty of " +
                     "all state authority.\n\n" +
                     "(2) The German people therefore acknowledge inviolable and inalienable human rights as " +
                     "the basis of every community, of peace and of justice in the world.",
            TextTr = "(1) İnsan onuru dokunulmazdır. Ona saygı göstermek ve onu korumak, tüm devlet erkinin " +
                     "görevidir.\n\n" +
                     "(2) Alman halkı bu nedenle, her insan topluluğunun, dünyadaki barışın ve adaletin " +
                     "temeli olarak dokunulmaz ve vazgeçilmez insan haklarını benimser."
        },
        new()
        {
            Title = "Berlin – kurz erklärt",
            Author = "Allgemeinwissen",
            TextDe = "Berlin ist die Hauptstadt der Bundesrepublik Deutschland und zugleich eines der 16 " +
                     "Bundesländer. Die Stadt ist in zwölf Bezirke aufgeteilt und hat rund 3,8 Millionen " +
                     "Einwohnerinnen und Einwohner, damit ist sie die bevölkerungsreichste Stadt Deutschlands.\n\n" +
                     "Berlin war von 1961 bis 1989 durch die Berliner Mauer geteilt, die Ost- und West-Berlin " +
                     "trennte. Seit dem Mauerfall am 9. November 1989 und der Wiedervereinigung 1990 ist Berlin " +
                     "wieder eine ungeteilte Stadt und seit 1999 wieder Regierungssitz.",
            TextEn = "Berlin is the capital of the Federal Republic of Germany and at the same time one of its " +
                     "16 federal states. The city is divided into twelve districts and has around 3.8 million " +
                     "residents, making it Germany's most populous city.\n\n" +
                     "From 1961 to 1989, Berlin was divided by the Berlin Wall, which separated East and West " +
                     "Berlin. Since the fall of the Wall on November 9, 1989, and reunification in 1990, Berlin " +
                     "has once again been an undivided city, and since 1999 the seat of government again.",
            TextTr = "Berlin, Almanya Federal Cumhuriyeti'nin başkentidir ve aynı zamanda 16 eyaletten biridir. " +
                     "Şehir on iki ilçeye ayrılmıştır ve yaklaşık 3,8 milyon nüfusuyla Almanya'nın en kalabalık " +
                     "şehridir.\n\n" +
                     "Berlin, 1961'den 1989'a kadar Doğu ve Batı Berlin'i ayıran Berlin Duvarı ile ikiye " +
                     "bölünmüştü. 9 Kasım 1989'da duvarın yıkılmasından ve 1990'daki yeniden birleşmeden bu " +
                     "yana Berlin tekrar bölünmemiş bir şehir, 1999'dan beri de yeniden hükümet merkezidir."
        },
        new()
        {
            Title = "An die Freude (Auszug)",
            Author = "Friedrich Schiller",
            TextDe = "Freude, schöner Götterfunken,\n" +
                     "Tochter aus Elysium,\n" +
                     "Wir betreten feuertrunken,\n" +
                     "Himmlische, dein Heiligtum.\n\n" +
                     "Deine Zauber binden wieder,\n" +
                     "Was die Mode streng geteilt;\n" +
                     "Alle Menschen werden Brüder,\n" +
                     "Wo dein sanfter Flügel weilt.",
            TextEn = "Joy, thou beauteous godly sparkle,\n" +
                     "Daughter from Elysium,\n" +
                     "Fire-drunken we now enter,\n" +
                     "Heavenly one, thy holy shrine.\n\n" +
                     "Thy magic reunites again\n" +
                     "What custom's sword has strictly parted;\n" +
                     "All people become brothers,\n" +
                     "Where thy gentle wing does rest.",
            TextTr = "Sevinç, güzel tanrısal kıvılcım,\n" +
                     "Elysium'un kızı,\n" +
                     "Ateşle sarhoş olmuş gireriz,\n" +
                     "Ey semavi, senin kutsal mekânına.\n\n" +
                     "Senin büyün yeniden birleştirir,\n" +
                     "Geleneğin sertçe ayırdığını;\n" +
                     "Bütün insanlar kardeş olur,\n" +
                     "Senin yumuşak kanadının durduğu yerde."
        },
        new()
        {
            Title = "Der Panther (Auszug)",
            Author = "Rainer Maria Rilke",
            TextDe = "Sein Blick ist vom Vorübergehn der Stäbe\n" +
                     "so müd geworden, dass er nichts mehr hält.\n" +
                     "Ihm ist, als ob es tausend Stäbe gäbe\n" +
                     "und hinter tausend Stäben keine Welt.",
            TextEn = "His gaze has grown so weary from the passing\n" +
                     "of bars, that it no longer holds a thing.\n" +
                     "To him it seems as though a thousand bars exist,\n" +
                     "and past a thousand bars there is no world.",
            TextTr = "Bakışı, geçip giden parmaklıklardan\n" +
                     "o kadar yorgun düştü ki artık hiçbir şeyi tutmuyor.\n" +
                     "Ona sanki bin tane parmaklık varmış\n" +
                     "ve bin parmaklığın ardında hiçbir dünya yokmuş gibi geliyor."
        },
        new()
        {
            Title = "Ben Gelmedim Dava İçin",
            Author = "Yunus Emre",
            TextTr = "Ben gelmedim dava için,\n" +
                     "Benim işim sevi için.\n" +
                     "Dostun evi gönüllerdir,\n" +
                     "Gönüller yapmaya geldim.",
            TextDe = "Ich bin nicht für den Streit gekommen,\n" +
                     "meine Aufgabe ist die Liebe.\n" +
                     "Das Haus des Freundes sind die Herzen,\n" +
                     "ich bin gekommen, um Herzen zu bauen.",
            TextEn = "I have not come for quarrel,\n" +
                     "my task is love alone.\n" +
                     "The friend's true home is people's hearts,\n" +
                     "I have come to build hearts."
        },
        new()
        {
            Title = "Aşkın Aldı Benden Beni",
            Author = "Yunus Emre",
            TextTr = "Aşkın aldı benden beni,\n" +
                     "Bana seni gerek seni.\n" +
                     "Ben yanarım dünü günü,\n" +
                     "Bana seni gerek seni.",
            TextDe = "Deine Liebe hat mich mir selbst genommen,\n" +
                     "ich brauche dich, nur dich.\n" +
                     "Ich brenne Tag und Nacht,\n" +
                     "ich brauche dich, nur dich.",
            TextEn = "Your love has taken me from myself,\n" +
                     "I need you, only you.\n" +
                     "I burn day and night,\n" +
                     "I need you, only you."
        },
        new()
        {
            Title = "I'm Nobody! Who are you?",
            Author = "Emily Dickinson",
            TextEn = "I'm Nobody! Who are you?\n" +
                     "Are you – Nobody – too?\n" +
                     "Then there's a pair of us!\n" +
                     "Don't tell! they'd advertise – you know!",
            TextDe = "Ich bin Niemand! Wer bist du?\n" +
                     "Bist du – Niemand – auch?\n" +
                     "Dann sind wir ein Paar!\n" +
                     "Sag nichts! Sie würden es bekannt machen – weißt du!",
            TextTr = "Ben Hiçkimseyim! Sen kimsin?\n" +
                     "Sen de mi – Hiçkimse – misin?\n" +
                     "O zaman ikimiz bir çiftiz!\n" +
                     "Söyleme! Bunu herkese duyururlar – bilirsin!"
        },
        new()
        {
            Title = "Twinkle, Twinkle, Little Star (Auszug)",
            Author = "Jane Taylor",
            TextEn = "Twinkle, twinkle, little star,\n" +
                     "How I wonder what you are!\n" +
                     "Up above the world so high,\n" +
                     "Like a diamond in the sky.",
            TextDe = "Funkle, funkle, kleiner Stern,\n" +
                     "wie gerne wüsste ich, was du bist!\n" +
                     "Hoch über der Welt, so weit oben,\n" +
                     "wie ein Diamant am Himmel.",
            TextTr = "Parıl parıl, küçük yıldız,\n" +
                     "keşke bilsem ne olduğunu!\n" +
                     "Dünyanın çok yukarısında,\n" +
                     "gökyüzündeki bir elmas gibi."
        },
        new()
        {
            Title = "Deutschland – kurz erklärt",
            Author = "Allgemeinwissen",
            TextDe = "Deutschland ist ein Land in Mitteleuropa mit rund 84 Millionen Einwohnerinnen und Einwohnern. " +
                     "Es besteht aus 16 Bundesländern und die Hauptstadt ist Berlin.\n\n" +
                     "Deutschland ist bekannt für seine Vielfalt an Dialekten, seine starke Wirtschaft und als " +
                     "Gründungsmitglied der Europäischen Union. Die Nationalflagge zeigt die Farben Schwarz, " +
                     "Rot und Gold.",
            TextEn = "Germany is a country in Central Europe with around 84 million residents. It consists of " +
                     "16 federal states, and its capital is Berlin.\n\n" +
                     "Germany is known for its variety of dialects, its strong economy, and as a founding " +
                     "member of the European Union. Its national flag shows the colors black, red, and gold.",
            TextTr = "Almanya, yaklaşık 84 milyon nüfusuyla Orta Avrupa'da bir ülkedir. 16 eyaletten oluşur ve " +
                     "başkenti Berlin'dir.\n\n" +
                     "Almanya, lehçe çeşitliliğiyle, güçlü ekonomisiyle ve Avrupa Birliği'nin kurucu " +
                     "üyelerinden biri olmasıyla bilinir. Ulusal bayrağı siyah, kırmızı ve altın renklerini taşır."
        },
        new()
        {
            Title = "Die Türkei – kurz erklärt",
            Author = "Allgemeinwissen",
            TextDe = "Die Türkei ist ein Land, das teils in Europa und teils in Asien liegt - man nennt das auch " +
                     "eine \"transkontinentale\" Lage. Die größte Stadt ist Istanbul, die Hauptstadt ist jedoch " +
                     "Ankara.\n\n" +
                     "Istanbul liegt am Bosporus, einer Meerenge, die Europa und Asien voneinander trennt. Die " +
                     "Türkei hat rund 85 Millionen Einwohnerinnen und Einwohner und ist bekannt für ihre lange " +
                     "Geschichte, zum Beispiel als Zentrum des Osmanischen Reiches.",
            TextEn = "Turkey is a country that lies partly in Europe and partly in Asia - this is also called " +
                     "a \"transcontinental\" location. Its largest city is Istanbul, but its capital is Ankara.\n\n" +
                     "Istanbul lies on the Bosphorus, a strait that separates Europe and Asia. Turkey has " +
                     "around 85 million residents and is known for its long history, for example as the " +
                     "center of the Ottoman Empire.",
            TextTr = "Türkiye, kısmen Avrupa'da kısmen de Asya'da yer alan bir ülkedir - buna \"kıtalar arası\" " +
                     "konum da denir. En büyük şehri İstanbul'dur, ancak başkenti Ankara'dır.\n\n" +
                     "İstanbul, Avrupa ile Asya'yı birbirinden ayıran bir boğaz olan Boğaziçi üzerinde yer alır. " +
                     "Türkiye'nin yaklaşık 85 milyon nüfusu vardır ve Osmanlı İmparatorluğu'nun merkezi olması " +
                     "gibi uzun tarihiyle bilinir."
        },
    };

    public static ReadingPiece GetForDate(DateOnly date)
    {
        var index = date.DayOfYear % Pool.Count;
        return Pool[index];
    }
}
