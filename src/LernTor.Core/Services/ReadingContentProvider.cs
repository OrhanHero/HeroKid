using LernTor.Core.Models;

namespace LernTor.Core.Services;

/// <summary>
/// Fester Pool an Gedichten/wichtigen Werken (gemeinfrei) für den täglichen Pflicht-Leseabschnitt -
/// bewusst ein Mix aus Deutsch, Türkisch, Englisch und Allgemeinwissen. Rotiert nach Tag im Jahr,
/// damit an einem Tag alle Aufrufe dasselbe Stück zeigen (egal wie oft die App neu gestartet wird),
/// am nächsten Tag aber ein neues dran ist.
/// </summary>
public static class ReadingContentProvider
{
    private static readonly IReadOnlyList<ReadingPiece> Pool = new List<ReadingPiece>
    {
        new()
        {
            Title = "Wandrers Nachtlied",
            Author = "Johann Wolfgang von Goethe",
            Language = "Deutsch",
            Text = "Über allen Gipfeln\n" +
                   "Ist Ruh,\n" +
                   "In allen Wipfeln\n" +
                   "Spürest du\n" +
                   "Kaum einen Hauch;\n" +
                   "Die Vögelein schweigen im Walde.\n" +
                   "Warte nur, balde\n" +
                   "Ruhest du auch."
        },
        new()
        {
            Title = "İstiklal Marşı (ilk iki kıta)",
            Author = "Mehmet Akif Ersoy",
            Language = "Türkçe",
            Text = "Korkma, sönmez bu şafaklarda yüzen al sancak;\n" +
                   "Sönmeden yurdumun üstünde tüten en son ocak.\n" +
                   "O benim milletimin yıldızıdır, parlayacak;\n" +
                   "O benimdir, o benim milletimindir ancak.\n\n" +
                   "Çatma, kurban olayım, çehreni ey nazlı hilal!\n" +
                   "Kahraman ırkıma bir gül! Ne bu şiddet, bu celal?\n" +
                   "Sana olmaz dökülen kanlarımız sonra helal…\n" +
                   "Hakkıdır, Hakk'a tapan, milletimin istiklal!"
        },
        new()
        {
            Title = "The Road Not Taken",
            Author = "Robert Frost",
            Language = "English",
            Text = "Two roads diverged in a yellow wood,\n" +
                   "And sorry I could not travel both\n" +
                   "And be one traveler, long I stood\n" +
                   "And looked down one as far as I could\n" +
                   "To where it bent in the undergrowth;\n\n" +
                   "Then took the other, as just as fair,\n" +
                   "And having perhaps the better claim,\n" +
                   "Because it was grassy and wanted wear;\n" +
                   "Though as for that the passing there\n" +
                   "Had worn them really about the same,\n\n" +
                   "And both that morning equally lay\n" +
                   "In leaves no step had trodden black.\n" +
                   "Oh, I kept the first for another day!\n" +
                   "Yet knowing how way leads on to way,\n" +
                   "I doubted if I should ever come back."
        },
        new()
        {
            Title = "Grundgesetz für die Bundesrepublik Deutschland, Artikel 1",
            Author = "Deutscher Bundestag",
            Language = "Allgemeinwissen",
            Text = "(1) Die Würde des Menschen ist unantastbar. Sie zu achten und zu schützen ist Verpflichtung " +
                   "aller staatlichen Gewalt.\n\n" +
                   "(2) Das Deutsche Volk bekennt sich darum zu unverletzlichen und unveräußerlichen " +
                   "Menschenrechten als Grundlage jeder menschlichen Gemeinschaft, des Friedens und der " +
                   "Gerechtigkeit in der Welt.\n\n" +
                   "(3) Die nachfolgenden Grundrechte binden Gesetzgebung, vollziehende Gewalt und " +
                   "Rechtsprechung als unmittelbar geltendes Recht."
        },
        new()
        {
            Title = "Die Lorelei",
            Author = "Heinrich Heine",
            Language = "Deutsch",
            Text = "Ich weiß nicht, was soll es bedeuten,\n" +
                   "Dass ich so traurig bin;\n" +
                   "Ein Märchen aus alten Zeiten,\n" +
                   "Das kommt mir nicht aus dem Sinn.\n\n" +
                   "Die Luft ist kühl und es dunkelt,\n" +
                   "Und ruhig fließt der Rhein;\n" +
                   "Der Gipfel des Berges funkelt\n" +
                   "Im Abendsonnenschein.\n\n" +
                   "Die schönste Jungfrau sitzet\n" +
                   "Dort oben wunderbar,\n" +
                   "Ihr goldnes Geschmeide blitzet,\n" +
                   "Sie kämmt ihr goldenes Haar."
        },
        new()
        {
            Title = "Gelin Tanış Olalım",
            Author = "Yunus Emre",
            Language = "Türkçe",
            Text = "Gelin tanış olalım,\n" +
                   "İşi kolay kılalım,\n" +
                   "Sevelim sevilelim,\n" +
                   "Dünya kimseye kalmaz.\n\n" +
                   "Ne var bu dünyanın işi,\n" +
                   "Ne şirin imiş bir bakışı,\n" +
                   "Anlar mı bilmez bunu,\n" +
                   "Meğer sohbetsiz kişi."
        },
        new()
        {
            Title = "The Tyger",
            Author = "William Blake",
            Language = "English",
            Text = "Tyger Tyger, burning bright,\n" +
                   "In the forests of the night;\n" +
                   "What immortal hand or eye,\n" +
                   "Could frame thy fearful symmetry?\n\n" +
                   "In what distant deeps or skies,\n" +
                   "Burnt the fire of thine eyes?\n" +
                   "On what wings dare he aspire?\n" +
                   "What the hand, dare seize the fire?"
        },
        new()
        {
            Title = "Allgemeines Menschenrecht auf Bildung",
            Author = "Vereinte Nationen (nach der Allgemeinen Erklärung der Menschenrechte, Artikel 26)",
            Language = "Allgemeinwissen",
            Text = "Jeder Mensch hat das Recht auf Bildung. Die Bildung ist wenigstens in den " +
                   "Grundschulstufen und den grundlegenden Bildungsstufen unentgeltlich. Der Grundschulunterricht " +
                   "ist obligatorisch.\n\n" +
                   "Die Bildung muss auf die volle Entfaltung der menschlichen Persönlichkeit und auf die " +
                   "Stärkung der Achtung vor den Menschenrechten und Grundfreiheiten gerichtet sein. Sie muss " +
                   "zu Verständnis, Toleranz und Freundschaft zwischen allen Nationen sowie rassischen und " +
                   "religiösen Gruppen beitragen."
        },
        new()
        {
            Title = "Erlkönig (Auszug)",
            Author = "Johann Wolfgang von Goethe",
            Language = "Deutsch",
            Text = "Wer reitet so spät durch Nacht und Wind?\n" +
                   "Es ist der Vater mit seinem Kind;\n" +
                   "Er hat den Knaben wohl in dem Arm,\n" +
                   "Er fasst ihn sicher, er hält ihn warm.\n\n" +
                   "Mein Sohn, was birgst du so bang dein Gesicht? -\n" +
                   "Siehst, Vater, du den Erlkönig nicht?\n" +
                   "Den Erlenkönig mit Kron und Schweif? -\n" +
                   "Mein Sohn, es ist ein Nebelstreif. -"
        },
        new()
        {
            Title = "Hope is the thing with feathers",
            Author = "Emily Dickinson",
            Language = "English",
            Text = "\"Hope\" is the thing with feathers -\n" +
                   "That perches in the soul -\n" +
                   "And sings the tune without the words -\n" +
                   "And never stops - at all -\n\n" +
                   "And sweetest - in the Gale - is heard -\n" +
                   "And sore must be the storm -\n" +
                   "That could abash the little Bird\n" +
                   "That kept so many warm -"
        },
        new()
        {
            Title = "Risale (Öğüt)",
            Author = "Yunus Emre",
            Language = "Türkçe",
            Text = "İlim ilim bilmektir,\n" +
                   "İlim kendin bilmektir,\n" +
                   "Sen kendini bilmezsen,\n" +
                   "Ya nice okumaktır?\n\n" +
                   "Okumaktan mana ne,\n" +
                   "Kişi Hakk'ı bilmektir,\n" +
                   "Çün okudun bilmezsin,\n" +
                   "Ha bir kuru emektir."
        },
        new()
        {
            Title = "Berlin – kurz erklärt",
            Author = "Allgemeinwissen",
            Language = "Allgemeinwissen",
            Text = "Berlin ist die Hauptstadt der Bundesrepublik Deutschland und zugleich eines der 16 " +
                   "Bundesländer. Die Stadt ist in zwölf Bezirke aufgeteilt und hat rund 3,8 Millionen " +
                   "Einwohnerinnen und Einwohner, damit ist sie die bevölkerungsreichste Stadt Deutschlands.\n\n" +
                   "Berlin war von 1961 bis 1989 durch die Berliner Mauer geteilt, die Ost- und West-Berlin " +
                   "trennte. Seit dem Mauerfall am 9. November 1989 und der Wiedervereinigung 1990 ist Berlin " +
                   "wieder eine ungeteilte Stadt und seit 1999 wieder Regierungssitz."
        },
    };

    public static ReadingPiece GetForDate(DateOnly date)
    {
        var index = date.DayOfYear % Pool.Count;
        return Pool[index];
    }
}
