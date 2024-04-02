//using NUnit.Allure.Attributes;
//using RestApiAutomationTraining.ApiActions;
//using RestApiAutomationTraining.Enums;
//using RestApiAutomationTraining.Helpers;
//using RestApiAutomationTraining.Models;
//using System.Net;

//namespace RestApiAutomationTraining.ApiTests.TestsWithHttpClient;

//[TestFixture]
//public class HttpClientTask40Tests : BaseApiTest
//{
//    //Scenario #1
//    //Given I am authorizated user
//    //When I send GET request to /users endpoint
//    //Then I get 200 respone code
//    //And I get all users stored in the application for now

//    [AllureName("Test Get Users - Unfiltered")]
//    [AllureEpic("Task 40")]
//    [Test]
//    public async Task GetUsersAsync_Valid()
//    {
//        #region Test pre-setup

//        const int extraUserNumber = 3;
//        var initialUsersCount = GetUserModels().Count;
//        var getUsersResponse = HttpApiActions.GetUsersAsync();
//        var count = getUsersResponse.Result.ToModel<List<UserModel>>().Count;

//        await CreateUsersAsync(extraUserNumber);

//        #endregion

//        var response = await HttpApiActions.GetUsersAsync();
//        var actualUserList = response.ToModel<List<UserModel>>();

//        Assert.Multiple(() =>
//        {
//            Asserts.AssertStatusCode(response, HttpStatusCode.OK);
//            Assert.That(actualUserList, Has.Count.GreaterThanOrEqualTo(initialUsersCount + extraUserNumber));
//        });
//    }

//    [AllureName("Test Get Users - Filtered by 'olderThan'")]
//    [AllureEpic("Task 40")]
//    [Test]
//    public async Task GetUsersAsync_FilteredByOlderThan_Valid()
//    {
//        const int ageLimit = 12;
//        const string paramName = "olderThan";

//        #region Test pre-setup

//        await CreateUsersAsync(3);

//        var expectedUserCount = GetUserModels()
//            .Count(_ => _.Age > ageLimit && _.Age != null);

//        #endregion

//        var response = ReadApiActions.GetUsers((paramName, ageLimit.ToString()));
//        var actualUsers = response.ToModel<List<UserModel>>();

//        Assert.Multiple(() =>
//        {
//            Asserts.AssertStatusCode(response, HttpStatusCode.OK);
//            Assert.That(actualUsers, Has.Count.EqualTo(expectedUserCount));
//        });
//    }

//    [AllureName("Test Get Users - Filtered by 'youngerThan'")]
//    [AllureEpic("Task 40")]
//    [Test]
//    public async Task GetUsersAsync_FilteredByYoungerThan_Valid()
//    {
//        const int ageLimit = 60;
//        const string paramName = "youngerThan";

//        #region Test pre-setup

//        CreateUsers(3);

//        var expectedUserCount = GetUserModels()
//            .Count(_ => _.Age < ageLimit && _.Age != null);

//        #endregion

//        var response = ReadApiActions.GetUsers((paramName, ageLimit.ToString()));
//        var actualUsers = response.ToModel<List<UserModel>>();

//        Assert.Multiple(() =>
//        {
//            Asserts.AssertStatusCode(response, HttpStatusCode.OK);
//            Assert.That(actualUsers, Has.Count.EqualTo(expectedUserCount));
//        });
//    }

//    [AllureName("Test Get Users - Filtered by 'sex'")]
//    [AllureEpic("Task 40")]
//    [Test]
//    public async Task GetUsersAsync_FilteredBySex_Valid()
//    {
//        var filterValue = SexEnum.FEMALE.ToString();
//        const string paramName = "sex";

//        #region Test pre-setup

//        CreateUsers(3);

//        var expectedUserCount = GetUserModels().Count(_ => _.Sex == filterValue);

//        #endregion

//        var response = ReadApiActions.GetUsers((paramName, filterValue));
//        var actualUsers = response.ToModel<List<UserModel>>();

//        Assert.Multiple(() =>
//        {
//            Asserts.AssertStatusCode(response, HttpStatusCode.OK);
//            Assert.That(actualUsers, Has.Count.EqualTo(expectedUserCount));
//        });
//    }
//}
