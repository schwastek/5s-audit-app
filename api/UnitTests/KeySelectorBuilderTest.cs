using Infrastructure;
using System;
using Xunit;

namespace UnitTests;

public sealed class When_building_key_selector
{
    private class Person
    {
        public string FirstName { get; set; } = string.Empty;
        public int Age { get; set; }
    }

    [Fact]
    public void Then_correct_string_should_be_returned()
    {
        // Arrange
        var person = new Person { FirstName = "Charlie" };

        // Act
        var lambda = KeySelectorBuilder.BuildKeySelector<Person>(nameof(Person.FirstName));
        var selector = lambda.Compile();
        var result = selector.DynamicInvoke(person) as string;

        // Assert
        Assert.Equal("Charlie", result);
    }

    [Fact]
    public void Then_correct_int_should_be_returned()
    {
        // Arrange
        var person = new Person { Age = 30 };

        // Act
        var lambda = KeySelectorBuilder.BuildKeySelector<Person>(nameof(Person.Age));
        var selector = lambda.Compile();
        var result = selector.DynamicInvoke(person) as int?;

        // Assert
        Assert.Equal(30, result);
    }

    [Fact]
    public void Then_error_is_thrown_for_invalid_property()
    {
        // Arrange
        var action = () => KeySelectorBuilder.BuildKeySelector<Person>("NonExistentProperty");

        // Act
        var ex = Assert.Throws<ArgumentException>(action);

        // Assert
        Assert.Contains("NonExistentProperty", ex.Message);
    }
}
