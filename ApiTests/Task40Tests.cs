using RestApiAutomationTraining.ApiActions;
using RestApiAutomationTraining.Enums;
using RestApiAutomationTraining.Helpers;
using RestApiAutomationTraining.Models;
using System.Net;

namespace RestApiAutomationTraining.ApiTests;

[TestFixture]
public class Task40Tests : BaseApiTest
{
    [Test]
    public void GetUsers_Valid()
    {
        #region Test pre-setup

        const int extraUserNumber = 3;
        var initialUsersCount = GetUserModels().Count;

        CreateUsers(extraUserNumber);

        #endregion

        var response = ReadApiActions.GetUsers();
        var actualUserList = response.ToModel<List<UserModel>>();

        Assert.Multiple(() =>
        {
            Asserts.AssertStatusCode(response, HttpStatusCode.OK);
            Assert.That(actualUserList, Has.Count.GreaterThanOrEqualTo(initialUsersCount + extraUserNumber));
        });
    }

    [Test]
    public void GetUsers_FilteredByOlderThan_Valid()
    {
        const int ageLimit = 12;
        const string paramName = "olderThan";

        #region Test pre-setup

        CreateUsers(3);

        var expectedUserCount = GetUserModels()
            .Count(_ => _.Age > ageLimit && _.Age != null);

        #endregion

        var response = ReadApiActions.GetUsers((paramName, ageLimit.ToString()));
        var actualUsers = response.ToModel<List<UserModel>>();

        Assert.Multiple(() =>
        {
            Asserts.AssertStatusCode(response, HttpStatusCode.OK);
            Assert.That(actualUsers, Has.Count.EqualTo(expectedUserCount));
        });
    }

    [Test]
    public void GetUsers_FilteredByYoungerThan_Valid()
    {
        const int ageLimit = 60;
        const string paramName = "youngerThan";

        #region Test pre-setup

        CreateUsers(3);

        var expectedUserCount = GetUserModels()
            .Count(_ => _.Age < ageLimit && _.Age != null);

        #endregion

        var response = ReadApiActions.GetUsers((paramName, ageLimit.ToString()));
        var actualUsers = response.ToModel<List<UserModel>>();

        Assert.Multiple(() =>
        {
            Asserts.AssertStatusCode(response, HttpStatusCode.OK);
            Assert.That(actualUsers, Has.Count.EqualTo(expectedUserCount));
        });
    }

    [Test]
    public void GetUsers_FilteredBySex_Valid()
    {
        var filterValue = SexEnum.FEMALE.ToString();
        const string paramName = "sex";

        #region Test pre-setup

        CreateUsers(3);

        var expectedUserCount = GetUserModels().Count(_ => _.Sex == filterValue);

        #endregion

        var response = ReadApiActions.GetUsers((paramName, filterValue));
        var actualUsers = response.ToModel<List<UserModel>>();

        Assert.Multiple(() =>
        {
            Asserts.AssertStatusCode(response, HttpStatusCode.OK);
            Assert.That(actualUsers, Has.Count.EqualTo(expectedUserCount));
        });
    }
}
