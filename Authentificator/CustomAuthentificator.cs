using RestApiAutomationTraining.Enums;
using RestApiAutomationTraining.Helpers;
using RestApiAutomationTraining.Models;
using RestSharp;
using RestSharp.Authenticators;

namespace RestApiAutomationTraining.Authentificator;

public abstract class CustomAuthentificator : AuthenticatorBase
{
    public CustomAuthentificator() : base("")
    {
    }

    protected string GetToken(string baseUrl, ScopeEnum scope)
    {
        if (string.IsNullOrEmpty(Token))
        {
            var options = new RestClientOptions(baseUrl)
            {
                Authenticator = new HttpBasicAuthenticator(
                GlobalConstants.ClientId, GlobalConstants.ClientSecret)
            };

            using var client = new RestClient(options);
            var request = new RestRequest("oauth/token")
                .AddParameter("grant_type", "client_credentials")
                .AddParameter("scope", scope.ToString().ToLower());
            var response = client.Execute(request, Method.Post);
            var tokenModel = JsonHelper.DeserializeObject<TokenResponseModel>(response);
            Token = $"{tokenModel.TokenType} {tokenModel.AccessToken}";
        }

        return Token;
    }
}
