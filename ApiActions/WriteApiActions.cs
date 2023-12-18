using RestApiAutomationTraining.Models;
using RestSharp;

namespace RestApiAutomationTraining.ApiActions;

public class WriteApiActions
{
    private readonly RestClient _client;

    public WriteApiActions(RestClient client)
    {
        _client = client;
    }

    /// <summary>
    /// Expand available zip codes
    /// </summary>
    /// <param name="zipCodes">zip codes as strings</param>
    /// <returns></returns>
    public RestResponse CreateZipCodes(params string[] zipCodes)
    {
        const string uri = "/zip-codes/expand";
        var request = new RestRequest(uri, Method.Post);
        request.AddJsonBody(zipCodes);
        var response = _client.Execute(request);

        return response;
    }

    public RestResponse CreateUser(UserModel user)
    {
        const string uri = "/users";
        var request = new RestRequest(uri, Method.Post);
        request.AddJsonBody(user);
        var response = _client.Execute(request);

        return response;
    }

    public RestResponse DeleteUser(UserModel user)
    {
        const string uri = "/users";
        var request = new RestRequest(uri, Method.Delete);
        request.AddJsonBody(user);
        var response = _client.Execute(request);

        return response;
    }

    public RestResponse UpdateUser(UpdateUserDto user)
    {
        const string uri = "/users";
        var request = new RestRequest(uri, Method.Patch);
        request.AddJsonBody(user);
        var response = _client.Execute(request);

        return response;
    }
}
