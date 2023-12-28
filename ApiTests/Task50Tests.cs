using RestApiAutomationTraining.ApiActions;
using RestApiAutomationTraining.Enums;
using RestApiAutomationTraining.Helpers;
using RestApiAutomationTraining.Models;

namespace RestApiAutomationTraining.ApiTests;

[TestFixture]
public class Task50Tests : BaseApiTest
{
    [SetUp]
    protected void TestSetup()
    {
        ClearUsers();
    }

    [Test]
    public void UpdateUser_UpdateAge_Valid()
    {
        #region Test pre-setup

        CreateUsers(1, true);
        var userToModify = GetUserModels().Last();

        string randomZipCode = StringHelper.GetRandomNumericString(5);
        AddNewZipCodes(randomZipCode);

        #endregion

        var modifiedUser = new UserModel(
            age: 100,
            name: StringHelper.GenerateName(8),
            sex: SexEnum.FEMALE.ToString(),
            zipCode: randomZipCode);

        var updateUserResponse = WriteApiActions.UpdateUser(new UpdateUserDto(modifiedUser, userToModify));
        var originalUser = GetUserModels().SingleOrDefault(user => user.Name == userToModify.Name);
        var updatedUser = GetUserModels().SingleOrDefault(user => user.Name == modifiedUser.Name);

        Assert.Multiple(() =>
        {
            Asserts.AssertStatusCode(updateUserResponse, 200);
            Assert.That(originalUser, Is.Null);
            Assert.That(updatedUser, Is.EqualTo(modifiedUser));
        });
    }

    [Test]
    public void UpdateUser_UnavailableZipCode_InValid()
    {
        #region Test pre-setup

        CreateUsers(1, true);
        var userToModify = GetUserModels().Last();

        string randomZipCode = StringHelper.GetRandomNumericString(6);

        #endregion

        var modifiedUser = new UserModel(
            age: Random.Next(1, 123),
            name: StringHelper.GenerateName(8),
            sex: SexEnum.FEMALE.ToString(),
            zipCode: randomZipCode);

        var updateUserResponse = WriteApiActions.UpdateUser(new UpdateUserDto(modifiedUser, userToModify));
        var originalUser = GetUserModels().SingleOrDefault(user => user.Name == userToModify.Name);
        var updatedUser = GetUserModels().SingleOrDefault(user => user.Name == modifiedUser.Name);

        Assert.Multiple(() =>
        {
            Asserts.AssertStatusCode(updateUserResponse, 424);
            Assert.That(updatedUser, Is.Null);
            Assert.That(originalUser, Is.EqualTo(userToModify));
        });

        /*
             * Bug: Updating user with unavailable Zip Code deletes the original user
             */
    }

    [Test]
    public void UpdateUser_NameMissed_Invalid()
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

        var updateUserResponse = WriteApiActions.UpdateUser(new UpdateUserDto(modifiedUser, userToModify));
        var originalUser = GetUserModels().SingleOrDefault(user => user.Name == userToModify.Name);
        var updatedUser = GetUserModels().SingleOrDefault(user => user.ZipCode == modifiedUser.ZipCode);

        Assert.Multiple(() =>
        {
            Asserts.AssertStatusCode(updateUserResponse, 409);
            Assert.That(updatedUser, Is.Null);
            Assert.That(originalUser, Is.EqualTo(userToModify));
        });

        /*
             * Bug: Updating user with invalid data deletes the original user
             */
    }

    [Test]
    public void UpdateUser_SexMissed_Invalid()
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

        var updateUserResponse = WriteApiActions.UpdateUser(new UpdateUserDto(modifiedUser, userToModify));
        var originalUser = GetUserModels().SingleOrDefault(user => user.Name == userToModify.Name);
        var updatedUser = GetUserModels().SingleOrDefault(user => user.Name == modifiedUser.Name);

        Assert.Multiple(() =>
        {
            Asserts.AssertStatusCode(updateUserResponse, 409);
            Assert.That(updatedUser, Is.Null);
            Assert.That(originalUser, Is.EqualTo(userToModify));
        });

        /*
             * Bug: Updating user with invalid data deletes the original user
             */
    }
}
