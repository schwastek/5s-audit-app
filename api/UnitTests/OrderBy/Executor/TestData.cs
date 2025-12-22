using System.Collections.Generic;
using System.Linq;

namespace UnitTests.OrderBy.Executor;

internal class Person
{
    public int Id { get; set; }
    public string FirstName { get; set; } = string.Empty;
    public int Age { get; set; }
}

internal static class TestData
{
    public static IQueryable<Person> People => new List<Person>
    {
        new Person { Id = 1, FirstName = "Charlie", Age = 30 },
        new Person { Id = 2, FirstName = "Alice",   Age = 30 },
        new Person { Id = 3, FirstName = "Bob",     Age = 25 },
        new Person { Id = 4, FirstName = "Bob",     Age = 40 }
    }.AsQueryable();
}
