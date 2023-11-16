using RestApiAutomationTraining.Authentificator;
using RestSharp;

namespace RestApiAutomationTraining.ApiClient;

public class ApiClientForWrite
{
    private static RestClient? _client;

    private ApiClientForWrite(string baseUrl)

    {
        _client = new RestClient(new RestClientOptions(baseUrl));
        _client.AddDefaultHeader(KnownHeaders.Authorization,
                    new WriteScopeAuthentificator().GetToken(baseUrl));
    }

    public static RestClient GetClient
    {
        get
        {
            if (_client == null)
            {
                _ = new ApiClientForWrite(GlobalConstants.BaseUrl);
            }

            return _client ?? throw new NullReferenceException("No instance found");
        }
    }
}
