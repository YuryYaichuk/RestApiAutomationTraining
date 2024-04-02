using RestApiAutomationTraining.Authentificator;
using RestApiAutomationTraining.Models;
using System.Net.Http.Headers;
using System.Text.Json;

namespace RestApiAutomationTraining.ApiClient;

public class HttpClientForWrite
{
    private static HttpClient? _client;

    public static HttpClient GetClient
    {
        get
        {
            if (_client == null)
            {
                _client = new HttpClient()
                {
                    BaseAddress = new Uri(GlobalConstants.BaseUrl)
                };
                var token = new TokenHttpClient("oauth/token")
                    .GetTokenAsync(GlobalConstants.ClientId, GlobalConstants.ClientSecret, Enums.ScopeEnum.Write)
                    .Result;

                var tokenModel = JsonSerializer.Deserialize<TokenResponseModel>(token);
                _client.DefaultRequestHeaders.Authorization =
                    new AuthenticationHeaderValue(tokenModel.TokenType, tokenModel.AccessToken);
            }

            return _client;
        }
    }
}
