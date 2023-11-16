using System.Text.Json.Serialization;

namespace RestApiAutomationTraining.Models;

public record TokenResponseModel
{
    [JsonPropertyName("token_type")]
    public string? TokenType { get; init; }

    [JsonPropertyName("access_token")]
    public string? AccessToken { get; init; }
}
