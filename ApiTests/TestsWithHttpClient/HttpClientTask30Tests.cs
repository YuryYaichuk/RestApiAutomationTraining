using NUnit.Allure.Attributes;
using RestApiAutomationTraining.ApiActions;
using RestApiAutomationTraining.Helpers;
using RestApiAutomationTraining.Logging;
using RestApiAutomationTraining.Models;
using System.Net;

namespace RestApiAutomationTraining.ApiTests.TestsWithHttpClient;

[TestFixture]
public class HttpClientTask30Tests : BaseApiTest
{
    //Scenario #1
    //Given I am authorized user
    //When I send POST request to /users endpoint
    //And Request body contains user to add
    //And All fields are filled in
    //Then I get 201 response code
    //And User is added to application
    //And Zip code is removed from available zip codes of application

    [Test]
    [AllureName("Test Create New User - All Fields Popolated")]
    [AllureEpic("Task 30")]
    public async Task CreateUserAsync_AllFieldsPopulated_Valid()
    {
        #region Test pre-setup

        string randomZipCode = StringHelper.GetRandomString(5);
        await AddNewZipCodesAsync(randomZipCode);

        #endregion

        var expectedUser = UserModel.GenerateRandomUser(randomZipCode, Random.Next(1, 124));
        var createUserResponse = await HttpApiActions.CreateUserAsync(expectedUser);
        PayloadCollector.AddPayload(expectedUser);

        Assert.Multiple(() =>
        {
            Asserts.AssertStatusCode(createUserResponse, HttpStatusCode.Created);
            Asserts.AssertContains(
                HttpApiActions.GetUsersAsync().Result.ToModel<List<UserModel>>(), expectedUser);
            Asserts.AssertDoesNotContain(
                HttpApiActions.GetZipCodesAsync().Result.ToModel<List<string>>(), expectedUser.ZipCode);
        });
    }

    //Scenario #2
    //Given I am authorized user
    //When I send POST request to /users endpoint
    //And Request body contains user to add
    //And Required fields are filled in
    //Then I get 201 response code
    //And User is added to application

    [Test]
    [AllureName("Test Create New User - Required Fields Only Popolated")]
    [AllureEpic("Task 30")]
    public async Task CreateUserAsync_RequiredFieldsOnlyPopulated_Valid()
    {
        var expectedUser = UserModel.GenerateRandomUser();
        var createUserResponse = await HttpApiActions.CreateUserAsync(expectedUser);

        PayloadCollector.AddPayload(expectedUser);

        Assert.Multiple(() =>
        {
            Asserts.AssertStatusCode(createUserResponse, HttpStatusCode.Created);
            Asserts.AssertContains(
                HttpApiActions.GetUsersAsync().Result.ToModel<List<UserModel>>(), expectedUser);
        });
    }

    //Scenario #3
    //Given I am authorized user
    //When I send POST request to /users endpoint
    //And Request body contains user to add
    //And All fields are filled in
    //And Zip code is incorrect(unavailable)
    //Then I get 424 response code
    //And User is not added to application

    [Test]
    [AllureName("Test Create New User - Unavailable ZIP Code")]
    [AllureEpic("Task 30")]
    public async Task CreateUserAsync_NotExistingZipCode_Invalid()
    {
        var expectedUser = UserModel.GenerateRandomUser(
            StringHelper.GetRandomNumericString(6), Random.Next(1, 124));

        var createUserResponse = await HttpApiActions.CreateUserAsync(expectedUser);

        PayloadCollector.AddPayload(expectedUser);

        Assert.Multiple(() =>
        {
            Asserts.AssertStatusCode(createUserResponse, HttpStatusCode.FailedDependency);
            Asserts.AssertDoesNotContain(
                HttpApiActions.GetUsersAsync().Result.ToModel<List<UserModel>>(), expectedUser);
        });
    }

    //Scenario #4
    //Given I am authorized user
    //When I send POST request to /users endpoint
    //And Request body contains user to add with the same name and sex as existing user in the system
    //Then I get 400 response code
    //And User is not added to application

    [Test]
    [AllureName("Test Create New User - User with specified Name/Sex already exists")]
    [AllureEpic("Task 30")]
    [AllureIssue("The system creates duplicates for users")]
    public async Task CreateUserAsync_UserAlreadyExists_Invalid()
    {
        #region Test pre-setup

        var expectedUser = UserModel.GenerateRandomUser();

        await AddNewUserAsync(expectedUser);

        #endregion

        var createUserResponse = await HttpApiActions.CreateUserAsync(expectedUser);

        PayloadCollector.AddPayload(expectedUser);

        Assert.Multiple(() =>
        {
            Asserts.AssertStatusCode(createUserResponse, HttpStatusCode.BadRequest);
            Asserts.AssertCount(1,
                HttpApiActions.GetUsersAsync().Result.ToModel<List<UserModel>>(), expectedUser);
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
