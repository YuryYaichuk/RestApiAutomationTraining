using NUnit.Allure.Core;
using RestApiAutomationTraining.ApiActions;
using RestApiAutomationTraining.ApiClient;
using RestApiAutomationTraining.Helpers;
using RestApiAutomationTraining.Logging;
using RestApiAutomationTraining.Models;
using RestSharp;

namespace RestApiAutomationTraining.ApiTests;

[TestFixture]
[AllureNUnit]
public abstract class BaseApiTest
{
    public static TestContext TestContext;
    protected static Random Random { get; } = new();
    protected ReadApiActions ReadApiActions { get; private set; }
    protected WriteApiActions WriteApiActions { get; private set; }

    [OneTimeSetUp]
    protected void TestOneTimeSetup()
    {
        ReadApiActions = new ReadApiActions(ApiClientForRead.GetClient);
        WriteApiActions = new WriteApiActions(ApiClientForWrite.GetClient);
    }

    [SetUp]
    protected virtual void TestSetup()
    {
        TestResults.SetLogger(TestContext.CurrentContext);
    }

    protected List<UserModel> CreateUsers(int numberOfUsers)
    {
        var newUsers = new List<UserModel>();
        var zipCodes = new List<string>();

        for (var i = 0; i < numberOfUsers; i++)
        {
            zipCodes.Add(StringHelper.GetRandomNumericString(6));
        }

        AddNewZipCodes(zipCodes.ToArray());

        zipCodes.ForEach(zip =>
        {
            var randomUser = UserModel.GenerateRandomUser(zip, Random.Next(1, 124));
            AddNewUser(randomUser);
            newUsers.Add(randomUser);

            Thread.Sleep(1000);
        });

        return newUsers;
    }

    protected void AddNewZipCodes(params string[] zipCodes)
    {
        var response = WriteApiActions.CreateZipCodes(zipCodes);

        CheckResponseIsSuccessful(response);
    }

    protected void AddNewUser(UserModel user)
    {
        var response = WriteApiActions.CreateUser(user);

        CheckResponseIsSuccessful(response);
    }

    protected void ClearZipCodes()
    {
        var zipCodes = GetZipCodesModel();

        foreach (var zipCode in zipCodes)
        {
            var user = UserModel.GenerateRandomUser(zipCode);
            AddNewUser(user);
            Thread.Sleep(1000);
        }

        if (GetZipCodesModel().Count != 0)
            throw new Exception("Not all zipCodes cleared");
    }

    /// <summary>
    /// Workaround for clearing all users
    /// </summary>
    /// <exception cref="Exception"></exception>
    protected void ClearUsers()
    {
        var users = GetUserModels();

        foreach (var user in users)
        {
            string randomZipCode = StringHelper.GetRandomNumericString(6);
            var modifiedUser = UserModel.GenerateRandomUser(randomZipCode);

            WriteApiActions.UpdateUserUsingPatch(new UpdateUserDto(modifiedUser, user));
            Thread.Sleep(1000);
        }

        if (GetUserModels().Count > 0)
            throw new Exception("Not all users cleared");
    }

    protected List<UserModel> GetUserModels() =>
        ReadApiActions.GetUsers().ToModel<List<UserModel>>();

    protected List<string> GetZipCodesModel()
    {
        var response = ReadApiActions.GetZipCodes();

        CheckResponseIsSuccessful(response);

        return response.ToModel<List<string>>();
    }
    
    private static void CheckResponseIsSuccessful(RestResponse response)
    {
        if (TestResults.Log is null) throw new Exception(
            "Logger was not initialized, make sure CustomLogger.SetLogger() is used in test pre-setup");

        if (!response.IsSuccessful)
        {
            TestResults.Log.Error("Unexpected StatusCode {StatusCode} for endpoint: {Method} {Uri}",
                response.StatusCode, response.Request.Method, response.ResponseUri);
            Assert.Fail("For details check log file");
        }
    }
}
