using RestApiAutomationTraining.Authentificator;
using RestSharp;

namespace RestApiAutomationTraining.ApiClient;

public class ApiClientForWrite
{
    private static RestClient? _client;

    public static RestClient GetClient
    {
        get
        {
            if (_client is null)
            {
                var authentificator = new WriteScopeAuthentificator(GlobalConstants.BaseUrl,
                    GlobalConstants.ClientId,
                    GlobalConstants.ClientSecret);
                _client = new RestClient(new RestClientOptions(GlobalConstants.BaseUrl));
                _client.AddDefaultHeader(KnownHeaders.Authorization, WriteScopeAuthentificator.GetToken());
            }

            return _client;
        }
    }
}
