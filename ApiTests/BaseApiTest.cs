using RestApiAutomationTraining.ApiActions;
using RestApiAutomationTraining.ApiClient;
using RestApiAutomationTraining.Helpers;
using RestApiAutomationTraining.Models;

namespace RestApiAutomationTraining.ApiTests;

public class BaseApiTest
{
    protected static Random Random { get; } = new();
    protected ReadApiActions ReadApiActions { get; private set; }
    protected WriteApiActions WriteApiActions { get; private set; }

    [OneTimeSetUp]
    protected void TestOneTimeSetup()
    {
        ReadApiActions = new ReadApiActions(ApiClientForRead.GetClient);
        WriteApiActions = new WriteApiActions(ApiClientForWrite.GetClient);
    }

    [TearDown]
    protected void TestCleanup()
    {

    }

    protected List<UserModel> CreateUsers(int numberOfUsers, bool addAge = false)
    {
        var newUsers = new List<UserModel>();
        var random = new Random();
        int? age = null;

        for (int i = 0; i < numberOfUsers; i++)
        {
            if (addAge) age = random.Next(1, 124);
            var randomUser = UserModel.GenerateRandomUser(age: age);
            var response = WriteApiActions.CreateUser(randomUser);
            AssertWrapper.AssertStatusCode(response, 201);
            newUsers.Add(randomUser);
            Thread.Sleep(1000);
        }

        return newUsers;
    }

    protected void AddNewZipCodes(params string[] zipCodes)
    {
        var response = WriteApiActions.CreateZipCodes(zipCodes);
        AssertWrapper.AssertStatusCode(response, 201);
    }

    protected void ClearZipCodes()
    {
        var zipCodes = JsonHelper.DeserializeObject<List<string>>(ReadApiActions.GetZipCodes());

        foreach (var zipCode in zipCodes)
        {
            var user = UserModel.GenerateRandomUser(zipCode);
            WriteApiActions.CreateUser(user);
            Thread.Sleep(1000);
        }

        if (JsonHelper.DeserializeObject<List<string>>(ReadApiActions.GetZipCodes()).Count != 0)
            throw new Exception("Not all zipCodes cleared");
    }

    protected void ClearUsers()
    {
        var users = JsonHelper.DeserializeObject<List<UserModel>>(ReadApiActions.GetUsers());

        foreach (var user in users)
        {
            var response = WriteApiActions.DeleteUser(user);
            Assert.That((int)response.StatusCode, Is.EqualTo(204));
            Thread.Sleep(1000);
        }

        if (JsonHelper.DeserializeObject<List<UserModel>>(ReadApiActions.GetUsers()).Count != 0)
            throw new Exception("Not all users cleared");
    }
}
