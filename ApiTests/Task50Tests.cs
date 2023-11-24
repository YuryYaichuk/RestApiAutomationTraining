using RestApiAutomationTraining.ApiActions;
using RestApiAutomationTraining.Enums;
using RestApiAutomationTraining.Helpers;
using RestApiAutomationTraining.Models;

namespace RestApiAutomationTraining.ApiTests;

[TestFixture]
public class Task50Tests : BaseApiTest
{
    [Test]
    public void UpdateUser_UpdateAge_Valid()
    {
        #region Test pre-setup

        CreateUsers(1, true);
        var userToModify = ReadApiActions.GetUserModels().Last();

        string randomZipCode = StringHelper.GetRandomNumericString(5);
        AddNewZipCodes(randomZipCode);

        #endregion

        var modifiedUser = new UserModel(
            age: 100,
            name: StringHelper.GenerateName(8),
            sex: SexEnum.FEMALE.ToString(),
            zipCode: randomZipCode);

        var updateUserResponse = WriteApiActions.UpdateUser(new UpdateUserDto(modifiedUser, userToModify));
        var originalUser = ReadApiActions.GetUserModels().SingleOrDefault(user => user.Name == userToModify.Name);
        var updatedUser = ReadApiActions.GetUserModels().SingleOrDefault(user => user.Name == modifiedUser.Name);

        Assert.Multiple(() =>
        {
            AssertWrapper.AssertStatusCode(updateUserResponse, 200);
            Assert.That(originalUser, Is.Null);
            Assert.That(updatedUser, Is.EqualTo(modifiedUser));
        });
    }

    [Test]
    public void UpdateUser_NameMissed_Invalid()
    {
    }
}
