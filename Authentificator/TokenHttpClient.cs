using RestApiAutomationTraining.Enums;
using System.Text;

namespace RestApiAutomationTraining.Authentificator;

public class TokenHttpClient
{
    private readonly HttpClient _httpClient;
    private readonly string _tokenEndpoint;

    public TokenHttpClient(string tokenEndpoint)
    {
        _httpClient = new HttpClient()
        {
            BaseAddress = new Uri(GlobalConstants.BaseUrl)
        };
        _tokenEndpoint = tokenEndpoint;
    }

    public async Task<string> GetTokenAsync(string user, string password, ScopeEnum scope)
    {
        _httpClient.DefaultRequestHeaders.Authorization =
            new System.Net.Http.Headers.AuthenticationHeaderValue("Basic",
            Convert.ToBase64String(Encoding.ASCII.GetBytes($"{user}:{password}")));

        var requestBody = new Dictionary<string, string>()
        {
            { "grant_type", "client_credentials" },
            { "scope", scope.ToString().ToLower() }
        };

        var requestContent = new FormUrlEncodedContent(requestBody);

        var response = await _httpClient.PostAsync(_tokenEndpoint, requestContent);
        response.EnsureSuccessStatusCode();

        var token = await response.Content.ReadAsStringAsync();

        return token;
    }
}
