//using Newtonsoft.Json;
//using Newtonsoft.Json.Serialization;
//using NUnit.Allure.Attributes;
//using RestApiAutomationTraining.Helpers;
//using RestApiAutomationTraining.Models;
//using System.Net;

//namespace RestApiAutomationTraining.ApiTests;

//public class Task70Tests : BaseApiTest
//{
//    //Scenario #1         
//    //Given I am authorized user
//    //When I send POST request to /users/upload endpoint
//    //And Request body contains json file with array of users to upload
//    //Then I get 201 response code
//    //And All users are replaced with users from file
//    //And Response contains number of uploaded users

//    [Test]
//    [AllureName("Test Upload Users From JSON File")]
//    [AllureEpic("Task 70")]
//    public void UploadUsers_Valid()
//    {
//        #region Test pre-setup

//        const string fileName = "TestDataFile.json";
//        var zipCodes = new List<string> { StringHelper.GetRandomNumericString(6), StringHelper.GetRandomNumericString(6) };
//        var users = zipCodes.Select(zip =>
//        {
//            return UserModel.GenerateRandomUser(zip, Random.Next(1, 124));
//        });

//        AddNewZipCodes(zipCodes.ToArray());

//        var usersJson = JsonConvert.SerializeObject(users, new JsonSerializerSettings
//        {
//            ContractResolver = new CamelCasePropertyNamesContractResolver()
//        });
//        File.WriteAllText(fileName, usersJson);
//        PayloadCollector.AddPayload(usersJson);

//        #endregion

//        var uploadUsersResponse = WriteApiActions.UploadUsers(fileName);
//        var getUsersResponse = ReadApiActions.GetUsers();

//        Assert.Multiple(() =>
//        {
//            Asserts.AssertStatusCode(uploadUsersResponse, HttpStatusCode.Created);
//            Asserts.AreEqual(usersJson, getUsersResponse);
//            Asserts.ResponseContainsText($"Number of users = {users.Count()}", uploadUsersResponse);
//        });
//    }


//    //Scenario #2 
//    //Given I am authorized user
//    //When I send POST request to /users/upload endpoint
//    //And Request body contains json file with array of users to upload
//    //And At least one user has incorrect(unavailable) zip code
//    //Then I get 424 response code
//    //And Users are not uploaded

//    [Test]
//    [AllureName("Test Upload Users From JSON File - One User with Unavailable ZIP Code")]
//    [AllureEpic("Task 70")]
//    [AllureIssue("Bug1: Status code 500 is returned instead of 424")]
//    [AllureIssue("Bug2: Valid users from list are uploaded anyway")]
//    public void UploadUsers_IncorrectZipCode_Invalid()
//    {
//        #region Test pre-setup

//        const string fileName = "TestDataFile.json";
//        var zipCodes = new List<string> { StringHelper.GetRandomNumericString(6), StringHelper.GetRandomNumericString(6) };
//        var users = zipCodes.Select(zip =>
//        {
//            return UserModel.GenerateRandomUser(zip, Random.Next(1, 124));
//        });

//        AddNewZipCodes(zipCodes[0]);

//        var usersJson = JsonConvert.SerializeObject(users, new JsonSerializerSettings
//        {
//            ContractResolver = new CamelCasePropertyNamesContractResolver()
//        });
//        File.WriteAllText(fileName, usersJson);
//        PayloadCollector.AddPayload(usersJson);

//        #endregion

//        var getUsersResponseBefore = ReadApiActions.GetUsers();
//        var uploadUsersResponse = WriteApiActions.UploadUsers(fileName);
//        var getUsersResponseAfter = ReadApiActions.GetUsers();

//        Assert.Multiple(() =>
//        {
//            Asserts.AssertStatusCode(uploadUsersResponse, HttpStatusCode.FailedDependency);
//            Asserts.AreEqual(getUsersResponseBefore, getUsersResponseAfter);
//        });

//        /*
//         * Bug1: Status code 500 is returned instead of 424, 
//         * Bug2: Valid users from list are uploaded anyway
//         */
//    }

//    //Scenario #3 
//    //Given I am authorized user
//    //When I send POST request to /users/upload endpoint
//    //And Request body contains json file with array of users to upload
//    //And At least one user has missed required field
//    //Then I get 409 response code
//    //And Users are not uploaded

//    [Test]
//    [AllureName("Test Upload Users From JSON File - One User with Missing Sex Value")]
//    [AllureEpic("Task 70")]
//    [AllureIssue("Bug1: Status code 500 is returned instead of 409")]
//    [AllureIssue("Bug2: Valid users from list are uploaded anyway")]
//    public void UploadUsers_MissingRequiredField_Invalid()
//    {
//        #region Test pre-setup

//        const string fileName = "TestDataFile.json";

//        var validUser = UserModel
//            .GenerateRandomUser(StringHelper.GetRandomNumericString(6), Random.Next(1, 124));
//        var invalidUser = UserModel
//            .GenerateRandomUser(StringHelper.GetRandomNumericString(6), Random.Next(1, 124))
//            with { Sex = null };

//        var users = new List<UserModel>() { validUser, invalidUser };

//        AddNewZipCodes(users.Select(_ => _.ZipCode).ToArray()!);

//        var usersJson = JsonConvert.SerializeObject(users, new JsonSerializerSettings
//        {
//            ContractResolver = new CamelCasePropertyNamesContractResolver()
//        });
//        File.WriteAllText(fileName, usersJson);
//        PayloadCollector.AddPayload(usersJson);

//        #endregion

//        var getUsersResponseBefore = ReadApiActions.GetUsers();
//        var uploadUsersResponse = WriteApiActions.UploadUsers(fileName);
//        var getUsersResponseAfter = ReadApiActions.GetUsers();

//        Assert.Multiple(() =>
//        {
//            Asserts.AssertStatusCode(uploadUsersResponse, HttpStatusCode.Conflict);
//            Asserts.AreEqual(getUsersResponseBefore, getUsersResponseAfter);
//        });

//        /*
//         * Bug1: Status code 500 is returned instead of 409, 
//         * Bug2: Valid users from list are uploaded anyway
//         */
//    }
//}
