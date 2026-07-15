using LernTor.Core.Models;

namespace LernTor.Core.Services;

/// <summary>
/// Fester Pool an Gedichten/wichtigen Werken (gemeinfrei) sowie eigens verfassten Kurzgeschichten für
/// den täglichen Pflicht-Leseabschnitt (aktuell 60 Stücke). Jedes Stück liegt parallel auf Deutsch,
/// Türkisch und Englisch vor, damit es in der ReadingView als drei Spalten nebeneinander gelesen
/// werden kann. Übersetzungen ins Deutsche/Türkische bzw. Englische, die im Original in einer
/// anderen Sprache verfasst sind, sind eigene, freie Übertragungen zum Vorlesen (keine Zitate
/// bestehender fremder Übersetzungen), um Urheberrechte Dritter (Übersetzer) nicht zu verletzen.
///
/// Die "Original-Geschichte (inspiriert von ...)"-Stücke referenzieren bekannte Marken/Figuren
/// (z.B. Minecraft, Super Mario, Batman) nur namentlich als Thema - Handlung, Dialoge und Wortlaut
/// sind komplett neu verfasst, es wird kein geschütztes Spiel-/Comic-/Anime-Material zitiert oder
/// reproduziert.
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
        new()
        {
            Title = "Der Mond ist aufgegangen (Auszug)",
            Author = "Matthias Claudius",
            TextDe = "Der Mond ist aufgegangen,\n" +
                     "Die goldnen Sternlein prangen\n" +
                     "Am Himmel hell und klar;\n" +
                     "Der Wald steht schwarz und schweiget,\n" +
                     "Und aus den Wiesen steiget\n" +
                     "Der weiße Nebel wunderbar.",
            TextEn = "The moon has risen high,\n" +
                     "The little golden stars shine\n" +
                     "Bright and clear in the sky;\n" +
                     "The forest stands black and silent,\n" +
                     "And from the meadows rises\n" +
                     "The white mist, wondrously.",
            TextTr = "Ay yükseldi göğe,\n" +
                     "Altın yıldızlar parlıyor\n" +
                     "Gökyüzünde aydınlık ve berrak;\n" +
                     "Orman kapkara durur ve susar,\n" +
                     "Ve çayırlardan yükselir\n" +
                     "Beyaz sis, harika bir şekilde."
        },
        new()
        {
            Title = "Mondnacht (Auszug)",
            Author = "Joseph von Eichendorff",
            TextDe = "Es war, als hätt' der Himmel\n" +
                     "Die Erde still geküsst,\n" +
                     "Dass sie im Blütenschimmer\n" +
                     "Von ihm nun träumen müsst.",
            TextEn = "It was as if the sky\n" +
                     "Had quietly kissed the earth,\n" +
                     "So that in blossom's shimmer\n" +
                     "She now must dream of him.",
            TextTr = "Sanki gökyüzü\n" +
                     "Dünyayı sessizce öpmüştü,\n" +
                     "Öyle ki o, çiçek parıltısı içinde\n" +
                     "Şimdi onu düşlemek zorundaydı."
        },
        new()
        {
            Title = "Max und Moritz (Vorwort, Auszug)",
            Author = "Wilhelm Busch",
            TextDe = "Ach, was muss man oft von bösen\n" +
                     "Kindern hören oder lesen!\n" +
                     "Wie zum Beispiel hier von diesen,\n" +
                     "Welche Max und Moritz hießen,\n" +
                     "Die, anstatt durch weise Lehren\n" +
                     "Sich zum Guten zu bekehren,\n" +
                     "Oftmals noch darüber lachten\n" +
                     "Und sich heimlich lustig machten.",
            TextEn = "Oh, how often one must hear or read\n" +
                     "About wicked children!\n" +
                     "Like, for example, here about these two,\n" +
                     "Who were called Max and Moritz,\n" +
                     "Who, instead of turning to goodness\n" +
                     "Through wise teachings,\n" +
                     "Often just laughed about it\n" +
                     "And secretly made fun of it.",
            TextTr = "Ah, kötü çocuklar hakkında\n" +
                     "Ne çok şey duymak ya da okumak zorunda kalıyor insan!\n" +
                     "Mesela burada anlatılan bu ikisi gibi,\n" +
                     "Adları Max ve Moritz'di,\n" +
                     "Akıllıca öğütlerle\n" +
                     "İyiye yönelmek yerine,\n" +
                     "Çoğu zaman bunlara sadece güldüler\n" +
                     "Ve gizlice alay ettiler."
        },
        new()
        {
            Title = "Der gute Kamerad (Auszug)",
            Author = "Ludwig Uhland",
            TextDe = "Ich hatt' einen Kameraden,\n" +
                     "Einen bessern findst du nit.\n" +
                     "Die Trommel schlug zum Streite,\n" +
                     "Er ging an meiner Seite\n" +
                     "In gleichem Schritt und Tritt.",
            TextEn = "I had a comrade,\n" +
                     "You will not find a better one.\n" +
                     "The drum called us to battle,\n" +
                     "He walked at my side\n" +
                     "In the very same step.",
            TextTr = "Bir yoldaşım vardı,\n" +
                     "Daha iyisini bulamazsın.\n" +
                     "Davul savaşa çağırdığında,\n" +
                     "O benim yanımda yürüdü\n" +
                     "Aynı adımla, aynı ritimle."
        },
        new()
        {
            Title = "Dandini Dandini Dastana",
            Author = "Anonim (Türk Halk Ninnisi)",
            TextTr = "Dandini dandini dastana,\n" +
                     "Danalar girmiş bostana,\n" +
                     "Kov bostancı danayı,\n" +
                     "Yemesin lahanayı.",
            TextDe = "Dandini dandini, schlaf mein Kind,\n" +
                     "Kälbchen sind in den Kohlgarten gelaufen,\n" +
                     "Vertreibe sie, lieber Gärtner,\n" +
                     "Lass sie den Kohl nicht essen.",
            TextEn = "Dandini dandini, hush now,\n" +
                     "The little calves ran into the cabbage garden,\n" +
                     "Chase them away, dear gardener,\n" +
                     "Don't let them eat the cabbage."
        },
        new()
        {
            Title = "Nasreddin Hoca ve Kazan Hikâyesi",
            Author = "Anonim (Türk Halk Fıkrası)",
            TextTr = "Bir gün Nasreddin Hoca, komşusundan büyük bir kazan ödünç aldı. Geri götürdüğünde içine " +
                     "küçük bir tencere koydu. Komşusu şaşırarak sordu: 'Bu da ne?' Nasreddin Hoca, 'Senin kazanın " +
                     "yavruladı' dedi. Komşu çok sevindi ve iki kabı da aldı.\n\n" +
                     "Birkaç hafta sonra Nasreddin Hoca kazanı yine ödünç aldı - ama bu sefer geri götürmedi. " +
                     "Komşu kazanını sorunca Nasreddin Hoca üzgün bir sesle, 'Maalesef kazanın öldü' dedi. Komşu, " +
                     "'Kazan nasıl ölür!' diye bağırdı. Nasreddin Hoca sakince, 'Ama az önce yavruladığına " +
                     "inanmıştın' dedi.",
            TextDe = "Eines Tages lieh sich Nasreddin Hodscha von seinem Nachbarn einen großen Kochtopf. Als er " +
                     "ihn zurückbrachte, legte er einen kleinen Topf hinein. 'Was ist das?', fragte der Nachbar " +
                     "erstaunt. 'Dein Topf hat ein Junges bekommen', antwortete Nasreddin Hodscha. Der Nachbar " +
                     "freute sich und nahm beide Töpfe.\n\n" +
                     "Ein paar Wochen später lieh sich Nasreddin Hodscha den großen Topf erneut - doch diesmal " +
                     "brachte er ihn nicht zurück. Als der Nachbar fragte, wo sein Topf sei, seufzte Nasreddin " +
                     "Hodscha traurig: 'Dein Topf ist leider gestorben.' - 'Ein Topf kann doch nicht sterben!', " +
                     "rief der Nachbar. 'Aber vorhin hast du geglaubt, dass er ein Junges bekommen hat', " +
                     "antwortete Nasreddin Hodscha ruhig.",
            TextEn = "One day, Nasreddin Hodja borrowed a large cooking pot from his neighbor. When he returned " +
                     "it, he placed a small pot inside it. 'What is this?' the neighbor asked, surprised. 'Your " +
                     "pot had a baby,' answered Nasreddin Hodja. The neighbor was delighted and took both pots.\n\n" +
                     "A few weeks later, Nasreddin Hodja borrowed the large pot again - but this time he did not " +
                     "bring it back. When the neighbor asked where his pot was, Nasreddin Hodja sighed sadly: " +
                     "'Your pot has died, unfortunately.' - 'A pot cannot die!' the neighbor cried. 'But earlier, " +
                     "you believed it had a baby,' Nasreddin Hodja replied calmly."
        },
        new()
        {
            Title = "My Shadow (Auszug)",
            Author = "Robert Louis Stevenson",
            TextEn = "I have a little shadow that goes in and out with me,\n" +
                     "And what can be the use of him is more than I can see.\n" +
                     "He is very, very like me from the heels up to the head;\n" +
                     "And I see him jump before me, when I jump into my bed.",
            TextDe = "Ich habe einen kleinen Schatten, der mit mir aus und ein geht,\n" +
                     "Und wozu er eigentlich gut ist, verstehe ich nicht ganz.\n" +
                     "Er ist mir sehr, sehr ähnlich, von den Fersen bis zum Kopf;\n" +
                     "Und ich sehe ihn vor mir springen, wenn ich in mein Bett springe.",
            TextTr = "Küçük bir gölgem var, benimle içeri dışarı gidiyor,\n" +
                     "Ve onun ne işe yaradığını tam olarak anlayamıyorum.\n" +
                     "Bana çok, çok benziyor, topuklarımdan başıma kadar;\n" +
                     "Ve yatağıma zıpladığımda, onun önümde zıpladığını görüyorum."
        },
        new()
        {
            Title = "Jabberwocky (Auszug, Unsinnsgedicht)",
            Author = "Lewis Carroll",
            TextEn = "'Twas brillig, and the slithy toves\n" +
                     "Did gyre and gimble in the wabe;\n" +
                     "All mimsy were the borogoves,\n" +
                     "And the mome raths outgrabe.",
            TextDe = "Es war brillig, und die schlichten Toven\n" +
                     "wirbelten und gimbelten im Wabe;\n" +
                     "ganz mimsig waren die Borogoven,\n" +
                     "und die fernen Raths outgraben.\n\n" +
                     "(Hinweis: Das englische Original erfindet bewusst Fantasiewörter - eine wörtliche " +
                     "Übersetzung ist deshalb nicht möglich.)",
            TextTr = "Uğultuydu vakit, kaygan tovlar\n" +
                     "dönüyor, gimbelliyordu wabe'de;\n" +
                     "bütün mimsi'ydi borogovlar,\n" +
                     "ve uzak rathlar outgrabe'liyordu.\n\n" +
                     "(Not: İngilizce orijinali bilerek uydurma kelimeler kullanır - bu yüzden tam bir çeviri " +
                     "mümkün değildir.)"
        },
        new()
        {
            Title = "I Wandered Lonely as a Cloud (Auszug)",
            Author = "William Wordsworth",
            TextEn = "I wandered lonely as a cloud\n" +
                     "That floats on high o'er vales and hills,\n" +
                     "When all at once I saw a crowd,\n" +
                     "A host, of golden daffodils;\n" +
                     "Beside the lake, beneath the trees,\n" +
                     "Fluttering and dancing in the breeze.",
            TextDe = "Ich wanderte einsam wie eine Wolke,\n" +
                     "Die hoch über Täler und Hügel schwebt,\n" +
                     "Als ich plötzlich eine Menge sah,\n" +
                     "Eine Schar goldener Narzissen;\n" +
                     "Neben dem See, unter den Bäumen,\n" +
                     "Flatternd und tanzend im Wind.",
            TextTr = "Yalnız bir bulut gibi dolaştım,\n" +
                     "Vadilerin ve tepelerin üzerinde yüksekte süzülen,\n" +
                     "Birden bir kalabalık gördüğümde,\n" +
                     "Altın rengi nergislerden oluşan bir topluluk;\n" +
                     "Gölün kenarında, ağaçların altında,\n" +
                     "Rüzgârda çırpınıp dans ederek."
        },
        new()
        {
            Title = "Who Has Seen the Wind?",
            Author = "Christina Rossetti",
            TextEn = "Who has seen the wind?\n" +
                     "Neither I nor you:\n" +
                     "But when the leaves hang trembling,\n" +
                     "The wind is passing through.",
            TextDe = "Wer hat den Wind gesehen?\n" +
                     "Weder ich noch du:\n" +
                     "Aber wenn die Blätter zitternd hängen,\n" +
                     "Zieht der Wind hindurch.",
            TextTr = "Rüzgârı kim gördü?\n" +
                     "Ne ben ne de sen:\n" +
                     "Ama yapraklar titreyerek asılıyken,\n" +
                     "Rüzgâr oradan geçiyordur."
        },
        new()
        {
            Title = "Das Sonnensystem – kurz erklärt",
            Author = "Allgemeinwissen",
            TextDe = "Unser Sonnensystem besteht aus der Sonne und allem, was sie mit ihrer Schwerkraft anzieht: " +
                     "acht Planeten, viele Monde, Zwergplaneten, Asteroiden und Kometen. Die Sonne selbst ist ein " +
                     "Stern und macht über 99 Prozent der gesamten Masse des Sonnensystems aus.\n\n" +
                     "Die acht Planeten heißen, von der Sonne aus gesehen: Merkur, Venus, Erde, Mars, Jupiter, " +
                     "Saturn, Uranus und Neptun. Die Erde ist bisher der einzige bekannte Planet, auf dem es " +
                     "Leben gibt.",
            TextEn = "Our solar system consists of the Sun and everything it attracts with its gravity: eight " +
                     "planets, many moons, dwarf planets, asteroids, and comets. The Sun itself is a star and " +
                     "makes up more than 99 percent of the solar system's total mass.\n\n" +
                     "The eight planets, seen from the Sun outward, are named: Mercury, Venus, Earth, Mars, " +
                     "Jupiter, Saturn, Uranus, and Neptune. Earth is so far the only known planet where life " +
                     "exists.",
            TextTr = "Güneş sistemimiz, Güneş'ten ve onun yer çekimiyle çektiği her şeyden oluşur: sekiz " +
                     "gezegen, birçok uydu, cüce gezegenler, asteroitler ve kuyruklu yıldızlar. Güneş'in kendisi " +
                     "bir yıldızdır ve güneş sisteminin toplam kütlesinin yüzde 99'undan fazlasını oluşturur.\n\n" +
                     "Güneş'ten dışarıya doğru sekiz gezegenin adları şöyledir: Merkür, Venüs, Dünya, Mars, " +
                     "Jüpiter, Satürn, Uranüs ve Neptün. Dünya, şimdiye kadar üzerinde yaşam olduğu bilinen tek " +
                     "gezegendir."
        },
        new()
        {
            Title = "Die Menschenrechte – kurz erklärt",
            Author = "Allgemeinwissen",
            TextDe = "Die Allgemeine Erklärung der Menschenrechte wurde 1948 von den Vereinten Nationen " +
                     "verabschiedet. Sie legt fest, dass alle Menschen frei und gleich an Würde und Rechten " +
                     "geboren werden - unabhängig von Herkunft, Sprache, Religion oder Geschlecht.\n\n" +
                     "Zu den Menschenrechten gehören zum Beispiel das Recht auf Leben und Freiheit, das Recht " +
                     "auf Bildung und das Recht, seine Meinung frei zu äußern. Die Menschenrechte gelten für " +
                     "jeden Menschen auf der Welt, egal in welchem Land er lebt.",
            TextEn = "The Universal Declaration of Human Rights was adopted by the United Nations in 1948. It " +
                     "states that all human beings are born free and equal in dignity and rights - regardless " +
                     "of origin, language, religion, or gender.\n\n" +
                     "Human rights include, for example, the right to life and freedom, the right to education, " +
                     "and the right to freely express one's opinion. Human rights apply to every person in the " +
                     "world, no matter which country they live in.",
            TextTr = "İnsan Hakları Evrensel Beyannamesi, 1948 yılında Birleşmiş Milletler tarafından kabul " +
                     "edildi. Bu beyanname, tüm insanların köken, dil, din veya cinsiyet fark etmeksizin özgür " +
                     "ve onur ile haklar bakımından eşit doğduğunu belirtir.\n\n" +
                     "İnsan hakları arasında örneğin yaşam ve özgürlük hakkı, eğitim hakkı ve fikrini özgürce " +
                     "ifade etme hakkı bulunur. İnsan hakları, dünyanın hangi ülkesinde yaşarsa yaşasın her " +
                     "insan için geçerlidir."
        },
        new()
        {
            Title = "Istanbul – kurz erklärt",
            Author = "Allgemeinwissen",
            TextDe = "Istanbul ist die größte Stadt der Türkei mit über 15 Millionen Einwohnerinnen und " +
                     "Einwohnern, aber nicht die Hauptstadt - das ist Ankara. Istanbul liegt auf zwei " +
                     "Kontinenten: Ein Teil der Stadt liegt in Europa, der andere in Asien, getrennt durch den " +
                     "Bosporus.\n\n" +
                     "Die Stadt war historisch als Byzanz und später als Konstantinopel bekannt und war " +
                     "Hauptstadt sowohl des Byzantinischen als auch des Osmanischen Reiches. Heute ist Istanbul " +
                     "für seine Moscheen wie die Hagia Sophia und die Blaue Moschee sowie für seinen belebten " +
                     "Großen Basar bekannt.",
            TextEn = "Istanbul is the largest city in Turkey, with more than 15 million residents, but it is " +
                     "not the capital - that is Ankara. Istanbul lies on two continents: one part of the city " +
                     "is in Europe, the other in Asia, separated by the Bosphorus.\n\n" +
                     "The city was historically known as Byzantium and later as Constantinople, and it was the " +
                     "capital of both the Byzantine and the Ottoman Empire. Today, Istanbul is known for its " +
                     "mosques such as the Hagia Sophia and the Blue Mosque, as well as for its lively Grand " +
                     "Bazaar.",
            TextTr = "İstanbul, 15 milyondan fazla nüfusuyla Türkiye'nin en büyük şehridir, ancak başkent " +
                     "değildir - başkent Ankara'dır. İstanbul iki kıtada yer alır: şehrin bir kısmı Avrupa'da, " +
                     "diğer kısmı ise Asya'dadır ve bu iki yaka Boğaziçi ile ayrılır.\n\n" +
                     "Şehir tarihte önce Bizans, sonra Konstantinopolis olarak bilinmiştir ve hem Bizans hem de " +
                     "Osmanlı İmparatorluğu'nun başkenti olmuştur. Bugün İstanbul; Ayasofya ve Sultanahmet " +
                     "Camii gibi camileriyle ve hareketli Kapalıçarşı'sıyla tanınır."
        },
        new()
        {
            Title = "Die Europäische Union – kurz erklärt",
            Author = "Allgemeinwissen",
            TextDe = "Die Europäische Union (EU) ist ein Zusammenschluss von derzeit 27 europäischen Ländern, " +
                     "die in vielen Bereichen zusammenarbeiten - zum Beispiel bei Handel, Reisefreiheit und " +
                     "teilweise auch bei einer gemeinsamen Währung, dem Euro.\n\n" +
                     "Dank der EU können Menschen in vielen Mitgliedsländern ohne Passkontrolle reisen, " +
                     "arbeiten und studieren. Deutschland ist eines der Gründungsmitglieder der EU, die Türkei " +
                     "ist bislang kein Mitglied, arbeitet aber seit Langem mit der EU zusammen.",
            TextEn = "The European Union (EU) is an association of currently 27 European countries that " +
                     "cooperate in many areas - for example in trade, freedom of travel, and in part also " +
                     "through a shared currency, the euro.\n\n" +
                     "Thanks to the EU, people in many member countries can travel, work, and study without " +
                     "passport checks. Germany is one of the founding members of the EU; Turkey is not yet a " +
                     "member but has cooperated with the EU for a long time.",
            TextTr = "Avrupa Birliği (AB), şu anda 27 Avrupa ülkesinin ticaret, seyahat özgürlüğü ve kısmen " +
                     "ortak bir para birimi olan Euro gibi birçok alanda iş birliği yaptığı bir birliktir.\n\n" +
                     "AB sayesinde birçok üye ülkedeki insanlar pasaport kontrolü olmadan seyahat edebilir, " +
                     "çalışabilir ve okuyabilir. Almanya, AB'nin kurucu üyelerinden biridir; Türkiye henüz üye " +
                     "değildir ama uzun zamandır AB ile iş birliği yapmaktadır."
        },
        new()
        {
            Title = "Minecraft-Abenteuer: Der verlorene Diamant",
            Author = "Original-Geschichte (inspiriert von Minecraft)",
            TextDe = "Steve stand tief in einer dunklen Höhle und sein Fackellicht flackerte. Irgendwo in der " +
                     "Nähe hörte er das leise Zischen eines Creepers. Vorsichtig grub er durch den Stein, bis " +
                     "er endlich einen glitzernden Diamanten fand. 'Endlich!', rief er und baute schnell eine " +
                     "Wand aus Stein, bevor der Creeper näherkommen konnte. Mit dem Diamanten in der Tasche " +
                     "kletterte er zurück ans Tageslicht.",
            TextEn = "Steve stood deep in a dark cave, his torchlight flickering. Somewhere nearby, he heard " +
                     "the quiet hiss of a creeper. Carefully he dug through the stone until he finally found a " +
                     "sparkling diamond. 'Finally!' he shouted, quickly building a wall of stone before the " +
                     "creeper could get closer. With the diamond in his pocket, he climbed back up to daylight.",
            TextTr = "Steve, karanlık bir mağaranın derinliklerinde duruyordu ve meşale ışığı titriyordu. " +
                     "Yakınlarda bir yerde bir Creeper'ın hafif tıslamasını duydu. Dikkatlice taşı kazdı, " +
                     "sonunda parıldayan bir elmas buldu. 'Sonunda!' diye bağırdı ve Creeper yaklaşamadan " +
                     "hızlıca taştan bir duvar ördü. Elmas cebindeyken gün ışığına doğru tekrar tırmandı."
        },
        new()
        {
            Title = "Minecraft-Abenteuer: Das Dorf der Villager",
            Author = "Original-Geschichte (inspiriert von Minecraft)",
            TextDe = "Als Alex ein kleines Dorf entdeckte, waren die Villager gerade dabei, ihre Felder zu " +
                     "bestellen. Ein Zombie hatte in der Nacht zuvor das Tor beschädigt. Alex sammelte Holz und " +
                     "Eisen und reparierte gemeinsam mit den Dorfbewohnern die Mauer. Am Abend tauschte sie " +
                     "Weizen gegen Smaragde ein und half so, das Dorf sicherer zu machen.",
            TextEn = "When Alex discovered a small village, the villagers were busy tending their fields. A " +
                     "zombie had damaged the gate the night before. Alex gathered wood and iron and repaired " +
                     "the wall together with the villagers. In the evening, she traded wheat for emeralds and " +
                     "helped make the village a little safer.",
            TextTr = "Alex küçük bir köy keşfettiğinde, köylüler tam da tarlalarını ekiyordu. Önceki gece bir " +
                     "zombi kapıyı hasara uğratmıştı. Alex odun ve demir topladı, köylülerle birlikte duvarı " +
                     "onardı. Akşam olunca buğdayı zümrütlerle takas etti ve köyü biraz daha güvenli hale " +
                     "getirmeye yardım etti."
        },
        new()
        {
            Title = "Minecraft-Abenteuer: Reise zum Nether",
            Author = "Original-Geschichte (inspiriert von Minecraft)",
            TextDe = "Mit einem selbstgebauten Portal aus Obsidian betrat Finn zum ersten Mal den Nether. Die " +
                     "Luft war heiß, überall floss Lava, und in der Ferne hörte er ein Piglin grunzen. Er " +
                     "tauschte vorsichtig etwas Gold gegen seltene Gegenstände und achtete darauf, keinem " +
                     "Ghast zu nahe zu kommen. Erschöpft, aber stolz, kehrte er mit einer Kiste voller " +
                     "Nether-Schätze zurück.",
            TextEn = "Using a portal he built from obsidian, Finn entered the Nether for the very first time. " +
                     "The air was hot, lava flowed everywhere, and in the distance he heard a piglin grunt. He " +
                     "carefully traded some gold for rare items and made sure not to get too close to a ghast. " +
                     "Exhausted but proud, he returned with a chest full of Nether treasures.",
            TextTr = "Finn, obsidyenden kendi yaptığı bir kapıyla ilk kez Nether'e girdi. Hava sıcaktı, her " +
                     "yerde lav akıyordu ve uzaktan bir Piglin'in homurtusunu duydu. Dikkatlice biraz altını " +
                     "nadir eşyalarla takas etti ve bir Ghast'a fazla yaklaşmamaya özen gösterdi. Yorgun ama " +
                     "gururlu bir şekilde, Nether hazineleriyle dolu bir sandıkla geri döndü."
        },
        new()
        {
            Title = "Super Mario: Der Sprung über die Lava",
            Author = "Original-Geschichte (inspiriert von Super Mario Bros.)",
            TextDe = "Mario rannte durch die Festung von Bowser, während unter ihm Lava brodelte. Mit einem " +
                     "mutigen Sprung erreichte er die nächste Plattform, sammelte eine leuchtende Münze ein und " +
                     "wich einem fliegenden Koopa-Panzer aus. Am Ende des Levels wartete schon die nächste " +
                     "Herausforderung - aber Mario war bereit.",
            TextEn = "Mario ran through Bowser's fortress while lava bubbled beneath him. With a brave jump, " +
                     "he reached the next platform, collected a glowing coin, and dodged a flying Koopa shell. " +
                     "At the end of the level, the next challenge was already waiting - but Mario was ready.",
            TextTr = "Mario, altında lavlar kaynarken Bowser'ın kalesinde koşuyordu. Cesur bir sıçrayışla bir " +
                     "sonraki platforma ulaştı, parlayan bir madeni para topladı ve uçan bir Koopa kabuğundan " +
                     "kaçındı. Bölümün sonunda bir sonraki zorluk çoktan bekliyordu - ama Mario hazırdı."
        },
        new()
        {
            Title = "Super Mario: Luigis mutiger Plan",
            Author = "Original-Geschichte (inspiriert von Super Mario Bros.)",
            TextDe = "Luigi zitterte ein wenig, als er das gruselige Schloss betrat, aber er dachte an seinen " +
                     "Bruder Mario und fasste Mut. Mit seiner Taschenlampe erhellte er dunkle Gänge und fing " +
                     "vorsichtig ein paar schüchterne Geister ein. Am Ende fand er den Schlüssel, der Mario aus " +
                     "seiner misslichen Lage befreien konnte.",
            TextEn = "Luigi trembled a little as he entered the spooky mansion, but he thought of his brother " +
                     "Mario and found his courage. With his flashlight, he lit up dark hallways and carefully " +
                     "captured a few shy ghosts. In the end, he found the key that could free Mario from his " +
                     "difficult situation.",
            TextTr = "Luigi, ürkütücü şatoya girerken biraz titredi, ama kardeşi Mario'yu düşünüp cesaretini " +
                     "topladı. Fenerini kullanarak karanlık koridorları aydınlattı ve birkaç çekingen hayaleti " +
                     "dikkatlice yakaladı. Sonunda, Mario'yu zor durumundan kurtarabilecek anahtarı buldu."
        },
        new()
        {
            Title = "Super Mario: Das Rennen auf der Regenbogenstrecke",
            Author = "Original-Geschichte (inspiriert von Super Mario Bros.)",
            TextDe = "Beim großen Kart-Rennen auf der Regenbogenstrecke lag Peach knapp in Führung. Toad " +
                     "versuchte mit einem Turbo-Pilz aufzuholen, während Yoshi geschickt einer Bananenschale " +
                     "auswich. Kurz vor der Ziellinie überholte Toad in letzter Sekunde - ein knappes, " +
                     "spannendes Rennen bis zum Schluss.",
            TextEn = "During the big kart race on Rainbow Road, Peach was narrowly in the lead. Toad tried to " +
                     "catch up with a turbo mushroom, while Yoshi skillfully dodged a banana peel. Just before " +
                     "the finish line, Toad overtook in the very last second - a close, exciting race right to " +
                     "the end.",
            TextTr = "Gökkuşağı Pisti'ndeki büyük kart yarışında Peach kıl payı önde gidiyordu. Toad turbo " +
                     "mantarla yetişmeye çalışırken, Yoshi bir muz kabuğundan ustaca kaçındı. Bitiş çizgisine " +
                     "gelmeden hemen önce Toad son saniyede öne geçti - sonuna kadar heyecanlı, kıyasıya bir " +
                     "yarış."
        },
        new()
        {
            Title = "AntonCraft: Der Bauplan für die Festung",
            Author = "Original-Geschichte (inspiriert von AntonCraft)",
            TextDe = "In seiner Minecraft-Welt begann Anton mit dem Bau einer riesigen Festung aus Stein und " +
                     "Holz. Zuerst plante er die Grundmauern, dann fügte er Türme und ein großes Tor hinzu. " +
                     "Seine Freunde in der Community gaben ihm Tipps in den Kommentaren, wie er die Festung " +
                     "noch sicherer machen könnte.",
            TextEn = "In his Minecraft world, Anton began building a huge fortress out of stone and wood. " +
                     "First, he planned the foundation walls, then he added towers and a large gate. His " +
                     "friends in the community gave him tips in the comments on how to make the fortress even " +
                     "safer.",
            TextTr = "Minecraft dünyasında Anton, taş ve tahtadan devasa bir kale inşa etmeye başladı. Önce " +
                     "temel duvarları planladı, sonra kuleler ve büyük bir kapı ekledi. Topluluktaki " +
                     "arkadaşları, kaleyi nasıl daha güvenli hale getirebileceği konusunda yorumlarda ona " +
                     "ipuçları verdi."
        },
        new()
        {
            Title = "AntonCraft: Ein Tag mit Freunden im Multiplayer",
            Author = "Original-Geschichte (inspiriert von AntonCraft)",
            TextDe = "Gemeinsam mit seinen Freunden im Multiplayer-Modus baute Anton eine Achterbahn quer durch " +
                     "die ganze Welt. Es gab viele Lacher, als jemand versehentlich ins Wasser fiel, und alle " +
                     "halfen zusammen, das nächste Teilstück der Strecke zu bauen. Am Ende der Sitzung war die " +
                     "Achterbahn fertig - und alle waren stolz auf ihr gemeinsames Werk.",
            TextEn = "Together with his friends in multiplayer mode, Anton built a roller coaster across the " +
                     "entire world. There were plenty of laughs when someone accidentally fell into the water, " +
                     "and everyone helped build the next section of the track together. By the end of the " +
                     "session, the roller coaster was finished - and everyone was proud of their shared " +
                     "creation.",
            TextTr = "Anton, çok oyunculu modda arkadaşlarıyla birlikte tüm dünyayı baştan sona kat eden bir " +
                     "hız treni inşa etti. Biri yanlışlıkla suya düştüğünde çok güldüler ve herkes parkurun bir " +
                     "sonraki bölümünü birlikte inşa etmeye yardım etti. Oturumun sonunda hız treni " +
                     "tamamlanmıştı - ve herkes ortak eserleriyle gurur duyuyordu."
        },
        new()
        {
            Title = "Geometry Dash: Der schwierige Sprung",
            Author = "Original-Geschichte (inspiriert von Geometry Dash)",
            TextDe = "Nach dutzenden Versuchen hatte Mia endlich den Rhythmus des Levels verinnerlicht. Im " +
                     "Takt der Musik ließ sie ihren Würfel über spitze Hindernisse springen und durch enge " +
                     "Lücken fliegen. Kurz vor dem Ende scheiterte sie fast an einem doppelten Sprung, doch sie " +
                     "konzentrierte sich und schaffte es bis zur Ziellinie.",
            TextEn = "After dozens of attempts, Mia had finally internalized the rhythm of the level. In time " +
                     "with the music, she made her cube jump over spiky obstacles and fly through narrow gaps. " +
                     "Just before the end, she almost failed at a double jump, but she focused and made it all " +
                     "the way to the finish line.",
            TextTr = "Onlarca denemeden sonra Mia sonunda bölümün ritmini içselleştirmişti. Müziğin temposuna " +
                     "ayak uydurarak küpünü sivri engellerin üzerinden atlattı ve dar boşluklardan uçurdu. " +
                     "Bitişe çok az kala çift zıplamada neredeyse başarısız oluyordu, ama odaklandı ve bitiş " +
                     "çizgisine kadar başardı."
        },
        new()
        {
            Title = "Geometry Dash: Ein eigenes Level bauen",
            Author = "Original-Geschichte (inspiriert von Geometry Dash)",
            TextDe = "Ben verbrachte den ganzen Nachmittag damit, ein eigenes Level im Editor zu gestalten. Er " +
                     "platzierte Sprungpads, wechselte die Hintergrundfarbe im Takt der Musik und testete jeden " +
                     "Abschnitt mehrmals. Als er das Level schließlich mit Freunden teilte, war er gespannt, " +
                     "wie schwer sie es finden würden.",
            TextEn = "Ben spent the whole afternoon designing his own level in the editor. He placed jump " +
                     "pads, changed the background color in time with the music, and tested every section " +
                     "multiple times. When he finally shared the level with friends, he was curious how " +
                     "difficult they would find it.",
            TextTr = "Ben, öğleden sonrasının tamamını editörde kendi bölümünü tasarlayarak geçirdi. Zıplama " +
                     "pedleri yerleştirdi, arka plan rengini müziğin temposuna göre değiştirdi ve her bölümü " +
                     "birkaç kez test etti. Sonunda bölümü arkadaşlarıyla paylaştığında, onların ne kadar zor " +
                     "bulacağını merak ediyordu."
        },
        new()
        {
            Title = "Rock'n'Roll: Die erste Bandprobe",
            Author = "Original-Geschichte",
            TextDe = "In der Garage stimmten Elif und ihre Freunde ihre Instrumente: E-Gitarre, Bass und " +
                     "Schlagzeug. Nach ein paar holprigen Versuchen fanden sie endlich einen gemeinsamen " +
                     "Rhythmus. Der treibende Rock'n'Roll-Beat wurde von Mal zu Mal fester, und am Ende der " +
                     "Probe hatten sie ihren ersten eigenen Song fast fertig.",
            TextEn = "In the garage, Elif and her friends tuned their instruments: electric guitar, bass, and " +
                     "drums. After a few bumpy attempts, they finally found a shared rhythm. The driving " +
                     "rock'n'roll beat grew tighter each time, and by the end of the rehearsal, they had almost " +
                     "finished their very first original song.",
            TextTr = "Garajda Elif ve arkadaşları enstrümanlarını akort ettiler: elektro gitar, bas ve davul. " +
                     "Birkaç sarsıntılı denemeden sonra sonunda ortak bir ritim buldular. Sürükleyici " +
                     "rock'n'roll ritmi her seferinde daha da oturdu ve provanın sonunda ilk kendi şarkılarını " +
                     "neredeyse tamamlamışlardı."
        },
        new()
        {
            Title = "Rock'n'Roll: Die Geschichte eines Musikstils",
            Author = "Allgemeinwissen",
            TextDe = "Rock'n'Roll entstand in den 1950er-Jahren in den USA aus einer Mischung von Blues, " +
                     "Country und Gospel. Er wurde schnell zur Musik einer ganzen Jugendgeneration, die zu " +
                     "treibenden Gitarrenriffs und mitreißenden Rhythmen tanzte. Bis heute prägt Rock'n'Roll " +
                     "viele moderne Musikstile, von Rockmusik bis Pop.",
            TextEn = "Rock'n'roll emerged in the USA in the 1950s from a mix of blues, country, and gospel. It " +
                     "quickly became the music of an entire generation of young people, who danced to driving " +
                     "guitar riffs and infectious rhythms. To this day, rock'n'roll continues to shape many " +
                     "modern music styles, from rock to pop.",
            TextTr = "Rock'n'roll, 1950'lerde ABD'de blues, country ve gospel'in bir karışımından doğdu. Kısa " +
                     "sürede, sürükleyici gitar riflerine ve coşturucu ritimlere dans eden koca bir gençlik " +
                     "kuşağının müziği hâline geldi. Rock'n'roll, günümüze kadar rock müzikten pop'a kadar " +
                     "birçok modern müzik tarzını etkilemeye devam ediyor."
        },
        new()
        {
            Title = "Pac-Man: Die Flucht durch das Labyrinth",
            Author = "Original-Geschichte (inspiriert von Pac-Man)",
            TextDe = "Mit vollem Tempo raste Pac-Man durch die engen Gänge des Labyrinths, immer dicht " +
                     "gefolgt von den bunten Geistern Blinky und Pinky. Gerade rechtzeitig erreichte er einen " +
                     "der großen Power-Pellets und drehte den Spieß um: Jetzt waren es die Geister, die fliehen " +
                     "mussten.",
            TextEn = "At full speed, Pac-Man raced through the narrow corridors of the maze, closely followed " +
                     "by the colorful ghosts Blinky and Pinky. Just in time, he reached one of the large power " +
                     "pellets and turned the tables: now it was the ghosts who had to flee.",
            TextTr = "Pac-Man, labirentin dar koridorlarında tam hızla koşuyordu, renkli hayaletler Blinky ve " +
                     "Pinky onu yakından takip ediyordu. Tam zamanında büyük güç haplarından birine ulaştı ve " +
                     "durumu tersine çevirdi: şimdi kaçması gerekenler hayaletlerdi."
        },
        new()
        {
            Title = "Pac-Man: Ein Highscore für die Ewigkeit",
            Author = "Original-Geschichte (inspiriert von Pac-Man)",
            TextDe = "Jeden Tag nach der Schule übte Deniz am alten Pac-Man-Automaten im Jugendzentrum. Er " +
                     "lernte die Muster der Geister genau kennen und wusste irgendwann, welchen Weg er nehmen " +
                     "musste, um jede Frucht einzusammeln. Als er schließlich den höchsten Highscore der Woche " +
                     "erreichte, jubelten alle seine Freunde.",
            TextEn = "Every day after school, Deniz practiced on the old Pac-Man arcade machine at the youth " +
                     "center. He got to know the ghosts' patterns exactly and eventually knew which path to " +
                     "take to collect every piece of fruit. When he finally reached the highest score of the " +
                     "week, all his friends cheered.",
            TextTr = "Deniz her gün okuldan sonra gençlik merkezindeki eski Pac-Man makinesinde pratik " +
                     "yapıyordu. Hayaletlerin hareket kalıplarını iyice öğrendi ve sonunda her meyveyi toplamak " +
                     "için hangi yolu izlemesi gerektiğini biliyordu. Sonunda haftanın en yüksek skoruna " +
                     "ulaştığında tüm arkadaşları onu tezahüratla kutladı."
        },
        new()
        {
            Title = "Kirby: Der rosa Held saugt alles auf",
            Author = "Original-Geschichte (inspiriert von Kirby Super Star)",
            TextDe = "Kirby rollte fröhlich durch die Blumenwiesen von Dream Land, als plötzlich ein fieser " +
                     "Gegner auftauchte. Ohne zu zögern sog Kirby ihn ein und verwandelte sich in einen " +
                     "Feuer-Kirby mit einer neuen Fähigkeit. Mit einem Feuerball vertrieb er die Bedrohung und " +
                     "rettete das Dorf der Waddle Dees.",
            TextEn = "Kirby rolled happily through the flower meadows of Dream Land when suddenly a nasty " +
                     "enemy appeared. Without hesitation, Kirby inhaled him and transformed into a fire Kirby " +
                     "with a brand-new ability. With a fireball, he chased away the threat and saved the " +
                     "village of the Waddle Dees.",
            TextTr = "Kirby, Dream Land'in çiçekli çayırlarında neşeyle yuvarlanırken birden kötü bir düşman " +
                     "ortaya çıktı. Kirby hiç tereddüt etmeden onu içine çekti ve yeni bir yeteneğe sahip " +
                     "ateş-Kirby'ye dönüştü. Bir ateş topuyla tehdidi kovaladı ve Waddle Dee'lerin köyünü " +
                     "kurtardı."
        },
        new()
        {
            Title = "Kirby: Zusammenhalt mit King Dedede",
            Author = "Original-Geschichte (inspiriert von Kirby Super Star)",
            TextDe = "Obwohl King Dedede und Kirby sich manchmal necken, arbeiteten sie diesmal zusammen, um " +
                     "die geheimnisvollen Sterne zurückzuholen, die aus dem Himmel über Dream Land " +
                     "verschwunden waren. Gemeinsam lösten sie knifflige Rätsel und besiegten am Ende einen " +
                     "gewaltigen Endgegner - Seite an Seite.",
            TextEn = "Even though King Dedede and Kirby sometimes tease each other, this time they worked " +
                     "together to bring back the mysterious stars that had disappeared from the sky above " +
                     "Dream Land. Together, they solved tricky puzzles and, in the end, defeated a huge final " +
                     "boss - side by side.",
            TextTr = "King Dedede ve Kirby bazen birbirleriyle takılsa da, bu sefer Dream Land'in gökyüzünden " +
                     "kaybolan gizemli yıldızları geri getirmek için birlikte çalıştılar. Birlikte zorlu " +
                     "bulmacaları çözdüler ve sonunda dev bir son patronu omuz omuza yenip devirdiler."
        },
        new()
        {
            Title = "Minecraft Legends: Der Angriff der Piglins",
            Author = "Original-Geschichte (inspiriert von Minecraft Legends)",
            TextDe = "Als die Piglin-Horden das friedliche Dorf angriffen, versammelte die junge Heldin " +
                     "schnell Wölfe, Golems und tapfere Verbündete. Gemeinsam bauten sie Verteidigungstürme und " +
                     "trieben die Angreifer mit vereinten Kräften zurück. Am Ende des Kampfes stand das Dorf " +
                     "zwar beschädigt, aber gerettet da.",
            TextEn = "When the piglin hordes attacked the peaceful village, the young hero quickly gathered " +
                     "wolves, golems, and brave allies. Together, they built defense towers and drove the " +
                     "attackers back with combined strength. By the end of the battle, the village stood " +
                     "damaged but saved.",
            TextTr = "Piglin sürüleri huzurlu köye saldırdığında, genç kahraman hızlıca kurtlar, golemler ve " +
                     "cesur müttefikler topladı. Birlikte savunma kuleleri inşa ettiler ve saldırganları " +
                     "birleşik güçleriyle geri püskürttüler. Savaşın sonunda köy hasar görmüş olsa da " +
                     "kurtarılmıştı."
        },
        new()
        {
            Title = "Minecraft Legends: Strategie über den Wolken",
            Author = "Original-Geschichte (inspiriert von Minecraft Legends)",
            TextDe = "Bevor die große Schlacht begann, plante das Team genau, wo sie ihre Verteidigungsanlagen " +
                     "errichten sollten. Sie sammelten Ressourcen aus der Overworld, bauten Katapulte und " +
                     "stimmten ihre Angriffswellen sorgfältig ab. Diese kluge Strategie machte am Ende den " +
                     "entscheidenden Unterschied gegen die Piglin-Übermacht.",
            TextEn = "Before the big battle began, the team carefully planned where to build their defenses. " +
                     "They gathered resources from the Overworld, built catapults, and carefully coordinated " +
                     "their attack waves. In the end, this smart strategy made the decisive difference against " +
                     "the piglin's superior numbers.",
            TextTr = "Büyük savaş başlamadan önce takım, savunma tesislerini nereye kuracaklarını dikkatlice " +
                     "planladı. Overworld'den kaynak topladılar, mancınıklar inşa ettiler ve saldırı dalgalarını " +
                     "özenle koordine ettiler. Sonunda bu akıllı strateji, Piglin'lerin sayısal üstünlüğüne " +
                     "karşı belirleyici farkı yarattı."
        },
        new()
        {
            Title = "Batman: Eine ruhige Nacht in Gotham?",
            Author = "Original-Geschichte (inspiriert von Batman)",
            TextDe = "Vom Dach eines Wolkenkratzers aus beobachtete Batman die Straßen von Gotham City. Ein " +
                     "verdächtiges Geräusch aus einer Seitengasse ließ ihn aufhorchen - jemand versuchte, in " +
                     "ein Geschäft einzubrechen. Lautlos glitt er hinab, überraschte die Einbrecher und übergab " +
                     "sie der Polizei, bevor er wieder in der Nacht verschwand.",
            TextEn = "From the roof of a skyscraper, Batman watched over the streets of Gotham City. A " +
                     "suspicious noise from a side alley made him look up - someone was trying to break into a " +
                     "shop. Silently, he glided down, surprised the burglars, and handed them over to the " +
                     "police before disappearing into the night again.",
            TextTr = "Bir gökdelenin çatısından Batman, Gotham City'nin sokaklarını izliyordu. Bir yan " +
                     "sokaktan gelen şüpheli bir ses dikkatini çekti - biri bir dükkâna girmeye çalışıyordu. " +
                     "Sessizce aşağı süzüldü, hırsızları şaşırttı ve tekrar geceye karışmadan önce onları " +
                     "polise teslim etti."
        },
        new()
        {
            Title = "Batman: Zusammenarbeit mit Commissioner Gordon",
            Author = "Original-Geschichte (inspiriert von Batman)",
            TextDe = "Als am Nachthimmel über Gotham das Bat-Signal aufleuchtete, wusste Batman sofort, dass " +
                     "Commissioner Gordon seine Hilfe brauchte. Gemeinsam werteten sie Hinweise in der " +
                     "Polizeizentrale aus und fanden heraus, wo sich die Täter versteckt hielten. Dank ihrer " +
                     "Zusammenarbeit konnte ein größerer Schaden für die Stadt verhindert werden.",
            TextEn = "When the Bat-Signal lit up the night sky over Gotham, Batman knew immediately that " +
                     "Commissioner Gordon needed his help. Together, they analyzed clues at police headquarters " +
                     "and found out where the culprits were hiding. Thanks to their cooperation, greater harm " +
                     "to the city could be prevented.",
            TextTr = "Gotham'ın gece gökyüzünde Yarasa İşareti belirdiğinde, Batman Komiser Gordon'ın " +
                     "yardımına ihtiyacı olduğunu hemen anladı. Polis merkezinde birlikte ipuçlarını " +
                     "değerlendirdiler ve failin nerede saklandığını buldular. İş birlikleri sayesinde şehre " +
                     "daha büyük bir zarar gelmesi önlendi."
        },
        new()
        {
            Title = "Batman: Der Wert von Mut und Geduld",
            Author = "Original-Geschichte (inspiriert von Batman)",
            TextDe = "Bevor Bruce Wayne zu Batman wurde, verbrachte er Jahre damit, Kampfkunst, " +
                     "Detektivarbeit und Wissenschaft zu erlernen. Er wusste: Echte Stärke kommt nicht nur aus " +
                     "Muskeln, sondern auch aus Geduld, Planung und dem Willen, nie aufzugeben. Diese Lektion " +
                     "half ihm, auch die schwierigsten Fälle in Gotham zu lösen.",
            TextEn = "Before Bruce Wayne became Batman, he spent years learning martial arts, detective work, " +
                     "and science. He knew that true strength comes not just from muscles, but also from " +
                     "patience, planning, and the will to never give up. This lesson helped him solve even the " +
                     "toughest cases in Gotham.",
            TextTr = "Bruce Wayne, Batman olmadan önce yıllarca dövüş sanatları, dedektiflik ve bilim öğrenmekle " +
                     "geçirdi. Gerçek gücün sadece kaslardan değil, sabırdan, planlamadan ve asla vazgeçmeme " +
                     "iradesinden geldiğini biliyordu. Bu ders, Gotham'daki en zor davaları bile çözmesine " +
                     "yardımcı oldu."
        },
        new()
        {
            Title = "One Punch Man: Ein Held aus Langeweile",
            Author = "Original-Geschichte (inspiriert von One Punch Man)",
            TextDe = "Saitama trainierte drei Jahre lang jeden Tag - hundert Liegestütze, hundert Sit-ups, " +
                     "zehn Kilometer laufen - bis er zum stärksten Helden der Welt wurde. Das Problem: Seitdem " +
                     "besiegt er jeden Gegner mit nur einem einzigen Schlag, und kein Kampf ist mehr wirklich " +
                     "spannend für ihn.",
            TextEn = "Saitama trained every single day for three years - a hundred push-ups, a hundred " +
                     "sit-ups, ten kilometers of running - until he became the strongest hero in the world. " +
                     "The problem: ever since, he defeats every opponent with just a single punch, and no fight " +
                     "is truly exciting for him anymore.",
            TextTr = "Saitama, dünyanın en güçlü kahramanı olana kadar üç yıl boyunca her gün antrenman yaptı - " +
                     "yüz şınav, yüz mekik, on kilometre koşu. Sorun şu ki, o zamandan beri her rakibini tek " +
                     "bir yumrukla yeniyor ve artık hiçbir dövüş onun için gerçekten heyecan verici değil."
        },
        new()
        {
            Title = "One Punch Man: Genos und der treue Schüler",
            Author = "Original-Geschichte (inspiriert von One Punch Man)",
            TextDe = "Der Cyborg-Held Genos bewunderte Saitamas unglaubliche Kraft so sehr, dass er ihn bat, " +
                     "sein Schüler werden zu dürfen. Gemeinsam kämpften sie gegen gefährliche Monster, auch " +
                     "wenn Saitama die meiste Zeit lieber im Sonderangebot einkaufen gegangen wäre. Trotzdem " +
                     "lernte Genos viel über Bescheidenheit und echten Heldenmut.",
            TextEn = "The cyborg hero Genos admired Saitama's incredible strength so much that he asked to " +
                     "become his student. Together they fought against dangerous monsters, even though Saitama " +
                     "would have preferred to go shopping for a discount sale most of the time. Still, Genos " +
                     "learned a lot about humility and true heroism.",
            TextTr = "Siborg kahraman Genos, Saitama'nın inanılmaz gücüne o kadar hayrandı ki onun öğrencisi " +
                     "olmak için izin istedi. Saitama çoğu zaman indirimli alışverişe gitmeyi tercih etse de, " +
                     "birlikte tehlikeli canavarlara karşı savaştılar. Yine de Genos, alçakgönüllülük ve " +
                     "gerçek kahramanlık hakkında çok şey öğrendi."
        },
        new()
        {
            Title = "Dragon Ball: Die Suche nach den sieben Kugeln",
            Author = "Original-Geschichte (inspiriert von Dragon Ball)",
            TextDe = "Son-Goku und seine Freunde reisten durch Wüsten, Wälder und über hohe Berge, um alle " +
                     "sieben Dragon Balls zu finden. Jede Kugel brachte neue Abenteuer und neue Gegner mit " +
                     "sich. Als schließlich die letzte Kugel gefunden war, konnten sie den mächtigen Drachen " +
                     "Shenlong rufen, um sich einen Wunsch zu erfüllen.",
            TextEn = "Son Goku and his friends traveled through deserts, forests, and over high mountains to " +
                     "find all seven Dragon Balls. Every single ball brought new adventures and new opponents " +
                     "along with it. When the last ball was finally found, they were able to summon the mighty " +
                     "dragon Shenlong to grant them a wish.",
            TextTr = "Son Goku ve arkadaşları, yedi Dragon Ball'un hepsini bulmak için çöllerden, ormanlardan " +
                     "ve yüksek dağların üzerinden geçerek yolculuk ettiler. Her bir küre yeni maceralar ve " +
                     "yeni rakipler getiriyordu. Sonunda son küre de bulunduğunda, bir dilek tutmak için güçlü " +
                     "ejderha Shenlong'u çağırabildiler."
        },
        new()
        {
            Title = "Dragon Ball: Training für den nächsten Kampf",
            Author = "Original-Geschichte (inspiriert von Dragon Ball)",
            TextDe = "Nach der Niederlage gegen einen mächtigen Gegner zog sich Vegeta zurück, um härter zu " +
                     "trainieren als je zuvor. Unter extremer Schwerkraft übte er neue Techniken, bis seine " +
                     "Muskeln vor Erschöpfung zitterten. Als der nächste Kampf begann, war er spürbar stärker " +
                     "geworden - Fleiß und Ausdauer hatten sich gelohnt.",
            TextEn = "After his defeat against a powerful opponent, Vegeta withdrew to train harder than ever " +
                     "before. Under extreme gravity, he practiced new techniques until his muscles trembled " +
                     "with exhaustion. When the next battle began, he had noticeably grown stronger - hard work " +
                     "and perseverance had paid off.",
            TextTr = "Güçlü bir rakibe karşı aldığı yenilginin ardından Vegeta, her zamankinden daha sıkı " +
                     "antrenman yapmak için bir kenara çekildi. Aşırı yer çekimi altında, kasları yorgunluktan " +
                     "titreyene kadar yeni teknikler üzerinde çalıştı. Bir sonraki dövüş başladığında belirgin " +
                     "biçimde güçlenmişti - emek ve azim işe yaramıştı."
        },
        new()
        {
            Title = "One Piece: Der Traum vom größten Schatz",
            Author = "Original-Geschichte (inspiriert von One Piece)",
            TextDe = "Monkey D. Ruffy träumte schon als Kind davon, den legendären Schatz 'One Piece' zu " +
                     "finden und König der Piraten zu werden. Mit seiner Mannschaft, der Strohhutbande, " +
                     "segelte er von Insel zu Insel und erlebte dabei gefährliche Stürme, seltsame " +
                     "Inselbewohner und spannende Schatzsuchen.",
            TextEn = "Even as a child, Monkey D. Luffy dreamed of finding the legendary treasure called 'One " +
                     "Piece' and becoming King of the Pirates. With his crew, the Straw Hat Pirates, he sailed " +
                     "from island to island, experiencing dangerous storms, strange islanders, and exciting " +
                     "treasure hunts along the way.",
            TextTr = "Monkey D. Luffy, çocukluğundan beri efsanevi hazine 'One Piece'i bulup Korsanlar Kralı " +
                     "olmayı hayal ediyordu. Mürettebatı Hasır Şapka Korsanları ile ada ada yelken açtı ve bu " +
                     "yolculukta tehlikeli fırtınalar, tuhaf ada sakinleri ve heyecan verici hazine avları " +
                     "yaşadı."
        },
        new()
        {
            Title = "One Piece: Zusammenhalt der Mannschaft",
            Author = "Original-Geschichte (inspiriert von One Piece)",
            TextDe = "Als die Strohhutbande in einen heftigen Sturm geriet, arbeitete jedes Crewmitglied genau " +
                     "nach seiner Stärke: Der Steuermann hielt Kurs, der Koch versorgte alle mit Kraft " +
                     "spendendem Essen, und der Arzt kümmerte sich um die Verletzten. Nur durch diesen " +
                     "Zusammenhalt überstand die Mannschaft die gefährliche Nacht auf hoher See.",
            TextEn = "When the Straw Hat crew got caught in a fierce storm, every crew member worked according " +
                     "to their own strength: the navigator kept the course, the cook provided everyone with " +
                     "energizing food, and the doctor looked after the injured. Only through this teamwork did " +
                     "the crew survive the dangerous night at sea.",
            TextTr = "Hasır Şapka mürettebatı şiddetli bir fırtınaya yakalandığında, her mürettebat üyesi " +
                     "kendi güçlü yanına göre çalıştı: dümenci rotayı korudu, aşçı herkese güç veren yemekler " +
                     "hazırladı, doktor ise yaralılarla ilgilendi. Mürettebat, açık denizdeki bu tehlikeli " +
                     "geceyi ancak bu dayanışma sayesinde atlattı."
        },
        new()
        {
            Title = "One Piece: Eine Insel voller Geheimnisse",
            Author = "Original-Geschichte (inspiriert von One Piece)",
            TextDe = "Auf einer nebligen, unbekannten Insel entdeckte die Mannschaft alte Ruinen mit " +
                     "rätselhaften Schriftzeichen. Nami studierte ihre Karte genau, während Robin die alten " +
                     "Inschriften zu entziffern versuchte. Gemeinsam fanden sie einen Hinweis, der sie ihrem " +
                     "großen Ziel ein Stück näherbrachte.",
            TextEn = "On a foggy, unknown island, the crew discovered ancient ruins covered in mysterious " +
                     "writing. Nami studied her map carefully, while Robin tried to decipher the old " +
                     "inscriptions. Together, they found a clue that brought them one step closer to their " +
                     "great goal.",
            TextTr = "Sisli, bilinmeyen bir adada mürettebat, gizemli yazıtlarla kaplı eski kalıntılar keşfetti. " +
                     "Nami haritasını dikkatle incelerken, Robin eski yazıtları çözmeye çalıştı. Birlikte, " +
                     "onları büyük hedeflerine bir adım daha yaklaştıran bir ipucu buldular."
        },
        new()
        {
            Title = "Backrooms: Der endlose gelbe Flur",
            Author = "Original-Geschichte (inspiriert von Backrooms)",
            TextDe = "Plötzlich befand sich Aylin in einem endlosen Flur mit gelben Tapeten und summendem " +
                     "Neonlicht - den sogenannten Backrooms. Der Teppichboden roch feucht und alt, und ihre " +
                     "Schritte hallten seltsam laut wider. Ruhig blieb sie stehen, überlegte genau und folgte " +
                     "schließlich einer leichten Zugluft, die sie zu einem Ausgang führte.",
            TextEn = "Suddenly, Aylin found herself in an endless hallway with yellow wallpaper and humming " +
                     "neon lights - the so-called Backrooms. The carpet smelled damp and old, and her " +
                     "footsteps echoed strangely loud. She stayed calm, thought carefully, and finally followed " +
                     "a slight draft of air that led her to an exit.",
            TextTr = "Aylin birdenbire sarı duvar kağıtlı ve vızıldayan neon ışıklı sonsuz bir koridorda buldu " +
                     "kendini - sözde Backrooms'ta. Halı nemli ve eski kokuyordu, adımları tuhaf bir şekilde " +
                     "yankılanıyordu. Sakin kaldı, dikkatlice düşündü ve sonunda kendisini bir çıkışa götüren " +
                     "hafif bir hava akımını takip etti."
        },
        new()
        {
            Title = "Backrooms: Zusammen ist man weniger allein",
            Author = "Original-Geschichte (inspiriert von Backrooms)",
            TextDe = "Als Kerem sich in den Backrooms verirrte, traf er zum Glück auf zwei andere " +
                     "'Entdecker', die schon länger unterwegs waren. Gemeinsam zeichneten sie eine Karte der " +
                     "Gänge, die sie bereits gesehen hatten, und teilten sich Wasser und Vorräte. Mit " +
                     "vereinten Kräften und viel Geduld fanden sie schließlich einen Weg zurück in die normale " +
                     "Welt.",
            TextEn = "When Kerem got lost in the Backrooms, he luckily met two other 'explorers' who had " +
                     "already been wandering for longer. Together, they drew a map of the hallways they had " +
                     "already seen and shared water and supplies. With combined effort and a lot of patience, " +
                     "they finally found a way back to the normal world.",
            TextTr = "Kerem Backrooms'ta kaybolduğunda, neyse ki daha uzun süredir orada dolaşan iki 'kaşif' " +
                     "ile karşılaştı. Birlikte daha önce gördükleri koridorların bir haritasını çizdiler ve su " +
                     "ile erzaklarını paylaştılar. Birleşik çaba ve bolca sabırla sonunda normal dünyaya dönüş " +
                     "yolunu buldular."
        },
    };

    public static ReadingPiece GetForDate(DateOnly date)
    {
        var index = date.DayOfYear % Pool.Count;
        return Pool[index];
    }

    /// <summary>
    /// Zweiter Lesetext des Tages: um die halbe Pool-Länge versetzt, sodass bei der Pool-Anordnung
    /// (erste Hälfte literarisch/Allgemeinwissen, zweite Hälfte Pop-Kultur) täglich ein Text aus
    /// jeder Hälfte zusammenkommt und nie zweimal derselbe. Zwei ganze Texte statt künstlich
    /// verlängerter Einzeltexte: die literarischen Stücke sind echte, gemeinfreie Gedichte - denen
    /// eine zweite Strophe anzudichten würde die Werke verfälschen.
    /// </summary>
    public static ReadingPiece GetSecondForDate(DateOnly date)
    {
        var index = (date.DayOfYear + Pool.Count / 2) % Pool.Count;
        return Pool[index];
    }
}
