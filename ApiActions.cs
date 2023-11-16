using RestApiAutomationTraining.Helpers;
using RestApiAutomationTraining.Models;
using RestSharp;

namespace RestApiAutomationTraining;

public class ApiActions
{
    /// <summary>
    /// Get available zip codes
    /// </summary>
    /// <returns></returns>
    public static RestResponse GetZipCodes()
    {
        const string uri = "/zip-codes";
        var request = new RestRequest(uri);
        var response = RestClientHelper.Execute(request);
        Assert.That(response.IsSuccessful, Is.True);
        return response;
    }

    /// <summary>
    /// Expand available zip codes
    /// </summary>
    /// <param name="zipCodes">zip codes as strings</param>
    /// <returns></returns>
    public static RestResponse CreateZipCodes(params string[] zipCodes)
    {
        const string uri = "/zip-codes/expand";
        var request = new RestRequest(uri, Method.Post);
        request.AddJsonBody(zipCodes);
        var response = RestClientHelper.Execute(request);
        Assert.That(response.IsSuccessful, Is.True);
        return response;
    }

    public static RestResponse GetUsers()
    {
        const string uri = "/users";
        var request = new RestRequest(uri);
        var response = RestClientHelper.Execute(request);
        return response;
    }

    public static RestResponse CreateUser(UserModel user)
    {
        const string uri = "/users";
        var request = new RestRequest(uri, Method.Post);
        request.AddJsonBody(user);
        var response = RestClientHelper.Execute(request);
        Assert.That(response.IsSuccessful, Is.True);
        return response;
    }
}
