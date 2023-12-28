using RestApiAutomationTraining.Helpers;
using System.Net;

namespace RestApiAutomationTraining.ApiTests;

public class Task60Tests : BaseApiTest
{
    [SetUp]
    protected void TestSetup()
    {
        ClearUsers();
    }

    //Scenario #1         
    //Given I am authorized user
    //When I send DELETE request to /users endpoint
    //And Request body contains user to delete
    //Then I get 204 response code
    //And User is deleted
    //And Zip code is returned in list of available zip codes
    public void DeleteUser_UserDeleted_Valid()
    {
        #region Test pre-setup

        CreateUsers(1, true);
        var userToDelete = GetUserModels().First();

        #endregion

        var deleteUserResponse = WriteApiActions.DeleteUser(userToDelete);

        Assert.Multiple(() =>
        {
            Asserts.AssertStatusCode(deleteUserResponse, HttpStatusCode.NoContent);
            Assert.That(GetUserModels().Any(u => u.Name == userToDelete.Name), Is.False);
            Assert.That(GetZipCodesModel().Single(zip => zip == userToDelete.ZipCode), Is.Not.Null);
        });
    }

    /*
     * Scenario #2
     * Given I am authorized user
     * When I send DELETE request to /users endpoint
     * And Request body contains user to delete(required fields only)
     * Then I get 204 response code
     * And User is deleted
     * And Zip code is returned in list of available zip codes
    */
    public void DeleteUser_UserDeleted_Valid()
    {
        #region Test pre-setup

        CreateUsers(1, true);
        var userToDelete = GetUserModels().First();

        #endregion

        var deleteUserResponse = WriteApiActions.DeleteUser(userToDelete);

        Assert.Multiple(() =>
        {
            Asserts.AssertStatusCode(deleteUserResponse, HttpStatusCode.NoContent);
            Assert.That(GetUserModels().Any(u => u.Name == userToDelete.Name), Is.False);
            Assert.That(GetZipCodesModel().Single(zip => zip == userToDelete.ZipCode), Is.Not.Null);
        });
    }

    /*
     * Scenario #3
     * Given I am authorized user
     * When I send DELETE request to /users endpoint
     * And Request body contains user to delete(any required field is missed)
     * Then I get 409 response code
     * And User is not deleted
     */
    public void DeleteUser_UserDeleted_Valid()
    {
        #region Test pre-setup

        CreateUsers(1, true);
        var userToDelete = GetUserModels().First();

        #endregion

        var deleteUserResponse = WriteApiActions.DeleteUser(userToDelete);

        Assert.Multiple(() =>
        {
            Asserts.AssertStatusCode(deleteUserResponse, HttpStatusCode.NoContent);
            Assert.That(GetUserModels().Any(u => u.Name == userToDelete.Name), Is.False);
            Assert.That(GetZipCodesModel().Single(zip => zip == userToDelete.ZipCode), Is.Not.Null);
        });
    }
}
