using RestApiAutomationTraining.Enums;
using RestSharp;

namespace RestApiAutomationTraining.Authentificator;

public class WriteScopeAuthentificator : CustomAuthentificator
{
    public WriteScopeAuthentificator() : base()
    {
    }

    public string GetToken(string baseUrl) => GetToken(baseUrl, ScopeEnum.Write);

    protected override ValueTask<Parameter> GetAuthenticationParameter(string accessToken)
    {
        throw new NotImplementedException();
    }
}
