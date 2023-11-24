using RestSharp;

namespace RestApiAutomationTraining.Helpers;

public static class RestResponseExtention
{
    public static T ToModel<T>(this RestResponse response)
    {
        Assert.That(response.IsSuccessful, Is.True);

        return JsonHelper.DeserializeObject<T>(response);
    }
}
