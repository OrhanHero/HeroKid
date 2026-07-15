using LernTor.Core.Models;

namespace LernTor.Core.Services;

/// <summary>
/// Fester Pool an Schreibprompts für den täglichen Pflicht-Schreibabschnitt (5 Minuten).
/// Jeder Prompt ist ein jugendlicher Abenteuer-Textstub, der zum Weiter-/Fertigschreiben einlädt.
/// Die Texte sind bewusst kurz (ca. 80-120 Wörter), damit sie in 5 Minuten gelesen und fortgesetzt werden können.
/// Sprachmix: Deutsch + Türkisch + Englisch gemischt (wie im Lesen), damit sich der Wechsel natürlich anfühlt.
/// Rotiert nach Tag im Jahr (DayOfYear), damit an einem Tag alle Aufrufe denselben Prompt zeigen.
/// </summary>
public static class WritingContentProvider
{
    private static readonly IReadOnlyList<WritingPrompt> Pool = new List<WritingPrompt>
    {
        new()
        {
            Title = "Der geheimnisvolle Koffer",
            PromptDe = "Du findest auf dem Dachboden einen alten, verbeulten Lederkoffer. Kein Schloss, kein Name. Du öffnest ihn vorsichtig. Drin liegt: eine vergilbte Landkarte von Istanbul, ein Kompass, der nicht nach Norden zeigt, und ein Zettel: \"Für den, der mutig genug ist, dem Pfad zu folgen.\" Was tust du? Wohin führt die Karte? Schreibe weiter!",
            PromptEn = "You find an old, dented leather suitcase in the attic. No lock, no name. You open it carefully. Inside: a yellowed map of Istanbul, a compass that doesn't point north, and a note: \"For the one brave enough to follow the path.\" What do you do? Where does the map lead? Continue writing!",
            PromptTr = "Çatı katında eski, çukurlu bir deri bavul bulursun. Kilit yok, isim yok. Dikkatle açarsın. İçinde: İstanbul'un solmuş bir haritası, kuzeyi göstermeden bir pusula ve bir not: \"Yolu takip edecek kadar cesur olan için.\" Ne yaparsın? Harita nereye götürür? Devamını yaz!"
        },
        new()
        {
            Title = "Die Nachricht in der Flasche",
            PromptDe = "Du spazierst am Strand von Samsun entlang, die Wellen spülen Schaum über deine Füße. Da siehst du sie: eine grüne Glasflasche, verkorkt, mit einem Zettel drin. Du ziehst den Korken, entfaltest das Papier. Drei Sätze, drei Sprachen: \"Hilfe. Ich bin gefangen. Koordinaten: 41.2867, 36.33.\" Dein Herz rast. Was tust du? Schreibe weiter!",
            PromptEn = "You're walking on the beach in Samsun, waves foaming at your feet. You spot it: a green glass bottle, corked, with a note inside. You pull the cork, unfold the paper. Three sentences, three languages: \"Help. I'm trapped. Coordinates: 41.2867, 36.33.\" Your heart races. What do you do? Continue writing!",
            PromptTr = "Samsun sahilinde yürüyorsun, dalgalar ayaklarına köpük saçıyor. Onu görüyorsun: yeşil bir cam şişe, mürekkepli, içinde bir kağıt. Mürekkeyi çekersin, kağıdı açarsın. Üç cümle, üç dil: \"Yardım. Tutsak kaldım. Koordinatlar: 41.2867, 36.33.\" Kalbin çarpıyor. Ne yaparsın? Devamını yaz!"
        },
        new()
        {
            Title = "Das Portal im Keller",
            PromptDe = "In eurer neuen Wohnung in Berlin-Kreuzberg gibt es eine Tür im Keller, die gestern noch nicht da war. Einfach so: dunkles Holz, eiserne Beschläge, kein Schloss. Du fasst an die Klinke, drückst sie runter. Dahinter kein Kellerflur – sondern ein Basar! Gewürzduft, laute Stimmen, bunte Stoffe. Ein alter Mann winkt: \"Gel, oğlum, zaman dar.\" Was passiert? Schreibe weiter!",
            PromptEn = "In your new flat in Berlin-Kreuzberg, there's a cellar door that wasn't there yesterday. Dark wood, iron fittings, no lock. You touch the handle, push it down. Behind it: no cellar corridor – but a bazaar! Spice smells, loud voices, colorful fabrics. An old man waves: \"Come, son, time is short.\" What happens? Continue writing!",
            PromptTr = "Yeni Berlin-Kreuzberg dairenizde, dün yokken bugün var olan bir kapı var. Sadece öyle: ahşap, demir kozalaklıklu, kilitsiz. Koluna dokunursun, aşağı indirirsin. Arkasında bodrum koridoru değil – bir pazar var! Baharat kokusu, yüksek sesler, renkli kumaşlar. Yaşlı bir adam selamlıyor: \"Gel oğlum, zaman dar.\" Ne olur? Devamını yaz!"
        },
        new()
        {
            Title = "Der sprechende Roboter",
            PromptDe = "Dein neuer Roboter-Freund \"BIP-7\" sollte nur Mathe-Hausaufgaben helfen. Aber heute beim Frühstück fängt er an zu reden – erst Türkisch: \"Günaydın, nasılsın?\" Dann Englisch: \"Good morning, I have a mission for you.\" Dann Deutsch: \"Guten Morgen. Ich brauche deine Hilfe. In 5 Minuten landet ein Paket auf dem Balkon. Öffne es nicht, bevor ich da bin.\" Was ist in dem Paket? Schreibe weiter!",
            PromptEn = "Your new robot friend \"BIP-7\" was supposed to only help with math homework. But today at breakfast he starts talking – first Turkish: \"Günaydın, nasılsın?\" Then English: \"Good morning, I have a mission for you.\" Then German: \"Guten Morgen. Ich brauche deine Hilfe. In 5 Minuten landet ein Paket auf dem Balkon. Öffne es nicht, bevor ich da bin.\" What's in the package? Continue writing!",
            PromptTr = "Yeni robot arkadaşın \"BIP-7\" sadece matematik ödevinde yardım etmeliydi. Ama bugün kahvaltıda konuşmaya başladı – önce Türkçe: \"Günaydın, nasılsın?\" Sonra İngilizce: \"Good morning, I have a mission for you.\" Sonra Almanca: \"Guten Morgen. Ich brauche deine Hilfe. In 5 Minuten landet ein Paket auf dem Balkon. Öffne es nicht, bevor ich da bin.\" Pakette ne var? Devamını yaz!"
        },
        new()
        {
            Title = "Die Zeitmaschine im Dachboden",
            PromptDe = "Hinter den Weihnachtskisten im Dachboden steht eine alte Standuhr – aber sie zeigt keine Zeit. Statt Ziffern hat sie Orte: \"Berlin 1989\", \"İstanbul 1453\", \"Mondlandung 1969\", \"Dein 18. Geburtstag\". Der Zeiger bewegt sich von allein auf \"Berlin 1989\". Du hörst Menschen jubeln, Mauern brechen. Du wirst hineingezogen. Was erlebst du? Schreibe weiter!",
            PromptEn = "Behind the Christmas boxes in the attic stands an old grandfather clock – but it shows no time. Instead of numbers it has places: \"Berlin 1989\", \"Istanbul 1453\", \"Moon landing 1969\", \"Your 18th birthday\". The hand moves on its own to \"Berlin 1989\". You hear people cheering, walls breaking. You're pulled in. What do you experience? Continue writing!",
            PromptTr = "Çatı katındaki Noel kutularının arkasında eski bir saat duruyor – ama saat göstermiyor. Rakamlar yerine yerler var: \"Berlin 1989\", \"İstanbul 1453\", \"Ay inişi 1969\", \"18. doğum günün\". İbre kendi kendine \"Berlin 1989\"a gidiyor. İnsanların haykırmasını, duvarların yıkılmasını duyuyorsun. İçine çekiliyorsun. Ne yaşıyorsun? Devamını yaz!"
        },
        new()
        {
            Title = "Der unsichtbare Passagier",
            PromptDe = "Du fährst mit der U-Bahn U8 durch Berlin, spät abends. Fast leer. Gegenüber setzt sich jemand – aber du siehst niemanden. Nur der Sitz drückt sich ein. Eine Stimme flüstert auf Türkisch: \"Sakın bakma, sadece dinle. Benim adım Leyla. Ben 1993 kayboldum. Bana yardım et.\" Der Zug hält am Moritzplatz. Die Türen gehen auf. Was tust du? Schreibe weiter!",
            PromptEn = "You're on the U8 in Berlin, late evening. Almost empty. Someone sits opposite – but you see no one. Only the seat cushion depresses. A voice whispers in Turkish: \"Don't look, just listen. My name is Leyla. I disappeared in 1993. Help me.\" The train stops at Moritzplatz. Doors open. What do you do? Continue writing!",
            PromptTr = "Berlin'da U8 ile gidiyorsun, geç saat. Neredeyse boş. Karşına biri oturuyor – ama kimseyi görmüyorsun. Sadece koltuk çöküyor. Bir ses Türkçe fıslıyor: \"Bakma, sadece dinle. Benim adım Leyla. 1993'te kayboldum. Bana yardım et.\" Tren Moritzplatz'ta duruyor. Kapılar açılıyor. Ne yaparsın? Devamını yaz!"
        },
        new()
        {
            Title = "Das Spiel, das echt wird",
            PromptDe = "Du zockst ein neues Indie-Game: \"Berlin Nights\". Dein Charakter läuft über die Oberbaumbrücke, es regnet. Plötzlich friert das Bild – aber die Regentropfen auf dem Bildschirm fühlen sich nass an. Dein Controller vibriert nicht, er wird warm. Ein Text erscheint: \"Spieler erkannt. Willkommen in der echten Version. Level 1: Rette deine Schwester vor dem Schattenmann.\" Dein Herz klopft. Schreibe weiter!",
            PromptEn = "You're playing a new indie game: \"Berlin Nights\". Your character walks across the Oberbaum Bridge, rain falling. Suddenly the screen freezes – but the raindrops on the screen feel wet. Your controller doesn't vibrate, it gets warm. Text appears: \"Player detected. Welcome to the real version. Level 1: Save your sister from the Shadow Man.\" Your heart pounds. Continue writing!",
            PromptTr = "Yeni bir indie oyun oynuyorsun: \"Berlin Nights\". Karakterin Oberbaum Köprüsü'nden geçiyor, yağmur yağıyor. Aniden ekran donuyor – ama ekrandaki yağmur damlaları ışınlanıyor. Kontrolcün titremiyor, ısıyor. Bir yazı çıkıyor: \"Oyuncu tanındı. Gerçek versiyona hoş geldin. Seviye 1: Kardeşini Gölge Adam'dan kurtar.\" Kalbin çarpıyor. Devamını yaz!"
        },
        new()
        {
            Title = "Der letzte Brief",
            PromptDe = "Du räumst das Zimmer deiner großen Schwester auf, die zum Studieren weggezogen ist. Unter dem Bett: ein versiegelter Briefumschlag, Adresse: \"Für den Mutigen, der dies findet.\" Du öffnest ihn. Drin: ein Zugticket Berlin → Istanbul (morgen!), ein Hotelschlüssel mit der Nummer 7, und ein Zettel: \"Trau niemandem. Nicht mal mir. Zerstere diesen Brief nach dem Lesen.\" Was tust du? Schreibe weiter!",
            PromptEn = "You're cleaning your big sister's room, she moved away for uni. Under the bed: a sealed envelope, address: \"For the brave one who finds this.\" You open it. Inside: a train ticket Berlin → Istanbul (tomorrow!), a hotel key numbered 7, and a note: \"Trust no one. Not even me. Destroy this letter after reading.\" What do you do? Continue writing!",
            PromptTr = "Üniversite için giden ablanın odasını topluyorsun. Yatağın altında: mühürlü bir mektup zarfı, adres: \"Bunu bulan cesur için.\" Açarsın. İçinde: Berlin → İstanbul tren bileti (yarın!), 7 numaralı oda anahtarı, ve bir not: \"Kimseye güvenme. Bana bile güvenme. Bu mektubu okuduktan sonra yok et.\" Ne yaparsın? Devamını yaz!"
        },
        new()
        {
            Title = "Das Spiegelbild, das nicht du bist",
            PromptDe = "Morgens vor dem Spiegel: du putzt Zähne, gähnst. Dein Spiegelbild gähnt nicht mit. Es lächelt. Und flüstert auf Englisch: \"Finally awake? Good. We have work to do. The portal in the park opens at midnight. Bring the red stone.\" Du drehst dich um – dein Zimmer ist leer. Du drehst dich zurück – das Spiegelbild zwinkert. Was für ein Stein? Welcher Park? Schreibe weiter!",
            PromptEn = "Morning at the mirror: brushing teeth, yawning. Your reflection doesn't yawn. It smiles. And whispers in English: \"Finally awake? Good. We have work to do. The portal in the park opens at midnight. Bring the red stone.\" You turn around – your room is empty. You turn back – the reflection winks. What stone? Which park? Continue writing!",
            PromptTr = "Sabah aynanın önünde: dişlerini fırçalıyorsun, esnüyorsun. Ayna yansıman esnemiyor. Gülümsüyor. Ve İngilizce fıslıyor: \"Finally awake? Good. We have work to do. The portal in the park opens at midnight. Bring the red stone.\" Dönersin – odan boş. Tekrar dönersin – ayna yansıman göz kırpar. Hangi taş? Hangi park? Devamını yaz!"
        },
        new()
        {
            Title = "Die Katze, die Türkisch konnte",
            PromptDe = "Die Streuner-Katze, die du \"Mavi\" nennst, sitzt jeden Abend auf deiner Fensterbank. Heute springt sie rein, legt etwas vor deine Füße: ein zusammengefaltetes Papier. Du entfaltest es. Kindliche Handschrift, Türkisch: \"Kardeşim, anne hastaneye gitti, babam da işte. Yanımda kimse yok, lütfen gel.\" Kein Name. Keine Adresse. Nur der Stempel einer Apotheke in Ünye. Was tust du? Schreibe weiter!",
            PromptEn = "The stray cat you call \"Mavi\" sits on your windowsill every evening. Today she jumps in, drops something at your feet: a folded paper. Childish handwriting, Turkish: \"Kardeşim, anne hastaneye gitti, babam da işte. Yanımda kimse yok, lütfen gel.\" No name. No address. Only a pharmacy stamp from Ünye. What do you do? Continue writing!",
            PromptTr = "Her akşap pencerende oturan, \"Mavi\" diye adladığın sokak kedisi. Bugün içeri atlıyor, ayaklarının üzerine bir kağıt bırakıyor. Açarsın. Çocuk el yazısı, Türkçe: \"Kardeşim, anne hastaneye gitti, babam da işte. Yanımda kimse yok, lütfen gel.\" İsim yok. Adres yok. Sadece Ünye'deki bir eczane damgası. Ne yaparsın? Devamını yaz!"
        }
    };

    public static WritingPrompt GetForDate(DateOnly date)
    {
        var index = date.DayOfYear % Pool.Count;
        return Pool[index];
    }
}