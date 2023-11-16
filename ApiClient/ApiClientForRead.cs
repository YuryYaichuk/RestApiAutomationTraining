using RestApiAutomationTraining.Authentificator;
using RestSharp;

namespace RestApiAutomationTraining.ApiClient;

public class ApiClientForRead
{
    private static RestClient? _client;

    private ApiClientForRead(string baseUrl)
    {
        _client = new RestClient(new RestClientOptions(baseUrl));
        _client.AddDefaultHeader(KnownHeaders.Authorization,
                    new ReadScopeAuthentificator().GetToken(baseUrl));
    }

    public static RestClient GetClient
    {
        get
        {
            if (_client == null)
            {
                _ = new ApiClientForRead(GlobalConstants.BaseUrl);
            }

            return _client ?? throw new NullReferenceException("No instance found");
        }
    }
}
