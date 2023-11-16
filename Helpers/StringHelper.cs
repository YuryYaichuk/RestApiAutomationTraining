using System.Text;

namespace RestApiAutomationTraining.Helpers;

public static class StringHelper
{
    private static readonly Random _random = new();

    public static string GenerateName(int len)
    {
        string[] consonants = { "b", "c", "d", "f", "g", "h", "j", "k", "l", "m", "l", "n", "p", "q", "r", "s", "sh", "zh", "t", "v", "w", "x" };
        string[] vowels = { "a", "e", "i", "o", "u", "ae", "y" };
        string Name = "";
        Name += consonants[_random.Next(consonants.Length)].ToUpper();
        Name += vowels[_random.Next(vowels.Length)];
        int b = 2; //b tells how many times a new letter has been added. It's 2 right now because the first two letters are already in the name.
        while (b < len)
        {
            Name += consonants[_random.Next(consonants.Length)];
            b++;
            Name += vowels[_random.Next(vowels.Length)];
            b++;
        }

        return Name;
    }

    public static string GetRandomNumericString(int length)
    {
        const string chars = "0123456789";
        return new string(Enumerable.Repeat(chars, length)
            .Select(s => s[_random.Next(s.Length)]).ToArray());
    }

    public static string GetRandomString(int size, bool lowerCase = false)
    {
        var builder = new StringBuilder(size);

        // Unicode/ASCII Letters are divided into two blocks
        // (Letters 65–90 / 97–122):   
        // The first group containing the uppercase letters and
        // the second group containing the lowercase.  

        // char is a single Unicode character  
        var offset = lowerCase ? 'a' : 'A';
        const int lettersOffset = 26; // A...Z or a..z: length = 26  

        for (var i = 0; i < size; i++)
        {
            var @char = (char)_random.Next(offset, offset + lettersOffset);
            builder.Append(@char);
        }

        return lowerCase ? builder.ToString().ToLower() : builder.ToString();
    }
}
