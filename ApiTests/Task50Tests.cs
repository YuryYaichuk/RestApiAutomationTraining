﻿using NUnit.Allure.Attributes;
using RestApiAutomationTraining.ApiActions;
using RestApiAutomationTraining.Helpers;
using RestApiAutomationTraining.Models;
using System.Net;

namespace RestApiAutomationTraining.ApiTests;

[TestFixture]
public class Task50Tests : BaseApiTest
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
    public void UpdateUser_WithPatch_UpdateAge_Valid()
    {
        #region Test pre-setup

        var userToModify = CreateUsers(1).First();

        string randomZipCode = StringHelper.GetRandomNumericString(5);
        AddNewZipCodes(randomZipCode);

        #endregion

        var modifiedUser = UserModel.GenerateRandomUser(randomZipCode, 100);
        var payLoad = new UpdateUserDto(modifiedUser, userToModify);
        var updateUserResponse = WriteApiActions.UpdateUserUsingPatch(payLoad);

        PayloadCollector.AddPayload(payLoad);

        Assert.Multiple(() =>
        {
            Asserts.AssertStatusCode(updateUserResponse, HttpStatusCode.OK);
            Assert.That(GetUserModels().Where(user => user.Name == userToModify.Name).ToList(), Has.Count.EqualTo(0),
                $"User was not updated [{userToModify}]");
            Assert.That(GetUserModels(), Does.Contain(modifiedUser), $"Updated user was not found: [{modifiedUser}]");
        });
    }

    [Test]
    [AllureName("Test Update User with PUT - Updating Age")]
    [AllureEpic("Task 50")]
    public void UpdateUser_WithPut_UpdateAge_Valid()
    {
        #region Test pre-setup

        var userToModify = CreateUsers(1).First();

        string randomZipCode = StringHelper.GetRandomNumericString(5);
        AddNewZipCodes(randomZipCode);

        #endregion

        var modifiedUser = UserModel.GenerateRandomUser(randomZipCode, 100);
        var payLoad = new UpdateUserDto(modifiedUser, userToModify);
        var updateUserResponse = WriteApiActions.UpdateUserUsingPut(payLoad);

        PayloadCollector.AddPayload(payLoad);

        Assert.Multiple(() =>
        {
            Asserts.AssertStatusCode(updateUserResponse, HttpStatusCode.OK);
            Assert.That(GetUserModels().Where(user => user.Name == userToModify.Name).ToList(), Has.Count.EqualTo(0),
                $"User was not updated [{userToModify}]");
            Assert.That(GetUserModels(), Does.Contain(modifiedUser), $"Updated user was not found: [{modifiedUser}]");
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
    public void UpdateUser_WithPatch_UnavailableZipCode_InValid()
    {
        #region Test pre-setup

        var userToModify = CreateUsers(1).First();
        var unavailableZipCode = StringHelper.GetRandomNumericString(6);

        #endregion

        var modifiedUser = UserModel.GenerateRandomUser(unavailableZipCode, Random.Next(1, 123));
        var payLoad = new UpdateUserDto(modifiedUser, userToModify);
        var updateUserResponse = WriteApiActions.UpdateUserUsingPatch(payLoad);

        PayloadCollector.AddPayload(payLoad);

        Assert.Multiple(() =>
        {
            Asserts.AssertStatusCode(updateUserResponse, HttpStatusCode.FailedDependency);
            Assert.That(GetUserModels(), Does.Contain(userToModify), $"Original user was not found: [{userToModify}]");
            Assert.That(GetUserModels(), Does.Not.Contain(modifiedUser),
                $"User was updated: [{userToModify}] replaced with [{modifiedUser}]");
        });

        // Bug: Updating user with unavailable Zip Code deletes the original user
    }

    [Test]
    [AllureName("Test Update User with PUT - New ZIP Code is Unavailable")]
    [AllureEpic("Task 50")]
    [AllureIssue("Bug: Updating user with unavailable Zip Code deletes the original user")]
    public void UpdateUser_WithPut_UnavailableZipCode_InValid()
    {
        #region Test pre-setup

        var userToModify = CreateUsers(1).First();
        var unavailableZipCode = StringHelper.GetRandomNumericString(6);

        #endregion

        var modifiedUser = UserModel.GenerateRandomUser(unavailableZipCode, Random.Next(1, 123));
        var payLoad = new UpdateUserDto(modifiedUser, userToModify);
        var updateUserResponse = WriteApiActions.UpdateUserUsingPut(payLoad);

        PayloadCollector.AddPayload(payLoad);

        Assert.Multiple(() =>
        {
            Asserts.AssertStatusCode(updateUserResponse, HttpStatusCode.FailedDependency);
            Assert.That(GetUserModels(), Does.Contain(userToModify), $"Original user was not found: [{userToModify}]");
            Assert.That(GetUserModels(), Does.Not.Contain(modifiedUser),
                $"User was updated: [{userToModify}] replaced with [{modifiedUser}]");
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
    public void UpdateUser_WithPatch_NameMissed_Invalid()
    {
        #region Test pre-setup

        var userToModify = CreateUsers(1).First();

        var randomZipCode = StringHelper.GetRandomNumericString(6);
        AddNewZipCodes(randomZipCode);

        #endregion

        var modifiedUser = UserModel.GenerateRandomUser(randomZipCode, Random.Next(1, 124)) with
        {
            Name = null
        };

        var payLoad = new UpdateUserDto(modifiedUser, userToModify);
        var updateUserResponse = WriteApiActions.UpdateUserUsingPatch(payLoad);

        PayloadCollector.AddPayload(payLoad);

        Assert.Multiple(() =>
        {
            Asserts.AssertStatusCode(updateUserResponse, HttpStatusCode.Conflict);
            Assert.That(GetUserModels(), Does.Contain(userToModify), $"Original user was not found: [{userToModify}]");
            Assert.That(GetUserModels(), Does.Not.Contain(modifiedUser),
                $"User was updated: [{userToModify}] replaced with [{modifiedUser}]");
        });

        // Bug: Updating user with invalid data deletes the original user
    }

    [Test]
    [AllureName("Test Update User with PUT - Name is Missing")]
    [AllureEpic("Task 50")]
    [AllureIssue("Bug: Updating user with invalid data deletes the original user")]
    public void UpdateUser_WithPut_NameMissed_Invalid()
    {
        #region Test pre-setup

        var userToModify = CreateUsers(1).First();

        var randomZipCode = StringHelper.GetRandomNumericString(6);
        AddNewZipCodes(randomZipCode);

        #endregion

        var modifiedUser = UserModel.GenerateRandomUser(randomZipCode, Random.Next(1, 124)) with
        {
            Name = null
        };

        var payLoad = new UpdateUserDto(modifiedUser, userToModify);
        var updateUserResponse = WriteApiActions.UpdateUserUsingPut(payLoad);

        PayloadCollector.AddPayload(payLoad);

        Assert.Multiple(() =>
        {
            Asserts.AssertStatusCode(updateUserResponse, HttpStatusCode.Conflict);
            Assert.That(GetUserModels(), Does.Contain(userToModify), $"Original user was not found: [{userToModify}]");
            Assert.That(GetUserModels(), Does.Not.Contain(modifiedUser),
                $"User was updated: [{userToModify}] replaced with [{modifiedUser}]");
        });

        // Bug: Updating user with invalid data deletes the original user
    }

    [Test]
    [AllureName("Test Update User with PATCH - Sex is Missing")]
    [AllureEpic("Task 50")]
    [AllureIssue("Bug: Updating user with invalid data deletes the original user")]
    public void UpdateUser_WithPatch_SexMissed_Invalid()
    {
        #region Test pre-setup

        var userToModify = CreateUsers(1).First();

        var randomZipCode = StringHelper.GetRandomNumericString(6);
        AddNewZipCodes(randomZipCode);

        #endregion

        var modifiedUser = UserModel.GenerateRandomUser(randomZipCode, Random.Next(1, 124)) with
        {
            Sex = null
        };

        var payLoad = new UpdateUserDto(modifiedUser, userToModify);
        var updateUserResponse = WriteApiActions.UpdateUserUsingPatch(payLoad);

        PayloadCollector.AddPayload(payLoad);

        Assert.Multiple(() =>
        {
            Asserts.AssertStatusCode(updateUserResponse, HttpStatusCode.Conflict);
            Assert.That(GetUserModels(), Does.Contain(userToModify), $"Original user was not found: [{userToModify}]");
            Assert.That(GetUserModels(), Does.Not.Contain(modifiedUser),
                $"User was updated: [{userToModify}] replaced with [{modifiedUser}]");
        });

        // Bug: Updating user with invalid data deletes the original user
    }

    [Test]
    [AllureName("Test Update User with PUT - Sex is Missing")]
    [AllureEpic("Task 50")]
    [AllureIssue("Bug: Updating user with invalid data deletes the original user")]
    public void UpdateUser_WithPut_SexMissed_Invalid()
    {
        #region Test pre-setup

        var userToModify = CreateUsers(1).First();

        var randomZipCode = StringHelper.GetRandomNumericString(6);
        AddNewZipCodes(randomZipCode);

        #endregion

        var modifiedUser = UserModel.GenerateRandomUser(randomZipCode, Random.Next(1, 124)) with
        {
            Sex = null
        };

        var payLoad = new UpdateUserDto(modifiedUser, userToModify);
        var updateUserResponse = WriteApiActions.UpdateUserUsingPut(payLoad);

        PayloadCollector.AddPayload(payLoad);

        Assert.Multiple(() =>
        {
            Asserts.AssertStatusCode(updateUserResponse, HttpStatusCode.Conflict);
            Assert.That(GetUserModels(), Does.Contain(userToModify), $"Original user was not found: [{userToModify}]");
            Assert.That(GetUserModels(), Does.Not.Contain(modifiedUser),
                $"User was updated: [{userToModify}] replaced with [{modifiedUser}]");
        });

        // Bug: Updating user with invalid data deletes the original user
    }
}
