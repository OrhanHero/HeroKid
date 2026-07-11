using LernTor.Core.Enums;

namespace LernTor.App.Localization;

public static class Translations
{
    public static readonly IReadOnlyDictionary<string, IReadOnlyDictionary<AppLanguage, string>> Map =
        new Dictionary<string, IReadOnlyDictionary<AppLanguage, string>>
        {
            ["App_Title"] = L("LernTor", "LernTor"),
            ["Main_ActiveProfile"] = L("👤 {0}", "👤 {0}"),

            ["Profile_Title"] = L("Wer möchte heute lernen?", "Bugün kim öğrenmek istiyor?"),
            ["Profile_AgeAndClass"] = L("{0} Jahre, Klasse {1}", "{0} yaşında, {1}. sınıf"),
            ["Profile_NewProfile"] = L("Neues Profil ➕", "Yeni profil ➕"),
            ["Profile_NameLabel"] = L("Name:", "İsim:"),
            ["Profile_AgeLabel"] = L("Alter (optional):", "Yaş (isteğe bağlı):"),
            ["Profile_ClassLabel"] = L("Klasse (z.B. 7b, optional):", "Sınıf (örn. 7b, isteğe bağlı):"),
            ["Profile_GradeLevelLabel"] = L("Lehrplan-Stufe:", "Müfredat seviyesi:"),
            ["Profile_CreateButton"] = L("Profil erstellen", "Profil oluştur"),
            ["Profile_CancelButton"] = L("Abbrechen", "İptal"),
            ["Profile_NameRequired"] = L("Bitte einen Namen eingeben.", "Lütfen bir isim gir."),
            ["Profile_GreetingMorning"] = L("☀️ Guten Morgen!", "☀️ Günaydın!"),
            ["Profile_GreetingAfternoon"] = L("👋 Schön, dass du da bist!", "👋 Gelmene sevindik!"),
            ["Profile_GreetingEvening"] = L("🌙 Guten Abend!", "🌙 İyi akşamlar!"),
            ["Profile_AvatarLabel"] = L("Such dir deinen Avatar aus:", "Avatarını seç:"),
            ["Profile_DoneToday"] = L("Heute schon geschafft! 🎉", "Bugün tamamlandı! 🎉"),

            ["Welcome_Greeting"] = L("Hallo, {0}!", "Merhaba, {0}!"),
            ["Welcome_Title"] = L("Willkommen bei LernTor!", "LernTor'a Hoş Geldin!"),
            ["Welcome_Subtitle"] = L(
                "Bevor du den PC benutzen kannst, lernen wir gemeinsam ein bisschen. Los geht's!",
                "Bilgisayarı kullanmadan önce biraz birlikte öğrenelim. Hadi başlayalım!"),
            ["Welcome_StartButton"] = L("Los geht's! 🚀", "Hadi başlayalım! 🚀"),
            ["Language_Switch"] = L("Türkçe", "Deutsch"),

            ["Stage_Vorlesen"] = L("Lesen", "Okuma"),
            ["Reading_Title"] = L("Zeit zum lauten Vorlesen 📖", "Yüksek sesle okuma zamanı 📖"),
            ["Reading_Instruction"] = L(
                "Lies den folgenden Text mindestens 5 Minuten lang LAUT vor. Der Timer läuft weiter, auch wenn du fertig bist - lies ihn dann einfach noch einmal, übe die Aussprache oder sprich über den Inhalt.",
                "Aşağıdaki metni en az 5 dakika boyunca YÜKSEK SESLE oku. Bitirsen bile sayaç devam eder - o zaman tekrar oku, telaffuzunu çalış ya da içeriği konuş."),
            ["Reading_TimeRemainingHint"] = L(
                "Weiter geht's, sobald der Timer abgelaufen ist.",
                "Sayaç bitince devam edebilirsin."),
            ["Reading_Continue"] = L("Weiter ➜", "Devam ➜"),

            ["Stage_News"] = L("Nachrichten", "Haberler"),
            ["Stage_Mathematik"] = L("Mathematik", "Matematik"),
            ["Stage_Deutsch"] = L("Deutsch", "Almanca"),
            ["Stage_Tuerkisch"] = L("Türkisch", "Türkçe"),
            ["Stage_Englisch"] = L("Englisch", "İngilizce"),
            ["Stage_Biologie"] = L("Biologie", "Biyoloji"),
            ["Stage_Chemie"] = L("Chemie", "Kimya"),
            ["Stage_Physik"] = L("Physik", "Fizik"),
            ["Stage_Gewi"] = L("Gesellschaftswissenschaften", "Sosyal Bilgiler"),
            ["Stage_Politik"] = L("Politik", "Siyaset Bilgisi"),
            ["Stage_Geo"] = L("Geografie", "Coğrafya"),
            ["Stage_Ethik"] = L("Ethik", "Etik"),
            ["Stage_Itg"] = L("Medienbildung (ITG)", "Medya Bilgisi (ITG)"),
            ["Stage_Abschlussquiz"] = L("Abschlussquiz", "Bitiş Sınavı"),

            ["News_Loading"] = L("Nachrichten werden geladen…", "Haberler yükleniyor…"),
            ["News_LoadError"] = L(
                "Es konnten gerade keine Nachrichten geladen werden. Bitte prüfe die Internetverbindung.",
                "Şu anda haberler yüklenemedi. Lütfen internet bağlantını kontrol et."),
            ["News_ArticleProgress"] = L("Artikel {0} von {1}", "Haber {0} / {1}"),
            ["News_AnswerQuestions"] = L("Beantworte die Fragen zum Artikel:", "Haberle ilgili soruları cevapla:"),
            ["News_NextArticle"] = L("Nächster Artikel ➜", "Sonraki Haber ➜"),
            ["News_FinishSection"] = L("Weiter zu Mathematik ➜", "Matematiğe geç ➜"),

            ["Exercise_QuestionProgress"] = L("Aufgabe {0} von {1}", "Soru {0} / {1}"),
            ["Exercise_SubmitAnswer"] = L("Antwort prüfen", "Cevabı kontrol et"),
            ["Exercise_NextQuestion"] = L("Nächste Aufgabe ➜", "Sonraki soru ➜"),
            ["Exercise_Correct"] = L("Richtig! Super gemacht! 🎉", "Doğru! Harikasın! 🎉"),
            ["Exercise_Incorrect"] = L("Nicht ganz richtig, aber weiter geht's!", "Tam doğru değil ama devam!"),
            ["Exercise_Explanation"] = L("Erklärung:", "Açıklama:"),
            ["Exercise_ShowHint"] = L("💡 Tipp anzeigen", "💡 İpucu göster"),
            ["Exercise_AskAi"] = L("🤖 KI fragen", "🤖 Yapay zekaya sor"),
            ["Exercise_AiThinking"] = L(
                "Die KI überlegt… (beim allerersten Mal wird einmalig ein KI-Modell heruntergeladen, das kann etwas dauern)",
                "Yapay zeka düşünüyor… (ilk kullanımda bir yapay zeka modeli indirilir, bu biraz zaman alabilir)"),
            ["Exercise_SendChat"] = L("Senden", "Gönder"),
            ["Exercise_YourAnswerPlaceholder"] = L("Deine Antwort…", "Cevabın…"),
            ["Exercise_FinishSubject"] = L("Bereich abgeschlossen! Weiter ➜", "Bölüm tamamlandı! Devam ➜"),

            ["Quiz_Title"] = L("Abschlussquiz", "Bitiş Sınavı"),
            ["Quiz_Intro"] = L(
                "Jetzt kommt das große Abschlussquiz! Du brauchst mindestens 50% richtige Antworten, um den PC freizuschalten.",
                "Şimdi büyük bitiş sınavı geliyor! Bilgisayarın kilidini açmak için en az %50 doğru cevap gerekiyor."),
            ["Quiz_StartButton"] = L("Quiz starten", "Sınava başla"),
            ["Quiz_SubmitFinal"] = L("Quiz abgeben", "Sınavı gönder"),

            ["Result_PassedTitle"] = L("Geschafft! 🎉🏆", "Başardın! 🎉🏆"),
            ["Result_PassedText"] = L(
                "Du hast das Abschlussquiz bestanden! Der PC ist jetzt freigeschaltet. Viel Spaß!",
                "Bitiş sınavını geçtin! Bilgisayarın kilidi artık açık. İyi eğlenceler!"),
            ["Result_FailedTitle"] = L("Fast geschafft!", "Neredeyse başardın!"),
            ["Result_FailedText"] = L(
                "Das war noch nicht ganz genug. Lass uns die schwächeren Themen nochmal üben, dann klappt's beim nächsten Versuch!",
                "Bu sefer yeterli değildi. Zayıf konuları tekrar çalışalım, bir sonraki denemede başaracaksın!"),
            ["Result_Score"] = L("Dein Ergebnis: {0} von {1} richtig ({2:P0})", "Sonucun: {1} sorudan {0} doğru ({2:P0})"),
            ["Result_RetryButton"] = L("Schwache Themen wiederholen", "Zayıf konuları tekrar çalış"),
            ["Result_UnlockButton"] = L("PC jetzt benutzen 🎮", "Bilgisayarı kullan 🎮"),

            ["Parent_LoginTitle"] = L("Eltern-Bereich", "Ebeveyn Alanı"),
            ["Parent_PasswordPrompt"] = L("Admin-Passwort eingeben:", "Yönetici şifresini gir:"),
            ["Parent_Login"] = L("Anmelden", "Giriş yap"),
            ["Parent_UnlockAndExit"] = L("PC entsperren & beenden 🔓", "Bilgisayarın kilidini aç ve kapat 🔓"),
            ["Parent_SetupPasswordPrompt"] = L(
                "Noch kein Passwort gesetzt. Bitte lege ein Admin-Passwort fest:", "Henüz şifre yok. Lütfen bir yönetici şifresi belirle:"),
            ["Parent_WrongPassword"] = L("Falsches Passwort.", "Yanlış şifre."),
            ["Parent_DisabledSubjects"] = L("Bereiche deaktivieren:", "Bölümleri devre dışı bırak:"),
            ["Parent_ActivityLog"] = L("Letzte Aktivitäten:", "Son etkinlikler:"),
            ["Parent_ActivityLogFor"] = L("Aktivitäten anzeigen für:", "Etkinlikleri göster:"),
            ["Parent_SkipUnlock"] = L("Sofort freischalten (überspringen)", "Hemen kilidi aç (atla)"),
            ["Parent_Save"] = L("Speichern", "Kaydet"),
            ["Parent_Close"] = L("Schließen", "Kapat"),
            ["Parent_DangerZone"] = L("Gefahrenzone", "Tehlikeli bölge"),
            ["Parent_ResetAllData"] = L("Alle Daten zurücksetzen…", "Tüm verileri sıfırla…"),

            ["Parent_CustomQuestions_Title"] = L("Eigene Aufgaben (z.B. von der Lehrkraft)", "Kendi sorularım (örn. öğretmenden)"),
            ["Parent_CustomQuestions_Intro"] = L(
                "Ergänzt die generierten Aufgaben - z.B. aktuelle Hausaufgaben oder Themen, die die Lehrkraft gerade durchnimmt.",
                "Üretilen soruları tamamlar - örn. güncel ödevler veya öğretmenin şu an işlediği konular."),
            ["Parent_CustomQuestions_Subject"] = L("Fach:", "Ders:"),
            ["Parent_CustomQuestions_Grade"] = L("Klassenstufe:", "Sınıf seviyesi:"),
            ["Parent_CustomQuestions_Type"] = L("Fragetyp:", "Soru türü:"),
            ["Parent_CustomQuestions_Topic"] = L("Thema/Quelle (z.B. \"Hausaufgabe KW12\"):", "Konu/Kaynak (örn. \"12. hafta ödevi\"):"),
            ["Parent_CustomQuestions_Prompt"] = L("Frage/Aufgabenstellung:", "Soru:"),
            ["Parent_CustomQuestions_Options"] = L(
                "Antwortoptionen, mit Komma getrennt (nur bei Multiple Choice/Wahr-Falsch):",
                "Cevap seçenekleri, virgülle ayrılmış (sadece çoktan seçmeli/doğru-yanlış):"),
            ["Parent_CustomQuestions_CorrectAnswers"] = L(
                "Richtige Antwort(en), mit Komma getrennt:", "Doğru cevap(lar), virgülle ayrılmış:"),
            ["Parent_CustomQuestions_Explanation"] = L(
                "Erklärung (wird nach der Antwort gezeigt):", "Açıklama (cevaptan sonra gösterilir):"),
            ["Parent_CustomQuestions_HelpHint"] = L(
                "Tipp (optional, vor der Antwort abrufbar, verrät nicht die Lösung):",
                "İpucu (isteğe bağlı, cevaptan önce görülebilir, çözümü vermez):"),
            ["Parent_CustomQuestions_Add"] = L("Aufgabe hinzufügen", "Soru ekle"),
            ["Parent_CustomQuestions_ExistingTitle"] = L("Bereits angelegte eigene Aufgaben:", "Zaten eklenmiş sorular:"),
            ["Parent_CustomQuestions_Empty"] = L("Noch keine eigenen Aufgaben angelegt.", "Henüz kendi sorun yok."),
            ["Parent_CustomQuestions_Delete"] = L("Löschen", "Sil"),

            ["Parent_Import_Title"] = L("Automatisches Einlesen von Lehrer-Unterlagen & KI-Lernchat", "Öğretmen belgelerini otomatik içe aktarma ve yapay zeka öğrenme sohbeti"),
            ["Parent_Import_Intro"] = L(
                "Komplett lokal, ohne Cloud-Anbindung: ein KI-Modell läuft direkt auf diesem PC und wird " +
                "beim ersten Gebrauch automatisch heruntergeladen (kein manueller Download nötig). Es lädt " +
                "eine PDF-/Word-Datei hoch und schlägt daraus Fragen vor (jeder Vorschlag muss geprüft und " +
                "einzeln bestätigt werden) - und beantwortet im \"🤖 KI fragen\"-Button bei jeder Aufgabe " +
                "Rückfragen der Kinder, wie bei einem Taschenrechner: sie kennt die richtige Antwort, soll " +
                "sie aber nicht sofort verraten, sondern zuerst durch Denkanstöße helfen.",
                "Tamamen yerel, bulut bağlantısı olmadan: bir yapay zeka modeli doğrudan bu bilgisayarda " +
                "çalışır ve ilk kullanımda otomatik olarak indirilir (manuel indirme gerekmez). Bir PDF/Word " +
                "dosyası yükler ve bundan sorular önerir (her öneri kontrol edilip tek tek onaylanmalıdır) - " +
                "ve her görevde \"🤖 Yapay zekaya sor\" düğmesiyle çocukların sorularını yanıtlar, bir hesap " +
                "makinesi gibi: doğru cevabı bilir, ama hemen söylemek yerine önce ipuçlarıyla yardım eder."),
            ["Parent_Import_LocalModelPath"] = L("Eigene Modelldatei verwenden (optional, .gguf):", "Kendi model dosyanı kullan (isteğe bağlı, .gguf):"),
            ["Parent_Import_PickModelFile"] = L("Modelldatei auswählen…", "Model dosyası seç…"),
            ["Parent_Import_PickFile"] = L("Datei auswählen…", "Dosya seç…"),
            ["Parent_Import_NoFile"] = L("Kein eigenes Modell ausgewählt - Standardmodell wird automatisch heruntergeladen.", "Kendi model seçilmedi - varsayılan model otomatik indirilecek."),
            ["Parent_Import_Run"] = L("Einlesen starten", "İçe aktarmayı başlat"),
            ["Parent_Import_Running"] = L("Wird eingelesen…", "İçe aktarılıyor…"),
            ["Parent_Import_ResultsTitle"] = L("Vorschläge zur Prüfung:", "Kontrol için öneriler:"),
            ["Parent_Import_Accept"] = L("Übernehmen", "Kabul et"),
            ["Parent_Import_Discard"] = L("Verwerfen", "Reddet"),
            ["Parent_Import_SourceExcerpt"] = L("Quelle im Dokument:", "Belgedeki kaynak:"),

            ["Common_Back"] = L("Zurück", "Geri"),
        };

    private static IReadOnlyDictionary<AppLanguage, string> L(string de, string tr) =>
        new Dictionary<AppLanguage, string> { [AppLanguage.Deutsch] = de, [AppLanguage.Tuerkisch] = tr };
}
