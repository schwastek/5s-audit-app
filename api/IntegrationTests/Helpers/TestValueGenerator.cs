using System;
using System.Security.Cryptography;

namespace IntegrationTests.Helpers;

public static class TestValueGenerator
{
    public static int GenerateInt(int minValue = 0, int maxValue = 5)
    {
        var random = new Random();
        // Add 1 to make the upper bound inclusive.
        var result = random.Next(minValue, maxValue + 1);

        return result;
    }

    public static string GenerateString(int length = 10)
    {
        var possibleCharacters = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";

        var result = RandomNumberGenerator.GetString(
            choices: possibleCharacters,
            length: length
        );

        return result;
    }
}
