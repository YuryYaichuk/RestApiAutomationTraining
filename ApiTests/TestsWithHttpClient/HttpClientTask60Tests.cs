//using NUnit.Allure.Attributes;
//using RestApiAutomationTraining.ApiActions;
//using RestApiAutomationTraining.Helpers;
//using RestApiAutomationTraining.Models;
//using System.Net;

//namespace RestApiAutomationTraining.ApiTests.TestsWithHttpClient;

//[TestFixture]
//public class HttpClientTask60Tests : BaseApiTest
//{
//    /* 
//     * Scenario #1
//     * Given I am authorized user
//     * When I send DELETE request to /users endpoint
//     * And Request body contains user to delete
//     * Then I get 204 response code
//     * And User is deleted
//     * And Zip code is returned in list of available zip codes
//     */

//    [Test]
//    [AllureName("Test Delete User")]
//    [AllureEpic("Task 60")]
//    public async Task DeleteUserAsync_UserDeleted_Valid()
//    {
//        #region Test pre-setup

//        var expectedZipCode = StringHelper.GetRandomNumericString(6);
//        await AddNewZipCodesAsync(expectedZipCode);

//        var expectedUser = UserModel.GenerateRandomUser(expectedZipCode, Random.Next(1, 124));
//        await AddNewUserAsync(expectedUser);

//        #endregion

//        var deleteUserResponse = WriteApiActions.DeleteUser(expectedUser);
//        PayloadCollector.AddPayload(expectedUser);

//        Assert.Multiple(() =>
//        {
//            Asserts.AssertStatusCode(deleteUserResponse, HttpStatusCode.NoContent);
//            Assert.That(GetUserModels().Any(u => u.Name == expectedUser.Name), Is.False,
//                $"User was not deleted: [{expectedUser}]");
//            Assert.That(GetZipCodesModel().Where(zip => zip == expectedUser.ZipCode).ToList(), Has.Count.EqualTo(1),
//                $"Zip codes returned to list of available zip codes");
//        });
//    }

//    /*
//     * Scenario #2
//     * Given I am authorized user
//     * When I send DELETE request to /users endpoint
//     * And Request body contains user to delete(required fields only)
//     * Then I get 204 response code
//     * And User is deleted
//     * And Zip code is returned in list of available zip codes
//    */

//    [Test]
//    [AllureName("Test Delete User - Required Fields Only Specified")]
//    [AllureEpic("Task 60")]
//    [AllureIssue("Bug: User is not deleted when required fields only specified")]
//    public async Task DeleteUserAsync_UserDeletedIfRequiredFieldsOnlySpecified_Valid()
//    {
//        #region Test pre-setup

//        var expectedZipCode = StringHelper.GetRandomNumericString(6);
//        await AddNewZipCodesAsync(expectedZipCode);

//        var expectedUser = UserModel.GenerateRandomUser(expectedZipCode, Random.Next(1, 124));
//        await AddNewUserAsync(expectedUser);

//        var userToDelete = expectedUser with
//        {
//            Age = null,
//            ZipCode = null
//        };

//        #endregion

//        var deleteUserResponse = WriteApiActions.DeleteUser(userToDelete);
//        PayloadCollector.AddPayload(userToDelete);

//        Assert.Multiple(() =>
//        {
//            Asserts.AssertStatusCode(deleteUserResponse, HttpStatusCode.NoContent);
//            Assert.That(GetUserModels().Any(u => u.Name == userToDelete.Name), Is.False,
//                $"User was not deleted: [{expectedUser}]");
//            Assert.That(GetZipCodesModel().Where(zip => zip == expectedUser.ZipCode).ToList(), Has.Count.EqualTo(1),
//                $"Zip codes returned to list of available zip codes");
//        });

//        /*
//         * Bug: User is not deleted when required fields only specified, 
//         * zip code does not return to list of available zip codes
//         */
//    }

//    /*
//     * Scenario #3
//     * Given I am authorized user
//     * When I send DELETE request to /users endpoint
//     * And Request body contains user to delete(any required field is missed)
//     * Then I get 409 response code
//     * And User is not deleted
//     */

//    [Test]
//    [AllureName("Test Delete User - Name is Not Specified")]
//    [AllureEpic("Task 60")]
//    public async Task DeleteUserAsync_RequiredFieldNameMissing_Invalid()
//    {
//        #region Test pre-setup

//        var expectedZipCode = StringHelper.GetRandomNumericString(6);
//        await AddNewZipCodesAsync(expectedZipCode);

//        var expectedUser = UserModel.GenerateRandomUser(expectedZipCode, Random.Next(1, 124));
//        await AddNewUserAsync(expectedUser);

//        var userToDelete = expectedUser with { Name = null };

//        #endregion

//        var deleteUserResponse = WriteApiActions.DeleteUser(userToDelete);
//        PayloadCollector.AddPayload(userToDelete);
//        var deletedUser = GetUserModels()
//            .Where(u => u.Name == expectedUser.Name && u.Sex == userToDelete.Sex)
//            .ToList();

//        Assert.Multiple(() =>
//        {
//            Asserts.AssertStatusCode(deleteUserResponse, HttpStatusCode.Conflict);
//            Assert.That(deletedUser, Has.Count.EqualTo(1), $"User was deleted: [{expectedUser}]");
//        });
//    }

//    [Test]
//    [AllureName("Test Delete User - Sex is Not Specified")]
//    [AllureEpic("Task 60")]
//    public async Task DeleteUserAsync_RequiredFieldSexMissing_Invalid()
//    {
//        #region Test pre-setup

//        var expectedZipCode = StringHelper.GetRandomNumericString(6);
//        await AddNewZipCodesAsync(expectedZipCode);

//        var expectedUser = UserModel.GenerateRandomUser(expectedZipCode, Random.Next(1, 124));
//        await AddNewUserAsync(expectedUser);

//        var userToDelete = GetUserModels().First() with { Sex = null };

//        #endregion

//        var deleteUserResponse = WriteApiActions.DeleteUser(userToDelete);
//        PayloadCollector.AddPayload(userToDelete);
//        var deletedUser = GetUserModels()
//            .Where(u => u.Name == expectedUser.Name)
//            .ToList();

//        Assert.Multiple(() =>
//        {
//            Asserts.AssertStatusCode(deleteUserResponse, HttpStatusCode.Conflict);
//            Assert.That(deletedUser, Has.Count.EqualTo(1), $"User was deleted: [{expectedUser}]");
//        });
//    }
//}
