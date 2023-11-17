using RestApiAutomationTraining.Enums;
using RestApiAutomationTraining.Helpers;
using RestApiAutomationTraining.Models;
using RestSharp;
using RestSharp.Authenticators;

namespace RestApiAutomationTraining.Authentificator;

public abstract class CustomAuthentificator : AuthenticatorBase
{
    protected RestClientOptions Options { get; }
    private static RestClient? _client;

    public CustomAuthentificator(string baseUrl, string user, string password) : base("")
    {
        Options = new RestClientOptions(baseUrl)
        {
            Authenticator = new HttpBasicAuthenticator(user, password)
        };

        _client = new RestClient(Options);
    }

    protected static string GetToken(ScopeEnum scope)
    {
        var request = new RestRequest("oauth/token")
                .AddParameter("grant_type", "client_credentials")
                .AddParameter("scope", scope.ToString().ToLower());
        var response = _client!.Execute(request, Method.Post);
        var tokenModel = JsonHelper.DeserializeObject<TokenResponseModel>(response);
        return $"{tokenModel.TokenType} {tokenModel.AccessToken}";
    }
}
