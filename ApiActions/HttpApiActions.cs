using NUnit.Allure.Attributes;
using RestApiAutomationTraining.ApiClient;
using RestApiAutomationTraining.Helpers;
using RestApiAutomationTraining.Models;
using System.Text.Json;

namespace RestApiAutomationTraining.ApiActions;

public class HttpApiActions
{
    /// <summary>
    /// Get available zip codes
    /// </summary>
    /// <returns></returns>
    [AllureStep("Getting zip codes from '/zip-codes'")]
    public static async Task<HttpResponseMessage> GetZipCodesAsync()
    {
        const string uri = "/zip-codes";

        return await HttpClientForRead.GetClient.GetAsync(uri);
    }

    /// <summary>
    /// Getting users
    /// </summary>
    /// <param name="parameters">key-value filters</param>
    /// <returns></returns>
    [AllureStep("Getting users from '/users'")]
    public static async Task<HttpResponseMessage> GetUsersAsync(params (string, string)[] parameters)
    {
        const string uri = "/users";
        var filters = string.Empty;

        if (parameters != null)
        {
            filters = "?" + string.Join("&", parameters);
        }

        return await HttpClientForRead.GetClient.GetAsync(uri + filters);
    }

    /// <summary>
    /// Expand available zip codes
    /// </summary>
    /// <param name="zipCodes">zip codes as strings</param>
    /// <returns></returns>
    [AllureStep("Adding new zip codes with POST '/zip-codes/expand'")]
    public static async Task<HttpResponseMessage> CreateZipCodesAsync(params string[] zipCodes)
    {
        const string uri = "/zip-codes/expand";
        var payload =
            new StringContent(JsonSerializer.Serialize(zipCodes), System.Text.Encoding.UTF8, "application/json");

        return await HttpClientForWrite.GetClient.PostAsync(uri, payload);
    }

    /// <summary>
    /// Creates a new user
    /// </summary>
    /// <param name="user"></param>
    /// <returns></returns>
    [AllureStep("Creating a new user with POST '/users'")]
    public static async Task<HttpResponseMessage> CreateUserAsync(UserModel user)
    {
        const string uri = "/users";
        var payload =
            new StringContent(JsonHelper.SerializeCamelCase(user), System.Text.Encoding.UTF8, "application/json");

        return await HttpClientForWrite.GetClient.PostAsync(uri, payload);
    }
}
