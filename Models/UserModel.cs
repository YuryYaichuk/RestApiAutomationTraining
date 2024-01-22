using RestApiAutomationTraining.Enums;
using RestApiAutomationTraining.Helpers;

namespace RestApiAutomationTraining.Models;

public record UserModel(string? Name, string? Sex, int? Age = null, string? ZipCode = null)
{
    public static UserModel GenerateRandomUser(string? zipCode = null, int? age = null)
    {
        var random = new Random();
        var enumValues = Enum.GetValues(typeof(SexEnum));
        var sex = (SexEnum)enumValues.GetValue(random.Next(0, enumValues.Length));

        return new(StringHelper.GenerateName(7), sex.ToString(), age, zipCode);
    }

    public override string? ToString() =>
        $"Name: [{Name}], Sex: [{Sex}], Age: [{Age}], ZipCode: [{ZipCode}]";
}


public class UpdateUserDto
{
    public UserModel UserNewValues { get; }
    public UserModel UserToChange { get; }

    public UpdateUserDto(UserModel newUser, UserModel userToUpdate)
    { 
        UserNewValues = newUser;
        UserToChange = userToUpdate;
    }
}
