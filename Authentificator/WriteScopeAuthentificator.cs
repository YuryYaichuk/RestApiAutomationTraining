using RestApiAutomationTraining.Enums;
using RestSharp;

namespace RestApiAutomationTraining.Authentificator;

public class WriteScopeAuthentificator : CustomAuthentificator
{
    public WriteScopeAuthentificator(string baseUrl, string user, string password)
        : base(baseUrl, user, password)
    {
    }

    public static string GetToken() => GetToken(ScopeEnum.Write);

    protected override ValueTask<Parameter> GetAuthenticationParameter(string accessToken)
    {
        throw new NotImplementedException();
    }
}
