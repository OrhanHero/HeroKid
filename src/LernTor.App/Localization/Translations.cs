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
            ["Reading_TabAll"] = L("Alle Sprachen", "Tüm diller"),
            ["Reading_ReadAloud"] = L("🔊 Vorlesen lassen", "🔊 Sesli oku"),
            ["Reading_StopReading"] = L("⏹ Stopp", "⏹ Durdur"),

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
            ["News_Cat_Berlin"] = L("Berlin", "Berlin"),
            ["News_Cat_Deutschland"] = L("Deutschland", "Almanya"),
            ["News_Cat_Welt"] = L("Welt", "Dünya"),
            ["News_Cat_Tuerkei"] = L("Türkei", "Türkiye"),
            ["News_Cat_Ki"] = L("KI & Technik", "Yapay zeka ve teknoloji"),
            ["News_Cat_Spiele"] = L("Spiele", "Oyunlar"),
            ["News_Cat_Finanzen"] = L("Finanzen", "Finans"),
            ["News_Cat_Wetter"] = L("Wetter", "Hava durumu"),
            ["News_Diff_Leicht"] = L("🟢 Leicht", "🟢 Kolay"),
            ["News_Diff_Mittel"] = L("🟡 Mittel", "🟡 Orta"),
            ["News_Diff_Schwer"] = L("🔴 Schwer", "🔴 Zor"),
            ["News_ReadingMinutes"] = L("🕒 ca. {0} Min.", "🕒 yakl. {0} dk"),
            ["News_WhyImportant"] = L("❓ Warum ist das wichtig?", "❓ Bu neden önemli?"),
            ["News_MeaningForKids"] = L("🧒 Was bedeutet das für dich?", "🧒 Bu senin için ne anlama geliyor?"),
            ["News_ExplainedTerms"] = L("💡 Schwierige Wörter, einfach erklärt", "💡 Zor kelimeler, basitçe açıklandı"),
            ["News_WeatherTitle"] = L("Wetter heute in Berlin", "Bugün Berlin'de hava"),
            ["News_SaveArticle"] = L("🔖 Merken", "🔖 Kaydet"),
            ["News_SavedState"] = L("✓ Gemerkt", "✓ Kaydedildi"),
            ["News_SavedArticles"] = L("🔖 Gemerkte Artikel", "🔖 Kaydedilen haberler"),
            ["News_SearchHint"] = L(
                "In den heutigen Artikeln suchen (mindestens 2 Buchstaben) - Treffer anklicken springt zum Artikel.",
                "Bugünkü haberlerde ara (en az 2 harf) - sonuca tıklamak habere götürür."),
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
                "Die KI überlegt… (die allererste Frage dauert am längsten: das Modell wird einmalig " +
                "heruntergeladen bzw. nach jedem App-Start neu in den Arbeitsspeicher geladen)",
                "Yapay zeka düşünüyor… (ilk soru en uzun sürer: model bir kez indirilir ve her " +
                "uygulama başlangıcında belleğe yeniden yüklenir)"),
            ["Exercise_SendChat"] = L("Senden", "Gönder"),
            ["Exercise_MinTimeCountdown"] = L(
                "⏳ Nimm dir Zeit zum Lesen – weiter in {0} s",
                "⏳ Okumak için zaman ayır – {0} sn sonra devam"),
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
            ["Parent_Import_ModelChoice"] = L("KI-Modell (wird beim ersten Gebrauch automatisch heruntergeladen):", "Yapay zeka modeli (ilk kullanımda otomatik indirilir):"),
            ["Parent_Import_LocalModelPath"] = L("Eigene Modelldatei verwenden (optional, .gguf - hat Vorrang vor der Auswahl oben):", "Kendi model dosyanı kullan (isteğe bağlı, .gguf - yukarıdaki seçimden önceliklidir):"),
            ["Parent_Import_PickModelFile"] = L("Modelldatei auswählen…", "Model dosyası seç…"),
            ["Parent_Import_PickFile"] = L("Datei auswählen…", "Dosya seç…"),
            ["Parent_Import_NoFile"] = L("Keine eigene Datei - das oben gewählte Modell wird verwendet.", "Kendi dosya yok - yukarıda seçilen model kullanılacak."),
            ["Parent_Import_Run"] = L("Einlesen starten", "İçe aktarmayı başlat"),
            ["Parent_Import_Running"] = L("Wird eingelesen…", "İçe aktarılıyor…"),
            ["Parent_Import_ResultsTitle"] = L("Vorschläge zur Prüfung:", "Kontrol için öneriler:"),
            ["Parent_Import_Accept"] = L("Übernehmen", "Kabul et"),
            ["Parent_Import_Discard"] = L("Verwerfen", "Reddet"),
            ["Parent_Import_SourceExcerpt"] = L("Quelle im Dokument:", "Belgedeki kaynak:"),
            ["Parent_Import_FieldTopic"] = L("Thema:", "Konu:"),
            ["Parent_Import_FieldPrompt"] = L("Frage/Aufgabenstellung:", "Soru/görev:"),
            ["Parent_Import_FieldOptions"] = L("Antwortoptionen (mit Komma getrennt):", "Cevap seçenekleri (virgülle ayrılmış):"),
            ["Parent_Import_FieldCorrectAnswers"] = L("Richtige Antwort(en) (mit Komma getrennt):", "Doğru cevap(lar) (virgülle ayrılmış):"),
            ["Parent_Import_FieldExplanation"] = L("Erklärung:", "Açıklama:"),
            ["Parent_Import_FieldHelpHint"] = L("Tipp (optional):", "İpucu (isteğe bağlı):"),
            ["Parent_Import_ErrorPromptMissing"] = L("Bitte eine Frage/Aufgabenstellung eingeben.", "Lütfen bir soru/görev gir."),
            ["Parent_Import_ErrorAnswerMissing"] = L("Bitte mindestens eine richtige Antwort eingeben.", "Lütfen en az bir doğru cevap gir."),
            ["Parent_Import_ErrorOptionsMissing"] = L("Bitte mindestens zwei Antwortoptionen eingeben (mit Komma getrennt).", "Lütfen en az iki cevap seçeneği gir (virgülle ayrılmış)."),
            ["Parent_Import_ErrorAnswerNotInOptions"] = L(
                "Die richtige Antwort muss wörtlich eine der Antwortoptionen sein.",
                "Doğru cevap, cevap seçeneklerinden biriyle birebir aynı olmalı."),

            ["Parent_DeleteProfile"] = L("🗑 Profil löschen", "🗑 Profili sil"),

            ["Parent_Rewards_Title"] = L("🎁 Belohnungen (Sterne einlösen)", "🎁 Ödüller (yıldızlarla al)"),
            ["Parent_Rewards_Intro"] = L(
                "Lege fest, was die Kinder mit ihren gesammelten Sternen einlösen können - z.B. " +
                "\"30 Minuten extra Spielzeit\" für 20 ⭐. Die Belohnungen erscheinen nach dem " +
                "bestandenen Abschlussquiz; eingelöste Belohnungen werden hier je Profil angezeigt " +
                "und von euch in der echten Welt eingelöst.",
                "Çocukların topladıkları yıldızlarla neler alabileceğini belirle - örneğin 20 ⭐ " +
                "karşılığında \"30 dakika ekstra oyun süresi\". Ödüller final sınavı geçildikten " +
                "sonra görünür; kullanılan ödüller burada profil bazında listelenir ve gerçek " +
                "hayatta sizin tarafınızdan verilir."),
            ["Parent_Rewards_EmojiHint"] = L("Emoji (optional, Standard 🎁)", "Emoji (isteğe bağlı, varsayılan 🎁)"),
            ["Parent_Rewards_TitleHint"] = L("Belohnung, z.B. \"30 Minuten extra Spielzeit\"", "Ödül, örn. \"30 dakika ekstra oyun süresi\""),
            ["Parent_Rewards_CostHint"] = L("Sterne-Kosten, z.B. 20", "Yıldız bedeli, örn. 20"),
            ["Parent_Rewards_Add"] = L("Hinzufügen", "Ekle"),
            ["Parent_Rewards_Empty"] = L("Noch keine Belohnungen angelegt.", "Henüz ödül eklenmedi."),
            ["Parent_Rewards_RedemptionsTitle"] = L("Eingelöste Belohnungen (gewähltes Profil oben):", "Kullanılan ödüller (yukarıda seçilen profil):"),
            ["Parent_Rewards_ErrorTitleMissing"] = L("Bitte eine Belohnung eintragen.", "Lütfen bir ödül gir."),
            ["Parent_Rewards_ErrorCostInvalid"] = L("Bitte gültige Sterne-Kosten eingeben (mindestens 1).", "Lütfen geçerli bir yıldız bedeli gir (en az 1)."),

            ["Result_Rewards_Title"] = L("🎁 Deine Belohnungen", "🎁 Ödüllerin"),
            ["Result_Rewards_Redeem"] = L("Einlösen", "Kullan"),
            ["Result_Rewards_Confirm"] = L(
                "{0} für {1} ⭐ einlösen? Deine Eltern lösen die Belohnung dann ein.",
                "{0} ödülünü {1} ⭐ karşılığında kullanmak istiyor musun? Ödülü ailen verecek."),

            ["Parent_Log_Title"] = L("Fehlerprotokoll", "Hata günlüğü"),
            ["Parent_Log_Intro"] = L(
                "Technisches Protokoll für die Fehlersuche: hier steht z.B., welche News-Quelle " +
                "nicht erreichbar war oder warum ein Download fehlgeschlagen ist. Eine Datei pro Tag " +
                "unter %LOCALAPPDATA%\\LernTor\\logs, älter als 14 Tage wird automatisch gelöscht. " +
                "Komplett lokal - nichts davon verlässt diesen PC.",
                "Hata ayıklama için teknik günlük: örneğin hangi haber kaynağına ulaşılamadığı veya " +
                "bir indirmenin neden başarısız olduğu burada görünür. Günde bir dosya " +
                "(%LOCALAPPDATA%\\LernTor\\logs), 14 günden eskiler otomatik silinir. Tamamen yerel - " +
                "hiçbir şey bu bilgisayardan çıkmaz."),
            ["Parent_Log_OpenFolder"] = L("📂 Log-Ordner öffnen", "📂 Günlük klasörünü aç"),
            ["Parent_Log_Refresh"] = L("Aktualisieren", "Yenile"),
            ["Parent_Log_Empty"] = L(
                "Heute wurden keine Probleme protokolliert. 👍",
                "Bugün hiçbir sorun kaydedilmedi. 👍"),

            ["Parent_Tts_Title"] = L("Natürliche Vorlesestimmen", "Doğal sesli okuma sesleri"),
            ["Parent_Tts_Intro"] = L(
                "Der Lesen-Abschnitt kann mit natürlichen, komplett lokalen Stimmen vorlesen (Piper, " +
                "je eine Stimme für Deutsch, Türkisch und Englisch - auch die türkische Aussprache " +
                "stimmt, die bei Windows meist ganz fehlt). Einmaliger Download von ~230 MB, danach " +
                "ohne Internet nutzbar. Solange die Stimmen nicht installiert sind, liest die " +
                "eingebaute Windows-Stimme vor.",
                "Okuma bölümü doğal, tamamen yerel seslerle okuyabilir (Piper; Almanca, Türkçe ve " +
                "İngilizce için birer ses - Windows'ta çoğu zaman hiç bulunmayan Türkçe telaffuz da " +
                "doğru). Bir kez ~230 MB indirilir, sonra internetsiz çalışır. Sesler kurulu değilken " +
                "Windows'un yerleşik sesi okur."),
            ["Parent_Tts_Install"] = L("Stimmen herunterladen (~230 MB)", "Sesleri indir (~230 MB)"),
            ["Parent_Tts_Installed"] = L("✓ Natürliche Stimmen installiert", "✓ Doğal sesler kurulu"),

            ["Parent_Report_Title"] = L("Bericht: Stärken & Schwächen", "Rapor: Güçlü ve zayıf yönler"),
            ["Parent_Report_Days7"] = L("Letzte 7 Tage", "Son 7 gün"),
            ["Parent_Report_Days30"] = L("Letzte 30 Tage", "Son 30 gün"),
            ["Parent_Report_NoData"] = L(
                "Noch keine Daten im gewählten Zeitraum - der Bericht füllt sich, sobald gelernt wurde.",
                "Seçilen dönemde henüz veri yok - öğrenmeye başlanınca rapor dolacak."),
            ["Parent_Report_LearnedDays"] = L("📅 An {0} von {1} Tagen gelernt", "📅 {1} günün {0} gününde öğrenildi"),
            ["Parent_Report_QuizTrend"] = L("📈 Abschlussquiz-Verlauf: {0}", "📈 Final sınavı gelişimi: {0}"),
            ["Parent_Report_NoQuiz"] = L("📈 Noch kein Abschlussquiz im Zeitraum.", "📈 Bu dönemde henüz final sınavı yok."),

            ["Steps_Reading"] = L("Lesen", "Okuma"),
            ["Steps_News"] = L("News", "Haberler"),
            ["Steps_Subjects"] = L("Fächer", "Dersler"),
            ["Steps_Quiz"] = L("Quiz", "Sınav"),

            ["Result_StarsSummary"] = L(
                "⭐ Heute verdient: +{0} Sterne · Insgesamt: {1}",
                "⭐ Bugün kazanılan: +{0} yıldız · Toplam: {1}"),

            ["Common_Back"] = L("Zurück", "Geri"),
        };

    private static IReadOnlyDictionary<AppLanguage, string> L(string de, string tr) =>
        new Dictionary<AppLanguage, string> { [AppLanguage.Deutsch] = de, [AppLanguage.Tuerkisch] = tr };
}
