using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Text.RegularExpressions;
using Google.Apis.Auth.OAuth2;
using LernTor.Core.Enums;

namespace LernTor.ContentGen.TeacherImport;

/// <summary>
/// <see cref="ITeacherQuestionSuggester"/>-Implementierung gegen die NotebookLM-Enterprise-API
/// (Google Cloud, Teil von Gemini Enterprise / Discovery Engine).
///
/// <para><b>Wichtiger Hinweis zum Implementierungsstand:</b> Diese Klasse wurde ohne Zugriff auf die
/// offizielle API-Dokumentation (docs.cloud.google.com/gemini/enterprise/notebooklm-enterprise/docs/api-notebooks)
/// geschrieben - der Netzwerkzugriff auf diesen Host war aus der Entwicklungsumgebung heraus durch die
/// Organisations-Policy blockiert (403, sowohl per curl als auch per WebFetch bestätigt). Die
/// Authentifizierung (Google-Dienstkonto/OAuth2 über <c>Google.Apis.Auth</c>) folgt etablierten,
/// stabilen Google-Cloud-Konventionen und sollte korrekt sein. Die konkreten Endpunkt-Pfade,
/// Feldnamen der Request-/Response-JSON-Objekte (<see cref="CreateNotebookAsync"/>,
/// <see cref="UploadSourceAsync"/>, <see cref="QueryNotebookAsync"/>) sind dagegen eine begründete
/// Best-Effort-Annahme nach dem Muster anderer Discovery-Engine-/Vertex-AI-APIs und MÜSSEN nach
/// dem ersten echten Testlauf mit echten Zugangsdaten anhand der offiziellen Dokumentation
/// verifiziert/korrigiert werden (siehe README, Abschnitt "Automatisches Einlesen von
/// Lehrer-Unterlagen").</para>
/// </summary>
public sealed class NotebookLmQuestionSuggester : ITeacherQuestionSuggester
{
    // ANNAHME (nicht verifiziert): Basis-URL für die Discovery-Engine-API, auf der NotebookLM
    // Enterprise laut Ankündigungen aufbaut. Bei Bedarf hier anpassen, sobald die echte Doku
    // vorliegt.
    private const string ApiBaseUrl = "https://discoveryengine.googleapis.com/v1alpha";
    private static readonly string[] OAuthScopes = { "https://www.googleapis.com/auth/cloud-platform" };

    private readonly HttpClient _httpClient;
    private readonly NotebookLmOptions _options;

    public NotebookLmQuestionSuggester(HttpClient httpClient, NotebookLmOptions options)
    {
        _httpClient = httpClient;
        _options = options;
    }

    public async Task<IReadOnlyList<ExtractedQuestionDraft>> SuggestQuestionsAsync(
        string documentText,
        Subject subject,
        GradeLevel gradeLevel,
        CancellationToken cancellationToken = default)
    {
        if (!_options.IsConfigured)
        {
            throw new NotSupportedException(
                "Automatisches Einlesen von Lehrer-Unterlagen ist vorbereitet, aber die NotebookLM-Anbindung " +
                "ist noch nicht konfiguriert (Projekt-ID und/oder Dienstkonto-Schlüsseldatei fehlen im " +
                "Eltern-Bereich unter 'Automatisches Einlesen'). Der manuelle Eigene-Aufgaben-Editor deckt " +
                "den Kernbedarf in der Zwischenzeit ab.");
        }

        var accessToken = await GetAccessTokenAsync(cancellationToken);
        var notebookName = await CreateNotebookAsync(accessToken, subject, gradeLevel, cancellationToken);

        try
        {
            await UploadSourceAsync(accessToken, notebookName, documentText, cancellationToken);
            var answerText = await QueryNotebookAsync(accessToken, notebookName, subject, gradeLevel, cancellationToken);
            return ParseDrafts(answerText, documentText);
        }
        finally
        {
            // Aufräumen: pro Import wird ein Wegwerf-Notebook angelegt, das danach nicht mehr
            // gebraucht wird. Fehler beim Löschen sollen den eigentlich schon erfolgreichen
            // Import nicht nachträglich als Fehlschlag erscheinen lassen.
            await TryDeleteNotebookAsync(accessToken, notebookName, cancellationToken);
        }
    }

    private async Task<string> GetAccessTokenAsync(CancellationToken cancellationToken)
    {
        var credential = GoogleCredential.FromFile(_options.ServiceAccountKeyPath!).CreateScoped(OAuthScopes);
        var token = await credential.GetAccessTokenForRequestAsync(cancellationToken: cancellationToken);

        if (string.IsNullOrEmpty(token))
        {
            throw new InvalidOperationException(
                "Konnte kein OAuth2-Zugriffstoken für das konfigurierte Google-Dienstkonto abrufen. " +
                "Bitte Projekt-ID und Schlüsseldatei im Eltern-Bereich prüfen.");
        }

        return token;
    }

    // ANNAHME (nicht verifiziert): POST .../notebooks legt ein neues Notebook an und liefert dessen
    // Ressourcennamen ("name": "projects/.../locations/.../notebooks/...") zurück.
    private async Task<string> CreateNotebookAsync(string accessToken, Subject subject, GradeLevel gradeLevel, CancellationToken cancellationToken)
    {
        var url = $"{ApiBaseUrl}/projects/{_options.ProjectId}/locations/{_options.Location}/notebooks";
        var request = new HttpRequestMessage(HttpMethod.Post, url)
        {
            Content = JsonContent.Create(new { title = $"LernTor Import – {subject} {gradeLevel}" })
        };
        request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

        using var response = await SendAsync(request, "Notebook anlegen", cancellationToken);
        var body = await response.Content.ReadFromJsonAsync<NotebookResourceDto>(cancellationToken: cancellationToken);

        return body?.Name
            ?? throw new InvalidOperationException("NotebookLM-Antwort beim Anlegen des Notebooks enthielt kein 'name'-Feld.");
    }

    // ANNAHME (nicht verifiziert): POST .../{notebook}/sources fügt eine Text-Quelle zum Notebook hinzu.
    private async Task UploadSourceAsync(string accessToken, string notebookName, string documentText, CancellationToken cancellationToken)
    {
        var url = $"{ApiBaseUrl}/{notebookName}/sources";
        var request = new HttpRequestMessage(HttpMethod.Post, url)
        {
            Content = JsonContent.Create(new { textContent = documentText })
        };
        request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

        using var _ = await SendAsync(request, "Dokument als Quelle hochladen", cancellationToken);
    }

    // ANNAHME (nicht verifiziert): POST .../{notebook}:query stellt eine Frage an das Notebook,
    // gestützt auf die zuvor hochgeladene(n) Quelle(n), und liefert die Modellantwort als Text zurück.
    private async Task<string> QueryNotebookAsync(string accessToken, string notebookName, Subject subject, GradeLevel gradeLevel, CancellationToken cancellationToken)
    {
        var prompt = BuildPrompt(subject, gradeLevel);
        var url = $"{ApiBaseUrl}/{notebookName}:query";
        var request = new HttpRequestMessage(HttpMethod.Post, url)
        {
            Content = JsonContent.Create(new { query = prompt })
        };
        request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

        using var response = await SendAsync(request, "Notebook abfragen", cancellationToken);
        var body = await response.Content.ReadFromJsonAsync<NotebookQueryResponseDto>(cancellationToken: cancellationToken);

        return body?.AnswerText
            ?? throw new InvalidOperationException("NotebookLM-Antwort auf die Abfrage enthielt keinen Antworttext.");
    }

    private async Task TryDeleteNotebookAsync(string accessToken, string notebookName, CancellationToken cancellationToken)
    {
        try
        {
            var url = $"{ApiBaseUrl}/{notebookName}";
            var request = new HttpRequestMessage(HttpMethod.Delete, url);
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
            using var response = await _httpClient.SendAsync(request, cancellationToken);
            // Bewusst kein Fehlerwurf hier - ein nicht gelöschtes Wegwerf-Notebook ist unschön,
            // aber kein Grund, den ansonsten erfolgreichen Import scheitern zu lassen.
        }
        catch (Exception)
        {
            // Aufräumfehler werden bewusst verschluckt (siehe Kommentar oben).
        }
    }

    private async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, string step, CancellationToken cancellationToken)
    {
        HttpResponseMessage response;
        try
        {
            response = await _httpClient.SendAsync(request, cancellationToken);
        }
        catch (HttpRequestException ex)
        {
            throw new InvalidOperationException(
                $"NotebookLM-Aufruf '{step}' ist netzwerkseitig fehlgeschlagen: {ex.Message}. " +
                "Prüfe Internetverbindung und ob die konfigurierte Projekt-ID/Region korrekt sind.", ex);
        }

        if (!response.IsSuccessStatusCode)
        {
            var errorBody = await response.Content.ReadAsStringAsync(cancellationToken);
            throw new InvalidOperationException(
                $"NotebookLM-Aufruf '{step}' schlug fehl: HTTP {(int)response.StatusCode} {response.ReasonPhrase}. " +
                $"Antwort: {errorBody}\n\n" +
                "Falls dies ein 404 oder eine Feldvalidierungsfehlermeldung ist, stimmt der angenommene " +
                "Endpunkt-Pfad/das Request-Format vermutlich nicht mit der echten API überein - bitte gegen " +
                "die offizielle NotebookLM-Enterprise-API-Dokumentation prüfen (siehe Klassenkommentar).");
        }

        return response;
    }

    private static string BuildPrompt(Subject subject, GradeLevel gradeLevel)
    {
        return
            $"Erstelle aus dem hochgeladenen Dokument bis zu 10 Quizfragen für ein Kind der {gradeLevel} " +
            $"im Fach {subject} auf Deutsch. Antworte AUSSCHLIESSLICH mit einem JSON-Objekt in exakt " +
            "folgendem Format, ohne weiteren Text davor oder danach:\n" +
            "{\"questions\":[{\"topic\":\"...\",\"prompt\":\"...\",\"type\":\"MultipleChoice|TrueFalse|OpenText\"," +
            "\"options\":[\"...\"],\"correctAnswers\":[\"...\"],\"explanation\":\"...\",\"helpHint\":\"...|null\"," +
            "\"sourceExcerpt\":\"...\"}]}\n" +
            "Bei OpenText-Fragen soll 'options' ein leeres Array sein. 'sourceExcerpt' soll die Textstelle " +
            "aus dem Dokument enthalten, auf der die Frage beruht.";
    }

    private static IReadOnlyList<ExtractedQuestionDraft> ParseDrafts(string answerText, string documentText)
    {
        var json = ExtractJsonObject(answerText);
        var parsed = JsonSerializer.Deserialize<NotebookLmAnswerDto>(json, JsonSerializerOptionsProvider.Options);

        if (parsed?.Questions is null || parsed.Questions.Count == 0)
        {
            return Array.Empty<ExtractedQuestionDraft>();
        }

        return parsed.Questions.Select(q => new ExtractedQuestionDraft
        {
            Topic = q.Topic ?? string.Empty,
            Prompt = q.Prompt ?? string.Empty,
            Type = ParseQuestionType(q.Type),
            Options = q.Options ?? new List<string>(),
            CorrectAnswers = q.CorrectAnswers ?? new List<string>(),
            Explanation = q.Explanation ?? string.Empty,
            HelpHint = q.HelpHint,
            SourceExcerpt = string.IsNullOrWhiteSpace(q.SourceExcerpt) ? Truncate(documentText, 200) : q.SourceExcerpt!
        }).ToList();
    }

    private static QuestionType ParseQuestionType(string? type) =>
        Enum.TryParse<QuestionType>(type, ignoreCase: true, out var parsed) ? parsed : QuestionType.OpenText;

    private static string Truncate(string text, int maxLength) =>
        text.Length <= maxLength ? text : text[..maxLength] + "…";

    /// <summary>
    /// LLM-Antworten enthalten das erwartete JSON-Objekt manchmal zusätzlich in Markdown-Codeblöcken
    /// (```json ... ```) oder mit erklärendem Text davor/danach - dieses Verfahren extrahiert robust
    /// das erste vollständige {...}-Objekt aus der Antwort.
    /// </summary>
    private static string ExtractJsonObject(string answerText)
    {
        var match = Regex.Match(answerText, @"\{[\s\S]*\}", RegexOptions.None, TimeSpan.FromSeconds(2));
        if (!match.Success)
        {
            throw new InvalidOperationException(
                "Konnte in der NotebookLM-Antwort kein JSON-Objekt finden. Rohantwort: " + Truncate(answerText, 500));
        }

        return match.Value;
    }

    private sealed class NotebookResourceDto
    {
        [JsonPropertyName("name")]
        public string? Name { get; set; }
    }

    private sealed class NotebookQueryResponseDto
    {
        // ANNAHME (nicht verifiziert): Feldname für den Antworttext des Modells.
        [JsonPropertyName("answerText")]
        public string? AnswerText { get; set; }
    }

    private sealed class NotebookLmAnswerDto
    {
        [JsonPropertyName("questions")]
        public List<NotebookLmQuestionDto>? Questions { get; set; }
    }

    private sealed class NotebookLmQuestionDto
    {
        [JsonPropertyName("topic")]
        public string? Topic { get; set; }

        [JsonPropertyName("prompt")]
        public string? Prompt { get; set; }

        [JsonPropertyName("type")]
        public string? Type { get; set; }

        [JsonPropertyName("options")]
        public List<string>? Options { get; set; }

        [JsonPropertyName("correctAnswers")]
        public List<string>? CorrectAnswers { get; set; }

        [JsonPropertyName("explanation")]
        public string? Explanation { get; set; }

        [JsonPropertyName("helpHint")]
        public string? HelpHint { get; set; }

        [JsonPropertyName("sourceExcerpt")]
        public string? SourceExcerpt { get; set; }
    }

    private static class JsonSerializerOptionsProvider
    {
        public static readonly JsonSerializerOptions Options = new()
        {
            PropertyNameCaseInsensitive = true
        };
    }
}
