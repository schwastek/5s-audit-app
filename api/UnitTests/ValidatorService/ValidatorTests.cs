using Features.Core.ValidatorService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using UnitTests.Helpers;
using Xunit;

namespace UnitTests.ValidatorService;

public class ValidatorTests
{
    private class SampleRequest
    {
        public string? Name { get; set; }
        public Guid? ObjectId { get; set; }
        public IEnumerable<string>? Tags { get; set; }
        public ICollection<string>? Categories { get; set; }
    }

    private static class ErrorCodes
    {
        public const string NameIsRequired = nameof(NameIsRequired);
        public const string ObjectIdIsRequired = nameof(ObjectIdIsRequired);
        public const string TagsIsRequired = nameof(TagsIsRequired);
        public const string CategoriesIsRequired = nameof(CategoriesIsRequired);
    }

    private class SampleRequestValidator : AbstractValidator<SampleRequest>
    {
        public override Task Validate(SampleRequest instance, CancellationToken cancellationToken)
        {
            if (IsEmpty(instance.Name)) AddError(ErrorCodes.NameIsRequired);
            if (IsEmpty(instance.ObjectId)) AddError(ErrorCodes.ObjectIdIsRequired);
            if (IsEmpty(instance.Tags)) AddError(ErrorCodes.TagsIsRequired);
            if (IsEmpty(instance.Categories)) AddError(ErrorCodes.CategoriesIsRequired);

            return Task.CompletedTask;
        }
    }

    [Fact]
    public async Task Validate_AddsErrors_WhenPropertiesInvalid()
    {
        var request = new SampleRequest
        {
            Name = string.Empty,
            ObjectId = Guid.Empty,
            Tags = new List<string>(),
            Categories = null
        };

        var validator = new SampleRequestValidator();
        await validator.Validate(request, CancellationToken.None);

        Assert.False(validator.IsValid);
        Assert.Equal(4, validator.Errors.Count);
        Assert.Contains(validator.Errors, e => e == ErrorCodes.NameIsRequired);
        Assert.Contains(validator.Errors, e => e == ErrorCodes.ObjectIdIsRequired);
        Assert.Contains(validator.Errors, e => e == ErrorCodes.TagsIsRequired);
        Assert.Contains(validator.Errors, e => e == ErrorCodes.CategoriesIsRequired);
    }

    [Fact]
    public async Task Validate_AddsErrors_WhenPropertiesValid()
    {
        var request = new SampleRequest
        {
            Name = RandomStringGenerator.Generate(),
            ObjectId = Guid.NewGuid(),
            Tags = new List<string>() { RandomStringGenerator.Generate() },
            Categories = [RandomStringGenerator.Generate()]
        };

        var validator = new SampleRequestValidator();
        await validator.Validate(request, CancellationToken.None);

        Assert.True(validator.IsValid);
        Assert.Empty(validator.Errors);
    }

    [Fact]
    public async Task IsEmpty_ReturnsTrue_ForNullString()
    {
        var request = new SampleRequest
        {
            Name = null
        };

        var validator = new SampleRequestValidator();
        await validator.Validate(request, CancellationToken.None);

        Assert.False(validator.IsValid);
    }

    [Fact]
    public async Task IsEmpty_ReturnsTrue_ForEmptyString()
    {
        var request = new SampleRequest
        {
            Name = string.Empty
        };

        var validator = new SampleRequestValidator();
        await validator.Validate(request, CancellationToken.None);

        Assert.False(validator.IsValid);
    }

    [Fact]
    public async Task IsEmpty_ReturnsTrue_ForNullGuid()
    {
        var request = new SampleRequest
        {
            ObjectId = null
        };

        var validator = new SampleRequestValidator();
        await validator.Validate(request, CancellationToken.None);

        Assert.False(validator.IsValid);
    }

    [Fact]
    public async Task IsEmpty_ReturnsTrue_ForEmptyGuid()
    {
        var request = new SampleRequest
        {
            ObjectId = Guid.Empty
        };

        var validator = new SampleRequestValidator();
        await validator.Validate(request, CancellationToken.None);

        Assert.False(validator.IsValid);
    }

    [Fact]
    public async Task IsEmpty_ReturnsTrue_ForNullCollection()
    {
        var request = new SampleRequest
        {
            Categories = null
        };

        var validator = new SampleRequestValidator();
        await validator.Validate(request, CancellationToken.None);

        Assert.False(validator.IsValid);
    }

    [Fact]
    public async Task IsEmpty_ReturnsTrue_ForEmptyCollection()
    {
        var request = new SampleRequest
        {
            Categories = new List<string>()
        };

        var validator = new SampleRequestValidator();
        await validator.Validate(request, CancellationToken.None);

        Assert.False(validator.IsValid);
    }

    [Fact]
    public async Task IsEmpty_ReturnsTrue_ForNullEnumerable()
    {
        var request = new SampleRequest
        {
            Tags = null
        };

        var validator = new SampleRequestValidator();
        await validator.Validate(request, CancellationToken.None);

        Assert.False(validator.IsValid);
    }

    [Fact]
    public async Task IsEmpty_ReturnsTrue_ForEmptyEnumerable()
    {
        var request = new SampleRequest
        {
            Tags = Enumerable.Empty<string>()
        };

        var validator = new SampleRequestValidator();
        await validator.Validate(request, CancellationToken.None);

        Assert.False(validator.IsValid);
    }
}
