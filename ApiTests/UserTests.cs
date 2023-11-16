using RestApiAutomationTraining.Helpers;
using RestApiAutomationTraining.Models;

namespace RestApiAutomationTraining.ApiTests;

public class UserTests
{
    [Test]
    public void GetUsers_Valid()
    {
        var response = ApiActions.GetUsers();

        Assert.That((int)response.StatusCode, Is.EqualTo(200));
    }

    [Test]
    public void CreateUser_AllFieldsPopulated_Valid()
    {
        string randomZipCode;
        var zipCodes = JsonHelper.DeserializeObject<List<string>>(ApiActions.GetZipCodes());

        if(zipCodes.Count > 0 )
        {
            randomZipCode = zipCodes.First();
        }
        else
        {
            ApiActions.CreateZipCodes(StringHelper.GenerateName(5).ToUpper());
            randomZipCode = JsonHelper.DeserializeObject<List<string>>(ApiActions.GetZipCodes()).First();
        }

        var randomUser = UserModel.GenerateRandomUser(randomZipCode, new Random().Next(1, 124));
        var response = ApiActions.CreateUser(randomUser);
        var users = JsonHelper.DeserializeObject<List<UserModel>>(ApiActions.GetUsers());
        zipCodes = JsonHelper.DeserializeObject<List<string>>(ApiActions.GetZipCodes());

        Assert.Multiple(() =>
        {
            Assert.That((int)response.StatusCode, Is.EqualTo(201));
            Assert.That(users.SingleOrDefault(_ =>
                _.Age == randomUser.Age &&
                _.Name == randomUser.Name &&
                _.ZipCode == randomUser.ZipCode), Is.Not.Null
            );
            Assert.That(zipCodes, Does.Not.Member(randomUser.ZipCode));
        });
    }

    [Test]
    public void CreateUser_RequiredFieldsOnlyPopulated_Valid()
    {
        var randomUser = UserModel.GenerateRandomUser();
        var response = ApiActions.CreateUser(randomUser);
        var users = JsonHelper.DeserializeObject<List<UserModel>>(ApiActions.GetUsers());

        Assert.Multiple(() =>
        {
            Assert.That((int)response.StatusCode, Is.EqualTo(201));
            Assert.That(users.SingleOrDefault(_ => _.Name == randomUser.Name), Is.Not.Null);
        });
    }

    [Test]
    public void CreateUser_NotExistingZipCode_Invalid()
    {
        var randomUser = UserModel.GenerateRandomUser(
            existingZipCode: StringHelper.GenerateName(5).ToUpper(),
            age: new Random().Next(1, 124));
        var response = ApiActions.CreateUser(randomUser);
        var users = JsonHelper.DeserializeObject<List<UserModel>>(ApiActions.GetUsers());

        Assert.Multiple(() =>
        {
            Assert.That((int)response.StatusCode, Is.EqualTo(424));
            Assert.That(users.SingleOrDefault(_ =>
                _.Age == randomUser.Age &&
                _.Name == randomUser.Name &&
                _.ZipCode == randomUser.ZipCode), Is.Null
            );
        });
    }

    [Test]
    public void CreateUser_UserAlreadyExists_Invalid()
    {
        UserModel randomUser = UserModel.GenerateRandomUser();
        ApiActions.CreateUser(randomUser);

        var response = ApiActions.CreateUser(randomUser);
        var users = JsonHelper.DeserializeObject<List<UserModel>>(ApiActions.GetUsers());

        Assert.Multiple(() =>
        {
            Assert.That((int)response.StatusCode, Is.EqualTo(400));
            Assert.That(users.Where(_ => _.Name == randomUser.Name).ToList(), Has.Count.EqualTo(1));
        });
    }
}
