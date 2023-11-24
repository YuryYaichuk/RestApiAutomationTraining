using RestApiAutomationTraining.ApiActions;
using RestApiAutomationTraining.Enums;
using RestApiAutomationTraining.Helpers;
using RestApiAutomationTraining.Models;

namespace RestApiAutomationTraining.ApiTests;

[TestFixture]
public class Task40Tests : BaseApiTest
{
    [Test]
    public void GetUsers_Valid()
    {
        #region Test pre-setup

        const int extraUserNumber = 3;
        var response = ReadApiActions.GetUsers();
        var users = JsonHelper.DeserializeObject<List<UserModel>>(response);
        var initialUsersCount = users.Count;
        CreateUsers(extraUserNumber);

        #endregion

        response = ReadApiActions.GetUsers();
        var actualUserList = JsonHelper.DeserializeObject<List<UserModel>>(response);

        Assert.Multiple(() =>
        {
            Assert.That((int)response.StatusCode, Is.EqualTo(200));
            Assert.That(actualUserList, Has.Count.GreaterThanOrEqualTo(initialUsersCount + extraUserNumber));
        });
    }

    [Test]
    public void GetUsers_FilteredByOlderThan_Valid()
    {
        const int ageLimit = 12;
        const string paramName = "olderThan";

        #region Test pre-setup

        CreateUsers(3, true);

        var response = ReadApiActions.GetUsers();
        var users = JsonHelper.DeserializeObject<List<UserModel>>(response);
        var expectedUserCount = users.Count(_ => _.Age > ageLimit && _.Age != null);

        #endregion

        response = ReadApiActions.GetUsers((paramName, ageLimit.ToString()));
        var actualUsers = JsonHelper.DeserializeObject<List<UserModel>>(response);

        Assert.Multiple(() =>
        {
            Assert.That((int)response.StatusCode, Is.EqualTo(200));
            Assert.That(actualUsers, Has.Count.EqualTo(expectedUserCount));
        });
    }

    [Test]
    public void GetUsers_FilteredByYongerThan_Valid()
    {
        const int ageLimit = 60;
        const string paramName = "yongerThan";

        #region Test pre-setup

        CreateUsers(3, true);

        var response = ReadApiActions.GetUsers();
        var users = JsonHelper.DeserializeObject<List<UserModel>>(response);
        var expectedUserCount = users.Count(_ => _.Age < ageLimit && _.Age != null);

        #endregion

        response = ReadApiActions.GetUsers((paramName, ageLimit.ToString()));
        var actualUsers = JsonHelper.DeserializeObject<List<UserModel>>(response);

        Assert.Multiple(() =>
        {
            AssertWrapper.AssertStatusCode(response, 200);
            Assert.That(actualUsers, Has.Count.EqualTo(expectedUserCount));
        });

        /*
             * Bug: Filtering by 'yongerThan' does not work
             * 
             * Error Message: 
             * Expected: property Count equal to 18
             * But was:  56
             */
    }

    [Test]
    public void GetUsers_FilteredBySex_Valid()
    {
        var filterValue = SexEnum.FEMALE.ToString();
        const string paramName = "sex";

        #region Test pre-setup

        CreateUsers(3);

        var response = ReadApiActions.GetUsers();
        var users = JsonHelper.DeserializeObject<List<UserModel>>(response);
        var expectedUserCount = users.Count(_ => _.Sex == filterValue);

        #endregion

        response = ReadApiActions.GetUsers((paramName, filterValue));
        var actualUsers = JsonHelper.DeserializeObject<List<UserModel>>(response);

        Assert.Multiple(() =>
        {
            Assert.That((int)response.StatusCode, Is.EqualTo(200));
            Assert.That(actualUsers, Has.Count.EqualTo(expectedUserCount));
        });
    }
}
