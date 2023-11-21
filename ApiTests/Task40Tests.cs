using RestApiAutomationTraining.Enums;
using RestApiAutomationTraining.Helpers;
using RestApiAutomationTraining.Models;

namespace RestApiAutomationTraining.ApiTests;

public class Task40Tests
{
    [Test]
    public void GetUsers_Valid()
    {
        #region Test pre-setup

        TestDataHelper.CreateUsers(3);

        #endregion

        var response = ApiActions.GetUsers();
        var users = JsonHelper.DeserializeObject<List<UserModel>>(response);

        Assert.Multiple(() =>
        {
            Assert.That((int)response.StatusCode, Is.EqualTo(200));
            Assert.That(users, Has.Count.GreaterThanOrEqualTo(3));
        });
    }

    [Test]
    public void GetUsers_FilteredByOlderThan_Valid()
    {
        #region Test pre-setup

        TestDataHelper.CreateUsers(3, true);

        #endregion

        var response = ApiActions.GetUsers(("olderThan", "1"));
        var users = JsonHelper.DeserializeObject<List<UserModel>>(response);

        Assert.Multiple(() =>
        {
            Assert.That((int)response.StatusCode, Is.EqualTo(200));
            Assert.That(users, Has.Count.GreaterThanOrEqualTo(3));
        });
    }

    [Test]
    public void GetUsers_FilteredByYongerThan_Valid()
    {
        #region Test pre-setup

        TestDataHelper.CreateUsers(3, true);

        #endregion

        var response = ApiActions.GetUsers(("yongerThan", "124"));
        var users = JsonHelper.DeserializeObject<List<UserModel>>(response);

        Assert.Multiple(() =>
        {
            Assert.That((int)response.StatusCode, Is.EqualTo(200));
            Assert.That(users, Has.Count.GreaterThanOrEqualTo(3));
        });
    }

    [Test]
    public void GetUsers_FilteredBySex_Valid()
    {
        #region Test pre-setup

        TestDataHelper.CreateUsers(3);

        #endregion

        var response = ApiActions.GetUsers(("sex", SexEnum.FEMALE.ToString()));
        var users = JsonHelper.DeserializeObject<List<UserModel>>(response);

        Assert.Multiple(() =>
        {
            Assert.That((int)response.StatusCode, Is.EqualTo(200));
            Assert.That(users, Has.Count.GreaterThanOrEqualTo(3));
        });
    }
}
