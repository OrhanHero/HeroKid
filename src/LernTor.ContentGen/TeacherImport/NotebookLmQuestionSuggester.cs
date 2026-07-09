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
/// <para><b>Verifizierungsstand:</b> Ursprünglich ohne Doku-Zugriff geschrieben (docs.cloud.google.com
/// war aus der Entwicklungsumgebung blockiert). Der Nutzer hat die offizielle Dokumentation
/// anschließend als PDF bereitgestellt, wodurch folgende Teile jetzt gegen echte, zitierte
/// Doku-Beispiele verifiziert werden konnten:</para>
/// <list type="bullet">
/// <item>Basis-URL mit Regions-Präfix (<c>https://{location}-discoveryengine.googleapis.com/v1alpha</c>),
/// Authentifizierung per Bearer-Token (Google-Dienstkonto-OAuth2, keine weiteren Header nötig),
/// Ressourcen-Pfadschema (<c>projects/{project}/locations/{location}/notebooks/{id}</c>).</item>
/// <item><see cref="CreateNotebookAsync"/> (<c>notebooks.create</c>), <see cref="UploadSourceAsync"/>
/// (<c>notebooks.sources.batchCreate</c> mit <c>textContent</c>) und
/// <see cref="DeleteNotebookAsync"/> (<c>notebooks.batchDelete</c>, ein POST mit <c>names</c>-Array,
/// KEIN HTTP-DELETE) sind jetzt nach dokumentierten Beispielen implementiert.</item>
/// <item><b>Weiterhin unverifiziert:</b> <see cref="QueryNotebookAsync"/>. Die Doku (in der
/// bereitgestellten Fassung) enthält keine eigene REST-Anleitung für die Frage-/Antwort-Funktion -
/// nur Feldnamen aus einer Audit-Log-Tabelle (RPC <c>NotebookService.InteractSources</c>, Felder
/// <c>name</c>/<c>input_sources</c>/<c>free_form_action</c> in der Anfrage,
/// <c>response.response</c> in der Antwort). Der REST-Pfad (<c>:interactSources</c>) und die genaue
/// innere Form von <c>free_form_action</c> sind daher weiterhin eine begründete Annahme und müssen
/// beim ersten echten Testlauf verifiziert werden (siehe README).</item>
/// </list>
/// </summary>
public sealed class NotebookLmQuestionSuggester : ITeacherQuestionSuggester
{
    private static readonly string[] OAuthScopes = { "https://www.googleapis.com/auth/cloud-platform" };

    private readonly HttpClient _httpClient;
    private readonly NotebookLmOptions _options;

    public NotebookLmQuestionSuggester(HttpClient httpClient, NotebookLmOptions options)
    {
        _httpClient = httpClient;
        _options = options;
    }

    /// <summary>
    /// Basis-URL mit Regions-Präfix vor dem Hostnamen (z.B. "us-discoveryengine.googleapis.com"),
    /// exakt wie in den offiziellen curl-Beispielen dokumentiert - eine reine
    /// "discoveryengine.googleapis.com" ohne Präfix (wie ursprünglich angenommen) ist laut Doku falsch.
    /// </summary>
    private string BaseUrl => $"https://{_options.Location}-discoveryengine.googleapis.com/v1alpha";

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
                "ist noch nicht konfiguriert (Projekt-Nummer und/oder Dienstkonto-Schlüsseldatei fehlen im " +
                "Eltern-Bereich unter 'Automatisches Einlesen'). Der manuelle Eigene-Aufgaben-Editor deckt " +
                "den Kernbedarf in der Zwischenzeit ab.");
        }

        var accessToken = await GetAccessTokenAsync(cancellationToken);
        var notebookName = await CreateNotebookAsync(accessToken, subject, gradeLevel, cancellationToken);

        try
        {
            var sourceNames = await UploadSourceAsync(accessToken, notebookName, documentText, cancellationToken);
            var answerText = await QueryNotebookAsync(accessToken, notebookName, sourceNames, subject, gradeLevel, cancellationToken);
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
        // GoogleCredential implementiert ITokenAccess explizit - GetAccessTokenForRequestAsync ist
        // deshalb nur über die Interface-Referenz aufrufbar, nicht direkt auf GoogleCredential.
        // Hinweis: GoogleCredential.FromFile(string) ist als "potenzielles Sicherheitsrisiko" markiert
        // (die Bibliothek empfiehlt stattdessen CredentialFactory + .ToGoogleCredential()) - hier
        // bewusst nicht umgestellt, da die genaue Signatur dieser Alternative aus dieser Sandbox nicht
        // abschließend verifiziert werden konnte und ein Fehlversuch den Build brechen würde.
#pragma warning disable CS0618
        ITokenAccess credential = GoogleCredential.FromFile(_options.ServiceAccountKeyPath!).CreateScoped(OAuthScopes);
#pragma warning restore CS0618
        var token = await credential.GetAccessTokenForRequestAsync(cancellationToken: cancellationToken);

        if (string.IsNullOrEmpty(token))
        {
            throw new InvalidOperationException(
                "Konnte kein OAuth2-Zugriffstoken für das konfigurierte Google-Dienstkonto abrufen. " +
                "Bitte Projekt-Nummer und Schlüsseldatei im Eltern-Bereich prüfen.");
        }

        return token;
    }

    /// <summary>
    /// Verifiziert gegen die offizielle Doku (Methode <c>notebooks.create</c>): POST auf die
    /// Notebook-Collection mit Body <c>{"title": "..."}</c>, Antwort enthält u.a. <c>notebookId</c>
    /// und <c>name</c> (voller Ressourcenname, z.B. "projects/123/locations/us/notebooks/abc").
    /// </summary>
    private async Task<string> CreateNotebookAsync(string accessToken, Subject subject, GradeLevel gradeLevel, CancellationToken cancellationToken)
    {
        var url = $"{BaseUrl}/projects/{_options.ProjectId}/locations/{_options.Location}/notebooks";
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

    /// <summary>
    /// Verifiziert gegen die offizielle Doku (Methode <c>notebooks.sources.batchCreate</c>): POST auf
    /// <c>{notebookName}/sources:batchCreate</c> mit <c>{"userContents": [{"textContent": {...}}]}</c>.
    /// Gibt die vollen Ressourcennamen der angelegten Quellen zurück (für die spätere Abfrage).
    /// </summary>
    private async Task<IReadOnlyList<string>> UploadSourceAsync(string accessToken, string notebookName, string documentText, CancellationToken cancellationToken)
    {
        var url = $"{BaseUrl}/{notebookName}/sources:batchCreate";
        var request = new HttpRequestMessage(HttpMethod.Post, url)
        {
            Content = JsonContent.Create(new
            {
                userContents = new[]
                {
                    new { textContent = new { sourceName = "LernTor-Import", content = documentText } }
                }
            })
        };
        request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

        using var response = await SendAsync(request, "Dokument als Quelle hochladen", cancellationToken);
        var body = await response.Content.ReadFromJsonAsync<SourcesBatchCreateResponseDto>(cancellationToken: cancellationToken);

        return body?.Sources?.Select(s => s.Name).Where(n => n is not null).Select(n => n!).ToList()
            ?? new List<string>();
    }

    /// <summary>
    /// NICHT verifiziert - die bereitgestellte Doku enthält keine eigene REST-Anleitung für diese
    /// Funktion. Aus einer Audit-Log-Feldtabelle ist bekannt, dass die zugrundeliegende RPC-Methode
    /// <c>NotebookService.InteractSources</c> heißt und die Anfrage die Felder <c>name</c>,
    /// <c>input_sources</c>, <c>free_form_action</c> hat, während die Antwort ein verschachteltes
    /// <c>response.response</c>-Feld für den eigentlichen Antworttext enthält. Der REST-Pfad
    /// (<c>:interactSources</c>, nach Google-API-Konvention für "custom methods") und die genaue
    /// innere Form von <c>free_form_action</c> sind eine Annahme und müssen mit echten Zugangsdaten
    /// überprüft werden.
    /// </summary>
    private async Task<string> QueryNotebookAsync(string accessToken, string notebookName, IReadOnlyList<string> sourceNames, Subject subject, GradeLevel gradeLevel, CancellationToken cancellationToken)
    {
        var prompt = BuildPrompt(subject, gradeLevel);
        var url = $"{BaseUrl}/{notebookName}:interactSources";
        var request = new HttpRequestMessage(HttpMethod.Post, url)
        {
            Content = JsonContent.Create(new
            {
                inputSources = sourceNames,
                freeFormAction = new { userQuery = prompt }
            })
        };
        request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

        using var response = await SendAsync(request, "Notebook abfragen (interactSources)", cancellationToken);
        var body = await response.Content.ReadFromJsonAsync<InteractSourcesResponseDto>(cancellationToken: cancellationToken);

        return body?.Response?.Response
            ?? throw new InvalidOperationException("NotebookLM-Antwort auf die Abfrage enthielt keinen Antworttext.");
    }

    /// <summary>
    /// Verifiziert gegen die offizielle Doku (Methode <c>notebooks.batchDelete</c>): KEIN HTTP-DELETE
    /// auf die einzelne Notebook-Ressource, sondern ein POST auf die Notebook-Collection mit
    /// <c>{"names": [notebookName]}</c>. Laut Doku liefert das Löschen eines nicht (mehr) existenten
    /// Notebooks denselben leeren Erfolg zurück wie ein echtes Löschen - kein Fehler, keine 404.
    /// </summary>
    private async Task TryDeleteNotebookAsync(string accessToken, string notebookName, CancellationToken cancellationToken)
    {
        try
        {
            var url = $"{BaseUrl}/projects/{_options.ProjectId}/locations/{_options.Location}/notebooks:batchDelete";
            var request = new HttpRequestMessage(HttpMethod.Post, url)
            {
                Content = JsonContent.Create(new { names = new[] { notebookName } })
            };
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
                "Prüfe Internetverbindung und ob die konfigurierte Projekt-Nummer/Region korrekt sind.", ex);
        }

        if (!response.IsSuccessStatusCode)
        {
            var errorBody = await response.Content.ReadAsStringAsync(cancellationToken);
            throw new InvalidOperationException(
                $"NotebookLM-Aufruf '{step}' schlug fehl: HTTP {(int)response.StatusCode} {response.ReasonPhrase}. " +
                $"Antwort: {errorBody}\n\n" +
                "Falls dies ein 404 oder eine Feldvalidierungsfehlermeldung beim Abfrage-Schritt ist, stimmt " +
                "der dafür angenommene Endpunkt-Pfad/das Request-Format vermutlich nicht mit der echten API " +
                "überein (siehe Klassenkommentar zu QueryNotebookAsync) - bitte gegen die offizielle " +
                "NotebookLM-Enterprise-API-Dokumentation prüfen.");
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

        [JsonPropertyName("notebookId")]
        public string? NotebookId { get; set; }
    }

    private sealed class SourcesBatchCreateResponseDto
    {
        [JsonPropertyName("sources")]
        public List<SourceResourceDto>? Sources { get; set; }
    }

    private sealed class SourceResourceDto
    {
        [JsonPropertyName("name")]
        public string? Name { get; set; }
    }

    /// <summary>Antwort von <c>:interactSources</c> - verschachteltes "response.response"-Feld laut
    /// Audit-Log-Feldtabelle (nicht direkt aus einem REST-Beispiel bestätigt).</summary>
    private sealed class InteractSourcesResponseDto
    {
        [JsonPropertyName("response")]
        public InteractSourcesInnerResponseDto? Response { get; set; }
    }

    private sealed class InteractSourcesInnerResponseDto
    {
        [JsonPropertyName("response")]
        public string? Response { get; set; }

        [JsonPropertyName("emptyAnswerReason")]
        public string? EmptyAnswerReason { get; set; }
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
