using Features.Core.ValidatorService;
using System;
using System.Collections.Generic;
using Xunit;

namespace UnitTests.ValidatorService;

public sealed class ValidatorHelperTests
{
    [Fact]
    public void IsEmpty_ReturnsTrue_ForNullString()
    {
        string? value = null;
        bool result = ValidatorHelper.IsEmpty(value);

        Assert.True(result);
    }

    [Fact]
    public void IsEmpty_ReturnsTrue_ForEmptyString()
    {
        string value = "";
        bool result = ValidatorHelper.IsEmpty(value);

        Assert.True(result);
    }

    [Fact]
    public void IsEmpty_ReturnsFalse_ForValidString()
    {
        string value = "Lorem ipsum";
        bool result = ValidatorHelper.IsEmpty(value);

        Assert.False(result);
    }

    [Fact]
    public void IsEmpty_ReturnsTrue_ForNullGuid()
    {
        Guid? value = null;
        bool result = ValidatorHelper.IsEmpty(value);

        Assert.True(result);
    }

    [Fact]
    public void IsEmpty_ReturnsTrue_ForEmptyGuid()
    {
        Guid value = Guid.Empty;
        bool result = ValidatorHelper.IsEmpty(value);

        Assert.True(result);
    }

    [Fact]
    public void IsEmpty_ReturnsFalse_ForValidGuid()
    {
        Guid value = Guid.NewGuid();
        bool result = ValidatorHelper.IsEmpty(value);

        Assert.False(result);
    }

    [Fact]
    public void IsEmpty_ReturnsTrue_ForNullCollection()
    {
        List<string>? collection = null;
        bool result = ValidatorHelper.IsEmpty(collection);

        Assert.True(result);
    }

    [Fact]
    public void IsEmpty_ReturnsTrue_ForEmptyCollection()
    {
        List<string> collection = [];
        bool result = ValidatorHelper.IsEmpty(collection);

        Assert.True(result);
    }

    [Fact]
    public void IsEmpty_ReturnsFalse_ForValidCollection()
    {
        List<string> collection = ["Lorem ipsum"];
        bool result = ValidatorHelper.IsEmpty(collection);

        Assert.False(result);
    }

    [Fact]
    public void IsEmpty_ReturnsTrue_ForNullEnumerable()
    {
        IEnumerable<string>? collection = null;
        bool result = ValidatorHelper.IsEmpty(collection);

        Assert.True(result);
    }

    [Fact]
    public void IsEmpty_ReturnsTrue_ForEmptyEnumerable()
    {
        IEnumerable<string> collection = [];
        bool result = ValidatorHelper.IsEmpty(collection);

        Assert.True(result);
    }

    [Fact]
    public void IsEmpty_ReturnsFalse_ForValidEnumerable()
    {
        IEnumerable<string> collection = ["Lorem ipsum"];
        bool result = ValidatorHelper.IsEmpty(collection);

        Assert.False(result);
    }

    [Fact]
    public void IsEmpty_ReturnsTrue_ForNullDateTime()
    {
        DateTime? value = null;
        bool result = ValidatorHelper.IsEmpty(value);

        Assert.True(result);
    }

    [Fact]
    public void IsEmpty_ReturnsTrue_ForDefaultDateTime()
    {
        DateTime value = default;
        bool result = ValidatorHelper.IsEmpty(value);

        Assert.True(result);
    }

    [Fact]
    public void IsEmpty_ReturnsFalse_ForValidDateTime()
    {
        DateTime value = DateTime.UtcNow;
        bool result = ValidatorHelper.IsEmpty(value);

        Assert.False(result);
    }

    [Fact]
    public void IsEmailAddress_ReturnsFalse_ForNullEmail()
    {
        string? value = null;
        bool result = ValidatorHelper.IsEmailAddress(value);

        Assert.False(result);
    }

    [Fact]
    public void IsEmailAddress_ReturnsFalse_ForInvalidEmail_NoAtSign()
    {
        string value = "name(at)domain.com";
        bool result = ValidatorHelper.IsEmailAddress(value);

        Assert.False(result);
    }

    [Fact]
    public void IsEmailAddress_ReturnsFalse_ForInvalidEmail_AtSignIsFirst()
    {
        string value = "@domain.com";
        bool result = ValidatorHelper.IsEmailAddress(value);

        Assert.False(result);
    }

    [Fact]
    public void IsEmailAddress_ReturnsFalse_ForInvalidEmail_AtSignIsLast()
    {
        string value = "name@";
        bool result = ValidatorHelper.IsEmailAddress(value);

        Assert.False(result);
    }

    [Fact]
    public void IsEmailAddress_ReturnsFalse_ForInvalidEmail_MultipleAtSigns()
    {
        string value = "name@@domain.com";
        bool result = ValidatorHelper.IsEmailAddress(value);

        Assert.False(result);
    }

    [Fact]
    public void IsEmailAddress_ReturnsTrue_ForValidEmail()
    {
        string value = "name@domain.com";
        bool result = ValidatorHelper.IsEmailAddress(value);

        Assert.True(result);
    }
}
