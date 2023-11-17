//using RestSharp;
//using System;
//using System.Collections.Generic;
//using System.Diagnostics;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;

//namespace RestApiAutomationTraining.Helpers;

//public class ApiWait
//{
//    public static void WaitUntilObjectCreated<T>(T expectedObject, Func<object> methodWithRequest,
//        int waitTimeSeconds = 10, int pollingMls = 1000)
//    {
//        var stopwatch = new Stopwatch();
//        stopwatch.Start();

//        while (true)
//        {
//            // This method works for code that returns IRest Responses and models
//            // Here we check the return type of the delegate
//            List<T> actualObject;

//            if (methodWithRequest.Invoke() is RestResponse response)
//            {
//                actualObject = JsonHelper.DeserializeObject<List<T>>(response);
//            }
//            else
//            {
//                actualObject = JsonUtility.SerializeCamelCase(methodWithRequest.Invoke());
//            }

//            //var actualPartialObject = JsonUtility.DeserializeCamelCase<T>(actualObject);

//            //if (actualPartialObject is not null && actualPartialObject!.Equals(expectedObject)) return;

//            if (stopwatch.Elapsed.TotalSeconds > waitTimeSeconds)
//            {
//                Assert.Fail($"Wait operation failed in {waitTimeSeconds} seconds." +
//                    $"\nExpected object:{JsonUtility.SerializeCamelCase(expectedObject)} " +
//                    $"\nActual object:{JsonUtility.SerializeCamelCase(actualPartialObject)}");

//            }

//            Thread.Sleep(pollingMls);

//        }
//    }
//}
