using RestApiAutomationTraining.Helpers;
using RestSharp;

namespace RestApiAutomationTraining.ApiTests;

public class BaseApiTest
{
    protected static T DeserializeResponse<T>(RestResponse response)
    {
        return JsonHelper.DeserializeObject<T>(response);
    }
}
