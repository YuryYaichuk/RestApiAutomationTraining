using RestSharp;
using System.Net;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace RestApiAutomationTraining.Helpers;

public class Asserts
{
    public static void AssertStatusCode(RestResponse response, int expectedCode)
    {
        var actualStatusCode = (int)response.StatusCode;
        Assert.That(actualStatusCode, Is.EqualTo(expectedCode),
            $"Wrong StatusCode for endpoint: [{response.Request.Method} {response.ResponseUri}]");
    }

    public static void AssertStatusCode(RestResponse response, HttpStatusCode expectedCode)
    {
        var actualStatusCode = response.StatusCode;
        Assert.That(actualStatusCode, Is.EqualTo(expectedCode),
            $"Wrong StatusCode for endpoint: [{response.Request.Method} {response.ResponseUri}]");
    }
}
