using NUnit.Allure.Attributes;
using RestApiAutomationTraining.ApiActions;
using RestApiAutomationTraining.Enums;
using RestApiAutomationTraining.Helpers;
using RestApiAutomationTraining.Models;
using System.Net;

namespace RestApiAutomationTraining.ApiTests.TestsWithHttpClient;

[TestFixture]
public class HttpClientTask40Tests : BaseApiTest
{
    //Scenario #1
    //Given I am authorizated user
    //When I send GET request to /users endpoint
    //Then I get 200 respone code
    //And I get all users stored in the application for now

    [AllureName("Test Get Users - Unfiltered")]
    [AllureEpic("Task 40")]
    [Test]
    public async Task GetUsersAsync_Valid()
    {
        #region Test pre-setup

        const int extraUserNumber = 3;
        var initialUsers = await GetUserModelsAsync();

        await CreateUsersAsync(extraUserNumber);

        #endregion

        var response = await HttpApiActions.GetUsersAsync();

        Assert.Multiple(async () =>
        {
            Asserts.AssertStatusCode(response, HttpStatusCode.OK);
            Assert.That(await response.ToModelAsync<List<UserModel>>(),
                Has.Count.GreaterThanOrEqualTo(initialUsers.Count + extraUserNumber));
        });
    }

    //Scenario #2
    //Given I am authorizated user
    //When I send GET request to /users endpoint
    //And I add olderThan parameter
    //Then I get 200 respone code
    //And I get all users older than value of parameter

    [AllureName("Test Get Users - Filtered by 'olderThan'")]
    [AllureEpic("Task 40")]
    [Test]
    public async Task GetUsersAsync_FilteredByOlderThan_Valid()
    {
        const int ageLimit = 12;
        const string paramName = "olderThan";

        #region Test pre-setup

        await CreateUsersAsync(3);

        var usersNotFiltered = await GetUserModelsAsync();

        #endregion

        var response = await HttpApiActions.GetUsersAsync((paramName, ageLimit.ToString()));

        Assert.Multiple(async () =>
        {
            Asserts.AssertStatusCode(response, HttpStatusCode.OK);
            Assert.That(await response.ToModelAsync<List<UserModel>>(),
                Has.Count.EqualTo(usersNotFiltered.Count(_ => _.Age > ageLimit && _.Age != null)));
        });
    }

    //Scenario #3
    //Given I am authorizated user
    //When I send GET request to /users endpoint
    //And I add youngerThan parameter
    //Then I get 200 respone code
    //And I get all users younger than value of parameter

    [AllureName("Test Get Users - Filtered by 'youngerThan'")]
    [AllureEpic("Task 40")]
    [Test]
    public async Task GetUsersAsync_FilteredByYoungerThan_Valid()
    {
        const int ageLimit = 60;
        const string paramName = "youngerThan";

        #region Test pre-setup

        await CreateUsersAsync(3);

        var usersNotFiltered = await GetUserModelsAsync();

        #endregion

        var response = await HttpApiActions.GetUsersAsync((paramName, ageLimit.ToString()));

        Assert.Multiple(async () =>
        {
            Asserts.AssertStatusCode(response, HttpStatusCode.OK);
            Assert.That(await response.ToModelAsync<List<UserModel>>(),
                Has.Count.EqualTo(usersNotFiltered.Count(_ => _.Age < ageLimit && _.Age != null)));
        });
    }

    //Scenario #4
    //Given I am authorizated user
    //When I send GET request to /users endpoint
    //And I add sex parameter
    //Then I get 200 respone code
    //And I get all users with sex value of parameter

    [AllureName("Test Get Users - Filtered by 'sex'")]
    [AllureEpic("Task 40")]
    [Test]
    public async Task GetUsersAsync_FilteredBySex_Valid()
    {
        var filterValue = SexEnum.FEMALE.ToString();
        const string paramName = "sex";

        #region Test pre-setup

        await CreateUsersAsync(3);

        var usersNotFiltered = await GetUserModelsAsync();

        #endregion

        var response = await HttpApiActions.GetUsersAsync((paramName, filterValue));

        Assert.Multiple(async () =>
        {
            Asserts.AssertStatusCode(response, HttpStatusCode.OK);
            Assert.That(await response.ToModelAsync<List<UserModel>>(),
                Has.Count.EqualTo(usersNotFiltered.Count(_ => _.Sex == filterValue)));
        });
    }
}
