using RestApiAutomationTraining.Enums;
using RestSharp;

namespace RestApiAutomationTraining.Authentificator;

public class ReadScopeAuthentificator : CustomAuthentificator
{
    public ReadScopeAuthentificator() : base()
    {
    }

    public string GetToken(string baseUrl) => GetToken(baseUrl, ScopeEnum.Read);

    protected override ValueTask<Parameter> GetAuthenticationParameter(string accessToken)
    {
        throw new NotImplementedException();
    }
}
