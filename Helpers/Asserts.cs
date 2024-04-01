using RestApiAutomationTraining.Logging;
using RestSharp;
using System.Net;

namespace RestApiAutomationTraining.Helpers;

public class Asserts
{
    public static void AssertStatusCode(RestResponse response, int expectedCode)
    {
        var actualStatusCode = (int)response.StatusCode;
        var errorMessage = $"Wrong StatusCode for endpoint: [{response.Request.Method} {response.ResponseUri}]\n" +
            $"Expected [{expectedCode}]\n" +
            $"Actual   [{actualStatusCode}]";

        if (actualStatusCode != expectedCode)
        {
            TestResults.Log.Error(errorMessage);
            Assert.Fail(errorMessage);
        }
        //Assert.That(actualStatusCode, Is.EqualTo(expectedCode), errorMessage);
    }

    public static void AssertStatusCode(RestResponse response, HttpStatusCode expectedCode)
    {
        var actualStatusCode = response.StatusCode;
        Assert.That(actualStatusCode, Is.EqualTo(expectedCode),
            $"Wrong StatusCode for endpoint: [{response.Request.Method} {response.ResponseUri}]");
    }

    public static void AssertContainsAll(IEnumerable<string> containingCollection, IEnumerable<string> containedCollection)
    {
        var notFoundItems = containedCollection.ToList().Where(item => !containingCollection.Contains(item));

        if (notFoundItems.Any())
        {
            Assert.Fail($"Expected items [{string.Join("|", notFoundItems)}]\n" +
                $"were not found in\n" +
                $"[{string.Join("|", containingCollection)}]");
        }
    }

    public static void AssertContains<T>(IEnumerable<T> containingCollection, T containedObject)
    {
        if (!containingCollection.Any(item => item.Equals(containedObject)))
        {
            Assert.Fail($"Expected item [{containedObject}]\n" +
                $"was NOT found in\n" +
                $"[{string.Join("\n", containingCollection)}]");
        }
    }

    public static void AssertDoesNotContain<T>(IEnumerable<T> containingCollection, T containedText)
    {
        if (containingCollection.Any(item => item.Equals(containedText)))
        {
            Assert.Fail($"Unexpected item [{containedText}]\n" +
                $"was found in\n" +
                $"[{string.Join("|", containingCollection)}]");
        }
    }

    public static void AssertCount(int expectedCount, int actualCount)
    {
        if (expectedCount != actualCount)
        {
            Assert.Fail($"Expected count [{expectedCount}]\n" +
                        $"Actual count   [{actualCount}]");
        }
    }

    public static void AssertCount<T>(int expectedCount, IEnumerable<T> collection, T objectToCount)
    {
        var actualCount = collection.Count(item => item.Equals(objectToCount));
        if (expectedCount != actualCount)
        {
            Assert.Fail($"Expected count [{expectedCount}] for [{objectToCount}]\n" +
                        $"Actual count   [{actualCount}]");
        }
    }

    public static void AreEqual(string expected, RestResponse actual)
    {
        if (expected != actual.Content)
        {
            Assert.Fail($"Expected [{expected}]\n" +
                        $"Actual   [{actual.Content}]");
        }
    }

    public static void AreEqual(RestResponse expected, RestResponse actual)
    {
        if (expected.Content != actual.Content)
        {
            Assert.Fail($"Expected [{expected}]\n" +
                        $"Actual   [{actual.Content}]");
        }
    }

    public static void ResponseContainsText(string expected, RestResponse actual)
    {
        if (!(actual.Content ?? string.Empty).Contains(expected))
        {
            Assert.Fail($"Expected [{expected}]\n" +
                        $"Actual   [{actual}]");
        }
    }
}
