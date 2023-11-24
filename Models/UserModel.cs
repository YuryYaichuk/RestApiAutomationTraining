using RestApiAutomationTraining.Enums;
using RestApiAutomationTraining.Helpers;

namespace RestApiAutomationTraining.Models;

public class UserModel
{
    public int? Age { get; set; }
    public string Name { get; set; }
    public string Sex { get; set; }
    public string? ZipCode { get; set; }

    public UserModel(int? age, string name, string sex, string? zipCode)
    {
        Age = age;
        Name = name;
        Sex = sex;
        ZipCode = zipCode;
    }

    public static UserModel GenerateRandomUser(string? zipCode = null, int? age = null)
    {
        var random = new Random();
        var enumValues = Enum.GetValues(typeof(SexEnum));
        var sex = (SexEnum)enumValues.GetValue(random.Next(0, enumValues.Length));

        return new(age, StringHelper.GenerateName(7),
            sex: sex.ToString(),
            zipCode: zipCode);
    }

    public override string ToString() =>
        $"Name: [{Name}], Sex: [{Sex}], Age: [{Age}], ZipCode: [{ZipCode}]";

    public override bool Equals(object? obj)
    {
        if (obj == null || obj is not UserModel) return false;

        var incomingObj = (UserModel)obj;
        return Name == incomingObj.Name &&
            Sex == incomingObj.Sex &&
            Age == incomingObj.Age &&
            ZipCode == incomingObj.ZipCode;
    }
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
