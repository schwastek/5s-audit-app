using System;
using System.Text;

namespace UnitTests.Helpers;

public static class RandomStringGenerator
{
    private const string _alphanumericCharacters = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
    private static readonly Random _random = new();

    public static string Generate(int length = 6)
    {
        if (length <= 0) throw new ArgumentOutOfRangeException(nameof(length), "Length must be greater than 0.");

        var result = new StringBuilder(length);

        for (int i = 0; i < length; i++)
        {
            var index = _random.Next(_alphanumericCharacters.Length);
            result.Append(_alphanumericCharacters[index]);
        }

        return result.ToString();
    }
}
