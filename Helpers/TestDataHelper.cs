using RestApiAutomationTraining.Models;

namespace RestApiAutomationTraining.Helpers;

public class TestDataHelper
{
    public static List<UserModel> CreateUsers(int numberOfUsers, bool addAge = false)
    {
        var newUsers = new List<UserModel>();
        var random = new Random();
        int? age = null;

        for (int i = 0; i < numberOfUsers; i++)
        {
            if (addAge) age = random.Next(1, 124);
            var randomUser = UserModel.GenerateRandomUser(age: age);
            ApiActions.CreateUser(randomUser);
            newUsers.Add(randomUser);
            Thread.Sleep(1000);
        }

        return newUsers;
    }
}
