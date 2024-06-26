﻿using NUnit.Allure.Attributes;
using RestSharp;

namespace RestApiAutomationTraining.Helpers;

public static class RestResponseExtention
{
    [AllureStep("Deserializing response")]
    public static T ToModel<T>(this RestResponse response)
    {
        if(response.IsSuccessful)
            return JsonHelper.DeserializeObject<T>(response);

        throw new Exception($"Response is invalid. Current status is {response.StatusCode}");
    }
}
