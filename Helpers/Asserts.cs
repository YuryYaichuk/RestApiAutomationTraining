using RestApiAutomationTraining.Logging;
using RestSharp;
using System.Net;

namespace RestApiAutomationTraining.Helpers;

public class Asserts
{
    public static void AssertStatusCode(RestResponse response, HttpStatusCode expectedCode)
    {
        var actualStatusCode = response.StatusCode;
        
        if (actualStatusCode != expectedCode)
        {
            var errorMessage = 
                $"Wrong StatusCode for endpoint: [{response.Request.Method} {response.ResponseUri}]\n" +
                $"Expected [{expectedCode}]\n" +
                $"Actual   [{actualStatusCode}]";

            TestResults.Log.Error(errorMessage);
            Assert.Fail(errorMessage);
        }
    }

    public static void AssertContainsAll(IEnumerable<string> containingCollection, IEnumerable<string> containedCollection)
    {
        var notFoundItems = containedCollection.ToList().Where(item => !containingCollection.Contains(item));

        if (notFoundItems.Any())
        {
            var errorMessage =
                $"Expected items [{string.Join("|", notFoundItems)}]\n" +
                $"were not found in\n" +
                $"[{string.Join("|", containingCollection)}]";

            TestResults.Log.Error(errorMessage);
            Assert.Fail(errorMessage);
        }
    }

    public static void AssertContains<T>(IEnumerable<T> containingCollection, T containedObject)
    {
        if (!containingCollection.Any(item => item.Equals(containedObject)))
        {
            var errorMessage =
                $"Expected item [{containedObject}]\n" +
                $"was NOT found in\n" +
                $"[{string.Join("\n", containingCollection)}]";

            TestResults.Log.Error(errorMessage);
            Assert.Fail(errorMessage);
        }
    }

    public static void AssertDoesNotContain<T>(IEnumerable<T> containingCollection, T containedText)
    {
        if (containingCollection.Any(item => item.Equals(containedText)))
        {
            var errorMessage =
                $"Unexpected item [{containedText}]\n" +
                $"was found in\n" +
                $"[{string.Join("|", containingCollection)}]";

            TestResults.Log.Error(errorMessage);
            Assert.Fail(errorMessage);
        }
    }

    public static void AssertCount(int expectedCount, int actualCount)
    {
        if (expectedCount != actualCount)
        {
            var errorMessage =
                $"Expected count [{expectedCount}]\n" +
                $"Actual count   [{actualCount}]";

            TestResults.Log.Error(errorMessage);
            Assert.Fail(errorMessage);
        }
    }

    public static void AssertCount<T>(int expectedCount, IEnumerable<T> collection, T objectToCount)
    {
        var actualCount = collection.Count(item => item.Equals(objectToCount));
        if (expectedCount != actualCount)
        {
            var errorMessage =
                $"Expected count [{expectedCount}] for [{objectToCount}]\n" +
                $"Actual count   [{actualCount}]";

            TestResults.Log.Error(errorMessage);
            Assert.Fail(errorMessage);
        }
    }

    public static void AreEqual(string expected,  RestResponse actual)
    {
        if (expected != actual.Content)
        {
            var errorMessage =
                $"Expected [{expected}]\n" +
                $"Actual   [{actual.Content}]";

            TestResults.Log.Error(errorMessage);
            Assert.Fail(errorMessage);
        }
    }

    public static void AreEqual(RestResponse expected, RestResponse actual)
    {
        if (expected.Content != actual.Content)
        {
            var errorMessage =
                $"Expected [{expected.Content}]\n" +
                $"Actual   [{actual.Content}]";

            TestResults.Log.Error(errorMessage);
            Assert.Fail(errorMessage);
        }
    }

    public static void ResponseContainsText(string expected, RestResponse actual)
    {
        if (!(actual.Content ?? string.Empty).Contains(expected))
        {
            var errorMessage =
                $"Expected [{expected}]\n" +
                $"Actual   [{actual.Content}]";

            TestResults.Log.Error(errorMessage);
            Assert.Fail(errorMessage);
        }
    }
}
