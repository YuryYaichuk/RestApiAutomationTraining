using RestApiAutomationTraining.Enums;
using RestSharp;

namespace RestApiAutomationTraining.Authentificator;

public class ReadScopeAuthentificator : CustomAuthentificator
{
    public ReadScopeAuthentificator(string baseUrl, string user, string password)
        : base(baseUrl, user, password)
    {
    }

    public static string GetToken() => GetToken(ScopeEnum.Read);

    protected override ValueTask<Parameter> GetAuthenticationParameter(string accessToken)
    {
        throw new NotImplementedException();
    }
}
