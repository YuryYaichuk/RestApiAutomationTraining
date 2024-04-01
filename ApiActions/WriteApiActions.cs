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

        return _client.Execute(request);
    }

    public RestResponse CreateUser(UserModel user)
    {
        const string uri = "/users";
        var request = new RestRequest(uri, Method.Post);
        request.AddJsonBody(user);

        return _client.Execute(request);
    }

    /// <summary>
    /// Deletes a specified user
    /// </summary>
    /// <param name="user">user model</param>
    /// <returns></returns>
    public RestResponse DeleteUser(UserModel user)
    {
        const string uri = "/users";
        var request = new RestRequest(uri, Method.Delete);
        request.AddJsonBody(user);

        return _client.Execute(request);
    }

    /// <summary>
    /// Updates an existing user by using PATCH request method
    /// </summary>
    /// <param name="user">UpdateUserDto model</param>
    /// <returns></returns>
    public RestResponse UpdateUserUsingPatch(UpdateUserDto user)
    {
        const string uri = "/users";
        var request = new RestRequest(uri, Method.Patch);
        request.AddJsonBody(user);

        return _client.Execute(request);
    }

    /// <summary>
    /// Updates an existing user by using PUT request method
    /// </summary>
    /// <param name="user">UpdateUserDto model</param>
    /// <returns></returns>
    public RestResponse UpdateUserUsingPut(UpdateUserDto user)
    {
        const string uri = "/users";
        var request = new RestRequest(uri, Method.Put);
        request.AddJsonBody(user);

        return _client.Execute(request);
    }

    /// <summary>
    /// Uploads JSON file with array of users
    /// </summary>
    /// <param name="filePath"></param>
    /// <returns></returns>
    public RestResponse UploadUsers(string filePath)
    {
        const string uri = "/users/upload";

        var request = new RestRequest(uri, Method.Post);
        request.AddFile("file", filePath);

        return _client.Execute(request);
    }
}
