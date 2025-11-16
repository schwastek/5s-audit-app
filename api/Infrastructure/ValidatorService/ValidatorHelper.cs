using System;
using System.Collections.Generic;

namespace Infrastructure.ValidatorService;

public static class ValidatorHelper
{
    public static bool IsEmpty(string? value)
    {
        return string.IsNullOrWhiteSpace(value);
    }

    public static bool IsEmpty(Guid? value)
    {
        return value is null || value == Guid.Empty;
    }

    public static bool IsEmpty<T>(ICollection<T>? collection)
    {
        if (collection is null) return true;

        return collection.Count == 0;
    }

    public static bool IsEmpty<T>(IEnumerable<T>? enumerable)
    {
        if (enumerable is null) return true;

        var enumerator = enumerable.GetEnumerator();

        using (enumerator as IDisposable)
        {
            return !enumerator.MoveNext();
        }
    }

    public static bool IsEmpty(DateTime? value)
    {
        return value is null || value == default(DateTime);
    }

    public static bool IsEmailAddress(string? value)
    {
        if (string.IsNullOrWhiteSpace(value)) return false;

        int atIndex = value.IndexOf('@');
        int lastAtIndex = value.LastIndexOf('@');

        // Valid if there is only one '@' character, and it is neither the first nor the last character.
        bool hasExactlyOneAt = atIndex == lastAtIndex;
        bool atIsNotFirst = atIndex > 0;
        bool atIsNotLast = atIndex != value.Length - 1;

        return hasExactlyOneAt && atIsNotFirst && atIsNotLast;
    }
}
