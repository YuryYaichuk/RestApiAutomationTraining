using RestApiAutomationTraining.ApiActions;
using RestApiAutomationTraining.Enums;
using RestApiAutomationTraining.Helpers;
using RestApiAutomationTraining.Models;
using System.Net;

namespace RestApiAutomationTraining.ApiTests;

[TestFixture]
public class Task50Tests : BaseApiTest
{
    [SetUp]
    protected void TestSetup()
    {
        ClearUsers();
    }


    /*
     * Scenario #1 
     * Given I am authorized user 
     * When I send PUT/PATCH request to /users endpoint
     * And Request body contains user to update and new values 
     * Then I get 200 response code
     * And User is updated 
     */

    [Test]
    public void UpdateUser_WithPatch_UpdateAge_Valid()
    {
        #region Test pre-setup

        CreateUsers(1, true);
        var userToModify = GetUserModels().Last();

        string randomZipCode = StringHelper.GetRandomNumericString(5);
        AddNewZipCodes(randomZipCode);

        #endregion

        var modifiedUser = new UserModel(StringHelper.GenerateName(8),
            SexEnum.FEMALE.ToString(), 100, randomZipCode);

        var updateUserResponse = WriteApiActions.UpdateUserUsingPatch(new UpdateUserDto(modifiedUser, userToModify));
        var originalUser = GetUserModels().SingleOrDefault(user => user.Name == userToModify.Name);
        var updatedUser = GetUserModels().SingleOrDefault(user => user.Name == modifiedUser.Name);

        Assert.Multiple(() =>
        {
            Asserts.AssertStatusCode(updateUserResponse, HttpStatusCode.OK);
            Assert.That(originalUser, Is.Null);
            Assert.That(updatedUser, Is.EqualTo(modifiedUser));
        });
    }

    [Test]
    public void UpdateUser_WithPut_UpdateAge_Valid()
    {
        #region Test pre-setup

        CreateUsers(1, true);
        var userToModify = GetUserModels().Last();

        string randomZipCode = StringHelper.GetRandomNumericString(5);
        AddNewZipCodes(randomZipCode);

        #endregion

        var modifiedUser = new UserModel(StringHelper.GenerateName(8),
            SexEnum.FEMALE.ToString(), 100, randomZipCode);

        var updateUserResponse = WriteApiActions.UpdateUserUsingPut(new UpdateUserDto(modifiedUser, userToModify));
        var originalUser = GetUserModels().SingleOrDefault(user => user.Name == userToModify.Name);
        var updatedUser = GetUserModels().SingleOrDefault(user => user.Name == modifiedUser.Name);

        Assert.Multiple(() =>
        {
            Asserts.AssertStatusCode(updateUserResponse, HttpStatusCode.OK);
            Assert.That(originalUser, Is.Null);
            Assert.That(updatedUser, Is.EqualTo(modifiedUser));
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
    public void UpdateUser_WithPatch_UnavailableZipCode_InValid()
    {
        #region Test pre-setup

        CreateUsers(1, true);
        var userToModify = GetUserModels().Last();

        string randomZipCode = StringHelper.GetRandomNumericString(6);

        #endregion

        var modifiedUser = new UserModel(StringHelper.GenerateName(8),
            SexEnum.FEMALE.ToString(), Random.Next(1, 123), randomZipCode);

        var updateUserResponse = WriteApiActions.UpdateUserUsingPatch(new UpdateUserDto(modifiedUser, userToModify));
        var originalUser = GetUserModels().SingleOrDefault(user => user.Name == userToModify.Name);
        var updatedUser = GetUserModels().SingleOrDefault(user => user.Name == modifiedUser.Name);

        Assert.Multiple(() =>
        {
            Asserts.AssertStatusCode(updateUserResponse, HttpStatusCode.FailedDependency);
            Assert.That(updatedUser, Is.Null);
            Assert.That(originalUser, Is.EqualTo(userToModify));
        });

        /*
             * Bug: Updating user with unavailable Zip Code deletes the original user
             */
    }

    [Test]
    public void UpdateUser_WithPut_UnavailableZipCode_InValid()
    {
        #region Test pre-setup

        CreateUsers(1, true);
        var userToModify = GetUserModels().Last();

        string randomZipCode = StringHelper.GetRandomNumericString(6);

        #endregion

        var modifiedUser = new UserModel(StringHelper.GenerateName(8),
            SexEnum.FEMALE.ToString(), Random.Next(1, 123), randomZipCode);

        var updateUserResponse = WriteApiActions.UpdateUserUsingPut(new UpdateUserDto(modifiedUser, userToModify));
        var originalUser = GetUserModels().SingleOrDefault(user => user.Name == userToModify.Name);
        var updatedUser = GetUserModels().SingleOrDefault(user => user.Name == modifiedUser.Name);

        Assert.Multiple(() =>
        {
            Asserts.AssertStatusCode(updateUserResponse, HttpStatusCode.FailedDependency);
            Assert.That(updatedUser, Is.Null);
            Assert.That(originalUser, Is.EqualTo(userToModify));
        });

        /*
             * Bug: Updating user with unavailable Zip Code deletes the original user
             */
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
    public void UpdateUser_WithPatch_NameMissed_Invalid()
    {
        #region Test pre-setup

        CreateUsers(1, true);
        var userToModify = GetUserModels().Last();

        string randomZipCode = StringHelper.GetRandomNumericString(6);
        AddNewZipCodes(randomZipCode);

        #endregion

        var modifiedUser = UserModel.GenerateRandomUser(randomZipCode, Random.Next(1, 124)) with
        {
            Name = null
        };

        var updateUserResponse = WriteApiActions.UpdateUserUsingPatch(new UpdateUserDto(modifiedUser, userToModify));
        var originalUser = GetUserModels().SingleOrDefault(user => user.Name == userToModify.Name);
        var updatedUser = GetUserModels().SingleOrDefault(user => user.ZipCode == modifiedUser.ZipCode);

        Assert.Multiple(() =>
        {
            Asserts.AssertStatusCode(updateUserResponse, HttpStatusCode.Conflict);
            Assert.That(updatedUser, Is.Null);
            Assert.That(originalUser, Is.EqualTo(userToModify));
        });

        /*
             * Bug: Updating user with invalid data deletes the original user
             */
    }

    [Test]
    public void UpdateUser_WithPut_NameMissed_Invalid()
    {
        #region Test pre-setup

        CreateUsers(1, true);
        var userToModify = GetUserModels().Last();

        string randomZipCode = StringHelper.GetRandomNumericString(6);
        AddNewZipCodes(randomZipCode);

        #endregion

        var modifiedUser = UserModel.GenerateRandomUser(randomZipCode, Random.Next(1, 124)) with
        {
            Name = null
        };

        var updateUserResponse = WriteApiActions.UpdateUserUsingPut(new UpdateUserDto(modifiedUser, userToModify));
        var originalUser = GetUserModels().SingleOrDefault(user => user.Name == userToModify.Name);
        var updatedUser = GetUserModels().SingleOrDefault(user => user.ZipCode == modifiedUser.ZipCode);

        Assert.Multiple(() =>
        {
            Asserts.AssertStatusCode(updateUserResponse, HttpStatusCode.Conflict);
            Assert.That(updatedUser, Is.Null);
            Assert.That(originalUser, Is.EqualTo(userToModify));
        });

        /*
             * Bug: Updating user with invalid data deletes the original user
             */
    }

    [Test]
    public void UpdateUser_WithPatch_SexMissed_Invalid()
    {
        #region Test pre-setup

        CreateUsers(1, true);
        var userToModify = GetUserModels().Last();

        string randomZipCode = StringHelper.GetRandomNumericString(6);
        AddNewZipCodes(randomZipCode);

        #endregion

        var modifiedUser = UserModel.GenerateRandomUser(randomZipCode, Random.Next(1, 124)) with
        {
            Sex = null
        };

        var updateUserResponse = WriteApiActions.UpdateUserUsingPatch(new UpdateUserDto(modifiedUser, userToModify));
        var originalUser = GetUserModels().SingleOrDefault(user => user.Name == userToModify.Name);
        var updatedUser = GetUserModels().SingleOrDefault(user => user.Name == modifiedUser.Name);

        Assert.Multiple(() =>
        {
            Asserts.AssertStatusCode(updateUserResponse, HttpStatusCode.Conflict);
            Assert.That(updatedUser, Is.Null);
            Assert.That(originalUser, Is.EqualTo(userToModify));
        });

        /*
             * Bug: Updating user with invalid data deletes the original user
             */
    }
    
    [Test]
    public void UpdateUser_WithPut_SexMissed_Invalid()
    {
        #region Test pre-setup

        CreateUsers(1, true);
        var userToModify = GetUserModels().Last();

        string randomZipCode = StringHelper.GetRandomNumericString(6);
        AddNewZipCodes(randomZipCode);

        #endregion

        var modifiedUser = UserModel.GenerateRandomUser(randomZipCode, Random.Next(1, 124)) with
        {
            Sex = null
        };

        var updateUserResponse = WriteApiActions.UpdateUserUsingPut(new UpdateUserDto(modifiedUser, userToModify));
        var originalUser = GetUserModels().SingleOrDefault(user => user.Name == userToModify.Name);
        var updatedUser = GetUserModels().SingleOrDefault(user => user.Name == modifiedUser.Name);

        Assert.Multiple(() =>
        {
            Asserts.AssertStatusCode(updateUserResponse, HttpStatusCode.Conflict);
            Assert.That(updatedUser, Is.Null);
            Assert.That(originalUser, Is.EqualTo(userToModify));
        });

        /*
             * Bug: Updating user with invalid data deletes the original user
             */
    }
}
