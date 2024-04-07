using RestApiAutomationTraining.Helpers;

namespace RestApiAutomationTraining.Logging;

public class PayloadCollector
{
    private readonly List<string> _payloads;

    public PayloadCollector()
    {
        _payloads = new List<string>();
    }

    public void AddPayload(object payload)
    {
        _payloads.Add(JsonHelper.SerializeCamelCase(payload));
    }

    public List<string> GetPayloads => _payloads;
}
