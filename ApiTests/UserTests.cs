using NUnit.Framework.Internal;
using RestApiAutomationTraining.Helpers;
using RestApiAutomationTraining.Models;

namespace RestApiAutomationTraining.ApiTests;

public class UserTests
{
    private static readonly Random _random = new();

    [Test]
    public void CreateUser_AllFieldsPopulated_Valid()
    {
        #region Test pre-setup

        string randomZipCode = StringHelper.GetRandomString(5);
        ApiActions.CreateZipCodes(randomZipCode);

        #endregion

        var randomUser = UserModel.GenerateRandomUser(randomZipCode, _random.Next(1, 124));
        var response = ApiActions.CreateUser(randomUser);
        var expectedUser = JsonHelper.DeserializeObject<List<UserModel>>(ApiActions.GetUsers())
            .SingleOrDefault(_ => _.Age == randomUser.Age && _.Name == randomUser.Name && _.ZipCode == randomUser.ZipCode);
        var zipCodes = JsonHelper.DeserializeObject<List<string>>(ApiActions.GetZipCodes());

        Assert.Multiple(() =>
        {
            Assert.That((int)response.StatusCode, Is.EqualTo(201));
            Assert.That(expectedUser, Is.Not.Null);
            Assert.That(zipCodes, Does.Not.Contain(randomUser.ZipCode));
        });
    }

    [Test]
    public void CreateUser_RequiredFieldsOnlyPopulated_Valid()
    {
        var randomUser = UserModel.GenerateRandomUser();
        var response = ApiActions.CreateUser(randomUser);
        var expectedUser = JsonHelper.DeserializeObject<List<UserModel>>(ApiActions.GetUsers())
            .SingleOrDefault(_ => _.Name == randomUser.Name);

        Assert.Multiple(() =>
        {
            Assert.That((int)response.StatusCode, Is.EqualTo(201));
            Assert.That(expectedUser, Is.Not.Null);
        });
    }

    [Test]
    public void CreateUser_NotExistingZipCode_Invalid()
    {
        var fakeZipCode = StringHelper.GetRandomNumericString(5);
        var randomUser = UserModel.GenerateRandomUser(fakeZipCode, _random.Next(1, 124));
        var response = ApiActions.CreateUser(randomUser);
        var expectedUser = JsonHelper.DeserializeObject<List<UserModel>>(ApiActions.GetUsers())
            .SingleOrDefault(_ =>
                _.Age == randomUser.Age &&
                _.Name == randomUser.Name &&
                _.ZipCode == randomUser.ZipCode);

        Assert.Multiple(() =>
        {
            Assert.That((int)response.StatusCode, Is.EqualTo(424));
            Assert.That(expectedUser, Is.Null);
        });
    }

    [Test]
    public void CreateUser_UserAlreadyExists_Invalid()
    {
        #region Test pre-setup

        UserModel randomUser = UserModel.GenerateRandomUser();
        ApiActions.CreateUser(randomUser);

        #endregion

        var response = ApiActions.CreateUser(randomUser);
        var users = JsonHelper.DeserializeObject<List<UserModel>>(ApiActions.GetUsers());

        Assert.Multiple(() =>
        {
            Assert.That((int)response.StatusCode, Is.EqualTo(400));
            Assert.That(users.Count(_ => _.Name == randomUser.Name), Is.EqualTo(1));
        });

        /*
             * Bug: The system creates duplicates for users
             * 
             * Preconditions:
             * - the user is authorized
             * - Get or create a new user
             * 
             * Steps:
             * 1. Send POST request to /users endpoint with an already existing user in the body
             * 
             * * Expected result: the error code 400 is returned
             * Actual result: success code 201 is returned
             * 
             * 2. Send GET request to /users endpoint and check results
             * 
             * Expected result: one more user with the same fields is not created
             * Actual result: the duplicate for the already existing user is created
             */
    }
}
