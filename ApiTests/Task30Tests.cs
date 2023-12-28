using NUnit.Framework.Internal;
using RestApiAutomationTraining.ApiActions;
using RestApiAutomationTraining.Helpers;
using RestApiAutomationTraining.Models;

namespace RestApiAutomationTraining.ApiTests;

[TestFixture]
public class Task30Tests : BaseApiTest
{
    [SetUp]
    protected void TestSetup()
    {
        ClearUsers();
    }

    [Test]
    public void CreateUser_AllFieldsPopulated_Valid()
    {
        #region Test pre-setup

        string randomZipCode = StringHelper.GetRandomString(5);
        AddNewZipCodes(randomZipCode);

        #endregion

        var expectedUser = UserModel.GenerateRandomUser(randomZipCode, Random.Next(1, 124));
        var createUserResponse = WriteApiActions.CreateUser(expectedUser);
        var actualUser = GetUserModels().SingleOrDefault(_ =>_.Name == expectedUser.Name);
        var zipCodes = GetZipCodesModel();

        Assert.Multiple(() =>
        {
            Asserts.AssertStatusCode(createUserResponse, 201);
            Assert.That(actualUser, Is.EqualTo(expectedUser));
            Assert.That(zipCodes, Does.Not.Contain(expectedUser.ZipCode));
        });
    }

    [Test]
    public void CreateUser_RequiredFieldsOnlyPopulated_Valid()
    {
        var expectedUser = UserModel.GenerateRandomUser();
        var createUserResponse = WriteApiActions.CreateUser(expectedUser);
        var actualUser = GetUserModels().SingleOrDefault(_ => _.Name == expectedUser.Name);

        Assert.Multiple(() =>
        {
            Asserts.AssertStatusCode(createUserResponse, 201);
            Assert.That(actualUser, Is.EqualTo(expectedUser));
        });
    }

    [Test]
    public void CreateUser_NotExistingZipCode_Invalid()
    {
        var fakeZipCode = StringHelper.GetRandomNumericString(5);
        var expectedUser = UserModel.GenerateRandomUser(fakeZipCode, Random.Next(1, 124));
        var createUserResponse = WriteApiActions.CreateUser(expectedUser);
        var actualUser = GetUserModels().SingleOrDefault(_ 
            => _.Name == expectedUser.Name && _.ZipCode == expectedUser.ZipCode);

        Assert.Multiple(() =>
        {
            Asserts.AssertStatusCode(createUserResponse, 424);
            Assert.That(actualUser, Is.Null);
        });
    }

    [Test]
    public void CreateUser_UserAlreadyExists_Invalid()
    {
        #region Test pre-setup

        UserModel expectedUser = UserModel.GenerateRandomUser();
        AddNewUser(expectedUser);

        #endregion

        var response = WriteApiActions.CreateUser(expectedUser);
        var usersCount = GetUserModels().Count(_ => _.Name == expectedUser.Name);

        Assert.Multiple(() =>
        {
            Asserts.AssertStatusCode(response, 400);
            Assert.That(usersCount, Is.EqualTo(1));
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
