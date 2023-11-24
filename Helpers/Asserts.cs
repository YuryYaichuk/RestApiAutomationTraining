using RestSharp;

namespace RestApiAutomationTraining.Helpers;

public class Asserts
{
    public static void AssertStatusCode(RestResponse response, int expectedCode)
    {
        var actualStatusCode = (int)response.StatusCode;
        Assert.That(actualStatusCode, Is.EqualTo(expectedCode),
            $"Wrong StatusCode for endpoint: [{response.Request.Method} {response.ResponseUri}]");
    }
}
