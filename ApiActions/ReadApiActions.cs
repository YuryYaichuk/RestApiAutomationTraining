using RestSharp;

namespace RestApiAutomationTraining.ApiActions;

public class ReadApiActions
{
    private readonly RestClient _client;

    public ReadApiActions(RestClient client)
    {
        _client = client;
    }

    /// <summary>
    /// Get available zip codes
    /// </summary>
    /// <returns></returns>
    public RestResponse GetZipCodes()
    {
        const string uri = "/zip-codes";
        var request = new RestRequest(uri);
        var response = _client.Execute(request);

        return response;
    }

    public RestResponse GetUsers(params (string, string)[] parameters)
    {
        const string uri = "/users";
        var request = new RestRequest(uri);

        if (parameters != null)
        {
            foreach (var pair in parameters)
            {
                request.AddQueryParameter(pair.Item1, pair.Item2);
            }
        }
        var response = _client.Execute(request);

        return response;
    }
}
