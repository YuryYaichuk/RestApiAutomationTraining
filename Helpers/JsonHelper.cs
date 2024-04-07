using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using RestSharp;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace RestApiAutomationTraining.Helpers;

public class JsonHelper
{
    public static T DeserializeObject<T>(RestResponse response)
    {
        var valueToDeserialize = response.Content ??
            throw new Exception($"No content found for request:\n {response.ResponseUri}");

        JsonSerializerOptions options = new()
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            PropertyNameCaseInsensitive = true,
            Converters = { new JsonStringEnumConverter() }
        };

        T? model;

        try
        {
            model = System.Text.Json.JsonSerializer.Deserialize<T>(valueToDeserialize, options);
        }
        catch (Exception ex)
        {
            throw new Exception($"Deserialization failed for {typeof(T).Name}. " +
                $"String to deserialize: {valueToDeserialize}\nOriginal error: {ex.Message}");
        }

        return model ?? throw new Exception("Model is null");
    }

    public static string SerializeCamelCase(object obj)
    {
        return JsonConvert.SerializeObject(obj, new JsonSerializerSettings
        {
            ContractResolver = new CamelCasePropertyNamesContractResolver()
        });
    }
}
