using RestApiAutomationTraining.Authentificator;
using RestSharp;

namespace RestApiAutomationTraining.ApiClient;

public class ApiClientForRead
{
    private static RestClient? _client;

    public static RestClient GetClient
    {
        get
        {
            if (_client == null)
            {
                var authentificator = new ReadScopeAuthentificator(GlobalConstants.BaseUrl,
                    GlobalConstants.ClientId,
                    GlobalConstants.ClientSecret);
                _client = new RestClient(new RestClientOptions(GlobalConstants.BaseUrl));
                _client.AddDefaultHeader(KnownHeaders.Authorization, ReadScopeAuthentificator.GetToken());
            }

            return _client;
        }
    }
}
