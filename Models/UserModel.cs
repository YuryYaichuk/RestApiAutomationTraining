using RestApiAutomationTraining.Enums;
using RestApiAutomationTraining.Helpers;
namespace RestApiAutomationTraining.Models;

public class UserModel
{
    public int? Age { get; set; }
    public string Name { get; set; }
    public SexEnum Sex { get; set; }
    public string? ZipCode { get; set; }

    public UserModel(int? age, string name, SexEnum sex, string? zipCode)
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

        return new(age, StringHelper.GenerateName(7),
            sex: (SexEnum)enumValues.GetValue(random.Next(0, enumValues.Length)),
            zipCode: zipCode);
    }
}
