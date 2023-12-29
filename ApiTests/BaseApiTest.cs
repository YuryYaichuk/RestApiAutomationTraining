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

    protected List<UserModel> CreateUsers(int numberOfUsers, bool addAge = false)
    {
        var newUsers = new List<UserModel>();
        //int? age = null;

        for (int i = 0; i < numberOfUsers; i++)
        {
            var zipCode = StringHelper.GetRandomNumericString(6);
            AddNewZipCodes(zipCode);

            //if (addAge) age = Random.Next(1, 124);
            var randomUser = UserModel.GenerateRandomUser(zipCode, Random.Next(1, 124));
            AddNewUser(randomUser);
            newUsers.Add(randomUser);

            Thread.Sleep(1000);
        }

        return newUsers;
    }

    protected void AddNewZipCodes(params string[] zipCodes)
    {
        var response = WriteApiActions.CreateZipCodes(zipCodes);
        Asserts.AssertStatusCode(response, 201);
    }

    protected void AddNewUser(UserModel user)
    {
        var response = WriteApiActions.CreateUser(user);
        Asserts.AssertStatusCode(response, 201);
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

    protected List<string> GetZipCodesModel() =>
        ReadApiActions.GetZipCodes().ToModel<List<string>>();
}
