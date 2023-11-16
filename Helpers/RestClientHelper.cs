using RestApiAutomationTraining.ApiClient;
using RestSharp;

namespace RestApiAutomationTraining.Helpers;

public class RestClientHelper
{
    public static RestResponse Execute(RestRequest request)
    {
        if (request.Method == Method.Get)
            return ApiClientForRead.GetClient.Execute(request);

        return ApiClientForWrite.GetClient.Execute(request);
    }
}
