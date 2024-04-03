using NUnit.Allure.Attributes;
using RestApiAutomationTraining.ApiActions;
using RestApiAutomationTraining.Helpers;
using RestApiAutomationTraining.Models;
using System.Net;

namespace RestApiAutomationTraining.ApiTests.TestsWithHttpClient;

[TestFixture]
public class HttpClientTask50Tests : BaseApiTest
{
    /*
     * Scenario #1 
     * Given I am authorized user 
     * When I send PUT/PATCH request to /users endpoint
     * And Request body contains user to update and new values 
     * Then I get 200 response code
     * And User is updated 
     */

    [Test]
    [AllureName("Test Update User with PATCH - Updating Age")]
    [AllureEpic("Task 50")]
    public async Task UpdateUserAsync_WithPatch_UpdateAge_Valid()
    {
        #region Test pre-setup

        var userToModify = (await CreateUsersAsync(1)).First();

        string randomZipCode = StringHelper.GetRandomNumericString(5);
        await AddNewZipCodesAsync(randomZipCode);

        #endregion

        var modifiedUser = UserModel.GenerateRandomUser(randomZipCode, 100);
        var payLoad = new UpdateUserDto(modifiedUser, userToModify);
        var updateUserResponse = await HttpApiActions.UpdateUserUsingPatchAsync(payLoad);

        PayloadCollector.AddPayload(payLoad);

        Assert.Multiple(async () =>
        {
            Asserts.AssertStatusCode(updateUserResponse, HttpStatusCode.OK);
            Asserts.AssertCount(0,
                (await GetUserModelsAsync()).Where(user => user.Name == userToModify.Name).ToList().Count);
            Asserts.AssertContains(await GetUserModelsAsync(), modifiedUser);
        });
    }

    [Test]
    [AllureName("Test Update User with PUT - Updating Age")]
    [AllureEpic("Task 50")]
    public async Task UpdateUserAsync_WithPut_UpdateAge_Valid()
    {
        #region Test pre-setup

        var userToModify = (await CreateUsersAsync(1)).First();

        string randomZipCode = StringHelper.GetRandomNumericString(5);
        await AddNewZipCodesAsync(randomZipCode);

        #endregion

        var modifiedUser = UserModel.GenerateRandomUser(randomZipCode, 100);
        var payLoad = new UpdateUserDto(modifiedUser, userToModify);
        var updateUserResponse = await HttpApiActions.UpdateUserUsingPutAsync(payLoad);

        PayloadCollector.AddPayload(payLoad);

        Assert.Multiple(async () =>
        {
            Asserts.AssertStatusCode(updateUserResponse, HttpStatusCode.OK);
            Asserts.AssertCount(0,
                (await GetUserModelsAsync()).Where(user => user.Name == userToModify.Name).ToList().Count);
            Asserts.AssertContains(await GetUserModelsAsync(), modifiedUser);
        });
    }

    /*
     * Scenario #2 
     * Given I am authorized user 
     * When I send PUT/PATCH request to /users endpoint 
     * And Request body contains user to update and new values 
     * And New zip code is incorrect (unavailable) 
     * Then I get 424 response code 
     * And User is not updated 
     */

    [Test]
    [AllureName("Test Update User with PATCH - New ZIP Code is Unavailable")]
    [AllureEpic("Task 50")]
    [AllureIssue("Bug: Updating user with unavailable Zip Code deletes the original user")]
    public async Task UpdateUserAsync_WithPatch_UnavailableZipCode_InValid()
    {
        #region Test pre-setup

        var userToModify = (await CreateUsersAsync(1)).First();
        var unavailableZipCode = StringHelper.GetRandomNumericString(6);

        #endregion

        var modifiedUser = UserModel.GenerateRandomUser(unavailableZipCode, Random.Next(1, 123));
        var payLoad = new UpdateUserDto(modifiedUser, userToModify);
        var updateUserResponse = await HttpApiActions.UpdateUserUsingPatchAsync(payLoad);

        PayloadCollector.AddPayload(payLoad);

        Assert.Multiple(async () =>
        {
            Asserts.AssertStatusCode(updateUserResponse, HttpStatusCode.FailedDependency);
            Asserts.AssertContains(await GetUserModelsAsync(), userToModify);
            Asserts.AssertDoesNotContain(await GetUserModelsAsync(), modifiedUser);
        });

        // Bug: Updating user with unavailable Zip Code deletes the original user
    }

    [Test]
    [AllureName("Test Update User with PUT - New ZIP Code is Unavailable")]
    [AllureEpic("Task 50")]
    [AllureIssue("Bug: Updating user with unavailable Zip Code deletes the original user")]
    public async Task UpdateUserAsync_WithPut_UnavailableZipCode_InValid()
    {
        #region Test pre-setup

        var userToModify = (await CreateUsersAsync(1)).First();
        var unavailableZipCode = StringHelper.GetRandomNumericString(6);

        #endregion

        var modifiedUser = UserModel.GenerateRandomUser(unavailableZipCode, Random.Next(1, 123));
        var payLoad = new UpdateUserDto(modifiedUser, userToModify);
        var updateUserResponse = await HttpApiActions.UpdateUserUsingPutAsync(payLoad);

        PayloadCollector.AddPayload(payLoad);

        Assert.Multiple(async () =>
        {
            Asserts.AssertStatusCode(updateUserResponse, HttpStatusCode.FailedDependency);
            Asserts.AssertContains(await GetUserModelsAsync(), userToModify);
            Asserts.AssertDoesNotContain(await GetUserModelsAsync(), modifiedUser);
        });

        // Bug: Updating user with unavailable Zip Code deletes the original user
    }


    /*
     * Scenario #3 
     * Given I am authorized user 
     * When I send PUT/PATCH request to /users endpoint 
     * And Request body contains user to update and new values 
     * And Required fields are missed 
     * Then I get 409 response code 
     * And User is not updated 
     */

    [Test]
    [AllureName("Test Update User with PATCH - Name is Missing")]
    [AllureEpic("Task 50")]
    [AllureIssue("Bug: Updating user with invalid data deletes the original user")]
    public async Task UpdateUserAsync_WithPatch_NameMissed_Invalid()
    {
        #region Test pre-setup

        var userToModify = (await CreateUsersAsync(1)).First();

        var randomZipCode = StringHelper.GetRandomNumericString(6);
        await AddNewZipCodesAsync(randomZipCode);

        #endregion

        var modifiedUser = UserModel.GenerateRandomUser(randomZipCode, Random.Next(1, 124)) with
        {
            Name = null
        };

        var payLoad = new UpdateUserDto(modifiedUser, userToModify);
        var updateUserResponse = await HttpApiActions.UpdateUserUsingPatchAsync(payLoad);

        PayloadCollector.AddPayload(payLoad);

        Assert.Multiple(async () =>
        {
            Asserts.AssertStatusCode(updateUserResponse, HttpStatusCode.Conflict);
            Asserts.AssertContains(await GetUserModelsAsync(), userToModify);
            Asserts.AssertDoesNotContain(await GetUserModelsAsync(), modifiedUser);
        });

        // Bug: Updating user with invalid data deletes the original user
    }

    [Test]
    [AllureName("Test Update User with PUT - Name is Missing")]
    [AllureEpic("Task 50")]
    [AllureIssue("Bug: Updating user with invalid data deletes the original user")]
    public async Task UpdateUserAsync_WithPut_NameMissed_Invalid()
    {
        #region Test pre-setup

        var userToModify = (await CreateUsersAsync(1)).First();

        var randomZipCode = StringHelper.GetRandomNumericString(6);
        await AddNewZipCodesAsync(randomZipCode);

        #endregion

        var modifiedUser = UserModel.GenerateRandomUser(randomZipCode, Random.Next(1, 124)) with
        {
            Name = null
        };

        var payLoad = new UpdateUserDto(modifiedUser, userToModify);
        var updateUserResponse = await HttpApiActions.UpdateUserUsingPutAsync(payLoad);

        PayloadCollector.AddPayload(payLoad);

        Assert.Multiple(async () =>
        {
            Asserts.AssertStatusCode(updateUserResponse, HttpStatusCode.Conflict);
            Asserts.AssertContains(await GetUserModelsAsync(), userToModify);
            Asserts.AssertDoesNotContain(await GetUserModelsAsync(), modifiedUser);
        });

        // Bug: Updating user with invalid data deletes the original user
    }

    [Test]
    [AllureName("Test Update User with PATCH - Sex is Missing")]
    [AllureEpic("Task 50")]
    [AllureIssue("Bug: Updating user with invalid data deletes the original user")]
    public async Task UpdateUserAsync_WithPatch_SexMissed_Invalid()
    {
        #region Test pre-setup

        var userToModify = (await CreateUsersAsync(1)).First();

        var randomZipCode = StringHelper.GetRandomNumericString(6);
        await AddNewZipCodesAsync(randomZipCode);

        #endregion

        var modifiedUser = UserModel.GenerateRandomUser(randomZipCode, Random.Next(1, 124)) with
        {
            Sex = null
        };

        var payLoad = new UpdateUserDto(modifiedUser, userToModify);
        var updateUserResponse = await HttpApiActions.UpdateUserUsingPatchAsync(payLoad);

        PayloadCollector.AddPayload(payLoad);

        Assert.Multiple(async () =>
        {
            Asserts.AssertStatusCode(updateUserResponse, HttpStatusCode.Conflict);
            Asserts.AssertContains(await GetUserModelsAsync(), userToModify);
            Asserts.AssertDoesNotContain(await GetUserModelsAsync(), modifiedUser);
        });

        // Bug: Updating user with invalid data deletes the original user
    }

    [Test]
    [AllureName("Test Update User with PUT - Sex is Missing")]
    [AllureEpic("Task 50")]
    [AllureIssue("Bug: Updating user with invalid data deletes the original user")]
    public async Task UpdateUserAsync_WithPut_SexMissed_Invalid()
    {
        #region Test pre-setup

        var userToModify = (await CreateUsersAsync(1)).First();

        var randomZipCode = StringHelper.GetRandomNumericString(6);
        await AddNewZipCodesAsync(randomZipCode);

        #endregion

        var modifiedUser = UserModel.GenerateRandomUser(randomZipCode, Random.Next(1, 124)) with
        {
            Sex = null
        };

        var payLoad = new UpdateUserDto(modifiedUser, userToModify);
        var updateUserResponse = await HttpApiActions.UpdateUserUsingPutAsync(payLoad);

        PayloadCollector.AddPayload(payLoad);

        Assert.Multiple(async () =>
        {
            Asserts.AssertStatusCode(updateUserResponse, HttpStatusCode.Conflict);
            Asserts.AssertContains(await GetUserModelsAsync(), userToModify);
            Asserts.AssertDoesNotContain(await GetUserModelsAsync(), modifiedUser);
        });

        // Bug: Updating user with invalid data deletes the original user
    }
}
