using LernTor.Core.Enums;

namespace LernTor.App.Localization;

public static class Translations
{
    public static readonly IReadOnlyDictionary<string, IReadOnlyDictionary<AppLanguage, string>> Map =
        new Dictionary<string, IReadOnlyDictionary<AppLanguage, string>>
        {
            ["App_Title"] = L("LernTor", "LernTor"),

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

            ["Welcome_Greeting"] = L("Hallo, {0}!", "Merhaba, {0}!"),
            ["Welcome_Title"] = L("Willkommen bei LernTor!", "LernTor'a Hoş Geldin!"),
            ["Welcome_Subtitle"] = L(
                "Bevor du den PC benutzen kannst, lernen wir gemeinsam ein bisschen. Los geht's!",
                "Bilgisayarı kullanmadan önce biraz birlikte öğrenelim. Hadi başlayalım!"),
            ["Welcome_StartButton"] = L("Los geht's! 🚀", "Hadi başlayalım! 🚀"),
            ["Language_Switch"] = L("Türkçe", "Deutsch"),

            ["Stage_News"] = L("Nachrichten", "Haberler"),
            ["Stage_Mathematik"] = L("Mathematik", "Matematik"),
            ["Stage_Deutsch"] = L("Deutsch", "Almanca"),
            ["Stage_Tuerkisch"] = L("Türkisch", "Türkçe"),
            ["Stage_Naturwissenschaften"] = L("Naturwissenschaften", "Fen Bilimleri"),
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
            ["Parent_SetupPasswordPrompt"] = L(
                "Noch kein Passwort gesetzt. Bitte lege ein Admin-Passwort fest:", "Henüz şifre yok. Lütfen bir yönetici şifresi belirle:"),
            ["Parent_WrongPassword"] = L("Falsches Passwort.", "Yanlış şifre."),
            ["Parent_DisabledSubjects"] = L("Bereiche deaktivieren:", "Bölümleri devre dışı bırak:"),
            ["Parent_TimeLimit"] = L("Tägliches Zeitlimit (Minuten, 0 = kein Limit):", "Günlük süre sınırı (dakika, 0 = sınırsız):"),
            ["Parent_ActivityLog"] = L("Letzte Aktivitäten:", "Son etkinlikler:"),
            ["Parent_ActivityLogFor"] = L("Aktivitäten anzeigen für:", "Etkinlikleri göster:"),
            ["Parent_SkipUnlock"] = L("Sofort freischalten (überspringen)", "Hemen kilidi aç (atla)"),
            ["Parent_Save"] = L("Speichern", "Kaydet"),
            ["Parent_Close"] = L("Schließen", "Kapat"),
            ["Common_Back"] = L("Zurück", "Geri"),
        };

    private static IReadOnlyDictionary<AppLanguage, string> L(string de, string tr) =>
        new Dictionary<AppLanguage, string> { [AppLanguage.Deutsch] = de, [AppLanguage.Tuerkisch] = tr };
}
